using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormApptsOther : System.Windows.Forms.Form{
		private System.Windows.Forms.CheckBox checkDone;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;
		private OpenDental.TableApptsOther tbApts;
		///<summary>The result of the window.  In other words, which button was clicked to exit the window.</summary>
		public OtherResult oResult;
		private System.Windows.Forms.TextBox textApptModNote;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butGoTo;
		private OpenDental.UI.Button butPin;
		private OpenDental.UI.Button butNew;
		private System.Windows.Forms.Label label2;
		///<summary>True if user double clicked on a blank area of appt module to get to this point.</summary>
		public bool InitialClick;
		private System.Windows.Forms.ListView listFamily;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private Appointment[] ListOth;
		private Recall[] RecallList;
		private Patient PatCur;
		private OpenDental.UI.Button butOK;
		private Family FamCur;
		///<summary>Almost always false.  Only set to true from TaskList to allow selecting one appointment for a patient.</summary>
		public bool SelectOnly;
		private OpenDental.UI.Button butRecall;
		///<summary>This will contain a selected appointment upon closing of the form in some situations.  Used when picking an appointment for task lists.  Also used if the GoTo or Create new buttons are clicked.</summary>
		public Appointment SelectedAppt;
		///<summary>If oResult=PinboardAndSearch, then when closing this form, this will contain the date to jump to when beginning the search.</summary>
		public string DateJumpToString;

		///<summary></summary>
		public FormApptsOther(Patient pat,Family fam){
			InitializeComponent();
			PatCur=pat;
			FamCur=fam;
			tbApts.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(tbApts_CellDoubleClicked);
			Lan.F(this);
			for(int i=0;i<listFamily.Columns.Count;i++){
				listFamily.Columns[i].Text=Lan.g(this,listFamily.Columns[i].Text);
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FormApptsOther));
			this.checkDone = new System.Windows.Forms.CheckBox();
			this.tbApts = new OpenDental.TableApptsOther();
			this.butCancel = new OpenDental.UI.Button();
			this.textApptModNote = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butGoTo = new OpenDental.UI.Button();
			this.butPin = new OpenDental.UI.Button();
			this.butNew = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.listFamily = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.butOK = new OpenDental.UI.Button();
			this.butRecall = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// checkDone
			// 
			this.checkDone.AutoCheck = false;
			this.checkDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.checkDone.Location = new System.Drawing.Point(29, 145);
			this.checkDone.Name = "checkDone";
			this.checkDone.Size = new System.Drawing.Size(210, 16);
			this.checkDone.TabIndex = 1;
			this.checkDone.TabStop = false;
			this.checkDone.Text = "Planned Appt Done";
			// 
			// tbApts
			// 
			this.tbApts.BackColor = System.Drawing.SystemColors.Window;
			this.tbApts.Location = new System.Drawing.Point(28, 168);
			this.tbApts.Name = "tbApts";
			this.tbApts.ScrollValue = 1;
			this.tbApts.SelectedIndices = new int[0];
			this.tbApts.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.tbApts.Size = new System.Drawing.Size(769, 404);
			this.tbApts.TabIndex = 2;
			this.tbApts.TabStop = false;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.butCancel.Location = new System.Drawing.Point(834, 618);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textApptModNote
			// 
			this.textApptModNote.BackColor = System.Drawing.Color.White;
			this.textApptModNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textApptModNote.ForeColor = System.Drawing.Color.Red;
			this.textApptModNote.Location = new System.Drawing.Point(594, 33);
			this.textApptModNote.Multiline = true;
			this.textApptModNote.Name = "textApptModNote";
			this.textApptModNote.ReadOnly = true;
			this.textApptModNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textApptModNote.Size = new System.Drawing.Size(202, 36);
			this.textApptModNote.TabIndex = 44;
			this.textApptModNote.Text = "";
			// 
			// label1
			// 
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(429, 37);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(163, 21);
			this.label1.TabIndex = 45;
			this.label1.Text = "Appointment Module Note";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butGoTo
			// 
			this.butGoTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoTo.Autosize = true;
			this.butGoTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoTo.Image = ((System.Drawing.Image)(resources.GetObject("butGoTo.Image")));
			this.butGoTo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGoTo.Location = new System.Drawing.Point(171, 618);
			this.butGoTo.Name = "butGoTo";
			this.butGoTo.Size = new System.Drawing.Size(106, 26);
			this.butGoTo.TabIndex = 46;
			this.butGoTo.Text = "&Go To Appt";
			this.butGoTo.Click += new System.EventHandler(this.butGoTo_Click);
			// 
			// butPin
			// 
			this.butPin.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPin.Autosize = true;
			this.butPin.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPin.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPin.Image = ((System.Drawing.Image)(resources.GetObject("butPin.Image")));
			this.butPin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPin.Location = new System.Drawing.Point(292, 618);
			this.butPin.Name = "butPin";
			this.butPin.Size = new System.Drawing.Size(134, 26);
			this.butPin.TabIndex = 47;
			this.butPin.Text = "Copy To &Pinboard";
			this.butPin.Click += new System.EventHandler(this.butPin_Click);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.Image = ((System.Drawing.Image)(resources.GetObject("butNew.Image")));
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(581, 618);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(106, 26);
			this.butNew.TabIndex = 48;
			this.butNew.Text = "Create &New";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(29, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(168, 17);
			this.label2.TabIndex = 57;
			this.label2.Text = "Recall for Family";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listFamily
			// 
			this.listFamily.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																								 this.columnHeader1,
																																								 this.columnHeader2,
																																								 this.columnHeader4,
																																								 this.columnHeader3,
																																								 this.columnHeader5});
			this.listFamily.FullRowSelect = true;
			this.listFamily.GridLines = true;
			this.listFamily.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listFamily.Location = new System.Drawing.Point(29, 36);
			this.listFamily.Name = "listFamily";
			this.listFamily.Size = new System.Drawing.Size(384, 97);
			this.listFamily.TabIndex = 58;
			this.listFamily.View = System.Windows.Forms.View.Details;
			this.listFamily.DoubleClick += new System.EventHandler(this.listFamily_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Family Member";
			this.columnHeader1.Width = 120;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Age";
			this.columnHeader2.Width = 40;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Gender";
			this.columnHeader4.Width = 50;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Due Date";
			this.columnHeader3.Width = 74;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Scheduled";
			this.columnHeader5.Width = 74;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.butOK.Location = new System.Drawing.Point(748, 618);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 59;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butRecall
			// 
			this.butRecall.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecall.Autosize = true;
			this.butRecall.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecall.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecall.Image = ((System.Drawing.Image)(resources.GetObject("butRecall.Image")));
			this.butRecall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRecall.Location = new System.Drawing.Point(441, 618);
			this.butRecall.Name = "butRecall";
			this.butRecall.Size = new System.Drawing.Size(125, 26);
			this.butRecall.TabIndex = 60;
			this.butRecall.Text = "Schedule Recall";
			this.butRecall.Click += new System.EventHandler(this.butRecall_Click);
			// 
			// FormApptsOther
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(924, 658);
			this.Controls.Add(this.butRecall);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.listFamily);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butNew);
			this.Controls.Add(this.butPin);
			this.Controls.Add(this.butGoTo);
			this.Controls.Add(this.textApptModNote);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbApts);
			this.Controls.Add(this.checkDone);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormApptsOther";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Other Appointments";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormApptsOther_Closing);
			this.Load += new System.EventHandler(this.FormApptsOther_Load);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary></summary>
		public OtherResult OResult{
			get{return oResult;}
		}

		private void FormApptsOther_Load(object sender, System.EventArgs e) {
			Text=Lan.g(this,"Appointments for")+" "+PatCur.GetNameLF();
			textApptModNote.Text=PatCur.ApptModNote;
			if(SelectOnly){
				butGoTo.Visible=false;
				butPin.Visible=false;
				butNew.Visible=false;
				label2.Visible=false;
				listFamily.Visible=false;
			}
			else{//much more typical
				butOK.Visible=false;
			}
			Filltb();
			if(PatCur.PatStatus==PatientStatus.Inactive
				|| PatCur.PatStatus==PatientStatus.Archived)
			{
				MsgBox.Show(this,"Warning. Patient is not active.");
			}
			if(PatCur.PatStatus==PatientStatus.Deceased){
				MsgBox.Show(this,"Warning. Patient is deceased.");
			}
		}

		private void Filltb(){
			RecallList=Recalls.GetList(FamCur.List);
			Appointment[] aptsOnePat;
			listFamily.Items.Clear();
			ListViewItem item;
			DateTime dateDue;
			for(int i=0;i<FamCur.List.Length;i++){
				item=new ListViewItem(FamCur.GetNameInFamFLI(i));
				if(FamCur.List[i].PatNum==PatCur.PatNum){
					item.BackColor=Color.Silver;
				}
				item.SubItems.Add(FamCur.List[i].Age.ToString());
				item.SubItems.Add(FamCur.List[i].Gender.ToString());
				dateDue=DateTime.MinValue;
				for(int j=0;j<RecallList.Length;j++){
					if(RecallList[j].PatNum==FamCur.List[i].PatNum){
						dateDue=RecallList[j].DateDue;
					}
				}
				if(dateDue.Year<1880){
					item.SubItems.Add("");
				}
				else{
					item.SubItems.Add(dateDue.ToShortDateString());
				}
				if(dateDue<=DateTime.Today){
					item.ForeColor=Color.Red;
				}
				aptsOnePat=Appointments.GetForPat(FamCur.List[i].PatNum);
				for(int a=0;a<aptsOnePat.Length;a++){
					if(aptsOnePat[a].AptDateTime.Date<=DateTime.Today){
						continue;//disregard old appts.
					}
					item.SubItems.Add(aptsOnePat[a].AptDateTime.ToShortDateString());
					break;//we only want one appt
					//could add condition here to add blank subitem if no date found
				}
				listFamily.Items.Add(item);
			}
			if(PatCur.NextAptNum==-1){ 
        checkDone.Checked=true;
      }
			else{ 
        checkDone.Checked=false;
      }
			ListOth=Appointments.GetForPat(PatCur.PatNum);
			tbApts.ResetRows(ListOth.Length);
			tbApts.SetGridColor(Color.DarkGray);
			for(int i=0;i<ListOth.Length;i++){
				tbApts.Cell[0,i]=ListOth[i].AptStatus.ToString();
				if(ListOth[i].AptDateTime.Year > 1880){
					if(ListOth[i].AptStatus!=ApptStatus.Planned){//don't show date/time for planned appts.
						tbApts.Cell[1,i]=ListOth[i].AptDateTime.ToString("d");
						tbApts.Cell[2,i]=ListOth[i].AptDateTime.ToString("t");
					}
        }
				else{
          tbApts.Cell[1,i]="";
					tbApts.Cell[2,i]="";
        }
				tbApts.Cell[3,i]=(ListOth[i].Pattern.Length*5).ToString();
				tbApts.Cell[4,i]=ListOth[i].ProcDescript;
				tbApts.Cell[5,i]=ListOth[i].Note;
			}
			tbApts.LayoutTables();
		}

		private void listFamily_DoubleClick(object sender, System.EventArgs e) {
			if(listFamily.SelectedIndices.Count==0){
				return;
			}
			int originalPatNum=PatCur.PatNum;
			Recall recallCur=null;
			for(int i=0;i<RecallList.Length;i++){
				if(RecallList[i].PatNum==FamCur.List[listFamily.SelectedIndices[0]].PatNum){
					recallCur=RecallList[i];
				}
			}
			if(recallCur==null){
				recallCur=new Recall();
				recallCur.PatNum=FamCur.List[listFamily.SelectedIndices[0]].PatNum;
				recallCur.RecallInterval=new Interval(0,0,6,0);
			}
			FormRecallListEdit FormRLE=new FormRecallListEdit(recallCur);
			FormRLE.ShowDialog();
			if(FormRLE.PinClicked){
				oResult=OtherResult.CopyToPinBoard;
				//already created curInfo in FormRE.
				DialogResult=DialogResult.OK;
			}
			else{
				FamCur=Patients.GetFamily(originalPatNum);
				PatCur=FamCur.GetPatient(originalPatNum);
				Filltb();
			}
		}

		private void butRecall_Click(object sender, System.EventArgs e) {
			Procedure[] procList=Procedures.Refresh(PatCur.PatNum);
			Recall[] recallList=Recalls.GetList(new int[] {PatCur.PatNum});//get the recall for this pt
			if(recallList.Length==0){
				MsgBox.Show(this,"This patient does not have any recall due.");
				return;
			}
			Recall recallCur=recallList[0];
			InsPlan[] planList=InsPlans.Refresh(FamCur);
			Appointment AptCur=Appointments.CreateRecallApt(PatCur,procList,recallCur,planList);
			CreateCurInfo(AptCur);
			oResult=OtherResult.PinboardAndSearch;
			if(recallCur.DateDue<DateTime.Today){
				DateJumpToString=DateTime.Today.ToShortDateString();//they are overdue
			}
			else{
				DateJumpToString=recallCur.DateDue.ToShortDateString();
			}
			DialogResult=DialogResult.OK;
		}

		private void butNew_Click(object sender, System.EventArgs e) {
			Appointment AptCur=new Appointment();
			AptCur.PatNum=PatCur.PatNum;
			if(PatCur.DateFirstVisit.Year<1880
				&& !Procedures.AreAnyComplete(PatCur.PatNum))//this only runs if firstVisit blank
			{
				AptCur.IsNewPatient=true;
			}
			AptCur.Pattern="/X/";
			if(PatCur.PriProv==0){
				AptCur.ProvNum=PIn.PInt(((Pref)PrefB.HList["PracticeDefaultProv"]).ValueString);
			}
			else{			
				AptCur.ProvNum=PatCur.PriProv;
			}
			AptCur.ProvHyg=PatCur.SecProv;
			AptCur.AptStatus=ApptStatus.Scheduled;
			AptCur.ClinicNum=PatCur.ClinicNum;
			if(InitialClick){//initially double clicked on appt module
				DateTime d=Appointments.DateSelected;
				int minutes=(int)(ContrAppt.SheetClickedonMin/ContrApptSheet.MinPerIncr)
					*ContrApptSheet.MinPerIncr;
				AptCur.AptDateTime=new DateTime(d.Year,d.Month,d.Day
					,ContrAppt.SheetClickedonHour,minutes,0);
				AptCur.Op=ContrAppt.SheetClickedonOp;
			}
			else{
				//new appt will be placed on pinboard instead of specific time
			}
			try{
				Appointments.InsertOrUpdate(AptCur,null,true);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			FormApptEdit FormApptEdit2=new FormApptEdit(AptCur);
			FormApptEdit2.IsNew=true;
			FormApptEdit2.ShowDialog();
			if(FormApptEdit2.DialogResult!=DialogResult.OK){
				return;
			}
			if(InitialClick){
				SelectedAppt=AptCur;
				oResult=OtherResult.CreateNew;
			}
			else{
				CreateCurInfo(AptCur);
				oResult=OtherResult.NewToPinBoard;
			}
			DialogResult=DialogResult.OK;
		}

		private void butPin_Click(object sender, System.EventArgs e) {
			if(tbApts.SelectedRow==-1){
				MessageBox.Show(Lan.g(this,"Please select appointment first."));
				return;
			}
			if(!OKtoSendToPinboard(ListOth[tbApts.SelectedRow]))
				return;
			CreateCurInfo(ListOth[tbApts.SelectedRow]);
			oResult=OtherResult.CopyToPinBoard;
			DialogResult=DialogResult.OK;
		}

		/// <summary>Tests the appointment to see if it is acceptable to send it to the pinboard.  Also asks user appropriate questions to verify that's what they want to do.  Returns false if it will not be going to pinboard after all.</summary>
		private bool OKtoSendToPinboard(Appointment AptCur){
			if(AptCur.AptStatus==ApptStatus.Planned){//if is a Planned appointment
				bool PlannedIsSched=false;
				for(int i=0;i<ListOth.Length;i++){
					if(ListOth[i].NextAptNum==PatCur.NextAptNum){//if the planned appointment is already sched
						PlannedIsSched=true;
					}
				}
				if(PlannedIsSched){
					if(MessageBox.Show(Lan.g(this,"The Planned appointment is already scheduled.  Do you wish to continue?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
						return false;
					}
				}
			}
			else{//if appointment is not Planned
				switch(AptCur.AptStatus){
					case ApptStatus.Complete:
						MessageBox.Show(Lan.g(this,"Not allowed to move a completed appointment from here."));
						return false;
					case ApptStatus.ASAP:
					case ApptStatus.Scheduled:
						if(MessageBox.Show(Lan.g(this,"Do you really want to move a previously scheduled appointment?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
							return false;
						}
						break;
					case ApptStatus.Broken://status gets changed after dragging off pinboard.
					case ApptStatus.None:
					case ApptStatus.UnschedList://status gets changed after dragging off pinboard.
						break;
				}			
			}
			//if it's a planned appointment, the planned appointment will end up on the pinboard.  The copy will be made after dragging it off the pinboard.
			return true;
		}

		private void tbApts_CellDoubleClicked(object sender, CellEventArgs e){
			int currentSelection=tbApts.SelectedRow;
			int currentScroll=tbApts.ScrollValue;
			FormApptEdit FormAE=new FormApptEdit(ListOth[e.Row]);
			FormAE.PinIsVisible=true;
			FormAE.ShowDialog();
			if(FormAE.DialogResult!=DialogResult.OK)
				return;
			if(FormAE.PinClicked){
				if(!OKtoSendToPinboard(ListOth[e.Row]))
					return;
				CreateCurInfo(ListOth[e.Row]);
				oResult=OtherResult.CopyToPinBoard;
				DialogResult=DialogResult.OK;
			}
			else{
				Filltb();
				tbApts.SetSelected(currentSelection,true);
				tbApts.ScrollValue=currentScroll;
			}
		}	

		///<summary>Prepares the necessary info for placement of the appointment on the pinboard.</summary>
		private void CreateCurInfo(Appointment AptCur){
			ContrAppt.CurInfo=new InfoApt();
			ContrAppt.CurInfo.MyApt=AptCur.Copy();
			Procedure[] procsForSingle;
			if(AptCur.AptNum==PatCur.NextAptNum){//if is Next apt
				procsForSingle=Procedures.GetProcsForSingle(AptCur.AptNum,true);
				ContrAppt.CurInfo.Production=Procedures.GetProductionOneApt(AptCur.AptNum,procsForSingle,true);
			}
			else{//normal apt
				procsForSingle=Procedures.GetProcsForSingle(AptCur.AptNum,false);
				ContrAppt.CurInfo.Production=Procedures.GetProductionOneApt(AptCur.AptNum,procsForSingle,false);
			}
			ContrAppt.CurInfo.Procs=procsForSingle;
			ContrAppt.CurInfo.MyPatient=PatCur.Copy();
		}

		private void butGoTo_Click(object sender, System.EventArgs e) {
			if(tbApts.SelectedRow==-1){
				MessageBox.Show(Lan.g(this,"Please select appointment first."));
				return;
			}
			if(ListOth[tbApts.SelectedRow].AptDateTime.Year<1880){
				MessageBox.Show(Lan.g(this,"Unable to go to unscheduled appointment."));
				return;
			}
			SelectedAppt=ListOth[tbApts.SelectedRow];
			oResult=OtherResult.GoTo;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//only used when selecting from TaskList. oResult is completely ignored in this case.
			//I didn't bother enabling double click. Maybe later.
			if(tbApts.SelectedRow==-1){
				MessageBox.Show(Lan.g(this,"Please select appointment first."));
				return;
			}
			SelectedAppt=ListOth[tbApts.SelectedRow];
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormApptsOther_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK)
				return;
			oResult=OtherResult.Cancel;
		}

		

		

	}
}
