using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{

	
  ///<summary></summary>
	public class ZipCodes{
		///<summary></summary>
		public static ZipCode[] List;
		///<summary></summary>
		public static ArrayList ALFrequent;
		///<summary></summary>
		public static ArrayList ALMatches;
		//public static Hashtable HList; 

		///<summary>Refresh done on startup and then whenever a change is made.</summary>
		public static void Refresh(){
			string command =
				"SELECT * from zipcode ORDER BY zipcodedigits";
			DataTable table=General.GetTable(command);;
			//HList=new Hashtable();
			ALFrequent=new ArrayList();
			List=new ZipCode[table.Rows.Count];
			for(int i=0;i<List.Length;i++){
				List[i]=new ZipCode();
				List[i].ZipCodeNum   =PIn.PInt   (table.Rows[i][0].ToString());
				List[i].ZipCodeDigits=PIn.PString(table.Rows[i][1].ToString());
				List[i].City         =PIn.PString(table.Rows[i][2].ToString());	
				List[i].State        =PIn.PString(table.Rows[i][3].ToString());	
				List[i].IsFrequent   =PIn.PBool  (table.Rows[i][4].ToString());
				if(List[i].IsFrequent){
					ALFrequent.Add(List[i]);
				}
				//HList.Add(List[i].ZipCodeNum,List[i]);
			}
		}

		///<summary></summary>
		public static void Insert(ZipCode Cur){
			if(PrefB.RandomKeys){
				Cur.ZipCodeNum=MiscData.GetKey("zipcode","ZipCodeNum");
			}
			string command="INSERT INTO zipcode (";
			if(PrefB.RandomKeys){
				 command+="ZipCodeNum,";
			}
			command+="zipcodedigits,city,state,isfrequent) VALUES(";
			if(PrefB.RandomKeys){
				command+="'"+POut.PInt(Cur.ZipCodeNum)+"', ";
			}
			command+=
				 "'"+POut.PString(Cur.ZipCodeDigits)+"', "
				+"'"+POut.PString(Cur.City)+"', "
				+"'"+POut.PString(Cur.State)+"', "
				+"'"+POut.PBool  (Cur.IsFrequent)+"')";
			if(PrefB.RandomKeys){
				General.NonQ(command);
			}
			else{
 				Cur.ZipCodeNum=General.NonQ(command,true);
			}
		}

		///<summary></summary>
		public static void Update(ZipCode Cur){
			string command = "UPDATE zipcode SET "
				+"zipcodedigits ='"+POut.PString(Cur.ZipCodeDigits)+"'"
				+",city ='"        +POut.PString(Cur.City)+"'"
				+",state ='"       +POut.PString(Cur.State)+"'"
				+",isfrequent ='"  +POut.PBool  (Cur.IsFrequent)+"'"
				+" WHERE zipcodenum = '"+POut.PInt(Cur.ZipCodeNum)+"'";
			General.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(ZipCode Cur){
			string command = "DELETE from zipcode WHERE zipcodenum = '"+POut.PInt(Cur.ZipCodeNum)+"'";
			General.NonQ(command);
		}

		///<summary></summary>
		public static void GetALMatches(string zipCodeDigits){
			ALMatches=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].ZipCodeDigits==zipCodeDigits){
					ALMatches.Add(List[i]);
				}
			}

		}

	}

	

}













