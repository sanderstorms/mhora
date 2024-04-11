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
using System.Text;
using Mhora.Calculation;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.SwissEph;

/// <summary>
///     A Simple wrapper around the swiss ephemeris DLL functions
///     Many function arguments use sane defaults for Jyotish programs
///     For documentation go to http://www.astro.ch and follow the
///     Swiss Ephemeris (for programmers) link.
/// </summary>
public static partial class sweph
{
	public static void Close()
	{
		if (IntPtr.Size == 4)
		{
			SwephDll.Swe32.swe_close();
		}
		else
		{
			SwephDll.Swe64.swe_close();
		}
	}


	public static byte[] Version(byte[] buffer)
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_version(buffer);
		}

		return SwephDll.Swe64.swe_version(buffer);
	}

	public static void SetEphePath(string path)
	{
		if (IntPtr.Size == 4)
		{
			SwephDll.Swe32.swe_set_ephe_path(path);
		}
		else
		{
			SwephDll.Swe64.swe_set_ephe_path(path);
		}
	}

	public static int DayOfWeek(JulianDate jd)
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_day_of_week(jd);
		}

		return SwephDll.Swe64.swe_day_of_week(jd);
	}

	public static void SetSidMode(int sid_mode, double t0, double ayan_t0)
	{
		if (IntPtr.Size == 4)
		{
			SwephDll.Swe32.swe_set_sid_mode(sid_mode, 0.0, 0.0);
		}
		else
		{
			SwephDll.Swe64.swe_set_sid_mode(sid_mode, 0.0, 0.0);
		}
	}

	public static double JulDay(int year, int month, int day, double hour)
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_julday(year, month, day, hour, SE_GREG_CAL);
		}

		return SwephDll.Swe64.swe_julday(year, month, day, hour, SE_GREG_CAL);
	}

	public static double RevJul(double tjd, out int year, out int month, out int day, out double hour)
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_revjul(tjd, 1, out year, out month, out day, out hour);
		}

		return SwephDll.Swe64.swe_revjul(tjd, 1, out year, out month, out day, out hour);
	}

	public static int CalcUT(this Horoscope h, JulianDate tjd_ut, int ipl, int addFlags, double[] xx, StringBuilder serr = null)
	{
		int ret;
		serr ??= new StringBuilder(256);

		if (IntPtr.Size == 4)
		{
			ret = SwephDll.Swe32.swe_calc_ut(tjd_ut, ipl, h.Iflag | addFlags, xx, serr);
		}
		else
		{
			ret = SwephDll.Swe64.swe_calc_ut(tjd_ut, ipl, h.Iflag | addFlags, xx, serr);
		}

		if (ret >= 0)
		{
			xx[0] += h.Options.AyanamsaOffset;
		}

		return ret;
	}

	public static int Calc(this Horoscope h, JulianDate tjd_ut, int ipl, int addFlags, double[] xx)
	{
		int ret;
		var serr = new StringBuilder(256);

		if (IntPtr.Size == 4)
		{
			ret = SwephDll.Swe32.swe_calc(tjd_ut, ipl, h.Iflag | addFlags, xx, serr);
		}
		else
		{
			ret = SwephDll.Swe64.swe_calc(tjd_ut, ipl, h.Iflag | addFlags, xx, serr);
		}

		if (ret >= 0)
		{
			xx[0] += h.Options.AyanamsaOffset;
		}

		return ret;
	}

	/*****************************************************
	**
	**   CalculatorSwe   ---   calcJd
	**
	******************************************************/
	public static double calcJd(JulianDate jd )
	{
		if (IntPtr.Size == 4)
		{
			return jd + SwephDll.Swe32.swe_deltat( jd );
		}
		return jd + SwephDll.Swe64.swe_deltat( jd );
	}

	public static int TimeEqu(JulianDate jd, out double e)
	{
		var err = new StringBuilder( 256 );
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_time_equ(jd, out e, err);
		}
		return SwephDll.Swe64.swe_time_equ(jd, out e, err);
	}

	public static double Deltat(double tjd_et)
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_deltat(tjd_et);
		}

		return SwephDll.Swe64.swe_deltat(tjd_et);
	}


	public enum EventType { SOLAR_EVENT_SUNRISE, SOLAR_EVENT_SUNSET, SOLAR_EVENT_MIDNIGHT, SOLAR_EVENT_NOON }

	/*****************************************************
	 **
	 **   CalculatorSwe   ---   calcNextSolarEvent
	 **
	 ******************************************************/
	public static double CalcNextSolarEvent(this Horoscope h, EventType type, JulianDate jd)
	{
		StringBuilder err  = new StringBuilder(256);
		var           rsmi = new double [3];
		double        tret = 0;
		int           flag = 0;

		switch (h.Options.SunrisePosition)
		{
			case HoroscopeOptions.SunrisePositionType.TrueDiscEdge:
				flag = SE_BIT_NO_REFRACTION;
				break;
			case HoroscopeOptions.SunrisePositionType.TrueDiscCenter:
				flag = SE_BIT_NO_REFRACTION | SE_BIT_DISC_CENTER;
				break;
			case HoroscopeOptions.SunrisePositionType.ApparentDiscCenter:
				flag = SE_BIT_DISC_CENTER;
				break;
		}

		switch ( type )
		{
			case EventType.SOLAR_EVENT_SUNRISE:
				flag |= SE_CALC_RISE;
				break;
			case EventType.SOLAR_EVENT_SUNSET:
				flag |= SE_CALC_SET;
				break;
			case EventType.SOLAR_EVENT_NOON:
				flag |= SE_CALC_MTRANSIT;
				break;
			case EventType.SOLAR_EVENT_MIDNIGHT:
				flag |= SE_CALC_ITRANSIT;
				break;
		}

		rsmi[0] = h.Info.Longitude;
		rsmi[1] = h.Info.Latitude;
		rsmi[2] = h.Info.Altitude;

		if (IntPtr.Size == 4)
		{
			SwephDll.Swe32.swe_rise_trans(jd, SE_SUN, string.Empty, h.Iflag, flag, rsmi, 0, 0, ref tret, err );
		}
		else
		{
			SwephDll.Swe64.swe_rise_trans(jd, SE_SUN, string.Empty, h.Iflag, flag, rsmi, 0, 0, ref tret, err );
		}
		return tret;
	}



	public static int SolEclipseWhenGlob(this Horoscope h, JulianDate tjd_ut, double[] tret, bool forward)
	{
		var serr = new StringBuilder(256);

		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_sol_eclipse_when_glob(tjd_ut, h.Iflag, 0, tret, !forward, serr);
		}

		return SwephDll.Swe64.swe_sol_eclipse_when_glob(tjd_ut, h.Iflag, 0, tret, !forward, serr);
	}

	public static int SolEclipseWhenLoc(this Horoscope h, JulianDate tjd_ut, double[] tret, double[] attr, bool forward)
	{
		var serr = new StringBuilder(256);
		var geopos = new double[3]
		{
			h.Info.Longitude,
			h.Info.Latitude,
			h.Info.Altitude
		};

		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_sol_eclipse_when_loc(tjd_ut, h.Iflag, geopos, tret, attr, !forward, serr);
		}

		return SwephDll.Swe64.swe_sol_eclipse_when_loc(tjd_ut, h.Iflag, geopos, tret, attr, !forward, serr);
	}

	public static void LunEclipseWhen(this Horoscope h, JulianDate tjd_ut, double[] tret, bool forward)
	{
		int ret;
		var serr = new StringBuilder(256);

		if (IntPtr.Size == 4)
		{
			ret = SwephDll.Swe32.swe_lun_eclipse_when(tjd_ut, h.Iflag, 0, tret, !forward, serr);
		}
		else
		{
			ret = SwephDll.Swe64.swe_lun_eclipse_when(tjd_ut, h.Iflag, 0, tret, !forward, serr);
		}

		if (ret < 0)
		{
			Application.Log.Debug("Sweph Error: {0}", serr);
			throw new Exception(serr.ToString());
		}
	}

	public struct aya_config
	{
		public JulianDate t0;
		public double ayan_t0;

		public aya_config(double t, double ayan)
		{
			t0      = t;
			ayan_t0 = ayan;
		}
	}

	public struct aya_init
	{
		public JulianDate t0;
		public double ayan_t0;
		public bool   t0_is_UT;
	}

	public static aya_init[] ayanamsa =
	{
		new()
		{
			t0       = 2433282.5,
			ayan_t0  = 24.042044444,
			t0_is_UT = false
		}, /* 0: Fagan/Bradley (Default) */
		/*{J1900, 360 - 337.53953},   * 1: Lahiri (Robert Hand) */
		new()
		{
			t0       = 2435553.5,
			ayan_t0  = 23.250182778 - 0.004660222,
			t0_is_UT = false
		},
		/* 1: Lahiri (derived from: Indian
		 * Astronomical Ephemeris 1989, p. 556;
		 * the subtracted value is nutation) */
		new()
		{
			t0       = J1900,
			ayan_t0  = 360 - 333.58695,
			t0_is_UT = false
		}, /* 2: Robert DeLuce (Constellational Astrology ... p. 5 */
		new()
		{
			t0       = J1900,
			ayan_t0  = 360 - 338.98556,
			t0_is_UT = false
		}, /* 3: B.V. Raman (Robert Hand) */
		new()
		{
			t0       = J1900,
			ayan_t0  = 360 - 341.33904,
			t0_is_UT = false
		}, /* 4: Usha/Shashi (Robert Hand) */
		new()
		{
			t0       = J1900,
			ayan_t0  = 360 - 337.636111,
			t0_is_UT = false
		}, /* 5: Krishnamurti (Robert Hand) */
		new()
		{
			t0       = J1900,
			ayan_t0  = 360 - 333.0369024,
			t0_is_UT = false
		}, /* 6: Djwhal Khool; (Graham Dawson)
		    *    Aquarius entered on 1 July 2117 */
		new()
		{
			t0       = J1900,
			ayan_t0  = 360 - 338.917778,
			t0_is_UT = false
		}, /* 7: Shri Yukteshwar; (David Cochrane) */
		//{2412543.5, 20.91, TRUE},          /* 7: Shri Yukteshwar; (Holy Science, p. xx) */
		new()
		{
			t0       = J1900,
			ayan_t0  = 360 - 338.634444,
			t0_is_UT = false
		}, /* 8: J.N. Bhasin; (David Cochrane) */
		/* 14 Sept. 2018: the following three ayanamshas have been wrong for
		 * many years */
		new()
		{
			t0       = 1684532.5,
			ayan_t0  = -5.66667,
			t0_is_UT = true
		}, /* 9: Babylonian, Kugler 1 */
		new()
		{
			t0       = 1684532.5,
			ayan_t0  = -4.26667,
			t0_is_UT = true
		}, /*10: Babylonian, Kugler 2 */
		new()
		{
			t0       = 1684532.5,
			ayan_t0  = -3.41667,
			t0_is_UT = true
		}, /*11: Babylonian, Kugler 3 */
		new()
		{
			t0       = 1684532.5,
			ayan_t0  = -4.46667,
			t0_is_UT = true
		}, /*12: Babylonian, Huber */
		/*{1684532.5, -4.56667, TRUE},         *12: Babylonian, Huber (Swisseph has been wrong for many years!) */
		new()
		{
			t0       = 1673941,
			ayan_t0  = -5.079167,
			t0_is_UT = true
		}, /*13: Babylonian, Mercier;
		    *    eta Piscium culminates with zero point */
		new()
		{
			t0       = 1684532.5,
			ayan_t0  = -4.44088389,
			t0_is_UT = true
		}, /*14: t0 is defined by Aldebaran at 15 Taurus */
		new()
		{
			t0       = 1674484,
			ayan_t0  = -9.33333,
			t0_is_UT = true
		}, /*15: Hipparchos */
		new()
		{
			t0       = 1927135.8747793,
			ayan_t0  = 0,
			t0_is_UT = true
		}, /*16: Sassanian */
		//{1746412.236, 0, FALSE},             /*17: Galactic Center at 0 Sagittarius */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*17: Galactic Center at 0 Sagittarius */
		new()
		{
			t0       = J2000,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*18: J2000 */
		new()
		{
			t0       = J1900,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*19: J1900 */
		new()
		{
			t0       = B1950,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*20: B1950 */
		new()
		{
			t0       = 1903396.8128654,
			ayan_t0  = 0,
			t0_is_UT = true
		}, /*21: Suryasiddhanta, assuming
	                                       ingress of mean Sun into Aries at point
					       of mean equinox of date on
					       21.3.499, near noon, Ujjain (75.7684565 E)
	                                       = 7:30:31.57 UT = 12:33:36 LMT*/
		new()
		{
			t0       = 1903396.8128654,
			ayan_t0  = -0.21463395,
			t0_is_UT = true
		}, /*22: Suryasiddhanta, assuming
					       ingress of mean Sun into Aries at
					       true position of mean Sun at same epoch */
		new()
		{
			t0       = 1903396.7895321,
			ayan_t0  = 0,
			t0_is_UT = true
		}, /*23: Aryabhata, same date, but UT 6:56:55.57
					       analogous 21 */
		new()
		{
			t0       = 1903396.7895321,
			ayan_t0  = -0.23763238,
			t0_is_UT = true
		}, /*24: Aryabhata, analogous 22 */
		new()
		{
			t0       = 1903396.8128654,
			ayan_t0  = -0.79167046,
			t0_is_UT = true
		}, /*25: SS, Revati/zePsc at polar long. 359°50'*/
		new()
		{
			t0       = 1903396.8128654,
			ayan_t0  = 2.11070444,
			t0_is_UT = true
		}, /*26: SS, Citra/Spica at polar long. 180° */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*27: True Citra (Spica exactly at 0 Libra) */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*28: True Revati (zeta Psc exactly at 29°50' Pisces) */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*29: True Pushya (delta Cnc exactly a 16 Cancer */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*30: R. Gil Brand; Galactic Center at golden section
	                                       between 0 Sco and 0 Aqu; note: 0° Aqu/Leo is
					       the symmetric axis of rulerships */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*31: Galactic Equator IAU 1958, i.e. galactic/ecliptic
	                                       intersection point based on galactic coordinate system */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*32: Galactic Equator True, i.e. galactic/ecliptic
	                                       intersection point based on the galactic pole as given in:
					       Liu/Zhu/Zhang, „Reconsidering the galactic
					       coordinate system“, A & A No. AA2010, Oct. 2010 */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*33: Galactic Equator Mula, i.e. galactic/ecliptic
	                                       intersection point in the middle of lunar mansion Mula */
		new()
		{
			t0       = 2451079.734892000,
			ayan_t0  = 30,
			t0_is_UT = false
		}, /*34: Skydram/Galactic Alignment (R. Mardyks);
	                                       autumn equinox aligned with Galactic Equator/Pole */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*35: Chandra Hari */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*36: Dhruva Galactic Centre Middle of Mula (Ernst Wilhelm) */
		new()
		{
			t0       = 1911797.740782065,
			ayan_t0  = 0,
			t0_is_UT = true
		}, /*37: Kali 3623 = 522 CE, Ujjain (75.7684565),
		    *    based on Kali midnight and SS year length */
		new()
		{
			t0       = 1721057.5,
			ayan_t0  = -3.2,
			t0_is_UT = true
		}, /*38: Babylonian (Britton 2010) */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*39: Sunil Sheoran ("Vedic") */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		}, /*40: Galactic Center at 0 Capricon (Cochrane) */
		new()
		{
			t0       = 2451544.5,
			ayan_t0  = 25.0,
			t0_is_UT = true
		}, /*41: "Galactic Equatorial" (N.A. Fiorenza) */
		new()
		{
			t0       = 1775845.5,
			ayan_t0  = -2.9422,
			t0_is_UT = true
		}, /*42: Vettius Valens (Moon; derived from
	                                           Holden 1995 p. 12 for epoch of Valens
						   1 Jan. 150 CE julian) */
		/*{2061539.789532065, 6.83333333, TRUE}, *41: Manjula's Laghumanasa, 10 March 932,
		 *    12 PM LMT Ujjain (75.7684565 E),
		 *    ayanamsha = 6°50' */
		new()
		{
			t0       = 0,
			ayan_t0  = 0,
			t0_is_UT = false
		} /*42: - */
	};


	public static aya_config[] aya_param =
	{
		new(2415020.0, 360 - 337.53953), // 1: Lahiri (Robert Hand) 
		new(2415020.0, 360 - 338.98556), // 3: Raman (Robert Hand) 
		new(2415020.0, 360 - 337.636111) // 5: Krishnamurti (Robert Hand) 
	};

	/*****************************************************
	 **
	 **   CalculatorSwe   ---   calcAyanamsa
	 **
	 ******************************************************/
	public static void SetAyanamsa(HoroscopeOptions.AyanamsaType type)
	{
		var    aya     = ayanamsa[type.Index()];
		double t0      = 0;
		double ayan_t0 = 0;

		if (type >= HoroscopeOptions.AyanamsaType.Lahiri && type <= HoroscopeOptions.AyanamsaType.Krishnamurti)
		{
			t0      = aya_param[(int) (type - 1)].t0;
			ayan_t0 = aya_param[(int) (type - 1)].ayan_t0;
		}

		/*
		if ( config->ephem->custom_aya_constant )
		{
			t = jd - t0;
			double years   = t       /365.25;
			double portion = years   /config->ephem->custom_aya_period;
			double aya     = portion * 360;
			//return red_deg( config->custom_ayan_t0 + 360 * ( jd - config->custom_t0 ) / ( config->custom_aya_period * 365.25 ));

			// bugfix 6.0: forgot ayan_t0
			//return red_deg( aya );
			return red_deg( ayan_t0 + aya );
		}
		else
		*/
		{
			if (t0 != 0)
			{
				if (IntPtr.Size == 4)
				{
					SwephDll.Swe32.swe_set_sid_mode(255, t0, ayan_t0);
				}
				else
				{
					SwephDll.Swe64.swe_set_sid_mode(255, aya.t0, aya.ayan_t0);
				}
			}
			else
			{
				SetSidMode(type.Index(), 0.0, 0.0);
			}
		}
	}

	public static double GetAyanamsaUT(JulianDate tjd_ut)
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_get_ayanamsa_ut(tjd_ut);
		}

		return SwephDll.Swe64.swe_get_ayanamsa_ut(tjd_ut);
	}

	public static int Rise(this Horoscope h, JulianDate tjd_ut, int ipl, int rsflag, double[] geopos, double atpress, double attemp, ref double tret, StringBuilder serr = null)
	{
		serr ??= new StringBuilder(256);

		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_rise_trans(tjd_ut, ipl, string.Empty, h.Iflag, SE_CALC_RISE | rsflag, geopos, atpress, attemp, ref tret, serr);
		}

		return SwephDll.Swe64.swe_rise_trans(tjd_ut, ipl, string.Empty, h.Iflag, SE_CALC_RISE | rsflag, geopos, atpress, attemp, ref tret, serr);
	}

	public static int Set(this Horoscope h, JulianDate tjd_ut, int ipl, int rsflag, double[] geopos, double atpress, double attemp, ref double tret)
	{
		var serr = new StringBuilder(256);

		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_rise_trans(tjd_ut, ipl, string.Empty, h.Iflag, SE_CALC_SET | rsflag, geopos, atpress, attemp, ref tret, serr);
		}

		return SwephDll.Swe64.swe_rise_trans(tjd_ut, ipl, string.Empty, h.Iflag, SE_CALC_SET | rsflag, geopos, atpress, attemp, ref tret, serr);
	}

	public static int Lmt(this Horoscope h, JulianDate tjd_ut, int ipl, int rsflag, double[] geopos, double atpress, double attemp, ref double tret)
	{
		var serr = new StringBuilder(256);

		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_rise_trans(tjd_ut, ipl, string.Empty, h.Iflag, rsflag, geopos, atpress, attemp, ref tret, serr);
		}

		return SwephDll.Swe64.swe_rise_trans(tjd_ut, ipl, string.Empty, h.Iflag, rsflag, geopos, atpress, attemp, ref tret, serr);
	}


	public static int Houses(double tjd_ut, double geolat, double geolon, int hsys, double[] cusps, double[] ascmc)
	{
		int ret;

		if (IntPtr.Size == 4)
		{
			ret = SwephDll.Swe32.swe_houses(tjd_ut, geolat, geolon, hsys, cusps, ascmc);
		}
		else
		{
			ret = SwephDll.Swe64.swe_houses(tjd_ut, geolat, geolon, hsys, cusps, ascmc);
		}

		return ret;
	}

	// hsys =
	// ‘B’ Alcabitus
	// ‘Y’ APC houses
	// ‘X’ Axial rotation system / Meridian system / Zariel
	// ‘H’ Azimuthal or horizontal system
	// ‘C’ Campanus
	// ‘F’ Carter "Poli-Equatorial"
	// ‘A’ or ‘E’ Equal (cusp 1 is Ascendant)
	// ‘D’ Equal MC (cusp 10 is MC)
	// ‘N’ Equal/1=Aries
	// ‘G’ Gauquelin sector
	//		Goelzer -> Krusinski
	//		Horizontal system -> Azimuthal system
	// ‘I’ Sunshine (Makransky, solution Treindl)
	// ‘i’ Sunshine (Makransky, solution Makransky)
	// ‘K’ Koch
	// ‘U’ Krusinski-Pisa-Goelzer
	//		Meridian system -> axial rotation
	// ‘M’ Morinus
	//		Neo-Porphyry -> Pullen SD
	//		Pisa -> Krusinski
	// ‘P’ Placidus
	//		Poli-Equatorial -> Carter
	// ‘T’ Polich/Page (“topocentric” system)
	// ‘O’ Porphyrius
	// ‘L’ Pullen SD (sinusoidal delta) – ex Neo-Porphyry
	// ‘Q’ Pullen SR (sinusoidal ratio)
	// ‘R’ Regiomontanus
	// ‘S’ Sripati
	//		“Topocentric” system -> Polich/Page
	// ‘V’ Vehlow equal (Asc. in middle of house 1)
	// ‘W’ Whole sign
	//		Zariel -> Axial rotation system
	public static int HousesEx(this Horoscope h, JulianDate tjd_ut, int iflag, double lat, double lon, int hsys, double[] cusps, double[] ascmc)
	{
		int ret;

		if (IntPtr.Size == 4)
		{
			ret = SwephDll.Swe32.swe_houses_ex(tjd_ut, iflag, lat, lon, hsys, cusps, ascmc);
		}
		else
		{
			ret = SwephDll.Swe64.swe_houses_ex(tjd_ut, iflag, lat, lon, hsys, cusps, ascmc);
		}

		var lOffset = h.Options.AyanamsaOffset;

		// House cusps defined from 1 to 12 inclusive as per sweph docs
		// Ascendants defined from 0 to 7 inclusive as per sweph docs
		for (var i = 1; i <= 12; i++)
		{
			cusps[i] = (double) new Longitude(cusps[i]).Add(lOffset).Value;
		}

		for (var i = 0; i <= 7; i++)
		{
			ascmc[i] = (double) new Longitude(ascmc[i]).Add(lOffset).Value;
		}

		return ret;
	}

	public static decimal Lagna(this Horoscope h, JulianDate tjd_ut)
	{
		var hi    = h.Info;
		var cusps = new double[13];
		var ascmc = new double[10];
		var ret   = h.HousesEx(tjd_ut, SEFLG_SIDEREAL, hi.Latitude, hi.Longitude, 'R', cusps, ascmc);
		return (decimal) ascmc[0];
	}

	/// <summary>
	///     This function must be called before topocentric planet positions for a certain birth place can be computed.
	///     It tells Swiss Ephemeris, what geographic position is to be used.
	///     Geographic longitude geolon and latitude geolat must be in degrees, the altitude above sea must be in meters.
	///     Neglecting the altitude can result in an error of about 2 arc seconds with the Moon and at an altitude 3000 m.
	///     After calling swe_set_topo(), add SEFLG_TOPOCTR to h.iflag and call swe_calc() as with an ordinary computation.
	/// </summary>
	/// <remarks>
	///     The parameters set by swe_set_topo() survive swe_close().
	/// </remarks>
	private static void SetTopo(double geolon, double geolat, double altitude)
	{
		if (IntPtr.Size == 4)
		{
			SwephDll.Swe32.swe_set_topo(geolon, geolat, altitude);
		}
		else
		{
			SwephDll.Swe64.swe_set_topo(geolon, geolat, altitude);
		}
	}

	/*****************************************************
	 **
	 **   CalculatorSwe   ---   calcSiderealTime
	 **
	 ******************************************************/
	public static double CalcSiderealTime(JulianDate jd, double longitude )
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_sidtime( jd + longitude / 360 );
		}
		return SwephDll.Swe64.swe_sidtime( jd + longitude / 360 );
	}

	//double tjdstart, /* Julian day number of start date for the search of the heliacal event 
	// */
	// double *dgeo /* geographic position (details below) */
	// dgeo[0]: geographic longitude;
	// dgeo[1]: geographic latitude;
	// dgeo[2]: geographic altitude (eye height) in meters.
	// double *datm, /* atmospheric conditions (details below) */
	// datm[0]: atmospheric pressure in mbar (hPa) ;
	// datm[1]: atmospheric temperature in degrees Celsius;
	// datm[2]: relative humidity in %;
	// datm[3]: if datm[3]>=1, then it is Meteorological Range [km] ;
	// if 1>datm[3]>0, then it is the total atmospheric coefficient (ktot) ;
	// double *dobs, /* observer description (details below) */
	// dobs[0]: age of observer in years (default = 36)
	// dobs[1]: Snellen ratio of observers eyes (default = 1 = normal)
	//The following parameters are only relevant if the flag SE_HELFLAG_OPTICAL_PARAMS is set:
	// dobs[2]: 0 = monocular, 1 = binocular (actually a boolean)
	// dobs[3]: telescope magnification: 0 = default to naked eye (binocular), 1 = naked eye
	// dobs[4]: optical aperture (telescope diameter) in mm
	// dobs[5]: optical transmission
	// char *objectname, /* name string of fixed star or planet */
	// int32 event_type, /* event type (details below) */
	// event_type = SE_HELIACAL_RISING (1): morning first (exists for all visible planets and stars);
	// event_type = SE_HELIACAL_SETTING (2): evening last (exists for all visible planets and stars);
	// event_type = SE_EVENING_FIRST (3): evening first (exists for Mercury, Venus, and the Moon);
	// event_type = SE_MORNING_LAST (4): morning last (exists for Mercury, Venus, and the Moon)
	// int32 helflag, /* calculation flag, bitmap (details below) */
	// double *dret, /* result: array of at least 50 doubles, of which 3 are used at the 
	// dret[0]: start visibility (Julian day number);
	// dret[1]: optimum visibility (Julian day number), zero if helflag >= SE_HELFLAG_AV;
	// dret[2]: end of visibility (Julian day number), zero if helflag >= SE_HELFLAG_AV
	// moment */
	// char * serr); /* error string */
	public static int HeliacalUt(double JDNDaysUTStart, double[] geopos, double[] datm, double[] dobs, StringBuilder ObjectName, int TypeEvent, int iflag, double[] dret, StringBuilder serr)
	{
		return (0);
	}

	//double tjd_ut, /* Julian day number */
	// double *dgeo, /* geographic position (details under swe_heliacal_ut() */
	// double *datm, /* atmospheric conditions (details under swe_heliacal_ut()) */
	// double *dobs, /* observer description (details under swe_heliacal_ut()) */
	// Swiss Ephemeris 2.10 Date and time conversion functions
	// swephprg.doc ~ 40 ~ i c
	// char *objectname, /* name string of fixed star or planet */
	// int32 event_type, /* event type (details under function swe_heliacal_ut()) */
	// int32 helflag, /* calculation flag, bitmap (details under swe_heliacal_ut()) */
	// double *darr, /* return array, declare array of 50 doubles */
	// char *serr); /* error string */
	//
	// The return array has the following data:
	// '0=AltO [deg] topocentric altitude of object (unrefracted)
	// '1=AppAltO [deg] apparent altitude of object (refracted)
	// '2=GeoAltO [deg] geocentric altitude of object
	// '3=AziO [deg] azimuth of object
	// '4=AltS [deg] topocentric altitude of Sun
	// '5=AziS [deg] azimuth of Sun
	// '6=TAVact [deg] actual topocentric arcus visionis
	// '7=ARCVact [deg] actual (geocentric) arcus visionis
	// '8=DAZact [deg] actual difference between object's and sun's azimuth
	// '9=ARCLact [deg] actual longitude difference between object and sun
	// '10=kact [-] extinction coefficient
	// '11=minTAV [deg] smallest topocentric arcus visionis
	// '12=TfistVR [JDN] first time object is visible, according to VR
	// '13=TbVR [JDN optimum time the object is visible, according to VR
	// '14=TlastVR [JDN] last time object is visible, according to VR
	// '15=TbYallop [JDN] best time the object is visible, according to Yallop
	// '16=WMoon [deg] crescent width of Moon
	// '17=qYal [-] q-test value of Yallop
	// '18=qCrit [-] q-test criterion of Yallop
	// '19=ParO [deg] parallax of object
	// '20 Magn [-] magnitude of object
	// '21=RiseO [JDN] rise/set time of object
	// '22=RiseS [JDN] rise/set time of Sun
	// '23=Lag [JDN] rise/set time of object minus rise/set time of Sun
	// '24=TvisVR [JDN] visibility duration
	// '25=LMoon [deg] crescent length of Moon
	// '26=CVAact [deg]
	// '27=Illum [%] new
	// '28=CVAact [deg] new
	// '29=MSk [-]
	public static int HeliacalPhenoUt(double JDNDaysUT, double[] geopos, double[] datm, double[] dobs, StringBuilder ObjectName, int TypeEvent, int helflag, double [] darr, StringBuilder serr)
	{
		return (0);
	}

	// attr[0] = phase angle (Earth-planet-sun)
	//attr[1] = phase (illumined fraction of disc)
	// attr[2] = elongation of planet
	//attr[3] = apparent diameter of disc
	//attr[4] = apparent magnitude
	// declare as attr[20] at least!
	public static int PhenoUT(double        tjd_ut, /* time Jul. Day UT */
	                          int           ipl,    /* planet number */
	                          int           iflag,  /* ephemeris flag */
	                          double []     attr,   /* return array, 20 doubles, see below */
	                          StringBuilder serr)  /* return error string */
	{
		if (IntPtr.Size == 4)
		{
			return SwephDll.Swe32.swe_pheno_ut(tjd_ut, ipl, iflag, attr, serr);
		}
		return SwephDll.Swe64.swe_pheno_ut(tjd_ut, ipl, iflag, attr, serr);
		
	}


	public static void Azalt(double   tjd_ut,    // UT
	                         int      calc_flag, // SE_ECL2HOR or SE_EQU2HOR
	                         double[] geopos,    // array of 3 doubles: geograph. long., lat., height
	                         double   atpress,   // atmospheric pressure in mbar (hPa)
	                         double   attemp,    // atmospheric temperature in degrees Celsius
	                         double[] xin,       // array of 3 doubles: position of body in either ecliptical or equatorial coordinates, depending on calc_flag
	                         double[] xaz) // return array of 3 doubles, containing azimuth, true altitude, apparent altitude;

	{
		if (IntPtr.Size == 4)
		{
			SwephDll.Swe32.swe_azalt(tjd_ut, calc_flag, geopos, atpress, attemp, xin, xaz);
		}
		else
		{
			SwephDll.Swe64.swe_azalt(tjd_ut, calc_flag, geopos, atpress, attemp, xin, xaz);
		}
	}


}