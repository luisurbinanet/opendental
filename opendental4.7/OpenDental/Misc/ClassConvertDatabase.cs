using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Resources;
using System.Text; 
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{

	///<summary></summary>
	public class ClassConvertDatabase{
		private System.Version FromVersion;
		private System.Version ToVersion;

		///<summary>Return false to indicate exit app.  Only called when program first starts up at the beginning of FormOpenDental.RefreshLocalData.</summary>
		public bool Convert(string fromVersion){
			FromVersion=new Version(fromVersion);
			ToVersion=new Version(Application.ProductVersion);
			if(FromVersion>=new Version("3.4.0") && PrefB.GetBool("CorruptedDatabase")){
				MsgBox.Show(this,"Your database is corrupted because a conversion failed.  Please contact us.  This database is unusable and you will need to restore from a backup.");
				return false;//shuts program down.
			}
			if(FromVersion.CompareTo(ToVersion)>0){//"Cannot convert database to an older version."
				//no longer necessary to catch it here.  It will be handled soon enough in CheckProgramVersion
				return true;
			}
			if(FromVersion < new Version("2.8.0")){
				MsgBox.Show(this,"This database is too old to easily convert in one step. Please upgrade to 2.1 if necessary, then to 2.8.  Then you will be able to upgrade to this version. We apologize for the inconvenience.");
				return false;
			}
			if(FromVersion.ToString()=="2.9.0.0"
				|| FromVersion.ToString()=="3.0.0.0")
			{
				MsgBox.Show(this,"Cannot convert this database version which was only for development purposes.");
				return false;
			}
			if(FromVersion < new Version("4.7.0.0")){
				if(MessageBox.Show(Lan.g(this,"Your database will now be converted")+"\r"
					+Lan.g(this,"from version")+" "+FromVersion.ToString()+"\r"
					+Lan.g(this,"to version")+" "+ToVersion.ToString()+"\r"
					+Lan.g(this,"The conversion works best if you are on the server.  Depending on the speed of your computer, it can be as fast as a few seconds, or it can take as long as 10 minutes.")
					,"",MessageBoxButtons.OKCancel)
					!=DialogResult.OK)
				{
					return false;//close the program
				}
			}
			else{
				return true;//no conversion necessary
			}
#if !DEBUG
			if(FormChooseDatabase.DBtype!=DatabaseType.MySql
				&& !MsgBox.Show(this,true,"If you have not made a backup, please Cancel and backup before continuing.  Continue?"))
			{
				return false;
			}
			try{
				MiscData.MakeABackup();
			}
			catch(Exception e){
				if(e.Message!=""){
					MessageBox.Show(e.Message);
				}
				MsgBox.Show(this,"Backup failed. Your database has not been altered.");
				return false;
			}
			try{
#endif
			if(FromVersion>=new Version("3.4.0")){
					Prefs.UpdateBool("CorruptedDatabase",true);
				}
				To2_8_2();//begins going through the chain of conversion steps
				MsgBox.Show(this,"Conversion successful");
				if(FromVersion>=new Version("3.4.0")){
					Prefs.Refresh();//or it won't know it has to update in the next line.
					Prefs.UpdateBool("CorruptedDatabase",false);
				}
				Prefs.Refresh();
				return true;
#if !DEBUG
			}
			catch(System.IO.FileNotFoundException e){
				MessageBox.Show(e.FileName+" "+Lan.g(this,"could not be found. Your database has not been altered and is still usable if you uninstall this version, then reinstall the previous version."));
				if(FromVersion>=new Version("3.4.0")){
					Prefs.UpdateBool("CorruptedDatabase",false);
				}
				//Prefs.Refresh();
				return false;
			}
			catch(System.IO.DirectoryNotFoundException){
				MessageBox.Show(Lan.g(this,"ConversionFiles folder could not be found. Your database has not been altered and is still usable if you uninstall this version, then reinstall the previous version."));
				if(FromVersion>=new Version("3.4.0")){
					Prefs.UpdateBool("CorruptedDatabase",false);
				}
				//Prefs.Refresh();
				return false;
			}
			catch(Exception ex){
			//	MessageBox.Show();
				MessageBox.Show(ex.Message+"\r\n\r\n"
					+Lan.g(this,"Conversion unsuccessful. Your database is now corrupted and you cannot use it.  Please contact us."));
				//Then, application will exit, and database will remain tagged as corrupted.
				return false;
			}
#endif
		}

		/// <summary>Takes a text file with a series of SQL commands, and sends them as queries to the database.  Used in version upgrades and in the function that downloads and installs the latest translations.  The filename is always relative to the application directory.  Throws an exception if it fails.  Due to spotty support for batch commands, this function is no longer used.</summary>
		public void ExecuteFile(string fileName) {
			//also used in the function that downloads and installs the latests translations.
			//if(!File.Exists(fileName)){
			//	throw new Exception(fileName+" could not be found.");
			//}
			StreamReader sr;
			string line="";
			//string cmd="";
			string command="";
			ArrayList AL=new ArrayList();
			sr=new StreamReader(fileName);//throws a FileNotFoundException if not found. We handle that.
			//sr.ReadToEnd();I would do this, except I'm worried about introducing a bug.
			while(true) {
				line=sr.ReadLine();
				if(line==null) {
					break;
				}
				command+=" "+line;
				//if(line.Length==0 || line.Substring(0,1)=="#") {//could improve white space handling
					//continue;
				//}
				//else {
				//	cmd+=" "+line;
				//	if(cmd.Substring(cmd.Length-1)==";") {//again, need to handle white space better
						//AL.Add(cmd);
						//commands+=
					//	cmd="";
				//	}
				//}
			}
			//string[] nonQArray=new string[AL.Count];
			//commands
			//AL.CopyTo(nonQArray);
			General.NonQEx(command);
			//NonQ(commands);
			sr.Close();
		}

		/*
		private int NonQ(string command){
			return NonQ(command,false);
		}

		///<summary>Designed not to catch exceptions</summary>
		private int NonQ(string[] commands){
			string command="";
			for(int i=0;i<commands.Length;i++) {
				if(i>0) {
					command+=" ";
				}
				command+=commands[i]+";";
			}
			if(RemotingClient.OpenDentBusinessIsLocal) {
				return GeneralB.NonQ(command,false);
			}
			else {
				DtoGeneralNonQ dto=new DtoGeneralNonQ();
				dto.Command=command;
				dto.GetInsertID=false;
				return RemotingClient.ProcessCommand(dto);
			}
		}

		///<summary>Designed not to catch exceptions</summary>
		private int NonQ(string command, bool getInsertID){
			if(RemotingClient.OpenDentBusinessIsLocal) {
				return GeneralB.NonQ(command,getInsertID);
			}
			else {
				DtoGeneralNonQ dto=new DtoGeneralNonQ();
				dto.Command=command;
				dto.GetInsertID=getInsertID;
				return RemotingClient.ProcessCommand(dto);
			}
		}

		///<summary>Designed not to catch exceptions</summary>
		private DataTable GetTable(string command) {
			if(RemotingClient.OpenDentBusinessIsLocal) {
				return GeneralB.GetTable(command).Tables[0];
			}
			else {
				DtoGeneralGetTable dto=new DtoGeneralGetTable();
				dto.Command=command;
				return RemotingClient.ProcessQuery(dto).Tables[0];
			}
		}*/

		private void To2_8_2() {
			if(FromVersion < new Version("2.8.2.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE insplan DROP TemplateNum"
					,"DROP TABLE instemplate"
					,"UPDATE preference SET ValueString = '2.8.2.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_8_3();
		}

		private void To2_8_3() {
			if(FromVersion < new Version("2.8.3.0")) {
				string[] commands=new string[]
				{
					"INSERT INTO preference VALUES ('RenaissanceLastBatchNumber','0')"
					,"INSERT INTO preference VALUES ('PatientSelectUsesSearchButton','0')"
					,"UPDATE preference SET ValueString = '2.8.3.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_8_6();
		}

		private void To2_8_6() {
			if(FromVersion < new Version("2.8.6.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE patient CHANGE City City VARCHAR(100) NOT NULL"
					,"ALTER TABLE patient CHANGE State State VARCHAR(100) NOT NULL"
					,"ALTER TABLE patient CHANGE Zip Zip VARCHAR(100) NOT NULL"
					,"ALTER TABLE patient CHANGE SSN SSN VARCHAR(100) NOT NULL"
					,"UPDATE preference SET ValueString = '2.8.6.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_8_10();
		}

		private void To2_8_10() {
			if(FromVersion < new Version("2.8.10.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE employer ADD Address varchar(255) NOT NULL"
					,"ALTER TABLE employer ADD Address2 varchar(255) NOT NULL"
					,"ALTER TABLE employer ADD City varchar(255) NOT NULL"
					,"ALTER TABLE employer ADD State varchar(255) NOT NULL"
					,"ALTER TABLE employer ADD Zip varchar(255) NOT NULL"
					,"ALTER TABLE employer ADD Phone varchar(255) NOT NULL"
					,"INSERT INTO preference VALUES ('CustomizedForPracticeWeb','0')"
					,"UPDATE preference SET ValueString = '2.8.10.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_8_14();
		}

		private void To2_8_14() {
			if(FromVersion < new Version("2.8.14.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE adjustment CHANGE AdjType AdjType smallint unsigned NOT NULL"
					,"ALTER TABLE appointment CHANGE Confirmed Confirmed smallint unsigned NOT NULL"
					,"ALTER TABLE payment CHANGE PayType PayType smallint unsigned NOT NULL"
					,"ALTER TABLE procedurecode CHANGE ProcCat ProcCat smallint unsigned NOT NULL"
					,"ALTER TABLE procedurelog CHANGE Priority Priority smallint unsigned NOT NULL"
					,"ALTER TABLE procedurelog CHANGE Dx Dx smallint unsigned NOT NULL"
					,"UPDATE preference SET ValueString = '2.8.14.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_9_1();
		}

		private void To2_9_1() {
			if(FromVersion < new Version("2.9.1.0")) {
				//check to see if the conversion file is available
				//if(!File.Exists(@"ConversionFiles\convert_2_9_1.txt")){
				//	throw new Exception(@"ConversionFiles\convert_2_9_1.txt"+" could not be found.");
				//}
				ExecuteFile(@"ConversionFiles\convert_2_9_1.txt");//might throw an exception which we handle.
				string[] commands=new string[]
				{
					"UPDATE preference SET ValueString = '2.9.1.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_9_2();
		}

		private void To2_9_2() {
			if(FromVersion < new Version("2.9.2.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE patient ADD PriPending tinyint(1) unsigned NOT NULL"
					,"ALTER TABLE patient ADD SecPending tinyint(1) unsigned NOT NULL"
					,"ALTER TABLE appointment ADD Assistant smallint unsigned NOT NULL"
					,"UPDATE preference SET ValueString = '2.9.2.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_9_5();
		}

		private void To2_9_5() {
			if(FromVersion < new Version("2.9.5.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE autocode ADD LessIntrusive tinyint(1) unsigned NOT NULL"
					,"UPDATE preference SET ValueString = '2.9.5.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To2_9_8();
		}

		private void To2_9_8() {
			if(FromVersion < new Version("2.9.8.0")) {
				string claimFormNum;
				//Change the PlaceNumericCode field for both HCFA forms
				string command="SELECT ClaimFormNum FROM claimform WHERE UniqueID = '4'";
				DataTable table=General.GetTableEx(command);
				string[] commands;
				if(table.Rows.Count>0) {
					claimFormNum=table.Rows[0][0].ToString();
					commands=new string[]
					{
						"UPDATE claimformitem SET FieldName='P1PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='751'"
						,"UPDATE claimformitem SET FieldName='P2PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='784'"
						,"UPDATE claimformitem SET FieldName='P3PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='817'"
						,"UPDATE claimformitem SET FieldName='P4PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='850'"
						,"UPDATE claimformitem SET FieldName='P5PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='884'"
						,"UPDATE claimformitem SET FieldName='P6PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='917'"
					};
					General.NonQEx(commands);
				}
				command="SELECT ClaimFormNum FROM claimform WHERE UniqueID = '5'";
				table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					claimFormNum=table.Rows[0][0].ToString();
					commands=new string[]
					{
						"UPDATE claimformitem SET FieldName='P1PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='751'"
						,"UPDATE claimformitem SET FieldName='P2PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='784'"
						,"UPDATE claimformitem SET FieldName='P3PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='817'"
						,"UPDATE claimformitem SET FieldName='P4PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='850'"
						,"UPDATE claimformitem SET FieldName='P5PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='884'"
						,"UPDATE claimformitem SET FieldName='P6PlaceNumericCode' "
						+"WHERE FieldName='PlaceNumericCode' && ClaimFormNum='"+claimFormNum+"' "
						+"&& YPos='917'"
					};
					General.NonQEx(commands);
				}
				//ADA2002 medicaid id's
				command="SELECT ClaimFormNum FROM claimform WHERE UniqueID = '1'";
				table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					claimFormNum=table.Rows[0][0].ToString();
					commands=new string[]
					{
						"INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+"'"+claimFormNum+"','TreatingDentistMedicaidID','492','946','117','14')"
						,"INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+"'"+claimFormNum+"','BillingDentistMedicaidID','39','990','120','14')"
					};
					General.NonQEx(commands);
				}
				//ADA2000 employer and 3 radiograph fields.
				command="SELECT ClaimFormNum FROM claimform WHERE UniqueID = '3'";
				table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					claimFormNum=table.Rows[0][0].ToString();
					commands=new string[]
					{
						"INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+"'"+claimFormNum+"','EmployerName','482','391','140','14')"
						,"INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+"'"+claimFormNum+"','IsRadiographsAttached','388','548','0','0')"
						,"INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+"'"+claimFormNum+"','RadiographsNotAttached','495','547','0','0')"
						,"INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+"'"+claimFormNum+"','RadiographsNumAttached','460','545','35','14')"
					};
					General.NonQEx(commands);
				}
				commands=new string[]
				{
					"UPDATE preference SET ValueString = '2.9.8.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_0_1();
		}

		///<summary>Used by To3_0_1. IMPORTANT: remember that this method alters TableQ.</summary>
		private int GetPercent(int patNum,int priPlanNum,int secPlanNum,string adaCode,string priORsec) {
			//command="SELECT 
			//get the covCatNum for this adaCode
			string command="SELECT CovCatNum FROM covspan "
				+"WHERE '"+POut.PString(adaCode)+"' > FromCode "
				+"AND '"+POut.PString(adaCode)+"' < ToCode";
			DataTable table=General.GetTableEx(command);
			if(table.Rows.Count==0) {
				return 0;//this code is not in any category, so coverage=0
			}
			int covCatNum=PIn.PInt(table.Rows[0][0].ToString());
			command="SELECT PlanNum,PriPatNum,SecPatNum,Percent FROM covpat WHERE "
				+"CovCatNum = '"+covCatNum.ToString()+"' "
				+"AND (PlanNum = '"+priPlanNum.ToString()+"' "
				+"OR PlanNum = '"+secPlanNum.ToString()+"' "
				+"OR PriPatNum = '"+patNum.ToString()+"' "
				+"OR SecPatNum = '"+patNum.ToString()+"')";
			table=General.GetTableEx(command);
			if(table.Rows.Count==0) {
				return 0;//no percentages have been entered for this patient or plan
			}
			for(int i=0;i<table.Rows.Count;i++) {
				//first handle the patient overrides
				if(priORsec=="pri" && PIn.PInt(table.Rows[i][1].ToString())==patNum) {
					return PIn.PInt(table.Rows[i][3].ToString());
				}
				if(priORsec=="sec" && PIn.PInt(table.Rows[i][2].ToString())==patNum) {
					return PIn.PInt(table.Rows[i][3].ToString());
				}
				//then handle the percentages attached to plans(much more common)
				if(priORsec=="pri" && PIn.PInt(table.Rows[i][0].ToString())==priPlanNum) {
					return PIn.PInt(table.Rows[i][3].ToString());
				}
				if(priORsec=="sec" && PIn.PInt(table.Rows[i][0].ToString())==secPlanNum) {
					return PIn.PInt(table.Rows[i][3].ToString());
				}
			}
			return 0;
		}

		private void To3_0_1() {
			if(FromVersion < new Version("3.0.1.0")) {
				//if(!File.Exists(@"ConversionFiles\convert_3_0_1.txt")){
				//	throw new Exception(@"ConversionFiles\convert_3_0_1.txt"+" could not be found.");
				//}
				ExecuteFile(@"ConversionFiles\convert_3_0_1.txt");//might throw an exception which we handle.
				//convert appointment patterns from ten minute to five minute intervals---------------------
				string command="SELECT AptNum,Pattern FROM appointment";
				DataTable table=General.GetTableEx(command);
				StringBuilder sb;
				string pattern;
				for(int i=0;i<table.Rows.Count;i++) {
					pattern=PIn.PString(table.Rows[i][1].ToString());
					sb=new StringBuilder();
					for(int j=0;j<pattern.Length;j++) {
						sb.Append(pattern.Substring(j,1));
						sb.Append(pattern.Substring(j,1));
					}
					command="UPDATE appointment SET "
						+"Pattern='"+POut.PString(sb.ToString())+"' "
						+"WHERE AptNum='"+table.Rows[i][0].ToString()+"'";
					General.NonQEx(command);
				}
				//add the default 5 Elements to each ApptView-----------------------------------------------
				command="SELECT ApptViewNum FROM apptview";
				table=General.GetTableEx(command);
				string[] commands;
				for(int i=0;i<table.Rows.Count;i++) {
					commands=new string[]
					{
						"INSERT INTO apptviewitem(ApptViewNum,ElementDesc,ElementOrder,ElementColor) "
							+"VALUES('"+table.Rows[i][0].ToString()+"','PatientName','0','-16777216')"
						,"INSERT INTO apptviewitem(ApptViewNum,ElementDesc,ElementOrder,ElementColor) "
							+"VALUES('"+table.Rows[i][0].ToString()+"','Lab','1','-65536')"
						,"INSERT INTO apptviewitem(ApptViewNum,ElementDesc,ElementOrder,ElementColor) "
							+"VALUES('"+table.Rows[i][0].ToString()+"','Procs','2','-16777216')"
						,"INSERT INTO apptviewitem(ApptViewNum,ElementDesc,ElementOrder,ElementColor) "
							+"VALUES('"+table.Rows[i][0].ToString()+"','Note','3','-16777216')"
						,"INSERT INTO apptviewitem(ApptViewNum,ElementDesc,ElementOrder,ElementColor) "
							+"VALUES('"+table.Rows[i][0].ToString()+"','Production','4','-16777216')"
					};
					General.NonQEx(commands);
				}
				//MessageBox.Show("Appointments converted.");
				//Any claimprocs attached to claims with ins being Cap, should be CapClaim, even if paid
				command="SELECT claimproc.ClaimProcNum FROM claimproc,insplan "
					+"WHERE claimproc.PlanNum=insplan.PlanNum "
					+"AND claimproc.ClaimNum != '0' "
					+"AND insplan.PlanType='c'";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE claimproc SET Status='5' "//CapClaim
						+"WHERE ClaimProcNum='"+table.Rows[i][0].ToString()+"'";
					General.NonQEx(command);
				}
				//edit any existing claimprocs-------------------------------------------------------------
				//These are all associated with claims, but we are not changing the claim values,
				//just some of the estimates.  None of these changes affect any claim or balance.
				//Ignore any status=CapClaim since these are all duplicates just for sending the claim
				//Add percentages etc from procedure
				int percentage=0;
				int planNum=0;
				double baseEst=0;
				double overrideAmt=0;
				DataTable procTable;
				command="SELECT claimproc.ClaimProcNum,patient.PriPlanNum,"//0,1
					+"patient.SecPlanNum,patient.PatNum,claimproc.PlanNum,procedurelog.ADACode,"//2,3,4,5
					+"procedurelog.OverridePri,procedurelog.OverrideSec,procedurelog.ProcFee "//6,7,8
					+"FROM claimproc,procedurelog,patient "
					//this next line ignores any claimprocs not attached to a proc. so skips adjustments.
					+"WHERE claimproc.ProcNum=procedurelog.ProcNum "
					+"AND patient.PatNum=procedurelog.PatNum "
					+"AND claimproc.Status != 2 "//skips preauths
					+"AND claimproc.Status != 4 "//skips supplemental
					+"AND claimproc.Status != 5 ";//skips capClaim
				table=General.GetTableEx(command);
				procTable=table.Copy();//so that we can perform other queries
				for(int i=0;i<procTable.Rows.Count;i++) {
					planNum=PIn.PInt(procTable.Rows[i][4].ToString());//claimproc.PlanNum
					//if primary
					if(planNum==PIn.PInt(procTable.Rows[i][1].ToString())) {//priPlanNum
						percentage=GetPercent(PIn.PInt(procTable.Rows[i][3].ToString()),//patNum
							PIn.PInt(procTable.Rows[i][1].ToString()),//priPlanNum
							PIn.PInt(procTable.Rows[i][2].ToString()),//secPlanNum
							PIn.PString(procTable.Rows[i][5].ToString()),//ADACode
							"pri");
						overrideAmt=PIn.PDouble(procTable.Rows[i][6].ToString());
					}
					//else if secondary
					else if(planNum==PIn.PInt(procTable.Rows[i][2].ToString())) {//priPlanNum
						percentage=GetPercent(PIn.PInt(procTable.Rows[i][3].ToString()),//patNum
							PIn.PInt(procTable.Rows[i][1].ToString()),//priPlanNum
							PIn.PInt(procTable.Rows[i][2].ToString()),//secPlanNum
							PIn.PString(procTable.Rows[i][5].ToString()),//ADACode
							"sec");
						overrideAmt=PIn.PDouble(procTable.Rows[i][7].ToString());
					}
					else {
						//plan is neither pri or sec, so disregard
						continue;
					}
					//fee x percentage:
					baseEst=PIn.PDouble(procTable.Rows[i][8].ToString())*(double)percentage/100;
					command="UPDATE claimproc SET "
						//+"AllowedAmt='-1',"
						+"Percentage='"+percentage.ToString()+"',"
						//+"PercentOverride='-1',"
						//+"CopayAmt='-1',"
						+"OverrideInsEst='"+overrideAmt.ToString()+"',"
						//+"OverAnnualMax='-1',"
						//+"PaidOtherIns='-1',"
						+"BaseEst='"+baseEst.ToString()+"'"
						//+"CopayOverride='-1'"
						+" WHERE ClaimProcNum='"+procTable.Rows[i][0].ToString()+"'";
					//MessageBox.Show(command);
					General.NonQEx(command);
				}
				//convert all estimates into claimprocs-------------------------------------------------
				command="SELECT procedurelog.ProcNum,procedurelog.PatNum,"//0,1
					+"procedurelog.ProvNum,patient.PriPlanNum,patient.SecPlanNum,"//2,3,4
					+"claimproc.ClaimProcNum,procedurelog.ADACode,procedurelog.ProcDate,"//5,6,7
					+"procedurelog.OverridePri,procedurelog.OverrideSec,procedurelog.NoBillIns,"//8,9,10
					+"procedurelog.CapCoPay,procedurelog.ProcStatus,procedurelog.ProcFee,"//11,12,13
					+"insplan.PlanType "//14
					+"FROM procedurelog,patient,insplan "
					+"LEFT JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum "//only interested in NULL
					+"WHERE procedurelog.PatNum=patient.PatNum "
					//this is to test for capitation. It also limits results to patients with insurance.
					+"AND patient.PriPlanNum=insplan.PlanNum "
					//+"AND patient.PriPlanNum > 0 "//only patients with insurance
					+"AND (procedurelog.ProcStatus=1 "//status TP
					+"OR procedurelog.ProcStatus=2) "//status C
					+"AND (claimproc.ClaimProcNum IS NULL "//only if not already attached to a claim
					+"OR claimproc.Status='5')";//or CapClaim
				table=General.GetTableEx(command);
				procTable=table.Copy();//so that we can perform other queries
				int status=0;
				double copay=0;
				double writeoff=0;
				for(int i=0;i<procTable.Rows.Count;i++) {//loop procedures
					//1. noBillIns
					if(PIn.PBool(procTable.Rows[i][10].ToString())//if noBillIns
						&& PIn.PDouble(procTable.Rows[i][11].ToString()) ==-1) {//and not a cap procedure
						//primary
						if(PIn.PInt(procTable.Rows[i][3].ToString())!=0) {//if has pri ins
							command="INSERT INTO claimproc(ProcNum,PatNum,ProvNum,Status,PlanNum,"
								+"DateCP,AllowedAmt,Percentage,PercentOverride,CopayAmt,OverrideInsEst,"
								+"NoBillIns,OverAnnualMax,PaidOtherIns) "
								+"VALUES ("
								+"'"+procTable.Rows[i][0].ToString()+"',"//procnum
								+"'"+procTable.Rows[i][1].ToString()+"',"//patnum
								+"'"+procTable.Rows[i][2].ToString()+"',"//provnum
								+"'6',"//status:Estimate
								+"'"+procTable.Rows[i][3].ToString()+"',"//priPlanNum
								+POut.PDate(PIn.PDate(procTable.Rows[i][7].ToString()))+","//dateCP
								//these -1's are unnecessary, but I already added them, so they are here.
								+"'-1',"//allowedamt
								+"'-1',"//percentage
								+"'-1',"//percentoverride
								+"'-1',"//copayamt
								+"'-1',"//overrideInsEst
								+"'1',"//NoBillIns,
								+"'-1',"//OverAnnualMax
								+"'-1'"//PaidOtherIns
								+")";
							General.NonQEx(command);
						}
						//secondary
						if(PIn.PInt(procTable.Rows[i][4].ToString())!=0) {//if has sec ins
							command="INSERT INTO claimproc(ProcNum,PatNum,ProvNum,Status,PlanNum,"
								+"DateCP,AllowedAmt,Percentage,PercentOverride,CopayAmt,OverrideInsEst,"
								+"NoBillIns,OverAnnualMax,PaidOtherIns) "
								+"VALUES ("
								+"'"+procTable.Rows[i][0].ToString()+"',"//procnum
								+"'"+procTable.Rows[i][1].ToString()+"',"//patnum
								+"'"+procTable.Rows[i][2].ToString()+"',"//provnum
								+"'6',"//status:Estimate
								+"'"+procTable.Rows[i][4].ToString()+"',"//secPlanNum
								+POut.PDate(PIn.PDate(procTable.Rows[i][7].ToString()))+","//dateCP
								+"'-1',"//allowedamt
								+"'-1',"//percentage
								+"'-1',"//percentoverride
								+"'-1',"//copayamt
								+"'-1',"//overrideInsEst
								+"'1',"//NoBillIns,
								+"'-1',"//OverAnnualMax
								+"'-1'"//PaidOtherIns
								+")";
							General.NonQEx(command);
						}
						continue;
					}//1. noBillIns
					//2. capitation. Always primary. If C, then affects aging via CapComplete.
					//Never attached to claim.
					copay=PIn.PDouble(procTable.Rows[i][11].ToString());
					//if CapCoPay not -1, and priIns is cap, then this is a cap proc
					if(copay!=-1 && PIn.PString(procTable.Rows[i][14].ToString())=="c") {
						if(PIn.PInt(procTable.Rows[i][12].ToString())==1) {//proc status =tp
							status=8;//claimProc status=CapEstimate
						}
						if(PIn.PInt(procTable.Rows[i][12].ToString())==2) {//proc status =c
							status=7;//claimProc status=CapComplete
						}
						writeoff=PIn.PDouble(procTable.Rows[i][13].ToString())//procFee
							-copay;
						command="INSERT INTO claimproc(ProcNum,PatNum,ProvNum,"
							+"Status,PlanNum,DateCP,WriteOff,AllowedAmt,Percentage,PercentOverride,"
							+"CopayAmt,OverrideInsEst,OverAnnualMax,PaidOtherIns,NoBillIns) "
							+"VALUES ("
							+"'"+procTable.Rows[i][0].ToString()+"',"//procnum
							+"'"+procTable.Rows[i][1].ToString()+"',"//patnum
							+"'"+procTable.Rows[i][2].ToString()+"',"//provnum
							+"'"+status.ToString()+"',"//status
							+"'"+procTable.Rows[i][3].ToString()+"',"//priPlanNum
							+POut.PDate(PIn.PDate(procTable.Rows[i][7].ToString()))+","//dateCP
							+"'"+writeoff.ToString()+"',"//writeoff
							+"'-1',"//allowedamt
							+"'-1',"//percentage
							+"'-1',"//percentoverride
							+"'"+copay.ToString()+"',"//copayamt
							+"'-1',"//overrideInsEst
							+"'-1',"//OverAnnualMax
							+"'-1',"//PaidOtherIns
							+"'"+procTable.Rows[i][10].ToString()+"'"//noBillIns is allowed for cap
							+")";
						General.NonQEx(command);
						continue;
					}
					//3. standard primary estimate:
					//always a primary estimate because original query excluded patients with no ins.
					planNum=PIn.PInt(procTable.Rows[i][3].ToString());//priPlanNum
					percentage=GetPercent(PIn.PInt(procTable.Rows[i][1].ToString()),//patNum
						PIn.PInt(procTable.Rows[i][3].ToString()),//priPlanNum
						PIn.PInt(procTable.Rows[i][4].ToString()),//secPlanNum
						PIn.PString(procTable.Rows[i][6].ToString()),//ADACode
						"pri");
					baseEst=PIn.PDouble(procTable.Rows[i][13].ToString())*(double)percentage/100;
					command="INSERT INTO claimproc(ProcNum,PatNum,ProvNum,"
						+"Status,PlanNum,DateCP,WriteOff,AllowedAmt,Percentage,PercentOverride,"
						+"CopayAmt,OverrideInsEst,NoBillIns,OverAnnualMax,PaidOtherIns,BaseEst) "
						+"VALUES ("
						+"'"+procTable.Rows[i][0].ToString()+"',"//procnum
						+"'"+procTable.Rows[i][1].ToString()+"',"//patnum
						+"'"+procTable.Rows[i][2].ToString()+"',"//provnum
						+"'6',"//status:Estimate
						+"'"+planNum.ToString()+"',"//plannum
						+POut.PDate(PIn.PDate(procTable.Rows[i][7].ToString()))+","//dateCP
						+"'0',"//writeoff
						+"'-1',"//allowedamt
						+"'"+percentage.ToString()+"',"//percentage
						+"'-1',"//percentoverride
						+"'-1',"//copayamt
						+"'"+procTable.Rows[i][8].ToString()+"',"//overrideInsEst-pri
						+"'0',"//NoBillIns,
						+"'-1',"//OverAnnualMax
						+"'-1',"//PaidOtherIns
						+"'"+baseEst.ToString()+"'"//BaseEst
						+")";
					General.NonQEx(command);
					//4. standard secondary estimate
					//secondary can be in addition to primary, or not at all
					planNum=PIn.PInt(procTable.Rows[i][4].ToString());//secPlanNum
					if(planNum==0) {
						continue;
					}
					percentage=GetPercent(PIn.PInt(procTable.Rows[i][1].ToString()),//patNum
						PIn.PInt(procTable.Rows[i][3].ToString()),//priPlanNum
						PIn.PInt(procTable.Rows[i][4].ToString()),//secPlanNum
						PIn.PString(procTable.Rows[i][6].ToString()),//ADACode
						"sec");
					baseEst=PIn.PDouble(procTable.Rows[i][13].ToString())*(double)percentage/100;
					command="INSERT INTO claimproc(ProcNum,PatNum,ProvNum,"
						+"Status,PlanNum,DateCP,WriteOff,AllowedAmt,Percentage,PercentOverride,"
						+"CopayAmt,OverrideInsEst,NoBillIns,OverAnnualMax,PaidOtherIns,BaseEst) "
						+"VALUES ("
						+"'"+procTable.Rows[i][0].ToString()+"',"//procnum
						+"'"+procTable.Rows[i][1].ToString()+"',"//patnum
						+"'"+procTable.Rows[i][2].ToString()+"',"//provnum
						+"'6',"//status:Estimate
						+"'"+planNum.ToString()+"',"//plannum
						+POut.PDate(PIn.PDate(procTable.Rows[i][7].ToString()))+","//dateCP
						+"'0',"//writeoff
						+"'-1',"//allowedamt
						+"'"+percentage.ToString()+"',"//percentage
						+"'-1',"//percentoverride
						+"'-1',"//copayamt
						+"'"+procTable.Rows[i][9].ToString()+"',"//overrideInsEst-pri
						+"'0',"//NoBillIns,
						+"'-1',"//OverAnnualMax
						+"'-1',"//PaidOtherIns
						+"'"+baseEst.ToString()+"'"//BaseEst
						+")";
					General.NonQEx(command);
				}//loop procedures
				command="UPDATE claimproc SET ProcDate=DateCP";//affects ALL patients
				General.NonQEx(command);
				//MessageBox.Show("Procedure percentages converted to claimprocs.");
				commands=new string[]
				{
					"UPDATE procedurelog SET OverridePri='0',OverrideSec='0',NoBillIns='0',"
						+"IsCovIns='0',CapCoPay='0'"
				};
				General.NonQEx(commands);
				//convert medical/service notes from defs table to quickpaste notes----------------------
				commands=new string[]
				{
					"INSERT INTO quickpastecat "
						+"VALUES ('1','Medical Urgent','0','22')"
					,"INSERT INTO quickpastecat "
						+"VALUES ('2','Medical Summary','1','9')"
					,"INSERT INTO quickpastecat "
						+"VALUES ('3','Service Notes','2','10')"
					,"INSERT INTO quickpastecat "
						+"VALUES ('4','Medical History','3','11')"
				};
				General.NonQEx(commands);
				command="SELECT * FROM definition WHERE Category='8'";//Medical Notes
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO quickpastenote (QuickPasteCatNum,ItemOrder,Note) "
						+"VALUES ('1','"+i.ToString()+"','"
						+POut.PString(table.Rows[i][3].ToString())+"')";
					General.NonQEx(command);
					command="INSERT INTO quickpastenote (QuickPasteCatNum,ItemOrder,Note) "
						+"VALUES ('2','"+i.ToString()+"','"
						+POut.PString(table.Rows[i][3].ToString())+"')";
					General.NonQEx(command);
					command="INSERT INTO quickpastenote (QuickPasteCatNum,ItemOrder,Note) "
						+"VALUES ('4','"+i.ToString()+"','"
						+POut.PString(table.Rows[i][3].ToString())+"')";
					General.NonQEx(command);
				}
				command="SELECT * FROM definition WHERE Category='14'";//Service Notes
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO quickpastenote (QuickPasteCatNum,ItemOrder,Note) "
						+"VALUES ('3','"+i.ToString()+"','"
						+POut.PString(table.Rows[i][3].ToString())+"')";
					General.NonQEx(command);
				}
				//add image categories to the chart module-----------------------------------------------
				command="SELECT MAX(ItemOrder) FROM definition WHERE Category=18";
				table=General.GetTableEx(command);
				int lastI=PIn.PInt(table.Rows[0][0].ToString());
				commands=new string[]
				{
					"INSERT INTO definition(Category,ItemOrder,ItemName,ItemValue) "
						+"VALUES(18,"+POut.PInt(lastI+1)+",'BWs','X')"
					,"INSERT INTO definition(Category,ItemOrder,ItemName,ItemValue) "
						+"VALUES(18,"+POut.PInt(lastI+2)+",'FMXs','X')"
					,"INSERT INTO definition(Category,ItemOrder,ItemName,ItemValue) "
						+"VALUES(18,"+POut.PInt(lastI+3)+",'Panos','X')"
					,"INSERT INTO definition(Category,ItemOrder,ItemName,ItemValue) "
						+"VALUES(18,"+POut.PInt(lastI+4)+",'Photos','X')"
					,"UPDATE preference SET ValueString = '3.0.1.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_0_2();
		}

		private void To3_0_2() {
			if(FromVersion < new Version("3.0.2.0")) {
				string[] commands=new string[]
				{
					"INSERT INTO preference VALUES('TreatPlanShowGraphics','1')"
					,"INSERT INTO preference VALUES('TreatPlanShowCompleted','1')"
					,"INSERT INTO preference VALUES('TreatPlanShowIns','1')"
					,"UPDATE preference SET ValueString = '3.0.2.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_0_3();
		}

		private void To3_0_3() {
			if(FromVersion < new Version("3.0.3.0")) {
				string command="SELECT CONCAT(CONCAT(LName,', '),FName) FROM payplan,patient "
					+"WHERE patient.PatNum=payplan.PatNum";
				DataTable table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					string planPats="";
					for(int i=0;i<table.Rows.Count;i++) {
						if(i>0) {
							planPats+=",";
						}
						planPats+=PIn.PString(table.Rows[i][0].ToString());
					}
					MessageBox.Show("You have payment plans for the following patients: "
						+planPats+".  "
						+"There was a bug in the way the amount due was being calculated, so you will "
						+"want to follow these steps to correct the amounts due.  For each payment plan, "
						+"simply open the plan from the patient account and then click OK.  This will "
						+"reset the amount due.");
				}
				string[] commands=new string[]
				{
					"ALTER TABLE payplan ADD TotalCost double NOT NULL"
					,"UPDATE payplan SET TotalCost = TotalAmount"
					,"UPDATE preference SET ValueString = '3.0.3.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_0_4();
		}

		private void To3_0_4() {
			if(FromVersion < new Version("3.0.4.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE procedurelog ADD HideGraphical tinyint unsigned NOT NULL"
					,"ALTER TABLE adjustment CHANGE AdjNote AdjNote text NOT NULL"
					,"UPDATE preference SET ValueString = '3.0.4.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_0_5();
		}

		private void To3_0_5() {
			if(FromVersion < new Version("3.0.5.0")) {
				//Delete procedures for patients that have been deleted:
				string command="SELECT patient.PatNum FROM patient,procedurelog "
					+"WHERE patient.PatNum=procedurelog.PatNum "
					+"AND patient.PatStatus=4";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM procedurelog "
						+"WHERE PatNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				//Delete extra est entries caused when patient switched plans before conversion:
				command=@"SELECT 
					cp1.ClaimProcNum,patient.PatNum,patient.LName,patient.FName
					FROM claimproc cp1,claimproc cp2,patient
					WHERE patient.PatNum=cp1.PatNum
					AND patient.PatNum=cp2.PatNum
					AND patient.PriPlanNum=cp1.PlanNum
					AND patient.SecPlanNum=0
					AND cp1.ProcNum=cp2.ProcNum
					AND cp1.ClaimProcNum!=cp2.ClaimProcNum
					AND cp1.Status=6";//estimate
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM claimproc "
						+"WHERE ClaimProcNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				string[] commands=new string[]
				{
					"ALTER TABLE claimform CHANGE UniqueID UniqueID varchar(255) NOT NULL"
					,"UPDATE claimform SET UniqueID=concat('OD',UniqueID)"
					,"UPDATE claimform SET UniqueID='' WHERE UniqueID='OD0'"
					,"UPDATE preference SET ValueString = '3.0.5.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_1_0();
		}

		private void To3_1_0() {
			if(FromVersion < new Version("3.1.0.0")) {
				//if(!File.Exists(@"ConversionFiles\convert_3_1_0.txt")){
				//	throw new Exception(@"ConversionFiles\convert_3_1_0.txt"+" could not be found.");
				//}
				ExecuteFile(@"ConversionFiles\convert_3_1_0.txt");//Might throw an exception which we handle
				//add Sirona Sidexis:
				string command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'Sirona', "
					+"'Sirona Sidexis from www.sirona.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\sidexis\sidexis.exe")+"', "
					+"'', "
					+"'')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'Sirona')";
				General.NonQEx(command);
				//convert recall
				//For inactive patients, assume no meaningful info if patients inactive,
				//so no need to create a recall. Only convert active patients.
				command="SELECT PatNum,RecallStatus,RecallInterval "
					+"FROM patient WHERE PatStatus=0";
				DataTable patTable=General.GetTableEx(command);
				DataTable table;
				DateTime previousDate;
				DateTime dueDate;
				int patNum;
				int status;
				int interval;
				Interval newInterval;
				for(int i=0;i<patTable.Rows.Count;i++) {
					patNum=PIn.PInt(patTable.Rows[i][0].ToString());
					status=PIn.PInt(patTable.Rows[i][1].ToString());
					interval=PIn.PInt(patTable.Rows[i][2].ToString());
					//get previous date
					command="SELECT MAX(procedurelog.procdate) "
						+"FROM procedurelog,procedurecode "
						+"WHERE procedurelog.PatNum="+patNum.ToString()
						+" AND procedurecode.ADACode = procedurelog.ADACode "
						+"AND procedurecode.SetRecall = 1 "
						+"AND (procedurelog.ProcStatus = 2 "
						+"OR procedurelog.ProcStatus = 3 "
						+"OR procedurelog.ProcStatus = 4) "
						+"GROUP BY procedurelog.PatNum";
					table=General.GetTableEx(command);
					if(table.Rows.Count==0) {
						previousDate=DateTime.MinValue;
					}
					else {
						previousDate=PIn.PDate(table.Rows[0][0].ToString());
					}
					//If no useful info and no trigger. No recall created
					if(status==0 && (interval==0 || interval==6)
						&& previousDate==DateTime.MinValue)//and no trigger
					{
						continue;
					}
					if(interval==0) {
						newInterval=new Interval(0,0,6,0);
					}
					else {
						newInterval=new Interval(0,0,interval,0);
					}
					if(previousDate==DateTime.MinValue) {
						dueDate=DateTime.MinValue;
					}
					else {
						dueDate=previousDate+newInterval;
					}
					command="INSERT INTO recall (PatNum,DateDueCalc,DateDue,DatePrevious,"
						+"RecallInterval,RecallStatus"
						+") VALUES ("
						+"'"+POut.PInt(patNum)+"', "
						+POut.PDate(dueDate)+", "
						+POut.PDate(dueDate)+", "
						+POut.PDate(previousDate)+", "
						+"'"+POut.PInt(newInterval.ToInt())+"', "
						+"'"+POut.PInt(status)+"')";
					General.NonQEx(command);
				}//for int i<patTable
				command="UPDATE preference SET ValueString = '3.1.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_1_3();
		}

		private void To3_1_3() {
			if(FromVersion < new Version("3.1.3.0")) {
				//0 values in date fields are causing a lot of program slowdown
				string[] commands=new string[]
				{
					"UPDATE adjustment SET AdjDate='0001-01-01' WHERE AdjDate='0000-00-00'"
					,"UPDATE appointment SET AptDateTime='0001-01-01 00:00:00' "
						+"WHERE AptDateTime LIKE '0000-00-00%'"
					,"UPDATE claim SET DateService='0001-01-01' WHERE DateService='0000-00-00'"
					,"UPDATE claim SET DateSent='0001-01-01' WHERE DateSent='0000-00-00'"
					,"UPDATE claim SET DateReceived='0001-01-01' WHERE DateReceived='0000-00-00'"
					,"UPDATE claim SET PriorDate='0001-01-01' WHERE PriorDate='0000-00-00'"
					,"UPDATE claim SET AccidentDate='0001-01-01' WHERE AccidentDate='0000-00-00'"
					,"UPDATE claim SET OrthoDate='0001-01-01' WHERE OrthoDate='0000-00-00'"
					,"UPDATE claimpayment SET CheckDate='0001-01-01' WHERE CheckDate='0000-00-00'"
					,"UPDATE claimproc SET DateCP='0001-01-01' WHERE DateCP='0000-00-00'"
					,"UPDATE claimproc SET ProcDate='0001-01-01' WHERE ProcDate='0000-00-00'"
					,"UPDATE insplan SET DateEffective='0001-01-01' WHERE DateEffective='0000-00-00'"
					,"UPDATE insplan SET DateTerm='0001-01-01' WHERE DateTerm='0000-00-00'"
					,"UPDATE insplan SET RenewMonth='1' WHERE RenewMonth='0'"
					,"UPDATE patient SET Birthdate='0001-01-01' WHERE Birthdate='0000-00-00'"
					,"UPDATE patient SET DateFirstVisit='0001-01-01' WHERE DateFirstVisit='0000-00-00'"
					,"UPDATE procedurelog SET ProcDate='0001-01-01' WHERE ProcDate='0000-00-00'"
					,"UPDATE procedurelog SET DateOriginalProsth='0001-01-01' "
						+"WHERE DateOriginalProsth='0000-00-00'"
					,"UPDATE procedurelog SET DateLocked='0001-01-01' WHERE DateLocked='0000-00-00'"
					,"UPDATE recall SET DateDueCalc='0001-01-01' WHERE DateDueCalc='0000-00-00'"
					,"UPDATE recall SET DateDue='0001-01-01' WHERE DateDue='0000-00-00'"
					,"UPDATE recall SET DatePrevious='0001-01-01' WHERE DatePrevious='0000-00-00'"
					,"ALTER table adjustment CHANGE AdjDate AdjDate date NOT NULL default '0001-01-01'"
					,"ALTER table appointment CHANGE AptDateTime AptDateTime datetime NOT NULL "
						+"default '0001-01-01 00:00:00'"
					,"ALTER table claim CHANGE DateService DateService date NOT NULL default '0001-01-01'"
					,"ALTER table claim CHANGE DateSent DateSent date NOT NULL default '0001-01-01'"
					,"ALTER table claim CHANGE DateReceived DateReceived date NOT NULL default '0001-01-01'"
					,"ALTER table claim CHANGE PriorDate PriorDate date NOT NULL default '0001-01-01'"
					,"ALTER table claim CHANGE AccidentDate AccidentDate date NOT NULL default '0001-01-01'"
					,"ALTER table claim CHANGE OrthoDate OrthoDate date NOT NULL default '0001-01-01'"
					,"ALTER table claimpayment CHANGE CheckDate CheckDate date NOT NULL default '0001-01-01'"
					,"ALTER table claimproc CHANGE DateCP DateCP date NOT NULL default '0001-01-01'"
					,"ALTER table claimproc CHANGE ProcDate ProcDate date NOT NULL default '0001-01-01'"
					,"ALTER table insplan CHANGE DateEffective DateEffective date NOT NULL default '0001-01-01'"
					,"ALTER table insplan CHANGE DateTerm DateTerm date NOT NULL default '0001-01-01'"
					,"ALTER table insplan CHANGE RenewMonth RenewMonth tinyint unsigned NOT NULL default '1'"
					,"ALTER table patient CHANGE Birthdate Birthdate date NOT NULL default '0001-01-01'"
					,"ALTER table patient CHANGE DateFirstVisit DateFirstVisit date NOT NULL default '0001-01-01'"
					,"ALTER table procedurelog CHANGE ProcDate ProcDate date NOT NULL default '0001-01-01'"
					,"ALTER table procedurelog CHANGE DateOriginalProsth DateOriginalProsth "
						+"date NOT NULL default '0001-01-01'"
					,"ALTER table procedurelog CHANGE DateLocked DateLocked date NOT NULL default '0001-01-01'"
					,"ALTER table recall CHANGE DateDueCalc DateDueCalc date NOT NULL default '0001-01-01'"
					,"ALTER table recall CHANGE DateDue DateDue date NOT NULL default '0001-01-01'"
					,"ALTER table recall CHANGE DatePrevious DatePrevious date NOT NULL default '0001-01-01'"
					//Set prosth codes
					,"UPDATE procedurecode SET IsProsth=1 WHERE ADACode='D2740' || ADACode='D2750' "
						+"|| ADACode='D2751' || ADACode='D2752' || ADACode='D2790' || ADACode='D2791' "
						+"|| ADACode='D2792' || ADACode='D5110' || ADACode='D5120' || ADACode='D5130' "
						+"|| ADACode='D5140' || ADACode='D5211' || ADACode='D5212' || ADACode='D5213' "
						+"|| ADACode='D5214' || ADACode='D5225' || ADACode='D5226' || ADACode='D5281'"
						+"|| ADACode='D5810' || ADACode='D5811' || ADACode='D5820' || ADACode='D5821'"
						+"|| ADACode LIKE 'D62%' || ADACode LIKE 'D65%' || ADACode LIKE 'D66%' "
						+"|| ADACode LIKE 'D67%'"
					//add new ada codes
					//,"INSERT INTO procedurecode "
					,"INSERT INTO definition(Category,ItemOrder,ItemName,ItemValue,ItemColor,IsHidden) "
						+"VALUES ('21','7','Commlog Appt Related','','-886','0')"
					,"UPDATE preference SET ValueString = '3.1.3.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_1_4();
		}

		private void To3_1_4() {
			if(FromVersion < new Version("3.1.4.0")) {
				string[] commands=new string[]
				{
					"ALTER table clearinghouse ADD LoginID varchar(255) NOT NULL"
					,"UPDATE clearinghouse SET ReceiverID='0135WCH00' WHERE ReceiverID='WebMD'"
					,"ALTER table provider ADD OutlineColor int NOT NULL"
					,"UPDATE provider SET OutlineColor ='-11711155'"
					,"UPDATE preference SET ValueString = '3.1.4.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_1_13();
		}

		private void To3_1_13() {
			if(FromVersion < new Version("3.1.13.0")) {
				//get rid of any medication pats where medication no longer exists.
				string command="SELECT medicationpat.MedicationPatNum FROM medicationpat "
					+"LEFT JOIN medication ON medicationpat.MedicationNum=medication.MedicationNum "
					+"WHERE medication.MedicationNum IS NULL";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM medicationpat WHERE MedicationPatNum="
						+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				string[] commands=new string[]
				{
					"INSERT INTO clearinghouse(Description,ExportPath,IsDefault,Payors,Eformat,ReceiverID,"
						+"SenderID,Password,ResponsePath,CommBridge,ClientProgram) "
						+@"VALUES('RECS','C:\\Recscom\\','0','','1','RECS','','',"
						+@"'','5','C:\\Recscom\\Recscom.exe')"
					,"UPDATE preference SET ValueString = '3.1.13.0' WHERE PrefName = 'DataBaseVersion'"
				};

				General.NonQEx(commands);
			}
			To3_1_16();
		}

		private void To3_1_16() {
			if(FromVersion < new Version("3.1.16.0")) {
				//this functionality is all copied directly from the Check Database tool.
				string command=@"SELECT PatNum FROM patient
					LEFT JOIN insplan on patient.PriPlanNum=insplan.PlanNum
					WHERE patient.PriPlanNum != 0
					AND insplan.PlanNum IS NULL";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE patient set PriPlanNum=0 "
						+"WHERE PatNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				command=@"SELECT ClaimProcNum FROM claimproc
					LEFT JOIN insplan ON claimproc.PlanNum=insplan.PlanNum
					WHERE insplan.PlanNum IS NULL";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM claimproc "
						+"WHERE ClaimProcNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				command=@"SELECT ClaimNum FROM claim
					LEFT JOIN insplan ON claim.PlanNum=insplan.PlanNum
					WHERE insplan.PlanNum IS NULL";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM claim "
						+"WHERE ClaimNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				command="UPDATE preference SET ValueString = '3.1.16.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_4_0();
		}

		private void To3_4_0() {
			if(FromVersion < new Version("3.4.0.0")) {
				ExecuteFile(@"ConversionFiles\convert_3_4_0.txt");//Might throw an exception which we handle.
				//----------------Copy payment dates into paysplits--------------------------------------
				string command="SELECT paysplit.SplitNum,payment.PayDate FROM payment,paysplit "
					+"WHERE payment.PayNum=paysplit.PayNum";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE paysplit SET "
						+"DatePay="+POut.PDate(PIn.PDate(table.Rows[i][1].ToString()))+" "
						+"WHERE SplitNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				//----------------Convert all discounts to adjustments-----------------------------------
				//add adjustment categories.
				command="SELECT Max(ItemOrder) FROM definition WHERE Category=1";
				table=General.GetTableEx(command);
				int firstItemOrder=PIn.PInt(table.Rows[0][0].ToString())+1;
				command="SELECT * FROM definition WHERE Category=15 ORDER BY ItemOrder";//cat=DiscountTypes
				table=General.GetTableEx(command);
				Hashtable HDiscToAdj=new Hashtable();//key=original defNum(discountType. value=new defNum(AdjType)
				int numAdj=0;
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO definition (category,itemorder,itemname,itemvalue,ishidden) VALUES("
					+"1, "//category=AdjTypes
					+"'"+POut.PInt(firstItemOrder+i)+"', "//itemOrder
					+"'"+POut.PString(PIn.PString(table.Rows[i][3].ToString()))+"', "//item name
					+"'-', "//itemValue. All discounts are negative
					+"'"+table.Rows[i][6].ToString()+"')";//is hidden
					numAdj=General.NonQEx(command,true);
					HDiscToAdj.Add(PIn.PInt(table.Rows[i][0].ToString()),//defNum of disc
						numAdj);//defNum of adj
				}
				//handle 0:
				HDiscToAdj.Add(0,numAdj);
				//create new adjustments from existing discounts
				command="SELECT * FROM paysplit WHERE IsDiscount=1";//0=SplitNum,1=SplitAmt,2=PatNum,3=ProcDate,
				//4=PayNum,5=IsDiscount,6=DiscountType,7=ProvNum,8=PayPlanNum,9=DatePay
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO adjustment (AdjDate,AdjAmt,PatNum, "
					+"AdjType,ProvNum,ProcDate) "//AdjNote
					+"VALUES("
					+POut.PDate(PIn.PDate(table.Rows[i][9].ToString()))+", "//entryDate
					+"'"+POut.PDouble(-PIn.PDouble(table.Rows[i][1].ToString()))+"', "//amt
					+"'"+POut.PInt(PIn.PInt(table.Rows[i][2].ToString()))+"', "//patNum
					+"'"+POut.PInt((int)HDiscToAdj[PIn.PInt(table.Rows[i][6].ToString())])+"', "//type
					+"'"+POut.PInt(PIn.PInt(table.Rows[i][7].ToString()))+"', "//provNum
					+POut.PDate(PIn.PDate(table.Rows[i][3].ToString()))+")";//procDate
					//note
					General.NonQEx(command);
				}
				command="DELETE FROM paysplit WHERE IsDiscount=1";
				General.NonQEx(command);
				//--------------------Printers----------------------------------------------------------
				command="SELECT * FROM computer WHERE PrinterName != ''";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO printer (ComputerNum,PrintSit,PrinterName,"
					+"DisplayPrompt) "
					+"VALUES("
					+"'"+POut.PInt(PIn.PInt(table.Rows[i][0].ToString()))+"', "
					+"'"+POut.PInt((int)PrintSituation.Default)+"', "
					+"'"+POut.PString(PIn.PString(table.Rows[i][2].ToString()))+"', "
					+"'1')";
					General.NonQEx(command);
				}
				command="UPDATE computer SET PrinterName = ''";
				General.NonQEx(command);
				//HouseCalls link-----------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'HouseCalls', "
					+"'HouseCalls from www.housecallsweb.com', "
					+"'0', "
					+"'', "
					+"'', "
					+"'"+POut.PString(@"Typical Export Path is C:\HouseCalls\")+"')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Export Path', "
					+"'"+POut.PString(@"C:\HouseCalls\")+"')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'HouseCalls')";
				General.NonQEx(command);
				//Delete program links for WebClaim and Renaissance--------------------------------------


				//Final cleanup-------------------------------------------------------------------------
				command="UPDATE preference SET ValueString = '3.4.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_4_7();
		}

		private void To3_4_7() {
			if(FromVersion < new Version("3.4.7.0")) {
				string[] commands=new string[]
				{
					"INSERT INTO clearinghouse(Description,ExportPath,IsDefault,Payors,Eformat,ReceiverID,"
						+"SenderID,Password,ResponsePath,CommBridge,ClientProgram) "
						+@"VALUES('WebClaim','C:\\WebClaim\\Upload\\','0','','1','330989922','','',"
						+@"'','4','')"
					,"UPDATE preference SET ValueString = '3.4.7.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_4_10();
		}

		private void To3_4_10() {
			//the only purpose of this is to check the bug fix in conversions
			if(FromVersion < new Version("3.4.10.0")) {
				string[] commands=new string[]
				{
					"UPDATE preference SET ValueString = '3.4.10.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_4_11();
		}

		private void To3_4_11() {
			if(FromVersion < new Version("3.4.11.0")) {
				//Planmeca link-----------------------------------------------------------------------
				string command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'Planmeca', "
					+"'Dimaxis from Planmeca', "
					+"'0', "
					+"'DxStart.exe', "
					+"'', "
					+"'"+POut.PString(@"Typical file path is DxStart.exe which is available from Planmeca and should be placed in the same folder as this program.")+"')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'Planmeca')";
				General.NonQEx(command);
				command=
					"UPDATE preference SET ValueString = '3.4.11.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_4_16();
		}

		private void To3_4_16() {
			if(FromVersion < new Version("3.4.16.0")) {
				string[] commands=new string[]
				{
					@"UPDATE clearinghouse SET Description='ClaimConnect',ExportPath='C:\\ClaimConnect\\Upload\\' WHERE Description='WebClaim'"
					,"UPDATE preference SET ValueString = '3.4.16.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_4_17();
		}

		private void To3_4_17() {
			if(FromVersion < new Version("3.4.17.0")) {
				string[] commands=new string[]
				{
					"UPDATE patient SET DateFirstVisit='0001-01-01' WHERE DateFirstVisit='0000-00-00'"
					,"UPDATE preference SET ValueString = '3.4.17.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_4_24();
		}

		private void To3_4_24() {
			if(FromVersion < new Version("3.4.24.0")) {
				//Delete program links for WebClaim and Renaissance--------------------------------------
				string command="SELECT ProgramNum FROM program WHERE ProgName='WebClaim' OR ProgName='Renaissance'";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM program WHERE ProgramNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
					command="DELETE FROM toolbutitem WHERE ProgramNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				//Fix utf8 binary collations for ADACode columns-------------------------------------------
				command="SELECT @@version";
				table=General.GetTableEx(command);
				string thisVersion=PIn.PString(table.Rows[0][0].ToString());
				string[] commands;
				if(thisVersion.Substring(0,3)=="4.1" || thisVersion.Substring(0,3)=="5.0") {
					commands=new string[]
					{
						"ALTER TABLE procedurecode CHANGE ADACode ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE procedurecode DEFAULT character set utf8"
						,"ALTER TABLE procedurecode MODIFY Descript varchar(255) character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY AbbrDesc varchar(50) character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY ProcTime varchar(24) character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY DefaultNote text character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY AlternateCode1 varchar(15) character set utf8 NOT NULL"
						,"ALTER TABLE procedurelog MODIFY ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE autocodeitem MODIFY ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE procbuttonitem MODIFY ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE covspan MODIFY FromCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE covspan MODIFY ToCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					};
					General.NonQEx(commands);
				}
				commands=new string[]
				{
				//Inmediata clearinghouse--------------------------------------------------------------
					"INSERT INTO clearinghouse(Description,ExportPath,IsDefault,Payors,Eformat,ReceiverID,"
						+"SenderID,Password,ResponsePath,CommBridge,ClientProgram) "
						+@"VALUES('Inmediata Health Group Corp','C:\\Inmediata\\Claims\\','0','','1','660610220','','',"
						+@"'C:\\Inmediata\\Reports\\','6','C:\\Program Files\\Inmediata\\IMPlug.exe')"
					,"UPDATE preference SET ValueString = '3.4.24.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_5_0();
		}

		private void To3_5_0() {
			if(FromVersion < new Version("3.5.0.0")) {
				ExecuteFile(@"ConversionFiles\convert_3_5_0.txt");//Might throw an exception which we handle.
				//Add patient picture category to images
				string command="SELECT MAX(ItemOrder) FROM definition WHERE Category=18";
				DataTable table=General.GetTableEx(command);
				int lastI=PIn.PInt(table.Rows[0][0].ToString());
				command="INSERT INTO definition(Category,ItemOrder,ItemName,ItemValue) "
					+"VALUES(18,"+POut.PInt(lastI+1)+",'Patient Pictures','P')";
				General.NonQEx(command);
				//ImageFX link-----------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'ImageFX', "
					+"'ImageFX from scican.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\ImageFX\ImageFX.exe")+"', "
					+"'', "
					+"'"+POut.PString(@"Typical file path is C:\ImageFX\ImageFX.exe")+"')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'ImageFX')";
				General.NonQEx(command);
				//fix the provider ID field----------------------------------------------------------------
				command="SELECT ClaimFormNum FROM claimform WHERE UniqueID='OD1'";
				table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					command="UPDATE claimformitem SET FieldName='BillingDentistProviderID' WHERE FieldName='BillingDentistMedicaidID' "
						+"AND ClaimFormNum="+table.Rows[0][0].ToString();
					General.NonQEx(command);
				}
				command=
					"UPDATE preference SET ValueString = '3.5.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_5_1();
		}

		private void To3_5_1() {
			if(FromVersion < new Version("3.5.1.0")) {
				string[] commands=new string[]
				{
					"ALTER TABLE schedule CHANGE Note Note TEXT NOT NULL"
					,"UPDATE preference SET ValueString = '3.5.1.0' WHERE PrefName = 'DataBaseVersion'"
				};
				General.NonQEx(commands);
			}
			To3_5_3();
		}

		private void To3_5_3() {
			if(FromVersion < new Version("3.5.3.0")) {
				//DentForms link-----------------------------------------------------------------------
				string command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'DentForms', "
					+"'DentForms from medictalk.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\MedicTalk\reports\mtconnector.exe")+"', "
					+"'', "
					+"'"+POut.PString(@"No command line is needed.  Typical path is C:\MedicTalk\reports\mtconnector.exe")+"')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'DentForms')";
				General.NonQEx(command);
				command="UPDATE tasklist SET DateType=0 WHERE Parent !=0";
				General.NonQEx(command);
				command="UPDATE task SET DateType=0, TaskStatus=0 WHERE TaskListNum !=0";
				General.NonQEx(command);
				command=
					"UPDATE preference SET ValueString = '3.5.3.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_6_0();
		}

		private void To3_6_0() {
			if(FromVersion < new Version("3.6.0.0")) {
				ExecuteFile(@"ConversionFiles\convert_3_6_0.txt");//Might throw an exception which we handle.
				string command;
				command=
					"UPDATE preference SET ValueString = '3.6.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_6_1();
		}

		private void To3_6_1() {
			if(FromVersion < new Version("3.6.1.0")) {
				string command;
				//Not sure how some of the dates got out of synch:
				command="UPDATE payment,paysplit SET paysplit.DatePay=payment.PayDate WHERE paysplit.PayNum=payment.PayNum";
				General.NonQEx(command);
				//or how procedures can accidently get attached to appointments for different patients:
				command="UPDATE procedurelog,appointment SET procedurelog.AptNum=0 "
					+"WHERE procedurelog.AptNum=appointment.AptNum AND appointment.PatNum!=procedurelog.PatNum";
				General.NonQEx(command);
				command=
					"UPDATE preference SET ValueString = '3.6.1.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_6_4();
		}

		private void To3_6_4() {
			//duplicate of To3_5_6 because we needed to fix for users who had already upgraded to 3.6
			if(FromVersion < new Version("3.6.4.0")) {
				string[] commands=new string[]
					{
						"ALTER TABLE procedurecode CHANGE ADACode ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE procedurecode DEFAULT character set utf8"
						,"ALTER TABLE procedurecode MODIFY Descript varchar(255) character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY AbbrDesc varchar(50) character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY ProcTime varchar(24) character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY DefaultNote text character set utf8 NOT NULL"
						,"ALTER TABLE procedurecode MODIFY AlternateCode1 varchar(15) character set utf8 NOT NULL"
						,"ALTER TABLE procedurelog MODIFY ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE autocodeitem MODIFY ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE procbuttonitem MODIFY ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE covspan MODIFY FromCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE covspan MODIFY ToCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
						,"ALTER TABLE fee MODIFY ADACode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					};
				General.NonQEx(commands);
				commands=new string[]
				{
					//AOS DATA clearinghouse----------------------------------------ADDED by SPK 7/13/05----------------------
					"INSERT INTO clearinghouse(Description,ExportPath,IsDefault,Payors,Eformat,ReceiverID,"
					+"SenderID,Password,ResponsePath,CommBridge,ClientProgram) "
					+@"VALUES('AOS Data systems','C:\\Program Files\\AOS\\','0','','1','AOS','','',"
					+@"'C:\\Program Files\\AOS\\','7','C:\\Program Files\\AOS\\AOSCommunicator\\AOSCommunicator.exe')"
 				};
				General.NonQEx(commands);
				string command="UPDATE preference SET ValueString = '3.6.4.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_6_5();
		}

		private void To3_6_5() {
			if(FromVersion < new Version("3.6.5.0")) {
				//delete any unattached adjustments
				string command="SELECT adjustment.AdjNum,procedurelog.ProcNum FROM adjustment "
					+"LEFT JOIN procedurelog ON procedurelog.ProcNum=adjustment.ProcNum "
					+"WHERE adjustment.ProcNum !=0 "
					+"AND procedurelog.ProcNum IS NULL";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM adjustment WHERE AdjNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				command="UPDATE preference SET ValueString = '3.6.5.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_7_0();
		}

		private void To3_7_0() {
			if(FromVersion < new Version("3.7.0.0")) {
				ExecuteFile(@"ConversionFiles\convert_3_7_0.txt");//Might throw an exception which we handle.
				string command;
				//Convert pay plans-----------------------------------------------------------------------------
				command="SELECT PayPlanNum,PatNum,Guarantor,PayPlanDate,TotalAmount,APR,"//0-5
					+"PeriodPayment,Term,AccumulatedDue,DateFirstPay,DownPayment,"//6-10
					+"Note,TotalCost,LastPayment "//11-13
					+"FROM payplan";
				DataTable table=General.GetTableEx(command);
				int payPlanNum;//0
				int patNum;//1
				int guarantor;//2
				DateTime payPlanDate;// 3
				double totalAmount;//4 aka principal. This gets reduced to 0 in loop
				double APR;// 5
				double monthlyPayment;//6
				int term;//7
				//CurrentDue 8
				DateTime dateFirstPay;//9
				double downPayment;//10
				//Note 11
				double totalCost;// 12. Princ+Int. This gets reduced to 0 in loop
				double lastPayment;//13
				//variables used for the individual charges:
				DateTime chargeDate;
				double principal;
				double interest;
				double monthlyRate;
				for(int i=0;i<table.Rows.Count;i++) {
					payPlanNum=    PIn.PInt(table.Rows[i][0].ToString());
					patNum=        PIn.PInt(table.Rows[i][1].ToString());
					guarantor=     PIn.PInt(table.Rows[i][2].ToString());
					payPlanDate=   PIn.PDate(table.Rows[i][3].ToString());
					totalAmount=   PIn.PDouble(table.Rows[i][4].ToString());
					APR=           PIn.PDouble(table.Rows[i][5].ToString());
					monthlyPayment=PIn.PDouble(table.Rows[i][6].ToString());
					term=          PIn.PInt(table.Rows[i][7].ToString());
					dateFirstPay=  PIn.PDate(table.Rows[i][9].ToString());
					downPayment=   PIn.PDouble(table.Rows[i][10].ToString());
					totalCost=     PIn.PDouble(table.Rows[i][12].ToString());
					lastPayment=   PIn.PDouble(table.Rows[i][13].ToString());
					//down payment
					if(downPayment>0) {
						chargeDate=payPlanDate;
						principal=downPayment;
						totalCost-=downPayment;
						totalAmount-=downPayment;
						interest=0;
						command="INSERT INTO payplancharge (PayPlanNum,Guarantor,PatNum,ChargeDate,Principal,Interest,Note) VALUES("
							+"'"+POut.PInt(payPlanNum)+"', "
							+"'"+POut.PInt(guarantor)+"', "
							+"'"+POut.PInt(patNum)+"', "
							+POut.PDate(chargeDate)+", "
							+"'"+POut.PDouble(principal)+"', "
							+"'"+POut.PDouble(interest)+"', "
							+"'Downpayment')";
						General.NonQEx(command);
					}
					if(APR==0) {
						monthlyRate=0;
					}
					else {
						monthlyRate=APR/100/12;
					}
					for(int j=0;j<term;j++) {
						chargeDate=dateFirstPay.AddMonths(j);
						if(j==term-1 && lastPayment==0) {//if this is the very last payment
							//all remaining principal gets applied
							principal=totalAmount;
							totalCost-=totalAmount;
							totalAmount=0;
							//all remaining interest gets applied
							interest=totalCost;
							totalCost=0;
						}
						else {
							interest=Math.Round((totalAmount*monthlyRate),2);//2 decimals
							principal=monthlyPayment-interest;
							totalAmount-=principal;
							totalCost-=monthlyPayment;
						}
						if(principal<0) {
							principal=0;
						}
						if(interest<0) {
							interest=0;
						}
						command="INSERT INTO payplancharge (PayPlanNum,Guarantor,PatNum,ChargeDate,Principal,Interest) VALUES("
							+"'"+POut.PInt(payPlanNum)+"', "
							+"'"+POut.PInt(guarantor)+"', "
							+"'"+POut.PInt(patNum)+"', "
							+POut.PDate(chargeDate)+", "
							+"'"+POut.PDouble(principal)+"', "
							+"'"+POut.PDouble(interest)+"')";
						General.NonQEx(command);
					}//loop term
					//last payment
					if(lastPayment!=0) {
						chargeDate=dateFirstPay.AddMonths(term);
						//all remaining principal gets applied
						principal=totalAmount;
						totalCost-=totalAmount;
						totalAmount=0;
						//all remaining interest gets applied
						interest=totalCost;
						totalCost=0;
						command="INSERT INTO payplancharge (PayPlanNum,Guarantor,PatNum,ChargeDate,Principal,Interest) VALUES("
							+"'"+POut.PInt(payPlanNum)+"', "
							+"'"+POut.PInt(guarantor)+"', "
							+"'"+POut.PInt(patNum)+"', "
							+POut.PDate(chargeDate)+", "
							+"'"+POut.PDouble(principal)+"', "
							+"'"+POut.PDouble(interest)+"')";
						General.NonQEx(command);
					}
				}
				//get rid of unwanted columns in pay plans
				string[] commands=new string[]
					{
						"ALTER TABLE payplan DROP TotalAmount"
						,"ALTER TABLE payplan DROP PeriodPayment"
						,"ALTER TABLE payplan DROP Term"
						,"ALTER TABLE payplan DROP AccumulatedDue"
						,"ALTER TABLE payplan DROP DateFirstPay"
						,"ALTER TABLE payplan DROP DownPayment"
						,"ALTER TABLE payplan DROP TotalCost"
						,"ALTER TABLE payplan DROP LastPayment"
					};
				General.NonQEx(commands);
				//Operatories----------------------------------------------------------------------------------------------
				command="SELECT DefNum,ItemOrder,ItemName,ItemValue,IsHidden FROM definition WHERE Category=9 ORDER BY ItemOrder";
				table=General.GetTableEx(command);
				//Hashtable hashOps=new Hashtable();//key=defNum,value=OperatoryNum
				int defNum;//represents the old opNum as it was in the database
				int opNum;//the newly assigned key
				string itemName;
				string itemValue;
				for(int i=0;i<table.Rows.Count;i++) {
					defNum=PIn.PInt(table.Rows[i][0].ToString());
					itemName=PIn.PString(table.Rows[i][2].ToString());
					itemValue=PIn.PString(table.Rows[i][3].ToString());
					command="INSERT INTO operatory (OpName,Abbrev,ItemOrder,IsHidden) VALUES("
						+"'"+POut.PString(itemName)+"', "
						+"'"+POut.PString(itemValue)+"', "
						+"'"+table.Rows[i][1].ToString()+"', "
						+"'"+table.Rows[i][4].ToString()+"')";
					opNum=General.NonQEx(command,true);
					command="UPDATE appointment SET Op="+POut.PInt(opNum)+" WHERE Op="+POut.PInt(defNum);
					General.NonQEx(command);
					command="UPDATE scheddefault SET Op="+POut.PInt(opNum)+" WHERE Op="+POut.PInt(defNum);
					General.NonQEx(command);
					command="UPDATE apptviewitem SET OpNum="+POut.PInt(opNum)+" WHERE OpNum="+POut.PInt(defNum);
					General.NonQEx(command);
				}
				command="DELETE FROM definition WHERE Category=9";
				General.NonQEx(command);
				//final cleanup-----------------------------------------------------------------------------------------
				command=
					"UPDATE preference SET ValueString = '3.7.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_7_2();
		}

		private void To3_7_2() {
			if(FromVersion < new Version("3.7.2.0")) {
				//add the new permission types to each group
				string command="SELECT UserGroupNum FROM usergroup";
				DataTable table=General.GetTableEx(command);
				int groupNum;
				for(int i=0;i<table.Rows.Count;i++) {
					groupNum=PIn.PInt(table.Rows[i][0].ToString());
					command="INSERT INTO grouppermission (UserGroupNum,PermType) VALUES("+POut.PInt(groupNum)+",25)";
					General.NonQEx(command);
					command="INSERT INTO grouppermission (UserGroupNum,PermType) VALUES("+POut.PInt(groupNum)+",26)";
					General.NonQEx(command);
					command="INSERT INTO grouppermission (UserGroupNum,PermType) VALUES("+POut.PInt(groupNum)+",27)";
					General.NonQEx(command);
					//by default, nobody will have permission to backup
					//command="INSERT INTO grouppermission (UserGroupNum,PermType) VALUES("+POut.PInt(groupNum)+",28)";
					//General.NonQEx(command);
					//also by default, nobody will have permission to TimcardsEditAll
				}
				command="ALTER TABLE user ADD EmployeeNum smallint NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.7.2.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_7_3();
		}

		private void To3_7_3() {
			if(FromVersion < new Version("3.7.3.0")) {
				string command="ALTER TABLE securitylog ADD PatNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				command="ALTER TABLE tasklist ADD DateTimeEntry datetime NOT NULL default '0001-01-01'";
				General.NonQEx(command);
				command="ALTER TABLE task ADD DateTimeEntry datetime NOT NULL default '0001-01-01'";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('BalancesDontSubtractIns','0')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.7.3.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_7_4();
		}

		private void To3_7_4() {
			if(FromVersion < new Version("3.7.4.0")) {
				//Easy Notes Pro link-----------------------------------------------------------------------
				string command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'EasyNotesPro', "
					+"'Easy Notes Pro from easynotespro.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\Program Files\EasyNotesPro\AppBarProcess.exe")+"', "
					+"'"+POut.PString("\""+@"C:\Program Files\EasyNotesPro\DefaultDentalToolbar.etb"+"\""+" OpenDental false")+"', "
					+"'"+POut.PString(@"Do not try to add buttons to your toolbars because that won't work.  Typical path is C:\Program Files\EasyNotesPro\AppBarProcess.exe")+"')";
				General.NonQEx(command,true);
				command=
					"UPDATE preference SET ValueString = '3.7.4.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_7_5();
		}

		private void To3_7_5() {
			if(FromVersion < new Version("3.7.5.0")) {
				string command="INSERT INTO preference VALUES ('TimecardSecurityEnabled','0')";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('RecallCardsShowReturnAdd','1')";
				General.NonQEx(command);
				command="ALTER TABLE insplan ADD BenefitNotes text NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.7.5.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_7_6();
		}

		private void To3_7_6() {
			if(FromVersion < new Version("3.7.6.0")) {
				string command="ALTER TABLE clinic ADD DefaultPlaceService tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.7.6.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_8_0();
		}

		private void To3_8_0() {
			if(FromVersion < new Version("3.8.0.0")) {
				ExecuteFile(@"ConversionFiles\convert_3_8_0.txt");//Might throw an exception which we handle.
				//add deposit slip permission to each group
				string command="SELECT UserGroupNum FROM usergroup";
				DataTable table=General.GetTableEx(command);
				int groupNum;
				for(int i=0;i<table.Rows.Count;i++) {
					groupNum=PIn.PInt(table.Rows[i][0].ToString());
					command="INSERT INTO grouppermission (UserGroupNum,PermType) VALUES("+POut.PInt(groupNum)+",30)";
					General.NonQEx(command);
				}
				//Populate the new column: claimpayment.CarrierName
				command="SELECT claimpayment.ClaimPaymentNum,carrier.CarrierName "
					+"FROM claimpayment,claimproc,insplan,carrier "
					+"WHERE claimproc.ClaimPaymentNum = claimpayment.ClaimPaymentNum "
					+"AND claimproc.PlanNum = insplan.PlanNum "
					+"AND insplan.CarrierNum = carrier.CarrierNum "
					+"GROUP BY claimpayment.ClaimPaymentNum";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE claimpayment SET CarrierName='"+POut.PString(PIn.PString(table.Rows[i][1].ToString()))+"' "
						+"WHERE ClaimPaymentNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				command="UPDATE preference SET ValueString = '3.8.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_8_5();
		}

		private void To3_8_5() {
			if(FromVersion < new Version("3.8.5.0")) {
				//Make a few changes to the paths in the ENP bridge
				string command;//="SELECT ProgramNum FROM program WHERE ProgName='EasyNotesPro'";
				//DataTable table=General.GetTableEx(command);
				//if(table.Rows.Count>0){//otherwise user might have deleted the bridge
				//int programNum=PIn.PInt(table.Rows[0][0].ToString());
				command="UPDATE program SET "
					+"CommandLine='"+POut.PString("\""+@"C:\Program Files\EasyNotesPro\DefaultDentalToolbar.etb"+"\""+" standalone true")+"' "
					+"WHERE ProgName='EasyNotesPro'";//+POut.PInt(programNum);
				General.NonQEx(command);
				//}
				command="UPDATE preference SET ValueString = '3.8.5.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_0();
		}

		private void To3_9_0() {
			if(FromVersion < new Version("3.9.0.0")) {
				ExecuteFile(@"ConversionFiles\Version 3 9 0\convert_3_9_0.txt");//Might throw an exception which we handle.
				//convert two letter languages to 5 char specific culture names-------------------------------------------------
				string command="";
				DataTable table;
				if(CultureInfo.CurrentCulture.Name=="en-US") {
					command="DELETE FROM languageforeign";
					General.NonQEx(command);
				}
				else {
					command="SELECT DISTINCT Culture FROM languageforeign";
					table=General.GetTableEx(command);
					CultureInfo ci;
					for(int i=0;i<table.Rows.Count;i++) {
						try {
							ci=new CultureInfo(table.Rows[i][0].ToString());
						}
						catch {
							MessageBox.Show("Invalid culture: "+table.Rows[i][0].ToString());
							continue;
						}
						FormConvertLang39 FormC=new FormConvertLang39();
						FormC.OldDisplayName=ci.DisplayName;
						FormC.ShowDialog();
						if(FormC.DialogResult!=DialogResult.OK) {
							continue;
						}
						command="UPDATE languageforeign SET Culture='"+FormC.NewName+"' "
							+"WHERE Culture='"+table.Rows[i][0].ToString()+"'";
						General.NonQEx(command);
					}
				}
				//------------------------------------------------------------------------------------------------------------
				//move all patient.PriPlanNum,PriRelationship,SecPlanNum,SecRelationship,
				//PriPending,SecPending,PriPatID,SecPatID to PatPlan objects
				command="SELECT PatNum,PriPlanNum,PriRelationship,PriPending,PriPatID FROM patient WHERE PriPlanNum>0";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO patplan (PatNum,PlanNum,Ordinal,IsPending,Relationship,PatID) VALUES ("
						+table.Rows[i][0].ToString()+","//patnum
						+table.Rows[i][1].ToString()+","//planNum
						+"1,"//Ordinal
						+table.Rows[i][3].ToString()+","//IsPending
						+table.Rows[i][2].ToString()+","//Relationship
						+"'"+POut.PString(PIn.PString(table.Rows[i][4].ToString()))+"'"//PatID
						+")";
					General.NonQEx(command);
				}
				command="SELECT PatNum,SecPlanNum,SecRelationship,SecPending,SecPatID FROM patient WHERE SecPlanNum>0";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO patplan (PatNum,PlanNum,Ordinal,IsPending,Relationship,PatID) VALUES ("
						+table.Rows[i][0].ToString()+","//patnum
						+table.Rows[i][1].ToString()+","//planNum
						+"2,"//Ordinal
						+table.Rows[i][3].ToString()+","//IsPending
						+table.Rows[i][2].ToString()+","//Relationship
						+"'"+POut.PString(PIn.PString(table.Rows[i][4].ToString()))+"'"//PatID
						+")";
					General.NonQEx(command);
				}
				//convert all covpat.PriPatNum and SecPatNum to PatPlanNum-----------------------------------------------------
				//primary
				command="SELECT covpat.CovPatNum,patplan.PatPlanNum FROM covpat,patplan "
					+"WHERE covpat.PriPatNum=patplan.PatNum "
					+"AND patplan.Ordinal=1";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE covpat SET PatPlanNum="+table.Rows[i][1].ToString()
						+" WHERE CovPatNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				//secondary
				command="SELECT covpat.CovPatNum,patplan.PatPlanNum FROM covpat,patplan "
					+"WHERE covpat.PriPatNum=patplan.PatNum "
					+"AND patplan.Ordinal=2";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE covpat SET PatPlanNum="+table.Rows[i][1].ToString()
						+" WHERE CovPatNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				//set patient.HasInsurance for everyone-----------------------------------------------------------------------
				command="SELECT DISTINCT PatNum FROM patplan";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE patient SET HasIns='I'"
						+" WHERE PatNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				//delete unwanted columns-------------------------------------------------------------------------------------
				string[] commands=new string[] {
					"ALTER TABLE covpat DROP PriPatNum"
					,"ALTER TABLE covpat DROP SecPatNum"
					,"ALTER TABLE patient DROP PriPlanNum"
					,"ALTER TABLE patient DROP SecPlanNum"
					,"ALTER TABLE patient DROP PriRelationship"
					,"ALTER TABLE patient DROP SecRelationship"
					,"ALTER TABLE patient DROP RecallInterval"
					,"ALTER TABLE patient DROP RecallStatus"
					,"ALTER TABLE patient DROP PriPending"
					,"ALTER TABLE patient DROP SecPending"
					,"ALTER TABLE patient DROP PriPatID"
					,"ALTER TABLE patient DROP SecPatID"
					,"ALTER TABLE insplan DROP Carrier"
					,"ALTER TABLE insplan DROP Phone"
					,"ALTER TABLE insplan DROP Address"
					,"ALTER TABLE insplan DROP Address2"
					,"ALTER TABLE insplan DROP City"
					,"ALTER TABLE insplan DROP State"
					,"ALTER TABLE insplan DROP Zip"
					,"ALTER TABLE insplan DROP NoSendElect"
					,"ALTER TABLE insplan DROP ElectID"
					,"ALTER TABLE insplan DROP Employer"
				};
				General.NonQEx(commands);
				//final cleanup----------------------------------------------------------------------------------------------
				command="UPDATE preference SET ValueString = '3.9.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_1();
		}

		private void To3_9_1() {
			if(FromVersion < new Version("3.9.1.0")) {
				string command="UPDATE preference SET PrefName = 'BackupToPath' WHERE PrefName = 'BackupPath'";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('BackupFromPath', '"+POut.PString(@"C:\mysql\data\")+"')";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('BackupRestoreFromPath', '"+POut.PString(@"D:\")+"')";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('BackupRestoreToPath', '"+POut.PString(@"C:\mysql\data\")+"')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.1.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_2();
		}

		private void To3_9_2() {
			if(FromVersion < new Version("3.9.2.0")) {
				//DBSWin link-----------------------------------------------------------------------
				string command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'DBSWin', "
					+"'DBSWin from www.duerruk.com', "
					+"'0', "
					+"'', "
					+"'', "
					+"'"+POut.PString(@"No command line or path is needed.")+"')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Text file path', "
					+"'"+POut.PString(@"C:\patdata.txt")+"')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'DBSWin')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.2.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_3();
		}

		private void To3_9_3() {
			if(FromVersion < new Version("3.9.3.0")) {
				string command="UPDATE preference SET ValueString = '-1' WHERE PrefName = 'InsBillingProv' AND ValueString='1'";
				General.NonQEx(command);
				//Add diagnosis fields to HCFA-1500
				int claimFormNum;
				command="SELECT ClaimFormNum FROM claimform WHERE UniqueID='OD4'";
				DataTable table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					claimFormNum=PIn.PInt(table.Rows[0][0].ToString());
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P1Diagnosis',446,749,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P2Diagnosis',446,781,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P3Diagnosis',446,816,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P4Diagnosis',446,849,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P5Diagnosis',446,882,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P6Diagnosis',446,915,75,16)";
					General.NonQEx(command);
				}
				command="SELECT ClaimFormNum FROM claimform WHERE UniqueID='OD5'";
				table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					claimFormNum=PIn.PInt(table.Rows[0][0].ToString());
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P1Diagnosis',446,749,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P2Diagnosis',446,781,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P3Diagnosis',446,816,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P4Diagnosis',446,849,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P5Diagnosis',446,882,75,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P6Diagnosis',446,915,75,16)";
					General.NonQEx(command);
				}
				command="ALTER TABLE procedurelog ADD IsPrincDiag tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.3.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_4();
		}

		private void To3_9_4() {
			if(FromVersion < new Version("3.9.4.0")) {
				string command="INSERT INTO preference VALUES ('BillingIncludeChanged', '1')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.4.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_5();
		}

		private void To3_9_5() {
			if(FromVersion < new Version("3.9.5.0")) {
				string command="INSERT INTO preference VALUES ('BackupRestoreAtoZToPath', '"+POut.PString(@"C:\OpenDentalData\")+"')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.5.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_6();
		}

		private void To3_9_6() {
			if(FromVersion < new Version("3.9.6.0")) {
				string command="ALTER TABLE referral CHANGE PatNum PatNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				command="ALTER TABLE refattach CHANGE PatNum PatNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.6.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_8();
		}

		private void To3_9_8() {
			if(FromVersion < new Version("3.9.8.0")) {
				//DentX link-----------------------------------------------------------------------
				string command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'DentX', "
					+"'ProImage from www.dent-x.com', "
					+"'0', "
					+"'', "
					+"'', "
					+"'"+POut.PString(@"No command line or path is needed.")+"')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'DentX')";
				General.NonQEx(command);
				//Lightyear bridge--------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'Lightyear', "
					+"'SpeedVision from www.lightyeardirect.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\Program Files\Speedvision\speedvision.exe")+"', "
					+"'', "
					+"'"+POut.PString(@"Path is usually C:\Program Files\Speedvision\speedvision.exe.  No command line is needed.")+"')";
				programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'Lightyear')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.8.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_9();
		}

		private void To3_9_9() {
			if(FromVersion < new Version("3.9.9.0")) {
				//TrackNPost clearinghouse
				string command="INSERT INTO clearinghouse(Description,ExportPath,IsDefault,Payors,Eformat,ReceiverID,"
					+"SenderID,Password,ResponsePath,CommBridge,ClientProgram) "
					+@"VALUES('Post-n-Track','C:\\PostnTrack\\Exports\\','0','','1','PostnTrack','','',"
					+@"'C:\\PostnTrack\\Reports\\','8','')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.9.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_17();
		}

		private void To3_9_17() {
			if(FromVersion < new Version("3.9.17.0")) {
				//Rename VixWin to VixWinOld-----------------------------------------------------------------------
				string command="UPDATE program SET ProgName='VixWinOld' WHERE ProgName='VixWin'";
				General.NonQEx(command);
				//Add new VixWin bridge---------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'VixWin', "
					+"'VixWin(new) from www.gendexxray.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\VixWin\VixWin.exe")+"', "
					+"'', "
					+"'')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'VixWin')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.17.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To3_9_18();
		}

		private void To3_9_18() {
			if(FromVersion < new Version("3.9.18.0")) {
				//fixes random keys problems:
				string command="ALTER TABLE referral CHANGE ReferralNum ReferralNum mediumint unsigned NOT NULL auto_increment";
				General.NonQEx(command);
				//these two lines were previously in place and must be accounted for.
				//command="ALTER TABLE patient CHANGE NextAptNum NextAptNum mediumint unsigned NOT NULL";
				//General.NonQEx(command);
				command="UPDATE preference SET ValueString = '3.9.18.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_0();
		}

		private void To4_0_0() {
			if(FromVersion < new Version("4.0.0.0")) {
				ExecuteFile(@"ConversionFiles\Version 4 0 0\convert_4_0_0.txt");//Might throw an exception which we handle.
				//first, get rid of a slight database inconsistency------------------------------------------------------------  
				//In my database, I found 65 duplicate covpat entries for certain plans. Users would not notice.
				//Running this loop adds a few minutes to the process, but is unavoidable.
				//Add some indexes to make this query go faster
				string command="ALTER TABLE covpat ADD INDEX indexPlanNum (PlanNum)";
				General.NonQEx(command);
				command="ALTER TABLE covpat ADD INDEX indexCovCatNum (CovCatNum)";
				General.NonQEx(command);
				command="ALTER TABLE covpat ADD INDEX indexPatPlanNum (PatPlanNum)";
				General.NonQEx(command);
				command="ALTER TABLE covpat ADD INDEX indexCovPatNum (CovPatNum)";
				General.NonQEx(command);
				command=@"SELECT * FROM covpat c1
					WHERE EXISTS(SELECT * FROM covpat c2 
					WHERE c1.PlanNum=c2.PlanNum
					AND c1.CovCatNum=c2.CovCatNum
					AND c1.PatPlanNum=c2.PatPlanNum
					AND c1.CovPatNum<c2.CovPatNum)";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="DELETE FROM covpat WHERE CovPatNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				//Add a CovCat for General------------------------------------------------------------------------------
				command="UPDATE covcat SET CovOrder=CovOrder+1";//Move all other covcats down one in order
				General.NonQEx(command);
				command="INSERT INTO covcat (Description,DefaultPercent,IsPreventive,"
					+"CovOrder,IsHidden) VALUES('General',-1,0,0,0)";
				int covCatNumGeneral=General.NonQEx(command,true);
				command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNumGeneral)+",'D0000','D9999')";
				General.NonQEx(command);
				//Add a note to all InsPlans that do not renew in Jan----------------------------------------------------------
				command="SELECT PlanNum,RenewMonth FROM insplan WHERE RenewMonth != '1'";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="UPDATE insplan SET "
						+"PlanNote = CONCAT('BENEFIT YEAR BEGINS IN MONTH "+table.Rows[i][1].ToString()
						+". SET EFFECTIVE DATE TO MATCH.',PlanNote) "
						+"WHERE PlanNum='"+table.Rows[i][0].ToString()+"'";
					General.NonQEx(command);
				}
				//Convert CovPats to Benefits---------------------------------------------------------------------------------
				command="SELECT DISTINCT covpat.CovCatNum,covpat.PlanNum,covpat.Percent,covpat.PatPlanNum,"//0-3
					+"IFNULL(insplan.DeductWaivPrev,1),covcat.IsPreventive,insplan.Deductible,"//4-6
					+"IFNULL(insplan.RenewMonth,1) "//7
					+"FROM covpat "
					+"LEFT JOIN insplan ON covpat.PlanNum=insplan.PlanNum "
					+"LEFT JOIN covcat ON covpat.CovCatNum=covcat.CovCatNum";
				Debug.WriteLine(command);
				//+"ORDER BY covpat.PatPlanNum DESC";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					//percentages
					command="INSERT INTO benefit (PlanNum,PatPlanNum,CovCatNum,BenefitType,Percent,MonetaryAmt,TimePeriod"
						+") VALUES("
						+"'"+table.Rows[i][1].ToString()+"', "//planNum=1
						+"'"+table.Rows[i][3].ToString()+"', "//patPlanNum=3
						+"'"+table.Rows[i][0].ToString()+"', "//CovCatNum=0
						+"'1', "//benefitType=Percentage
						+"'"+table.Rows[i][2].ToString()+"', "//Percent=2
						+"'0', ";//MonetaryAmt
					if(table.Rows[i][7].ToString()=="1") {//RenewMonth=Jan
						command+="'2')";//TimePeriod=CalendarYear
					}
					else {
						command+="'1')";//TimePeriod=ServiceYear
					}
					General.NonQEx(command);
					//deductibles waived on preventive
					if(table.Rows[i][6].ToString()=="-1"//deductible=-1(unknown)
						|| table.Rows[i][6].ToString()=="0"//deductible=0
						|| table.Rows[i][5].ToString()=="0"//not preventive
						|| table.Rows[i][4].ToString()=="-1"//deductWaivPrev=-1 (not known if waived)
						|| table.Rows[i][4].ToString()=="0")//deductWaivPrev=0 (not waived)
					{
						continue;
					}
					command="INSERT INTO benefit (PlanNum,PatPlanNum,CovCatNum,BenefitType,Percent,MonetaryAmt,TimePeriod"
						+") VALUES("
						+"'"+table.Rows[i][1].ToString()+"', "//planNum=1
						+"'"+table.Rows[i][3].ToString()+"', "//patPlanNum=3
						+"'"+table.Rows[i][0].ToString()+"', "//CovCatNum=0
						+"'2', "//benefitType=Deductible
						+"'0', "//Percent=3
						+"'0', ";//MonetaryAmt=0 since waived
					if(table.Rows[i][7].ToString()=="1") {//RenewMonth=Jan
						command+="'2')";//TimePeriod=CalendarYear
					}
					else {
						command+="'1')";//TimePeriod=ServiceYear
					}
					General.NonQEx(command);
				}
				//Convert remaining InsPlan fields to benefits-------------------------------------------------------------
				command="SELECT PlanNum,AnnualMax,Deductible,FloToAge,MissToothExcl,MajorWait,OrthoMax,RenewMonth "
					+"FROM insplan";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					//AnnualMax
					if(PIn.PDouble(table.Rows[i][1].ToString())>0) {//if there is an annual max
						command="INSERT INTO benefit (PlanNum,PatPlanNum,CovCatNum,BenefitType,Percent,MonetaryAmt,TimePeriod"
							+") VALUES("
							+"'"+table.Rows[i][0].ToString()+"', "//planNum
							+"'0',"//patPlanNum
							+"'"+POut.PInt(covCatNumGeneral)+"',"//CovCatNum
							+"'"+POut.PInt((int)InsBenefitType.Limitations)+"', "
							+"'0',"//percent
							+"'"+table.Rows[i][1].ToString()+"', ";//max
						if(table.Rows[i][7].ToString()=="1") {//RenewMonth=Jan
							command+="'"+POut.PInt((int)BenefitTimePeriod.CalendarYear)+"')";
						}
						else {
							command+="'"+POut.PInt((int)BenefitTimePeriod.ServiceYear)+"')";
						}
						General.NonQEx(command);
					}
					//Deductible
					if(PIn.PDouble(table.Rows[i][2].ToString())>-1) {//if there is a deductible
						command="INSERT INTO benefit (PlanNum,PatPlanNum,CovCatNum,BenefitType,Percent,MonetaryAmt,TimePeriod"
							+") VALUES("
							+"'"+table.Rows[i][0].ToString()+"', "//planNum
							+"'0',"//patPlanNum
							+"'"+POut.PInt(covCatNumGeneral)+"',"//CovCatNum
							+"'"+POut.PInt((int)InsBenefitType.Deductible)+"', "
							+"'0',"//percent
							+"'"+table.Rows[i][2].ToString()+"', ";//deductible amt
						if(table.Rows[i][7].ToString()=="1") {//RenewMonth=Jan
							command+="'"+POut.PInt((int)BenefitTimePeriod.CalendarYear)+"')";
						}
						else {
							command+="'"+POut.PInt((int)BenefitTimePeriod.ServiceYear)+"')";
						}
						General.NonQEx(command);
					}
					//FloToAge
					if(CultureInfo.CurrentCulture.Name=="en-US" && table.Rows[i][3].ToString() != "-1") {
						command="INSERT INTO benefit (PlanNum,PatPlanNum,CovCatNum,ADACode,BenefitType,Percent,MonetaryAmt,"
							+"TimePeriod,QuantityQualifier,Quantity) VALUES("
							+"'"+table.Rows[i][0].ToString()+"', "//planNum
							+"'0',"//patPlanNum
							+"'"+POut.PInt(covCatNumGeneral)+"',"//CovCatNum=general. But ignored because of ADACode
							+"'D1204',"//ADACode for Adult Flo
							+"'"+POut.PInt((int)InsBenefitType.Limitations)+"', "
							+"'0',"//percent
							+"'0', ";//amt
						if(table.Rows[i][7].ToString()=="1") {//RenewMonth=Jan
							command+="'"+POut.PInt((int)BenefitTimePeriod.CalendarYear)+"', ";
						}
						else {
							command+="'"+POut.PInt((int)BenefitTimePeriod.ServiceYear)+"', ";
						}
						command+="'"+POut.PInt((int)BenefitQuantity.AgeLimit)+"', "
							+"'"+table.Rows[i][3].ToString()+"')";//this should work for 0,18, and 99
						General.NonQEx(command);
					}
					//MissToothExcl
					if(table.Rows[i][4].ToString()!="0") {//if it's not unknown
						command="UPDATE insplan SET "
							+"PlanNote = CONCAT('Missing tooth exclusion: "+((YN)PIn.PInt(table.Rows[i][4].ToString())).ToString()
							+". ',PlanNote) "
							+"WHERE PlanNum='"+table.Rows[i][0].ToString()+"'";
						General.NonQEx(command);
					}
					//MajorWait
					if(table.Rows[i][5].ToString()!="0") {//if it's not unknown
						command="UPDATE insplan SET "
							+"PlanNote = CONCAT('Wait on major: "+((YN)PIn.PInt(table.Rows[i][5].ToString())).ToString()
							+". ',PlanNote) "
							+"WHERE PlanNum='"+table.Rows[i][0].ToString()+"'";
						General.NonQEx(command);
					}
					//OrthoMax
					if(PIn.PInt(table.Rows[i][6].ToString())>0) {//not -1 or 0
						command="UPDATE insplan SET "
							+"PlanNote = CONCAT('Ortho Max: "+table.Rows[i][6].ToString()
							+". ',PlanNote) "
							+"WHERE PlanNum='"+table.Rows[i][0].ToString()+"'";
						General.NonQEx(command);
					}
				}
				string[] commands=new string[]
				{
					 "ALTER TABLE insplan DROP AnnualMax"
					,"ALTER TABLE insplan DROP RenewMonth"
					,"ALTER TABLE insplan DROP Deductible"
					,"ALTER TABLE insplan DROP DeductWaivPrev"
					,"ALTER TABLE insplan DROP OrthoMax"
					,"ALTER TABLE insplan DROP FloToAge"
					,"ALTER TABLE insplan DROP MissToothExcl"
					,"ALTER TABLE insplan DROP MajorWait"
					,"ALTER TABLE insplan DROP IsWrittenOff"
					,"DROP TABLE covpat"
					,"ALTER TABLE covcat DROP IsPreventive"
					,"ALTER TABLE covcat ADD EbenefitCat tinyint unsigned NOT NULL"
					,"UPDATE insplan SET SubscNote=PlanNote"
					,"UPDATE insplan SET PlanNote=''"
				};
				General.NonQEx(commands);
				//Add enhanced Trophy bridge---------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'TrophyEnhanced', "
					+"'Trophy(enhanced) from www.trophy-imaging.com', "
					+"'0', "
					+"'"+POut.PString(@"TW.exe")+"', "
					+"'', "
					+"'"+POut.PString(@"The storage path is where all images are stored.  For instance \\SERVER\TrophyImages (no trailing \).  Each patient must also have a folder specified in the patient edit window.  For instance S\SmithJohn or whatever the current folder structure is.")+"')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Storage Path', "
					+"'')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'Trophy')";
				General.NonQEx(command);
				//Add DentalEye bridge----------------------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'DentalEye', "
					+"'DentalEye from www.dentaleye.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\DentalEye\DentalEye.exe")+"', "
					+"'', "
					+"'')";
				programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'DentalEye')";
				General.NonQEx(command);
				//Add lab fee fields to Canadian claim form
				int claimFormNum;
				command="SELECT ClaimFormNum FROM claimform WHERE UniqueID='OD6'";
				table=General.GetTableEx(command);
				if(table.Rows.Count>0) {
					claimFormNum=PIn.PInt(table.Rows[0][0].ToString());
					//get rid of the existing dentist fee column.
					command="DELETE FROM claimformitem WHERE ClaimFormNum='"+POut.PInt(claimFormNum)
						+"' AND XPos=342 AND FieldName LIKE '%Fee'";
					General.NonQEx(command);
					//add the lab fee column
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P1Lab',440,394,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P2Lab',440,411,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P3Lab',440,428,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P4Lab',440,445,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P5Lab',440,462,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P6Lab',440,479,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P7Lab',440,496,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P8Lab',440,513,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P9Lab',440,530,66,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P10Lab',440,547,66,16)";
					General.NonQEx(command);
					//add the dentist fee column (fee-lab)
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P1FeeMinusLab',342,394,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P2FeeMinusLab',342,411,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P3FeeMinusLab',342,428,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P4FeeMinusLab',342,445,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P5FeeMinusLab',342,462,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P6FeeMinusLab',342,479,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P7FeeMinusLab',342,496,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P8FeeMinusLab',342,513,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P9FeeMinusLab',342,530,62,16)";
					General.NonQEx(command);
					command="INSERT INTO claimformitem (ClaimFormNum,FieldName,XPos,YPos,Width,Height) VALUES("
						+POut.PInt(claimFormNum)+",'P10FeeMinusLab',342,547,62,16)";
					General.NonQEx(command);
					//make dates wider so the year doesn't get cut off
					command="UPDATE claimformitem SET Width=85,XPos=28 "
						+"WHERE FieldName LIKE 'P%Date' AND XPos=38 "
						+"AND ClaimFormNum="+POut.PInt(claimFormNum);
					General.NonQEx(command);
				}
				//add chart of accounts-----------------------------------------------------------------
				commands=new string[]
				{
					 "INSERT INTO account (Description,AcctType) VALUES('Checking Account',0)"
					,"INSERT INTO account (Description,AcctType) VALUES('Cash Box',0)"
					,"INSERT INTO account (Description,AcctType) VALUES('Employee Advances',0)"
					,"INSERT INTO account (Description,AcctType) VALUES('Equipment',0)"
					,"INSERT INTO account (Description,AcctType) VALUES('Accumulated Depreciation, Equipment',0)"
					,"INSERT INTO account (Description,AcctType) VALUES('Bank Loans Payable',1)"
					,"INSERT INTO account (Description,AcctType) VALUES('Stated Capital',2)"
					,"INSERT INTO account (Description,AcctType) VALUES('Retained Earnings',2)"
					,"INSERT INTO account (Description,AcctType) VALUES('Patient Fee Income',3)"
					,"INSERT INTO account (Description,AcctType) VALUES('Employee Benefits',4)"
					,"INSERT INTO account (Description,AcctType) VALUES('Supplies',4)"
					,"INSERT INTO account (Description,AcctType) VALUES('Services',4)"
					,"INSERT INTO account (Description,AcctType) VALUES('Wages',4)"
				};
				General.NonQEx(commands);
				command="UPDATE preference SET ValueString = '4.0.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_2();
		}

		private void To4_0_2() {
			if(FromVersion < new Version("4.0.2.0")) {
				string command="ALTER TABLE account ADD Inactive tinyint unsigned NOT NULL";
				General.NonQEx(command);
				//add accounting permission to each admin group------------------------------------------------------
				command="SELECT UserGroupNum FROM grouppermission "
					+"WHERE PermType="+POut.PInt((int)Permissions.SecurityAdmin);
				DataTable table=General.GetTableEx(command);
				int groupNum;
				for(int i=0;i<table.Rows.Count;i++) {
					groupNum=PIn.PInt(table.Rows[i][0].ToString());
					command="INSERT INTO grouppermission (UserGroupNum,PermType) "
						+"VALUES("+POut.PInt(groupNum)+","+POut.PInt((int)Permissions.AccountingCreate)+")";
					General.NonQEx(command);
					command="INSERT INTO grouppermission (UserGroupNum,PermType) "
						+"VALUES("+POut.PInt(groupNum)+","+POut.PInt((int)Permissions.AccountingEdit)+")";
					General.NonQEx(command);
				}
				//fix the planned appointment 'done' feature--------------------------------------------------------------
				command="ALTER TABLE patient ADD PlannedIsDone tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE patient SET PlannedIsDone=1 WHERE NextAptNum = -1";
				General.NonQEx(command);
				//these two lines were previously in place in version 3.9.18.  Calling them again doesn't hurt
				command="ALTER TABLE patient CHANGE NextAptNum NextAptNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.0.2.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_3();
		}

		private void To4_0_3() {
			if(FromVersion < new Version("4.0.3.0")) {
				string command="ALTER TABLE account ADD AccountColor int NOT NULL";
				General.NonQEx(command);
				command="UPDATE account SET AccountColor = -1";//white
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('AccountingDepositAccounts','')";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('AccountingIncomeAccount','')";
				General.NonQEx(command);
				//two of these were simply deleted in the very next upgrade.
				//command="INSERT INTO preference VALUES ('AccountingCashDepAccounts','')";
				//General.NonQEx(command);
				command="INSERT INTO preference VALUES ('AccountingCashIncomeAccount','')";
				General.NonQEx(command);
				//command="INSERT INTO preference VALUES ('AccountingCashPaymentType','')";
				//General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.0.3.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_4();
		}

		private void To4_0_4() {
			if(FromVersion < new Version("4.0.4.0")) {
				string command=@"CREATE TABLE accountingautopay(
					AccountingAutoPayNum mediumint unsigned NOT NULL auto_increment,
					PayType smallint unsigned NOT NULL,
					PickList varchar(255) NOT NULL,
					PRIMARY KEY (AccountingAutoPayNum)
					) DEFAULT CHARSET=utf8;";
				General.NonQEx(command);
				command="DELETE FROM preference WHERE PrefName='AccountingCashDepAccounts'";
				General.NonQEx(command);
				command="DELETE FROM preference WHERE PrefName='AccountingCashPaymentType'";
				General.NonQEx(command);
				command="ALTER TABLE transaction ADD PayNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.0.4.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_5();
		}

		private void To4_0_5() {
			if(FromVersion < new Version("4.0.5.0")) {
				string command=@"CREATE TABLE reconcile(
					ReconcileNum mediumint unsigned NOT NULL auto_increment,
					AccountNum mediumint unsigned NOT NULL,
					StartingBal double NOT NULL,
					EndingBal double NOT NULL,
					DateReconcile date NOT NULL default '0001-01-01',
					IsLocked tinyint unsigned NOT NULL,
					PRIMARY KEY (ReconcileNum)
					) DEFAULT CHARSET=utf8;";
				General.NonQEx(command);
				command="ALTER TABLE journalentry ADD ReconcileNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.0.5.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_9();
		}

		private void To4_0_9() {
			if(FromVersion < new Version("4.0.9.0")) {
				string command="INSERT INTO preference VALUES ('SkipComputeAgingInAccount','0')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.0.9.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_10();
		}

		private void To4_0_10() {
			if(FromVersion < new Version("4.0.10.0")) {
				string command="";
				General.NonQEx(command);
				//Add Trojan bridge----------------------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'Trojan', "
					+"'Trojan from www.trojanonline.com', "
					+"'0', "
					+"'', "
					+"'', "
					+"'No path is needed.  No buttons are available.  Uses the standalone Trojan program.')";
				General.NonQEx(command);
				//Add IAP bridge----------------------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'IAP', "
					+"'Insurance Answers Plus from www.iaplus.com', "
					+"'0', "
					+"'', "
					+"'', "
					+"'No path is needed.  No buttons are available.')";
				General.NonQEx(command);
				//fix referrals
				command="ALTER TABLE refattach CHANGE ReferralNum ReferralNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				//disable medical claims
				command="INSERT INTO preference VALUES ('MedicalEclaimsEnabled','0')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.0.10.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_11();
		}

		private void To4_0_11() {
			if(FromVersion < new Version("4.0.11.0")) {
				//delete all percentages for medicaid and capitation plans
				string command=@"SELECT BenefitNum
					FROM insplan,benefit 
					WHERE benefit.PlanNum=insplan.PlanNum
					AND (PlanType='f' OR PlanType='c')
					AND BenefitType=1";
				DataTable table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++){
					command="DELETE FROM benefit WHERE BenefitNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				command="UPDATE preference SET ValueString = '4.0.11.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_0_13();
		}

		private void To4_0_13() {
			if(FromVersion < new Version("4.0.13.0")) {
				//Add sales tax fields. Even though we will not use them yet, some customers might make user of them. 
				string command="INSERT INTO preference VALUES ('SalesTaxPercentage','0')";
				General.NonQEx(command);
				command="ALTER TABLE procedurecode ADD IsTaxed tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.0.13.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_1_0();
		}

		private void To4_1_0() {
			if(FromVersion < new Version("4.1.0.0")) {
				string command;
				if(CultureInfo.CurrentCulture.Name=="en-US"){
					//Convert CovCats to new names and ranges----------------------------------------------------------------------
					//General
					command="UPDATE covcat SET EbenefitCat=1 WHERE Description='General'";
					General.NonQEx(command);
					//Hide all previous covcats.
					command="UPDATE covcat SET IsHidden=1 WHERE Description != 'General'";
					General.NonQEx(command);
					//Create all the new cateories from scratch
					int covCatNum;
					//Diagnostic
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Diagnostic','100','"
						+POut.PInt((int)EbenefitCategory.Diagnostic)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D0000','D0999')";
					General.NonQEx(command);
					//RoutinePreventive
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Preventive','100','"
						+POut.PInt((int)EbenefitCategory.RoutinePreventive)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D1000','D1999')";
					General.NonQEx(command);
					//Restorative
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Restorative','80','"
						+POut.PInt((int)EbenefitCategory.Restorative)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D2000','D2999')";
					General.NonQEx(command);
					//Endo
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Endo','80','"
						+POut.PInt((int)EbenefitCategory.Endodontics)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D3000','D3999')";
					General.NonQEx(command);
					//Perio
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Perio','80','"
						+POut.PInt((int)EbenefitCategory.Periodontics)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D4000','D4999')";
					General.NonQEx(command);
					//OralSurgery
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Oral Surgery','80','"
						+POut.PInt((int)EbenefitCategory.OralSurgery)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D7000','D7999')";
					General.NonQEx(command);
					//Crowns
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Crowns','50','"
						+POut.PInt((int)EbenefitCategory.Crowns)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D2700','D2799')";
					General.NonQEx(command);
					//Prosth
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Prosth','50','"
						+POut.PInt((int)EbenefitCategory.Prosthodontics)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D5000','D5899')";
					General.NonQEx(command);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D6200','D6899')";
					General.NonQEx(command);
					//MaxProsth
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Maxillofacial Prosth','-1','"
						+POut.PInt((int)EbenefitCategory.MaxillofacialProsth)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D5900','D5999')";
					General.NonQEx(command);
					//Accident
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Accident','-1','"
						+POut.PInt((int)EbenefitCategory.Accident)+"')";
					covCatNum=General.NonQEx(command,true);
					//Ortho
					command="INSERT INTO covcat (Description,DefaultPercent,EbenefitCat) VALUES('Ortho','-1','"
						+POut.PInt((int)EbenefitCategory.Orthodontics)+"')";
					covCatNum=General.NonQEx(command,true);
					command="INSERT INTO covspan (CovCatNum,FromCode,ToCode) VALUES("+POut.PInt(covCatNum)+",'D8000','D8999')";
					General.NonQEx(command);
					//Then, order everything
					command="SELECT * FROM covcat ORDER BY "
						+"EbenefitCat != "+POut.PInt((int)EbenefitCategory.General)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Diagnostic)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.RoutinePreventive)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Restorative)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Endodontics)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Periodontics)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.OralSurgery)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Crowns)//subcategory of Restorative
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Prosthodontics)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.MaxillofacialProsth)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Accident)
						+",EbenefitCat != "+POut.PInt((int)EbenefitCategory.Orthodontics);
					DataTable table=General.GetTableEx(command);
					for(int i=0;i<table.Rows.Count;i++){
						command="UPDATE covcat SET CovOrder="+POut.PInt(i)+" WHERE CovCatNum="+table.Rows[i][0].ToString();
						General.NonQEx(command);
					}
				}
				command="ALTER TABLE fee ADD INDEX indexADACode (ADACode)";
				General.NonQEx(command);
				command="ALTER TABLE fee ADD INDEX indexFeeSched (FeeSched)";
				General.NonQEx(command);
				//ProcButton categories---------------------------------------------------------------------------------
				command="ALTER TABLE procbutton ADD Category smallint unsigned NOT NULL";
				General.NonQEx(command);
				command="INSERT INTO definition (category,itemorder,itemname) VALUES(26,0,'All')";
				int defNum=General.NonQEx(command,true);
				command="UPDATE procbutton SET Category="+POut.PInt(defNum);
				General.NonQEx(command);
				command="ALTER TABLE procbutton ADD ButtonImage text NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.1.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_1_2();
		}

		private void To4_1_2() {
			if(FromVersion < new Version("4.1.2.0")) {
				string command;
				command="DELETE FROM preference WHERE PrefName= 'SkipComputeAgingInAccount'";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('StatementShowReturnAddress','1')";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('ShowIDinTitleBar','0')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.1.2.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_2_0();
		}

		private void To4_2_0() {
			if(FromVersion < new Version("4.2.0.0")) {
				string command;
				//string[] commands;//=new string[] {
				command="ALTER TABLE procedurecode ADD PaintType tinyint NOT NULL";
				General.NonQEx(command);
				command="SELECT * FROM definition WHERE Category=22 ORDER BY ItemOrder";
				DataTable table=General.GetTableEx(command);
				Color cDark;
				Color cLight;
				for(int i=0;i<table.Rows.Count;i++){
					cDark=Color.FromArgb(PIn.PInt(table.Rows[i][5].ToString()));
					cLight=Color.FromArgb((cDark.R+255)/2,(cDark.G+255)/2,(cDark.B+255)/2);
					command="INSERT INTO definition (Category,ItemOrder,ItemName,ItemColor) VALUES(22,"
						+POut.PInt(i+5)+",'"+POut.PString(PIn.PString(table.Rows[i][3].ToString())+" (light)")
						+"','" +POut.PInt(cLight.ToArgb())+"')";
					General.NonQEx(command);
				}
				//Conversions to painting type are listed in order previously displayed.
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.Extraction)+" WHERE GTypeNum=1";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.Implant)+" WHERE GTypeNum=10";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.RCT)+" WHERE GTypeNum=4";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.PostBU)+" WHERE GTypeNum=5";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.FillingDark)+" WHERE GTypeNum=2";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.FillingLight)+" WHERE GTypeNum=3";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.FillingLight)+" WHERE GTypeNum=19";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.FillingLight)+" WHERE GTypeNum=11";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.CrownDark)+" WHERE GTypeNum=6";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.CrownLight)+" WHERE GTypeNum=7";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.CrownLight)+" WHERE GTypeNum=20";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.CrownLight)+" WHERE GTypeNum=9";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.BridgeDark)+" WHERE GTypeNum=12";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.BridgeLight)+" WHERE GTypeNum=13";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.BridgeLight)+" WHERE GTypeNum=21";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.BridgeLight)+" WHERE GTypeNum=14";
				General.NonQEx(command);
				//veneer not painted
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.DentureDark)+" WHERE GTypeNum=24";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.DentureLight)+" WHERE GTypeNum=25";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.DentureLight)+" WHERE GTypeNum=26";
				General.NonQEx(command);
				command="UPDATE procedurecode SET PaintType="+POut.PInt((int)ToothPaintingType.DentureLight)+" WHERE GTypeNum=27";
				General.NonQEx(command);
				command=@"CREATE TABLE toothinitial(
					ToothInitialNum mediumint unsigned NOT NULL auto_increment,
					PatNum mediumint unsigned NOT NULL,
					ToothNum varchar(2) NOT NULL,
					InitialType tinyint unsigned NOT NULL,
          Movement float NOT NULL,
					PRIMARY KEY (ToothInitialNum)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				//convert all previous extractions to missing teeth.
				command="SELECT PatNum,ToothNum FROM procedurelog,procedurecode "
					+"WHERE procedurelog.ADACode=procedurecode.ADACode "
					+"AND procedurecode.RemoveTooth=1 "
					+"AND (procedurelog.ProcStatus=2 OR procedurelog.ProcStatus=3 OR procedurelog.ProcStatus=4)";
				table=General.GetTableEx(command);
				for(int i=0;i<table.Rows.Count;i++) {
					command="INSERT INTO toothinitial(PatNum,ToothNum,InitialType) VALUES("
						+table.Rows[i][0].ToString()+",'"
						+POut.PString(PIn.PString(table.Rows[i][1].ToString()))
						+"',0)";
					General.NonQEx(command);
				}
				command="UPDATE preference SET ValueString = '4.2.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_2_1();
		}

		private void To4_2_1() {
			if(FromVersion < new Version("4.2.1.0")) {
				string command="SELECT PatNum,PrimaryTeeth FROM patient WHERE PrimaryTeeth != ''";
				DataTable table=General.GetTableEx(command);
				string[] priTeeth;
				for(int i=0;i<table.Rows.Count;i++){
					priTeeth=(PIn.PString(table.Rows[i][1].ToString())).Split(new char[] {','});
					for(int t=0;t<priTeeth.Length;t++){
						if(priTeeth[t]==""){
							continue;
						}
						command="INSERT INTO toothinitial (PatNum,ToothNum,InitialType) VALUES("
							+table.Rows[i][0].ToString()
							+",'"+POut.PString(priTeeth[t])+"',2)";
						General.NonQEx(command);
					}
				}
				command="UPDATE preference SET ValueString = '4.2.1.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_2_8();
		}

		private void To4_2_8() {
			if(FromVersion < new Version("4.2.8.0")) {
				string command="UPDATE procedurecode SET PaintType=13, TreatArea=2 WHERE ADACode='D1351'";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.2.8.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_2_9();
		}

		private void To4_2_9() {
			if(FromVersion < new Version("4.2.9.0")) {
				string command;
				//Add Florida probe bridge----------------------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'FloridaProbe', "
					+"'Florida Probe from www.floridaprobe.com', "
					+"'0', "
					+"'"+POut.PString(@"fp32")+"', "
					+"'', "
					+"'No command line is needed.')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'Florida Probe')";
				General.NonQEx(command);
				//Add Dr Ceph bridge----------------------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'DrCeph', "
					+"'Dr. Ceph from www.fyitek.com', "
					+"'0', "
					+"'', "
					+"'', "
					+"'No path or command line is needed.')";
				programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'Dr Ceph')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.2.9.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_2_10();
		}

		private void To4_2_10() {
			if(FromVersion < new Version("4.2.10.0")) {
				string command="ALTER TABLE procedurecode ADD GraphicColor int NOT NULL";
				General.NonQEx(command);
				command="INSERT INTO definition (Category,ItemOrder,ItemName,ItemColor) VALUES(12,6,"
						+"'CommLog',-65536)";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.2.10.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_3_0();
		}

		private void To4_3_0(){
			if(FromVersion < new Version("4.3.0.0")) {
				ExecuteFile(@"ConversionFiles\Version 4 3 0\convert_4_3_0.txt");//Might throw an exception which we handle.
				string command;
				//Add NewPatientForm bridge-----------------------------------------------------------------------------------
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'NewPatientForm.com', "
					+"'NewPatientForm.com - Online Registration', "
					+"'0', "
					+"'"+POut.PString(@"https://secure.newpatientform.com/ODXNewForms.aspx?un=[username]&pw=[password]")+"', "
					+"'', "
					+"'This function automatically downloads and imports new patient forms that have been completed online.  The button only works from the Images module.')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ImagesModule).ToString()+"', "
					+"'NewPatientForm')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.3.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_3_3();
		}

		private void To4_3_3() {
			if(FromVersion < new Version("4.3.3.0")) {
				string command="INSERT INTO preference VALUES ('ReportFolderName','Reports')";
				General.NonQEx(command);
				if(!Directory.Exists(PrefB.GetString("DocPath")+"Reports")){
					if(Directory.Exists(PrefB.GetString("DocPath"))){
						Directory.CreateDirectory(PrefB.GetString("DocPath")+"Reports");
					}
				}
				command="UPDATE preference SET ValueString = '4.3.3.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_3_4();
		}

		private void To4_3_4() {
			if(FromVersion < new Version("4.3.4.0")) {
				//get rid of any leading ? in quickpastenote
				string command="SELECT QuickPasteNoteNum,Abbreviation FROM quickpastenote";
				DataTable table=General.GetTableEx(command);
				string note;
				for(int i=0;i<table.Rows.Count;i++) {
					note=PIn.PString(table.Rows[i][1].ToString());
					if(note.Contains("?")){
						note=note.Replace("?","");
						command="UPDATE quickpastenote SET Abbreviation='"+POut.PString(note)+"' "
							+"WHERE QuickPasteNoteNum="+table.Rows[i][0].ToString();
						General.NonQEx(command);
					}
				}
				command="UPDATE preference SET ValueString = '4.3.4.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_4_0();
		}

		private void To4_4_0() {
			if(FromVersion < new Version("4.4.0.0")) {
				ExecuteFile(@"ConversionFiles\Version 4 4 0\convert_4_4_0.txt");//Might throw an exception which we handle.
				string command;
				//add PerioPal bridge
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'PerioPal', "
					+"'PerioPal from www.periopal.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\Program Files\PerioPal\PerioPal.exe")+"', "
					+"'', "
					+"'')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'PerioPal')";
				General.NonQEx(command);
				//add MediaDent bridge
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'MediaDent', "
					+"'MediaDent from www.mediadentusa.com', "
					+"'0', "
					+"'mediadent.exe', "
					+"'', "
					+"'"+POut.PString(@"Example of image folder: C:\Mediadent\patients\")+"')";
				programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Image Folder', "
					+"'"+POut.PString(@"C:\Mediadent\patients\")+"')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'MediaDent')";
				General.NonQEx(command);

				command="UPDATE preference SET ValueString = '4.4.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_4_9();
		}

		private void To4_4_9() {
			if(FromVersion < new Version("4.4.9.0")) {
				string command="INSERT INTO preference VALUES ('EasyHideHospitals','1')";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD Ward varchar(255) NOT NULL";
				General.NonQEx(command);
				command="ALTER TABLE schedule CHANGE ScheduleNum ScheduleNum mediumint unsigned NOT NULL auto_increment";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.4.9.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_4_10();
		}

		private void To4_4_10() {
			if(FromVersion < new Version("4.4.10.0")) {
				string command;
				//EMS clearinghouse------------------------------------
				command="INSERT INTO clearinghouse(Description,ExportPath,IsDefault,Payors,Eformat,ReceiverID,"
					+"SenderID,Password,ResponsePath,CommBridge,ClientProgram) "
					+@"VALUES('EMS','C:\\EMS\\Exports\\','0','','1','EMS','','',"
					+@"'','0','')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.4.10.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_5_0();
		}

		private void To4_5_0() {
			if(FromVersion < new Version("4.5.0.0")) {
				string command;
				string[] commands=new string[]
				{
					"ALTER TABLE procedurelog DROP OverridePri",
					"ALTER TABLE procedurelog DROP OverrideSec",
					"ALTER TABLE procedurelog DROP NoBillIns",
					"ALTER TABLE procedurelog DROP IsCovIns",
					"ALTER TABLE procedurelog DROP CapCoPay",
					"ALTER TABLE procedurelog DROP HideGraphical",
					"ALTER TABLE procedurelog CHANGE NextAptNum PlannedAptNum  mediumint unsigned NOT NULL"
				};
				General.NonQEx(commands);
				command=@"CREATE TABLE procnote(
					ProcNoteNum mediumint unsigned NOT NULL auto_increment,
					PatNum mediumint unsigned NOT NULL,
					ProcNum mediumint unsigned NOT NULL,
					EntryDateTime datetime NOT NULL default '0001-01-01 00:00:00',
					UserNum mediumint unsigned NOT NULL,
					Note text NOT NULL,
					PRIMARY KEY (ProcNoteNum),
					INDEX (PatNum),
					INDEX (ProcNum),
					INDEX (UserNum)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				//All previous notes will not have a user assigned.
				command=@"INSERT INTO procnote (PatNum,ProcNum,EntryDateTime,Note)
					SELECT PatNum,ProcNum,DateLocked,ProcNote
					FROM procedurelog
					WHERE ProcNote != ''";
				General.NonQEx(command);
				commands=new string[]
				{
					"ALTER TABLE procedurelog DROP DateLocked",
					"ALTER TABLE procedurelog DROP ProcNote"
				};
				General.NonQEx(commands);
				commands=new string[]
				{
					"ALTER TABLE procnote ADD SigIsTopaz tinyint unsigned NOT NULL",
					"ALTER TABLE procnote ADD Signature text NOT NULL"
				};
				General.NonQEx(commands);
				commands=new string[]
				{
					"INSERT INTO preference VALUES ('EmailUsername','')",
					"INSERT INTO preference VALUES ('EmailPassword','')",
					"INSERT INTO preference VALUES ('EmailPort','587')"
				};
				General.NonQEx(commands);
				command=@"CREATE TABLE emailattach(
					EmailAttachNum mediumint unsigned NOT NULL auto_increment,
					EmailMessageNum mediumint unsigned NOT NULL,
					DisplayedFileName varchar(255) NOT NULL,
					ActualFileName varchar(255) NOT NULL,
					PRIMARY KEY (EmailAttachNum),
					INDEX (EmailMessageNum)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				if(!Directory.Exists(PrefB.GetString("DocPath")+"EmailAttachments")) {
					if(Directory.Exists(PrefB.GetString("DocPath"))) {
						Directory.CreateDirectory(PrefB.GetString("DocPath")+"EmailAttachments");
					}
				}
				if(!Directory.Exists(PrefB.GetString("DocPath")+"Forms")) {
					if(Directory.Exists(PrefB.GetString("DocPath"))) {
						Directory.CreateDirectory(PrefB.GetString("DocPath")+"Forms");
					}
				}
				commands=new string[]
				{
					"ALTER TABLE document ADD Note text NOT NULL",
					"ALTER TABLE document ADD SigIsTopaz tinyint unsigned NOT NULL",
					"ALTER TABLE document ADD Signature text NOT NULL",
					"INSERT INTO preference VALUES ('BankRouting','')",
					"INSERT INTO preference VALUES ('BankAddress','')",
					"ALTER TABLE procedurelog CHANGE ProcDate ProcDate datetime NOT NULL default '0001-01-01 00:00:00'"
				};
				General.NonQEx(commands);
				command="UPDATE preference SET ValueString = '4.5.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_5_1();
		}

		private void To4_5_1() {
			if(FromVersion<new Version("4.5.1.0")) {
				string command="UPDATE procedurelog SET AptNum=0 WHERE ProcStatus=6";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString='' WHERE PrefName='BillingSelectBillingTypes'";
				General.NonQEx(command);
				
				command="UPDATE preference SET ValueString = '4.5.1.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_5_14();
		}

		private void To4_5_14() {
			if(FromVersion<new Version("4.5.14.0")) {
				//add XDR bridge:
				string command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'XDR', "
					+"'XDR from www.XDRradiology.com', "
					+"'0', "
					+"'"+POut.PString(@"C:\Program Files\DxS\bin\XDR.exe")+"', "
					+"'', "
					+"'')";
				int programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'InfoFile path', "
					+"'"+POut.PString(@"C:\Program Files\Dxs\bin\infofile.txt")+"')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'XDR')";
				General.NonQEx(command);
				command="ALTER TABLE insplan ADD FilingCode tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.5.14.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_5_21();
		}

		private void To4_5_21() {
			if(FromVersion<new Version("4.5.21.0")) {
				//All 4 of these fields added on 12/21/06:
				string command="ALTER TABLE patient ADD PreferConfirmMethod tinyint unsigned NOT NULL AFTER Ward";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD SchedBeforeTime time AFTER PreferConfirmMethod";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD SchedAfterTime time AFTER SchedBeforeTime";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD SchedDayOfWeek tinyint unsigned NOT NULL AFTER SchedAfterTime";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.5.21.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_0();
		}

		private void To4_6_0() {
			if(FromVersion<new Version("4.6.0.0")) {
				string command;
				command=@"CREATE TABLE formpat(
					FormPatNum mediumint unsigned NOT NULL auto_increment,
					PatNum mediumint unsigned NOT NULL,
					FormDateTime datetime NOT NULL default '0001-01-01',
					PRIMARY KEY (FormPatNum),
					INDEX (PatNum)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				command="ALTER TABLE emailmessage ADD SentOrReceived tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="UPDATE emailmessage SET SentOrReceived = 1 WHERE YEAR(MsgDateTime) > '1900'";
				General.NonQEx(command);
				command="UPDATE emailmessage SET MsgDateTime = NOW() WHERE YEAR(MsgDateTime) < '1900'";
				General.NonQEx(command);
				command="DELETE FROM commlog WHERE EmailMessageNum > 0";
				General.NonQEx(command);
				command="ALTER TABLE commlog DROP EmailMessageNum";
				General.NonQEx(command);
				command="ALTER TABLE question ADD FormPatNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				command="SELECT DISTINCT PatNum FROM question";
				DataTable table=General.GetTableEx(command);
				int formPatNum;
				for(int i=0;i<table.Rows.Count;i++){
					command="INSERT INTO formpat (PatNum,FormDateTime) VALUES("
						+"'"+table.Rows[i][0].ToString()+"', NOW())";
					formPatNum=General.NonQEx(command,true);
					command="UPDATE question SET FormPatNum="+POut.PInt(formPatNum)+" WHERE PatNum="+table.Rows[i][0].ToString();
					General.NonQEx(command);
				}
				command=@"CREATE TABLE etrans(
					EtransNum mediumint unsigned NOT NULL auto_increment,
					DateTimeTrans datetime NOT NULL default '0001-01-01',
					ClearinghouseNum mediumint unsigned NOT NULL,
					Etype tinyint unsigned NOT NULL,
					ClaimNum mediumint unsigned NOT NULL,
					OfficeSequenceNumber mediumint unsigned NOT NULL,
					CarrierTransCounter mediumint unsigned NOT NULL,
					CarrierTransCounter2 mediumint unsigned NOT NULL,
					CarrierNum mediumint unsigned NOT NULL,
					CarrierNum2 mediumint unsigned NOT NULL,
					PatNum mediumint unsigned NOT NULL,
					PRIMARY KEY (EtransNum),
					INDEX (ClaimNum),
					INDEX (CarrierNum),
					INDEX (CarrierNum2)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				command=@"CREATE TABLE canadianclaim(
					ClaimNum mediumint unsigned NOT NULL,
					MaterialsForwarded char(5) NOT NULL,
					ReferralProviderNum char(10) NOT NULL,
					ReferralReason tinyint unsigned NOT NULL,
					SecondaryCoverage char(1) NOT NULL,
					IsInitialLower char(1) NOT NULL,
					DateInitialLower date NOT NULL default '0001-01-01',
					MandProsthMaterial tinyint unsigned NOT NULL,
					IsInitialUpper char(1) NOT NULL,
					DateInitialUpper date NOT NULL default '0001-01-01',
					MaxProsthMaterial tinyint unsigned NOT NULL,
					EligibilityCode tinyint unsigned NOT NULL,
					SchoolName varchar(25) NOT NULL,
					PayeeCode tinyint unsigned NOT NULL,
					PRIMARY KEY (ClaimNum)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				command="ALTER TABLE carrier ADD IsCDA tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="ALTER TABLE carrier ADD IsPMP tinyint unsigned NOT NULL";
				General.NonQEx(command);
				command="ALTER TABLE provider ADD CanadianOfficeNum varchar(100) NOT NULL";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('LanguagesUsedByPatients','')";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD Language varchar(100) NOT NULL";
				General.NonQEx(command);
				command=@"CREATE TABLE canadianextract(
					CanadianExtractNum mediumint unsigned NOT NULL auto_increment,
					ClaimNum mediumint unsigned NOT NULL,
					ToothNum varchar(10) NOT NULL,
					DateExtraction date NOT NULL default '0001-01-01',
					PRIMARY KEY (CanadianExtractNum),
					INDEX (ClaimNum)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				command="ALTER TABLE insplan ADD DentaideCardSequence tinyint unsigned NOT NULL";
				General.NonQEx(command);
				//added 11/30/06 after r42.
				command="ALTER TABLE etrans ADD MessageText text NOT NULL";
				General.NonQEx(command);
				//added 12/2/06 after r46:
				command="ALTER TABLE procedurelog DROP LabFee";
				General.NonQEx(command);
				command="ALTER TABLE procedurelog ADD ProcNumLab mediumint unsigned NOT NULL";
				General.NonQEx(command);
				//added 12/8/06 after r57:
				command="ALTER TABLE procedurecode ADD IsCanadianLab tinyint unsigned NOT NULL";
				General.NonQEx(command);
				//added 12/21/06 after r71:
				//Also, see the previous method at line 3450, where 4 more fields were added.
				command="ALTER TABLE claimproc ADD LineNumber tinyint unsigned NOT NULL";
				General.NonQEx(command);
				//added 12/22/06 after r72:
				command=@"CREATE TABLE canadiannetwork(
					CanadianNetworkNum mediumint unsigned NOT NULL auto_increment,
					Abbrev varchar(20) NOT NULL,
					Descript varchar(255) NOT NULL,
					PRIMARY KEY (CanadianNetworkNum)
					) DEFAULT CHARSET=utf8";
				General.NonQEx(command);
				string[] commands=new string[]
				{
					"INSERT INTO canadiannetwork VALUES (1,'AHI','BC Emergis (formerly Assure Health Inc)')",
					"INSERT INTO canadiannetwork VALUES (2,'NDC','National Data Corporation')",
					"INSERT INTO canadiannetwork VALUES (3,'CD','Centre Dentaide')",
					"INSERT INTO canadiannetwork VALUES (4,'ABC','Alberta Blue Cross')",
					"INSERT INTO canadiannetwork VALUES (5,'MBC','Manitoba Blue Cross')",
					"INSERT INTO canadiannetwork VALUES (6,'PBC','Pacific Blue Cross')"					
				};
				General.NonQEx(commands);
				command="UPDATE preference SET ValueString = '4.6.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_2();
		}

		private void To4_6_2() {
			if(FromVersion<new Version("4.6.2.0")) {
				string command;
				command="ALTER TABLE carrier ADD CDAnetVersion varchar(100) NOT NULL";
				General.NonQEx(command);
				command="ALTER TABLE carrier ADD CanadianNetworkNum mediumint unsigned NOT NULL";
				General.NonQEx(command);
				//CDAnet clearinghouse------------------------------------------------------------------------------------
				command="INSERT INTO clearinghouse(Description,ExportPath,IsDefault,Payors,Eformat,ReceiverID,"
					+"SenderID,Password,ResponsePath,CommBridge,ClientProgram) "
					+@"VALUES('CDAnet','C:\\CCD\\','0','','3','','','',"
					+@"'','9','C:\\CCD\\CCD32.exe')";
				General.NonQEx(command);


				command="UPDATE preference SET ValueString = '4.6.2.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_6();
		}

		private void To4_6_6() {
			if(FromVersion<new Version("4.6.6.0")) {
				string command="ALTER TABLE user RENAME TO userod";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.6.6.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_9();
		}

		///<summary> The following string is used to upgrade MySQL databases into a compatible format for conversion to Oracle. A similar table will also used for the database maintinence tool in order to ensure that the columns below do not contain null data. The array below is composed of groups of 3 strings, following the format: tablename columnname type.</summary>
		private static string[] removeNotNullFieldCommands=new string[] {
			"account","Description","varchar(255)",
			"account","BankNumber","varchar(255)",
			"accountingautopay","PickList","varchar(255)",
			"adjustment","AdjNote","text",
			"appointment","Pattern","varchar(255)",
			"appointment","Note","text",
			"appointment","ProcDescript","varchar(255)",
			"appointmentrule","RuleDesc","varchar(255)",
			"appointmentrule","ADACodeStart","varchar(15) character set utf8 collate utf8_bin",
			"appointmentrule","ADACodeEnd","varchar(15) character set utf8 collate utf8_bin",
			"apptview","Description","varchar(255)",
			"apptviewitem","ElementDesc","varchar(255)",
			"autocode","Description","varchar(255)",
			"autocodeitem","ADACode","varchar(15) character set utf8 collate utf8_bin",
			"benefit","ADACode","varchar(15) character set utf8 collate utf8_bin",
			"canadianclaim","MaterialsForwarded","char(5)",
			"canadianclaim","ReferralProviderNum","char(10)",
			"canadianclaim","SecondaryCoverage","char(1)",
			"canadianclaim","IsInitialLower","char(1)",
			"canadianclaim","IsInitialUpper","char(1)",
			"canadianclaim","SchoolName","varchar(25)",
			"canadianextract","ToothNum","varchar(10)",
			"canadiannetwork","Abbrev","varchar(20)",
			"canadiannetwork","Descript","varchar(255)",
			"carrier","CarrierName","varchar(255)",
			"carrier","Address","varchar(255)",
			"carrier","Address2","varchar(255)",
			"carrier","City","varchar(255)",
			"carrier","State","varchar(255)",
			"carrier","Zip","varchar(255)",
			"carrier","Phone","varchar(255)",
			"carrier","ElectID","varchar(255)",
			"carrier","CDAnetVersion","varchar(100)",
			"claim","ClaimStatus","char(1)",
			"claim","PreAuthString","varchar(40)",
			"claim","IsProsthesis","char(1)",
			"claim","ReasonUnderPaid","varchar(255)",
			"claim","ClaimNote","varchar(255)",
			"claim","ClaimType","varchar(255)",
			"claim","RefNumString","varchar(40)",
			"claim","AccidentRelated","char(1)",
			"claim","AccidentST","varchar(2)",
			"claimform","Description","varchar(50)",
			"claimform","FontName","varchar(255)",
			"claimform","UniqueID","varchar(255)",
			"claimformitem","ImageFileName","varchar(255)",
			"claimformitem","FieldName","varchar(255)",
			"claimformitem","FormatString","varchar(255)",
			"claimpayment","CheckNum","varchar(25)",
			"claimpayment","BankBranch","varchar(25)",
			"claimpayment","Note","varchar(255)",
			"claimpayment","CarrierName","varchar(255)",
			"claimproc","Remarks","varchar(255)",
			"claimproc","CodeSent","varchar(15)",
			"clearinghouse","Description","varchar(255)",
			"clearinghouse","ExportPath","text",
			"clearinghouse","Payors","text",
			"clearinghouse","ReceiverID","varchar(255)",
			"clearinghouse","SenderID","varchar(255)",
			"clearinghouse","Password","varchar(255)",
			"clearinghouse","ResponsePath","varchar(255)",
			"clearinghouse","ClientProgram","varchar(255)",
			"clearinghouse","LoginID","varchar(255)",
			"clinic","Description","varchar(255)",
			"clinic","Address","varchar(255)",
			"clinic","Address2","varchar(255)",
			"clinic","City","varchar(255)",
			"clinic","State","varchar(255)",
			"clinic","Zip","varchar(255)",
			"clinic","Phone","varchar(255)",
			"clinic","BankNumber","varchar(255)",
			"clockevent","Note","text",
			"commlog","Note","text",
			"computer","CompName","varchar(100)",
			"computer","PrinterName","varchar(255)",
			"contact","LName","varchar(255)",
			"contact","FName","varchar(255)",
			"contact","WkPhone","varchar(255)",
			"contact","Fax","varchar(255)",
			"contact","Notes","text",
			"county","CountyName","varchar(255)",
			"county","CountyCode","varchar(255)",
			"covcat","Description","varchar(50)",
			"covspan","FromCode","varchar(15) character set utf8 collate utf8_bin",
			"covspan","ToCode","varchar(15) character set utf8 collate utf8_bin",
			"definition","ItemName","varchar(255)",
			"definition","ItemValue","varchar(255)",
			"deposit","BankAccountInfo","text",
			"disease","PatNote","text",
			"diseasedef","DiseaseName","varchar(255)",
			"document","Description","varchar(255)",
			"document","FileName","varchar(255)",
			"document","ToothNumbers","varchar(255)",
			"document","Note","text",
			"document","Signature","text",
			"dunning","DunMessage","text",
			"electid","PayorID","varchar(255)",
			"electid","CarrierName","varchar(255)",
			"electid","ProviderTypes","varchar(255)",
			"electid","Comments","text",
			"emailattach","DisplayedFileName","varchar(255)",
			"emailattach","ActualFileName","varchar(255)",
			"emailmessage","ToAddress","text",
			"emailmessage","FromAddress","text",
			"emailmessage","Subject","text",
			"emailmessage","BodyText","text",
			"emailtemplate","Subject","text",
			"emailtemplate","BodyText","text",
			"employee","LName","varchar(255)",
			"employee","FName","varchar(255)",
			"employee","MiddleI","varchar(255)",
			"employee","ClockStatus","varchar(255)",
			"employer","EmpName","varchar(255)",
			"employer","Address","varchar(255)",
			"employer","Address2","varchar(255)",
			"employer","City","varchar(255)",
			"employer","State","varchar(255)",
			"employer","Zip","varchar(255)",
			"employer","Phone","varchar(255)",
			"etrans","MessageText","text",
			"fee","ADACode","varchar(15) character set utf8 collate utf8_bin",
			"graphicelement","ToothNum","varchar(2)",
			"graphicelement","Description","varchar(100)",
			"graphicelement","Surface","varchar(5)",
			"graphicshape","ShapeType","char(1)",
			"graphicshape","Description","varchar(100)",
			"graphictype","Description","varchar(100)",
			"graphictype","BrushType","varchar(100)",
			"graphictype","SpecialType","varchar(100)",
			"insplan","GroupName","varchar(50)",
			"insplan","GroupNum","varchar(20)",
			"insplan","PlanNote","text",
			"insplan","PlanType","char(1)",
			"insplan","SubscriberID","varchar(40)",
			"insplan","TrojanID","varchar(100)",
			"insplan","DivisionNo","varchar(255)",
			"insplan","BenefitNotes","text",
			"insplan","SubscNote","text",
			"instructor","LName","varchar(255)",
			"instructor","FName","varchar(255)",
			"instructor","Suffix","varchar(100)",
			"journalentry","Memo","text",
			"journalentry","Splits","text",
			"journalentry","CheckNumber","varchar(255)",
			"language","EnglishComments","text",
			"language","ClassType","text",
			"language","English","text",
			"languageforeign","ClassType","text",
			"languageforeign","English","text",
			"languageforeign","Culture","varchar(255)",
			"languageforeign","Translation","text",
			"languageforeign","Comments","text",
			"letter","Description","varchar(255)",
			"letter","BodyText","text",
			"lettermerge","Description","varchar(255)",
			"lettermerge","TemplateName","varchar(255)",
			"lettermerge","DataFileName","varchar(255)",
			"lettermergefield","FieldName","varchar(255)",
			"medication","MedName","varchar(255)",
			"medication","Notes","text",
			"medicationpat","PatNote","text",
			"operatory","OpName","varchar(255)",
			"operatory","Abbrev","varchar(255)",
			"patfield","FieldName","varchar(255)",
			"patfield","FieldValue","text",
			"patfielddef","FieldName","varchar(255)",
			"patient","LName","varchar(100)",
			"patient","FName","varchar(100)",
			"patient","MiddleI","varchar(100)",
			"patient","Preferred","varchar(100)",
			"patient","SSN","varchar(100)",
			"patient","Address","varchar(100)",
			"patient","Address2","varchar(100)",
			"patient","City","varchar(100)",
			"patient","State","varchar(100)",
			"patient","Zip","varchar(100)",
			"patient","HmPhone","varchar(30)",
			"patient","WkPhone","varchar(30)",
			"patient","WirelessPhone","varchar(30)",
			"patient","CreditType","char(1)",
			"patient","Email","varchar(100)",
			"patient","Salutation","varchar(100)",
			"patient","ImageFolder","varchar(100)",
			"patient","AddrNote","text",
			"patient","FamFinUrgNote","text",
			"patient","MedUrgNote","varchar(255)",
			"patient","ApptModNote","varchar(255)",
			"patient","StudentStatus","char(1)",
			"patient","SchoolName","varchar(30)",
			"patient","ChartNumber","varchar(20)",
			"patient","MedicaidID","varchar(20)",
			"patient","PrimaryTeeth","varchar(255)",
			"patient","EmploymentNote","varchar(255)",
			"patient","County","varchar(255)",
			"patient","GradeSchool","varchar(255)",
			"patient","HasIns","varchar(255)",
			"patient","TrophyFolder","varchar(255)",
			"patient","Ward","varchar(255)",
			"patient","Language","varchar(100)",
			"patientnote","FamFinancial","text",
			"patientnote","ApptPhone","text",
			"patientnote","Medical","text",
			"patientnote","Service","text",
			"patientnote","MedicalComp","text",
			"patientnote","Treatment","text",
			"patplan","PatID","varchar(100)",
			"payment","CheckNum","varchar(25)",
			"payment","BankBranch","varchar(25)",
			"payment","PayNote","varchar(255)",
			"payplan","Note","text",
			"payplancharge","Note","text",
			"preference","PrefName","varchar(255)",
			"preference","ValueString","text",
			"printer","PrinterName","varchar(255)",
			"procbutton","Description","varchar(255)",
			"procbutton","ButtonImage","text",
			"procbuttonitem","ADACode","varchar(15) character set utf8 collate utf8_bin",
			"procedurecode","ADACode","varchar(15) character set utf8 collate utf8_bin",
			"procedurecode","Descript","varchar(255)",
			"procedurecode","AbbrDesc","varchar(50)",
			"procedurecode","ProcTime","varchar(24)",
			"procedurecode","DefaultNote","text",
			"procedurecode","AlternateCode1","varchar(15)",
			"procedurecode","MedicalCode","varchar(15) character set utf8 collate utf8_bin",
			"procedurecode","LaymanTerm","varchar(255)",
			"procedurelog","ADACode","varchar(15) character set utf8 collate utf8_bin",
			"procedurelog","Surf","varchar(10)",
			"procedurelog","ToothNum","varchar(2)",
			"procedurelog","ToothRange","varchar(100)",
			"procedurelog","Prosthesis","char(1)",
			"procedurelog","ClaimNote","varchar(80)",
			"procedurelog","MedicalCode","varchar(15) character set utf8 collate utf8_bin",
			"procedurelog","DiagnosticCode","varchar(255)",
			"procnote","Note","text",
			"procnote","Signature","text",
			"proctp","ToothNumTP","varchar(255)",
			"proctp","Surf","varchar(255)",
			"proctp","ADACode","varchar(255)",
			"proctp","Descript","varchar(255)",
			"program","ProgName","varchar(100)",
			"program","ProgDesc","varchar(100)",
			"program","Path","varchar(255)",
			"program","CommandLine","varchar(255)",
			"program","Note","text",
			"programproperty","PropertyDesc","varchar(255)",
			"programproperty","PropertyValue","varchar(255)",
			"provider","Abbr","varchar(5)",
			"provider","LName","varchar(100)",
			"provider","FName","varchar(100)",
			"provider","MI","varchar(100)",
			"provider","Suffix","varchar(100)",
			"provider","SSN","varchar(12)",
			"provider","StateLicense","varchar(15)",
			"provider","DEANum","varchar(15)",
			"provider","BlueCrossID","varchar(25)",
			"provider","MedicaidID","varchar(20)",
			"provider","NationalProvID","varchar(255)",
			"provider","CanadianOfficeNum","varchar(100)",
			"providerident","PayorID","varchar(255)",
			"providerident","IDNumber","varchar(255)",
			"question","Description","text",
			"question","Answer","text",
			"questiondef","Description","text",
			"quickpastecat","Description","varchar(255)",
			"quickpastecat","DefaultForTypes","text",
			"quickpastenote","Note","text",
			"quickpastenote","Abbreviation","varchar(255)",
			"recall","Note","text",
			"referral","LName","varchar(100)",
			"referral","FName","varchar(100)",
			"referral","MName","varchar(100)",
			"referral","SSN","varchar(9)",
			"referral","ST","varchar(2)",
			"referral","Telephone","varchar(10)",
			"referral","Address","varchar(100)",
			"referral","Address2","varchar(100)",
			"referral","City","varchar(100)",
			"referral","Zip","varchar(10)",
			"referral","Note","text",
			"referral","Phone2","varchar(30)",
			"referral","Title","varchar(255)",
			"referral","EMail","varchar(255)",
			"repeatcharge","ADACode","varchar(15) character set utf8 collate utf8_bin",
			"repeatcharge","Note","text",
			"rxdef","Drug","varchar(255)",
			"rxdef","Sig","varchar(255)",
			"rxdef","Disp","varchar(255)",
			"rxdef","Refills","varchar(30)",
			"rxdef","Notes","varchar(255)",
			"rxpat","Drug","varchar(255)",
			"rxpat","Sig","varchar(255)",
			"rxpat","Disp","varchar(255)",
			"rxpat","Refills","varchar(30)",
			"rxpat","Notes","varchar(255)",
			"schedule","Note","text",
			"school","SchoolName","varchar(255)",
			"school","SchoolCode","varchar(255)",
			"schoolclass","Descript","varchar(255)",
			"schoolcourse","CourseID","varchar(255)",
			"schoolcourse","Descript","varchar(255)",
			"screen","GradeSchool","varchar(255)",
			"screen","County","varchar(255)",
			"screen","ProvName","varchar(255)",
			"screengroup","Description","varchar(255)",
			"securitylog","LogText","text",
			"sigbutdef","ButtonText","varchar(255)",
			"sigbutdef","ComputerName","varchar(255)",
			"sigelementdef","SigText","varchar(255)",
			"sigelementdef","Sound","text",
			"signal","FromUser","varchar(255)",
			"signal","SigText","text",
			"signal","ToUser","varchar(255)",
			"task","Descript","text",
			"tasklist","Descript","varchar(255)",
			"terminalactive","ComputerName","varchar(255)",
			"timeadjust","Note","text",
			"toolbutitem","ButtonText","varchar(255)",
			"toothinitial","ToothNum","varchar(2)",
			"treatplan","Heading","varchar(255)",
			"treatplan","Note","text",
			"usergroup","Description","varchar(255)",
			"userod","UserName","varchar(255)",
			"userod","Password","varchar(255)",
			"userquery","Description","varchar(255)",
			"userquery","FileName","varchar(255)",
			"userquery","QueryText","text",
			"zipcode","ZipCodeDigits","varchar(20)",
			"zipcode","City","varchar(100)",
			"zipcode","State","varchar(20)",
		};

		private void To4_6_9() {
			if(FromVersion<new Version("4.6.9.0")) {
				string command="ALTER TABLE commlog CHANGE Mode Mode_ tinyint(3) unsigned NOT NULL default '0'";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD PreferContactMethod tinyint unsigned NOT NULL AFTER PreferConfirmMethod";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD PreferRecallMethod tinyint unsigned NOT NULL AFTER PreferContactMethod";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.6.9.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_11();
		}

		private void To4_6_11() {
			if(FromVersion<new Version("4.6.11.0")) {
				string command="INSERT INTO preference VALUES ('OracleInsertId','')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.6.11.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_12();
		}

		///<summary>The following changes are to remove the NOT NULL from string fields for all tables in the database in order to become compatible with Oracle, since in Oracle null is the same as the empty string.</summary>
		private void To4_6_12() {
			if(FromVersion<new Version("4.6.12.0")) {
				string command="";
				for(int i=0;i<removeNotNullFieldCommands.Length;i+=3) {
					command="ALTER TABLE "+removeNotNullFieldCommands[i]+" MODIFY "+//table name
							removeNotNullFieldCommands[i+1]+" "+removeNotNullFieldCommands[i+2];	//column name then type
					if(removeNotNullFieldCommands[i+2].ToUpper()!="TEXT") {//For all fields which are not of text type, define default.
						command+=" default ''";
					}
					General.NonQEx(command);
				}
				//added after r49
				command="INSERT INTO preference VALUES ('DefaultClaimForm','1')";
				General.NonQEx(command);
				command="ALTER TABLE patient ADD AdmitDate date NOT NULL default '0001-01-01'";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.6.12.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_13();
		}

		///<summary></summary>
		private void To4_6_13() {
			if(FromVersion<new Version("4.6.13.0")) {
				string command="";
				command="INSERT INTO preference VALUES ('RegistrationNumberClaim','')";
				General.NonQEx(command);
				command="UPDATE preference SET ValueString = '4.6.13.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_19();
		}

		///<summary></summary>
		private void To4_6_19() {
			if(FromVersion<new Version("4.6.19.0")) {
				//Owandy X-ray Bridge created by SPK 10/06, added 2/22/07-----------------------------------------------------------
				string command = "INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+ ") VALUES("
					+ "'Owandy', "
					+ "'QuickVision from owandy.com', "
					+ "'0', "
					+ "'" + POut.PString(@"\Juliew\mj32.exe") + "', "
					+ "' C /ALINK', "
					+ "'" + POut.PString(@"Typical file path with parameters is C:\Juliew\mj32.exe C /ALINK.  Use C /LINK for QV version < 3.15. Letter C refers to drive.") + "')";
				int programNum =General.NonQEx(command,true);//we now have a ProgramNum to work with
				command = "INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+ "VALUES ("
					+ "'" + POut.PInt(programNum) + "', "
					+ "'" + POut.PInt((int)ToolBarsAvail.ChartModule) + "', "
					+ "'Owandy')";
				General.NonQEx(command);
				//Vipersoft bridge:
				command="INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+") VALUES("
					+"'Vipersoft', "
					+"'Vipersoft aka Clarity', "
					+"'0', "
					+"'"+POut.PString(@"C:\Program Files\Vipersoft\Vipersoft.exe")+"', "
					+"'', "
					+"'')";
				programNum=General.NonQEx(command,true);//we now have a ProgramNum to work with
				command="INSERT INTO programproperty (ProgramNum,PropertyDesc,PropertyValue"
					+") VALUES("
					+"'"+programNum.ToString()+"', "
					+"'Enter 0 to use PatientNum, or 1 to use ChartNum', "
					+"'0')";
				General.NonQEx(command);
				command="INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+"VALUES ("
					+"'"+programNum.ToString()+"', "
					+"'"+((int)ToolBarsAvail.ChartModule).ToString()+"', "
					+"'Vipersoft')";
				General.NonQEx(command);
				if(FormChooseDatabase.DBtype==DatabaseType.MySql){
					command="ALTER TABLE userod ADD ClinicNum mediumint NOT NULL";
					General.NonQEx(command);
				}
				else{
					command="ALTER TABLE userod ADD ClinicNum int";
					General.NonQEx(command);
				}
				command="UPDATE preference SET ValueString = '4.6.19.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_6_22();
		}

		///<summary></summary>
		private void To4_6_22() {
			if(FromVersion<new Version("4.6.22.0")) {
				string command="";
				if(FormChooseDatabase.DBtype==DatabaseType.MySql){
					command="ALTER TABLE sigelementdef CHANGE Sound Sound mediumtext";
					General.NonQEx(command);
				}
				command="UPDATE preference SET ValueString = '4.6.22.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			To4_7_0();
		}

		///<summary></summary>
		private void To4_7_0() {
			if(FromVersion<new Version("4.7.0.0")) {
				/*string command="";
				if(FormChooseDatabase.DBtype==DatabaseType.MySql){
					command="ALTER TABLE document ADD CropX mediumint NOT NULL";
					General.NonQEx(command);
					command="ALTER TABLE document ADD CropY mediumint NOT NULL";
					General.NonQEx(command);
					command="ALTER TABLE document ADD CropW mediumint NOT NULL";
					General.NonQEx(command);
					command="ALTER TABLE document ADD CropH mediumint NOT NULL";
					General.NonQEx(command);
					command="ALTER TABLE document ADD WindowingMin mediumint NOT NULL";
					General.NonQEx(command);
					command="ALTER TABLE document ADD WindowingMax mediumint NOT NULL";
					General.NonQEx(command);
				}
				else{
					command="ALTER TABLE document ADD CropX int";
					General.NonQEx(command);
					command="ALTER TABLE document ADD CropY int";
					General.NonQEx(command);
					command="ALTER TABLE document ADD CropW int";
					General.NonQEx(command);
					command="ALTER TABLE document ADD CropH int";
					General.NonQEx(command);
					command="ALTER TABLE document ADD WindowingMin int";
					General.NonQEx(command);
					command="ALTER TABLE document ADD WindowingMax int";
					General.NonQEx(command);
				}
				command="INSERT INTO preference VALUES ('ImageWindowingMin','64')";
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('ImageWindowingMax','192')";
				General.NonQEx(command);
				if(FormChooseDatabase.DBtype==DatabaseType.MySql){
					command=@"CREATE TABLE mountdef(
						MountDefNum int NOT NULL auto_increment,
						Description varchar(255),
						ItemOrder mediumint NOT NULL,
						IsRadiograph tinyint unsigned NOT NULL,
						Width mediumint NOT NULL,
						Height mediumint NOT NULL,
						PRIMARY KEY (MountDefNum)
						) DEFAULT CHARSET=utf8";
				}
				else{
					command=@"CREATE TABLE mountdef(
						MountDefNum int NOT NULL,
						Description varchar(255),
						ItemOrder int NOT NULL,
						IsRadiograph int NOT NULL,
						Width int NOT NULL,
						Height int NOT NULL,
						PRIMARY KEY (MountDefNum)
						)";
				}
				General.NonQEx(command);
				if(FormChooseDatabase.DBtype==DatabaseType.MySql) {
					command=@"CREATE TABLE mountitemdef(
						MountItemDefNum int NOT NULL auto_increment,
						MountDefNum int NOT NULL,
						Xpos mediumint NOT NULL,
						Ypos mediumint NOT NULL,
						Width mediumint NOT NULL,
						Height mediumint NOT NULL,
						PRIMARY KEY (MountItemDefNum)
						) DEFAULT CHARSET=utf8";
				}
				else {
					command=@"CREATE TABLE mountitemdef(
						MountItemDefNum int NOT NULL,
						MountDefNum int NOT NULL,
						Xpos int NOT NULL,
						Ypos int NOT NULL,
						Width int NOT NULL,
						Height int NOT NULL,
						PRIMARY KEY (MountItemDefNum)
						)";
				}
				General.NonQEx(command);
				command="INSERT INTO preference VALUES ('XRayExposureLevel','1')";
				General.NonQEx(command);*/
				//Dxis Bridge---------------------------------------------------------------------------
				string command = "INSERT INTO program (ProgName,ProgDesc,Enabled,Path,CommandLine,Note"
					+ ") VALUES("
					+ "'Dxis', "
					+ "'DXIS from dxis.com', "
					+ "'0', "
					+ "'" + POut.PString(@"C:\Dxis\Dxis.exe") + "', "
					+ "'', "
					+ "'" + POut.PString(@"") + "')";
				int programNum =General.NonQEx(command,true);//we now have a ProgramNum to work with
				command = "INSERT INTO toolbutitem (ProgramNum,ToolBar,ButtonText) "
					+ "VALUES ("
					+ "'" + POut.PInt(programNum) + "', "
					+ "'" + POut.PInt((int)ToolBarsAvail.ChartModule) + "', "
					+ "'DXIS')";
				General.NonQEx(command);


				command="UPDATE preference SET ValueString = '4.7.0.0' WHERE PrefName = 'DataBaseVersion'";
				General.NonQEx(command);
			}
			//To4_7_?();
		}


	}

}


























