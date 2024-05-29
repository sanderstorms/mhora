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
using System.Collections.Generic;
using System.Text;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.SwissEph;
using Mhora.SwissEph.Helpers;
using Mhora.Util;

namespace Mhora.Calculation;

/// <summary>
///     Simple functions that don't belong anywhere else
/// </summary>
public static class Basics
{


	public static Longitude CalculateBodyLongitude(this Horoscope h, JulianDate ut, int ipl)
	{
		var sterr    = new StringBuilder();
		var position = new double[6];

		var result = h.CalcUT(ut, ipl, 0, position);

		if (result == sweph.ERR)
		{
			throw new SwedllException(sterr.ToString());
		}
		/*
		var bodyPosition = new BodyPosition
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
	public static Position CalculateSingleBodyPosition(this Horoscope h, JulianDate ut, int ipl, Body body, BodyType bodyType)
	{
		if (body == Body.Lagna)
		{
			var b = new Position(h, body, bodyType, new Longitude(h.Lagna(ut)));
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
	public static List<Position> CalculateBodyPositions(this Horoscope h, JulianDate sunrise)
	{
		var hi = h.Info;
		var o  = h.Options;

		var serr      = new StringBuilder(256);
		var ephePath = MhoraGlobalOptions.Instance.HOptions.EphemerisPath;

		// The order of the array must reflect the order define in Basics.GrahaIndex
		var stdGrahas = new List<Position>();

		sweph.SetEphePath(ephePath);
		var juldayUt      = h.Info.Jd;
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
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_URANUS, Body.Uranus, BodyType.OuterPlanet));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_NEPTUNE, Body.Neptune, BodyType.OuterPlanet));
		stdGrahas.Add(h.CalculateSingleBodyPosition(juldayUt, sweph.SE_PLUTO, Body.Pluto, BodyType.OuterPlanet));
		var rahu = h.CalculateSingleBodyPosition(juldayUt, swephRahuBody, Body.Rahu, BodyType.Graha);

		var ketu = h.CalculateSingleBodyPosition(juldayUt, swephRahuBody, Body.Ketu, BodyType.Graha);
		ketu.Longitude = rahu.Longitude.Add(new Longitude(180.0));
		stdGrahas.Add(rahu);
		stdGrahas.Add(ketu);

		var asc = h.Lagna(juldayUt);
		stdGrahas.Add(new Position(h, Body.Lagna, BodyType.Lagna, new Longitude(asc)));

		//Bhaav    Lagna: changes sign every 5 Ghatees (every 2 hours)
		//Horaa Lagna: changes sign every 2.5 Ghatees (each hour)
		//Ghatee   Lagna: changes sign every Ghatee (every 24 min)
		//Vighatee Lagna: changes sign every Vighatee (every 24 sec)

		var istaGhati = h.Vara.Isthaghati.Ghati;
		var blLon     = stdGrahas[0].Longitude.Add(new Longitude(istaGhati * 6));			//bhava = 2 ghati = 30 degrees
		var hlLon     = stdGrahas[0].Longitude.Add(new Longitude(istaGhati * 12));			//hora = 15 degrees
		var glLon     = stdGrahas[0].Longitude.Add(new Longitude(istaGhati * 12 * 2.5));
		var vlLon     = stdGrahas[0].Longitude.Add(new Longitude(istaGhati * 12 * 2.5 * 2.5 * 6));

		stdGrahas.Add(new Position(h, Body.BhavaLagna, BodyType.SpecialLagna, blLon));
		stdGrahas.Add(new Position(h, Body.HoraLagna, BodyType.SpecialLagna, hlLon));
		stdGrahas.Add(new Position(h, Body.GhatiLagna, BodyType.SpecialLagna, glLon));
		stdGrahas.Add(new Position(h, Body.VighatiLagna, BodyType.SpecialLagna, vlLon));


		return stdGrahas;
	}
}