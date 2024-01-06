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
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Components;
using Mhora.Hora;
using Mhora.Panchanga;
using Mhora.Settings;
using Mhora.Util;
using Mhora.Varga;

namespace Mhora.Jhora;

/// <summary>
///     Summary description for JhoraMainTab.
/// </summary>
public class JhoraMainTab : MhoraControl
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;

	private bool bTabDasaLoaded;

	private bool bTabPanchangaLoaded;

	//bool bTabTajakaLoaded = false;
	private bool bTabTithiPraveshLoaded;

	private bool bTabTransitsLoaded;

	//bool bTabBasicsLoaded = false;
	private bool bTabVargasLoaded;

	private ContextMenu contextMenu1;
	private TabControl  mTab;
	private TabPage     tabBasics;
	private TabPage     tabDasa;
	private TabPage     tabPanchanga;

	private TabPage tabTithiPravesh;
	private TabPage tabTransits;
	private TabPage tabVargas;

	public JhoraMainTab(Horoscope _h)
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		mTab.TabPages[0] = tabBasics;
		mTab.TabPages[1] = tabVargas;
		mTab.TabPages[2] = tabDasa;
		mTab.TabPages[3] = tabTransits;
		mTab.TabPages[4] = tabPanchanga;
		mTab.TabPages[5] = tabTithiPravesh;

		mTab.SelectedTab = tabBasics;

		//
		// TODO: Add any constructor code after InitializeComponent call
		//
		h                                      =  _h;
		MhoraGlobalOptions.DisplayPrefsChanged += OnRedisplay;
		OnRedisplay(MhoraGlobalOptions.Instance);

		AddControlToTab(tabBasics, new JhoraBasicsTab(h));
		//this.bTabBasicsLoaded = true;
	}

	private void AddControlToTab(TabPage tab, MhoraControl mcontrol)
	{
		var container = new MhoraControlContainer(mcontrol);
		container.Dock = DockStyle.Fill;
		tab.Controls.Add(container);
	}

	public void OnRedisplay(object o)
	{
		Font = MhoraGlobalOptions.Instance.GeneralFont;
		/*
		this.tabBasics.Font = this.Font;
		this.tabDasa.Font = this.Font;
		this.tabPanchanga.Font = this.Font;
		this.tabTithiPravesh.Font = this.Font;
		this.tabTransits.Font = this.Font;
		this.tabVargas.Font = this.Font;
		*/
	}

	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (components != null)
			{
				components.Dispose();
			}
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
		this.mTab            = new System.Windows.Forms.TabControl();
		this.tabBasics       = new System.Windows.Forms.TabPage();
		this.tabTransits     = new System.Windows.Forms.TabPage();
		this.tabDasa         = new System.Windows.Forms.TabPage();
		this.tabVargas       = new System.Windows.Forms.TabPage();
		this.tabPanchanga    = new System.Windows.Forms.TabPage();
		this.tabTithiPravesh = new System.Windows.Forms.TabPage();
		this.contextMenu1    = new System.Windows.Forms.ContextMenu();
		this.mTab.SuspendLayout();
		this.SuspendLayout();
		// 
		// mTab
		// 
		this.mTab.Controls.Add(this.tabBasics);
		this.mTab.Controls.Add(this.tabDasa);
		this.mTab.Controls.Add(this.tabTransits);
		this.mTab.Controls.Add(this.tabVargas);
		this.mTab.Controls.Add(this.tabPanchanga);
		this.mTab.Controls.Add(this.tabTithiPravesh);
		this.mTab.Dock                 =  System.Windows.Forms.DockStyle.Fill;
		this.mTab.Font                 =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.mTab.Location             =  new System.Drawing.Point(0, 0);
		this.mTab.Name                 =  "mTab";
		this.mTab.Padding              =  new System.Drawing.Point(20, 4);
		this.mTab.SelectedIndex        =  0;
		this.mTab.Size                 =  new System.Drawing.Size(472, 256);
		this.mTab.TabIndex             =  0;
		this.mTab.SelectedIndexChanged += new System.EventHandler(this.mTab_SelectedIndexChanged);
		// 
		// tabBasics
		// 
		this.tabBasics.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
		this.tabBasics.Location =  new System.Drawing.Point(4, 25);
		this.tabBasics.Name     =  "tabBasics";
		this.tabBasics.Size     =  new System.Drawing.Size(464, 227);
		this.tabBasics.TabIndex =  1;
		this.tabBasics.Text     =  "Basics";
		this.tabBasics.Click    += new System.EventHandler(this.tabBasics_Click);
		// 
		// tabTransits
		// 
		this.tabTransits.Location =  new System.Drawing.Point(4, 25);
		this.tabTransits.Name     =  "tabTransits";
		this.tabTransits.Size     =  new System.Drawing.Size(464, 227);
		this.tabTransits.TabIndex =  4;
		this.tabTransits.Text     =  "Transits";
		this.tabTransits.Click    += new System.EventHandler(this.tabTransits_Click);
		// 
		// tabDasa
		// 
		this.tabDasa.Location =  new System.Drawing.Point(4, 25);
		this.tabDasa.Name     =  "tabDasa";
		this.tabDasa.Size     =  new System.Drawing.Size(464, 227);
		this.tabDasa.TabIndex =  0;
		this.tabDasa.Text     =  "Dasas";
		this.tabDasa.Click    += new System.EventHandler(this.tabDasa_Click);
		// 
		// tabVargas
		// 
		this.tabVargas.Location =  new System.Drawing.Point(4, 25);
		this.tabVargas.Name     =  "tabVargas";
		this.tabVargas.Size     =  new System.Drawing.Size(464, 227);
		this.tabVargas.TabIndex =  2;
		this.tabVargas.Text     =  "Varga";
		this.tabVargas.Click    += new System.EventHandler(this.tabVargas_Click);
		// 
		// tabPanchanga
		// 
		this.tabPanchanga.Location =  new System.Drawing.Point(4, 25);
		this.tabPanchanga.Name     =  "tabPanchanga";
		this.tabPanchanga.Size     =  new System.Drawing.Size(464, 227);
		this.tabPanchanga.TabIndex =  6;
		this.tabPanchanga.Text     =  "Panchanga";
		this.tabPanchanga.Click    += new System.EventHandler(this.tabPanchanga_Click);
		// 
		// tabTithiPravesh
		// 
		this.tabTithiPravesh.Location = new System.Drawing.Point(4, 25);
		this.tabTithiPravesh.Name     = "tabTithiPravesh";
		this.tabTithiPravesh.Size     = new System.Drawing.Size(464, 227);
		this.tabTithiPravesh.TabIndex = 5;
		this.tabTithiPravesh.Text     = "TithiPravesh";
		// 
		// JhoraMainTab
		// 
		this.Controls.Add(this.mTab);
		this.Name = "JhoraMainTab";
		this.Size = new System.Drawing.Size(472, 256);
		this.mTab.ResumeLayout(false);
		this.ResumeLayout(false);
	}

#endregion


	private void tabBasics_Click(object sender, EventArgs e)
	{
	}

	private void tabTransits_Click(object sender, EventArgs e)
	{
	}

	private void tabVargas_Click(object sender, EventArgs e)
	{
	}

	private void mTab_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (mTab.SelectedTab == tabTransits && bTabTransitsLoaded == false)
		{
			AddControlToTab(tabTransits, new TransitSearch(h));
			bTabTransitsLoaded = true;
			return;
		}

		if (mTab.SelectedTab == tabDasa && bTabDasaLoaded == false)
		{
			var mc = new MhoraControl();
			AddControlToTab(tabDasa, mc);

			//MhoraControlContainer mcc = new MhoraControlContainer(mc);
			mc.ControlHoroscope = h;
			switch (h.info.type)
			{
				case HoraInfo.Name.TithiPravesh:
					mc.ViewControl(MhoraControlContainer.BaseUserOptions.ViewType.DasaTithiPraveshAshtottariCompressedTithi);
					break;
				default:
					mc.ViewControl(MhoraControlContainer.BaseUserOptions.ViewType.DasaVimsottari);
					break;
			}

			bTabDasaLoaded = true;
			return;
		}

		if (mTab.SelectedTab == tabVargas && bTabVargasLoaded == false)
		{
			AddControlToTab(tabVargas, new DivisionalChart(h));
			bTabVargasLoaded = true;
		}

		if (mTab.SelectedTab == tabTransits && bTabTransitsLoaded == false)
		{
			AddControlToTab(tabTransits, new TransitSearch(h));
			bTabTransitsLoaded = true;
		}

		if (mTab.SelectedTab == tabTithiPravesh && bTabTithiPraveshLoaded == false)
		{
			var dc = new DasaControl(h, new TithiPraveshDasa(h));
			dc.LinkToHoroscope      = false;
			dc.DasaOptions.YearType = ToDate.DateType.TithiPraveshYear;
			dc.Reset();
			AddControlToTab(tabTithiPravesh, dc);
			bTabTithiPraveshLoaded = true;
		}

		//if (this.mTab.SelectedTab == tabTajaka && this.bTabTajakaLoaded == false)
		//{
		//	this.AddControlToTab(tabTajaka, new DasaControl(h, new TajakaDasa(h)));
		//	this.bTabTajakaLoaded = true;		
		//}
		if (mTab.SelectedTab == tabPanchanga && bTabPanchangaLoaded == false)
		{
			AddControlToTab(tabPanchanga, new PanchangaControl(h));
			bTabPanchangaLoaded = true;
		}
	}

	private void tabPanchanga_Click(object sender, EventArgs e)
	{
	}

	private void tabDasa_Click(object sender, EventArgs e)
	{
	}
}