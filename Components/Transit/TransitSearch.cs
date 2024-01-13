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
using Mhora.Components.Property;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Elements.Hora;
using Mhora.SwissEph;
using Mhora.Tables;
using Mhora.Util;
using CuspTransitSearch = Mhora.Elements.CuspTransitSearch;
using Retrogression = Mhora.Elements.Retrogression;

namespace Mhora.Components.Transit;

public class TransitSearch : MhoraControl
{
	private readonly IContainer components = null;

	private readonly TransitSearchOptions opts;
	private          Button               bClearResults;
	private          Button               bGlobalLunarEclipse;
	private          Button               bGlobSolEcl;
	private          Button               bLocalSolarEclipse;
	private          Button               bNow;
	private          Button               bProgression;
	private          Button               bProgressionLon;
	private          Button               bRetroCusp;
	private          Button               bSolarNewYear;
	private          Button               bStartSearch;
	private          Button               bTransitNextVarga;
	private          Button               bTransitPrevVarga;
	private          Button               bVargaChange;
	private          ColumnHeader         Date;
	private          ColumnHeader         Event;
	private          GroupBox             groupBox1;
	private          GroupBox             groupBox2;
	private          ContextMenu          mContext;
	private          ListView             mlTransits;
	private          ColumnHeader         Moment;
	private          MenuItem             mOpenTransit;
	private          MenuItem             mOpenTransitChartPrev;
	private          MenuItem             mOpenTransitNext;
	private          ColumnHeader         Name1;
	private          Panel                panel1;
	public           PropertyGrid         pgOptions;

	public TransitSearch(Horoscope _h)
	{
		// This call is required by the Windows Form Designer.
		InitializeComponent();

		h                                      =  _h;
		MhoraGlobalOptions.DisplayPrefsChanged += Redisplay;
		opts                                   =  new TransitSearchOptions();
		updateOptions();
		AddViewsToContextMenu(mContext);

		ToolTip tt = null;

		tt = new ToolTip();
		tt.SetToolTip(bTransitPrevVarga, "Find when the graha goes to the previous rasi only");
		tt = new ToolTip();
		tt.SetToolTip(bTransitNextVarga, "Find when the graha goes to the next rasi only");
		tt = new ToolTip();
		tt.SetToolTip(bVargaChange, "Find when the graha changes rasis");

		// TODO: Add any initialization after the InitializeComponent call
	}

	public void Redisplay(object o)
	{
		mlTransits.Font      = MhoraGlobalOptions.Instance.GeneralFont;
		mlTransits.BackColor = MhoraGlobalOptions.Instance.TableBackgroundColor;
		mlTransits.ForeColor = MhoraGlobalOptions.Instance.TableForegroundColor;
		pgOptions.Font       = MhoraGlobalOptions.Instance.GeneralFont;
	}

	public void Reset()
	{
		updateOptions();
		mlTransits.Items.Clear();
		Redisplay(MhoraGlobalOptions.Instance);

		//this.mlTransits.Font = MhoraGlobalOptions.Instance.GeneralFont;
		//this.mlTransits.BackColor = MhoraGlobalOptions.Instance.DasaBackgroundColor;
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

#region Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.Name1                 = new System.Windows.Forms.ColumnHeader();
		this.Moment                = new System.Windows.Forms.ColumnHeader();
		this.mContext              = new System.Windows.Forms.ContextMenu();
		this.mOpenTransit          = new System.Windows.Forms.MenuItem();
		this.pgOptions             = new System.Windows.Forms.PropertyGrid();
		this.mlTransits            = new System.Windows.Forms.ListView();
		this.Event                 = new System.Windows.Forms.ColumnHeader();
		this.Date                  = new System.Windows.Forms.ColumnHeader();
		this.bLocalSolarEclipse    = new System.Windows.Forms.Button();
		this.bSolarNewYear         = new System.Windows.Forms.Button();
		this.bProgressionLon       = new System.Windows.Forms.Button();
		this.bClearResults         = new System.Windows.Forms.Button();
		this.bNow                  = new System.Windows.Forms.Button();
		this.bStartSearch          = new System.Windows.Forms.Button();
		this.bRetroCusp            = new System.Windows.Forms.Button();
		this.bProgression          = new System.Windows.Forms.Button();
		this.groupBox1             = new System.Windows.Forms.GroupBox();
		this.bTransitPrevVarga     = new System.Windows.Forms.Button();
		this.bVargaChange          = new System.Windows.Forms.Button();
		this.bTransitNextVarga     = new System.Windows.Forms.Button();
		this.bGlobSolEcl           = new System.Windows.Forms.Button();
		this.panel1                = new System.Windows.Forms.Panel();
		this.groupBox2             = new System.Windows.Forms.GroupBox();
		this.bGlobalLunarEclipse   = new System.Windows.Forms.Button();
		this.mOpenTransitNext      = new System.Windows.Forms.MenuItem();
		this.mOpenTransitChartPrev = new System.Windows.Forms.MenuItem();
		this.groupBox1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.SuspendLayout();
		// 
		// Name1
		// 
		this.Name1.Text  = "Type";
		this.Name1.Width = 269;
		// 
		// Moment
		// 
		this.Moment.Text  = "Moment";
		this.Moment.Width = 188;
		// 
		// mContext
		// 
		this.mContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.mOpenTransit,
			this.mOpenTransitNext,
			this.mOpenTransitChartPrev
		});
		this.mContext.Popup += new System.EventHandler(this.mContext_Popup);
		// 
		// mOpenTransit
		// 
		this.mOpenTransit.Index =  0;
		this.mOpenTransit.Text  =  "Open Transit Chart";
		this.mOpenTransit.Click += new System.EventHandler(this.mOpenTransit_Click);
		// 
		// pgOptions
		// 
		this.pgOptions.CommandsVisibleIfAvailable =  true;
		this.pgOptions.HelpVisible                =  false;
		this.pgOptions.LargeButtons               =  false;
		this.pgOptions.LineColor                  =  System.Drawing.SystemColors.ScrollBar;
		this.pgOptions.Location                   =  new System.Drawing.Point(16, 8);
		this.pgOptions.Name                       =  "pgOptions";
		this.pgOptions.PropertySort               =  System.Windows.Forms.PropertySort.Categorized;
		this.pgOptions.Size                       =  new System.Drawing.Size(296, 152);
		this.pgOptions.TabIndex                   =  5;
		this.pgOptions.Text                       =  "Options";
		this.pgOptions.ToolbarVisible             =  false;
		this.pgOptions.ViewBackColor              =  System.Drawing.SystemColors.Window;
		this.pgOptions.ViewForeColor              =  System.Drawing.SystemColors.WindowText;
		this.pgOptions.Click                      += new System.EventHandler(this.pGrid_Click);
		// 
		// mlTransits
		// 
		this.mlTransits.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.mlTransits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this.Event,
			this.Date
		});
		this.mlTransits.FullRowSelect        =  true;
		this.mlTransits.Location             =  new System.Drawing.Point(16, 168);
		this.mlTransits.Name                 =  "mlTransits";
		this.mlTransits.Size                 =  new System.Drawing.Size(648, 208);
		this.mlTransits.TabIndex             =  9;
		this.mlTransits.View                 =  System.Windows.Forms.View.Details;
		this.mlTransits.SelectedIndexChanged += new System.EventHandler(this.mlTransits_SelectedIndexChanged_1);
		// 
		// Event
		// 
		this.Event.Text  = "Event";
		this.Event.Width = 387;
		// 
		// Date
		// 
		this.Date.Text  = "Date";
		this.Date.Width = 173;
		// 
		// bLocalSolarEclipse
		// 
		this.bLocalSolarEclipse.Location =  new System.Drawing.Point(8, 40);
		this.bLocalSolarEclipse.Name     =  "bLocalSolarEclipse";
		this.bLocalSolarEclipse.Size     =  new System.Drawing.Size(104, 23);
		this.bLocalSolarEclipse.TabIndex =  13;
		this.bLocalSolarEclipse.Text     =  "(L) Sol. Ecl.";
		this.bLocalSolarEclipse.Click    += new System.EventHandler(this.bLocSolEclipse_Click);
		// 
		// bSolarNewYear
		// 
		this.bSolarNewYear.Location =  new System.Drawing.Point(152, 168);
		this.bSolarNewYear.Name     =  "bSolarNewYear";
		this.bSolarNewYear.Size     =  new System.Drawing.Size(104, 23);
		this.bSolarNewYear.TabIndex =  9;
		this.bSolarNewYear.Text     =  "Solar Year";
		this.bSolarNewYear.Click    += new System.EventHandler(this.bSolarNewYear_Click);
		// 
		// bProgressionLon
		// 
		this.bProgressionLon.Location =  new System.Drawing.Point(152, 136);
		this.bProgressionLon.Name     =  "bProgressionLon";
		this.bProgressionLon.Size     =  new System.Drawing.Size(104, 23);
		this.bProgressionLon.TabIndex =  8;
		this.bProgressionLon.Text     =  "Progress Lons";
		this.bProgressionLon.Click    += new System.EventHandler(this.bProgressionLon_Click);
		// 
		// bClearResults
		// 
		this.bClearResults.Location =  new System.Drawing.Point(16, 16);
		this.bClearResults.Name     =  "bClearResults";
		this.bClearResults.Size     =  new System.Drawing.Size(104, 23);
		this.bClearResults.TabIndex =  3;
		this.bClearResults.Text     =  "Clear Results";
		this.bClearResults.Click    += new System.EventHandler(this.bClearResults_Click);
		// 
		// bNow
		// 
		this.bNow.Location =  new System.Drawing.Point(16, 40);
		this.bNow.Name     =  "bNow";
		this.bNow.Size     =  new System.Drawing.Size(104, 23);
		this.bNow.TabIndex =  7;
		this.bNow.Text     =  "Now";
		this.bNow.Click    += new System.EventHandler(this.bNow_Click);
		// 
		// bStartSearch
		// 
		this.bStartSearch.Location    =  new System.Drawing.Point(16, 72);
		this.bStartSearch.Name        =  "bStartSearch";
		this.bStartSearch.RightToLeft =  System.Windows.Forms.RightToLeft.No;
		this.bStartSearch.Size        =  new System.Drawing.Size(104, 23);
		this.bStartSearch.TabIndex    =  2;
		this.bStartSearch.Text        =  "Find Transit";
		this.bStartSearch.Click       += new System.EventHandler(this.bStartSearch_Click);
		// 
		// bRetroCusp
		// 
		this.bRetroCusp.Location    =  new System.Drawing.Point(16, 96);
		this.bRetroCusp.Name        =  "bRetroCusp";
		this.bRetroCusp.RightToLeft =  System.Windows.Forms.RightToLeft.No;
		this.bRetroCusp.Size        =  new System.Drawing.Size(104, 23);
		this.bRetroCusp.TabIndex    =  4;
		this.bRetroCusp.Text        =  "Find Retro";
		this.bRetroCusp.Click       += new System.EventHandler(this.bRetroCusp_Click);
		// 
		// bProgression
		// 
		this.bProgression.Location =  new System.Drawing.Point(152, 112);
		this.bProgression.Name     =  "bProgression";
		this.bProgression.Size     =  new System.Drawing.Size(104, 23);
		this.bProgression.TabIndex =  6;
		this.bProgression.Text     =  "Progress Time";
		this.bProgression.Click    += new System.EventHandler(this.bProgression_Click);
		// 
		// groupBox1
		// 
		this.groupBox1.Controls.Add(this.bTransitPrevVarga);
		this.groupBox1.Controls.Add(this.bVargaChange);
		this.groupBox1.Controls.Add(this.bTransitNextVarga);
		this.groupBox1.Location = new System.Drawing.Point(144, 8);
		this.groupBox1.Name     = "groupBox1";
		this.groupBox1.Size     = new System.Drawing.Size(120, 96);
		this.groupBox1.TabIndex = 12;
		this.groupBox1.TabStop  = false;
		this.groupBox1.Text     = "Change Vargas";
		// 
		// bTransitPrevVarga
		// 
		this.bTransitPrevVarga.Location  =  new System.Drawing.Point(8, 16);
		this.bTransitPrevVarga.Name      =  "bTransitPrevVarga";
		this.bTransitPrevVarga.Size      =  new System.Drawing.Size(104, 23);
		this.bTransitPrevVarga.TabIndex  =  12;
		this.bTransitPrevVarga.Text      =  "<-- Prev";
		this.bTransitPrevVarga.TextAlign =  System.Drawing.ContentAlignment.MiddleLeft;
		this.bTransitPrevVarga.Click     += new System.EventHandler(this.bTransitPrevVarga_Click);
		// 
		// bVargaChange
		// 
		this.bVargaChange.Location =  new System.Drawing.Point(8, 40);
		this.bVargaChange.Name     =  "bVargaChange";
		this.bVargaChange.Size     =  new System.Drawing.Size(104, 23);
		this.bVargaChange.TabIndex =  13;
		this.bVargaChange.Text     =  "Change";
		this.bVargaChange.Click    += new System.EventHandler(this.bVargaChange_Click);
		// 
		// bTransitNextVarga
		// 
		this.bTransitNextVarga.Location  =  new System.Drawing.Point(8, 64);
		this.bTransitNextVarga.Name      =  "bTransitNextVarga";
		this.bTransitNextVarga.Size      =  new System.Drawing.Size(104, 23);
		this.bTransitNextVarga.TabIndex  =  10;
		this.bTransitNextVarga.Text      =  "Next  -->";
		this.bTransitNextVarga.TextAlign =  System.Drawing.ContentAlignment.MiddleRight;
		this.bTransitNextVarga.Click     += new System.EventHandler(this.bTransitNextVarga_Click);
		// 
		// bGlobSolEcl
		// 
		this.bGlobSolEcl.Location =  new System.Drawing.Point(8, 64);
		this.bGlobSolEcl.Name     =  "bGlobSolEcl";
		this.bGlobSolEcl.Size     =  new System.Drawing.Size(104, 23);
		this.bGlobSolEcl.TabIndex =  11;
		this.bGlobSolEcl.Text     =  "(G) Sol. Ecl.";
		this.bGlobSolEcl.Click    += new System.EventHandler(this.bGlobSolEclipse_Click);
		// 
		// panel1
		// 
		this.panel1.Anchor      = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.panel1.AutoScroll  = true;
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.bSolarNewYear);
		this.panel1.Controls.Add(this.bProgressionLon);
		this.panel1.Controls.Add(this.bClearResults);
		this.panel1.Controls.Add(this.bNow);
		this.panel1.Controls.Add(this.bStartSearch);
		this.panel1.Controls.Add(this.bRetroCusp);
		this.panel1.Controls.Add(this.bProgression);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Location =  new System.Drawing.Point(320, 8);
		this.panel1.Name     =  "panel1";
		this.panel1.Size     =  new System.Drawing.Size(304, 152);
		this.panel1.TabIndex =  8;
		this.panel1.Paint    += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
		// 
		// groupBox2
		// 
		this.groupBox2.Controls.Add(this.bLocalSolarEclipse);
		this.groupBox2.Controls.Add(this.bGlobSolEcl);
		this.groupBox2.Controls.Add(this.bGlobalLunarEclipse);
		this.groupBox2.Location = new System.Drawing.Point(8, 128);
		this.groupBox2.Name     = "groupBox2";
		this.groupBox2.Size     = new System.Drawing.Size(120, 96);
		this.groupBox2.TabIndex = 15;
		this.groupBox2.TabStop  = false;
		this.groupBox2.Text     = "Eclipses";
		// 
		// bGlobalLunarEclipse
		// 
		this.bGlobalLunarEclipse.Location =  new System.Drawing.Point(8, 16);
		this.bGlobalLunarEclipse.Name     =  "bGlobalLunarEclipse";
		this.bGlobalLunarEclipse.Size     =  new System.Drawing.Size(104, 23);
		this.bGlobalLunarEclipse.TabIndex =  14;
		this.bGlobalLunarEclipse.Text     =  "Lunar Ecl.";
		this.bGlobalLunarEclipse.Click    += new System.EventHandler(this.bGlobalLunarEclipse_Click);
		// 
		// mOpenTransitNext
		// 
		this.mOpenTransitNext.Index =  1;
		this.mOpenTransitNext.Text  =  "Open Transit Chart (Compress &Next)";
		this.mOpenTransitNext.Click += new System.EventHandler(this.mOpenTransitNext_Click);
		// 
		// mOpenTransitChartPrev
		// 
		this.mOpenTransitChartPrev.Index =  2;
		this.mOpenTransitChartPrev.Text  =  "Open Transit Chart (Compress &Prev)";
		this.mOpenTransitChartPrev.Click += new System.EventHandler(this.mOpenTransitChartPrev_Click);
		// 
		// TransitSearch
		// 
		this.ContextMenu = this.mContext;
		this.Controls.Add(this.pgOptions);
		this.Controls.Add(this.panel1);
		this.Controls.Add(this.mlTransits);
		this.Name =  "TransitSearch";
		this.Size =  new System.Drawing.Size(672, 384);
		this.Load += new System.EventHandler(this.TransitSearch_Load);
		this.groupBox1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.ResumeLayout(false);
	}

#endregion

	private void TransitSearch_Load(object sender, EventArgs e)
	{
		Reset();
	}

	public object SetOptions(object o)
	{
		return opts.CopyFrom(o);
	}

	private void bOpts_Click(object sender, EventArgs e)
	{
		var f = new MhoraOptions(opts, SetOptions);
		f.ShowDialog();
	}


	private Horoscope utToHoroscope(double found_ut, ref Moment m2)
	{
		// turn into horoscope
		int    year = 0, month = 0, day = 0;
		double hour = 0;
		found_ut += h.info.Timezone.toDouble() / 24.0;
		sweph.RevJul(found_ut, ref year, ref month, ref day, ref hour);
		var m        = new Moment(year, month, day, hour);
		var inf      = new HoraInfo(m, (HMSInfo) h.info.Latitude.Clone(), (HMSInfo) h.info.Longitude.Clone(), (HMSInfo) h.info.Timezone.Clone());
		var hTransit = new Horoscope(inf, (HoroscopeOptions) h.options.Clone());

		sweph.RevJul(found_ut + 5.0, ref year, ref month, ref day, ref hour);
		m2 = new Moment(year, month, day, hour);
		return hTransit;
	}

	private void ApplyLocal(Moment m)
	{
		if (opts.Apply)
		{
			h.info.tob = m;
			h.OnChanged();
		}
	}

	private double DirectSpeed(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:   return 365.2425;
			case Body.BodyType.Moon:  return 28.0;
			case Body.BodyType.Lagna: return 1.0;
		}

		return 0.0;
	}

	private void DirectProgression()
	{
		if (opts.SearchBody != Body.BodyType.Sun && opts.SearchBody != Body.BodyType.Moon) // &&
			//opts.SearchBody != Body.Type.Lagna)
		{
			return;
		}

		var julday_ut = opts.StartDate.toUniversalTime() - h.info.Timezone.toDouble() / 24.0;
		//;.tob.time / 24.0;

		if (julday_ut <= h.baseUT)
		{
			MessageBox.Show("Error: Unable to progress in the future");
			return;
		}

		var totalProgression     = GetProgressionDegree();
		var totalProgressionOrig = totalProgression;

		sweph.obtainLock(h);
		var r = new Retrogression(h, opts.SearchBody);

		var start_lon = r.GetLon(h.baseUT);
		//mhora.Log.Debug ("Real start lon is {0}", start_lon);
		var curr_julday = h.baseUT;
		var t           = new Elements.Transit(h, opts.SearchBody);
		while (totalProgression >= 360.0)
		{
			curr_julday      =  t.LinearSearch(curr_julday + DirectSpeed(opts.SearchBody), start_lon, t.GenericLongitude);
			totalProgression -= 360.0;
		}

		curr_julday = t.LinearSearch(curr_julday + totalProgression / 360.0 * DirectSpeed(opts.SearchBody), start_lon.add(totalProgression), t.GenericLongitude);


		//bool bDiscard = true;
		//Longitude got_lon = t.GenericLongitude(curr_julday, ref bDiscard);
		//mhora.Log.Debug ("Found Progressed Sun at {0}+{1}={2}={3}", 
		//	start_lon.value, new Longitude(totalProgressionOrig).value,
		//	got_lon.value, got_lon.sub(start_lon.add(totalProgressionOrig)).value
		//	);

		sweph.releaseLock(h);

		var          m2       = new Moment(0, 0, 0, 0.0);
		var          hTransit = utToHoroscope(curr_julday, ref m2);
		var          fmt      = hTransit.info.DateOfBirth.ToString();
		ListViewItem li       = new TransitItem(hTransit);
		li.Text = string.Format("{0}'s Prog: {2}+{3:00.00} deg", opts.SearchBody, totalProgressionOrig, (int) Math.Floor(totalProgressionOrig / 360.0), new Longitude(totalProgressionOrig).value);
		li.SubItems.Add(fmt);
		ApplyLocal(hTransit.info.tob);

		mlTransits.Items.Add(li);
		updateOptions();
	}

	private double GetProgressionDegree()
	{
		var julday_ut = opts.StartDate.toUniversalTime() - h.info.Timezone.toDouble() / 24.0;
		var ut_diff   = julday_ut                        - h.baseUT;

		//mhora.Log.Debug ("Expected ut_diff is {0}", ut_diff);
		var bDummy = true;
		sweph.obtainLock(h);
		var t         = new Elements.Transit(h);
		var lon_start = t.LongitudeOfSun(h.baseUT, ref bDummy);
		var lon_prog  = t.LongitudeOfSun(julday_ut, ref bDummy);

		//mhora.Log.Debug ("Progression lons are {0} and {1}", lon_start, lon_prog);

		var dExpectedLon = ut_diff * 360.0 / 365.2425;
		var lon_expected = lon_start.add(dExpectedLon);
		sweph.releaseLock(h);


		if (Elements.Transit.CircLonLessThan(lon_expected, lon_prog))
		{
			dExpectedLon += lon_prog.sub(lon_expected).value;
		}
		else
		{
			dExpectedLon -= lon_expected.sub(lon_prog).value;
		}

		var dp = h.getPosition(opts.SearchBody).toDivisionPosition(opts.Division);

		//mhora.Log.Debug ("Sun progress {0} degrees in elapsed time", dExpectedLon);

		var ret = dExpectedLon / 360.0 * (30.0 / opts.Division.NumPartsInDivision());
		//(dp.cusp_higher - dp.cusp_lower);
		//mhora.Log.Debug ("Progressing by {0} degrees", ret);
		return ret;
	}

	private void bProgression_Click(object sender, EventArgs e)
	{
		if ((int) opts.SearchBody <= (int) Body.BodyType.Moon || (int) opts.SearchBody > (int) Body.BodyType.Saturn)
		{
			DirectProgression();
			return;
		}

		var dp                = h.getPosition(opts.SearchBody).toDivisionPosition(opts.Division);
		var yearlyProgression = (dp.cusp_higher - dp.cusp_lower) / 30.0;
		var julday_ut         = sweph.JulDay(opts.StartDate.year, opts.StartDate.month, opts.StartDate.day, opts.StartDate.hour + opts.StartDate.minute / 60.0 + opts.StartDate.second / 3600.0);

		if (julday_ut <= h.baseUT)
		{
			MessageBox.Show("Error: Unable to progress in the future");
			return;
		}


		var totalProgression     = GetProgressionDegree();
		var totalProgressionOrig = totalProgression;

		//mhora.Log.Debug ("Total Progression is {0}", totalProgression);
		var becomesDirect = false;
		sweph.obtainLock(h);
		var    r        = new Retrogression(h, opts.SearchBody);
		var    curr_ut  = h.baseUT;
		double next_ut  = 0;
		var    found_ut = h.baseUT;
		while (true)
		{
			next_ut = r.findNextCuspForward(curr_ut, ref becomesDirect);
			var curr_lon = r.GetLon(curr_ut);
			var next_lon = r.GetLon(next_ut);


			if (false == becomesDirect && next_lon.sub(curr_lon).value >= totalProgression)
			{
				//mhora.Log.Debug ("1 Found {0} in {1}", totalProgression, next_lon.sub(curr_lon).value);
				found_ut = r.GetTransitForward(curr_ut, curr_lon.add(totalProgression));
				break;
			}

			if (becomesDirect && curr_lon.sub(next_lon).value >= totalProgression)
			{
				//mhora.Log.Debug ("2 Found {0} in {1}", totalProgression, curr_lon.sub(next_lon).value);
				found_ut = r.GetTransitForward(curr_ut, curr_lon.sub(totalProgression));
				break;
			}

			if (false == becomesDirect)
			{
				//mhora.Log.Debug ("Progression: {0} degrees gone in direct motion", next_lon.sub(curr_lon).value);
				totalProgression -= next_lon.sub(curr_lon).value;
			}
			else
			{
				//mhora.Log.Debug ("Progression: {0} degrees gone in retro motion", curr_lon.sub(next_lon).value);
				totalProgression -= curr_lon.sub(next_lon).value;
			}

			curr_ut = next_ut + 5.0;
		}

		sweph.releaseLock(h);

		var          m2       = new Moment(0, 0, 0, 0.0);
		var          hTransit = utToHoroscope(found_ut, ref m2);
		var          fmt      = hTransit.info.DateOfBirth.ToString();
		ListViewItem li       = new TransitItem(hTransit);
		li.Text = string.Format("{0}'s Prog: {2}+{3:00.00} deg", opts.SearchBody, totalProgressionOrig, (int) Math.Floor(totalProgressionOrig / 360.0), new Longitude(totalProgressionOrig).value);
		li.SubItems.Add(fmt);
		mlTransits.Items.Add(li);
		updateOptions();
		ApplyLocal(hTransit.info.tob);
	}

	private void bProgressionLon_Click(object sender, EventArgs e)
	{
		if (opts.Apply == false)
		{
			MessageBox.Show("This will modify the current chart. You must set Apply to 'true'");
			return;
		}

		h.OnChanged();
		var degToProgress = GetProgressionDegree();
		var lonProgress   = new Longitude(degToProgress);

		foreach (Position bp in h.positionList)
		{
			bp.longitude = bp.longitude.add(lonProgress);
		}

		h.OnlySignalChanged();
	}

	private void bRetroCusp_Click(object sender, EventArgs e)
	{
		if ((int) opts.SearchBody <= (int) Body.BodyType.Moon || (int) opts.SearchBody > (int) Body.BodyType.Saturn)
		{
			return;
		}

		var becomesDirect = false;
		sweph.obtainLock(h);
		var r         = new Retrogression(h, opts.SearchBody);
		var julday_ut = opts.StartDate.toUniversalTime() - h.info.Timezone.toDouble() / 24.0;
		var found_ut  = julday_ut;
		if (opts.Forward)
		{
			found_ut = r.findNextCuspForward(julday_ut, ref becomesDirect);
		}
		else
		{
			found_ut = r.findNextCuspBackward(julday_ut, ref becomesDirect);
		}


		var bForward  = false;
		var found_lon = r.GetLon(found_ut, ref bForward);
		sweph.releaseLock(h);

		// turn into horoscope
		int    year = 0, month = 0, day = 0;
		double hour = 0;
		found_ut += h.info.Timezone.toDouble() / 24.0;
		sweph.RevJul(found_ut, ref year, ref month, ref day, ref hour);
		var m        = new Moment(year, month, day, hour);
		var inf      = new HoraInfo(m, (HMSInfo) h.info.Latitude.Clone(), (HMSInfo) h.info.Longitude.Clone(), (HMSInfo) h.info.Timezone.Clone());
		var hTransit = new Horoscope(inf, (HoroscopeOptions) h.options.Clone());

		if (opts.Forward)
		{
			sweph.RevJul(found_ut + 5.0, ref year, ref month, ref day, ref hour);
		}
		else
		{
			sweph.RevJul(found_ut - 5.0, ref year, ref month, ref day, ref hour);
		}

		var m2 = new Moment(year, month, day, hour);
		opts.StartDate = m2;
		// add entry to our list
		var          fmt = hTransit.info.DateOfBirth.ToString();
		ListViewItem li  = new TransitItem(hTransit);
		li.Text = opts.SearchBody.Name();
		if (becomesDirect)
		{
			li.Text += " goes direct at " + found_lon;
		}
		else
		{
			li.Text += " goes retrograde at " + found_lon;
		}

		li.SubItems.Add(fmt);
		mlTransits.Items.Add(li);
		updateOptions();
		ApplyLocal(hTransit.info.tob);
	}

	private void bStartSearch_Click(object sender, EventArgs e)
	{
		StartSearch(true);
	}

	private void UpdateDateForNextSearch(double ut)
	{
		int    year = 0, month = 0, day = 0;
		double hour = 0;

		var offset = 10.0 / (24.0 * 60.0 * 60.0);
		if (opts.Forward)
		{
			ut += offset;
		}
		else
		{
			ut -= offset;
		}


		sweph.RevJul(ut, ref year, ref month, ref day, ref hour);
		var m2 = new Moment(year, month, day, hour);
		opts.StartDate = m2;
		updateOptions();
	}


	private double StartSearch(bool bUpdateDate)
	{
		var found_lon = opts.TransitPoint.add(0);
		var bForward  = true;
		var found_ut  = h.TransitSearch(opts.SearchBody, opts.StartDate, opts.Forward, opts.TransitPoint, found_lon, ref bForward);


		var m2       = new Moment(0, 0, 0, 0);
		var hTransit = utToHoroscope(found_ut, ref m2);
		UpdateDateForNextSearch(found_ut);

		// add entry to our list
		var          fmt = hTransit.info.DateOfBirth.ToString();
		ListViewItem li  = new TransitItem(hTransit);
		li.Text = opts.SearchBody.Name();
		if (bForward == false)
		{
			li.Text += " (R)";
		}

		li.Text += " transits " + found_lon;

		li.SubItems.Add(fmt);
		mlTransits.Items.Add(li);
		updateOptions();
		ApplyLocal(hTransit.info.tob);

		return found_ut;
	}


	private void mlTransits_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void mContext_Popup(object sender, EventArgs e)
	{
	}

	private void openTransitHelper(Horoscope hTransit)
	{
		hTransit.info.type = HoraInfo.ChartType.Transit;
		var mcTransit = new MhoraChild(hTransit);
		mcTransit.Name      = "Transit Chart";
		mcTransit.Text      = "Transit Chart";
		mcTransit.MdiParent = MhoraGlobalOptions.mainControl;
		mcTransit.Show();
	}

	private void mOpenTransit_Click(object sender, EventArgs e)
	{
		if (mlTransits.SelectedItems.Count == 0)
		{
			return;
		}

		var ti       = (TransitItem) mlTransits.SelectedItems[0];
		var hTransit = ti.GetHoroscope();
		openTransitHelper(hTransit);
	}


	private void mOpenTransitNext_Click(object sender, EventArgs e)
	{
		if (mlTransits.SelectedItems.Count == 0)
		{
			return;
		}

		var ti       = (TransitItem) mlTransits.SelectedItems[0];
		var hTransit = ti.GetHoroscope();

		var nextEntry = mlTransits.SelectedItems[0].Index + 1;
		if (mlTransits.Items.Count >= nextEntry + 1)
		{
			var tiNext       = (TransitItem) mlTransits.Items[nextEntry];
			var hTransitNext = tiNext.GetHoroscope();

			var ut_diff = hTransitNext.baseUT - hTransit.baseUT;
			if (ut_diff > 0)
			{
				hTransit.info.defaultYearCompression = 1;
				hTransit.info.defaultYearLength      = ut_diff;
				hTransit.info.defaultYearType        = ToDate.DateType.FixedYear;
			}
		}

		openTransitHelper(hTransit);
	}

	private void mOpenTransitChartPrev_Click(object sender, EventArgs e)
	{
		if (mlTransits.SelectedItems.Count == 0)
		{
			return;
		}

		var ti       = (TransitItem) mlTransits.SelectedItems[0];
		var hTransit = ti.GetHoroscope();
		hTransit.info.type = HoraInfo.ChartType.Transit;

		var prevEntry = mlTransits.SelectedItems[0].Index - 1;
		if (prevEntry >= 0)
		{
			var tiPrev       = (TransitItem) mlTransits.Items[prevEntry];
			var hTransitPrev = tiPrev.GetHoroscope();

			var ut_diff = hTransit.baseUT - hTransitPrev.baseUT;
			if (ut_diff > 0)
			{
				hTransit.info.defaultYearCompression = 1;
				hTransit.info.defaultYearLength      = ut_diff;
				hTransit.info.defaultYearType        = ToDate.DateType.FixedYear;
			}
		}

		openTransitHelper(hTransit);
	}


	private void cbGrahas_SelectedIndexChanged(object sender, EventArgs e)
	{
	}


	private void bClearResults_Click(object sender, EventArgs e)
	{
		mlTransits.Items.Clear();
	}

	private void pGrid_Click(object sender, EventArgs e)
	{
	}


	private void bNow_Click(object sender, EventArgs e)
	{
		var now = DateTime.Now;
		var m   = new Moment(now.Year, now.Month, now.Day, now.Hour + now.Minute / 60.0 + now.Second / 3600.0);
		opts.StartDate = m;
		updateOptions();
	}

	private void bSolarNewYear_Click(object sender, EventArgs e)
	{
		opts.SearchBody         = Body.BodyType.Sun;
		opts.TransitPoint.value = 0;
		updateOptions();
		bStartSearch_Click(sender, e);
	}

	private void bTransitPrevVarga_Click(object sender, EventArgs e)
	{
		var h2 = (Horoscope) h.Clone();
		h2.info.tob = opts.StartDate;
		h2.OnChanged();
		var dp = h2.getPosition(opts.SearchBody).toDivisionPosition(opts.Division);
		opts.TransitPoint = new Longitude(dp.cusp_lower);

		var found_ut = StartSearch(false) + h.info.Timezone.toDouble() / 24.0;
		UpdateDateForNextSearch(found_ut);
		updateOptions();
	}

	private void updateOptions()
	{
		pgOptions.SelectedObject = new GlobalizedPropertiesWrapper(opts);
	}

	private void bTransitNextVarga_Click(object sender, EventArgs e)
	{
		// Update Search Parameters
		var h2 = (Horoscope) h.Clone();
		h2.info.tob = opts.StartDate;
		h2.OnChanged();
		var dp = h2.getPosition(opts.SearchBody).toDivisionPosition(opts.Division);
		opts.TransitPoint = new Longitude(dp.cusp_higher);
		opts.TransitPoint = opts.TransitPoint.add(1.0 / (60.0 * 60.0 * 60.0));

		var found_ut = StartSearch(false) + h.info.Timezone.toDouble() / 24.0;
		UpdateDateForNextSearch(found_ut);
		updateOptions();
	}

	private void bVargaChange_Click(object sender, EventArgs e)
	{
		if (opts.SearchBody == Body.BodyType.Sun || opts.SearchBody == Body.BodyType.Moon || opts.SearchBody == Body.BodyType.Lagna)
		{
			if (opts.Forward)
			{
				bTransitNextVarga_Click(sender, e);
			}
			else
			{
				bTransitPrevVarga_Click(sender, e);
			}

			return;
		}

		var h2 = (Horoscope) h.Clone();
		h2.info.tob = opts.StartDate;
		h2.OnChanged();
		var bp = h2.getPosition(opts.SearchBody);
		var dp = bp.toDivisionPosition(opts.Division);

		var becomesDirect = false;
		var bForward      = false;
		sweph.obtainLock(h);
		var r                   = new Retrogression(h, opts.SearchBody);
		var julday_ut           = opts.StartDate.toUniversalTime() - h.info.Timezone.toDouble() / 24.0;
		var found_ut            = julday_ut;
		var bTransitForwardCusp = true;
		while (true)
		{
			if (opts.Forward)
			{
				found_ut = r.findNextCuspForward(found_ut, ref becomesDirect);
			}
			else
			{
				found_ut = r.findNextCuspBackward(found_ut, ref becomesDirect);
			}

			var found_lon = r.GetLon(found_ut, ref bForward);


			if (new Longitude(dp.cusp_higher).isBetween(bp.longitude, found_lon))
			{
				bTransitForwardCusp = true;
				break;
			}

			if (new Longitude(dp.cusp_lower).isBetween(found_lon, bp.longitude))
			{
				bTransitForwardCusp = false;
				break;
			}

			if (opts.Forward)
			{
				found_ut += 5.0;
			}
			else
			{
				found_ut -= 5.0;
			}
		}

		sweph.releaseLock(h);
		if (opts.Forward)
		{
			found_ut += 5.0;
		}
		else
		{
			found_ut -= 5.0;
		}

		UpdateDateForNextSearch(found_ut);

		if (bTransitForwardCusp)
		{
			opts.TransitPoint.value = dp.cusp_higher;
			updateOptions();
			bStartSearch_Click(sender, e);
		}
		else
		{
			opts.TransitPoint.value = dp.cusp_lower;
			updateOptions();
			bStartSearch_Click(sender, e);
		}
	}

	private void panel1_Paint(object sender, PaintEventArgs e)
	{
	}


	protected override void copyToClipboard()
	{
		var s = string.Empty;
		foreach (ListViewItem li in mlTransits.Items)
		{
			foreach (ListViewItem.ListViewSubItem si in li.SubItems)
			{
				s += si.Text + ". ";
			}

			s += "\r\n";
			Clipboard.SetDataObject(s, true);
		}
	}

	private void SolarEclipseHelper(double ut, string desc)
	{
		SolarEclipseHelper(ut, ut - 1, ut + 1, desc);
	}

	private void SolarEclipseHelper(double ut, double start, double end, string desc)
	{
		if (ut < start || ut > end)
		{
			return;
		}

		var li       = new ListViewItem(desc);
		var m        = new Moment(0, 0, 0, 0);
		var hTransit = utToHoroscope(ut, ref m);
		li.SubItems.Add(hTransit.info.tob.ToString());
		mlTransits.Items.Add(li);
	}

	private void bGlobSolEclipse_Click(object sender, EventArgs e)
	{
		var julday_ut = opts.StartDate.toUniversalTime(h);
		var tret      = new double[10];
		sweph.obtainLock(h);
		sweph.SolEclipseWhenGlob(julday_ut, tret, opts.Forward);
		sweph.releaseLock(h);
		SolarEclipseHelper(tret[2], "Global Solar Eclipse Begins");
		SolarEclipseHelper(tret[3], "   Global Solar Eclipse Ends");
		SolarEclipseHelper(tret[4], tret[2], tret[3], "   Global Solar Eclipse Totality Begins");
		SolarEclipseHelper(tret[5], tret[2], tret[3], "   Global Solar Eclipse Totality Ends");
		SolarEclipseHelper(tret[0], "   Global Solar Eclipse Maximum");
		SolarEclipseHelper(tret[6], tret[2], tret[3], "   Global Solar Eclipse Centerline Begins");
		SolarEclipseHelper(tret[7], tret[2], tret[3], "   Global Solar Eclipse Centerline Begins");
		if (opts.Forward)
		{
			opts.StartDate = new Moment(tret[3] + 1.0, h);
		}
		else
		{
			opts.StartDate = new Moment(tret[2] - 1.0, h);
		}

		updateOptions();
	}

	private void bLocSolEclipse_Click(object sender, EventArgs e)
	{
		var julday_ut = opts.StartDate.toUniversalTime(h);
		var tret      = new double[10];
		var attr      = new double[10];
		sweph.obtainLock(h);
		sweph.SolEclipseWhenLoc(h.info, julday_ut, tret, attr, opts.Forward);
		sweph.releaseLock(h);
		SolarEclipseHelper(tret[0], "Local Solar Eclipse Maximum");
		SolarEclipseHelper(tret[1], tret[0] - 1, tret[0] + 1, "   Local Solar Eclipse 1st Contact");
		SolarEclipseHelper(tret[2], tret[0] - 1, tret[0] + 1, "   Local Solar Eclipse 2nd Contact");
		SolarEclipseHelper(tret[3], tret[0] - 1, tret[0] + 1, "   Local Solar Eclipse 3rd Contact");
		SolarEclipseHelper(tret[4], tret[0] - 1, tret[0] + 1, "   Local Solar Eclipse 4th Contact");
		if (opts.Forward)
		{
			opts.StartDate = new Moment(tret[0] + 1.0, h);
		}
		else
		{
			opts.StartDate = new Moment(tret[0] - 1.0, h);
		}

		updateOptions();
	}

	private void bGlobalLunarEclipse_Click(object sender, EventArgs e)
	{
		var julday_ut = opts.StartDate.toUniversalTime(h);
		var tret      = new double[10];
		sweph.obtainLock(h);
		sweph.LunEclipseWhen(julday_ut, tret, opts.Forward);
		sweph.releaseLock(h);
		SolarEclipseHelper(tret[0], "Lunar Eclipse Maximum");
		SolarEclipseHelper(tret[2], tret[0] - 1, tret[0] + 1, "   Lunar Eclipse Begins");
		SolarEclipseHelper(tret[3], tret[0] - 1, tret[0] + 1, "   Lunar Eclipse Ends");
		SolarEclipseHelper(tret[4], tret[0] - 1, tret[0] + 1, "   Lunar Total Eclipse Begins");
		SolarEclipseHelper(tret[5], tret[0] - 1, tret[0] + 1, "   Lunar Total Eclipse Ends");
		SolarEclipseHelper(tret[6], tret[0] - 1, tret[0] + 1, "   Lunar Penumbral Eclipse Begins");
		SolarEclipseHelper(tret[7], tret[0] - 1, tret[0] + 1, "   Lunar Penumbral Eclipse Ends");
		if (opts.Forward)
		{
			opts.StartDate = new Moment(tret[0] + 1.0, h);
		}
		else
		{
			opts.StartDate = new Moment(tret[0] - 1.0, h);
		}

		updateOptions();
	}

	private void mlTransits_SelectedIndexChanged_1(object sender, EventArgs e)
	{
	}
}