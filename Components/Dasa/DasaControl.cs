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
using Mhora.SwissEph;
using Mhora.Tables;
using Mhora.Util;

namespace Mhora.Components.Dasa;

/// <summary>
///     Summary description for DasaControl.
/// </summary>
public class DasaControl : MhoraControl //System.Windows.Forms.UserControl
{
	private static readonly ToolTip tooltip_event = new();

	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;

	private readonly IDasa  id;
	private          Button bDasaOptions;
	private          Button bDateOptions;
	private          Button bGrahaStrengths;
	private          Button bNextCycle;
	private          Button bPrevCycle;
	private          Button bRasiStrengths;

	private ColumnHeader Dasa;
	private ContextMenu  dasaContextMenu;
	private Label        dasaInfo;
	private ListView     dasaItemList;
	private MenuItem     m3Parts;
	private MenuItem     mCompressedTithiPraveshaFixed;
	private MenuItem     mCompressedYogaPraveshaYoga;
	private MenuItem     mCompressLunar;
	private MenuItem     mCompressSolar;
	private MenuItem     mCompressTithiPraveshaSolar;
	private MenuItem     mCompressTithiPraveshaTithi;
	private MenuItem     mCompressYoga;
	private MenuItem     mCustomYears;
	private MenuItem     mDateOptions;
	private MenuItem     mEditDasas;
	private MenuItem     mEntryChart;
	private MenuItem     mEntryChartCompressed;
	private MenuItem     mEntryDate;
	private MenuItem     mEntrySunriseChart;
	private MenuItem     menuItem1;
	private MenuItem     menuItem2;
	private MenuItem     menuItem3;
	private MenuItem     menuItem4;
	private MenuItem     menuItem5;
	private MenuItem     menuItem6;

	private EventUserOptions mEventOptionsCache;
	private MenuItem         mFixedYears360;
	private MenuItem         mFixedYears365;

	private int min_cycle, max_cycle;

	private MenuItem     mLocateEvent;
	private MenuItem     mNextCycle;
	private MenuItem     mNormalize;
	private MenuItem     mOptions;
	private MenuItem     mPreviousCycle;
	private MenuItem     mReset;
	private MenuItem     mResetParamAyus;
	private MenuItem     mShowEvents;
	private MenuItem     mSolarYears;
	private MenuItem     mTithiYears;
	private MenuItem     mTriBhagi40;
	private MenuItem     mTribhagi80;
	private ColumnHeader StartDate;
	private ToDate       td;


	public DasaControl(Horoscope _h, IDasa _id)
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitForm call
		h           = _h;
		id          = _id;
		DasaOptions = new Elements.Dasas.Dasa.Options();

		if (h.info.defaultYearCompression != 0)
		{
			DasaOptions.Compression = h.info.defaultYearCompression;
			DasaOptions.YearLength  = h.info.defaultYearLength;
			DasaOptions.YearType    = h.info.defaultYearType;
		}


		SetDasaYearType();
		//td = new ToDate (h.baseUT, mDasaOptions.YearLength, 0.0, h);
		mShowEvents.Checked = MhoraGlobalOptions.Instance.DasaShowEvents;
		ResetDisplayOptions(MhoraGlobalOptions.Instance);

		var d = (Elements.Dasas.Dasa) id;
		d.RecalculateEvent                     += recalculateEntries;
		MhoraGlobalOptions.DisplayPrefsChanged += ResetDisplayOptions;
		h.Changed                              += OnRecalculate;
		SetDescriptionLabel();
		d.Changed += OnDasaChanged;
		if (dasaItemList.Items.Count >= 1)
		{
			dasaItemList.Items[0].Selected = true;
		}

		VScroll = true;
		Reset();

		//this.LocateChartEvents();
	}

	public Elements.Dasas.Dasa.Options DasaOptions
	{
		get;
	}

	public object DasaSpecificOptions
	{
		get => id.GetOptions();
		set => id.SetOptions(value);
	}


	public bool LinkToHoroscope
	{
		set
		{
			if (value)
			{
				h.Changed                          += OnRecalculate;
				((Elements.Dasas.Dasa) id).Changed += OnDasaChanged;
			}
			else
			{
				h.Changed                          -= OnRecalculate;
				((Elements.Dasas.Dasa) id).Changed += OnDasaChanged;
			}
		}
	}

	private void SetDescriptionLabel()
	{
		dasaInfo.Text = id.Description();

		dasaInfo.Text += " (";

		if (DasaOptions.Compression > 0)
		{
			dasaInfo.Text += DasaOptions.Compression.ToString();
		}


		dasaInfo.Text = string.Format("{0} {1:0.00} {2}", dasaInfo.Text, DasaOptions.YearLength, DasaOptions.YearType);

		dasaInfo.Text += " )";
	}

	public void ResetDisplayOptions(object o)
	{
		dasaItemList.BackColor = MhoraGlobalOptions.Instance.DasaBackgroundColor;
		dasaItemList.Font      = MhoraGlobalOptions.Instance.GeneralFont;
		foreach (ListViewItem li in dasaItemList.Items)
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

		dasaItemList.HoverSelection = MhoraGlobalOptions.Instance.DasaHoverSelect;
		LocateChartEvents();
	}

	public void Reset()
	{
		id.recalculateOptions();
		SetDescriptionLabel();
		dasaItemList.Items.Clear();
		SetDasaYearType();
		min_cycle = max_cycle = 0;
		var compress          = DasaOptions.Compression == 0.0 ? 0.0 : DasaOptions.Compression / id.paramAyus();

		sweph.obtainLock(h);
		var a = id.Dasa(0);
		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.populateListViewItemMembers(td, id);
			dasaItemList.Items.Add(di);
		}

		sweph.releaseLock(h);
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
			if (components != null)
			{
				components.Dispose();
			}
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
		this.dasaItemList                  = new System.Windows.Forms.ListView();
		this.Dasa                          = new System.Windows.Forms.ColumnHeader();
		this.StartDate                     = new System.Windows.Forms.ColumnHeader();
		this.dasaContextMenu               = new System.Windows.Forms.ContextMenu();
		this.mEntryChart                   = new System.Windows.Forms.MenuItem();
		this.mEntrySunriseChart            = new System.Windows.Forms.MenuItem();
		this.mEntryDate                    = new System.Windows.Forms.MenuItem();
		this.mLocateEvent                  = new System.Windows.Forms.MenuItem();
		this.mReset                        = new System.Windows.Forms.MenuItem();
		this.m3Parts                       = new System.Windows.Forms.MenuItem();
		this.mShowEvents                   = new System.Windows.Forms.MenuItem();
		this.mOptions                      = new System.Windows.Forms.MenuItem();
		this.mDateOptions                  = new System.Windows.Forms.MenuItem();
		this.mPreviousCycle                = new System.Windows.Forms.MenuItem();
		this.mNextCycle                    = new System.Windows.Forms.MenuItem();
		this.menuItem3                     = new System.Windows.Forms.MenuItem();
		this.mSolarYears                   = new System.Windows.Forms.MenuItem();
		this.mTithiYears                   = new System.Windows.Forms.MenuItem();
		this.mFixedYears360                = new System.Windows.Forms.MenuItem();
		this.mFixedYears365                = new System.Windows.Forms.MenuItem();
		this.mCustomYears                  = new System.Windows.Forms.MenuItem();
		this.menuItem5                     = new System.Windows.Forms.MenuItem();
		this.mTribhagi80                   = new System.Windows.Forms.MenuItem();
		this.mTriBhagi40                   = new System.Windows.Forms.MenuItem();
		this.mResetParamAyus               = new System.Windows.Forms.MenuItem();
		this.menuItem6                     = new System.Windows.Forms.MenuItem();
		this.mCompressSolar                = new System.Windows.Forms.MenuItem();
		this.mCompressLunar                = new System.Windows.Forms.MenuItem();
		this.mCompressYoga                 = new System.Windows.Forms.MenuItem();
		this.mCompressTithiPraveshaTithi   = new System.Windows.Forms.MenuItem();
		this.mCompressTithiPraveshaSolar   = new System.Windows.Forms.MenuItem();
		this.mCompressedTithiPraveshaFixed = new System.Windows.Forms.MenuItem();
		this.mCompressedYogaPraveshaYoga   = new System.Windows.Forms.MenuItem();
		this.menuItem4                     = new System.Windows.Forms.MenuItem();
		this.mEditDasas                    = new System.Windows.Forms.MenuItem();
		this.mNormalize                    = new System.Windows.Forms.MenuItem();
		this.menuItem1                     = new System.Windows.Forms.MenuItem();
		this.menuItem2                     = new System.Windows.Forms.MenuItem();
		this.dasaInfo                      = new System.Windows.Forms.Label();
		this.bPrevCycle                    = new System.Windows.Forms.Button();
		this.bNextCycle                    = new System.Windows.Forms.Button();
		this.bDasaOptions                  = new System.Windows.Forms.Button();
		this.bDateOptions                  = new System.Windows.Forms.Button();
		this.bRasiStrengths                = new System.Windows.Forms.Button();
		this.bGrahaStrengths               = new System.Windows.Forms.Button();
		this.mEntryChartCompressed         = new System.Windows.Forms.MenuItem();
		this.SuspendLayout();
		// 
		// dasaItemList
		// 
		this.dasaItemList.AllowColumnReorder = true;
		this.dasaItemList.AllowDrop          = true;
		this.dasaItemList.Anchor             = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.dasaItemList.BackColor          = System.Drawing.Color.Lavender;
		this.dasaItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this.Dasa,
			this.StartDate
		});
		this.dasaItemList.ContextMenu          =  this.dasaContextMenu;
		this.dasaItemList.Font                 =  new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.dasaItemList.ForeColor            =  System.Drawing.Color.Black;
		this.dasaItemList.FullRowSelect        =  true;
		this.dasaItemList.HideSelection        =  false;
		this.dasaItemList.HoverSelection       =  true;
		this.dasaItemList.Location             =  new System.Drawing.Point(8, 40);
		this.dasaItemList.MultiSelect          =  false;
		this.dasaItemList.Name                 =  "dasaItemList";
		this.dasaItemList.Size                 =  new System.Drawing.Size(424, 264);
		this.dasaItemList.TabIndex             =  0;
		this.dasaItemList.View                 =  System.Windows.Forms.View.Details;
		this.dasaItemList.MouseDown            += new System.Windows.Forms.MouseEventHandler(this.dasaItemList_MouseDown);
		this.dasaItemList.Click                += new System.EventHandler(this.dasaItemList_Click);
		this.dasaItemList.MouseUp              += new System.Windows.Forms.MouseEventHandler(this.dasaItemList_MouseUp);
		this.dasaItemList.DragDrop             += new System.Windows.Forms.DragEventHandler(this.dasaItemList_DragDrop);
		this.dasaItemList.MouseEnter           += new System.EventHandler(this.dasaItemList_MouseEnter);
		this.dasaItemList.DragEnter            += new System.Windows.Forms.DragEventHandler(this.dasaItemList_DragEnter);
		this.dasaItemList.MouseMove            += new System.Windows.Forms.MouseEventHandler(this.dasaItemList_MouseMove);
		this.dasaItemList.SelectedIndexChanged += new System.EventHandler(this.dasaItemList_SelectedIndexChanged);
		// 
		// Dasa
		// 
		this.Dasa.Text  = "Dasa";
		this.Dasa.Width = 150;
		// 
		// StartDate
		// 
		this.StartDate.Text  = "Dates";
		this.StartDate.Width = 500;
		// 
		// dasaContextMenu
		// 
		this.dasaContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.mEntryChart,
			this.mEntryChartCompressed,
			this.mEntrySunriseChart,
			this.mEntryDate,
			this.mLocateEvent,
			this.mReset,
			this.m3Parts,
			this.mShowEvents,
			this.mOptions,
			this.mDateOptions,
			this.mPreviousCycle,
			this.mNextCycle,
			this.menuItem3,
			this.menuItem4,
			this.menuItem1,
			this.menuItem2
		});
		this.dasaContextMenu.Popup += new System.EventHandler(this.dasaContextMenu_Popup);
		// 
		// mEntryChart
		// 
		this.mEntryChart.Index =  0;
		this.mEntryChart.Text  =  "&Entry Chart";
		this.mEntryChart.Click += new System.EventHandler(this.mEntryChart_Click);
		// 
		// mEntrySunriseChart
		// 
		this.mEntrySunriseChart.Index =  2;
		this.mEntrySunriseChart.Text  =  "Entry &Sunrise Chart";
		this.mEntrySunriseChart.Click += new System.EventHandler(this.mEntrySunriseChart_Click);
		// 
		// mEntryDate
		// 
		this.mEntryDate.Index =  3;
		this.mEntryDate.Text  =  "Copy Entry Date";
		this.mEntryDate.Click += new System.EventHandler(this.mEntryDate_Click);
		// 
		// mLocateEvent
		// 
		this.mLocateEvent.Index =  4;
		this.mLocateEvent.Text  =  "Locate An Event";
		this.mLocateEvent.Click += new System.EventHandler(this.mLocateEvent_Click);
		// 
		// mReset
		// 
		this.mReset.Index =  5;
		this.mReset.Text  =  "&Reset";
		this.mReset.Click += new System.EventHandler(this.mReset_Click);
		// 
		// m3Parts
		// 
		this.m3Parts.Index =  6;
		this.m3Parts.Text  =  "3 Parts";
		this.m3Parts.Click += new System.EventHandler(this.m3Parts_Click);
		// 
		// mShowEvents
		// 
		this.mShowEvents.Checked =  true;
		this.mShowEvents.Index   =  7;
		this.mShowEvents.Text    =  "Show Events";
		this.mShowEvents.Click   += new System.EventHandler(this.mShowEvents_Click);
		// 
		// mOptions
		// 
		this.mOptions.Index   =  8;
		this.mOptions.Text    =  "Dasa &Options";
		this.mOptions.Visible =  false;
		this.mOptions.Click   += new System.EventHandler(this.mOptions_Click);
		// 
		// mDateOptions
		// 
		this.mDateOptions.Index   =  9;
		this.mDateOptions.Text    =  "&Date Options";
		this.mDateOptions.Visible =  false;
		this.mDateOptions.Click   += new System.EventHandler(this.mDateOptions_Click);
		// 
		// mPreviousCycle
		// 
		this.mPreviousCycle.Index   =  10;
		this.mPreviousCycle.Text    =  "&Previous Cycle";
		this.mPreviousCycle.Visible =  false;
		this.mPreviousCycle.Click   += new System.EventHandler(this.mPreviousCycle_Click);
		// 
		// mNextCycle
		// 
		this.mNextCycle.Index   =  11;
		this.mNextCycle.Text    =  "&Next Cycle";
		this.mNextCycle.Visible =  false;
		this.mNextCycle.Click   += new System.EventHandler(this.mNextCycle_Click);
		// 
		// menuItem3
		// 
		this.menuItem3.Index = 12;
		this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.mSolarYears,
			this.mTithiYears,
			this.mFixedYears360,
			this.mFixedYears365,
			this.mCustomYears,
			this.menuItem5,
			this.mTribhagi80,
			this.mTriBhagi40,
			this.mResetParamAyus,
			this.menuItem6,
			this.mCompressSolar,
			this.mCompressLunar,
			this.mCompressYoga,
			this.mCompressTithiPraveshaTithi,
			this.mCompressTithiPraveshaSolar,
			this.mCompressedTithiPraveshaFixed,
			this.mCompressedYogaPraveshaYoga
		});
		this.menuItem3.Text = "Year Options";
		// 
		// mSolarYears
		// 
		this.mSolarYears.Index =  0;
		this.mSolarYears.Text  =  "&Solar Years (360 degrees)";
		this.mSolarYears.Click += new System.EventHandler(this.mSolarYears_Click);
		// 
		// mTithiYears
		// 
		this.mTithiYears.Index =  1;
		this.mTithiYears.Text  =  "&Tithi Years (360 tithis)";
		this.mTithiYears.Click += new System.EventHandler(this.mTithiYears_Click);
		// 
		// mFixedYears360
		// 
		this.mFixedYears360.Index =  2;
		this.mFixedYears360.Text  =  "Savana Years (360 days)";
		this.mFixedYears360.Click += new System.EventHandler(this.mFixedYears360_Click);
		// 
		// mFixedYears365
		// 
		this.mFixedYears365.Index =  3;
		this.mFixedYears365.Text  =  "~ Solar Year (365.2425 days)";
		this.mFixedYears365.Click += new System.EventHandler(this.mFixedYears365_Click);
		// 
		// mCustomYears
		// 
		this.mCustomYears.Index =  4;
		this.mCustomYears.Text  =  "&Custom Years";
		this.mCustomYears.Click += new System.EventHandler(this.mCustomYears_Click);
		// 
		// menuItem5
		// 
		this.menuItem5.Index = 5;
		this.menuItem5.Text  = "-";
		// 
		// mTribhagi80
		// 
		this.mTribhagi80.Index =  6;
		this.mTribhagi80.Text  =  "Tribhagi ParamAyus (80 Years)";
		this.mTribhagi80.Click += new System.EventHandler(this.mTribhagi80_Click);
		// 
		// mTriBhagi40
		// 
		this.mTriBhagi40.Index =  7;
		this.mTriBhagi40.Text  =  "Tribhagi ParamAyus (40 Years)";
		this.mTriBhagi40.Click += new System.EventHandler(this.mTriBhagi40_Click);
		// 
		// mResetParamAyus
		// 
		this.mResetParamAyus.Index =  8;
		this.mResetParamAyus.Text  =  "Regular ParamAyus";
		this.mResetParamAyus.Click += new System.EventHandler(this.mResetParamAyus_Click);
		// 
		// menuItem6
		// 
		this.menuItem6.Index = 9;
		this.menuItem6.Text  = "-";
		// 
		// mCompressSolar
		// 
		this.mCompressSolar.Index =  10;
		this.mCompressSolar.Text  =  "Compress to Solar Year";
		this.mCompressSolar.Click += new System.EventHandler(this.mCompressSolar_Click);
		// 
		// mCompressLunar
		// 
		this.mCompressLunar.Index =  11;
		this.mCompressLunar.Text  =  "Compress to Tithi Year";
		this.mCompressLunar.Click += new System.EventHandler(this.mCompressLunar_Click);
		// 
		// mCompressYoga
		// 
		this.mCompressYoga.Index =  12;
		this.mCompressYoga.Text  =  "Compress to Yoga Year";
		this.mCompressYoga.Click += new System.EventHandler(this.mCompressYoga_Click);
		// 
		// mCompressTithiPraveshaTithi
		// 
		this.mCompressTithiPraveshaTithi.Index =  13;
		this.mCompressTithiPraveshaTithi.Text  =  "Compress to Tithi Pravesha Year (Tithi)";
		this.mCompressTithiPraveshaTithi.Click += new System.EventHandler(this.mCompressTithiPraveshaTithi_Click);
		// 
		// mCompressTithiPraveshaSolar
		// 
		this.mCompressTithiPraveshaSolar.Index =  14;
		this.mCompressTithiPraveshaSolar.Text  =  "Compress to Tithi Pravesha Year (Solar)";
		this.mCompressTithiPraveshaSolar.Click += new System.EventHandler(this.mCompressTithiPraveshaSolar_Click);
		// 
		// mCompressedTithiPraveshaFixed
		// 
		this.mCompressedTithiPraveshaFixed.Index =  15;
		this.mCompressedTithiPraveshaFixed.Text  =  "Compress to Tithi Pravesha Year (Fixed)";
		this.mCompressedTithiPraveshaFixed.Click += new System.EventHandler(this.mCompressedTithiPraveshaFixed_Click);
		// 
		// mCompressedYogaPraveshaYoga
		// 
		this.mCompressedYogaPraveshaYoga.Index =  16;
		this.mCompressedYogaPraveshaYoga.Text  =  "Compress to Yoga Pravesha Year (Yoga)";
		this.mCompressedYogaPraveshaYoga.Click += new System.EventHandler(this.mCompressedYogaPraveshaYoga_Click);
		// 
		// menuItem4
		// 
		this.menuItem4.Index = 13;
		this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.mEditDasas,
			this.mNormalize
		});
		this.menuItem4.Text = "Advanced";
		// 
		// mEditDasas
		// 
		this.mEditDasas.Index =  0;
		this.mEditDasas.Text  =  "Edit Dasas";
		this.mEditDasas.Click += new System.EventHandler(this.mEditDasas_Click);
		// 
		// mNormalize
		// 
		this.mNormalize.Index =  1;
		this.mNormalize.Text  =  "Normalize Dates";
		this.mNormalize.Click += new System.EventHandler(this.menuItem5_Click);
		// 
		// menuItem1
		// 
		this.menuItem1.Index = 14;
		this.menuItem1.Text  = "-";
		// 
		// menuItem2
		// 
		this.menuItem2.Index = 15;
		this.menuItem2.Text  = "-";
		// 
		// dasaInfo
		// 
		this.dasaInfo.Anchor    =  ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.dasaInfo.Location  =  new System.Drawing.Point(184, 8);
		this.dasaInfo.Name      =  "dasaInfo";
		this.dasaInfo.Size      =  new System.Drawing.Size(232, 23);
		this.dasaInfo.TabIndex  =  1;
		this.dasaInfo.TextAlign =  System.Drawing.ContentAlignment.MiddleLeft;
		this.dasaInfo.Click     += new System.EventHandler(this.dasaInfo_Click);
		// 
		// bPrevCycle
		// 
		this.bPrevCycle.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.bPrevCycle.Location =  new System.Drawing.Point(8, 8);
		this.bPrevCycle.Name     =  "bPrevCycle";
		this.bPrevCycle.Size     =  new System.Drawing.Size(24, 23);
		this.bPrevCycle.TabIndex =  2;
		this.bPrevCycle.Text     =  "<";
		this.bPrevCycle.Click    += new System.EventHandler(this.bPrevCycle_Click);
		// 
		// bNextCycle
		// 
		this.bNextCycle.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.bNextCycle.Location =  new System.Drawing.Point(32, 8);
		this.bNextCycle.Name     =  "bNextCycle";
		this.bNextCycle.Size     =  new System.Drawing.Size(24, 23);
		this.bNextCycle.TabIndex =  3;
		this.bNextCycle.Text     =  ">";
		this.bNextCycle.Click    += new System.EventHandler(this.bNextCycle_Click);
		// 
		// bDasaOptions
		// 
		this.bDasaOptions.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.bDasaOptions.Location =  new System.Drawing.Point(64, 8);
		this.bDasaOptions.Name     =  "bDasaOptions";
		this.bDasaOptions.Size     =  new System.Drawing.Size(40, 23);
		this.bDasaOptions.TabIndex =  4;
		this.bDasaOptions.Text     =  "Opts";
		this.bDasaOptions.Click    += new System.EventHandler(this.bDasaOptions_Click);
		// 
		// bDateOptions
		// 
		this.bDateOptions.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.bDateOptions.Location =  new System.Drawing.Point(104, 8);
		this.bDateOptions.Name     =  "bDateOptions";
		this.bDateOptions.Size     =  new System.Drawing.Size(24, 23);
		this.bDateOptions.TabIndex =  5;
		this.bDateOptions.Text     =  "Yr";
		this.bDateOptions.Click    += new System.EventHandler(this.bDateOptions_Click);
		// 
		// bRasiStrengths
		// 
		this.bRasiStrengths.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.bRasiStrengths.Location =  new System.Drawing.Point(128, 8);
		this.bRasiStrengths.Name     =  "bRasiStrengths";
		this.bRasiStrengths.Size     =  new System.Drawing.Size(24, 23);
		this.bRasiStrengths.TabIndex =  6;
		this.bRasiStrengths.Text     =  "R";
		this.bRasiStrengths.Click    += new System.EventHandler(this.bRasiStrengths_Click);
		// 
		// bGrahaStrengths
		// 
		this.bGrahaStrengths.Font     =  new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
		this.bGrahaStrengths.Location =  new System.Drawing.Point(152, 8);
		this.bGrahaStrengths.Name     =  "bGrahaStrengths";
		this.bGrahaStrengths.Size     =  new System.Drawing.Size(24, 23);
		this.bGrahaStrengths.TabIndex =  7;
		this.bGrahaStrengths.Text     =  "G";
		this.bGrahaStrengths.Click    += new System.EventHandler(this.bGrahaStrengths_Click);
		// 
		// mEntryChartCompressed
		// 
		this.mEntryChartCompressed.Index =  1;
		this.mEntryChartCompressed.Text  =  "Entry Chart (&Compressed)";
		this.mEntryChartCompressed.Click += new System.EventHandler(this.mEntryChartCompressed_Click);
		// 
		// DasaControl
		// 
		this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.Controls.Add(this.bGrahaStrengths);
		this.Controls.Add(this.bRasiStrengths);
		this.Controls.Add(this.bDateOptions);
		this.Controls.Add(this.bDasaOptions);
		this.Controls.Add(this.bNextCycle);
		this.Controls.Add(this.bPrevCycle);
		this.Controls.Add(this.dasaInfo);
		this.Controls.Add(this.dasaItemList);
		this.Name =  "DasaControl";
		this.Size =  new System.Drawing.Size(440, 312);
		this.Load += new System.EventHandler(this.DasaControl_Load);
		this.ResumeLayout(false);
	}

#endregion

	private void mEntryChart_Click(object sender, EventArgs e)
	{
		if (dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) dasaItemList.SelectedItems[0];

		sweph.obtainLock(h);
		var m = td.AddYears(di.entry.startUT);
		h2.info.tob = m;
		sweph.releaseLock(h);

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry - (" + di.entry.shortDesc + ") " + id.Description());
	}


	private void mEntryChartCompressed_Click(object sender, EventArgs e)
	{
		if (dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) dasaItemList.SelectedItems[0];

		sweph.obtainLock(h);
		var m    = td.AddYears(di.entry.startUT);
		var mEnd = td.AddYears(di.entry.startUT + di.entry.dasaLength);

		var ut_diff = mEnd.toUniversalTime() - m.toUniversalTime();
		h2.info.tob = m;
		sweph.releaseLock(h);


		h2.info.defaultYearCompression = 1;
		h2.info.defaultYearLength      = ut_diff;
		h2.info.defaultYearType        = ToDate.DateType.FixedYear;

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry - (" + di.entry.shortDesc + ") " + id.Description());
	}


	private void mEntrySunriseChart_Click(object sender, EventArgs e)
	{
		if (dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) dasaItemList.SelectedItems[0];

		sweph.obtainLock(h);
		var m = td.AddYears(di.entry.startUT);
		sweph.releaseLock(h);
		h2.info.tob = m;

		h2.OnChanged();

		// if done once, get something usually 2+ minutes off. 
		// don't know why this is.
		var offsetSunrise = h2.hoursAfterSunrise() / 24.0;
		m           = new Moment(h2.baseUT - offsetSunrise, h2);
		h2.info.tob = m;
		h2.OnChanged();

		// so do it a second time, getting sunrise + 1 second.
		offsetSunrise = h2.hoursAfterSunrise() / 24.0;
		m             = new Moment(h2.baseUT - offsetSunrise + 1.0 / (24.0 * 60.0 * 60.0), h2);
		h2.info.tob   = m;
		h2.OnChanged();

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry Sunrise - (" + di.entry.shortDesc + ") " + id.Description());
	}


	private void mEntryDate_Click(object sender, EventArgs e)
	{
		if (dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var di = (DasaItem) dasaItemList.SelectedItems[0];
		sweph.obtainLock(h);
		var m = td.AddYears(di.entry.startUT);
		sweph.releaseLock(h);
		Clipboard.SetDataObject(m.ToString(), true);
	}

	private void SplitDasa()
	{
		if (dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		SplitDasa((DasaItem) dasaItemList.SelectedItems[0]);
	}

	private void SplitDasa(DasaItem di)
	{
		//Trace.Assert(dasaItemList.SelectedItems.Count >= 1, "dasaItemList::doubleclick");
		var index = di.Index + 1;

		var action_inserting = true;


		dasaItemList.BeginUpdate();
		while (index < dasaItemList.Items.Count)
		{
			var tdi = (DasaItem) dasaItemList.Items[index];
			if (tdi.entry.level > di.entry.level)
			{
				action_inserting = false;
				dasaItemList.Items.Remove(tdi);
			}
			else
			{
				break;
			}
		}

		if (action_inserting == false)
		{
			dasaItemList.EndUpdate();
			return;
		}

		var a        = id.AntarDasa(di.entry);
		var compress = DasaOptions.Compression == 0.0 ? 0.0 : DasaOptions.Compression / id.paramAyus();

		sweph.obtainLock(h);
		foreach (DasaEntry de in a)
		{
			var pdi = new DasaItem(de);
			pdi.populateListViewItemMembers(td, id);
			dasaItemList.Items.Insert(index, pdi);
			index = index + 1;
		}

		sweph.releaseLock(h);
		dasaItemList.EndUpdate();
		//this.dasaItemList.Items[index-1].Selected = true;
	}

	protected override void copyToClipboard()
	{
		var iMaxDescLength = 0;
		for (var i = 0; i < dasaItemList.Items.Count; i++)
		{
			iMaxDescLength = Math.Max(dasaItemList.Items[i].Text.Length, iMaxDescLength);
		}

		iMaxDescLength += 2;

		var s = dasaInfo.Text + "\r\n\r\n";
		for (var i = 0; i < dasaItemList.Items.Count; i++)
		{
			var li         = dasaItemList.Items[i];
			var di         = (DasaItem) li;
			var levelSpace = di.entry.level * 2;
			s += li.Text.PadRight(iMaxDescLength + levelSpace, ' ');

			for (var j = 1; j < li.SubItems.Count; j++)
			{
				s += "(" + li.SubItems[j].Text + ") ";
			}

			s += "\r\n";
		}

		Clipboard.SetDataObject(s);
	}

	private void recalculateEntries()
	{
		SetDescriptionLabel();
		dasaItemList.Items.Clear();
		var a = new ArrayList();
		for (var i = min_cycle; i <= max_cycle; i++)
		{
			var b = id.Dasa(i);
			a.AddRange(b);
		}

		sweph.obtainLock(h);
		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.populateListViewItemMembers(td, id);
			dasaItemList.Items.Add(di);
		}

		sweph.releaseLock(h);
		LocateChartEvents();
	}

	private void mOptions_Click(object sender, EventArgs e)
	{
		//object wrapper = new GlobalizedPropertiesWrapper(id.GetOptions());
		var f = new MhoraOptions(id.GetOptions(), id.SetOptions);
		f.pGrid.ExpandAllGridItems();
		f.ShowDialog();
	}

	private void mPreviousCycle_Click(object sender, EventArgs e)
	{
		min_cycle--;
		var a = id.Dasa(min_cycle);
		var i = 0;
		sweph.obtainLock(h);
		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.populateListViewItemMembers(td, id);
			dasaItemList.Items.Insert(i, di);
			i++;
		}

		sweph.releaseLock(h);
	}

	private void mNextCycle_Click(object sender, EventArgs e)
	{
		max_cycle++;
		var a = id.Dasa(max_cycle);
		sweph.obtainLock(h);
		foreach (DasaEntry de in a)
		{
			var di = new DasaItem(de);
			di.populateListViewItemMembers(td, id);
			dasaItemList.Items.Add(di);
		}

		sweph.releaseLock(h);
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
		AddViewsToContextMenu(dasaContextMenu);
	}

	private void SetDasaYearType()
	{
		var compress = DasaOptions.Compression == 0.0 ? 0.0 : DasaOptions.Compression / id.paramAyus();
		if (DasaOptions.YearType == ToDate.DateType.FixedYear) // ||
			//mDasaOptions.YearType == ToDate.DateType.TithiYear)
		{
			td = new ToDate(h.baseUT, DasaOptions.YearLength, compress, h);
		}
		else
		{
			td = new ToDate(h.baseUT, DasaOptions.YearType, DasaOptions.YearLength, compress, h);
		}

		td.SetOffset(DasaOptions.OffsetDays + DasaOptions.OffsetHours / 24.0 + DasaOptions.OffsetMinutes / (24.0 * 60.0));
	}

	private object SetDasaOptions(object o)
	{
		var opts = (Elements.Dasas.Dasa.Options) o;
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
		var di = (DasaItem) dasaItemList.GetItemAt(e.X, e.Y);
		if (di == null)
		{
			return;
		}

		tooltip_event.SetToolTip(dasaItemList, di.EventDesc);
		tooltip_event.InitialDelay = 0;

		if (MhoraGlobalOptions.Instance.DasaMoveSelect)
		{
			di.Selected = true;
		}

		//mhora.Log.Debug ("MouseMove: {0} {1}", e.Y, li != null ? li.Value : -1);
		//if (li != null)
		//	li.Selected = true;
	}

	private void dasaItemList_MouseDown(object sender, MouseEventArgs e)
	{
		//this.dasaItemList_MouseMove(sender, e);
	}

	private void dasaItemList_MouseEnter(object sender, EventArgs e)
	{
		//mhora.Log.Debug ("Mouse Enter");
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

		var li = dasaItemList.GetItemAt(e.X, e.Y);
		//mhora.Log.Debug ("MouseMove Click: {0} {1}", e.Y, li != null ? li.Value : -1);
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
		var td_pravesh = new ToDate(h.baseUT, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
		var td_tithi   = new ToDate(h.baseUT, ToDate.DateType.TithiYear, 360.0, 0, h);
		sweph.obtainLock(h);
		if (td_tithi.AddYears(1).toUniversalTime() + 15.0 < td_pravesh.AddYears(1).toUniversalTime())
		{
			DasaOptions.YearLength = 390;
		}
		else
		{
			DasaOptions.YearLength = 360;
		}

		sweph.releaseLock(h);
		DasaOptions.Compression = 1;
		SetDasaYearType();
		Reset();
	}

	public void compressToYogaPraveshaYearYoga()
	{
		DasaOptions.YearType = ToDate.DateType.YogaYear;
		var td_pravesh = new ToDate(h.baseUT, ToDate.DateType.YogaPraveshYear, 360.0, 0, h);
		var td_yoga    = new ToDate(h.baseUT, ToDate.DateType.YogaYear, 324.0, 0, h);
		sweph.obtainLock(h);
		var    date_to_surpass = td_pravesh.AddYears(1).toUniversalTime() - 5;
		var    date_current    = td_yoga.AddYears(0).toUniversalTime();
		double months          = 0;
		while (date_current < date_to_surpass)
		{
			Application.Log.Debug("{0} > {1}", new Moment(date_current, h), new Moment(date_to_surpass, h));

			months++;
			date_current = td_yoga.AddYears(months / 12.0).toUniversalTime();
		}

		sweph.releaseLock(h);
		DasaOptions.Compression = 1;
		DasaOptions.YearLength  = (int) months * 27;
		SetDasaYearType();
		Reset();
	}

	private void mCompressedYogaPraveshaYoga_Click(object sender, EventArgs e)
	{
		compressToYogaPraveshaYearYoga();
	}

	private void mCompressTithiPraveshaSolar_Click(object sender, EventArgs e)
	{
		var td_pravesh = new ToDate(h.baseUT, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
		sweph.obtainLock(h);
		var ut_start = td_pravesh.AddYears(0).toUniversalTime();
		var ut_end   = td_pravesh.AddYears(1).toUniversalTime();
		var sp_start = Basics.CalculateSingleBodyPosition(ut_start, sweph.BodyNameToSweph(Body.Name.Sun), Body.Name.Sun, Body.Type.Graha, h);
		var sp_end   = Basics.CalculateSingleBodyPosition(ut_end, sweph.BodyNameToSweph(Body.Name.Sun), Body.Name.Sun, Body.Type.Graha, h);
		var lDiff    = sp_end.longitude.sub(sp_start.longitude);
		var diff     = lDiff.value;
		if (diff < 120.0)
		{
			diff += 360.0;
		}

		DasaOptions.YearType   = ToDate.DateType.SolarYear;
		DasaOptions.YearLength = diff;
		sweph.releaseLock(h);
		DasaOptions.Compression = 1;
		Reset();
	}

	private void mCompressedTithiPraveshaFixed_Click(object sender, EventArgs e)
	{
		var td_pravesh = new ToDate(h.baseUT, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
		sweph.obtainLock(h);
		DasaOptions.YearType   = ToDate.DateType.FixedYear;
		DasaOptions.YearLength = td_pravesh.AddYears(1).toUniversalTime() - td_pravesh.AddYears(0).toUniversalTime();
		sweph.releaseLock(h);
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
		dasaItemList.Items.Clear();
		var al = ((DasaEntriesWrapper) a).Entries;
		sweph.obtainLock(h);
		for (var i = 0; i < al.Length; i++)
		{
			var di = new DasaItem(al[i]);
			di.populateListViewItemMembers(td, id);
			dasaItemList.Items.Add(di);
		}

		sweph.releaseLock(h);
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
		var al    = new DasaEntry[dasaItemList.Items.Count];
		var am    = new DasaItem[dasaItemList.Items.Count];
		var start = 0.0;
		if (dasaItemList.Items.Count >= 1)
		{
			start = ((DasaItem) dasaItemList.Items[0]).entry.startUT;
		}

		for (var i = 0; i < dasaItemList.Items.Count; i++)
		{
			var di = (DasaItem) dasaItemList.Items[i];
			al[i] = di.entry;
			if (al[i].level == 1)
			{
				al[i].startUT =  start;
				start         += al[i].dasaLength;
			}

			am[i] = new DasaItem(al[i]);
		}

		dasaItemList.Items.Clear();
		sweph.obtainLock(h);
		for (var i = 0; i < am.Length; i++)
		{
			am[i].populateListViewItemMembers(td, id);
			dasaItemList.Items.Add(am[i]);
		}

		sweph.releaseLock(h);
	}

	private void mDasaDown_Click(object sender, EventArgs e)
	{
		if (dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var h2 = (Horoscope) h.Clone();
		var di = (DasaItem) dasaItemList.SelectedItems[0];

		var m = td.AddYears(di.entry.startUT);
		h2.info.tob = m;

		var mchild = (MhoraChild) ParentForm;
		var mcont  = (MainForm) ParentForm.ParentForm;

		mcont.AddChild(h2, mchild.Name + ": Dasa Entry Chart - (((" + di.entry.shortDesc + "))) " + id.Description());
	}

	private void mEditDasas_Click(object sender, EventArgs e)
	{
		var al = new DasaEntry[dasaItemList.Items.Count];
		for (var i = 0; i < dasaItemList.Items.Count; i++)
		{
			var di = (DasaItem) dasaItemList.Items[i];
			al[i] = di.entry;
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


	private void ExpandEvent(Moment m, int levels, string eventDesc)
	{
		var ut_m = m.toUniversalTime(h);
		for (var i = 0; i < dasaItemList.Items.Count; i++)
		{
			var di = (DasaItem) dasaItemList.Items[i];

			sweph.obtainLock(h);
			var m_start = td.AddYears(di.entry.startUT);
			var m_end   = td.AddYears(di.entry.startUT + di.entry.dasaLength);
			sweph.releaseLock(h);


			var ut_start = m_start.toUniversalTime(h);
			var ut_end   = m_end.toUniversalTime(h);


			if (ut_m >= ut_start && ut_m < ut_end)
			{
				Application.Log.Debug("Found: Looking for {0} between {1} and {2}", m, m_start, m_end);

				if (levels > di.entry.level)
				{
					if (i == dasaItemList.Items.Count - 1)
					{
						dasaItemList.SelectedItems.Clear();
						dasaItemList.Items[i].Selected = true;
						SplitDasa((DasaItem) dasaItemList.Items[i]);
					}

					if (i < dasaItemList.Items.Count - 1)
					{
						var di_next = (DasaItem) dasaItemList.Items[i + 1];
						if (di_next.entry.level == di.entry.level)
						{
							dasaItemList.SelectedItems.Clear();
							dasaItemList.Items[i].Selected = true;
							SplitDasa((DasaItem) dasaItemList.Items[i]);
						}
					}
				}
				else if (levels == di.entry.level)
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

	public object LocateEvent(object _euo)
	{
		var euo = (EventUserOptions) _euo;
		mEventOptionsCache = euo;
		ExpandEvent(euo.EventDate, euo.Depth, "Event: " + euo.EventDate);
		return _euo;
	}

	public void LocateChartEvents()
	{
		if (mShowEvents.Checked == false)
		{
			return;
		}

		foreach (var ue in h.info.Events)
		{
			if (ue.WorkWithEvent)
			{
				ExpandEvent(ue.EventTime, MhoraGlobalOptions.Instance.DasaEventsLevel, ue.ToString());
			}
		}
	}

	private void mLocateEvent_Click(object sender, EventArgs e)
	{
		if (mEventOptionsCache == null)
		{
			var dtNow = DateTime.Now;
			mEventOptionsCache = new EventUserOptions(new Moment(dtNow.Year, dtNow.Month, dtNow.Day, dtNow.Hour + dtNow.Minute / 60.0 + dtNow.Second / 3600.0));
		}

		new MhoraOptions(mEventOptionsCache, LocateEvent).ShowDialog();
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

			id.DivisionChanged(div);
		}
	}

	private void dasaInfo_Click(object sender, EventArgs e)
	{
	}

	private void mShowEvents_Click(object sender, EventArgs e)
	{
		mShowEvents.Checked = !mShowEvents.Checked;
	}

	private void m3Parts_Click(object sender, EventArgs e)
	{
		if (dasaItemList.SelectedItems.Count == 0)
		{
			return;
		}

		var di = (DasaItem) dasaItemList.SelectedItems[0];
		var de = di.entry;

		var form = new Dasa3Parts(h, de, td);
		form.Show();
	}

	private class DasaEntriesWrapper
	{
		public DasaEntriesWrapper(DasaEntry[] _al)
		{
			Entries = _al;
		}

		public DasaEntry[] Entries
		{
			get;
		}
	}


	private class EventUserOptions : ICloneable
	{
		public EventUserOptions(Moment _m)
		{
			EventDate = (Moment) _m.Clone();
			Depth     = 2;
		}

		public Moment EventDate
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