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
using System.Drawing;
using System.Windows.Forms;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;
using Mhora.Tables;
using CuspTransitSearch = Mhora.Elements.CuspTransitSearch;

namespace Mhora.Components.Varga;

/// <summary>
///     Summary description for VargaRectificationForm.
/// </summary>
public class VargaRectificationForm : Form
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;

	private readonly Division          dtypeRasi = new(Vargas.DivisionType.Rasi);

	private readonly Horoscope h;
	private readonly int       half_tick_height = 3;
	private readonly Body.BodyType mBody            = Body.BodyType.Lagna;
	private readonly Moment    mOriginal;
	private readonly int       unit_height = 30;

	private readonly int                  vname_width = 50;
	private          Bitmap               bmpBuffer;
	private          DivisionalChart      dc;
	private          ContextMenu          mContext;
	private          MenuItem             menuCenter;
	private          MenuItem             menuCopyToClipboard;
	private          MenuItem             menuDasavargas;
	private          MenuItem             menuDisplaySeconds;
	private          MenuItem             menuDouble;
	private          MenuItem             menuHalve;
	private          MenuItem             menuItem1;
	private          MenuItem             menuNadiamsavargas;
	private          MenuItem             menuOptions;
	private          MenuItem             menuReset;
	private          MenuItem             menuSaptavargas;
	private          MenuItem             menuShadvargas;
	private          MenuItem             menuShodasavargas;
	private          double[][]           momentCusps;
	private          UserOptions          opts;
	private          double               ut_higher;
	private          double               ut_lower;
	private          ZodiacHouse.Rasi[][] zhCusps;
	private          int                  zoomHeight;
	private          int                  zoomWidth;

	public VargaRectificationForm(Horoscope _h, DivisionalChart _dc, Division _dtype)
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		if (false == MhoraGlobalOptions.Instance.VargaRectificationFormSize.IsEmpty)
		{
			Size = MhoraGlobalOptions.Instance.VargaRectificationFormSize;
		}

		h         = _h;
		dc        = _dc;
		mOriginal = (Moment) h.info.tob.Clone();
		PopulateOptionsInit(_dtype);
		//this.PopulateOptions();
		PopulateCache();
		HScroll = true;
		VScroll = true;
		Invalidate();
	}

	private Moment utToMoment(double found_ut)
	{
		// turn into horoscope
		int    year = 0, month = 0, day = 0;
		double hour = 0;
		found_ut += h.info.tz.toDouble() / 24.0;
		sweph.RevJul(found_ut, ref year, ref month, ref day, ref hour);
		var m = new Moment(year, month, day, hour);
		return m;
	}

	private double momentToUT(Moment m)
	{
		var local_ut = sweph.JulDay(m.year, m.month, m.day, m.time);
		return local_ut - h.info.tz.toDouble() / 24.0;
	}

	private void PopulateOptions()
	{
		ut_lower  = momentToUT(opts.StartTime);
		ut_higher = momentToUT(opts.EndTime);
	}

	private void PopulateOptionsInit(Division dtype)
	{
		var dp       = h.getPosition(mBody).toDivisionPosition(dtypeRasi);
		var foundLon = new Longitude(0);
		var bForward = true;
		ut_lower  = h.TransitSearch(mBody, h.info.tob, false, new Longitude(dp.cusp_lower), foundLon, ref bForward);
		ut_higher = h.TransitSearch(mBody, h.info.tob, true, new Longitude(dp.cusp_higher), foundLon, ref bForward);


		var ut_span = (ut_higher - ut_lower) / dtype.NumPartsInDivision() * 5.0;
		var ut_curr = h.baseUT;
		ut_lower  = ut_curr - ut_span / 2.0;
		ut_higher = ut_curr + ut_span / 2.0;

		//double ut_extra = (ut_higher-ut_lower)*(1.0/3.0);
		//ut_lower -= ut_extra;
		//ut_higher += ut_extra;


		//ut_lower = h.baseUT - 1.0/24.0;
		//ut_higher = h.baseUT + 1.0/24.0;
		opts = new UserOptions(utToMoment(ut_lower), utToMoment(ut_higher), dtype);
	}

	private void PopulateCache()
	{
		momentCusps = new double[opts.Divisions.Length][];
		zhCusps     = new ZodiacHouse.Rasi[opts.Divisions.Length][];
		for (var i = 0; i < opts.Divisions.Length; i++)
		{
			var dtype = opts.Divisions[i];
			var al    = new ArrayList();
			var zal   = new ArrayList();
			//mhora.Log.Debug ("Calculating cusps for {0} between {1} and {2}", 
			//	dtype, this.utToMoment(ut_lower), this.utToMoment(ut_higher));
			var ut_curr = ut_lower - 1.0 / (24.0 * 60.0);

			sweph.obtainLock(h);
			var bp = Basics.CalculateSingleBodyPosition(ut_curr, sweph.BodyNameToSweph(mBody), mBody, Body.Type.Graha, h);
			sweph.releaseLock(h);
			//BodyPosition bp = (BodyPosition)h.getPosition(mBody).Clone();
			//DivisionPosition dp = bp.toDivisionPosition(this.dtypeRasi);

			var dp = bp.toDivisionPosition(dtype);

			//mhora.Log.Debug ("Longitude at {0} is {1} as is in varga rasi {2}",
			//	this.utToMoment(ut_curr), bp.longitude, dp.zodiac_house.value);

			//bp.longitude = new Longitude(dp.cusp_lower - 0.2);
			//dp = bp.toDivisionPosition(dtype);

			while (true)
			{
				var m        = utToMoment(ut_curr);
				var foundLon = new Longitude(0);
				var bForward = true;

				//mhora.Log.Debug ("    Starting search at {0}", this.utToMoment(ut_curr));

				ut_curr = h.TransitSearch(mBody, utToMoment(ut_curr), true, new Longitude(dp.cusp_higher), foundLon, ref bForward);

				bp.longitude = new Longitude(dp.cusp_higher + 0.1);
				dp           = bp.toDivisionPosition(dtype);

				if (ut_curr >= ut_lower && ut_curr <= ut_higher + 1.0 / (24.0 * 60.0 * 60.0) * 5.0)
				{
					//	mhora.Log.Debug ("{0}: {1} at {2}",
					//		dtype, foundLon, this.utToMoment(ut_curr));
					al.Add(ut_curr);
					zal.Add(dp.zodiac_house.Sign);
				}
				else if (ut_curr > ut_higher)
				{
					//	mhora.Log.Debug ("- {0}: {1} at {2}",
					//		dtype, foundLon, this.utToMoment(ut_curr));						
					break;
				}

				ut_curr += 1.0 / (24.0 * 60.0 * 60.0) * 5.0;
			}

			momentCusps[i] = (double[]) al.ToArray(typeof(double));
			zhCusps[i]     = (ZodiacHouse.Rasi[]) zal.ToArray(typeof(ZodiacHouse.Rasi));
		}


		//for (int i=0; i<opts.Divisions.Length; i++)
		//{
		//	for (int j=0; j<momentCusps[i].Length; j++)
		//	{
		//		mhora.Log.Debug ("Cusp for {0} at {1}", opts.Divisions[i], momentCusps[i][j]);
		//	}
		//}
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
		this.mContext            = new System.Windows.Forms.ContextMenu();
		this.menuOptions         = new System.Windows.Forms.MenuItem();
		this.menuReset           = new System.Windows.Forms.MenuItem();
		this.menuCenter          = new System.Windows.Forms.MenuItem();
		this.menuHalve           = new System.Windows.Forms.MenuItem();
		this.menuDouble          = new System.Windows.Forms.MenuItem();
		this.menuDisplaySeconds  = new System.Windows.Forms.MenuItem();
		this.menuItem1           = new System.Windows.Forms.MenuItem();
		this.menuShadvargas      = new System.Windows.Forms.MenuItem();
		this.menuSaptavargas     = new System.Windows.Forms.MenuItem();
		this.menuDasavargas      = new System.Windows.Forms.MenuItem();
		this.menuShodasavargas   = new System.Windows.Forms.MenuItem();
		this.menuNadiamsavargas  = new System.Windows.Forms.MenuItem();
		this.menuCopyToClipboard = new System.Windows.Forms.MenuItem();
		// 
		// mContext
		// 
		this.mContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.menuOptions,
			this.menuReset,
			this.menuCenter,
			this.menuHalve,
			this.menuDouble,
			this.menuDisplaySeconds,
			this.menuItem1,
			this.menuCopyToClipboard
		});
		// 
		// menuOptions
		// 
		this.menuOptions.Index =  0;
		this.menuOptions.Text  =  "Options";
		this.menuOptions.Click += new System.EventHandler(this.menuOptions_Click);
		// 
		// menuReset
		// 
		this.menuReset.Index =  1;
		this.menuReset.Text  =  "Reset Original Time";
		this.menuReset.Click += new System.EventHandler(this.menuReset_Click);
		// 
		// menuCenter
		// 
		this.menuCenter.Index =  2;
		this.menuCenter.Text  =  "Center to Current Time";
		this.menuCenter.Click += new System.EventHandler(this.menuCenter_Click);
		// 
		// menuHalve
		// 
		this.menuHalve.Index =  3;
		this.menuHalve.Text  =  "Zoom In";
		this.menuHalve.Click += new System.EventHandler(this.menuHalve_Click);
		// 
		// menuDouble
		// 
		this.menuDouble.Index =  4;
		this.menuDouble.Text  =  "Zoom Out";
		this.menuDouble.Click += new System.EventHandler(this.menuDouble_Click);
		// 
		// menuDisplaySeconds
		// 
		this.menuDisplaySeconds.Index =  5;
		this.menuDisplaySeconds.Text  =  "Display Seconds";
		this.menuDisplaySeconds.Click += new System.EventHandler(this.menuDisplaySeconds_Click);
		// 
		// menuItem1
		// 
		this.menuItem1.Index = 6;
		this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.menuShadvargas,
			this.menuSaptavargas,
			this.menuDasavargas,
			this.menuShodasavargas,
			this.menuNadiamsavargas
		});
		this.menuItem1.Text = "Show Vargas";
		// 
		// menuShadvargas
		// 
		this.menuShadvargas.Index =  0;
		this.menuShadvargas.Text  =  "Shadvargas";
		this.menuShadvargas.Click += new System.EventHandler(this.menuShadvargas_Click);
		// 
		// menuSaptavargas
		// 
		this.menuSaptavargas.Index =  1;
		this.menuSaptavargas.Text  =  "Saptavargas";
		this.menuSaptavargas.Click += new System.EventHandler(this.menuSaptavargas_Click);
		// 
		// menuDasavargas
		// 
		this.menuDasavargas.Index =  2;
		this.menuDasavargas.Text  =  "Dasavargas";
		this.menuDasavargas.Click += new System.EventHandler(this.menuDasavargas_Click);
		// 
		// menuShodasavargas
		// 
		this.menuShodasavargas.Index =  3;
		this.menuShodasavargas.Text  =  "Shodasavargas";
		this.menuShodasavargas.Click += new System.EventHandler(this.menuShodasavargas_Click);
		// 
		// menuNadiamsavargas
		// 
		this.menuNadiamsavargas.Index =  4;
		this.menuNadiamsavargas.Text  =  "Nadiamsa vargas";
		this.menuNadiamsavargas.Click += new System.EventHandler(this.menuNadiamsavargas_Click);
		// 
		// menuCopyToClipboard
		// 
		this.menuCopyToClipboard.Index =  7;
		this.menuCopyToClipboard.Text  =  "Copy To Clipboard";
		this.menuCopyToClipboard.Click += new System.EventHandler(this.menuCopyToClipboard_Click);
		// 
		// VargaRectificationForm
		// 
		this.AutoScale         =  false;
		this.AutoScaleBaseSize =  new System.Drawing.Size(5, 13);
		this.AutoScroll        =  true;
		this.ClientSize        =  new System.Drawing.Size(512, 142);
		this.ContextMenu       =  this.mContext;
		this.Name              =  "VargaRectificationForm";
		this.Text              =  "Lagna Based Rectification Helper";
		this.Resize            += new System.EventHandler(this.VargaRectificationForm_Resize);
		this.Click             += new System.EventHandler(this.VargaRectificationForm_Click);
		this.Load              += new System.EventHandler(this.VargaRectificationForm_Load);
		this.DoubleClick       += new System.EventHandler(this.VargaRectificationForm_DoubleClick);
		this.Paint             += new System.Windows.Forms.PaintEventHandler(this.VargaRectificationForm_Paint);
	}

#endregion

	private void VargaRectificationForm_Load(object sender, EventArgs e)
	{
	}

	private void Draw(Graphics g)
	{
		var f_time   = MhoraGlobalOptions.Instance.GeneralFont;
		var p_black  = new Pen(Brushes.Black);
		var p_lgray  = new Pen(Brushes.LightGray);
		var p_orange = new Pen(Brushes.DarkOrange);
		var p_red    = new Pen(Brushes.DarkRed);

		//int bar_width = this.Width - vname_width*2;
		var    bar_width = zoomWidth - vname_width * 2;
		float  x_offset  = 0;
		string s;
		SizeF  sz;

		g.Clear(Color.AliceBlue);

		x_offset = (float) ((momentToUT(mOriginal) - ut_lower) / (ut_higher - ut_lower) * bar_width) + vname_width;
		g.DrawLine(p_lgray, x_offset, unit_height / 2, x_offset, opts.Divisions.Length  * unit_height + unit_height / 2);


		x_offset = (float) ((h.baseUT - ut_lower) / (ut_higher - ut_lower) * bar_width) + vname_width;
		float y_max = opts.Divisions.Length                                * unit_height + unit_height / 2;
		g.DrawLine(p_red, x_offset, unit_height / 2, x_offset, y_max);
		var mNow = utToMoment(h.baseUT);
		s  = mNow.ToTimeString(menuDisplaySeconds.Checked);
		sz = g.MeasureString(s, f_time);
		g.DrawString(s, f_time, Brushes.DarkRed, x_offset - sz.Width / 2, y_max);


		for (var iVarga = 0; iVarga < opts.Divisions.Length; iVarga++)
		{
			var varga_y = (iVarga + 1) * unit_height;
			g.DrawLine(p_black, vname_width, varga_y, vname_width + bar_width, varga_y);
			s  = string.Format("D-{0}", opts.Divisions[iVarga].NumPartsInDivision());
			sz = g.MeasureString(s, f_time);
			g.DrawString(s, f_time, Brushes.Gray, 4, varga_y - sz.Height / 2);


			float old_x_offset = 0;
			for (var j = 0; j < momentCusps[iVarga].Length; j++)
			{
				var ut_curr = momentCusps[iVarga][j];
				var perc    = (ut_curr - ut_lower) / (ut_higher - ut_lower) * 100.0;
				//mhora.Log.Debug ("Vargas {0}, perc {1}", opts.Divisions[iVarga], perc);
				x_offset = (float) ((ut_curr - ut_lower) / (ut_higher - ut_lower) * bar_width) + vname_width;

				//(float)((ut_curr-ut_lower)/(ut_higher/ut_lower)*bar_width);
				var m = utToMoment(ut_curr);
				s  = string.Format("{0} {1}", m.ToTimeString(menuDisplaySeconds.Checked), ZodiacHouse.ToShortString(zhCusps[iVarga][j]));
				sz = g.MeasureString(s, f_time);
				if (old_x_offset + sz.Width < x_offset)
				{
					g.DrawLine(p_black, x_offset, varga_y          - half_tick_height, x_offset, varga_y + half_tick_height);
					g.DrawString(s, f_time, Brushes.Gray, x_offset - sz.Width / 2, varga_y               - sz.Height - half_tick_height);
					//							x_offset-(sz.Width/2), varga_y-sz.Height-half_tick_height);
					old_x_offset = x_offset;

					//						s = zhCusps[iVarga][j].ToString();
					//						sz = g.MeasureString(s, f_time);
					//						g.DrawString(s, f_time, Brushes.Black,
					//							x_offset, varga_y - sz.Height);
				}
				else
				{
					g.DrawLine(p_orange, x_offset, varga_y - half_tick_height, x_offset, varga_y + half_tick_height);
				}
			}
		}


		//this.Height);
	}

	private void VargaRectificationForm_Paint(object sender, PaintEventArgs e)
	{
		zoomHeight = (opts.Divisions.Length + 1) * unit_height + unit_height / 2;

		if (bmpBuffer != null && bmpBuffer.Width == zoomWidth && bmpBuffer.Height == zoomHeight)
		{
			e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
			e.Graphics.DrawImage(bmpBuffer, 0, 0);
			return;
		}

		zoomWidth = Width;
		var displayGraphics = CreateGraphics();
		bmpBuffer = new Bitmap(zoomWidth, zoomHeight, displayGraphics);
		var imageGraphics = Graphics.FromImage(bmpBuffer);
		Draw(imageGraphics);
		displayGraphics.Dispose();
		e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);
		e.Graphics.DrawImage(bmpBuffer, 0, 0);

		AutoScrollMinSize = new Size(zoomWidth, zoomHeight);
	}

	private void VargaRectificationForm_Resize(object sender, EventArgs e)
	{
		MhoraGlobalOptions.Instance.VargaRectificationFormSize = Size;
		bmpBuffer                                              = null;
		Invalidate();
	}

	public object SetOptions(object _uo)
	{
		//UserOptions uo = (UserOptions)_uo;
		//opts.StartTime = uo.StartTime;
		//opts.EndTime = uo.EndTime;

		var ret = opts.CopyFrom(_uo);
		PopulateOptions();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
		return ret;
	}

	private void menuOptions_Click(object sender, EventArgs e)
	{
		new MhoraOptions(opts, SetOptions).ShowDialog();
	}

	private void VargaRectificationForm_DoubleClick(object sender, EventArgs e)
	{
		var pt = PointToClient(MousePosition);

		var click_width = pt.X - (double) vname_width;
		//double bar_width = (double)(1000 - vname_width*2);
		double bar_width = Width - vname_width * 2;
		var    perc      = click_width / bar_width;

		if (perc < 0 && perc > 100)
		{
			return;
		}

		var ut_new = ut_lower + (ut_higher - ut_lower) * perc;
		var mNew   = utToMoment(ut_new);
		h.info.tob = mNew;
		h.OnChanged();
		bmpBuffer = null;
		Invalidate();

		//mhora.Log.Debug ("Click at {0}. Width at {1}. Percentage is {2}", 
		//	click_width, bar_width, perc);
	}

	private void menuReset_Click(object sender, EventArgs e)
	{
		h.info.tob = (Moment) mOriginal.Clone();
		h.OnChanged();
		bmpBuffer = null;
		Invalidate();
	}

	private void VargaRectificationForm_Click(object sender, EventArgs e)
	{
		//this.VargaRectificationForm_DoubleClick(sender,e);
	}

	private void menuCopyToClipboard_Click(object sender, EventArgs e)
	{
		//Graphics displayGraphics = this.CreateGraphics();
		//Bitmap bmpBuffer = new Bitmap(this.Width, this.Height, displayGraphics);
		//Graphics imageGraphics = Graphics.FromImage(bmpBuffer);
		//this.Draw(imageGraphics);
		//displayGraphics.Dispose();

		Clipboard.SetDataObject(bmpBuffer, true);
	}

	private void UpdateOptsFromUT()
	{
		opts.StartTime = utToMoment(ut_lower);
		opts.EndTime   = utToMoment(ut_higher);
	}

	private void menuCenter_Click(object sender, EventArgs e)
	{
		var ut_half = (ut_higher - ut_lower) / 2.0;
		var ut_curr = momentToUT(h.info.tob);
		ut_lower  = ut_curr - ut_half;
		ut_higher = ut_curr + ut_half;
		UpdateOptsFromUT();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuHalve_Click(object sender, EventArgs e)
	{
		var ut_curr    = momentToUT(h.info.tob);
		var ut_quarter = (ut_higher - ut_lower) / 4.0;
		ut_lower  = ut_curr - ut_quarter;
		ut_higher = ut_curr + ut_quarter;
		UpdateOptsFromUT();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuDouble_Click(object sender, EventArgs e)
	{
		var ut_curr = momentToUT(h.info.tob);
		var ut_half = ut_higher - ut_lower;
		ut_lower  = ut_curr - ut_half;
		ut_higher = ut_curr + ut_half;
		UpdateOptsFromUT();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuShadvargas_Click(object sender, EventArgs e)
	{
		opts.Divisions = Vargas.Shadvargas();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuSaptavargas_Click(object sender, EventArgs e)
	{
		opts.Divisions = Vargas.Saptavargas();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuDasavargas_Click(object sender, EventArgs e)
	{
		opts.Divisions = Vargas.Dasavargas();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuShodasavargas_Click(object sender, EventArgs e)
	{
		opts.Divisions = Vargas.Shodasavargas();
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuNadiamsavargas_Click(object sender, EventArgs e)
	{
		var divs_shod = Vargas.Shodasavargas();
		var divs      = new Division[divs_shod.Length + 1];
		divs_shod.CopyTo(divs, 0);
		divs[divs_shod.Length] = new Division(Vargas.DivisionType.NadiamsaCKN);
		opts.Divisions         = divs;
		PopulateCache();
		bmpBuffer = null;
		Invalidate();
	}

	private void menuDisplaySeconds_Click(object sender, EventArgs e)
	{
		menuDisplaySeconds.Checked = !menuDisplaySeconds.Checked;
		bmpBuffer                  = null;
		Invalidate();
	}

	public class UserOptions : ICloneable
	{
		public UserOptions(Moment _start, Moment _end, Division dtype)
		{
			StartTime = _start;
			EndTime   = _end;

			if (dtype.MultipleDivisions.Length == 1 && dtype.MultipleDivisions[0].Varga != Vargas.DivisionType.Rasi && dtype.MultipleDivisions[0].Varga != Vargas.DivisionType.Navamsa)
			{
				Divisions = new[]
				{
					new Division(Vargas.DivisionType.Rasi),
					new Division(Vargas.DivisionType.Navamsa),
					dtype
				};
			}
			else
			{
				Divisions = new[]
				{
					new Division(Vargas.DivisionType.Rasi),
					new Division(Vargas.DivisionType.Saptamsa),
					new Division(Vargas.DivisionType.Navamsa)
				};
			}
		}

		public UserOptions(Moment _start, Moment _end)
		{
			StartTime = _start;
			EndTime   = _end;
		}

		public Division[] Divisions
		{
			get;
			set;
		} =
		{
			new(Vargas.DivisionType.Rasi),
			new(Vargas.DivisionType.DrekkanaParasara),
			new(Vargas.DivisionType.Navamsa),
			new(Vargas.DivisionType.Saptamsa),
			new(Vargas.DivisionType.Dasamsa),
			new(Vargas.DivisionType.Dwadasamsa),
			new(Vargas.DivisionType.Shodasamsa)
		};

		public Moment StartTime
		{
			get;
			set;
		}

		public Moment EndTime
		{
			get;
			set;
		}

		public object Clone()
		{
			var uo = new UserOptions((Moment) StartTime.Clone(), (Moment) EndTime.Clone());
			uo.Divisions = (Division[]) Divisions.Clone();
			return uo;
		}

		public object CopyFrom(object _uo)
		{
			var uo = (UserOptions) _uo;
			StartTime = uo.StartTime;
			EndTime   = uo.EndTime;
			Divisions = (Division[]) uo.Divisions.Clone();
			return Clone();
		}
	}
}