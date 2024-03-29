using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class AppointmentRules {
		///<summary></summary>
		public static AppointmentRule[] List;

		///<summary>Fills List with all AppointmentRules.</summary>
		public static void Refresh() {
			string command="SELECT * FROM appointmentrule";
			DataTable table=General.GetTable(command);
			List=new AppointmentRule[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				List[i]=new AppointmentRule();
				List[i].AppointmentRuleNum = PIn.PInt(table.Rows[i][0].ToString());
				List[i].RuleDesc           = PIn.PString(table.Rows[i][1].ToString());
				List[i].ADACodeStart       = PIn.PString(table.Rows[i][2].ToString());
				List[i].ADACodeEnd         = PIn.PString(table.Rows[i][3].ToString());
				List[i].IsEnabled          = PIn.PBool(table.Rows[i][4].ToString());
			}
		}

		///<summary></summary>
		public static void Insert(AppointmentRule rule){
			string command= "INSERT INTO appointmentrule (RuleDesc,ADACodeStart,ADACodeEnd,IsEnabled) VALUES("
				+"'"+POut.PString(rule.RuleDesc)+"', "
				+"'"+POut.PString(rule.ADACodeStart)+"', "
				+"'"+POut.PString(rule.ADACodeEnd)+"', "
				+"'"+POut.PBool  (rule.IsEnabled)+"')";
 			rule.AppointmentRuleNum=General.NonQ(command,true);
		}

		///<summary></summary>
		public static void Update(AppointmentRule rule){
			string command= "UPDATE appointmentrule SET " 
				+ "RuleDesc = '"      +POut.PString(rule.RuleDesc)+"'"
				+ ",ADACodeStart = '" +POut.PString(rule.ADACodeStart)+"'"
				+ ",ADACodeEnd = '"   +POut.PString(rule.ADACodeEnd)+"'"
				+ ",IsEnabled = '"    +POut.PBool  (rule.IsEnabled)+"'"
				+" WHERE AppointmentRuleNum = '" +POut.PInt   (rule.AppointmentRuleNum)+"'";
 			General.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(AppointmentRule rule){
			string command="DELETE FROM appointmentrule" 
				+" WHERE AppointmentRuleNum = "+POut.PInt(rule.AppointmentRuleNum);
 			General.NonQ(command);
		}

		///<summary>Whenever an appointment is scheduled, the procedures which would be double booked are calculated.  In this method, those procedures are checked to see if the double booking should be blocked.  If double booking is indeed blocked, then a separate function will tell the user which category.</summary>
		public static bool IsBlocked(ArrayList adaCodes){
			for(int j=0;j<adaCodes.Count;j++){
				for(int i=0;i<List.Length;i++){
					if(!List[i].IsEnabled){
						continue;
					}
					if(String.Compare((string)adaCodes[j],List[i].ADACodeStart) < 0){
						continue;
					}
					if(String.Compare((string)adaCodes[j],List[i].ADACodeEnd) > 0) {
						continue;
					}
					return true;
				}
			}
			return false;
		}

		///<summary>Whenever an appointment is blocked from being double booked, this method will tell the user which category.</summary>
		public static string GetBlockedDescription(ArrayList adaCodes){
			for(int j=0;j<adaCodes.Count;j++) {
				for(int i=0;i<List.Length;i++) {
					if(!List[i].IsEnabled) {
						continue;
					}
					if(String.Compare((string)adaCodes[j],List[i].ADACodeStart) < 0) {
						continue;
					}
					if(String.Compare((string)adaCodes[j],List[i].ADACodeEnd) > 0) {
						continue;
					}
					return List[i].RuleDesc;
				}
			}
			return "";
		}

		
		

	}
	


}













