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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Mhora.Components.Varga;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Elements.Dasas;
using Mhora.Util;

namespace Mhora.Components.Dasa;

/// <summary>
///     Summary description for DasaControl.
/// </summary>
public class DasaControl : MhoraControl //System.Windows.Forms.UserControl
{
	private static readonly ToolTip TooltipEvent = new();

	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container _components = null;

	private readonly IDasa  _id;
	private          Button _bDasaOptions;
	private          Button _bDateOptions;
	private          Button _bGrahaStrengths;
	private          Button _bNextCycle;
	private          Button _bPrevCycle;
	private          Button _bRasiStrengths;

	private ColumnHeader _dasa;
	private ContextMenu  _dasaContextMenu;
	private Label        _dasaInfo;
	private ListView     _dasaItemList;
	private MenuItem     _m3Parts;
	private MenuItem     _mCompressedTithiPraveshaFixed;
	private MenuItem     _mCompressedYogaPraveshaYoga;
	private MenuItem     _mCompressLunar;
	private MenuItem     _mCompressSolar;
	private MenuItem     _mCompressTithiPraveshaSolar;
	private MenuItem     _mCompressTithiPraveshaTithi;
	private MenuItem     _mCompressYoga;
	private MenuItem     _mCustomYears;
	private MenuItem     _mDateOptions;
	private MenuItem     _mEditDasas;
	private MenuItem     _mEntryChart;
	private MenuItem     _mEntryChartCompressed;
	private MenuItem     _mEntryDate;
	private MenuItem     _mEntrySunriseChart;
	private MenuItem     _menuItem1;
	private MenuItem     _menuItem2;
	private MenuItem     _menuItem3;
	private MenuItem     _menuItem4;
	private MenuItem     _menuItem5;
	private MenuItem     _menuItem6;

	private EventUserOptions _mEventOptionsCache;
	private MenuItem         _mFixedYears360;
	private MenuItem         _mFixedYears365;

	private int _minCycle, _maxCycle;

	private MenuItem     _mLocateEvent;
	private MenuItem     _mNextCycle;
	private MenuItem     _mNormalize;
	private MenuItem     _mOptions;
	private MenuItem     _mPreviousCycle;
	private MenuItem     _mReset;
	private MenuItem     _mResetParamAyus;
	private MenuItem     _mShowEvents;
	private MenuItem     _mSolarYears;
	private MenuItem     _mTithiYears;
	private MenuItem     _mTriBhagi40;
	private MenuItem     _mTribhagi80;
	private ColumnHeader _startDate;
	private ToDate       _td;


	public DasaControl(Horoscope horoscope, IDasa id)
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitForm call
		h           = horoscope;
		_id    = id;
		DasaOptions = new Elements.Dasas.Dasa.DasaOptions();

		if (h.Info.DefaultYearCompression != 0)
		{
			DasaOptions.Compression = h.Info.DefaultYearCompression;
			DasaOptions.YearLength  = h.Info.DefaultYearLength;
			DasaOptions.YearType    = h.Info.DefaultYearType;
		}


		SetDasaYearType();
		//td = new ToDate (horoscope.info.Jd, mDasaOptions.YearLength, 0.0, horoscope);
		_mShowEvents.Checked = MhoraGlobalOptions.Instance.DasaShowEvents;
		ResetDisplayOptions(MhoraGlobalOptions.Instance);

		var d = (Elements.Dasas.Dasa) _id;
		d.RecalculateEvent                     += RecalculateEntries;
		MhoraGlobalOptions.DisplayPrefsChanged += ResetDisplayOptions;
		h.Changed                         += OnRecalculate;
		SetDescriptionLabel();
		d.Changed += OnDasaChanged;
		if (_dasaItemList.Items.Count >= 1)
		{
			_dasaItemList.Items[0].Selected = true;
		}

		VScroll = true;
		Reset();

		//this.LocateChartEvents();
	}

	public Elements.Dasas.Dasa.DasaOptions DasaOptions
	{
		get;
	}

	public object DasaSpecificOptions
	{
		get => _id.GetOptions();
		set => _id.SetOptions(value);
	}


	public bool LinkToHoroscope
	{
		set
		{
			if (value)
			{
				h.Changed                          += OnRecalculate;
				((Elements.Dasas.Dasa) _id).Changed += OnDasaChanged;
			}
			else
			{
				h.Changed                          -= OnRecalculate;
				((Elements.Dasas.Dasa) _id).Changed += OnDasaChanged;
			}
		}
	}

	private void SetDescriptionLabel()
	{
		_dasaInfo.Text = _id.Description();

		_dasaInfo.Text += " (";

		if (DasaOptions.Compression > 0)
		{
			_dasaInfo.Text += DasaOptions.Compression.ToString();
		}


		_dasaInfo.Text = string.Format("{0} {1:0.00} {2}", _dasaInfo.Text, DasaOptions.YearLength, DasaOptions.YearType);

		_dasaInfo.Text += " )";
	}

	public void ResetDisplayOptions(object o)
	{
		_dasaItemList.BackColor = MhoraGlobalOptions.Instance.DasaBackgroundColor;
		_dasaItemList.Font      = MhoraGlobalOptions.Instance.GeneralFont;
		foreach (ListViewItem li in _dasaItemList.Items)
		{
			var di = (DasaItem) li;
			li.BackColor = MhoraGlobalOptions.Instance.DasaBackgroundColor;
			li.Font      = MhoraGlobalOptions.Instance.GeneralFont;
			foreach (ListViewItem.ListViewSubItem si in li.SubItems)
			{
				si.BackColor = MhoraGlobalOptions.Instance.DasaBackgroundColor;
			}

			di.EventDesc = string.Empty;
			if (li.SubItems.Count >= 2)
			{
				li.SubItems[0].ForeColor = MhoraGlobalOptions.Instance.DasaPeriodColor;
				li.SubItems[1].ForeColor = MhoraGlobalOptions.Instance.DasaDateColor;
				li.SubItems[1].Font      = MhoraGlobalOptions.Instance.FixedWidthFont;
			}
		}

		_dasaItemList.HoverSelection = MhoraGlobalOptions.Instance.DasaHoverSelect;
		LocateChartEvents();
	}

	public void Reset()
	{
		_id.RecalculateOptions();
		SetDescriptionLabel();
		_dasaItemList.Items.Clear();
		SetDasaYearType();
		_minCycle = _maxCycle = 0;
		var compress          = DasaOptions.Compression == 0.0 ? 0.0 : DasaOptions.Compression / _id.ParamAyus();

		var a = _id.Dasa(0);
		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.PopulateListViewItemMembers(_td, _id);
			_dasaItemList.Items.Add(di);
		}

		LocateChartEvents();
	}

	public void OnRecalculate(object o)
	{
		Reset();
	}

	public void OnDasaChanged(object o)
	{
		//Reset();
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

#region Component Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this._dasaItemList                  = new System.Windows.Forms.ListView();
		this._dasa                          = new System.Windows.Forms.ColumnHeader();
		this._startDate                     = new System.Windows.Forms.ColumnHeader();
		this._dasaContextMenu               = new System.Windows.Forms.ContextMenu();
		this._mEntryChart                   = new System.Windows.Forms.MenuItem();
		this._mEntrySunriseChart            = new System.Windows.Forms.MenuItem();
		this._mEntryDate                    = new System.Windows.Forms.MenuItem();
		this._mLocateEvent                  = new System.Windows.Forms.MenuItem();
		this._mReset                        = new System.Windows.Forms.MenuItem();
		this._m3Parts                       = new System.Windows.Forms.MenuItem();
		this._mShowEvents                   = new System.Windows.Forms.MenuItem();
		this._mOptions                      = new System.Windows.Forms.MenuItem();
		this._mDateOptions                  = new System.Windows.Forms.MenuItem();
		this._mPreviousCycle                = new System.Windows.Forms.MenuItem();
		this._mNextCycle                    = new System.Windows.Forms.MenuItem();
		this._menuItem3                     = new System.Windows.Forms.MenuItem();
		this._mSolarYears                   = new System.Windows.Forms.MenuItem();
		this._mTithiYears                   = new System.Windows.Forms.MenuItem();
		this._mFixedYears360                = new System.Windows.Forms.MenuItem();
		this._mFixedYears365                = new System.Windows.Forms.MenuItem();
		this._mCustomYears                  = new System.Windows.Forms.MenuItem();
		this._menuItem5                     = new System.Windows.Forms.MenuItem();
		this._mTribhagi80                   = new System.Windows.Forms.MenuItem();
		this._mTriBhagi40                   = new System.Windows.Forms.MenuItem();
		this._mResetParamAyus               = new System.Windows.Forms.MenuItem();
		this._menuItem6                     = new System.Windows.Forms.MenuItem();
		this._mCompressSolar                = new System.Windows.Forms.MenuItem();
		this._mCompressLunar                = new System.Windows.Forms.MenuItem();
		this._mCompressYoga                 = new System.Windows.Forms.MenuItem();
		this._mCompressTithiPraveshaTithi   = new System.Windows.Forms.MenuItem();
		this._mCompressTithiPraveshaSolar   = new System.Windows.Forms.MenuItem();
		this._mCompressedTithiPraveshaFixed = new System.Windows.Forms.MenuItem();
		this._mCompressedYogaPraveshaYoga   = new System.Windows.Forms.MenuItem();
		this._menuItem4                     = new System.Windows.Forms.MenuItem();
		this._mEditDasas                    = new System.Windows.Forms.MenuItem();
		this._mNormalize                    = new System.Windows.Forms.MenuItem();
		this._menuItem1                     = new System.Windows.Forms.MenuItem();
		this._menuItem2                     = new System.Windows.Forms.MenuItem();
		this._dasaInfo                      = new System.Windows.Forms.Label();
		this._bPrevCycle                    = new System.Windows.Forms.Button();
		this._bNextCycle                    = new System.Windows.Forms.Button();
		this._bDasaOptions                  = new System.Windows.Forms.Button();
		this._bDateOptions                  = new System.Windows.Forms.Button();
		this._bRasiStrengths                = new System.Windows.Forms.Button();
		this._bGrahaStrengths               = new System.Windows.Forms.Button();
		this._mEntryChartCompressed         = new System.Windows.Forms.MenuItem();
		this.SuspendLayout();
		// 
		// dasaItemList
		// 
		this._dasaItemList.AllowColumnReorder = true;
		this._dasaItemList.AllowDrop          = true;
		this._dasaItemList.Anchor             = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this._dasaItemList.BackColor          = System.Drawing.Color.Lavender;
		this._dasaItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this._dasa,
			this._startDate
		});
		this._dasaItemList.ContextMenu          =  this._dasaContextMenu;
		this._dasaItemList.Font                 =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._dasaItemList.ForeColor            =  System.Drawing.Color.Black;
		this._dasaItemList.FullRowSelect        =  true;
		this._dasaItemList.HideSelection        =  false;
		this._dasaItemList.HoverSelection       =  true;
		this._dasaItemList.Location             =  new System.Drawing.Point(8, 40);
		this._dasaItemList.MultiSelect          =  false;
		this._dasaItemList.Name                 =  "_dasaItemList";
		this._dasaItemList.Size                 =  new System.Drawing.Size(424, 264);
		this._dasaItemList.TabIndex             =  0;
		this._dasaItemList.View                 =  System.Windows.Forms.View.Details;
		this._dasaItemList.MouseDown            += new System.Windows.Forms.MouseEventHandler(this.dasaItemList_MouseDown);
		this._dasaItemList.Click                += new System.EventHandler(this.dasaItemList_Click);
		this._dasaItemList.MouseUp              += new System.Windows.Forms.MouseEventHandler(this.dasaItemList_MouseUp);
		this._dasaItemList.DragDrop             += new System.Windows.Forms.DragEventHandler(this.dasaItemList_DragDrop);
		this._dasaItemList.MouseEnter           += new System.EventHandler(this.dasaItemList_MouseEnter);
		this._dasaItemList.DragEnter            += new System.Windows.Forms.DragEventHandler(this.dasaItemList_DragEnter);
		this._dasaItemList.MouseMove            += new System.Windows.Forms.MouseEventHandler(this.dasaItemList_MouseMove);
		this._dasaItemList.SelectedIndexChanged += new System.EventHandler(this.dasaItemList_SelectedIndexChanged);
		// 
		// Dasa
		// 
		this._dasa.Text  = "Dasa";
		this._dasa.Width = 150;
		// 
		// StartDate
		// 
		this._startDate.Text  = "Dates";
		this._startDate.Width = 500;
		// 
		// dasaContextMenu
		// 
		this._dasaContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this._mEntryChart,
			this._mEntryChartCompressed,
			this._mEntrySunriseChart,
			this._mEntryDate,
			this._mLocateEvent,
			this._mReset,
			this._m3Parts,
			this._mShowEvents,
			this._mOptions,
			this._mDateOptions,
			this._mPreviousCycle,
			this._mNextCycle,
			this._menuItem3,
			this._menuItem4,
			this._menuItem1,
			this._menuItem2
		});
		this._dasaContextMenu.Popup += new System.EventHandler(this.dasaContextMenu_Popup);
		// 
		// mEntryChart
		// 
		this._mEntryChart.Index =  0;
		this._mEntryChart.Text  =  "&Entry Chart";
		this._mEntryChart.Click += new System.EventHandler(this.mEntryChart_Click);
		// 
		// mEntrySunriseChart
		// 
		this._mEntrySunriseChart.Index =  2;
		this._mEntrySunriseChart.Text  =  "Entry &Sunrise Chart";
		this._mEntrySunriseChart.Click += new System.EventHandler(this.mEntrySunriseChart_Click);
		// 
		// mEntryDate
		// 
		this._mEntryDate.Index =  3;
		this._mEntryDate.Text  =  "Copy Entry Date";
		this._mEntryDate.Click += new System.EventHandler(this.mEntryDate_Click);
		// 
		// mLocateEvent
		// 
		this._mLocateEvent.Index =  4;
		this._mLocateEvent.Text  =  "Locate An Event";
		this._mLocateEvent.Click += new System.EventHandler(this.mLocateEvent_Click);
		// 
		// mReset
		// 
		this._mReset.Index =  5;
		this._mReset.Text  =  "&Reset";
		this._mReset.Click += new System.EventHandler(this.mReset_Click);
		// 
		// m3Parts
		// 
		this._m3Parts.Index =  6;
		this._m3Parts.Text  =  "3 Parts";
		this._m3Parts.Click += new System.EventHandler(this.m3Parts_Click);
		// 
		// mShowEvents
		// 
		this._mShowEvents.Checked =  true;
		this._mShowEvents.Index   =  7;
		this._mShowEvents.Text    =  "Show Events";
		this._mShowEvents.Click   += new System.EventHandler(this.mShowEvents_Click);
		// 
		// mOptions
		// 
		this._mOptions.Index   =  8;
		this._mOptions.Text    =  "Dasa &Options";
		this._mOptions.Visible =  false;
		this._mOptions.Click   += new System.EventHandler(this.mOptions_Click);
		// 
		// mDateOptions
		// 
		this._mDateOptions.Index   =  9;
		this._mDateOptions.Text    =  "&Date Options";
		this._mDateOptions.Visible =  false;
		this._mDateOptions.Click   += new System.EventHandler(this.mDateOptions_Click);
		// 
		// mPreviousCycle
		// 
		this._mPreviousCycle.Index   =  10;
		this._mPreviousCycle.Text    =  "&Previous Cycle";
		this._mPreviousCycle.Visible =  false;
		this._mPreviousCycle.Click   += new System.EventHandler(this.mPreviousCycle_Click);
		// 
		// mNextCycle
		// 
		this._mNextCycle.Index   =  11;
		this._mNextCycle.Text    =  "&Next Cycle";
		this._mNextCycle.Visible =  false;
		this._mNextCycle.Click   += new System.EventHandler(this.mNextCycle_Click);
		// 
		// menuItem3
		// 
		this._menuItem3.Index = 12;
		this._menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this._mSolarYears,
			this._mTithiYears,
			this._mFixedYears360,
			this._mFixedYears365,
			this._mCustomYears,
			this._menuItem5,
			this._mTribhagi80,
			this._mTriBhagi40,
			this._mResetParamAyus,
			this._menuItem6,
			this._mCompressSolar,
			this._mCompressLunar,
			this._mCompressYoga,
			this._mCompressTithiPraveshaTithi,
			this._mCompressTithiPraveshaSolar,
			this._mCompressedTithiPraveshaFixed,
			this._mCompressedYogaPraveshaYoga
		});
		this._menuItem3.Text = "Year Options";
		// 
		// mSolarYears
		// 
		this._mSolarYears.Index =  0;
		this._mSolarYears.Text  =  "&Solar Years (360 degrees)";
		this._mSolarYears.Click += new System.EventHandler(this.mSolarYears_Click);
		// 
		// mTithiYears
		// 
		this._mTithiYears.Index =  1;
		this._mTithiYears.Text  =  "&Tithis Years (360 tithis)";
		this._mTithiYears.Click += new System.EventHandler(this.mTithiYears_Click);
		// 
		// mFixedYears360
		// 
		this._mFixedYears360.Index =  2;
		this._mFixedYears360.Text  =  "Savana Years (360 days)";
		this._mFixedYears360.Click += new System.EventHandler(this.mFixedYears360_Click);
		// 
		// mFixedYears365
		// 
		this._mFixedYears365.Index =  3;
		this._mFixedYears365.Text  =  "~ Solar Year (365.2425 days)";
		this._mFixedYears365.Click += new System.EventHandler(this.mFixedYears365_Click);
		// 
		// mCustomYears
		// 
		this._mCustomYears.Index =  4;
		this._mCustomYears.Text  =  "&Custom Years";
		this._mCustomYears.Click += new System.EventHandler(this.mCustomYears_Click);
		// 
		// menuItem5
		// 
		this._menuItem5.Index = 5;
		this._menuItem5.Text  = "-";
		// 
		// mTribhagi80
		// 
		this._mTribhagi80.Index =  6;
		this._mTribhagi80.Text  =  "Tribhagi ParamAyus (80 Years)";
		this._mTribhagi80.Click += new System.EventHandler(this.mTribhagi80_Click);
		// 
		// mTriBhagi40
		// 
		this._mTriBhagi40.Index =  7;
		this._mTriBhagi40.Text  =  "Tribhagi ParamAyus (40 Years)";
		this._mTriBhagi40.Click += new System.EventHandler(this.mTriBhagi40_Click);
		// 
		// mResetParamAyus
		// 
		this._mResetParamAyus.Index =  8;
		this._mResetParamAyus.Text  =  "Regular ParamAyus";
		this._mResetParamAyus.Click += new System.EventHandler(this.mResetParamAyus_Click);
		// 
		// menuItem6
		// 
		this._menuItem6.Index = 9;
		this._menuItem6.Text  = "-";
		// 
		// mCompressSolar
		// 
		this._mCompressSolar.Index =  10;
		this._mCompressSolar.Text  =  "Compress to Solar Year";
		this._mCompressSolar.Click += new System.EventHandler(this.mCompressSolar_Click);
		// 
		// mCompressLunar
		// 
		this._mCompressLunar.Index =  11;
		this._mCompressLunar.Text  =  "Compress to Tithis Year";
		this._mCompressLunar.Click += new System.EventHandler(this.mCompressLunar_Click);
		// 
		// mCompressYoga
		// 
		this._mCompressYoga.Index =  12;
		this._mCompressYoga.Text  =  "Compress to Yoga Year";
		this._mCompressYoga.Click += new System.EventHandler(this.mCompressYoga_Click);
		// 
		// mCompressTithiPraveshaTithi
		// 
		this._mCompressTithiPraveshaTithi.Index =  13;
		this._mCompressTithiPraveshaTithi.Text  =  "Compress to Tithis Pravesha Year (Tithis)";
		this._mCompressTithiPraveshaTithi.Click += new System.EventHandler(this.mCompressTithiPraveshaTithi_Click);
		// 
		// mCompressTithiPraveshaSolar
		// 
		this._mCompressTithiPraveshaSolar.Index =  14;
		this._mCompressTithiPraveshaSolar.Text  =  "Compress to Tithis Pravesha Year (Solar)";
		this._mCompressTithiPraveshaSolar.Click += new System.EventHandler(this.mCompressTithiPraveshaSolar_Click);
		// 
		// mCompressedTithiPraveshaFixed
		// 
		this._mCompressedTithiPraveshaFixed.Index =  15;
		this._mCompressedTithiPraveshaFixed.Text  =  "Compress to Tithis Pravesha Year (Fixed)";
		this._mCompressedTithiPraveshaFixed.Click += new System.EventHandler(this.mCompressedTithiPraveshaFixed_Click);
		// 
		// mCompressedYogaPraveshaYoga
		// 
		this._mCompressedYogaPraveshaYoga.Index =  16;
		this._mCompressedYogaPraveshaYoga.Text  =  "Compress to Yoga Pravesha Year (Yoga)";
		this._mCompressedYogaPraveshaYoga.Click += new System.EventHandler(this.mCompressedYogaPraveshaYoga_Click);
		// 
		// menuItem4
		// 
		this._menuItem4.Index = 13;
		this._menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this._mEditDasas,
			this._mNormalize
		});
		this._menuItem4.Text = "Advanced";
		// 
		// mEditDasas
		// 
		this._mEditDasas.Index =  0;
		this._mEditDasas.Text  =  "Edit Dasas";
		this._mEditDasas.Click += new System.EventHandler(this.mEditDasas_Click);
		// 
		// mNormalize
		// 
		this._mNormalize.Index =  1;
		this._mNormalize.Text  =  "Normalize Dates";
		this._mNormalize.Click += new System.EventHandler(this.menuItem5_Click);
		// 
		// menuItem1
		// 
		this._menuItem1.Index = 14;
		this._menuItem1.Text  = "-";
		// 
		// menuItem2
		// 
		this._menuItem2.Index = 15;
		this._menuItem2.Text  = "-";
		// 
		// dasaInfo
		// 
		this._dasaInfo.Anchor    =  ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this._dasaInfo.Location  =  new System.Drawing.Point(184, 8);
		this._dasaInfo.Name      =  "_dasaInfo";
		this._dasaInfo.Size      =  new System.Drawing.Size(232, 23);
		this._dasaInfo.TabIndex  =  1;
		this._dasaInfo.TextAlign =  System.Drawing.ContentAlignment.MiddleLeft;
		this._dasaInfo.Click     += new System.EventHandler(this.dasaInfo_Click);
		// 
		// bPrevCycle
		// 
		this._bPrevCycle.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._bPrevCycle.Location =  new System.Drawing.Point(8, 8);
		this._bPrevCycle.Name     =  "_bPrevCycle";
		this._bPrevCycle.Size     =  new System.Drawing.Size(24, 23);
		this._bPrevCycle.TabIndex =  2;
		this._bPrevCycle.Text     =  "<";
		this._bPrevCycle.Click    += new System.EventHandler(this.bPrevCycle_Click);
		// 
		// bNextCycle
		// 
		this._bNextCycle.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._bNextCycle.Location =  new System.Drawing.Point(32, 8);
		this._bNextCycle.Name     =  "_bNextCycle";
		this._bNextCycle.Size     =  new System.Drawing.Size(24, 23);
		this._bNextCycle.TabIndex =  3;
		this._bNextCycle.Text     =  ">";
		this._bNextCycle.Click    += new System.EventHandler(this.bNextCycle_Click);
		// 
		// bDasaOptions
		// 
		this._bDasaOptions.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._bDasaOptions.Location =  new System.Drawing.Point(64, 8);
		this._bDasaOptions.Name     =  "_bDasaOptions";
		this._bDasaOptions.Size     =  new System.Drawing.Size(40, 23);
		this._bDasaOptions.TabIndex =  4;
		this._bDasaOptions.Text     =  "Opts";
		this._bDasaOptions.Click    += new System.EventHandler(this.bDasaOptions_Click);
		// 
		// bDateOptions
		// 
		this._bDateOptions.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._bDateOptions.Location =  new System.Drawing.Point(104, 8);
		this._bDateOptions.Name     =  "_bDateOptions";
		this._bDateOptions.Size     =  new System.Drawing.Size(24, 23);
		this._bDateOptions.TabIndex =  5;
		this._bDateOptions.Text     =  "Yr";
		this._bDateOptions.Click    += new System.EventHandler(this.bDateOptions_Click);
		// 
		// bRasiStrengths
		// 
		this._bRasiStrengths.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._bRasiStrengths.Location =  new System.Drawing.Point(128, 8);
		this._bRasiStrengths.Name     =  "_bRasiStrengths";
		this._bRasiStrengths.Size     =  new System.Drawing.Size(24, 23);
		this._bRasiStrengths.TabIndex =  6;
		this._bRasiStrengths.Text     =  "R";
		this._bRasiStrengths.Click    += new System.EventHandler(this.bRasiStrengths_Click);
		// 
		// bGrahaStrengths
		// 
		this._bGrahaStrengths.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this._bGrahaStrengths.Location =  new System.Drawing.Point(152, 8);
		this._bGrahaStrengths.Name     =  "_bGrahaStrengths";
		this._bGrahaStrengths.Size     =  new System.Drawing.Size(24, 23);
		this._bGrahaStrengths.TabIndex =  7;
		this._bGrahaStrengths.Text     =  "G";
		this._bGrahaStrengths.Click    += new System.EventHandler(this.bGrahaStrengths_Click);
		// 
		// mEntryChartCompressed
		// 
		this._mEntryChartCompressed.Index =  1;
		this._mEntryChartCompressed.Text  =  "Entry Chart (&Compressed)";
		this._mEntryChartCompressed.Click += new System.EventHandler(this.mEntryChartCompressed_Click);
		// 
		// DasaControl
		// 
		this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.Controls.Add(this._bGrahaStrengths);
		this.Controls.Add(this._bRasiStrengths);
		this.Controls.Add(this._bDateOptions);
		this.Controls.Add(this._bDasaOptions);
		this.Controls.Add(this._bNextCycle);
		this.Controls.Add(this._bPrevCycle);
		this.Controls.Add(this._dasaInfo);
		this.Controls.Add(this._dasaItemList);
		this.Name =  "DasaControl";
		this.Size =  new System.Drawing.Size(440, 312);
		this.Load += new System.EventHandler(this.DasaControl_Load);
		this.ResumeLayout(false);
	}

#endregion

	private void mEntryChart_Click(object sender, EventArgs e)
	{
		if (_dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) _dasaItemList.SelectedItems[0];

		var m = _td.AddYears(di.Entry.StartUt);
		h2.Info.DateOfBirth = m;

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry - (" + di.Entry.DasaName + ") " + _id.Description());
	}


	private void mEntryChartCompressed_Click(object sender, EventArgs e)
	{
		if (_dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) _dasaItemList.SelectedItems[0];

		var m    = _td.AddYears(di.Entry.StartUt);
		var mEnd = _td.AddYears(di.Entry.StartUt + di.Entry.DasaLength);

		var utDiff = mEnd.ToUniversalTime() - m.ToUniversalTime();
		h2.Info.DateOfBirth = m;

		h2.Info.DefaultYearCompression = 1;
		h2.Info.DefaultYearLength      = utDiff.TotalHours;
		h2.Info.DefaultYearType        = ToDate.DateType.FixedYear;

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry - (" + di.Entry.DasaName + ") " + _id.Description());
	}


	private void mEntrySunriseChart_Click(object sender, EventArgs e)
	{
		if (_dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) _dasaItemList.SelectedItems[0];

		var m = _td.AddYears(di.Entry.StartUt);
		h2.Info.DateOfBirth = m;

		h2.OnChanged();

		// if done once, get something usually 2+ minutes off. 
		// don't know why this is.
		var offsetSunrise = h2.HoursAfterSunrise() / 24.0;
		m                   = h2.Moment(h2.Info.Jd - offsetSunrise);
		h2.Info.DateOfBirth = m;
		h2.OnChanged();

		// so do it a second time, getting sunrise + 1 second.
		offsetSunrise       = h2.HoursAfterSunrise() / 24.0;
		m                   = h2.Moment(h2.Info.Jd - offsetSunrise + 1.0 / (24.0 * 60.0 * 60.0));
		h2.Info.DateOfBirth = m;
		h2.OnChanged();

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry Sunrise - (" + di.Entry.DasaName + ") " + _id.Description());
	}


	private void mEntryDate_Click(object sender, EventArgs e)
	{
		if (_dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var di = (DasaItem) _dasaItemList.SelectedItems[0];
		var m = _td.AddYears(di.Entry.StartUt);
		Clipboard.SetDataObject(m.ToString(), true);
	}

	private void SplitDasa()
	{
		if (_dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		SplitDasa((DasaItem) _dasaItemList.SelectedItems[0]);
	}

	private void SplitDasa(DasaItem di)
	{
		//Trace.Assert(dasaItemList.SelectedItems.Count >= 1, "dasaItemList::doubleclick");
		var index = di.Index + 1;

		var actionInserting = true;


		_dasaItemList.BeginUpdate();
		while (index < _dasaItemList.Items.Count)
		{
			var tdi = (DasaItem) _dasaItemList.Items[index];
			if (tdi.Entry.Level > di.Entry.Level)
			{
				actionInserting = false;
				_dasaItemList.Items.Remove(tdi);
			}
			else
			{
				break;
			}
		}

		if (actionInserting == false)
		{
			_dasaItemList.EndUpdate();
			return;
		}

		var a        = _id.AntarDasa(di.Entry);
		var compress = DasaOptions.Compression == 0.0 ? 0.0 : DasaOptions.Compression / _id.ParamAyus();

		foreach (DasaEntry de in a)
		{
			var pdi = new DasaItem(de);
			pdi.PopulateListViewItemMembers(_td, _id);
			_dasaItemList.Items.Insert(index, pdi);
			index = index + 1;
		}

		_dasaItemList.EndUpdate();
		//this.dasaItemList.Items[index-1].Selected = true;
	}

	protected override void copyToClipboard()
	{
		var iMaxDescLength = 0;
		for (var i = 0; i < _dasaItemList.Items.Count; i++)
		{
			iMaxDescLength = Math.Max(_dasaItemList.Items[i].Text.Length, iMaxDescLength);
		}

		iMaxDescLength += 2;

		var s = _dasaInfo.Text + "\r\n\r\n";
		for (var i = 0; i < _dasaItemList.Items.Count; i++)
		{
			var li         = _dasaItemList.Items[i];
			var di         = (DasaItem) li;
			var levelSpace = di.Entry.Level * 2;
			s += li.Text.PadRight(iMaxDescLength + levelSpace, ' ');

			for (var j = 1; j < li.SubItems.Count; j++)
			{
				s += "(" + li.SubItems[j].Text + ") ";
			}

			s += "\r\n";
		}

		Clipboard.SetDataObject(s);
	}

	private void RecalculateEntries()
	{
		SetDescriptionLabel();
		_dasaItemList.Items.Clear();
		var a = new ArrayList();
		for (var i = _minCycle; i <= _maxCycle; i++)
		{
			var b = _id.Dasa(i);
			a.AddRange(b);
		}

		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.PopulateListViewItemMembers(_td, _id);
			_dasaItemList.Items.Add(di);
		}

		LocateChartEvents();
	}

	private void mOptions_Click(object sender, EventArgs e)
	{
		//object wrapper = new GlobalizedPropertiesWrapper(id.GetOptions());
		var f = new MhoraOptions(_id.GetOptions(), _id.SetOptions);
		f.pGrid.ExpandAllGridItems();
		f.ShowDialog();
	}

	private void mPreviousCycle_Click(object sender, EventArgs e)
	{
		_minCycle--;
		var a = _id.Dasa(_minCycle);
		var i = 0;
		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.PopulateListViewItemMembers(_td, _id);
			_dasaItemList.Items.Insert(i, di);
			i++;
		}
	}

	private void mNextCycle_Click(object sender, EventArgs e)
	{
		_maxCycle++;
		var a = _id.Dasa(_maxCycle);
		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.PopulateListViewItemMembers(_td, _id);
			_dasaItemList.Items.Add(di);
		}
	}

	private void mReset_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void mVimsottari_Click(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaVimsottari);
	}

	private void mAshtottari_Click(object sender, EventArgs e)
	{
		((MhoraControlContainer) Parent).h = h;
		((MhoraControlContainer) Parent).SetView(MhoraControlContainer.BaseUserOptions.ViewType.DasaAshtottari);
	}


	private void DasaControl_Load(object sender, EventArgs e)
	{
		AddViewsToContextMenu(_dasaContextMenu);
	}

	private void SetDasaYearType()
	{
		var compress = DasaOptions.Compression == 0.0 ? 0.0 : DasaOptions.Compression / _id.ParamAyus();
		if (DasaOptions.YearType == ToDate.DateType.FixedYear) // ||
			//mDasaOptions.YearType == ToDate.DateType.TithiYear)
		{
			_td = new ToDate(h.Info.Jd, DasaOptions.YearLength, compress, h);
		}
		else
		{
			_td = new ToDate(h.Info.Jd, DasaOptions.YearType, DasaOptions.YearLength, compress, h);
		}

		_td.SetOffset(DasaOptions.OffsetDays + DasaOptions.OffsetHours / 24.0 + DasaOptions.OffsetMinutes / (24.0 * 60.0));
	}

	private object SetDasaOptions(object o)
	{
		var opts = (Elements.Dasas.Dasa.DasaOptions) o;
		DasaOptions.Copy(opts);
		SetDasaYearType();
		Reset();
		return DasaOptions.Clone();
	}

	private void mDateOptions_Click(object sender, EventArgs e)
	{
		Form f = new MhoraOptions(DasaOptions.Clone(), SetDasaOptions);
		f.ShowDialog();
	}

	private void dasaItemList_MouseMove(object sender, MouseEventArgs e)
	{
		var di = (DasaItem) _dasaItemList.GetItemAt(e.X, e.Y);
		if (di == null)
		{
			return;
		}

		TooltipEvent.SetToolTip(_dasaItemList, di.EventDesc);
		TooltipEvent.InitialDelay = 0;

		if (MhoraGlobalOptions.Instance.DasaMoveSelect)
		{
			di.Selected = true;
		}

		//Mhora.Log.Debug ("MouseMove: {0} {1}", e.Y, li != null ? li.Value : -1);
		//if (li != null)
		//	li.Selected = true;
	}

	private void dasaItemList_MouseDown(object sender, MouseEventArgs e)
	{
		//this.dasaItemList_MouseMove(sender, e);
	}

	private void dasaItemList_MouseEnter(object sender, EventArgs e)
	{
		//Mhora.Log.Debug ("Mouse Enter");
		//this.dasaItemList.Focus();
		//this.dasaItemList.Items[0].Selected = true;
	}

	private void dasaItemList_SelectedIndexChanged(object sender, EventArgs e)
	{
		//if (this.dasaItemList.SelectedItems.Count <= 0)
		//	return;

		//DasaItem di = (DasaItem)this.dasaItemList.SelectedItems[0];

		//tooltip_event.SetToolTip(this.dasaItemList, di.EventDesc);
		//tooltip_event.InitialDelay = 0;
	}


	private void dasaItemList_Click(object sender, EventArgs e)
	{
	}

	private void dasaItemList_MouseUp(object sender, MouseEventArgs e)
	{
		dasaItemList_MouseMove(sender, e);
		if (e.Button == MouseButtons.Left)
		{
			SplitDasa();
		}

		var li = _dasaItemList.GetItemAt(e.X, e.Y);
		//Mhora.Log.Debug ("MouseMove Click: {0} {1}", e.Y, li != null ? li.Value : -1);
		if (li != null)
		{
			li.Selected = true;
		}
	}

	private void mFixedYears365_Click(object sender, EventArgs e)
	{
		if (DasaOptions.YearType == ToDate.DateType.FixedYear && DasaOptions.YearLength == 365.2425)
		{
			return;
		}

		DasaOptions.YearType   = ToDate.DateType.FixedYear;
		DasaOptions.YearLength = 365.2425;
		SetDasaYearType();
		Reset();
	}

	private void mTithiYears_Click(object sender, EventArgs e)
	{
		if (DasaOptions.YearType == ToDate.DateType.TithiYear && DasaOptions.YearLength == 360.0)
		{
			return;
		}

		DasaOptions.YearType   = ToDate.DateType.TithiYear;
		DasaOptions.YearLength = 360.0;
		SetDasaYearType();
		Reset();
	}

	private void mSolarYears_Click(object sender, EventArgs e)
	{
		if (DasaOptions.YearType == ToDate.DateType.SolarYear && DasaOptions.YearLength == 360.0)
		{
			return;
		}

		DasaOptions.YearType   = ToDate.DateType.SolarYear;
		DasaOptions.YearLength = 360.0;
		SetDasaYearType();
		Reset();
	}

	private void mFixedYears360_Click(object sender, EventArgs e)
	{
		if (DasaOptions.YearType == ToDate.DateType.FixedYear && DasaOptions.YearLength == 360.0)
		{
			return;
		}

		DasaOptions.YearType   = ToDate.DateType.FixedYear;
		DasaOptions.YearLength = 360.0;
		SetDasaYearType();
		Reset();
	}

	private void mTribhagi80_Click(object sender, EventArgs e)
	{
		if (DasaOptions.Compression == 80)
		{
			return;
		}

		DasaOptions.Compression = 80;
		SetDasaYearType();
		Reset();
	}

	private void mTriBhagi40_Click(object sender, EventArgs e)
	{
		if (DasaOptions.Compression == 40)
		{
			return;
		}

		DasaOptions.Compression = 40;
		SetDasaYearType();
		Reset();
	}

	private void mResetParamAyus_Click(object sender, EventArgs e)
	{
		if (DasaOptions.Compression == 0)
		{
			return;
		}

		DasaOptions.Compression = 0;
		SetDasaYearType();
		Reset();
	}

	private void mCompressSolar_Click(object sender, EventArgs e)
	{
		if (DasaOptions.Compression == 1 && DasaOptions.YearType == ToDate.DateType.SolarYear && DasaOptions.YearLength == 360)
		{
			return;
		}

		DasaOptions.Compression = 1;
		DasaOptions.YearLength  = 360.0;
		DasaOptions.YearType    = ToDate.DateType.SolarYear;
		SetDasaYearType();
		Reset();
	}

	private void mCompressLunar_Click(object sender, EventArgs e)
	{
		if (DasaOptions.Compression == 1 && DasaOptions.YearType == ToDate.DateType.TithiYear && DasaOptions.YearLength == 360)
		{
			return;
		}

		DasaOptions.Compression = 1;
		DasaOptions.YearLength  = 360.0;
		DasaOptions.YearType    = ToDate.DateType.TithiYear;
		SetDasaYearType();
		Reset();
	}

	private void mCompressYoga_Click(object sender, EventArgs e)
	{
		if (DasaOptions.Compression == 1 && DasaOptions.YearType == ToDate.DateType.YogaYear && DasaOptions.YearLength == 324)
		{
			return;
		}

		DasaOptions.Compression = 1;
		DasaOptions.YearLength  = 324;
		DasaOptions.YearType    = ToDate.DateType.YogaYear;
		SetDasaYearType();
		Reset();
	}

	private void mCompressTithiPraveshaTithi_Click(object sender, EventArgs e)
	{
		DasaOptions.YearType = ToDate.DateType.TithiYear;
		var tdPravesh = new ToDate(h.Info.Jd, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
		var tdTithi   = new ToDate(h.Info.Jd, ToDate.DateType.TithiYear, 360.0, 0, h);
		if (tdTithi.AddYears(1).UniversalTime() + 15.0 < tdPravesh.AddYears(1).UniversalTime())
		{
			DasaOptions.YearLength = 390;
		}
		else
		{
			DasaOptions.YearLength = 360;
		}
		DasaOptions.Compression = 1;
		SetDasaYearType();
		Reset();
	}

	public void CompressToYogaPraveshaYearYoga()
	{
		DasaOptions.YearType = ToDate.DateType.YogaYear;
		var tdPravesh = new ToDate(h.Info.Jd, ToDate.DateType.YogaPraveshYear, 360.0, 0, h);
		var tdYoga    = new ToDate(h.Info.Jd, ToDate.DateType.YogaYear, 324.0, 0, h);
		var    dateToSurpass = tdPravesh.AddYears(1).UniversalTime() - 5;
		var    dateCurrent    = tdYoga.AddYears(0).UniversalTime();
		double months          = 0;
		while (dateCurrent < dateToSurpass)
		{
			Application.Log.Debug("{0} > {1}", h.Moment(dateCurrent), h.Moment(dateToSurpass));

			months++;
			dateCurrent = tdYoga.AddYears(months / 12.0).UniversalTime();
		}
		DasaOptions.Compression = 1;
		DasaOptions.YearLength  = (int) months * 27;
		SetDasaYearType();
		Reset();
	}

	private void mCompressedYogaPraveshaYoga_Click(object sender, EventArgs e)
	{
		CompressToYogaPraveshaYearYoga();
	}

	private void mCompressTithiPraveshaSolar_Click(object sender, EventArgs e)
	{
		var tdPravesh = new ToDate(h.Info.Jd, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
		var utStart = tdPravesh.AddYears(0).ToUniversalTime();
		var utEnd   = tdPravesh.AddYears(1).ToUniversalTime();
		var spStart = h.CalculateSingleBodyPosition(utStart.Time().TotalHours, Body.BodyType.Sun.SwephBody(), Body.BodyType.Sun, Body.Type.Graha);
		var spEnd   = h.CalculateSingleBodyPosition(utEnd.Time().TotalHours, Body.BodyType.Sun.SwephBody(), Body.BodyType.Sun, Body.Type.Graha);
		var lDiff    = spEnd.Longitude.Sub(spStart.Longitude);
		var diff     = lDiff.Value;
		if (diff < 120.0)
		{
			diff += 360.0;
		}

		DasaOptions.YearType   = ToDate.DateType.SolarYear;
		DasaOptions.YearLength = diff;
		DasaOptions.Compression = 1;
		Reset();
	}

	private void mCompressedTithiPraveshaFixed_Click(object sender, EventArgs e)
	{
		var tdPravesh = new ToDate(h.Info.Jd, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
		DasaOptions.YearType   = ToDate.DateType.FixedYear;
		DasaOptions.YearLength = tdPravesh.AddYears(1).UniversalTime() - tdPravesh.AddYears(0).UniversalTime();
		Reset();
	}


	private void mCustomYears_Click(object sender, EventArgs e)
	{
		mDateOptions_Click(sender, e);
	}

	private void bPrevCycle_Click(object sender, EventArgs e)
	{
		mPreviousCycle_Click(sender, e);
	}

	private void bNextCycle_Click(object sender, EventArgs e)
	{
		mNextCycle_Click(sender, e);
	}

	private void bDasaOptions_Click(object sender, EventArgs e)
	{
		mOptions_Click(sender, e);
	}

	private void bDateOptions_Click(object sender, EventArgs e)
	{
		mDateOptions_Click(sender, e);
	}

	private void bRasiStrengths_Click(object sender, EventArgs e)
	{
		new RasiStrengthsControl(h).ShowDialog();
		//this.mRasiStrengths_Click(sender, e);		
	}

	public object SetDasasOptions(object a)
	{
		_dasaItemList.Items.Clear();
		var al = ((DasaEntriesWrapper) a).Entries;
		for (var i = 0; i < al.Length; i++)
		{
			var di = new DasaItem(al[i]);
			di.PopulateListViewItemMembers(_td, _id);
			_dasaItemList.Items.Add(di);
		}
		LocateChartEvents();
		return a;
	}

	private void bEditItems_Click(object sender, EventArgs e)
	{
	}

	private void dasaContextMenu_Popup(object sender, EventArgs e)
	{
	}

	private void menuItem5_Click(object sender, EventArgs e)
	{
		var al    = new DasaEntry[_dasaItemList.Items.Count];
		var am    = new DasaItem[_dasaItemList.Items.Count];
		var start = 0.0;
		if (_dasaItemList.Items.Count >= 1)
		{
			start = ((DasaItem) _dasaItemList.Items[0]).Entry.StartUt;
		}

		for (var i = 0; i < _dasaItemList.Items.Count; i++)
		{
			var di = (DasaItem) _dasaItemList.Items[i];
			al[i] = di.Entry;
			if (al[i].Level == 1)
			{
				al[i].StartUt =  start;
				start         += al[i].DasaLength;
			}

			am[i] = new DasaItem(al[i]);
		}

		_dasaItemList.Items.Clear();
		for (var i = 0; i < am.Length; i++)
		{
			am[i].PopulateListViewItemMembers(_td, _id);
			_dasaItemList.Items.Add(am[i]);
		}
	}

	private void mDasaDown_Click(object sender, EventArgs e)
	{
		if (_dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) _dasaItemList.SelectedItems[0];

		var m = _td.AddYears(di.Entry.StartUt);
		h2.Info.DateOfBirth = m;

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry Chart - (((" + di.Entry.DasaName + "))) " + _id.Description());
	}

	private void mEditDasas_Click(object sender, EventArgs e)
	{
		var al = new DasaEntry[_dasaItemList.Items.Count];
		for (var i = 0; i < _dasaItemList.Items.Count; i++)
		{
			var di = (DasaItem) _dasaItemList.Items[i];
			al[i] = di.Entry;
		}

		var dw = new DasaEntriesWrapper(al);
		var f  = new MhoraOptions(dw, SetDasasOptions, true);
		f.ShowDialog();
	}

	private void bGrahaStrengths_Click(object sender, EventArgs e)
	{
		var gc = new GrahaStrengthsControl(h);
		gc.ShowDialog();
	}


	private void ExpandEvent(DateTime m, int levels, string eventDesc)
	{
		var utM = h.UniversalTime(m);
		for (var i = 0; i < _dasaItemList.Items.Count; i++)
		{
			var di = (DasaItem) _dasaItemList.Items[i];

			var mStart = _td.AddYears(di.Entry.StartUt);
			var mEnd   = _td.AddYears(di.Entry.StartUt + di.Entry.DasaLength);

			var utStart = h.UniversalTime(mStart);
			var utEnd   = h.UniversalTime(mEnd);


			if (utM >= utStart && utM < utEnd)
			{
				Application.Log.Debug("Found: Looking for {0} between {1} and {2}", m, mStart, mEnd);

				if (levels > di.Entry.Level)
				{
					if (i == _dasaItemList.Items.Count - 1)
					{
						_dasaItemList.SelectedItems.Clear();
						_dasaItemList.Items[i].Selected = true;
						SplitDasa((DasaItem) _dasaItemList.Items[i]);
					}

					if (i < _dasaItemList.Items.Count - 1)
					{
						var diNext = (DasaItem) _dasaItemList.Items[i + 1];
						if (diNext.Entry.Level == di.Entry.Level)
						{
							_dasaItemList.SelectedItems.Clear();
							_dasaItemList.Items[i].Selected = true;
							SplitDasa((DasaItem) _dasaItemList.Items[i]);
						}
					}
				}
				else if (levels == di.Entry.Level)
				{
					foreach (ListViewItem.ListViewSubItem si in di.SubItems)
					{
						si.BackColor = MhoraGlobalOptions.Instance.DasaHighlightColor;
					}

					di.EventDesc += eventDesc;
				}
			}
		}
	}

	public object LocateEvent(object euo)
	{
		var options = (EventUserOptions) euo;
		_mEventOptionsCache = options;
		ExpandEvent(options.EventDate, options.Depth, "Event: " + options.EventDate);
		return euo;
	}

	public void LocateChartEvents()
	{
		if (_mShowEvents.Checked == false)
		{
			return;
		}

		foreach (var ue in h.Info.Events)
		{
			if (ue.WorkWithEvent)
			{
				ExpandEvent(ue.EventTime, MhoraGlobalOptions.Instance.DasaEventsLevel, ue.ToString());
			}
		}
	}

	private void mLocateEvent_Click(object sender, EventArgs e)
	{
		if (_mEventOptionsCache == null)
		{
			var dtNow = DateTime.Now;
			_mEventOptionsCache = new EventUserOptions(dtNow);
		}

		new MhoraOptions(_mEventOptionsCache, LocateEvent).ShowDialog();
	}


	private void dasaItemList_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(typeof(DivisionalChart)))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void dasaItemList_DragDrop(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(typeof(DivisionalChart)))
		{
			var div = Division.CopyFromClipboard();
			if (null == div)
			{
				return;
			}

			_id.DivisionChanged(div);
		}
	}

	private void dasaInfo_Click(object sender, EventArgs e)
	{
	}

	private void mShowEvents_Click(object sender, EventArgs e)
	{
		_mShowEvents.Checked = !_mShowEvents.Checked;
	}

	private void m3Parts_Click(object sender, EventArgs e)
	{
		if (_dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var di = (DasaItem) _dasaItemList.SelectedItems[0];
		var de = di.Entry;

		var form = new Dasa3Parts(h, de, _td);
		form.Show();
	}

	private class DasaEntriesWrapper
	{
		public DasaEntriesWrapper(DasaEntry[] al)
		{
			Entries = al;
		}

		public DasaEntry[] Entries
		{
			get;
		}
	}


	private class EventUserOptions : ICloneable
	{
		public EventUserOptions(DateTime m)
		{
			EventDate = m;
			Depth     = 2;
		}

		public DateTime EventDate
		{
			get;
		}

		public int Depth
		{
			get;
			set;
		}

		public object Clone()
		{
			var euo = new EventUserOptions(EventDate);
			euo.Depth = Depth;
			return euo;
		}
	}
}