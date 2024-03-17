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

	public void SetView(BaseUserOptions.ViewType view)
	{
		MhoraControl mc = null;
		;

		switch (view)
		{
			case BaseUserOptions.ViewType.DivisionalChart:
				mc = new DivisionalChart(h);
				break;
			case BaseUserOptions.ViewType.Ashtakavarga:
				mc = new AshtakavargaControl(h);
				break;
			case BaseUserOptions.ViewType.ChakraSarvatobhadra81:
				mc = new Sarvatobhadra81Control(h);
				break;
			case BaseUserOptions.ViewType.NavamsaCircle:
				mc = new NavamsaControl(h);
				break;
			case BaseUserOptions.ViewType.VaraChakra:
				mc = new VaraChakra(h);
				break;
			case BaseUserOptions.ViewType.Panchanga:
				mc = new MhoraPanchangaControl(h);
				break;
			case BaseUserOptions.ViewType.KutaMatching:
			{
				var h2 = h;
				foreach (var f in MhoraGlobalOptions.MainControl.MdiChildren)
				{
					if (f is MhoraChild)
					{
						var mch = (MhoraChild) f;
						if (h == h2 && mch.getHoroscope() != h2)
						{
							h2 = mch.getHoroscope();
							break;
						}
					}
				}

				mc = new KutaMatchingControl(h, h);
			}
				break;
			case BaseUserOptions.ViewType.DasaVimsottari:
				mc = new MhoraDasaControl(h, new VimsottariDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaYogini:
				mc = new MhoraDasaControl(h, new YoginiDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaShodashottari:
				mc = new MhoraDasaControl(h, new ShodashottariDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaAshtottari:
				mc = new MhoraDasaControl(h, new AshtottariDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaTithiAshtottari:
				mc = new MhoraDasaControl(h, new TithiAshtottariDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaKaranaChaturashitiSama:
				mc = new MhoraDasaControl(h, new KaranaChaturashitiSamaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaYogaVimsottari:
				mc = new MhoraDasaControl(h, new YogaVimsottariDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaLagnaKendradiRasi:
				mc = new MhoraDasaControl(h, new LagnaKendradiRasiDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaKarakaKendradiGraha:
				mc = new MhoraDasaControl(h, new KarakaKendradiGrahaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaKalachakra:
				mc = new MhoraDasaControl(h, new KalachakraDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaMoola:
				mc = new MhoraDasaControl(h, new MoolaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaNavamsa:
				mc = new MhoraDasaControl(h, new NavamsaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaMandooka:
				mc = new MhoraDasaControl(h, new MandookaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaChara:
				mc = new MhoraDasaControl(h, new CharaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaTrikona:
				mc = new MhoraDasaControl(h, new TrikonaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaSu:
				mc = new MhoraDasaControl(h, new SuDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaSudarshanaChakra:
				mc = new MhoraDasaControl(h, new SudarshanaChakraDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaMudda:
			{
				var dc = new MhoraDasaControl(h, new VimsottariDasa(h));
				dc.DasaOptions.YearType    = ToDate.DateType.SolarYear;
				dc.DasaOptions.YearLength  = 360;
				dc.DasaOptions.Compression = 1;
				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaSudarshanaChakraCompressed:
			{
				var dc = new MhoraDasaControl(h, new SudarshanaChakraDasa(h));
				dc.DasaOptions.YearType    = ToDate.DateType.SolarYear;
				dc.DasaOptions.YearLength  = 360;
				dc.DasaOptions.Compression = 1;
				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaYogaPraveshVimsottariCompressedYoga:
			{
				var dc = new MhoraDasaControl(h, new YogaVimsottariDasa(h));
				dc.CompressToYogaPraveshaYearYoga();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaTithiPraveshAshtottariCompressedTithi:
			{
				var dc = new MhoraDasaControl(h, new TithiAshtottariDasa(h));
				dc.DasaOptions.YearType = ToDate.DateType.TithiYear;
				var td_pravesh = new ToDate(h.Info.Jd, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
				var td_tithi   = new ToDate(h.Info.Jd, ToDate.DateType.TithiYear, 360.0, 0, h);
				if (td_tithi.AddYears(1).ToJulian() + 15.0 < td_pravesh.AddYears(1).ToJulian())
				{
					dc.DasaOptions.YearLength = 390;
				}

				dc.DasaOptions.Compression = 1;

				var tuo = (TithiAshtottariDasa.UserOptions) dc.DasaSpecificOptions;
				tuo.UseTithiRemainder  = true;
				dc.DasaSpecificOptions = tuo;

				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaTithiPraveshAshtottariCompressedFixed:
			{
				var dc         = new MhoraDasaControl(h, new TithiAshtottariDasa(h));
				var td_pravesh = new ToDate(h.Info.Jd, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
				dc.DasaOptions.YearType   = ToDate.DateType.FixedYear;
				dc.DasaOptions.YearLength = td_pravesh.AddYears(1).ToJulian() - td_pravesh.AddYears(0).ToJulian();

				var tuo = (TithiAshtottariDasa.UserOptions) dc.DasaSpecificOptions;
				tuo.UseTithiRemainder      = true;
				dc.DasaSpecificOptions     = tuo;
				dc.DasaOptions.Compression = 1;

				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaTithiPraveshAshtottariCompressedSolar:
			{
				var dc         = new MhoraDasaControl(h, new TithiAshtottariDasa(h));
				var td_pravesh = new ToDate(h.Info.Jd, ToDate.DateType.TithiPraveshYear, 360.0, 0, h);
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

				dc.DasaOptions.YearType   = ToDate.DateType.SolarYear;
				dc.DasaOptions.YearLength = (double)diff;

				var tuo = (TithiAshtottariDasa.UserOptions) dc.DasaSpecificOptions;
				tuo.UseTithiRemainder  = true;
				dc.DasaSpecificOptions = tuo;


				//dc.DasaOptions.YearType = ToDate.DateType.FixedYear;
				//dc.DasaOptions.YearLength = td_pravesh.AddYears(1).toUniversalTime() - 
				//	td_pravesh.AddYears(0).toUniversalTime();
				dc.DasaOptions.Compression = 1;

				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaDwadashottari:
				mc = new MhoraDasaControl(h, new DwadashottariDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaPanchottari:
				mc = new MhoraDasaControl(h, new PanchottariDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaShatabdika:
				mc = new MhoraDasaControl(h, new ShatabdikaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaChaturashitiSama:
				mc = new MhoraDasaControl(h, new ChaturashitiSamaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaDwisaptatiSama:
				mc = new MhoraDasaControl(h, new DwisaptatiSamaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaShatTrimshaSama:
				mc = new MhoraDasaControl(h, new ShatTrimshaSamaDasa(h));
				break;
			case BaseUserOptions.ViewType.BasicCalculations:
				mc = new BasicCalculationsControl(h);
				break;
			case BaseUserOptions.ViewType.KeyInfo:
				mc = new KeyInfoControl(h);
				break;
			case BaseUserOptions.ViewType.Balas:
				mc = new BalasControl(h);
				break;
			case BaseUserOptions.ViewType.TransitSearch:
				mc = new TransitSearch(h);
				break;
			case BaseUserOptions.ViewType.NaisargikaRasiDasa:
				mc = new MhoraDasaControl(h, new NaisargikaRasiDasa(h));
				break;
			case BaseUserOptions.ViewType.NaisargikaGrahaDasa:
				mc = new MhoraDasaControl(h, new NaisargikaGrahaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaNarayana:
				mc = new MhoraDasaControl(h, new NarayanaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaNarayanaSama:
				mc = new MhoraDasaControl(h, new NarayanaSamaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaShoola:
				mc = new MhoraDasaControl(h, new ShoolaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaNiryaanaShoola:
				mc = new MhoraDasaControl(h, new NirayaanaShoolaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaDrig:
				mc = new MhoraDasaControl(h, new DrigDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaTajaka:
				mc = new MhoraDasaControl(h, new TajakaDasa(h));
				break;
			case BaseUserOptions.ViewType.DasaTithiPravesh:
			{
				var dc = new MhoraDasaControl(h, new TithiPraveshDasa(h));
				dc.DasaOptions.YearType = ToDate.DateType.TithiPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaYogaPravesh:
			{
				var dc = new MhoraDasaControl(h, new YogaPraveshDasa(h));
				dc.DasaOptions.YearType = ToDate.DateType.YogaPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaNakshatraPravesh:
			{
				var dc = new MhoraDasaControl(h, new NakshatraPraveshDasa(h));
				dc.DasaOptions.YearType = ToDate.DateType.NakshatraPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaKaranaPravesh:
			{
				var dc = new MhoraDasaControl(h, new KaranaPraveshDasa(h));
				dc.DasaOptions.YearType = ToDate.DateType.KaranaPraveshYear;
				dc.LinkToHoroscope      = false;
				dc.Reset();
				mc = dc;
			}
				break;
			case BaseUserOptions.ViewType.DasaTattwa:
				mc = new MhoraDasaControl(h, new TattwaDasa(h));
				break;
			default:
				Debug.Assert(false, "Unknown View Internal error");
				break;
		}

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
		public enum ViewType
		{
			DivisionalChart,
			Ashtakavarga,
			KeyInfo,
			BasicCalculations,
			Balas,
			TransitSearch,
			NavamsaCircle,
			VaraChakra,
			KutaMatching,
			ChakraSarvatobhadra81,
			Panchanga,
			DasaAshtottari,
			DasaVimsottari,
			DasaMudda,
			DasaShodashottari,
			DasaDwadashottari,
			DasaPanchottari,
			DasaShatabdika,
			DasaTithiAshtottari,
			DasaYogaVimsottari,
			DasaKaranaChaturashitiSama,
			DasaTithiPraveshAshtottariCompressedFixed,
			DasaTithiPraveshAshtottariCompressedSolar,
			DasaTithiPraveshAshtottariCompressedTithi,
			DasaYogaPraveshVimsottariCompressedYoga,
			DasaChaturashitiSama,
			DasaDwisaptatiSama,
			DasaShatTrimshaSama,
			DasaDrig,
			DasaNarayana,
			DasaNarayanaSama,
			DasaShoola,
			DasaNiryaanaShoola,
			DasaSu,
			DasaKalachakra,
			DasaTajaka,
			DasaTithiPravesh,
			DasaYogaPravesh,
			DasaNakshatraPravesh,
			DasaKaranaPravesh,
			DasaTattwa,
			NaisargikaRasiDasa,
			NaisargikaGrahaDasa,
			DasaSudarshanaChakra,
			DasaSudarshanaChakraCompressed,
			DasaYogini,
			DasaNavamsa,
			DasaMandooka,
			DasaChara,
			DasaTrikona,
			DasaLagnaKendradiRasi,
			DasaMoola,
			DasaKarakaKendradiGraha
		}

		public BaseUserOptions()
		{
			View = ViewType.DivisionalChart;
		}

		public ViewType View
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