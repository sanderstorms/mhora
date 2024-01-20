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
using System.Diagnostics;
using Mhora.SwissEph;
using Mhora.Tables;
using Mhora.Util;

namespace Mhora.Elements.Calculation;

/// <summary>
///     Summary description for Balas.
/// </summary>
public static class ShadBalas
{
	private static void VerifyGraha(this Horoscope h, Body.BodyType b)
	{
		var _b = (int) b;
		Debug.Assert(_b >= (int) Body.BodyType.Sun && _b <= (int) Body.BodyType.Saturn);
	}

	public static double UcchaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		var debLon = b.DebilitationDegree();
		var posLon = h.getPosition(b).longitude;
		var diff   = posLon.sub(debLon).value;
		if (diff > 180)
		{
			diff = 360 - diff;
		}

		return diff / 180.0 * 60.0;
	}

	public static bool GetsOjaBala(this Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Moon:
			case Body.BodyType.Venus: return false;
			default: return true;
		}
	}

	public static double OjaYugmaHelper(this Body.BodyType b, ZodiacHouse zh)
	{
		if (GetsOjaBala(b))
		{
			if (zh.IsOdd())
			{
				return 15.0;
			}

			return 0.0;
		}

		if (zh.IsOdd())
		{
			return 0.0;
		}

		return 15.0;
	}

	public static double OjaYugmaRasyAmsaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		var    bp      = h.getPosition(b);
		var    zh_rasi = bp.toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house;
		var    zh_amsa = bp.toDivisionPosition(new Division(Vargas.DivisionType.Navamsa)).zodiac_house;
		double s       = 0;
		s += OjaYugmaHelper(b, zh_rasi);
		s += OjaYugmaHelper(b, zh_amsa);
		return s;
	}

	public static double KendraBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		var zh_b = h.getPosition(b).toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house;
		var zh_l = h.getPosition(Body.BodyType.Lagna).toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house;
		var diff = zh_l.NumHousesBetween(zh_b);
		switch (diff % 3)
		{
			case 1: return 60;
			case 2: return 30.0;
			case 0:
			default: return 15.0;
		}
	}

	public static double DrekkanaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		var part = h.getPosition(b).partOfZodiacHouse(3);
		if (part == 1 && (b == Body.BodyType.Sun || b == Body.BodyType.Jupiter || b == Body.BodyType.Mars))
		{
			return 15.0;
		}

		if (part == 2 && (b == Body.BodyType.Saturn || b == Body.BodyType.Mercury))
		{
			return 15.0;
		}

		if (part == 3 && (b == Body.BodyType.Moon || b == Body.BodyType.Venus))
		{
			return 15.0;
		}

		return 0;
	}

	public static double DigBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		int[] powerlessHouse =
		{
			4,
			10,
			4,
			7,
			7,
			10,
			1
		};
		var lagLon = h.getPosition(Body.BodyType.Lagna).longitude;
		var debLon = new Longitude(lagLon.toZodiacHouseBase());
		debLon = debLon.add(powerlessHouse[(int) b] * 30.0 + 15.0);
		var posLon = h.getPosition(b).longitude;

		Application.Log.Debug("digBala {0} {1} {2}", b, posLon.value, debLon.value);

		var diff = posLon.sub(debLon).value;
		if (diff > 180)
		{
			diff = 360 - diff;
		}

		return diff / 180.0 * 60.0;
	}

	public static double NathonnathaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);

		if (b == Body.BodyType.Mercury)
		{
			return 60;
		}

		var    lmt_midnight = h.lmt_offset * 24.0;
		var    lmt_noon     = 12.0 + h.lmt_offset * 24.0;
		double diff         = 0;
		var    time = h.info.DateOfBirth.Time().TotalHours;
		if (time > lmt_noon)
		{
			diff = lmt_midnight - time;
		}
		else
		{
			diff = time - lmt_midnight;
		}

		while (diff < 0)
		{
			diff += 12.0;
		}

		diff = diff / 12.0 * 60.0;

		if (b == Body.BodyType.Moon || b == Body.BodyType.Mars || b == Body.BodyType.Saturn)
		{
			diff = 60 - diff;
		}

		return diff;
	}

	public static double PakshaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);

		var mlon = h.getPosition(Body.BodyType.Moon).longitude;
		var slon = h.getPosition(Body.BodyType.Sun).longitude;

		var diff = mlon.sub(slon).value;
		if (diff > 180)
		{
			diff = 360.0 - diff;
		}

		var shubha = diff / 3.0;
		var paapa  = 60.0 - shubha;

		switch (b)
		{
			case Body.BodyType.Sun:
			case Body.BodyType.Mars:
			case Body.BodyType.Saturn: return paapa;
			case Body.BodyType.Moon: return shubha * 2.0;
			default:
			case Body.BodyType.Mercury:
			case Body.BodyType.Jupiter:
			case Body.BodyType.Venus: return shubha;
		}
	}

	public static double TribhaagaBala(this Horoscope h, Body.BodyType b)
	{
		var ret = Body.BodyType.Jupiter;
		h.VerifyGraha(b);
		if (h.isDayBirth())
		{
			var length = (h.sunset - h.sunrise) / 3;
			var offset = h.info.DateOfBirth.Time ().TotalHours - h.sunrise;
			var part   = (int) Math.Floor(offset / length);
			switch (part)
			{
				case 0:
					ret = Body.BodyType.Mercury;
					break;
				case 1:
					ret = Body.BodyType.Sun;
					break;
				case 2:
					ret = Body.BodyType.Saturn;
					break;
			}
		}
		else
		{
			var length = (h.next_sunrise + 24.0 - h.sunset) / 3;
			var offset = h.info.DateOfBirth.Time ().TotalHours - h.sunset;
			if (offset < 0)
			{
				offset += 24;
			}

			var part = (int) Math.Floor(offset / length);
			switch (part)
			{
				case 0:
					ret = Body.BodyType.Moon;
					break;
				case 1:
					ret = Body.BodyType.Venus;
					break;
				case 2:
					ret = Body.BodyType.Mars;
					break;
			}
		}

		if (b == Body.BodyType.Jupiter || b == ret)
		{
			return 60;
		}

		return 0;
	}

	public static double NaisargikaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		switch (b)
		{
			case Body.BodyType.Sun:     return 60;
			case Body.BodyType.Moon:    return 51.43;
			case Body.BodyType.Mars:    return 17.14;
			case Body.BodyType.Mercury: return 25.70;
			case Body.BodyType.Jupiter: return 34.28;
			case Body.BodyType.Venus:   return 42.85;
			case Body.BodyType.Saturn:  return 8.57;
		}

		return 0;
	}

	public static void KalaHelper(this Horoscope h, ref Body.BodyType yearLord, ref Body.BodyType monthLord)
	{
		var date       = new DateTime(1827, 5, 2);
		var dstOffset  = h.info.City.Country.TimeZone.TimeZoneInfo.GetUtcOffset(date);
		var ut_arghana = sweph.JulDay(1827, 5, 2, -dstOffset.TotalHours + 12.0 / 24.0);
		var ut_noon    = h.info.Jd - h.info.DateOfBirth.Time ().TotalDays + 12.0 / 24.0;

		var diff = ut_noon - ut_arghana;
		if (diff >= 0)
		{
			var quo = Math.Floor(diff / 360.0);
			diff -= quo * 360.0;
		}
		else
		{
			var pdiff = -diff;
			var quo   = Math.Ceiling(pdiff / 360.0);
			diff += quo * 360.0;
		}

		var diff_year = diff;
		while (diff > 30.0)
		{
			diff -= 30.0;
		}

		var diff_month = diff;
		while (diff > 7)
		{
			diff -= 7.0;
		}

		yearLord  = ((Tables.Hora.Weekday) sweph.DayOfWeek(ut_noon - diff_year)).WeekdayRuler();
		monthLord = ((Tables.Hora.Weekday) sweph.DayOfWeek(ut_noon - diff_month)).WeekdayRuler();
	}

	public static double AbdaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		Body.BodyType yearLord = Body.BodyType.Sun, monthLord = Body.BodyType.Sun;
		h.KalaHelper(ref yearLord, ref monthLord);
		if (yearLord == b)
		{
			return 15.0;
		}

		return 0.0;
	}

	public static double MasaBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		Body.BodyType yearLord = Body.BodyType.Sun, monthLord = Body.BodyType.Sun;
		h.KalaHelper(ref yearLord, ref monthLord);
		if (monthLord == b)
		{
			return 30.0;
		}

		return 0.0;
	}

	public static double VaraBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		if (h.wday.WeekdayRuler() == b)
		{
			return 45.0;
		}

		return 0.0;
	}

	public static double HoraBala(this Horoscope h, Body.BodyType b)
	{
		h.VerifyGraha(b);
		if (h.calculateHora() == b)
		{
			return 60.0;
		}

		return 0.0;
	}
}