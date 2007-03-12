/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormProcEdit : System.Windows.Forms.Form{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textProc;
		private System.Windows.Forms.TextBox textSurfaces;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textDesc;
		private System.Windows.Forms.Label label7;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textRange;
		private System.ComponentModel.IContainer components;// Required designer variable.
		private System.Windows.Forms.Label labelTooth;
		private System.Windows.Forms.Label labelRange;
		private System.Windows.Forms.Label labelSurfaces;
		private System.Windows.Forms.GroupBox groupQuadrant;
		private System.Windows.Forms.RadioButton radioUR;
		private System.Windows.Forms.RadioButton radioUL;
		private System.Windows.Forms.RadioButton radioLL;
		private System.Windows.Forms.RadioButton radioLR;
		private System.Windows.Forms.GroupBox groupArch;
		private System.Windows.Forms.RadioButton radioU;
		private System.Windows.Forms.RadioButton radioL;
		private System.Windows.Forms.GroupBox groupSextant;
		private System.Windows.Forms.RadioButton radioS1;
		private System.Windows.Forms.RadioButton radioS3;
		private System.Windows.Forms.RadioButton radioS2;
		private System.Windows.Forms.RadioButton radioS4;
		private System.Windows.Forms.RadioButton radioS5;
		private System.Windows.Forms.RadioButton radioS6;
		private System.Windows.Forms.Label label9;
		private OpenDental.ValidDate textDate;
		///<summary>Mostly used for permissions.</summary>
		public bool IsNew;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labelClaim;
		private System.Windows.Forms.ListBox listBoxTeeth;
		private System.Windows.Forms.ListBox listBoxTeeth2;
		private OpenDental.UI.Button butChange;
		//private ProcStat OriginalStatus;
		private System.Windows.Forms.TextBox textTooth;
		private System.Windows.Forms.ErrorProvider errorProvider2;
		private OpenDental.UI.Button butEditAnyway;
		private System.Windows.Forms.Label labelDx;
		private System.Windows.Forms.ComboBox comboPlaceService;
		private System.Windows.Forms.Label labelPlaceService;
		private OpenDental.UI.Button butSetComplete;
		private System.Windows.Forms.Label label10;
		private ProcedureCode ProcedureCode2;
		private System.Windows.Forms.Label label13;
		private OpenDental.TableProcIns tbIns;
		private OpenDental.UI.Button butAddEstimate;
		private Procedure ProcCur;
		private Procedure ProcOld;
		private ClaimProc[] ClaimProcList;
		private OpenDental.ValidDouble textProcFee;
		private System.Windows.Forms.CheckBox checkNoBillIns;
		private OpenDental.ODtextBox textNotes;
		private System.Windows.Forms.GroupBox groupProsth;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelClaimNote;
		private System.Windows.Forms.ListBox listProsth;
		private OpenDental.ValidDate textDateOriginalProsth;
		private OpenDental.ODtextBox textClaimNote;
		private ClaimProc[] ClaimProcsForProc;
		//private Adjustment[] AdjForProc;
		private ArrayList PaySplitsForProc;
		private ArrayList AdjustmentsForProc;
		private Patient PatCur;
		private Family FamCur;
		private OpenDental.UI.Button butAddAdjust;
		private OpenDental.TableProcAdj tbAdj;
		private OpenDental.TableProcPay tbPay;
		private InsPlan[] PlanList;
		private System.Windows.Forms.Label labelIncomplete;
		private OpenDental.ValidDate textDateEntry;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		///<summary>List of all payments (not paysplits) that this procedure is attached to.</summary>
		private Payment[] PaymentsForProc;
		//private User user;
		private uint m_autoAPIMsg;//ENP
		private const string APPBAR_AUTOMATION_API_MESSAGE = "EZNotes.AppBarStandalone.Auto.API.Message"; 
		private const uint MSG_RESTORE=2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textMedicalCode;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox textDiagnosticCode;//ENP
		private const uint MSG_GETLASTNOTE=3;
		private System.Windows.Forms.CheckBox checkIsPrincDiag;//ENP
		private PatPlan[] PatPlanList;
		private ListBox listProcStatus;
		private Label label14;
		private Label label15;
		private Label label16;
		private OpenDental.UI.Button butTopazSign;
		private OpenDental.UI.Button butClearSig;
		private OpenDental.UI.SignatureBox sigBox;
		private Benefit[] BenefitList;
		private Topaz.SigPlusNET sigBoxTopaz;
		private bool SigChanged;
		private ComboBox comboProvNum;
		private ComboBox comboDx;
		private ComboBox comboPriority;
		private TextBox textUser;
		///<summary>This keeps the noteChanged event from erasing the signature when first loading.</summary>
		private bool IsStartingUp;

		///<summary>Inserts are no longer done within this dialog, but must be done ahead of time from outside.You must specify a procedure to edit, and only the changes that are made in this dialog get saved.  Only used when double click in Account, Chart, TP, and in ContrChart.AddProcedure().  The procedure may be deleted if new, and user hits Cancel.</summary>
		public FormProcEdit(Procedure proc,Patient patCur,Family famCur,InsPlan[] planList){
			ProcCur=proc;
			ProcOld=proc.Copy();
			PatCur=patCur;
			FamCur=famCur;
			PlanList=planList;
			InitializeComponent();
			Lan.F(this);
			sigBoxTopaz.Location=sigBox.Location;//this puts both boxes in the same spot.
			sigBoxTopaz.Visible=false;
			sigBox.SetTabletState(1);//It starts out accepting input. It will be set to 0 if a sig is already present.  It will be set back to 1 if note changes or if user clicks Clear.
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelTooth = new System.Windows.Forms.Label();
			this.labelSurfaces = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textProc = new System.Windows.Forms.TextBox();
			this.textTooth = new System.Windows.Forms.TextBox();
			this.textSurfaces = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textDesc = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.labelRange = new System.Windows.Forms.Label();
			this.textRange = new System.Windows.Forms.TextBox();
			this.groupQuadrant = new System.Windows.Forms.GroupBox();
			this.radioLR = new System.Windows.Forms.RadioButton();
			this.radioLL = new System.Windows.Forms.RadioButton();
			this.radioUL = new System.Windows.Forms.RadioButton();
			this.radioUR = new System.Windows.Forms.RadioButton();
			this.groupArch = new System.Windows.Forms.GroupBox();
			this.radioL = new System.Windows.Forms.RadioButton();
			this.radioU = new System.Windows.Forms.RadioButton();
			this.groupSextant = new System.Windows.Forms.GroupBox();
			this.radioS6 = new System.Windows.Forms.RadioButton();
			this.radioS5 = new System.Windows.Forms.RadioButton();
			this.radioS4 = new System.Windows.Forms.RadioButton();
			this.radioS2 = new System.Windows.Forms.RadioButton();
			this.radioS3 = new System.Windows.Forms.RadioButton();
			this.radioS1 = new System.Windows.Forms.RadioButton();
			this.label9 = new System.Windows.Forms.Label();
			this.labelDx = new System.Windows.Forms.Label();
			this.textDate = new OpenDental.ValidDate();
			this.textProcFee = new OpenDental.ValidDouble();
			this.panel1 = new System.Windows.Forms.Panel();
			this.listBoxTeeth = new System.Windows.Forms.ListBox();
			this.textDateEntry = new OpenDental.ValidDate();
			this.label12 = new System.Windows.Forms.Label();
			this.listBoxTeeth2 = new System.Windows.Forms.ListBox();
			this.butChange = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkIsPrincDiag = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textDiagnosticCode = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textMedicalCode = new System.Windows.Forms.TextBox();
			this.labelClaim = new System.Windows.Forms.Label();
			this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
			this.butEditAnyway = new OpenDental.UI.Button();
			this.comboPlaceService = new System.Windows.Forms.ComboBox();
			this.labelPlaceService = new System.Windows.Forms.Label();
			this.butSetComplete = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.tbIns = new OpenDental.TableProcIns();
			this.butAddEstimate = new OpenDental.UI.Button();
			this.checkNoBillIns = new System.Windows.Forms.CheckBox();
			this.textNotes = new OpenDental.ODtextBox();
			this.groupProsth = new System.Windows.Forms.GroupBox();
			this.listProsth = new System.Windows.Forms.ListBox();
			this.textDateOriginalProsth = new OpenDental.ValidDate();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textClaimNote = new OpenDental.ODtextBox();
			this.labelClaimNote = new System.Windows.Forms.Label();
			this.tbAdj = new OpenDental.TableProcAdj();
			this.tbPay = new OpenDental.TableProcPay();
			this.butAddAdjust = new OpenDental.UI.Button();
			this.labelIncomplete = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.listProcStatus = new System.Windows.Forms.ListBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.butTopazSign = new OpenDental.UI.Button();
			this.butClearSig = new OpenDental.UI.Button();
			this.sigBox = new OpenDental.UI.SignatureBox();
			this.sigBoxTopaz = new Topaz.SigPlusNET();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.comboDx = new System.Windows.Forms.ComboBox();
			this.comboProvNum = new System.Windows.Forms.ComboBox();
			this.textUser = new System.Windows.Forms.TextBox();
			this.groupQuadrant.SuspendLayout();
			this.groupArch.SuspendLayout();
			this.groupSextant.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
			this.groupProsth.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(34,27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70,12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(26,44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79,12);
			this.label2.TabIndex = 1;
			this.label2.Text = "Procedure";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelTooth
			// 
			this.labelTooth.Location = new System.Drawing.Point(68,88);
			this.labelTooth.Name = "labelTooth";
			this.labelTooth.Size = new System.Drawing.Size(36,12);
			this.labelTooth.TabIndex = 2;
			this.labelTooth.Text = "Tooth";
			this.labelTooth.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelTooth.Visible = false;
			// 
			// labelSurfaces
			// 
			this.labelSurfaces.Location = new System.Drawing.Point(33,116);
			this.labelSurfaces.Name = "labelSurfaces";
			this.labelSurfaces.Size = new System.Drawing.Size(73,16);
			this.labelSurfaces.TabIndex = 3;
			this.labelSurfaces.Text = "Surfaces";
			this.labelSurfaces.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelSurfaces.Visible = false;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(30,139);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(75,16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Amount";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textProc
			// 
			this.textProc.Location = new System.Drawing.Point(106,42);
			this.textProc.Name = "textProc";
			this.textProc.ReadOnly = true;
			this.textProc.Size = new System.Drawing.Size(76,20);
			this.textProc.TabIndex = 6;
			// 
			// textTooth
			// 
			this.textTooth.Location = new System.Drawing.Point(106,86);
			this.textTooth.Name = "textTooth";
			this.textTooth.Size = new System.Drawing.Size(28,20);
			this.textTooth.TabIndex = 7;
			this.textTooth.Visible = false;
			this.textTooth.Validating += new System.ComponentModel.CancelEventHandler(this.textTooth_Validating);
			// 
			// textSurfaces
			// 
			this.textSurfaces.Location = new System.Drawing.Point(106,114);
			this.textSurfaces.Name = "textSurfaces";
			this.textSurfaces.Size = new System.Drawing.Size(68,20);
			this.textSurfaces.TabIndex = 4;
			this.textSurfaces.Visible = false;
			this.textSurfaces.Validating += new System.ComponentModel.CancelEventHandler(this.textSurfaces_Validating);
			this.textSurfaces.TextChanged += new System.EventHandler(this.textSurfaces_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(0,62);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(105,16);
			this.label6.TabIndex = 13;
			this.label6.Text = "Description";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDesc
			// 
			this.textDesc.BackColor = System.Drawing.SystemColors.Control;
			this.textDesc.Location = new System.Drawing.Point(106,62);
			this.textDesc.Name = "textDesc";
			this.textDesc.Size = new System.Drawing.Size(216,20);
			this.textDesc.TabIndex = 16;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(429,145);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(73,16);
			this.label7.TabIndex = 0;
			this.label7.Text = "&Notes";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.Location = new System.Drawing.Point(779,643);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(76,26);
			this.butOK.TabIndex = 12;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(870,643);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(76,26);
			this.butCancel.TabIndex = 13;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.Image = ((System.Drawing.Image)(resources.GetObject("butDelete.Image")));
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(2,643);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(83,26);
			this.butDelete.TabIndex = 8;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// labelRange
			// 
			this.labelRange.Location = new System.Drawing.Point(24,88);
			this.labelRange.Name = "labelRange";
			this.labelRange.Size = new System.Drawing.Size(82,16);
			this.labelRange.TabIndex = 33;
			this.labelRange.Text = "Tooth Range";
			this.labelRange.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelRange.Visible = false;
			// 
			// textRange
			// 
			this.textRange.Location = new System.Drawing.Point(106,86);
			this.textRange.Name = "textRange";
			this.textRange.Size = new System.Drawing.Size(100,20);
			this.textRange.TabIndex = 34;
			this.textRange.Visible = false;
			// 
			// groupQuadrant
			// 
			this.groupQuadrant.Controls.Add(this.radioLR);
			this.groupQuadrant.Controls.Add(this.radioLL);
			this.groupQuadrant.Controls.Add(this.radioUL);
			this.groupQuadrant.Controls.Add(this.radioUR);
			this.groupQuadrant.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupQuadrant.Location = new System.Drawing.Point(104,80);
			this.groupQuadrant.Name = "groupQuadrant";
			this.groupQuadrant.Size = new System.Drawing.Size(108,56);
			this.groupQuadrant.TabIndex = 36;
			this.groupQuadrant.TabStop = false;
			this.groupQuadrant.Text = "Quadrant";
			this.groupQuadrant.Visible = false;
			// 
			// radioLR
			// 
			this.radioLR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioLR.Location = new System.Drawing.Point(12,36);
			this.radioLR.Name = "radioLR";
			this.radioLR.Size = new System.Drawing.Size(40,16);
			this.radioLR.TabIndex = 3;
			this.radioLR.Text = "LR";
			this.radioLR.Click += new System.EventHandler(this.radioLR_Click);
			// 
			// radioLL
			// 
			this.radioLL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioLL.Location = new System.Drawing.Point(64,36);
			this.radioLL.Name = "radioLL";
			this.radioLL.Size = new System.Drawing.Size(40,16);
			this.radioLL.TabIndex = 1;
			this.radioLL.Text = "LL";
			this.radioLL.Click += new System.EventHandler(this.radioLL_Click);
			// 
			// radioUL
			// 
			this.radioUL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioUL.Location = new System.Drawing.Point(64,16);
			this.radioUL.Name = "radioUL";
			this.radioUL.Size = new System.Drawing.Size(40,16);
			this.radioUL.TabIndex = 0;
			this.radioUL.Text = "UL";
			this.radioUL.Click += new System.EventHandler(this.radioUL_Click);
			// 
			// radioUR
			// 
			this.radioUR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioUR.Location = new System.Drawing.Point(12,16);
			this.radioUR.Name = "radioUR";
			this.radioUR.Size = new System.Drawing.Size(40,16);
			this.radioUR.TabIndex = 0;
			this.radioUR.Text = "UR";
			this.radioUR.Click += new System.EventHandler(this.radioUR_Click);
			// 
			// groupArch
			// 
			this.groupArch.Controls.Add(this.radioL);
			this.groupArch.Controls.Add(this.radioU);
			this.groupArch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupArch.Location = new System.Drawing.Point(104,80);
			this.groupArch.Name = "groupArch";
			this.groupArch.Size = new System.Drawing.Size(60,56);
			this.groupArch.TabIndex = 3;
			this.groupArch.TabStop = false;
			this.groupArch.Text = "Arch";
			this.groupArch.Visible = false;
			// 
			// radioL
			// 
			this.radioL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioL.Location = new System.Drawing.Point(12,36);
			this.radioL.Name = "radioL";
			this.radioL.Size = new System.Drawing.Size(28,16);
			this.radioL.TabIndex = 1;
			this.radioL.Text = "L";
			this.radioL.Click += new System.EventHandler(this.radioL_Click);
			// 
			// radioU
			// 
			this.radioU.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioU.Location = new System.Drawing.Point(12,16);
			this.radioU.Name = "radioU";
			this.radioU.Size = new System.Drawing.Size(32,16);
			this.radioU.TabIndex = 0;
			this.radioU.Text = "U";
			this.radioU.Click += new System.EventHandler(this.radioU_Click);
			// 
			// groupSextant
			// 
			this.groupSextant.Controls.Add(this.radioS6);
			this.groupSextant.Controls.Add(this.radioS5);
			this.groupSextant.Controls.Add(this.radioS4);
			this.groupSextant.Controls.Add(this.radioS2);
			this.groupSextant.Controls.Add(this.radioS3);
			this.groupSextant.Controls.Add(this.radioS1);
			this.groupSextant.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupSextant.Location = new System.Drawing.Point(104,80);
			this.groupSextant.Name = "groupSextant";
			this.groupSextant.Size = new System.Drawing.Size(156,56);
			this.groupSextant.TabIndex = 5;
			this.groupSextant.TabStop = false;
			this.groupSextant.Text = "Sextant";
			this.groupSextant.Visible = false;
			// 
			// radioS6
			// 
			this.radioS6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS6.Location = new System.Drawing.Point(12,36);
			this.radioS6.Name = "radioS6";
			this.radioS6.Size = new System.Drawing.Size(36,16);
			this.radioS6.TabIndex = 5;
			this.radioS6.Text = "6";
			this.radioS6.Click += new System.EventHandler(this.radioS6_Click);
			// 
			// radioS5
			// 
			this.radioS5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS5.Location = new System.Drawing.Point(60,36);
			this.radioS5.Name = "radioS5";
			this.radioS5.Size = new System.Drawing.Size(36,16);
			this.radioS5.TabIndex = 4;
			this.radioS5.Text = "5";
			this.radioS5.Click += new System.EventHandler(this.radioS5_Click);
			// 
			// radioS4
			// 
			this.radioS4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS4.Location = new System.Drawing.Point(108,36);
			this.radioS4.Name = "radioS4";
			this.radioS4.Size = new System.Drawing.Size(36,16);
			this.radioS4.TabIndex = 1;
			this.radioS4.Text = "4";
			this.radioS4.Click += new System.EventHandler(this.radioS4_Click);
			// 
			// radioS2
			// 
			this.radioS2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS2.Location = new System.Drawing.Point(60,16);
			this.radioS2.Name = "radioS2";
			this.radioS2.Size = new System.Drawing.Size(36,16);
			this.radioS2.TabIndex = 2;
			this.radioS2.Text = "2";
			this.radioS2.Click += new System.EventHandler(this.radioS2_Click);
			// 
			// radioS3
			// 
			this.radioS3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS3.Location = new System.Drawing.Point(108,16);
			this.radioS3.Name = "radioS3";
			this.radioS3.Size = new System.Drawing.Size(36,16);
			this.radioS3.TabIndex = 0;
			this.radioS3.Text = "3";
			this.radioS3.Click += new System.EventHandler(this.radioS3_Click);
			// 
			// radioS1
			// 
			this.radioS1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS1.Location = new System.Drawing.Point(12,16);
			this.radioS1.Name = "radioS1";
			this.radioS1.Size = new System.Drawing.Size(36,16);
			this.radioS1.TabIndex = 0;
			this.radioS1.Text = "1";
			this.radioS1.Click += new System.EventHandler(this.radioS1_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(3,185);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100,14);
			this.label9.TabIndex = 45;
			this.label9.Text = "Provider";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDx
			// 
			this.labelDx.Location = new System.Drawing.Point(3,209);
			this.labelDx.Name = "labelDx";
			this.labelDx.Size = new System.Drawing.Size(100,14);
			this.labelDx.TabIndex = 46;
			this.labelDx.Text = "Diagnosis";
			this.labelDx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(106,22);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(76,20);
			this.textDate.TabIndex = 0;
			// 
			// textProcFee
			// 
			this.textProcFee.Location = new System.Drawing.Point(106,136);
			this.textProcFee.Name = "textProcFee";
			this.textProcFee.Size = new System.Drawing.Size(68,20);
			this.textProcFee.TabIndex = 6;
			this.textProcFee.Validating += new System.ComponentModel.CancelEventHandler(this.textProcFee_Validating);
			// 
			// panel1
			// 
			this.panel1.AllowDrop = true;
			this.panel1.Controls.Add(this.listBoxTeeth);
			this.panel1.Controls.Add(this.textDesc);
			this.panel1.Controls.Add(this.textDateEntry);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.labelTooth);
			this.panel1.Controls.Add(this.labelSurfaces);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.textSurfaces);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.groupArch);
			this.panel1.Controls.Add(this.textDate);
			this.panel1.Controls.Add(this.groupQuadrant);
			this.panel1.Controls.Add(this.groupSextant);
			this.panel1.Controls.Add(this.textProcFee);
			this.panel1.Controls.Add(this.textTooth);
			this.panel1.Controls.Add(this.labelRange);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.textProc);
			this.panel1.Controls.Add(this.listBoxTeeth2);
			this.panel1.Controls.Add(this.textRange);
			this.panel1.Controls.Add(this.butChange);
			this.panel1.Location = new System.Drawing.Point(0,0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(383,179);
			this.panel1.TabIndex = 2;
			// 
			// listBoxTeeth
			// 
			this.listBoxTeeth.AllowDrop = true;
			this.listBoxTeeth.ColumnWidth = 16;
			this.listBoxTeeth.Font = new System.Drawing.Font("Microsoft Sans Serif",8F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.listBoxTeeth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
			this.listBoxTeeth.Location = new System.Drawing.Point(106,82);
			this.listBoxTeeth.MultiColumn = true;
			this.listBoxTeeth.Name = "listBoxTeeth";
			this.listBoxTeeth.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.listBoxTeeth.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxTeeth.Size = new System.Drawing.Size(272,17);
			this.listBoxTeeth.TabIndex = 1;
			this.listBoxTeeth.Visible = false;
			this.listBoxTeeth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxTeeth_MouseDown);
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(106,2);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(76,20);
			this.textDateEntry.TabIndex = 95;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(0,6);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(104,16);
			this.label12.TabIndex = 96;
			this.label12.Text = "Date Completed";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listBoxTeeth2
			// 
			this.listBoxTeeth2.ColumnWidth = 16;
			this.listBoxTeeth2.Items.AddRange(new object[] {
            "32",
            "31",
            "30",
            "29",
            "28",
            "27",
            "26",
            "25",
            "24",
            "23",
            "22",
            "21",
            "20",
            "19",
            "18",
            "17"});
			this.listBoxTeeth2.Location = new System.Drawing.Point(106,96);
			this.listBoxTeeth2.MultiColumn = true;
			this.listBoxTeeth2.Name = "listBoxTeeth2";
			this.listBoxTeeth2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxTeeth2.Size = new System.Drawing.Size(272,17);
			this.listBoxTeeth2.TabIndex = 2;
			this.listBoxTeeth2.Visible = false;
			this.listBoxTeeth2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxTeeth2_MouseDown);
			// 
			// butChange
			// 
			this.butChange.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butChange.Autosize = true;
			this.butChange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChange.Location = new System.Drawing.Point(184,37);
			this.butChange.Name = "butChange";
			this.butChange.Size = new System.Drawing.Size(74,25);
			this.butChange.TabIndex = 37;
			this.butChange.Text = "C&hange";
			this.butChange.Click += new System.EventHandler(this.butChange_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkIsPrincDiag);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.textDiagnosticCode);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.textMedicalCode);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(405,1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(205,79);
			this.groupBox1.TabIndex = 97;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Medical";
			// 
			// checkIsPrincDiag
			// 
			this.checkIsPrincDiag.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsPrincDiag.Location = new System.Drawing.Point(31,56);
			this.checkIsPrincDiag.Name = "checkIsPrincDiag";
			this.checkIsPrincDiag.Size = new System.Drawing.Size(137,19);
			this.checkIsPrincDiag.TabIndex = 101;
			this.checkIsPrincDiag.Text = "Is Principal Diagnosis";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(15,37);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(81,12);
			this.label11.TabIndex = 99;
			this.label11.Text = "ICD-9 Code";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDiagnosticCode
			// 
			this.textDiagnosticCode.Location = new System.Drawing.Point(99,34);
			this.textDiagnosticCode.Name = "textDiagnosticCode";
			this.textDiagnosticCode.Size = new System.Drawing.Size(76,20);
			this.textDiagnosticCode.TabIndex = 100;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16,17);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(81,12);
			this.label8.TabIndex = 97;
			this.label8.Text = "Medical Code";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textMedicalCode
			// 
			this.textMedicalCode.Location = new System.Drawing.Point(99,14);
			this.textMedicalCode.Name = "textMedicalCode";
			this.textMedicalCode.Size = new System.Drawing.Size(76,20);
			this.textMedicalCode.TabIndex = 98;
			// 
			// labelClaim
			// 
			this.labelClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelClaim.Font = new System.Drawing.Font("Microsoft Sans Serif",9F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelClaim.Location = new System.Drawing.Point(111,624);
			this.labelClaim.Name = "labelClaim";
			this.labelClaim.Size = new System.Drawing.Size(480,44);
			this.labelClaim.TabIndex = 50;
			this.labelClaim.Text = "This procedure is attached to a claim, so certain fields should not be edited.  Y" +
    "ou should reprint the claim if any significant changes are made.";
			this.labelClaim.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelClaim.Visible = false;
			// 
			// errorProvider2
			// 
			this.errorProvider2.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.errorProvider2.ContainerControl = this;
			// 
			// butEditAnyway
			// 
			this.butEditAnyway.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butEditAnyway.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditAnyway.Autosize = true;
			this.butEditAnyway.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditAnyway.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditAnyway.Location = new System.Drawing.Point(594,643);
			this.butEditAnyway.Name = "butEditAnyway";
			this.butEditAnyway.Size = new System.Drawing.Size(104,26);
			this.butEditAnyway.TabIndex = 51;
			this.butEditAnyway.Text = "&Edit Anyway";
			this.butEditAnyway.Visible = false;
			this.butEditAnyway.Click += new System.EventHandler(this.butEditAnyway_Click);
			// 
			// comboPlaceService
			// 
			this.comboPlaceService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlaceService.Location = new System.Drawing.Point(106,246);
			this.comboPlaceService.MaxDropDownItems = 30;
			this.comboPlaceService.Name = "comboPlaceService";
			this.comboPlaceService.Size = new System.Drawing.Size(177,21);
			this.comboPlaceService.TabIndex = 6;
			// 
			// labelPlaceService
			// 
			this.labelPlaceService.Location = new System.Drawing.Point(-11,251);
			this.labelPlaceService.Name = "labelPlaceService";
			this.labelPlaceService.Size = new System.Drawing.Size(114,16);
			this.labelPlaceService.TabIndex = 53;
			this.labelPlaceService.Text = "Place of Service";
			this.labelPlaceService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSetComplete
			// 
			this.butSetComplete.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butSetComplete.Autosize = true;
			this.butSetComplete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetComplete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetComplete.Location = new System.Drawing.Point(849,29);
			this.butSetComplete.Name = "butSetComplete";
			this.butSetComplete.Size = new System.Drawing.Size(98,25);
			this.butSetComplete.TabIndex = 54;
			this.butSetComplete.Text = "Set Complete";
			this.butSetComplete.Click += new System.EventHandler(this.butSetComplete_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(31,230);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(72,16);
			this.label10.TabIndex = 56;
			this.label10.Text = "Priority";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(849,57);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(110,33);
			this.label13.TabIndex = 58;
			this.label13.Text = "Also changes date and adds note.";
			// 
			// tbIns
			// 
			this.tbIns.BackColor = System.Drawing.SystemColors.Window;
			this.tbIns.Location = new System.Drawing.Point(1,438);
			this.tbIns.Name = "tbIns";
			this.tbIns.ScrollValue = 221;
			this.tbIns.SelectedIndices = new int[0];
			this.tbIns.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.tbIns.Size = new System.Drawing.Size(959,94);
			this.tbIns.TabIndex = 59;
			this.tbIns.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(this.tbIns_CellDoubleClicked);
			// 
			// butAddEstimate
			// 
			this.butAddEstimate.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAddEstimate.Autosize = true;
			this.butAddEstimate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddEstimate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddEstimate.Image = ((System.Drawing.Image)(resources.GetObject("butAddEstimate.Image")));
			this.butAddEstimate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddEstimate.Location = new System.Drawing.Point(2,408);
			this.butAddEstimate.Name = "butAddEstimate";
			this.butAddEstimate.Size = new System.Drawing.Size(109,26);
			this.butAddEstimate.TabIndex = 60;
			this.butAddEstimate.Text = "Add Estimate";
			this.butAddEstimate.Click += new System.EventHandler(this.butAddEstimate_Click);
			// 
			// checkNoBillIns
			// 
			this.checkNoBillIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNoBillIns.Location = new System.Drawing.Point(141,414);
			this.checkNoBillIns.Name = "checkNoBillIns";
			this.checkNoBillIns.Size = new System.Drawing.Size(152,18);
			this.checkNoBillIns.TabIndex = 9;
			this.checkNoBillIns.Text = "Do Not Bill to Ins";
			this.checkNoBillIns.ThreeState = true;
			this.checkNoBillIns.Click += new System.EventHandler(this.checkNoBillIns_Click);
			// 
			// textNotes
			// 
			this.textNotes.AcceptsReturn = true;
			this.textNotes.AcceptsTab = true;
			this.textNotes.Location = new System.Drawing.Point(504,139);
			this.textNotes.Multiline = true;
			this.textNotes.Name = "textNotes";
			this.textNotes.QuickPasteType = OpenDentBusiness.QuickPasteType.Procedure;
			this.textNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textNotes.Size = new System.Drawing.Size(450,180);
			this.textNotes.TabIndex = 1;
			this.textNotes.TextChanged += new System.EventHandler(this.textNotes_TextChanged);
			// 
			// groupProsth
			// 
			this.groupProsth.Controls.Add(this.listProsth);
			this.groupProsth.Controls.Add(this.textDateOriginalProsth);
			this.groupProsth.Controls.Add(this.label4);
			this.groupProsth.Controls.Add(this.label3);
			this.groupProsth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupProsth.Location = new System.Drawing.Point(15,270);
			this.groupProsth.Name = "groupProsth";
			this.groupProsth.Size = new System.Drawing.Size(275,85);
			this.groupProsth.TabIndex = 7;
			this.groupProsth.TabStop = false;
			this.groupProsth.Text = "Prosthesis Replacement";
			// 
			// listProsth
			// 
			this.listProsth.Location = new System.Drawing.Point(91,17);
			this.listProsth.Name = "listProsth";
			this.listProsth.Size = new System.Drawing.Size(163,43);
			this.listProsth.TabIndex = 0;
			// 
			// textDateOriginalProsth
			// 
			this.textDateOriginalProsth.Location = new System.Drawing.Point(91,61);
			this.textDateOriginalProsth.Name = "textDateOriginalProsth";
			this.textDateOriginalProsth.Size = new System.Drawing.Size(73,20);
			this.textDateOriginalProsth.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5,64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(84,16);
			this.label4.TabIndex = 4;
			this.label4.Text = "Original Date";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(2,17);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90,41);
			this.label3.TabIndex = 0;
			this.label3.Text = "Crown, Bridge, Denture, or RPD";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textClaimNote
			// 
			this.textClaimNote.AcceptsReturn = true;
			this.textClaimNote.Location = new System.Drawing.Point(106,359);
			this.textClaimNote.MaxLength = 80;
			this.textClaimNote.Multiline = true;
			this.textClaimNote.Name = "textClaimNote";
			this.textClaimNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Procedure;
			this.textClaimNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textClaimNote.Size = new System.Drawing.Size(277,43);
			this.textClaimNote.TabIndex = 10;
			// 
			// labelClaimNote
			// 
			this.labelClaimNote.Location = new System.Drawing.Point(0,359);
			this.labelClaimNote.Name = "labelClaimNote";
			this.labelClaimNote.Size = new System.Drawing.Size(104,41);
			this.labelClaimNote.TabIndex = 65;
			this.labelClaimNote.Text = "Claim Note (keep it very short)";
			this.labelClaimNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tbAdj
			// 
			this.tbAdj.BackColor = System.Drawing.SystemColors.Window;
			this.tbAdj.Location = new System.Drawing.Point(466,564);
			this.tbAdj.Name = "tbAdj";
			this.tbAdj.ScrollValue = 33;
			this.tbAdj.SelectedIndices = new int[0];
			this.tbAdj.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.tbAdj.Size = new System.Drawing.Size(494,72);
			this.tbAdj.TabIndex = 70;
			this.tbAdj.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(this.tbAdj_CellDoubleClicked);
			// 
			// tbPay
			// 
			this.tbPay.BackColor = System.Drawing.SystemColors.Window;
			this.tbPay.Location = new System.Drawing.Point(1,564);
			this.tbPay.Name = "tbPay";
			this.tbPay.ScrollValue = 33;
			this.tbPay.SelectedIndices = new int[0];
			this.tbPay.SelectionMode = System.Windows.Forms.SelectionMode.One;
			this.tbPay.Size = new System.Drawing.Size(449,72);
			this.tbPay.TabIndex = 71;
			this.tbPay.CellDoubleClicked += new OpenDental.ContrTable.CellEventHandler(this.tbPay_CellDoubleClicked);
			// 
			// butAddAdjust
			// 
			this.butAddAdjust.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAddAdjust.Autosize = true;
			this.butAddAdjust.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddAdjust.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddAdjust.Image = ((System.Drawing.Image)(resources.GetObject("butAddAdjust.Image")));
			this.butAddAdjust.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAdjust.Location = new System.Drawing.Point(466,534);
			this.butAddAdjust.Name = "butAddAdjust";
			this.butAddAdjust.Size = new System.Drawing.Size(114,26);
			this.butAddAdjust.TabIndex = 72;
			this.butAddAdjust.Text = "Add Adjustment";
			this.butAddAdjust.Click += new System.EventHandler(this.butAddAdjust_Click);
			// 
			// labelIncomplete
			// 
			this.labelIncomplete.Font = new System.Drawing.Font("Microsoft Sans Serif",10F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelIncomplete.ForeColor = System.Drawing.Color.DarkRed;
			this.labelIncomplete.Location = new System.Drawing.Point(379,163);
			this.labelIncomplete.Name = "labelIncomplete";
			this.labelIncomplete.Size = new System.Drawing.Size(123,18);
			this.labelIncomplete.TabIndex = 73;
			this.labelIncomplete.Text = "Incomplete";
			this.labelIncomplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(504,94);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(177,21);
			this.comboClinic.TabIndex = 74;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(388,96);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(114,16);
			this.labelClinic.TabIndex = 75;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listProcStatus
			// 
			this.listProcStatus.FormattingEnabled = true;
			this.listProcStatus.Location = new System.Drawing.Point(723,20);
			this.listProcStatus.Name = "listProcStatus";
			this.listProcStatus.Size = new System.Drawing.Size(120,82);
			this.listProcStatus.TabIndex = 76;
			this.listProcStatus.Click += new System.EventHandler(this.listProcStatus_Click);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(720,1);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(133,16);
			this.label14.TabIndex = 77;
			this.label14.Text = "Procedure Status";
			this.label14.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(373,326);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(126,16);
			this.label15.TabIndex = 79;
			this.label15.Text = "Signature / Initials";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(429,118);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(73,16);
			this.label16.TabIndex = 80;
			this.label16.Text = "User";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butTopazSign
			// 
			this.butTopazSign.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butTopazSign.Autosize = true;
			this.butTopazSign.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTopazSign.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTopazSign.Location = new System.Drawing.Point(873,359);
			this.butTopazSign.Name = "butTopazSign";
			this.butTopazSign.Size = new System.Drawing.Size(81,25);
			this.butTopazSign.TabIndex = 82;
			this.butTopazSign.Text = "Sign Topaz";
			this.butTopazSign.Click += new System.EventHandler(this.butTopazSign_Click);
			// 
			// butClearSig
			// 
			this.butClearSig.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butClearSig.Autosize = true;
			this.butClearSig.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearSig.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearSig.Location = new System.Drawing.Point(873,325);
			this.butClearSig.Name = "butClearSig";
			this.butClearSig.Size = new System.Drawing.Size(81,25);
			this.butClearSig.TabIndex = 85;
			this.butClearSig.Text = "Clear Sig";
			this.butClearSig.Click += new System.EventHandler(this.butClearSig_Click);
			// 
			// sigBox
			// 
			this.sigBox.Location = new System.Drawing.Point(505,325);
			this.sigBox.Name = "sigBox";
			this.sigBox.Size = new System.Drawing.Size(362,79);
			this.sigBox.TabIndex = 86;
			this.sigBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sigBox_MouseUp);
			// 
			// sigBoxTopaz
			// 
			this.sigBoxTopaz.Location = new System.Drawing.Point(426,207);
			this.sigBoxTopaz.Name = "sigBoxTopaz";
			this.sigBoxTopaz.Size = new System.Drawing.Size(362,79);
			this.sigBoxTopaz.TabIndex = 87;
			this.sigBoxTopaz.Text = "sigPlusNET1";
			// 
			// comboPriority
			// 
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.Location = new System.Drawing.Point(106,224);
			this.comboPriority.MaxDropDownItems = 30;
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(177,21);
			this.comboPriority.TabIndex = 98;
			// 
			// comboDx
			// 
			this.comboDx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDx.Location = new System.Drawing.Point(106,202);
			this.comboDx.MaxDropDownItems = 30;
			this.comboDx.Name = "comboDx";
			this.comboDx.Size = new System.Drawing.Size(177,21);
			this.comboDx.TabIndex = 99;
			// 
			// comboProvNum
			// 
			this.comboProvNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvNum.Location = new System.Drawing.Point(106,180);
			this.comboProvNum.MaxDropDownItems = 30;
			this.comboProvNum.Name = "comboProvNum";
			this.comboProvNum.Size = new System.Drawing.Size(177,21);
			this.comboProvNum.TabIndex = 100;
			// 
			// textUser
			// 
			this.textUser.Location = new System.Drawing.Point(504,117);
			this.textUser.Name = "textUser";
			this.textUser.ReadOnly = true;
			this.textUser.Size = new System.Drawing.Size(119,20);
			this.textUser.TabIndex = 101;
			// 
			// FormProcEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(962,675);
			this.Controls.Add(this.butTopazSign);
			this.Controls.Add(this.textUser);
			this.Controls.Add(this.comboProvNum);
			this.Controls.Add(this.comboDx);
			this.Controls.Add(this.comboPriority);
			this.Controls.Add(this.comboPlaceService);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.labelPlaceService);
			this.Controls.Add(this.labelDx);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.sigBoxTopaz);
			this.Controls.Add(this.sigBox);
			this.Controls.Add(this.butClearSig);
			this.Controls.Add(this.groupProsth);
			this.Controls.Add(this.textNotes);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.labelIncomplete);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.listProcStatus);
			this.Controls.Add(this.butAddAdjust);
			this.Controls.Add(this.textClaimNote);
			this.Controls.Add(this.butAddEstimate);
			this.Controls.Add(this.butSetComplete);
			this.Controls.Add(this.butEditAnyway);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.tbPay);
			this.Controls.Add(this.tbAdj);
			this.Controls.Add(this.labelClaimNote);
			this.Controls.Add(this.checkNoBillIns);
			this.Controls.Add(this.tbIns);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.labelClaim);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProcEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Procedure Info";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProcEdit_Closing);
			this.Load += new System.EventHandler(this.FormProcInfo_Load);
			this.groupQuadrant.ResumeLayout(false);
			this.groupArch.ResumeLayout(false);
			this.groupSextant.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
			this.groupProsth.ResumeLayout(false);
			this.groupProsth.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormProcInfo_Load(object sender, System.EventArgs e){
			//richTextBox1.Text="This is a test of the functions of a rich text box.";
			//webBrowser1.
			//richTextBox1.Select(10,4);
			//richTextBox1.SelectionFont=new Font(FontFamily.GenericMonospace,8);
			//richTextBox1.Select(22,9);
			//richTextBox1.SelectionFont=new Font(FontFamily.GenericMonospace,8,FontStyle.Underline);
			textDateEntry.Text=ProcCur.DateEntryC.ToShortDateString();
			if(PrefB.GetBool("EasyHidePublicHealth")){
				labelPlaceService.Visible=false;
				comboPlaceService.Visible=false;
			}
			if(PrefB.GetBool("UseInternationalToothNumbers")){
				listBoxTeeth.Items.Clear();
				listBoxTeeth.Items.AddRange(new string[] {"18","17","16","15","14","13","12","11","21","22","23","24","25","26","27","28"});
				listBoxTeeth2.Items.Clear();
				listBoxTeeth2.Items.AddRange(new string[] {"48","47","46","45","44","43","42","41","31","32","33","34","35","36","37","38"});
			}
			Claims.Refresh(PatCur.PatNum);
			ProcedureCode2=ProcedureCodes.GetProcCode(ProcCur.ADACode);
			if(IsNew){
				if(ProcCur.ProcStatus==ProcStat.C){
					if(!Security.IsAuthorized(Permissions.ProcComplCreate)){
						DialogResult=DialogResult.Cancel;
						return;
					}
				}
				//SetControls();
				//return;
			}
			else{
				if(ProcCur.ProcStatus==ProcStat.C){
					if(!Security.IsAuthorized(Permissions.ProcComplEdit,ProcCur.DateEntryC)){
						butOK.Enabled=false;//use this state to cascade permission to any form openned from here
						butDelete.Enabled=false;
						butChange.Enabled=false;
						butEditAnyway.Enabled=false;
						butSetComplete.Enabled=false;
					}
				}
			}
			ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			PatPlanList=PatPlans.Refresh(PatCur.PatNum);
			BenefitList=Benefits.Refresh(PatPlanList);
			if(Procedures.IsAttachedToClaim(ProcCur,ClaimProcList)){
				panel1.Enabled=false;
				listProcStatus.Enabled=false;
				checkNoBillIns.Enabled=false;
				butChange.Enabled=false;
				butDelete.Enabled=false;
				butEditAnyway.Visible=true;
				labelClaim.Visible=true;
			}
			if(PrefB.GetBool("EasyHideClinical")){
				labelDx.Visible=false;
				comboDx.Visible=false;
			}
			if(PrefB.GetBool("EasyNoClinics")){
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			/*if(CultureInfo.CurrentCulture.Name.Substring(3)=="CA"){//en-CA or fr-CA
				textLabFee.Text=ProcCur.LabFee.ToString("n");
				textLabCode.Text=ProcCur.LabProcCode;
			}
			else{
				labelLabFee.Visible=false;
				textLabFee.Visible=false;
				labelLabCode.Visible=false;
				textLabCode.Visible=false;
			}*/
			IsStartingUp=true;
			FillControls();
			SetControls();
			FillIns(false);
			FillPayments();
			FillAdj();
			IsStartingUp=false;
		}		

		///<summary>Only run on startup. Fills the basic controls, except not the ones in the upper left panel which are handled in SetControls.</summary>
		private void FillControls(){
			listProcStatus.Items.Clear();
			listProcStatus.Items.Add(Lan.g(this,"Treatment Planned"));
			listProcStatus.Items.Add(Lan.g(this,"Complete"));
			if(!PrefB.GetBool("EasyHideClinical")) {
				listProcStatus.Items.Add(Lan.g(this,"Existing-Current Prov"));
				listProcStatus.Items.Add(Lan.g(this,"Existing-Other Prov"));
				listProcStatus.Items.Add(Lan.g(this,"Referred Out"));
			}
			listProcStatus.Items.Add(Lan.g(this,"Deleted"));
			if(ProcCur.ProcStatus==ProcStat.TP){
				listProcStatus.SelectedIndex=0;
			}
			if(ProcCur.ProcStatus==ProcStat.C) {
				listProcStatus.SelectedIndex=1;
			}
			if(PrefB.GetBool("EasyHideClinical")) {
				if(ProcCur.ProcStatus==ProcStat.D) {
					listProcStatus.SelectedIndex=2;
				}
			}
			else{
				if(ProcCur.ProcStatus==ProcStat.EC) {
					listProcStatus.SelectedIndex=2;
				}
				if(ProcCur.ProcStatus==ProcStat.EO) {
					listProcStatus.SelectedIndex=3;
				}
				if(ProcCur.ProcStatus==ProcStat.R) {
					listProcStatus.SelectedIndex=4;
				}
				if(ProcCur.ProcStatus==ProcStat.D) {
					listProcStatus.SelectedIndex=5;
				}
			}
			if(ProcCur.ProcStatus==ProcStat.D){
				butOK.Enabled=false;
				butDelete.Enabled=false;
				butChange.Enabled=false;
				butEditAnyway.Enabled=false;
				butSetComplete.Enabled=false;
				butAddEstimate.Enabled=false;
				butAddAdjust.Enabled=false;
			}
			//if clinical is hidden, then there's a chance that no item is selected at this point.
			comboDx.Items.Clear();
			for(int i=0;i<DefB.Short[(int)DefCat.Diagnosis].Length;i++){
				comboDx.Items.Add(DefB.Short[(int)DefCat.Diagnosis][i].ItemName);
				if(DefB.Short[(int)DefCat.Diagnosis][i].DefNum==ProcCur.Dx)
					comboDx.SelectedIndex=i;
			}
			comboProvNum.Items.Clear();
			for(int i=0;i<Providers.List.Length;i++){
				comboProvNum.Items.Add(Providers.List[i].Abbr);
				if(Providers.List[i].ProvNum==ProcCur.ProvNum)
					comboProvNum.SelectedIndex=i;
			}
			comboPriority.Items.Clear();
			comboPriority.Items.Add(Lan.g(this,"no priority"));
			comboPriority.SelectedIndex=0;
			for(int i=0;i<DefB.Short[(int)DefCat.TxPriorities].Length;i++){
				comboPriority.Items.Add(DefB.Short[(int)DefCat.TxPriorities][i].ItemName);
				if(DefB.Short[(int)DefCat.TxPriorities][i].DefNum==ProcCur.Priority)
					comboPriority.SelectedIndex=i+1;
			}
			textNotes.Text=ProcCur.Note;
			textNotes.Select(textNotes.Text.Length,0);
			CheckForCompleteNote();
			comboPlaceService.Items.Clear();
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)ProcCur.PlaceService;
			//checkHideGraphical.Checked=ProcCur.HideGraphical;
			comboClinic.Items.Clear();
			comboClinic.Items.Add(Lan.g(this,"none"));
			comboClinic.SelectedIndex=0;
			for(int i=0;i<Clinics.List.Length;i++){
				comboClinic.Items.Add(Clinics.List[i].Description);
				if(Clinics.List[i].ClinicNum==ProcCur.ClinicNum){
					comboClinic.SelectedIndex=i+1;
				}
			}
			if(ProcedureCode2.IsProsth){
				listProsth.Items.Add(Lan.g(this,"No"));
				listProsth.Items.Add(Lan.g(this,"Initial"));
				listProsth.Items.Add(Lan.g(this,"Replacement"));
				switch(ProcCur.Prosthesis){
					case "":
						listProsth.SelectedIndex=0;
						break;
					case "I":
						listProsth.SelectedIndex=1;
						break;
					case "R":
						listProsth.SelectedIndex=2;
						break;
				}
				if(ProcCur.DateOriginalProsth.Year>1880){
					textDateOriginalProsth.Text=ProcCur.DateOriginalProsth.ToShortDateString();
				}
			}
			else{
				groupProsth.Visible=false;
			}
			textClaimNote.Text=ProcCur.ClaimNote;
			textUser.Text=UserodB.GetName(ProcCur.UserNum);//might be blank. Will change automatically if user changes note or alters sig.
			if(ProcCur.SigIsTopaz){
				if(ProcCur.Signature!=""){
					sigBoxTopaz.Visible=true;
					sigBoxTopaz.ClearTablet();
					sigBoxTopaz.SetSigCompressionMode(0);
					sigBoxTopaz.SetEncryptionMode(0);
					sigBoxTopaz.SetKeyString("0000000000000000");
					sigBoxTopaz.SetAutoKeyData(ProcCur.Note+ProcCur.UserNum.ToString());
					sigBoxTopaz.SetEncryptionMode(2);//high encryption
					sigBoxTopaz.SetSigCompressionMode(2);//high compression
					sigBoxTopaz.SetSigString(ProcCur.Signature);
					//if(sigBoxTopaz.NumberOfTabletPoints() > 0){
					//}
				}
			}
			else{
				if(ProcCur.Signature!=null && ProcCur.Signature!="") {
					sigBox.ClearTablet();
					//sigBox.SetSigCompressionMode(0);
					//sigBox.SetEncryptionMode(0);
					sigBox.SetKeyString("0000000000000000");
					sigBox.SetAutoKeyData(ProcCur.Note+ProcCur.UserNum.ToString());
					//sigBox.SetEncryptionMode(2);//high encryption
					//sigBox.SetSigCompressionMode(2);//high compression
					sigBox.SetSigString(ProcCur.Signature);
					//if(sigBox.NumberOfTabletPoints() > 0) {//the signature was successfully returned!
					//}
					sigBox.SetTabletState(0);//not accepting input.  To accept input, change the note, or clear the sig.
				}
			}
			
			/*if(ProcCur.DateLocked.Year>1880){//if locked
				butLock.Visible=false;
				//textNotes.ReadOnly=true;
				textNotes.Enabled=false;
				butDelete.Enabled=false;
				groupStatus.Enabled=false;
				butSetComplete.Enabled=false;
				textDateLocked.Text=ProcCur.DateLocked.ToShortDateString();
			}
			else{
				labelLocked.Visible=false;
				textDateLocked.Visible=false;
			}*/
		}

		///<summary>Called on open and after changing code.  Sets the visibilities and the data of all the fields in the upper left panel.</summary>
		private void SetControls(){
			textDate.Text=ProcCur.ProcDate.ToString("d");
			textProc.Text=ProcCur.ADACode;
			textDesc.Text=ProcedureCode2.Descript;
			textMedicalCode.Text=ProcCur.MedicalCode;
			textDiagnosticCode.Text=ProcCur.DiagnosticCode;
			checkIsPrincDiag.Checked=ProcCur.IsPrincDiag;
			switch (ProcedureCode2.TreatArea){
				case TreatmentArea.Surf:
					this.textTooth.Visible=true;
					this.labelTooth.Visible=true;
					this.textSurfaces.Visible=true;
					this.labelSurfaces.Visible=true;
					if(Tooth.IsValidDB(ProcCur.ToothNum)){
						errorProvider2.SetError(textTooth,"");
						textTooth.Text=Tooth.ToInternat(ProcCur.ToothNum);
						textSurfaces.Text=Tooth.SurfTidy(ProcCur.Surf,ProcCur.ToothNum,false);
					}
					else{
						errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
						textTooth.Text=ProcCur.ToothNum;
						//textSurfaces.Text=Tooth.SurfTidy(ProcCur.Surf,"");//only valid toothnums allowed
					}
					if(textSurfaces.Text=="")
						errorProvider2.SetError(textSurfaces,"No surfaces selected.");
					else
						errorProvider2.SetError(textSurfaces,"");
					break;
				case TreatmentArea.Tooth:
					this.textTooth.Visible=true;
					this.labelTooth.Visible=true;
					if(Tooth.IsValidDB(ProcCur.ToothNum)){
						errorProvider2.SetError(textTooth,"");
						textTooth.Text=Tooth.ToInternat(ProcCur.ToothNum);
					}
					else{
						errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
						textTooth.Text=ProcCur.ToothNum;
					}
					break;
				case TreatmentArea.Mouth:
						break;
				case TreatmentArea.Quad:
					this.groupQuadrant.Visible=true;
					switch (ProcCur.Surf){
						case "UR": this.radioUR.Checked=true; break;
						case "UL": this.radioUL.Checked=true; break;
						case "LR": this.radioLR.Checked=true; break;
						case "LL": this.radioLL.Checked=true; break;
						//default : 
					}
					break;
				case TreatmentArea.Sextant:
					this.groupSextant.Visible=true;
					switch (ProcCur.Surf){
						case "1": this.radioS1.Checked=true; break;
						case "2": this.radioS2.Checked=true; break;
						case "3": this.radioS3.Checked=true; break;
						case "4": this.radioS4.Checked=true; break;
						case "5": this.radioS5.Checked=true; break;
						case "6": this.radioS6.Checked=true; break;
						//default:
					}
					break;
				case TreatmentArea.Arch:
					this.groupArch.Visible=true;
					switch (ProcCur.Surf){
						case "U": this.radioU.Checked=true; break;
						case "L": this.radioL.Checked=true; break;
					}
					break;
				case TreatmentArea.ToothRange:
					this.labelRange.Visible=true;
					this.listBoxTeeth.Visible=true;
					this.listBoxTeeth2.Visible=true;
					if(ProcCur.ToothRange==null){
						break;
					}
   			  string[] sArray=ProcCur.ToothRange.Split(',');
          for(int i=0;i<sArray.Length;i++)  {
            for(int j=0;j<listBoxTeeth.Items.Count;j++)  {
              if(Tooth.ToInternat(sArray[i])==listBoxTeeth.Items[j].ToString())
				 		    listBoxTeeth.SelectedItem=Tooth.ToInternat(sArray[i]);
					  }
  			    for(int j=0;j<listBoxTeeth2.Items.Count;j++)  {
              if(Tooth.ToInternat(sArray[i])==listBoxTeeth2.Items[j].ToString())
				 		    listBoxTeeth2.SelectedItem=Tooth.ToInternat(sArray[i]);
            }
					} 
					break;
			}//end switch
			textProcFee.Text=ProcCur.ProcFee.ToString("n");
		}//end SetControls

		private void FillIns(){
			FillIns(true);
		}

		private void FillIns(bool refreshClaimProcsFirst){
			if(refreshClaimProcsFirst){
				ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			}
			ClaimProcsForProc=ClaimProcs.GetForProc(ClaimProcList,ProcCur.ProcNum);
			tbIns.ResetRows(ClaimProcsForProc.Length);
			checkNoBillIns.CheckState=CheckState.Unchecked;
			bool allNoBillIns=true;
			for(int i=0;i<ClaimProcsForProc.Length;i++){
				if(ClaimProcsForProc[i].NoBillIns){
					checkNoBillIns.CheckState=CheckState.Indeterminate;
				}
				else{
					allNoBillIns=false;
				}
				tbIns.Cell[0,i]=InsPlans.GetDescript(ClaimProcsForProc[i].PlanNum,FamCur,PlanList);
				if(ClaimProcsForProc[i].PlanNum==PatPlans.GetPlanNum(PatPlanList,1)){
					tbIns.Cell[1,i]="Pri";
				}
				else if(ClaimProcsForProc[i].PlanNum==PatPlans.GetPlanNum(PatPlanList,2)){
					tbIns.Cell[1,i]="Sec";
				}
				switch(ClaimProcsForProc[i].Status){
					case ClaimProcStatus.Received:
						tbIns.Cell[2,i]="Recd";
						break;
					case ClaimProcStatus.NotReceived:
						tbIns.Cell[2,i]="NotRec";
						break;
					//adjustment would never show here
					case ClaimProcStatus.Preauth:
						tbIns.Cell[2,i]="PreA";
						break;
					case ClaimProcStatus.Supplemental:
						tbIns.Cell[2,i]="Supp";
						break;
					case ClaimProcStatus.CapClaim:
						tbIns.Cell[2,i]="CapClaim";
						break;
					case ClaimProcStatus.Estimate:
						tbIns.Cell[2,i]="Est";
						break;
					case ClaimProcStatus.CapEstimate:
						tbIns.Cell[2,i]="CapEst";
						break;
					case ClaimProcStatus.CapComplete:
						tbIns.Cell[2,i]="CapComp";
						break;
				}
				if(ClaimProcsForProc[i].NoBillIns){
					tbIns.Cell[3,i]="X";
					if(ClaimProcsForProc[i].Status!=ClaimProcStatus.CapComplete
						&& ClaimProcsForProc[i].Status!=ClaimProcStatus.CapEstimate)
					{					
						tbIns.Cell[4,i]="";
						tbIns.Cell[5,i]="";
						tbIns.Cell[6,i]="";
						tbIns.Cell[7,i]="";
						tbIns.Cell[8,i]="";
						tbIns.Cell[9,i]="";
						tbIns.Cell[10,i]="";
						tbIns.Cell[11,i]="";
						tbIns.Cell[12,i]="";
						continue;
					}
				}
				int percent=0;
				if(ClaimProcsForProc[i].PercentOverride==-1){
					if(ClaimProcsForProc[i].Percentage==-1){
						//blank?
					}
					else{
						percent=ClaimProcsForProc[i].Percentage;
					}
				}
				else{
					percent=ClaimProcsForProc[i].PercentOverride;
				}
				tbIns.Cell[4,i]=percent.ToString();
				if(ClaimProcsForProc[i].CopayOverride!=-1){
					tbIns.Cell[5,i]=ClaimProcsForProc[i].CopayOverride.ToString("n");
				}
				else if(ClaimProcsForProc[i].CopayAmt!=-1){
					tbIns.Cell[5,i]=ClaimProcsForProc[i].CopayAmt.ToString("n");
				}
				tbIns.Cell[6,i]=ClaimProcsForProc[i].BaseEst.ToString("n");
				if(ClaimProcsForProc[i].OverrideInsEst!=-1){
					tbIns.Cell[7,i]=ClaimProcsForProc[i].OverrideInsEst.ToString("n");
					tbIns.FontBold[7,i]=true;
				}
				else{
					tbIns.FontBold[6,i]=true;
				}
				tbIns.Cell[8,i]=ClaimProcsForProc[i].DedApplied.ToString("n");
				tbIns.Cell[9,i]=ClaimProcsForProc[i].InsPayEst.ToString("n");
				tbIns.Cell[10,i]=ClaimProcsForProc[i].InsPayAmt.ToString("n");
				tbIns.Cell[11,i]=ClaimProcsForProc[i].WriteOff.ToString("n");
				tbIns.Cell[12,i]=ClaimProcsForProc[i].Remarks;
				if(ClaimProcsForProc[i].Status==ClaimProcStatus.CapEstimate
					|| ClaimProcsForProc[i].Status==ClaimProcStatus.CapComplete){
					tbIns.Cell[4,i]="";//percent
					tbIns.Cell[6,i]="";//baseEst
					tbIns.Cell[8,i]="";//deduct
					tbIns.Cell[9,i]="";//insest
					tbIns.Cell[10,i]="";//inspay
				}
				if(ClaimProcsForProc[i].Status==ClaimProcStatus.Estimate){
					//tbIns.Cell[8,i]="";//deduct
					//tbIns.Cell[9,i]="";//insest
					tbIns.Cell[10,i]="";//inspay
					tbIns.Cell[11,i]="";//writeoff
				}
			}
			if(ClaimProcsForProc.Length==0)
				checkNoBillIns.CheckState=CheckState.Unchecked;
			else if(allNoBillIns){
				checkNoBillIns.CheckState=CheckState.Checked;
			}
			tbIns.SetGridColor(Color.LightGray);
			tbIns.LayoutTables();
		}

		private void tbIns_CellDoubleClicked(object sender, OpenDental.CellEventArgs e) {
			FormClaimProc FormC=new FormClaimProc(ClaimProcsForProc[e.Row],ProcCur,FamCur,PlanList);
			if(!butOK.Enabled){
				FormC.NoPermission=true;
			}
			FormC.ShowDialog();
			FillIns();
		}

		private void butAddEstimate_Click(object sender, System.EventArgs e) {
			FormInsPlanSelect FormIS=new FormInsPlanSelect(PatCur.PatNum);
			FormIS.ShowDialog();
			if(FormIS.DialogResult==DialogResult.Cancel){
				return;
			}
			Benefit[] benList=Benefits.Refresh(PatPlanList);
			ClaimProc cp=new ClaimProc();
			ClaimProcs.CreateEst(cp,ProcCur,FormIS.SelectedPlan);
			if(FormIS.SelectedPlan.PlanNum==PatPlans.GetPlanNum(PatPlanList,1)){
				ClaimProcs.ComputeBaseEst(cp,ProcCur,PriSecTot.Pri,PlanList,PatPlanList,benList);
			}
			else if(FormIS.SelectedPlan.PlanNum==PatPlans.GetPlanNum(PatPlanList,2)){
				ClaimProcs.ComputeBaseEst(cp,ProcCur,PriSecTot.Sec,PlanList,PatPlanList,benList);
			}
			FormClaimProc FormC=new FormClaimProc(cp,ProcCur,FamCur,PlanList);
			//FormC.NoPermission not needed because butAddEstimate not enabled
			FormC.ShowDialog();
			if(FormC.DialogResult==DialogResult.Cancel){
				ClaimProcs.Delete(cp);
			}
			FillIns();
		}

		private void FillPayments(){
			PaySplit[] PaySplitList=PaySplits.Refresh(ProcCur.PatNum);
			PaySplitsForProc=PaySplits.GetForProc(ProcCur.ProcNum,PaySplitList);
			int[] payNums=new int[PaySplitsForProc.Count];
			for(int i=0;i<payNums.Length;i++){
				payNums[i]=((PaySplit)PaySplitsForProc[i]).PayNum;
			}
			PaymentsForProc=Payments.GetPayments(payNums);
			tbPay.ResetRows(PaySplitsForProc.Count);
			Payment PaymentCur;//used in loop
			for(int i=0;i<PaySplitsForProc.Count;i++){
				tbPay.Cell[0,i]=((PaySplit)PaySplitsForProc[i]).DatePay.ToShortDateString();
				tbPay.Cell[1,i]=((PaySplit)PaySplitsForProc[i]).SplitAmt.ToString("F");
				tbPay.FontBold[1,i]=true;
				PaymentCur=Payments.GetFromList(((PaySplit)PaySplitsForProc[i]).PayNum,PaymentsForProc);
				tbPay.Cell[2,i]=PaymentCur.PayAmt.ToString("F");
				tbPay.Cell[3,i]=PaymentCur.PayNote;
			}
			tbPay.SetGridColor(Color.LightGray);
			tbPay.LayoutTables();
		}

		private void tbPay_CellDoubleClicked(object sender, OpenDental.CellEventArgs e) {
			Payment PaymentCur=Payments.GetFromList(((PaySplit)PaySplitsForProc[e.Row]).PayNum,PaymentsForProc);
			FormPayment FormP=new FormPayment(PatCur,FamCur,PaymentCur);
			FormP.InitialPaySplit=((PaySplit)PaySplitsForProc[e.Row]).SplitNum;
			FormP.ShowDialog();
			FillPayments();
		}

		private void FillAdj(){
			Adjustment[] AdjustmentList=Adjustments.Refresh(ProcCur.PatNum);
			AdjustmentsForProc=Adjustments.GetForProc(ProcCur.ProcNum,AdjustmentList);
			tbAdj.ResetRows(AdjustmentsForProc.Count);
			for(int i=0;i<AdjustmentsForProc.Count;i++){
				tbAdj.Cell[0,i]=((Adjustment)AdjustmentsForProc[i]).AdjDate.ToShortDateString();
				tbAdj.Cell[1,i]=((Adjustment)AdjustmentsForProc[i]).AdjAmt.ToString("F");
				tbAdj.FontBold[1,i]=true;
				tbAdj.Cell[2,i]=DefB.GetName(DefCat.AdjTypes,((Adjustment)AdjustmentsForProc[i]).AdjType);
				tbAdj.Cell[3,i]=((Adjustment)AdjustmentsForProc[i]).AdjNote;
			}
			tbAdj.SetGridColor(Color.LightGray);
			tbAdj.LayoutTables();
		}

		private void butAddAdjust_Click(object sender, System.EventArgs e) {
			if(ProcCur.ProcStatus!=ProcStat.C){
				MsgBox.Show(this,"Adjustments may only be added to completed procedures.");
				return;
			}
			Adjustment adj=new Adjustment();
			adj.PatNum=PatCur.PatNum;
			adj.ProvNum=ProcCur.ProvNum;
			adj.DateEntry=DateTime.Today;//but will get overwritten to server date
			adj.AdjDate=DateTime.Today;
			adj.ProcDate=ProcCur.ProcDate;
			adj.ProcNum=ProcCur.ProcNum;
			FormAdjust FormA=new FormAdjust(PatCur,adj);
			FormA.IsNew=true;
			FormA.ShowDialog();
			FillAdj();
		}

		private void tbAdj_CellDoubleClicked(object sender, OpenDental.CellEventArgs e) {
			FormAdjust FormA=new FormAdjust(PatCur,(Adjustment)AdjustmentsForProc[e.Row]);
			FormA.ShowDialog();
			FillAdj();
		}

		private void textProcFee_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if(textProcFee.errorProvider1.GetError(textProcFee)!=""){
				return;
			}
			double procFee;
			if(textProcFee.Text==""){
				procFee=0;
			}
			else{
				procFee=PIn.PDouble(textProcFee.Text);
			}
			if(ProcCur.ProcFee==procFee){
				return;
			}
			ProcCur.ProcFee=procFee;
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ClaimProcList,false,PlanList,PatPlanList,BenefitList);
			FillIns();
		}

		private void textTooth_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			textTooth.Text=textTooth.Text.ToUpper();
			if(!Tooth.IsValidEntry(textTooth.Text))
				errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
			else
				errorProvider2.SetError(textTooth,"");
		}

		private void textSurfaces_TextChanged(object sender, System.EventArgs e) {
			int cursorPos = textSurfaces.SelectionStart;
			textSurfaces.Text=textSurfaces.Text.ToUpper();
			textSurfaces.SelectionStart=cursorPos;
		}

		private void textSurfaces_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if(Tooth.IsValidEntry(textTooth.Text)){
				textSurfaces.Text=Tooth.SurfTidy(textSurfaces.Text,Tooth.FromInternat(textTooth.Text),false);
			}
			else{
				textSurfaces.Text=Tooth.SurfTidy(textSurfaces.Text,"",false);
			}
			if(textSurfaces.Text=="")
				errorProvider2.SetError(textSurfaces,"No surfaces selected.");
			else
				errorProvider2.SetError(textSurfaces,"");
		}

		private void listBoxTeeth2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
		  listBoxTeeth.SelectedIndex=-1;
		}

		private void listBoxTeeth_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
		  listBoxTeeth2.SelectedIndex=-1;
		}

		private void butChange_Click(object sender, System.EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
      FormP.IsSelectionMode=true;
      FormP.ShowDialog();
      if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
      ProcCur.ADACode=FormP.SelectedADA;
      ProcedureCode2=ProcedureCodes.GetProcCode(FormP.SelectedADA);
      textDesc.Text=ProcedureCode2.Descript;
      ProcCur.ProcFee=Fees.GetAmount0(FormP.SelectedADA,Fees.GetFeeSched(PatCur,PlanList,PatPlanList));
			switch(ProcedureCode2.TreatArea){ 
				case TreatmentArea.Quad:
					ProcCur.Surf="UR";
					radioUR.Checked=true;
					break;
				case TreatmentArea.Sextant:
					ProcCur.Surf="1";
					radioS1.Checked=true;
					break;
				case TreatmentArea.Arch:
					ProcCur.Surf="U";
					radioU.Checked=true;
					break;
			}
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ClaimProcList,false,PlanList,PatPlanList,BenefitList);
			FillIns();
      SetControls();
		}

		private void butEditAnyway_Click(object sender, System.EventArgs e) {
			panel1.Enabled=true;
			listProcStatus.Enabled=true;
			checkNoBillIns.Enabled=true;
			butDelete.Enabled=true;
			butChange.Enabled=true;
			//checkIsCovIns.Enabled=true;
		}

		private void listProcStatus_Click(object sender,EventArgs e) {
			if(listProcStatus.SelectedIndex==0){
				ProcCur.ProcStatus=ProcStat.TP;
				//fee starts out 0 if EO, EC, etc.  This updates fee if changing to TP so it won't stay 0.
				if(ProcCur.ProcFee==0) {
					ProcCur.ProcFee=Fees.GetAmount0(ProcCur.ADACode,Fees.GetFeeSched(PatCur,PlanList,PatPlanList));
					textProcFee.Text=ProcCur.ProcFee.ToString("f");
				}
			}
			if(listProcStatus.SelectedIndex==1){
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
					//set it back to whatever it was before
					if(ProcCur.ProcStatus==ProcStat.TP) {
						listProcStatus.SelectedIndex=0;
					}
					else if(PrefB.GetBool("EasyHideClinical")) {
						if(ProcCur.ProcStatus==ProcStat.D) {
							listProcStatus.SelectedIndex=2;
						}
						else{
							listProcStatus.SelectedIndex=-1;//original status must not be visible
						}
					}
					else {
						if(ProcCur.ProcStatus==ProcStat.EO) {
							listProcStatus.SelectedIndex=2;
						}
						if(ProcCur.ProcStatus==ProcStat.EC) {
							listProcStatus.SelectedIndex=3;
						}
						if(ProcCur.ProcStatus==ProcStat.R) {
							listProcStatus.SelectedIndex=4;
						}
						if(ProcCur.ProcStatus==ProcStat.D) {
							listProcStatus.SelectedIndex=5;
						}
					}
					return;
				}
				Procedures.SetDateFirstVisit(DateTime.Today,2,PatCur);
				ProcCur.ProcStatus=ProcStat.C;
			}
			if(listProcStatus.SelectedIndex==2) {
				if(PrefB.GetBool("EasyHideClinical")){
					ProcCur.ProcStatus=ProcStat.D;
				}
				else{
					ProcCur.ProcStatus=ProcStat.EC;
				}
			}
			if(listProcStatus.SelectedIndex==3) {
				ProcCur.ProcStatus=ProcStat.EO;
			}
			if(listProcStatus.SelectedIndex==4) {
				ProcCur.ProcStatus=ProcStat.R;
			}
			if(listProcStatus.SelectedIndex==5) {
				ProcCur.ProcStatus=ProcStat.D;
			}
		}

		private void butSetComplete_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ProcComplCreate)){
				return;
			}
			Procedures.SetDateFirstVisit(DateTime.Today,2,PatCur);
			if(ProcCur.AptNum!=0){//if attached to an appointment
				textDate.Text=Appointments.GetOneApt(ProcCur.AptNum).AptDateTime.ToShortDateString();
			}
			else{
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			if(ProcedureCode2.PaintType==ToothPaintingType.Extraction){//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(ProcCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			textNotes.Text+=ProcedureCode2.DefaultNote;
			listProcStatus.SelectedIndex=-1;
			//radioStatusC.Checked=true;
			ProcCur.ProcStatus=ProcStat.C;
			comboPlaceService.SelectedIndex
				=PIn.PInt(((Pref)PrefB.HList["DefaultProcedurePlaceService"]).ValueString);
			if(EntriesAreValid()){
				SaveAndClose();
			}
		}

		private void radioUR_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="UR";
		}

		private void radioUL_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="UL";
		}

		private void radioLR_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="LR";
		}

		private void radioLL_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="LL";
		}

		private void radioU_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="U";
		}

		private void radioL_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="L";
		}

		private void radioS1_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="1";
		}

		private void radioS2_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="2";
		}

		private void radioS3_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="3";
		}

		private void radioS4_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="4";
		}

		private void radioS5_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="5";
		}

		private void radioS6_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="6";
		}

		private void checkNoBillIns_Click(object sender, System.EventArgs e) {
			if(checkNoBillIns.CheckState==CheckState.Indeterminate){
				//not allowed to set to indeterminate, so move on
				checkNoBillIns.CheckState=CheckState.Unchecked;
			}
			for(int i=0;i<ClaimProcsForProc.Length;i++){
				//ignore CapClaim,NotReceived,PreAuth,Recieved,Supplemental
				if(ClaimProcsForProc[i].Status==ClaimProcStatus.Estimate
					|| ClaimProcsForProc[i].Status==ClaimProcStatus.CapComplete
					|| ClaimProcsForProc[i].Status==ClaimProcStatus.CapEstimate)
				{
					if(checkNoBillIns.CheckState==CheckState.Checked){
						ClaimProcsForProc[i].NoBillIns=true;
					}
					else{//unchecked
						ClaimProcsForProc[i].NoBillIns=false;
					}
					ClaimProcs.Update(ClaimProcsForProc[i]);
				}
			}
			//next line is needed to recalc BaseEst, etc, for claimprocs that are no longer NoBillIns
			//also, if they are NoBillIns, then it clears out the other values.
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ClaimProcList,false,PlanList,PatPlanList,BenefitList);
			FillIns();
		}

		private void textNotes_TextChanged(object sender, System.EventArgs e) {
			CheckForCompleteNote();
			if(!IsStartingUp//so this happens only if user changes the note
				&& !SigChanged)//and the original signature is still showing.
			{
				sigBox.ClearTablet();
				sigBoxTopaz.ClearTablet();
				sigBoxTopaz.Visible=false;//until user explicitly starts it.
				sigBox.SetTabletState(1);//on-screen box is now accepting input.
				SigChanged=true;
				ProcCur.UserNum=Security.CurUser.UserNum;
				textUser.Text=UserodB.GetName(ProcCur.UserNum);
			}
		}

		private void CheckForCompleteNote(){
			if(textNotes.Text.IndexOf("\"\"")==-1){
				//no occurances of ""
				labelIncomplete.Visible=false;
			}
			else{
				labelIncomplete.Visible=true;
			}
		}

		private void butClearSig_Click(object sender,EventArgs e) {
			sigBox.ClearTablet();
			sigBoxTopaz.ClearTablet();
			sigBoxTopaz.Visible=false;//until user explicitly starts it.
			sigBox.SetTabletState(1);//on-screen box is now accepting input.
			SigChanged=true;
			ProcCur.UserNum=Security.CurUser.UserNum;
			textUser.Text=UserodB.GetName(ProcCur.UserNum);
		}

		private void butTopazSign_Click(object sender,EventArgs e) {
			sigBoxTopaz.Visible=true;
			sigBoxTopaz.SetTabletState(1);
			SigChanged=true;
			ProcCur.UserNum=Security.CurUser.UserNum;
			textUser.Text=UserodB.GetName(ProcCur.UserNum);
		}

		//private void butTopazStop_Click(object sender,EventArgs e) {
			//we might just have this happen automatically when closing form.
		//}

		//private void sigBox_MouseDown(object sender,MouseEventArgs e) {
			
		//}

		private void sigBox_MouseUp(object sender,MouseEventArgs e) {
			//this is done on mouse up so that the initial pen capture won't be delayed.
			if(sigBox.GetTabletState()==1//if accepting input.
				&& !SigChanged)//and sig not changed yet
			{
				//sigBox handles its own pen input.
				SigChanged=true;
				ProcCur.UserNum=Security.CurUser.UserNum;
				textUser.Text=UserodB.GetName(ProcCur.UserNum);
			}
		}


		private void butDelete_Click(object sender, System.EventArgs e) {
			if(MessageBox.Show(Lan.g(this,"Delete Procedure?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			if(Procedures.Delete(ProcCur.ProcNum)){//also deletes the claimProcs and adjustments.
				Recalls.Synch(ProcCur.PatNum);//needs to be moved into ProcedureB.Delete
				DialogResult=DialogResult.OK;	
			}
		}

		/*private void DeleteClaimProcs(){
			for(int i=0;i<ClaimProcList.Length;i++){
				if(ClaimProcList[i].ProcNum==ProcCur.ProcNum){
					ClaimProcList[i].Delete();
				}
			}
		}*/

		private void butOK_Click(object sender, System.EventArgs e){
			if(EntriesAreValid()){
				SaveAndClose();
			}
		}

		private bool EntriesAreValid(){
			if(  textDate.errorProvider1.GetError(textDate)!=""
				|| textProcFee.errorProvider1.GetError(textProcFee)!=""
				//|| textLabFee.errorProvider1.GetError(textLabFee)!=""
				|| textDateOriginalProsth.errorProvider1.GetError(textDateOriginalProsth)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(errorProvider2.GetError(textSurfaces)!=""
				|| errorProvider2.GetError(textTooth)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(textMedicalCode.Text!="" && !ProcedureCodes.HList.Contains(textMedicalCode.Text)){
				MsgBox.Show(this,"Invalid medical code.  It must refer to an existing procedure code.");
				return false;
			}
			/*if(CultureInfo.CurrentCulture.Name.Substring(3)=="CA"){//en-CA or fr-CA
				if(textLabCode.Text!="" && !ProcedureCodes.HList.Contains(textLabCode.Text)) {
					MsgBox.Show(this,"Invalid lab code.  It must refer to an existing procedure code.");
					return false;
				}
				if(PIn.PDouble(textLabFee.Text) >0 && textLabCode.Text=="") {
					MsgBox.Show(this,"Must enter a lab code if a lab fee is entered.");
					return false;
				}
			}*/
			if(ProcedureCode2.IsProsth){
				if(listProsth.SelectedIndex==0
					|| (listProsth.SelectedIndex==2 && textDateOriginalProsth.Text==""))
				{
					if(MessageBox.Show(Lan.g(this,"Prosthesis date not entered. Continue anyway?")
						,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
					{
						return false;
					}
				}
			}
			return true;
		}

		///<summary>MUST call EntriesAreValid first.  Used from OK_Click and from butSetComplete_Click</summary>
		private void SaveAndClose(){
			if(textProcFee.Text==""){
				textProcFee.Text="0";
			}
			ProcCur.PatNum=PatCur.PatNum;
			ProcCur.ADACode=this.textProc.Text;
			ProcCur.MedicalCode=textMedicalCode.Text;
			ProcCur.DiagnosticCode=textDiagnosticCode.Text;
			ProcCur.IsPrincDiag=checkIsPrincDiag.Checked;
			if(ProcOld.ProcStatus!=ProcStat.C && ProcCur.ProcStatus==ProcStat.C){
				ProcCur.DateEntryC=DateTime.Now;//this triggers it to set to server time NOW().
			}
			ProcCur.ProcDate=PIn.PDate(this.textDate.Text);
			ProcCur.ProcFee=PIn.PDouble(textProcFee.Text);
			//ProcCur.LabFee=PIn.PDouble(textLabFee.Text);
			//ProcCur.LabProcCode=textLabCode.Text;
			//MessageBox.Show(ProcCur.ProcFee.ToString());
			//Dx taken care of when radio pushed
			switch(ProcedureCode2.TreatArea){
				case TreatmentArea.Surf:
					ProcCur.Surf=textSurfaces.Text;
					ProcCur.ToothNum=Tooth.FromInternat(textTooth.Text);
					break;
				case TreatmentArea.Tooth:
					ProcCur.Surf="";
					ProcCur.ToothNum=Tooth.FromInternat(textTooth.Text);
					break;
				case TreatmentArea.Mouth:
					ProcCur.Surf="";
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.Quad:
					//surf set when radio pushed
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.Sextant:
					//surf taken care of when radio pushed
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.Arch:
					//don't HAVE to select arch
					//taken care of when radio pushed
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.ToothRange:
					if (listBoxTeeth.SelectedItems.Count<1 && listBoxTeeth2.SelectedItems.Count<1) {
						MessageBox.Show(Lan.g(this,"Must pick at least 1 tooth"));
						return;
					}
          string range="";
		      for(int j=0;j<listBoxTeeth.SelectedItems.Count;j++){
						if(j!=0)
							range+=",";
            range+=Tooth.FromInternat(listBoxTeeth.SelectedItems[j].ToString());
          }
		      for(int j=0;j<listBoxTeeth2.SelectedItems.Count;j++){
						if(j!=0)
							range+=",";
            range+=Tooth.FromInternat(listBoxTeeth2.SelectedItems[j].ToString());
          }
			    ProcCur.ToothRange=range;
					ProcCur.Surf="";
					ProcCur.ToothNum="";	
					break;
			}
			//Status taken care of when list pushed
			ProcCur.Note=this.textNotes.Text;
			SaveSignature();
			if(comboProvNum.SelectedIndex!=-1)
				ProcCur.ProvNum=Providers.List[comboProvNum.SelectedIndex].ProvNum;
			if(comboDx.SelectedIndex!=-1)
				ProcCur.Dx=DefB.Short[(int)DefCat.Diagnosis][comboDx.SelectedIndex].DefNum;
			if(comboPriority.SelectedIndex==0)
				ProcCur.Priority=0;
			else
				ProcCur.Priority=DefB.Short[(int)DefCat.TxPriorities][comboPriority.SelectedIndex-1].DefNum;
			ProcCur.PlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			if(comboClinic.SelectedIndex==0){
				ProcCur.ClinicNum=0;
			}
			else{
				ProcCur.ClinicNum=Clinics.List[comboClinic.SelectedIndex-1].ClinicNum;
			}
			//ProcCur.HideGraphical=checkHideGraphical.Checked;
			if(ProcedureCode2.IsProsth){
				switch(listProsth.SelectedIndex){
					case 0:
						ProcCur.Prosthesis="";
						break;
					case 1:
						ProcCur.Prosthesis="I";
						break;
					case 2:
						ProcCur.Prosthesis="R";
						break;
				}
				ProcCur.DateOriginalProsth=PIn.PDate(textDateOriginalProsth.Text);
			}
			else{
				ProcCur.Prosthesis="";
				ProcCur.DateOriginalProsth=DateTime.MinValue;
			}
			ProcCur.ClaimNote=textClaimNote.Text;
			Procedures.Update(ProcCur,ProcOld);
			Recalls.Synch(ProcCur.PatNum);
			if(IsNew){
				if(ProcOld.ProcStatus!=ProcStat.C && ProcCur.ProcStatus==ProcStat.C){
					//if status was changed to complete
					SecurityLogs.MakeLogEntry(Permissions.ProcComplCreate,PatCur.PatNum,
						PatCur.GetNameLF()+", "+ProcCur.ADACode+", "
						+ProcCur.ProcFee.ToString("c"));
				}
			}
			else{
				if(ProcOld.ProcStatus==ProcStat.C){
					SecurityLogs.MakeLogEntry(Permissions.ProcComplEdit,PatCur.PatNum,
						PatCur.GetNameLF()+", "+ProcCur.ADACode+", "
						+ProcCur.ProcFee.ToString("c"));
				}
			}
			if((ProcCur.ProcStatus==ProcStat.C || ProcCur.ProcStatus==ProcStat.EC || ProcCur.ProcStatus==ProcStat.EO)
				&& ProcedureCodes.GetProcCode(ProcCur.ADACode).PaintType==ToothPaintingType.Extraction) {
				//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(ProcCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			ProcOld=ProcCur.Copy();//in case we now make more changes.
			//these areas have no autocodes
			if(ProcedureCode2.TreatArea==TreatmentArea.Mouth
				|| ProcedureCode2.TreatArea==TreatmentArea.Quad
				|| ProcedureCode2.TreatArea==TreatmentArea.Sextant)
			{
				DialogResult=DialogResult.OK;
				return;
			}
			//this represents the suggested ADAcode based on the autocodes set up.
			string verifyADA;
			AutoCode AutoCodeCur=null;
			if(ProcedureCode2.TreatArea==TreatmentArea.Arch){
				if(ProcCur.Surf==""){
					DialogResult=DialogResult.OK;
					return;
				}
				if(ProcCur.Surf=="U"){
					verifyADA=AutoCodeItems.VerifyCode
						(ProcCur.ADACode,"1","",false,PatCur.PatNum,PatCur.Age,out AutoCodeCur);//max
				}
				else{
					verifyADA=AutoCodeItems.VerifyCode
						(ProcCur.ADACode,"32","",false,PatCur.PatNum,PatCur.Age,out AutoCodeCur);//mand
				}
			}
			else if(ProcedureCode2.TreatArea==TreatmentArea.ToothRange){
				//test for max or mand.
				if(listBoxTeeth.SelectedItems.Count<1)
					verifyADA=AutoCodeItems.VerifyCode
						(ProcCur.ADACode,"32","",false,PatCur.PatNum,PatCur.Age,out AutoCodeCur);//mand
				else
					verifyADA=AutoCodeItems.VerifyCode
						(ProcCur.ADACode,"1","",false,PatCur.PatNum,PatCur.Age,out AutoCodeCur);//max
			}
			else{//surf or tooth
				verifyADA=AutoCodeItems.VerifyCode
					(ProcCur.ADACode,ProcCur.ToothNum,ProcCur.Surf,false,PatCur.PatNum,PatCur.Age,out AutoCodeCur);
			}
			if(ProcCur.ADACode!=verifyADA){
				string desc=ProcedureCodes.GetProcCode(verifyADA).Descript;
				FormAutoCodeLessIntrusive FormA=new FormAutoCodeLessIntrusive();
				FormA.mainText=verifyADA+" ("+desc+") "+Lan.g(this,"is the recommended procedure code for this procedure.  Change procedure code and fee?");
				FormA.ShowDialog();
				if(FormA.DialogResult!=DialogResult.OK){
					DialogResult=DialogResult.OK;
					return;
				}
				if(FormA.CheckedBox){
					AutoCodeCur.LessIntrusive=true;
					AutoCodes.Update(AutoCodeCur);
					DataValid.SetInvalid(InvalidTypes.AutoCodes);
				}
				ProcCur.ADACode=verifyADA;
				ProcCur.ProcFee=Fees.GetAmount0(ProcCur.ADACode,Fees.GetFeeSched(PatCur,PlanList,PatPlanList));
				Procedures.Update(ProcCur,ProcOld);
				Recalls.Synch(ProcCur.PatNum);
				if(ProcCur.ProcStatus==ProcStat.C){
					SecurityLogs.MakeLogEntry(Permissions.ProcComplEdit,PatCur.PatNum,
						PatCur.GetNameLF()+", "+ProcCur.ADACode+", "
						+ProcCur.ProcFee.ToString("c"));
				}
			}
      DialogResult=DialogResult.OK;
			//it is assumed that we will do an immediate refresh after closing this window.
		}

		private void SaveSignature(){
			if(SigChanged){
				if(sigBoxTopaz.Visible){
					ProcCur.SigIsTopaz=true;
					if(sigBoxTopaz.NumberOfTabletPoints()==0){
						ProcCur.Signature="";
						return;
					}
					sigBoxTopaz.SetSigCompressionMode(0);
					sigBoxTopaz.SetEncryptionMode(0);
					sigBoxTopaz.SetKeyString("0000000000000000");
					sigBoxTopaz.SetAutoKeyData(ProcCur.Note+ProcCur.UserNum.ToString());
					sigBoxTopaz.SetEncryptionMode(2);
					sigBoxTopaz.SetSigCompressionMode(2);
					ProcCur.Signature=sigBoxTopaz.GetSigString();
				}
				else{
					ProcCur.SigIsTopaz=false;
					if(sigBox.NumberOfTabletPoints()==0) {
						ProcCur.Signature="";
						return;
					}
					//sigBox.SetSigCompressionMode(0);
					//sigBox.SetEncryptionMode(0);
					sigBox.SetKeyString("0000000000000000");
					sigBox.SetAutoKeyData(ProcCur.Note+ProcCur.UserNum.ToString());
					//sigBox.SetEncryptionMode(2);
					//sigBox.SetSigCompressionMode(2);
					ProcCur.Signature=sigBox.GetSigString();
				}
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProcEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK){
				//this catches date,prov,fee,status,etc for all claimProcs attached to this proc:
				Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ClaimProcList,false,PlanList,PatPlanList,BenefitList);
				return;
			}
			if(IsNew){//if cancelling on a new procedure
				//delete any newly created claimprocs
				for(int i=0;i<ClaimProcList.Length;i++){
					if(ClaimProcList[i].ProcNum==ProcCur.ProcNum){
						ClaimProcs.Delete(ClaimProcList[i]);
					}
				}
			}
		}

		

		
		

		

		


		

		//private void richTextBox1_TextChanged(object sender, System.EventArgs e) {
		//	textBox1.Text=richTextBox1.Rtf;
		//}

	
		

		

		

		

		

	

		

		

		

		

	

		

		

		
		

		
		

	}
}
