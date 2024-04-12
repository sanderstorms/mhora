/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Mhora.Components.Controls;
using Mhora.Components.DasaControl;
using Mhora.Components.File;
using Mhora.Components.Jhora;
using Mhora.Components.VargaControl;
using Mhora.Dasas.NakshatraDasa;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Components;

/// <summary>
///     Summary description for MhoraChild.
/// </summary>
public class MhoraChild : Form
{
	private IContainer components;
	private readonly Horoscope h;
	private          MainMenu  childMenu;

	private MhoraSplitContainer Contents;

	private MainMenu mainMenu1;
	private MenuItem menuCalcOpts;
	private MenuItem menuDobOptions;
	private MenuItem menuItem1;
	private MenuItem menuItem2;
	private MenuItem menuItem3;
	private MenuItem menuItem4;
	private MenuItem menuItem5;
	private MenuItem menuItem6;
	private MenuItem menuItemChartNotes;
	private MenuItem menuItemFile;
	private MenuItem menuItemFileClose;
	private MenuItem menuItemFilePrint;
	private MenuItem menuItemFileSave;
	private MenuItem menuItemFileSaveAs;
	private MenuItem menuItemPrintPreview;
	private MenuItem menuLayout2by2;
	private MenuItem menuLayout3by3;
	private MenuItem menuLayoutJhora;
	private MenuItem menuLayoutTabbed;
	private MenuItem menuStrengthOpts;
	private Controls.TimeAdjustment timeAdjustment;
    private MenuItem menuItemTimeAdjustment;
    public string   mJhdFileName;

	public MhoraChild(Horoscope _h)
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		//
		// TODO: Add any constructor code after InitializeComponent call
		//
		h                       =  _h;
		timeAdjustment.Horoscope = _h;
		timeAdjustment.OnChange += OnChangeTime;
	}

	private void OnChangeTime(DateTime dateTime)
	{
		h.Info.DateOfBirth = dateTime;
		h.OnChanged();
	}

	private void OnAdjustTime(object sender, EventArgs e)
	{
		timeAdjustment.Visible ^= true;
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		timeAdjustment.Left = Width - timeAdjustment.Width - 10;
	}


	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
		}

		base.Dispose(disposing);
	}

#region Windows Form Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
            this.components = new System.ComponentModel.Container();
            this.childMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemFileSave = new System.Windows.Forms.MenuItem();
            this.menuItemFileSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItemFileClose = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemPrintPreview = new System.Windows.Forms.MenuItem();
            this.menuItemFilePrint = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItemChartNotes = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuDobOptions = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuLayoutJhora = new System.Windows.Forms.MenuItem();
            this.menuLayoutTabbed = new System.Windows.Forms.MenuItem();
            this.menuLayout2by2 = new System.Windows.Forms.MenuItem();
            this.menuLayout3by3 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuStrengthOpts = new System.Windows.Forms.MenuItem();
            this.menuCalcOpts = new System.Windows.Forms.MenuItem();
            this.menuItemTimeAdjustment = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.timeAdjustment = new Mhora.Components.Controls.TimeAdjustment();
            this.SuspendLayout();
            // 
            // childMenu
            // 
            this.childMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItem1,
            this.menuItemTimeAdjustment});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFileSave,
            this.menuItemFileSaveAs,
            this.menuItemFileClose,
            this.menuItem5,
            this.menuItemPrintPreview,
            this.menuItemFilePrint,
            this.menuItem6,
            this.menuItemChartNotes});
            this.menuItemFile.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItemFile.Text = "&File";
            // 
            // menuItemFileSave
            // 
            this.menuItemFileSave.Index = 0;
            this.menuItemFileSave.MergeOrder = 1;
            this.menuItemFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuItemFileSave.Text = "&Save";
            this.menuItemFileSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
            // 
            // menuItemFileSaveAs
            // 
            this.menuItemFileSaveAs.Index = 1;
            this.menuItemFileSaveAs.MergeOrder = 1;
            this.menuItemFileSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.menuItemFileSaveAs.Text = "Save &As";
            this.menuItemFileSaveAs.Click += new System.EventHandler(this.menuItemFileSaveAs_Click);
            // 
            // menuItemFileClose
            // 
            this.menuItemFileClose.Index = 2;
            this.menuItemFileClose.MergeOrder = 1;
            this.menuItemFileClose.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
            this.menuItemFileClose.Text = "&Close";
            this.menuItemFileClose.Click += new System.EventHandler(this.menuItemFileClose_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.MergeOrder = 1;
            this.menuItem5.Text = "-";
            // 
            // menuItemPrintPreview
            // 
            this.menuItemPrintPreview.Index = 4;
            this.menuItemPrintPreview.MergeOrder = 1;
            this.menuItemPrintPreview.Text = "Print Pre&view";
            this.menuItemPrintPreview.Click += new System.EventHandler(this.menuItemPrintPreview_Click);
            // 
            // menuItemFilePrint
            // 
            this.menuItemFilePrint.Index = 5;
            this.menuItemFilePrint.MergeOrder = 1;
            this.menuItemFilePrint.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.menuItemFilePrint.Text = "&Print";
            this.menuItemFilePrint.Click += new System.EventHandler(this.menuItemFilePrint_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 6;
            this.menuItem6.MergeOrder = 1;
            this.menuItem6.Text = "-";
            // 
            // menuItemChartNotes
            // 
            this.menuItemChartNotes.Index = 7;
            this.menuItemChartNotes.MergeOrder = 1;
            this.menuItemChartNotes.Text = "Chart Notes";
            this.menuItemChartNotes.Click += new System.EventHandler(this.menuItemChartNotes_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuDobOptions,
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
            this.menuItem1.MergeOrder = 1;
            this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItem1.Text = "&Options";
            // 
            // menuDobOptions
            // 
            this.menuDobOptions.Index = 0;
            this.menuDobOptions.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.menuDobOptions.Text = "&Birth Data && Events";
            this.menuDobOptions.Click += new System.EventHandler(this.menuDobOptions_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuLayoutJhora,
            this.menuLayoutTabbed,
            this.menuLayout2by2,
            this.menuLayout3by3});
            this.menuItem2.Text = "Layout";
            // 
            // menuLayoutJhora
            // 
            this.menuLayoutJhora.Index = 0;
            this.menuLayoutJhora.Text = "2 x &1";
            this.menuLayoutJhora.Click += new System.EventHandler(this.menuLayoutJhora_Click);
            // 
            // menuLayoutTabbed
            // 
            this.menuLayoutTabbed.Index = 1;
            this.menuLayoutTabbed.Text = "2 x 1 (&Tabbed)";
            this.menuLayoutTabbed.Click += new System.EventHandler(this.menuLayoutTabbed_Click);
            // 
            // menuLayout2by2
            // 
            this.menuLayout2by2.Index = 2;
            this.menuLayout2by2.Text = "&2 x 2";
            this.menuLayout2by2.Click += new System.EventHandler(this.menuLayout2by2_Click);
            // 
            // menuLayout3by3
            // 
            this.menuLayout3by3.Index = 3;
            this.menuLayout3by3.Text = "&3 x 3";
            this.menuLayout3by3.Click += new System.EventHandler(this.menuLayout3by3_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuStrengthOpts,
            this.menuCalcOpts});
            this.menuItem4.MergeOrder = 2;
            this.menuItem4.Text = "Advanced Options";
            // 
            // menuStrengthOpts
            // 
            this.menuStrengthOpts.Index = 0;
            this.menuStrengthOpts.MergeOrder = 2;
            this.menuStrengthOpts.Text = "Edit Chart &Strength Options";
            this.menuStrengthOpts.Click += new System.EventHandler(this.menuStrengthOpts_Click);
            // 
            // menuCalcOpts
            // 
            this.menuCalcOpts.Index = 1;
            this.menuCalcOpts.MergeOrder = 2;
            this.menuCalcOpts.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            this.menuCalcOpts.Text = "Edit Chart &Calculation Options";
            this.menuCalcOpts.Click += new System.EventHandler(this.menuCalcOpts_Click);
            // 
            // menuItemTimeAdjustment
            // 
            this.menuItemTimeAdjustment.Index = 2;
            this.menuItemTimeAdjustment.Text = "Time Adjustment";
            this.menuItemTimeAdjustment.Click += new System.EventHandler(this.OnAdjustTime);
            // 
            // timeAdjustment
            // 
            this.timeAdjustment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timeAdjustment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeAdjustment.Horoscope = null;
            this.timeAdjustment.Location = new System.Drawing.Point(496, 4);
            this.timeAdjustment.Margin = new System.Windows.Forms.Padding(4);
            this.timeAdjustment.Name = "timeAdjustment";
            this.timeAdjustment.Size = new System.Drawing.Size(249, 39);
            this.timeAdjustment.TabIndex = 0;
            // 
            // MhoraChild
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(758, 266);
            this.Controls.Add(this.timeAdjustment);
            this.Menu = this.childMenu;
            this.Name = "MhoraChild";
            this.Text = "MhoraChild";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MhoraChild_Closing);
            this.Load += new System.EventHandler(this.MhoraChild_Load);
            this.ResumeLayout(false);

	}

#endregion

	private void MhoraChild_Load(object sender, EventArgs e)
	{
		Contents = null;
		//this.menuLayoutJhora_Click(sender,e);
		//this.menuLayout2by2_Click(sender, e);
		menuLayoutTabbed_Click(sender, e);
		//this.menuLayoutJhora_Click (sender, e);
		/*
		MhoraDasaControl dc = //new BasicCalculationsControl(h);
		    new MhoraDasaControl(h, new VimsottariDasa(h));
		MhoraControlContainer c_dc = new MhoraControlContainer(dc);

		DivisionalChart div_rasi = new DivisionalChart(h);
		MhoraControlContainer c_div_rasi = new MhoraControlContainer(div_rasi);

		DivisionalChart div_nav = new DivisionalChart(h);
		div_nav.options.Division = DivisionType.Navamsa;
		MhoraControlContainer c_div_nav = new MhoraControlContainer(div_nav);


		MhoraSplitContainer sp_ud = new MhoraSplitContainer(c_div_rasi);
		sp_ud.Control2 = c_div_nav;
		sp_ud.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;

		MhoraSplitContainer sp_dc = new MhoraSplitContainer(c_dc);

		MhoraSplitContainer sp_lr = new MhoraSplitContainer(sp_ud);
		sp_lr.Control2 = sp_dc;

		int sz = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2 - 50;
		sp_lr.Control1.Width = sz;
		sp_ud.Control1.Height = sz;
		//sp_lr.Control1.Height = 300;
		//sp_lr.Control2.Width = 300;
		//sp_lr.Control2.Height = 300;





		this.Controls.AddRange(new Control[]{sp_lr});

		DivisionalChart ds = new DivisionalChart(h);
		Splitter sp = new Splitter();
		VimsottariDasa vd1 = new VimsottariDasa(h);
		//VimsottariDasa vd2 = new VimsottariDasa(h);
		//vd2.options.SeedBody = VimsottariDasa.UserOptions.StartBodyType.Moon;
		//vd2.options.start_graha = Body.Type.Moon;
		//vd2.options.start_graha = Body.Type.Moon;
		MhoraDasaControl dc1 = new MhoraDasaControl(h, vd1,sp);
		//MhoraDasaControl dc2 = new MhoraDasaControl(h, vd2);

		sp.Dock = DockStyle.Top;
		dc1.Dock = DockStyle.Top;
		ds.Dock = DockStyle.Fill;

		//dc2.Dock = DockStyle.Fill;
		this.Controls.AddRange(new Control[]{ds, sp, dc1});
		sp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
*/
	}

	public Horoscope Horoscope => h;

	private void rtOutput_TextChanged(object sender, EventArgs e)
	{
	}

	private void DoNothing(object a)
	{
	}

	public void menuShowDobOptions()
	{
		using (var birthDetails = new BirthDetailsDialog(h.Info))
		{
			if (birthDetails.ShowDialog() == DialogResult.OK)
			{
				h.UpdateHoraInfo(birthDetails.Info);
				Refresh();
			}
		}
		//var f = new MhoraOptions(h.info.Clone(), h.UpdateHoraInfo);
		//f.ShowDialog();
	}

	private void menuDobOptions_Click(object sender, EventArgs e)
	{
		menuShowDobOptions();
		//object wrapper = new GlobalizedPropertiesWrapper((HoraInfo)h.info.Clone());
	}

	public void saveJhdFile()
	{
		if (mJhdFileName == null || mJhdFileName.Length == 0)
		{
			saveAsJhdFile();
		}

		try
		{
			if (h.Info.FileType == HoraInfo.EFileType.JagannathaHora)
			{
				new Jhd(mJhdFileName).ToFile(h.Info);
			}
			else
			{
				new Mhd(mJhdFileName).ToFile(h.Info);
			}
		}
		catch (ArgumentNullException)
		{
		}
		catch
		{
			MessageBox.Show(this, "Error Saving File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	public void saveAsJhdFile()
	{
		var ofd = new SaveFileDialog();
		ofd.AddExtension = true;
		ofd.Filter       = "Jagannatha Hora Files (*.jhd)|*.jhd|Mudgala Hora Files (*.mhd)|*.mhd";
		ofd.FilterIndex  = 1;

		if (ofd.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		if (ofd.FileName.Length == 0)
		{
			return;
		}

		var sparts = ofd.FileName.ToLower().Split('.');
		try
		{
			if (sparts[sparts.Length - 1] == "jhd")
			{
				h.Info.FileType = HoraInfo.EFileType.JagannathaHora;
				new Jhd(ofd.FileName).ToFile(h.Info);
			}
			else
			{
				h.Info.Export (ofd.FileName);
			}

			mJhdFileName = ofd.FileName;
		}
		catch (ArgumentNullException)
		{
		}
		//catch 
		//{
		//	MessageBox.Show(this, "Error Saving File", "Error", 
		//		MessageBoxButtons.OK, MessageBoxIcon.Error);
		//}
	}

	private void menuItemFileSaveAs_Click(object sender, EventArgs e)
	{
		saveAsJhdFile();
	}

	private void menuItemFileSave_Click(object sender, EventArgs e)
	{
		saveJhdFile();
	}


	private void menuItemFileClose_Click(object sender, EventArgs e)
	{
		Close();
		Dispose();
	}

	private void menuLayoutJhora_Click(object sender, EventArgs e)
	{
		if (Contents != null)
		{
			Controls.Remove(Contents);
		}

		var dc   = new MhoraDasaControl(h, new VimsottariDasa(h));
		var c_dc = new MhoraControlContainer(dc);

		var div_rasi   = new DivisionalChart(h);
		var c_div_rasi = new MhoraControlContainer(div_rasi);

		var div_nav = new DivisionalChart(h);
		div_nav.options.Varga = new Division(DivisionType.Navamsa);
		div_nav.SetOptions(div_nav.options);
		var c_div_nav = new MhoraControlContainer(div_nav);


		var sp_ud = new MhoraSplitContainer(c_div_rasi);
		sp_ud.Control2 = c_div_nav;
		sp_ud.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;

		var sp_dc = new MhoraSplitContainer(c_dc);

		var sp_lr = new MhoraSplitContainer(sp_ud);
		sp_lr.Control2 = sp_dc;

		var sz = Screen.PrimaryScreen.WorkingArea.Height / 2 - 50;
		sp_lr.Control1.Width  = sz;
		sp_ud.Control1.Height = sz;
		Controls.AddRange([
			                  sp_lr
		                  ]);
		Contents = sp_lr;
	}

	private void menuLayoutTabbed_Click(object sender, EventArgs e)
	{
		if (Contents != null)
		{
			Controls.Remove(Contents);
		}

		MhoraControl mc = new JhoraMainTab(h);
		//MhoraDasaControl dc = new MhoraDasaControl(h, new VimsottariDasa(h));
		var c_dc = new MhoraControlContainer(mc);

		var div_rasi   = new DivisionalChart(h);
		var c_div_rasi = new MhoraControlContainer(div_rasi);

		var div_nav = new DivisionalChart(h);
		div_nav.options.Varga = new Division(DivisionType.Navamsa);
		div_nav.SetOptions(div_nav.options);
		var c_div_nav = new MhoraControlContainer(div_nav);


		var sp_ud = new MhoraSplitContainer(c_div_rasi);
		sp_ud.Control2 = c_div_nav;
		sp_ud.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;

		var sp_dc = new MhoraSplitContainer(c_dc);

		var sp_lr = new MhoraSplitContainer(sp_ud);
		sp_lr.Control2 = sp_dc;

		var sz = Screen.PrimaryScreen.WorkingArea.Height / 2 - 50;
		sp_lr.Control1.Width  = sz;
		sp_ud.Control1.Height = sz;
		Controls.AddRange([
			                  sp_lr
		                  ]);
		Contents = sp_lr;
	}

	private void menuLayout2by2_Click(object sender, EventArgs e)
	{
		if (Contents != null)
		{
			Controls.Remove(Contents);
		}

		var dc1   = new MhoraDasaControl(h, new VimsottariDasa(h));
		var c_dc1 = new MhoraControlContainer(dc1);

		var dc2   = new BasicCalculationsControl(h);
		var c_dc2 = new MhoraControlContainer(dc2);

		var div_rasi   = new DivisionalChart(h);
		var c_div_rasi = new MhoraControlContainer(div_rasi);

		var div_nav = new DivisionalChart(h);
		div_nav.options.Varga = new Division(DivisionType.Navamsa);
		div_nav.SetOptions(div_nav.options);
		var c_div_nav = new MhoraControlContainer(div_nav);


		var sp_ud = new MhoraSplitContainer(c_div_rasi);
		sp_ud.Control2 = c_div_nav;
		sp_ud.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;

		var sp_ud2 = new MhoraSplitContainer(c_dc1);
		sp_ud2.Control2 = c_dc2;
		sp_ud2.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;

		var sp_dc = new MhoraSplitContainer(sp_ud2);

		var sp_lr = new MhoraSplitContainer(sp_ud);
		sp_lr.Control2 = sp_dc;

		var sz = Screen.PrimaryScreen.WorkingArea.Height / 2 - 50;
		sp_lr.Control1.Width   = sz;
		sp_ud.Control1.Height  = sz;
		sp_ud2.Control1.Height = sz;
		Controls.AddRange([
			                  sp_lr
		                  ]);
		Contents = sp_lr;
	}

	private void MhoraChild_Closing(object sender, CancelEventArgs e)
	{
		//this.Close();
		//this.Dispose();
	}

	public object SetCalcOptions(object o)
	{
		var ho = (HoroscopeOptions) o;
		h.Options.Copy(ho);
		h.OnChanged();
		return h.Options.Clone();
	}

	public object SetStrengthOptions(object o)
	{
		var so = (StrengthOptions) o;
		h.StrengthOptions.Copy(so);
		h.OnChanged();
		return h.StrengthOptions.Clone();
	}

	private void menuCalcOpts_Click(object sender, EventArgs e)
	{
		var f = new MhoraOptions(h.Options, SetCalcOptions);
		f.ShowDialog();
	}


	private void menuStrengthOpts_Click(object sender, EventArgs e)
	{
		var f = new MhoraOptions(h.StrengthOptions, SetStrengthOptions);
		f.ShowDialog();
	}

	private void menuLayout3by3_Click(object sender, EventArgs e)
	{
		if (Contents != null)
		{
			Controls.Remove(Contents);
		}

		var d1   = new DivisionalChart(h);
		var c_d1 = new MhoraControlContainer(d1);

		var d2 = new DivisionalChart(h);
		d2.options.Varga = new Division(DivisionType.DrekkanaParasara);
		d2.SetOptions(d2.options);
		var c_d2 = new MhoraControlContainer(d2);

		var d3 = new DivisionalChart(h);
		d3.options.Varga = new Division(DivisionType.Navamsa);
		d3.SetOptions(d3.options);
		var c_d3 = new MhoraControlContainer(d3);

		var d4 = new DivisionalChart(h);
		d4.options.Varga = new Division(DivisionType.Saptamsa);
		d4.SetOptions(d4.options);
		var c_d4 = new MhoraControlContainer(d4);

		var d5 = new DivisionalChart(h);
		d5.options.Varga = new Division(DivisionType.Dasamsa);
		d5.SetOptions(d5.options);
		var c_d5 = new MhoraControlContainer(d5);

		var d6 = new DivisionalChart(h);
		d6.options.Varga = new Division(DivisionType.Vimsamsa);
		d6.SetOptions(d6.options);
		var c_d6 = new MhoraControlContainer(d6);


		var sp_ud1 = new MhoraSplitContainer(c_d1);
		sp_ud1.Control2 = c_d2;
		sp_ud1.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;

		var sp_ud2 = new MhoraSplitContainer(c_d3);
		sp_ud2.Control2 = c_d4;
		sp_ud2.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;

		var sp_ud3 = new MhoraSplitContainer(c_d5);
		sp_ud3.Control2 = c_d6;
		sp_ud3.DrawDock = MhoraSplitContainer.DrawStyle.UpDown;


		var lr2 = new MhoraSplitContainer(sp_ud2);
		lr2.Control2 = sp_ud3;


		var lr1 = new MhoraSplitContainer(sp_ud1);
		lr1.Control2 = lr2;


		var h_sz = Screen.PrimaryScreen.WorkingArea.Height / 2 - 30;
		var w_sz = Screen.PrimaryScreen.WorkingArea.Width  / 3 - 30;
		var sz   = Math.Min(h_sz, w_sz);
		lr1.Control1.Width     = sz;
		lr2.Control1.Width     = sz;
		sp_ud1.Control1.Height = sz;
		sp_ud2.Control1.Height = sz;
		sp_ud3.Control1.Height = sz;

		Controls.AddRange([
			                  lr1
		                  ]);
		Contents = lr1;
	}

	public object OnCalcOptsChanged(object o)
	{
		h.Options.Copy((HoroscopeOptions) o);
		h.OnChanged();
		return h.Options.Clone();
	}

	private void menuEditCalcOpts_Click(object sender, EventArgs e)
	{
		new MhoraOptions(h.Options, OnCalcOptsChanged).ShowDialog();
	}

	public void menuPrint()
	{
		var mdoc     = new MhoraPrintDocument(h);
		var dlgPrint = new PrintDialog();
		dlgPrint.Document = mdoc;

		if (dlgPrint.ShowDialog() == DialogResult.OK)
		{
			mdoc.Print();
		}
	}

	private void menuItemFilePrint_Click(object sender, EventArgs e)
	{
		menuPrint();
	}

	public void menuPrintPreview()
	{
		var mdoc       = new MhoraPrintDocument(h);
		var dlgPreview = new PrintPreviewDialog();
		dlgPreview.Document = mdoc;
		dlgPreview.ShowDialog();
	}

	private void menuItemPrintPreview_Click(object sender, EventArgs e)
	{
		menuPrintPreview();
	}

	private void menuItemChartNotes_Click(object sender, EventArgs e)
	{
		if (null == mJhdFileName || mJhdFileName.Length == 0)
		{
			MessageBox.Show("Please save the chart before editing notes");
			return;
		}

		var fi  = new FileInfo(mJhdFileName);
		var ext = fi.Extension;

		var sfBase = new string(mJhdFileName.ToCharArray(), 0, mJhdFileName.Length - ext.Length);
		var sfExt  = MhoraGlobalOptions.Instance.ChartNotesFileExtension;
		var sfName = sfBase;

		if (sfExt.Length > 0 && sfExt[0] == '.')
		{
			sfName += sfExt;
		}
		else
		{
			sfName += "." + sfExt;
		}

		try
		{
			if (false == System.IO.File.Exists(sfName))
			{
				System.IO.File.Create(sfName).Close();
			}

			Process.Start(sfName);
		}
		catch
		{
			MessageBox.Show(string.Format("An error occurred. Unable to open file {0}", sfName));
		}
	}

	private void menuItemEvalYogas_Click(object sender, EventArgs e)
	{
		//this.evaluateYogas();
		//FindYogas.Test(h, new Division(DivisionType.Rasi));
	}
}