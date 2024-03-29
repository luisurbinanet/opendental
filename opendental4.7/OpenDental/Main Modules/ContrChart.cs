/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDental.UI;
using Tao.Platform.Windows;
using SparksToothChart;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class ContrChart : System.Windows.Forms.UserControl	{
		private OpenDental.UI.Button butAddProc;
		private OpenDental.UI.Button butM;
		private OpenDental.UI.Button butOI;
		private OpenDental.UI.Button butD;
		private OpenDental.UI.Button butBF;
		private OpenDental.UI.Button butL;
		private OpenDental.UI.Button butV;
		private System.Windows.Forms.TextBox textSurf;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioEntryTP;
		private System.Windows.Forms.RadioButton radioEntryEO;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.RadioButton radioEntryEC;
		private ProcStat newStatus;
		private OpenDental.UI.Button button1;
		private System.Windows.Forms.RadioButton radioEntryC;
		private bool dataValid=false;
		private System.Windows.Forms.ListBox listDx;
		private int[] hiLightedRows=new int[1];
		private ContrApptSingle ApptPlanned;
		private System.Windows.Forms.CheckBox checkDone;
		private System.Windows.Forms.Label labelMinutes;
		private System.Windows.Forms.RadioButton radioEntryR;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.CheckBox checkNotes;
		private System.Windows.Forms.CheckBox checkShowR;
		private OpenDental.UI.Button butShowNone;
		private OpenDental.UI.Button butShowAll;
		private System.Windows.Forms.CheckBox checkShowE;
		private System.Windows.Forms.CheckBox checkShowC;
		private System.Windows.Forms.CheckBox checkShowTP;
		private System.Windows.Forms.CheckBox checkRx;
		private System.Windows.Forms.GroupBox groupShow;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.TextBox textADACode;
		private System.Windows.Forms.Label label14;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butNew;
		private OpenDental.UI.Button butClear;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.Label labelDx;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.ContextMenu menuProgRight;
		private System.Windows.Forms.MenuItem menuItemPrintProg;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabControl tabControlImages;
		private System.Windows.Forms.Panel panelImages;
		private bool TreatmentNoteChanged;
		///<summary>Keeps track of which tab is selected. It's the index of the selected tab.</summary>
		private int selectedImageTab=0;
		private bool MouseIsDownOnImageSplitter;
		private int ImageSplitterOriginalY;
		private int OriginalImageMousePos;
		private System.Windows.Forms.ImageList imageListThumbnails;
		private System.Windows.Forms.ListView listViewImages;
		///<summary>The indices of the image categories which are visible in Chart.</summary>
		private ArrayList visImageCats;
		///<summary>The indices within Documents.List[i] of docs which are visible in Chart.</summary>
		private ArrayList visImages;
		///<summary>Full path to the patient folder, including \ on the end</summary>
		private string patFolder;
		private OpenDental.ODtextBox textTreatmentNotes;
		private System.Windows.Forms.ContextMenu menuPatient;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkToday;
		private FormImageViewer formImageViewer;
		private Family FamCur;
		private Patient PatCur;
		private InsPlan[] PlanList;
		private System.Windows.Forms.GroupBox groupPlanned;
		///<summary></summary>
		[Category("Data"),Description("Occurs when user changes current patient, usually by clicking on the Select Patient button.")]
		public event PatientSelectedEventHandler PatientSelected=null;
		///<summary>For one patient. Allows highlighting rows.</summary>
		private Appointment[] ApptList;
		private System.Drawing.Printing.PrintDocument pd2;
		private int pagesPrinted;
		private System.Windows.Forms.CheckBox checkShowTeeth;//used in printing progress notes
		private bool headingPrinted;
		private int headingPrintH;
		private Document[] DocumentList;
		private PatPlan[] PatPlanList;
		private MenuItem menuItemSetComplete;
		private MenuItem menuItemEditSelected;
		private OpenDental.UI.Button butPin;
		private ListBox listButtonCats;
		private ListView listViewButtons;
		private Benefit[] BenefitList;
		private ImageList imageListProcButtons;
		private ColumnHeader columnHeader1;
		private TabControl tabProc;
		private TabPage tabEnterTx;
		private TabPage tabMissing;
		private ProcButton[] ProcButtonList;
		private TabPage tabPrimary;
		private TabPage tabMovements;
		private GroupBox groupBox1;
		private OpenDental.UI.Button butUnhide;
		private Label label5;
		private ListBox listHidden;
		private OpenDental.UI.Button butEdentulous;
		private Label label7;
		private OpenDental.UI.Button butNotMissing;
		private OpenDental.UI.Button butMissing;
		private OpenDental.UI.Button butHidden;
		private GroupBox groupBox3;
		private Label label8;
		private GroupBox groupBox4;
		private ValidDouble textTipB;
		private Label label11;
		private ValidDouble textTipM;
		private Label label12;
		private ValidDouble textRotate;
		private Label label15;
		private ValidDouble textShiftB;
		private Label label10;
		private ValidDouble textShiftO;
		private Label label9;
		private ValidDouble textShiftM;
		private OpenDental.UI.Button butRotatePlus;
		private OpenDental.UI.Button butMixed;
		private OpenDental.UI.Button butAllPrimary;
		private OpenDental.UI.Button butAllPerm;
		private GroupBox groupBox5;
		private OpenDental.UI.Button butPerm;
		private OpenDental.UI.Button butPrimary;
		private int SelectedProcTab;
		private OpenDental.UI.Button butBig;
		private OpenDental.UI.Button butTipBplus;
		private OpenDental.UI.Button butTipMplus;
		private OpenDental.UI.Button butShiftBplus;
		private OpenDental.UI.Button butShiftOplus;
		private OpenDental.UI.Button butShiftMplus;
		private Label label16;
		private OpenDental.UI.Button butApplyMovements;
		private OpenDental.UI.Button butTipBminus;
		private OpenDental.UI.Button butTipMminus;
		private OpenDental.UI.Button butRotateMinus;
		private OpenDental.UI.Button butShiftBminus;
		private OpenDental.UI.Button butShiftOminus;
		private OpenDental.UI.Button butShiftMminus;
		private ODGrid gridProg;
		private ODGrid gridPtInfo;
		private CheckBox checkComm;
		private ToothInitial[] ToothInitialList;
		private MenuItem menuItemPrintDay;
		///<summary>a list of the hidden teeth as strings. Includes "1"-"32", and "A"-"Z"</summary>
		private ArrayList HiddenTeeth;
		private CheckBox checkAudit;
		///<summary>This date will usually have minVal except while the hospital print function is running.</summary>
		private DateTime hospitalDate;
		private List<DocAttach> DocAttachList;
		private GraphicalToothChart toothChart;
		private PatientNote PatientNoteCur;
		private DataSet DataSetMain;
		private MenuItem menuItemLabFee;
		private MenuItem menuItemLabFeeDetach;
		private MenuItem menuItemDelete;
		///<summary>A subset of DataSetMain.  The procedures that need to be drawn in the graphical tooth chart.</summary>
		List<DataRow> ProcList;
			
		///<summary></summary>
		public ContrChart(){
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
			//tbProg.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(tbProg_CellDoubleClicked);
			//listProcButtons.Click += new System.EventHandler(this.listProcButtons_Click);
			//VQLink=new VisiQuick(Handle);		// TJE
			tabControlImages.DrawItem += new DrawItemEventHandler(OnDrawItem);
			//EventHandler onClick=new EventHandler(menuItem_Click);
			toothChart.TaoRenderEnabled=true;
			toothChart.TaoInitializeContexts();
			if(CultureInfo.CurrentCulture.Name.Substring(3)!="CA"){//Canada
				menuItemLabFee.Visible=false;
				menuItemLabFeeDetach.Visible=false;
			}
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			//if (VQLink != null)					// TJE
			//	VQLink.DoneVQ();				// TJE
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrChart));
			this.textSurf = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioEntryR = new System.Windows.Forms.RadioButton();
			this.radioEntryC = new System.Windows.Forms.RadioButton();
			this.radioEntryEO = new System.Windows.Forms.RadioButton();
			this.radioEntryEC = new System.Windows.Forms.RadioButton();
			this.radioEntryTP = new System.Windows.Forms.RadioButton();
			this.listDx = new System.Windows.Forms.ListBox();
			this.labelDx = new System.Windows.Forms.Label();
			this.groupPlanned = new System.Windows.Forms.GroupBox();
			this.butPin = new OpenDental.UI.Button();
			this.butClear = new OpenDental.UI.Button();
			this.butNew = new OpenDental.UI.Button();
			this.labelMinutes = new System.Windows.Forms.Label();
			this.checkDone = new System.Windows.Forms.CheckBox();
			this.listViewButtons = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.imageListProcButtons = new System.Windows.Forms.ImageList(this.components);
			this.listButtonCats = new System.Windows.Forms.ListBox();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.checkToday = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textADACode = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.groupShow = new System.Windows.Forms.GroupBox();
			this.checkAudit = new System.Windows.Forms.CheckBox();
			this.checkComm = new System.Windows.Forms.CheckBox();
			this.checkShowTeeth = new System.Windows.Forms.CheckBox();
			this.checkNotes = new System.Windows.Forms.CheckBox();
			this.checkRx = new System.Windows.Forms.CheckBox();
			this.checkShowR = new System.Windows.Forms.CheckBox();
			this.butShowNone = new OpenDental.UI.Button();
			this.butShowAll = new OpenDental.UI.Button();
			this.checkShowE = new System.Windows.Forms.CheckBox();
			this.checkShowC = new System.Windows.Forms.CheckBox();
			this.checkShowTP = new System.Windows.Forms.CheckBox();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.menuProgRight = new System.Windows.Forms.ContextMenu();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.menuItemSetComplete = new System.Windows.Forms.MenuItem();
			this.menuItemEditSelected = new System.Windows.Forms.MenuItem();
			this.menuItemPrintProg = new System.Windows.Forms.MenuItem();
			this.menuItemPrintDay = new System.Windows.Forms.MenuItem();
			this.menuItemLabFeeDetach = new System.Windows.Forms.MenuItem();
			this.menuItemLabFee = new System.Windows.Forms.MenuItem();
			this.tabControlImages = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.panelImages = new System.Windows.Forms.Panel();
			this.listViewImages = new System.Windows.Forms.ListView();
			this.imageListThumbnails = new System.Windows.Forms.ImageList(this.components);
			this.menuPatient = new System.Windows.Forms.ContextMenu();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.tabProc = new System.Windows.Forms.TabControl();
			this.tabEnterTx = new System.Windows.Forms.TabPage();
			this.butD = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butBF = new OpenDental.UI.Button();
			this.butL = new OpenDental.UI.Button();
			this.butM = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butAddProc = new OpenDental.UI.Button();
			this.butV = new OpenDental.UI.Button();
			this.butOI = new OpenDental.UI.Button();
			this.tabMissing = new System.Windows.Forms.TabPage();
			this.butUnhide = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.listHidden = new System.Windows.Forms.ListBox();
			this.butEdentulous = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.butNotMissing = new OpenDental.UI.Button();
			this.butMissing = new OpenDental.UI.Button();
			this.butHidden = new OpenDental.UI.Button();
			this.tabMovements = new System.Windows.Forms.TabPage();
			this.label16 = new System.Windows.Forms.Label();
			this.butApplyMovements = new OpenDental.UI.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.butTipBplus = new OpenDental.UI.Button();
			this.butTipBminus = new OpenDental.UI.Button();
			this.butTipMplus = new OpenDental.UI.Button();
			this.butTipMminus = new OpenDental.UI.Button();
			this.butRotatePlus = new OpenDental.UI.Button();
			this.butRotateMinus = new OpenDental.UI.Button();
			this.textTipB = new OpenDental.ValidDouble();
			this.label11 = new System.Windows.Forms.Label();
			this.textTipM = new OpenDental.ValidDouble();
			this.label12 = new System.Windows.Forms.Label();
			this.textRotate = new OpenDental.ValidDouble();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.butShiftBplus = new OpenDental.UI.Button();
			this.butShiftBminus = new OpenDental.UI.Button();
			this.butShiftOplus = new OpenDental.UI.Button();
			this.butShiftOminus = new OpenDental.UI.Button();
			this.butShiftMplus = new OpenDental.UI.Button();
			this.butShiftMminus = new OpenDental.UI.Button();
			this.textShiftB = new OpenDental.ValidDouble();
			this.label10 = new System.Windows.Forms.Label();
			this.textShiftO = new OpenDental.ValidDouble();
			this.label9 = new System.Windows.Forms.Label();
			this.textShiftM = new OpenDental.ValidDouble();
			this.label8 = new System.Windows.Forms.Label();
			this.tabPrimary = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.butPerm = new OpenDental.UI.Button();
			this.butPrimary = new OpenDental.UI.Button();
			this.butMixed = new OpenDental.UI.Button();
			this.butAllPrimary = new OpenDental.UI.Button();
			this.butAllPerm = new OpenDental.UI.Button();
			this.toothChart = new SparksToothChart.GraphicalToothChart();
			this.gridProg = new OpenDental.UI.ODGrid();
			this.butBig = new OpenDental.UI.Button();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.button1 = new OpenDental.UI.Button();
			this.textTreatmentNotes = new OpenDental.ODtextBox();
			this.gridPtInfo = new OpenDental.UI.ODGrid();
			this.groupBox2.SuspendLayout();
			this.groupPlanned.SuspendLayout();
			this.groupShow.SuspendLayout();
			this.tabControlImages.SuspendLayout();
			this.panelImages.SuspendLayout();
			this.tabProc.SuspendLayout();
			this.tabEnterTx.SuspendLayout();
			this.tabMissing.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabMovements.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabPrimary.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// textSurf
			// 
			this.textSurf.BackColor = System.Drawing.SystemColors.Window;
			this.textSurf.Location = new System.Drawing.Point(8,2);
			this.textSurf.Name = "textSurf";
			this.textSurf.ReadOnly = true;
			this.textSurf.Size = new System.Drawing.Size(72,20);
			this.textSurf.TabIndex = 25;
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.Window;
			this.groupBox2.Controls.Add(this.radioEntryR);
			this.groupBox2.Controls.Add(this.radioEntryC);
			this.groupBox2.Controls.Add(this.radioEntryEO);
			this.groupBox2.Controls.Add(this.radioEntryEC);
			this.groupBox2.Controls.Add(this.radioEntryTP);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(1,87);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(88,96);
			this.groupBox2.TabIndex = 35;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Entry Status";
			// 
			// radioEntryR
			// 
			this.radioEntryR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryR.Location = new System.Drawing.Point(8,76);
			this.radioEntryR.Name = "radioEntryR";
			this.radioEntryR.Size = new System.Drawing.Size(75,16);
			this.radioEntryR.TabIndex = 4;
			this.radioEntryR.Text = "Referred";
			this.radioEntryR.CheckedChanged += new System.EventHandler(this.radioEntryR_CheckedChanged);
			// 
			// radioEntryC
			// 
			this.radioEntryC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryC.Location = new System.Drawing.Point(8,31);
			this.radioEntryC.Name = "radioEntryC";
			this.radioEntryC.Size = new System.Drawing.Size(74,16);
			this.radioEntryC.TabIndex = 3;
			this.radioEntryC.Text = "C";
			this.radioEntryC.CheckedChanged += new System.EventHandler(this.radioEntryC_CheckedChanged);
			// 
			// radioEntryEO
			// 
			this.radioEntryEO.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryEO.Location = new System.Drawing.Point(8,61);
			this.radioEntryEO.Name = "radioEntryEO";
			this.radioEntryEO.Size = new System.Drawing.Size(72,16);
			this.radioEntryEO.TabIndex = 2;
			this.radioEntryEO.Text = "Ex Other";
			this.radioEntryEO.CheckedChanged += new System.EventHandler(this.radioEntryEO_CheckedChanged);
			// 
			// radioEntryEC
			// 
			this.radioEntryEC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryEC.Location = new System.Drawing.Point(8,46);
			this.radioEntryEC.Name = "radioEntryEC";
			this.radioEntryEC.Size = new System.Drawing.Size(77,16);
			this.radioEntryEC.TabIndex = 1;
			this.radioEntryEC.Text = "Ex Cur";
			this.radioEntryEC.CheckedChanged += new System.EventHandler(this.radioEntryEC_CheckedChanged);
			// 
			// radioEntryTP
			// 
			this.radioEntryTP.Checked = true;
			this.radioEntryTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioEntryTP.Location = new System.Drawing.Point(8,16);
			this.radioEntryTP.Name = "radioEntryTP";
			this.radioEntryTP.Size = new System.Drawing.Size(77,16);
			this.radioEntryTP.TabIndex = 0;
			this.radioEntryTP.TabStop = true;
			this.radioEntryTP.Text = "TP";
			this.radioEntryTP.CheckedChanged += new System.EventHandler(this.radioEntryTP_CheckedChanged);
			// 
			// listDx
			// 
			this.listDx.Location = new System.Drawing.Point(91,16);
			this.listDx.Name = "listDx";
			this.listDx.Size = new System.Drawing.Size(94,173);
			this.listDx.TabIndex = 46;
			this.listDx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listDx_MouseDown);
			// 
			// labelDx
			// 
			this.labelDx.Location = new System.Drawing.Point(89,-2);
			this.labelDx.Name = "labelDx";
			this.labelDx.Size = new System.Drawing.Size(100,18);
			this.labelDx.TabIndex = 47;
			this.labelDx.Text = "Diagnosis";
			this.labelDx.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupPlanned
			// 
			this.groupPlanned.Controls.Add(this.butPin);
			this.groupPlanned.Controls.Add(this.butClear);
			this.groupPlanned.Controls.Add(this.butNew);
			this.groupPlanned.Controls.Add(this.labelMinutes);
			this.groupPlanned.Controls.Add(this.checkDone);
			this.groupPlanned.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupPlanned.Location = new System.Drawing.Point(0,30);
			this.groupPlanned.Name = "groupPlanned";
			this.groupPlanned.Size = new System.Drawing.Size(198,114);
			this.groupPlanned.TabIndex = 43;
			this.groupPlanned.TabStop = false;
			this.groupPlanned.Text = "Planned Appointment";
			// 
			// butPin
			// 
			this.butPin.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butPin.Autosize = true;
			this.butPin.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPin.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPin.CornerRadius = 4F;
			this.butPin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPin.Location = new System.Drawing.Point(117,84);
			this.butPin.Name = "butPin";
			this.butPin.Size = new System.Drawing.Size(75,23);
			this.butPin.TabIndex = 6;
			this.butPin.Text = "Pin Board";
			this.butPin.Click += new System.EventHandler(this.butPin_Click);
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClear.Location = new System.Drawing.Point(117,57);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(75,23);
			this.butClear.TabIndex = 5;
			this.butClear.Text = "Clear";
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.CornerRadius = 4F;
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(117,30);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(75,23);
			this.butNew.TabIndex = 4;
			this.butNew.Text = "New";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// labelMinutes
			// 
			this.labelMinutes.Location = new System.Drawing.Point(118,90);
			this.labelMinutes.Name = "labelMinutes";
			this.labelMinutes.Size = new System.Drawing.Size(66,14);
			this.labelMinutes.TabIndex = 3;
			// 
			// checkDone
			// 
			this.checkDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDone.Location = new System.Drawing.Point(124,12);
			this.checkDone.Name = "checkDone";
			this.checkDone.Size = new System.Drawing.Size(56,16);
			this.checkDone.TabIndex = 0;
			this.checkDone.Text = "Done";
			this.checkDone.Click += new System.EventHandler(this.checkDone_Click);
			// 
			// listViewButtons
			// 
			this.listViewButtons.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listViewButtons.AutoArrange = false;
			this.listViewButtons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.listViewButtons.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listViewButtons.Location = new System.Drawing.Point(311,40);
			this.listViewButtons.MultiSelect = false;
			this.listViewButtons.Name = "listViewButtons";
			this.listViewButtons.Size = new System.Drawing.Size(178,186);
			this.listViewButtons.SmallImageList = this.imageListProcButtons;
			this.listViewButtons.TabIndex = 188;
			this.listViewButtons.UseCompatibleStateImageBehavior = false;
			this.listViewButtons.View = System.Windows.Forms.View.Details;
			this.listViewButtons.Click += new System.EventHandler(this.listViewButtons_Click);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 155;
			// 
			// imageListProcButtons
			// 
			this.imageListProcButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListProcButtons.ImageStream")));
			this.imageListProcButtons.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListProcButtons.Images.SetKeyName(0,"deposit.gif");
			// 
			// listButtonCats
			// 
			this.listButtonCats.Location = new System.Drawing.Point(187,40);
			this.listButtonCats.MultiColumn = true;
			this.listButtonCats.Name = "listButtonCats";
			this.listButtonCats.Size = new System.Drawing.Size(122,186);
			this.listButtonCats.TabIndex = 59;
			this.listButtonCats.Click += new System.EventHandler(this.listButtonCats_Click);
			// 
			// comboPriority
			// 
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.Location = new System.Drawing.Point(91,205);
			this.comboPriority.MaxDropDownItems = 40;
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(96,21);
			this.comboPriority.TabIndex = 54;
			// 
			// checkToday
			// 
			this.checkToday.Checked = true;
			this.checkToday.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkToday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkToday.Location = new System.Drawing.Point(1,188);
			this.checkToday.Name = "checkToday";
			this.checkToday.Size = new System.Drawing.Size(80,18);
			this.checkToday.TabIndex = 58;
			this.checkToday.Text = "Today";
			this.checkToday.CheckedChanged += new System.EventHandler(this.checkToday_CheckedChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(89,188);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(79,17);
			this.label6.TabIndex = 57;
			this.label6.Text = "Priority";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textADACode
			// 
			this.textADACode.Location = new System.Drawing.Point(330,3);
			this.textADACode.Name = "textADACode";
			this.textADACode.Size = new System.Drawing.Size(108,20);
			this.textADACode.TabIndex = 50;
			this.textADACode.Text = "Type ADA Code";
			this.textADACode.Enter += new System.EventHandler(this.textADACode_Enter);
			this.textADACode.TextChanged += new System.EventHandler(this.textADACode_TextChanged);
			this.textADACode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textADACode_KeyDown);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(283,6);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(47,17);
			this.label14.TabIndex = 51;
			this.label14.Text = "Or";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(308,21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(128,18);
			this.label13.TabIndex = 49;
			this.label13.Text = "Or Single Click:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupShow
			// 
			this.groupShow.Controls.Add(this.checkAudit);
			this.groupShow.Controls.Add(this.checkComm);
			this.groupShow.Controls.Add(this.checkShowTeeth);
			this.groupShow.Controls.Add(this.checkNotes);
			this.groupShow.Controls.Add(this.checkRx);
			this.groupShow.Controls.Add(this.checkShowR);
			this.groupShow.Controls.Add(this.butShowNone);
			this.groupShow.Controls.Add(this.butShowAll);
			this.groupShow.Controls.Add(this.checkShowE);
			this.groupShow.Controls.Add(this.checkShowC);
			this.groupShow.Controls.Add(this.checkShowTP);
			this.groupShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupShow.Location = new System.Drawing.Point(200,30);
			this.groupShow.Name = "groupShow";
			this.groupShow.Size = new System.Drawing.Size(211,114);
			this.groupShow.TabIndex = 38;
			this.groupShow.TabStop = false;
			this.groupShow.Text = "Show:";
			// 
			// checkAudit
			// 
			this.checkAudit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAudit.Location = new System.Drawing.Point(137,89);
			this.checkAudit.Name = "checkAudit";
			this.checkAudit.Size = new System.Drawing.Size(73,13);
			this.checkAudit.TabIndex = 17;
			this.checkAudit.Text = "Audit";
			this.checkAudit.Click += new System.EventHandler(this.checkAudit_Click);
			// 
			// checkComm
			// 
			this.checkComm.Checked = true;
			this.checkComm.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkComm.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkComm.Location = new System.Drawing.Point(104,32);
			this.checkComm.Name = "checkComm";
			this.checkComm.Size = new System.Drawing.Size(95,13);
			this.checkComm.TabIndex = 16;
			this.checkComm.Text = "Comm Log";
			this.checkComm.Click += new System.EventHandler(this.checkComm_Click);
			// 
			// checkShowTeeth
			// 
			this.checkShowTeeth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTeeth.Location = new System.Drawing.Point(104,62);
			this.checkShowTeeth.Name = "checkShowTeeth";
			this.checkShowTeeth.Size = new System.Drawing.Size(104,13);
			this.checkShowTeeth.TabIndex = 15;
			this.checkShowTeeth.Text = "Selected Teeth";
			this.checkShowTeeth.Click += new System.EventHandler(this.checkShowTeeth_Click);
			// 
			// checkNotes
			// 
			this.checkNotes.AllowDrop = true;
			this.checkNotes.Checked = true;
			this.checkNotes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNotes.Location = new System.Drawing.Point(104,47);
			this.checkNotes.Name = "checkNotes";
			this.checkNotes.Size = new System.Drawing.Size(102,13);
			this.checkNotes.TabIndex = 11;
			this.checkNotes.Text = "Proc Notes";
			this.checkNotes.Click += new System.EventHandler(this.checkNotes_Click);
			// 
			// checkRx
			// 
			this.checkRx.Checked = true;
			this.checkRx.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkRx.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRx.Location = new System.Drawing.Point(104,16);
			this.checkRx.Name = "checkRx";
			this.checkRx.Size = new System.Drawing.Size(95,13);
			this.checkRx.TabIndex = 8;
			this.checkRx.Text = "Rx";
			this.checkRx.Click += new System.EventHandler(this.checkRx_Click);
			// 
			// checkShowR
			// 
			this.checkShowR.Checked = true;
			this.checkShowR.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowR.Location = new System.Drawing.Point(4,60);
			this.checkShowR.Name = "checkShowR";
			this.checkShowR.Size = new System.Drawing.Size(98,16);
			this.checkShowR.TabIndex = 14;
			this.checkShowR.Text = "Referred";
			this.checkShowR.Click += new System.EventHandler(this.checkShowR_Click);
			// 
			// butShowNone
			// 
			this.butShowNone.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShowNone.Autosize = true;
			this.butShowNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowNone.CornerRadius = 4F;
			this.butShowNone.Location = new System.Drawing.Point(69,84);
			this.butShowNone.Name = "butShowNone";
			this.butShowNone.Size = new System.Drawing.Size(58,23);
			this.butShowNone.TabIndex = 13;
			this.butShowNone.Text = "None";
			this.butShowNone.Click += new System.EventHandler(this.butShowNone_Click);
			// 
			// butShowAll
			// 
			this.butShowAll.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShowAll.Autosize = true;
			this.butShowAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowAll.CornerRadius = 4F;
			this.butShowAll.Location = new System.Drawing.Point(5,84);
			this.butShowAll.Name = "butShowAll";
			this.butShowAll.Size = new System.Drawing.Size(53,23);
			this.butShowAll.TabIndex = 12;
			this.butShowAll.Text = "All";
			this.butShowAll.Click += new System.EventHandler(this.butShowAll_Click);
			// 
			// checkShowE
			// 
			this.checkShowE.Checked = true;
			this.checkShowE.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowE.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowE.Location = new System.Drawing.Point(4,45);
			this.checkShowE.Name = "checkShowE";
			this.checkShowE.Size = new System.Drawing.Size(100,16);
			this.checkShowE.TabIndex = 10;
			this.checkShowE.Text = "Existing";
			this.checkShowE.Click += new System.EventHandler(this.checkShowE_Click);
			// 
			// checkShowC
			// 
			this.checkShowC.Checked = true;
			this.checkShowC.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowC.Location = new System.Drawing.Point(4,30);
			this.checkShowC.Name = "checkShowC";
			this.checkShowC.Size = new System.Drawing.Size(100,16);
			this.checkShowC.TabIndex = 9;
			this.checkShowC.Text = "Completed";
			this.checkShowC.Click += new System.EventHandler(this.checkShowC_Click);
			// 
			// checkShowTP
			// 
			this.checkShowTP.Checked = true;
			this.checkShowTP.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowTP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTP.Location = new System.Drawing.Point(4,16);
			this.checkShowTP.Name = "checkShowTP";
			this.checkShowTP.Size = new System.Drawing.Size(101,13);
			this.checkShowTP.TabIndex = 8;
			this.checkShowTP.Text = "Treat Plan";
			this.checkShowTP.Click += new System.EventHandler(this.checkShowTP_Click);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0,"");
			this.imageListMain.Images.SetKeyName(1,"");
			this.imageListMain.Images.SetKeyName(2,"");
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(1,411);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(384,18);
			this.label4.TabIndex = 180;
			this.label4.Text = "Treatment Notes (for items that do not display above)";
			// 
			// menuProgRight
			// 
			this.menuProgRight.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDelete,
            this.menuItemSetComplete,
            this.menuItemEditSelected,
            this.menuItemPrintProg,
            this.menuItemPrintDay,
            this.menuItemLabFeeDetach,
            this.menuItemLabFee});
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 0;
			this.menuItemDelete.Text = "Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
			// 
			// menuItemSetComplete
			// 
			this.menuItemSetComplete.Index = 1;
			this.menuItemSetComplete.Text = "Set Complete";
			this.menuItemSetComplete.Click += new System.EventHandler(this.menuItemSetComplete_Click);
			// 
			// menuItemEditSelected
			// 
			this.menuItemEditSelected.Index = 2;
			this.menuItemEditSelected.Text = "Edit";
			this.menuItemEditSelected.Visible = false;
			this.menuItemEditSelected.Click += new System.EventHandler(this.menuItemEditSelected_Click);
			// 
			// menuItemPrintProg
			// 
			this.menuItemPrintProg.Index = 3;
			this.menuItemPrintProg.Text = "Print Progress Notes ...";
			this.menuItemPrintProg.Click += new System.EventHandler(this.menuItemPrintProg_Click);
			// 
			// menuItemPrintDay
			// 
			this.menuItemPrintDay.Index = 4;
			this.menuItemPrintDay.Text = "Print Day for Hospital";
			this.menuItemPrintDay.Click += new System.EventHandler(this.menuItemPrintDay_Click);
			// 
			// menuItemLabFeeDetach
			// 
			this.menuItemLabFeeDetach.Index = 5;
			this.menuItemLabFeeDetach.Text = "Detach Lab Fee";
			this.menuItemLabFeeDetach.Click += new System.EventHandler(this.menuItemLabFeeDetach_Click);
			// 
			// menuItemLabFee
			// 
			this.menuItemLabFee.Index = 6;
			this.menuItemLabFee.Text = "Attach Lab Fee";
			this.menuItemLabFee.Click += new System.EventHandler(this.menuItemLabFee_Click);
			// 
			// tabControlImages
			// 
			this.tabControlImages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControlImages.Controls.Add(this.tabPage1);
			this.tabControlImages.Controls.Add(this.tabPage2);
			this.tabControlImages.Controls.Add(this.tabPage4);
			this.tabControlImages.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tabControlImages.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControlImages.ItemSize = new System.Drawing.Size(42,22);
			this.tabControlImages.Location = new System.Drawing.Point(0,728);
			this.tabControlImages.Name = "tabControlImages";
			this.tabControlImages.SelectedIndex = 0;
			this.tabControlImages.Size = new System.Drawing.Size(939,27);
			this.tabControlImages.TabIndex = 185;
			this.tabControlImages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlImages_MouseDown);
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4,4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(931,0);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "BW\'s";
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4,4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(931,-3);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Pano";
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(4,4);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(931,-3);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "tabPage4";
			// 
			// panelImages
			// 
			this.panelImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelImages.Controls.Add(this.listViewImages);
			this.panelImages.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelImages.ForeColor = System.Drawing.SystemColors.ControlText;
			this.panelImages.Location = new System.Drawing.Point(0,673);
			this.panelImages.Name = "panelImages";
			this.panelImages.Padding = new System.Windows.Forms.Padding(0,4,0,0);
			this.panelImages.Size = new System.Drawing.Size(939,55);
			this.panelImages.TabIndex = 186;
			this.panelImages.Visible = false;
			this.panelImages.MouseLeave += new System.EventHandler(this.panelImages_MouseLeave);
			this.panelImages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelImages_MouseDown);
			this.panelImages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelImages_MouseMove);
			this.panelImages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelImages_MouseUp);
			// 
			// listViewImages
			// 
			this.listViewImages.Activation = System.Windows.Forms.ItemActivation.TwoClick;
			this.listViewImages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewImages.HideSelection = false;
			this.listViewImages.LabelWrap = false;
			this.listViewImages.LargeImageList = this.imageListThumbnails;
			this.listViewImages.Location = new System.Drawing.Point(0,4);
			this.listViewImages.MultiSelect = false;
			this.listViewImages.Name = "listViewImages";
			this.listViewImages.Size = new System.Drawing.Size(937,49);
			this.listViewImages.TabIndex = 0;
			this.listViewImages.UseCompatibleStateImageBehavior = false;
			this.listViewImages.DoubleClick += new System.EventHandler(this.listViewImages_DoubleClick);
			// 
			// imageListThumbnails
			// 
			this.imageListThumbnails.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListThumbnails.ImageSize = new System.Drawing.Size(100,100);
			this.imageListThumbnails.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// pd2
			// 
			this.pd2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd2_PrintPage);
			// 
			// tabProc
			// 
			this.tabProc.Controls.Add(this.tabEnterTx);
			this.tabProc.Controls.Add(this.tabMissing);
			this.tabProc.Controls.Add(this.tabMovements);
			this.tabProc.Controls.Add(this.tabPrimary);
			this.tabProc.Location = new System.Drawing.Point(415,32);
			this.tabProc.Name = "tabProc";
			this.tabProc.SelectedIndex = 0;
			this.tabProc.Size = new System.Drawing.Size(497,261);
			this.tabProc.TabIndex = 190;
			this.tabProc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabProc_MouseDown);
			// 
			// tabEnterTx
			// 
			this.tabEnterTx.Controls.Add(this.listDx);
			this.tabEnterTx.Controls.Add(this.listViewButtons);
			this.tabEnterTx.Controls.Add(this.groupBox2);
			this.tabEnterTx.Controls.Add(this.listButtonCats);
			this.tabEnterTx.Controls.Add(this.butD);
			this.tabEnterTx.Controls.Add(this.comboPriority);
			this.tabEnterTx.Controls.Add(this.textSurf);
			this.tabEnterTx.Controls.Add(this.textDate);
			this.tabEnterTx.Controls.Add(this.butBF);
			this.tabEnterTx.Controls.Add(this.checkToday);
			this.tabEnterTx.Controls.Add(this.butL);
			this.tabEnterTx.Controls.Add(this.label6);
			this.tabEnterTx.Controls.Add(this.butM);
			this.tabEnterTx.Controls.Add(this.butOK);
			this.tabEnterTx.Controls.Add(this.butAddProc);
			this.tabEnterTx.Controls.Add(this.butV);
			this.tabEnterTx.Controls.Add(this.textADACode);
			this.tabEnterTx.Controls.Add(this.butOI);
			this.tabEnterTx.Controls.Add(this.label14);
			this.tabEnterTx.Controls.Add(this.labelDx);
			this.tabEnterTx.Controls.Add(this.label13);
			this.tabEnterTx.Location = new System.Drawing.Point(4,22);
			this.tabEnterTx.Name = "tabEnterTx";
			this.tabEnterTx.Padding = new System.Windows.Forms.Padding(3);
			this.tabEnterTx.Size = new System.Drawing.Size(489,235);
			this.tabEnterTx.TabIndex = 0;
			this.tabEnterTx.Text = "Enter Treatment";
			this.tabEnterTx.UseVisualStyleBackColor = true;
			// 
			// butD
			// 
			this.butD.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butD.Autosize = true;
			this.butD.BackColor = System.Drawing.SystemColors.Control;
			this.butD.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butD.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butD.CornerRadius = 4F;
			this.butD.Location = new System.Drawing.Point(61,43);
			this.butD.Name = "butD";
			this.butD.Size = new System.Drawing.Size(24,20);
			this.butD.TabIndex = 20;
			this.butD.Text = "D";
			this.butD.UseVisualStyleBackColor = false;
			this.butD.Click += new System.EventHandler(this.butD_Click);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(0,205);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(89,20);
			this.textDate.TabIndex = 55;
			// 
			// butBF
			// 
			this.butBF.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butBF.Autosize = true;
			this.butBF.BackColor = System.Drawing.SystemColors.Control;
			this.butBF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBF.CornerRadius = 4F;
			this.butBF.Location = new System.Drawing.Point(22,23);
			this.butBF.Name = "butBF";
			this.butBF.Size = new System.Drawing.Size(28,20);
			this.butBF.TabIndex = 21;
			this.butBF.Text = "B/F";
			this.butBF.UseVisualStyleBackColor = false;
			this.butBF.Click += new System.EventHandler(this.butBF_Click);
			// 
			// butL
			// 
			this.butL.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butL.Autosize = true;
			this.butL.BackColor = System.Drawing.SystemColors.Control;
			this.butL.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butL.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butL.CornerRadius = 4F;
			this.butL.Location = new System.Drawing.Point(32,63);
			this.butL.Name = "butL";
			this.butL.Size = new System.Drawing.Size(24,20);
			this.butL.TabIndex = 22;
			this.butL.Text = "L";
			this.butL.UseVisualStyleBackColor = false;
			this.butL.Click += new System.EventHandler(this.butL_Click);
			// 
			// butM
			// 
			this.butM.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butM.Autosize = true;
			this.butM.BackColor = System.Drawing.SystemColors.Control;
			this.butM.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butM.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butM.CornerRadius = 4F;
			this.butM.Location = new System.Drawing.Point(3,43);
			this.butM.Name = "butM";
			this.butM.Size = new System.Drawing.Size(24,20);
			this.butM.TabIndex = 18;
			this.butM.Text = "M";
			this.butM.UseVisualStyleBackColor = false;
			this.butM.Click += new System.EventHandler(this.butM_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(442,1);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(44,23);
			this.butOK.TabIndex = 52;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butAddProc
			// 
			this.butAddProc.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAddProc.Autosize = true;
			this.butAddProc.BackColor = System.Drawing.SystemColors.Control;
			this.butAddProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddProc.CornerRadius = 4F;
			this.butAddProc.Location = new System.Drawing.Point(191,1);
			this.butAddProc.Name = "butAddProc";
			this.butAddProc.Size = new System.Drawing.Size(89,23);
			this.butAddProc.TabIndex = 17;
			this.butAddProc.Text = "Procedure List";
			this.butAddProc.UseVisualStyleBackColor = false;
			this.butAddProc.Click += new System.EventHandler(this.butAddProc_Click);
			// 
			// butV
			// 
			this.butV.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butV.Autosize = true;
			this.butV.BackColor = System.Drawing.SystemColors.Control;
			this.butV.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butV.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butV.CornerRadius = 4F;
			this.butV.Location = new System.Drawing.Point(50,23);
			this.butV.Name = "butV";
			this.butV.Size = new System.Drawing.Size(17,20);
			this.butV.TabIndex = 24;
			this.butV.Text = "V";
			this.butV.UseVisualStyleBackColor = false;
			this.butV.Click += new System.EventHandler(this.butV_Click);
			// 
			// butOI
			// 
			this.butOI.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOI.Autosize = true;
			this.butOI.BackColor = System.Drawing.SystemColors.Control;
			this.butOI.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOI.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOI.CornerRadius = 4F;
			this.butOI.Location = new System.Drawing.Point(27,43);
			this.butOI.Name = "butOI";
			this.butOI.Size = new System.Drawing.Size(34,20);
			this.butOI.TabIndex = 19;
			this.butOI.Text = "O/I";
			this.butOI.UseVisualStyleBackColor = false;
			this.butOI.Click += new System.EventHandler(this.butOI_Click);
			// 
			// tabMissing
			// 
			this.tabMissing.Controls.Add(this.butUnhide);
			this.tabMissing.Controls.Add(this.label5);
			this.tabMissing.Controls.Add(this.listHidden);
			this.tabMissing.Controls.Add(this.butEdentulous);
			this.tabMissing.Controls.Add(this.groupBox1);
			this.tabMissing.Location = new System.Drawing.Point(4,22);
			this.tabMissing.Name = "tabMissing";
			this.tabMissing.Padding = new System.Windows.Forms.Padding(3);
			this.tabMissing.Size = new System.Drawing.Size(489,235);
			this.tabMissing.TabIndex = 1;
			this.tabMissing.Text = "Missing Teeth";
			this.tabMissing.UseVisualStyleBackColor = true;
			// 
			// butUnhide
			// 
			this.butUnhide.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butUnhide.Autosize = true;
			this.butUnhide.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnhide.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnhide.CornerRadius = 4F;
			this.butUnhide.Location = new System.Drawing.Point(307,113);
			this.butUnhide.Name = "butUnhide";
			this.butUnhide.Size = new System.Drawing.Size(71,23);
			this.butUnhide.TabIndex = 20;
			this.butUnhide.Text = "Unhide";
			this.butUnhide.Click += new System.EventHandler(this.butUnhide_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(304,12);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(147,17);
			this.label5.TabIndex = 19;
			this.label5.Text = "Hidden Teeth";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listHidden
			// 
			this.listHidden.FormattingEnabled = true;
			this.listHidden.Location = new System.Drawing.Point(307,33);
			this.listHidden.Name = "listHidden";
			this.listHidden.Size = new System.Drawing.Size(94,69);
			this.listHidden.TabIndex = 18;
			// 
			// butEdentulous
			// 
			this.butEdentulous.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butEdentulous.Autosize = true;
			this.butEdentulous.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdentulous.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdentulous.CornerRadius = 4F;
			this.butEdentulous.Location = new System.Drawing.Point(31,113);
			this.butEdentulous.Name = "butEdentulous";
			this.butEdentulous.Size = new System.Drawing.Size(82,23);
			this.butEdentulous.TabIndex = 16;
			this.butEdentulous.Text = "Edentulous";
			this.butEdentulous.Click += new System.EventHandler(this.butEdentulous_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.butNotMissing);
			this.groupBox1.Controls.Add(this.butMissing);
			this.groupBox1.Controls.Add(this.butHidden);
			this.groupBox1.Location = new System.Drawing.Point(20,12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(267,90);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Set Selected Teeth";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(115,46);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(146,17);
			this.label7.TabIndex = 20;
			this.label7.Text = "(including numbers)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butNotMissing
			// 
			this.butNotMissing.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butNotMissing.Autosize = true;
			this.butNotMissing.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNotMissing.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNotMissing.CornerRadius = 4F;
			this.butNotMissing.Location = new System.Drawing.Point(11,53);
			this.butNotMissing.Name = "butNotMissing";
			this.butNotMissing.Size = new System.Drawing.Size(82,23);
			this.butNotMissing.TabIndex = 15;
			this.butNotMissing.Text = "Not Missing";
			this.butNotMissing.Click += new System.EventHandler(this.butNotMissing_Click);
			// 
			// butMissing
			// 
			this.butMissing.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butMissing.Autosize = true;
			this.butMissing.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMissing.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMissing.CornerRadius = 4F;
			this.butMissing.Location = new System.Drawing.Point(11,21);
			this.butMissing.Name = "butMissing";
			this.butMissing.Size = new System.Drawing.Size(82,23);
			this.butMissing.TabIndex = 14;
			this.butMissing.Text = "Missing";
			this.butMissing.Click += new System.EventHandler(this.butMissing_Click);
			// 
			// butHidden
			// 
			this.butHidden.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butHidden.Autosize = true;
			this.butHidden.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHidden.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHidden.CornerRadius = 4F;
			this.butHidden.Location = new System.Drawing.Point(172,21);
			this.butHidden.Name = "butHidden";
			this.butHidden.Size = new System.Drawing.Size(82,23);
			this.butHidden.TabIndex = 17;
			this.butHidden.Text = "Hidden";
			this.butHidden.Click += new System.EventHandler(this.butHidden_Click);
			// 
			// tabMovements
			// 
			this.tabMovements.Controls.Add(this.label16);
			this.tabMovements.Controls.Add(this.butApplyMovements);
			this.tabMovements.Controls.Add(this.groupBox4);
			this.tabMovements.Controls.Add(this.groupBox3);
			this.tabMovements.Location = new System.Drawing.Point(4,22);
			this.tabMovements.Name = "tabMovements";
			this.tabMovements.Size = new System.Drawing.Size(489,235);
			this.tabMovements.TabIndex = 3;
			this.tabMovements.Text = "Movements";
			this.tabMovements.UseVisualStyleBackColor = true;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(180,183);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(267,18);
			this.label16.TabIndex = 29;
			this.label16.Text = "(if you typed in changes)";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butApplyMovements
			// 
			this.butApplyMovements.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butApplyMovements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butApplyMovements.Autosize = true;
			this.butApplyMovements.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butApplyMovements.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butApplyMovements.CornerRadius = 4F;
			this.butApplyMovements.Location = new System.Drawing.Point(377,154);
			this.butApplyMovements.Name = "butApplyMovements";
			this.butApplyMovements.Size = new System.Drawing.Size(68,23);
			this.butApplyMovements.TabIndex = 16;
			this.butApplyMovements.Text = "Apply";
			this.butApplyMovements.Click += new System.EventHandler(this.butApplyMovements_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.butTipBplus);
			this.groupBox4.Controls.Add(this.butTipBminus);
			this.groupBox4.Controls.Add(this.butTipMplus);
			this.groupBox4.Controls.Add(this.butTipMminus);
			this.groupBox4.Controls.Add(this.butRotatePlus);
			this.groupBox4.Controls.Add(this.butRotateMinus);
			this.groupBox4.Controls.Add(this.textTipB);
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.textTipM);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Controls.Add(this.textRotate);
			this.groupBox4.Controls.Add(this.label15);
			this.groupBox4.Location = new System.Drawing.Point(255,12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(207,109);
			this.groupBox4.TabIndex = 15;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Rotate/Tip degrees";
			// 
			// butTipBplus
			// 
			this.butTipBplus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butTipBplus.Autosize = true;
			this.butTipBplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipBplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipBplus.CornerRadius = 4F;
			this.butTipBplus.Font = new System.Drawing.Font("Microsoft Sans Serif",12F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butTipBplus.Location = new System.Drawing.Point(159,76);
			this.butTipBplus.Name = "butTipBplus";
			this.butTipBplus.Size = new System.Drawing.Size(31,23);
			this.butTipBplus.TabIndex = 34;
			this.butTipBplus.Text = "+";
			this.butTipBplus.Click += new System.EventHandler(this.butTipBplus_Click);
			// 
			// butTipBminus
			// 
			this.butTipBminus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butTipBminus.Autosize = true;
			this.butTipBminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipBminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipBminus.CornerRadius = 4F;
			this.butTipBminus.Font = new System.Drawing.Font("Microsoft Sans Serif",15F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butTipBminus.Location = new System.Drawing.Point(122,76);
			this.butTipBminus.Name = "butTipBminus";
			this.butTipBminus.Size = new System.Drawing.Size(31,23);
			this.butTipBminus.TabIndex = 35;
			this.butTipBminus.Text = "-";
			this.butTipBminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butTipBminus.Click += new System.EventHandler(this.butTipBminus_Click);
			// 
			// butTipMplus
			// 
			this.butTipMplus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butTipMplus.Autosize = true;
			this.butTipMplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipMplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipMplus.CornerRadius = 4F;
			this.butTipMplus.Font = new System.Drawing.Font("Microsoft Sans Serif",12F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butTipMplus.Location = new System.Drawing.Point(159,47);
			this.butTipMplus.Name = "butTipMplus";
			this.butTipMplus.Size = new System.Drawing.Size(31,23);
			this.butTipMplus.TabIndex = 32;
			this.butTipMplus.Text = "+";
			this.butTipMplus.Click += new System.EventHandler(this.butTipMplus_Click);
			// 
			// butTipMminus
			// 
			this.butTipMminus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butTipMminus.Autosize = true;
			this.butTipMminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTipMminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTipMminus.CornerRadius = 4F;
			this.butTipMminus.Font = new System.Drawing.Font("Microsoft Sans Serif",15F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butTipMminus.Location = new System.Drawing.Point(122,47);
			this.butTipMminus.Name = "butTipMminus";
			this.butTipMminus.Size = new System.Drawing.Size(31,23);
			this.butTipMminus.TabIndex = 33;
			this.butTipMminus.Text = "-";
			this.butTipMminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butTipMminus.Click += new System.EventHandler(this.butTipMminus_Click);
			// 
			// butRotatePlus
			// 
			this.butRotatePlus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butRotatePlus.Autosize = true;
			this.butRotatePlus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRotatePlus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRotatePlus.CornerRadius = 4F;
			this.butRotatePlus.Font = new System.Drawing.Font("Microsoft Sans Serif",12F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butRotatePlus.Location = new System.Drawing.Point(159,18);
			this.butRotatePlus.Name = "butRotatePlus";
			this.butRotatePlus.Size = new System.Drawing.Size(31,23);
			this.butRotatePlus.TabIndex = 30;
			this.butRotatePlus.Text = "+";
			this.butRotatePlus.Click += new System.EventHandler(this.butRotatePlus_Click);
			// 
			// butRotateMinus
			// 
			this.butRotateMinus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butRotateMinus.Autosize = true;
			this.butRotateMinus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRotateMinus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRotateMinus.CornerRadius = 4F;
			this.butRotateMinus.Font = new System.Drawing.Font("Microsoft Sans Serif",15F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butRotateMinus.Location = new System.Drawing.Point(122,18);
			this.butRotateMinus.Name = "butRotateMinus";
			this.butRotateMinus.Size = new System.Drawing.Size(31,23);
			this.butRotateMinus.TabIndex = 31;
			this.butRotateMinus.Text = "-";
			this.butRotateMinus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butRotateMinus.Click += new System.EventHandler(this.butRotateMinus_Click);
			// 
			// textTipB
			// 
			this.textTipB.Location = new System.Drawing.Point(72,77);
			this.textTipB.Name = "textTipB";
			this.textTipB.Size = new System.Drawing.Size(38,20);
			this.textTipB.TabIndex = 29;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(3,77);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(68,18);
			this.label11.TabIndex = 28;
			this.label11.Text = "Labial Tip";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTipM
			// 
			this.textTipM.Location = new System.Drawing.Point(72,49);
			this.textTipM.Name = "textTipM";
			this.textTipM.Size = new System.Drawing.Size(38,20);
			this.textTipM.TabIndex = 25;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(3,49);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(68,18);
			this.label12.TabIndex = 24;
			this.label12.Text = "Mesial Tip";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRotate
			// 
			this.textRotate.Location = new System.Drawing.Point(72,20);
			this.textRotate.Name = "textRotate";
			this.textRotate.Size = new System.Drawing.Size(38,20);
			this.textRotate.TabIndex = 21;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(3,20);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(68,18);
			this.label15.TabIndex = 20;
			this.label15.Text = "Rotate";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.butShiftBplus);
			this.groupBox3.Controls.Add(this.butShiftBminus);
			this.groupBox3.Controls.Add(this.butShiftOplus);
			this.groupBox3.Controls.Add(this.butShiftOminus);
			this.groupBox3.Controls.Add(this.butShiftMplus);
			this.groupBox3.Controls.Add(this.butShiftMminus);
			this.groupBox3.Controls.Add(this.textShiftB);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.textShiftO);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.textShiftM);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Location = new System.Drawing.Point(20,12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(207,109);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Shift millimeters";
			// 
			// butShiftBplus
			// 
			this.butShiftBplus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShiftBplus.Autosize = true;
			this.butShiftBplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftBplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftBplus.CornerRadius = 4F;
			this.butShiftBplus.Font = new System.Drawing.Font("Microsoft Sans Serif",12F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butShiftBplus.Location = new System.Drawing.Point(158,76);
			this.butShiftBplus.Name = "butShiftBplus";
			this.butShiftBplus.Size = new System.Drawing.Size(31,23);
			this.butShiftBplus.TabIndex = 40;
			this.butShiftBplus.Text = "+";
			this.butShiftBplus.Click += new System.EventHandler(this.butShiftBplus_Click);
			// 
			// butShiftBminus
			// 
			this.butShiftBminus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShiftBminus.Autosize = true;
			this.butShiftBminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftBminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftBminus.CornerRadius = 4F;
			this.butShiftBminus.Font = new System.Drawing.Font("Microsoft Sans Serif",15F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butShiftBminus.Location = new System.Drawing.Point(121,76);
			this.butShiftBminus.Name = "butShiftBminus";
			this.butShiftBminus.Size = new System.Drawing.Size(31,23);
			this.butShiftBminus.TabIndex = 41;
			this.butShiftBminus.Text = "-";
			this.butShiftBminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butShiftBminus.Click += new System.EventHandler(this.butShiftBminus_Click);
			// 
			// butShiftOplus
			// 
			this.butShiftOplus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShiftOplus.Autosize = true;
			this.butShiftOplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftOplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftOplus.CornerRadius = 4F;
			this.butShiftOplus.Font = new System.Drawing.Font("Microsoft Sans Serif",12F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butShiftOplus.Location = new System.Drawing.Point(158,47);
			this.butShiftOplus.Name = "butShiftOplus";
			this.butShiftOplus.Size = new System.Drawing.Size(31,23);
			this.butShiftOplus.TabIndex = 38;
			this.butShiftOplus.Text = "+";
			this.butShiftOplus.Click += new System.EventHandler(this.butShiftOplus_Click);
			// 
			// butShiftOminus
			// 
			this.butShiftOminus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShiftOminus.Autosize = true;
			this.butShiftOminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftOminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftOminus.CornerRadius = 4F;
			this.butShiftOminus.Font = new System.Drawing.Font("Microsoft Sans Serif",15F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butShiftOminus.Location = new System.Drawing.Point(121,47);
			this.butShiftOminus.Name = "butShiftOminus";
			this.butShiftOminus.Size = new System.Drawing.Size(31,23);
			this.butShiftOminus.TabIndex = 39;
			this.butShiftOminus.Text = "-";
			this.butShiftOminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butShiftOminus.Click += new System.EventHandler(this.butShiftOminus_Click);
			// 
			// butShiftMplus
			// 
			this.butShiftMplus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShiftMplus.Autosize = true;
			this.butShiftMplus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftMplus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftMplus.CornerRadius = 4F;
			this.butShiftMplus.Font = new System.Drawing.Font("Microsoft Sans Serif",12F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butShiftMplus.Location = new System.Drawing.Point(158,18);
			this.butShiftMplus.Name = "butShiftMplus";
			this.butShiftMplus.Size = new System.Drawing.Size(31,23);
			this.butShiftMplus.TabIndex = 36;
			this.butShiftMplus.Text = "+";
			this.butShiftMplus.Click += new System.EventHandler(this.butShiftMplus_Click);
			// 
			// butShiftMminus
			// 
			this.butShiftMminus.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butShiftMminus.Autosize = true;
			this.butShiftMminus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShiftMminus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShiftMminus.CornerRadius = 4F;
			this.butShiftMminus.Font = new System.Drawing.Font("Microsoft Sans Serif",15F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.butShiftMminus.Location = new System.Drawing.Point(121,18);
			this.butShiftMminus.Name = "butShiftMminus";
			this.butShiftMminus.Size = new System.Drawing.Size(31,23);
			this.butShiftMminus.TabIndex = 37;
			this.butShiftMminus.Text = "-";
			this.butShiftMminus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.butShiftMminus.Click += new System.EventHandler(this.butShiftMminus_Click);
			// 
			// textShiftB
			// 
			this.textShiftB.Location = new System.Drawing.Point(72,77);
			this.textShiftB.Name = "textShiftB";
			this.textShiftB.Size = new System.Drawing.Size(38,20);
			this.textShiftB.TabIndex = 29;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(3,77);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(68,18);
			this.label10.TabIndex = 28;
			this.label10.Text = "Labial";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textShiftO
			// 
			this.textShiftO.Location = new System.Drawing.Point(72,49);
			this.textShiftO.Name = "textShiftO";
			this.textShiftO.Size = new System.Drawing.Size(38,20);
			this.textShiftO.TabIndex = 25;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(3,49);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(68,18);
			this.label9.TabIndex = 24;
			this.label9.Text = "Occlusal";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textShiftM
			// 
			this.textShiftM.Location = new System.Drawing.Point(72,20);
			this.textShiftM.Name = "textShiftM";
			this.textShiftM.Size = new System.Drawing.Size(38,20);
			this.textShiftM.TabIndex = 21;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(3,20);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(68,18);
			this.label8.TabIndex = 20;
			this.label8.Text = "Mesial";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPrimary
			// 
			this.tabPrimary.Controls.Add(this.groupBox5);
			this.tabPrimary.Controls.Add(this.butMixed);
			this.tabPrimary.Controls.Add(this.butAllPrimary);
			this.tabPrimary.Controls.Add(this.butAllPerm);
			this.tabPrimary.Location = new System.Drawing.Point(4,22);
			this.tabPrimary.Name = "tabPrimary";
			this.tabPrimary.Size = new System.Drawing.Size(489,235);
			this.tabPrimary.TabIndex = 2;
			this.tabPrimary.Text = "Primary";
			this.tabPrimary.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.butPerm);
			this.groupBox5.Controls.Add(this.butPrimary);
			this.groupBox5.Location = new System.Drawing.Point(20,12);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(153,90);
			this.groupBox5.TabIndex = 21;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Set Selected Teeth";
			// 
			// butPerm
			// 
			this.butPerm.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butPerm.Autosize = true;
			this.butPerm.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPerm.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPerm.CornerRadius = 4F;
			this.butPerm.Location = new System.Drawing.Point(11,53);
			this.butPerm.Name = "butPerm";
			this.butPerm.Size = new System.Drawing.Size(82,23);
			this.butPerm.TabIndex = 15;
			this.butPerm.Text = "Permanent";
			this.butPerm.Click += new System.EventHandler(this.butPerm_Click);
			// 
			// butPrimary
			// 
			this.butPrimary.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butPrimary.Autosize = true;
			this.butPrimary.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrimary.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrimary.CornerRadius = 4F;
			this.butPrimary.Location = new System.Drawing.Point(11,21);
			this.butPrimary.Name = "butPrimary";
			this.butPrimary.Size = new System.Drawing.Size(82,23);
			this.butPrimary.TabIndex = 14;
			this.butPrimary.Text = "Primary";
			this.butPrimary.Click += new System.EventHandler(this.butPrimary_Click);
			// 
			// butMixed
			// 
			this.butMixed.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butMixed.Autosize = true;
			this.butMixed.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMixed.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMixed.CornerRadius = 4F;
			this.butMixed.Location = new System.Drawing.Point(334,33);
			this.butMixed.Name = "butMixed";
			this.butMixed.Size = new System.Drawing.Size(107,23);
			this.butMixed.TabIndex = 20;
			this.butMixed.Text = "Set Mixed Dentition";
			this.butMixed.Click += new System.EventHandler(this.butMixed_Click);
			// 
			// butAllPrimary
			// 
			this.butAllPrimary.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAllPrimary.Autosize = true;
			this.butAllPrimary.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAllPrimary.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAllPrimary.CornerRadius = 4F;
			this.butAllPrimary.Location = new System.Drawing.Point(201,33);
			this.butAllPrimary.Name = "butAllPrimary";
			this.butAllPrimary.Size = new System.Drawing.Size(107,23);
			this.butAllPrimary.TabIndex = 19;
			this.butAllPrimary.Text = "Set All Primary";
			this.butAllPrimary.Click += new System.EventHandler(this.butAllPrimary_Click);
			// 
			// butAllPerm
			// 
			this.butAllPerm.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAllPerm.Autosize = true;
			this.butAllPerm.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAllPerm.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAllPerm.CornerRadius = 4F;
			this.butAllPerm.Location = new System.Drawing.Point(201,65);
			this.butAllPerm.Name = "butAllPerm";
			this.butAllPerm.Size = new System.Drawing.Size(107,23);
			this.butAllPerm.TabIndex = 18;
			this.butAllPerm.Text = "Set All Permament";
			this.butAllPerm.Click += new System.EventHandler(this.butAllPerm_Click);
			// 
			// toothChart
			// 
			this.toothChart.ColorBackground = System.Drawing.Color.FromArgb(((int)(((byte)(95)))),((int)(((byte)(95)))),((int)(((byte)(130)))));
			this.toothChart.Location = new System.Drawing.Point(0,146);
			this.toothChart.Name = "toothChart";
			this.toothChart.Size = new System.Drawing.Size(410,307);
			this.toothChart.TabIndex = 194;
			this.toothChart.TaoRenderEnabled = false;
			this.toothChart.Text = "graphicalToothChart1";
			this.toothChart.UseInternational = false;
			// 
			// gridProg
			// 
			this.gridProg.HScrollVisible = true;
			this.gridProg.Location = new System.Drawing.Point(415,295);
			this.gridProg.Name = "gridProg";
			this.gridProg.ScrollValue = 0;
			this.gridProg.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.gridProg.Size = new System.Drawing.Size(524,227);
			this.gridProg.TabIndex = 192;
			this.gridProg.Title = "Progress Notes";
			this.gridProg.TranslationName = "TableProg";
			this.gridProg.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProg_CellDoubleClick);
			this.gridProg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridProg_MouseUp);
			this.gridProg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridProg_KeyDown);
			// 
			// butBig
			// 
			this.butBig.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butBig.Autosize = true;
			this.butBig.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBig.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBig.CornerRadius = 4F;
			this.butBig.Location = new System.Drawing.Point(375,146);
			this.butBig.Name = "butBig";
			this.butBig.Size = new System.Drawing.Size(35,23);
			this.butBig.TabIndex = 191;
			this.butBig.Text = "Big";
			this.butBig.Click += new System.EventHandler(this.butBig_Click);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0,0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(939,29);
			this.ToolBarMain.TabIndex = 177;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// button1
			// 
			this.button1.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.button1.Autosize = true;
			this.button1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.button1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.button1.CornerRadius = 4F;
			this.button1.Location = new System.Drawing.Point(127,692);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75,23);
			this.button1.TabIndex = 36;
			this.button1.Text = "invisible";
			this.button1.Visible = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textTreatmentNotes
			// 
			this.textTreatmentNotes.AcceptsReturn = true;
			this.textTreatmentNotes.Location = new System.Drawing.Point(0,456);
			this.textTreatmentNotes.Multiline = true;
			this.textTreatmentNotes.Name = "textTreatmentNotes";
			this.textTreatmentNotes.QuickPasteType = OpenDentBusiness.QuickPasteType.ChartTreatment;
			this.textTreatmentNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textTreatmentNotes.Size = new System.Drawing.Size(411,71);
			this.textTreatmentNotes.TabIndex = 187;
			this.textTreatmentNotes.Leave += new System.EventHandler(this.textTreatmentNotes_Leave);
			this.textTreatmentNotes.TextChanged += new System.EventHandler(this.textTreatmentNotes_TextChanged);
			// 
			// gridPtInfo
			// 
			this.gridPtInfo.HScrollVisible = false;
			this.gridPtInfo.Location = new System.Drawing.Point(0,528);
			this.gridPtInfo.Name = "gridPtInfo";
			this.gridPtInfo.ScrollValue = 0;
			this.gridPtInfo.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.gridPtInfo.Size = new System.Drawing.Size(411,212);
			this.gridPtInfo.TabIndex = 193;
			this.gridPtInfo.Title = "Patient Info";
			this.gridPtInfo.TranslationName = "TableChartPtInfo";
			this.gridPtInfo.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPtInfo_CellDoubleClick);
			// 
			// ContrChart
			// 
			this.Controls.Add(this.gridProg);
			this.Controls.Add(this.butBig);
			this.Controls.Add(this.tabProc);
			this.Controls.Add(this.panelImages);
			this.Controls.Add(this.tabControlImages);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.groupPlanned);
			this.Controls.Add(this.groupShow);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textTreatmentNotes);
			this.Controls.Add(this.gridPtInfo);
			this.Controls.Add(this.toothChart);
			this.Controls.Add(this.label4);
			this.Name = "ContrChart";
			this.Size = new System.Drawing.Size(939,755);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ContrChart_Layout);
			this.Resize += new System.EventHandler(this.ContrChart_Resize);
			this.groupBox2.ResumeLayout(false);
			this.groupPlanned.ResumeLayout(false);
			this.groupShow.ResumeLayout(false);
			this.tabControlImages.ResumeLayout(false);
			this.panelImages.ResumeLayout(false);
			this.tabProc.ResumeLayout(false);
			this.tabEnterTx.ResumeLayout(false);
			this.tabEnterTx.PerformLayout();
			this.tabMissing.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabMovements.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPrimary.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		///<summary></summary>
		public bool DataValid{
      get{
        return dataValid;
      }
      set{
        dataValid=value;
      }
    }

		private void ContrChart_Layout(object sender, System.Windows.Forms.LayoutEventArgs e){
			gridProg.Height=ClientSize.Height-tabControlImages.Height-gridProg.Location.Y+1;
			if(panelImages.Visible){
				gridProg.Height-=(panelImages.Height+2);
			}
		}

		private void ContrChart_Resize(object sender,EventArgs e) {
			if(gridProg.Columns !=null && gridProg.Columns.Count>0){
				int gridW=0;
				for(int i=0;i<gridProg.Columns.Count;i++){
					gridW+=gridProg.Columns[i].ColWidth;
				}
				if(gridProg.Location.X+gridW+20 < ClientSize.Width){//if space is big enough to allow full width
					gridProg.Width=gridW+20;
				}
				else{
					if(ClientSize.Width>gridProg.Location.X){//prevents an error
						gridProg.Width=ClientSize.Width-gridProg.Location.X-1;
					}
				}
			}
			gridPtInfo.Height=tabControlImages.Top-gridPtInfo.Top;
		}

		//private void ContrChart_Resize(object sender, System.EventArgs e) {
		//	tbProg.LayoutTables();
		//}

		///<summary></summary>
		public void InstantClasses(){
			newStatus=ProcStat.TP;
			ApptPlanned=new ContrApptSingle();
			ApptPlanned.Info.IsNext=true;
			ApptPlanned.Location=new Point(1,17);
			ApptPlanned.Visible=false;
			groupPlanned.Controls.Add(ApptPlanned);
			ApptPlanned.DoubleClick += new System.EventHandler(ApptPlanned_DoubleClick);
			tabProc.SelectedIndex=0;
			tabProc.Height=253;
			gridProg.Location=new Point(tabProc.Left,tabProc.Bottom+2);
			gridProg.Height=this.ClientSize.Height-gridProg.Location.Y-2;
			//can't use Lan.F
			Lan.C(this,new Control[]
				{
				groupPlanned,
				checkDone,
				butNew,
				butClear,
				checkShowTP,
				checkShowC,
				checkShowE,
				checkShowR,
				checkRx,
				checkNotes,
				labelDx,
				butM,
				butOI,
				butD,
				butL,
				butBF,
				butV,
				groupBox2,
				radioEntryTP,
				radioEntryC,
				radioEntryEC,
				radioEntryEO,
				radioEntryR,
				checkToday,
				labelDx,
				label6,
				butAddProc,
				label14,
				//textADACode is handled in ClearButtons()
				butOK,
				label13,
				label4
			});
			//Lan.C(this,menuPrimary.MenuItems[0]);
			//Lan.C(this,menuPrimary.MenuItems[1]);
			//Lan.C(this,menuPrimary.MenuItems[2]);
			LayoutToolBar();
		}

		///<summary>Causes the toolbars to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			button=new ODToolBarButton(Lan.g(this,"Select Patient"),0,"","Patient");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=menuPatient;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"New Rx"),1,"","Rx"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			//button=new ODToolBarButton(Lan.g(this,"Primary"),-1
			//	,Lan.g(this,"Change the Primary/Permanent status of teeth"),"Primary");
			//button.Style=ODToolBarButtonStyle.DropDownButton;
			//button.DropDownMenu=menuPrimary;
			//ToolBarMain.Buttons.Add(button);
			//ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Perio Chart"),2,"","Perio"));
			ArrayList toolButItems=ToolButItems.GetForToolBar(ToolBarsAvail.ChartModule);
			for(int i=0;i<toolButItems.Count;i++){
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				ToolBarMain.Buttons.Add(new ODToolBarButton(((ToolButItem)toolButItems[i]).ButtonText
					,-1,"",((ToolButItem)toolButItems[i]).ProgramNum));
			}
			ToolBarMain.Invalidate();
			/*toolBarProcs.Buttons.Clear();
			button=new ODToolBarButton(Lan.g(this,"EnterTx"),-1,"","EnterTx");
			button.Style=ODToolBarButtonStyle.ToggleButton;
			toolBarProcs.Buttons.Add(button);
			button=new ODToolBarButton(Lan.g(this,"Initial"),-1,"","Initial");
			button.Style=ODToolBarButtonStyle.ToggleButton;
			toolBarProcs.Buttons.Add(button);*/
		}

		///<summary></summary>
		public void ModuleSelected(int patNum){
			//toothChart.Visible=true;
			EasyHideClinicalData();
			RefreshModuleData(patNum);
			RefreshModuleScreen();
		}

		///<summary></summary>
		public void ModuleUnselected(){
			//toothChart.Dispose();
			if(FamCur==null)
				return;
			if(TreatmentNoteChanged){
				PatientNoteCur.Treatment=textTreatmentNotes.Text;
				PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
				TreatmentNoteChanged=false;
			}
			FamCur=null;
			PatCur=null;
			PlanList=null;
			//from FillProgNotes:
			//ProcList=null;
			//Procedures.HList=null;
			//Procedures.MissingTeeth=null;
			//RxPats.List=null;
			//RefAttaches.List=null;
		}

		private void RefreshModuleData(int patNum){
			if(patNum==0){
				PatCur=null;
				FamCur=null;
				return;
			}
			FamCur=Patients.GetFamily(patNum);
			PatCur=FamCur.GetPatient(patNum);
			PlanList=InsPlans.Refresh(FamCur);
			PatPlanList=PatPlans.Refresh(patNum);
			BenefitList=Benefits.Refresh(PatPlanList);
			//CovPats.Refresh(PlanList,PatPlanList);
			PatientNoteCur=PatientNotes.Refresh(patNum,PatCur.Guarantor);
      //ClaimProcs.Refresh();
			//RefAttaches.Refresh();
			GetImageFolder();
			DocAttachList=DocAttaches.Refresh(patNum);
			DocumentList=Documents.Refresh(DocAttachList);
			//Procs get refreshed in FillProgNotes
			ApptList=Appointments.GetForPat(patNum);
			ToothInitialList=ToothInitials.Refresh(patNum);
		}

		private void GetImageFolder(){
			//this is the same code as in the Images module
			Patient patOld=PatCur.Copy();
			if(PatCur.ImageFolder==""){//creates new folder for patient if none present
				string name=PatCur.LName+PatCur.FName;
				string folder="";
				for(int i=0;i<name.Length;i++){
					if(Char.IsLetter(name,i)){
						folder+=name.Substring(i,1);
					}
				}
				folder+=PatCur.PatNum.ToString();//ensures unique name
				try{
					PatCur.ImageFolder=folder;
					patFolder=((Pref)PrefB.HList["DocPath"]).ValueString
						+PatCur.ImageFolder.Substring(0,1)+@"\"
						+PatCur.ImageFolder+@"\";
					Directory.CreateDirectory(patFolder);
					Patients.Update(PatCur,patOld);
					//ModuleSelected(PatCur.PatNum);
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
					//MessageBox.Show(Lan.g(this,"Error.  Could not create folder for patient. "));
					patFolder="";
					return;
				}
			}
			else{//patient folder already created once
				patFolder=((Pref)PrefB.HList["DocPath"]).ValueString
					+PatCur.ImageFolder.Substring(0,1)+@"\"
					+PatCur.ImageFolder+@"\";
			}
			if(!Directory.Exists(patFolder)){//this makes it more resiliant and allows copies
					//of the opendentaldata folder to be used in read-only situations.
				try{
					Directory.CreateDirectory(patFolder);
				}
				catch{
					MessageBox.Show(Lan.g(this,"Error.  Could not create folder for patient. "));
					patFolder="";
					return;
				}
			}
		}

		private void RefreshModuleScreen(){
			ParentForm.Text=Patients.GetMainTitle(PatCur);
			if(PatCur==null){
				groupShow.Enabled=false;
				gridPtInfo.Enabled=false;
				groupPlanned.Enabled=false;
				toothChart.Enabled=false;
				gridProg.Enabled=false;
				butBig.Enabled=false;
				ToolBarMain.Buttons["Rx"].Enabled=false;
				ToolBarMain.Buttons["Perio"].Enabled=false;
				tabProc.Enabled=false;
			}
			else {
				groupShow.Enabled=true;
				gridPtInfo.Enabled=true;
				groupPlanned.Enabled=true;
				toothChart.Enabled=true;
				gridProg.Enabled=true;
				butBig.Enabled=true;
				ToolBarMain.Buttons["Rx"].Enabled=true;
				ToolBarMain.Buttons["Perio"].Enabled=true;
				tabProc.Enabled=true;
			}
			FillPatientButton();
			ToolBarMain.Invalidate();
			ClearButtons();
			FillProgNotes();
			FillPlanned();
			FillPtInfo();
      FillDxProcImage();
			FillImages();
		}

		private void FillPatientButton(){
			Patients.AddPatsToMenu(menuPatient,new EventHandler(menuPatient_Click),PatCur,FamCur);
		}

		private void menuPatient_Click(object sender,System.EventArgs e) {
			int newPatNum=Patients.ButtonSelect(menuPatient,sender,FamCur);
			OnPatientSelected(newPatNum);
			ModuleSelected(newPatNum);
		}

		private void EasyHideClinicalData(){
			if(((Pref)PrefB.HList["EasyHideClinical"]).ValueString=="1"){
				gridPtInfo.Visible=false;
				checkShowE.Visible=false;
				checkShowR.Visible=false;
				checkRx.Visible=false;
				checkComm.Visible=false;
				checkNotes.Visible=false;
				butShowNone.Visible=false;
				butShowAll.Visible=false;
				//panelEnterTx.Visible=false;//next line changes it, though
				radioEntryEC.Visible=false;
				radioEntryEO.Visible=false;
				radioEntryR.Visible=false;
				labelDx.Visible=false;
				listDx.Visible=false;
			}
			else{
				gridPtInfo.Visible=true;
				checkShowE.Visible=true;
				checkShowR.Visible=true;
				checkRx.Visible=true;
				checkComm.Visible=true;
				checkNotes.Visible=true;
				butShowNone.Visible=true;
				butShowAll.Visible=true;
				radioEntryEC.Visible=true;
				radioEntryEO.Visible=true;
				radioEntryR.Visible=true;
				labelDx.Visible=true;
				listDx.Visible=true;
			}
		}

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				//standard predefined button
				switch(e.Button.Tag.ToString()){
					case "Patient":
						OnPatient_Click();
						break;
					case "Rx":
						OnRx_Click();
						break;
					case "Primary":
						//only respond to dropdown
						//OnPrimary_Click();
						break;
					case "Perio":
						OnPerio_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(int)){
				Programs.Execute((int)e.Button.Tag,PatCur);
			}
		}

		private void OnPatient_Click(){
			FormPatientSelect formPS=new FormPatientSelect();
			formPS.ShowDialog();
			if(formPS.DialogResult==DialogResult.OK){
				OnPatientSelected(formPS.SelectedPatNum);
				ModuleSelected(formPS.SelectedPatNum);
			}
		}

		///<summary></summary>
		private void OnPatientSelected(int patNum){
			PatientSelectedEventArgs eArgs=new OpenDental.PatientSelectedEventArgs(patNum);
			if(PatientSelected!=null)
				PatientSelected(this,eArgs);
		}

		private void OnRx_Click(){
			if(!Security.IsAuthorized(Permissions.RxCreate)){
				return;
			}
			FormRxSelect FormRS=new FormRxSelect(PatCur);
			FormRS.ShowDialog();
			if(FormRS.DialogResult!=DialogResult.OK) return;
			ModuleSelected(PatCur.PatNum);
			SecurityLogs.MakeLogEntry(Permissions.RxCreate,PatCur.PatNum,PatCur.GetNameLF());
		}

		private void OnPerio_Click(){
			FormPerio FormP=new FormPerio(PatCur);
			FormP.ShowDialog();
		}

		private void FillPlanned(){
			if(PatCur==null){
				ApptPlanned.Visible=false;
				checkDone.Checked=false;
				butPin.Enabled=false;
				butClear.Enabled=false;
				labelMinutes.Text="";
				return;
			}
			if(PatCur.PlannedIsDone) {
				checkDone.Checked=true;
			}
			else {
				checkDone.Checked=false;
			}
			if(PatCur.NextAptNum==0){
				ApptPlanned.Visible=false;
				butPin.Enabled=false;
				butClear.Enabled=false;
				labelMinutes.Text="";
				return;
			}
			//MessageBox.Show
			Appointment apt=Appointments.GetOneApt(PatCur.NextAptNum);
			if(apt==null){//next appointment not found
				Patient patOld=PatCur.Copy();
				PatCur.NextAptNum=0;
				Patients.Update(PatCur,patOld);
				FamCur=Patients.GetFamily(PatCur.PatNum);//might be overkill
				ApptPlanned.Visible=false;
				checkDone.Checked=false;
				butPin.Enabled=false;
				butClear.Enabled=false;
				labelMinutes.Text="";
				return;
			}
			ApptPlanned.Info.MyApt=apt.Copy();
			Procedure[] procs=Procedures.GetProcsForSingle(ApptPlanned.Info.MyApt.AptNum,true);
			ApptPlanned.Info.Procs=procs;
			ApptPlanned.Info.Production=Procedures.GetProductionOneApt(ApptPlanned.Info.MyApt.AptNum,procs,true);
			ApptPlanned.Info.MyPatient=PatCur.Copy();
			ApptPlanned.SetSize();
			ApptPlanned.Width=114;
			ApptPlanned.CreateShadow();
			ApptPlanned.Visible=true;
			ApptPlanned.Refresh();
			butPin.Enabled=true;
			butClear.Enabled=true;
			labelMinutes.Text=(ApptPlanned.Info.MyApt.Pattern.Length*5).ToString()+" minutes";
			//ContrApptSingle.ApptIsSelected=false;
		}

		private void FillPtInfo(){
			gridPtInfo.Height=tabControlImages.Top-gridPtInfo.Top;
			textTreatmentNotes.Text="";
			if(PatCur!=null){
				textTreatmentNotes.Text=PatientNoteCur.Treatment;
				textTreatmentNotes.Enabled=true;
				textTreatmentNotes.Select(textTreatmentNotes.Text.Length+2,1);
				textTreatmentNotes.ScrollToCaret();
				TreatmentNoteChanged=false;
			}
			gridPtInfo.BeginUpdate();
			gridPtInfo.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",100);//Lan.g("TableChartPtInfo",""),);
			gridPtInfo.Columns.Add(col);
			col=new ODGridColumn("",300);
			gridPtInfo.Columns.Add(col);
			gridPtInfo.Rows.Clear();
			if(PatCur==null){
				gridPtInfo.EndUpdate();
				return;
			}
			ODGridRow row;
			//Credit type
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","ABC0"));
			row.Cells.Add(PatCur.CreditType);
			gridPtInfo.Rows.Add(row);
			//Billing type
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Billing Type"));
			row.Cells.Add(DefB.GetName(DefCat.BillingTypes,PatCur.BillingType));
			gridPtInfo.Rows.Add(row);
			//Referral
			RefAttach[] RefAttachList=RefAttaches.Refresh(PatCur.PatNum);
			string referral="";
			for(int i=0;i<RefAttachList.Length;i++) {
				if(RefAttachList[i].IsFrom) {
					referral=Referrals.GetName(RefAttachList[i].ReferralNum);
					break;
				}
			}
			if(referral=="") {
				referral="??";
			}
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Referred From"));
			row.Cells.Add(referral);
			gridPtInfo.Rows.Add(row);
			//Date First Visit
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Date First Visit"));
			if(PatCur.DateFirstVisit.Year<1880)
				row.Cells.Add("??");
			else if(PatCur.DateFirstVisit==DateTime.Today)
				row.Cells.Add(Lan.g("TableChartPtInfo","NEW PAT"));
			else
				row.Cells.Add(PatCur.DateFirstVisit.ToShortDateString());
			gridPtInfo.Rows.Add(row);
			//PriIns
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Pri Ins"));
			string name;
			if(PatPlanList.Length>0) {
				name=InsPlans.GetCarrierName(PatPlans.GetPlanNum(PatPlanList,1),PlanList);
				if(PatPlanList[0].IsPending)
					name+=Lan.g("TableChartPtInfo"," (pending)");
				row.Cells.Add(name);
			}
			else{
				row.Cells.Add("");
			}
			gridPtInfo.Rows.Add(row);
			//SecIns
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Sec Ins"));
			if(PatPlanList.Length>1) {
				name=InsPlans.GetCarrierName(PatPlans.GetPlanNum(PatPlanList,2),PlanList);
				if(PatPlanList[1].IsPending)
					name+=Lan.g("TableChartPtInfo"," (pending)");
				row.Cells.Add(name);
			}
			else {
				row.Cells.Add("");
			}
			gridPtInfo.Rows.Add(row);
			ODGridCell cell;
			//premed flag. Row 6
			if(PatCur.Premed){
				row=new ODGridRow();
				row.Cells.Add("");
				cell=new ODGridCell();
				cell.Text=Lan.g("TableChartPtInfo","Premedicate");
				cell.ColorText=Color.Red;
				cell.Bold=YN.Yes;
				row.Cells.Add(cell);
				row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
				gridPtInfo.Rows.Add(row);
			}
			//diseases
			Disease[] DiseaseList=Diseases.Refresh(PatCur.PatNum);
			row=new ODGridRow();
			cell=new ODGridCell(Lan.g("TableChartPtInfo","Diseases"));
			cell.Bold=YN.Yes;
			row.Cells.Add(cell);
			if(DiseaseList.Length>0) {
				row.Cells.Add("");
			}
			else {
				row.Cells.Add(Lan.g("TableChartPtInfo","none"));
			}
			row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
			gridPtInfo.Rows.Add(row);
			for(int i=0;i<DiseaseList.Length;i++) {
				row=new ODGridRow();
				cell=new ODGridCell(DiseaseDefs.GetName(DiseaseList[i].DiseaseDefNum));
				cell.ColorText=Color.Red;
				cell.Bold=YN.Yes;
				row.Cells.Add(cell);
				row.Cells.Add(DiseaseList[i].PatNote);
				row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
				gridPtInfo.Rows.Add(row);
			}
			//MedUrgNote 
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Med Urgent"));
			cell=new ODGridCell();
			cell.Text=PatCur.MedUrgNote;
			cell.ColorText=Color.Red;
			cell.Bold=YN.Yes;
			row.Cells.Add(cell);
			row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
			gridPtInfo.Rows.Add(row);
			//Medical
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Medical Summary"));
			row.Cells.Add(PatientNoteCur.Medical);
			row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
			gridPtInfo.Rows.Add(row);
			//Service
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableChartPtInfo","Service Notes"));
			row.Cells.Add(PatientNoteCur.Service);
			row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
			gridPtInfo.Rows.Add(row);
			//medications
			Medications.Refresh();
			MedicationPats.Refresh(PatCur.PatNum);
			row=new ODGridRow();
			cell=new ODGridCell(Lan.g("TableChartPtInfo","Medications"));
			cell.Bold=YN.Yes;
			row.Cells.Add(cell);
			if(MedicationPats.List.Length>0) {
				row.Cells.Add("");
			}
			else{
				row.Cells.Add(Lan.g("TableChartPtInfo","none"));
			}
			row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
			gridPtInfo.Rows.Add(row);
			string text;
			Medication med;
			for(int i=0;i<MedicationPats.List.Length;i++) {
				row=new ODGridRow();
				med=Medications.GetMedication(MedicationPats.List[i].MedicationNum);
				text=med.MedName;
				if(med.MedicationNum != med.GenericNum){
					text+="("+Medications.GetMedication(med.GenericNum).MedName+")";
				}
				row.Cells.Add(text);
				text=MedicationPats.List[i].PatNote
					+"("+Medications.GetGeneric(MedicationPats.List[i].MedicationNum).Notes+")";
				row.Cells.Add(text);
				row.ColorBackG=DefB.Long[(int)DefCat.MiscColors][3].ItemColor;
				gridPtInfo.Rows.Add(row);
			}
			
			gridPtInfo.EndUpdate();
		}

		private void textTreatmentNotes_TextChanged(object sender, System.EventArgs e) {
			TreatmentNoteChanged=true;
		}

		private void textTreatmentNotes_Leave(object sender, System.EventArgs e) {
			//need to skip this if selecting another module. Handled in ModuleUnselected due to click event
			if(FamCur==null)
				return;
			if(TreatmentNoteChanged){
				PatientNoteCur.Treatment=textTreatmentNotes.Text;
				PatientNotes.Update(PatientNoteCur,PatCur.Guarantor);
				TreatmentNoteChanged=false;
			}
		}

		///<summary>The supplied procedure row must include these columns: ProcDate,ProcStatus,ADACode,Surf,ToothNum, and ToothRange, all in raw database format.</summary>
		private bool ShouldDisplayProc(DataRow row){
			//if printing for hospital
			if(hospitalDate.Year>1880) {
				if(hospitalDate!=PIn.PDateT(row["ProcDate"].ToString()).Date) {
					return false;
				}
				if(row["ProcStatus"].ToString()!=((int)ProcStat.C).ToString()) {
					return false;
				}
			}
			if(checkShowTeeth.Checked) {
				bool showProc=false;
				ArrayList selectedTeeth=new ArrayList();//integers 1-32
				for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
					selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
				}
				switch(ProcedureCodes.GetProcCode(row["ADACode"].ToString()).TreatArea) {
					case TreatmentArea.Arch:
						for(int s=0;s<selectedTeeth.Count;s++) {
							if(row["Surf"].ToString()=="U" && (int)selectedTeeth[s]<17) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="L" && (int)selectedTeeth[s]>16) {
								showProc=true;
							}
						}
						break;
					case TreatmentArea.Mouth:
					case TreatmentArea.None:
					case TreatmentArea.Sextant://nobody will miss it
						showProc=false;
						break;
					case TreatmentArea.Quad:
						for(int s=0;s<selectedTeeth.Count;s++) {
							if(row["Surf"].ToString()=="UR" && (int)selectedTeeth[s]<=8) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="UL" && (int)selectedTeeth[s]>=9 && (int)selectedTeeth[s]<=16) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="LL" && (int)selectedTeeth[s]>=17 && (int)selectedTeeth[s]<=24) {
								showProc=true;
							}
							else if(row["Surf"].ToString()=="LR" && (int)selectedTeeth[s]>=25 && (int)selectedTeeth[s]<=32) {
								showProc=true;
							}
						}
						break;
					case TreatmentArea.Surf:
					case TreatmentArea.Tooth:
						for(int s=0;s<selectedTeeth.Count;s++) {
							if(Tooth.ToInt(row["ToothNum"].ToString())==(int)selectedTeeth[s]) {
								showProc=true;
							}
						}
						break;
					case TreatmentArea.ToothRange:
						string[] range=row["ToothRange"].ToString().Split(',');
						for(int s=0;s<selectedTeeth.Count;s++) {
							for(int r=0;r<range.Length;r++) {
								if(Tooth.ToInt(range[r])==(int)selectedTeeth[s]) {
									showProc=true;
								}
							}
						}
						break;
				}
				if(!showProc) {
					return false;
				}
			}
			switch((ProcStat)PIn.PInt(row["ProcStatus"].ToString())) {
				case ProcStat.TP:
					if(checkShowTP.Checked) {
						return true;
					}
					break;
				case ProcStat.C:
					if(checkShowC.Checked) {
						return true;
					}
					break;
				case ProcStat.EC:
					if(checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.EO:
					if(checkShowE.Checked) {
						return true;
					}
					break;
				case ProcStat.R:
					if(checkShowR.Checked) {
						return true;
					}
					break;
				case ProcStat.D:
					if(checkAudit.Checked) {
						return true;
					}
					break;
			}
			return false;
		}

		private void FillProgNotes(){
			ArrayList selectedTeeth=new ArrayList();//integers 1-32
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
			}
			DataSetMain=null;
			if(PatCur!=null){
				DataSetMain=ChartModule.GetAll(PatCur.PatNum,checkAudit.Checked);
			}
			gridProg.BeginUpdate();
			gridProg.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProg","Date"),67);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Th"),27);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Surf"),40);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Dx"),28);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Description"),218);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Stat"),25);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Prov"),42);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Amount"),48,HorizontalAlignment.Right);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","ADA Code"),62,HorizontalAlignment.Center);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","User"),62,HorizontalAlignment.Center);
			gridProg.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProg","Signed"),55,HorizontalAlignment.Center);
			gridProg.Columns.Add(col);
			gridProg.NoteSpanStart=2;
			gridProg.NoteSpanStop=7;
			gridProg.Rows.Clear();
			ODGridRow row;
			//Type type;
			if(DataSetMain==null) {
				gridProg.EndUpdate();
				FillToothChart(false);//?
				return;
			}
			DataTable table=DataSetMain.Tables["ProgNotes"];
			ProcList=new List<DataRow>();
			for(int i=0;i<table.Rows.Count;i++){
				if(table.Rows[i]["ProcNum"].ToString()!="0"){//if this is a procedure
					if(ShouldDisplayProc(table.Rows[i])){
						ProcList.Add(table.Rows[i]);//show it in the graphical tooth chart
						//and add it to the grid below.
					}
					else{
						continue;
					}
				}
				else if(table.Rows[i]["CommlogNum"].ToString()!="0"){//if this is a commlog
					if(!checkComm.Checked) {
						continue;
					}
				}
				else if(table.Rows[i]["RxNum"].ToString()!="0") {//if this is an Rx
					if(!checkRx.Checked){
						continue;
					}
				}
				row=new ODGridRow();
				row.ColorLborder=Color.Black;
				//remember that columns that start with lowercase are already altered for display rather than being raw data.
				row.Cells.Add(table.Rows[i]["procDate"].ToString());
				row.Cells.Add(table.Rows[i]["toothNum"].ToString());
				row.Cells.Add(table.Rows[i]["Surf"].ToString());
				row.Cells.Add(table.Rows[i]["dx"].ToString());
				row.Cells.Add(table.Rows[i]["description"].ToString());
				row.Cells.Add(table.Rows[i]["procStatus"].ToString());
				row.Cells.Add(table.Rows[i]["prov"].ToString());
				row.Cells.Add(table.Rows[i]["procFee"].ToString());
				row.Cells.Add(table.Rows[i]["ADACode"].ToString());
				row.Cells.Add(table.Rows[i]["user"].ToString());
				row.Cells.Add(table.Rows[i]["signature"].ToString());
				if(checkNotes.Checked){
					row.Note=table.Rows[i]["note"].ToString();
				}
				row.ColorText=Color.FromArgb(PIn.PInt(table.Rows[i]["colorText"].ToString()));
				row.ColorBackG=Color.FromArgb(PIn.PInt(table.Rows[i]["colorBackG"].ToString()));
				row.Tag=table.Rows[i];
				gridProg.Rows.Add(row);
			}
			if(gridProg.Columns !=null && gridProg.Columns.Count>0) {
				int gridW=0;
				for(int i=0;i<gridProg.Columns.Count;i++) {
					gridW+=gridProg.Columns[i].ColWidth;
				}
				if(gridProg.Location.X+gridW+20 < ClientSize.Width) {//if space is big enough to allow full width
					gridProg.Width=gridW+20;
				}
				else {
					gridProg.Width=ClientSize.Width-gridProg.Location.X-1;
				}
			}
			gridProg.EndUpdate();
			gridProg.ScrollToEnd();
			FillToothChart(false);
		}

		///<summary>This is, of course, called when module refreshed.  But it's also called when user sets missing teeth or tooth movements.  In that case, the Progress notes are not refreshed, so it's a little faster.  This also fills in the movement amounts.</summary>
		private void FillToothChart(bool retainSelection){
			Cursor=Cursors.WaitCursor;
			toothChart.SuspendLayout();
			toothChart.UseInternational=PrefB.GetBool("UseInternationalToothNumbers");
			toothChart.ColorBackground=DefB.Long[(int)DefCat.ChartGraphicColors][10].ItemColor;
			toothChart.ColorText=DefB.Long[(int)DefCat.ChartGraphicColors][11].ItemColor;
			toothChart.ColorTextHighlight=DefB.Long[(int)DefCat.ChartGraphicColors][12].ItemColor;
			toothChart.ColorBackHighlight=DefB.Long[(int)DefCat.ChartGraphicColors][13].ItemColor;
			//remember which teeth were selected
			ArrayList selectedTeeth=new ArrayList();//integers 1-32
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				selectedTeeth.Add(Tooth.ToInt(toothChart.SelectedTeeth[i]));
			}
			toothChart.ResetTeeth();
			if(PatCur==null) {
				toothChart.ResumeLayout();
				FillMovementsAndHidden();
				Cursor=Cursors.Default;
				return;
			}
			if(checkShowTeeth.Checked || retainSelection) {
				for(int i=0;i<selectedTeeth.Count;i++) {
					toothChart.SetSelected((int)selectedTeeth[i],true);
				}
			}
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for(int i=0;i<ToothInitialList.Length;i++){
				if(ToothInitialList[i].InitialType==ToothInitialType.Primary){
					toothChart.SetToPrimary(ToothInitialList[i].ToothNum);
				}
			}
			for(int i=0;i<ToothInitialList.Length;i++){
				switch(ToothInitialList[i].InitialType){
					case ToothInitialType.Missing:
						toothChart.SetInvisible(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChart.HideTooth(ToothInitialList[i].ToothNum);
						break;
					//case ToothInitialType.Primary:
					//	break;
					case ToothInitialType.Rotate:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,ToothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,ToothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,ToothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,ToothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,ToothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,0,ToothInitialList[i].Movement);
						break;
				}
			}
			DrawProcsOfStatus(ProcStat.EO);
			DrawProcsOfStatus(ProcStat.EC);
			DrawProcsOfStatus(ProcStat.C);
			DrawProcsOfStatus(ProcStat.R);
			DrawProcsOfStatus(ProcStat.TP);
			toothChart.ResumeLayout();
			FillMovementsAndHidden();
			Cursor=Cursors.Default;
		}

		private void DrawProcsOfStatus(ProcStat procStat){
			//this requires: ProcStatus, ADACode, ToothNum, Surf, and ToothRange.  All need to be raw database values.
			string[] teeth;
			Color cLight=Color.White;
			Color cDark=Color.White;
			for(int i=0;i<ProcList.Count;i++) {
				if(PIn.PInt(ProcList[i]["ProcStatus"].ToString())!=(int)procStat) {
					continue;
				}
				if(ProcedureCodes.GetProcCode(ProcList[i]["ADACode"].ToString()).PaintType==ToothPaintingType.Extraction && (
					PIn.PInt(ProcList[i]["ProcStatus"].ToString())==(int)ProcStat.C
					|| PIn.PInt(ProcList[i]["ProcStatus"].ToString())==(int)ProcStat.EC
					|| PIn.PInt(ProcList[i]["ProcStatus"].ToString())==(int)ProcStat.EO
					)) {
					continue;//prevents the red X. Missing teeth already handled.
				}
				if(ProcedureCodes.GetProcCode(ProcList[i]["ADACode"].ToString()).GraphicColor==Color.FromArgb(0)){
					switch((ProcStat)PIn.PInt(ProcList[i]["ProcStatus"].ToString())) {
						case ProcStat.C:
							cDark=DefB.Short[(int)DefCat.ChartGraphicColors][1].ItemColor;
							cLight=DefB.Short[(int)DefCat.ChartGraphicColors][6].ItemColor;
							break;
						case ProcStat.TP:
							cDark=DefB.Short[(int)DefCat.ChartGraphicColors][0].ItemColor;
							cLight=DefB.Short[(int)DefCat.ChartGraphicColors][5].ItemColor;
							break;
						case ProcStat.EC:
							cDark=DefB.Short[(int)DefCat.ChartGraphicColors][2].ItemColor;
							cLight=DefB.Short[(int)DefCat.ChartGraphicColors][7].ItemColor;
							break;
						case ProcStat.EO:
							cDark=DefB.Short[(int)DefCat.ChartGraphicColors][3].ItemColor;
							cLight=DefB.Short[(int)DefCat.ChartGraphicColors][8].ItemColor;
							break;
						case ProcStat.R:
							cDark=DefB.Short[(int)DefCat.ChartGraphicColors][4].ItemColor;
							cLight=DefB.Short[(int)DefCat.ChartGraphicColors][9].ItemColor;
							break;
					}
				}
				else{
					cDark=ProcedureCodes.GetProcCode(ProcList[i]["ADACode"].ToString()).GraphicColor;
					cLight=ProcedureCodes.GetProcCode(ProcList[i]["ADACode"].ToString()).GraphicColor;
				}
				switch(ProcedureCodes.GetProcCode(ProcList[i]["ADACode"].ToString()).PaintType){
					case ToothPaintingType.BridgeDark:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,ProcList[i]["ToothNum"].ToString())){
							toothChart.SetPontic(ProcList[i]["ToothNum"].ToString(),cDark);
						}
						else{
							toothChart.SetCrown(ProcList[i]["ToothNum"].ToString(),cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,ProcList[i]["ToothNum"].ToString())) {
							toothChart.SetPontic(ProcList[i]["ToothNum"].ToString(),cLight);
						}
						else {
							toothChart.SetCrown(ProcList[i]["ToothNum"].ToString(),cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						toothChart.SetCrown(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.CrownLight:
						toothChart.SetCrown(ProcList[i]["ToothNum"].ToString(),cLight);
						break;
					case ToothPaintingType.DentureDark:
						if(ProcList[i]["Surf"].ToString()=="U"){
							teeth=new string[14];
							for(int t=0;t<14;t++){
								teeth[t]=(t+2).ToString();
							}
						}
						else if(ProcList[i]["Surf"].ToString()=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else{
							teeth=ProcList[i]["ToothRange"].ToString().Split(new char[] {','});
						}
						for(int t=0;t<teeth.Length;t++){
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cDark);
							}
							else {
								toothChart.SetCrown(teeth[t],cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if(ProcList[i]["Surf"].ToString()=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(ProcList[i]["Surf"].ToString()=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=ProcList[i]["ToothRange"].ToString().Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cLight);
							}
							else {
								toothChart.SetCrown(teeth[t],cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						toothChart.SetBigX(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.FillingDark:
						toothChart.SetSurfaceColors(ProcList[i]["ToothNum"].ToString(),ProcList[i]["Surf"].ToString(),cDark);
						break;
				  case ToothPaintingType.FillingLight:
						toothChart.SetSurfaceColors(ProcList[i]["ToothNum"].ToString(),ProcList[i]["Surf"].ToString(),cLight);
						break;
					case ToothPaintingType.Implant:
						toothChart.SetImplant(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.PostBU:
						toothChart.SetBU(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.RCT:
						toothChart.SetRCT(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
					case ToothPaintingType.Sealant:
						toothChart.SetSealant(ProcList[i]["ToothNum"].ToString(),cDark);
						break;
				}
			}
		}

		private void checkToday_CheckedChanged(object sender, System.EventArgs e) {
			if(checkToday.Checked){
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			else{
				//
			}
		}

		///<summary>Gets run with each ModuleSelected.  Fills Dx, Priorities, ProcButtons, Date, and Image categories</summary>
    private void FillDxProcImage(){
			//if(textDate.errorProvider1.GetError(textDate)==""){
			if(checkToday.Checked){//textDate.Text=="" || 
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			//}
			listDx.Items.Clear();
			for(int i=0;i<DefB.Short[(int)DefCat.Diagnosis].Length;i++){//move to instantClasses?
				this.listDx.Items.Add(DefB.Short[(int)DefCat.Diagnosis][i].ItemName);
			}
			int selectedPriority=comboPriority.SelectedIndex;//retain current selection
			comboPriority.Items.Clear();
			comboPriority.Items.Add(Lan.g(this,"no priority"));
			for(int i=0;i<DefB.Short[(int)DefCat.TxPriorities].Length;i++){
				this.comboPriority.Items.Add(DefB.Short[(int)DefCat.TxPriorities][i].ItemName);
			}
			if(selectedPriority>0 && selectedPriority<comboPriority.Items.Count)
				//set the selected to what it was before.
				comboPriority.SelectedIndex=selectedPriority;
			else
				comboPriority.SelectedIndex=0;
				//or just set to no priority
      //listProcButtons.Items.Clear();
			//for(int i=0;i<ProcButtons.List.Length;i++){
      //  listProcButtons.Items.Add(ProcButtons.List[i].Description);  
			//}
			//ListView is replacing the old button list:---------------------------------------------------------
			int selectedButtonCat=listButtonCats.SelectedIndex;
			listButtonCats.Items.Clear();
			for(int i=0;i<DefB.Short[(int)DefCat.ProcButtonCats].Length;i++){
				listButtonCats.Items.Add(DefB.Short[(int)DefCat.ProcButtonCats][i].ItemName);
			}
			if(selectedButtonCat < listButtonCats.Items.Count){
				listButtonCats.SelectedIndex=selectedButtonCat;
			}
			if(listButtonCats.SelectedIndex==-1	&& listButtonCats.Items.Count>0){
				listButtonCats.SelectedIndex=0;
			}
			FillProcButtons();
			int selectedImageTab=tabControlImages.SelectedIndex;//retains current selection
			tabControlImages.TabPages.Clear();
			TabPage page;
			page=new TabPage();
			page.Text=Lan.g(this,"All");
			tabControlImages.TabPages.Add(page);
			visImageCats=new ArrayList();
			for(int i=0;i<DefB.Short[(int)DefCat.ImageCats].Length;i++){
				if(DefB.Short[(int)DefCat.ImageCats][i].ItemValue=="X" || DefB.Short[(int)DefCat.ImageCats][i].ItemValue=="XP"){
					//if tagged to show in Chart
					visImageCats.Add(i);
					page=new TabPage();
					page.Text=DefB.Short[(int)DefCat.ImageCats][i].ItemName;
					tabControlImages.TabPages.Add(page);
				}
			}
			if(selectedImageTab<tabControlImages.TabCount){
				tabControlImages.SelectedIndex=selectedImageTab;
			}
    }

		private void FillProcButtons(){
			listViewButtons.Items.Clear();
			imageListProcButtons.Images.Clear();
			if(listButtonCats.SelectedIndex==-1){
				ProcButtonList=new ProcButton[0];
				return;
			}
			ProcButtons.Refresh();
			ProcButtonList=ProcButtons.GetForCat(DefB.Short[(int)DefCat.ProcButtonCats][listButtonCats.SelectedIndex].DefNum);
			ListViewItem item;
			for(int i=0;i<ProcButtonList.Length;i++){
				if(ProcButtonList[i].ButtonImage!=null) {
					//image keys are simply the ProcButtonNum
					imageListProcButtons.Images.Add(ProcButtonList[i].ProcButtonNum.ToString(),ProcButtonList[i].ButtonImage);
				}
				item=new ListViewItem(new string[] {ProcButtonList[i].Description},ProcButtonList[i].ProcButtonNum.ToString());
				listViewButtons.Items.Add(item);
			}
    }

		///<summary>Gets run on ModuleSelected and each time a different images tab is selected. It first creates any missing thumbnails, then displays them. So it will be faster after the first time.</summary>
		private void FillImages(){
			visImages=new ArrayList();
			listViewImages.Items.Clear();
			imageListThumbnails.Images.Clear();
			if(PatCur==null){
				return;
			}
			if(patFolder==""){
				return;
			}
			if(!panelImages.Visible){
				return;
			}
			//create Thumbnails folder
			if(!Directory.Exists(patFolder+@"\Thumbnails")){
				try{
					Directory.CreateDirectory(patFolder+@"\Thumbnails");
				}
				catch{
					MessageBox.Show(Lan.g(this,"Error.  Could not create thumbnails folder. "));
					return;
				}
			}
			StringFormat notAvailFormat=new StringFormat();
			notAvailFormat.Alignment=StringAlignment.Center;
			notAvailFormat.LineAlignment=StringAlignment.Center;
			for(int i=0;i<DocumentList.Length;i++){
				if(!visImageCats.Contains(DefB.GetOrder(DefCat.ImageCats,DocumentList[i].DocCategory))){
					continue;//if category not visible, continue
				}
				if(tabControlImages.SelectedIndex>0){//any category except 'all'
					if(DocumentList[i].DocCategory!=DefB.Short[(int)DefCat.ImageCats]
						[(int)visImageCats[tabControlImages.SelectedIndex-1]].DefNum)
					{
						continue;//if not in category, continue
					}
				}
				//Documents.Cur=DocumentList[i];
				string thumbFileName=patFolder+@"Thumbnails\"+DocumentList[i].FileName;
				//Thumbs.db has nothing to do with Open Dental. It is a hidden Windows file.
				if(File.Exists(thumbFileName) && ContrDocs.HasImageExtension(DocumentList[i].FileName))
				{//use existing thumbnail
					imageListThumbnails.Images.Add(Bitmap.FromFile(thumbFileName));
				}else{//add thumbnail or create generic "not available"
					int thumbSize=imageListThumbnails.ImageSize.Width;//All thumbnails are square.
					Bitmap thumbBitmap;
					if(File.Exists(patFolder+DocumentList[i].FileName)
						&& ContrDocs.HasImageExtension(DocumentList[i].FileName)){//create and save thumbnail?
						//Gets the cropped/flipped/rotated image with any color filtering applied.
						Bitmap fullImage=ContrDocs.ApplyDocumentSettingsToImage(DocumentList[i],
							new Bitmap(patFolder+DocumentList[i].FileName),ContrDocs.ApplySettings.ALL);
						thumbBitmap=ContrDocs.GetThumbnail(fullImage,thumbSize);
						thumbBitmap.Save(thumbFileName);
					}//if File.Exists(original image)
					else{//original image not even present, or is not a supported image type, so show default thumbnail
						thumbBitmap=new Bitmap(thumbSize,thumbSize);
						Graphics g=Graphics.FromImage(thumbBitmap);
						g.InterpolationMode=InterpolationMode.High;
						g.FillRectangle(Brushes.Gray,0,0,thumbBitmap.Width,thumbBitmap.Height);
						g.DrawString("Thumbnail not available",Font,Brushes.Black,
							new RectangleF(0,0,thumbSize,thumbSize),notAvailFormat);
						g.Dispose();
					}
					imageListThumbnails.Images.Add(thumbBitmap);
				}//else add thumbnail
				visImages.Add(i);
				ListViewItem item=new ListViewItem(DocumentList[i].DateCreated.ToShortDateString()+": "
					+DocumentList[i].Description,imageListThumbnails.Images.Count-1);
				//item.ToolTipText=patFolder+DocumentList[i].FileName;//not supported by Mono
        listViewImages.Items.Add(item);
			}//for
		}

		#region EnterTx
		private void ClearButtons() {
			//unfortuantely, these colors no longer show since the XP button style was introduced.
			butM.BackColor=Color.FromName("Control"); ;
			butOI.BackColor=Color.FromName("Control");
			butD.BackColor=Color.FromName("Control");
			butL.BackColor=Color.FromName("Control");
			butBF.BackColor=Color.FromName("Control");
			butV.BackColor=Color.FromName("Control");
			textSurf.Text="";
			listDx.SelectedIndex=-1;
			//listProcButtons.SelectedIndex=-1;
			listViewButtons.SelectedIndices.Clear();
			textADACode.Text=Lan.g(this,"Type ADA Code");
		}

		private void UpdateSurf (){
			textSurf.Text="";
			if(toothChart.SelectedTeeth.Length==0){
				return;
			}
			if(butM.BackColor==Color.White){
				textSurf.AppendText("M");
			}
			if(butOI.BackColor==Color.White){
				if(ToothGraphic.IsAnterior(toothChart.SelectedTeeth[0])){
					textSurf.AppendText("I");
				}
				else{	
					textSurf.AppendText("O");
				}
			}
			if(butD.BackColor==Color.White){
				textSurf.AppendText("D");
			}
			if(butV.BackColor==Color.White){
				textSurf.AppendText("V");
			}
			if(butBF.BackColor==Color.White){
				if(ToothGraphic.IsAnterior(toothChart.SelectedTeeth[0])) {
					textSurf.AppendText("F");
				}
				else {
					textSurf.AppendText("B");
				}
			}
			if(butL.BackColor==Color.White){
				textSurf.AppendText("L");
			}
		}

		private void butBF_Click(object sender, System.EventArgs e){
			if(butBF.BackColor==Color.White){
				butBF.BackColor=SystemColors.Control;
			}
			else{
				butBF.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butV_Click(object sender, System.EventArgs e){
			if(butV.BackColor==Color.White){
				butV.BackColor=SystemColors.Control;
			}
			else{
				butV.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butM_Click(object sender, System.EventArgs e){
			if(butM.BackColor==Color.White){
				butM.BackColor=SystemColors.Control;
			}
			else{
				butM.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butOI_Click(object sender, System.EventArgs e){
			if(butOI.BackColor==Color.White){
				butOI.BackColor=SystemColors.Control;
			}
			else{
				butOI.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butD_Click(object sender, System.EventArgs e){
			if(butD.BackColor==Color.White){
				butD.BackColor=SystemColors.Control;
			}
			else{
				butD.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butL_Click(object sender, System.EventArgs e){
			if(butL.BackColor==Color.White){
				butL.BackColor=SystemColors.Control;
			}
			else{
				butL.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void gridProg_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DataRow row=(DataRow)gridProg.Rows[e.Row].Tag;
			if(row["ProcNum"].ToString()!="0"){
				if(checkAudit.Checked){
					MsgBox.Show(this,"Not allowed to edit procedures when in audit mode.");
					return;
				}
				Procedure proc=Procedures.GetOneProc(PIn.PInt(row["ProcNum"].ToString()),true);
				FormProcEdit FormP=new FormProcEdit(proc,PatCur,FamCur,PlanList);
				FormP.ShowDialog();
				if(FormP.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			else if(row["CommlogNum"].ToString()!="0"){
				Commlog comm=Commlogs.GetOne(PIn.PInt(row["CommlogNum"].ToString()));
				FormCommItem FormC=new FormCommItem(comm);
				FormC.ShowDialog();
				if(FormC.DialogResult!=DialogResult.OK){
					return;
				}
			}
			else if(row["RxNum"].ToString()!="0") {
				RxPat rx=RxPats.GetRx(PIn.PInt(row["RxNum"].ToString()));
				FormRxEdit FormRxE=new FormRxEdit(PatCur,rx);
				FormRxE.ShowDialog();
				if(FormRxE.DialogResult!=DialogResult.OK){
					return;
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		///<summary>Sets many fields for a new procedure, then displays it for editing before inserting it into the db.  No need to worry about ProcOld because it's an insert, not an update.</summary>
		private void AddProcedure(Procedure ProcCur){
			//procnum
			ProcCur.PatNum=PatCur.PatNum;
			//aptnum
			//adacode
			if(newStatus==ProcStat.EO){
				ProcCur.ProcDate=DateTime.MinValue;
			}
			else if(textDate.errorProvider1.GetError(textDate)!=""){
				ProcCur.ProcDate=DateTime.Today;
			}
			else{
				ProcCur.ProcDate=PIn.PDate(textDate.Text);
			}
			if(newStatus==ProcStat.R || newStatus==ProcStat.EO || newStatus==ProcStat.EC)
				ProcCur.ProcFee=0;
			else
				ProcCur.ProcFee=Fees.GetAmount0(ProcCur.ADACode,Fees.GetFeeSched(PatCur,PlanList,PatPlanList));
			//ProcCur.OverridePri=-1;
			//ProcCur.OverrideSec=-1;
			//surf
			//ToothNum
			//Procedures.Cur.ToothRange
			//ProcCur.NoBillIns=ProcedureCodes.GetProcCode(ProcCur.ADACode).NoBillIns;
			if(comboPriority.SelectedIndex==0)
				ProcCur.Priority=0;
			else
				ProcCur.Priority=DefB.Short[(int)DefCat.TxPriorities][comboPriority.SelectedIndex-1].DefNum;
			ProcCur.ProcStatus=newStatus;
			if(newStatus==ProcStat.C){
				ProcCur.Note=ProcedureCodes.GetProcCode(ProcCur.ADACode).DefaultNote;
			}
			else{
				ProcCur.Note="";
			}
			if(ProcedureCodes.GetProcCode(ProcCur.ADACode).IsHygiene
				&& PatCur.SecProv != 0){
				ProcCur.ProvNum=PatCur.SecProv;
			}
			else{
				ProcCur.ProvNum=PatCur.PriProv;
			}
			ProcCur.ClinicNum=PatCur.ClinicNum;
			if(listDx.SelectedIndex!=-1)
				ProcCur.Dx=DefB.Short[(int)DefCat.Diagnosis][listDx.SelectedIndex].DefNum;
			//nextaptnum
			ProcCur.DateEntryC=DateTime.Now;
			ProcCur.MedicalCode=ProcedureCodes.GetProcCode(ProcCur.ADACode).MedicalCode;
			Procedures.Insert(ProcCur);
			if((ProcCur.ProcStatus==ProcStat.C || ProcCur.ProcStatus==ProcStat.EC || ProcCur.ProcStatus==ProcStat.EO)
				&& ProcedureCodes.GetProcCode(ProcCur.ADACode).PaintType==ToothPaintingType.Extraction) {
				//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(PatCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,new ClaimProc[0],true,PlanList,PatPlanList,BenefitList);
			FormProcEdit FormPE=new FormProcEdit(ProcCur,PatCur.Copy(),FamCur,PlanList);
			FormPE.IsNew=true;
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.Cancel){
				//any created claimprocs are automatically deleted from within procEdit window.
				Procedures.Delete(ProcCur.ProcNum);//also deletes the claimprocs
			}
			else{
				Recalls.Synch(PatCur.PatNum);
			}
		}
			
		private void AddQuick(Procedure ProcCur){
			//procnum
			ProcCur.PatNum=PatCur.PatNum;
			//aptnum
			//adacode
			if(newStatus==ProcStat.EO){
				ProcCur.ProcDate=DateTime.MinValue;
			}
			else if(textDate.errorProvider1.GetError(textDate)!=""){
				ProcCur.ProcDate=DateTime.Today;
			}
			else{
				ProcCur.ProcDate=PIn.PDate(textDate.Text);
			}
			if(newStatus==ProcStat.R || newStatus==ProcStat.EO || newStatus==ProcStat.EC)
				ProcCur.ProcFee=0;
			else
				ProcCur.ProcFee=Fees.GetAmount0(ProcCur.ADACode,Fees.GetFeeSched(PatCur,PlanList,PatPlanList));
			//ProcCur.OverridePri=-1;
			//ProcCur.OverrideSec=-1;
			//surf
			//toothnum
			//ToothRange
			//ProcCur.NoBillIns=ProcedureCodes.GetProcCode(ProcCur.ADACode).NoBillIns;
			if(comboPriority.SelectedIndex==0)
				ProcCur.Priority=0;
			else
				ProcCur.Priority=DefB.Short[(int)DefCat.TxPriorities][comboPriority.SelectedIndex-1].DefNum;
			ProcCur.ProcStatus=newStatus;
			if(newStatus==ProcStat.C) {
				ProcCur.Note=ProcedureCodes.GetProcCode(ProcCur.ADACode).DefaultNote;
			}
			else {
				ProcCur.Note="";
			}
			if(ProcedureCodes.GetProcCode(ProcCur.ADACode).IsHygiene
				&& PatCur.SecProv != 0){
				ProcCur.ProvNum=PatCur.SecProv;
			}
			else{
				ProcCur.ProvNum=PatCur.PriProv;
			}
			ProcCur.ClinicNum=PatCur.ClinicNum;
			if(listDx.SelectedIndex!=-1)
				ProcCur.Dx=DefB.Short[(int)DefCat.Diagnosis][listDx.SelectedIndex].DefNum;
			ProcCur.MedicalCode=ProcedureCodes.GetProcCode(ProcCur.ADACode).MedicalCode;
			//nextaptnum
			//ProcCur.CapCoPay=-1;
			//if(Patients.Cur.PriPlanNum!=0){//if patient has insurance
			//ProcCur.IsCovIns=true;
				//InsPlans.GetCur(Patients.Cur.PriPlanNum);
				//if(InsPlans.Cur.PlanType=="c"){
					//also handles fine if copayfeesched=0:
				//ProcCur.CapCoPay=Fees.GetAmount(ProcCur.ADACode,InsPlans.Cur.CopayFeeSched);
				//}
			//}
			//MessageBox.Show(Procedures.NewProcedure.ProcFee.ToString());
			//MessageBox.Show(Procedures.NewProcedure.ProcDate);
			//if(Procedures.Cur.ProcStatus==ProcStat.C){
			//	Procedures.PutBal(Procedures.Cur.ProcDate,Procedures.Cur.ProcFee);
			//}
			Procedures.Insert(ProcCur);
			if((ProcCur.ProcStatus==ProcStat.C || ProcCur.ProcStatus==ProcStat.EC || ProcCur.ProcStatus==ProcStat.EO)
				&& ProcedureCodes.GetProcCode(ProcCur.ADACode).PaintType==ToothPaintingType.Extraction) {
				//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(PatCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			Recalls.Synch(PatCur.PatNum);
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,new ClaimProc[0],true,PlanList,PatPlanList,BenefitList);
		}

		private void butAddProc_Click(object sender, System.EventArgs e){
			if(newStatus==ProcStat.C){
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)){
					return;
				}
			}
			bool isValid;
			TreatmentArea tArea;
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) return;
			Procedures.SetDateFirstVisit(DateTime.Today,1,PatCur);
			Procedure ProcCur;
			for(int n=0;n==0 || n<toothChart.SelectedTeeth.Length;n++){
				isValid=true;
				ProcCur=new Procedure();//going to be an insert, so no need to set Procedures.CurOld
				//Procedure
				ProcCur.ADACode = FormP.SelectedADA;
				//Procedures.Cur.ADACode=ProcButtonItems.adaCodeList[i];
				tArea=ProcedureCodes.GetProcCode(ProcCur.ADACode).TreatArea;
				if((tArea==TreatmentArea.Arch
					|| tArea==TreatmentArea.Mouth
					|| tArea==TreatmentArea.Quad
					|| tArea==TreatmentArea.Sextant
					|| tArea==TreatmentArea.ToothRange)
					&& n>0){//the only two left are tooth and surf
					continue;//only entered if n=0, so they don't get entered more than once.
				}
				else if(tArea==TreatmentArea.Quad){
				//	switch(quadCount){
				//		case 0: Procedures.Cur.Surf="UR"; break;
				//		case 1: Procedures.Cur.Surf="UL"; break;
				//		case 2: Procedures.Cur.Surf="LL"; break;
				//		case 3: Procedures.Cur.Surf="LR"; break;
				//		default: Procedures.Cur.Surf="UR"; break;//this could happen.
				//	}
				//	quadCount++;
				//	AddQuick();
					//Procedures.Cur=ProcCur;
					AddProcedure(ProcCur);
				}
				else if(tArea==TreatmentArea.Surf){
					if(textSurf.Text=="")
						isValid=false;
					else
						ProcCur.Surf=textSurf.Text;
					if(toothChart.SelectedTeeth.Length==0)
						isValid=false;
					else
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
					//Procedures.Cur=ProcCur;
					if(isValid)
						AddQuick(ProcCur);
					else
						AddProcedure(ProcCur);
				}
				else if(tArea==TreatmentArea.Tooth){
					if(toothChart.SelectedTeeth.Length==0){
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);
					}
					else{
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						//Procedures.Cur=ProcCur;
						AddQuick(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.ToothRange){
					if(toothChart.SelectedTeeth.Length==0){
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);
					}
					else{
						ProcCur.ToothRange="";
						for(int b=0;b<toothChart.SelectedTeeth.Length;b++){
							if(b!=0) ProcCur.ToothRange+=",";
							ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
						}
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);//it's nice to see the procedure to verify the range
					}
				}
				else if(tArea==TreatmentArea.Arch){
					if(toothChart.SelectedTeeth.Length==0){
						//Procedures.Cur=ProcCur;
						AddProcedure(ProcCur);
						continue;
					}
					if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])){
						ProcCur.Surf="U";
					}
					else{
						ProcCur.Surf="L";
					}
					//Procedures.Cur=ProcCur;
					AddQuick(ProcCur);
				}
				else if(tArea==TreatmentArea.Sextant){
					//Procedures.Cur=ProcCur;
					AddProcedure(ProcCur);
				}
				else{//mouth
					//Procedures.Cur=ProcCur;
					AddQuick(ProcCur);
				}
			}//for n
			ModuleSelected(PatCur.PatNum);
			if(newStatus==ProcStat.C){
				SecurityLogs.MakeLogEntry(Permissions.ProcComplCreate,PatCur.PatNum,
					PatCur.GetNameLF()+", "
					+DateTime.Today.ToShortDateString());
			}
		}
		
		private void listDx_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			//newDx=Defs.Defns[(int)DefCat.Diagnosis][listDx.IndexFromPoint(e.X,e.Y)].DefNum;
		}

		/*private void tbProg_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Delete || e.KeyCode==Keys.Back){
				DeleteRows();
			}
		}*/

		private void gridProg_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Delete || e.KeyCode==Keys.Back) {
				DeleteRows();
			}
		}

		//private void butDel_Click(object sender, System.EventArgs e) {
		//	DeleteRows();
		//}

		private void DeleteRows(){
			if(gridProg.SelectedIndices.Length==0){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete Selected Item(s)?"),"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK){
				return;
			}
			int skippedC=0;
			int skippedComlog=0;
			DataRow row;
			for(int i=0;i<gridProg.SelectedIndices.Length;i++){
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(row["ProcNum"].ToString()!="0"){
					if(PIn.PInt(row["ProcStatus"].ToString())==(int)ProcStat.C){
						skippedC++;
					}
					else{
						Procedures.Delete(PIn.PInt(row["ProcNum"].ToString()));//also deletes the claimprocs
					}
				}
				else if(row["RxNum"].ToString()!="0"){
					RxPats.Delete(PIn.PInt(row["RxNum"].ToString()));
				}
				else if(row["CommlogNum"].ToString()!="0"){
					skippedComlog++;
				}
			}
			Recalls.Synch(PatCur.PatNum);
			if(skippedC>0){
				MessageBox.Show(Lan.g(this,"Not allowed to delete completed procedures from here.")+"\r"
					+skippedC.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			if(skippedComlog>0) {
				MessageBox.Show(Lan.g(this,"Not allowed to delete commlog entries from here.")+"\r"
					+skippedComlog.ToString()+" "+Lan.g(this,"item(s) skipped."));
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void radioEntryEO_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.EO;
		}

		private void radioEntryEC_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.EC;
		}

		private void radioEntryTP_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.TP;
		}

		private void radioEntryC_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.C;
		}

		private void radioEntryR_CheckedChanged(object sender,System.EventArgs e) {
			newStatus=ProcStat.R;
		}

		private void listButtonCats_Click(object sender,EventArgs e) {
			FillProcButtons();
		}

		/*private void listProcButtons_Click(object sender, System.EventArgs e) {
			if(newStatus==ProcStat.C){
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)){
					return;
				}
			}
			if(listProcButtons.SelectedIndex==-1){
				return;
			}
		  ProcButton ProcButtonCur=ProcButtons.List[listProcButtons.SelectedIndex];
			ProcButtonClicked(ProcButtonCur.ProcButtonNum);
		}*/

		private void listViewButtons_Click(object sender,EventArgs e) {
			if(newStatus==ProcStat.C) {
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
					return;
				}
			}
			if(listViewButtons.SelectedIndices.Count==0) {
				return;
			}
			ProcButton ProcButtonCur=ProcButtonList[listViewButtons.SelectedIndices[0]];
			ProcButtonClicked(ProcButtonCur.ProcButtonNum);
		}

		private void ProcButtonClicked(int procButtonNum){
			Procedures.SetDateFirstVisit(DateTime.Today,1,PatCur);
			bool isValid;
			TreatmentArea tArea;
			int quadCount=0;//automates quadrant codes.
			string[] adaCodeList=ProcButtonItems.GetADAListForButton(procButtonNum);
			int[] autoCodeList=ProcButtonItems.GetAutoListForButton(procButtonNum);
			
			Procedure ProcCur;
			for(int i=0;i<adaCodeList.Length;i++){
				//needs to loop at least once, regardless of whether any teeth are selected.	
				for(int n=0;n==0 || n<toothChart.SelectedTeeth.Length;n++){
					isValid=true;
					ProcCur=new Procedure();//insert, so no need to set CurOld
					ProcCur.ADACode=adaCodeList[i];
					tArea=ProcedureCodes.GetProcCode(ProcCur.ADACode).TreatArea;
					if((tArea==TreatmentArea.Arch
						|| tArea==TreatmentArea.Mouth
						|| tArea==TreatmentArea.Quad
						|| tArea==TreatmentArea.Sextant
						|| tArea==TreatmentArea.ToothRange)
						&& n>0){//the only two left are tooth and surf
						continue;//only entered if n=0, so they don't get entered more than once.
					}
					else if(tArea==TreatmentArea.Quad){
						switch(quadCount){
							case 0: ProcCur.Surf="UR"; break;
							case 1: ProcCur.Surf="UL"; break;
							case 2: ProcCur.Surf="LL"; break;
							case 3: ProcCur.Surf="LR"; break;
							default: ProcCur.Surf="UR"; break;//this could happen.
						}
						quadCount++;
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Surf){
						if(textSurf.Text=="")
							isValid=false;
						else
							ProcCur.Surf=textSurf.Text;
						if(toothChart.SelectedTeeth.Length==0)
							isValid=false;
						else
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						if(isValid)
							AddQuick(ProcCur);
						else
							AddProcedure(ProcCur);
					}
					else if(tArea==TreatmentArea.Tooth){
						if(toothChart.SelectedTeeth.Length==0){
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.ToothRange){
						if(toothChart.SelectedTeeth.Length==0){
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothRange="";
							for(int b=0;b<toothChart.SelectedTeeth.Length;b++){
								if(b!=0) ProcCur.ToothRange+=",";
								ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
							}
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.Arch){
						if(toothChart.SelectedTeeth.Length==0){
							AddProcedure(ProcCur);
							continue;
						}
						if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])){
							ProcCur.Surf="U";
						}
						else{
							ProcCur.Surf="L";
						}
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Sextant){
						AddProcedure(ProcCur);
					}
					else{//mouth
						AddQuick(ProcCur);
					}
				}//n selected teeth
			}//end Part 1 checking for AdaCodes, now will check for AutoCodes
			string toothNum;
			string surf;
			bool isAdditional;
			for(int i=0;i<autoCodeList.Length;i++){
				for(int n=0;n==0 || n<toothChart.SelectedTeeth.Length;n++){
					isValid=true;
					if(toothChart.SelectedTeeth.Length!=0)
						toothNum=toothChart.SelectedTeeth[n];
					else
						toothNum="";
					surf=textSurf.Text;
					isAdditional= n!=0;
					ProcCur=new Procedure();//this will be an insert, so no need to set CurOld
					ProcCur.ADACode=AutoCodeItems.GetADA(autoCodeList[i],toothNum,surf,isAdditional,PatCur.PatNum,PatCur.Age);
					tArea=ProcedureCodes.GetProcCode(ProcCur.ADACode).TreatArea;
					if((tArea==TreatmentArea.Arch
						|| tArea==TreatmentArea.Mouth
						|| tArea==TreatmentArea.Quad
						|| tArea==TreatmentArea.Sextant
						|| tArea==TreatmentArea.ToothRange)
						&& n>0){//the only two left are tooth and surf
						continue;//only entered if n=0, so they don't get entered more than once.
					}
					else if(tArea==TreatmentArea.Quad){
						switch(quadCount){
							case 0: ProcCur.Surf="UR"; break;
							case 1: ProcCur.Surf="UL"; break;
							case 2: ProcCur.Surf="LL"; break;
							case 3: ProcCur.Surf="LR"; break;
							default: ProcCur.Surf="UR"; break;//this could happen.
						}
						quadCount++;
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Surf){
						if(textSurf.Text=="")
							isValid=false;
						else
							ProcCur.Surf=textSurf.Text;
						if(toothChart.SelectedTeeth.Length==0)
							isValid=false;
						else
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						if(isValid)
							AddQuick(ProcCur);
						else
							AddProcedure(ProcCur);
					}
					else if(tArea==TreatmentArea.Tooth){
						if(toothChart.SelectedTeeth.Length==0){
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothNum=toothChart.SelectedTeeth[n];
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.ToothRange){
						if(toothChart.SelectedTeeth.Length==0){
							AddProcedure(ProcCur);
						}
						else{
							ProcCur.ToothRange="";
							for(int b=0;b<toothChart.SelectedTeeth.Length;b++){
								if(b!=0) ProcCur.ToothRange+=",";
								ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
							}
							AddQuick(ProcCur);
						}
					}
					else if(tArea==TreatmentArea.Arch){
						if(toothChart.SelectedTeeth.Length==0){
							AddProcedure(ProcCur);
							continue;
						}
						if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])){
							ProcCur.Surf="U";
						}
						else{
							ProcCur.Surf="L";
						}
						AddQuick(ProcCur);
					}
					else if(tArea==TreatmentArea.Sextant){
						AddProcedure(ProcCur);
					}
					else{//mouth
						AddQuick(ProcCur);
					}
				}//n selected teeth
			}//for i
			ModuleSelected(PatCur.PatNum);
			if(newStatus==ProcStat.C){
				SecurityLogs.MakeLogEntry(Permissions.ProcComplCreate,PatCur.PatNum,
					PatCur.GetNameLF()+", "
					+DateTime.Today.ToShortDateString());
			}
		}
		
		private void textADACode_Enter(object sender,System.EventArgs e) {
			if(textADACode.Text==Lan.g(this,"Type ADA Code")) {
				textADACode.Text="";
			}
		}

		private void textADACode_KeyDown(object sender,System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Return) {
				EnterTypedCode();
			}
		}

		private void textADACode_TextChanged(object sender,System.EventArgs e) {
			if(textADACode.Text=="d") {
				textADACode.Text="D";
				textADACode.SelectionStart=1;
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			EnterTypedCode();
		}

		private void EnterTypedCode() {
			if(newStatus==ProcStat.C) {
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
					return;
				}
			}
			if(CultureInfo.CurrentCulture.Name=="en-US"
				&& Regex.IsMatch(textADACode.Text,@"^\d{4}$")//if exactly 4 digits
				&& !ProcedureCodes.HList.ContainsKey(textADACode.Text))//and the 4 digit code is not found
			{
				textADACode.Text="D"+textADACode.Text;
			}
			if(!ProcedureCodes.HList.ContainsKey(textADACode.Text)) {
				MessageBox.Show(Lan.g(this,"Invalid code."));
				//textADACode.Text="";
				textADACode.SelectionStart=textADACode.Text.Length;
				return;
			}
			Procedures.SetDateFirstVisit(DateTime.Today,1,PatCur);
			TreatmentArea tArea;
			Procedure ProcCur;
			int quadCount=0;//automates quadrant codes.
			for(int n=0;n==0 || n<toothChart.SelectedTeeth.Length;n++) {//always loops at least once.
				ProcCur=new Procedure();//this will be an insert, so no need to set CurOld
				ProcCur.ADACode=textADACode.Text;
				bool isValid=true;
				tArea=ProcedureCodes.GetProcCode(ProcCur.ADACode).TreatArea;
				if((tArea==TreatmentArea.Arch
					|| tArea==TreatmentArea.Mouth
					|| tArea==TreatmentArea.Quad
					|| tArea==TreatmentArea.Sextant
					|| tArea==TreatmentArea.ToothRange)
					&& n>0) {//the only two left are tooth and surf
					continue;//only entered if n=0, so they don't get entered more than once.
				}
				else if(tArea==TreatmentArea.Quad) {
					switch(quadCount) {
						case 0: ProcCur.Surf="UR"; break;
						case 1: ProcCur.Surf="UL"; break;
						case 2: ProcCur.Surf="LL"; break;
						case 3: ProcCur.Surf="LR"; break;
						default: ProcCur.Surf="UR"; break;//this could happen.
					}
					quadCount++;
					AddQuick(ProcCur);
				}
				else if(tArea==TreatmentArea.Surf) {
					if(textSurf.Text=="")
						isValid=false;
					else
						ProcCur.Surf=textSurf.Text;
					if(toothChart.SelectedTeeth.Length==0)
						isValid=false;
					else
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
					if(isValid)
						AddQuick(ProcCur);
					else
						AddProcedure(ProcCur);
				}
				else if(tArea==TreatmentArea.Tooth) {
					if(toothChart.SelectedTeeth.Length==0) {
						AddProcedure(ProcCur);
					}
					else {
						ProcCur.ToothNum=toothChart.SelectedTeeth[n];
						AddQuick(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.ToothRange) {
					if(toothChart.SelectedTeeth.Length==0) {
						AddProcedure(ProcCur);
					}
					else {
						ProcCur.ToothRange="";
						for(int b=0;b<toothChart.SelectedTeeth.Length;b++) {
							if(b!=0) ProcCur.ToothRange+=",";
							ProcCur.ToothRange+=toothChart.SelectedTeeth[b];
						}
						AddQuick(ProcCur);
					}
				}
				else if(tArea==TreatmentArea.Arch) {
					if(toothChart.SelectedTeeth.Length==0) {
						AddProcedure(ProcCur);
						continue;
					}
					if(Tooth.IsMaxillary(toothChart.SelectedTeeth[0])) {
						ProcCur.Surf="U";
					}
					else {
						ProcCur.Surf="L";
					}
					AddQuick(ProcCur);
				}
				else if(tArea==TreatmentArea.Sextant) {
					AddProcedure(ProcCur);
				}
				else {//mouth
					AddQuick(ProcCur);
				}
			}//n selected teeth
			ModuleSelected(PatCur.PatNum);
			textADACode.Text="";
			textADACode.Select();
			if(newStatus==ProcStat.C) {
				SecurityLogs.MakeLogEntry(Permissions.ProcComplCreate,PatCur.PatNum,
					PatCur.GetNameLF()+", "
					+DateTime.Today.ToShortDateString());
			}
		}
		#endregion EnterTx

		#region MissingTeeth
		private void butMissing_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0){
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++){
				ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Missing);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butNotMissing_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.ClearValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Missing);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butEdentulous_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Missing);
			for(int i=1;i<=32;i++){
				ToothInitials.SetValueQuick(PatCur.PatNum,i.ToString(),ToothInitialType.Missing,0);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butHidden_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Hidden);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butUnhide_Click(object sender,EventArgs e) {
			if(listHidden.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select an item from the list first.");
				return;
			}
			ToothInitials.ClearValue(PatCur.PatNum,(string)HiddenTeeth[listHidden.SelectedIndex],ToothInitialType.Hidden);
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}
		#endregion MissingTeeth

		#region Movements
		private void FillMovementsAndHidden(){
			if(tabProc.Height<50){//skip if the tab control is short(not visible){
				return;
			}
			if(tabProc.SelectedIndex==2)//movements tab
			{
				if(toothChart.SelectedTeeth.Length==0) {
					textShiftM.Text="";
					textShiftO.Text="";
					textShiftB.Text="";
					textRotate.Text="";
					textTipM.Text="";
					textTipB.Text="";
					return;
				}
				textShiftM.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.ShiftM).ToString();  
				textShiftO.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.ShiftO).ToString();
				textShiftB.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.ShiftB).ToString();
				textRotate.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.Rotate).ToString();
				textTipM.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.TipM).ToString();
				textTipB.Text=
					ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[0],ToothInitialType.TipB).ToString();
				//At this point, all 6 blanks have either a number or 0.
				//As we go through this loop, none of the values will change.
				//The only thing that will happen is that some of them will become blank.
				string move;
				for(int i=1;i<toothChart.SelectedTeeth.Length;i++){
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.ShiftM).ToString();
					if(textShiftM.Text != move){
						textShiftM.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.ShiftO).ToString();
					if(textShiftO.Text != move) {
						textShiftO.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.ShiftB).ToString();
					if(textShiftB.Text != move) {
						textShiftB.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.Rotate).ToString();
					if(textRotate.Text != move) {
						textRotate.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.TipM).ToString();
					if(textTipM.Text != move) {
						textTipM.Text="";
					}
					move=ToothInitials.GetMovement(ToothInitialList,toothChart.SelectedTeeth[i],ToothInitialType.TipB).ToString();
					if(textTipB.Text != move) {
						textTipB.Text="";
					}
				}
			}//if movements tab
			if(tabProc.SelectedIndex==1){//missing teeth
				listHidden.Items.Clear();
				HiddenTeeth=ToothInitials.GetHiddenTeeth(ToothInitialList);
				for(int i=0;i<HiddenTeeth.Count;i++){
					listHidden.Items.Add(Tooth.ToInternat((string)HiddenTeeth[i]));
				}
			}
		}

		private void butShiftMminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftM,-2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftMplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftM,2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftOminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftO,-2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftOplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftO,2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftBminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftB,-2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butShiftBplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.ShiftB,2);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butRotateMinus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Rotate,-20);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butRotatePlus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Rotate,20);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipMminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipM,-10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipMplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipM,10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipBminus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipB,-10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butTipBplus_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.AddMovement(ToothInitialList,PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.TipB,10);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}

		private void butApplyMovements_Click(object sender,EventArgs e) {
			if(textShiftM.errorProvider1.GetError(textShiftM)!=""
				|| textShiftO.errorProvider1.GetError(textShiftO)!=""
				|| textShiftB.errorProvider1.GetError(textShiftB)!=""
				|| textRotate.errorProvider1.GetError(textRotate)!=""
				|| textTipM.errorProvider1.GetError(textTipM)!=""
				|| textTipB.errorProvider1.GetError(textTipB)!="")
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++){
				if(textShiftM.Text!=""){
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.ShiftM,PIn.PFloat(textShiftM.Text));
				}
				if(textShiftO.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.ShiftO,PIn.PFloat(textShiftO.Text));
				}
				if(textShiftB.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.ShiftB,PIn.PFloat(textShiftB.Text));
				}
				if(textRotate.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.Rotate,PIn.PFloat(textRotate.Text));
				}
				if(textTipM.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.TipM,PIn.PFloat(textTipM.Text));
				}
				if(textTipB.Text!="") {
					ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],
						ToothInitialType.TipB,PIn.PFloat(textTipB.Text));
				}
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(true);
		}
		#endregion Movements

		#region Primary
		private void butPrimary_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				ToothInitials.SetValue(PatCur.PatNum,toothChart.SelectedTeeth[i],ToothInitialType.Primary);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butPerm_Click(object sender,EventArgs e) {
			if(toothChart.SelectedTeeth.Length==0) {
				MsgBox.Show(this,"Please select teeth first.");
				return;
			}
			for(int i=0;i<toothChart.SelectedTeeth.Length;i++) {
				if(ToothGraphic.IsPrimary(toothChart.SelectedTeeth[i])){
					ToothInitials.ClearValue(PatCur.PatNum,ToothGraphic.PriToPerm(toothChart.SelectedTeeth[i])
						,ToothInitialType.Primary);
				}
				else{
					ToothInitials.ClearValue(PatCur.PatNum,toothChart.SelectedTeeth[i]
						,ToothInitialType.Primary);
				}
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butAllPrimary_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Primary);
			for(int i=1;i<=32;i++){
				ToothInitials.SetValueQuick(PatCur.PatNum,i.ToString(),ToothInitialType.Primary,0);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butAllPerm_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Primary);
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}

		private void butMixed_Click(object sender,EventArgs e) {
			ToothInitials.ClearAllValuesForType(PatCur.PatNum,ToothInitialType.Primary);
			string[] priTeeth=new string[] 
				{"1","2","4","5","6","11","12","13","15","16","17","18","20","21","22","27","28","29","31","32"};
			for(int i=0;i<priTeeth.Length;i++) {
				ToothInitials.SetValueQuick(PatCur.PatNum,priTeeth[i],ToothInitialType.Primary,0);
			}
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			FillToothChart(false);
		}
		#endregion Primary

		private void button1_Click(object sender, System.EventArgs e) {
			//sometimes used for testing purposes
		}

		private void checkShowTP_Click(object sender, System.EventArgs e) {
			FillProgNotes();
		}

		private void checkShowC_Click(object sender, System.EventArgs e) {
			FillProgNotes();
		}

		private void checkShowE_Click(object sender, System.EventArgs e) {
			FillProgNotes();
		}

		private void checkShowR_Click(object sender, System.EventArgs e) {
			FillProgNotes();
		}

		private void checkNotes_Click(object sender, System.EventArgs e) {
			FillProgNotes();
		}

		private void checkRx_Click(object sender, System.EventArgs e) {
			FillProgNotes();
		}

		private void checkComm_Click(object sender,EventArgs e) {
			FillProgNotes();
		}

		private void checkShowTeeth_Click(object sender, System.EventArgs e) {
			FillProgNotes();
		}

		private void checkAudit_Click(object sender,EventArgs e) {
			FillProgNotes();
		}

		private void butShowAll_Click(object sender, System.EventArgs e) {
			checkShowTP.Checked=true;
			checkShowC.Checked=true;
			checkShowE.Checked=true;
			checkShowR.Checked=true;
			checkNotes.Checked=true;
			checkRx.Checked=true;
			checkComm.Checked=true;
			checkShowTeeth.Checked=false;
			FillProgNotes();
		}

		private void butShowNone_Click(object sender, System.EventArgs e) {
			checkShowTP.Checked=false;
			checkShowC.Checked=false;
			checkShowE.Checked=false;
			checkShowR.Checked=false;
			checkNotes.Checked=false;
			checkRx.Checked=false;
			checkComm.Checked=false;
			checkShowTeeth.Checked=false;
			FillProgNotes();
		}

		private void gridPtInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row>=6){// && e.Row<=8){
				FormMedical FormM=new FormMedical(PatientNoteCur,PatCur);
				FormM.ShowDialog();
				ModuleSelected(PatCur.PatNum);
			}
		}

		private void checkDone_Click(object sender, System.EventArgs e) {
			Patient oldPat=PatCur.Copy();
			if(checkDone.Checked){
				if(ApptPlanned.Visible){
					if(!MsgBox.Show(this,true,"Existing planned appointment will be deleted")){
						checkDone.Checked=false;
						return; 
					}
					Procedures.UnattachProcsInPlannedAppt(ApptPlanned.Info.MyApt.AptNum);
					Appointments.Delete(ApptPlanned.Info.MyApt);
				}
				PatCur.NextAptNum=0;//-1;
				PatCur.PlannedIsDone=true;
				Patients.Update(PatCur,oldPat);
			}
			else{
				PatCur.NextAptNum=0;
				PatCur.PlannedIsDone=false;
				Patients.Update(PatCur,oldPat);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void butNew_Click(object sender, System.EventArgs e) {
			if(ApptPlanned.Visible){
				if(MessageBox.Show(Lan.g(this,"Replace existing planned appointment?")
					,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
					return;
				Procedures.UnattachProcsInPlannedAppt(ApptPlanned.Info.MyApt.AptNum);
				Appointments.Delete(ApptPlanned.Info.MyApt);
			}
			Appointment AptCur=new Appointment();
			AptCur.PatNum=PatCur.PatNum;
			AptCur.ProvNum=PatCur.PriProv;
			AptCur.ClinicNum=PatCur.ClinicNum;
			AptCur.AptStatus=ApptStatus.Planned;
			AptCur.AptDateTime=DateTime.Today;
			AptCur.Pattern="/X/";
			Appointments.InsertOrUpdate(AptCur,null,true);
			FormApptEdit FormApptEdit2=new FormApptEdit(AptCur);
			FormApptEdit2.IsNew=true;
			FormApptEdit2.ShowDialog();
			if(FormApptEdit2.DialogResult!=DialogResult.OK){
				//delete new appt and unattach procs already handled in dialog
				//not needed: Patients.Cur.NextAptNum=0;
				FillPlanned();
				return;
			}
			Procedure[] myProcList=Procedures.Refresh(PatCur.PatNum);
			bool allProcsHyg=true;
			for(int i=0;i<myProcList.Length;i++){
				if(myProcList[i].PlannedAptNum!=AptCur.AptNum)
					continue;//only concerned with procs on this nextApt
				if(!ProcedureCodes.GetProcCode(myProcList[i].ADACode).IsHygiene){
					allProcsHyg=false;
					break;
				}
			}
			if(allProcsHyg && PatCur.SecProv!=0){
				Appointment aptOld=AptCur.Copy();
				AptCur.ProvNum=PatCur.SecProv;
				Appointments.InsertOrUpdate(AptCur,aptOld,false);
			}
			Patient patOld=PatCur.Copy();
			PatCur.NextAptNum=AptCur.AptNum;
			PatCur.PlannedIsDone=false;
			Patients.Update(PatCur,patOld);
			ModuleSelected(PatCur.PatNum);//if procs were added in appt, then this will display them
		}

		///<summary>Not enabled if there is no planned appointment.</summary>
		private void butClear_Click(object sender, System.EventArgs e) {
			//if(!ApptPlanned.Visible){
			//	MessageBox.Show(Lan.g(this,"No planned appointment is present."));
			//	return;
			//}
			if(MessageBox.Show(Lan.g(this,"Delete planned appointment?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
				return;
			Procedures.UnattachProcsInPlannedAppt(ApptPlanned.Info.MyApt.AptNum);
			Appointments.Delete(ApptPlanned.Info.MyApt);
			Patient patOld=PatCur.Copy();
			PatCur.NextAptNum=0;
			Patients.Update(PatCur,patOld);
			ModuleSelected(PatCur.PatNum);
			//FillNext();
		}

		///<summary>Not enabled if there is no planned appointment.</summary>
		private void butPin_Click(object sender,EventArgs e) {
			//OnPatientSelected(FormCS.GotoPatNum);
			GotoModule.PinToAppt(ApptPlanned.Info.MyApt);
		}

		private void ApptPlanned_DoubleClick(object sender, System.EventArgs e){
			FormApptEdit FormAE=new FormApptEdit(ApptPlanned.Info.MyApt);
			FormAE.ShowDialog();
			ModuleSelected(PatCur.PatNum);//if procs were added in appt, then this will display them
		}

		private void toothChart_Click(object sender,EventArgs e) {
			textSurf.Text="";
			//if(toothChart.SelectedTeeth.Length==1) {
				//butO.BackColor=SystemColors.Control;
				//butB.BackColor=SystemColors.Control;
				//butF.BackColor=SystemColors.Control;
				//if(Tooth.IsAnterior(toothChart.SelectedTeeth[0])) {
					//butB.Text="";
					//butO.Text="";
					//butB.Enabled=false;
					//butO.Enabled=false;
					//butF.Text="F";
					//butI.Text="I";
					//butF.Enabled=true;
					//butI.Enabled=true;
				//}
				//else {
					//butB.Text="B";
					//butO.Text="O";
					//butB.Enabled=true;
					//butO.Enabled=true;
					//butF.Text="";
					//butI.Text="";
					//butF.Enabled=false;
					//butI.Enabled=false;
				//}
			//}
			if(checkShowTeeth.Checked) {
				FillProgNotes();
			}
			FillMovementsAndHidden();
		}

		//private void tbProg_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			
		//}

		private void gridProg_MouseUp(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right) {
				if(PrefB.GetBool("EasyHideHospitals")){
					menuItemPrintDay.Visible=false;
				}
				else{
					menuItemPrintDay.Visible=true;
				}
				menuProgRight.Show(gridProg,new Point(e.X,e.Y));
			}
		}

		private void menuItemPrintProg_Click(object sender, System.EventArgs e) {
			pagesPrinted=0;
			headingPrinted=false;
			#if DEBUG
				PrintReport(true);
			#else
				PrintReport(false);	
			#endif
		}

		private void menuItemPrintDay_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select at least one item first.");
				return;
			}
			Type type=gridProg.Rows[gridProg.SelectedIndices[0]].Tag.GetType();
			if(type==typeof(Procedure)){
				hospitalDate=((Procedure)gridProg.Rows[gridProg.SelectedIndices[0]].Tag).ProcDate.Date;
			}
			else if(type==typeof(RxPat)) {
				hospitalDate=((RxPat)gridProg.Rows[gridProg.SelectedIndices[0]].Tag).RxDate.Date;
			}
			else if(type==typeof(Commlog)) {
				hospitalDate=((Commlog)gridProg.Rows[gridProg.SelectedIndices[0]].Tag).CommDateTime.Date;
			}
			bool showRx=this.checkRx.Checked;
			bool showComm=this.checkComm.Checked;
			checkRx.Checked=false;
			checkComm.Checked=false;
			FillProgNotes();
			try{
				pagesPrinted=0;
				headingPrinted=false;
				#if DEBUG
					PrintDay(true);
				#else
					PrintDay(false);	
				#endif
			}
			catch{

			}
			hospitalDate=DateTime.MinValue;
			checkRx.Checked=showRx;
			checkComm.Checked=showComm;
			FillProgNotes();
		}

		private void menuItemDelete_Click(object sender,EventArgs e) {
			DeleteRows();
		}

		private void menuItemSetComplete_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
				return;
			}
			if(gridProg.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			if(checkAudit.Checked) {
				MsgBox.Show(this,"Not allowed in audit mode.");
				return;
			}
			Procedure procCur;
			Procedure procOld;
			ProcedureCode procCode;
			Appointment apt;
			ClaimProc[] ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			DataRow row;
			for(int i=0;i<gridProg.SelectedIndices.Length;i++) {
				row=(DataRow)gridProg.Rows[gridProg.SelectedIndices[i]].Tag;
				if(row["ProcNum"].ToString()=="0") {
					continue;//not a procedure
				}
				apt=null;
				procCur=Procedures.GetOneProc(PIn.PInt(row["ProcNum"].ToString()),true);
				//The next few lines were removed at a user's request.  I couldn't remember why we added them in the first place.
				//He wanted them removed so that he could update the date of multiple procedures at once.  But this only makes sense
				//to him because he does not attach procs to appointments.
				//if(procCur.ProcStatus==ProcStat.C) {//don't allow setting a procedure complete again.
				//	continue;
				//}
				procOld=procCur.Copy();
				procCode=ProcedureCodes.GetProcCode(procCur.ADACode);
				if(procOld.ProcStatus!=ProcStat.C) {
					//if procedure was already complete, then don't add more notes.
					procCur.Note+=procCode.DefaultNote;//note wasn't complete, so add notes
				}
				procCur.DateEntryC=DateTime.Now;
				if(procCur.AptNum!=0) {//if attached to an appointment
					apt=Appointments.GetOneApt(procCur.AptNum);
					procCur.ProcDate=apt.AptDateTime;
					procCur.ClinicNum=apt.ClinicNum;
					procCur.PlaceService=Clinics.GetPlaceService(apt.ClinicNum);
				}
				else {
					procCur.ProcDate=PIn.PDate(textDate.Text);
					procCur.PlaceService=(PlaceOfService)PrefB.GetInt("DefaultProcedurePlaceService");
				}
				Procedures.SetDateFirstVisit(procCur.ProcDate,2,PatCur);
				if(procCode.PaintType==ToothPaintingType.Extraction){//if an extraction, then mark previous procs hidden
					//Procedures.SetHideGraphical(procCur);//might not matter anymore
					ToothInitials.SetValue(PatCur.PatNum,procCur.ToothNum,ToothInitialType.Missing);
				}
				procCur.ProcStatus=ProcStat.C;
				Procedures.Update(procCur,procOld);
				//Tried to move it to the business layer, but too complex for now.
				//Procedures.SetComplete(
				//	((Procedure)gridProg.Rows[gridProg.SelectedIndices[i]].Tag).ProcNum,PIn.PDate(textDate.Text));
				Procedures.ComputeEstimates(procCur,procCur.PatNum,ClaimProcList,false,PlanList,PatPlanList,BenefitList);
			}
			Recalls.Synch(PatCur.PatNum);
			//if(skipped>0){
			//	MessageBox.Show(Lan.g(this,".")+"\r\n"
			//		+skipped.ToString()+" "+Lan.g(this,"procedures(s) skipped."));
			//}
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemEditSelected_Click(object sender,EventArgs e) {
			//not functional yet
		}

		private void menuItemLabFee_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length!=2){
				MsgBox.Show(this,"Please select exactly two procedures, one regular and one lab.");
				return;
			}
			DataRow row1=DataSetMain.Tables["ProgNotes"].Rows[gridProg.SelectedIndices[0]];
			DataRow row2=DataSetMain.Tables["ProgNotes"].Rows[gridProg.SelectedIndices[1]];
			if(row1["ProcNum"].ToString()=="0" || row2["ProcNum"].ToString()=="0"){
				MsgBox.Show(this,"Both selected items must be procedures.");
				return;
			}
			bool isLab1=ProcedureCodes.GetProcCode(row1["ADACode"].ToString()).IsCanadianLab;
			bool isLab2=ProcedureCodes.GetProcCode(row2["ADACode"].ToString()).IsCanadianLab;
			if((isLab1 && isLab2) || (!isLab1 && !isLab2)) {
				MsgBox.Show(this,"One of the procedures must be a lab procedure as defined in Procedure Codes.");
				return;
			}
			Procedure procLab;
			Procedure procOld;
			if(isLab1){
				procLab=Procedures.GetOneProc(PIn.PInt(row1["ProcNum"].ToString()),false);
				procOld=procLab.Copy();
				procLab.ProcNumLab=PIn.PInt(row2["ProcNum"].ToString());
			}
			else{
				procLab=Procedures.GetOneProc(PIn.PInt(row2["ProcNum"].ToString()),false);
				procOld=procLab.Copy();
				procLab.ProcNumLab=PIn.PInt(row1["ProcNum"].ToString());
			}
			Procedures.Update(procLab,procOld);
			ModuleSelected(PatCur.PatNum);
		}

		private void menuItemLabFeeDetach_Click(object sender,EventArgs e) {
			if(gridProg.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select exactly one lab procedure first.");
				return;
			}
			DataRow row=DataSetMain.Tables["ProgNotes"].Rows[gridProg.SelectedIndices[0]];
			if(row["ProcNum"].ToString()=="0") {
				MsgBox.Show(this,"Please select a lab procedure first.");
				return;
			}
			if(row["ProcNumLab"].ToString()=="0") {
				MsgBox.Show(this,"The selected procedure is not attached as a lab procedure.");
				return;
			}
			Procedure procLab=Procedures.GetOneProc(PIn.PInt(row["ProcNum"].ToString()),false);
			Procedure procOld=procLab.Copy();
			procLab.ProcNumLab=0;
			Procedures.Update(procLab,procOld);
			ModuleSelected(PatCur.PatNum);
		}

		///<summary>Preview is only used for debugging.</summary>
		public void PrintReport(bool justPreview){
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			//pd2.DefaultPageSettings.Margins=new Margins(50,50,40,25);
			if(pd2.DefaultPageSettings.PaperSize.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			try{
				if(justPreview){
					FormRpPrintPreview pView = new FormRpPrintPreview();
					pView.printPreviewControl2.Document=pd2;
					pView.ShowDialog();				
			  }
				else{
					if(Printers.SetPrinter(pd2,PrintSituation.Default)){
						pd2.Print();
					}
				}
			}
			catch{
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			#region printHeading
			if(!headingPrinted){
				text="Chart Progress Notes";
				g.DrawString(text,headingFont,Brushes.Black,400-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=PatCur.GetNameFL();
				g.DrawString(text,subHeadingFont,Brushes.Black,400-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text=DateTime.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,400-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=30;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			int totalPages=gridProg.GetNumberOfPages(bounds,headingPrintH);
			yPos=gridProg.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			#region OldCode
			/*
			#region ColDefs
			//always defines and always prints on each page
			int rowH=(int)g.MeasureString("anything",bodyFont).Height;
			int[] colW=new int[8];
			colW[0]=80;
			colW[1]=30;
			colW[2]=53;
			colW[3]=30;
			colW[4]=325;
			colW[5]=30;
			colW[6]=55;
			colW[7]=60;
			int[] colPos=new int[colW.Length+1];//last entry represents the right side of the last col
			for(int i=0;i<colW.Length;i++){
				if(i==0){
					colPos[i]=75;
					continue;
				}
				colPos[i]=colPos[i-1]+colW[i-1];
				if(i==colW.Length-1){
					colPos[i+1]=colPos[i]+colW[i];
				}
			}
			HorizontalAlignment[] colAlign=new HorizontalAlignment[8];
			colAlign[7]=HorizontalAlignment.Right;
			string[] ColCaption=new string[8];
			ColCaption[0]=Lan.g("TableProg","Date");
			ColCaption[1]=Lan.g("TableProg","Th");
			ColCaption[2]=Lan.g("TableProg","Surf");
			ColCaption[3]=Lan.g("TableProg","Dx");
			ColCaption[4]=Lan.g("TableProg","Description");
			ColCaption[5]=Lan.g("TableProg","Stat");
			ColCaption[6]=Lan.g("TableProg","Prov");
			ColCaption[7]=Lan.g("TableProg","Amount");
			g.FillRectangle(Brushes.LightGray,colPos[0],yPos,colPos[colPos.Length-1]-colPos[0],16);
			g.DrawRectangle(Pens.Black,colPos[0],yPos,colPos[colPos.Length-1]-colPos[0],16);  
			for(int i=1;i<colPos.Length;i++) 
				g.DrawLine(new Pen(Color.Black),colPos[i],yPos,colPos[i],yPos+16);
			//Prints the Column Titles
			for(int i=0;i<ColCaption.Length;i++){ 
				if(colAlign[i]==HorizontalAlignment.Right){
					g.DrawString(ColCaption[i],totalFont,Brushes.Black,colPos[i+1]-g.MeasureString(ColCaption[i],totalFont).Width-1,yPos);
				}
				else 
					g.DrawString(Lan.g(this,ColCaption[i]),totalFont,Brushes.Black,colPos[i]+1,yPos);
			}
			yPos+=16;
			#endregion
			#region printBody
			while(linesPrinted < gridProg.Rows.Count 
				&& yPos//+g.MeasureString(((ProgLine)ProgLineAL[linesPrinted]).Note,bodyFont,colPos[5]-colPos[4]).Height 
				< e.MarginBounds.Height)
			{
				if(((ProgLine)ProgLineAL[linesPrinted]).IsNote){
					text=((ProgLine)ProgLineAL[linesPrinted]).Note;
					g.DrawString(text,bodyFont,Brushes.Black,new RectangleF(colPos[2]+1,yPos,colPos[8]-colPos[2]-4,bodyFont.GetHeight(g)));
					//Column lines		
					for(int i=0;i<colPos.Length-1;i++){
						//left vertical
						if(i<3){
	  					g.DrawLine(Pens.Gray,colPos[i],yPos+rowH,colPos[i],yPos);
						}
						//lower
						if(linesPrinted==ProgLineAL.Count || !((ProgLine)ProgLineAL[linesPrinted+1]).IsNote){
							g.DrawLine(Pens.Gray,colPos[i],yPos+rowH,colPos[i+1],yPos+rowH);
						}
					}
					//right vertical
					g.DrawLine(Pens.Gray,colPos[colPos.Length-1],yPos+rowH,colPos[colPos.Length-1],yPos);
					yPos+=rowH;
					linesPrinted++;
					continue;
				}
				for(int i=0;i<colPos.Length-1;i++){
					text=gridProg.Rows[linesPrinted].Cells[i].Text;
					switch(i){
						case 0:
							text=((ProgLine)ProgLineAL[linesPrinted]).Date;
							break;
						case 1:
							text=((ProgLine)ProgLineAL[linesPrinted]).Th;
							break;
						case 2:
							text=((ProgLine)ProgLineAL[linesPrinted]).Surf;
							break;
						case 3:
							text=((ProgLine)ProgLineAL[linesPrinted]).Dx;
							break;
						case 4:
							text=((ProgLine)ProgLineAL[linesPrinted]).Description;
							break;
						case 5:
							text=((ProgLine)ProgLineAL[linesPrinted]).Stat;
							break;
						case 6:
							text=((ProgLine)ProgLineAL[linesPrinted]).Prov;
							break;
						case 7:
							text=((ProgLine)ProgLineAL[linesPrinted]).Amount;
							break;
						default:
							text="";
							break;
					}
  				if(colAlign[i]==HorizontalAlignment.Right){
						g.DrawString(text,bodyFont,Brushes.Black,colPos[i+1]-g.MeasureString(text,bodyFont).Width-1,yPos);
					}
					else{
						g.DrawString(text,bodyFont,Brushes.Black,new RectangleF(colPos[i]+1,yPos,colPos[i+1]-colPos[i]-4,bodyFont.GetHeight(g)));
					}
					//left vertical
					g.DrawLine(Pens.Gray,colPos[i],yPos+rowH,colPos[i],yPos);
					//lower
					g.DrawLine(Pens.Gray,colPos[i],yPos+rowH,colPos[i+1],yPos+rowH);
				} 
				//right vertical
				g.DrawLine(Pens.Gray,colPos[colPos.Length-1],yPos+rowH,colPos[colPos.Length-1],yPos);
				yPos+=rowH;
				linesPrinted++;
			}
			#endregion
			*/
			#endregion OldCode
			pagesPrinted++;
			if(pagesPrinted < totalPages){
				e.HasMorePages=true;
			}
			else{
				e.HasMorePages=false;
			}
		}

		///<summary>Preview is only used for debugging.</summary>
		public void PrintDay(bool justPreview) {
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPageDay);
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd2.OriginAtMargins=true;
			if(pd2.DefaultPageSettings.PaperSize.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			try {
				if(justPreview) {
					FormRpPrintPreview pView = new FormRpPrintPreview();
					pView.printPreviewControl2.Document=pd2;
					pView.ShowDialog();
				}
				else {
					if(Printers.SetPrinter(pd2,PrintSituation.Default)) {
						pd2.Print();
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd2_PrintPageDay(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=new Rectangle(50,40,800,1020);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text="Chart Progress Notes";
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=PatCur.GetNameFL();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text="Birthdate: "+PatCur.Birthdate.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text="Printed: "+DateTime.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text="Ward: "+PatCur.Ward;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				/*string fileName=Documents.GetPatPict(PatCur.PatNum);
				if(fileName!="") {
					Image picturePat=null;
					string fullName=PrefB.GetString("DocPath")
						+PatCur.ImageFolder.Substring(0,1)+@"\"
						+PatCur.ImageFolder+@"\"
						+fileName;
					if(File.Exists(fullName)) {
						try {
							picturePat=Image.FromFile(fullName);
						}
						catch {
							;
						}
					}
					if(picturePat!=null){
						//Image.GetThumbnailImageAbort myCallback=new Image.GetThumbnailImageAbort(ThumbnailCallback);
						//Image myThumbnail=picturePat.GetThumbnailImage(80,80,myCallback,IntPtr.Zero);
						g.DrawImage(GetThumbnail(picturePat,80),center-40,yPos);
					}
					yPos+=80;
				}*/
				Bitmap picturePat;
				bool patientPictExists=Documents.GetPatPict(PatCur.PatNum,PrefB.GetString("DocPath")
					+PatCur.ImageFolder.Substring(0,1)+@"\"+PatCur.ImageFolder+@"\",out picturePat);
				if(picturePat!=null){//Successfully loaded a patient picture?
					Bitmap thumbnail=ContrDocs.GetThumbnail(picturePat,80);
					g.DrawImage(thumbnail,center-40,yPos);
				}
				if(patientPictExists){
					yPos+=80;
				}
				yPos+=30;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			int totalPages=gridProg.GetNumberOfPages(bounds,headingPrintH);
			yPos=gridProg.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			
			pagesPrinted++;
			if(pagesPrinted < totalPages) {
				e.HasMorePages=true;
			}
			else {
				g.DrawString("Signature_________________________________________________________",
								subHeadingFont,Brushes.Black,160,yPos+20);
				e.HasMorePages=false;
			}
		}

		///<summary>Draws one button for the tabControlImages.</summary>
		private void OnDrawItem(object sender, DrawItemEventArgs e){
      Graphics g=e.Graphics;
      Pen penBlue=new Pen(Color.FromArgb(97,136,173));
			Pen penRed=new Pen(Color.FromArgb(140,51,46));
			Pen penOrange=new Pen(Color.FromArgb(250,176,3),2);
			Pen penDkOrange=new Pen(Color.FromArgb(227,119,4));
			SolidBrush brBlack=new SolidBrush(Color.Black);
			int selected=tabControlImages.TabPages.IndexOf(tabControlImages.SelectedTab);
			Rectangle bounds=e.Bounds;
			Rectangle rect=new Rectangle(bounds.X+2,bounds.Y+1,bounds.Width-5,bounds.Height-4);
			if(e.Index==selected){
				g.FillRectangle(new SolidBrush(Color.White),rect);
				//g.DrawRectangle(penBlue,rect);
				g.DrawLine(penOrange,rect.X,rect.Bottom-1,rect.Right,rect.Bottom-1);
				g.DrawLine(penDkOrange,rect.X+1,rect.Bottom,rect.Right-2,rect.Bottom);
				g.DrawString(tabControlImages.TabPages[e.Index].Text,Font,brBlack,bounds.X+3,bounds.Y+6);
			}
			else{
				g.DrawString(tabControlImages.TabPages[e.Index].Text,Font,brBlack,bounds.X,bounds.Y);
			}
    }

		private void panelImages_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(e.Y>3){
				return;
			}
			MouseIsDownOnImageSplitter=true;
			ImageSplitterOriginalY=panelImages.Top;
			OriginalImageMousePos=panelImages.Top+e.Y;
		}

		private void panelImages_MouseLeave(object sender, System.EventArgs e) {
			//not needed.
		}

		private void panelImages_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(!MouseIsDownOnImageSplitter){
				if(e.Y<=3){
					panelImages.Cursor=Cursors.HSplit;
				}
				else{
					panelImages.Cursor=Cursors.Default;
				}
				return;
			}
			//panelNewTop
			int panelNewH=panelImages.Bottom
				-(ImageSplitterOriginalY+(panelImages.Top+e.Y)-OriginalImageMousePos);//-top
			if(panelNewH<10)//cTeeth.Bottom)
				panelNewH=10;//cTeeth.Bottom;//keeps it from going too low
			if(panelNewH>panelImages.Bottom-toothChart.Bottom)
				panelNewH=panelImages.Bottom-toothChart.Bottom;//keeps it from going too high
			panelImages.Height=panelNewH;
			//tbProg.LayoutTables();//too much flicker
		}

		private void panelImages_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(!MouseIsDownOnImageSplitter){
				return;
			}
			MouseIsDownOnImageSplitter=false;
			//tbProg.Height=ClientSize.Height-panelImages.Top-tbProg.Location.Y-2;
			//tbProg.LayoutTables();
		}

		private void tabProc_MouseDown(object sender,MouseEventArgs e) {
			//selected tab will have changed, so we need to test the original selected tab:
			Rectangle rect=tabProc.GetTabRect(SelectedProcTab);
			if(rect.Contains(e.X,e.Y) && tabProc.Height>27){//clicked on the already selected tab which was maximized
				tabProc.Height=27;
				tabProc.Refresh();
				gridProg.Location=new Point(tabProc.Left,tabProc.Bottom+1);
				gridProg.Height=this.ClientSize.Height-gridProg.Location.Y-2;
			}
			else if(tabProc.Height==27){//clicked on a minimized tab
				tabProc.Height=254;
				tabProc.Refresh();
				gridProg.Location=new Point(tabProc.Left,tabProc.Bottom+1);
				gridProg.Height=this.ClientSize.Height-gridProg.Location.Y-2;
			}
			else{//clicked on a new tab
				//height will have already been set, so do nothing
			}
			SelectedProcTab=tabProc.SelectedIndex;
			FillMovementsAndHidden();
		}

		private void tabControlImages_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(selectedImageTab==-1){
				selectedImageTab=tabControlImages.SelectedIndex;
				return;
			}
			Rectangle rect=tabControlImages.GetTabRect(selectedImageTab);
			if(rect.Contains(e.X,e.Y)){//clicked on the already selected tab
				if(panelImages.Visible){
					panelImages.Visible=false;
				}
				else{
					panelImages.Visible=true;
				}
				//tbProg.LayoutTables();
			}
			else{//clicked on a new tab
				if(!panelImages.Visible){
					panelImages.Visible=true;
					//tbProg.LayoutTables();
				}
			}
			selectedImageTab=tabControlImages.SelectedIndex;
			FillImages();//it will not actually fill the images unless panelImages is visible
		}

		private void listViewImages_DoubleClick(object sender, System.EventArgs e) {
			if(listViewImages.SelectedIndices.Count==0){
				return;//clicked on white space.
			}
			Document DocCur=DocumentList[(int)visImages[listViewImages.SelectedIndices[0]]];
			if(!ContrDocs.HasImageExtension(DocCur.FileName)){
				try{
					Process.Start(patFolder+DocCur.FileName);
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
				}
				return;
			}
			if(formImageViewer==null || !formImageViewer.Visible){
				formImageViewer=new FormImageViewer();
				formImageViewer.Show();
			}
			if(formImageViewer.WindowState==FormWindowState.Minimized){
				formImageViewer.WindowState=FormWindowState.Normal;
			}
			formImageViewer.BringToFront();
			formImageViewer.SetImage(DocCur,PatCur.GetNameLF()+" - "
				+DocCur.DateCreated.ToShortDateString()+": "+DocCur.Description);
		}

		private void butBig_Click(object sender,EventArgs e) {
			FormToothChartingBig FormT=new FormToothChartingBig(checkShowTeeth.Checked,ToothInitialList,ProcList);
			FormT.Show();
		}

		

		

		

		

		

		

		

		

		

		

		#region VisiQuick integration code written by Thomas Jensen tje@thomsystems.com 
		/*
		private void XrayLinkBtn_Click(object sender, System.EventArgs e)	// TJE
		{
			if (!Patients.PatIsLoaded || Patients.Cur.PatNum<1)
				return;
			VQLink.VQStart(false,"",0,0);
		}

		private void SetPanelCol(Panel p, char c)	// TJE
		{
			if (c != '0')
				p.BackColor=SystemColors.ActiveCaption;
			else
				p.BackColor=SystemColors.ActiveBorder;
		}

		private void VQUpdatePatient()	// TJE
		{
			String	s;
			if (!Patients.PatIsLoaded || Patients.Cur.PatNum<1)	
				s="";
			else
				s=VQLink.SearchTStatus(Patients.Cur.PatNum);
			if (s.Length>=32) 
			{
				SetPanelCol(tooth11,s[0]);
				SetPanelCol(tooth12,s[1]);
				SetPanelCol(tooth13,s[2]);
				SetPanelCol(tooth14,s[3]);
				SetPanelCol(tooth15,s[4]);
				SetPanelCol(tooth16,s[5]);
				SetPanelCol(tooth17,s[6]);
				SetPanelCol(tooth18,s[7]);
				SetPanelCol(tooth21,s[8]);
				SetPanelCol(tooth22,s[9]);
				SetPanelCol(tooth23,s[10]);
				SetPanelCol(tooth24,s[11]);
				SetPanelCol(tooth25,s[12]);
				SetPanelCol(tooth26,s[13]);
				SetPanelCol(tooth27,s[14]);
				SetPanelCol(tooth28,s[15]);
				SetPanelCol(tooth31,s[16]);
				SetPanelCol(tooth32,s[17]);
				SetPanelCol(tooth33,s[18]);
				SetPanelCol(tooth34,s[19]);
				SetPanelCol(tooth35,s[20]);
				SetPanelCol(tooth36,s[21]);
				SetPanelCol(tooth37,s[22]);
				SetPanelCol(tooth38,s[23]);
				SetPanelCol(tooth41,s[24]);
				SetPanelCol(tooth42,s[25]);
				SetPanelCol(tooth43,s[26]);
				SetPanelCol(tooth44,s[27]);
				SetPanelCol(tooth45,s[28]);
				SetPanelCol(tooth46,s[29]);
				SetPanelCol(tooth47,s[30]);
				SetPanelCol(tooth48,s[31]);
			}
			if (s.Length>=32+6) 
			{
				SetPanelCol(toothpanos,s[32]);
				SetPanelCol(toothcephs,s[33]);
				if (s[34]!='0' | s[35]!='0' | s[36]!='0' | s[37]!='0') 
				{
					SetPanelCol(toothbw,'1');
					SetPanelCol(toothbwfloat,'1');
				}
				else
				{
					SetPanelCol(toothbw,'0');
					SetPanelCol(toothbwfloat,'0');
				}
			}
			if (s.Length>=32+6+9) 
			{
				if (s[39]!='0' | s[40]!='0' | s[41]!='0' | s[43]!='0') 
					SetPanelCol(toothcolors,'1');
				else
					SetPanelCol(toothcolors,'0');
				SetPanelCol(toothxrays,s[42]);
				SetPanelCol(toothpanos,s[44]);
				SetPanelCol(toothcephs,s[45]);
				SetPanelCol(toothdocs,s[46]);
			}
			if (s.Length>=32+6+9+1) 
			{
				SetPanelCol(toothfiles,s[47]);
			}
		}

		private void tooth18_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos(((Panel)sender).Name.Substring(5,2),VisiQuick.spf_tinymode+VisiQuick.spf_single,0);	
		}

		private void toothbwfloat_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_tinymode+VisiQuick.spf_2horizontal,VisiQuick.spi_bitewings);
		}

		private void toothbw_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.npi_xrayview,VisiQuick.spi_bitewings);
		}

		private void toothxrays_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.VQStart(false,"",0,VisiQuick.npi_xrayview);
		}

		private void toothcolors_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.VQStart(false,"",0,VisiQuick.npi_colorview);
		}

		private void toothpanos_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_panview);
		}

		private void toothcephs_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_cephview);
		}

		private void toothdocs_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_docview);
		}

		private void toothfiles_Click(object sender, System.EventArgs e)	// TJE
		{
			VQLink.SearchPhotos("",VisiQuick.spf_single,VisiQuick.spi_fileview);
		}
		*/
		#endregion
	}//end class


	


}
