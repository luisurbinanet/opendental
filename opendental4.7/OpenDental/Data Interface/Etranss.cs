using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class Etranss {

		///<summary>Gets data for the history grid in the SendClaims window.</summary>
		public static DataTable RefreshHistory(DateTime dateFrom,DateTime dateTo) {
			string command="Select CONCAT(CONCAT(patient.LName,', '),patient.FName) AS PatName,carrier.CarrierName,"
				+"clearinghouse.Description AS Clearinghouse,DateTimeTrans,etrans.OfficeSequenceNumber,"
				+"etrans.CarrierTransCounter,Etype,etrans.ClaimNum,etrans.EtransNum "
				+"FROM etrans "
				+"LEFT JOIN carrier ON etrans.CarrierNum=carrier.CarrierNum "
				+"LEFT JOIN patient ON patient.PatNum=etrans.PatNum "
				+"LEFT JOIN clearinghouse ON clearinghouse.ClearinghouseNum=etrans.ClearinghouseNum WHERE ";
				if(FormChooseDatabase.DBtype==DatabaseType.Oracle){
					command+="TO_";
				}
				command+="DATE(DateTimeTrans) >= "+POut.PDate(dateFrom)+" AND ";
				if(FormChooseDatabase.DBtype==DatabaseType.Oracle){
					command+="TO_";
				}
				command+="DATE(DateTimeTrans) <= "+POut.PDate(dateTo)+" "
					+"ORDER BY DateTimeTrans";
			DataTable table=General.GetTable(command);
			DataTable tHist=new DataTable("Table");
			tHist.Columns.Add("patName");
			tHist.Columns.Add("CarrierName");
			tHist.Columns.Add("Clearinghouse");
			tHist.Columns.Add("dateTimeTrans");
			tHist.Columns.Add("OfficeSequenceNumber");
			tHist.Columns.Add("CarrierTransCounter");
			tHist.Columns.Add("etype");
			tHist.Columns.Add("Etype");
			tHist.Columns.Add("ClaimNum");
			tHist.Columns.Add("EtransNum");
			DataRow row;
			string etype;
			for(int i=0;i<table.Rows.Count;i++) {
				row=tHist.NewRow();
				row["patName"]=table.Rows[i]["PatName"].ToString();
				row["CarrierName"]=table.Rows[i]["CarrierName"].ToString();
				row["Clearinghouse"]=table.Rows[i]["Clearinghouse"].ToString();
				row["dateTimeTrans"]=PIn.PDateT(table.Rows[i]["DateTimeTrans"].ToString()).ToShortDateString();
				row["OfficeSequenceNumber"]=table.Rows[i]["OfficeSequenceNumber"].ToString();
				row["CarrierTransCounter"]=table.Rows[i]["CarrierTransCounter"].ToString();
				row["Etype"]=table.Rows[i]["Etype"].ToString();
				etype=Lan.g("enumEtransType",((EtransType)PIn.PInt(table.Rows[i]["Etype"].ToString())).ToString());
				if(etype.EndsWith("_CA")){
					etype=etype.Substring(0,etype.Length-3);
				}
				row["etype"]=etype;
				row["ClaimNum"]=table.Rows[i]["ClaimNum"].ToString();
				row["EtransNum"]=table.Rows[i]["EtransNum"].ToString();
				tHist.Rows.Add(row);
			}
			return tHist;
		}

		///<summary></summary>
		public static Etrans GetEtrans(int etransNum){
			string command="SELECT * FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			DataTable table=General.GetTable(command);
			Etrans etrans=new Etrans();
			etrans.EtransNum           =PIn.PInt   (table.Rows[0][0].ToString());
			etrans.DateTimeTrans       =PIn.PDateT (table.Rows[0][1].ToString());
			etrans.ClearinghouseNum    =PIn.PInt   (table.Rows[0][2].ToString());
			etrans.Etype               =(EtransType)PIn.PInt(table.Rows[0][3].ToString());
			etrans.ClaimNum            =PIn.PInt   (table.Rows[0][4].ToString());
			etrans.OfficeSequenceNumber=PIn.PInt   (table.Rows[0][5].ToString());
			etrans.CarrierTransCounter =PIn.PInt   (table.Rows[0][6].ToString());
			etrans.CarrierTransCounter2=PIn.PInt   (table.Rows[0][7].ToString());
			etrans.CarrierNum          =PIn.PInt   (table.Rows[0][8].ToString());
			etrans.CarrierNum2         =PIn.PInt   (table.Rows[0][9].ToString());
			etrans.PatNum              =PIn.PInt   (table.Rows[0][10].ToString());
			etrans.MessageText         =PIn.PString(table.Rows[0][11].ToString());
			return etrans;
		}

		///<summary>Not for claim types, just other types, including Eligibility. This function gets run first.  Then, the messagetext is created and an attempt is made to send the message.  Finally, the messagetext is added to the etrans.  This is necessary because the transaction numbers must be incremented and assigned to each message before creating the message and attempting to send.  If it fails, we will need to roll back.  Provide EITHER a carrierNum OR a canadianNetworkNum.  Many transactions can be sent to a carrier or to a network.</summary>
		public static Etrans CreateCanadianOutput(int patNum, int carrierNum, int canadianNetworkNum, int clearinghouseNum, EtransType etype){
			//validation of carrier vs network
			if(etype==EtransType.Eligibility_CA){
				//only carrierNum is allowed (and required)
				if(carrierNum==0){
					throw new ApplicationException("Carrier not supplied for Etranss.CreateCanadianOutput.");
				}
				if(canadianNetworkNum!=0){
					throw new ApplicationException("NetworkNum not allowed for Etranss.CreateCanadianOutput.");
				}
			}
			Etrans etrans=new Etrans();
			//etrans.DateTimeTrans handled automatically
			etrans.ClearinghouseNum=clearinghouseNum;
			etrans.Etype=etype;
			etrans.ClaimNum=0;//no claim involved
			etrans.PatNum=patNum;
			//CanadianNetworkNum?
			etrans.CarrierNum=carrierNum;
			//InsPlanNum? (for eligibility)
			//Get next OfficeSequenceNumber-----------------------------------------------------------------------------------------
			etrans.OfficeSequenceNumber=0;
			string command="SELECT MAX(OfficeSequenceNumber) FROM etrans";
			DataTable table=General.GetTable(command);
			if(table.Rows.Count>0) {
				etrans.OfficeSequenceNumber=PIn.PInt(table.Rows[0][0].ToString());
				if(etrans.OfficeSequenceNumber==999999){//if the office has sent > 1 million messages, and has looped back around to 1.
					//get the date of the most recent max
					//This works, but it got even more complex for CarrierTransCounter, so we will just throw an exception for now.
					/*command="SELECT MAX(DateTimeTrans) FROM etrans WHERE OfficeSequenceNumber=999999";
					table=General.GetTable(command);
					DateTime maxDateT=PIn.PDateT(table.Rows[0][0].ToString());
					//then, just get the max that's newer than that.
					command="SELECT MAX(OfficeSequenceNumber) FROM etrans WHERE DateTimeTrans > '"+POut.PDateT(maxDateT)+"'";
					table=General.GetTable(command);
					if(table.Rows.Count>0) {
						etrans.OfficeSequenceNumber=PIn.PInt(table.Rows[0][0].ToString());
					}*/
					throw new ApplicationException("OfficeSequenceNumber has maxed out at 999999.  This program will need to be enhanced.");
				}
			}			
			etrans.OfficeSequenceNumber++;
			if(etype==EtransType.Eligibility_CA){
				//find the next CarrierTransCounter------------------------------------------------------------------------------------
				etrans.CarrierTransCounter=0;
				command="SELECT MAX(CarrierTransCounter) FROM etrans"
					+"WHERE CarrierNum="+POut.PInt(etrans.CarrierNum);
				table=General.GetTable(command);
				int tempcounter=0;
				if(table.Rows.Count>0) {
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter) {
					etrans.CarrierTransCounter=tempcounter;
				}
				command="SELECT MAX(CarrierTransCounter2) FROM etrans "
					+"WHERE CarrierNum2="+POut.PInt(etrans.CarrierNum);
				table=General.GetTable(command);
				if(table.Rows.Count>0) {
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter) {
					etrans.CarrierTransCounter=tempcounter;
				}
				if(etrans.CarrierTransCounter==99999){
					throw new ApplicationException("CarrierTransCounter has maxed out at 99999.  This program will need to be enhanced.");
					//maybe by adding a reset date to the preference table which will apply to all counters as a whole.
				}
				etrans.CarrierTransCounter++;
			}
			Insert(etrans);
			return etrans;
		}

		///<summary>Sets the status of the claim to sent.  Also makes an entry in etrans.  If this is canadian eclaims, then this function gets run first.  Then, the messagetext is created and an attempt is made to send the claim.  Finally, the messagetext and added to the etrans.  This is necessary because the transaction numbers must be incremented and assigned to each claim before creating the message and attempting to send.  If it fails, Canadians will need to delete the etrans entries (or we will need to roll back the changes).</summary>
		public static Etrans SetClaimSentOrPrinted(int claimNum, int patNum, int clearinghouseNum, EtransType etype) {
			string command= "UPDATE claim SET ClaimStatus = 'S' WHERE claimnum = "+POut.PInt(claimNum);
			General.NonQ(command);
			Etrans etrans=new Etrans();
			//etrans.DateTimeTrans handled automatically
			etrans.ClearinghouseNum=clearinghouseNum;
			etrans.Etype=etype;
			etrans.ClaimNum=claimNum;
			etrans.PatNum=patNum;
			//Get the primary and secondary carrierNums for this claim.
			command="SELECT carrier1.CarrierNum,carrier2.CarrierNum AS CarrierNum2 FROM claim "
				+"LEFT JOIN insplan insplan1 ON insplan1.PlanNum=claim.PlanNum "
				+"LEFT JOIN carrier carrier1 ON carrier1.CarrierNum=insplan1.CarrierNum "
				+"LEFT JOIN insplan insplan2 ON insplan2.PlanNum=claim.PlanNum2 "
				+"LEFT JOIN carrier carrier2 ON carrier2.CarrierNum=insplan2.CarrierNum "
				+"WHERE claim.ClaimNum="+POut.PInt(claimNum);
			DataTable table=General.GetTable(command);
			etrans.CarrierNum=PIn.PInt(table.Rows[0][0].ToString());
			etrans.CarrierNum2=PIn.PInt(table.Rows[0][1].ToString());//might be 0 if no secondary on this claim
			if(etype==EtransType.Claim_CA){
				etrans.OfficeSequenceNumber=0;
				//find the next officeSequenceNumber
				command="SELECT MAX(OfficeSequenceNumber) FROM etrans";
				table=General.GetTable(command);
				if(table.Rows.Count>0){
					etrans.OfficeSequenceNumber=PIn.PInt(table.Rows[0][0].ToString());
					if(etrans.OfficeSequenceNumber==999999) {//if the office has sent > 1 million messages, and has looped back around to 1.
						throw new ApplicationException
							("OfficeSequenceNumber has maxed out at 999999.  This program will need to be enhanced.");
					}
				}
				etrans.OfficeSequenceNumber++;
				//find the next CarrierTransCounter for the primary carrier
				etrans.CarrierTransCounter=0;
				command="SELECT MAX(CarrierTransCounter) FROM etrans "
					+"WHERE CarrierNum="+POut.PInt(etrans.CarrierNum);
				table=General.GetTable(command);
				int tempcounter=0;
				if(table.Rows.Count>0){
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter){
					etrans.CarrierTransCounter=tempcounter;
				}
				command="SELECT MAX(CarrierTransCounter2) FROM etrans "
					+"WHERE CarrierNum2="+POut.PInt(etrans.CarrierNum);
				table=General.GetTable(command);
				if(table.Rows.Count>0){
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter) {
					etrans.CarrierTransCounter=tempcounter;
				}
				if(etrans.CarrierTransCounter==99999) {
					throw new ApplicationException("CarrierTransCounter has maxed out at 99999.  This program will need to be enhanced.");
				}
				etrans.CarrierTransCounter++;
				if(etrans.CarrierNum2>0){//if there is secondary coverage on this claim
					etrans.CarrierTransCounter2=1;
					command="SELECT MAX(CarrierTransCounter) FROM etrans "
						+"WHERE CarrierNum="+POut.PInt(etrans.CarrierNum2);
					table=General.GetTable(command);
					if(table.Rows.Count>0){
						tempcounter=PIn.PInt(table.Rows[0][0].ToString());
					}
					if(tempcounter>etrans.CarrierTransCounter2) {
						etrans.CarrierTransCounter2=tempcounter;
					}
					command="SELECT MAX(CarrierTransCounter2) FROM etrans "
						+"WHERE CarrierNum2="+POut.PInt(etrans.CarrierNum2);
					table=General.GetTable(command);
					if(table.Rows.Count>0){
						tempcounter=PIn.PInt(table.Rows[0][0].ToString());
					}
					if(tempcounter>etrans.CarrierTransCounter2) {
						etrans.CarrierTransCounter2=tempcounter;
					}
					if(etrans.CarrierTransCounter2==99999) {
						throw new ApplicationException("CarrierTransCounter has maxed out at 99999.  This program will need to be enhanced.");
					}
					etrans.CarrierTransCounter2++;
				}
			}
			Insert(etrans);
			return etrans;
		}
		
		///<summary>DateTimeTrans is always handled automatically here.  No need to set it in advance.</summary>
		private static void Insert(Etrans etrans){
			if(PrefB.RandomKeys) {
				etrans.EtransNum=MiscData.GetKey("etrans","EtransNum");
			}
			string command="INSERT INTO etrans (";
			if(PrefB.RandomKeys) {
				command+="EtransNum,";
			}
			command+="DateTimeTrans,ClearinghouseNum,Etype,ClaimNum,OfficeSequenceNumber,CarrierTransCounter,"
				+"CarrierTransCounter2,CarrierNum,CarrierNum2,PatNum,MessageText) VALUES(";
			if(PrefB.RandomKeys) {
				command+="'"+POut.PInt(etrans.EtransNum)+"', ";
			}
			if(FormChooseDatabase.DBtype==DatabaseType.Oracle) {
				command+=POut.PDateT(MiscData.GetNowDateTime());
			}else{//Assume MySQL
				command+="NOW()";
			}
			command+=", " //"'"+POut.PDateT (etrans.DateTimeTrans)+"', "
				+"'"+POut.PInt   (etrans.ClearinghouseNum)+"', "
				+"'"+POut.PInt   ((int)etrans.Etype)+"', "
				+"'"+POut.PInt   (etrans.ClaimNum)+"', "
				+"'"+POut.PInt   (etrans.OfficeSequenceNumber)+"', "
				+"'"+POut.PInt   (etrans.CarrierTransCounter)+"', "
				+"'"+POut.PInt   (etrans.CarrierTransCounter2)+"', "
				+"'"+POut.PInt   (etrans.CarrierNum)+"', "
				+"'"+POut.PInt   (etrans.CarrierNum2)+"', "
				+"'"+POut.PInt   (etrans.PatNum)+"', "
				+"'"+POut.PString(etrans.MessageText)+"')";
			if(PrefB.RandomKeys) {
				General.NonQ(command);
			}
			else {
				etrans.EtransNum=General.NonQ(command,true);
			}
		}

		///<summary></summary>
		public static void SetMessage(int etransNum, string msg) {
			string command= "UPDATE etrans SET MessageText='"+POut.PString(msg)+"' "
				+"WHERE EtransNum = '"+POut.PInt(etransNum)+"'";
			General.NonQ(command);
		}

		///<summary>Deletes the etrans entry and changes the status of the claim back to W.  If it encounters an entry that's not a claim, it skips it for now.  Later, it will handle all types of undo.  It will also check Canadian claims to prevent alteration if an ack or EOB has been received.</summary>
		public static void Undo(int etransNum){
			//see if it's a claim.
			string command="SELECT ClaimNum FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			DataTable table=General.GetTable(command);
			int claimNum=PIn.PInt(table.Rows[0][0].ToString());
			if(claimNum==0){//if no claim
				return;//for now
			}
			//future Canadian check will go here

			//Change the claim back to W.
			command="UPDATE claim SET ClaimStatus='W' WHERE ClaimNum="+POut.PInt(claimNum);
			General.NonQ(command);
			//Delete this etrans
			command="DELETE FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			General.NonQ(command);
		}

		///<summary>Deletes the etrans entry.  Only used when the etrans entry was created, but then the communication with the clearinghouse failed.  So this is just a rollback function.  Will throw exception if there's a message attached to the etrans or if the etrans does not exist.</summary>
		public static void Delete(int etransNum) {
			//see if there's a message
			string command="SELECT MessageText FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			DataTable table=General.GetTable(command);
			if(table.Rows[0][0].ToString()!=""){//this throws exception if 0 rows.
				throw new ApplicationException("Error. Etrans must not have messagetext attached yet.");
			}
			command="DELETE FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			General.NonQ(command);
		}
		

	}

	




}

















