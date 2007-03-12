using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormClaimPrint : System.Windows.Forms.Form{
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.PrintPreviewControl Preview2;
		private System.Drawing.Printing.PrintDocument pd2;
		private OpenDental.UI.Button butPrint;
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public int ThisClaimNum;
		///<summary></summary>
		public int ThisPatNum;
		//<summary>This will be 0 unless the user is trying to print a batch e-claim with a predefined ClaimForm.</summary>
		//public int ClaimFormNum;
		///<summary></summary>
		public bool PrintBlank;
		private System.Windows.Forms.PrintDialog printDialog2;
		///<summary></summary>
		public bool PrintImmediately;
    private string[] displayStrings;
		///<summary>The claimprocs for this claim, not including payments and duplicates.</summary>
		private List<ClaimProc> claimprocs;
		///<summary>For batch generic e-claims, this just prints the data and not the background.</summary>
		public bool HideBackground;
		private System.Windows.Forms.Label labelTotPages;
		private OpenDental.UI.Button butBack;
		private OpenDental.UI.Button butFwd;
		private int pagesPrinted;
		private int totalPages;
		//<summary>Set to true if using this class just to generate strings for the Renaissance link.</summary>
		//private bool IsRenaissance;
		private ClaimProc[] ClaimProcsForClaim;
		///<summary>All procedures for the patient.</summary>
		private Procedure[] ProcList;
		///<summary>This is set externally for Renaissance and generic e-claims.  If it was not set ahead of time, it will set in FillDisplayStrings according to the insPlan.</summary>
		public ClaimForm ClaimFormCur;
		private InsPlan[] PlanList;
		private Claim ClaimCur;
		///<summary>Always length of 4.</summary>
		private string[] diagnoses;

		///<summary></summary>
		public FormClaimPrint(){
			InitializeComponent();
			Lan.F(this,new Control[] 
			{
				this.labelTotPages//exclude
			});
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormClaimPrint));
			this.butClose = new OpenDental.UI.Button();
			this.Preview2 = new System.Windows.Forms.PrintPreviewControl();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.butPrint = new OpenDental.UI.Button();
			this.printDialog2 = new System.Windows.Forms.PrintDialog();
			this.labelTotPages = new System.Windows.Forms.Label();
			this.butBack = new OpenDental.UI.Button();
			this.butFwd = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(770, 768);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 25);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// Preview2
			// 
			this.Preview2.AutoZoom = false;
			this.Preview2.Location = new System.Drawing.Point(0, 0);
			this.Preview2.Name = "Preview2";
			this.Preview2.Size = new System.Drawing.Size(738, 798);
			this.Preview2.TabIndex = 1;
			this.Preview2.Zoom = 1;
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.Location = new System.Drawing.Point(769, 728);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 25);
			this.butPrint.TabIndex = 2;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// labelTotPages
			// 
			this.labelTotPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTotPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTotPages.Location = new System.Drawing.Point(774, 679);
			this.labelTotPages.Name = "labelTotPages";
			this.labelTotPages.Size = new System.Drawing.Size(54, 18);
			this.labelTotPages.TabIndex = 22;
			this.labelTotPages.Text = "1 / 2";
			this.labelTotPages.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// butBack
			// 
			this.butBack.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butBack.Autosize = true;
			this.butBack.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBack.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBack.Image = ((System.Drawing.Image)(resources.GetObject("butBack.Image")));
			this.butBack.Location = new System.Drawing.Point(752, 676);
			this.butBack.Name = "butBack";
			this.butBack.Size = new System.Drawing.Size(18, 23);
			this.butBack.TabIndex = 23;
			this.butBack.Click += new System.EventHandler(this.butBack_Click);
			// 
			// butFwd
			// 
			this.butFwd.AdjustImageLocation = new System.Drawing.Point(1, 0);
			this.butFwd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butFwd.Autosize = true;
			this.butFwd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFwd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFwd.Image = ((System.Drawing.Image)(resources.GetObject("butFwd.Image")));
			this.butFwd.Location = new System.Drawing.Point(830, 676);
			this.butFwd.Name = "butFwd";
			this.butFwd.Size = new System.Drawing.Size(18, 23);
			this.butFwd.TabIndex = 24;
			this.butFwd.Click += new System.EventHandler(this.butFwd_Click);
			// 
			// FormClaimPrint
			// 
			this.AcceptButton = this.butPrint;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(860, 816);
			this.Controls.Add(this.labelTotPages);
			this.Controls.Add(this.butBack);
			this.Controls.Add(this.butFwd);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.Preview2);
			this.Controls.Add(this.butClose);
			this.Name = "FormClaimPrint";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Print Claim";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FormClaimPrint_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormClaimPrint_Layout);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClaimPrint_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			Preview2.Height=ClientRectangle.Height;
			Preview2.Width=ClientRectangle.Width-160;
			//butClose.Location=new Point(ClientRectangle.Width-100,ClientRectangle.Height-70);
			//butPrint.Location=new Point(ClientRectangle.Width-100,ClientRectangle.Height-140);
		}
		private void FormClaimPrint_Load(object sender, System.EventArgs e) {
			if(PrinterSettings.InstalledPrinters.Count==0) {
				MessageBox.Show(Lan.g(this,"No printer installed"));
				return;
			}
			pd2=new PrintDocument();
			pagesPrinted=0;
			pd2.OriginAtMargins=true;
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			if(pd2.DefaultPageSettings.PaperSize.Height<400) {//some printers report page size of 0.
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			pd2.PrintPage+=new PrintPageEventHandler(this.pd2_PrintPage);
			Preview2.Document=pd2;//display document
			Preview2.InvalidatePreview();
		}

		///<summary>Only called from external forms without ever loading this form.  Prints without showing any print preview.  Returns true if printed successfully.  You have to supply a printer name because this can be called multiple times when printing batch claims.</summary>
		public bool PrintImmediate(string printerName,short copies){
			pd2=new PrintDocument();
			pagesPrinted=0;
			pd2.OriginAtMargins=true;
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd2.PrintPage+=new PrintPageEventHandler(this.pd2_PrintPage);
			pd2.PrinterSettings.PrinterName=printerName;
			pd2.PrinterSettings.Copies=copies;
			try{
				pd2.Print();
			}
			catch{
				MessageBox.Show(Lan.g("Printer","Printer not available"));
				return false;
			}
			return true;
		}

		private void pd2_PrintPage(object sender, PrintPageEventArgs ev){//raised for each page to be printed.
			FillDisplayStrings();
			int procLimit=ProcLimitForFormat();
			//claimprocs is filled in FillDisplayStrings
			if(claimprocs.Count==0)
				totalPages=1;
			else
				totalPages=(int)Math.Ceiling((double)claimprocs.Count/(double)procLimit);
			FillProcStrings(pagesPrinted*procLimit,procLimit);
			Graphics grfx=ev.Graphics;
			float xPosText;
			for(int i=0;i<ClaimFormCur.Items.Length;i++){
				if(ClaimFormCur.Items[i].ImageFileName==""){//field
					xPosText=ClaimFormCur.Items[i].XPos+ClaimFormCur.OffsetX;
					if(ClaimFormCur.Items[i].FieldName=="P1Fee"
						|| ClaimFormCur.Items[i].FieldName=="P2Fee"
						|| ClaimFormCur.Items[i].FieldName=="P3Fee"
						|| ClaimFormCur.Items[i].FieldName=="P4Fee"
						|| ClaimFormCur.Items[i].FieldName=="P5Fee"
						|| ClaimFormCur.Items[i].FieldName=="P6Fee"
						|| ClaimFormCur.Items[i].FieldName=="P7Fee"
						|| ClaimFormCur.Items[i].FieldName=="P8Fee"
						|| ClaimFormCur.Items[i].FieldName=="P9Fee"
						|| ClaimFormCur.Items[i].FieldName=="P10Fee"
						|| ClaimFormCur.Items[i].FieldName=="P1Lab"
						|| ClaimFormCur.Items[i].FieldName=="P2Lab"
						|| ClaimFormCur.Items[i].FieldName=="P3Lab"
						|| ClaimFormCur.Items[i].FieldName=="P4Lab"
						|| ClaimFormCur.Items[i].FieldName=="P5Lab"
						|| ClaimFormCur.Items[i].FieldName=="P6Lab"
						|| ClaimFormCur.Items[i].FieldName=="P7Lab"
						|| ClaimFormCur.Items[i].FieldName=="P8Lab"
						|| ClaimFormCur.Items[i].FieldName=="P9Lab"
						|| ClaimFormCur.Items[i].FieldName=="P10Lab"
						|| ClaimFormCur.Items[i].FieldName=="P1FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P2FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P3FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P4FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P5FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P6FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P7FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P8FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P9FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="P10FeeMinusLab"
						|| ClaimFormCur.Items[i].FieldName=="TotalFee")
					{
						//this aligns it to the right
						xPosText-=grfx.MeasureString(displayStrings[i],new Font(ClaimFormCur.FontName,ClaimFormCur.FontSize)).Width;
					}
					grfx.DrawString(displayStrings[i]
						,new Font(ClaimFormCur.FontName,ClaimFormCur.FontSize)
						,new SolidBrush(Color.Black)
						,new RectangleF(xPosText,ClaimFormCur.Items[i].YPos+ClaimFormCur.OffsetY
						,ClaimFormCur.Items[i].Width,ClaimFormCur.Items[i].Height));
				}
				else{//image
					if(!ClaimFormCur.PrintImages){
						continue;
					}
					if(HideBackground){
						continue;
					}
					string fileName=((Pref)PrefB.HList["DocPath"]).ValueString+@"\"
						+ClaimFormCur.Items[i].ImageFileName;
					if(!File.Exists(fileName)){
						//MessageBox.Show("File not found.");
						continue;
					}
					Image thisImage=Image.FromFile(fileName);
					if(fileName.Substring(fileName.Length-3)=="jpg"){
						grfx.DrawImage(thisImage
							,ClaimFormCur.Items[i].XPos+ClaimFormCur.OffsetX
							,ClaimFormCur.Items[i].YPos+ClaimFormCur.OffsetY
							,(int)(thisImage.Width/thisImage.HorizontalResolution*100)
							,(int)(thisImage.Height/thisImage.VerticalResolution*100));
					}
					else if(fileName.Substring(fileName.Length-3)=="gif"){
						grfx.DrawImage(thisImage
							,ClaimFormCur.Items[i].XPos+ClaimFormCur.OffsetX
							,ClaimFormCur.Items[i].YPos+ClaimFormCur.OffsetY
							,ClaimFormCur.Items[i].Width
							,ClaimFormCur.Items[i].Height);
					}
					else if(fileName.Substring(fileName.Length-3)=="emf"){
						grfx.DrawImage(thisImage
							,ClaimFormCur.Items[i].XPos+ClaimFormCur.OffsetX
							,ClaimFormCur.Items[i].YPos+ClaimFormCur.OffsetY
							,thisImage.Width,thisImage.Height);
					}
				}
			}
			pagesPrinted++;
			if(totalPages==pagesPrinted){
				ev.HasMorePages=false;
				labelTotPages.Text="1 / "+totalPages.ToString();
			}
			else{
				//MessageBox.Show(pagesPrinted.ToString()+","+totalPages.ToString());
				ev.HasMorePages=true;
			}
		}

		///<summary>Only used when the print button is clicked from within this form during print preview.</summary>
		public bool PrintClaim(){
			pd2.OriginAtMargins=true;
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pagesPrinted=0;
			if(Printers.SetPrinter(pd2,PrintSituation.Claim)){
				try{
					pd2.Print();
				}
				catch{
					MessageBox.Show(Lan.g("Printer","Printer not available."));
					return false;
				}
			}
			else{
				return false;//if they hit cancel
			}		
			return true;
		}

		///<summary>Called from Bridges.Renaissance, this takes the supplied ClaimFormItems.ListForForm, and generates an array of strings that will get saved into a text file.  First dimension of array is the pages. Second dimension is the lines in the page.</summary>
		public string[][] FillRenaissance(){
			//IsRenaissance=true;
			int procLimit=8;
			FillDisplayStrings();//claimprocs is filled in FillDisplayStrings
														//, so this is just a little extra work
			totalPages=(int)Math.Ceiling((double)claimprocs.Count/(double)procLimit);
			string[][] retVal=new string[totalPages][];
			for(int i=0;i<totalPages;i++){
				pagesPrinted=i;
				//not sure if I also need to do FillDisplayStrings here
				FillProcStrings(pagesPrinted*procLimit,procLimit);
				retVal[i]=(string[])displayStrings.Clone();
			}
			return retVal;
		}

		///<summary>Gets all necessary info from db based on ThisPatNum and ThisClaimNum.  Then fills displayStrings with the actual text that will display on claim.</summary>
		private void FillDisplayStrings(){
			if(PrintBlank){
				ClaimFormCur=ClaimForms.GetClaimForm(1);//hard coded to ADA claimform for now.
				//ClaimFormItems.GetListForForm(ClaimFormCur.ClaimFormNum);
				displayStrings=new string[ClaimFormCur.Items.Length];
				claimprocs=new List<ClaimProc>();
				return;
			}
			Family FamCur=Patients.GetFamily(ThisPatNum);
			Patient PatCur=FamCur.GetPatient(ThisPatNum);
			Claims.Refresh(PatCur.PatNum);
			ClaimCur=((Claim)Claims.HList[ThisClaimNum]).Copy();
			PlanList=InsPlans.Refresh(FamCur);
			InsPlan otherPlan=InsPlans.GetPlan(ClaimCur.PlanNum2,PlanList);
			if(otherPlan==null){
				otherPlan=new InsPlan();//easier than leaving it null
			}
			Carrier otherCarrier=new Carrier();
			if(otherPlan.PlanNum!=0){
				otherCarrier=Carriers.GetCarrier(otherPlan.CarrierNum);
			}
			//Employers.GetEmployer(otherPlan.EmployerNum);
			//Employer otherEmployer=Employers.Cur;//not actually used
			//then get the main plan
			InsPlan planCur=InsPlans.GetPlan(ClaimCur.PlanNum,PlanList);
			Clinic clinic=Clinics.GetClinic(ClaimCur.ClinicNum);
			Carrier carrier=Carriers.GetCarrier(planCur.CarrierNum);
			//Employers.GetEmployer(InsPlans.Cur.EmployerNum);
			Patient subsc;
			if(FamCur.GetIndex(planCur.Subscriber)==-1){//from another family
				subsc=Patients.GetPat(planCur.Subscriber);
				//Patients.Cur;
				//Patients.GetFamily(ThisPatNum);//return to current family
			}
			else{
				subsc=FamCur.List[FamCur.GetIndex(planCur.Subscriber)];
			}
			Patient otherSubsc=new Patient();
			if(otherPlan.PlanNum!=0){//if secondary insurance exists
				if(FamCur.GetIndex(otherPlan.Subscriber)==-1){//from another family
					otherSubsc=Patients.GetPat(otherPlan.Subscriber);
					//Patients.Cur;
					//Patients.GetFamily(ThisPatNum);//return to current family
				}
				else{
					otherSubsc=FamCur.List[FamCur.GetIndex(otherPlan.Subscriber)];
				}				
			}	
			ProcList=Procedures.Refresh(PatCur.PatNum);
			ToothInitial[] initialList=ToothInitials.Refresh(PatCur.PatNum);
      ClaimProc[] ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
      ClaimProcsForClaim=ClaimProcs.GetForClaim(ClaimProcList,ClaimCur.ClaimNum); 
			claimprocs=new List<ClaimProc>();
			bool includeThis;
			for(int i=0;i<ClaimProcsForClaim.Length;i++){//fill the arraylist
				if(ClaimProcsForClaim[i].ProcNum==0){
					continue;//skip payments
				}
				includeThis=true;
				for(int j=0;j<claimprocs.Count;j++){//loop through existing claimprocs
					if(claimprocs[j].ProcNum==ClaimProcsForClaim[i].ProcNum){
						includeThis=false;//skip duplicate procedures
					}
				}
				if(includeThis){
					claimprocs.Add(ClaimProcsForClaim[i]);	
				}
			}
			ArrayList missingTeeth=ToothInitials.GetMissingOrHiddenTeeth(initialList);
			Procedure proc;
			ProcedureCode procCode;
			for(int j=missingTeeth.Count-1;j>=0;j--) {//loop backwards to keep index accurate as items are removed
				//if the missing tooth is missing because of an extraction being billed here, then exclude it
				for(int p=0;p<claimprocs.Count;p++) {
					proc=Procedures.GetProc(ProcList,claimprocs[p].ProcNum);
					procCode=ProcedureCodes.GetProcCode(proc.ADACode);
					if(procCode.PaintType==ToothPaintingType.Extraction && proc.ToothNum==(string)missingTeeth[j]) {
						missingTeeth.RemoveAt(j);
						break;
					}
				}
			}
			//diagnoses---------------------------------------------------------------------------------------
			diagnoses=new string[4];
			for(int i=0;i<4;i++){
				diagnoses[i]="";
			}
			for(int i=0;i<claimprocs.Count;i++){
				proc=Procedures.GetProc(ProcList,claimprocs[i].ProcNum);
				if(proc.DiagnosticCode==""){
					continue;
				}
				for(int d=0;d<4;d++){
					if(diagnoses[d]==proc.DiagnosticCode){
						break;//if it's already been added
					}
					if(diagnoses[d]==""){//we're at the end of the list of existing diagnoses, and no match
						diagnoses[d]=proc.DiagnosticCode;//so add it.
						break;
					}
				}
				//There's still a chance that the diagnosis didn't get added, if there were more than 4.
			}
			Provider treatDent=Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvTreat)];
			if(ClaimFormCur==null){
				ClaimFormCur=ClaimForms.GetClaimForm(planCur.ClaimFormNum);
			}
			//else{//usually only for batch generic e-claims and Renaissance
			//	ClaimFormCur=ClaimForms.GetClaimForm(ClaimFormNum);
			//}
			//if(!IsRenaissance){
				//for renaissance, this is skipped because the ListForForm will have already been filled.
			//must fix this line:
			//	ClaimFormItems.GetListForForm(ClaimFormCur.ClaimFormNum);
			//}
			displayStrings=new string[ClaimFormCur.Items.Length];
			//a value is set for every item, but not every case will have a matching claimform item.
			for(int i=0;i<ClaimFormCur.Items.Length;i++){
				if(ClaimFormCur.Items[i]==null){//Renaissance does not use [0]
					displayStrings[i]="";
					continue;
				}
				switch(ClaimFormCur.Items[i].FieldName){
					default://image. or procedure which gets filled in FillProcStrings.
						displayStrings[i]="";
						break;
					case "FixedText":
						displayStrings[i]=ClaimFormCur.Items[i].FormatString;
						break;
					case "IsPreAuth":
						if(ClaimCur.ClaimType=="PreAuth")
							displayStrings[i]="X";
						break;
					case "IsStandardClaim":
						if(ClaimCur.ClaimType!="PreAuth")
							displayStrings[i]="X";
						break;
					case "IsMedicaidClaim"://this should later be replaced with an insplan field.
						if(PatCur.MedicaidID!="")
							displayStrings[i]="X";
						break;
					case "PreAuthString":
						displayStrings[i]=ClaimCur.PreAuthString;
						break;
					case "PriInsCarrierName":
						displayStrings[i]=carrier.CarrierName;
						break;
					case "PriInsAddress":
						displayStrings[i]=carrier.Address;
						break;
					case "PriInsAddress2":
						displayStrings[i]=carrier.Address2;
						break;
					case "PriInsAddressComplete":
						displayStrings[i]=carrier.Address+" "+carrier.Address2;
						break;
					case "PriInsCity":
						displayStrings[i]=carrier.City;
						break;
					case "PriInsST":
						displayStrings[i]=carrier.State;
						break;
					case "PriInsZip":
						displayStrings[i]=carrier.Zip;
						break;
					case "OtherInsExists":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]="X";
						break;
					case "OtherInsNotExists":
						if(otherPlan.PlanNum==0)
							displayStrings[i]="X";
						break;
					case "OtherInsSubscrLastFirst":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherSubsc.LName+", "+otherSubsc.FName+", "+otherSubsc.MiddleI;
						break;
					case "OtherInsSubscrDOB":
						if(otherPlan.PlanNum!=0)
							if(ClaimFormCur.Items[i].FormatString=="")
								displayStrings[i]=otherSubsc.Birthdate.ToShortDateString();
							else
								displayStrings[i]=otherSubsc.Birthdate.ToString
									(ClaimFormCur.Items[i].FormatString);
						break;
					case "OtherInsSubscrIsMale":
						if(otherPlan.PlanNum!=0 && otherSubsc.Gender==PatientGender.Male)
							displayStrings[i]="X";
						break;
					case "OtherInsSubscrIsFemale":
						if(otherPlan.PlanNum!=0 && otherSubsc.Gender==PatientGender.Female)
							displayStrings[i]="X";
						break;
					case "OtherInsSubscrID":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherPlan.SubscriberID;
						break;
						//if(otherPlan.PlanNum!=0 && otherSubsc.SSN.Length==9){
						//	displayStrings[i]=otherSubsc.SSN.Substring(0,3)
						//		+"-"+otherSubsc.SSN.Substring(3,2)
						//		+"-"+otherSubsc.SSN.Substring(5);
						//}
						//break;
					case "OtherInsGroupNum":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherPlan.GroupNum;
						break;
					case "OtherInsRelatIsSelf":
						if(otherPlan.PlanNum!=0 && ClaimCur.PatRelat2==Relat.Self)
							displayStrings[i]="X";
						break;
					case "OtherInsRelatIsSpouse":
						if(otherPlan.PlanNum!=0 && ClaimCur.PatRelat2==Relat.Spouse)
							displayStrings[i]="X";
						break;
					case "OtherInsRelatIsChild":
						if(otherPlan.PlanNum!=0 && ClaimCur.PatRelat2==Relat.Child)
							displayStrings[i]="X";
						break;
					case "OtherInsRelatIsOther":
						if(otherPlan.PlanNum!=0 && (
							ClaimCur.PatRelat2==Relat.Dependent
							|| ClaimCur.PatRelat2==Relat.Employee
							|| ClaimCur.PatRelat2==Relat.HandicapDep
							|| ClaimCur.PatRelat2==Relat.InjuredPlaintiff
							|| ClaimCur.PatRelat2==Relat.LifePartner
							|| ClaimCur.PatRelat2==Relat.SignifOther
							))
							displayStrings[i]="X";
						break;
					case "OtherInsCarrierName":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherCarrier.CarrierName;
						break;
					case "OtherInsAddress":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherCarrier.Address;
						break;
					case "OtherInsCity":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherCarrier.City;
						break;
					case "OtherInsST":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherCarrier.State;
						break;
					case "OtherInsZip":
						if(otherPlan.PlanNum!=0)
							displayStrings[i]=otherCarrier.Zip;
						break;
					case "SubscrLastFirst":
						displayStrings[i]=subsc.LName+", "+subsc.FName+", "+subsc.MiddleI;
						break;
					case "SubscrAddress":
						displayStrings[i]=subsc.Address;
						break;
					case "SubscrAddress2":
						displayStrings[i]=subsc.Address2;
						break;
					case "SubscrAddressComplete":
						displayStrings[i]=subsc.Address+" "+subsc.Address2;
						break;
					case "SubscrCity":
						displayStrings[i]=subsc.City;
						break;
					case "SubscrST":
						displayStrings[i]=subsc.State;
						break;
					case "SubscrZip":
						displayStrings[i]=subsc.Zip;
						break;
					case "SubscrPhone":
						displayStrings[i]=subsc.HmPhone;
						break;
					case "SubscrDOB":
						if(ClaimFormCur.Items[i].FormatString=="")
							displayStrings[i]=subsc.Birthdate.ToShortDateString();//MM/dd/yyyy
						else
							displayStrings[i]=subsc.Birthdate.ToString(ClaimFormCur.Items[i].FormatString);
						break;
					case "SubscrIsMale":
						if(subsc.Gender==PatientGender.Male)
							displayStrings[i]="X";
						break;
					case "SubscrIsFemale":
						if(subsc.Gender==PatientGender.Female)
							displayStrings[i]="X";
						break;
					case "SubscrIsMarried":
						if(subsc.Position==PatientPosition.Married)
							displayStrings[i]="X";
						break;
					case "SubscrIsSingle":
						if(subsc.Position==PatientPosition.Single
							|| subsc.Position==PatientPosition.Child
							|| subsc.Position==PatientPosition.Widowed)
							displayStrings[i]="X";
						break;
					case "SubscrID":
						displayStrings[i]=planCur.SubscriberID;
						break;
					case "SubscrIsFTStudent":
						if(subsc.StudentStatus=="F")
							displayStrings[i]="X";
						break;
					case "SubscrIsPTStudent":
						if(subsc.StudentStatus=="P")
							displayStrings[i]="X";
						break;
					case "GroupNum":
						displayStrings[i]=planCur.GroupNum;
						break;
					case "EmployerName":
						displayStrings[i]=Employers.GetEmployer(planCur.EmployerNum).EmpName;;
						break;
					case "RelatIsSelf":
						if(ClaimCur.PatRelat==Relat.Self)
							displayStrings[i]="X";
						break;
					case "RelatIsSpouse":
						if(ClaimCur.PatRelat==Relat.Spouse)
							displayStrings[i]="X";
						break;
					case "RelatIsChild":
						if(ClaimCur.PatRelat==Relat.Child)
							displayStrings[i]="X";
						break;
					case "RelatIsOther":
						if(ClaimCur.PatRelat==Relat.Dependent
							|| ClaimCur.PatRelat==Relat.Employee
							|| ClaimCur.PatRelat==Relat.HandicapDep
							|| ClaimCur.PatRelat==Relat.InjuredPlaintiff
							|| ClaimCur.PatRelat==Relat.LifePartner
							|| ClaimCur.PatRelat==Relat.SignifOther)
							displayStrings[i]="X";
						break;
					case "Relationship":
						displayStrings[i]=ClaimCur.PatRelat.ToString();
						break;
					case "IsFTStudent":
						if(PatCur.StudentStatus=="F")
							displayStrings[i]="X";
						break;
					case "IsPTStudent":
						if(PatCur.StudentStatus=="P")
							displayStrings[i]="X";
						break;
					case "IsStudent":
						if(PatCur.StudentStatus=="P" || PatCur.StudentStatus=="F")
							displayStrings[i]="X";
						break;
					case "PatientLastFirst":
						displayStrings[i]=PatCur.LName+", "+PatCur.FName+", "+PatCur.MiddleI;
						break;
					case "PatientFirstName":
						displayStrings[i] = PatCur.FName;
						break;
					case "PatientMiddleName":
						displayStrings[i] = PatCur.MiddleI;
						break;
					case "PatientLastName":
						displayStrings[i] = PatCur.LName;
						break;
					case "PatientAddress":
						displayStrings[i]=PatCur.Address;
						break;
					case "PatientAddress2":
						displayStrings[i]=PatCur.Address2;
						break;
					case "PatientAddressComplete":
						displayStrings[i]=PatCur.Address+" "+PatCur.Address2;
						break;
					case "PatientCity":
						displayStrings[i]=PatCur.City;
						break;
					case "PatientST":
						displayStrings[i]=PatCur.State;
						break;
					case "PatientZip":
						displayStrings[i]=PatCur.Zip;
						break;
					case "PatientPhone":
						displayStrings[i]=PatCur.HmPhone;
						break;
					case "PatientDOB":
						if(ClaimFormCur.Items[i].FormatString=="")
							displayStrings[i]=PatCur.Birthdate.ToShortDateString();//MM/dd/yyyy
						else
							displayStrings[i]=PatCur.Birthdate.ToString
								(ClaimFormCur.Items[i].FormatString);
						break;
					case "PatientIsMale":
						if(PatCur.Gender==PatientGender.Male)
							displayStrings[i]="X";
						break;
					case "PatientIsFemale":
						if(PatCur.Gender==PatientGender.Female)
							displayStrings[i]="X";
						break;
					case "PatientGender":
						if(PatCur.Gender==PatientGender.Male)
							displayStrings[i]="Male";
						else if(PatCur.Gender==PatientGender.Female)
							displayStrings[i]="Female";
						break;
					case "PatientIsMarried":
						if(PatCur.Position==PatientPosition.Married)
							displayStrings[i]="X";
						break;
					case "PatientIsSingle":
						if(PatCur.Position==PatientPosition.Single
							|| PatCur.Position==PatientPosition.Child
							|| PatCur.Position==PatientPosition.Widowed)
							displayStrings[i]="X";
						break;
					case "PatientSSN":
						if(PatCur.SSN.Length==9){
							displayStrings[i]=PatCur.SSN.Substring(0,3)
								+"-"+PatCur.SSN.Substring(3,2)
								+"-"+PatCur.SSN.Substring(5);
						}
						break;
					case "PatientMedicaidID":
						displayStrings[i]=PatCur.MedicaidID;
						break;
					case "PatientID-MedicaidOrSSN":
						if(PatCur.MedicaidID!="")
							displayStrings[i]=PatCur.MedicaidID;
						else
							displayStrings[i]=PatCur.SSN;
						break;
					case "Diagnosis1":
						displayStrings[i]=diagnoses[0];
						break;
					case "Diagnosis2":
						displayStrings[i]=diagnoses[1];
						break;
					case "Diagnosis3":
						displayStrings[i]=diagnoses[2];
						break;
					case "Diagnosis4":
						displayStrings[i]=diagnoses[3];
						break;
			//this is where the procedures used to be
					case "Miss1":
						if(missingTeeth.Contains("1"))
							displayStrings[i]="X";
						break;
					case "Miss2":
						if(missingTeeth.Contains("2"))
							displayStrings[i]="X";
						break;
					case "Miss3":
						if(missingTeeth.Contains("3"))
							displayStrings[i]="X";
						break;
					case "Miss4":
						if(missingTeeth.Contains("4"))
							displayStrings[i]="X";
						break;
					case "Miss5":
						if(missingTeeth.Contains("5"))
							displayStrings[i]="X";
						break;
					case "Miss6":
						if(missingTeeth.Contains("6"))
							displayStrings[i]="X";
						break;
					case "Miss7":
						if(missingTeeth.Contains("7"))
							displayStrings[i]="X";
						break;
					case "Miss8":
						if(missingTeeth.Contains("8"))
							displayStrings[i]="X";
						break;
					case "Miss9":
						if(missingTeeth.Contains("9"))
							displayStrings[i]="X";
						break;
					case "Miss10":
						if(missingTeeth.Contains("10"))
							displayStrings[i]="X";
						break;
					case "Miss11":
						if(missingTeeth.Contains("11"))
							displayStrings[i]="X";
						break;
					case "Miss12":
						if(missingTeeth.Contains("12"))
							displayStrings[i]="X";
						break;
					case "Miss13":
						if(missingTeeth.Contains("13"))
							displayStrings[i]="X";
						break;
					case "Miss14":
						if(missingTeeth.Contains("14"))
							displayStrings[i]="X";
						break;
					case "Miss15":
						if(missingTeeth.Contains("15"))
							displayStrings[i]="X";
						break;
					case "Miss16":
						if(missingTeeth.Contains("16"))
							displayStrings[i]="X";
						break;
					case "Miss17":
						if(missingTeeth.Contains("17"))
							displayStrings[i]="X";
						break;
					case "Miss18":
						if(missingTeeth.Contains("18"))
							displayStrings[i]="X";
						break;
					case "Miss19":
						if(missingTeeth.Contains("19"))
							displayStrings[i]="X";
						break;
					case "Miss20":
						if(missingTeeth.Contains("20"))
							displayStrings[i]="X";
						break;
					case "Miss21":
						if(missingTeeth.Contains("21"))
							displayStrings[i]="X";
						break;
					case "Miss22":
						if(missingTeeth.Contains("22"))
							displayStrings[i]="X";
						break;
					case "Miss23":
						if(missingTeeth.Contains("23"))
							displayStrings[i]="X";
						break;
					case "Miss24":
						if(missingTeeth.Contains("24"))
							displayStrings[i]="X";
						break;
					case "Miss25":
						if(missingTeeth.Contains("25"))
							displayStrings[i]="X";
						break;
					case "Miss26":
						if(missingTeeth.Contains("26"))
							displayStrings[i]="X";
						break;
					case "Miss27":
						if(missingTeeth.Contains("27"))
							displayStrings[i]="X";
						break;
					case "Miss28":
						if(missingTeeth.Contains("28"))
							displayStrings[i]="X";
						break;
					case "Miss29":
						if(missingTeeth.Contains("29"))
							displayStrings[i]="X";
						break;
					case "Miss30":
						if(missingTeeth.Contains("30"))
							displayStrings[i]="X";
						break;
					case "Miss31":
						if(missingTeeth.Contains("31"))
							displayStrings[i]="X";
						break;
					case "Miss32":
						if(missingTeeth.Contains("32"))
							displayStrings[i]="X";
						break;
					case "Remarks":
						displayStrings[i]=ClaimCur.ClaimNote;
						break;
					case "PatientRelease":
						if(planCur.ReleaseInfo)
							displayStrings[i]="Signature on File"; 
						break;
					case "PatientReleaseDate":
						if(planCur.ReleaseInfo && ClaimCur.DateSent.Year > 1860){
							if(ClaimFormCur.Items[i].FormatString=="")
								displayStrings[i]=ClaimCur.DateSent.ToShortDateString();
							else
								displayStrings[i]=ClaimCur.DateSent.ToString
									(ClaimFormCur.Items[i].FormatString);
						} 
						break;
					case "PatientAssignment":
						if(planCur.AssignBen)
							displayStrings[i]="Signature on File"; 
						break;
					case "PatientAssignmentDate":
						if(planCur.AssignBen && ClaimCur.DateSent.Year > 1860){
							if(ClaimFormCur.Items[i].FormatString=="")
								displayStrings[i]=ClaimCur.DateSent.ToShortDateString();
							else
								displayStrings[i]=ClaimCur.DateSent.ToString
									(ClaimFormCur.Items[i].FormatString);
						}
						break;
					case "PlaceIsOffice":
						if(ClaimCur.PlaceService==PlaceOfService.Office)
							displayStrings[i]="X";
						break;
					case "PlaceIsHospADA2002":
						if(ClaimCur.PlaceService==PlaceOfService.InpatHospital
							|| ClaimCur.PlaceService==PlaceOfService.OutpatHospital)
							displayStrings[i]="X";
						break;
					case "PlaceIsExtCareFacilityADA2002":
						if(ClaimCur.PlaceService==PlaceOfService.AdultLivCareFac
							|| ClaimCur.PlaceService==PlaceOfService.SkilledNursFac)
							displayStrings[i]="X";
						break;
					case "PlaceIsOtherADA2002":
						if(ClaimCur.PlaceService==PlaceOfService.PatientsHome
							|| ClaimCur.PlaceService==PlaceOfService.OtherLocation)
							displayStrings[i]="X";
						break;
					case "PlaceIsInpatHosp":
						if(ClaimCur.PlaceService==PlaceOfService.InpatHospital)
							displayStrings[i]="X";
						break;
					case "PlaceIsOutpatHosp":
						if(ClaimCur.PlaceService==PlaceOfService.OutpatHospital)
							displayStrings[i]="X";
						break;
					case "PlaceIsAdultLivCareFac":
						if(ClaimCur.PlaceService==PlaceOfService.AdultLivCareFac)
							displayStrings[i]="X";
						break;
					case "PlaceIsSkilledNursFac":
						if(ClaimCur.PlaceService==PlaceOfService.SkilledNursFac)
							displayStrings[i]="X";
						break;
					case "PlaceIsPatientsHome":
						if(ClaimCur.PlaceService==PlaceOfService.PatientsHome)
							displayStrings[i]="X";
						break;
					case "PlaceIsOtherLocation":
						if(ClaimCur.PlaceService==PlaceOfService.OtherLocation)
							displayStrings[i]="X";
						break;
					case "PlaceNumericCode":
						displayStrings[i]=GetPlaceOfServiceNum(ClaimCur.PlaceService);
						break;
					case "IsRadiographsAttached":
						if(ClaimCur.Radiographs>0)
							displayStrings[i]="X";
						break;
					case "RadiographsNumAttached":
						displayStrings[i]=ClaimCur.Radiographs.ToString();
						break;
					case "RadiographsNotAttached":
						if(ClaimCur.Radiographs==0)
							displayStrings[i]="X";
						break;
					//"ImagesEnclosed":
					//"ModelsEnclosed":
					case "IsNotOrtho":
						if(!ClaimCur.IsOrtho)
							displayStrings[i]="X";
						break;
					case "IsOrtho":
						if(ClaimCur.IsOrtho)
							displayStrings[i]="X";
						break;
					case "DateOrthoPlaced":
						if(ClaimCur.OrthoDate.Year > 1880){
							if(ClaimFormCur.Items[i].FormatString=="")
								displayStrings[i]=ClaimCur.OrthoDate.ToShortDateString();
							else
								displayStrings[i]=ClaimCur.OrthoDate.ToString
									(ClaimFormCur.Items[i].FormatString);
						}
						break;
					case "MonthsOrthoRemaining":
						if(ClaimCur.OrthoRemainM > 0)
							displayStrings[i]=ClaimCur.OrthoRemainM.ToString();
						break;
					case "IsNotProsth":
						if(ClaimCur.IsProsthesis=="N")
							displayStrings[i]="X";
						break;
					case "IsInitialProsth":
						if(ClaimCur.IsProsthesis=="I")
							displayStrings[i]="X";
						break;
					case "IsNotReplacementProsth":
						if(ClaimCur.IsProsthesis!="R")//=='I'nitial or 'N'o
							displayStrings[i]="X";
						break;
					case "IsReplacementProsth":
						if(ClaimCur.IsProsthesis=="R")
							displayStrings[i]="X";
						break;
					case "DatePriorProsthPlaced":
						if(ClaimCur.PriorDate.Year > 1860){
							if(ClaimFormCur.Items[i].FormatString=="")
								displayStrings[i]=ClaimCur.PriorDate.ToShortDateString();
							else
								displayStrings[i]=ClaimCur.PriorDate.ToString
									(ClaimFormCur.Items[i].FormatString);
						}
						break;
					case "IsOccupational":
						if(ClaimCur.AccidentRelated=="E")
							displayStrings[i]="X";
						break;
					case "IsNotOccupational":
						if(ClaimCur.AccidentRelated!="E")
							displayStrings[i]="X";
						break;
					case "IsAutoAccident":
						if(ClaimCur.AccidentRelated=="A")
							displayStrings[i]="X";
						break;
					case "IsNotAutoAccident":
						if(ClaimCur.AccidentRelated!="A")
							displayStrings[i]="X";
						break;
					case "IsOtherAccident":
						if(ClaimCur.AccidentRelated=="O")
							displayStrings[i]="X";
						break;
					case "IsNotOtherAccident":
						if(ClaimCur.AccidentRelated!="O")
							displayStrings[i]="X";
						break;
					case "IsNotAccident":
						if(ClaimCur.AccidentRelated!="O" && ClaimCur.AccidentRelated!="A")
							displayStrings[i]="X";
						break;
					case "IsAccident":
						if(ClaimCur.AccidentRelated!="")
							displayStrings[i]="X";
						break;
					case "AccidentDate":
						if(ClaimCur.AccidentDate.Year > 1860){
							if(ClaimFormCur.Items[i].FormatString=="")
								displayStrings[i]=ClaimCur.AccidentDate.ToShortDateString();
							else
								displayStrings[i]=ClaimCur.AccidentDate.ToString
									(ClaimFormCur.Items[i].FormatString);
						}
						break;
					case "AccidentST":
						displayStrings[i]=ClaimCur.AccidentST;
						break;
					case "BillingDentist":
						Provider P=Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvBill)];
						displayStrings[i]=P.FName+" "+P.MI+" "+P.LName+" "+P.Suffix;
						break;
					case "BillingDentistAddress":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeAddress"]).ValueString;
						else
							displayStrings[i]=clinic.Address;
						break;
					case "BillingDentistAddress2":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeAddress2"]).ValueString;
						else
							displayStrings[i]=clinic.Address2;
						break;
					case "BillingDentistCity":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeCity"]).ValueString;
						else
							displayStrings[i]=clinic.City;
						break;
					case "BillingDentistST":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeST"]).ValueString;
						else
							displayStrings[i]=clinic.State;
						break;
					case "BillingDentistZip":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeZip"]).ValueString;
						else
							displayStrings[i]=clinic.Zip;
						break;
					case "BillingDentistMedicaidID":
						displayStrings[i]=Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvBill)].MedicaidID;
						break;
					case "BillingDentistProviderID":
						ProviderIdent[] provIdents=ProviderIdents.GetForPayor(ClaimCur.ProvBill,carrier.ElectID);
						if(provIdents.Length>0){
							displayStrings[i]=provIdents[0].IDNumber;//just use the first one we find
						}
						break;
					case "BillingDentistNPI":
						displayStrings[i]=Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvBill)].NationalProvID;
						break;
					case "BillingDentistLicenseNum":
						displayStrings[i]=Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvBill)].StateLicense;
						break;
					case "BillingDentistSSNorTIN":
						displayStrings[i]=Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvBill)].SSN;
						break;
					case "BillingDentistNumIsSSN":
						if(!Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvBill)].UsingTIN)
							displayStrings[i]="X";
						break;
					case "BillingDentistNumIsTIN":
						if(Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvBill)].UsingTIN)
							displayStrings[i]="X";
						break;
					case "BillingDentistPh123":
						if(clinic==null){
							if(((Pref)PrefB.HList["PracticePhone"]).ValueString.Length==10){
								displayStrings[i]=((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(0,3);
							}
						}
						else{
							if(clinic.Phone.Length==10){
								displayStrings[i]=clinic.Phone.Substring(0,3);
							}
						}
						break;
					case "BillingDentistPh456":
						if(clinic==null){
							if(((Pref)PrefB.HList["PracticePhone"]).ValueString.Length==10){
								displayStrings[i]=((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(3,3);
							}
						}
						else{
							if(clinic.Phone.Length==10){
								displayStrings[i]=clinic.Phone.Substring(3,3);
							}
						}
						break;
					case "BillingDentistPh78910":
						if(clinic==null){
							if(((Pref)PrefB.HList["PracticePhone"]).ValueString.Length==10){
								displayStrings[i]=((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(6);
							}
						}
						else{
							if(clinic.Phone.Length==10){
								displayStrings[i]=clinic.Phone.Substring(6);
							}
						}
						break;
					case "BillingDentistPhoneFormatted":
						if(clinic==null){
							if(((Pref)PrefB.HList["PracticePhone"]).ValueString.Length==10){
								displayStrings[i]="("+((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(0,3)
									+")"+((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(3,3)
									+"-"+((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(6);
							}
						}
						else{
							if(clinic.Phone.Length==10){
								displayStrings[i]="("+clinic.Phone.Substring(0,3)
									+")"+clinic.Phone.Substring(3,3)
									+"-"+clinic.Phone.Substring(6);
							}
						}
						break;
					case "BillingDentistPhoneRaw":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticePhone"]).ValueString;
						else
							displayStrings[i]=clinic.Phone;
						break;
					case "TreatingDentistSignature":
						if(treatDent.SigOnFile){
							displayStrings[i]=treatDent.FName+" "+treatDent.MI+" "+treatDent.LName+" "
								+treatDent.Suffix+" SOF";
						}
						break;
					case "TreatingDentistSigDate":
						if(treatDent.SigOnFile && ClaimCur.DateSent.Year > 1860){
							if(ClaimFormCur.Items[i].FormatString=="")
								displayStrings[i]=ClaimCur.DateSent.ToShortDateString();
							else
								displayStrings[i]=ClaimCur.DateSent.ToString
									(ClaimFormCur.Items[i].FormatString);
						}
						break;
					case "TreatingDentistMedicaidID":
						displayStrings[i]=treatDent.MedicaidID;
						break;
					case "TreatingDentistProviderID":
						provIdents=ProviderIdents.GetForPayor(ClaimCur.ProvTreat,carrier.ElectID);
						if(provIdents.Length>0) {
							displayStrings[i]=provIdents[0].IDNumber;//just use the first one we find
						}
						break;
					case "TreatingDentistNPI":
						displayStrings[i]=treatDent.NationalProvID;
						break;
					case "TreatingDentistLicense":
						displayStrings[i]=treatDent.StateLicense;
						break;
					case "TreatingDentistAddress":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeAddress"]).ValueString;
						else
							displayStrings[i]=clinic.Address;
						break;
					case "TreatingDentistCity":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeCity"]).ValueString;
						else
							displayStrings[i]=clinic.City;
						break;
					case "TreatingDentistST":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeST"]).ValueString;
						else
							displayStrings[i]=clinic.State;
						break;
					case "TreatingDentistZip":
						if(clinic==null)
							displayStrings[i]=((Pref)PrefB.HList["PracticeZip"]).ValueString;
						else
							displayStrings[i]=clinic.Zip;
						break;
					case "TreatingDentistPh123":
						if(clinic==null){
							if(((Pref)PrefB.HList["PracticePhone"]).ValueString.Length==10){
								displayStrings[i]=((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(0,3);
							}
						}
						else{
							if(clinic.Phone.Length==10){
								displayStrings[i]=clinic.Phone.Substring(0,3);
							}
						}
						break;
					case "TreatingDentistPh456":
						if(clinic==null){
							if(((Pref)PrefB.HList["PracticePhone"]).ValueString.Length==10){
								displayStrings[i]=((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(3,3);
							}
						}
						else{
							if(clinic.Phone.Length==10){
								displayStrings[i]=clinic.Phone.Substring(3,3);
							}
						}
						break;
					case "TreatingDentistPh78910":
						if(clinic==null){
							if(((Pref)PrefB.HList["PracticePhone"]).ValueString.Length==10){
								displayStrings[i]=((Pref)PrefB.HList["PracticePhone"]).ValueString.Substring(6);
							}
						}
						else{
							if(clinic.Phone.Length==10){
								displayStrings[i]=clinic.Phone.Substring(6);
							}
						}
						break;
					case "TreatingProviderSpecialty":
						displayStrings[i]=Eclaims.X12.GetTaxonomy
							(Providers.ListLong[Providers.GetIndexLong(ClaimCur.ProvTreat)].Specialty);
						break;
				}//switch
				if(CultureInfo.CurrentCulture.Name=="nl-BE"//Dutch Belgium
					&& displayStrings[i]=="")
				{
					displayStrings[i]="*   *   *";
				}
			}//for
		}
	
		/// <summary></summary>
		private string GetPlaceOfServiceNum(PlaceOfService place){
			switch(place){
				default:
					return "";
				case PlaceOfService.AdultLivCareFac:
					return "33";//aka Custodial care facility
				case PlaceOfService.InpatHospital:
					return "21";
				case PlaceOfService.Office:
					return "11";
				case PlaceOfService.OutpatHospital:
					return "22";
				case PlaceOfService.PatientsHome:
					return "12";
				case PlaceOfService.SkilledNursFac:
					return "31";
				case PlaceOfService.MobileUnit:
					return "15";
				case PlaceOfService.School:
					return "03";
				case PlaceOfService.MilitaryTreatFac:
					return "26";
				case PlaceOfService.FederalHealthCenter:
					return "50";
				case PlaceOfService.PublicHealthClinic:
					return "71";
				case PlaceOfService.RuralHealthClinic:
					return "72";
			}
		}

		/// <summary></summary>
		/// <param name="startProc">For page 1, this will be 0, otherwise it might be 10, 8, 20, or whatever.  It is the 0-based index of the first proc. Depends on how many procedures this claim format can display and which page we are on.</param>
		/// <param name="totProcs">The number of procedures that can be displayed or printed per claim form.  Depends on the individual claim format. For example, 10 on the ADA2002</param>
		private void FillProcStrings(int startProc,int totProcs){
			for(int i=0;i<ClaimFormCur.Items.Length;i++){
				if(ClaimFormCur.Items[i]==null){//Renaissance does not use [0]
					continue;
				}
				switch(ClaimFormCur.Items[i].FieldName){
					//there is no default, because any non-matches will remain as ""
					case "P1Date":
						displayStrings[i]=GetProcInfo("Date",1+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P1Area":
						displayStrings[i]=GetProcInfo("Area",1+startProc);
						break;
					case "P1System":
						displayStrings[i]=GetProcInfo("System",1+startProc);
						break;
					case "P1ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",1+startProc);
						break;
					case "P1Surface":
						displayStrings[i]=GetProcInfo("Surface",1+startProc);
						break;
					case "P1Code":
						displayStrings[i]=GetProcInfo("Code",1+startProc);
						break;
					case "P1Description":
						displayStrings[i]=GetProcInfo("Desc",1+startProc);
						break;
					case "P1Fee":
						displayStrings[i]=GetProcInfo("Fee",1+startProc);
						break;
					case "P1TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",1+startProc);
						break;
					case "P1PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",1+startProc);
						break;
					case "P1Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",1+startProc);
						break;
					case "P1Lab":
						displayStrings[i]=GetProcInfo("Lab",1+startProc);
						break;
					case "P1FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",1+startProc);
						break;
					case "P1ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",1+startProc);
						break;
					case "P1TreatProvNPI":
						displayStrings[i]=GetProcInfo("TreatProvNPI",1+startProc);
						break;
					case "P2Date":
						displayStrings[i]=GetProcInfo("Date",2+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P2Area":
						displayStrings[i]=GetProcInfo("Area",2+startProc);
						break;
					case "P2System":
						displayStrings[i]=GetProcInfo("System",2+startProc);
						break;
					case "P2ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",2+startProc);
						break;
					case "P2Surface":
						displayStrings[i]=GetProcInfo("Surface",2+startProc);
						break;
					case "P2Code":
						displayStrings[i]=GetProcInfo("Code",2+startProc);
						break;
					case "P2Description":
						displayStrings[i]=GetProcInfo("Desc",2+startProc);
						break;
					case "P2Fee":
						displayStrings[i]=GetProcInfo("Fee",2+startProc);
						break;
					case "P2TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",2+startProc);
						break;
					case "P2PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",2+startProc);
						break;
					case "P2Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",2+startProc);
						break;
					case "P2Lab":
						displayStrings[i]=GetProcInfo("Lab",2+startProc);
						break;
					case "P2FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",2+startProc);
						break;
					case "P2ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",2+startProc);
						break;
					case "P2TreatProvNPI":
						displayStrings[i]=GetProcInfo("TreatProvNPI",2+startProc);
						break;
					case "P3Date":
						displayStrings[i]=GetProcInfo("Date",3+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P3Area":
						displayStrings[i]=GetProcInfo("Area",3+startProc);
						break;
					case "P3System":
						displayStrings[i]=GetProcInfo("System",3+startProc);
						break;
					case "P3ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",3+startProc);
						break;
					case "P3Surface":
						displayStrings[i]=GetProcInfo("Surface",3+startProc);
						break;
					case "P3Code":
						displayStrings[i]=GetProcInfo("Code",3+startProc);
						break;
					case "P3Description":
						displayStrings[i]=GetProcInfo("Desc",3+startProc);
						break;
					case "P3Fee":
						displayStrings[i]=GetProcInfo("Fee",3+startProc);
						break;
					case "P3TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",3+startProc);
						break;
					case "P3PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",3+startProc);
						break;
					case "P3Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",3+startProc);
						break;
					case "P3Lab":
						displayStrings[i]=GetProcInfo("Lab",3+startProc);
						break;
					case "P3FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",3+startProc);
						break;
					case "P3ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",3+startProc);
						break;
					case "P3TreatProvNPI":
						displayStrings[i]=GetProcInfo("TreatProvNPI",3+startProc);
						break;
					case "P4Date":
						displayStrings[i]=GetProcInfo("Date",4+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P4Area":
						displayStrings[i]=GetProcInfo("Area",4+startProc);
						break;
					case "P4System":
						displayStrings[i]=GetProcInfo("System",4+startProc);
						break;
					case "P4ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",4+startProc);
						break;
					case "P4Surface":
						displayStrings[i]=GetProcInfo("Surface",4+startProc);
						break;
					case "P4Code":
						displayStrings[i]=GetProcInfo("Code",4+startProc);
						break;
					case "P4Description":
						displayStrings[i]=GetProcInfo("Desc",4+startProc);
						break;
					case "P4Fee":
						displayStrings[i]=GetProcInfo("Fee",4+startProc);
						break;
					case "P4TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",4+startProc);
						break;
					case "P4PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",4+startProc);
						break;
					case "P4Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",4+startProc);
						break;
					case "P4Lab":
						displayStrings[i]=GetProcInfo("Lab",4+startProc);
						break;
					case "P4FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",4+startProc);
						break;
					case "P4ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",4+startProc);
						break;
					case "P4TreatProvNPI":
						displayStrings[i]=GetProcInfo("TreatProvNPI",4+startProc);
						break;
					case "P5Date":
						displayStrings[i]=GetProcInfo("Date",5+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P5Area":
						displayStrings[i]=GetProcInfo("Area",5+startProc);
						break;
					case "P5System":
						displayStrings[i]=GetProcInfo("System",5+startProc);
						break;
					case "P5ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",5+startProc);
						break;
					case "P5Surface":
						displayStrings[i]=GetProcInfo("Surface",5+startProc);
						break;
					case "P5Code":
						displayStrings[i]=GetProcInfo("Code",5+startProc);
						break;
					case "P5Description":
						displayStrings[i]=GetProcInfo("Desc",5+startProc);
						break;
					case "P5Fee":
						displayStrings[i]=GetProcInfo("Fee",5+startProc);
						break;
					case "P5TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",5);
						break;
					case "P5PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",5+startProc);
						break;
					case "P5Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",5+startProc);
						break;
					case "P5Lab":
						displayStrings[i]=GetProcInfo("Lab",5+startProc);
						break;
					case "P5FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",5+startProc);
						break;
					case "P5ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",5+startProc);
						break;
					case "P5TreatProvNPI":
						displayStrings[i]=GetProcInfo("TreatProvNPI",5+startProc);
						break;
					case "P6Date":
						displayStrings[i]=GetProcInfo("Date",6+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P6Area":
						displayStrings[i]=GetProcInfo("Area",6+startProc);
						break;
					case "P6System":
						displayStrings[i]=GetProcInfo("System",6+startProc);
						break;
					case "P6ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",6+startProc);
						break;
					case "P6Surface":
						displayStrings[i]=GetProcInfo("Surface",6+startProc);
						break;
					case "P6Code":
						displayStrings[i]=GetProcInfo("Code",6+startProc);
						break;
					case "P6Description":
						displayStrings[i]=GetProcInfo("Desc",6+startProc);
						break;
					case "P6Fee":
						displayStrings[i]=GetProcInfo("Fee",6+startProc);
						break;
					case "P6TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",6+startProc);
						break;
					case "P6PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",6+startProc);
						break;
					case "P6Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",6+startProc);
						break;
					case "P6Lab":
						displayStrings[i]=GetProcInfo("Lab",6+startProc);
						break;
					case "P6FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",6+startProc);
						break;
					case "P6ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",6+startProc);
						break;
					case "P6TreatProvNPI":
						displayStrings[i]=GetProcInfo("TreatProvNPI",6+startProc);
						break;
					case "P7Date":
						displayStrings[i]=GetProcInfo("Date",7+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P7Area":
						displayStrings[i]=GetProcInfo("Area",7+startProc);
						break;
					case "P7System":
						displayStrings[i]=GetProcInfo("System",7+startProc);
						break;
					case "P7ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",7+startProc);
						break;
					case "P7Surface":
						displayStrings[i]=GetProcInfo("Surface",7+startProc);
						break;
					case "P7Code":
						displayStrings[i]=GetProcInfo("Code",7+startProc);
						break;
					case "P7Description":
						displayStrings[i]=GetProcInfo("Desc",7+startProc);
						break;
					case "P7Fee":
						displayStrings[i]=GetProcInfo("Fee",7+startProc);
						break;
					case "P7TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",7+startProc);
						break;
					case "P7PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",7+startProc);
						break;
					case "P7Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",7+startProc);
						break;
					case "P7Lab":
						displayStrings[i]=GetProcInfo("Lab",7+startProc);
						break;
					case "P7FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",7+startProc);
						break;
					case "P7ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",7+startProc);
						break;
					case "P8Date":
						displayStrings[i]=GetProcInfo("Date",8+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P8Area":
						displayStrings[i]=GetProcInfo("Area",8+startProc);
						break;
					case "P8System":
						displayStrings[i]=GetProcInfo("System",8+startProc);
						break;
					case "P8ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",8+startProc);
						break;
					case "P8Surface":
						displayStrings[i]=GetProcInfo("Surface",8+startProc);
						break;
					case "P8Code":
						displayStrings[i]=GetProcInfo("Code",8+startProc);
						break;
					case "P8Description":
						displayStrings[i]=GetProcInfo("Desc",8+startProc);
						break;
					case "P8Fee":
						displayStrings[i]=GetProcInfo("Fee",8+startProc);
						break;
					case "P8TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",8+startProc);
						break;
					case "P8PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",8+startProc);
						break;
					case "P8Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",8+startProc);
						break;
					case "P8Lab":
						displayStrings[i]=GetProcInfo("Lab",8+startProc);
						break;
					case "P8FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",8+startProc);
						break;
					case "P8ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",8+startProc);
						break;
					case "P9Date":
						displayStrings[i]=GetProcInfo("Date",9+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P9Area":
						displayStrings[i]=GetProcInfo("Area",9+startProc);
						break;
					case "P9System":
						displayStrings[i]=GetProcInfo("System",9+startProc);
						break;
					case "P9ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",9+startProc);
						break;
					case "P9Surface":
						displayStrings[i]=GetProcInfo("Surface",9+startProc);
						break;
					case "P9Code":
						displayStrings[i]=GetProcInfo("Code",9+startProc);
						break;
					case "P9Description":
						displayStrings[i]=GetProcInfo("Desc",9+startProc);
						break;
					case "P9Fee":
						displayStrings[i]=GetProcInfo("Fee",9+startProc);
						break;
					case "P9TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",9+startProc);
						break;
					case "P9PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",9+startProc);
						break;
					case "P9Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",9+startProc);
						break;
					case "P9Lab":
						displayStrings[i]=GetProcInfo("Lab",9+startProc);
						break;
					case "P9FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",9+startProc);
						break;
					case "P9ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",9+startProc);
						break;
					case "P10Date":
						displayStrings[i]=GetProcInfo("Date",10+startProc,ClaimFormCur.Items[i].FormatString);
						break;
					case "P10Area":
						displayStrings[i]=GetProcInfo("Area",10+startProc);
						break;
					case "P10System":
						displayStrings[i]=GetProcInfo("System",10+startProc);
						break;
					case "P10ToothNumber":
						displayStrings[i]=GetProcInfo("ToothNum",10+startProc);
						break;
					case "P10Surface":
						displayStrings[i]=GetProcInfo("Surface",10+startProc);
						break;
					case "P10Code":
						displayStrings[i]=GetProcInfo("Code",10+startProc);
						break;
					case "P10Description":
						displayStrings[i]=GetProcInfo("Desc",10+startProc);
						break;
					case "P10Fee":
						displayStrings[i]=GetProcInfo("Fee",10+startProc);
						break;
					case "P10TreatDentMedicaidID":
						displayStrings[i]=GetProcInfo("TreatDentMedicaidID",10+startProc);
						break;
					case "P10PlaceNumericCode":
						displayStrings[i]=GetProcInfo("PlaceNumericCode",10+startProc);
						break;
					case "P10Diagnosis":
						displayStrings[i]=GetProcInfo("Diagnosis",10+startProc);
						break;
					case "P10Lab":
						displayStrings[i]=GetProcInfo("Lab",10+startProc);
						break;
					case "P10FeeMinusLab":
						displayStrings[i]=GetProcInfo("FeeMinusLab",10+startProc);
						break;
					case "P10ToothNumOrArea":
						displayStrings[i]=GetProcInfo("ToothNumOrArea",10+startProc);
						break;
					case "TotalFee":
						double fee=0;//fee only for this page. Each page is treated like a separate claim.
						for(int f=startProc;f<startProc+totProcs;f++){//eg f=0;f<10;f++
							if(f < claimprocs.Count)
								fee+=((ClaimProc)claimprocs[f]).FeeBilled;
						}
						displayStrings[i]=fee.ToString("F");
						break;
					case "DateOfService"://only for this page, Earliest proc date.
						DateTime dateService=((ClaimProc)claimprocs[0]).ProcDate;
						for(int f=startProc;f<startProc+totProcs;f++){//eg f=0;f<10;f++
							if(f < claimprocs.Count && ((ClaimProc)claimprocs[f]).ProcDate < dateService)
								dateService=((ClaimProc)claimprocs[f]).ProcDate;
						}
						if(ClaimFormCur.Items[i].FormatString=="")
							displayStrings[i]=dateService.ToShortDateString();
						else
							displayStrings[i]=dateService.ToString(ClaimFormCur.Items[i].FormatString);
						break;
				}//switch
				if(CultureInfo.CurrentCulture.Name=="nl-BE"//Dutch Belgium
					&& displayStrings[i]=="")
				{
					displayStrings[i]="*   *   *";
				}
			}//for i
		}

		/// <summary>Uses the fee field to determine how many procedures this claim will print.</summary>
		/// <returns></returns>
		private int ProcLimitForFormat(){
			int retVal=0;
			//loop until a match is not found.  The max of 10 is built in because of course it will never match to 11 since there is no such fieldName.
			for(int i=0;i<15;i++){
				for(int ii=0;ii<ClaimFormCur.Items.Length;ii++){
					if(ClaimFormCur.Items[ii].FieldName=="P"+i.ToString()+"Fee"){
						retVal=i;
					}
				}//for ii
			}
			if(retVal==0){//if claimform doesn't use fees, use procedurecode
				for(int i=0;i<15;i++){
					for(int ii=0;ii<ClaimFormCur.Items.Length;ii++){
						if(ClaimFormCur.Items[ii].FieldName=="P"+i.ToString()+"Code"){
							retVal=i;
						}
					}//for ii
				}
			}
			if(retVal==0){//if STILL zero
				retVal=10;
			}
			return retVal;
		}

		/// <summary>Overload that does not need a stringFormat</summary>
		/// <param name="field"></param>
		/// <param name="procIndex"></param>
		/// <returns></returns>
		private string GetProcInfo(string field,int procIndex){
			return GetProcInfo(field,procIndex,"");
		}

		/// <summary>Gets the string to be used for this field and index.</summary>
		/// <param name="field"></param>
		/// <param name="procIndex"></param>
		/// <param name="stringFormat"></param>
		/// <returns></returns>
		private string GetProcInfo(string field,int procIndex, string stringFormat){
			//remember that procIndex is 1 based, not 0 based, 
			procIndex--;//so convert to 0 based
			if(claimprocs.Count <= procIndex){
				//if(CultureInfo.CurrentCulture.Name=="nl-BE"){//Dutch Belgium
				//	return"*   *   *";
				//}
				//else{
				return "";
				//}
			}
			if(field=="System")
				return "JP";
			if(field=="Code")
				return claimprocs[procIndex].CodeSent;
			if(field=="System")
				return "JP";
			if(field=="Fee"){
				return claimprocs[procIndex].FeeBilled.ToString("F");
			}
			Procedure ProcCur=Procedures.GetProc(ProcList,claimprocs[procIndex].ProcNum);
			ProcedureCode procCode=ProcedureCodes.GetProcCode(ProcCur.ADACode);
			if(field=="Desc")
				if(procCode.TreatArea==TreatmentArea.Quad){
					return ProcCur.Surf+" "+procCode.Descript;
				}
				else{
					return procCode.Descript;
				}
			if(field=="Date"){
				if(ClaimCur.ClaimType=="PreAuth")//no date on preauth procedures
					return "";
				if(stringFormat=="")
					return claimprocs[procIndex].ProcDate.ToShortDateString();
				return claimprocs[procIndex].ProcDate.ToString(stringFormat);
			}
			if(field=="TreatDentMedicaidID"){
				if(claimprocs[procIndex].ProvNum==0){
					return "";
				}
				else return Providers.ListLong[Providers.GetIndexLong(claimprocs[procIndex].ProvNum)].MedicaidID;
			}
			if(field=="TreatProvNPI"){
				if(claimprocs[procIndex].ProvNum==0) {
					return "";
				}
				else
					return Providers.ListLong[Providers.GetIndexLong(claimprocs[procIndex].ProvNum)].NationalProvID;
			}
			if(field=="PlaceNumericCode"){
				return GetPlaceOfServiceNum(ClaimCur.PlaceService);
			}
			//(Procedure)Procedures.HList[ClaimProcsForClaim[procIndex].ProcNum];
			//Procedure ProcOld=ProcCur.Copy();
			if(field=="Diagnosis"){
				if(ProcCur.DiagnosticCode==""){
					return "";
				}
				//string diagstr="";//would concat them together, but OD only allows one diagnosis per code anyway.
				for(int d=0;d<diagnoses.Length;d++){
					if(diagnoses[d]==ProcCur.DiagnosticCode){
						return (d+1).ToString();
					}
				}
				return ProcCur.DiagnosticCode;
			}
			if(field=="Lab"){// && ProcCur.LabFee>0) {
				return "";//ProcCur.LabFee.ToString("n");
			}
			if(field=="FeeMinusLab") {
				return "";//(((ClaimProc)claimprocs[procIndex]).FeeBilled-ProcCur.LabFee).ToString("n");
			}
			string area="";
			string toothNum="";
			string surf="";
			switch(((ProcedureCode)ProcedureCodes.HList[ProcCur.ADACode]).TreatArea){
				case TreatmentArea.Surf:
					//area blank
					toothNum=Tooth.ToInternat(ProcCur.ToothNum);
					surf=Tooth.SurfTidy(ProcCur.Surf,ProcCur.ToothNum,true);
					break;
				case TreatmentArea.Tooth:
					//area blank
					toothNum=Tooth.ToInternat(ProcCur.ToothNum);
					//surf blank
					break;
				case TreatmentArea.Quad:
					area=AreaToCode(ProcCur.Surf);//"UL" etc -> 20 etc
					//num blank
					//surf blank
					break;
				case TreatmentArea.Sextant:
					area="";//leave it blank.  Never used anyway.
					//area="S"+ProcCur.Surf;//area
					//num blank
					//surf blank
					break;
				case TreatmentArea.Arch:
					area=AreaToCode(ProcCur.Surf);//area "U", etc
					//num blank
					//surf blank
					break;
				case TreatmentArea.ToothRange:
					//area blank
					toothNum="";
					for(int i=0;i<ProcCur.ToothRange.Split(',').Length;i++){
						if(!Tooth.IsValidDB(ProcCur.ToothRange.Split(',')[i])){
							continue;
						}
						if(i>0){
							toothNum+=",";
						}
						toothNum+=Tooth.ToInternat(ProcCur.ToothRange.Split(',')[i]);
					}
					//surf blank
					break;
				default://mouth
					//area?
					break;
			}//switch treatarea
      switch(field){
				case "Area":
					return area;
				case "ToothNum":
					return toothNum;
				case "Surface":
					return surf;
				case "ToothNumOrArea":
					if(toothNum!=""){
						return toothNum;
					}
					else{
						return area;
					}
			}
			MessageBox.Show("error in getprocinfo");
			return "";//should never get to here
		}

		private string AreaToCode(string area){
			switch(area){
				case "U":
					return "01";
				case "L":
					return "02";
				case "UR":
					return "10";
				case "UL":
					return "20";
				case "LL":
					return "30";
				case "LR":
					return "40";
			}
			return "";
		}

		private void butBack_Click(object sender, System.EventArgs e){
			if(Preview2.StartPage==0) return;
			Preview2.StartPage--;
			labelTotPages.Text=(Preview2.StartPage+1).ToString()
				+" / "+totalPages.ToString();
		}

		private void butFwd_Click(object sender, System.EventArgs e){
			if(Preview2.StartPage==totalPages-1) return;
			Preview2.StartPage++;
			labelTotPages.Text=(Preview2.StartPage+1).ToString()
				+" / "+totalPages.ToString();
		}

		private void butPrint_Click(object sender, System.EventArgs e){
			if(PrintClaim()){
				Etranss.SetClaimSentOrPrinted(ThisClaimNum,ClaimCur.PatNum,0,EtransType.ClaimPrinted);
				//Claims.UpdateStatus(ThisClaimNum,"P");
				DialogResult=DialogResult.OK;
			}
			else{
				DialogResult=DialogResult.Cancel;
			}
			//Close();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.OK;
		}


	}

}
















