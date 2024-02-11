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
using Mhora.Definitions;
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
	/// <param name="x">The value to be normalized</param>
	/// <param name="lower">The lower bound of normalization</param>
	/// <param name="upper">The upper bound of normalization</param>
	/// <returns>
	///     The normalized value of x, where lower <= x <= upper </returns>
	public static int NormalizeInc(this int x, int lower, int upper)
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
	/// <param name="x">The value to be normalized</param>
	/// <param name="lower">The lower bound of normalization</param>
	/// <param name="upper">The upper bound of normalization</param>
	/// <returns>
	///     The normalized value of x, where lower = x <= upper </returns>
	public static double NormalizeExc(this double x, double lower, double upper)
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

	public static ZodiacHouse GetMoolaTrikonaRasi(this Body b)
	{
		var z = ZodiacHouse.Ari;
		switch (b)
		{
			case Body.Sun:
				z = ZodiacHouse.Leo;
				break;
			case Body.Moon:
				z = ZodiacHouse.Tau;
				break;
			case Body.Mars:
				z = ZodiacHouse.Ari;
				break;
			case Body.Mercury:
				z = ZodiacHouse.Vir;
				break;
			case Body.Jupiter:
				z = ZodiacHouse.Sag;
				break;
			case Body.Venus:
				z = ZodiacHouse.Lib;
				break;
			case Body.Saturn:
				z = ZodiacHouse.Aqu;
				break;
			case Body.Rahu:
				z = ZodiacHouse.Vir;
				break;
			case Body.Ketu:
				z = ZodiacHouse.Pis;
				break;
		}

		return (z);
	}




	/// <summary>
	///     Specify the Lord of a ZodiacHouse. The owernership of the nodes is not considered
	/// </summary>
	/// <param name="zh">The House whose lord should be returned</param>
	/// <returns>The lord of zh</returns>
	public static Body SimpleLordOfZodiacHouse(this ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Ari: return Body.Mars;
			case ZodiacHouse.Tau: return Body.Venus;
			case ZodiacHouse.Gem: return Body.Mercury;
			case ZodiacHouse.Can: return Body.Moon;
			case ZodiacHouse.Leo: return Body.Sun;
			case ZodiacHouse.Vir: return Body.Mercury;
			case ZodiacHouse.Lib: return Body.Venus;
			case ZodiacHouse.Sco: return Body.Mars;
			case ZodiacHouse.Sag: return Body.Jupiter;
			case ZodiacHouse.Cap: return Body.Saturn;
			case ZodiacHouse.Aqu: return Body.Saturn;
			case ZodiacHouse.Pis: return Body.Jupiter;
		}

		Trace.Assert(false, string.Format("Basics.SimpleLordOfZodiacHouse for {0} failed", (int) zh));
		return Body.Other;
	}


	public static Longitude CalculateBodyLongitude(this Horoscope h, double ut, int ipl)
	{
		var sterr    = new StringBuilder();
		var position = new double[6];

		var result = h.CalcUT(h.Info.Jd, ipl, 0, position);

		if (result == sweph.ERR)
		{
			throw new SwedllException(sterr.ToString());
		}
		/*
		var bodyPositin = new BodyPosition
		{
			Longitude      = position[0],
			Latitude       = position[1],
			Distance       = position[2],
			LongitudeSpeed = position[3],
			LatitudeSpeed  = position[4],
			DistanceSpeed  = position[5]
		};
		*/
		return new Longitude (position[0]);
	}

	/// <summary>
	///     Calculated a BodyPosition for a given time and place using the swiss ephemeris
	/// </summary>
	/// <param name="ut">The time for which the calculations should be performed</param>
	/// <param name="ipl">The Swiss Ephemeris body Type</param>
	/// <param name="body">The local application body name</param>
	/// <param name="bodyType">The local application body type</param>
	/// <returns>A BodyPosition class</returns>
	public static Position CalculateSingleBodyPosition(this Horoscope h, double ut, int ipl, Body body, BodyType bodyType)
	{
		if (body == Body.Lagna)
		{
			var b = new Position(h, body, bodyType, new Longitude(h.Lagna(ut)), 0, 0, 0, 0, 0);
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

			var b = new Position(h, body, bodyType, new Longitude(xx[0]), xx[1], xx[2], xx[3], xx[4], xx[5]);
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
	public static ArrayList CalculateBodyPositions(this Horoscope h, double sunrise)
	{
		var hi = h.Info;
		var o  = h.Options;

		var serr      = new StringBuilder(256);
		var ephePath = MhoraGlobalOptions.Instance.HOptions.EphemerisPath;

		// The order of the array must reflect the order define in Basics.GrahaIndex
		var stdGrahas = new ArrayList(20);

		sweph.SetEphePath(ephePath);
		var juldayUt = h.UniversalTime(hi.DateOfBirth); // (hi.tob - hi.DstOffset).ToJulian();

		var swephRahuBody = sweph.SE_MEAN_NODE;
		if (o.NodeType == HoroscopeOptions.ENodeType.True)
		{
			swephRahuBody = sweph.SE_TRUE_NODE;
		}

		var addFlags = 0;
		if (o.GrahaPositionType == HoroscopeOptions.EGrahaPositionType.True)
		{
			addFlags = sweph.SEFLG_TRUEPOS;
		}

		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_SUN, Body.Sun, BodyType.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_MOON, Body.Moon, BodyType.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_MARS, Body.Mars, BodyType.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_MERCURY, Body.Mercury, BodyType.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_JUPITER, Body.Jupiter, BodyType.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_VENUS, Body.Venus, BodyType.Graha));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_SATURN, Body.Saturn, BodyType.Graha));
		var rahu = h.CalculateSingleBodyPosition(juldayUt, swephRahuBody, Body.Rahu, BodyType.Graha);

		var ketu = h.CalculateSingleBodyPosition(juldayUt, swephRahuBody, Body.Ketu, BodyType.Graha);
		ketu.Longitude = rahu.Longitude.Add(new Longitude(180.0));
		stdGrahas.Add(rahu);
		stdGrahas.Add(ketu);

		var asc = h.Lagna(juldayUt);
		stdGrahas.Add(new Position(h, Body.Lagna, BodyType.Lagna, new Longitude(asc), 0, 0, 0, 0, 0));

		var istaGhati = NormalizeExc(hi.DateOfBirth.Time ().TotalHours - sunrise, 0.0, 24.0) * 2.5;
		var glLon     = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(istaGhati        * 30.0));
		var hlLon     = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(istaGhati * 30.0 / 2.5));
		var blLon     = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(istaGhati * 30.0 / 5.0));

		var vl = istaGhati * 5.0;
		while (istaGhati > 12.0)
		{
			istaGhati -= 12.0;
		}

		var vlLon = ((Position) stdGrahas[0]).Longitude.Add(new Longitude(vl * 30.0));

		stdGrahas.Add(new Position(h, Body.BhavaLagna, BodyType.SpecialLagna, blLon, 0, 0, 0, 0, 0));
		stdGrahas.Add(new Position(h, Body.HoraLagna, BodyType.SpecialLagna, hlLon, 0, 0, 0, 0, 0));
		stdGrahas.Add(new Position(h, Body.GhatiLagna, BodyType.SpecialLagna, glLon, 0, 0, 0, 0, 0));
		stdGrahas.Add(new Position(h, Body.VighatiLagna, BodyType.SpecialLagna, vlLon, 0, 0, 0, 0, 0));


		return stdGrahas;
	}
}