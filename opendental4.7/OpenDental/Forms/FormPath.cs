using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormPath : System.Windows.Forms.Form{
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textExportPath;
		private System.Windows.Forms.TextBox textDocPath;
		private OpenDental.UI.Button butBrowseExport;
		private OpenDental.UI.Button buBrowseDoc;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private OpenDental.UI.Button butBrowseLetter;
		private System.Windows.Forms.TextBox textLetterMergePath;
		private System.Windows.Forms.FolderBrowserDialog fb;
    //private bool IsBackup=false;
		//private User user;

		///<summary></summary>
		public FormPath(){
			InitializeComponent();
			Lan.F(this);
			//Lan.C(this, new System.Windows.Forms.Control[] {
			//	this.textBox1,
			//	this.textBox3
			//});
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPath));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textDocPath = new System.Windows.Forms.TextBox();
			this.textExportPath = new System.Windows.Forms.TextBox();
			this.butBrowseExport = new OpenDental.UI.Button();
			this.buBrowseDoc = new OpenDental.UI.Button();
			this.fb = new System.Windows.Forms.FolderBrowserDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butBrowseLetter = new OpenDental.UI.Button();
			this.textLetterMergePath = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.Location = new System.Drawing.Point(620,362);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(620,396);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textDocPath
			// 
			this.textDocPath.Location = new System.Drawing.Point(10,104);
			this.textDocPath.Name = "textDocPath";
			this.textDocPath.Size = new System.Drawing.Size(518,20);
			this.textDocPath.TabIndex = 0;
			this.textDocPath.Leave += new System.EventHandler(this.textDocPath_Leave);
			// 
			// textExportPath
			// 
			this.textExportPath.Location = new System.Drawing.Point(10,210);
			this.textExportPath.Name = "textExportPath";
			this.textExportPath.Size = new System.Drawing.Size(515,20);
			this.textExportPath.TabIndex = 1;
			this.textExportPath.Leave += new System.EventHandler(this.textExportPath_Leave);
			// 
			// butBrowseExport
			// 
			this.butBrowseExport.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butBrowseExport.Autosize = true;
			this.butBrowseExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseExport.Location = new System.Drawing.Point(530,208);
			this.butBrowseExport.Name = "butBrowseExport";
			this.butBrowseExport.Size = new System.Drawing.Size(76,25);
			this.butBrowseExport.TabIndex = 91;
			this.butBrowseExport.Text = "Browse";
			this.butBrowseExport.Click += new System.EventHandler(this.butBrowseExport_Click);
			// 
			// buBrowseDoc
			// 
			this.buBrowseDoc.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.buBrowseDoc.Autosize = true;
			this.buBrowseDoc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buBrowseDoc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buBrowseDoc.Location = new System.Drawing.Point(531,102);
			this.buBrowseDoc.Name = "buBrowseDoc";
			this.buBrowseDoc.Size = new System.Drawing.Size(76,25);
			this.buBrowseDoc.TabIndex = 90;
			this.buBrowseDoc.Text = "&Browse";
			this.buBrowseDoc.Click += new System.EventHandler(this.buBrowseDoc_Click);
			// 
			// fb
			// 
			this.fb.SelectedPath = "C:\\";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11,144);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(543,65);
			this.label1.TabIndex = 92;
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(11,34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(545,63);
			this.label2.TabIndex = 93;
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(11,239);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(543,65);
			this.label3.TabIndex = 96;
			this.label3.Text = resources.GetString("label3.Text");
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butBrowseLetter
			// 
			this.butBrowseLetter.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butBrowseLetter.Autosize = true;
			this.butBrowseLetter.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseLetter.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseLetter.Location = new System.Drawing.Point(530,303);
			this.butBrowseLetter.Name = "butBrowseLetter";
			this.butBrowseLetter.Size = new System.Drawing.Size(76,25);
			this.butBrowseLetter.TabIndex = 95;
			this.butBrowseLetter.Text = "Browse";
			this.butBrowseLetter.Click += new System.EventHandler(this.butBrowseLetter_Click);
			// 
			// textLetterMergePath
			// 
			this.textLetterMergePath.Location = new System.Drawing.Point(10,305);
			this.textLetterMergePath.Name = "textLetterMergePath";
			this.textLetterMergePath.Size = new System.Drawing.Size(515,20);
			this.textLetterMergePath.TabIndex = 94;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif",9F,System.Drawing.FontStyle.Bold,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.label4.Location = new System.Drawing.Point(12,9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(483,23);
			this.label4.TabIndex = 97;
			this.label4.Text = "The first box is mandatory.  The others are optional.";
			// 
			// FormPath
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(718,437);
			this.Controls.Add(this.butBrowseLetter);
			this.Controls.Add(this.butBrowseExport);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textLetterMergePath);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buBrowseDoc);
			this.Controls.Add(this.textDocPath);
			this.Controls.Add(this.textExportPath);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPath";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Paths";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormPath_Closing);
			this.Load += new System.EventHandler(this.FormPath_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPath_Load(object sender, System.EventArgs e){
			/*if(PermissionsOld.AuthorizationRequired("Data Paths")){
				user=Users.Authenticate("Data Paths");
				if(user==null){
					DialogResult=DialogResult.Cancel;
					return;
				}
				if(!UserPermissions.IsAuthorized("Data Paths",user)){
					MsgBox.Show(this,"You do not have permission for this feature");
					DialogResult=DialogResult.Cancel;
					return;
				}	
			}*/
			textDocPath.Text=((Pref)PrefB.HList["DocPath"]).ValueString;
			textExportPath.Text=((Pref)PrefB.HList["ExportPath"]).ValueString;
			textLetterMergePath.Text=((Pref)PrefB.HList["LetterMergePath"]).ValueString;
		}

		private void textDocPath_Leave(object sender, System.EventArgs e) {
			//if(!textDocPath.Text.EndsWith(@"\")){
			//	textDocPath.Text+=@"\";
			//}
		}

		private void textExportPath_Leave(object sender, System.EventArgs e) {
			//if(!textExportPath.Text.EndsWith(@"\")){
			//	textExportPath.Text+=@"\";
			//}
		}

		private void butOK_Click(object sender, System.EventArgs e){
			/*string remoteUri = "http://www.open-dent.com/languages/";
			string fileName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName+".sql";//eg. es.sql for spanish
			//string fileName="bogus.sql";
			string myStringWebResource = null;
			WebClient myWebClient = new WebClient();
			myStringWebResource = remoteUri + fileName;
			try{
				//myWebClient.Credentials=new NetworkCredential("username","password","www.open-dent.com");
				myWebClient.DownloadFile(myStringWebResource,fileName);
			}
			catch{
				MessageBox.Show("Either you do not have internet access, or no translations are available for "+CultureInfo.CurrentCulture.Parent.DisplayName);
				return;
			}
			ClassConvertDatabase ConvertDB=new ClassConvertDatabase();
			if(!ConvertDB.ExecuteFile(fileName)){
				MessageBox.Show("Translations not installed properly.");
				return;
			}
			LanguageForeigns.Refresh();
			MessageBox.Show("Done");*/
			if(!textDocPath.Text.EndsWith(@"\")
				&& !textDocPath.Text.EndsWith(@"/"))
			{
				MessageBox.Show(Lan.g(this,"Document Path is not valid."));
				return;
			}
			if(!Directory.Exists(textDocPath.Text)){
				MessageBox.Show(Lan.g(this,"Document Path is not valid."));
				return;
    	}
			if(!Directory.Exists(textDocPath.Text+"A\\")){
				MessageBox.Show(Lan.g(this,"Document Path is not correct.  Must contain folders A-Z"));
				return;
			}
      //CheckIfDocBackup();//checks if new folder is pointing at a backup
      /*if(IsBackup){
				if(MessageBox.Show(Lan.g(this,"You are setting you Image Folder to a Backup Folder.  Do you wish to continue?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
					return;   
				} 
      }*/
			Prefs.UpdateString("DocPath",textDocPath.Text);
			Prefs.UpdateString("ExportPath",textExportPath.Text);
			Prefs.UpdateString("LetterMergePath",textLetterMergePath.Text);
			DataValid.SetInvalid(InvalidTypes.Prefs);
			//SecurityLogs.MakeLogEntry("Form Path","Altered Path",user);
			DialogResult=DialogResult.OK;
		}

    /*private void CheckIfDocBackup(){
      IsBackup=false;
 			DirectoryInfo dirInfo=new DirectoryInfo(textDocPath.Text);
			FileInfo[] fi=dirInfo.GetFiles();
			for(int i=0;i<fi.Length;i++){
				if(fi[i].Name=="Backup"){
          IsBackup=true;   
        }
			}	       
    }*/

		private void buBrowseDoc_Click(object sender, System.EventArgs e){
		  fb.ShowDialog();
      textDocPath.Text=fb.SelectedPath+@"\";
		}

		private void butBrowseExport_Click(object sender, System.EventArgs e){
		  fb.ShowDialog();
      textExportPath.Text=fb.SelectedPath+@"\";		
		}

		private void butBrowseLetter_Click(object sender, System.EventArgs e) {
			fb.ShowDialog();
      textLetterMergePath.Text=fb.SelectedPath+@"\";		
		}

		private void butCancel_Click(object sender, System.EventArgs e){
			DialogResult=DialogResult.Cancel;
		}

		private void FormPath_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK)
				return;
			if(!Directory.Exists(((Pref)PrefB.HList["DocPath"]).ValueString) || !Directory.Exists(((Pref)PrefB.HList["DocPath"]).ValueString+"A\\")){
				MessageBox.Show(Lan.g(this,"Invalid Document path.  Closing Free Dental."));
				Application.Exit();
			}
		}

		



	}
}
