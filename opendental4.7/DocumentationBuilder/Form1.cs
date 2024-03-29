//How to format comments to trigger links:
//FK to definition.DefNum is triggered by "FK to ".  It then looks for ".".  So anything can follow after.
//and:
//"Enum:" Then, the enum name must follow.  It must then be followed by a space or by nothing at all.  NO PERIOD allowed.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace DocumentationBuilder {
	public partial class Form1:Form {
		DataConnection dcon;
		string command;
		XPathNavigator Navigator;

		public Form1() {
			dcon=new DataConnection();
			InitializeComponent();
		}

		private void Form1_Load(object sender,EventArgs e) {
			textConnStr.Text=dcon.ConnStr;
		}

		private void butBuild_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			//dcon=new DataConnection();
			command="SHOW TABLES";
			DataTable table=dcon.GetTable(command);
			string outputFile=@"..\..\OpenDentalDocumentation.xml";
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			//input:
			string inputFile=@"..\..\..\bin\Release\OpenDental.xml";
			XmlDocument document=new XmlDocument();
			document.Load(inputFile);
			Navigator=document.CreateNavigator();
			using(XmlWriter writer=XmlWriter.Create(outputFile, settings)){
				//<?xml-stylesheet type="text/xsl" href="OpenDentalDocumentation.xsl"?>
				writer.WriteProcessingInstruction("xml-stylesheet","type=\"text/xsl\" href=\"OpenDentalDocumentation.xsl\"");
					//("<?xml-stylesheet type=\"text/xsl\" href=\"OpenDentalDocumentation.xsl\"?>");
				writer.WriteStartElement("database");
				writer.WriteAttributeString("version",textVersion.Text);
				for(int i=0;i<table.Rows.Count;i++){
					WriteTable(writer,table.Rows[i][0].ToString());
				}
				writer.WriteEndElement();
				writer.Flush();
			}
			//ProcessStartInfo startInfo=new ProcessStartInfo();
			Process.Start("Notepad.exe",outputFile);
			Application.Exit();
		}

		private void WriteTable(XmlWriter writer,string tableName){
			writer.WriteStartElement("table");
			writer.WriteAttributeString("name",tableName);
			//table summary
			writer.WriteStartElement("summary");
			writer.WriteString(GetSummary("T:OpenDental."+GetTableName(tableName)));
			writer.WriteEndElement();
			command="SHOW COLUMNS FROM "+tableName;
			DataTable table=dcon.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				WriteColumn(writer,i,tableName,table.Rows[i][0].ToString(),table.Rows[i][1].ToString());
			}
			writer.WriteEndElement();
		}

		private void WriteColumn(XmlWriter writer,int order,string tableName,
			string colName,string sqlType)
		{
			
			writer.WriteStartElement("column");
			writer.WriteAttributeString("order",order.ToString());
			writer.WriteAttributeString("name",colName);
			if(sqlType=="tinyint(3) unsigned") {
				sqlType="tinyint";
			}
			else if(sqlType=="tinyint(1) unsigned") {//not used very much
				sqlType="tinyint";
			}
			else if(sqlType=="smallint(5) unsigned") {
				sqlType="smallint";
			}
			else if(sqlType=="mediumint(8) unsigned") {
				sqlType="mediumint";
			}
			else if(sqlType.EndsWith(" unsigned")){
				sqlType=sqlType.Substring(0,sqlType.Length-9);
			}
			writer.WriteAttributeString("type",sqlType);
			string summary=GetSummary("F:OpenDental."+GetTableName(tableName)+"."+colName);
			if(summary.StartsWith("FK to ")){//eg FK to definition.DefNum
				int indexDot=summary.IndexOf(".");
				if(indexDot!=-1){
					string fkTable=summary.Substring(6,indexDot-6);
					writer.WriteAttributeString("fk",fkTable);
				}
			}
			//column summary
			writer.WriteStartElement("summary");
			writer.WriteString(summary);
			writer.WriteEndElement();
			if(summary.StartsWith("Enum:")){
				int indexSpace=summary.IndexOf(" ");//the space will be found after the name of the enum
				string enumName="";
				if(indexSpace==-1 && summary.Length>5){//Enum is listed, but no other comments.
					enumName=summary.Substring(5);
				}
				else if(indexSpace > 5){//This if statement just protects against a space right after the Enum:
					enumName=summary.Substring(5,indexSpace-5);
				}
				if(enumName!=""){
					WriteEnum(writer,enumName);
				}
			}
			writer.WriteEndElement();
		}

		private void WriteEnum(XmlWriter writer,string enumName){
			//get an ordered list from OpenDental.xml
			//T:OpenDental.AccountType
			//first the summary for the enum itsef
			XPathNavigator navEnum=Navigator.SelectSingleNode("//member[@name='T:OpenDental."+enumName+"']");
			if(navEnum==null) {
				return;
			}
			string summary=navEnum.Value;
			writer.WriteStartElement("Enumeration");
			writer.WriteAttributeString("name",enumName);
			writer.WriteElementString("summary",summary);
			//now, the individual enumsItems
			//F:OpenDental.AccountType.Asset
			//*[starts-with(name(),'B')]
			XPathNodeIterator nodes=Navigator.Select("//member[contains(@name,'F:OpenDental."+enumName+".')]");
				//("//member[@name='F:OpenDental."+enumName+".*']");
			string itemName;
			int lastDot;
			while(nodes.MoveNext()) {
				writer.WriteStartElement("EnumValue");
				summary=nodes.Current.Value;
				//nodes.Current.MoveToAttribute("name",null);
				itemName=nodes.Current.GetAttribute("name","");
				lastDot=itemName.LastIndexOf(".");
				itemName=itemName.Substring(lastDot+1);
				writer.WriteAttributeString("name",itemName);
				writer.WriteString(summary);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		///<summary>Gets the tablename that's used in the program based on the database tablename.  They are usually the same, except for capitalization.</summary>
		private string GetTableName(string dbTable){
			switch(dbTable){
				case "accountingautopay": return "AccountingAutoPay";
				case "appointmentrule": return "AppointmentRule";
				case "apptview": return "ApptView";
				case "apptviewitem": return "ApptViewItem";
				case "autocode": return "AutoCode";
				case "autocodecond": return "AutoCodeCond";
				case "autocodeitem": return "AutoCodeItem";
				case "claimform": return "ClaimForm";
				case "claimformitem": return "ClaimFormItem";
				case "claimpayment": return "ClaimPayment";
				case "claimproc": return "ClaimProc";
				case "clockevent": return "ClockEvent";
				case "covcat": return "CovCat";
				case "covspan": return "CovSpan";
				case "definition": return "Def";
				case "diseasedef": return "DiseaseDef";
				case "docattach": return "DocAttach";
				case "electid": return "ElectID";
				case "emailmessage": return "EmailMessage";
				case "emailtemplate": return "EmailTemplate";
				case "graphicassembly": return "GraphicAssembly";
				case "grouppermission": return "GroupPermission";
				case "insplan": return "InsPlan";
				case "journalentry": return "JournalEntry";
				case "languageforeign": return "LanguageForeign";
				case "lettermerge": return "LetterMerge";
				case "lettermergefield": return "LetterMergeField";
				case "medicationpat": return "MedicationPat";
				case "patfield": return "PatField";
				case "patfielddef": return "PatFieldDef";
				case "patientnote": return "PatientNote";
				case "patplan": return "PatPlan";
				case "payperiod": return "PayPeriod";
				case "payplan": return "PayPlan";
				case "payplancharge": return "PayPlanCharge";
				case "paysplit": return "PaySplit";
				case "perioexam": return "PerioExam";
				case "periomeasure": return "PerioMeasure";
				case "procbutton": return "ProcButton";
				case "procbuttonitem": return "ProcButtonItem";
				case "procedurecode": return "ProcedureCode";
				case "procedurelog": return "Procedure";
				case "proctp": return "ProcTP";
				case "programproperty": return "ProgramProperty";
				case "providerident": return "ProviderIdent";
				case "questiondef": return "QuestionDef";
				case "quickpastecat": return "QuickPasteCat";
				case "quickpastenote": return "QuickPasteNote";
				case "refattach": return "RefAttach";
				case "repeatcharge": return "RepeatCharge";
				case "rxalert": return "RxAlert";
				case "rxdef": return "RxDef";
				case "rxpat": return "RxPat";
				case "scheddefault": return "SchedDefault";
				case "schoolclass": return "SchoolClass";
				case "schoolcourse": return "SchoolCourse";
				case "screengroup": return "ScreenGroup";
				case "securitylog": return "SecurityLog";
				case "sigbutdef": return "SigButDef";
				case "sigbutdefelement": return "SigButDefElement";
				case "sigelement": return "SigElement";
				case "sigelementdef": return "SigElementDef";
				case "tasklist": return "TaskList";
				case "terminalactive": return "TerminalActive";
				case "timeadjust": return "TimeAdjust";
				case "toolbutitem": return "ToolButItem";
				case "toothinitial": return "ToothInitial";
				case "treatplan": return "TreatPlan";
				case "usergroup": return "UserGroup";
				case "userquery": return "UserQuery";
				case "zipcode": return "ZipCode";
			}
			return dbTable.Substring(0,1).ToUpper()+dbTable.Substring(1);
		}

		///<summary>Gets the summary from the xml file.  The full and correct member name must be supplied.</summary>
		private string GetSummary(string member){
			XPathNavigator navOne=Navigator.SelectSingleNode("//member[@name='"+member+"']");
			if(navOne==null){
				return "";
			}
			return navOne.SelectSingleNode("summary").Value;
			//F:OpenDental.ReportParameter.DefaultValues']").Value;
		}

		






	}
}