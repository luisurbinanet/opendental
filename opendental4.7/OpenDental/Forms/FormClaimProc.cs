using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental
{
	/// <summary>
	/// Summary description for FormClaimProcEdit.
	/// </summary>
	public class FormClaimProc : System.Windows.Forms.Form
	{
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.Label label9;
		private OpenDental.ValidDouble textInsPayAmt;
		private System.Windows.Forms.TextBox textRemarks;
		private System.Windows.Forms.ListBox listStatus;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.ValidDouble textWriteOff;
		private OpenDental.ValidDouble textInsPayEst;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textInsPlan;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.CheckBox checkNoBillIns;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textPercentage;
		private OpenDental.ValidDouble textCopayAmt;
		private OpenDental.ValidDouble textOverrideInsEst;
		private OpenDental.ValidNumber textPercentOverride;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.ListBox listProv;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.TextBox textCodeSent;
		private OpenDental.ValidDouble textFeeBilled;
		private System.Windows.Forms.CheckBox checkDedBeforePerc;
		private OpenDental.ValidDouble textDedBeforePerc;
		private OpenDental.ValidDouble textDedApplied;
		private System.Windows.Forms.RadioButton radioEstimate;
		private System.Windows.Forms.RadioButton radioClaim;
		private OpenDental.ValidDate textDateCP;
		private OpenDental.ValidDouble textAllowedAmt;
		private OpenDental.ValidDouble textOverAnnualMax;
		private OpenDental.ValidDouble textPaidOtherIns;
		private System.Windows.Forms.Label labelInsFee;
		private System.Windows.Forms.Label labelPatWriteOff;
		private System.Windows.Forms.Label labelPatInsPayEst;
		private System.Windows.Forms.Label labelInsOverrideInsEst;
		private System.Windows.Forms.Label labelInsPercentage;
		private System.Windows.Forms.Label labelInsDedBeforePerc;
		private System.Windows.Forms.TextBox textFee;
		private System.Windows.Forms.Label labelPatFee;
		private System.Windows.Forms.Label labelInsOverAnnualMax;
		private System.Windows.Forms.Label labelPatTotal;
		private System.Windows.Forms.Label labelInsDedApplied;
		private System.Windows.Forms.Label labelInsPaidOtherIns;
		private System.Windows.Forms.Label labelInsCopay;
		private System.Windows.Forms.Label labelInsPercentOverride;
		private System.Windows.Forms.Label labelInsAllowed;
		private System.Windows.Forms.Label labelInsInsPayEst;
		private System.Windows.Forms.Label labelPatInsPayAmt;
		private System.Windows.Forms.Label labelInsInsPayAmt;
		private System.Windows.Forms.Label labelPatOverrideInsEstt;
		//public bool IsNew;
		///<summary>Set to true if this claimProc is accessed from within a claim. This changes the behavior of the form, allowing more freedom with fields that are also totalled for entire claim.  This freedom is normally restricted so that claim totals will stay synchronized with individual claimprocs.</summary>
		public bool IsInClaim;
		private System.Windows.Forms.Label labelDedBeforePerc;
		private System.Windows.Forms.Label labelDedApplied;
		private System.Windows.Forms.Panel panelEstimateInfo;
		private System.Windows.Forms.Label labelNotInClaim;
		private System.Windows.Forms.Label labelAttachedToCheck;
		private System.Windows.Forms.GroupBox groupClaimInfo;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBaseEst;
		private System.Windows.Forms.Label labelInsBaseEst;
		private System.Windows.Forms.Label labelPatBaseEstt;
		private System.Windows.Forms.Label labelInsPayAmt;
		private System.Windows.Forms.Label labelInsPayEst;
		private System.Windows.Forms.Label labelOverAnnualMax;
		private System.Windows.Forms.Label labelPaidOtherIns;
		private System.Windows.Forms.Label labelPatPortion;
		private System.Windows.Forms.Label labelCopayAmt;
		private System.Windows.Forms.Label labelWriteOff;
		private System.Windows.Forms.Panel panelClaimBar;
		private System.Windows.Forms.Label labelFee;
		private System.Windows.Forms.Label labelCodeSent;
		private System.Windows.Forms.Label labelFeeBilled;
		private System.Windows.Forms.Label labelRemarks;
		///<summary>True if this is a procedure, and false if only a claim total.</summary>
		private bool IsProc;
		///<summary>Stores the procedure for this ClaimProc if applicable</summary>
		//private Procedure procCur;
		private ClaimProc ClaimProcCur;
		///<summary>If user hits cancel, then the claimproc is reset using this.</summary>
		private ClaimProc ClaimProcOld;
		private System.Windows.Forms.Label labelInsCopayOverride;
		private OpenDental.ValidDouble textCopayOverride;
		private System.Windows.Forms.Label labelCopayOverride;
		private System.Windows.Forms.Panel panelClaimExtras;
		///<summary>The procedure to which this claimproc is attached.</summary>
		public Procedure proc;
		private System.Windows.Forms.GroupBox groupClaim;
		private OpenDental.ValidDate textProcDate;
		private System.Windows.Forms.Label labelProcDate;
		///<summary>This is used when we don't have access to proc.</summary>
		private double ProcFee;
		///<summary>This is used when we don't have access to proc.</summary>
		private string ProcADACode;
		private Family FamCur;
		private InsPlan[] PlanList;
		private System.Windows.Forms.Label labelCarrierAllowedAmt;
		private System.Windows.Forms.TextBox textCarrierAllowedAmt;
		private OpenDental.UI.Button butUpdateAllowed;
		///<summary>Set this to true if user does not have permission.</summary>
		public bool NoPermission;
		private OpenDental.ValidDate textDateEntry;
		private System.Windows.Forms.Label labelDateEntry;
		private double CarrierAllowedAmt;

		///<summary>procCur can be null if not editing from within an actual procedure.</summary>
		public FormClaimProc(ClaimProc claimProcCur,Procedure procCur,Family famCur,InsPlan[] planList){
			ClaimProcCur=claimProcCur;
			ClaimProcOld=ClaimProcCur.Copy();
			proc=procCur;
			FamCur=famCur;
			PlanList=planList;
			InitializeComponent();// Required for Windows Form Designer support
			//can't use Lan.F because of complexity of label use
			Lan.C(this, new System.Windows.Forms.Control[]
				{
					this,
					this.label1,
					this.label9,
					this.label30,
					this.labelProcDate,
					this.label28,
					this.label29,
					this.groupClaim,
					this.radioEstimate,
					this.radioClaim,
					this.labelCodeSent,
					this.labelFeeBilled,
					this.labelRemarks,
					this.labelNotInClaim,
					this.checkNoBillIns,
					this.label14,
					this.labelPatPortion,
					this.labelFee,
					this.label3,
					this.checkDedBeforePerc,
					this.labelDedBeforePerc,
					this.labelCopayAmt,
					this.labelCopayOverride,
					this.label4,
					this.label13,
					this.label5,
					this.label12,
					this.groupClaimInfo,
					this.labelDedApplied,
					this.labelPaidOtherIns,
					this.labelOverAnnualMax,
					this.labelInsPayEst,
					this.labelInsPayAmt,
					this.labelWriteOff,
					this.labelDateEntry
					//this.butRecalc
			});
			Lan.C("All", new System.Windows.Forms.Control[] {
				butOK,
				butCancel,
				butDelete,
			});
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimProc));
			this.labelDedBeforePerc = new System.Windows.Forms.Label();
			this.labelInsPayAmt = new System.Windows.Forms.Label();
			this.labelRemarks = new System.Windows.Forms.Label();
			this.textRemarks = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.listStatus = new System.Windows.Forms.ListBox();
			this.labelWriteOff = new System.Windows.Forms.Label();
			this.labelInsPayEst = new System.Windows.Forms.Label();
			this.labelNotInClaim = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textInsPlan = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textPercentage = new System.Windows.Forms.TextBox();
			this.labelCopayAmt = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.checkNoBillIns = new System.Windows.Forms.CheckBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.labelInsFee = new System.Windows.Forms.Label();
			this.labelPatWriteOff = new System.Windows.Forms.Label();
			this.labelPatInsPayEst = new System.Windows.Forms.Label();
			this.labelInsOverrideInsEst = new System.Windows.Forms.Label();
			this.labelInsPercentage = new System.Windows.Forms.Label();
			this.labelPatPortion = new System.Windows.Forms.Label();
			this.labelInsDedBeforePerc = new System.Windows.Forms.Label();
			this.labelFee = new System.Windows.Forms.Label();
			this.textFee = new System.Windows.Forms.TextBox();
			this.labelPatFee = new System.Windows.Forms.Label();
			this.labelOverAnnualMax = new System.Windows.Forms.Label();
			this.labelInsOverAnnualMax = new System.Windows.Forms.Label();
			this.labelPatTotal = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.listProv = new System.Windows.Forms.ListBox();
			this.label30 = new System.Windows.Forms.Label();
			this.labelCodeSent = new System.Windows.Forms.Label();
			this.textCodeSent = new System.Windows.Forms.TextBox();
			this.labelFeeBilled = new System.Windows.Forms.Label();
			this.labelInsDedApplied = new System.Windows.Forms.Label();
			this.labelDedApplied = new System.Windows.Forms.Label();
			this.labelInsPaidOtherIns = new System.Windows.Forms.Label();
			this.labelPaidOtherIns = new System.Windows.Forms.Label();
			this.labelInsCopay = new System.Windows.Forms.Label();
			this.labelInsPercentOverride = new System.Windows.Forms.Label();
			this.labelInsAllowed = new System.Windows.Forms.Label();
			this.labelInsInsPayEst = new System.Windows.Forms.Label();
			this.checkDedBeforePerc = new System.Windows.Forms.CheckBox();
			this.labelPatInsPayAmt = new System.Windows.Forms.Label();
			this.panelClaimBar = new System.Windows.Forms.Panel();
			this.labelInsInsPayAmt = new System.Windows.Forms.Label();
			this.groupClaim = new System.Windows.Forms.GroupBox();
			this.labelAttachedToCheck = new System.Windows.Forms.Label();
			this.radioClaim = new System.Windows.Forms.RadioButton();
			this.radioEstimate = new System.Windows.Forms.RadioButton();
			this.panelClaimExtras = new System.Windows.Forms.Panel();
			this.labelPatOverrideInsEstt = new System.Windows.Forms.Label();
			this.panelEstimateInfo = new System.Windows.Forms.Panel();
			this.labelCarrierAllowedAmt = new System.Windows.Forms.Label();
			this.textCarrierAllowedAmt = new System.Windows.Forms.TextBox();
			this.labelInsCopayOverride = new System.Windows.Forms.Label();
			this.labelCopayOverride = new System.Windows.Forms.Label();
			this.labelInsBaseEst = new System.Windows.Forms.Label();
			this.labelPatBaseEstt = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBaseEst = new System.Windows.Forms.TextBox();
			this.groupClaimInfo = new System.Windows.Forms.GroupBox();
			this.labelProcDate = new System.Windows.Forms.Label();
			this.labelDateEntry = new System.Windows.Forms.Label();
			this.textDateEntry = new OpenDental.ValidDate();
			this.butUpdateAllowed = new OpenDental.UI.Button();
			this.textProcDate = new OpenDental.ValidDate();
			this.textFeeBilled = new OpenDental.ValidDouble();
			this.textWriteOff = new OpenDental.ValidDouble();
			this.textOverAnnualMax = new OpenDental.ValidDouble();
			this.textInsPayEst = new OpenDental.ValidDouble();
			this.textInsPayAmt = new OpenDental.ValidDouble();
			this.textDedApplied = new OpenDental.ValidDouble();
			this.textPaidOtherIns = new OpenDental.ValidDouble();
			this.textDateCP = new OpenDental.ValidDate();
			this.butDelete = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textCopayOverride = new OpenDental.ValidDouble();
			this.textDedBeforePerc = new OpenDental.ValidDouble();
			this.textAllowedAmt = new OpenDental.ValidDouble();
			this.textCopayAmt = new OpenDental.ValidDouble();
			this.textOverrideInsEst = new OpenDental.ValidDouble();
			this.textPercentOverride = new OpenDental.ValidNumber();
			this.groupClaim.SuspendLayout();
			this.panelClaimExtras.SuspendLayout();
			this.panelEstimateInfo.SuspendLayout();
			this.groupClaimInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelDedBeforePerc
			// 
			this.labelDedBeforePerc.Location = new System.Drawing.Point(8,129);
			this.labelDedBeforePerc.Name = "labelDedBeforePerc";
			this.labelDedBeforePerc.Size = new System.Drawing.Size(128,17);
			this.labelDedBeforePerc.TabIndex = 11;
			this.labelDedBeforePerc.Text = "Deductible";
			this.labelDedBeforePerc.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelInsPayAmt
			// 
			this.labelInsPayAmt.Location = new System.Drawing.Point(14,101);
			this.labelInsPayAmt.Name = "labelInsPayAmt";
			this.labelInsPayAmt.Size = new System.Drawing.Size(126,17);
			this.labelInsPayAmt.TabIndex = 13;
			this.labelInsPayAmt.Text = "Insurance Paid";
			this.labelInsPayAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelRemarks
			// 
			this.labelRemarks.Location = new System.Drawing.Point(14,48);
			this.labelRemarks.Name = "labelRemarks";
			this.labelRemarks.Size = new System.Drawing.Size(113,37);
			this.labelRemarks.TabIndex = 14;
			this.labelRemarks.Text = "Remarks from EOB";
			this.labelRemarks.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textRemarks
			// 
			this.textRemarks.Location = new System.Drawing.Point(129,49);
			this.textRemarks.MaxLength = 255;
			this.textRemarks.Multiline = true;
			this.textRemarks.Name = "textRemarks";
			this.textRemarks.Size = new System.Drawing.Size(290,129);
			this.textRemarks.TabIndex = 15;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(132,25);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80,17);
			this.label9.TabIndex = 16;
			this.label9.Text = "Status";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listStatus
			// 
			this.listStatus.Location = new System.Drawing.Point(133,44);
			this.listStatus.Name = "listStatus";
			this.listStatus.Size = new System.Drawing.Size(95,108);
			this.listStatus.TabIndex = 17;
			this.listStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listStatus_MouseDown);
			// 
			// labelWriteOff
			// 
			this.labelWriteOff.Location = new System.Drawing.Point(19,123);
			this.labelWriteOff.Name = "labelWriteOff";
			this.labelWriteOff.Size = new System.Drawing.Size(120,17);
			this.labelWriteOff.TabIndex = 19;
			this.labelWriteOff.Text = "Write Off";
			this.labelWriteOff.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelInsPayEst
			// 
			this.labelInsPayEst.Location = new System.Drawing.Point(12,80);
			this.labelInsPayEst.Name = "labelInsPayEst";
			this.labelInsPayEst.Size = new System.Drawing.Size(129,17);
			this.labelInsPayEst.TabIndex = 21;
			this.labelInsPayEst.Text = "Insurance Estimate";
			this.labelInsPayEst.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelNotInClaim
			// 
			this.labelNotInClaim.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelNotInClaim.Location = new System.Drawing.Point(118,246);
			this.labelNotInClaim.Name = "labelNotInClaim";
			this.labelNotInClaim.Size = new System.Drawing.Size(331,17);
			this.labelNotInClaim.TabIndex = 26;
			this.labelNotInClaim.Text = "Changes can only be made from within the claim.";
			this.labelNotInClaim.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9,7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121,14);
			this.label1.TabIndex = 28;
			this.label1.Text = "Ins Plan:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textInsPlan
			// 
			this.textInsPlan.Location = new System.Drawing.Point(133,4);
			this.textInsPlan.Name = "textInsPlan";
			this.textInsPlan.ReadOnly = true;
			this.textInsPlan.Size = new System.Drawing.Size(357,20);
			this.textInsPlan.TabIndex = 29;
			this.textInsPlan.Text = "An insurance plan";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8,91);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128,17);
			this.label3.TabIndex = 31;
			this.label3.Text = "Allowed";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(30,192);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(107,14);
			this.label4.TabIndex = 32;
			this.label4.Text = "Percentage";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPercentage
			// 
			this.textPercentage.Location = new System.Drawing.Point(168,190);
			this.textPercentage.Name = "textPercentage";
			this.textPercentage.ReadOnly = true;
			this.textPercentage.Size = new System.Drawing.Size(48,20);
			this.textPercentage.TabIndex = 33;
			this.textPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelCopayAmt
			// 
			this.labelCopayAmt.Location = new System.Drawing.Point(4,149);
			this.labelCopayAmt.Name = "labelCopayAmt";
			this.labelCopayAmt.Size = new System.Drawing.Size(133,17);
			this.labelCopayAmt.TabIndex = 37;
			this.labelCopayAmt.Text = "Patient Copay";
			this.labelCopayAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6,255);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(131,17);
			this.label12.TabIndex = 39;
			this.label12.Text = "Override Ins Estimate";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkNoBillIns
			// 
			this.checkNoBillIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNoBillIns.Location = new System.Drawing.Point(580,9);
			this.checkNoBillIns.Name = "checkNoBillIns";
			this.checkNoBillIns.Size = new System.Drawing.Size(270,22);
			this.checkNoBillIns.TabIndex = 40;
			this.checkNoBillIns.Text = "Do Not Bill to This Insurance";
			this.checkNoBillIns.Click += new System.EventHandler(this.checkNoBillIns_Click);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(4,213);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(132,14);
			this.label13.TabIndex = 44;
			this.label13.Text = "Percent Override";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.label14.Location = new System.Drawing.Point(210,9);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(74,36);
			this.label14.TabIndex = 48;
			this.label14.Text = "Insurance Portion:";
			this.label14.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// labelInsFee
			// 
			this.labelInsFee.Location = new System.Drawing.Point(217,50);
			this.labelInsFee.Name = "labelInsFee";
			this.labelInsFee.Size = new System.Drawing.Size(67,16);
			this.labelInsFee.TabIndex = 49;
			this.labelInsFee.Text = "$520.00";
			this.labelInsFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPatWriteOff
			// 
			this.labelPatWriteOff.Location = new System.Drawing.Point(294,126);
			this.labelPatWriteOff.Name = "labelPatWriteOff";
			this.labelPatWriteOff.Size = new System.Drawing.Size(67,16);
			this.labelPatWriteOff.TabIndex = 50;
			this.labelPatWriteOff.Text = "-$20.00";
			this.labelPatWriteOff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPatInsPayEst
			// 
			this.labelPatInsPayEst.Location = new System.Drawing.Point(294,80);
			this.labelPatInsPayEst.Name = "labelPatInsPayEst";
			this.labelPatInsPayEst.Size = new System.Drawing.Size(67,16);
			this.labelPatInsPayEst.TabIndex = 51;
			this.labelPatInsPayEst.Text = "-$200.00";
			this.labelPatInsPayEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInsOverrideInsEst
			// 
			this.labelInsOverrideInsEst.Location = new System.Drawing.Point(217,256);
			this.labelInsOverrideInsEst.Name = "labelInsOverrideInsEst";
			this.labelInsOverrideInsEst.Size = new System.Drawing.Size(67,16);
			this.labelInsOverrideInsEst.TabIndex = 53;
			this.labelInsOverrideInsEst.Text = "= $400.00";
			this.labelInsOverrideInsEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInsPercentage
			// 
			this.labelInsPercentage.Location = new System.Drawing.Point(217,191);
			this.labelInsPercentage.Name = "labelInsPercentage";
			this.labelInsPercentage.Size = new System.Drawing.Size(67,16);
			this.labelInsPercentage.TabIndex = 54;
			this.labelInsPercentage.Text = "x  80%";
			this.labelInsPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPatPortion
			// 
			this.labelPatPortion.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelPatPortion.Location = new System.Drawing.Point(289,12);
			this.labelPatPortion.Name = "labelPatPortion";
			this.labelPatPortion.Size = new System.Drawing.Size(69,33);
			this.labelPatPortion.TabIndex = 55;
			this.labelPatPortion.Text = "Patient Portion:";
			this.labelPatPortion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// labelInsDedBeforePerc
			// 
			this.labelInsDedBeforePerc.Location = new System.Drawing.Point(217,128);
			this.labelInsDedBeforePerc.Name = "labelInsDedBeforePerc";
			this.labelInsDedBeforePerc.Size = new System.Drawing.Size(67,16);
			this.labelInsDedBeforePerc.TabIndex = 57;
			this.labelInsDedBeforePerc.Text = "-$50.00";
			this.labelInsDedBeforePerc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelFee
			// 
			this.labelFee.Location = new System.Drawing.Point(25,51);
			this.labelFee.Name = "labelFee";
			this.labelFee.Size = new System.Drawing.Size(107,14);
			this.labelFee.TabIndex = 58;
			this.labelFee.Text = "Fee";
			this.labelFee.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textFee
			// 
			this.textFee.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textFee.Location = new System.Drawing.Point(152,51);
			this.textFee.Name = "textFee";
			this.textFee.ReadOnly = true;
			this.textFee.Size = new System.Drawing.Size(61,13);
			this.textFee.TabIndex = 59;
			this.textFee.Text = "520.00";
			this.textFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelPatFee
			// 
			this.labelPatFee.Location = new System.Drawing.Point(291,50);
			this.labelPatFee.Name = "labelPatFee";
			this.labelPatFee.Size = new System.Drawing.Size(67,16);
			this.labelPatFee.TabIndex = 60;
			this.labelPatFee.Text = "$520.00";
			this.labelPatFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelOverAnnualMax
			// 
			this.labelOverAnnualMax.Location = new System.Drawing.Point(11,59);
			this.labelOverAnnualMax.Name = "labelOverAnnualMax";
			this.labelOverAnnualMax.Size = new System.Drawing.Size(129,17);
			this.labelOverAnnualMax.TabIndex = 62;
			this.labelOverAnnualMax.Text = "Over Annual Max";
			this.labelOverAnnualMax.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelInsOverAnnualMax
			// 
			this.labelInsOverAnnualMax.Location = new System.Drawing.Point(220,60);
			this.labelInsOverAnnualMax.Name = "labelInsOverAnnualMax";
			this.labelInsOverAnnualMax.Size = new System.Drawing.Size(67,16);
			this.labelInsOverAnnualMax.TabIndex = 63;
			this.labelInsOverAnnualMax.Text = "-$150.00";
			this.labelInsOverAnnualMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPatTotal
			// 
			this.labelPatTotal.Location = new System.Drawing.Point(294,151);
			this.labelPatTotal.Name = "labelPatTotal";
			this.labelPatTotal.Size = new System.Drawing.Size(67,16);
			this.labelPatTotal.TabIndex = 64;
			this.labelPatTotal.Text = "= $400.00";
			this.labelPatTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(6,180);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(125,17);
			this.label28.TabIndex = 65;
			this.label28.Text = "Payment Date";
			this.label28.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(9,224);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(121,17);
			this.label29.TabIndex = 67;
			this.label29.Text = "Description";
			this.label29.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(133,220);
			this.textDescription.Name = "textDescription";
			this.textDescription.ReadOnly = true;
			this.textDescription.Size = new System.Drawing.Size(203,20);
			this.textDescription.TabIndex = 68;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(263,44);
			this.listProv.Name = "listProv";
			this.listProv.Size = new System.Drawing.Size(120,108);
			this.listProv.TabIndex = 70;
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(262,26);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(73,17);
			this.label30.TabIndex = 69;
			this.label30.Text = "Provider";
			this.label30.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelCodeSent
			// 
			this.labelCodeSent.Location = new System.Drawing.Point(8,10);
			this.labelCodeSent.Name = "labelCodeSent";
			this.labelCodeSent.Size = new System.Drawing.Size(121,14);
			this.labelCodeSent.TabIndex = 74;
			this.labelCodeSent.Text = "Code Sent to Ins";
			this.labelCodeSent.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCodeSent
			// 
			this.textCodeSent.Location = new System.Drawing.Point(129,7);
			this.textCodeSent.Name = "textCodeSent";
			this.textCodeSent.Size = new System.Drawing.Size(77,20);
			this.textCodeSent.TabIndex = 73;
			// 
			// labelFeeBilled
			// 
			this.labelFeeBilled.Location = new System.Drawing.Point(7,30);
			this.labelFeeBilled.Name = "labelFeeBilled";
			this.labelFeeBilled.Size = new System.Drawing.Size(121,17);
			this.labelFeeBilled.TabIndex = 71;
			this.labelFeeBilled.Text = "Fee Billed to Ins";
			this.labelFeeBilled.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelInsDedApplied
			// 
			this.labelInsDedApplied.Location = new System.Drawing.Point(224,18);
			this.labelInsDedApplied.Name = "labelInsDedApplied";
			this.labelInsDedApplied.Size = new System.Drawing.Size(63,16);
			this.labelInsDedApplied.TabIndex = 77;
			this.labelInsDedApplied.Text = "-$50.00";
			this.labelInsDedApplied.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDedApplied
			// 
			this.labelDedApplied.Location = new System.Drawing.Point(10,18);
			this.labelDedApplied.Name = "labelDedApplied";
			this.labelDedApplied.Size = new System.Drawing.Size(129,17);
			this.labelDedApplied.TabIndex = 76;
			this.labelDedApplied.Text = "Deductible";
			this.labelDedApplied.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelInsPaidOtherIns
			// 
			this.labelInsPaidOtherIns.Location = new System.Drawing.Point(222,39);
			this.labelInsPaidOtherIns.Name = "labelInsPaidOtherIns";
			this.labelInsPaidOtherIns.Size = new System.Drawing.Size(65,16);
			this.labelInsPaidOtherIns.TabIndex = 80;
			this.labelInsPaidOtherIns.Text = "-$88.00";
			this.labelInsPaidOtherIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPaidOtherIns
			// 
			this.labelPaidOtherIns.Location = new System.Drawing.Point(11,39);
			this.labelPaidOtherIns.Name = "labelPaidOtherIns";
			this.labelPaidOtherIns.Size = new System.Drawing.Size(129,17);
			this.labelPaidOtherIns.TabIndex = 79;
			this.labelPaidOtherIns.Text = "Paid By Other Ins";
			this.labelPaidOtherIns.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelInsCopay
			// 
			this.labelInsCopay.Location = new System.Drawing.Point(217,150);
			this.labelInsCopay.Name = "labelInsCopay";
			this.labelInsCopay.Size = new System.Drawing.Size(67,16);
			this.labelInsCopay.TabIndex = 81;
			this.labelInsCopay.Text = "-$10.00";
			this.labelInsCopay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInsPercentOverride
			// 
			this.labelInsPercentOverride.Location = new System.Drawing.Point(217,211);
			this.labelInsPercentOverride.Name = "labelInsPercentOverride";
			this.labelInsPercentOverride.Size = new System.Drawing.Size(67,16);
			this.labelInsPercentOverride.TabIndex = 82;
			this.labelInsPercentOverride.Text = "x  80%";
			this.labelInsPercentOverride.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInsAllowed
			// 
			this.labelInsAllowed.Location = new System.Drawing.Point(217,90);
			this.labelInsAllowed.Name = "labelInsAllowed";
			this.labelInsAllowed.Size = new System.Drawing.Size(67,16);
			this.labelInsAllowed.TabIndex = 83;
			this.labelInsAllowed.Text = "$500.00";
			this.labelInsAllowed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInsInsPayEst
			// 
			this.labelInsInsPayEst.Location = new System.Drawing.Point(222,80);
			this.labelInsInsPayEst.Name = "labelInsInsPayEst";
			this.labelInsInsPayEst.Size = new System.Drawing.Size(65,16);
			this.labelInsInsPayEst.TabIndex = 85;
			this.labelInsInsPayEst.Text = "= $200.00";
			this.labelInsInsPayEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkDedBeforePerc
			// 
			this.checkDedBeforePerc.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDedBeforePerc.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDedBeforePerc.Location = new System.Drawing.Point(19,109);
			this.checkDedBeforePerc.Name = "checkDedBeforePerc";
			this.checkDedBeforePerc.Size = new System.Drawing.Size(192,18);
			this.checkDedBeforePerc.TabIndex = 86;
			this.checkDedBeforePerc.Text = "Apply deductible before %";
			this.checkDedBeforePerc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDedBeforePerc.Click += new System.EventHandler(this.checkDedBeforePerc_Click);
			// 
			// labelPatInsPayAmt
			// 
			this.labelPatInsPayAmt.Location = new System.Drawing.Point(294,102);
			this.labelPatInsPayAmt.Name = "labelPatInsPayAmt";
			this.labelPatInsPayAmt.Size = new System.Drawing.Size(67,16);
			this.labelPatInsPayAmt.TabIndex = 87;
			this.labelPatInsPayAmt.Text = "-$200.00";
			this.labelPatInsPayAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelClaimBar
			// 
			this.panelClaimBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelClaimBar.Location = new System.Drawing.Point(301,146);
			this.panelClaimBar.Name = "panelClaimBar";
			this.panelClaimBar.Size = new System.Drawing.Size(65,1);
			this.panelClaimBar.TabIndex = 88;
			// 
			// labelInsInsPayAmt
			// 
			this.labelInsInsPayAmt.Location = new System.Drawing.Point(220,102);
			this.labelInsInsPayAmt.Name = "labelInsInsPayAmt";
			this.labelInsInsPayAmt.Size = new System.Drawing.Size(67,16);
			this.labelInsInsPayAmt.TabIndex = 89;
			this.labelInsInsPayAmt.Text = "$200.00";
			this.labelInsInsPayAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupClaim
			// 
			this.groupClaim.Controls.Add(this.labelAttachedToCheck);
			this.groupClaim.Controls.Add(this.labelNotInClaim);
			this.groupClaim.Controls.Add(this.radioClaim);
			this.groupClaim.Controls.Add(this.radioEstimate);
			this.groupClaim.Controls.Add(this.panelClaimExtras);
			this.groupClaim.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupClaim.Location = new System.Drawing.Point(14,240);
			this.groupClaim.Name = "groupClaim";
			this.groupClaim.Size = new System.Drawing.Size(460,309);
			this.groupClaim.TabIndex = 90;
			this.groupClaim.TabStop = false;
			this.groupClaim.Text = "Claim";
			// 
			// labelAttachedToCheck
			// 
			this.labelAttachedToCheck.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelAttachedToCheck.Location = new System.Drawing.Point(118,270);
			this.labelAttachedToCheck.Name = "labelAttachedToCheck";
			this.labelAttachedToCheck.Size = new System.Drawing.Size(333,29);
			this.labelAttachedToCheck.TabIndex = 27;
			this.labelAttachedToCheck.Text = "This is attached to an insurance check, so certain changes are not allowed.";
			// 
			// radioClaim
			// 
			this.radioClaim.AutoCheck = false;
			this.radioClaim.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioClaim.Location = new System.Drawing.Point(100,33);
			this.radioClaim.Name = "radioClaim";
			this.radioClaim.Size = new System.Drawing.Size(353,18);
			this.radioClaim.TabIndex = 1;
			this.radioClaim.Text = "This is part of a claim.";
			this.radioClaim.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			// 
			// radioEstimate
			// 
			this.radioEstimate.AutoCheck = false;
			this.radioEstimate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEstimate.Location = new System.Drawing.Point(100,10);
			this.radioEstimate.Name = "radioEstimate";
			this.radioEstimate.Size = new System.Drawing.Size(352,22);
			this.radioEstimate.TabIndex = 0;
			this.radioEstimate.Text = "This is an estimate only. It has not been attached to a claim.";
			// 
			// panelClaimExtras
			// 
			this.panelClaimExtras.Controls.Add(this.labelRemarks);
			this.panelClaimExtras.Controls.Add(this.textRemarks);
			this.panelClaimExtras.Controls.Add(this.labelCodeSent);
			this.panelClaimExtras.Controls.Add(this.textCodeSent);
			this.panelClaimExtras.Controls.Add(this.labelFeeBilled);
			this.panelClaimExtras.Controls.Add(this.textFeeBilled);
			this.panelClaimExtras.Location = new System.Drawing.Point(4,54);
			this.panelClaimExtras.Name = "panelClaimExtras";
			this.panelClaimExtras.Size = new System.Drawing.Size(438,188);
			this.panelClaimExtras.TabIndex = 97;
			// 
			// labelPatOverrideInsEstt
			// 
			this.labelPatOverrideInsEstt.Location = new System.Drawing.Point(291,256);
			this.labelPatOverrideInsEstt.Name = "labelPatOverrideInsEstt";
			this.labelPatOverrideInsEstt.Size = new System.Drawing.Size(67,16);
			this.labelPatOverrideInsEstt.TabIndex = 91;
			this.labelPatOverrideInsEstt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelEstimateInfo
			// 
			this.panelEstimateInfo.Controls.Add(this.labelCarrierAllowedAmt);
			this.panelEstimateInfo.Controls.Add(this.textCarrierAllowedAmt);
			this.panelEstimateInfo.Controls.Add(this.labelInsCopayOverride);
			this.panelEstimateInfo.Controls.Add(this.textCopayOverride);
			this.panelEstimateInfo.Controls.Add(this.labelCopayOverride);
			this.panelEstimateInfo.Controls.Add(this.labelInsBaseEst);
			this.panelEstimateInfo.Controls.Add(this.labelPatBaseEstt);
			this.panelEstimateInfo.Controls.Add(this.label5);
			this.panelEstimateInfo.Controls.Add(this.textBaseEst);
			this.panelEstimateInfo.Controls.Add(this.labelInsOverrideInsEst);
			this.panelEstimateInfo.Controls.Add(this.labelInsCopay);
			this.panelEstimateInfo.Controls.Add(this.labelPatPortion);
			this.panelEstimateInfo.Controls.Add(this.labelInsDedBeforePerc);
			this.panelEstimateInfo.Controls.Add(this.labelFee);
			this.panelEstimateInfo.Controls.Add(this.labelInsAllowed);
			this.panelEstimateInfo.Controls.Add(this.labelPatFee);
			this.panelEstimateInfo.Controls.Add(this.checkDedBeforePerc);
			this.panelEstimateInfo.Controls.Add(this.textDedBeforePerc);
			this.panelEstimateInfo.Controls.Add(this.labelPatOverrideInsEstt);
			this.panelEstimateInfo.Controls.Add(this.labelDedBeforePerc);
			this.panelEstimateInfo.Controls.Add(this.textAllowedAmt);
			this.panelEstimateInfo.Controls.Add(this.label3);
			this.panelEstimateInfo.Controls.Add(this.label4);
			this.panelEstimateInfo.Controls.Add(this.textPercentage);
			this.panelEstimateInfo.Controls.Add(this.textCopayAmt);
			this.panelEstimateInfo.Controls.Add(this.labelInsPercentage);
			this.panelEstimateInfo.Controls.Add(this.labelCopayAmt);
			this.panelEstimateInfo.Controls.Add(this.textOverrideInsEst);
			this.panelEstimateInfo.Controls.Add(this.labelInsPercentOverride);
			this.panelEstimateInfo.Controls.Add(this.textFee);
			this.panelEstimateInfo.Controls.Add(this.label12);
			this.panelEstimateInfo.Controls.Add(this.label13);
			this.panelEstimateInfo.Controls.Add(this.textPercentOverride);
			this.panelEstimateInfo.Controls.Add(this.label14);
			this.panelEstimateInfo.Controls.Add(this.labelInsFee);
			this.panelEstimateInfo.Location = new System.Drawing.Point(487,61);
			this.panelEstimateInfo.Name = "panelEstimateInfo";
			this.panelEstimateInfo.Size = new System.Drawing.Size(370,307);
			this.panelEstimateInfo.TabIndex = 94;
			// 
			// labelCarrierAllowedAmt
			// 
			this.labelCarrierAllowedAmt.Location = new System.Drawing.Point(8,70);
			this.labelCarrierAllowedAmt.Name = "labelCarrierAllowedAmt";
			this.labelCarrierAllowedAmt.Size = new System.Drawing.Size(127,14);
			this.labelCarrierAllowedAmt.TabIndex = 101;
			this.labelCarrierAllowedAmt.Text = "Carrier Allowed Amt";
			this.labelCarrierAllowedAmt.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCarrierAllowedAmt
			// 
			this.textCarrierAllowedAmt.Location = new System.Drawing.Point(139,67);
			this.textCarrierAllowedAmt.Name = "textCarrierAllowedAmt";
			this.textCarrierAllowedAmt.ReadOnly = true;
			this.textCarrierAllowedAmt.Size = new System.Drawing.Size(77,20);
			this.textCarrierAllowedAmt.TabIndex = 102;
			this.textCarrierAllowedAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelInsCopayOverride
			// 
			this.labelInsCopayOverride.Location = new System.Drawing.Point(217,171);
			this.labelInsCopayOverride.Name = "labelInsCopayOverride";
			this.labelInsCopayOverride.Size = new System.Drawing.Size(67,16);
			this.labelInsCopayOverride.TabIndex = 100;
			this.labelInsCopayOverride.Text = "-$10.00";
			this.labelInsCopayOverride.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCopayOverride
			// 
			this.labelCopayOverride.Location = new System.Drawing.Point(3,170);
			this.labelCopayOverride.Name = "labelCopayOverride";
			this.labelCopayOverride.Size = new System.Drawing.Size(133,17);
			this.labelCopayOverride.TabIndex = 99;
			this.labelCopayOverride.Text = "Patient Copay Override";
			this.labelCopayOverride.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelInsBaseEst
			// 
			this.labelInsBaseEst.Location = new System.Drawing.Point(217,234);
			this.labelInsBaseEst.Name = "labelInsBaseEst";
			this.labelInsBaseEst.Size = new System.Drawing.Size(67,16);
			this.labelInsBaseEst.TabIndex = 96;
			this.labelInsBaseEst.Text = "= $400.00";
			this.labelInsBaseEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPatBaseEstt
			// 
			this.labelPatBaseEstt.Location = new System.Drawing.Point(291,234);
			this.labelPatBaseEstt.Name = "labelPatBaseEstt";
			this.labelPatBaseEstt.Size = new System.Drawing.Size(67,16);
			this.labelPatBaseEstt.TabIndex = 97;
			this.labelPatBaseEstt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(30,234);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(107,14);
			this.label5.TabIndex = 94;
			this.label5.Text = "Base Estimate";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBaseEst
			// 
			this.textBaseEst.Location = new System.Drawing.Point(139,232);
			this.textBaseEst.Name = "textBaseEst";
			this.textBaseEst.ReadOnly = true;
			this.textBaseEst.Size = new System.Drawing.Size(77,20);
			this.textBaseEst.TabIndex = 95;
			this.textBaseEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// groupClaimInfo
			// 
			this.groupClaimInfo.Controls.Add(this.textWriteOff);
			this.groupClaimInfo.Controls.Add(this.labelPatInsPayAmt);
			this.groupClaimInfo.Controls.Add(this.labelInsInsPayEst);
			this.groupClaimInfo.Controls.Add(this.textOverAnnualMax);
			this.groupClaimInfo.Controls.Add(this.textInsPayEst);
			this.groupClaimInfo.Controls.Add(this.labelOverAnnualMax);
			this.groupClaimInfo.Controls.Add(this.labelInsOverAnnualMax);
			this.groupClaimInfo.Controls.Add(this.labelPatTotal);
			this.groupClaimInfo.Controls.Add(this.labelInsPayEst);
			this.groupClaimInfo.Controls.Add(this.labelInsPayAmt);
			this.groupClaimInfo.Controls.Add(this.textInsPayAmt);
			this.groupClaimInfo.Controls.Add(this.labelPaidOtherIns);
			this.groupClaimInfo.Controls.Add(this.labelInsDedApplied);
			this.groupClaimInfo.Controls.Add(this.textDedApplied);
			this.groupClaimInfo.Controls.Add(this.labelInsInsPayAmt);
			this.groupClaimInfo.Controls.Add(this.labelDedApplied);
			this.groupClaimInfo.Controls.Add(this.labelInsPaidOtherIns);
			this.groupClaimInfo.Controls.Add(this.textPaidOtherIns);
			this.groupClaimInfo.Controls.Add(this.panelClaimBar);
			this.groupClaimInfo.Controls.Add(this.labelWriteOff);
			this.groupClaimInfo.Controls.Add(this.labelPatWriteOff);
			this.groupClaimInfo.Controls.Add(this.labelPatInsPayEst);
			this.groupClaimInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupClaimInfo.Location = new System.Drawing.Point(483,336);
			this.groupClaimInfo.Name = "groupClaimInfo";
			this.groupClaimInfo.Size = new System.Drawing.Size(375,213);
			this.groupClaimInfo.TabIndex = 0;
			this.groupClaimInfo.TabStop = false;
			this.groupClaimInfo.Text = "Claim Info";
			// 
			// labelProcDate
			// 
			this.labelProcDate.Location = new System.Drawing.Point(6,202);
			this.labelProcDate.Name = "labelProcDate";
			this.labelProcDate.Size = new System.Drawing.Size(126,17);
			this.labelProcDate.TabIndex = 96;
			this.labelProcDate.Text = "Procedure Date";
			this.labelProcDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelDateEntry
			// 
			this.labelDateEntry.Location = new System.Drawing.Point(6,159);
			this.labelDateEntry.Name = "labelDateEntry";
			this.labelDateEntry.Size = new System.Drawing.Size(125,17);
			this.labelDateEntry.TabIndex = 99;
			this.labelDateEntry.Text = "Pay Entry Date";
			this.labelDateEntry.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(133,155);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(83,20);
			this.textDateEntry.TabIndex = 100;
			// 
			// butUpdateAllowed
			// 
			this.butUpdateAllowed.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butUpdateAllowed.Autosize = true;
			this.butUpdateAllowed.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUpdateAllowed.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUpdateAllowed.Location = new System.Drawing.Point(442,125);
			this.butUpdateAllowed.Name = "butUpdateAllowed";
			this.butUpdateAllowed.Size = new System.Drawing.Size(75,26);
			this.butUpdateAllowed.TabIndex = 98;
			this.butUpdateAllowed.Text = "Update";
			this.butUpdateAllowed.Click += new System.EventHandler(this.butUpdateAllowed_Click);
			// 
			// textProcDate
			// 
			this.textProcDate.Location = new System.Drawing.Point(133,198);
			this.textProcDate.Name = "textProcDate";
			this.textProcDate.Size = new System.Drawing.Size(83,20);
			this.textProcDate.TabIndex = 97;
			// 
			// textFeeBilled
			// 
			this.textFeeBilled.Location = new System.Drawing.Point(129,28);
			this.textFeeBilled.Name = "textFeeBilled";
			this.textFeeBilled.Size = new System.Drawing.Size(77,20);
			this.textFeeBilled.TabIndex = 72;
			// 
			// textWriteOff
			// 
			this.textWriteOff.Location = new System.Drawing.Point(144,121);
			this.textWriteOff.Name = "textWriteOff";
			this.textWriteOff.Size = new System.Drawing.Size(77,20);
			this.textWriteOff.TabIndex = 18;
			this.textWriteOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textWriteOff.Leave += new System.EventHandler(this.textWriteOff_Leave);
			// 
			// textOverAnnualMax
			// 
			this.textOverAnnualMax.Location = new System.Drawing.Point(144,57);
			this.textOverAnnualMax.Name = "textOverAnnualMax";
			this.textOverAnnualMax.Size = new System.Drawing.Size(77,20);
			this.textOverAnnualMax.TabIndex = 61;
			this.textOverAnnualMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textOverAnnualMax.Leave += new System.EventHandler(this.textOverAnnualMax_Leave);
			// 
			// textInsPayEst
			// 
			this.textInsPayEst.Location = new System.Drawing.Point(144,78);
			this.textInsPayEst.Name = "textInsPayEst";
			this.textInsPayEst.Size = new System.Drawing.Size(77,20);
			this.textInsPayEst.TabIndex = 20;
			this.textInsPayEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textInsPayEst.Leave += new System.EventHandler(this.textInsPayEst_Leave);
			// 
			// textInsPayAmt
			// 
			this.textInsPayAmt.Location = new System.Drawing.Point(144,99);
			this.textInsPayAmt.Name = "textInsPayAmt";
			this.textInsPayAmt.Size = new System.Drawing.Size(77,20);
			this.textInsPayAmt.TabIndex = 0;
			this.textInsPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textInsPayAmt.Leave += new System.EventHandler(this.textInsPayAmt_Leave);
			// 
			// textDedApplied
			// 
			this.textDedApplied.Location = new System.Drawing.Point(144,15);
			this.textDedApplied.Name = "textDedApplied";
			this.textDedApplied.Size = new System.Drawing.Size(77,20);
			this.textDedApplied.TabIndex = 75;
			this.textDedApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textDedApplied.Leave += new System.EventHandler(this.textDedApplied_Leave);
			// 
			// textPaidOtherIns
			// 
			this.textPaidOtherIns.Location = new System.Drawing.Point(144,36);
			this.textPaidOtherIns.Name = "textPaidOtherIns";
			this.textPaidOtherIns.Size = new System.Drawing.Size(77,20);
			this.textPaidOtherIns.TabIndex = 78;
			this.textPaidOtherIns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textPaidOtherIns.Leave += new System.EventHandler(this.textPaidOtherIns_Leave);
			// 
			// textDateCP
			// 
			this.textDateCP.Location = new System.Drawing.Point(133,176);
			this.textDateCP.Name = "textDateCP";
			this.textDateCP.Size = new System.Drawing.Size(83,20);
			this.textDateCP.TabIndex = 66;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.Image = ((System.Drawing.Image)(resources.GetObject("butDelete.Image")));
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(16,568);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(87,26);
			this.butDelete.TabIndex = 3;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(780,572);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.Location = new System.Drawing.Point(680,572);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textCopayOverride
			// 
			this.textCopayOverride.Location = new System.Drawing.Point(139,169);
			this.textCopayOverride.Name = "textCopayOverride";
			this.textCopayOverride.Size = new System.Drawing.Size(77,20);
			this.textCopayOverride.TabIndex = 98;
			this.textCopayOverride.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textCopayOverride.Leave += new System.EventHandler(this.textCopayOverride_Leave);
			this.textCopayOverride.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCopayOverride_KeyUp);
			// 
			// textDedBeforePerc
			// 
			this.textDedBeforePerc.Location = new System.Drawing.Point(139,127);
			this.textDedBeforePerc.Name = "textDedBeforePerc";
			this.textDedBeforePerc.Size = new System.Drawing.Size(77,20);
			this.textDedBeforePerc.TabIndex = 10;
			this.textDedBeforePerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textDedBeforePerc.Leave += new System.EventHandler(this.textDedBeforePerc_Leave);
			// 
			// textAllowedAmt
			// 
			this.textAllowedAmt.Location = new System.Drawing.Point(139,88);
			this.textAllowedAmt.Name = "textAllowedAmt";
			this.textAllowedAmt.Size = new System.Drawing.Size(77,20);
			this.textAllowedAmt.TabIndex = 30;
			this.textAllowedAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textAllowedAmt.Leave += new System.EventHandler(this.textAllowedAmt_Leave);
			// 
			// textCopayAmt
			// 
			this.textCopayAmt.Location = new System.Drawing.Point(139,148);
			this.textCopayAmt.Name = "textCopayAmt";
			this.textCopayAmt.ReadOnly = true;
			this.textCopayAmt.Size = new System.Drawing.Size(77,20);
			this.textCopayAmt.TabIndex = 36;
			this.textCopayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textOverrideInsEst
			// 
			this.textOverrideInsEst.Location = new System.Drawing.Point(139,253);
			this.textOverrideInsEst.Name = "textOverrideInsEst";
			this.textOverrideInsEst.Size = new System.Drawing.Size(77,20);
			this.textOverrideInsEst.TabIndex = 38;
			this.textOverrideInsEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textOverrideInsEst.Leave += new System.EventHandler(this.textOverrideInsEst_Leave);
			// 
			// textPercentOverride
			// 
			this.textPercentOverride.Location = new System.Drawing.Point(168,211);
			this.textPercentOverride.MaxVal = 255;
			this.textPercentOverride.MinVal = 0;
			this.textPercentOverride.Name = "textPercentOverride";
			this.textPercentOverride.Size = new System.Drawing.Size(48,20);
			this.textPercentOverride.TabIndex = 45;
			this.textPercentOverride.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textPercentOverride.Leave += new System.EventHandler(this.textPercentOverride_Leave);
			// 
			// FormClaimProc
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(889,618);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.labelDateEntry);
			this.Controls.Add(this.butUpdateAllowed);
			this.Controls.Add(this.textProcDate);
			this.Controls.Add(this.labelProcDate);
			this.Controls.Add(this.groupClaim);
			this.Controls.Add(this.groupClaimInfo);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.textDateCP);
			this.Controls.Add(this.textInsPlan);
			this.Controls.Add(this.listProv);
			this.Controls.Add(this.label30);
			this.Controls.Add(this.label29);
			this.Controls.Add(this.label28);
			this.Controls.Add(this.checkNoBillIns);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.listStatus);
			this.Controls.Add(this.panelEstimateInfo);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClaimProc";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Claim Procedure";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClaimProc_Closing);
			this.Load += new System.EventHandler(this.FormClaimProcEdit_Load);
			this.groupClaim.ResumeLayout(false);
			this.panelClaimExtras.ResumeLayout(false);
			this.panelClaimExtras.PerformLayout();
			this.panelEstimateInfo.ResumeLayout(false);
			this.panelEstimateInfo.PerformLayout();
			this.groupClaimInfo.ResumeLayout(false);
			this.groupClaimInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClaimProcEdit_Load(object sender, System.EventArgs e) {
			if(NoPermission){
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
			//if(ClaimProcCur.PlanNum>0){
			//	InsPlans.GetCur(ClaimProcCur.PlanNum,PlanList);
			//}
			textInsPlan.Text=InsPlans.GetDescript(ClaimProcCur.PlanNum,FamCur,PlanList);
			checkNoBillIns.Checked=ClaimProcCur.NoBillIns;
			if(ClaimProcCur.ClaimPaymentNum>0){//attached to ins check
				textDateCP.ReadOnly=true;//DateCP always the same as the payment date.
			}//otherwise, we do need to allow editing so that Writeoff dates can be changed.
			if(ClaimProcCur.ProcNum==0){//total payment for a claim
				IsProc=false;
				textDescription.Text="Total Payment";
				textProcDate.ReadOnly=false;
			}
			else{
				IsProc=true;
				if(proc==null){
					ProcFee=Procedures.GetProcFee(ClaimProcCur.ProcNum);
					ProcADACode=Procedures.GetADA(ClaimProcCur.ProcNum);
				}
				else{
					ProcFee=proc.ProcFee;
					ProcADACode=proc.ADACode;
				}
				textDescription.Text=ProcedureCodes.GetProcCode(ProcADACode).Descript;
				textProcDate.ReadOnly=true;//user not allowed to edit ProcDate unless it's for a total payment
			}
			if(ClaimProcCur.ClaimNum>0){//attached to claim
				radioClaim.Checked=true;
				checkNoBillIns.Enabled=false;
				if(!IsInClaim){//accessing it from the Procedure window
					textCodeSent.ReadOnly=true;
					textFeeBilled.ReadOnly=true;
					labelNotInClaim.Visible=true;
					textDedApplied.ReadOnly=true;
					textDedBeforePerc.ReadOnly=true;
					textPaidOtherIns.ReadOnly=true;
					textOverAnnualMax.ReadOnly=true;
					textInsPayEst.ReadOnly=true;
					textInsPayAmt.ReadOnly=true;
					textWriteOff.ReadOnly=true;
					//butRecalc.Enabled=false;
				}
				else{//not from the procedure window
					labelNotInClaim.Visible=false;
				}
				groupClaimInfo.Visible=true;
				if(ClaimProcCur.ProcNum==0){//if a total entry rather than by proc
					panelEstimateInfo.Visible=false;
					//foreach(System.Windows.Forms.Control control in groupClaimInfo.Controls){
					//	control.Visible=false;
					//}
					labelInsDedApplied.Visible=false;
					labelInsPaidOtherIns.Visible=false;
					labelInsOverAnnualMax.Visible=false;
					labelInsInsPayEst.Visible=false;
					labelInsInsPayAmt.Visible=false;
					labelPatInsPayEst.Visible=false;
					labelPatInsPayAmt.Visible=false;
					labelPatWriteOff.Visible=false;
					panelClaimBar.Visible=false;
					labelPatTotal.Visible=false;
					labelInsPayAmt.Font=new Font(labelInsPayAmt.Font,FontStyle.Bold);
					labelProcDate.Visible=false;
					textProcDate.Visible=false;
					labelCodeSent.Visible=false;
					textCodeSent.Visible=false;
					labelFeeBilled.Visible=false;
					textFeeBilled.Visible=false;
				}
				else if(ClaimProcCur.Status==ClaimProcStatus.Received){
					labelInsPayAmt.Font=new Font(labelInsPayAmt.Font,FontStyle.Bold);
				}
				//butOK.Enabled=false;
				//butDelete.Enabled=false;
				//MessageBox.Show(panelEstimateInfo.Visible.ToString());
			}
			else if(ClaimProcCur.PlanNum>0
				&& (ClaimProcCur.Status==ClaimProcStatus.CapEstimate
				|| ClaimProcCur.Status==ClaimProcStatus.CapComplete))
			{
				//InsPlans.Cur.PlanType=="c"){//capitation proc,whether Estimate or CapComplete,never billed to ins
				foreach(System.Windows.Forms.Control control in panelEstimateInfo.Controls){
					control.Visible=false;
				}
				foreach(System.Windows.Forms.Control control in groupClaimInfo.Controls){
					control.Visible=false;
				}
				groupClaimInfo.Text="";
				labelFee.Visible=true;
				textFee.Visible=true;
				labelPatPortion.Visible=true;
				labelPatFee.Visible=true;
				labelCopayAmt.Visible=true;
				textCopayAmt.Visible=true;
				labelCopayOverride.Visible=true;
				textCopayOverride.Visible=true;
				labelWriteOff.Visible=true;
				textWriteOff.Visible=true;
				labelPatWriteOff.Visible=true;
				panelClaimBar.Visible=true;
				labelPatTotal.Visible=true;
				groupClaim.Visible=false;
				labelNotInClaim.Visible=false;
				//checkNoBillIns.Visible=false;
			}
			else{//estimate
				//groupClaimInfo.Text="";
				labelInsPayAmt.Visible=false;
				textInsPayAmt.Visible=false;
				labelInsInsPayAmt.Visible=false;
				labelPatInsPayAmt.Visible=false;
				labelWriteOff.Visible=false;
				textWriteOff.Visible=false;
				labelPatWriteOff.Visible=false;
				radioEstimate.Checked=true;
				labelNotInClaim.Visible=false;
				textDedApplied.ReadOnly=true;
				textPaidOtherIns.ReadOnly=true;
				textOverAnnualMax.ReadOnly=true;
				textInsPayEst.ReadOnly=true;
				//groupClaimInfo.Visible=false;
				panelClaimExtras.Visible=false;
			}
			listStatus.Items.Clear();
			listStatus.Items.Add(Lan.g(this,"Estimate"));
			listStatus.Items.Add(Lan.g(this,"Not Received"));
			listStatus.Items.Add(Lan.g(this,"Received"));
			listStatus.Items.Add(Lan.g(this,"PreAuthorization"));
			listStatus.Items.Add(Lan.g(this,"Supplemental"));
			listStatus.Items.Add(Lan.g(this,"CapClaim"));
			listStatus.Items.Add(Lan.g(this,"CapEstimate"));
			listStatus.Items.Add(Lan.g(this,"CapComplete"));
			SetListStatus(ClaimProcCur.Status);
			if(ClaimProcCur.Status==ClaimProcStatus.Received || ClaimProcCur.Status==ClaimProcStatus.Supplemental){
				labelDateEntry.Visible=true;
				textDateEntry.Visible=true;
			}
			else{
				labelDateEntry.Visible=false;
				textDateEntry.Visible=false;
			}
			listProv.Items.Clear();
			for(int i=0;i<Providers.List.Length;i++){
				listProv.Items.Add(Providers.List[i].Abbr);
				if(ClaimProcCur.ProvNum==Providers.List[i].ProvNum){
					listProv.SelectedIndex=i;
				}
			}
			//this is not used, because the provider might simply be hidden. See bottom of page.
			//if(listProv.SelectedIndex==-1){
			//	listProv.SelectedIndex=0;//there should always be a provider
			//}
			textDateEntry.Text=ClaimProcCur.DateEntry.ToShortDateString();
			if(ClaimProcCur.ProcDate.Year<1880){
				textProcDate.Text="";
			}
			else{
				textProcDate.Text=ClaimProcCur.ProcDate.ToShortDateString();
			}
			if(ClaimProcCur.DateCP.Year<1880){
				textDateCP.Text="";
			}
			else{
				textDateCP.Text=ClaimProcCur.DateCP.ToShortDateString();
			}
			textCodeSent.Text=ClaimProcCur.CodeSent;
			textFeeBilled.Text=ClaimProcCur.FeeBilled.ToString("n");
			textRemarks.Text=ClaimProcCur.Remarks;
			if(ClaimProcCur.ClaimPaymentNum>0){
				textInsPayAmt.ReadOnly=true;
				//listStatus.Enabled=false;//this is handled in the mousedown event
				butDelete.Enabled=false;
			}
			else{
				labelAttachedToCheck.Visible=false;
			}
			FillInitialAmounts();
			ComputeAmounts();
			//MessageBox.Show(panelEstimateInfo.Visible.ToString());
		}


		private void SetListStatus(ClaimProcStatus status){
			switch(status){
				case ClaimProcStatus.Estimate:
					listStatus.SelectedIndex=0;
					break;
				case ClaimProcStatus.NotReceived:
					listStatus.SelectedIndex=1;
					break;
				case ClaimProcStatus.Received:
					listStatus.SelectedIndex=2;
					break;
				case ClaimProcStatus.Preauth:
					listStatus.SelectedIndex=3;
					break;
				//adjustments have a completely different user interface. Cannot access from here.
				case ClaimProcStatus.Supplemental:
					listStatus.SelectedIndex=4;
					break;
				case ClaimProcStatus.CapClaim:
					listStatus.SelectedIndex=5;
					break;
				case ClaimProcStatus.CapEstimate:
					listStatus.SelectedIndex=6;
					break;
				case ClaimProcStatus.CapComplete:
					listStatus.SelectedIndex=7;
					break;
			}
		}

		///<summary>All text boxes will be blank before this is run.  It is only run once.</summary>
		private void FillInitialAmounts(){
			if(IsProc){
				textFee.Text=ProcFee.ToString("f");
			}
			else{
				//except this textbox starts with a value just as a placeholder
				textFee.Text="";
			}
			FillAllowed();
			if(ClaimProcCur.AllowedAmt!=-1){
				textAllowedAmt.Text=ClaimProcCur.AllowedAmt.ToString("f");
			}
			checkDedBeforePerc.Checked=ClaimProcCur.DedBeforePerc;
			//set both deductible fields to the deductible value. Visibility will be set later.
			//if(checkDedBeforePerc.Checked)
			textDedBeforePerc.Text=ClaimProcCur.DedApplied.ToString("f");
			//else
			textDedApplied.Text=ClaimProcCur.DedApplied.ToString("f");
			if(ClaimProcCur.CopayAmt!=-1){
				textCopayAmt.Text=ClaimProcCur.CopayAmt.ToString("f");
			}
			if(ClaimProcCur.CopayOverride!=-1){
				textCopayOverride.Text=ClaimProcCur.CopayOverride.ToString("f");
			}
			if(ClaimProcCur.Percentage!=-1){
				textPercentage.Text=ClaimProcCur.Percentage.ToString();
			}
			if(ClaimProcCur.PercentOverride!=-1){
				textPercentOverride.Text=ClaimProcCur.PercentOverride.ToString();
			}
			textBaseEst.Text=ClaimProcCur.BaseEst.ToString("f");
			if(ClaimProcCur.OverrideInsEst!=-1){
				textOverrideInsEst.Text=ClaimProcCur.OverrideInsEst.ToString("f");
			}
			//textDedApplied already handled
			if(ClaimProcCur.PaidOtherIns!=-1){
				textPaidOtherIns.Text=ClaimProcCur.PaidOtherIns.ToString("f");
			}
			if(ClaimProcCur.OverAnnualMax!=-1){
				textOverAnnualMax.Text=ClaimProcCur.OverAnnualMax.ToString("f");
			}
			textInsPayEst.Text=ClaimProcCur.InsPayEst.ToString("f");
			textInsPayAmt.Text=ClaimProcCur.InsPayAmt.ToString("f");
			textWriteOff.Text=ClaimProcCur.WriteOff.ToString("f");
		}

		///<summary>Fills the carrier allowed amount.  Called from FillInitialAmounts and from butUpdateAllowed_Click</summary>
		private void FillAllowed(){
			if(IsProc){
				CarrierAllowedAmt=InsPlans.GetAllowed(ProcADACode,ClaimProcCur.PlanNum,PlanList);
				if(CarrierAllowedAmt==-1){
					textCarrierAllowedAmt.Text="";
				}
				else{
					textCarrierAllowedAmt.Text=CarrierAllowedAmt.ToString("f");
				}
			}
			else{
				textCarrierAllowedAmt.Text="";
			}
		}

		private void butUpdateAllowed_Click(object sender, System.EventArgs e) {
			InsPlan plan=InsPlans.GetPlan(ClaimProcCur.PlanNum,PlanList);
			if(plan==null){
				//this should never happen
			}
			if(plan.AllowedFeeSched==0){
				MsgBox.Show(this,"Allowed fee schedule has not been set for this insurance plan");
				return;
			}
			int feeOrder=DefB.GetOrder(DefCat.FeeSchedNames,plan.AllowedFeeSched);
			if(feeOrder==-1){
				MsgBox.Show(this,"Allowed fee schedule is hidden, so no changes can be made.");
				return;
			}
			Fee FeeCur=Fees.GetFeeByOrder(ProcADACode,feeOrder);
			FormFeeEdit FormFE=new FormFeeEdit();
			if(FeeCur==null){
				FeeCur=new Fee();
				FeeCur.ADACode=ProcADACode;
				FeeCur.FeeSched=plan.AllowedFeeSched;
				Fees.Insert(FeeCur);
				FormFE.IsNew=true;
			}
			FormFE.FeeCur=FeeCur;
			FormFE.ShowDialog();
			if(FormFE.DialogResult==DialogResult.OK){
				Fees.Refresh();
				DataValid.SetInvalid(InvalidTypes.Fees);
			}
			FillAllowed();
		}

		private void ComputeAmounts(){
			if(!AllAreValid()){
				return;
			}
			if(checkNoBillIns.Checked){
				if(ClaimProcCur.Status==ClaimProcStatus.CapEstimate
					|| ClaimProcCur.Status==ClaimProcStatus.CapComplete)
				{
					panelEstimateInfo.Visible=true;
					groupClaimInfo.Visible=true;
				}
				else{
					panelEstimateInfo.Visible=false;
					groupClaimInfo.Visible=false;
					return;
				}
			}
			else{
				if(ClaimProcCur.ProcNum!=0){//if a total payment, then this protects panel from inadvertently
						//being set visible again.  All other situations, it's based on NoBillIns
					panelEstimateInfo.Visible=true;
				}
				groupClaimInfo.Visible=true;
			}
			//all labels must have a value set.
			//double fee=0;
			double totalEstimate=0;
			if(IsProc){
				//fee=ProcFee;
				labelPatFee.Text=ProcFee.ToString("c");
			}
			else{
				labelPatFee.Text="";
			}
			double allowed=-1;
			if(textAllowedAmt.Text==""){//Use fee.
				labelInsFee.Text=ProcFee.ToString("c");
				labelInsAllowed.Text="";
				totalEstimate=ProcFee;
			}
			else{//Override the fee
				allowed=PIn.PDouble(textAllowedAmt.Text);
				labelInsFee.Text="";
				labelInsAllowed.Text=allowed.ToString("c");
				totalEstimate=allowed;
			}
			double dedApplied=-1;
			if(ClaimProcCur.Status==ClaimProcStatus.CapEstimate
				|| ClaimProcCur.Status==ClaimProcStatus.CapComplete){
				//then don't worry about deductibles
			}
			else if(checkDedBeforePerc.Checked){//deductible before %
				labelDedBeforePerc.Visible=true;
				textDedBeforePerc.Visible=true;
				labelDedApplied.Visible=false;
				textDedApplied.Visible=false;
				labelInsDedApplied.Text="";
				if(textDedBeforePerc.Text==""){
					labelInsDedBeforePerc.Text="";
				}
				else{
					dedApplied=PIn.PDouble(textDedBeforePerc.Text);
					if(dedApplied==0)
						labelInsDedBeforePerc.Text="";
					else
						labelInsDedBeforePerc.Text="- "+dedApplied.ToString("c");
					totalEstimate-=dedApplied;
				}
			}
			else{//deductible after %
				labelDedBeforePerc.Visible=false;
				textDedBeforePerc.Visible=false;
				labelDedApplied.Visible=true;
				textDedApplied.Visible=true;
				labelInsDedBeforePerc.Text="";
				if(textDedApplied.Text==""){
					labelInsDedApplied.Text="";
				}
				else{
					dedApplied=PIn.PDouble(textDedApplied.Text);
					if(dedApplied==0)
						labelInsDedApplied.Text="";
					else
						labelInsDedApplied.Text="- "+dedApplied.ToString("c");
					//does not affect ins estimate until later in sequence
				}
			}
			double copay=0;//never gets set to -1
			if(textCopayOverride.Text==""){//no override, so only show copayAmt
				if(ClaimProcCur.CopayAmt!=-1){
					copay=ClaimProcCur.CopayAmt;
				}
				labelInsCopayOverride.Text="";
				if(copay==0)
					labelInsCopay.Text="";
				else
					labelInsCopay.Text="- "+copay.ToString("c");
			}
			else{//override, so only show copayOverride
				copay=PIn.PDouble(textCopayOverride.Text);
				labelInsCopay.Text="";
				if(copay==0)
					labelInsCopayOverride.Text="";
				else
					labelInsCopayOverride.Text="- "+copay.ToString("c");
			}
			totalEstimate-=copay;
			int percentage=ClaimProcCur.Percentage;
			if(percentage==-1){//not sure when this would happen
				labelInsPercentage.Text="";
			}
			else{
				labelInsPercentage.Text="x  "+percentage.ToString()+"%";
				if(textPercentOverride.Text==""){
					totalEstimate=totalEstimate*(double)percentage/100;
				}
			}
			double percentOverride=-1;
			if(textPercentOverride.Text==""){
				labelInsPercentOverride.Text="";
			}
			else{
				percentOverride=PIn.PInt(textPercentOverride.Text);
				labelInsPercentOverride.Text="x  "+percentOverride.ToString()+"%";
				labelInsPercentage.Text="";
				totalEstimate=totalEstimate*(double)percentOverride/100;
			}
			double overrideInsEst=-1;
			double insPayEst=0;
			textBaseEst.Text=totalEstimate.ToString("f");
			if(textOverrideInsEst.Text==""){//use calculated estimate
				insPayEst=totalEstimate;
				labelInsBaseEst.Text="= "+totalEstimate.ToString("c");
				//labelPatBaseEst.Text="- "+totalEstimate.ToString("c");
				labelInsOverrideInsEst.Text="";
				//labelPatOverrideInsEst.Text="";
			}
			else{//override
				overrideInsEst=PIn.PDouble(textOverrideInsEst.Text);
				insPayEst=overrideInsEst;
				labelInsBaseEst.Text="";
				//labelPatBaseEst.Text="";
				labelInsOverrideInsEst.Text="= "+overrideInsEst.ToString("c");
				//labelPatOverrideInsEst.Text="- "+overrideInsEst.ToString("c");
			}
			//CLAIM INFO------------------------------------------------------------------------
			//if(listStatus.SelectedIndex==0){//estimate
			//	groupClaimInfo.Visible=false;
			//	labelPatEstimate.Text="= "+(ProcFee-insPayEst).ToString("c");
			//	return;
			//}
			//labelPatEstimate.Text="";
			//labelPatOverrideInsEst.Text="";
			groupClaimInfo.Visible=true;
			if(!checkDedBeforePerc.Checked && dedApplied!=-1){//ded applied after percentage, and a value entered
				//label already handled
				insPayEst-=dedApplied;
			}
			double paidOtherIns=-1;
			if(textPaidOtherIns.Text==""){
				labelInsPaidOtherIns.Text="";
			}
			else{
				paidOtherIns=PIn.PDouble(textPaidOtherIns.Text);
				if(paidOtherIns==0)
					labelInsPaidOtherIns.Text="";
				else
				labelInsPaidOtherIns.Text=" -"+paidOtherIns.ToString("c");
				insPayEst-=paidOtherIns;
			}
			double overAnnualMax=-1;
			if(textOverAnnualMax.Text==""){
				labelInsOverAnnualMax.Text="";
			}
			else{
				overAnnualMax=PIn.PDouble(textOverAnnualMax.Text);
				if(overAnnualMax==0)
					labelInsOverAnnualMax.Text="";
				else
					labelInsOverAnnualMax.Text="- "+overAnnualMax.ToString("c");
				insPayEst-=overAnnualMax;
			}
			if(ClaimProcCur.Status==ClaimProcStatus.CapEstimate
				|| ClaimProcCur.Status==ClaimProcStatus.CapComplete){
				insPayEst=0;
				textInsPayEst.Text="0";
			}		
			else if(textInsPayEst.Text==""){//use calculated insPayEst
				//
			}
			else{//use override
				insPayEst=PIn.PDouble(textInsPayEst.Text);
			}
			labelInsInsPayEst.Text="= "+insPayEst.ToString("c");
			labelPatInsPayEst.Text="- "+insPayEst.ToString("c");
			double insPayAmt=PIn.PDouble(textInsPayAmt.Text);
			if(listStatus.SelectedIndex==0){//estimate
				//
			}
			else if(listStatus.SelectedIndex==1){//not received
				labelInsInsPayAmt.Text="";
				labelPatInsPayAmt.Text="";
			}
			else{//received or estimate
				labelPatInsPayEst.Text="";//override the estimate
				labelInsInsPayAmt.Text=insPayAmt.ToString("c");
				labelPatInsPayAmt.Text="- "+insPayAmt.ToString("c");
			}
			double writeOff=0;
			if(textWriteOff.Text==""){
				writeOff=0;
				labelPatWriteOff.Text="";
			}
			else{
				writeOff=PIn.PDouble(textWriteOff.Text);
				if(writeOff==0)
					labelPatWriteOff.Text="";
				else
					labelPatWriteOff.Text="- "+writeOff.ToString("c");
			}
			double patTotal=0;
			if(ClaimProcCur.Status==ClaimProcStatus.Estimate){
				patTotal=ProcFee-insPayEst;
			}
			//An estimate will not even have this section visible.
			else if(ClaimProcCur.Status==ClaimProcStatus.NotReceived){//not received.
				patTotal=ProcFee-insPayEst-writeOff;
			}
			else if(ClaimProcCur.Status==ClaimProcStatus.CapEstimate
				|| ClaimProcCur.Status==ClaimProcStatus.CapComplete){
				patTotal=ProcFee-writeOff;
			}
			else{
				patTotal=ProcFee-insPayAmt-writeOff;
			}
			labelPatTotal.Text="= "+patTotal.ToString("c");
		}

		private void listStatus_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			//MessageBox.Show(listStatus.SelectedIndex.ToString());
			//new selected index will already be set
			if(ClaimProcCur.Status!=ClaimProcStatus.Estimate//not an estimate
				&& listStatus.SelectedIndex==0)//and clicked on estimate
			{
				SetListStatus(ClaimProcCur.Status);//no change
				return;
			}
			if(ClaimProcCur.Status==ClaimProcStatus.Estimate){//is an estimate
				SetListStatus(ClaimProcCur.Status);//no change
				return;
			}
			if(ClaimProcCur.Status==ClaimProcStatus.CapComplete
				|| ClaimProcCur.Status==ClaimProcStatus.CapEstimate){//is a cap procedure
				SetListStatus(ClaimProcCur.Status);//no change
				return;
			}
			if(ClaimProcCur.ClaimPaymentNum>0){
				SetListStatus(ClaimProcCur.Status);//no change
				return;
			}
			switch(listStatus.SelectedIndex){
				case 0:
					ClaimProcCur.Status=ClaimProcStatus.Estimate;
					break;
				case 1:
					ClaimProcCur.Status=ClaimProcStatus.NotReceived;
					break;
				case 2:
					ClaimProcCur.Status=ClaimProcStatus.Received;
					break;
				case 3:
					ClaimProcCur.Status=ClaimProcStatus.Preauth;
					break;
				case 4:
					ClaimProcCur.Status=ClaimProcStatus.Supplemental;
					break;
				case 5:
					ClaimProcCur.Status=ClaimProcStatus.CapClaim;
					break;
				case 6:
					ClaimProcCur.Status=ClaimProcStatus.CapEstimate;
					break;
				case 7:
					ClaimProcCur.Status=ClaimProcStatus.CapComplete;
					break;
			}
			if(ClaimProcCur.Status==ClaimProcStatus.Received || ClaimProcCur.Status==ClaimProcStatus.Supplemental){
				labelDateEntry.Visible=true;
				textDateEntry.Visible=true;
			}
			else{
				labelDateEntry.Visible=false;
				textDateEntry.Visible=false;
			}
		}

		private void checkNoBillIns_Click(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textAllowedAmt_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void checkDedBeforePerc_Click(object sender, System.EventArgs e) {
			//click will have already changed the value of the checkbox
			if(checkDedBeforePerc.Checked){
				textDedBeforePerc.Text=textDedApplied.Text;
			}
			else{
				textDedApplied.Text=textDedBeforePerc.Text;
			}
			ComputeAmounts();
		}

		private void textDedBeforePerc_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textCopayOverride_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textCopayOverride_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(ClaimProcCur.Status!=ClaimProcStatus.CapEstimate
				&& ClaimProcCur.Status!=ClaimProcStatus.CapComplete){
				return;
			}
			if(textCopayAmt.errorProvider1.GetError(textCopayOverride)!=""
				){
				return;
			}
			double copay=PIn.PDouble(textCopayOverride.Text);
			//always a procedure
			//double fee=procCur.ProcFee;
			double writeoff=ProcFee-copay;
			if(writeoff<0)
				writeoff=0;
			textWriteOff.Text=writeoff.ToString("n");
			labelPatWriteOff.Text="- "+writeoff.ToString("c");
			labelPatTotal.Text="= "+(ProcFee-writeoff).ToString("c");
		}

		private void textPercentOverride_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textOverrideInsEst_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textDedApplied_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textPaidOtherIns_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textOverAnnualMax_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textInsPayEst_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textInsPayAmt_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

		private void textWriteOff_Leave(object sender, System.EventArgs e) {
			ComputeAmounts();
		}

			/* old code
			textAllowedAmt.Text=ClaimProcCur.AllowedAmt.ToString("n");
			if(ClaimProcCur.Percentage==-1){
				textPercentage.Text="";
			}
			else{
				textPercentage.Text=ClaimProcCur.Percentage.ToString();
			}
			if(ClaimProcCur.PercentOverride==-1){
				textPercentOverride.Text="";
			}
			else{
				textPercentOverride.Text=ClaimProcCur.PercentOverride.ToString();
			}
			textCopayAmt.Text=ClaimProcCur.CopayAmt.ToString("n");
			textOverrideInsEst.Text=ClaimProcCur.OverrideInsEst.ToString("n");
			textDedApplied.Text=ClaimProcCur.DedApplied.ToString("n");
			textInsPayEst.Text=ClaimProcCur.InsPayEst.ToString("n");
			textInsPayAmt.Text=ClaimProcCur.InsPayAmt.ToString("n");
			textWriteOff.Text=ClaimProcCur.WriteOff.ToString("n");
			textRemarks.Text=ClaimProcCur.Remarks;
			double amt=PIn.PDouble(textAmount.Text.ToString());
			double priPercent=CovPats.GetPercent(Procedures.Cur.ADACode,PriSecTot.Pri);
			double secPercent=CovPats.GetPercent(Procedures.Cur.ADACode,PriSecTot.Sec);
			textPriPercent.Text=(priPercent*100).ToString();
			textSecPercent.Text=(secPercent*100).ToString();
			double priEst=Procedures.Cur.ProcFee*priPercent;
			double secEst=Procedures.Cur.ProcFee*secPercent;
			textPriEst.Text=(priEst).ToString("F");
			if(Procedures.Cur.ProcFee-priEst < secEst)
				secEst=Procedures.Cur.ProcFee-priEst;
			textSecEst.Text=secEst.ToString("F");
			if(Claims.Cur.ClaimType=="P"){
				labelPriEst.Font=new Font("Microsoft Sans Serif",9,FontStyle.Bold);
				labelPriOver.Font=new Font("Microsoft Sans Serif",9,FontStyle.Bold);
			}
			else if(Claims.Cur.ClaimType=="S"){
				labelSecEst.Font=new Font("Microsoft Sans Serif",9,FontStyle.Bold);
				labelSecOver.Font=new Font("Microsoft Sans Serif",9,FontStyle.Bold);
			}*/

		///<summary>Remember that this will never even happen unless this is just an estimate because the button is not visible.</summary>
		private void butDelete_Click(object sender, System.EventArgs e) {
			if(MessageBox.Show(Lan.g(this,"Delete this estimate?"),""
				,MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			ClaimProcs.Delete(ClaimProcCur);
			DialogResult=DialogResult.OK;
		}

		private bool AllAreValid(){
			if(  textFeeBilled.errorProvider1.GetError(textFeeBilled)!=""
				|| textAllowedAmt.errorProvider1.GetError(textAllowedAmt)!=""
				|| textDedBeforePerc.errorProvider1.GetError(textDedBeforePerc)!=""
				|| textCopayOverride.errorProvider1.GetError(textCopayAmt)!=""
				|| textPercentOverride.errorProvider1.GetError(textPercentOverride)!=""
				|| textOverrideInsEst.errorProvider1.GetError(textOverrideInsEst)!=""
				|| textDedApplied.errorProvider1.GetError(textDedApplied)!=""
				|| textInsPayEst.errorProvider1.GetError(textInsPayEst)!=""
				|| textInsPayAmt.errorProvider1.GetError(textInsPayAmt)!=""
				|| textWriteOff.errorProvider1.GetError(textWriteOff)!=""
				|| textProcDate.errorProvider1.GetError(textProcDate)!=""
				|| textDateCP.errorProvider1.GetError(textDateCP)!=""
				){
				return false;
			}
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!AllAreValid()){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			//status already handled
			if(listProv.SelectedIndex!=-1){//if no prov selected, then that prov must simply be hidden,
													//because all claimprocs are initially created with a prov(except preauth).
													//So, in this case, don't change.
				ClaimProcCur.ProvNum=Providers.List[listProv.SelectedIndex].ProvNum;
			}
			ClaimProcCur.ProcDate=PIn.PDate(textProcDate.Text);
			if(!textDateCP.ReadOnly){
				ClaimProcCur.DateCP=PIn.PDate(textDateCP.Text);
			}
			ClaimProcCur.CodeSent=textCodeSent.Text;
			ClaimProcCur.FeeBilled=PIn.PDouble(textFeeBilled.Text);
			ClaimProcCur.Remarks=textRemarks.Text;
			ClaimProcCur.NoBillIns=checkNoBillIns.Checked;
			if(textAllowedAmt.Text==""){
				ClaimProcCur.AllowedAmt=-1;
			}
			else{
				ClaimProcCur.AllowedAmt=PIn.PDouble(textAllowedAmt.Text);
			}
			if(checkDedBeforePerc.Checked){
				ClaimProcCur.DedBeforePerc=true;
				ClaimProcCur.DedApplied=PIn.PDouble(textDedBeforePerc.Text);//automatically changes "" to 0
			}
			else{
				ClaimProcCur.DedBeforePerc=false;
				ClaimProcCur.DedApplied=PIn.PDouble(textDedApplied.Text);
			}
			if(textCopayOverride.Text==""){
				ClaimProcCur.CopayOverride=-1;
			}
			else{
				ClaimProcCur.CopayOverride=PIn.PDouble(textCopayOverride.Text);
			}
			if(textPercentOverride.Text==""){
				ClaimProcCur.PercentOverride=-1;
			}
			else{
				ClaimProcCur.PercentOverride=PIn.PInt(textPercentOverride.Text);
			}
			ClaimProcCur.BaseEst=PIn.PDouble(textBaseEst.Text);
			if(textOverrideInsEst.Text==""){
				ClaimProcCur.OverrideInsEst=-1;
			}
			else{
				ClaimProcCur.OverrideInsEst=PIn.PDouble(textOverrideInsEst.Text);
			}
			//dedApplied already handled
			if(textPaidOtherIns.Text==""){
				ClaimProcCur.PaidOtherIns=-1;
			}
			else{
				ClaimProcCur.PaidOtherIns=PIn.PDouble(textPaidOtherIns.Text);
			}
			if(textOverAnnualMax.Text==""){
				ClaimProcCur.OverAnnualMax=-1;
			}
			else{
				ClaimProcCur.OverAnnualMax=PIn.PDouble(textOverAnnualMax.Text);
			}
			ClaimProcCur.InsPayEst=PIn.PDouble(textInsPayEst.Text);
			ClaimProcCur.InsPayAmt=PIn.PDouble(textInsPayAmt.Text);
			ClaimProcCur.WriteOff=Math.Abs(PIn.PDouble(textWriteOff.Text));
			//if status was changed to received, then set DateEntry
			if(ClaimProcOld.Status!=ClaimProcStatus.Received && ClaimProcOld.Status!=ClaimProcStatus.Supplemental){
				if(ClaimProcCur.Status==ClaimProcStatus.Received || ClaimProcOld.Status==ClaimProcStatus.Supplemental){
					ClaimProcCur.DateEntry=DateTime.Now;
				}
			}
			ClaimProcs.Update(ClaimProcCur);
			//there is no functionality here for insert cur, because all claimprocs are
			//created before editing.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormClaimProc_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK){
				return;
			}
			ClaimProcCur=ClaimProcOld.Copy();//revert back to the old ClaimProc
		}

		

		


		
		

		

		

		

		

		

	}
}

















