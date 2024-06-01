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
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Components.DasaControl;
using Mhora.Components.PanchangaControl;
using Mhora.Components.VargaControl;
using Mhora.Dasas.GrahaDasa;
using Mhora.Dasas.NakshatraDasa;
using Mhora.Dasas.RasiDasa;
using Mhora.Dasas.YearlyDasa;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;
using TransitSearch = Mhora.Components.TransitControl.TransitSearch;

namespace Mhora.Components;

/// <summary>
///     Summary description for MhoraControlContainer.
/// </summary>
public class MhoraControlContainer : UserControl
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;

	public  Horoscope       h;
	private MhoraControl    mControl;
	public  BaseUserOptions options;

	public MhoraControlContainer(MhoraControl _mControl)
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitForm call
		Control = _mControl;
	}

	public MhoraControl Control
	{
		get => mControl;
		set
		{
			if (mControl != null)
			{
				Controls.Remove(mControl);
			}

			mControl      = value;
			mControl.Dock = DockStyle.Fill;
			Controls.Add(mControl);
			mControl.Parent = this;
		}
	}

	public MhoraControl GetMhoraControl(MhoraViewType view)
	{
		switch (view)
		{
			case MhoraViewType.DivisionalChart:
				return new DivisionalChart(h);
			case MhoraViewType.Ashtakavarga:
				return  new AshtakavargaControl(h);
			case MhoraViewType.ChakraSarvatobhadra81:
				return  new Sarvatobhadra81Control(h);
			case MhoraViewType.NavamsaCircle:
				return  new NavamsaControl(h);
			case MhoraViewType.VaraChakra:
				return new VaraChakra(h);
			case MhoraViewType.Panchanga:
				return new MhoraPanchangaControl(h);
			case MhoraViewType.KutaMatching:
			{
				var h2 = h;
				foreach (var f in MhoraGlobalOptions.MainControl.MdiChildren)
				{
					if (f is MhoraChild)
					{
						var mch = (MhoraChild) f;
						if (h == h2 && mch.Horoscope != h2)
						{
							h2 = mch.Horoscope;
							break;
						}
					}
				}

				return new KutaMatchingControl(h, h2);
			}
			case MhoraViewType.DasaVimsottari:
				return new MhoraDasaControl(h, new VimsottariDasa(h));
			case MhoraViewType.DasaYogini:
				return new MhoraDasaControl(h, new YoginiDasa(h));
			case MhoraViewType.DasaShodashottari:
				return new MhoraDasaControl(h, new ShodashottariDasa(h));
			case MhoraViewType.DasaAshtottari:
				return new MhoraDasaControl(h, new AshtottariDasa(h));
			case MhoraViewType.DasaTithiAshtottari:
				return new MhoraDasaControl(h, new TithiAshtottariDasa(h));
			case MhoraViewType.DasaKaranaChaturashitiSama:
				return new MhoraDasaControl(h, new KaranaChaturashitiSamaDasa(h));
			case MhoraViewType.DasaYogaVimsottari:
				return new MhoraDasaControl(h, new YogaVimsottariDasa(h));
			case MhoraViewType.DasaLagnaKendradiRasi:
				return new MhoraDasaControl(h, new LagnaKendradiRasiDasa(h));
			case MhoraViewType.DasaKarakaKendradiGraha:
				return new MhoraDasaControl(h, new KarakaKendradiGrahaDasa(h));
			case MhoraViewType.DasaKalachakra:
				return new MhoraDasaControl(h, new KalachakraDasa(h));
			case MhoraViewType.DasaMoola:
				return new MhoraDasaControl(h, new MoolaDasa(h));
			case MhoraViewType.DasaNavamsa:
				return new MhoraDasaControl(h, new NavamsaDasa(h));
			case MhoraViewType.DasaMandooka:
				return new MhoraDasaControl(h, new MandookaDasa(h));
			case MhoraViewType.DasaChara:
				return new MhoraDasaControl(h, new CharaDasa(h));
			case MhoraViewType.DasaTrikona:
				return new MhoraDasaControl(h, new TrikonaDasa(h));
			case MhoraViewType.DasaSu:
				return new MhoraDasaControl(h, new SuDasa(h));
			case MhoraViewType.DasaSudarshanaChakra:
				return new MhoraDasaControl(h, new SudarshanaChakraDasa(h));
			case MhoraViewType.DasaMudda:
			{
				var dc = new MhoraDasaControl(h, new VimsottariDasa(h));
				dc.DasaOptions.YearType    = DateType.SolarYear;
				dc.DasaOptions.YearLength  = 360;
				dc.DasaOptions.Compression = 1;
				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaSudarshanaChakraCompressed:
			{
				var dc = new MhoraDasaControl(h, new SudarshanaChakraDasa(h));
				dc.DasaOptions.YearType    = DateType.SolarYear;
				dc.DasaOptions.YearLength  = 360;
				dc.DasaOptions.Compression = 1;
				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaYogaPraveshVimsottariCompressedYoga:
			{
				var dc = new MhoraDasaControl(h, new YogaVimsottariDasa(h));
				dc.CompressToYogaPraveshaYearYoga();
				return dc;
			}
			case MhoraViewType.DasaTithiPraveshAshtottariCompressedTithi:
			{
				var dc = new MhoraDasaControl(h, new TithiAshtottariDasa(h));
				dc.DasaOptions.YearType = DateType.TithiYear;
				var td_pravesh = new ToDate(h.Info.Jd, DateType.TithiPraveshYear, 360.0, 0, h);
				var td_tithi   = new ToDate(h.Info.Jd, DateType.TithiYear, 360.0, 0, h);
				if (td_tithi.AddYears(1).ToJulian() + 15.0 < td_pravesh.AddYears(1).ToJulian())
				{
					dc.DasaOptions.YearLength = 390;
				}

				dc.DasaOptions.Compression = 1;

				var tuo = (TithiAshtottariDasa.UserOptions) dc.DasaSpecificOptions;
				tuo.UseTithiRemainder  = true;
				dc.DasaSpecificOptions = tuo;

				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaTithiPraveshAshtottariCompressedFixed:
			{
				var dc         = new MhoraDasaControl(h, new TithiAshtottariDasa(h));
				var td_pravesh = new ToDate(h.Info.Jd, DateType.TithiPraveshYear, 360.0, 0, h);
				dc.DasaOptions.YearType   = DateType.FixedYear;
				dc.DasaOptions.YearLength = (td_pravesh.AddYears(1) - td_pravesh.AddYears(0)).TotalDays;

				var tuo = (TithiAshtottariDasa.UserOptions) dc.DasaSpecificOptions;
				tuo.UseTithiRemainder      = true;
				dc.DasaSpecificOptions     = tuo;
				dc.DasaOptions.Compression = 1;

				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaTithiPraveshAshtottariCompressedSolar:
			{
				var dc         = new MhoraDasaControl(h, new TithiAshtottariDasa(h));
				var td_pravesh = new ToDate(h.Info.Jd, DateType.TithiPraveshYear, 360.0, 0, h);
				var ut_start   = td_pravesh.AddYears(0).ToUniversalTime();
				var ut_end     = td_pravesh.AddYears(1).ToUniversalTime();
				var sp_start   = h.CalculateSingleBodyPosition(ut_start.Time().TotalHours, Body.Sun.SwephBody(), Body.Sun, BodyType.Graha);
				var sp_end     = h.CalculateSingleBodyPosition(ut_end.Time().TotalHours, Body.Sun.SwephBody(), Body.Sun, BodyType.Graha);
				var lDiff      = sp_end.Longitude.Sub(sp_start.Longitude);
				var diff       = lDiff.Value;
				if (diff < 120)
				{
					diff += 360;
				}

				dc.DasaOptions.YearType   = DateType.SolarYear;
				dc.DasaOptions.YearLength = (double)diff;

				var tuo = (TithiAshtottariDasa.UserOptions) dc.DasaSpecificOptions;
				tuo.UseTithiRemainder  = true;
				dc.DasaSpecificOptions = tuo;


				//dc.DasaOptions.YearType = DateType.FixedYear;
				//dc.DasaOptions.YearLength = td_pravesh.AddYears(1).toUniversalTime() - 
				//	td_pravesh.AddYears(0).toUniversalTime();
				dc.DasaOptions.Compression = 1;

				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaDwadashottari:
				return new MhoraDasaControl(h, new DwadashottariDasa(h));
			case MhoraViewType.DasaPanchottari:
				return new MhoraDasaControl(h, new PanchottariDasa(h));
			case MhoraViewType.DasaShatabdika:
				return new MhoraDasaControl(h, new ShatabdikaDasa(h));
			case MhoraViewType.DasaChaturashitiSama:
				return new MhoraDasaControl(h, new ChaturashitiSamaDasa(h));
			case MhoraViewType.DasaDwisaptatiSama:
				return new MhoraDasaControl(h, new DwisaptatiSamaDasa(h));
			case MhoraViewType.DasaShatTrimshaSama:
				return new MhoraDasaControl(h, new ShatTrimshaSamaDasa(h));
			case MhoraViewType.BasicCalculations:
				return new BasicCalculationsControl(h);
			case MhoraViewType.KeyInfo:
				return new KeyInfoControl(h);
			case MhoraViewType.Balas:
				return new BalasControl(h);
			case MhoraViewType.TransitSearch:
				return new TransitSearch(h);
			case MhoraViewType.NaisargikaRasiDasa:
				return new MhoraDasaControl(h, new NaisargikaRasiDasa(h));
			case MhoraViewType.NaisargikaGrahaDasa:
				return new MhoraDasaControl(h, new NaisargikaGrahaDasa(h));
			case MhoraViewType.DasaNarayana:
				return new MhoraDasaControl(h, new NarayanaDasa(h));
			case MhoraViewType.DasaNarayanaSama:
				return new MhoraDasaControl(h, new NarayanaSamaDasa(h));
			case MhoraViewType.DasaShoola:
				return new MhoraDasaControl(h, new ShoolaDasa(h));
			case MhoraViewType.DasaNiryaanaShoola:
				return new MhoraDasaControl(h, new NirayaanaShoolaDasa(h));
			case MhoraViewType.DasaDrig:
				return new MhoraDasaControl(h, new DrigDasa(h));
			case MhoraViewType.DasaTajaka:
				return new MhoraDasaControl(h, new TajakaDasa(h));
			case MhoraViewType.DasaTithiPravesh:
			{
				var dc = new MhoraDasaControl(h, new TithiPraveshDasa(h));
				dc.DasaOptions.YearType = DateType.TithiPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaYogaPravesh:
			{
				var dc = new MhoraDasaControl(h, new YogaPraveshDasa(h));
				dc.DasaOptions.YearType = DateType.YogaPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaNakshatraPravesh:
			{
				var dc = new MhoraDasaControl(h, new NakshatraPraveshDasa(h));
				dc.DasaOptions.YearType = DateType.NakshatraPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaKaranaPravesh:
			{
				var dc = new MhoraDasaControl(h, new KaranaPraveshDasa(h));
				dc.DasaOptions.YearType = DateType.KaranaPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				return dc;
			}
			case MhoraViewType.DasaTattwa:
				return new MhoraDasaControl(h, new TattwaDasa(h));
			default: 
				return null;
		}
	}

	public void SetView(MhoraViewType view)
	{
		var mc = GetMhoraControl(view);
		mc.Dock = DockStyle.Fill;
		Control?.Dispose();

		Control = mc;
	}

	public void SetBaseOptions(object o)
	{
		var uo = (BaseUserOptions) o;
		options.View = uo.View;
		SetView(uo.View);
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

#region Component Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		// 
		// MhoraControlContainer
		// 
		this.Name =  "MhoraControlContainer";
		this.Load += new System.EventHandler(this.MhoraControlContainer_Load);
	}

#endregion

	private void mControl_Load(object sender, EventArgs e)
	{
	}

	private void MhoraControlContainer_Load(object sender, EventArgs e)
	{
		options = new BaseUserOptions();
	}

	public class BaseUserOptions : ICloneable
	{

		public BaseUserOptions()
		{
			View = MhoraViewType.DivisionalChart;
		}

		public MhoraViewType View
		{
			get;
			set;
		}

		public object Clone()
		{
			var uo = new BaseUserOptions
			{
				View = View
			};
			return uo;
		}
	}
}