using System.ComponentModel;
using System.Windows.Forms;
using Mhora.Database.Settings;

namespace Mhora
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private int                childCount;
		public  MhoraGlobalOptions gOpts;
		private MenuItem           mDecreaseFontSize;
		private ImageList          MdiImageList;
		private MainMenu           MdiMenu;
		private StatusBar          MdiStatusBar;
		private ToolBar            MdiToolBar;
		private MenuItem           mEditCalcPrefs;
		private MenuItem           mEditStrengthPrefs;
		private MenuItem           menuItem1;
		private MenuItem           menuItem2;
		private MenuItem           menuItem4;
		private MenuItem           menuItemFile;
		private MenuItem           menuItemFileExit;
		private MenuItem           menuItemFileNew;
		private MenuItem           menuItemFileNewPrasna;
		private MenuItem           menuItemFileOpen;
		private MenuItem           menuItemHelp;
		private MenuItem           menuItemHelpAboutMhora;
		private MenuItem           menuItemHelpAboutSjc;
		private MenuItem           menuItemNewView;
		private MenuItem           menuItemSplash;
		private MenuItem           menuItemWindow;
		private MenuItem           menuItemWindowCascade;
		private MenuItem           menuItemWindowTile;
		private MenuItem           mIncreaseFontSize;
		private MenuItem           mResetPreferences;
		private MenuItem           mResetStrengthPreferences;
		private MenuItem           mSavePreferences;
		private MenuItem           mViewPreferences;
		private ToolBarButton      toolbarButtonDisp;
		private ToolBarButton      toolbarButtonDob;
		private ToolBarButton      toolbarButtonHelp;
		private ToolBarButton      toolbarButtonNew;
		private ToolBarButton      toolbarButtonOpen;
		private ToolBarButton      toolbarButtonPreview;
		private ToolBarButton      toolbarButtonPrint;
		private ToolBarButton      toolbarButtonSave;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.MdiMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemFileNew = new System.Windows.Forms.MenuItem();
			this.menuItemFileNewPrasna = new System.Windows.Forms.MenuItem();
			this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
			this.menuItemFileExit = new System.Windows.Forms.MenuItem();
			this.mViewPreferences = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mEditStrengthPrefs = new System.Windows.Forms.MenuItem();
			this.mEditCalcPrefs = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mResetPreferences = new System.Windows.Forms.MenuItem();
			this.mResetStrengthPreferences = new System.Windows.Forms.MenuItem();
			this.mSavePreferences = new System.Windows.Forms.MenuItem();
			this.mIncreaseFontSize = new System.Windows.Forms.MenuItem();
			this.mDecreaseFontSize = new System.Windows.Forms.MenuItem();
			this.menuItemWindow = new System.Windows.Forms.MenuItem();
			this.menuItemWindowTile = new System.Windows.Forms.MenuItem();
			this.menuItemWindowCascade = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemNewView = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemHelpAboutMhora = new System.Windows.Forms.MenuItem();
			this.menuItemHelpAboutSjc = new System.Windows.Forms.MenuItem();
			this.menuItemSplash = new System.Windows.Forms.MenuItem();
			this.MdiStatusBar = new System.Windows.Forms.StatusBar();
			this.MdiToolBar = new System.Windows.Forms.ToolBar();
			this.toolbarButtonNew = new System.Windows.Forms.ToolBarButton();
			this.toolbarButtonOpen = new System.Windows.Forms.ToolBarButton();
			this.toolbarButtonSave = new System.Windows.Forms.ToolBarButton();
			this.toolbarButtonPrint = new System.Windows.Forms.ToolBarButton();
			this.toolbarButtonPreview = new System.Windows.Forms.ToolBarButton();
			this.toolbarButtonDob = new System.Windows.Forms.ToolBarButton();
			this.toolbarButtonDisp = new System.Windows.Forms.ToolBarButton();
			this.toolbarButtonHelp = new System.Windows.Forms.ToolBarButton();
			this.MdiImageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// MdiMenu
			// 
			this.MdiMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.mViewPreferences,
            this.menuItemWindow,
            this.menuItemHelp});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFileNew,
            this.menuItemFileNewPrasna,
            this.menuItemFileOpen,
            this.menuItemFileExit});
			this.menuItemFile.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.menuItemFile.Text = "&File";
			// 
			// menuItemFileNew
			// 
			this.menuItemFileNew.Index = 0;
			this.menuItemFileNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.menuItemFileNew.Text = "&New";
			this.menuItemFileNew.Click += new System.EventHandler(this.menuItemFileNew_Click);
			// 
			// menuItemFileNewPrasna
			// 
			this.menuItemFileNewPrasna.Index = 1;
			this.menuItemFileNewPrasna.Text = "New &Prasna";
			this.menuItemFileNewPrasna.Click += new System.EventHandler(this.menuItemFileNewPrasna_Click);
			// 
			// menuItemFileOpen
			// 
			this.menuItemFileOpen.Index = 2;
			this.menuItemFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuItemFileOpen.Text = "&Open";
			this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
			// 
			// menuItemFileExit
			// 
			this.menuItemFileExit.Index = 3;
			this.menuItemFileExit.MergeOrder = 2;
			this.menuItemFileExit.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
			this.menuItemFileExit.Text = "E&xit";
			this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
			// 
			// mViewPreferences
			// 
			this.mViewPreferences.Index = 1;
			this.mViewPreferences.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4,
            this.mEditStrengthPrefs,
            this.mEditCalcPrefs,
            this.menuItem2,
            this.mResetPreferences,
            this.mResetStrengthPreferences,
            this.mSavePreferences,
            this.mIncreaseFontSize,
            this.mDecreaseFontSize});
			this.mViewPreferences.MergeOrder = 1;
			this.mViewPreferences.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.mViewPreferences.Text = "&Options";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 0;
			this.menuItem4.MergeOrder = 1;
			this.menuItem4.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
			this.menuItem4.Text = "Edit Global Display Options";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// mEditStrengthPrefs
			// 
			this.mEditStrengthPrefs.Index = 1;
			this.mEditStrengthPrefs.MergeOrder = 1;
			this.mEditStrengthPrefs.Text = "Edit Global Strength Options";
			this.mEditStrengthPrefs.Click += new System.EventHandler(this.mEditStrengthPrefs_Click);
			// 
			// mEditCalcPrefs
			// 
			this.mEditCalcPrefs.Index = 2;
			this.mEditCalcPrefs.MergeOrder = 1;
			this.mEditCalcPrefs.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
			this.mEditCalcPrefs.Text = "Edit Global Calculation Options";
			this.mEditCalcPrefs.Click += new System.EventHandler(this.mEditCalcPrefs_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.MergeOrder = 3;
			this.menuItem2.Text = "-";
			// 
			// mResetPreferences
			// 
			this.mResetPreferences.Index = 4;
			this.mResetPreferences.MergeOrder = 3;
			this.mResetPreferences.Text = "Reset All Options";
			this.mResetPreferences.Click += new System.EventHandler(this.mResetPreferences_Click);
			// 
			// mResetStrengthPreferences
			// 
			this.mResetStrengthPreferences.Index = 5;
			this.mResetStrengthPreferences.MergeOrder = 3;
			this.mResetStrengthPreferences.Text = "Reset Strength Options";
			this.mResetStrengthPreferences.Click += new System.EventHandler(this.mResetStrengthPreferences_Click);
			// 
			// mSavePreferences
			// 
			this.mSavePreferences.Index = 6;
			this.mSavePreferences.MergeOrder = 3;
			this.mSavePreferences.Text = "Save Options";
			this.mSavePreferences.Click += new System.EventHandler(this.mSavePreferences_Click);
			// 
			// mIncreaseFontSize
			// 
			this.mIncreaseFontSize.Index = 7;
			this.mIncreaseFontSize.MergeOrder = 3;
			this.mIncreaseFontSize.Text = "+ Font Size";
			this.mIncreaseFontSize.Click += new System.EventHandler(this.mIncreaseFontSize_Click);
			// 
			// mDecreaseFontSize
			// 
			this.mDecreaseFontSize.Index = 8;
			this.mDecreaseFontSize.MergeOrder = 3;
			this.mDecreaseFontSize.Text = "- Size Size";
			this.mDecreaseFontSize.Click += new System.EventHandler(this.mDecreaseFontSize_Click);
			// 
			// menuItemWindow
			// 
			this.menuItemWindow.Index = 2;
			this.menuItemWindow.MdiList = true;
			this.menuItemWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemWindowTile,
            this.menuItemWindowCascade,
            this.menuItem1,
            this.menuItemNewView});
			this.menuItemWindow.MergeOrder = 2;
			this.menuItemWindow.Text = "&Window";
			// 
			// menuItemWindowTile
			// 
			this.menuItemWindowTile.Index = 0;
			this.menuItemWindowTile.Text = "&Tile";
			this.menuItemWindowTile.Click += new System.EventHandler(this.menuItemWindowTile_Click);
			// 
			// menuItemWindowCascade
			// 
			this.menuItemWindowCascade.Index = 1;
			this.menuItemWindowCascade.Text = "&Cascade";
			this.menuItemWindowCascade.Click += new System.EventHandler(this.menuItemWindowCascade_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.Text = "-";
			// 
			// menuItemNewView
			// 
			this.menuItemNewView.Index = 3;
			this.menuItemNewView.Text = "&New View";
			this.menuItemNewView.Click += new System.EventHandler(this.menuItemNewView_Click);
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = 3;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHelpAboutMhora,
            this.menuItemHelpAboutSjc,
            this.menuItemSplash});
			this.menuItemHelp.MergeOrder = 2;
			this.menuItemHelp.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.menuItemHelp.Text = "&Help";
			// 
			// menuItemHelpAboutMhora
			// 
			this.menuItemHelpAboutMhora.Index = 0;
			this.menuItemHelpAboutMhora.Text = "&About Mudgala Hora";
			this.menuItemHelpAboutMhora.Click += new System.EventHandler(this.menuItemHelpAboutMhora_Click);
			// 
			// menuItemHelpAboutSjc
			// 
			this.menuItemHelpAboutSjc.Index = 1;
			this.menuItemHelpAboutSjc.Text = "About &SJC";
			this.menuItemHelpAboutSjc.Click += new System.EventHandler(this.menuItemHelpAboutSjc_Click);
			// 
			// menuItemSplash
			// 
			this.menuItemSplash.Index = 2;
			this.menuItemSplash.Text = "&View Splash Screen";
			this.menuItemSplash.Click += new System.EventHandler(this.menuItemSplash_Click);
			// 
			// MdiStatusBar
			// 
			this.MdiStatusBar.Location = new System.Drawing.Point(0, 264);
			this.MdiStatusBar.Name = "MdiStatusBar";
			this.MdiStatusBar.Size = new System.Drawing.Size(456, 25);
			this.MdiStatusBar.TabIndex = 1;
			// 
			// MdiToolBar
			// 
			this.MdiToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolbarButtonNew,
            this.toolbarButtonOpen,
            this.toolbarButtonSave,
            this.toolbarButtonPrint,
            this.toolbarButtonPreview,
            this.toolbarButtonDob,
            this.toolbarButtonDisp,
            this.toolbarButtonHelp});
			this.MdiToolBar.DropDownArrows = true;
			this.MdiToolBar.ImageList = this.MdiImageList;
			this.MdiToolBar.Location = new System.Drawing.Point(0, 0);
			this.MdiToolBar.Name = "MdiToolBar";
			this.MdiToolBar.ShowToolTips = true;
			this.MdiToolBar.Size = new System.Drawing.Size(456, 28);
			this.MdiToolBar.TabIndex = 2;
			this.MdiToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MdiToolBar_ButtonClick);
			// 
			// toolbarButtonNew
			// 
			this.toolbarButtonNew.ImageIndex = 0;
			this.toolbarButtonNew.Name = "toolbarButtonNew";
			this.toolbarButtonNew.Tag = "New";
			this.toolbarButtonNew.ToolTipText = "New Chart";
			// 
			// toolbarButtonOpen
			// 
			this.toolbarButtonOpen.ImageIndex = 1;
			this.toolbarButtonOpen.Name = "toolbarButtonOpen";
			this.toolbarButtonOpen.Tag = "ButtonOpen";
			this.toolbarButtonOpen.ToolTipText = "Open Chart";
			// 
			// toolbarButtonSave
			// 
			this.toolbarButtonSave.ImageIndex = 2;
			this.toolbarButtonSave.Name = "toolbarButtonSave";
			this.toolbarButtonSave.Tag = "Save";
			this.toolbarButtonSave.ToolTipText = "Save Chart";
			// 
			// toolbarButtonPrint
			// 
			this.toolbarButtonPrint.ImageIndex = 4;
			this.toolbarButtonPrint.Name = "toolbarButtonPrint";
			this.toolbarButtonPrint.ToolTipText = "Print";
			// 
			// toolbarButtonPreview
			// 
			this.toolbarButtonPreview.ImageIndex = 5;
			this.toolbarButtonPreview.Name = "toolbarButtonPreview";
			this.toolbarButtonPreview.ToolTipText = "Print Preview";
			// 
			// toolbarButtonDob
			// 
			this.toolbarButtonDob.ImageIndex = 7;
			this.toolbarButtonDob.Name = "toolbarButtonDob";
			this.toolbarButtonDob.ToolTipText = "Birth Data, Events";
			// 
			// toolbarButtonDisp
			// 
			this.toolbarButtonDisp.ImageIndex = 8;
			this.toolbarButtonDisp.Name = "toolbarButtonDisp";
			this.toolbarButtonDisp.ToolTipText = "Display Preferences";
			// 
			// toolbarButtonHelp
			// 
			this.toolbarButtonHelp.ImageIndex = 6;
			this.toolbarButtonHelp.Name = "toolbarButtonHelp";
			this.toolbarButtonHelp.Tag = "ButtonHelp";
			this.toolbarButtonHelp.ToolTipText = "Help";
			// 
			// MdiImageList
			// 
			this.MdiImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.MdiImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.MdiImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(456, 289);
			this.Controls.Add(this.MdiToolBar);
			this.Controls.Add(this.MdiStatusBar);
			this.IsMdiContainer = true;
			this.Menu = this.MdiMenu;
			this.Name = "MainForm";
			this.Text = "Mudgala Hora 0.3";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MhoraContainer_Closing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}