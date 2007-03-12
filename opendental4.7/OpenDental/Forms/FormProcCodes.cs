/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormProcCodes : System.Windows.Forms.Form{
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		///<summary>If IsSelectionMode=true and DialogResult=OK, then this will contain the selected ADACode.</summary>
		public string SelectedADA;	
		private System.Windows.Forms.ListBox listFeeSched;
		private System.Windows.Forms.Label labelFeeSched;
		private OpenDental.UI.Button butNew;
		private OpenDental.UI.Button butEditFeeSched;
		private OpenDental.UI.Button butTools;
		private System.Windows.Forms.GroupBox groupFeeScheds;
		private bool changed;
		private ListBox listCategories;
		private Label label1;
		private Label label2;
		private GroupBox groupBox1;
		private TextBox textDescription;
		private OpenDental.UI.Button butEditCategories;
		private TextBox textCode;
		private Label label3;
		private OpenDental.UI.ODGrid gridMain;
		private CheckBox checkShowHidden;
		private DataTable ProcTable;
		private TextBox textAbbreviation;
		private Label label4;
		private OpenDental.UI.Button butAll;
		///<summary>Set to true externally in order to let user select one procedure code.</summary>
		public bool IsSelectionMode;
		private ComboBox comboCompare1;
		private Label label5;
		private ComboBox comboCompare2;
		private OpenDental.UI.Button butImport;
		private OpenDental.UI.Button butExport;
		///<summary>The list of definitions that is currently showing in the category list.</summary>
		private Def[] CatList;

		///<summary></summary>
		public FormProcCodes(){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcCodes));
			this.listFeeSched = new System.Windows.Forms.ListBox();
			this.labelFeeSched = new System.Windows.Forms.Label();
			this.groupFeeScheds = new System.Windows.Forms.GroupBox();
			this.butTools = new OpenDental.UI.Button();
			this.butEditFeeSched = new OpenDental.UI.Button();
			this.listCategories = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butAll = new OpenDental.UI.Button();
			this.textAbbreviation = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.butEditCategories = new OpenDental.UI.Button();
			this.textCode = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.comboCompare1 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboCompare2 = new System.Windows.Forms.ComboBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butNew = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.groupFeeScheds.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listFeeSched
			// 
			this.listFeeSched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.listFeeSched.Location = new System.Drawing.Point(747,24);
			this.listFeeSched.Name = "listFeeSched";
			this.listFeeSched.Size = new System.Drawing.Size(200,498);
			this.listFeeSched.TabIndex = 11;
			this.listFeeSched.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listFeeSched_MouseDown);
			// 
			// labelFeeSched
			// 
			this.labelFeeSched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelFeeSched.Location = new System.Drawing.Point(745,4);
			this.labelFeeSched.Name = "labelFeeSched";
			this.labelFeeSched.Size = new System.Drawing.Size(132,17);
			this.labelFeeSched.TabIndex = 12;
			this.labelFeeSched.Text = "View Fee Sched";
			this.labelFeeSched.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupFeeScheds
			// 
			this.groupFeeScheds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupFeeScheds.Controls.Add(this.butTools);
			this.groupFeeScheds.Controls.Add(this.butEditFeeSched);
			this.groupFeeScheds.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupFeeScheds.Location = new System.Drawing.Point(747,591);
			this.groupFeeScheds.Name = "groupFeeScheds";
			this.groupFeeScheds.Size = new System.Drawing.Size(200,51);
			this.groupFeeScheds.TabIndex = 14;
			this.groupFeeScheds.TabStop = false;
			this.groupFeeScheds.Text = "Fee Schedules";
			// 
			// butTools
			// 
			this.butTools.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butTools.Autosize = true;
			this.butTools.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTools.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTools.Location = new System.Drawing.Point(109,18);
			this.butTools.Name = "butTools";
			this.butTools.Size = new System.Drawing.Size(81,26);
			this.butTools.TabIndex = 14;
			this.butTools.Text = "Tools";
			this.butTools.Click += new System.EventHandler(this.butTools_Click);
			// 
			// butEditFeeSched
			// 
			this.butEditFeeSched.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butEditFeeSched.Autosize = true;
			this.butEditFeeSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditFeeSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditFeeSched.Location = new System.Drawing.Point(12,18);
			this.butEditFeeSched.Name = "butEditFeeSched";
			this.butEditFeeSched.Size = new System.Drawing.Size(81,26);
			this.butEditFeeSched.TabIndex = 13;
			this.butEditFeeSched.Text = "Edit Names";
			this.butEditFeeSched.Click += new System.EventHandler(this.butEditFeeSched_Click);
			// 
			// listCategories
			// 
			this.listCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listCategories.FormattingEnabled = true;
			this.listCategories.Location = new System.Drawing.Point(10,126);
			this.listCategories.Name = "listCategories";
			this.listCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listCategories.Size = new System.Drawing.Size(155,368);
			this.listCategories.TabIndex = 15;
			this.listCategories.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listCategories_MouseUp);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7,100);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108,23);
			this.label1.TabIndex = 16;
			this.label1.Text = "By Category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(1,42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91,20);
			this.label2.TabIndex = 17;
			this.label2.Text = "By Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butAll);
			this.groupBox1.Controls.Add(this.textAbbreviation);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.checkShowHidden);
			this.groupBox1.Controls.Add(this.butEditCategories);
			this.groupBox1.Controls.Add(this.textCode);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.listCategories);
			this.groupBox1.Controls.Add(this.textDescription);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(6,16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(175,556);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAll.Location = new System.Drawing.Point(103,100);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(62,25);
			this.butAll.TabIndex = 24;
			this.butAll.Text = "All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// textAbbreviation
			// 
			this.textAbbreviation.Location = new System.Drawing.Point(92,17);
			this.textAbbreviation.Name = "textAbbreviation";
			this.textAbbreviation.Size = new System.Drawing.Size(73,20);
			this.textAbbreviation.TabIndex = 23;
			this.textAbbreviation.TextChanged += new System.EventHandler(this.textAbbreviation_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(1,16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(91,20);
			this.label4.TabIndex = 22;
			this.label4.Text = "By Abbreviation";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShowHidden.AutoSize = true;
			this.checkShowHidden.Location = new System.Drawing.Point(10,533);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(90,17);
			this.checkShowHidden.TabIndex = 20;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.Click += new System.EventHandler(this.checkShowHidden_Click);
			// 
			// butEditCategories
			// 
			this.butEditCategories.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butEditCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditCategories.Autosize = true;
			this.butEditCategories.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditCategories.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditCategories.Location = new System.Drawing.Point(10,501);
			this.butEditCategories.Name = "butEditCategories";
			this.butEditCategories.Size = new System.Drawing.Size(94,26);
			this.butEditCategories.TabIndex = 21;
			this.butEditCategories.Text = "Edit Categories";
			this.butEditCategories.Click += new System.EventHandler(this.butEditCategories_Click);
			// 
			// textCode
			// 
			this.textCode.Location = new System.Drawing.Point(92,69);
			this.textCode.Name = "textCode";
			this.textCode.Size = new System.Drawing.Size(73,20);
			this.textCode.TabIndex = 20;
			this.textCode.TextChanged += new System.EventHandler(this.textCode_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(1,68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91,20);
			this.label3.TabIndex = 19;
			this.label3.Text = "By Code";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(92,43);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(73,20);
			this.textDescription.TabIndex = 18;
			this.textDescription.TextChanged += new System.EventHandler(this.textDescription_TextChanged);
			// 
			// comboCompare1
			// 
			this.comboCompare1.FormattingEnabled = true;
			this.comboCompare1.Location = new System.Drawing.Point(747,542);
			this.comboCompare1.Name = "comboCompare1";
			this.comboCompare1.Size = new System.Drawing.Size(200,21);
			this.comboCompare1.TabIndex = 20;
			this.comboCompare1.SelectionChangeCommitted += new System.EventHandler(this.comboCompare1_SelectionChangeCommitted);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(745,522);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(199,17);
			this.label5.TabIndex = 21;
			this.label5.Text = "Compare Fee Schedules";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboCompare2
			// 
			this.comboCompare2.FormattingEnabled = true;
			this.comboCompare2.Location = new System.Drawing.Point(747,565);
			this.comboCompare2.Name = "comboCompare2";
			this.comboCompare2.Size = new System.Drawing.Size(200,21);
			this.comboCompare2.TabIndex = 22;
			this.comboCompare2.SelectionChangeCommitted += new System.EventHandler(this.comboCompare2_SelectionChangeCommitted);
			// 
			// gridMain
			// 
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(187,8);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(553,674);
			this.gridMain.TabIndex = 19;
			this.gridMain.Title = "Procedures";
			this.gridMain.TranslationName = "TableProcedures";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.Image = ((System.Drawing.Image)(resources.GetObject("butNew.Image")));
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(79,656);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(102,26);
			this.butNew.TabIndex = 0;
			this.butNew.Text = "&New Code";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.Location = new System.Drawing.Point(859,657);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.Location = new System.Drawing.Point(763,656);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butImport
			// 
			this.butImport.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butImport.Autosize = true;
			this.butImport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butImport.Location = new System.Drawing.Point(98,624);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(83,26);
			this.butImport.TabIndex = 23;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(98,592);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(83,26);
			this.butExport.TabIndex = 24;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// FormProcCodes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(956,695);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.butImport);
			this.Controls.Add(this.listFeeSched);
			this.Controls.Add(this.comboCompare2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.comboCompare1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupFeeScheds);
			this.Controls.Add(this.butNew);
			this.Controls.Add(this.labelFeeSched);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Name = "FormProcCodes";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Procedure Codes";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProcedures_Closing);
			this.Load += new System.EventHandler(this.FormProcCodes_Load);
			this.groupFeeScheds.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion


		private void FormProcCodes_Load(object sender, System.EventArgs e){
			if(!Security.IsAuthorized(Permissions.Setup,DateTime.MinValue,true)) {
				groupFeeScheds.Visible=false;
				butEditCategories.Visible=false;
				butNew.Visible=false;
				butImport.Visible=false;
				butExport.Visible=false;
			}
			if(!IsSelectionMode){
				butOK.Visible=false;
				butCancel.Text=Lan.g(this,"Close");
			}
			FillCats();
			for(int i=0;i<listCategories.Items.Count;i++) {
				listCategories.SetSelected(i,true);
			}
			FillFeeSchedules();
			FillGrid();
		}

		private void FillFeeSchedules(){
			listFeeSched.Items.Clear();
			for(int i=0;i<DefB.Short[(int)DefCat.FeeSchedNames].Length;i++) {
				this.listFeeSched.Items.Add(DefB.Short[(int)DefCat.FeeSchedNames][i].ItemName);
			}
			if(listFeeSched.Items.Count>0) {
				listFeeSched.SelectedIndex=0;
			}
			comboCompare1.Items.Clear();
			comboCompare1.Items.Add(Lan.g(this,"none"));
			comboCompare1.SelectedIndex=0;
			comboCompare2.Items.Clear();
			comboCompare2.Items.Add(Lan.g(this,"none"));
			comboCompare2.SelectedIndex=0;
			for(int i=0;i<DefB.Short[(int)DefCat.FeeSchedNames].Length;i++) {
				comboCompare1.Items.Add(DefB.Short[(int)DefCat.FeeSchedNames][i].ItemName);
				comboCompare2.Items.Add(DefB.Short[(int)DefCat.FeeSchedNames][i].ItemName);
			}
		}

		private void FillCats(){
			ArrayList selected=new ArrayList();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++){
				selected.Add(CatList[listCategories.SelectedIndices[i]].DefNum);
			}
			if(checkShowHidden.Checked){
				CatList=DefB.Long[(int)DefCat.ProcCodeCats];
			}
			else{
				CatList=DefB.Short[(int)DefCat.ProcCodeCats];
			}
			listCategories.Items.Clear();
			for(int i=0;i<CatList.Length;i++) {
				listCategories.Items.Add(CatList[i].ItemName);
				if(selected.Contains(CatList[i].DefNum)){
					listCategories.SetSelected(i,true);
				}
			}
		}

		private void FillGrid(){
			if(listFeeSched.Items.Count==0){
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				gridMain.EndUpdate();
				MsgBox.Show(this,"You must have at least one fee schedule created.");
				return;
			}
			string selected="";
			if(gridMain.GetSelectedIndex() !=-1){
				selected=ProcTable.Rows[gridMain.GetSelectedIndex()][3].ToString();
			}
			int scroll=gridMain.ScrollValue;
			int[] cats=new int[listCategories.SelectedIndices.Count];
			for(int i=0;i<listCategories.SelectedIndices.Count;i++){
				cats[i]=CatList[listCategories.SelectedIndices[i]].DefNum;
			}
			int feeSched=DefB.Short[(int)DefCat.FeeSchedNames][listFeeSched.SelectedIndex].DefNum;
			int feeSchedComp1=0;
			if(comboCompare1.SelectedIndex!=0) {
				feeSchedComp1=DefB.Short[(int)DefCat.FeeSchedNames][comboCompare1.SelectedIndex-1].DefNum;
			}
			int feeSchedComp2=0;
			if(comboCompare2.SelectedIndex!=0) {
				feeSchedComp2=DefB.Short[(int)DefCat.FeeSchedNames][comboCompare2.SelectedIndex-1].DefNum;
			}
			ProcTable=ProcedureCodes.GetProcTable(textAbbreviation.Text,textDescription.Text,textCode.Text,cats,feeSched,
				feeSchedComp1,feeSchedComp2);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProcedures","Category"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcedures","Description"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcedures","Abbr"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcedures","Code"),50);
			gridMain.Columns.Add(col);
			string heading=DefB.Short[(int)DefCat.FeeSchedNames][listFeeSched.SelectedIndex].ItemName;
			if(heading.Length>8){
				heading=heading.Substring(0,8);
			}
			col=new ODGridColumn(heading,50,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			heading="";
			if(comboCompare1.SelectedIndex!=0){
				heading=DefB.Short[(int)DefCat.FeeSchedNames][comboCompare1.SelectedIndex-1].ItemName;
			}
			if(heading.Length>8) {
				heading=heading.Substring(0,8);
			}
			col=new ODGridColumn(heading,50,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			heading="";
			if(comboCompare2.SelectedIndex!=0) {
				heading=DefB.Short[(int)DefCat.FeeSchedNames][comboCompare2.SelectedIndex-1].ItemName;
			}
			if(heading.Length>8) {
				heading=heading.Substring(0,8);
			}
			col=new ODGridColumn(heading,50,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);	
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ProcTable.Rows.Count;i++){
				row=new ODGridRow();
				if(i==0 || ProcTable.Rows[i-1]["ProcCat"].ToString() != ProcTable.Rows[i]["ProcCat"].ToString()){
					row.Cells.Add(DefB.GetName(DefCat.ProcCodeCats,PIn.PInt(ProcTable.Rows[i]["ProcCat"].ToString())));
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(ProcTable.Rows[i]["Descript"].ToString());
				row.Cells.Add(ProcTable.Rows[i]["AbbrDesc"].ToString());
				row.Cells.Add(ProcTable.Rows[i]["ADACode"].ToString());
				if(ProcTable.Rows[i]["FeeAmt1"].ToString()=="-1") {
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(PIn.PDouble(ProcTable.Rows[i]["FeeAmt1"].ToString()).ToString("n"));
				}
				
				if(ProcTable.Rows[i]["FeeAmt2"].ToString()=="-1") {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(PIn.PDouble(ProcTable.Rows[i]["FeeAmt2"].ToString()).ToString("n"));
				}
				if(ProcTable.Rows[i]["FeeAmt3"].ToString()=="-1") {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(PIn.PDouble(ProcTable.Rows[i]["FeeAmt3"].ToString()).ToString("n"));
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollValue=scroll;
			if(selected!=""){//if a row was previously selected
				for(int i=0;i<ProcTable.Rows.Count;i++){
					if(ProcTable.Rows[i][3].ToString()==selected){
						gridMain.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void butAll_Click(object sender,EventArgs e) {
			for(int i=0;i<listCategories.Items.Count;i++){
				listCategories.SetSelected(i,true);
			}
			FillGrid();
		}

		private void butEditCategories_Click(object sender,EventArgs e) {
			//won't even be visible if no permission
			ArrayList selected=new ArrayList();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++) {
				selected.Add(CatList[listCategories.SelectedIndices[i]].DefNum);
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.ProcCodeCats);
			FormD.ShowDialog();
			DataValid.SetInvalid(InvalidTypes.Defs);
			changed=true;
			FillCats();
			for(int i=0;i<CatList.Length;i++) {
				if(selected.Contains(CatList[i].DefNum)) {
					listCategories.SetSelected(i,true);
				}
			}
			//we need to move security log to within the definition window for more complete tracking
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions");
			FillGrid();
		}

		private void textAbbreviation_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textCode_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void listCategories_MouseUp(object sender,MouseEventArgs e) {
			FillGrid();
		}

		private void checkShowHidden_Click(object sender,EventArgs e) {
			FillCats();
			FillGrid();
		}

		private void listFeeSched_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			FillGrid();
		}

		private void comboCompare1_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboCompare2_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butEditFeeSched_Click(object sender, System.EventArgs e) {
			//won't even be visible if no permission
			int selectedSched=0;
			if(listFeeSched.SelectedIndex !=-1){
				selectedSched=DefB.Short[(int)DefCat.FeeSchedNames][listFeeSched.SelectedIndex].DefNum;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.FeeSchedNames);
			FormD.ShowDialog();
			DataValid.SetInvalid(InvalidTypes.Defs | InvalidTypes.Fees);
			Fees.Refresh();
			ProcedureCodes.Refresh();
			changed=true;
			FillFeeSchedules();
			for(int i=0;i<DefB.Short[(int)DefCat.FeeSchedNames].Length;i++){
				if(DefB.Short[(int)DefCat.FeeSchedNames][i].DefNum==selectedSched){
					listFeeSched.SelectedIndex=i;
				}
			}
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions");
			//FillGrid();//will be done automatically because of lines above			
		}

		private void butTools_Click(object sender, System.EventArgs e) {
			FormFeeSchedTools FormF=new FormFeeSchedTools(DefB.Short[(int)DefCat.FeeSchedNames][listFeeSched.SelectedIndex].DefNum);
			FormF.ShowDialog();
			if(FormF.DialogResult==DialogResult.Cancel){
				return;
			}
			Fees.Refresh();
			ProcedureCodes.Refresh();
			changed=true;
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Fee Schedule Tools");
		}		

		private void butExport_Click(object sender,EventArgs e) {
			if(ProcTable.Rows.Count==0){
				MsgBox.Show(this,"No procedurecodes are displayed for export.");
				return;
			}
			if(!MsgBox.Show(this,true,"Only the codes showing in this list will be exported.  Continue?")){
				return;
			}
			List<ProcedureCode> listCodes=new List<ProcedureCode>();
			for(int i=0;i<ProcTable.Rows.Count;i++){
				if(ProcTable.Rows[i]["ADACode"].ToString()==""){
					continue;
				}
				listCodes.Add(ProcedureCodes.GetProcCode(ProcTable.Rows[i]["ADACode"].ToString()));
			}
			//ClaimForm ClaimFormCur=ClaimForms.ListLong[listClaimForms.SelectedIndex];
			SaveFileDialog saveDlg=new SaveFileDialog();
			string filename="ProcCodes.xml";
			saveDlg.InitialDirectory=PrefB.GetString("ExportPath");
			saveDlg.FileName=filename;
			if(saveDlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			//MessageBox.Show(saveDlg.FileName);
			XmlSerializer serializer=new XmlSerializer(typeof(List<ProcedureCode>));
			TextWriter writer=new StreamWriter(saveDlg.FileName);
			serializer.Serialize(writer,listCodes);
			writer.Close();
			MessageBox.Show("Exported");
		}

		private void butImport_Click(object sender,EventArgs e) {
			OpenFileDialog openDlg=new OpenFileDialog();
			openDlg.InitialDirectory=PrefB.GetString("ExportPath");
			if(openDlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			int rowsInserted=0;
			try {
				rowsInserted=ImportProcCodes(openDlg.FileName,true);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				FillGrid();
				return;
			}
			MessageBox.Show("Procedure codes inserted: "+rowsInserted);
			DataValid.SetInvalid(InvalidTypes.Defs);
			changed=true;
			FillCats();
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Imported Procedure Codes");
		}

		///<Summary>Can be called externally as part of the update sequence.  Surround with try catch.  Returns number of codes inserted.</Summary>
		public static int ImportProcCodes(string path, bool askBeforeReplace) {
			if(!File.Exists(path)) {
				throw new ApplicationException(Lan.g("FormProcCodes","File does not exist."));
			}
			XmlSerializer serializer=new XmlSerializer(typeof(List<ProcedureCode>));
			List<ProcedureCode> listCodes=new List<ProcedureCode>();
			//ClaimForm tempClaimForm=new ClaimForm();
			try {
				using(TextReader reader=new StreamReader(path)){
					listCodes=(List<ProcedureCode>)serializer.Deserialize(reader);
				}
			}
			catch {
				throw new ApplicationException(Lan.g("FormProcCodes","Invalid file format"));
			}
			int retVal=0;
			for(int i=0;i<listCodes.Count;i++){
				if(ProcedureCodes.HList.ContainsKey(listCodes[i].ADACode)){
					continue;//don't import duplicates.
				}
				listCodes[i].ProcCat=DefB.GetByExactName(DefCat.ProcCodeCats,listCodes[i].ProcCatDescript);
				if(listCodes[i].ProcCat==0){//no category exists with that name
					Def def=new Def();
					def.Category=DefCat.ProcCodeCats;
					def.ItemName=listCodes[i].ProcCatDescript;
					def.ItemOrder=DefB.Long[(int)DefCat.ProcCodeCats].Length;
					Defs.Insert(def);
					listCodes[i].ProcCat=def.DefNum;
				}
				ProcedureCodes.Insert(listCodes[i]);
				retVal++;
			}
			return retVal;
		}


		private void butNew_Click(object sender, System.EventArgs e) {
			//won't be visible if no permission
			FormProcCodeNew FormPCN=new FormProcCodeNew();
			FormPCN.ShowDialog();
			if(FormPCN.DialogResult!=DialogResult.OK){
				return;
			}
			if(FormPCN.textNewCode.Text=="") {
				return;
			}
			ProcedureCode procCode;
			if(ProcedureCodes.HList.ContainsKey(FormPCN.textNewCode.Text)){
				procCode=(ProcedureCode)ProcedureCodes.HList[FormPCN.textNewCode.Text];
				textAbbreviation.Text="";
				textDescription.Text="";
				textCode.Text=FormPCN.textNewCode.Text;
				if(DefB.GetHidden(DefCat.ProcCodeCats,procCode.ProcCat)){
					checkShowHidden.Checked=true;
					FillCats();
				}
				for(int i=0;i<CatList.Length;i++) {
					if(CatList[i].DefNum==procCode.ProcCat){
						listCategories.SetSelected(i,true);
					}
					else{
						listCategories.SetSelected(i,false);
					}
				}
				FillGrid();
				MessageBox.Show(Lan.g(this,"That code already exists."));
				return;
			}
			procCode=new ProcedureCode();
			procCode.ADACode=FormPCN.textNewCode.Text;
			//procCode.ProcTime="/X/";//moved to contructor.
			procCode.ProcCat=DefB.Short[(int)DefCat.ProcCodeCats][0].DefNum;
			//procCode.GraphicColor=Color.FromArgb(0);//moved to contructor.
			ProcedureCodes.Insert(procCode);
			FormProcCodeEdit FormP=new FormProcCodeEdit(procCode);
			FormP.IsNew=true;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK){
				changed=true;
				FillGrid();
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Added Procedure Code: "+procCode.ADACode);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsSelectionMode){
				SelectedADA=ProcTable.Rows[e.Row]["ADACode"].ToString();
				DialogResult=DialogResult.OK;
				return;
			}
			//else not selecting a code
			if(!Security.IsAuthorized(Permissions.Setup,DateTime.MinValue,true)){
				return;
			}
			string ada=ProcTable.Rows[e.Row]["ADACode"].ToString();
			if(e.Col>3){//if double clicked on a fee
				Fee FeeCur=null;
				int feesched=0;
				if(e.Col==4){
					FeeCur=Fees.GetFeeByOrder(ada,listFeeSched.SelectedIndex);
					feesched=DefB.Short[(int)DefCat.FeeSchedNames][listFeeSched.SelectedIndex].DefNum;
				}
				if(e.Col==5) {
					if(comboCompare1.SelectedIndex==0){
						return;
					}
					FeeCur=Fees.GetFeeByOrder(ada,comboCompare1.SelectedIndex-1);
					feesched=DefB.Short[(int)DefCat.FeeSchedNames][comboCompare1.SelectedIndex-1].DefNum;
				}
				if(e.Col==6) {
					if(comboCompare2.SelectedIndex==0) {
						return;
					}
					FeeCur=Fees.GetFeeByOrder(ada,comboCompare2.SelectedIndex-1);
					feesched=DefB.Short[(int)DefCat.FeeSchedNames][comboCompare2.SelectedIndex-1].DefNum;
				}
				FormFeeEdit FormFE=new FormFeeEdit();
				if(FeeCur==null) {
					FeeCur=new Fee();
					FeeCur.ADACode=ada;
					FeeCur.FeeSched=feesched;
					Fees.Insert(FeeCur);
					FormFE.IsNew=true;
				}
				FormFE.FeeCur=FeeCur;
				FormFE.ShowDialog();
				if(FormFE.DialogResult==DialogResult.OK) {
					Fees.Refresh();
					changed=true;
					FillGrid();
				}
			}
			else {//not on a fee: Edit code instead
				FormProcCodeEdit FormPCE=new FormProcCodeEdit(ProcedureCodes.GetProcCode(ada));
				FormPCE.IsNew=false;
				FormPCE.ShowDialog();
				if(FormPCE.DialogResult==DialogResult.OK) {
					//ProcedureCodes.Refresh();
					changed=true;
					//Fees.Refresh();//fees were already refreshed within procCodeEdit
					FillGrid();
				}
			}
			
		}

		private void butOK_Click(object sender, System.EventArgs e){
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a procedure code first.");
				return;
			}
			SelectedADA=ProcTable.Rows[gridMain.GetSelectedIndex()][3].ToString();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e){
			DialogResult=DialogResult.Cancel;
		}

		private void FormProcedures_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidTypes.ProcCodes | InvalidTypes.Fees);
			}
		}

		

		


		

		

		

		

		

		

		

		


	}

	
}
