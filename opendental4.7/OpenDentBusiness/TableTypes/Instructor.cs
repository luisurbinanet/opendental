using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>Used by dental schools.</summary>
	public class Instructor{
		///<summary>Primary key.</summary>
		public int InstructorNum;
		///<summary>.</summary>
		public string LName;
		///<summary>.</summary>
		public string FName;
		///<summary>eg DMD, DDS, RDH.</summary>
		public string Suffix;
		//<summary></summary>
		//public bool IsHidden;//do this later
		
		///<summary></summary>
		public Instructor Copy(){
			Instructor i=new Instructor();
			i.InstructorNum=InstructorNum;
			i.LName=LName;
			i.FName=FName;
			i.Suffix=Suffix;
			return i;
		}
	}

	

	


}




















