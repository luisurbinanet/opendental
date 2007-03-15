/*=============================================================================================================
Open Dental is a dental practice management program.
Copyright (C) 2003,2004,2005,2006  Jordan Sparks, DMD.  http://www.open-dent.com,  http://www.docsparks.com

This program is free software; you can redistribute it and/or modify it under the terms of the
GNU General Public License as published by the Free Software Foundation; either version 2 of the License,
or (at your option) any later version.

This program is distributed in the hope that it will be useful, but without any warranty. See the GNU General Public License
for more details, available at http://www.opensource.org/licenses/gpl-license.php

Any changes to this program must follow the guidelines of the GPL license if a modified version is to be
redistributed.
===============================================================================================================*/
//For now, all screens are assumed to have available 990x734.  That would be a screen resolution of 1024x768 with a single width taskbar docked to any one of the four sides of the screen.

//The 7 main controls are slightly narrower due to menu bar on left of 51. Max size 939x708

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormOpenDental:System.Windows.Forms.Form {
		private System.ComponentModel.IContainer components;
		//private bool[,] buttonDown=new bool[2,6];
		private System.Windows.Forms.Timer timerTimeIndic;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemSettings;
		private System.Windows.Forms.MenuItem menuItemReports;
		private System.Windows.Forms.MenuItem menuItemPrinter;
		private System.Windows.Forms.MenuItem menuItemImaging;
		private System.Windows.Forms.MenuItem menuItemDataPath;
		private System.Windows.Forms.MenuItem menuItemConfig;
		private System.Windows.Forms.MenuItem menuItemAutoCodes;
		private System.Windows.Forms.MenuItem menuItemDefinitions;
		private System.Windows.Forms.MenuItem menuItemInsCats;
		private System.Windows.Forms.MenuItem menuItemLinks;
		private System.Windows.Forms.MenuItem menuItemRecall;
		private System.Windows.Forms.MenuItem menuItemEmployees;
		private System.Windows.Forms.MenuItem menuItemPractice;
		private System.Windows.Forms.MenuItem menuItemPrescriptions;
		private System.Windows.Forms.MenuItem menuItemProviders;
		private System.Windows.Forms.MenuItem menuItemProcCodes;
		private System.Windows.Forms.MenuItem menuItemPracDef;
		private System.Windows.Forms.MenuItem menuItemPracSched;
		private System.Windows.Forms.MenuItem menuItemPrintScreen;
		private System.Windows.Forms.MenuItem menuItemFinanceCharge;
		private System.Windows.Forms.MenuItem menuItemAging;
		private System.Windows.Forms.MenuItem menuItemDaily;
		private System.Windows.Forms.MenuItem menuItemRpProc;
		private System.Windows.Forms.MenuItem menuItemRpPay;
		private System.Windows.Forms.MenuItem menuItemRpAdj;
		private System.Windows.Forms.MenuItem menuItemMonthly;
		private System.Windows.Forms.MenuItem menuItemRpOutInsClaims;
		private System.Windows.Forms.MenuItem menuItemSched;
		private System.Windows.Forms.MenuItem menuItemRpAging;
		private System.Windows.Forms.MenuItem menuItemRpProcNoBilled;
		private System.Windows.Forms.MenuItem menuItemRpClaimsNotSent;
		private System.Windows.Forms.MenuItem menuItemRpFinanceCharge;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItemUserQuery;
		private System.Windows.Forms.MenuItem menuItemList;
		private System.Windows.Forms.MenuItem menuItemTranslation;
		private System.Windows.Forms.MenuItem menuItemPatList;
		private System.Windows.Forms.MenuItem menuItemRpProcCodes;
		private System.Windows.Forms.MenuItem menuItemRxs;
		private System.Windows.Forms.MenuItem menuItemRefs;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItemLists;
		private System.Windows.Forms.MenuItem menuItemTools;
		private System.Windows.Forms.MenuItem menuItemReferrals;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemDatabaseMaintenance;
		private System.Windows.Forms.MenuItem menuItemProcedureButtons;
		private System.Windows.Forms.MenuItem menuItemZipCodes;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuTelephone;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItemHelpIndex;
		private System.Windows.Forms.MenuItem menuItemClaimForms;
		private System.Windows.Forms.MenuItem menuItemContacts;
		private System.Windows.Forms.MenuItem menuItemMedications;
		private OpenDental.OutlookBar myOutlookBar;
		private System.Windows.Forms.ImageList imageList32;
		private System.Windows.Forms.MenuItem menuItemApptViews;
		private System.Windows.Forms.MenuItem menuItemRpCapitation;
		private System.Windows.Forms.MenuItem menuItemPracticeWebReports;
		private System.Windows.Forms.MenuItem menuItemComputers;
		private System.Windows.Forms.MenuItem menuItemEmployers;
		private System.Windows.Forms.MenuItem menuItemEasy;
		private System.Windows.Forms.MenuItem menuItemCarriers;
		private System.Windows.Forms.MenuItem menuItemSchools;
		private System.Windows.Forms.MenuItem menuItemCounties;
		private System.Windows.Forms.MenuItem menuItemScreening;
		private System.Windows.Forms.MenuItem menuItemPHSep;
		private System.Windows.Forms.MenuItem menuItemPHRawScreen;
		private System.Windows.Forms.MenuItem menuItemPHRawPop;
		private System.Windows.Forms.MenuItem menuItemPHScreen;
		private System.Windows.Forms.MenuItem menuItemInsCarriers;
		private System.Windows.Forms.MenuItem menuItemEmail;
		private System.Windows.Forms.MenuItem menuItemHelpContents;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItemRpProdInc;
		private System.Windows.Forms.MenuItem menuAppointments;
		private Image buttonsShadow;
		///<summary>The only reason this is public static is so that it can be seen from the terminal manager.  Otherwise, it's passed around properly.</summary>
		public static int CurPatNum;
		private System.Windows.Forms.MenuItem menuItemClearinghouses;
		private System.Windows.Forms.MenuItem menuItemUpdate;
		private System.Windows.Forms.MenuItem menuItemRpProcNote;
		private System.Windows.Forms.MenuItem menuItemHelpWindows;
		private System.Windows.Forms.MenuItem menuItemMisc;
		private System.Windows.Forms.MenuItem menuItemBirthdays;
		private System.Windows.Forms.MenuItem menuItemProvDefault;
		private System.Windows.Forms.MenuItem menuItemProvSched;
		private System.Windows.Forms.MenuItem menuItemBlockoutDefault;
		private System.Windows.Forms.MenuItem menuItemRemote;
		private System.Windows.Forms.MenuItem menuItemInstructors;
		private System.Windows.Forms.MenuItem menuItemSchoolClass;
		private System.Windows.Forms.MenuItem menuItemSchoolCourses;
		private System.Windows.Forms.MenuItem menuItemCourseGrades;
		private System.Windows.Forms.MenuItem menuItemPatientImport;
		private System.Windows.Forms.MenuItem menuItemSecurity;
		private System.Windows.Forms.MenuItem menuItemLogOff;
		//<summary>Will be true if this is the second instance of Open Dental running on this computer. This might happen with terminal services or fast user switching.  If true, then the message listening is disabled.  This might cause synchronisation issues if used extensively.  A timed synchronization is planned for this situation.</summary>
		//private static bool IsSecondInstance;
		private System.Windows.Forms.MenuItem menuItemInsPlans;
		private System.Windows.Forms.MenuItem menuItemClinics;
		private System.Windows.Forms.MenuItem menuItemOperatories;
		private System.Windows.Forms.Timer timerSignals;
		///<summary>When user logs out, this keeps track of where they were for when they log back in.</summary>
		private int LastModule;
		private System.Windows.Forms.MenuItem menuItemRDLReport;
		private System.Windows.Forms.MenuItem menuItemReportsSetup;
		private System.Windows.Forms.MenuItem menuItemRepeatingCharges;
		private System.Windows.Forms.MenuItem menuItemImportXML;
		private MenuItem menuItemPayPeriods;
		private MenuItem menuItemApptRules;
		private MenuItem menuItemRouting;
		private MenuItem menuItemAuditTrail;
		private MenuItem menuItemPatFieldDefs;
		private MenuItem menuItemDiseases;
		private MenuItem menuItemTerminal;
		private MenuItem menuItemTerminalManager;
		private MenuItem menuItemQuestions;
		private MenuItem menuItemCustomReports;
		private MenuItem menuItemMessaging;
		private OpenDental.UI.LightSignalGrid lightSignalGrid1;
		private MenuItem menuItemMessagingButs;
		///<summary>This is not the actual date/time last refreshed.  It is really the date/time of the last item in the database retrieved on previous refreshes.  That way, the local workstation time is irrelevant.</summary>
		private DateTime signalLastRefreshed;
		private FormSplash Splash;//SPK/AAD 1/10/07
		private Bitmap bitmapIcon;
		private MenuItem menuItemRpWriteoff;
		private ContrAppt ContrAppt2;
		private ContrFamily ContrFamily2;
		private ContrAccount ContrAccount2;
		private ContrTreat ContrTreat2;
		private ContrDocs ContrDocs2;
		///<summary>A list of button definitions for this computer.</summary>
		private SigButDef[] SigButDefList;

		///<summary></summary>
		public FormOpenDental(){
			Logger.openlog.level=Logger.Severity.DEBUG;//Allow logging of all messages. Only use in-house, or on a particular customer.
			//Logger.openlog.level=Logger.Severity.INFO;//Allow logging of all messages, except debug messages.
			Splash=new FormSplash();//SPK/AAD 1/10/07
			Splash.Show();//SPK/AAD 1/10/07
			InitializeComponent();
			ContrAppt2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			ContrFamily2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			ContrAccount2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			ContrTreat2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
//			ContrChart2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
			ContrDocs2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);
//			ContrManage2.PatientSelected+=new PatientSelectedEventHandler(Contr_PatientSelected);*/
			GotoModule.ModuleSelected+=new ModuleEventHandler(GotoModule_ModuleSelected);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOpenDental));
			this.timerTimeIndic = new System.Windows.Forms.Timer(this.components);
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemLogOff = new System.Windows.Forms.MenuItem();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemPrinter = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItemConfig = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.menuItemApptRules = new System.Windows.Forms.MenuItem();
			this.menuItemApptViews = new System.Windows.Forms.MenuItem();
			this.menuItemAutoCodes = new System.Windows.Forms.MenuItem();
			this.menuItemClaimForms = new System.Windows.Forms.MenuItem();
			this.menuItemClearinghouses = new System.Windows.Forms.MenuItem();
			this.menuItemComputers = new System.Windows.Forms.MenuItem();
			this.menuItemDataPath = new System.Windows.Forms.MenuItem();
			this.menuItemDefinitions = new System.Windows.Forms.MenuItem();
			this.menuItemDiseases = new System.Windows.Forms.MenuItem();
			this.menuItemEasy = new System.Windows.Forms.MenuItem();
			this.menuItemEmail = new System.Windows.Forms.MenuItem();
			this.menuItemImaging = new System.Windows.Forms.MenuItem();
			this.menuItemInsCats = new System.Windows.Forms.MenuItem();
			this.menuItemMessaging = new System.Windows.Forms.MenuItem();
			this.menuItemMessagingButs = new System.Windows.Forms.MenuItem();
			this.menuItemMisc = new System.Windows.Forms.MenuItem();
			this.menuItemOperatories = new System.Windows.Forms.MenuItem();
			this.menuItemPatFieldDefs = new System.Windows.Forms.MenuItem();
			this.menuItemPayPeriods = new System.Windows.Forms.MenuItem();
			this.menuItemPractice = new System.Windows.Forms.MenuItem();
			this.menuItemProcedureButtons = new System.Windows.Forms.MenuItem();
			this.menuItemLinks = new System.Windows.Forms.MenuItem();
			this.menuItemQuestions = new System.Windows.Forms.MenuItem();
			this.menuItemRecall = new System.Windows.Forms.MenuItem();
			this.menuItemSecurity = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItemSched = new System.Windows.Forms.MenuItem();
			this.menuItemPracDef = new System.Windows.Forms.MenuItem();
			this.menuItemPracSched = new System.Windows.Forms.MenuItem();
			this.menuItemProvDefault = new System.Windows.Forms.MenuItem();
			this.menuItemProvSched = new System.Windows.Forms.MenuItem();
			this.menuItemBlockoutDefault = new System.Windows.Forms.MenuItem();
			this.menuItemLists = new System.Windows.Forms.MenuItem();
			this.menuItemProcCodes = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemClinics = new System.Windows.Forms.MenuItem();
			this.menuItemContacts = new System.Windows.Forms.MenuItem();
			this.menuItemCounties = new System.Windows.Forms.MenuItem();
			this.menuItemSchoolClass = new System.Windows.Forms.MenuItem();
			this.menuItemSchoolCourses = new System.Windows.Forms.MenuItem();
			this.menuItemEmployees = new System.Windows.Forms.MenuItem();
			this.menuItemEmployers = new System.Windows.Forms.MenuItem();
			this.menuItemInstructors = new System.Windows.Forms.MenuItem();
			this.menuItemCarriers = new System.Windows.Forms.MenuItem();
			this.menuItemInsPlans = new System.Windows.Forms.MenuItem();
			this.menuItemMedications = new System.Windows.Forms.MenuItem();
			this.menuItemProviders = new System.Windows.Forms.MenuItem();
			this.menuItemPrescriptions = new System.Windows.Forms.MenuItem();
			this.menuItemReferrals = new System.Windows.Forms.MenuItem();
			this.menuItemSchools = new System.Windows.Forms.MenuItem();
			this.menuItemZipCodes = new System.Windows.Forms.MenuItem();
			this.menuItemReports = new System.Windows.Forms.MenuItem();
			this.menuItemRDLReport = new System.Windows.Forms.MenuItem();
			this.menuItemReportsSetup = new System.Windows.Forms.MenuItem();
			this.menuItemUserQuery = new System.Windows.Forms.MenuItem();
			this.menuItemPracticeWebReports = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItemRpProdInc = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemDaily = new System.Windows.Forms.MenuItem();
			this.menuItemRpAdj = new System.Windows.Forms.MenuItem();
			this.menuItemRpPay = new System.Windows.Forms.MenuItem();
			this.menuItemRpProc = new System.Windows.Forms.MenuItem();
			this.menuItemRpWriteoff = new System.Windows.Forms.MenuItem();
			this.menuItemRpProcNote = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemMonthly = new System.Windows.Forms.MenuItem();
			this.menuItemRpAging = new System.Windows.Forms.MenuItem();
			this.menuItemRpClaimsNotSent = new System.Windows.Forms.MenuItem();
			this.menuItemRpCapitation = new System.Windows.Forms.MenuItem();
			this.menuItemRpFinanceCharge = new System.Windows.Forms.MenuItem();
			this.menuItemRpOutInsClaims = new System.Windows.Forms.MenuItem();
			this.menuItemRpProcNoBilled = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItemList = new System.Windows.Forms.MenuItem();
			this.menuAppointments = new System.Windows.Forms.MenuItem();
			this.menuItemBirthdays = new System.Windows.Forms.MenuItem();
			this.menuItemInsCarriers = new System.Windows.Forms.MenuItem();
			this.menuItemPatList = new System.Windows.Forms.MenuItem();
			this.menuItemRxs = new System.Windows.Forms.MenuItem();
			this.menuItemRpProcCodes = new System.Windows.Forms.MenuItem();
			this.menuItemRefs = new System.Windows.Forms.MenuItem();
			this.menuItemRouting = new System.Windows.Forms.MenuItem();
			this.menuItemPHSep = new System.Windows.Forms.MenuItem();
			this.menuItemPHRawScreen = new System.Windows.Forms.MenuItem();
			this.menuItemPHRawPop = new System.Windows.Forms.MenuItem();
			this.menuItemPHScreen = new System.Windows.Forms.MenuItem();
			this.menuItemCourseGrades = new System.Windows.Forms.MenuItem();
			this.menuItemCustomReports = new System.Windows.Forms.MenuItem();
			this.menuItemTools = new System.Windows.Forms.MenuItem();
			this.menuItemPrintScreen = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuTelephone = new System.Windows.Forms.MenuItem();
			this.menuItemPatientImport = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItemAuditTrail = new System.Windows.Forms.MenuItem();
			this.menuItemDatabaseMaintenance = new System.Windows.Forms.MenuItem();
			this.menuItemImportXML = new System.Windows.Forms.MenuItem();
			this.menuItemAging = new System.Windows.Forms.MenuItem();
			this.menuItemFinanceCharge = new System.Windows.Forms.MenuItem();
			this.menuItemRepeatingCharges = new System.Windows.Forms.MenuItem();
			this.menuItemTranslation = new System.Windows.Forms.MenuItem();
			this.menuItemScreening = new System.Windows.Forms.MenuItem();
			this.menuItemTerminal = new System.Windows.Forms.MenuItem();
			this.menuItemTerminalManager = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemHelpWindows = new System.Windows.Forms.MenuItem();
			this.menuItemHelpContents = new System.Windows.Forms.MenuItem();
			this.menuItemHelpIndex = new System.Windows.Forms.MenuItem();
			this.menuItemRemote = new System.Windows.Forms.MenuItem();
			this.menuItemUpdate = new System.Windows.Forms.MenuItem();
			this.imageList32 = new System.Windows.Forms.ImageList(this.components);
			this.timerSignals = new System.Windows.Forms.Timer(this.components);
			this.ContrAppt2 = new OpenDental.ContrAppt();
			this.lightSignalGrid1 = new OpenDental.UI.LightSignalGrid();
			this.myOutlookBar = new OpenDental.OutlookBar();
			this.ContrFamily2 = new OpenDental.ContrFamily();
			this.ContrAccount2 = new OpenDental.ContrAccount();
			this.ContrTreat2 = new OpenDental.ContrTreat();
			this.ContrDocs2 = new OpenDental.ContrDocs();
			this.SuspendLayout();
			// 
			// timerTimeIndic
			// 
			this.timerTimeIndic.Interval = 60000;
			this.timerTimeIndic.Tick += new System.EventHandler(this.timerTimeIndic_Tick);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLogOff,
            this.menuItemFile,
            this.menuItemSettings,
            this.menuItemLists,
            this.menuItemReports,
            this.menuItemCustomReports,
            this.menuItemTools,
            this.menuItemHelp});
			// 
			// menuItemLogOff
			// 
			this.menuItemLogOff.Index = 0;
			this.menuItemLogOff.Text = "Log &Off";
			this.menuItemLogOff.Click += new System.EventHandler(this.menuItemLogOff_Click);
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 1;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPrinter,
            this.menuItem6,
            this.menuItemConfig,
            this.menuItem7,
            this.menuItemExit});
			this.menuItemFile.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuItemFile.Text = "&File";
			// 
			// menuItemPrinter
			// 
			this.menuItemPrinter.Index = 0;
			this.menuItemPrinter.Text = "&Printers";
			this.menuItemPrinter.Click += new System.EventHandler(this.menuItemPrinter_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.Text = "-";
			// 
			// menuItemConfig
			// 
			this.menuItemConfig.Index = 2;
			this.menuItemConfig.Text = "&Choose Database";
			this.menuItemConfig.Click += new System.EventHandler(this.menuItemConfig_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 3;
			this.menuItem7.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 4;
			this.menuItemExit.ShowShortcut = false;
			this.menuItemExit.Text = "E&xit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.Index = 2;
			this.menuItemSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemApptRules,
            this.menuItemApptViews,
            this.menuItemAutoCodes,
            this.menuItemClaimForms,
            this.menuItemClearinghouses,
            this.menuItemComputers,
            this.menuItemDataPath,
            this.menuItemDefinitions,
            this.menuItemDiseases,
            this.menuItemEasy,
            this.menuItemEmail,
            this.menuItemImaging,
            this.menuItemInsCats,
            this.menuItemMessaging,
            this.menuItemMessagingButs,
            this.menuItemMisc,
            this.menuItemOperatories,
            this.menuItemPatFieldDefs,
            this.menuItemPayPeriods,
            this.menuItemPractice,
            this.menuItemProcedureButtons,
            this.menuItemLinks,
            this.menuItemQuestions,
            this.menuItemRecall,
            this.menuItemSecurity,
            this.menuItem10,
            this.menuItemSched,
            this.menuItemPracDef,
            this.menuItemPracSched,
            this.menuItemProvDefault,
            this.menuItemProvSched,
            this.menuItemBlockoutDefault});
			this.menuItemSettings.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuItemSettings.Text = "&Setup";
			// 
			// menuItemApptRules
			// 
			this.menuItemApptRules.Index = 0;
			this.menuItemApptRules.Text = "Appointment Rules";
			this.menuItemApptRules.Click += new System.EventHandler(this.menuItemApptRules_Click);
			// 
			// menuItemApptViews
			// 
			this.menuItemApptViews.Index = 1;
			this.menuItemApptViews.Text = "Appointment Views";
			this.menuItemApptViews.Click += new System.EventHandler(this.menuItemApptViews_Click);
			// 
			// menuItemAutoCodes
			// 
			this.menuItemAutoCodes.Index = 2;
			this.menuItemAutoCodes.Text = "Auto Codes";
			this.menuItemAutoCodes.Click += new System.EventHandler(this.menuItemAutoCodes_Click);
			// 
			// menuItemClaimForms
			// 
			this.menuItemClaimForms.Index = 3;
			this.menuItemClaimForms.Text = "Claim Forms";
			this.menuItemClaimForms.Click += new System.EventHandler(this.menuItemClaimForms_Click);
			// 
			// menuItemClearinghouses
			// 
			this.menuItemClearinghouses.Index = 4;
			this.menuItemClearinghouses.Text = "Clearinghouses";
			this.menuItemClearinghouses.Click += new System.EventHandler(this.menuItemClearinghouses_Click);
			// 
			// menuItemComputers
			// 
			this.menuItemComputers.Index = 5;
			this.menuItemComputers.Text = "Computers";
			this.menuItemComputers.Click += new System.EventHandler(this.menuItemComputers_Click);
			// 
			// menuItemDataPath
			// 
			this.menuItemDataPath.Index = 6;
			this.menuItemDataPath.Text = "Data Paths";
			this.menuItemDataPath.Click += new System.EventHandler(this.menuItemDataPath_Click);
			// 
			// menuItemDefinitions
			// 
			this.menuItemDefinitions.Index = 7;
			this.menuItemDefinitions.Text = "Definitions";
			this.menuItemDefinitions.Click += new System.EventHandler(this.menuItemDefinitions_Click);
			// 
			// menuItemDiseases
			// 
			this.menuItemDiseases.Index = 8;
			this.menuItemDiseases.Text = "Diseases";
			this.menuItemDiseases.Click += new System.EventHandler(this.menuItemDiseases_Click);
			// 
			// menuItemEasy
			// 
			this.menuItemEasy.Index = 9;
			this.menuItemEasy.Text = "Easy Options";
			this.menuItemEasy.Click += new System.EventHandler(this.menuItemEasy_Click);
			// 
			// menuItemEmail
			// 
			this.menuItemEmail.Index = 10;
			this.menuItemEmail.Text = "E-mail";
			this.menuItemEmail.Click += new System.EventHandler(this.menuItemEmail_Click);
			// 
			// menuItemImaging
			// 
			this.menuItemImaging.Index = 11;
			this.menuItemImaging.Text = "Imaging";
			this.menuItemImaging.Click += new System.EventHandler(this.menuItemImaging_Click);
			// 
			// menuItemInsCats
			// 
			this.menuItemInsCats.Index = 12;
			this.menuItemInsCats.Text = "Insurance Categories";
			this.menuItemInsCats.Click += new System.EventHandler(this.menuItemInsCats_Click);
			// 
			// menuItemMessaging
			// 
			this.menuItemMessaging.Index = 13;
			this.menuItemMessaging.Text = "Messaging";
			this.menuItemMessaging.Click += new System.EventHandler(this.menuItemMessaging_Click);
			// 
			// menuItemMessagingButs
			// 
			this.menuItemMessagingButs.Index = 14;
			this.menuItemMessagingButs.Text = "Messaging Buttons";
			this.menuItemMessagingButs.Click += new System.EventHandler(this.menuItemMessagingButs_Click);
			// 
			// menuItemMisc
			// 
			this.menuItemMisc.Index = 15;
			this.menuItemMisc.Text = "Miscellaneous";
			this.menuItemMisc.Click += new System.EventHandler(this.menuItemMisc_Click);
			// 
			// menuItemOperatories
			// 
			this.menuItemOperatories.Index = 16;
			this.menuItemOperatories.Text = "Operatories";
			this.menuItemOperatories.Click += new System.EventHandler(this.menuItemOperatories_Click);
			// 
			// menuItemPatFieldDefs
			// 
			this.menuItemPatFieldDefs.Index = 17;
			this.menuItemPatFieldDefs.Text = "Patient Field Defs";
			this.menuItemPatFieldDefs.Click += new System.EventHandler(this.menuItemPatFieldDefs_Click);
			// 
			// menuItemPayPeriods
			// 
			this.menuItemPayPeriods.Index = 18;
			this.menuItemPayPeriods.Text = "Pay Periods";
			this.menuItemPayPeriods.Click += new System.EventHandler(this.menuItemPayPeriods_Click);
			// 
			// menuItemPractice
			// 
			this.menuItemPractice.Index = 19;
			this.menuItemPractice.Text = "Practice";
			this.menuItemPractice.Click += new System.EventHandler(this.menuItemPractice_Click);
			// 
			// menuItemProcedureButtons
			// 
			this.menuItemProcedureButtons.Index = 20;
			this.menuItemProcedureButtons.Text = "Procedure Buttons";
			this.menuItemProcedureButtons.Click += new System.EventHandler(this.menuItemProcedureButtons_Click);
			// 
			// menuItemLinks
			// 
			this.menuItemLinks.Index = 21;
			this.menuItemLinks.Text = "Program Links";
			this.menuItemLinks.Click += new System.EventHandler(this.menuItemLinks_Click);
			// 
			// menuItemQuestions
			// 
			this.menuItemQuestions.Index = 22;
			this.menuItemQuestions.Text = "Questionnaire";
			this.menuItemQuestions.Click += new System.EventHandler(this.menuItemQuestions_Click);
			// 
			// menuItemRecall
			// 
			this.menuItemRecall.Index = 23;
			this.menuItemRecall.Text = "Recall";
			this.menuItemRecall.Click += new System.EventHandler(this.menuItemRecall_Click);
			// 
			// menuItemSecurity
			// 
			this.menuItemSecurity.Index = 24;
			this.menuItemSecurity.Text = "Security";
			this.menuItemSecurity.Click += new System.EventHandler(this.menuItemSecurity_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 25;
			this.menuItem10.Text = "-";
			// 
			// menuItemSched
			// 
			this.menuItemSched.Index = 26;
			this.menuItemSched.Text = "SCHEDULES";
			// 
			// menuItemPracDef
			// 
			this.menuItemPracDef.Index = 27;
			this.menuItemPracDef.Text = "   Practice Default";
			this.menuItemPracDef.Click += new System.EventHandler(this.menuItemPracDef_Click);
			// 
			// menuItemPracSched
			// 
			this.menuItemPracSched.Index = 28;
			this.menuItemPracSched.Text = "   Practice";
			this.menuItemPracSched.Click += new System.EventHandler(this.menuItemPracSched_Click);
			// 
			// menuItemProvDefault
			// 
			this.menuItemProvDefault.Index = 29;
			this.menuItemProvDefault.Text = "   Provider Default";
			this.menuItemProvDefault.Click += new System.EventHandler(this.menuItemProvDefault_Click);
			// 
			// menuItemProvSched
			// 
			this.menuItemProvSched.Index = 30;
			this.menuItemProvSched.Text = "   Provider";
			this.menuItemProvSched.Click += new System.EventHandler(this.menuItemProvSched_Click);
			// 
			// menuItemBlockoutDefault
			// 
			this.menuItemBlockoutDefault.Index = 31;
			this.menuItemBlockoutDefault.Text = "   Blockout Default";
			this.menuItemBlockoutDefault.Click += new System.EventHandler(this.menuItemBlockoutDefault_Click);
			// 
			// menuItemLists
			// 
			this.menuItemLists.Index = 3;
			this.menuItemLists.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemProcCodes,
            this.menuItem5,
            this.menuItemClinics,
            this.menuItemContacts,
            this.menuItemCounties,
            this.menuItemSchoolClass,
            this.menuItemSchoolCourses,
            this.menuItemEmployees,
            this.menuItemEmployers,
            this.menuItemInstructors,
            this.menuItemCarriers,
            this.menuItemInsPlans,
            this.menuItemMedications,
            this.menuItemProviders,
            this.menuItemPrescriptions,
            this.menuItemReferrals,
            this.menuItemSchools,
            this.menuItemZipCodes});
			this.menuItemLists.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
			this.menuItemLists.Text = "&Lists";
			// 
			// menuItemProcCodes
			// 
			this.menuItemProcCodes.Index = 0;
			this.menuItemProcCodes.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF;
			this.menuItemProcCodes.Text = "&Procedure Codes";
			this.menuItemProcCodes.Click += new System.EventHandler(this.menuItemProcCodes_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "-";
			// 
			// menuItemClinics
			// 
			this.menuItemClinics.Index = 2;
			this.menuItemClinics.Text = "Clinics";
			this.menuItemClinics.Click += new System.EventHandler(this.menuItemClinics_Click);
			// 
			// menuItemContacts
			// 
			this.menuItemContacts.Index = 3;
			this.menuItemContacts.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftC;
			this.menuItemContacts.Text = "&Contacts";
			this.menuItemContacts.Click += new System.EventHandler(this.menuItemContacts_Click);
			// 
			// menuItemCounties
			// 
			this.menuItemCounties.Index = 4;
			this.menuItemCounties.Text = "Counties";
			this.menuItemCounties.Click += new System.EventHandler(this.menuItemCounties_Click);
			// 
			// menuItemSchoolClass
			// 
			this.menuItemSchoolClass.Index = 5;
			this.menuItemSchoolClass.Text = "Dental School Classes";
			this.menuItemSchoolClass.Click += new System.EventHandler(this.menuItemSchoolClass_Click);
			// 
			// menuItemSchoolCourses
			// 
			this.menuItemSchoolCourses.Index = 6;
			this.menuItemSchoolCourses.Text = "Dental School Courses";
			this.menuItemSchoolCourses.Click += new System.EventHandler(this.menuItemSchoolCourses_Click);
			// 
			// menuItemEmployees
			// 
			this.menuItemEmployees.Index = 7;
			this.menuItemEmployees.Text = "&Employees";
			this.menuItemEmployees.Click += new System.EventHandler(this.menuItemEmployees_Click);
			// 
			// menuItemEmployers
			// 
			this.menuItemEmployers.Index = 8;
			this.menuItemEmployers.Text = "Employers";
			this.menuItemEmployers.Click += new System.EventHandler(this.menuItemEmployers_Click);
			// 
			// menuItemInstructors
			// 
			this.menuItemInstructors.Index = 9;
			this.menuItemInstructors.Text = "Instructors";
			this.menuItemInstructors.Click += new System.EventHandler(this.menuItemInstructors_Click);
			// 
			// menuItemCarriers
			// 
			this.menuItemCarriers.Index = 10;
			this.menuItemCarriers.Text = "Insurance Carriers";
			this.menuItemCarriers.Click += new System.EventHandler(this.menuItemCarriers_Click);
			// 
			// menuItemInsPlans
			// 
			this.menuItemInsPlans.Index = 11;
			this.menuItemInsPlans.Text = "&Insurance Plans";
			this.menuItemInsPlans.Click += new System.EventHandler(this.menuItemInsPlans_Click);
			// 
			// menuItemMedications
			// 
			this.menuItemMedications.Index = 12;
			this.menuItemMedications.Text = "&Medications";
			this.menuItemMedications.Click += new System.EventHandler(this.menuItemMedications_Click);
			// 
			// menuItemProviders
			// 
			this.menuItemProviders.Index = 13;
			this.menuItemProviders.Text = "Providers";
			this.menuItemProviders.Click += new System.EventHandler(this.menuItemProviders_Click);
			// 
			// menuItemPrescriptions
			// 
			this.menuItemPrescriptions.Index = 14;
			this.menuItemPrescriptions.Text = "Pre&scriptions";
			this.menuItemPrescriptions.Click += new System.EventHandler(this.menuItemPrescriptions_Click);
			// 
			// menuItemReferrals
			// 
			this.menuItemReferrals.Index = 15;
			this.menuItemReferrals.Text = "&Referrals";
			this.menuItemReferrals.Click += new System.EventHandler(this.menuItemReferrals_Click);
			// 
			// menuItemSchools
			// 
			this.menuItemSchools.Index = 16;
			this.menuItemSchools.Text = "Sites";
			this.menuItemSchools.Click += new System.EventHandler(this.menuItemSchools_Click);
			// 
			// menuItemZipCodes
			// 
			this.menuItemZipCodes.Index = 17;
			this.menuItemZipCodes.Text = "&Zip Codes";
			this.menuItemZipCodes.Click += new System.EventHandler(this.menuItemZipCodes_Click);
			// 
			// menuItemReports
			// 
			this.menuItemReports.Index = 4;
			this.menuItemReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRDLReport,
            this.menuItemReportsSetup,
            this.menuItemUserQuery,
            this.menuItemPracticeWebReports,
            this.menuItem12,
            this.menuItemRpProdInc,
            this.menuItem2,
            this.menuItemDaily,
            this.menuItemRpAdj,
            this.menuItemRpPay,
            this.menuItemRpProc,
            this.menuItemRpWriteoff,
            this.menuItemRpProcNote,
            this.menuItem3,
            this.menuItemMonthly,
            this.menuItemRpAging,
            this.menuItemRpClaimsNotSent,
            this.menuItemRpCapitation,
            this.menuItemRpFinanceCharge,
            this.menuItemRpOutInsClaims,
            this.menuItemRpProcNoBilled,
            this.menuItem8,
            this.menuItemList,
            this.menuAppointments,
            this.menuItemBirthdays,
            this.menuItemInsCarriers,
            this.menuItemPatList,
            this.menuItemRxs,
            this.menuItemRpProcCodes,
            this.menuItemRefs,
            this.menuItemRouting,
            this.menuItemPHSep,
            this.menuItemPHRawScreen,
            this.menuItemPHRawPop,
            this.menuItemPHScreen,
            this.menuItemCourseGrades});
			this.menuItemReports.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.menuItemReports.Text = "&Reports";
			// 
			// menuItemRDLReport
			// 
			this.menuItemRDLReport.Index = 0;
			this.menuItemRDLReport.Text = "RDL Report";
			this.menuItemRDLReport.Visible = false;
			this.menuItemRDLReport.Click += new System.EventHandler(this.menuItemRDLReport_Click);
			// 
			// menuItemReportsSetup
			// 
			this.menuItemReportsSetup.Index = 1;
			this.menuItemReportsSetup.Text = "Setup Reports";
			this.menuItemReportsSetup.Visible = false;
			this.menuItemReportsSetup.Click += new System.EventHandler(this.menuItemReportsSetup_Click);
			// 
			// menuItemUserQuery
			// 
			this.menuItemUserQuery.Index = 2;
			this.menuItemUserQuery.Text = "&User Query";
			this.menuItemUserQuery.Click += new System.EventHandler(this.menuItemUserQuery_Click);
			// 
			// menuItemPracticeWebReports
			// 
			this.menuItemPracticeWebReports.Index = 3;
			this.menuItemPracticeWebReports.Text = "Other Reports";
			this.menuItemPracticeWebReports.Click += new System.EventHandler(this.menuItemPracticeWebReports_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 4;
			this.menuItem12.Text = "-";
			// 
			// menuItemRpProdInc
			// 
			this.menuItemRpProdInc.Index = 5;
			this.menuItemRpProdInc.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftP;
			this.menuItemRpProdInc.Text = "&Production and Income";
			this.menuItemRpProdInc.Click += new System.EventHandler(this.menuItemRpProdInc_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 6;
			this.menuItem2.Text = "-";
			// 
			// menuItemDaily
			// 
			this.menuItemDaily.Index = 7;
			this.menuItemDaily.Text = "DAILY";
			// 
			// menuItemRpAdj
			// 
			this.menuItemRpAdj.Index = 8;
			this.menuItemRpAdj.Text = "   &Adjustments";
			this.menuItemRpAdj.Click += new System.EventHandler(this.menuItemRpAdj_Click);
			// 
			// menuItemRpPay
			// 
			this.menuItemRpPay.Index = 9;
			this.menuItemRpPay.Text = "   Pa&yments";
			this.menuItemRpPay.Click += new System.EventHandler(this.menuItemRpPay_Click);
			// 
			// menuItemRpProc
			// 
			this.menuItemRpProc.Index = 10;
			this.menuItemRpProc.Text = "   P&rocedures";
			this.menuItemRpProc.Click += new System.EventHandler(this.menuItemRpProc_Click);
			// 
			// menuItemRpWriteoff
			// 
			this.menuItemRpWriteoff.Index = 11;
			this.menuItemRpWriteoff.Text = "   Writeoffs";
			this.menuItemRpWriteoff.Click += new System.EventHandler(this.menuItemRpWriteoff_Click);
			// 
			// menuItemRpProcNote
			// 
			this.menuItemRpProcNote.Index = 12;
			this.menuItemRpProcNote.Text = "   Incomplete Procedure Notes";
			this.menuItemRpProcNote.Click += new System.EventHandler(this.menuItemRpProcNote_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 13;
			this.menuItem3.Text = "-";
			// 
			// menuItemMonthly
			// 
			this.menuItemMonthly.Index = 14;
			this.menuItemMonthly.Text = "MONTHLY";
			// 
			// menuItemRpAging
			// 
			this.menuItemRpAging.Index = 15;
			this.menuItemRpAging.Text = "   Aging Report";
			this.menuItemRpAging.Click += new System.EventHandler(this.menuItemRpAging_Click);
			// 
			// menuItemRpClaimsNotSent
			// 
			this.menuItemRpClaimsNotSent.Index = 16;
			this.menuItemRpClaimsNotSent.Text = "   Claims Not Sent";
			this.menuItemRpClaimsNotSent.Click += new System.EventHandler(this.menuItemRpClaimsNotSent_Click);
			// 
			// menuItemRpCapitation
			// 
			this.menuItemRpCapitation.Index = 17;
			this.menuItemRpCapitation.Text = "   Capitation Utilization";
			this.menuItemRpCapitation.Click += new System.EventHandler(this.menuItemRpCapitation_Click);
			// 
			// menuItemRpFinanceCharge
			// 
			this.menuItemRpFinanceCharge.Index = 18;
			this.menuItemRpFinanceCharge.Text = "   Finance Charge Report";
			this.menuItemRpFinanceCharge.Click += new System.EventHandler(this.menuItemRpFinanceCharge_Click);
			// 
			// menuItemRpOutInsClaims
			// 
			this.menuItemRpOutInsClaims.Index = 19;
			this.menuItemRpOutInsClaims.Text = "   Outstanding Insurance Claims";
			this.menuItemRpOutInsClaims.Click += new System.EventHandler(this.menuItemRpOutInsClaims_Click);
			// 
			// menuItemRpProcNoBilled
			// 
			this.menuItemRpProcNoBilled.Index = 20;
			this.menuItemRpProcNoBilled.Text = "   Procedures Not Billed To Insurance";
			this.menuItemRpProcNoBilled.Click += new System.EventHandler(this.menuItemRpProcNoBilled_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 21;
			this.menuItem8.Text = "-";
			// 
			// menuItemList
			// 
			this.menuItemList.Index = 22;
			this.menuItemList.Text = "LISTS";
			// 
			// menuAppointments
			// 
			this.menuAppointments.Index = 23;
			this.menuAppointments.Text = "   Appointments";
			this.menuAppointments.Click += new System.EventHandler(this.menuAppointments_Click);
			// 
			// menuItemBirthdays
			// 
			this.menuItemBirthdays.Index = 24;
			this.menuItemBirthdays.Text = "   Birthdays";
			this.menuItemBirthdays.Click += new System.EventHandler(this.menuItemBirthdays_Click);
			// 
			// menuItemInsCarriers
			// 
			this.menuItemInsCarriers.Index = 25;
			this.menuItemInsCarriers.Text = "   Insurance Plans";
			this.menuItemInsCarriers.Click += new System.EventHandler(this.menuItemInsCarriers_Click);
			// 
			// menuItemPatList
			// 
			this.menuItemPatList.Index = 26;
			this.menuItemPatList.Text = "   Patients";
			this.menuItemPatList.Click += new System.EventHandler(this.menuItemPatList_Click);
			// 
			// menuItemRxs
			// 
			this.menuItemRxs.Index = 27;
			this.menuItemRxs.Text = "   Prescriptions";
			this.menuItemRxs.Click += new System.EventHandler(this.menuItemRxs_Click);
			// 
			// menuItemRpProcCodes
			// 
			this.menuItemRpProcCodes.Index = 28;
			this.menuItemRpProcCodes.Text = "   Procedure Codes";
			this.menuItemRpProcCodes.Click += new System.EventHandler(this.menuItemRpProcCodes_Click);
			// 
			// menuItemRefs
			// 
			this.menuItemRefs.Index = 29;
			this.menuItemRefs.Text = "   Referrals";
			this.menuItemRefs.Click += new System.EventHandler(this.menuItemRefs_Click);
			// 
			// menuItemRouting
			// 
			this.menuItemRouting.Index = 30;
			this.menuItemRouting.Text = "   Routing Slips";
			this.menuItemRouting.Click += new System.EventHandler(this.menuItemRouting_Click);
			// 
			// menuItemPHSep
			// 
			this.menuItemPHSep.Index = 31;
			this.menuItemPHSep.Text = "-";
			// 
			// menuItemPHRawScreen
			// 
			this.menuItemPHRawScreen.Index = 32;
			this.menuItemPHRawScreen.Text = "Raw Screening Data";
			this.menuItemPHRawScreen.Click += new System.EventHandler(this.menuItemPHRawScreen_Click);
			// 
			// menuItemPHRawPop
			// 
			this.menuItemPHRawPop.Index = 33;
			this.menuItemPHRawPop.Text = "Raw Population Data";
			this.menuItemPHRawPop.Click += new System.EventHandler(this.menuItemPHRawPop_Click);
			// 
			// menuItemPHScreen
			// 
			this.menuItemPHScreen.Index = 34;
			this.menuItemPHScreen.Text = "Screening Report";
			this.menuItemPHScreen.Click += new System.EventHandler(this.menuItemPHScreen_Click);
			// 
			// menuItemCourseGrades
			// 
			this.menuItemCourseGrades.Index = 35;
			this.menuItemCourseGrades.Text = "Dental School Course Grades";
			this.menuItemCourseGrades.Click += new System.EventHandler(this.menuItemCourseGrades_Click);
			// 
			// menuItemCustomReports
			// 
			this.menuItemCustomReports.Index = 5;
			this.menuItemCustomReports.Text = "Custom Reports";
			// 
			// menuItemTools
			// 
			this.menuItemTools.Index = 6;
			this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPrintScreen,
            this.menuItem1,
            this.menuItem9,
            this.menuItemAuditTrail,
            this.menuItemDatabaseMaintenance,
            this.menuItemImportXML,
            this.menuItemAging,
            this.menuItemFinanceCharge,
            this.menuItemRepeatingCharges,
            this.menuItemTranslation,
            this.menuItemScreening,
            this.menuItemTerminal,
            this.menuItemTerminalManager});
			this.menuItemTools.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
			this.menuItemTools.Text = "&Tools";
			// 
			// menuItemPrintScreen
			// 
			this.menuItemPrintScreen.Index = 0;
			this.menuItemPrintScreen.Text = "&Print Screen Tool";
			this.menuItemPrintScreen.Click += new System.EventHandler(this.menuItemPrintScreen_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuTelephone,
            this.menuItemPatientImport});
			this.menuItem1.Text = "Misc Tools";
			// 
			// menuTelephone
			// 
			this.menuTelephone.Index = 0;
			this.menuTelephone.Text = "Telephone Numbers";
			this.menuTelephone.Click += new System.EventHandler(this.menuTelephone_Click);
			// 
			// menuItemPatientImport
			// 
			this.menuItemPatientImport.Index = 1;
			this.menuItemPatientImport.Text = "Import Patient From Text File";
			this.menuItemPatientImport.Click += new System.EventHandler(this.menuItemPatientImport_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "-";
			// 
			// menuItemAuditTrail
			// 
			this.menuItemAuditTrail.Index = 3;
			this.menuItemAuditTrail.Text = "Audit Trail";
			this.menuItemAuditTrail.Click += new System.EventHandler(this.menuItemAuditTrail_Click);
			// 
			// menuItemDatabaseMaintenance
			// 
			this.menuItemDatabaseMaintenance.Index = 4;
			this.menuItemDatabaseMaintenance.Text = "Database Maintenance";
			this.menuItemDatabaseMaintenance.Click += new System.EventHandler(this.menuItemDatabaseMaintenance_Click);
			// 
			// menuItemImportXML
			// 
			this.menuItemImportXML.Index = 5;
			this.menuItemImportXML.Text = "Import Patient XML";
			this.menuItemImportXML.Click += new System.EventHandler(this.menuItemImportXML_Click);
			// 
			// menuItemAging
			// 
			this.menuItemAging.Index = 6;
			this.menuItemAging.Text = "Calculate &Aging";
			this.menuItemAging.Click += new System.EventHandler(this.menuItemAging_Click);
			// 
			// menuItemFinanceCharge
			// 
			this.menuItemFinanceCharge.Index = 7;
			this.menuItemFinanceCharge.Text = "Run &Finance Charges";
			this.menuItemFinanceCharge.Click += new System.EventHandler(this.menuItemFinanceCharge_Click);
			// 
			// menuItemRepeatingCharges
			// 
			this.menuItemRepeatingCharges.Index = 8;
			this.menuItemRepeatingCharges.Text = "Update Repeating Charges";
			this.menuItemRepeatingCharges.Click += new System.EventHandler(this.menuItemRepeatingCharges_Click);
			// 
			// menuItemTranslation
			// 
			this.menuItemTranslation.Index = 9;
			this.menuItemTranslation.Text = "Language Translation";
			this.menuItemTranslation.Click += new System.EventHandler(this.menuItemTranslation_Click);
			// 
			// menuItemScreening
			// 
			this.menuItemScreening.Index = 10;
			this.menuItemScreening.Text = "Public Health Screening";
			this.menuItemScreening.Click += new System.EventHandler(this.menuItemScreening_Click);
			// 
			// menuItemTerminal
			// 
			this.menuItemTerminal.Index = 11;
			this.menuItemTerminal.Text = "Terminal";
			this.menuItemTerminal.Click += new System.EventHandler(this.menuItemTerminal_Click);
			// 
			// menuItemTerminalManager
			// 
			this.menuItemTerminalManager.Index = 12;
			this.menuItemTerminalManager.Text = "Terminal Manager";
			this.menuItemTerminalManager.Click += new System.EventHandler(this.menuItemTerminalManager_Click);
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = 7;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHelpWindows,
            this.menuItemHelpContents,
            this.menuItemHelpIndex,
            this.menuItemRemote,
            this.menuItemUpdate});
			this.menuItemHelp.Text = "&Help";
			// 
			// menuItemHelpWindows
			// 
			this.menuItemHelpWindows.Index = 0;
			this.menuItemHelpWindows.Text = "Local Help-Windows";
			this.menuItemHelpWindows.Click += new System.EventHandler(this.menuItemHelpWindows_Click);
			// 
			// menuItemHelpContents
			// 
			this.menuItemHelpContents.Index = 1;
			this.menuItemHelpContents.Text = "Online Help - Contents";
			this.menuItemHelpContents.Click += new System.EventHandler(this.menuItemHelpContents_Click);
			// 
			// menuItemHelpIndex
			// 
			this.menuItemHelpIndex.Index = 2;
			this.menuItemHelpIndex.Shortcut = System.Windows.Forms.Shortcut.ShiftF1;
			this.menuItemHelpIndex.Text = "Online Help - Index";
			this.menuItemHelpIndex.Click += new System.EventHandler(this.menuItemHelpIndex_Click);
			// 
			// menuItemRemote
			// 
			this.menuItemRemote.Index = 3;
			this.menuItemRemote.Text = "Remote Support Now";
			this.menuItemRemote.Click += new System.EventHandler(this.menuItemRemote_Click);
			// 
			// menuItemUpdate
			// 
			this.menuItemUpdate.Index = 4;
			this.menuItemUpdate.Text = "&Update";
			this.menuItemUpdate.Click += new System.EventHandler(this.menuItemUpdate_Click);
			// 
			// imageList32
			// 
			this.imageList32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList32.ImageStream")));
			this.imageList32.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList32.Images.SetKeyName(0,"Appt32.gif");
			this.imageList32.Images.SetKeyName(1,"Family32b.gif");
			this.imageList32.Images.SetKeyName(2,"Account32b.gif");
			this.imageList32.Images.SetKeyName(3,"TreatPlan3D.gif");
			this.imageList32.Images.SetKeyName(4,"chart32.gif");
			this.imageList32.Images.SetKeyName(5,"Images32.gif");
			this.imageList32.Images.SetKeyName(6,"Manage32.gif");
			// 
			// timerSignals
			// 
			this.timerSignals.Tick += new System.EventHandler(this.timerSignals_Tick);
			// 
			// ContrAppt2
			// 
			this.ContrAppt2.Location = new System.Drawing.Point(97,12);
			this.ContrAppt2.Name = "ContrAppt2";
			this.ContrAppt2.Size = new System.Drawing.Size(857,643);
			this.ContrAppt2.TabIndex = 21;
			// 
			// lightSignalGrid1
			// 
			this.lightSignalGrid1.Location = new System.Drawing.Point(0,463);
			this.lightSignalGrid1.Name = "lightSignalGrid1";
			this.lightSignalGrid1.Size = new System.Drawing.Size(50,206);
			this.lightSignalGrid1.TabIndex = 20;
			this.lightSignalGrid1.Text = "lightSignalGrid1";
			this.lightSignalGrid1.ButtonClick += new OpenDental.UI.ODLightSignalGridClickEventHandler(this.lightSignalGrid1_ButtonClick);
			// 
			// myOutlookBar
			// 
			this.myOutlookBar.Dock = System.Windows.Forms.DockStyle.Left;
			this.myOutlookBar.ImageList = this.imageList32;
			this.myOutlookBar.Location = new System.Drawing.Point(0,0);
			this.myOutlookBar.Name = "myOutlookBar";
			this.myOutlookBar.Size = new System.Drawing.Size(51,663);
			this.myOutlookBar.TabIndex = 18;
			this.myOutlookBar.Text = "outlookBar1";
			this.myOutlookBar.ButtonClicked += new OpenDental.ButtonClickedEventHandler(this.myOutlookBar_ButtonClicked);
			// 
			// ContrFamily2
			// 
			this.ContrFamily2.Location = new System.Drawing.Point(109,38);
			this.ContrFamily2.Name = "ContrFamily2";
			this.ContrFamily2.Size = new System.Drawing.Size(845,599);
			this.ContrFamily2.TabIndex = 22;
			// 
			// ContrAccount2
			// 
			this.ContrAccount2.Location = new System.Drawing.Point(109,38);
			this.ContrAccount2.Name = "ContrAccount2";
			this.ContrAccount2.Size = new System.Drawing.Size(796,599);
			this.ContrAccount2.TabIndex = 23;
			// 
			// ContrTreat2
			// 
			this.ContrTreat2.Location = new System.Drawing.Point(97,25);
			this.ContrTreat2.Name = "ContrTreat2";
			this.ContrTreat2.Size = new System.Drawing.Size(857,612);
			this.ContrTreat2.TabIndex = 24;
			// 
			// ContrDocs2
			// 
			this.ContrDocs2.Location = new System.Drawing.Point(97,31);
			this.ContrDocs2.Name = "ContrDocs2";
			this.ContrDocs2.Size = new System.Drawing.Size(845,606);
			this.ContrDocs2.TabIndex = 25;
			// 
			// FormOpenDental
			// 
			this.ClientSize = new System.Drawing.Size(982,663);
			this.Controls.Add(this.ContrDocs2);
			this.Controls.Add(this.ContrTreat2);
			this.Controls.Add(this.ContrAccount2);
			this.Controls.Add(this.ContrFamily2);
			this.Controls.Add(this.ContrAppt2);
			this.Controls.Add(this.lightSignalGrid1);
			this.Controls.Add(this.myOutlookBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Menu = this.mainMenu;
			this.Name = "FormOpenDental";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text = "Open Dental";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormOpenDental_Layout);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormOpenDental_Closing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormOpenDental_KeyDown);
			this.Load += new System.EventHandler(this.FormOpenDental_Load);
			this.ResumeLayout(false);

		}
		#endregion
	
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();//changes appearance to XP
			Application.DoEvents();//workaround for a known MS bug in the line above
			Application.Run(new FormOpenDental());
		}

		private void FormOpenDental_Load(object sender, System.EventArgs e){
			Splash.Dispose();//SPK/AAD 1/10/07
			allNeutral();
			FormChooseDatabase formChooseDb=new FormChooseDatabase();
			formChooseDb.GetConfig();
			if(formChooseDb.NoShow) {
				//DataClass.SetConnection();
				if(!formChooseDb.TryToConnect()) {
					formChooseDb.ShowDialog();
					if(formChooseDb.DialogResult==DialogResult.Cancel) {
						Application.Exit();
						return;
					}
				}
			}
			else {
				formChooseDb.ShowDialog();
				if(formChooseDb.DialogResult==DialogResult.Cancel) {
					Application.Exit();
					return;
				}
			}
			//if(!Prefs.CheckMySqlVersion50()){
			//	Application.Exit();
			//	return;
			//}
			//StoredProcedureLoader.LoadIfNeeded();
			Cursor=Cursors.WaitCursor;
			if(!RefreshLocalData(InvalidTypes.AllLocal,true)){
				Cursor=Cursors.Default;
				return;
			}
			//Lan.Refresh();//automatically skips if current culture is en-US
			//LanguageForeigns.Refresh(CultureInfo.CurrentCulture);//automatically skips if current culture is en-US
			//buttonsShadow=imageList2x6.Images[0];  //(Image)pictButtons.Image.Clone();
			DataValid.BecameInvalid += new OpenDental.ValidEventHandler(DataValid_BecameInvalid);
			signalLastRefreshed=MiscData.GetNowDateTime();
			timerSignals.Interval=PrefB.GetInt("ProcessSigsIntervalInSecs")*1000;
			timerSignals.Enabled=true;
			ContrAccount2.InstantClasses();
			ContrAppt2.InstantClasses();
//			ContrChart2.InstantClasses();
			ContrDocs2.InstantClasses();
			ContrFamily2.InstantClasses();
//			ContrManage2.InstantClasses();
			ContrTreat2.InitializeOnStartup();
			timerTimeIndic.Enabled=true;
			myOutlookBar.Buttons[0].Caption=Lan.g(this,"Appts");
			myOutlookBar.Buttons[1].Caption=Lan.g(this,"Family");
			myOutlookBar.Buttons[2].Caption=Lan.g(this,"Account");
			myOutlookBar.Buttons[3].Caption=Lan.g(this,"Treat' Plan");
			//myOutlookBar.Buttons[4].Caption=Lan.g(this,"Chart");//??done in RefreshLocalData
			myOutlookBar.Buttons[5].Caption=Lan.g(this,"Images");
			myOutlookBar.Buttons[6].Caption=Lan.g(this,"Manage");
			foreach(MenuItem menuItem in mainMenu.MenuItems){
				TranslateMenuItem(menuItem);
			}
			if(CultureInfo.CurrentCulture.Name=="en-US"){
				CultureInfo cInfo=(CultureInfo)CultureInfo.CurrentCulture.Clone();
				cInfo.DateTimeFormat.ShortDatePattern="MM/dd/yyyy";
				Application.CurrentCulture=cInfo;
			}
			if(CultureInfo.CurrentCulture.TwoLetterISOLanguageName=="en"){
				menuItemTranslation.Visible=false;
			}
			if(!File.Exists("Help.chm")){
				menuItemHelpWindows.Visible=false;
			}
			if(!File.Exists("remoteclient.exe")){
				menuItemRemote.Visible=false;
			}
			Userod adminUser=Userods.GetAdminUser();
			if(adminUser.Password=="") {
				Security.CurUser=adminUser.Copy();
			}
			else {
				FormLogOn FormL=new FormLogOn();
				FormL.ShowDialog();
				if(FormL.DialogResult==DialogResult.Cancel) {
					Cursor=Cursors.Default;
					Application.Exit();
					return;
				}
			}
			myOutlookBar.SelectedIndex=Security.GetModule(0);
			myOutlookBar.Invalidate();
			SetModuleSelected();
			Cursor=Cursors.Default;
			if(myOutlookBar.SelectedIndex==-1){
				MsgBox.Show(this,"You do not have permission to use any modules.");
			}
			Bridges.Trojan.StartupCheck();
		}

		///<summary>Refreshes certain rarely used data from database.  Must supply the types of data to refresh as flags.  Also performs a few other tasks that must be done when local data is changed.</summary>
		private bool RefreshLocalData(InvalidTypes itypes,bool isStartingUp){
			if((itypes & InvalidTypes.Prefs)==InvalidTypes.Prefs){
				Prefs.Refresh();
				if(isStartingUp){
					if(!Prefs.CheckMySqlVersion41()){
						return false;
					}
					if(!Prefs.ConvertDB()){
						return false;
					}
					if(!Directory.Exists(((Pref)PrefB.HList["DocPath"]).ValueString)
						|| !Directory.Exists(((Pref)PrefB.HList["DocPath"]).ValueString+"A"))
					{
						//PermissionsOld.Refresh();
						//UserPermissions.Refresh();
						//Providers.Refresh();
						//Employees.Refresh();
						Userods.Refresh();
						FormPath FormP=new FormPath();
						FormP.ShowDialog();
						if(FormP.DialogResult!=DialogResult.OK){
							return false;
						}
						else{
							Prefs.Refresh();//because listening thread not started yet.
						}
					}
					if(!Prefs.CheckProgramVersion()){
						return false;
					}
					Lan.Refresh();//automatically skips if current culture is en-US
					LanguageForeigns.Refresh(CultureInfo.CurrentCulture);//automatically skips if current culture is en-US
				}
				if(((Pref)PrefB.HList["EasyHidePublicHealth"]).ValueString=="1"){
					menuItemSchools.Visible=false;
					menuItemCounties.Visible=false;
					menuItemScreening.Visible=false;
					menuItemPHSep.Visible=false;
					menuItemPHRawScreen.Visible=false;
					menuItemPHRawPop.Visible=false;
					menuItemPHScreen.Visible=false;
				}
				else{
					menuItemSchools.Visible=true;
					menuItemCounties.Visible=true;
					menuItemScreening.Visible=true;
					menuItemPHSep.Visible=true;
					//menuItemPublicHealth.Visible=true;
					menuItemPHRawScreen.Visible=true;
					menuItemPHRawPop.Visible=true;
					menuItemPHScreen.Visible=true;
				}
				if(PrefB.GetBool("EasyNoClinics")){
					menuItemClinics.Visible=false;
				}
				else{
					menuItemClinics.Visible=true;
				}
				if(((Pref)PrefB.HList["EasyHideClinical"]).ValueString=="1"){
					myOutlookBar.Buttons[4].Caption=Lan.g(this,"Procs");
				}
				else{
					myOutlookBar.Buttons[4].Caption=Lan.g(this,"Chart");
				}
				if(((Pref)PrefB.HList["EasyBasicModules"]).ValueString=="1"){
					myOutlookBar.Buttons[3].Visible=false;
					myOutlookBar.Buttons[5].Visible=false;
					myOutlookBar.Buttons[6].Visible=false;
					//pictButtons.Visible=false;
				}
				else{
					myOutlookBar.Buttons[3].Visible=true;
					myOutlookBar.Buttons[5].Visible=true;
					myOutlookBar.Buttons[6].Visible=true;
					//pictButtons.Visible=true;
				}
				myOutlookBar.Invalidate();
				if(PrefB.GetBool("EasyHideDentalSchools")){
					menuItemSchoolClass.Visible=false;
					menuItemSchoolCourses.Visible=false;
					menuItemInstructors.Visible=false;
					menuItemCourseGrades.Visible=false;
				}
				else{
					menuItemSchoolClass.Visible=true;
					menuItemSchoolCourses.Visible=true;
					menuItemInstructors.Visible=true;
					menuItemCourseGrades.Visible=true;
				}
				if(PrefB.GetBool("EasyHideRepeatCharges")){
					menuItemRepeatingCharges.Visible=false;
				}
				else{
					menuItemRepeatingCharges.Visible=true;
				}
				menuItemCustomReports.MenuItems.Clear();
				if(Directory.Exists(PrefB.GetString("DocPath")+PrefB.GetString("ReportFolderName"))){
					DirectoryInfo infoDir=new DirectoryInfo(PrefB.GetString("DocPath")+PrefB.GetString("ReportFolderName"));
					FileInfo[] filesRdl=infoDir.GetFiles("*.rdl");
					for(int i=0;i<filesRdl.Length;i++) {
						menuItemCustomReports.MenuItems.Add(Path.GetFileNameWithoutExtension(filesRdl[i].Name),
							new System.EventHandler(this.menuItemRDLReport_Click));
					}
				}
				if(menuItemCustomReports.MenuItems.Count==0){
					menuItemCustomReports.Visible=false;
				}
				else{
					menuItemCustomReports.Visible=true;
				}
			}//if(InvalidTypes.Prefs)
			//if(!BackupJobs.IsBackup()){
			//	MessageBox.Show(Lan.g(this,"Missing Pref 'IsDatabaseBackup', Must update database."));
			//}
			if((itypes & InvalidTypes.AutoCodes)==InvalidTypes.AutoCodes){
				AutoCodes.Refresh();
				AutoCodeItems.Refresh();
				AutoCodeConds.Refresh();
			}
			//BackupJobs.Refresh();
			if((itypes & InvalidTypes.Carriers)==InvalidTypes.Carriers){
				Carriers.Refresh();//run on startup, after telephone reformat, after list edit.
			}
			if((itypes & InvalidTypes.ClaimForms)==InvalidTypes.ClaimForms){
				ClaimFormItems.Refresh();
				ClaimForms.Refresh();
			}
			
			if((itypes & InvalidTypes.ClearHouses)==InvalidTypes.ClearHouses){
				Clearinghouses.Refresh();
				SigElementDefs.Refresh();
				SigButDefs.Refresh();
				//SigButDefElements.Refresh();//now part of SigButDefs.Refresh()
				//SigButDefList=SigButDefs.GetByComputer(SystemInformation.ComputerName);
				FillSignalButtons(null);
			}
			if((itypes & InvalidTypes.Computers)==InvalidTypes.Computers){
				Computers.Refresh();
				Printers.Refresh();
			}
			if((itypes & InvalidTypes.Defs)==InvalidTypes.Defs){
				Defs.Refresh();
			}
			if((itypes & InvalidTypes.DentalSchools)==InvalidTypes.DentalSchools){
				Instructors.Refresh();
				SchoolClasses.Refresh();
				SchoolCourses.Refresh();
			}
			if((itypes & InvalidTypes.Email)==InvalidTypes.Email){
				EmailTemplates.Refresh();
				DiseaseDefs.Refresh();
			}
			if((itypes & InvalidTypes.Employees)==InvalidTypes.Employees){
				Employees.Refresh();
				PayPeriods.Refresh();
			}
			if((itypes & InvalidTypes.Fees)==InvalidTypes.Fees){
				Fees.Refresh();
			}
			if((itypes & InvalidTypes.InsCats)==InvalidTypes.InsCats){
				CovCats.Refresh();
				CovSpans.Refresh();
			}
			if((itypes & InvalidTypes.Letters)==InvalidTypes.Letters){
				Letters.Refresh();
			}
			if((itypes & InvalidTypes.LetterMerge)==InvalidTypes.LetterMerge){
				LetterMergeFields.Refresh();
				LetterMerges.Refresh();
			}
			if((itypes & InvalidTypes.Operatories)==InvalidTypes.Operatories){
				Operatories.Refresh();
				AccountingAutoPays.Refresh();
			}
			if((itypes & InvalidTypes.ProcButtons)==InvalidTypes.ProcButtons){
				ProcButtons.Refresh();
				ProcButtonItems.Refresh();
			}
			if((itypes & InvalidTypes.ProcCodes)==InvalidTypes.ProcCodes){
				ProcedureCodes.Refresh();
			}
			if((itypes & InvalidTypes.Programs)==InvalidTypes.Programs){
				Programs.Refresh();
				ProgramProperties.Refresh();
				menuItemPracticeWebReports.Visible=Programs.IsEnabled("PracticeWebReports");
			}
			if((itypes & InvalidTypes.Providers)==InvalidTypes.Providers){
				Providers.Refresh();
				ProviderIdents.Refresh();
				Clinics.Refresh();
			}
			if((itypes & InvalidTypes.QuickPaste)==InvalidTypes.QuickPaste){
				QuickPasteNotes.Refresh();
				QuickPasteCats.Refresh();
			}
			//if((itypes & InvalidTypes)==InvalidTypes){
			//	Reports.Refresh();
			//}
			if((itypes & InvalidTypes.Security)==InvalidTypes.Security){
				Userods.Refresh();
				UserGroups.Refresh();
				GroupPermissions.Refresh();
			}
			if((itypes & InvalidTypes.Sched)==InvalidTypes.Sched){
				SchedDefaults.Refresh();//assumed to change rarely
				//Schedules.Refresh();//Schedules are refreshed as needed.  Not here.
			}
			if((itypes & InvalidTypes.Startup)==InvalidTypes.Startup){
				Employers.Refresh();//only needed when opening the prog. After that, automated.
				ElectIDs.Refresh();//only run on startup
				Referrals.Refresh();//Referrals are also refreshed dynamically.
			}
			if((itypes & InvalidTypes.ToolBut)==InvalidTypes.ToolBut){
				ToolButItems.Refresh();
				ContrAccount2.LayoutToolBar();
				ContrAppt2.LayoutToolBar();
//ContrChart2.LayoutToolBar();
				ContrDocs2.LayoutToolBar();
				ContrFamily2.LayoutToolBar();
			}
			if((itypes & InvalidTypes.Views)==InvalidTypes.Views){
				AppointmentRules.Refresh();
				ApptViews.Refresh();
				ApptViewItems.Refresh();
				ContrAppt2.FillViews();
			}
			if((itypes & InvalidTypes.ZipCodes)==InvalidTypes.ZipCodes){
				ZipCodes.Refresh();
				PatFieldDefs.Refresh();
			}
			ContrTreat2.InitializeLocalData();//easier to leave this here for now than to split it.*/
			return true;
		}

		private void FormOpenDental_Layout(object sender, System.Windows.Forms.LayoutEventArgs e){
			if(Width<200) Width=200;
			ContrAccount2.Location=new Point(myOutlookBar.Width,0);
			ContrAccount2.Width=this.ClientSize.Width-ContrAccount2.Location.X;
			ContrAccount2.Height=this.ClientSize.Height;
			ContrAppt2.Location=new Point(myOutlookBar.Width,0);
			ContrAppt2.Width=this.ClientSize.Width-ContrAppt2.Location.X;
			ContrAppt2.Height=this.ClientSize.Height;
/*			ContrChart2.Location=new Point(myOutlookBar.Width,0);
			ContrChart2.Width=this.ClientSize.Width-ContrChart2.Location.X;
			ContrChart2.Height=this.ClientSize.Height;*/
			ContrDocs2.Location=new Point(myOutlookBar.Width,0);
			ContrDocs2.Width=this.ClientSize.Width-ContrDocs2.Location.X;
			ContrDocs2.Height=this.ClientSize.Height;
			ContrFamily2.Location=new Point(myOutlookBar.Width,0);
			ContrFamily2.Width=this.ClientSize.Width-ContrFamily2.Location.X;
			ContrFamily2.Height=this.ClientSize.Height;
			//ContrManage2.Location=new Point(myOutlookBar.Width,0);
			//ContrManage2.Width=this.ClientSize.Width-ContrManage2.Location.X;
			//ContrManage2.Height=this.ClientSize.Height;
			ContrTreat2.Location=new Point(myOutlookBar.Width,0);
			ContrTreat2.Width=this.ClientSize.Width-ContrFamily2.Location.X;
			ContrTreat2.Height=this.ClientSize.Height;
		}

		
		///<summary>This is called when any local data becomes outdated.  It's purpose is to tell the other computers to update certain local data.</summary>
		private void DataValid_BecameInvalid(OpenDental.ValidEventArgs e){
			if(e.OnlyLocal){
				RefreshLocalData(InvalidTypes.AllLocal,true);//does local computer only
				return;
			}
			if(e.ITypes!=InvalidTypes.Date){
				//local refresh for dates is handled within ContrAppt, not here
				RefreshLocalData(e.ITypes,false);//does local computer
			}
			Signal sig=new Signal();
			sig.ITypes=e.ITypes;
			sig.DateViewing=Appointments.DateSelected;//ignored if ITypes not InvalidTypes.Date
			sig.SigType=SignalType.Invalid;
			Signals.Insert(sig);
		}

		///<summary>Happens when any of the modules changes the current patient.  The calling module should then refresh itself.  The current patNum is stored here in the parent form so that when switching modules, the parent form knows which patient to call up for that module.</summary>
		private void Contr_PatientSelected(object sender, PatientSelectedEventArgs e){
			CurPatNum=e.PatNum;
		}

		private void GotoModule_ModuleSelected(ModuleEventArgs e){
			if(e.DateSelected.Year>1880){
				Appointments.DateSelected=e.DateSelected;
			}
			if(e.SelectedAptNum>0){
				ContrApptSingle.SelectedAptNum=e.SelectedAptNum;
			}
			//patient would have been set separately ahead of time
			//CurPatNum=Appointments.Cur.PatNum;
			UnselectActive();
			allNeutral();
			if(e.ClaimNum>0){
				myOutlookBar.SelectedIndex=e.IModule;
				ContrAccount2.Visible=true;
				this.ActiveControl=this.ContrAccount2;
				ContrAccount2.ModuleSelected(CurPatNum,e.ClaimNum);
			}
			else if(e.PinAppt!=null){
				myOutlookBar.SelectedIndex=e.IModule;
				ContrAppt2.Visible=true;
				this.ActiveControl=this.ContrAppt2;
				ContrAppt2.ModuleSelected(CurPatNum,e.PinAppt);
			}
			else if(e.IModule!=-1){
				myOutlookBar.SelectedIndex=e.IModule;
				SetModuleSelected();
			}
			myOutlookBar.Invalidate();
		}

		///<summary>If this is initial call when opening program, then set sigListButs=null.  This will cause a call to be made to database to get current status of buttons.  Otherwise, it adds the signals passed in to the current state, then paints.</summary>
		private void FillSignalButtons(Signal[] sigListButs){
			if(sigListButs==null){
				SigButDefList=SigButDefs.GetByComputer(SystemInformation.ComputerName);
				lightSignalGrid1.SetButtons(SigButDefList);
				sigListButs=Signals.RefreshCurrentButState();
			}
			SigElementDef element;
			SigButDef butDef;
			int row;
			Color color;
			for(int i=0;i<sigListButs.Length;i++){
				if(sigListButs[i].AckTime.Year>1880){//process ack
					int rowAck=lightSignalGrid1.ProcessAck(sigListButs[i].SignalNum);
					if(rowAck!=-1){
						butDef=SigButDefs.GetByIndex(rowAck,SigButDefList);
						if(butDef!=null){
							PaintOnIcon(butDef.SynchIcon,Color.White);
						}
					}
				}
				else{//process normal message
					row=0;
					color=Color.White;
					for(int e=0;e<sigListButs[i].ElementList.Length;e++){
						element=SigElementDefs.GetElement(sigListButs[i].ElementList[e].SigElementDefNum);
						if(element.LightRow!=0){
							row=element.LightRow;
						}
						if(element.LightColor.ToArgb()!=Color.White.ToArgb()){
							color=element.LightColor;
						}
					}
					if(row!=0 && color!=Color.White) {
						lightSignalGrid1.SetButtonActive(row-1,color,sigListButs[i]);
						butDef=SigButDefs.GetByIndex(row-1,SigButDefList);
						if(butDef!=null){
							PaintOnIcon(butDef.SynchIcon,color);
						}
					}
				}
			}
		}

		///<summary>Pass in the cellNum as 1-based.</summary>
		private void PaintOnIcon(int cellNum,Color color){
			Graphics g;
			if(bitmapIcon==null){
				bitmapIcon=new Bitmap(16,16);
				g=Graphics.FromImage(bitmapIcon);
				g.FillRectangle(new SolidBrush(Color.White),0,0,15,15);
				//horizontal
				g.DrawLine(Pens.Black,0,0,15,0);
				g.DrawLine(Pens.Black,0,5,15,5);
				g.DrawLine(Pens.Black,0,10,15,10);
				g.DrawLine(Pens.Black,0,15,15,15);
				//vertical
				g.DrawLine(Pens.Black,0,0,0,15);
				g.DrawLine(Pens.Black,5,0,5,15);
				g.DrawLine(Pens.Black,10,0,10,15);
				g.DrawLine(Pens.Black,15,0,15,15);
				g.Dispose();
			}
			if(cellNum==0){
				return;
			}
			g=Graphics.FromImage(bitmapIcon);
			int x=0;
			int y=0;
			switch(cellNum){
				case 1: x=1; y=1; break;
				case 2: x=6; y=1; break;
				case 3: x=11; y=1; break;
				case 4: x=1; y=6; break;
				case 5: x=6; y=6; break;
				case 6: x=11; y=6; break;
				case 7: x=1; y=11; break;
				case 8: x=6; y=11; break;
				case 9: x=11; y=11; break;
			}
			g.FillRectangle(new SolidBrush(color),x,y,4,4);
			Icon=Icon.FromHandle(bitmapIcon.GetHicon());
			g.Dispose();
		}

		private void lightSignalGrid1_ButtonClick(object sender,OpenDental.UI.ODLightSignalGridClickEventArgs e) {
			if(e.ActiveSignal!=null){//user trying to ack an existing light signal
				Signals.AckButton(e.ButtonIndex+1,signalLastRefreshed);
				//then, manually ack the light on this computer.  The second ack in a few seconds will be ignored.
				lightSignalGrid1.SetButtonActive(e.ButtonIndex,Color.White,null);
				SigButDef butDef=SigButDefs.GetByIndex(e.ButtonIndex,SigButDefList);
				if(butDef!=null) {
					PaintOnIcon(butDef.SynchIcon,Color.White);
				}
				return;
			}
			if(e.ButtonDef==null || e.ButtonDef.ElementList.Length==0){//there is no signal to send
				return;
			}
			//user trying to send a signal
			Signal sig=new Signal();
			sig.SigType=SignalType.Button;
			//sig.ToUser=sigElementDefUser[listTo.SelectedIndex].SigText;
			//sig.FromUser=sigElementDefUser[listFrom.SelectedIndex].SigText;
			//need to do this all as a transaction?
			Signals.Insert(sig);
			int row=0;
			Color color=Color.White;
			SigElementDef def;
			SigElement element;
			SigButDefElement[] butElements=SigButDefElements.GetForButton(e.ButtonDef.SigButDefNum);
			for(int i=0;i<butElements.Length;i++){
				element=new SigElement();
				element.SigElementDefNum=butElements[i].SigElementDefNum;
				element.SignalNum=sig.SignalNum;
				SigElements.Insert(element);
				if(SigElementDefs.GetElement(element.SigElementDefNum).SigElementType==SignalElementType.User){
					sig.ToUser=SigElementDefs.GetElement(element.SigElementDefNum).SigText;
					Signals.Update(sig);
				}
				def=SigElementDefs.GetElement(element.SigElementDefNum);
				if(def.LightRow!=0) {
					row=def.LightRow;
				}
				if(def.LightColor.ToArgb()!=Color.White.ToArgb()) {
					color=def.LightColor;
				}
			}
			sig.ElementList=new SigElement[0];//we don't really care about these
			if(row!=0 && color!=Color.White) {
				lightSignalGrid1.SetButtonActive(row-1,color,sig);//this just makes it seem more responsive.
				//we can skip painting on the icon
			}
		}
	
		///<summary>Called every time timerSignals_Tick fires.  Usually about every 5-10 seconds.</summary>
		public void ProcessSignals(){
			Signal[] sigList=Signals.RefreshTimed(signalLastRefreshed);//this also attaches all elements to their sigs
			if(sigList.Length==0){
				return;
			}
			if(sigList[sigList.Length-1].AckTime.Year>1880){
				signalLastRefreshed=sigList[sigList.Length-1].AckTime;
			}
			else{
				signalLastRefreshed=sigList[sigList.Length-1].SigDateTime;
			}
			if(ContrAppt2.Visible && Signals.ApptNeedsRefresh(sigList,Appointments.DateSelected.Date)){
				ContrAppt2.RefreshModuleScreen();
			}
			InvalidTypes invalidTypes=Signals.GetInvalidTypes(sigList);
			if(invalidTypes!=0){
				RefreshLocalData(invalidTypes,false);
			}
			Signal[] sigListButs=Signals.GetButtonSigs(sigList);
//			ContrManage2.LogMsgs(sigListButs);
			FillSignalButtons(sigListButs);
	//Need to add a test to this: do not play messages that are over 2 minutes old.
			Thread newThread=new Thread(new ParameterizedThreadStart(PlaySounds));
			newThread.Start(sigListButs);
		}

		private void PlaySounds(Object objSignalList){
			Signal[] signalList=(Signal[])objSignalList;
			string strSound;
			byte[] rawData;
			MemoryStream stream;
			SoundPlayer simpleSound;
			//loop through each signal
			for(int s=0;s<signalList.Length;s++){
				if(signalList[s].AckTime.Year>1880){
					continue;//don't play any sounds for acks.
				}
				if(s>0){
					Thread.Sleep(1000);//pause 1 second between signals.
				}
				//play all the sounds.
				for(int e=0;e<signalList[s].ElementList.Length;e++){
					strSound=SigElementDefs.GetElement(signalList[s].ElementList[e].SigElementDefNum).Sound;
					if(strSound==""){
						continue;
					}
					try {
						rawData=Convert.FromBase64String(strSound);
						stream=new MemoryStream(rawData);
						simpleSound = new SoundPlayer(stream);
						simpleSound.PlaySync();//sound will finish playing before thread continues
					}
					catch {
						//do nothing
					}
				}
			}
		}

		private void myOutlookBar_ButtonClicked(object sender, OpenDental.ButtonClicked_EventArgs e){
			switch(myOutlookBar.SelectedIndex){
				case 0:
					if(!Security.IsAuthorized(Permissions.AppointmentsModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 1:
					if(!Security.IsAuthorized(Permissions.FamilyModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 2:
					if(!Security.IsAuthorized(Permissions.AccountModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 3:
					if(!Security.IsAuthorized(Permissions.TPModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 4:
					if(!Security.IsAuthorized(Permissions.ChartModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 5:
					if(!Security.IsAuthorized(Permissions.ImagesModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 6:
					if(!Security.IsAuthorized(Permissions.ManageModule)){
						e.Cancel=true;
						return;
					}
					break;
			}
			UnselectActive();
			allNeutral();
			SetModuleSelected();
		}

		///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing.</summary>
		private void SetModuleSelected(){
			switch(myOutlookBar.SelectedIndex){
				case 0:
					ContrAppt2.Visible=true;
					this.ActiveControl=this.ContrAppt2;
					ContrAppt2.ModuleSelected(CurPatNum);
					break;
				case 1:
					ContrFamily2.Visible=true;
					this.ActiveControl=this.ContrFamily2;
					ContrFamily2.ModuleSelected(CurPatNum);
					break;
				case 2:
					ContrAccount2.Visible=true;
					this.ActiveControl=this.ContrAccount2;
					ContrAccount2.ModuleSelected(CurPatNum);
					break;
				case 3:
					ContrTreat2.Visible=true;
					this.ActiveControl=this.ContrTreat2;
					ContrTreat2.ModuleSelected(CurPatNum);
					break;
/*				case 4:
					ContrChart2.Visible=true;
					this.ActiveControl=this.ContrChart2;
					ContrChart2.ModuleSelected(CurPatNum);
					break;*/
				case 5:
					ContrDocs2.Visible=true;
					this.ActiveControl=this.ContrDocs2;
					ContrDocs2.ModuleSelected(CurPatNum);
					break;
/*				case 6:
					ContrManage2.Visible=true;
					this.ActiveControl=this.ContrManage2;
					ContrManage2.ModuleSelected();
					break;
				case 7:
					//ContrAppt2.Visible=true;
					//this.ActiveControl=this.ContrAppt2;
					//ContrAppt2.ModuleSelected();
					break;*/
			}
		}

		private void allNeutral(){
			ContrAppt2.Visible=false;
			ContrFamily2.Visible=false;
			ContrAccount2.Visible=false;
			ContrTreat2.Visible=false;
//			ContrChart2.Visible=false;
			ContrDocs2.Visible=false;
//			ContrManage2.Visible=false;*/
		}

		private void UnselectActive(){
			if(ContrAppt2.Visible)
				ContrAppt2.ModuleUnselected();
			if(ContrFamily2.Visible)
				ContrFamily2.ModuleUnselected();
			if(ContrAccount2.Visible)
				ContrAccount2.ModuleUnselected();
			if(ContrTreat2.Visible)
				ContrTreat2.ModuleUnselected();
//			if(ContrChart2.Visible)
//				ContrChart2.ModuleUnselected();
			if(ContrDocs2.Visible)
				ContrDocs2.ModuleUnselected();
			//ContrStaff2.Visible=false;
		}

		private void RefreshCurrentModule(){
			if(ContrAppt2.Visible)
				ContrAppt2.ModuleSelected(CurPatNum);
			if(ContrFamily2.Visible)
				ContrFamily2.ModuleSelected(CurPatNum);
			if(ContrAccount2.Visible)
				ContrAccount2.ModuleSelected(CurPatNum);
			if(ContrTreat2.Visible)
				ContrTreat2.ModuleSelected(CurPatNum);
//			if(ContrChart2.Visible)
//				ContrChart2.ModuleSelected(CurPatNum);
			if(ContrDocs2.Visible)
				ContrDocs2.ModuleSelected(CurPatNum);
		}

		/// <summary>sends function key presses to the appointment module</summary>
		private void FormOpenDental_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(ContrAppt2.Visible && e.KeyCode>=Keys.F1 && e.KeyCode<=Keys.F12)
				ContrAppt2.FunctionKeyPress(e.KeyCode);
		}

		private void FormOpenDental_Closing(object sender, System.ComponentModel.CancelEventArgs e){
			//NotifyIcon1.Visible=false;
			//Thread2.Abort();
			//if(this.TcpListener2!=null){
			//	this.TcpListener2.Stop();  
			//}
			//Application.Exit();
		}

		private void timerTimeIndic_Tick(object sender, System.EventArgs e){
			if(WindowState!=FormWindowState.Minimized
				&& ContrAppt2.Visible){
				ContrAppt2.TickRefresh();
      }
		}

		private void timerSignals_Tick(object sender, System.EventArgs e) {
			ProcessSignals();
		}

		///<summary>This is recursive</summary>
		private void TranslateMenuItem(MenuItem menuItem){
			//first translate any child menuItems
			foreach(MenuItem mi in menuItem.MenuItems){
				TranslateMenuItem(mi);
			}
			//then this menuitem
			Lan.C("MainMenu",menuItem);
		}

		#region MenuEvents
		private void menuItemLogOff_Click(object sender, System.EventArgs e) {
			LastModule=myOutlookBar.SelectedIndex;
			myOutlookBar.SelectedIndex=-1;
			myOutlookBar.Invalidate();
			UnselectActive();
			allNeutral();
			FormLogOn FormL=new FormLogOn();
			FormL.ShowDialog();
			if(FormL.DialogResult==DialogResult.Cancel){
				Application.Exit();
				return;
			}
			myOutlookBar.SelectedIndex=Security.GetModule(LastModule);
			myOutlookBar.Invalidate();
			SetModuleSelected();
			if(myOutlookBar.SelectedIndex==-1){
				MsgBox.Show(this,"You do not have permission to use any modules.");
			}
		}

		//File
		private void menuItemPrinter_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPrinterSetup FormPS=new FormPrinterSetup();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Printers");
		}

		private void menuItemConfig_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ChooseDatabase)){
				return;
			}
			FormChooseDatabase FormC=new FormChooseDatabase();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.ChooseDatabase,0,"");
			if(FormC.DialogResult==DialogResult.Cancel){
				return;
			}
			CurPatNum=0;
			//RefreshCurrentModule();//clumsy but necessary. Sets local PatNums to null.
			RefreshLocalData(InvalidTypes.AllLocal,true);
			//RefreshCurrentModule();
			menuItemLogOff_Click(this,e);//this is a quick shortcut.
		}

		private void menuItemExit_Click(object sender, System.EventArgs e) {
			//Thread2.Abort();
			//if(this.TcpListener2!=null){
			//	this.TcpListener2.Stop();  
			//}
			Application.Exit();
		}

		//FormBackupJobsSelect FormBJS=new FormBackupJobsSelect();
		//FormBJS.ShowDialog();	

		//Setup
		private void menuItemApptViews_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormApptViews FormAV=new FormApptViews();
			FormAV.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Views");
		}

		private void menuItemApptRules_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormApptRules FormA=new FormApptRules();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Rules");
		}

		private void menuItemAutoCodes_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormAutoCode FormAC=new FormAutoCode();
			FormAC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Auto Codes");
		}

		private void menuItemClaimForms_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClaimForms FormCF=new FormClaimForms();
			FormCF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Claim Forms");
		}

		private void menuItemClearinghouses_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClearinghouses FormC=new FormClearinghouses();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Clearinghouses");
		}

		private void menuItemComputers_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormComputers FormC=new FormComputers();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Computers");
		}

		private void menuItemDataPath_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPath FormP=new FormPath();
			FormP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Data Path");	
		}

		private void menuItemDefinitions_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.AccountColors);//just the first cat.
			FormD.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions");
		}

		private void menuItemDiseases_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDiseaseDefs FormD=new FormDiseaseDefs();
			FormD.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Disease Defs");
		}

		private void menuItemEasy_Click(object sender, System.EventArgs e) {
/*			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormEasy FormE=new FormEasy();
			FormE.ShowDialog();
			ContrAccount2.LayoutToolBar();//for repeating charges
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Easy Options");*/
		}

		private void menuItemEmail_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormEmailSetup FormE=new FormEmailSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Email");
		}

		private void menuItemImaging_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormImagingSetup FormI=new FormImagingSetup();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Imaging");
		}

		private void menuItemInsCats_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInsCatsSetup FormE=new FormInsCatsSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Insurance Categories");
		}

		private void menuItemMessaging_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMessagingSetup FormM=new FormMessagingSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Messaging");
		}

		private void menuItemMessagingButs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMessagingButSetup FormM=new FormMessagingButSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Messaging");
		}

		private void menuItemMisc_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormMisc FormM=new FormMisc();
			FormM.ShowDialog();
			//signalLastRefreshed=MiscData.GetNowDate();
			if(timerSignals.Interval==0){
				timerSignals.Enabled=false;
			}
			else{
				timerSignals.Interval=PrefB.GetInt("ProcessSigsIntervalInSecs")*1000;
				timerSignals.Enabled=true;
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Misc");
		}

		private void menuItemOperatories_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormOperatories FormO=new FormOperatories();
			FormO.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Operatories");
		}

		private void menuItemPatFieldDefs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormPatFieldDefs FormP=new FormPatFieldDefs();
			FormP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Patient Field Defs");
		}

		private void menuItemPayPeriods_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormPayPeriods FormP=new FormPayPeriods();
			FormP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Pay Periods");
		}

		private void menuItemPractice_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPractice FormPr=new FormPractice();
			FormPr.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Practice Info");
		}

		private void menuItemProcedureButtons_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormProcButtons FormPB=new FormProcButtons();
			FormPB.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Procedure Buttons");	
		}

		private void menuItemLinks_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormProgramLinks FormPL=new FormProgramLinks();
			FormPL.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Program Links");
		}

		private void menuItemQuestions_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormQuestionDefs FormQ=new FormQuestionDefs();
			FormQ.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Questionnaire");
		}

		private void menuItemRecall_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRecallSetup FormRS=new FormRecallSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall");	
		}

		private void menuItemSecurity_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)){
				return;
			}
			FormSecurity FormS=new FormSecurity(); 
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"");
		}
		
		//Setup-Schedules
		private void menuItemPracDef_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules)){
				return;
			}
			FormSchedDefault FormSD=new FormSchedDefault(ScheduleType.Practice);
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"Practice Default");
		}

		private void menuItemPracSched_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules)){
				return;
			}
			FormSchedPractice FormSP=new FormSchedPractice(ScheduleType.Practice);
			FormSP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"Practice");
		}

		private void menuItemProvDefault_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules)){
				return;
			}
			FormSchedDefault FormSD=new FormSchedDefault(ScheduleType.Provider);
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"Provider Default");	
		}

		private void menuItemProvSched_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Schedules)){
				return;
			}
			FormSchedPractice FormSP=new FormSchedPractice(ScheduleType.Provider);
			FormSP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"Provider");
		}

		private void menuItemBlockoutDefault_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)){
				return;
			}
			FormSchedDefault FormSD=new FormSchedDefault(ScheduleType.Blockout);
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Default");	
		}

		//Lists

		private void menuItemProcCodes_Click(object sender, System.EventArgs e) {
			//security handled within form
			FormProcCodes FormP=new FormProcCodes();
			FormP.ShowDialog();	
		}

		private void menuItemClinics_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClinics FormC=new FormClinics();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Clinics");
		}
		
		private void menuItemContacts_Click(object sender, System.EventArgs e) {
			FormContacts FormC=new FormContacts();
			FormC.ShowDialog();
		}

		private void menuItemCounties_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormCounties FormC=new FormCounties();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Counties");
		}

		private void menuItemSchoolClass_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSchoolClasses FormS=new FormSchoolClasses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Classes");
		}

		private void menuItemSchoolCourses_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSchoolCourses FormS=new FormSchoolCourses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Courses");
		}

		private void menuItemEmployees_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormEmployee FormEmp=new FormEmployee();
			FormEmp.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Employees");	
		}

		private void menuItemEmployers_Click(object sender, System.EventArgs e) {
			FormEmployers FormE=new FormEmployers();
			FormE.ShowDialog();
		}

		private void menuItemInstructors_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInstructors FormI=new FormInstructors();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Instructors");
		}

		private void menuItemCarriers_Click(object sender, System.EventArgs e) {
			FormCarriers FormC=new FormCarriers();
			FormC.ShowDialog();
			RefreshCurrentModule();
		}

		private void menuItemInsPlans_Click(object sender, System.EventArgs e) {
			FormInsPlans FormIP = new FormInsPlans();
			FormIP.ShowDialog();
			RefreshCurrentModule();
		}

		private void menuItemMedications_Click(object sender, System.EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.ShowDialog();
		}

		private void menuItemProviders_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormProviderSelect FormPS=new FormProviderSelect();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Providers");		
		}

		private void menuItemPrescriptions_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRxSetup FormRxSetup2=new FormRxSetup();
			FormRxSetup2.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rx");		
		}

		private void menuItemReferrals_Click(object sender, System.EventArgs e) {
			FormReferralSelect FormRS=new FormReferralSelect();
			FormRS.ShowDialog();		
		}

		private void menuItemSchools_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSchools FormS=new FormSchools();
			FormS.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental Schools");
		}

		private void menuItemZipCodes_Click(object sender, System.EventArgs e) {
			//if(!Security.IsAuthorized(Permissions.Setup)){
			//	return;
			//}
			FormZipCodes FormZ=new FormZipCodes();
			FormZ.ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Setup,"Zip Codes");
		}

		//Reports
		private void menuItemReportsSetup_Click(object sender, System.EventArgs e) {
			try{
				Process.Start("RdlDesigner.exe");
			}
			catch{
				MsgBox.Show(this,"Could not locate RdlDesigner.exe.");
			}
		}

		private void menuItemUserQuery_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQuery)){
				return;
			}
			FormQuery FormQuery2=new FormQuery();
			FormQuery2.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.UserQuery,0,"");
		}

		private void menuItemPracticeWebReports_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			try{
				Process.Start("PWReports.exe");
			}
			catch{
				MessageBox.Show("PracticeWeb Reports module unavailable.");
			}
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Practice Web");
		}

		private void menuItemRpProdInc_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpProdInc FormPI=new FormRpProdInc();
			FormPI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Production and Income");
		}

		//Reports-Daily
		private void menuItemRpAdj_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpAdjSheet FormAdjSheet=new FormRpAdjSheet();
			FormAdjSheet.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Adjustments");
		}

		/*private void menuItemRpDepSlip_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpDepositSlip FormDS=new FormRpDepositSlip();
			FormDS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Deposit Slip");
		}*/

		private void menuItemRpPay_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpPaySheet FormPaySheet=new FormRpPaySheet();
			FormPaySheet.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Payments");
		}
		
		private void menuItemRpProc_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpProcSheet FormProcSheet=new FormRpProcSheet();
			FormProcSheet.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Procedures");	
		}

		private void menuItemRpWriteoff_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)) {
				return;
			}
			FormRpWriteoffSheet FormW=new FormRpWriteoffSheet();
			FormW.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Writeoffs");	
		}

		private void menuItemRpProcNote_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpProcNote FormPN=new FormRpProcNote();
			FormPN.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Procedure Notes");
		}

		//Reports-Monthly
		private void menuItemRpAging_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpAging FormA=new FormRpAging();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Aging");
		}

		private void menuItemRpClaimsNotSent_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpClaimNotSent FormClaim=new FormRpClaimNotSent();
			FormClaim.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Claims Not Sent");
		}

		private void menuItemRpCapitation_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpCapitation FormC=new FormRpCapitation();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Capitation");
		}

		private void menuItemRpFinanceCharge_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpFinanceCharge FormRpFinance=new FormRpFinanceCharge();
			FormRpFinance.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Finance Charges");
		}

		private void menuItemRpOutInsClaims_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpOutInsClaims FormOut=new FormRpOutInsClaims();
			FormOut.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Outstanding Insurance Claims");
		}

		private void menuItemRpProcNoBilled_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpProcNotBilledIns FormProc=new FormRpProcNotBilledIns();
			FormProc.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Procedures not billed to insurance.");
		}

		//Reports-Lists
		private void menuAppointments_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpAppointments FormA=new FormRpAppointments();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Appointments");
		}

		private void menuItemBirthdays_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpBirthday FormB=new FormRpBirthday();
			FormB.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Birthdays");
		}

		private void menuItemInsCarriers_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpInsCo FormInsCo=new FormRpInsCo();
			FormInsCo.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Insurance Carriers");	
		}

		private void menuItemPatList_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpPatients FormPatients=new FormRpPatients();
			FormPatients.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Patient List");				
		}

		private void menuItemRxs_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpPrescriptions FormPrescript=new FormRpPrescriptions();
			FormPrescript.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Rx");
		}

		private void menuItemRpProcCodes_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpProcCodes FormProcCodes=new FormRpProcCodes();
			FormProcCodes.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Procedure Codes");
		}

		private void menuItemRefs_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpReferrals FormReferral=new FormRpReferrals();
			FormReferral.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Referrals");	
		}

		private void menuItemRouting_Click(object sender,EventArgs e) {
			FormRpRouting FormR=new FormRpRouting();
			FormR.ShowDialog();
		}

		//Public Health
		private void menuItemPHRawScreen_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpPHRawScreen FormPH=new FormRpPHRawScreen();
			FormPH.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PH Raw Screening");
		}

		private void menuItemPHRawPop_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpPHRawPop FormPH=new FormRpPHRawPop();
			FormPH.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PH Raw population");
		}

		private void menuItemPHScreen_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			MessageBox.Show("This report is incomplete.");
			//SecurityLogs.MakeLogEntry(Permissions.Reports,"");
		}

		private void menuItemCourseGrades_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)){
				return;
			}
			FormRpCourseGrades FormR=new FormRpCourseGrades();
			FormR.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Dental School Grades");
		}

		//Custom Reports
		private void menuItemRDLReport_Click(object sender,System.EventArgs e) {
			FormReport FormR=new FormReport();
			FormR.SourceFilePath=PrefB.GetString("DocPath")+PrefB.GetString("ReportFolderName")+"\\"+
				((MenuItem)sender).Text+".rdl";
			FormR.ShowDialog();
		}

		//Letters
		/*
		private void menuItemLetterSetup_Click(object sender, System.EventArgs e) {
			FormLetterSetup FormLS=new FormLetterSetup();
			FormLS.ShowDialog();
		}*/

		/*
		private void menuItemLetter_Click(object sender, System.EventArgs e) {
			MessageBox.Show(((MenuItem)sender).Index.ToString());
		}*/

		//Tools
		private void menuItemPrintScreen_Click(object sender, System.EventArgs e) {
			FormPrntScrn FormPS=new FormPrntScrn();
			FormPS.ShowDialog();
		}

		private void menuItemDatabaseMaintenance_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormDatabaseMaintenance FormDM=new FormDatabaseMaintenance();
			FormDM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Database Maintenance");
		}

		private void menuTelephone_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormTelephone FormT=new FormTelephone();
			FormT.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Telephone");
		}

		private void menuItemPatientImport_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)){
				return;
			}
			FormImport FormI=new FormImport();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Patient Import Tool");
		}

		/*
		private void menuItemPaymentPlans_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPayPlanUpdate FormPPU=new FormPayPlanUpdate();
			FormPPU.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,"Payment Plan Update");
		}*/

		private void menuItemAuditTrail_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			FormAudit FormA=new FormAudit();
			FormA.CurPatNum=CurPatNum;
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Audit Trail");
		}

		private void menuItemImportXML_Click(object sender, System.EventArgs e) {
			FormImportXML FormI=new FormImportXML();
			FormI.ShowDialog();
		}

		private void menuItemAging_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormAging FormAge=new FormAging();
			FormAge.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Aging Update");
		}

		private void menuItemFinanceCharge_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormFinanceCharges FormFC=new FormFinanceCharges();
			FormFC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Run Finance Charges");
		}

		private void menuItemRepeatingCharges_Click(object sender, System.EventArgs e) {
			FormRepeatChargesUpdate FormR=new FormRepeatChargesUpdate();
			FormR.ShowDialog();
		}

		private void menuItemTerminal_Click(object sender,EventArgs e) {
			FormTerminal FormT=new FormTerminal();
			FormT.Show();
			//Application.Exit();//always close after coming out of terminal mode as a safety precaution.
		}

		private void menuItemTerminalManager_Click(object sender,EventArgs e) {
			FormTerminalManager FormT=new FormTerminalManager();
			FormT.Show();
		}

		private void menuItemTranslation_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormTranslationCat FormTC=new FormTranslationCat();
			FormTC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Translations");
		}

		private void menuItemScreening_Click(object sender, System.EventArgs e) {
			FormScreenings FormS=new FormScreenings();
			FormS.ShowDialog();
		}

		//Help
		private void menuItemHelpWindows_Click(object sender, System.EventArgs e) {
			try{
				Process.Start("Help.chm");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		private void menuItemHelpContents_Click(object sender, System.EventArgs e) {
			try{
				Process.Start("http://www.open-dent.com/manual/toc.html");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		private void menuItemHelpIndex_Click(object sender, System.EventArgs e) {
			try{
				Process.Start(@"http://www.open-dent.com/manual/alphabetical.html");
			}
			catch{
				MessageBox.Show("Could not find file.");
			}
		}

		private void menuItemRemote_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,true,"A remote connection will now be attempted. Do NOT continue unless you are already on the phone with us.  Do you want to continue?"))
			{
				return;
			}
			try{
				Process.Start("remoteclient.exe");//Network streaming remote client or any other similar client
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		/*private void menuItemRemote2_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"A remote connection will now be attempted. Do NOT continue unless you are already on the phone with us.  Do you want to continue?")) {
				return;
			}
			try {
				Process.Start("remoteclient2.exe");//Network streaming remote client or any other similar client
			}
			catch {
				MsgBox.Show(this,"Could not find file.");
			}
		}*/

		private void menuItemUpdate_Click(object sender, System.EventArgs e) {
			
			FormUpdate FormU = new FormUpdate();
			FormU.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Update Version");
		}

		

		

		

		

		

		

		

		

	

		

		

		

		

		

		

		

		

		

	
		

		

		

		

		

		



		/*private void menuItemDaily_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) {
			//MessageBox.Show(e.Bounds.ToString());
			e.Graphics.DrawString("Dailyyyy",new Font("Microsoft Sans Serif",8),Brushes.Black,e.Bounds.X,e.Bounds.Y);
		}

		private void menuItemDaily_Click(object sender, System.EventArgs e) {
		
		}*/

		#endregion

		
	
		

		

		

	

		

		

	}
}
