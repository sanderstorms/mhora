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
using System.Diagnostics;
using System.Text;
using Mhora.Database.Settings;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;
using Mhora.SwissEph.Helpers;
using Mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     Simple functions that don't belong anywhere else
/// </summary>
public static class Basics
{



	/// <summary>
	///     Normalize a number between bounds
	/// </summary>
	/// <param name="lower">The lower bound of normalization</param>
	/// <param name="upper">The upper bound of normalization</param>
	/// <param name="x">The value to be normalized</param>
	/// <returns>
	///     The normalized value of x, where lower <= x <= upper </returns>
	public static int NormalizeInc(int lower, int upper, int x)
	{
		var size = upper - lower + 1;
		while (x > upper)
		{
			x -= size;
		}

		while (x < lower)
		{
			x += size;
		}

		Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
		return x;
	}

	/// <summary>
	///     Normalize a number between bounds
	/// </summary>
	/// <param name="lower">The lower bound of normalization</param>
	/// <param name="upper">The upper bound of normalization</param>
	/// <param name="x">The value to be normalized</param>
	/// <returns>
	///     The normalized value of x, where lower = x <= upper </returns>
	public static double NormalizeExc(double lower, double upper, double x)
	{
		var size = upper - lower;
		while (x > upper)
		{
			x -= size;
		}

		while (x <= lower)
		{
			x += size;
		}

		Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
		return x;
	}

	public static double NormalizeExcLower(this double x, double lower, double upper)
	{
		var size = upper - lower;
		while (x >= upper)
		{
			x -= size;
		}

		while (x < lower)
		{
			x += size;
		}

		Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
		return x;
	}

	public static ZodiacHouse GetMoolaTrikonaRasi(this Body.BodyType b)
	{
		var z = ZodiacHouse.Rasi.Ari;
		switch (b)
		{
			case Body.BodyType.Sun:
				z = ZodiacHouse.Rasi.Leo;
				break;
			case Body.BodyType.Moon:
				z = ZodiacHouse.Rasi.Tau;
				break;
			case Body.BodyType.Mars:
				z = ZodiacHouse.Rasi.Ari;
				break;
			case Body.BodyType.Mercury:
				z = ZodiacHouse.Rasi.Vir;
				break;
			case Body.BodyType.Jupiter:
				z = ZodiacHouse.Rasi.Sag;
				break;
			case Body.BodyType.Venus:
				z = ZodiacHouse.Rasi.Lib;
				break;
			case Body.BodyType.Saturn:
				z = ZodiacHouse.Rasi.Aqu;
				break;
			case Body.BodyType.Rahu:
				z = ZodiacHouse.Rasi.Vir;
				break;
			case Body.BodyType.Ketu:
				z = ZodiacHouse.Rasi.Pis;
				break;
		}

		return new ZodiacHouse(z);
	}




	/// <summary>
	///     Specify the Lord of a ZodiacHouse. The owernership of the nodes is not considered
	/// </summary>
	/// <param name="zh">The House whose lord should be returned</param>
	/// <returns>The lord of zh</returns>
	public static Body.BodyType SimpleLordOfZodiacHouse(this ZodiacHouse.Rasi zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Rasi.Ari: return Body.BodyType.Mars;
			case ZodiacHouse.Rasi.Tau: return Body.BodyType.Venus;
			case ZodiacHouse.Rasi.Gem: return Body.BodyType.Mercury;
			case ZodiacHouse.Rasi.Can: return Body.BodyType.Moon;
			case ZodiacHouse.Rasi.Leo: return Body.BodyType.Sun;
			case ZodiacHouse.Rasi.Vir: return Body.BodyType.Mercury;
			case ZodiacHouse.Rasi.Lib: return Body.BodyType.Venus;
			case ZodiacHouse.Rasi.Sco: return Body.BodyType.Mars;
			case ZodiacHouse.Rasi.Sag: return Body.BodyType.Jupiter;
			case ZodiacHouse.Rasi.Cap: return Body.BodyType.Saturn;
			case ZodiacHouse.Rasi.Aqu: return Body.BodyType.Saturn;
			case ZodiacHouse.Rasi.Pis: return Body.BodyType.Jupiter;
		}

		Trace.Assert(false, string.Format("Basics.SimpleLordOfZodiacHouse for {0} failed", (int) zh));
		return Body.BodyType.Other;
	}


	public static Longitude CalculateBodyLongitude(this Horoscope h, double ut, int ipl)
	{
		var xx = new double[6]
		{
			0,
			0,
			0,
			0,
			0,
			0
		};
		try
		{
			h.CalcUT(ut, ipl, 0, xx);
			return new Longitude(xx[0]);
		}
		catch (SwephException exc)
		{
			Application.Log.Debug("Sweph: {0}\n", exc.status);
			throw new Exception(string.Empty);
		}
	}

	/// <summary>
	///     Calculated a BodyPosition for a given time and place using the swiss ephemeris
	/// </summary>
	/// <param name="ut">The time for which the calculations should be performed</param>
	/// <param name="ipl">The Swiss Ephemeris body Type</param>
	/// <param name="body">The local application body name</param>
	/// <param name="type">The local application body type</param>
	/// <returns>A BodyPosition class</returns>
	public static Position CalculateSingleBodyPosition(this Horoscope h, double ut, int ipl, Body.BodyType body, Body.Type type)
	{
		if (body == Body.BodyType.Lagna)
		{
			var b = new Position(h, body, type, new Longitude(h.Lagna(ut)), 0, 0, 0, 0, 0);
			return b;
		}

		var xx = new double[6]
		{
			0,
			0,
			0,
			0,
			0,
			0
		};
		try
		{
			h.CalcUT(ut, ipl, 0, xx);

			var b = new Position(h, body, type, new Longitude(xx[0]), xx[1], xx[2], xx[3], xx[4], xx[5]);
			return b;
		}
		catch (SwephException exc)
		{
			Application.Log.Debug("Sweph: {0}\n", exc.status);
			throw new Exception(string.Empty);
		}
	}


	/// <summary>
	///     Given a HoraInfo object (all required user inputs), calculate a list of
	///     all bodypositions we will ever require
	/// </summary>
	/// <param name="h">The HoraInfo object</param>
	/// <returns></returns>
	public static ArrayList CalculateBodyPositions(Horoscope h, double sunrise)
	{
		var hi = h.Info;
		var o  = h.Options;

		var serr      = new StringBuilder(256);
		var ephePath = MhoraGlobalOptions.Instance.HOptions.EphemerisPath;

		// The order of the array must reflect the order define in Basics.GrahaIndex
		var stdGrahas = new ArrayList(20);

		sweph.SetEphePath(ephePath);
		var juldayUt = h.UniversalTime(hi.DateOfBirth); // (hi.tob - hi.DstOffset).UniversalTime();

		var swephRahuBody = sweph.SE_MEAN_NODE;
		if (o.nodeType == HoroscopeOptions.ENodeType.True)
		{
			swephRahuBody = sweph.SE_TRUE_NODE;
		}

		var addFlags = 0;
		if (o.grahaPositionType == HoroscopeOptions.EGrahaPositionType.True)
		{
			addFlags = sweph.SEFLG_TRUEPOS;
		}

		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_SUN, Body.BodyType.Sun, Body.Type.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_MOON, Body.BodyType.Moon, Body.Type.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_MARS, Body.BodyType.Mars, Body.Type.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_MERCURY, Body.BodyType.Mercury, Body.Type.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_JUPITER, Body.BodyType.Jupiter, Body.Type.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_VENUS, Body.BodyType.Venus, Body.Type.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_SATURN, Body.BodyType.Saturn, Body.Type.Graha));
		var rahu = h.CalculateSingleBodyPosition(juldayUt, swephRahuBody, Body.BodyType.Rahu, Body.Type.Graha);

		var ketu = h.CalculateSingleBodyPosition(juldayUt, swephRahuBody, Body.BodyType.Ketu, Body.Type.Graha);
		ketu.Longitude = rahu.Longitude.Add(new Longitude(180.0));
		stdGrahas.Add(rahu);
		stdGrahas.Add(ketu);

		var asc = h.Lagna(juldayUt);
		stdGrahas.Add(new Position(h, Body.BodyType.Lagna, Body.Type.Lagna, new Longitude(asc), 0, 0, 0, 0, 0));

		var istaGhati = NormalizeExc(0.0, 24.0, hi.DateOfBirth.Time ().TotalHours - sunrise) * 2.5;
		var glLon     = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(istaGhati        * 30.0));
		var hlLon     = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(istaGhati * 30.0 / 2.5));
		var blLon     = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(istaGhati * 30.0 / 5.0));

		var vl = istaGhati * 5.0;
		while (istaGhati > 12.0)
		{
			istaGhati -= 12.0;
		}

		var vlLon = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(vl * 30.0));

		stdGrahas.Add(new Position(h, Body.BodyType.BhavaLagna, Body.Type.SpecialLagna, blLon, 0, 0, 0, 0, 0));
		stdGrahas.Add(new Position(h, Body.BodyType.HoraLagna, Body.Type.SpecialLagna, hlLon, 0, 0, 0, 0, 0));
		stdGrahas.Add(new Position(h, Body.BodyType.GhatiLagna, Body.Type.SpecialLagna, glLon, 0, 0, 0, 0, 0));
		stdGrahas.Add(new Position(h, Body.BodyType.VighatiLagna, Body.Type.SpecialLagna, vlLon, 0, 0, 0, 0, 0));


		return stdGrahas;
	}
}