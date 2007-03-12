using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormAging : System.Windows.Forms.Form{
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.TextBox textBox1;
		private OpenDental.ValidDate textDateLast;
		private OpenDental.ValidDate textDateCalc;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public FormAging(){
			InitializeComponent();
			Lan.F(this);
			Lan.C(this,new Control[]
				{this.textBox1});
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
			this.textDateLast = new OpenDental.ValidDate();
			this.label1 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textDateCalc = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textDateLast
			// 
			this.textDateLast.Location = new System.Drawing.Point(172, 230);
			this.textDateLast.Name = "textDateLast";
			this.textDateLast.ReadOnly = true;
			this.textDateLast.Size = new System.Drawing.Size(94, 20);
			this.textDateLast.TabIndex = 12;
			this.textDateLast.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(22, 234);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(146, 16);
			this.label1.TabIndex = 13;
			this.label1.Text = "Last Calculated";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(440, 246);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 15;
			this.butCancel.Text = "&Cancel";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.Location = new System.Drawing.Point(440, 212);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 14;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(28, 12);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(476, 124);
			this.textBox1.TabIndex = 16;
			this.textBox1.Text = @"This tool recalculates aging for all patients. 

Depending on the size of your database, it could take up to ten minutes.   It's faster if you run it from your server instead of a workstation.

The results can be viewed in various reports.  Be aware that if you later open a patient's account, the aging for that account will be immediately recalculated as of the current date.";
			// 
			// textDateCalc
			// 
			this.textDateCalc.Location = new System.Drawing.Point(172, 190);
			this.textDateCalc.Name = "textDateCalc";
			this.textDateCalc.Size = new System.Drawing.Size(94, 20);
			this.textDateCalc.TabIndex = 17;
			this.textDateCalc.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(22, 194);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(146, 16);
			this.label2.TabIndex = 18;
			this.label2.Text = "Calculate as of";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormAging
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(532, 282);
			this.Controls.Add(this.textDateCalc);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.textDateLast);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAging";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Calculate Aging";
			this.Load += new System.EventHandler(this.FormAging_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAging_Load(object sender, System.EventArgs e) {
			textDateCalc.Text=DateTime.Today.ToShortDateString();
			DateTime dateLastAging=PIn.PDate(PrefB.GetString("DateLastAging"));
			if(dateLastAging.Year<1880){
				textDateLast.Text="";
			}
			else{
				textDateLast.Text=dateLastAging.ToShortDateString();
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if( textDateCalc.errorProvider1.GetError(textDateCalc)!=""
				){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(!MsgBox.Show(this,true,"Click OK to update aging.")){
				return;
			}
			Cursor=Cursors.WaitCursor;
			Patients.ResetAging();
			int[] allGuarantors=Ledgers.GetAllGuarantors();
			for(int i=0;i<allGuarantors.Length;i++){
				Ledgers.ComputeAging(allGuarantors[i],PIn.PDate(textDateCalc.Text));
				Patients.UpdateAging(allGuarantors[i],Ledgers.Bal[0],Ledgers.Bal[1],Ledgers.Bal[2]
					,Ledgers.Bal[3],Ledgers.InsEst,Ledgers.BalTotal);
			}
			if(Prefs.UpdateString("DateLastAging",POut.PDate(DateTime.Today,false))){
				DataValid.SetInvalid(InvalidTypes.Prefs);
			}
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Aging Complete");
			DialogResult=DialogResult.OK;
		}

	}
}
