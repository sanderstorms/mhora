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
using Mhora.Components.Varga;
using Mhora.Database.Settings;
using Mhora.Elements.Calculation;

namespace Mhora.Components.Jhora;

/// <summary>
///     Summary description for JhoraBasicsTab.
/// </summary>
public class JhoraBasicsTab : MhoraControl
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container _components = null;

	private bool _bTabAshtakavargaLoaded;

	//bool bTabKeyInfoLoaded = false;
	private bool _bTabCalculationsLoaded;
	private bool _bTabNavamsaChakraLoaded;
	private bool _bTabYogasLoaded;

	private TabPage    _tabAshtakavarga;
	private TabPage    _tabCalculations;
	private TabControl _tabControl1;
	private TabPage    _tabKeyInfo;
	private TabPage    _tabNavamsaChakra;
	private TabPage    _tabYogas;

	public JhoraBasicsTab(Horoscope h)
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		//
		// TODO: Add any constructor code after InitializeComponent call
		//
		base.h                                      =  h;
		MhoraGlobalOptions.DisplayPrefsChanged += OnRedisplay;
		OnRedisplay(MhoraGlobalOptions.Instance);
		AddControlToTab(_tabKeyInfo, new KeyInfoControl(base.h));
		//this.AddControlToTab (tabTest, new BalasControl(h));
		//this.AddControlToTab (tabTest, new Sarvatobhadra81Control(h));
		//this.AddControlToTab (tabTest, new KutaMatchingControl(h, h));
		//this.AddControlToTab (tabTest, new VaraChakra(h));

		_tabControl1.TabPages[0] = _tabKeyInfo;
		_tabControl1.TabPages[1] = _tabCalculations;
		_tabControl1.TabPages[2] = _tabNavamsaChakra;
		_tabControl1.TabPages[3] = _tabAshtakavarga;
		_tabControl1.TabPages[4] = _tabYogas;

		_tabControl1.SelectedTab = _tabKeyInfo;
		//this.tabControl1.SelectedTab = tabTest;
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
		this._tabControl1      = new System.Windows.Forms.TabControl();
		this._tabKeyInfo       = new System.Windows.Forms.TabPage();
		this._tabNavamsaChakra = new System.Windows.Forms.TabPage();
		this._tabCalculations  = new System.Windows.Forms.TabPage();
		this._tabAshtakavarga  = new System.Windows.Forms.TabPage();
		this._tabYogas         = new System.Windows.Forms.TabPage();
		this._tabControl1.SuspendLayout();
		this.SuspendLayout();
		// 
		// tabControl1
		// 
		this._tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this._tabControl1.Controls.Add(this._tabKeyInfo);
		this._tabControl1.Controls.Add(this._tabCalculations);
		this._tabControl1.Controls.Add(this._tabYogas);
		this._tabControl1.Controls.Add(this._tabNavamsaChakra);
		this._tabControl1.Controls.Add(this._tabAshtakavarga);
		this._tabControl1.Dock                 =  System.Windows.Forms.DockStyle.Fill;
		this._tabControl1.Font                 =  new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._tabControl1.Location             =  new System.Drawing.Point(0, 0);
		this._tabControl1.Name                 =  "_tabControl1";
		this._tabControl1.Padding              =  new System.Drawing.Point(15, 3);
		this._tabControl1.SelectedIndex        =  0;
		this._tabControl1.Size                 =  new System.Drawing.Size(292, 266);
		this._tabControl1.TabIndex             =  0;
		this._tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
		// 
		// tabKeyInfo
		// 
		this._tabKeyInfo.Location =  new System.Drawing.Point(4, 4);
		this._tabKeyInfo.Name     =  "_tabKeyInfo";
		this._tabKeyInfo.Size     =  new System.Drawing.Size(284, 238);
		this._tabKeyInfo.TabIndex =  0;
		this._tabKeyInfo.Text     =  "Key Info";
		this._tabKeyInfo.Click    += new System.EventHandler(this.tabKeyInfo_Click);
		// 
		// tabNavamsaChakra
		// 
		this._tabNavamsaChakra.Location =  new System.Drawing.Point(4, 4);
		this._tabNavamsaChakra.Name     =  "_tabNavamsaChakra";
		this._tabNavamsaChakra.Size     =  new System.Drawing.Size(284, 238);
		this._tabNavamsaChakra.TabIndex =  3;
		this._tabNavamsaChakra.Text     =  "Chakra";
		this._tabNavamsaChakra.Click    += new System.EventHandler(this.tabTest_Click);
		// 
		// tabCalculations
		// 
		this._tabCalculations.Location = new System.Drawing.Point(4, 4);
		this._tabCalculations.Name     = "_tabCalculations";
		this._tabCalculations.Size     = new System.Drawing.Size(284, 238);
		this._tabCalculations.TabIndex = 1;
		this._tabCalculations.Text     = "Calculations";
		// 
		// tabAshtakavarga
		// 
		this._tabAshtakavarga.Location = new System.Drawing.Point(4, 4);
		this._tabAshtakavarga.Name     = "_tabAshtakavarga";
		this._tabAshtakavarga.Size     = new System.Drawing.Size(284, 238);
		this._tabAshtakavarga.TabIndex = 2;
		this._tabAshtakavarga.Text     = "Ashtakavarga";
		// 
		// tabYogas
		// 
		this._tabYogas.Location = new System.Drawing.Point(4, 4);
		this._tabYogas.Name     = "_tabYogas";
		this._tabYogas.Size     = new System.Drawing.Size(284, 238);
		this._tabYogas.TabIndex = 4;
		this._tabYogas.Text     = "Yogas";
		// 
		// JhoraBasicsTab
		// 
		this.Controls.Add(this._tabControl1);
		this.Name = "JhoraBasicsTab";
		this.Size = new System.Drawing.Size(292, 266);
		this._tabControl1.ResumeLayout(false);
		this.ResumeLayout(false);
	}

#endregion

	private void tabKeyInfo_Click(object sender, EventArgs e)
	{
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		var tp = _tabControl1.SelectedTab;
		if (tp == _tabCalculations && _bTabCalculationsLoaded == false)
		{
			AddControlToTab(_tabCalculations, new BasicCalculationsControl(h));
			_bTabCalculationsLoaded = true;
		}

		if (tp == _tabAshtakavarga && _bTabAshtakavargaLoaded == false)
		{
			AddControlToTab(_tabAshtakavarga, new AshtakavargaControl(h));
			_bTabAshtakavargaLoaded = true;
		}

		if (tp == _tabNavamsaChakra && _bTabNavamsaChakraLoaded == false)
		{
			AddControlToTab(_tabNavamsaChakra, new NavamsaControl(h));
			_bTabNavamsaChakraLoaded = true;
		}

		if (tp == _tabYogas && _bTabYogasLoaded == false)
		{
			//this.AddControlToTab(tabYogas, new YogaControl(h));
			_bTabYogasLoaded = true;
		}
	}

	private void tabTest_Click(object sender, EventArgs e)
	{
	}
}