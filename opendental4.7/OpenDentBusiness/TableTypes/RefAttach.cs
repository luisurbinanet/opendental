using System;

namespace OpenDentBusiness{

	///<summary>Attaches a referral to a patient.</summary>
	public class RefAttach{  
		///<summary>Primary key.</summary>
		public int RefAttachNum;
		///<summary>FK to referral.ReferralNum.</summary>
		public int ReferralNum;
		///<summary>FK to patient.PatNum.</summary>
		public int PatNum;
		///<summary>Order to display in patient info. Will be automated more in future.</summary>
		public int ItemOrder;
		///<summary>Date of referral.</summary>
		public DateTime RefDate;//
		///<summary>true=from, false=to</summary>
		public bool IsFrom;

		///<summary>Returns a copy of this RefAttach.</summary>
		public RefAttach Copy(){
			RefAttach r=new RefAttach();
			r.RefAttachNum=RefAttachNum;
			r.ReferralNum=ReferralNum;
			r.PatNum=PatNum;
			r.ItemOrder=ItemOrder;
			r.RefDate=RefDate;
			r.IsFrom=IsFrom;
			return r;
		}


	}

	

	

}













