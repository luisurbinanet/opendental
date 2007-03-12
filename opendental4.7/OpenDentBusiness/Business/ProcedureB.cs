using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProcedureB{
		///<summary></summary>
		public static DataSet Refresh(int patNum){
			string command="SELECT * FROM procedurelog WHERE PatNum="+POut.PInt(patNum);
			//if(!includeDeletedAndNotes){
				command+=" AND ProcStatus !=6";//don't include deleted
			//}
			command+=" ORDER BY ProcDate,ADACode";
				//notes:
			/*	+";SELECT * FROM procnote WHERE PatNum="+POut.PInt(patNum)
				+" ORDER BY EntryDateTime";*/
			DataConnection dcon=new DataConnection();
			DataSet ds=dcon.GetDs(command);
			//add a note column to the proc table.
			//ds.Tables[0].Columns.Add("UserNum");
			//ds.Tables[0].Columns.Add("Note");//,,typeof(string));
			//ds.Tables[0].Columns.Add("SigIsTopaz");//,typeof(string));
			//ds.Tables[0].Columns.Add("Signature");//,typeof(string));
			/*
			for(int i=0;i<ds.Tables[0].Rows.Count;i++){//loop through each proc
				for(int n=ds.Tables[1].Rows.Count-1;n>=0;n--){//loop through each note, backwards.
					if(ds.Tables[0].Rows[i]["ProcNum"].ToString() != ds.Tables[1].Rows[n]["ProcNum"].ToString()) {
						continue;
					}
					//userNum=PIn.PInt(ds.Tables[1].Rows[n]["UserNum"].ToString());
					ds.Tables[0].Rows[i]["UserNum"]   =ds.Tables[1].Rows[n]["UserNum"].ToString();
					ds.Tables[0].Rows[i]["Note"]      =ds.Tables[1].Rows[n]["Note"].ToString();
					ds.Tables[0].Rows[i]["SigIsTopaz"]=ds.Tables[1].Rows[n]["SigIsTopaz"].ToString();
					ds.Tables[0].Rows[i]["Signature"] =ds.Tables[1].Rows[n]["Signature"].ToString();
					break;//out of note loop.
				}
			}*/
			//ds.Tables.RemoveAt(1);
			return ds;
		}

		public static int Insert(Procedure proc){
			if(PrefB.RandomKeys) {
				proc.ProcNum=MiscDataB.GetKey("procedurelog","ProcNum");
			}
			string command= "INSERT INTO procedurelog (";
			if(PrefB.RandomKeys) {
				command+="ProcNum,";
			}
			command+="PatNum, AptNum, ADACode, ProcDate,ProcFee,Surf,"
				+"ToothNum,ToothRange,Priority,ProcStatus,ProvNum,"
				+"Dx,PlannedAptNum,PlaceService,Prosthesis,DateOriginalProsth,ClaimNote,"
				+"DateEntryC,ClinicNum,MedicalCode,DiagnosticCode,IsPrincDiag,ProcNumLab) VALUES(";
			if(PrefB.RandomKeys) {
				command+="'"+POut.PInt(proc.ProcNum)+"', ";
			}
			command+=
				 "'"+POut.PInt   (proc.PatNum)+"', "
				+"'"+POut.PInt   (proc.AptNum)+"', "
				+"'"+POut.PString(proc.ADACode)+"', "
				+POut.PDate  (proc.ProcDate)+", "
				+"'"+POut.PDouble(proc.ProcFee)+"', "
				+"'"+POut.PString(proc.Surf)+"', "
				+"'"+POut.PString(proc.ToothNum)+"', "
				+"'"+POut.PString(proc.ToothRange)+"', "
				+"'"+POut.PInt   (proc.Priority)+"', "
				+"'"+POut.PInt   ((int)proc.ProcStatus)+"', "
				+"'"+POut.PInt   (proc.ProvNum)+"', "
				+"'"+POut.PInt   (proc.Dx)+"', "
				+"'"+POut.PInt   (proc.PlannedAptNum)+"', "
				+"'"+POut.PInt   ((int)proc.PlaceService)+"', "
				+"'"+POut.PString(proc.Prosthesis)+"', "
				+POut.PDate  (proc.DateOriginalProsth)+", "
				+"'"+POut.PString(proc.ClaimNote)+"', ";
			if(DataConnection.DBtype==DatabaseType.Oracle){
				command+=POut.PDateT(MiscDataB.GetNowDateTime());
			}else{//Assume MySQL
				command+="NOW()";
			}
			command+=", "//DateEntryC
				+"'"+POut.PInt   (proc.ClinicNum)+"', "
				+"'"+POut.PString(proc.MedicalCode)+"', "
				+"'"+POut.PString(proc.DiagnosticCode)+"', "
				+"'"+POut.PBool  (proc.IsPrincDiag)+"', "
				+"'"+POut.PInt   (proc.ProcNumLab)+"')";
			//MessageBox.Show(cmd.CommandText);
			DataConnection dcon=new DataConnection();
			if(PrefB.RandomKeys) {
				dcon.NonQ(command);
			}
			else {
				dcon.NonQ(command,true);
				proc.ProcNum=dcon.InsertID;
			}
			if(proc.Note!=""){
				ProcNote note=new ProcNote();
				note.PatNum=proc.PatNum;
				note.ProcNum=proc.ProcNum;
				note.UserNum=proc.UserNum;
				note.Note=proc.Note;
				ProcNoteB.Insert(note);
			}
			return proc.ProcNum;
		}

		///<summary>Updates only the changed columns.</summary>
		public static int Update(Procedure proc,Procedure oldProc){
			bool comma=false;
			string c = "UPDATE procedurelog SET ";
			if(proc.PatNum!=oldProc.PatNum){
				c+="PatNum = '"     +POut.PInt   (proc.PatNum)+"'";
				comma=true;
			}
			if(proc.AptNum!=oldProc.AptNum){
				if(comma) c+=",";
				c+="AptNum = '"		+POut.PInt   (proc.AptNum)+"'";
				comma=true;
			}
			if(proc.ADACode!=oldProc.ADACode){
				if(comma) c+=",";
				c+="ADACode = '"		+POut.PString(proc.ADACode)+"'";
				comma=true;
			}
			if(proc.ProcDate!=oldProc.ProcDate){
				if(comma) c+=",";
				c+="ProcDate = "	+POut.PDate  (proc.ProcDate);
				comma=true;
			}
			if(proc.ProcFee!=oldProc.ProcFee){
				if(comma) c+=",";
				c+="ProcFee = '"		+POut.PDouble(proc.ProcFee)+"'";
				comma=true;
			}
			if(proc.Surf!=oldProc.Surf){
				if(comma) c+=",";
				c+="Surf = '"			+POut.PString(proc.Surf)+"'";
				comma=true;
			}
			if(proc.ToothNum!=oldProc.ToothNum){
				if(comma) c+=",";
				c+="ToothNum = '"	+POut.PString(proc.ToothNum)+"'";
				comma=true;
			}
			if(proc.ToothRange!=oldProc.ToothRange){
				if(comma) c+=",";
				c+="ToothRange = '"+POut.PString(proc.ToothRange)+"'";
				comma=true;
			}
			if(proc.Priority!=oldProc.Priority){
				if(comma) c+=",";
				c+="Priority = '"	+POut.PInt   (proc.Priority)+"'";
				comma=true;
			}
			if(proc.ProcStatus!=oldProc.ProcStatus){
				if(comma) c+=",";
				c+="ProcStatus = '"+POut.PInt   ((int)proc.ProcStatus)+"'";
				comma=true;
			}
			if(proc.ProvNum!=oldProc.ProvNum){
				if(comma) c+=",";
				c+="ProvNum = '"		+POut.PInt   (proc.ProvNum)+"'";
				comma=true;
			}
			if(proc.Dx!=oldProc.Dx){
				if(comma) c+=",";
				c+="Dx = '"				+POut.PInt   (proc.Dx)+"'";
				comma=true;
			}
			if(proc.PlannedAptNum!=oldProc.PlannedAptNum){
				if(comma) c+=",";
				c+="PlannedAptNum = '"+POut.PInt   (proc.PlannedAptNum)+"'";
				comma=true;
			}
			if(proc.PlaceService!=oldProc.PlaceService){
				if(comma) c+=",";
				c+="PlaceService = '"	+POut.PInt   ((int)proc.PlaceService)+"'";
				comma=true;
			}
			if(proc.Prosthesis!=oldProc.Prosthesis){
				if(comma) c+=",";
				c+="Prosthesis = '"+POut.PString(proc.Prosthesis)+"'";
				comma=true;
			}
			if(proc.DateOriginalProsth!=oldProc.DateOriginalProsth){
				if(comma) c+=",";
				c+="DateOriginalProsth = "+POut.PDate  (proc.DateOriginalProsth);
				comma=true;
			}
			if(proc.ClaimNote!=oldProc.ClaimNote){
				if(comma) c+=",";
				c+="ClaimNote = '"+POut.PString (proc.ClaimNote)+"'";
				comma=true;
			}
			if(proc.DateEntryC!=oldProc.DateEntryC){
				if(comma) c+=",";
				c+="DateEntryC = ";
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					c+=POut.PDateT(MiscDataB.GetNowDateTime());
				}
				else {//Assume MySQL
					c+="NOW()";
				}
				comma=true;
			}
			if(proc.ClinicNum!=oldProc.ClinicNum){
				if(comma) c+=",";
				c+="ClinicNum = '"+POut.PInt   (proc.ClinicNum)+"'";
				comma=true;
			}
			if(proc.MedicalCode!=oldProc.MedicalCode){
				if(comma) c+=",";
				c+="MedicalCode = '"+POut.PString(proc.MedicalCode)+"'";
				comma=true;
			}
			if(proc.DiagnosticCode!=oldProc.DiagnosticCode){
				if(comma) c+=",";
				c+="DiagnosticCode = '"+POut.PString(proc.DiagnosticCode)+"'";
				comma=true;
			}
			if(proc.IsPrincDiag!=oldProc.IsPrincDiag){
				if(comma) c+=",";
				c+="IsPrincDiag = '"+POut.PBool(proc.IsPrincDiag)+"'";
				comma=true;
			}
			if(proc.ProcNumLab!=oldProc.ProcNumLab) {
				if(comma) c+=",";
				c+="ProcNumLab = '"+POut.PInt(proc.ProcNumLab)+"'";
				comma=true;
			}
			int rowsChanged=0;
			if(comma){
				c+=" WHERE ProcNum = '"+POut.PInt(proc.ProcNum)+"'";
				DataConnection dcon=new DataConnection();
				rowsChanged=dcon.NonQ(c);
			}
			else{
				//rowsChanged=0;//this means no change is actually required.
			}
			if(proc.Note!=oldProc.Note
				|| proc.UserNum!=oldProc.UserNum
				|| proc.SigIsTopaz!=oldProc.SigIsTopaz
				|| proc.Signature!=oldProc.Signature)
			{
				ProcNote note=new ProcNote();
				note.PatNum=proc.PatNum;
				note.ProcNum=proc.ProcNum;
				note.UserNum=proc.UserNum;
				note.Note=proc.Note;
				note.SigIsTopaz=proc.SigIsTopaz;
				note.Signature=proc.Signature;
				ProcNoteB.Insert(note);
			}
			return rowsChanged;
		}

		///<summary>This does not actually delete the procedure, but just changes the status to deleted.  Throws exception if not allowed to delete.</summary>
		public static int Delete(int procNum) {
			//Test to see if any payment at all has been received for this proc
			string command="SELECT COUNT(*) FROM claimproc WHERE ProcNum="+POut.PInt(procNum)
				+" AND InsPayAmt > 0 AND Status != "+POut.PInt((int)ClaimProcStatus.Preauth);
			DataConnection dcon=new DataConnection();
			if(dcon.GetCount(command)!="0"){
				throw new Exception(Lan.g("Procedures","Not allowed to delete a procedure that is attached to a payment."));
			}
			//delete adjustments
			command="DELETE FROM adjustment WHERE ProcNum='"+POut.PInt(procNum)+"'";
			dcon.NonQ(command);
			//delete claimprocs
			command="DELETE from claimproc WHERE ProcNum = '"+POut.PInt(procNum)+"'";
			dcon.NonQ(command);
			//set the procedure deleted
			command="UPDATE procedurelog SET ProcStatus = "+POut.PInt((int)ProcStat.D)+", "
				+"AptNum=0, "
				+"PlannedAptNum=0 "
				+"WHERE ProcNum = '"+POut.PInt(procNum)+"'";
			int rowsChanged=dcon.NonQ(command);
			//Recalls.Synch(ProcCur.PatNum);//later
			return rowsChanged;
		}
	
		///<summary></summary>
		public static int UpdateAptNum(int procNum,int newAptNum){
			string command="UPDATE procedurelog SET AptNum = "+POut.PInt(newAptNum)
				+" WHERE ProcNum = "+POut.PInt(procNum);
			DataConnection dcon=new DataConnection();
			int rowsChanged=dcon.NonQ(command);
			return rowsChanged;
		}
	
		///<summary></summary>
		public static int UpdatePlannedAptNum(int procNum,int newPlannedAptNum) {
			string command="UPDATE procedurelog SET PlannedAptNum = "+POut.PInt(newPlannedAptNum)
				+" WHERE ProcNum = "+POut.PInt(procNum);
			DataConnection dcon=new DataConnection();
			int rowsChanged=dcon.NonQ(command);
			return rowsChanged;
		}
	
		///<summary></summary>
		public static int UpdatePriority(int procNum,int newPriority) {
			string command="UPDATE procedurelog SET Priority = "+POut.PInt(newPriority)
				+" WHERE ProcNum = "+POut.PInt(procNum);
			DataConnection dcon=new DataConnection();
			int rowsChanged=dcon.NonQ(command);
			return rowsChanged;
		}
	
		///<summary></summary>
		public static int UpdateFee(int procNum,double newFee) {
			string command="UPDATE procedurelog SET ProcFee = "+POut.PDouble(newFee)
				+" WHERE ProcNum = "+POut.PInt(procNum);
			DataConnection dcon=new DataConnection();
			int rowsChanged=dcon.NonQ(command);
			return rowsChanged;
		}

		///<summary>The supplied DataRows must include the following columns: Priority,ToothRange,ToothNum,ADACode.  This sorts procedures based on priority, then tooth number, then adaCode.  It does not care about dates or status.  Currently used in TP module and Chart module sorting.</summary>
		public static int CompareProcedures(DataRow x,DataRow y) {
			//first, by priority
			if(x["Priority"].ToString()!=y["Priority"].ToString()) {//if priorities are different
				if(x["Priority"].ToString()=="0") {
					return 1;//x is greater than y. Priorities always come first.
				}
				if(y["Priority"].ToString()=="0") {
					return -1;//x is less than y. Priorities always come first.
				}
				return DefB.GetOrder(DefCat.TxPriorities,PIn.PInt(x["Priority"].ToString())).CompareTo
					(DefB.GetOrder(DefCat.TxPriorities,PIn.PInt(y["Priority"].ToString())));
			}
			//priorities are the same, so sort by toothrange
			if(x["ToothRange"].ToString()!=y["ToothRange"].ToString()) {
				//empty toothranges come before filled toothrange values
				return x["ToothRange"].ToString().CompareTo(y["ToothRange"].ToString());
			}
			//toothranges are the same (usually empty), so compare toothnumbers
			if(x["ToothNum"].ToString()!=y["ToothNum"].ToString()) {
				//this also puts invalid or empty toothnumbers before the others.
				return Tooth.ToInt(x["ToothNum"].ToString()).CompareTo(Tooth.ToInt(y["ToothNum"].ToString()));
			}
			//priority and toothnums are the same, so sort by adacode.
			return x["ADACode"].ToString().CompareTo(y["ADACode"].ToString());
			//return 0;//priority, tooth number, and adacode are all the same
		}







	}


}
