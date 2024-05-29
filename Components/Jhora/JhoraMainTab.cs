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
using Mhora.Components.DasaControl;
using Mhora.Components.PanchangaControl;
using Mhora.Components.VargaControl;
using Mhora.Dasas.YearlyDasa;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using TransitSearch = Mhora.Components.TransitControl.TransitSearch;

namespace Mhora.Components.Jhora;

/// <summary>
///     Summary description for JhoraMainTab.
/// </summary>
public class JhoraMainTab : MhoraControl
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container _components = null;

	private bool _bTabDasaLoaded;

	private bool _bTabPanchangaLoaded;

	//bool bTabTajakaLoaded = false;
	private bool _bTabTithiPraveshLoaded;

	private bool _bTabTransitsLoaded;

	//bool bTabBasicsLoaded = false;
	private bool _bTabVargasLoaded;

	private ContextMenu _contextMenu1;
	private TabControl  _mTab;
	private TabPage     _tabBasics;
	private TabPage     _tabDasa;
	private TabPage     _tabPanchanga;

	private TabPage _tabTithiPravesh;
	private TabPage _tabTransits;
	private TabPage _tabVargas;

	public JhoraMainTab(Horoscope h)
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		_mTab.TabPages[0] = _tabBasics;
		_mTab.TabPages[1] = _tabVargas;
		_mTab.TabPages[2] = _tabDasa;
		_mTab.TabPages[3] = _tabTransits;
		_mTab.TabPages[4] = _tabPanchanga;
		_mTab.TabPages[5] = _tabTithiPravesh;

		_mTab.SelectedTab = _tabBasics;

		//
		// TODO: Add any constructor code after InitializeComponent call
		//
		this.h                                      =  h;
		MhoraGlobalOptions.DisplayPrefsChanged += OnRedisplay;
		OnRedisplay(MhoraGlobalOptions.Instance);

		AddControlToTab(_tabBasics, new JhoraBasicsTab(this.h));
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
			_components?.Dispose();
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
		this._mTab            = new System.Windows.Forms.TabControl();
		this._tabBasics       = new System.Windows.Forms.TabPage();
		this._tabTransits     = new System.Windows.Forms.TabPage();
		this._tabDasa         = new System.Windows.Forms.TabPage();
		this._tabVargas       = new System.Windows.Forms.TabPage();
		this._tabPanchanga    = new System.Windows.Forms.TabPage();
		this._tabTithiPravesh = new System.Windows.Forms.TabPage();
		this._contextMenu1    = new System.Windows.Forms.ContextMenu();
		this._mTab.SuspendLayout();
		this.SuspendLayout();
		// 
		// mTab
		// 
		this._mTab.Controls.Add(this._tabBasics);
		this._mTab.Controls.Add(this._tabDasa);
		this._mTab.Controls.Add(this._tabTransits);
		this._mTab.Controls.Add(this._tabVargas);
		this._mTab.Controls.Add(this._tabPanchanga);
		this._mTab.Controls.Add(this._tabTithiPravesh);
		this._mTab.Dock                 =  System.Windows.Forms.DockStyle.Fill;
		this._mTab.Font                 =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._mTab.Location             =  new System.Drawing.Point(0, 0);
		this._mTab.Name                 =  "_mTab";
		this._mTab.Padding              =  new System.Drawing.Point(20, 4);
		this._mTab.SelectedIndex        =  0;
		this._mTab.Size                 =  new System.Drawing.Size(472, 256);
		this._mTab.TabIndex             =  0;
		this._mTab.SelectedIndexChanged += new System.EventHandler(this.mTab_SelectedIndexChanged);
		// 
		// tabBasics
		// 
		this._tabBasics.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
		this._tabBasics.Location =  new System.Drawing.Point(4, 25);
		this._tabBasics.Name     =  "_tabBasics";
		this._tabBasics.Size     =  new System.Drawing.Size(464, 227);
		this._tabBasics.TabIndex =  1;
		this._tabBasics.Text     =  "Basics";
		this._tabBasics.Click    += new System.EventHandler(this.tabBasics_Click);
		// 
		// tabTransits
		// 
		this._tabTransits.Location =  new System.Drawing.Point(4, 25);
		this._tabTransits.Name     =  "_tabTransits";
		this._tabTransits.Size     =  new System.Drawing.Size(464, 227);
		this._tabTransits.TabIndex =  4;
		this._tabTransits.Text     =  "Transits";
		this._tabTransits.Click    += new System.EventHandler(this.tabTransits_Click);
		// 
		// tabDasa
		// 
		this._tabDasa.Location =  new System.Drawing.Point(4, 25);
		this._tabDasa.Name     =  "_tabDasa";
		this._tabDasa.Size     =  new System.Drawing.Size(464, 227);
		this._tabDasa.TabIndex =  0;
		this._tabDasa.Text     =  "Dasas";
		this._tabDasa.Click    += new System.EventHandler(this.tabDasa_Click);
		// 
		// tabVargas
		// 
		this._tabVargas.Location =  new System.Drawing.Point(4, 25);
		this._tabVargas.Name     =  "_tabVargas";
		this._tabVargas.Size     =  new System.Drawing.Size(464, 227);
		this._tabVargas.TabIndex =  2;
		this._tabVargas.Text     =  "Vargas";
		this._tabVargas.Click    += new System.EventHandler(this.tabVargas_Click);
		// 
		// tabPanchanga
		// 
		this._tabPanchanga.Location =  new System.Drawing.Point(4, 25);
		this._tabPanchanga.Name     =  "_tabPanchanga";
		this._tabPanchanga.Size     =  new System.Drawing.Size(464, 227);
		this._tabPanchanga.TabIndex =  6;
		this._tabPanchanga.Text     =  "Panchanga";
		this._tabPanchanga.Click    += new System.EventHandler(this.tabPanchanga_Click);
		// 
		// tabTithiPravesh
		// 
		this._tabTithiPravesh.Location = new System.Drawing.Point(4, 25);
		this._tabTithiPravesh.Name     = "_tabTithiPravesh";
		this._tabTithiPravesh.Size     = new System.Drawing.Size(464, 227);
		this._tabTithiPravesh.TabIndex = 5;
		this._tabTithiPravesh.Text     = "TithiPravesh";
		// 
		// JhoraMainTab
		// 
		this.Controls.Add(this._mTab);
		this.Name = "JhoraMainTab";
		this.Size = new System.Drawing.Size(472, 256);
		this._mTab.ResumeLayout(false);
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
		if (_mTab.SelectedTab == _tabTransits && _bTabTransitsLoaded == false)
		{
			AddControlToTab(_tabTransits, new TransitSearch(h));
			_bTabTransitsLoaded = true;
			return;
		}

		if (_mTab.SelectedTab == _tabDasa && _bTabDasaLoaded == false)
		{
			var mc = new MhoraControl();
			AddControlToTab(_tabDasa, mc);

			//MhoraControlContainer mcc = new MhoraControlContainer(mc);
			mc.ControlHoroscope = h;
			switch (h.Info.Type)
			{
				case HoraInfo.ChartType.TithiPravesh:
					mc.ViewControl(MhoraViewType.DasaTithiPraveshAshtottariCompressedTithi);
					break;
				default:
					mc.ViewControl(MhoraViewType.DasaVimsottari);
					break;
			}

			_bTabDasaLoaded = true;
			return;
		}

		if (_mTab.SelectedTab == _tabVargas && _bTabVargasLoaded == false)
		{
			AddControlToTab(_tabVargas, new DivisionalChart(h));
			_bTabVargasLoaded = true;
		}

		if (_mTab.SelectedTab == _tabTransits && _bTabTransitsLoaded == false)
		{
			AddControlToTab(_tabTransits, new TransitSearch(h));
			_bTabTransitsLoaded = true;
		}

		if (_mTab.SelectedTab == _tabTithiPravesh && _bTabTithiPraveshLoaded == false)
		{
			var dc = new MhoraDasaControl(h, new TithiPraveshDasa(h));
			dc.LinkToHoroscope      = false;
			dc.DasaOptions.YearType = DateType.TithiPraveshYear;
			dc.Reset();
			AddControlToTab(_tabTithiPravesh, dc);
			_bTabTithiPraveshLoaded = true;
		}

		//if (this.mTab.SelectedTab == tabTajaka && this.bTabTajakaLoaded == false)
		//{
		//	this.AddControlToTab(tabTajaka, new MhoraDasaControl(h, new TajakaDasa(h)));
		//	this.bTabTajakaLoaded = true;		
		//}
		if (_mTab.SelectedTab == _tabPanchanga && _bTabPanchangaLoaded == false)
		{
			AddControlToTab(_tabPanchanga, new MhoraPanchangaControl(h));
			_bTabPanchangaLoaded = true;
		}
	}

	private void tabPanchanga_Click(object sender, EventArgs e)
	{
	}

	private void tabDasa_Click(object sender, EventArgs e)
	{
	}
}