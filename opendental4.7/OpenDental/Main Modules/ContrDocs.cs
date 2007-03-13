/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
//#define ISXP
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text; 
using System.Windows.Forms;
//using WIALib;
using OpenDental.UI;
using OpenDentBusiness;
using Tao.OpenGl;
using CodeBase;
//using xImageDeviceManager;

namespace OpenDental{

	///<summary></summary>
	public class ContrDocs : System.Windows.Forms.UserControl	{
		private System.Windows.Forms.ImageList imageListTree;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageListTools2;
		private System.Windows.Forms.PrintDialog PrintDialog1;
		private System.Drawing.Printing.PrintDocument PrintDocument2;
		private System.Windows.Forms.TreeView TreeDocuments;
		///<summary>When dragging on Picturebox, this is the starting point in PictureBox coordinates.</summary>
		private Point MouseDownOrigin;
		private bool MouseIsDown;
		private System.Drawing.Bitmap ImageCurrent=null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuPrefs;
    //private FormDocInfo formDocInfo2;
		///<summary>The path to the patient folder, including the letter folder, and ending with \.  It's public for NewPatientForm.com functionality.</summary>
		public string patFolder;
		private OpenDental.UI.ODToolBar ToolBarMain;
		//private string imageFileName;
		///<summary>Starts out as false. It's only used when repainting the toolbar, not to test mode.</summary>
		private bool IsCropMode;//
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.ContextMenu contextTree;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.ContextMenu menuPatient;
		private Panel panelNote;
		private Label label1;
		private TextBox textNote;
		private SignatureBox sigBox;
		private Label label15;
		private Label labelInvalidSig;
		private Timer timer1;
		private ContrWindowingSlider brightnessContrastSlider;
		private ODToolBar paintTools;
		private Panel paintToolsUnderline;
		private Panel panel1;
		private System.Windows.Forms.PictureBox PictureBox1;
		///<summary></summary>
		[Category("Data"),Description("Occurs when user changes current patient, usually by clicking on the Select Patient button.")]
		public event PatientSelectedEventHandler PatientSelected=null;
		// declarations, spk 10/05/04
		///<summary></summary>
		[System.Runtime.InteropServices.DllImport("EZTW32.DLL")]
		public static extern int TWAIN_AcquireToFilename(IntPtr hwndApp, string bmpFileName); 
		///<summary></summary>
		[System.Runtime.InteropServices.DllImport("EZTW32.DLL")] 
		public static extern int TWAIN_SelectImageSource(IntPtr hwndApp); 
		///<summary></summary>
		[System.Runtime.InteropServices.DllImport("EZTW32.DLL")] 
		public static extern int TWAIN_AcquireToClipboard(IntPtr hwndApp, long wPixTypes); 
		///<summary></summary>
		[System.Runtime.InteropServices.DllImport("EZTW32.DLL")] 
		public static extern int TWAIN_IsAvailable(); 
		///<summary></summary>
		[System.Runtime.InteropServices.DllImport("EZTW32.DLL")] 
		public static extern int TWAIN_EasyVersion();// spk 10/05/04
		///<summary>The only reason this is public is for NewPatientForm.com functionality.</summary>
		public Patient PatCur;
		private Family FamCur;
		private ContextMenu menuForms;
		///<summary>The offset of the image due to the grab tool. Used as a basis for calculating imageTranslation.</summary>
		PointF imageLocation=new PointF(0,0);
		///<summary>The true offset of the image in screen-space.</summary>
		PointF imageTranslation=new PointF(0,0);
		///<summary>The current zoom of the image. 1 implies normal size, <1 implies the image is shrunk, >1 imples the image is blown-up.</summary>
		float imageZoom=1.0f;
		///<summary>The current amount. The zoomLevel is 0 after an image is loaded. The image is zoomed a factor of (initial image zoom)*(2^zoomLevel)</summary>
		int zoomLevel=0;
		///<summary>Represents the current factor for level of zoom from the initial zoom of the image. This is calculated directly as 2^zoomLevel every time a zoom occurs. Recalculated from zoomLevel each time, so that zoomFactor always hits the exact same values for the exact same zoom levels (not loss of data).</summary>
		float zoomFactor=1;
		Bitmap backBuffer=null;
		Graphics backBuffGraph=null;
		///<summary>Represents the ImageCurrent image after the document settings have been applied.</summary>
		Bitmap renderImage=null;
		Rectangle cropTangle=new Rectangle(0,0,-1,-1);
		const int thumbSize=100;
		Point mouseLocation=new Point(0,0);
		bool movingItem=false;
		TreeNode dragItem=null;
		TreeNode contextNode=null;
		Document previousImageSettings=null;
		string prevImageFile="";
		bool signing=false;
		//DeviceControl xRayImageController=null;
		System.Threading.Thread imageSettingApplicationThread=null;
		///<summary>Used to prevent concurrent access to the current image by multiple threads.</summary>
		int curImageWidth=0;
		///<summary>Used to prevent concurrent access to the current image by multiple threads.</summary>
		int curImageHeight=0;

		///<summary></summary>
		public ContrDocs(){
			InitializeComponent();
			//We always capture with a Suni device for now.
			//TODO: In the future use a device locator in the xImagingDeviceManager
			//project to return the appropriate general device control.
			//xRayImageController=new SuniDeviceControl();
			//this.xRayImageController.OnCaptureComplete+=new System.EventHandler(this.OnCaptureComplete);
			//this.xRayImageController.OnCaptureAbort+=new System.EventHandler(this.OnCaptureAborted);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrDocs));
			this.TreeDocuments = new System.Windows.Forms.TreeView();
			this.contextTree = new System.Windows.Forms.ContextMenu();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.imageListTree = new System.Windows.Forms.ImageList(this.components);
			this.PrintDialog1 = new System.Windows.Forms.PrintDialog();
			this.PrintDocument2 = new System.Drawing.Printing.PrintDocument();
			this.imageListTools2 = new System.Windows.Forms.ImageList(this.components);
			this.PictureBox1 = new System.Windows.Forms.PictureBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuPrefs = new System.Windows.Forms.MenuItem();
			this.menuPatient = new System.Windows.Forms.ContextMenu();
			this.panelNote = new System.Windows.Forms.Panel();
			this.labelInvalidSig = new System.Windows.Forms.Label();
			this.sigBox = new OpenDental.UI.SignatureBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.paintToolsUnderline = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.paintTools = new OpenDental.UI.ODToolBar();
			this.brightnessContrastSlider = new OpenDental.UI.ContrWindowingSlider();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
			this.panelNote.SuspendLayout();
			this.SuspendLayout();
			// 
			// TreeDocuments
			// 
			this.TreeDocuments.ContextMenu = this.contextTree;
			this.TreeDocuments.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.TreeDocuments.FullRowSelect = true;
			this.TreeDocuments.HideSelection = false;
			this.TreeDocuments.ImageIndex = 2;
			this.TreeDocuments.ImageList = this.imageListTree;
			this.TreeDocuments.Indent = 20;
			this.TreeDocuments.Location = new System.Drawing.Point(0,33);
			this.TreeDocuments.Name = "TreeDocuments";
			this.TreeDocuments.SelectedImageIndex = 2;
			this.TreeDocuments.Size = new System.Drawing.Size(228,519);
			this.TreeDocuments.TabIndex = 0;
			this.TreeDocuments.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeDocuments_AfterSelect);
			this.TreeDocuments.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeDocuments_MouseUp);
			this.TreeDocuments.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeDocuments_MouseMove);
			this.TreeDocuments.MouseHover += new System.EventHandler(this.TreeDocuments_MouseHover);
			this.TreeDocuments.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeDocuments_ItemDrag);
			// 
			// contextTree
			// 
			this.contextTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Print";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "Delete";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "Info";
			// 
			// imageListTree
			// 
			this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
			this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTree.Images.SetKeyName(0,"");
			this.imageListTree.Images.SetKeyName(1,"");
			this.imageListTree.Images.SetKeyName(2,"");
			this.imageListTree.Images.SetKeyName(3,"");
			this.imageListTree.Images.SetKeyName(4,"");
			this.imageListTree.Images.SetKeyName(5,"");
			// 
			// PrintDialog1
			// 
			this.PrintDialog1.AllowPrintToFile = false;
			this.PrintDialog1.Document = this.PrintDocument2;
			// 
			// PrintDocument2
			// 
			this.PrintDocument2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument2_PrintPage);
			// 
			// imageListTools2
			// 
			this.imageListTools2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTools2.ImageStream")));
			this.imageListTools2.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTools2.Images.SetKeyName(0,"");
			this.imageListTools2.Images.SetKeyName(1,"");
			this.imageListTools2.Images.SetKeyName(2,"");
			this.imageListTools2.Images.SetKeyName(3,"");
			this.imageListTools2.Images.SetKeyName(4,"");
			this.imageListTools2.Images.SetKeyName(5,"");
			this.imageListTools2.Images.SetKeyName(6,"");
			this.imageListTools2.Images.SetKeyName(7,"");
			this.imageListTools2.Images.SetKeyName(8,"");
			this.imageListTools2.Images.SetKeyName(9,"");
			this.imageListTools2.Images.SetKeyName(10,"");
			this.imageListTools2.Images.SetKeyName(11,"");
			this.imageListTools2.Images.SetKeyName(12,"");
			this.imageListTools2.Images.SetKeyName(13,"");
			this.imageListTools2.Images.SetKeyName(14,"");
			this.imageListTools2.Images.SetKeyName(15,"");
			this.imageListTools2.Images.SetKeyName(16,"");
			this.imageListTools2.Images.SetKeyName(17,"copy.gif");
			// 
			// PictureBox1
			// 
			this.PictureBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
			this.PictureBox1.BackColor = System.Drawing.SystemColors.Window;
			this.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PictureBox1.InitialImage = null;
			this.PictureBox1.Location = new System.Drawing.Point(233,63);
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.Size = new System.Drawing.Size(703,370);
			this.PictureBox1.TabIndex = 6;
			this.PictureBox1.TabStop = false;
			this.PictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
			this.PictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
			this.PictureBox1.MouseHover += new System.EventHandler(this.PictureBox1_MouseHover);
			this.PictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
			this.PictureBox1.SizeChanged += new System.EventHandler(this.PictureBox1_SizeChanged);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuPrefs});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuExit});
			this.menuItem1.Text = "File";
			// 
			// menuExit
			// 
			this.menuExit.Index = 0;
			this.menuExit.Text = "Exit";
			// 
			// menuPrefs
			// 
			this.menuPrefs.Index = 1;
			this.menuPrefs.Text = "Preferences";
			// 
			// panelNote
			// 
			this.panelNote.Controls.Add(this.labelInvalidSig);
			this.panelNote.Controls.Add(this.sigBox);
			this.panelNote.Controls.Add(this.label15);
			this.panelNote.Controls.Add(this.label1);
			this.panelNote.Controls.Add(this.textNote);
			this.panelNote.Location = new System.Drawing.Point(234,489);
			this.panelNote.Name = "panelNote";
			this.panelNote.Size = new System.Drawing.Size(705,64);
			this.panelNote.TabIndex = 11;
			this.panelNote.Visible = false;
			this.panelNote.DoubleClick += new System.EventHandler(this.panelNote_DoubleClick);
			// 
			// labelInvalidSig
			// 
			this.labelInvalidSig.BackColor = System.Drawing.SystemColors.Window;
			this.labelInvalidSig.Font = new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.labelInvalidSig.Location = new System.Drawing.Point(414,35);
			this.labelInvalidSig.Name = "labelInvalidSig";
			this.labelInvalidSig.Size = new System.Drawing.Size(196,59);
			this.labelInvalidSig.TabIndex = 94;
			this.labelInvalidSig.Text = "Invalid Signature -  Document or note has changed since it was signed.";
			this.labelInvalidSig.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelInvalidSig.DoubleClick += new System.EventHandler(this.labelInvalidSig_DoubleClick);
			// 
			// sigBox
			// 
			this.sigBox.Location = new System.Drawing.Point(308,20);
			this.sigBox.Name = "sigBox";
			this.sigBox.Size = new System.Drawing.Size(394,91);
			this.sigBox.TabIndex = 90;
			this.sigBox.DoubleClick += new System.EventHandler(this.sigBox_DoubleClick);
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(305,0);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(63,18);
			this.label15.TabIndex = 87;
			this.label15.Text = "Signature";
			this.label15.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label15.DoubleClick += new System.EventHandler(this.label15_DoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0,0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38,18);
			this.label1.TabIndex = 1;
			this.label1.Text = "Note";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.label1.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
			// 
			// textNote
			// 
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.Location = new System.Drawing.Point(0,20);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.ReadOnly = true;
			this.textNote.Size = new System.Drawing.Size(302,91);
			this.textNote.TabIndex = 0;
			this.textNote.DoubleClick += new System.EventHandler(this.textNote_DoubleClick);
			this.textNote.MouseHover += new System.EventHandler(this.textNote_MouseHover);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 20;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// paintToolsUnderline
			// 
			this.paintToolsUnderline.BackColor = System.Drawing.SystemColors.ControlDark;
			this.paintToolsUnderline.Location = new System.Drawing.Point(236,56);
			this.paintToolsUnderline.Name = "paintToolsUnderline";
			this.paintToolsUnderline.Size = new System.Drawing.Size(702,2);
			this.paintToolsUnderline.TabIndex = 15;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Location = new System.Drawing.Point(233,29);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(2,29);
			this.panel1.TabIndex = 16;
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListTools2;
			this.ToolBarMain.Location = new System.Drawing.Point(0,0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(939,29);
			this.ToolBarMain.TabIndex = 10;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// paintTools
			// 
			this.paintTools.ImageList = this.imageListTools2;
			this.paintTools.Location = new System.Drawing.Point(440,28);
			this.paintTools.Name = "paintTools";
			this.paintTools.Size = new System.Drawing.Size(499,29);
			this.paintTools.TabIndex = 14;
			this.paintTools.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.paintTools_ButtonClick);
			// 
			// brightnessContrastSlider
			// 
			this.brightnessContrastSlider.Enabled = false;
			this.brightnessContrastSlider.Location = new System.Drawing.Point(240,36);
			this.brightnessContrastSlider.MaxVal = 255;
			this.brightnessContrastSlider.MinVal = 0;
			this.brightnessContrastSlider.Name = "brightnessContrastSlider";
			this.brightnessContrastSlider.Size = new System.Drawing.Size(194,14);
			this.brightnessContrastSlider.TabIndex = 12;
			this.brightnessContrastSlider.Text = "contrWindowingSlider1";
			this.brightnessContrastSlider.Scroll += new System.EventHandler(this.brightnessContrastSlider_Scroll);
			this.brightnessContrastSlider.ScrollComplete += new System.EventHandler(this.brightnessContrastSlider_ScrollComplete);
			// 
			// ContrDocs
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.paintToolsUnderline);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.paintTools);
			this.Controls.Add(this.brightnessContrastSlider);
			this.Controls.Add(this.panelNote);
			this.Controls.Add(this.PictureBox1);
			this.Controls.Add(this.TreeDocuments);
			this.Name = "ContrDocs";
			this.Size = new System.Drawing.Size(939,606);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ContrDocs_MouseMove);
			this.Resize += new System.EventHandler(this.ContrDocs_Resize);
			((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
			this.panelNote.ResumeLayout(false);
			this.panelNote.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void ContrDocs_Resize(object sender,EventArgs e) {
			ResizeAll();
		}

		private void ResizeAll(){
			TreeDocuments.Height=Height-TreeDocuments.Location.Y-2;
			PictureBox1.Width=Width-PictureBox1.Location.X-4;
			panelNote.Width=PictureBox1.Width;
			panelNote.Height=(int)Math.Min(114,Height-PictureBox1.Location.Y);
			int panelNoteHeight=(panelNote.Visible?panelNote.Height:0);
			PictureBox1.Height=Height-panelNoteHeight-PictureBox1.Location.Y;
			panelNote.Location=new Point(PictureBox1.Left,Height-panelNoteHeight-1);
			paintTools.Location=new Point(brightnessContrastSlider.Location.X+brightnessContrastSlider.Width+4,
				paintTools.Location.Y);
			paintTools.Size=new Size(PictureBox1.Width-brightnessContrastSlider.Width-4,paintTools.Height);
			paintToolsUnderline.Location=new Point(PictureBox1.Location.X,paintToolsUnderline.Location.Y);
			paintToolsUnderline.Width=Width-paintToolsUnderline.Location.X;
		}

		///<summary></summary>
		public void InstantClasses(){
			MouseDownOrigin=new Point();
			Lan.C(this, new System.Windows.Forms.Control[] {
				//this.button1,
			});
			LayoutToolBar();
			contextTree.MenuItems.Clear();
			contextTree.MenuItems.Add("Print",new System.EventHandler(menuTree_Click));
			contextTree.MenuItems.Add("Delete",new System.EventHandler(menuTree_Click));
			contextTree.MenuItems.Add("Info",new System.EventHandler(menuTree_Click));
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			paintTools.Buttons.Clear();
			ODToolBarButton button;
			button=new ODToolBarButton(Lan.g(this,"Select Patient"),0,"","Patient");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=menuPatient;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",1,Lan.g(this,"Print"),"Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",2,Lan.g(this,"Delete"),"Delete"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",3,Lan.g(this,"Item Info"),"Info"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Sign"),-1,Lan.g(this,"Sign this document"),"Sign"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Scan:"),-1,"","");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton("",14,Lan.g(this,"Scan Document"),"ScanDoc"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",16,Lan.g(this,"Scan Radiograph"),"ScanXRay"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",15,Lan.g(this,"Scan Photo"),"ScanPhoto"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Import"),5,Lan.g(this,"Import From File"),"Import"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Copy"),17,Lan.g(this,"Copy displayed image to clipboard"),"Copy"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Paste"),6,Lan.g(this,"Paste From Clipboard"),"Paste"));
			button=new ODToolBarButton(Lan.g(this,"Forms"),-1,"","Forms");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			menuForms=new ContextMenu();
			if(Directory.Exists(PrefB.GetString("DocPath")+"Forms")){
				DirectoryInfo dirInfo=new DirectoryInfo(PrefB.GetString("DocPath")+"Forms");
				FileInfo[] fileInfos=dirInfo.GetFiles();
				for(int i=0;i<fileInfos.Length;i++){
					if(IsAcceptableFileName(fileInfos[i].FullName)){
						menuForms.MenuItems.Add(fileInfos[i].Name,new System.EventHandler(menuForms_Click));
					}
				}
			}
			button.DropDownMenu=menuForms;
			ToolBarMain.Buttons.Add(button);
			/*button=new ODToolBarButton(Lan.g(this,"Capture"),-1,"Capture Image From Device","Capture");
			button.Style=ODToolBarButtonStyle.ToggleButton;
			ToolBarMain.Buttons.Add(button);*/
			button=new ODToolBarButton("",7,Lan.g(this,"Crop Tool"),"Crop");
			button.Style=ODToolBarButtonStyle.ToggleButton;
			if(IsCropMode){
				button.Pushed=true;
			}
			paintTools.Buttons.Add(button);
			button=new ODToolBarButton("",10,Lan.g(this,"Hand Tool"),"Hand");
			button.Style=ODToolBarButtonStyle.ToggleButton;
			if(!IsCropMode){
				button.Pushed=true;
			}
			paintTools.Buttons.Add(button);
			paintTools.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			paintTools.Buttons.Add(new ODToolBarButton("",8,Lan.g(this,"Zoom In"),"ZoomIn"));
			paintTools.Buttons.Add(new ODToolBarButton("",9,Lan.g(this,"Zoom Out"),"ZoomOut"));
			paintTools.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Rotate:"),-1,"","");
			button.Style=ODToolBarButtonStyle.Label;
			paintTools.Buttons.Add(button);
			paintTools.Buttons.Add(new ODToolBarButton("",11,Lan.g(this,"Flip"),"Flip"));
			paintTools.Buttons.Add(new ODToolBarButton("",12,Lan.g(this,"Rotate Left"),"RotateL"));
			paintTools.Buttons.Add(new ODToolBarButton("",13,Lan.g(this,"Rotate Right"),"RotateR"));

			ArrayList toolButItems=ToolButItems.GetForToolBar(ToolBarsAvail.ImagesModule);
			for(int i=0;i<toolButItems.Count;i++){
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				ToolBarMain.Buttons.Add(new ODToolBarButton(((ToolButItem)toolButItems[i]).ButtonText
					,-1,"",((ToolButItem)toolButItems[i]).ProgramNum));
			}
			ToolBarMain.Invalidate();
			paintTools.Invalidate();
		}

		///<summary></summary>
		public void ModuleSelected(int patNum){
			RefreshModuleData(patNum);
			RefreshModuleScreen();
		}

		///<summary></summary>
		public void ModuleUnselected(){
			FamCur=null;
			PatCur=null;
			//Cancel current image capture by manually untoggling the capture button.
			//ToolBarMain.Buttons["Capture"].Pushed=false;//TODO: uncomment this line when XRay captures are supported.
			//OnCapture_Click();//TODO: uncomment when XRay capture is supported.
		}

		///<summary>This is public for NewPatientForm functionality.</summary>
  	public void RefreshModuleData(int patNum){
			if(patNum==0){
				PatCur=null;
				FamCur=null;
				return;
			}
			FamCur=Patients.GetFamily(patNum);
			PatCur=FamCur.GetPatient(patNum);
			if(ParentForm != null){ //Added so NewPatientform can have access without showing
				ParentForm.Text=Patients.GetMainTitle(PatCur);
			}
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
					Patient PatOld=PatCur.Copy();
					PatCur.ImageFolder=folder;
					patFolder=((Pref)PrefB.HList["DocPath"]).ValueString
						+PatCur.ImageFolder.Substring(0,1)+@"\"
						+PatCur.ImageFolder+@"\";
					Directory.CreateDirectory(patFolder);
					Patients.Update(PatCur,PatOld);
				}
				catch{
					MessageBox.Show(Lan.g(this,"Error.  Could not create folder for patient. "));
					return;
				}
			}
			else{//patient folder already created once
				patFolder=PrefB.GetString("DocPath")
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
					return;
				}
			}
			//now find all files in the patient folder that are not in the db and add them
			List<DocAttach> DocAttachList=DocAttaches.Refresh(PatCur.PatNum);
			Document[] DocumentList=Documents.Refresh(DocAttachList);
			DirectoryInfo di=new DirectoryInfo(patFolder);
			FileInfo[] fiList=di.GetFiles();
			int countAdded=0;
			string[] usedNames=new string[DocumentList.Length];
			for(int i=0;i<DocumentList.Length;i++) {
				usedNames[i]=DocumentList[i].FileName;
			}
			for(int i=0;i<fiList.Length;i++){
				if(!DocumentB.IsFileNameInList(fiList[i].Name,usedNames)
					&& IsAcceptableFileName(fiList[i].Name)){
					Document doc=new Document();
					doc.DateCreated=DateTime.Today;
					doc.Description=fiList[i].Name;
					doc.DocCategory=DefB.Short[(int)DefCat.ImageCats][0].DefNum;
					doc.FileName=fiList[i].Name;
					doc.WithPat=PatCur.PatNum;
					Documents.Insert(doc,PatCur);
					countAdded++;
				}
			}
			if(countAdded>0){
				MessageBox.Show(countAdded.ToString()+" documents found and added to the first category.");
			}
			//it will refresh in FillDocList																					 
		}

		private void RefreshModuleScreen(){
			ParentForm.Text=Patients.GetMainTitle(PatCur);
			if(PatCur!=null){
				//ParentForm.Text=((Pref)PrefB.HList["MainWindowTitle"]).ValueString+" - "
				//	+PatCur.GetNameLF();
				ToolBarMain.Buttons["Print"].Enabled=true;
				ToolBarMain.Buttons["Delete"].Enabled=true;
				ToolBarMain.Buttons["Info"].Enabled=true;
				ToolBarMain.Buttons["Import"].Enabled=true;
				ToolBarMain.Buttons["ScanDoc"].Enabled=true;
				ToolBarMain.Buttons["ScanXRay"].Enabled=true;
				ToolBarMain.Buttons["ScanPhoto"].Enabled=true;
				ToolBarMain.Buttons["Copy"].Enabled=true;
				ToolBarMain.Buttons["Paste"].Enabled=true;
				ToolBarMain.Buttons["Forms"].Enabled=true;
				//ToolBarMain.Buttons["Capture"].Enabled=true;//TODO: uncomment this line when XRay captures are supported.
				paintTools.Buttons["Crop"].Enabled=true;
				paintTools.Buttons["Hand"].Enabled=true;
				paintTools.Buttons["ZoomIn"].Enabled=true;
				paintTools.Buttons["ZoomOut"].Enabled=true;
				paintTools.Buttons["Flip"].Enabled=true;
				paintTools.Buttons["RotateR"].Enabled=true;
				paintTools.Buttons["RotateL"].Enabled=true;
			}
			else{
				//ParentForm.Text=((Pref)PrefB.HList["MainWindowTitle"]).ValueString;
				//PatCur=new Patient();
				ToolBarMain.Buttons["Print"].Enabled=false;
				ToolBarMain.Buttons["Delete"].Enabled=false;
				ToolBarMain.Buttons["Info"].Enabled=false;
				ToolBarMain.Buttons["Import"].Enabled=false;
				ToolBarMain.Buttons["ScanDoc"].Enabled=false;
				ToolBarMain.Buttons["ScanXRay"].Enabled=false;
				ToolBarMain.Buttons["ScanPhoto"].Enabled=false;
				ToolBarMain.Buttons["Copy"].Enabled=false;
				ToolBarMain.Buttons["Paste"].Enabled=false;
				ToolBarMain.Buttons["Forms"].Enabled=false;
				//ToolBarMain.Buttons["Capture"].Enabled=false;//TODO: uncomment this line when XRay captures are supported.
				paintTools.Buttons["Crop"].Enabled=false;
				paintTools.Buttons["Hand"].Enabled=false;
				paintTools.Buttons["ZoomIn"].Enabled=false;
				paintTools.Buttons["ZoomOut"].Enabled=false;
				paintTools.Buttons["Flip"].Enabled=false;
				paintTools.Buttons["RotateR"].Enabled=false;
				paintTools.Buttons["RotateL"].Enabled=false;
			}
			FillPatientButton();
			ToolBarMain.Invalidate();
			paintTools.Invalidate();
			FillDocList("",false);
		}

		private void FillPatientButton(){
			Patients.AddPatsToMenu(menuPatient,new EventHandler(menuPatient_Click),PatCur,FamCur);
		}

		private void menuPatient_Click(object sender,System.EventArgs e) {
			int newPatNum=Patients.ButtonSelect(menuPatient,sender,FamCur);
			OnPatientSelected(newPatNum);
			ModuleSelected(newPatNum);
		}

		///<summary></summary>
		private void OnPatientSelected(int patNum){
			PatientSelectedEventArgs eArgs=new OpenDental.PatientSelectedEventArgs(patNum);
			if(PatientSelected!=null)
				PatientSelected(this,eArgs);
		}

		private TreeNode GetTreeDocumentNode(string docNum) {
			return GetTreeDocumentNode(docNum,TreeDocuments.Nodes);//This defines the root node.
		}

		///<summary>Searches the current file tree for a document row which has the given unique document number. This will work for a tree with any number of nested folders, as long as tags are defined only for items which correspond to data rows.</summary>
		private TreeNode GetTreeDocumentNode(string docNum, TreeNodeCollection rootNodes){
			if(docNum!=null && rootNodes!=null){
				foreach(TreeNode n in rootNodes){
					if(n!=null){
						Document nodeDoc=GetDocumentFromNode(n);
						if(nodeDoc!=null && nodeDoc.DocNum.ToString()==docNum){
							return n;
						}else{//Check the child nodes.
							TreeNode child=GetTreeDocumentNode(docNum,n.Nodes);
							if(child!=null){
								return child;
							}
						}
					}
				}
			}
			return null;
		}

		private int DocumentTypeToImageListIndex(ImageType imageType){
			if(imageType==ImageType.File){
				return 5;
			}else if(imageType==ImageType.Radiograph){
				return 3;
			}else if(imageType==ImageType.Photo){
				return 4;
			}
			return 2;//document
		}

		private void AddTreeDocumentNodeAtIndex(Document doc,DataRow tag,bool selectNewNode,int index) {
			TreeNode addNode=new TreeNode(doc.DateCreated.ToString("d")+": "+doc.Description);
			int parentNode=DefB.GetOrder(DefCat.ImageCats,doc.DocCategory);
			parentNode=(parentNode>0?parentNode:0);
			TreeDocuments.Nodes[parentNode].Nodes.Insert(index,addNode);
			addNode.Tag=tag;
			addNode.ImageIndex=DocumentTypeToImageListIndex(doc.ImgType);
			addNode.SelectedImageIndex=addNode.ImageIndex;
			if(selectNewNode) {
				TreeDocuments.SelectedNode=addNode;
			}
		}

		///<summary>The document must have already been comitted to the database.</summary>
		private void AddTreeDocumentNode(Document doc,DataRow tag,bool selectNewNode){
			int parentNode=DefB.GetOrder(DefCat.ImageCats,doc.DocCategory);
			parentNode=(parentNode>0?parentNode:0);
			int i=TreeDocuments.Nodes[parentNode].Nodes.Count-1;
			while(i>=0 && //Order items by date in ascending order.
				Documents.Fill((DataRow)TreeDocuments.Nodes[parentNode].Nodes[i].Tag).DateCreated.CompareTo(doc.DateCreated)>0){
				i--;
			}
			AddTreeDocumentNodeAtIndex(doc,tag,selectNewNode,i+1);
		}

		private void DeleteTreeDocumentNode(TreeNode deleteNode){
			if(deleteNode==null || deleteNode.Parent==null){
				return;
			}
			deleteNode.Parent.Nodes.Remove(deleteNode);
		}

		private void MoveTreeDocumentNode(Document moveDoc,int newCategory,bool selectMovedNode){
			if(moveDoc==null){
				return;
			}
			TreeNode moveNode=GetTreeDocumentNode(moveDoc.DocNum.ToString());
			if(moveNode==null) {
				return;
			}
			string curFolder=GetCurrentFolderName(moveNode);
			string destFolder=DefB.GetName(DefCat.ImageCats,newCategory);
			DataRow tag=Documents.GetDocumentRow(moveDoc.DocNum.ToString());
			DeleteTreeDocumentNode(moveNode);
			//Only physically move the node in the tree if it is changing parent folders.
			if(curFolder!=destFolder){
				AddTreeDocumentNode(moveDoc,tag,selectMovedNode);
			}else{//Otherwise, simply update the document in the tree.
				AddTreeDocumentNodeAtIndex(moveDoc,tag,selectMovedNode,moveNode.Index);
			}
		}

		/// <summary>Refreshes list from db, then fills the treeview.  Set to an existing document number (as a string) to keep the current doc displayed, or set to "" to clear the current document.</summary>
		private void FillDocList(string docNum,bool verbose){
  	  string sNewNode;
			TreeDocuments.SelectedNode=null;
			TreeDocuments.Nodes.Clear();
			if(PatCur==null){
				return;
			}
			List<DocAttach> DocAttachList=DocAttaches.Refresh(PatCur.PatNum);
			if(DocAttachList==null){
				return;
			}
			DataTable documentTable=Documents.RefreshTable(DocAttachList);
			if(documentTable==null){
				return;
			}
			Document[] DocumentList=Documents.Fill(documentTable);
			if(DocumentList==null){
				return;
			}
			//Add all predefined folder names to the tree.
			for(int i=0;i<DefB.Short[(int)DefCat.ImageCats].Length;i++){
				sNewNode=DefB.Short[(int)DefCat.ImageCats][i].ItemName;
				TreeDocuments.Nodes.Add(new TreeNode(sNewNode));
				TreeDocuments.Nodes[i].SelectedImageIndex=1;
				TreeDocuments.Nodes[i].ImageIndex=1;
			}
			int hiddenDocCount=0;
			//Add all documents as stored in the database to the tree.
			for(int i=0;i<DocumentList.Length;i++){
				if(DefB.GetOrder(DefCat.ImageCats,DocumentList[i].DocCategory)!=-1) {//Don't add hidden documents.
					AddTreeDocumentNode(DocumentList[i],documentTable.Rows[i],false);
				}else{
					hiddenDocCount++;
				}
			}
			TreeDocuments.ExpandAll();
			TreeDocuments.SelectedNode=GetTreeDocumentNode(docNum);
			if(verbose){
				if(hiddenDocCount>0){
					MessageBox.Show("There are currently "+hiddenDocCount+" hidden documents for this patient that are not visible in the file list.");
				}
			}
		}

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				switch(e.Button.Tag.ToString()){
					case "Patient":
						OnPat_Click();
						break;
					case "Print":
						OnPrint_Click(TreeDocuments.SelectedNode);
						break;
					case "Delete":
						OnDelete_Click(TreeDocuments.SelectedNode,true);
						break;
					case "Info":
						OnInfo_Click(TreeDocuments.SelectedNode);
						break;
					case "Sign":
						OnSign_Click();
						break;
					case "ScanDoc":
						OnScan_Click("doc");
						break;
					case "ScanXRay":
						OnScan_Click("xray");
						break;
					case "ScanPhoto":
						OnScan_Click("photo");
						break;
					case "Import":
						OnImport_Click();
						break;
					case "Copy":
						OnCopy_Click();
						break;
					case "Paste":
						OnPaste_Click();
						break;
					case "Forms":
						MsgBox.Show(this,"Use the dropdown list.  Add forms to the list by copying image files into your A-Z folder, Forms.  Restart the program to see newly added forms.");
						break;
					case "Capture":
						//OnCapture_Click();//TODO: uncomment when XRay capture is supported.
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(int)){
				Programs.Execute((int)e.Button.Tag,PatCur);
			}
		}

		private void paintTools_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)) {
				switch(e.Button.Tag.ToString()) {
					case "Crop":
						OnCrop_Click();
						break;
					case "Hand":
						OnHand_Click();
						break;
					case "ZoomIn":
						OnZoomIn_Click();
						break;
					case "ZoomOut":
						OnZoomOut_Click();
						break;
					case "Flip":
						OnFlip_Click();
						break;
					case "RotateL":
						OnRotateL_Click();
						break;
					case "RotateR":
						OnRotateR_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(int)) {
				Programs.Execute((int)e.Button.Tag,PatCur);
			}
		}

		private void OnPat_Click() {
			FormPatientSelect formPS=new FormPatientSelect();
			formPS.ShowDialog();
			if(formPS.DialogResult==DialogResult.OK){
				OnPatientSelected(formPS.SelectedPatNum);
				ModuleSelected(formPS.SelectedPatNum);
			}
			FillDocList("",true);
		}

		private void OnPrint_Click(TreeNode printNode) {
			Document printDoc=GetDocumentFromNode(printNode);
			if(printDoc!=null){
				if(printNode!=TreeDocuments.SelectedNode){
					TreeDocuments.SelectedNode=printNode;//Loads the document so it can be viewed and printed.
				}
				try{
					if(PrintDialog1.ShowDialog()==DialogResult.OK){
						if(PrintDocument2.DefaultPageSettings.PaperSize.Width==0 ||
								PrintDocument2.DefaultPageSettings.PaperSize.Height==0) {
							PrintDocument2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
						}
						PrintDocument2.OriginAtMargins=true;
						PrintDocument2.DefaultPageSettings.Margins=new Margins(50,50,50,50);//Half-inch all around.
						PrintDocument2.Print();
					}
				}
				catch(System.Exception ex){
					MessageBox.Show(Lan.g(this,"An error occurred while printing"), ex.ToString());
				}
			}
		}

		///<summary>If the node does not correspond to a valid document, nothing happens. Otherwise the document record and its corresponding file are deleted. The function only reports errors or asks the user if they want to delete the file if verbose is set to true.</summary>
		private void OnDelete_Click(TreeNode deleteNode,bool verbose){
			if(deleteNode==null || deleteNode.Parent==null){
				return;
			}
			if(verbose){
				if(MessageBox.Show("Delete document '"+deleteNode.Text+"'?","",MessageBoxButtons.OKCancel)!=DialogResult.OK)
					return;
			}
			deleteNode.Parent.Nodes.Remove(deleteNode);
			Document deleteDoc=GetDocumentFromNode(deleteNode);
			if(deleteDoc!=null){
				try {
					TreeDocuments.SelectedNode=null;
					EraseCurrentImage();//Release access to current image so it may be properly deleted.
					string srcFile=patFolder+deleteDoc.FileName;
					if(File.Exists(srcFile)) {
						File.Delete(srcFile);
					}
				}catch{
					if(verbose) {
						MessageBox.Show(Lan.g(this,"Could not delete file. It may be in use elsewhere, or may have already been deleted."));
					}
				}
				Documents.Delete(deleteDoc);
			}
		}

		private void OnSign_Click(){
			Document curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
			if(curDoc==null){
				return;
			}
			signing=true;
			FormDocSign FormD=new FormDocSign(curDoc,patFolder);
			FormD.Location=PointToScreen(new Point(PictureBox1.Left,this.ClientRectangle.Bottom-FormD.Height));
			FormD.Width=PictureBox1.Width;
			FormD.ShowDialog();
			//Reload the document into the tree (since the row may have changed).
			MoveTreeDocumentNode(curDoc,curDoc.DocCategory,true);
			signing=false;
		}

		private void FillSignature() {
			Document curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
			if(curDoc==null) {
				return;
			}
			textNote.Text=curDoc.Note;
			//sigBoxTopaz.Location=sigBox.Location;//this puts both boxes in the same spot.
			//sigBoxTopaz.Visible=false;
			sigBox.SetTabletState(0);//never accepts input here
			//sigBoxTopaz.SetTabletState(0);
			labelInvalidSig.Visible=false;
			if(curDoc.SigIsTopaz) {
				if(curDoc.Signature!=null && curDoc.Signature!="") {
					/*sigBoxTopaz.Visible=true;
					sigBoxTopaz.ClearTablet();
					sigBoxTopaz.SetSigCompressionMode(0);
					sigBoxTopaz.SetEncryptionMode(0);
					sigBoxTopaz.SetKeyString(GetHashString(curDoc));
					sigBoxTopaz.SetEncryptionMode(2);//high encryption
					sigBoxTopaz.SetSigCompressionMode(2);//high compression
					sigBoxTopaz.SetSigString(curDoc.Signature);
					if(sigBoxTopaz.NumberOfTabletPoints()==0) {
						labelInvalidSig.Visible=true;
					}*/
				}
			}else{
				sigBox.ClearTablet();
				if(curDoc.Signature!=null && curDoc.Signature!="") {
					sigBox.SetKeyString(GetHashString(curDoc));
					sigBox.SetSigString(curDoc.Signature);
					if(sigBox.NumberOfTabletPoints()==0) {
						labelInvalidSig.Visible=true;
					}
					sigBox.SetTabletState(0);//not accepting input.
				}
			}
		}

		private string GetHashString(Document curDoc) {
			//the key data is the bytes of the file, concatenated with the bytes of the note.
			byte[] textbytes;
			if(curDoc.Note==null) {
				textbytes=Encoding.UTF8.GetBytes("");
			}
			else {
				textbytes=Encoding.UTF8.GetBytes(curDoc.Note);
			}
			string path=patFolder+curDoc.FileName;
			if(!File.Exists(path)) {
				return "";
			}
			FileStream fs=new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.Read);
			int fileLength=(int)fs.Length;
			byte[] buffer=new byte[fileLength+textbytes.Length];
			fs.Read(buffer,0,fileLength);
			fs.Close();
			Array.Copy(textbytes,0,buffer,fileLength,textbytes.Length);
			HashAlgorithm algorithm=MD5.Create();
			byte[] hash=algorithm.ComputeHash(buffer);//always results in length of 16.
			return Encoding.ASCII.GetString(hash);
		}

		///<summary>Valid values for scanType are "doc","xray",and "photo"</summary>
		private void OnScan_Click(string scanType) {
			//A user may have more than one scanning device. 
			//The code below will allow the user to select one.
			long wPIXTypes;
			IDataObject oDataObject;
			try{
				wPIXTypes=TWAIN_SelectImageSource(this.Handle);
				if(wPIXTypes==0) {//user clicked Cancel
					return;
				}
				this.Cursor=Cursors.WaitCursor;
				TWAIN_AcquireToClipboard(this.Handle,wPIXTypes);
				oDataObject=Clipboard.GetDataObject();
				if(!oDataObject.GetDataPresent(DataFormats.Bitmap,true) || !oDataObject.GetDataPresent(DataFormats.Dib,true)) {
					throw new Exception("Unknown image data format.");
				}
			}catch(Exception ex){
				MessageBox.Show("The image could not be acquired from the device. "+
					"Please check to see that the device is properly connected to the computer. Specific error: "+ex.Message);
				return;
			}
			Document doc=new Document();
			if(scanType=="doc") {
				doc.ImgType=ImageType.Document;
			}else if(scanType=="xray"){
				doc.ImgType=ImageType.Radiograph;
			}else if(scanType=="photo"){
				doc.ImgType=ImageType.Photo;
			}
			doc.FileName=".jpg";
			doc.DateCreated=DateTime.Today;
			doc.WithPat=PatCur.PatNum;
			Documents.Insert(doc,PatCur);//creates filename and saves to db
			bool saved=true;
			try{//Create corresponding file.
				ImageCodecInfo myImageCodecInfo;
				ImageCodecInfo[] encoders;
				encoders=ImageCodecInfo.GetImageEncoders();
				myImageCodecInfo=null;
				for(int j=0;j<encoders.Length;j++) {
					if(encoders[j].MimeType=="image/jpeg")
						myImageCodecInfo=encoders[j];
				}
				System.Drawing.Imaging.Encoder myEncoder=System.Drawing.Imaging.Encoder.Quality;
				EncoderParameters myEncoderParameters=new EncoderParameters(1);
				long qualityL=0;
				if(scanType=="doc"){
					//Possible values 0-100?
					qualityL=(long)Convert.ToInt32(((Pref)PrefB.HList["ScannerCompression"]).ValueString);
				}else if(scanType=="xray"){
					qualityL=Convert.ToInt64(((Pref)PrefB.HList["ScannerCompressionRadiographs"]).ValueString);
				}else if(scanType=="photo"){
					qualityL=Convert.ToInt64(((Pref)PrefB.HList["ScannerCompressionPhotos"]).ValueString);
				}
				EncoderParameter myEncoderParameter=new EncoderParameter(myEncoder,qualityL);
				myEncoderParameters.Param[0]=myEncoderParameter;
				Bitmap scannedImage=null;
				if(oDataObject.GetDataPresent(DataFormats.Bitmap,true)) {
					scannedImage=(Bitmap)oDataObject.GetData(DataFormats.Bitmap);
				}else{// if(oDataObject.GetDataPresent(DataFormats.Dib,true)) {
					scannedImage=(Bitmap)oDataObject.GetData(DataFormats.Dib);
				}
				//AutoCrop()?
				scannedImage.Save(patFolder+doc.FileName,myImageCodecInfo,myEncoderParameters);
			}catch{
				MessageBox.Show(Lan.g(this,"Unable to save document."));
				saved=false;
				Documents.Delete(doc);
			}
			if(saved){
				DataRow tag=Documents.GetDocumentRow(doc.DocNum.ToString());
				doc.DocCategory=DefB.GetByExactName(DefCat.ImageCats,GetCurrentFolderName(TreeDocuments.SelectedNode));
				AddTreeDocumentNode(doc,tag,true);
				FormDocInfo formDocInfo=new FormDocInfo(PatCur,doc,GetCurrentFolderName(TreeDocuments.SelectedNode));
				formDocInfo.ShowDialog();
				if(formDocInfo.DialogResult!=DialogResult.OK) {
					OnDelete_Click(GetTreeDocumentNode(doc.DocNum.ToString()),false);
				}else{
					MoveTreeDocumentNode(doc,doc.DocCategory,true);
				}
			}			
		}

		private void OnImport_Click() {
			OpenFileDialog openFileDialog=new OpenFileDialog();
			openFileDialog.Multiselect=true;
			if(openFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			string[] fileNames=openFileDialog.FileNames;
			if(fileNames.Length<1){
				return;
			}
			string docNum="";
			Document doc=null;
			for(int i=0;i<fileNames.Length;i++){
				openFileDialog.FileName=fileNames[i];
				doc=new Document();
				//Document.Insert will use this extension when naming:
				doc.FileName=Path.GetExtension(openFileDialog.FileName);
				doc.DateCreated=DateTime.Today;
				doc.WithPat=PatCur.PatNum;
				doc.ImgType=HasImageExtension(doc.FileName)?ImageType.Photo:ImageType.Document;
				Documents.Insert(doc,PatCur);//this assigns a filename and saves to db
				bool copied=true;
				try{
					File.Copy(openFileDialog.FileName,patFolder+doc.FileName);
				}catch(Exception ex){
					MessageBox.Show(Lan.g(this,"Unable to copy file, May be in use: ")+ex.Message+": "+openFileDialog.FileName);
					Documents.Delete(doc);
					copied=false;
				}
				if(copied){
					DataRow tag=Documents.GetDocumentRow(doc.DocNum.ToString());
					doc.DocCategory=DefB.GetByExactName(DefCat.ImageCats,GetCurrentFolderName(TreeDocuments.SelectedNode));
					AddTreeDocumentNode(doc,tag,true);
					int startCateogory=doc.DocCategory;
					FormDocInfo FormD=new FormDocInfo(PatCur,doc,GetCurrentFolderName(TreeDocuments.SelectedNode));
					FormD.ShowDialog();//some of the fields might get changed, but not the filename
					if(FormD.DialogResult!=DialogResult.OK){
						OnDelete_Click(GetTreeDocumentNode(doc.DocNum.ToString()),false);
					}else{
						docNum=doc.DocNum.ToString();
						MoveTreeDocumentNode(doc,doc.DocCategory,true);
					}
				}
			}
			//Reselect the last successfully added node when necessary.
			if(doc!=null && doc.DocNum.ToString()!=docNum){
				TreeDocuments.SelectedNode=GetTreeDocumentNode(docNum);
			}
		}

		private void OnCopy_Click(){
			this.Cursor=Cursors.WaitCursor;
			//Crop and color function have already been applied to the render image.
			Bitmap copyImage=ApplyDocumentSettingsToImage(GetDocumentFromNode(TreeDocuments.SelectedNode),
				renderImage,ApplySettings.FLIP|ApplySettings.ROTATE);
			if(copyImage!=null){
				Clipboard.SetDataObject(copyImage);
			}
		}

		private void OnPaste_Click() {
			IDataObject clipboard=Clipboard.GetDataObject();
			if(!clipboard.GetDataPresent(DataFormats.Bitmap)){
				MessageBox.Show(Lan.g(this,"No bitmap present on clipboard"));	
				return;
			}
			this.Cursor=Cursors.WaitCursor;
			Document doc=new Document();
			doc.FileName=".jpg";
			doc.DateCreated=DateTime.Today;
			doc.WithPat=PatCur.PatNum;
			doc.ImgType=ImageType.Photo;
			Documents.Insert(doc,PatCur);//this assigns a filename and saves to db
			string srcFile=patFolder+doc.FileName;
			try{
				Bitmap pasteImage=(Bitmap)clipboard.GetData(DataFormats.Bitmap);
				pasteImage.Save(srcFile);
			}catch{
				MessageBox.Show(Lan.g(this,"Error saving document."));
				Documents.Delete(doc);
				return;
			}
			DataRow tag=Documents.GetDocumentRow(doc.DocNum.ToString());
			doc.DocCategory=DefB.GetByExactName(DefCat.ImageCats,GetCurrentFolderName(TreeDocuments.SelectedNode));
			AddTreeDocumentNode(doc,tag,true);
			FormDocInfo formD=new FormDocInfo(PatCur,doc,GetCurrentFolderName(TreeDocuments.SelectedNode));
			formD.ShowDialog();
			if(formD.DialogResult!=DialogResult.OK){
				OnDelete_Click(GetTreeDocumentNode(doc.DocNum.ToString()),false);
			}else{
				MoveTreeDocumentNode(doc,doc.DocCategory,true);
			}
		}

		private void OnCrop_Click() {
			//remember it's testing after the push has been completed
			if(paintTools.Buttons["Crop"].Pushed){ //Crop Mode
				paintTools.Buttons["Hand"].Pushed=false;
				PictureBox1.Cursor = Cursors.Default;
			}		
			else{
				paintTools.Buttons["Crop"].Pushed=true;
			}
			IsCropMode=true;
			paintTools.Invalidate();
		}

		private void OnHand_Click() {
			if(paintTools.Buttons["Hand"].Pushed){//Hand Mode
				paintTools.Buttons["Crop"].Pushed=false;
				PictureBox1.Cursor=Cursors.Hand;
			}else{
				paintTools.Buttons["Hand"].Pushed=true;
			}
			IsCropMode=false;
			paintTools.Invalidate();
		}

		private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e){
			if(renderImage==null || renderImage.Width<1 || renderImage.Height<1){
				e.HasMorePages=false;
				return;
			}
			this.Cursor=Cursors.WaitCursor;
			//Crop and color function have already been applied to the render image.
			Bitmap printImage=ApplyDocumentSettingsToImage(GetDocumentFromNode(TreeDocuments.SelectedNode),
				renderImage,ApplySettings.FLIP|ApplySettings.ROTATE);
			RectangleF rectf=e.MarginBounds;
			float ratio=Math.Min(rectf.Width/printImage.Width,rectf.Height/printImage.Height);
			Graphics g=e.Graphics;
			g.InterpolationMode=InterpolationMode.HighQualityBicubic;
			g.CompositingQuality=CompositingQuality.HighQuality;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.PixelOffsetMode=PixelOffsetMode.HighQuality;
			g.DrawImage(printImage,0,0,(int)(printImage.Width*ratio),(int)(printImage.Height*ratio));
			e.HasMorePages =false;
		}

		private void menuTree_Click(object sender, System.EventArgs e) {
			switch(((MenuItem)sender).Index){
				case 0://print
					OnPrint_Click(contextNode);
					break;
				case 1://delete
					OnDelete_Click(contextNode,true);
					break;
				case 2://info
					OnInfo_Click(contextNode);
					break;
			}
		}

		private void menuForms_Click(object sender, System.EventArgs e) {
			string fileName=PrefB.GetString("DocPath")+@"Forms\"+((MenuItem)sender).Text;
			if(!File.Exists(fileName)){
				MessageBox.Show(Lan.g(this,"Could not find file: ")+fileName);
				return;
			}
			Document doc=new Document();
			//Document.Insert will use this extension when naming:
			doc.FileName=Path.GetExtension(fileName);
			doc.DateCreated=DateTime.Today;
			doc.WithPat=PatCur.PatNum;
			doc.ImgType=ImageType.Document;
			Documents.Insert(doc,PatCur);//this assigns a filename and saves to db
			bool copied=true;
			try{
				File.Copy(fileName,patFolder+doc.FileName);
			}catch{
				MessageBox.Show(Lan.g(this,"Unable to copy file. May be in use: ")+fileName);
				copied=false;
				Documents.Delete(doc);
			}
			if(copied){
				DataRow tag=Documents.GetDocumentRow(doc.DocNum.ToString());
				doc.DocCategory=DefB.GetByExactName(DefCat.ImageCats,GetCurrentFolderName(TreeDocuments.SelectedNode));
				AddTreeDocumentNode(doc,tag,true);
				FormDocInfo FormD=new FormDocInfo(PatCur,doc,GetCurrentFolderName(TreeDocuments.SelectedNode));
				FormD.ShowDialog();//some of the fields might get changed, but not the filename
				if(FormD.DialogResult!=DialogResult.OK){
					OnDelete_Click(GetTreeDocumentNode(doc.DocNum.ToString()),false);
				}else{
					MoveTreeDocumentNode(doc,doc.DocCategory,true);
				}
			}
		}

		private void textNote_DoubleClick(object sender,EventArgs e) {
			OnSign_Click();
		}

		private void label1_DoubleClick(object sender,EventArgs e) {
			OnSign_Click();
		}

		private void label15_DoubleClick(object sender,EventArgs e) {
			OnSign_Click();
		}

		private void sigBox_DoubleClick(object sender,EventArgs e) {
			OnSign_Click();
		}

		private void sigBoxTopaz_DoubleClick(object sender,EventArgs e) {
			OnSign_Click();
		}

		private void labelInvalidSig_DoubleClick(object sender,EventArgs e) {
			OnSign_Click();
		}

		private void panelNote_DoubleClick(object sender,EventArgs e) {
			OnSign_Click();
		}

		private void ContrDocs_MouseMove(object sender,MouseEventArgs e) {
			mouseLocation=e.Location;
		}

		private void textNote_MouseHover(object sender,EventArgs e) {
			textNote.Cursor=Cursors.IBeam;
		}

		///<summary>Gets the category folder name for the given document node.</summary>
		private string GetCurrentFolderName(TreeNode node) {
			if(node!=null){
				while(node.Parent!=null){//Find the corresponding root level node.
					node=node.Parent;
				}
				return node.Text;
			}
			return"";
		}

		private Document GetDocumentFromNode(TreeNode node) {
			if(node!=null){
				return Documents.Fill((DataRow)node.Tag);//Returns null if tag is null.
			}
			return null;
		}

		private void TreeDocuments_AfterSelect(object sender,TreeViewEventArgs e) {
			Document selectDoc=GetDocumentFromNode(e.Node);
			if(selectDoc==null){
				return;
			}
			if(selectDoc.WindowingMax<=selectDoc.WindowingMin) {//Contrast/brightness have never been set yet or are invalid?
				if(selectDoc.ImgType==ImageType.Radiograph) {
					selectDoc.WindowingMin=PrefB.GetInt("ImageWindowingMin");
					selectDoc.WindowingMax=PrefB.GetInt("ImageWindowingMax");
				}else{
					selectDoc.WindowingMin=0;//Normal brightness/contrast.
					selectDoc.WindowingMax=255;
				}
				Documents.Update(selectDoc);
				MoveTreeDocumentNode(selectDoc,selectDoc.DocCategory,true);//Update the tree list.
			}
			brightnessContrastSlider.MinVal=selectDoc.WindowingMin;
			brightnessContrastSlider.MaxVal=selectDoc.WindowingMax;
			string srcFileName=patFolder+selectDoc.FileName;
			if(!File.Exists(srcFileName)) {
				MessageBox.Show(Lan.g(this,"File not found: ")+srcFileName);
			}
		}

		private void TreeDocuments_ItemDrag(object sender,ItemDragEventArgs e) {
			dragItem=(TreeNode)e.Item;
			if(e.Button==MouseButtons.Left){
				if(dragItem.Parent==null){
					//This node is a folder or is being dragged with the wrong mouse button.
					TreeDocuments.SelectedNode=dragItem;
				}else if(dragItem!=null) {
					movingItem=true;
					TreeDocuments.Cursor=Cursors.Hand;
				}
			}
		}

		private void TreeDocuments_MouseHover(object sender,EventArgs e) {
			if(movingItem) {
				TreeDocuments.Cursor=Cursors.Hand;//Paused in the middle of moving a document
			}else{
				TreeDocuments.Cursor=Cursors.Default;
			}
		}

		private void TreeDocuments_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(movingItem) {
				TreeDocuments.Cursor=Cursors.Hand;
			}else{
				TreeDocuments.Cursor=Cursors.Default;
			}
		}

		private void TreeDocuments_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!movingItem) {
				return;
			}
			movingItem=false;
			TreeNode upNode=TreeDocuments.GetNodeAt(e.X,e.Y);
			if(upNode!=null) {
				Document dragDoc=GetDocumentFromNode(dragItem);
				if(dragDoc!=null) {
					if(upNode.Parent==null) {
						dragDoc.DocCategory=DefB.Short[(int)DefCat.ImageCats][upNode.Index].DefNum;
					}else{
						dragDoc.DocCategory=GetDocumentFromNode(upNode).DocCategory;
					}
					Documents.Update(dragDoc);
					MoveTreeDocumentNode(dragDoc,dragDoc.DocCategory,true);
				}
			}
		}

		/*
		//not supported by Mono
		private void TreeDocuments_NodeMouseClick(object sender,TreeNodeMouseClickEventArgs e) {
			if(e.Button!=MouseButtons.Right){
				return;
			}
			if(e.Node!=null && e.Node.Parent!=null) {
				TreeDocuments.ContextMenu=contextTree;
			}else{
				TreeDocuments.ContextMenu=null;
				TreeDocuments.SelectedNode=e.Node;
			}
			contextNode=e.Node;
		}

		private void TreeDocuments_NodeMouseDoubleClick(object sender,TreeNodeMouseClickEventArgs e) {
			Document nodeDoc=GetDocumentFromNode(e.Node);
			if(nodeDoc!=null) {
				string srcFileName=patFolder+nodeDoc.FileName;
				if(!HasImageExtension(srcFileName)) {//Only launch files which cannot be handled by Open Dental.
					try {
						Process.Start(srcFileName);
					}
					catch(Exception ex) {
						MessageBox.Show(ex.Message);
					}
				}
			}
		}*/

		private void OnInfo_Click(TreeNode infoNode) {
			Document infoDoc=GetDocumentFromNode(infoNode);
			if(infoDoc!=null) {
				TreeDocuments.SelectedNode=infoNode;//Pre-render the image.
				FormDocInfo formDocInfo2=new FormDocInfo(PatCur,infoDoc,GetCurrentFolderName(TreeDocuments.SelectedNode));
				formDocInfo2.ShowDialog();
				if(formDocInfo2.DialogResult!=DialogResult.OK) {
					return;
				}
				MoveTreeDocumentNode(infoDoc,infoDoc.DocCategory,true);
			}
		}

		private void OnZoomIn_Click() {
			zoomLevel++;
			PointF c=new PointF(PictureBox1.ClientRectangle.Width/2.0f,PictureBox1.ClientRectangle.Height/2.0f);
			PointF p=new PointF(c.X-imageTranslation.X,c.Y-imageTranslation.Y);
			imageTranslation=new PointF(imageTranslation.X-p.X,imageTranslation.Y-p.Y);
			zoomFactor=(float)Math.Pow(2,zoomLevel);
		}

		private void OnZoomOut_Click() {
			zoomLevel--;
			PointF c=new PointF(PictureBox1.ClientRectangle.Width/2.0f,PictureBox1.ClientRectangle.Height/2.0f);
			PointF p=new PointF(c.X-imageTranslation.X,c.Y-imageTranslation.Y);
			imageTranslation=new PointF(imageTranslation.X+p.X/2.0f,imageTranslation.Y+p.Y/2.0f);
			zoomFactor=(float)Math.Pow(2,zoomLevel);
		}

		private void DeleteThumbnailImage(Document doc){
			string thumbnailFile=patFolder+@"Thumbnails\"+doc.FileName;
			if(File.Exists(thumbnailFile)) {
				try {
					File.Delete(thumbnailFile);
				}
				catch {
					//Two users *might* edit the same image at the same time, so the image might already be deleted.
				}
			}
		}

		private void OnFlip_Click() {
			Document curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
			if(curDoc==null){
				return;
			}
			this.Cursor=Cursors.WaitCursor;
			curDoc.IsFlipped=!curDoc.IsFlipped;
			Documents.Update(curDoc);
			MoveTreeDocumentNode(curDoc,curDoc.DocCategory,true);//Update the file list data.
			DeleteThumbnailImage(curDoc);
		}

		private void OnRotateL_Click() {
			Document curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
			if(curDoc==null) {
				return;
			}
			this.Cursor=Cursors.WaitCursor;
			curDoc.DegreesRotated-=90;
			while(curDoc.DegreesRotated<0) {
				curDoc.DegreesRotated+=360;
			}
			Documents.Update(curDoc);
			MoveTreeDocumentNode(curDoc,curDoc.DocCategory,true);//Update the file list data.
			DeleteThumbnailImage(curDoc);
		}

		private void OnRotateR_Click(){
			Document curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
			if(curDoc==null) {
				return;
			}
			this.Cursor=Cursors.WaitCursor;
			curDoc.DegreesRotated=(curDoc.DegreesRotated+90)%360;
			Documents.Update(curDoc);
			MoveTreeDocumentNode(curDoc,curDoc.DocCategory,true);//Update the file list data.
			DeleteThumbnailImage(curDoc);
		}

		///<summary>Keeps the back buffer for the picture box to be the same in dimensions as the picture box itself.</summary>
		private void PictureBox1_SizeChanged(object sender,EventArgs e) {
			try{
				if(backBuffGraph!=null) {
					backBuffGraph.Dispose();
				}
				backBuffGraph=null;
				if(backBuffer!=null) {
					backBuffer.Dispose();
				}
				backBuffer=null;
				int width=PictureBox1.Bounds.Width;
				int height=PictureBox1.Bounds.Height;
				if(width>0 && height>0) {
					backBuffer=new Bitmap(width,height);
					backBuffGraph=Graphics.FromImage(backBuffer);
				}
			}
			catch(Exception ex) {
				Logger.openlog.curSev=Logger.Severity.ERROR;
				Logger.openlog.Log("ContrDocs.OnRotateR_Click: "+ex.ToString());
			}
		}

		///<summary>cropTangle must be invalid before this is called.</summary>
		private void PictureBox1_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseDownOrigin=new Point(e.X,e.Y);
			MouseIsDown=true;
			imageLocation=new PointF(imageTranslation.X,imageTranslation.Y);
		}

		private void PictureBox1_MouseHover(object sender,EventArgs e) {
			if(paintTools.Buttons["Hand"].Pushed) {//Hand mode.
				PictureBox1.Cursor=Cursors.Hand;
			}else{
				PictureBox1.Cursor=Cursors.Default;
			}
		}

		private void PictureBox1_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(paintTools.Buttons["Hand"].Pushed) {//Hand mode.
				if(MouseIsDown) {
					imageTranslation=new PointF(imageLocation.X+(e.Location.X-MouseDownOrigin.X),
						imageLocation.Y+(e.Location.Y-MouseDownOrigin.Y));
				}
			}else if(paintTools.Buttons["Crop"].Pushed){
				if(MouseIsDown){
					float[] intersect=ODMathLib.IntersectRectangles(Math.Min(e.Location.X,MouseDownOrigin.X),
						Math.Min(e.Location.Y,MouseDownOrigin.Y),Math.Abs(e.Location.X-MouseDownOrigin.X),
						Math.Abs(e.Location.Y-MouseDownOrigin.Y),PictureBox1.ClientRectangle.X,PictureBox1.ClientRectangle.Y,
						PictureBox1.ClientRectangle.Width-1,PictureBox1.ClientRectangle.Height-1);
					if(intersect.Length<0){
						cropTangle=new Rectangle(0,0,-1,-1);
					}else{
						cropTangle=new Rectangle((int)intersect[0],(int)intersect[1],(int)intersect[2],(int)intersect[3]);
					}
				}
			}
		}

		private void PictureBox1_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseIsDown=false;
			if(paintTools.Buttons["Crop"].Pushed) {//Crop Mode
				Document curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
				if(curDoc!=null && cropTangle.Width>0 && cropTangle.Height>0) {
					if(MessageBox.Show(Lan.g(this,"Crop to Rectangle?"),"",MessageBoxButtons.OKCancel)==DialogResult.OK){
						PointF cropPoint1=ScreenPointToUnalteredDocumentPoint(cropTangle.Location,curDoc,
							curImageWidth,curImageHeight,imageZoom*zoomFactor,imageTranslation);
						PointF cropPoint2=ScreenPointToUnalteredDocumentPoint(new Point(cropTangle.Location.X+cropTangle.Width,
							cropTangle.Location.Y+cropTangle.Height),curDoc,curImageWidth,curImageHeight,
							imageZoom*zoomFactor,imageTranslation);
						//cropPoint1 and cropPoint2 together define an axis-aligned bounding area, or our crop area. 
						//However, the two points have no guaranteed order, thus we must sort them using Math.Min.
						Rectangle rawCropRect=new Rectangle(
							(int)Math.Round((decimal)Math.Min(cropPoint1.X,cropPoint2.X)),
							(int)Math.Round((decimal)Math.Min(cropPoint1.Y,cropPoint2.Y)),
							(int)Math.Ceiling((decimal)Math.Abs(cropPoint1.X-cropPoint2.X)),
							(int)Math.Ceiling((decimal)Math.Abs(cropPoint1.Y-cropPoint2.Y)));
						//We must also intersect the old cropping rectangle with the new cropping rectangle, so that part of
						//the image does not come back as a result of multiple crops.
						Rectangle oldCropRect=DocCropRect(curDoc,curImageWidth,curImageHeight);
						float[] finalCropRect=ODMathLib.IntersectRectangles(rawCropRect.X,rawCropRect.Y,rawCropRect.Width,
							rawCropRect.Height,oldCropRect.X,oldCropRect.Y,oldCropRect.Width,oldCropRect.Height);
						//Will return a null intersection when the user chooses a crop rectangle which is
						//entirely outside the current visible portion of the image. Can also return a zero-area rect,
						//if the entire image is cropped away.
						if(finalCropRect.Length==4 && finalCropRect[2]>0 && finalCropRect[3]>0){
							Rectangle prevCropRect=DocCropRect(curDoc,curImageWidth,curImageHeight);
							curDoc.CropX=(int)finalCropRect[0];
							curDoc.CropY=(int)finalCropRect[1];
							curDoc.CropW=(int)Math.Ceiling(finalCropRect[2]);
							curDoc.CropH=(int)Math.Ceiling(finalCropRect[3]);
							Documents.Update(curDoc);
							MoveTreeDocumentNode(curDoc,curDoc.DocCategory,true);//Update the file list data.
							DeleteThumbnailImage(curDoc);
							Rectangle newCropRect=DocCropRect(curDoc,curImageWidth,curImageHeight);
							//Update the location of the image so that the cropped portion of the image does not move in screen space.
							PointF prevCropCenter=new PointF(prevCropRect.X+prevCropRect.Width/2.0f,prevCropRect.Y+prevCropRect.Height/2.0f);
							PointF newCropCenter=new PointF(newCropRect.X+newCropRect.Width/2.0f,newCropRect.Y+newCropRect.Height/2.0f);
							PointF[] imageCropCenters=new PointF[] {
								prevCropCenter,
								newCropCenter
							};
							Matrix docMat=GetDocumentFlippedRotatedMatrix(curDoc);
							docMat.Scale(imageZoom*zoomFactor,imageZoom*zoomFactor);
							docMat.TransformPoints(imageCropCenters);
							imageTranslation=new PointF(imageTranslation.X+(imageCropCenters[1].X-imageCropCenters[0].X),
								imageTranslation.Y+(imageCropCenters[1].Y-imageCropCenters[0].Y));
							TreeDocuments.SelectedNode=TreeDocuments.SelectedNode;//Reload the image.
						}
					}
					cropTangle=new Rectangle(0,0,-1,-1);
				}
			}
		}

		private void brightnessContrastSlider_Scroll(object sender,EventArgs e){
			if(TreeDocuments.SelectedNode!=null && TreeDocuments.SelectedNode.Tag!=null){
				((DataRow)TreeDocuments.SelectedNode.Tag)["WindowingMin"]=brightnessContrastSlider.MinVal.ToString();
				((DataRow)TreeDocuments.SelectedNode.Tag)["WindowingMax"]=brightnessContrastSlider.MaxVal.ToString();
			}
		}

		private void brightnessContrastSlider_ScrollComplete(object sender,EventArgs e) {
			Document curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
			if(curDoc!=null) {
				Documents.Update(curDoc);
				DeleteThumbnailImage(curDoc);
			}
		}

		/*
		private void OnCapture_Click() {
			//bool capture=ToolBarMain.Buttons["Capture"].Pushed;//TODO: uncomment when XRay capture is supported.
			bool capture=false;
			if(capture){
				xRayImageController.CaptureXRay();
			}else{//The user unselected the image capture button, so cancel the current image capture.
				xRayImageController.KillXRayThread();//Stop current xRay capture and call OnCaptureAborted() when done.
			}
		}

		///<summary>Called on successful capture of image.</summary>
		private void OnCaptureComplete(object sender,EventArgs e) {
			Bitmap capturedImage=xRayImageController.capturedImage;
			Document doc=new Document();
			doc.ImgType=ImageType.Radiograph;
			doc.FileName=".jpg";
			doc.DateCreated=DateTime.Today;
			doc.WithPat=PatCur.PatNum;
			Documents.Insert(doc,PatCur);//creates filename and saves to db
			bool saved=true;
			try{				
				capturedImage.Save(patFolder+doc.FileName);
			}catch{
				MessageBox.Show(Lan.g(this,"Unable to save captured XRay image as document."));
				saved=false;
				Documents.Delete(doc);
			}
			if(saved){
				DataRow tag=Documents.GetDocumentRow(doc.DocNum.ToString());
				doc.DocCategory=DefB.GetByExactName(DefCat.ImageCats,GetCurrentFolderName(TreeDocuments.SelectedNode));
				AddTreeDocumentNode(doc,tag,true);
			}
			OnCapture_Click();//Prepare to capture another XRay, even if the last XRay failed to save.
		}

		///<summary>Called under any error circumstance resulting from the image capture process.</summary>
		private void OnCaptureAborted(object sender,EventArgs e) {
			//ToolBarMain.Buttons["Capture"].Pushed=false;//TODO: uncomment when XRay capture is supported.
			ToolBarMain.Invalidate();
		}*/

		private void EraseCurrentImage(){
			KillImageApplicationThread();//Stop any current access to the current image so we can dispose it.
			if(ImageCurrent!=null){
				ImageCurrent.Dispose();
				ImageCurrent=null;
			}
			if(renderImage!=null){
				renderImage.Dispose();
				renderImage=null;
			}
		}

		///<summary>Returns true if a new image had to be loaded from file.</summary>
		private bool SetCurrentImage(Document curDoc){
			string srcFileName=patFolder+curDoc.FileName;
			bool imageLoaded=false;
			if(srcFileName!=prevImageFile) {
				this.Cursor=Cursors.WaitCursor;
				prevImageFile=srcFileName;
				if(File.Exists(srcFileName)) {
					if(HasImageExtension(srcFileName)) {
						KillImageApplicationThread();//Remove any current access to the current image.
						ImageCurrent=new Bitmap(srcFileName);
						curImageWidth=ImageCurrent.Width;
						curImageHeight=ImageCurrent.Height;
					}else{
						EraseCurrentImage();
					}
					imageLoaded=true;
				}else{
					EraseCurrentImage();
				}
				this.Cursor=Cursors.Default;
			}
			bool settingsChanged=(srcFileName!=prevImageFile || previousImageSettings==null ||
															previousImageSettings.DocNum!=curDoc.DocNum || previousImageSettings.CropY!=curDoc.CropY ||
															previousImageSettings.CropW!=curDoc.CropW || previousImageSettings.CropH!=curDoc.CropH ||
															previousImageSettings.WindowingMax!=curDoc.WindowingMax || 
															previousImageSettings.WindowingMin!=curDoc.WindowingMin);
			if(settingsChanged){
				previousImageSettings=curDoc.Copy();
				KillImageApplicationThread();
				if(imageLoaded){
					//Set the unedited image until it is updated.
					renderImage=null;
				}
				Document docCopy=curDoc.Copy();
				System.Threading.ThreadStart imageSettingDeligate=
					delegate { this.ImageSettingApplicationThread(docCopy,ImageCurrent,imageLoaded); };
				imageSettingApplicationThread=new System.Threading.Thread(imageSettingDeligate);
				imageSettingApplicationThread.IsBackground=true;
				imageSettingApplicationThread.Priority=System.Threading.ThreadPriority.BelowNormal;
				imageSettingApplicationThread.TrySetApartmentState(System.Threading.ApartmentState.MTA);
				imageSettingApplicationThread.Start();
			}
			return imageLoaded;
		}

		private void ReloadImageSettings(int docImageWidth,int docImageHeight,Document curDoc) {
			//Choose an initial zoom so that the image is scaled to fit the picture box size.
			//Keep in mind that bitmaps are not allowed to have either a width or height of 0,
			//so the following equations will always work. The following subtracts from the 
			//picture box width to force a little extra white space.
			RectangleF imageRect=CalcImageDims(docImageWidth,docImageHeight,curDoc);
			float matchWidth=PictureBox1.Width-16;
			matchWidth=(matchWidth<=0?1:matchWidth);
			float matchHeight=PictureBox1.Height-16;
			matchHeight=(matchHeight<=0?1:matchHeight);
			imageZoom=(float)Math.Min(matchWidth/imageRect.Width,matchHeight/imageRect.Height);
			zoomLevel=0;
			zoomFactor=1;
			imageTranslation=new PointF(PictureBox1.Width/2.0f,PictureBox1.Height/2.0f);
			cropTangle=new Rectangle(0,0,-1,-1);
		}

		private void ImageSettingApplicationThread(Document curDocCopy,Bitmap curImageCopy,bool resetImageSettings){
			try{
				Bitmap resultImage=ApplyDocumentSettingsToImage(curDocCopy,curImageCopy,
						ApplySettings.CROP|ApplySettings.COLORFUNCTION);
				if(resetImageSettings){
					ReloadImageSettings(resultImage.Width,resultImage.Height,curDocCopy);
				}
				renderImage=resultImage;
			}catch(System.Threading.ThreadAbortException){
				return;//Exit as requested.
			}catch(Exception){
				//We don't draw anyting on error (because most of the time it will be due to the current selection state).
			}
		}

		///<summary>Kills the image processing thread if it is currently running.</summary>
		private void KillImageApplicationThread() {
			if(imageSettingApplicationThread!=null) {//Clear any previous image processing.
				if(imageSettingApplicationThread.IsAlive){
					imageSettingApplicationThread.Abort();
					while(imageSettingApplicationThread.IsAlive) {
						//Wait for the thread to stop execution.
					}
				}
				imageSettingApplicationThread=null;
			}
		}

		///<summary>Thread in charge of rendering the image in its current state. The image calculations are not performed here, only rendering of the image is performed here, so that we can guarantee a fast display. Having the display in its own thread allows for better code management, and ensures that the screen will redraw if tabbed out/in.</summary>
		private void timer1_Tick(object sender,EventArgs e){
			if(!this.Visible) {
				return;
			}
			//js I think this is unnecessary:
			if(this.Cursor==Cursors.WaitCursor){//By the time this function is called, it is not possible that we are in a waiting state
				this.Cursor=Cursors.Default;			
			}
			Color clearColor=Pens.White.Color;
			Graphics pbg=PictureBox1.CreateGraphics();
			Document curDoc=null;
			try{
				curDoc=GetDocumentFromNode(TreeDocuments.SelectedNode);
				//Adjust panel size to show note and or signature when it is available.
				bool oldPanelVis=panelNote.Visible;
				panelNote.Visible=(signing || 
					(curDoc.Note!=null && curDoc.Note!="") || 
					(curDoc.Signature!=null && curDoc.Signature!=""));
				bool panelNoteVisChange=(oldPanelVis!=panelNote.Visible);
				if(panelNoteVisChange) {
					ResizeAll();
				}
				if(panelNote.Visible){
					FillSignature();
				}
				backBuffGraph.Clear(clearColor);
				if(SetCurrentImage(curDoc) || panelNoteVisChange){
					ReloadImageSettings(renderImage.Width,renderImage.Height,curDoc);
				}
				backBuffGraph.Transform=GetScreenMatrix(curDoc,curImageWidth,curImageHeight,
					imageZoom*zoomFactor,imageTranslation);
				backBuffGraph.DrawImage(renderImage,0,0);
				if(cropTangle!=null && cropTangle.Width>0 && cropTangle.Height>0) {//Must be drawn last so it is on top.
					backBuffGraph.ResetTransform();
					backBuffGraph.DrawRectangle(Pens.Blue,cropTangle);
				}
				pbg.DrawImage(backBuffer,0,0);
				pbg.Dispose();
				if(!brightnessContrastSlider.Enabled){
					brightnessContrastSlider.Enabled=true;
				}
			}catch(Exception){
				if(curDoc==null || renderImage==null){
					if(brightnessContrastSlider.Enabled){
						brightnessContrastSlider.Enabled=false;
						brightnessContrastSlider.MinVal=0;
						brightnessContrastSlider.MaxVal=255;
					}
				}
				if(pbg!=null){
					pbg.Clear(clearColor);
				}
				pbg.Dispose();
			}
		}

		//===================================== STATIC FUNCTIONS =================================================

		///<summary>The screen matrix of the image is relative to the upper left of the image, but our calculations are from the center of the image (since the calculations are easier everywhere else if taken from the center). This function converts our calculation matrix into an equivalent screen matrix for display. Assumes document rotations are in 90 degree multiples.</summary>
		public static Matrix GetScreenMatrix(Document doc,int docOriginalImageWidth,int docOriginalImageHeight,
				float imageScale,PointF imageTranslation) {			
			Matrix docMat=GetDocumentFlippedRotatedMatrix(doc);
			docMat.Scale(imageScale,imageScale);
			Rectangle cropRect=DocCropRect(doc,docOriginalImageWidth,docOriginalImageHeight);
			//The screen matrix of a GDI image is always relative to the upper left hand corner of the image.
			PointF preOrigin=new PointF(-cropRect.Width/2.0f,-cropRect.Height/2.0f);
			PointF[] screenMatPoints=new PointF[]{
				preOrigin,
				new PointF(preOrigin.X+1	,preOrigin.Y  ),
				new PointF(preOrigin.X		,preOrigin.Y+1),
			};
			docMat.TransformPoints(screenMatPoints);
			Matrix screenMat=new Matrix(	screenMatPoints[1].X-screenMatPoints[0].X,//X.X
																		screenMatPoints[1].Y-screenMatPoints[0].Y,//X.Y
																		screenMatPoints[2].X-screenMatPoints[0].X,//Y.X
																		screenMatPoints[2].Y-screenMatPoints[0].Y,//Y.Y
																		screenMatPoints[0].X+imageTranslation.X,	//Dx
																		screenMatPoints[0].Y+imageTranslation.Y);	//Dy
			return screenMat;
		}

		///<summary>Calculates the image dimensions after factoring flip and rotation of the given document.</summary>
		public static RectangleF CalcImageDims(int imageWidth,int imageHeight,Document doc) {
			Matrix orientation=GetScreenMatrix(doc,imageWidth,imageHeight,1,new PointF(0,0));
			PointF[] corners=new PointF[] {
				new PointF(-imageWidth/2,-imageHeight/2),
				new PointF(imageWidth/2,-imageHeight/2),
				new PointF(-imageWidth/2,imageHeight/2),
				new PointF(imageWidth/2,imageHeight/2),
			};
			orientation.TransformPoints(corners);
			float minx=corners[0].X;
			float maxx=minx;
			float miny=corners[0].Y;
			float maxy=miny;
			for(int i=1;i<corners.Length;i++) {
				if(corners[i].X<minx) {
					minx=corners[i].X;
				}
				else if(corners[i].X>maxx) {
					maxx=corners[i].X;
				}
				if(corners[i].Y<miny) {
					miny=corners[i].Y;
				}
				else if(corners[i].Y>maxy) {
					maxy=corners[i].Y;
				}
			}
			return new RectangleF(0,0,maxx-minx,maxy-miny);
		}

		///<summary>Converts a screen-space location into a location which is relative to the given document in its unrotated/unflipped/unscaled/untranslated state.</summary>
		public static PointF ScreenPointToUnalteredDocumentPoint(PointF screenLocation,Document doc,
				int docOriginalImageWidth,int docOriginalImageHeight,float imageScale,PointF imageTranslation){
			Matrix docMat=GetDocumentFlippedRotatedMatrix(doc);
			docMat.Scale(imageScale,imageScale);
			//Now we have a matrix representing the image in its current state-space.
			float[] docMatAxes=docMat.Elements;
			float px=screenLocation.X-imageTranslation.X;
			float py=screenLocation.Y-imageTranslation.Y;
			//The origin of our internal image axis is always relative to the center of the crop rectangle.
			Rectangle docCropRect=DocCropRect(doc,docOriginalImageWidth,docOriginalImageHeight);
			PointF cropRectCenter=new PointF(docCropRect.X+docCropRect.Width/2.0f,
				docCropRect.Y+docCropRect.Height/2.0f);
			return new PointF(
				(cropRectCenter.X+(px*docMatAxes[0]+py*docMatAxes[1])/(imageScale*imageScale)),
				(cropRectCenter.Y+(px*docMatAxes[2]+py*docMatAxes[3])/(imageScale*imageScale)));
		}

		///<summary>Returns true if the given filename contains a supported file image extension.</summary>
		public static bool HasImageExtension(string fileName) {
			string ext=Path.GetExtension(fileName).ToLower();
			//The following supported bitmap types were found on a microsoft msdn page:
			return (ext==".jpg"||ext==".jpeg"||ext==".tga"||ext==".bmp"||ext==".tif"||
				ext==".tiff"||ext==".gif"||ext==".emf"||ext==".exif"||ext==".ico"||ext==".png"||ext==".wmf");
		}

		///<summary>Returns false if the file is a specific short file name that is not accepted or contains one of the unsupported file exentions.</summary>
		public static bool IsAcceptableFileName(string file){
			string[] specificBadFileNames=new string[] {
				"thumbs.db"
			};
			for(int i=0;i<specificBadFileNames.Length;i++){
				if(file.Length>=specificBadFileNames[i].Length && 
					file.Substring(file.Length-specificBadFileNames[i].Length,
					specificBadFileNames[i].Length).ToLower()==specificBadFileNames[i]){
					return false;
				}
			}
			return true;
		}

		///<summary>Returns a matrix for the given document which represents flipping over the Y-axis before rotating. Of course, if doc.IsFlipped is false, then no flipping is performed, and if doc.DegreesRotated is a multiple of 360 then no rotation is performed.</summary>
		public static Matrix GetDocumentFlippedRotatedMatrix(Document doc) {
			Matrix result=new Matrix(doc.IsFlipped?-1:1,0,//X-axis
																0,1,//Y-axis
																0,0);//Offset/Translation(dx,dy)
			result.Rotate(doc.IsFlipped?-doc.DegreesRotated:doc.DegreesRotated);
			return result;
		}

		public static Rectangle DocCropRect(Document doc,int originalImageWidth,int originalImageHeight) {
			if(doc.CropW>0 && doc.CropH>0){//Crop rectangles of 0 area are considered non-existant (i.e. no cropping).
				return new Rectangle(doc.CropX,doc.CropY,doc.CropW,doc.CropH);
			}
			return new Rectangle(0,0,originalImageWidth,originalImageHeight);
		}

		public enum ApplySettings{
			ALL=						0xFF,
			CROP=						0x01,
			FLIP=						0x02,
			ROTATE=					0x04,
			COLORFUNCTION=	0x08,
		};

		///<summary>Applies the document specified cropping, flip, rotation, brightness and contrast transformations to the image and returns the resulting image. Zoom and translation must be handled by the calling code. The returned image is always a new image that can be modified without affecting the original image. The change in the image's center point is returned into deltaCenter, so that rotation offsets can be properly calculated when displaying the returned image.</summary>
		public static Bitmap ApplyDocumentSettingsToImage(Document docSettings,Bitmap image,ApplySettings options){
			if(image==null){//Any operation on a non-existant image produces a non-existant image.
				return null;
			}
			if(docSettings==null){//No settings implies no operations, implies that the image should be returned unaltered.
				return (Bitmap)image.Clone();
			}
			//CROP - Implies that the croping rectangle must be saved in raw-image-space coordinates, 
			//with an origin of that equal to the upper left hand portion of the image.
			Rectangle cropResult;
			if((options&ApplySettings.CROP)!=0 &&							//Crop not requested.
				docSettings.CropW>0 && docSettings.CropH>0){	//No clip area yet defined, so no clipping is performed.
				float[] cropDims=ODMathLib.IntersectRectangles(0,0,image.Width,image.Height,//Intersect image rectangle with
					docSettings.CropX,docSettings.CropY,docSettings.CropW,docSettings.CropH);//document crop rectangle.
				if(cropDims.Length==0){//The entire image has been cropped away.
					return null;
				}
				//Rounds dims up, so that data is not lost, but possibly not removing all of what was expected.
				cropResult=new Rectangle((int)cropDims[0],(int)cropDims[1],
					(int)Math.Ceiling(cropDims[2]),(int)Math.Ceiling(cropDims[3]));
			}else{
				cropResult=new Rectangle(0,0,image.Width,image.Height);//No cropping.
			}
			Bitmap cropped=new Bitmap(cropResult.Width,cropResult.Height,PixelFormat.Format32bppArgb);//Always use 32-bit in memory.
			Graphics g=Graphics.FromImage(cropped);
			g.InterpolationMode=InterpolationMode.HighQualityBicubic;
			g.CompositingQuality=CompositingQuality.HighQuality;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.PixelOffsetMode=PixelOffsetMode.HighQuality;
			g.DrawImage(image,new Rectangle(0,0,cropped.Width,cropped.Height),cropResult,GraphicsUnit.Pixel);
			g.Dispose();
			//FLIP AND ROTATE - must match the operations in GetDocumentFlippedRotatedMatrix().
			if((options&ApplySettings.FLIP)!=0){
				if(docSettings.IsFlipped){
					cropped.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
			}
			if((options&ApplySettings.ROTATE)!=0) {
				if(docSettings.DegreesRotated%360==90){
					cropped.RotateFlip(RotateFlipType.Rotate90FlipNone);
				}else if(docSettings.DegreesRotated%360==180) {
					cropped.RotateFlip(RotateFlipType.Rotate180FlipNone);
				}else if(docSettings.DegreesRotated%360==270){
					cropped.RotateFlip(RotateFlipType.Rotate270FlipNone);
				}
			}
			//APPLY BRIGHTNESS AND CONTRAST - 
			//TODO: should be updated later for more general functions 
			//(create inputValues and outputValues from stored db function/table).
			if((options&ApplySettings.COLORFUNCTION)!=0){
				float[] inputValues=new float[] {
					docSettings.WindowingMin/255f,
					docSettings.WindowingMax/255f,
				};
				float[] outputValues=new float[]{
					0,
					1,
				};
				BitmapData croppedData=null;
				try{
					croppedData=cropped.LockBits(new Rectangle(0,0,cropped.Width,cropped.Height),
						ImageLockMode.ReadWrite,cropped.PixelFormat);
					unsafe{
						byte* pBits;
						if(croppedData.Stride<0){
							pBits=(byte*)croppedData.Scan0.ToPointer()+croppedData.Stride*(croppedData.Height-1);
						}else{
							pBits=(byte*)croppedData.Scan0.ToPointer();
						}
						//The following loop goes through each byte of each 32-bit value and applies the color function to it.
						//Thus, the same transformation is performed to all 4 color components equivalently for each pixel.
						for(int i=0;i<croppedData.Stride*croppedData.Height;i++){
							float colorComponent=pBits[i]/255f;
							float rangedOutput;
							if(colorComponent<=inputValues[0]) {
								rangedOutput=outputValues[0];
							}else if(colorComponent>=inputValues[inputValues.Length-1]) {
								rangedOutput=outputValues[outputValues.Length-1];
							}else{
								int j=0;
								while(!(inputValues[j]<=colorComponent && colorComponent<inputValues[j+1])) {
									j++;
								}
								rangedOutput=((colorComponent-inputValues[j])*(outputValues[j+1]-outputValues[j]))/(inputValues[j+1]-inputValues[j]);
							}
							pBits[i]=(byte)Math.Round(255*rangedOutput);
						}
					}
				}catch{
				}finally{
					try{
						cropped.UnlockBits(croppedData);
					}catch{
					}
				}
			}
			return cropped;
		}

		///<summary>specify the size of the square to return</summary>
		public static Bitmap GetThumbnail(Image original,int size) {
			Bitmap retVal=new Bitmap(size,size);
			Graphics g=Graphics.FromImage(retVal);
			g.InterpolationMode=InterpolationMode.HighQualityBicubic;
			g.CompositingQuality=CompositingQuality.HighQuality;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.PixelOffsetMode=PixelOffsetMode.HighQuality;
			if(original.Height>original.Width) {//original is too tall
				float ratio=(float)size/(float)original.Height;
				float w=(float)original.Width*ratio;
				g.DrawImage(original,(size-w)/2f,0,w,(float)size);
			}else {//original is too wide
				float ratio=(float)size/(float)original.Width;
				float h=(float)original.Height*ratio;
				g.DrawImage(original,0,(size-h)/2f,(float)size,h);
			}
			g.Dispose();
			return retVal;
		}
		
		///<summary>Takes in a mount object and finds all the images pertaining to the mount, then concatonates them together into one large, unscaled image and returns that image.</summary>
		//public static Image CreateMountImage(Mount mount){
			//TODO:
		//}

	}
}