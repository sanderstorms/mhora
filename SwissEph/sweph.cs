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
using System.Runtime.InteropServices;
using System.Text;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Elements.Hora;

namespace Mhora.SwissEph;

/// <summary>
///     A Simple wrapper around the swiss ephemeris DLL functions
///     Many function arguments use sane defaults for Jyotish programs
///     For documentation go to http://www.astro.ch and follow the
///     Swiss Ephemeris (for programmers) link.
/// </summary>
public static class sweph
{
	public const int SE_JUL_CAL  = 0;
	public const int SE_GREG_CAL = 1;

	/* for swe_azalt() and swe_azalt_rev() */
	public const int SE_ECL2HOR = 0;
	public const int SE_EQU2HOR = 1;
	public const int SE_HOR2ECL = 0;
	public const int SE_HOR2EQU = 1;

	public static int SEFLG_SWIEPH     = 2;
	public static int SEFLG_TRUEPOS    = 16;
	public static int SEFLG_SPEED      = 256;
	public static int SEFLG_SIDEREAL   = 64 * 1024;
	public static int SEFLG_EQUATORIAL = 2  * 1024;

	public static int iflag = SEFLG_SWIEPH | SEFLG_SPEED | SEFLG_SIDEREAL;

	public static int SE_AYANAMSA_LAHIRI = 1;
	public static int SE_AYANAMSA_RAMAN  = 3;
	public static int ayanamsa           = SE_AYANAMSA_LAHIRI;

	public static int SE_SUN       = 0;
	public static int SE_MOON      = 1;
	public static int SE_MERCURY   = 2;
	public static int SE_VENUS     = 3;
	public static int SE_MARS      = 4;
	public static int SE_JUPITER   = 5;
	public static int SE_SATURN    = 6;
	public static int SE_MEAN_NODE = 10;
	public static int SE_TRUE_NODE = 11;

	public static int SE_CALC_RISE         = 1;
	public static int SE_CALC_SET          = 2;
	public static int SE_CALC_MTRANSIT     = 4;
	public static int SE_CALC_ITRANSIT     = 8;
	public static int SE_BIT_DISC_CENTER   = 256;
	public static int SE_BIT_NO_REFRACTION = 512;

	public static int SE_WK_MONDAY    = 0;
	public static int SE_WK_TUESDAY   = 1;
	public static int SE_WK_WEDNESDAY = 2;
	public static int SE_WK_THURSDAY  = 3;
	public static int SE_WK_FRIDAY    = 4;
	public static int SE_WK_SATURDAY  = 5;
	public static int SE_WK_SUNDAY    = 6;


	private static Horoscope mCurrentLockHolder;
	private static object    SwephLockObject;

	public static void checkLock()
	{
		lock (SwephLockObject)
		{
			if (mCurrentLockHolder == null)
			{
				throw new Exception("Sweph: Unable to run. Sweph lock not obtained");
			}
		}
	}

	public static void obtainLock(Horoscope h)
	{
		if (SwephLockObject == null)
		{
			SwephLockObject = new object();
		}

		lock (SwephLockObject)
		{
			if (mCurrentLockHolder != null)
			{
				throw new Exception("Sweph: obtainLock failed. Sweph Lock still held");
			}

			//Debug.WriteLine("Sweph Lock obtained");
			mCurrentLockHolder = h;
			SetSidMode((int) h.options.Ayanamsa, 0.0, 0.0);
		}
	}

	public static void releaseLock(Horoscope h)
	{
		if (mCurrentLockHolder == null)
		{
			throw new Exception("Sweph: releaseLock failed. Lock not held");
		}

		if (mCurrentLockHolder != h)
		{
			throw new Exception("Sweph: releaseLock failed. Not lock owner");
		}

		//Debug.WriteLine("Sweph Lock released");
		mCurrentLockHolder = null;
	}

	public static int BodyNameToSweph(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return SE_SUN;
			case Body.BodyType.Moon:    return SE_MOON;
			case Body.BodyType.Mars:    return SE_MARS;
			case Body.BodyType.Mercury: return SE_MERCURY;
			case Body.BodyType.Jupiter: return SE_JUPITER;
			case Body.BodyType.Venus:   return SE_VENUS;
			case Body.BodyType.Saturn:  return SE_SATURN;
			case Body.BodyType.Lagna:   return SE_BIT_NO_REFRACTION;
			default:                    throw new Exception();
		}
	}

	public static void Close()
	{
		if (IntPtr.Size == 4)
		{
			Swe32.swe_close();
		}
		else
		{
			Swe64.swe_close();
		}
	}


	public static byte[] Version(byte[] buffer)
	{
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_version(buffer);
		}

		return Swe64.swe_version(buffer);
	}

	public static void SetPath(string path)
	{
		if (IntPtr.Size == 4)
		{
			Swe32.swe_set_ephe_path(path);
		}
		else
		{
			Swe64.swe_set_ephe_path(path);
		}
	}

	public static int DayOfWeek(double jd)
	{
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_day_of_week(jd);
		}

		return Swe64.swe_day_of_week(jd);
	}

	public static void SetSidMode(int sid_mode, double t0, double ayan_t0)
	{
		checkLock();
		if (IntPtr.Size == 4)
		{
			Swe32.swe_set_sid_mode(sid_mode, 0.0, 0.0);
		}
		else
		{
			Swe64.swe_set_sid_mode(sid_mode, 0.0, 0.0);
		}
	}

	public static double JulDay(int year, int month, int day, double hour)
	{
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_julday(year, month, day, hour, SE_GREG_CAL);
		}

		return Swe64.swe_julday(year, month, day, hour, SE_GREG_CAL);
	}

	public static double RevJul(double tjd, ref int year, ref int month, ref int day, ref double hour)
	{
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_revjul(tjd, 1, ref year, ref month, ref day, ref hour);
		}

		return Swe64.swe_revjul(tjd, 1, ref year, ref month, ref day, ref hour);
	}

	public static int CalcUT(double tjd_ut, int ipl, int addFlags, double[] xx, StringBuilder serr = null)
	{
		int ret;
		serr ??= new StringBuilder(256);

		checkLock();
		if (IntPtr.Size == 4)
		{
			ret = Swe32.swe_calc_ut(tjd_ut, ipl, iflag | addFlags, xx, serr);
		}
		else
		{
			ret = Swe64.swe_calc_ut(tjd_ut, ipl, iflag | addFlags, xx, serr);
		}

		if (ret >= 0)
		{
			xx[0] += mCurrentLockHolder.options.AyanamsaOffset.toDouble();
		}

		return ret;
	}

	public static int Calc(double tjd_ut, int ipl, int addFlags, double[] xx)
	{
		int ret;
		var serr = new StringBuilder(256);

		checkLock();
		if (IntPtr.Size == 4)
		{
			ret = Swe32.swe_calc(tjd_ut, ipl, iflag | addFlags, xx, serr);
		}
		else
		{
			ret = Swe64.swe_calc(tjd_ut, ipl, iflag | addFlags, xx, serr);
		}

		if (ret >= 0)
		{
			xx[0] += mCurrentLockHolder.options.AyanamsaOffset.toDouble();
		}

		return ret;
	}


	public static int SolEclipseWhenGlob(double tjd_ut, double[] tret, bool forward)
	{
		var serr = new StringBuilder(256);

		checkLock();
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_sol_eclipse_when_glob(tjd_ut, iflag, 0, tret, !forward, serr);
		}

		return Swe64.swe_sol_eclipse_when_glob(tjd_ut, iflag, 0, tret, !forward, serr);
	}

	public static int SolEclipseWhenLoc(HoraInfo hi, double tjd_ut, double[] tret, double[] attr, bool forward)
	{
		var serr = new StringBuilder(256);
		var geopos = new double[3]
		{
			hi.Longitude.toDouble(),
			hi.Latitude.toDouble(),
			hi.Altitude
		};

		checkLock();
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_sol_eclipse_when_loc(tjd_ut, iflag, geopos, tret, attr, !forward, serr);
		}

		return Swe64.swe_sol_eclipse_when_loc(tjd_ut, iflag, geopos, tret, attr, !forward, serr);
	}

	public static void LunEclipseWhen(double tjd_ut, double[] tret, bool forward)
	{
		int ret;
		var serr = new StringBuilder(256);

		checkLock();
		if (IntPtr.Size == 4)
		{
			ret = Swe32.swe_lun_eclipse_when(tjd_ut, iflag, 0, tret, !forward, serr);
		}
		else
		{
			ret = Swe64.swe_lun_eclipse_when(tjd_ut, iflag, 0, tret, !forward, serr);
		}

		if (ret < 0)
		{
			Application.Log.Debug("Sweph Error: {0}", serr);
			throw new SwephException(serr.ToString());
		}
	}

	public static double GetAyanamsaUT(double tjd_ut)
	{
		checkLock();
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_get_ayanamsa_ut(tjd_ut);
		}

		return Swe64.swe_get_ayanamsa_ut(tjd_ut);
	}

	public static int Rise(double tjd_ut, int ipl, int rsflag, double[] geopos, double atpress, double attemp, double[] tret, StringBuilder serr = null)
	{
		serr ??= new StringBuilder(256);

		checkLock();
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_rise_trans(tjd_ut, ipl, string.Empty, iflag, SE_CALC_RISE | rsflag, geopos, atpress, attemp, tret, serr);
		}

		return Swe64.swe_rise_trans(tjd_ut, ipl, string.Empty, iflag, SE_CALC_RISE | rsflag, geopos, atpress, attemp, tret, serr);
	}

	public static int Set(double tjd_ut, int ipl, int rsflag, double[] geopos, double atpress, double attemp, double[] tret)
	{
		var serr = new StringBuilder(256);

		checkLock();
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_rise_trans(tjd_ut, ipl, string.Empty, iflag, SE_CALC_SET | rsflag, geopos, atpress, attemp, tret, serr);
		}

		return Swe64.swe_rise_trans(tjd_ut, ipl, string.Empty, iflag, SE_CALC_SET | rsflag, geopos, atpress, attemp, tret, serr);
	}

	public static int Lmt(double tjd_ut, int ipl, int rsflag, double[] geopos, double atpress, double attemp, double[] tret)
	{
		var serr = new StringBuilder(256);

		checkLock();
		if (IntPtr.Size == 4)
		{
			return Swe32.swe_rise_trans(tjd_ut, ipl, string.Empty, iflag, rsflag, geopos, atpress, attemp, tret, serr);
		}

		return Swe64.swe_rise_trans(tjd_ut, ipl, string.Empty, iflag, rsflag, geopos, atpress, attemp, tret, serr);
	}


	public static int HousesEx(double tjd_ut, int iflag, double lat, double lon, int hsys, double[] cusps, double[] ascmc)
	{
		int ret;
		checkLock();

		if (IntPtr.Size == 4)
		{
			ret = Swe32.swe_houses_ex(tjd_ut, iflag, lat, lon, hsys, cusps, ascmc);
		}
		else
		{
			ret = Swe64.swe_houses_ex(tjd_ut, iflag, lat, lon, hsys, cusps, ascmc);
		}

		var lOffset = new Longitude(mCurrentLockHolder.options.AyanamsaOffset.toDouble());

		// House cusps defined from 1 to 12 inclusive as per sweph docs
		// Ascendants defined from 0 to 7 inclusive as per sweph docs
		for (var i = 1; i <= 12; i++)
		{
			cusps[i] = new Longitude(cusps[i]).add(lOffset).value;
		}

		for (var i = 0; i <= 7; i++)
		{
			ascmc[i] = new Longitude(ascmc[i]).add(lOffset).value;
		}

		return ret;
	}

	public static double Lagna(double tjd_ut)
	{
		checkLock();
		var hi    = mCurrentLockHolder.info;
		var cusps = new double[13];
		var ascmc = new double[10];
		var ret   = HousesEx(tjd_ut, SEFLG_SIDEREAL, hi.Latitude.toDouble(), hi.Longitude.toDouble(), 'R', cusps, ascmc);
		return ascmc[0];
	}

	/// <summary>
	///     This function must be called before topocentric planet positions for a certain birth place can be computed.
	///     It tells Swiss Ephemeris, what geographic position is to be used.
	///     Geographic longitude geolon and latitude geolat must be in degrees, the altitude above sea must be in meters.
	///     Neglecting the altitude can result in an error of about 2 arc seconds with the Moon and at an altitude 3000 m.
	///     After calling swe_set_topo(), add SEFLG_TOPOCTR to iflag and call swe_calc() as with an ordinary computation.
	/// </summary>
	/// <remarks>
	///     The parameters set by swe_set_topo() survive swe_close().
	/// </remarks>
	private static void SetTopo(double geolon, double geolat, double altitude)
	{
		if (IntPtr.Size == 4)
		{
			Swe32.swe_set_topo(geolon, geolat, altitude);
		}
		else
		{
			Swe64.swe_set_topo(geolon, geolat, altitude);
		}
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
			Swe32.swe_azalt(tjd_ut, calc_flag, geopos, atpress, attemp, xin, xaz);
		}
		else
		{
			Swe64.swe_azalt(tjd_ut, calc_flag, geopos, atpress, attemp, xin, xaz);
		}
	}


	private static class Swe64
	{
		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_close();

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern byte[] swe_version(byte[] svers);

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_set_ephe_path(string path);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_set_sid_mode")]
		public static extern void swe_set_sid_mode(int sid_mode, double t0, double ayan_t0);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_julday")]
		public static extern double swe_julday(int year, int month, int day, double hour, int gregflag);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_revjul")]
		public static extern double swe_revjul(double tjd, int gregflag, ref int year, ref int month, ref int day, ref double hour);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_glob")]
		public static extern int swe_sol_eclipse_when_glob(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		/* planets, moon, nodes etc. */
		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_calc")]
		public static extern int swe_calc(double tjd, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_calc_ut")]
		public static extern int swe_calc_ut(double tjd_ut, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_loc")]
		public static extern int swe_sol_eclipse_when_loc(double tjd_ut, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_eclipse_when")]
		public static extern int swe_lun_eclipse_when(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_occult_when_loc")]
		public static extern int swe_lun_occult_when_loc(double tjd_ut, int ipl, ref string starname, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_get_ayanamsa_ut")]
		public static extern double swe_get_ayanamsa_ut(double tjd_ut);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_rise_trans")]
		public static extern int swe_rise_trans(double tjd_ut, int ipl, string starname, int epheflag, int rsmi, double[] geopos, double atpress, double attemp, double[] tret, StringBuilder serr);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_houses_ex")]
		public static extern int swe_houses_ex(double tjd_ut, int iflag, double lat, double lon, int hsys, double[] cusps, double[] ascmc);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_day_of_week")]
		public static extern int swe_day_of_week(double jd);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_deltat")]
		public static extern double swe_deltat(double tjd_et);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_set_tid_acc")]
		public static extern void swe_set_tid_acc(double t_acc);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_time_equ")]
		public static extern int swe_time_equ(double tjd_et, ref double e, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_set_topo(double geolon, double geolat, double altitude);

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_azalt(double   tjd_ut,    // UT
		                                    int      calc_flag, // SE_ECL2HOR or SE_EQU2HOR
		                                    double[] geopos,    // array of 3 doubles: geograph. long., lat., height
		                                    double   atpress,   // atmospheric pressure in mbar (hPa)
		                                    double   attemp,    // atmospheric temperature in degrees Celsius
		                                    double[] xin,       // array of 3 doubles: position of body in either ecliptical or equatorial coordinates, depending on calc_flag
		                                    double[] xaz); // return array of 3 doubles, containing azimuth, true altitude, apparent altitude;
	}

	private static class Swe32
	{
		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_close();

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern byte[] swe_version(byte[] svers);

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_set_ephe_path(string path);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_set_sid_mode")]
		public static extern void swe_set_sid_mode(int sid_mode, double t0, double ayan_t0);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_julday")]
		public static extern double swe_julday(int year, int month, int day, double hour, int gregflag);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_revjul")]
		public static extern double swe_revjul(double tjd, int gregflag, ref int year, ref int month, ref int day, ref double hour);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_glob")]
		public static extern int swe_sol_eclipse_when_glob(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_calc")]
		public static extern int swe_calc(double tjd, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_calc_ut")]
		public static extern int swe_calc_ut(double tjd_ut, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_loc")]
		public static extern int swe_sol_eclipse_when_loc(double tjd_ut, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_eclipse_when")]
		public static extern int swe_lun_eclipse_when(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_occult_when_loc")]
		public static extern int swe_lun_occult_when_loc(double tjd_ut, int ipl, ref string starname, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);


		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_get_ayanamsa_ut")]
		public static extern double swe_get_ayanamsa_ut(double tjd_ut);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_rise_trans")]
		public static extern int swe_rise_trans(double tjd_ut, int ipl, string starname, int epheflag, int rsmi, double[] geopos, double atpress, double attemp, double[] tret, StringBuilder serr);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_houses_ex")]
		public static extern int swe_houses_ex(double tjd_ut, int iflag, double lat, double lon, int hsys, double[] cusps, double[] ascmc);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_day_of_week")]
		public static extern int swe_day_of_week(double jd);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_deltat")]
		public static extern double swe_deltat(double tjd_et);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_set_tid_acc")]
		public static extern void swe_set_tid_acc(double t_acc);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_time_equ")]
		public static extern int swe_time_equ(double tjd_et, ref double e, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_set_topo(double geolon, double geolat, double altitude);

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_azalt(double   tjd_ut,    // UT
		                                    int      calc_flag, // SE_ECL2HOR or SE_EQU2HOR
		                                    double[] geopos,    // array of 3 doubles: geograph. long., lat., height
		                                    double   atpress,   // atmospheric pressure in mbar (hPa)
		                                    double   attemp,    // atmospheric temperature in degrees Celsius
		                                    double[] xin,       // array of 3 doubles: position of body in either ecliptical or equatorial coordinates, depending on calc_flag
		                                    double[] xaz); // return array of 3 doubles, containing azimuth, true altitude, apparent altitude;
	}
}