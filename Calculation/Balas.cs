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
using Mhora.Varga;

namespace Mhora.Calculation;

/// <summary>
///     Summary description for Balas.
/// </summary>
public class ShadBalas
{
	private readonly Horoscope h;

	public ShadBalas(Horoscope _h)
	{
		h = _h;
	}

	private void verifyGraha(Tables.Body.Name b)
	{
		var _b = (int) b;
		Debug.Assert(_b >= (int) Tables.Body.Name.Sun && _b <= (int) Tables.Body.Name.Saturn);
	}

	public double ucchaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		var debLon = Tables.Body.debilitationDegree(b);
		var posLon = h.getPosition(b).longitude;
		var diff   = posLon.sub(debLon).value;
		if (diff > 180)
		{
			diff = 360 - diff;
		}

		return diff / 180.0 * 60.0;
	}

	public bool getsOjaBala(Tables.Body.Name b)
	{
		switch (b)
		{
			case Tables.Body.Name.Moon:
			case Tables.Body.Name.Venus:
				return false;
			default: return true;
		}
	}

	public double ojaYugmaHelper(Tables.Body.Name b, ZodiacHouse zh)
	{
		if (getsOjaBala(b))
		{
			if (zh.isOdd())
			{
				return 15.0;
			}

			return 0.0;
		}

		if (zh.isOdd())
		{
			return 0.0;
		}

		return 15.0;
	}

	public double ojaYugmaRasyAmsaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		var    bp      = h.getPosition(b);
		var    zh_rasi = bp.toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house;
		var    zh_amsa = bp.toDivisionPosition(new Division(Basics.DivisionType.Navamsa)).zodiac_house;
		double s       = 0;
		s += ojaYugmaHelper(b, zh_rasi);
		s += ojaYugmaHelper(b, zh_amsa);
		return s;
	}

	public double kendraBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		var zh_b = h.getPosition(b).toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house;
		var zh_l = h.getPosition(Tables.Body.Name.Lagna).toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house;
		var diff = zh_l.numHousesBetween(zh_b);
		switch (diff % 3)
		{
			case 1: return 60;
			case 2: return 30.0;
			case 0:
			default:
				return 15.0;
		}
	}

	public double drekkanaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		var part = h.getPosition(b).partOfZodiacHouse(3);
		if (part == 1 && (b == Tables.Body.Name.Sun || b == Tables.Body.Name.Jupiter || b == Tables.Body.Name.Mars))
		{
			return 15.0;
		}

		if (part == 2 && (b == Tables.Body.Name.Saturn || b == Tables.Body.Name.Mercury))
		{
			return 15.0;
		}

		if (part == 3 && (b == Tables.Body.Name.Moon || b == Tables.Body.Name.Venus))
		{
			return 15.0;
		}

		return 0;
	}

	public double digBala(Tables.Body.Name b)
	{
		verifyGraha(b);
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
		var lagLon = h.getPosition(Tables.Body.Name.Lagna).longitude;
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

	public double nathonnathaBala(Tables.Body.Name b)
	{
		verifyGraha(b);

		if (b == Tables.Body.Name.Mercury)
		{
			return 60;
		}

		var    lmt_midnight = h.lmt_offset * 24.0;
		var    lmt_noon     = 12.0 + h.lmt_offset * 24.0;
		double diff         = 0;
		if (h.info.tob.time > lmt_noon)
		{
			diff = lmt_midnight - h.info.tob.time;
		}
		else
		{
			diff = h.info.tob.time - lmt_midnight;
		}

		while (diff < 0)
		{
			diff += 12.0;
		}

		diff = diff / 12.0 * 60.0;

		if (b == Tables.Body.Name.Moon || b == Tables.Body.Name.Mars || b == Tables.Body.Name.Saturn)
		{
			diff = 60 - diff;
		}

		return diff;
	}

	public double pakshaBala(Tables.Body.Name b)
	{
		verifyGraha(b);

		var mlon = h.getPosition(Tables.Body.Name.Moon).longitude;
		var slon = h.getPosition(Tables.Body.Name.Sun).longitude;

		var diff = mlon.sub(slon).value;
		if (diff > 180)
		{
			diff = 360.0 - diff;
		}

		var shubha = diff / 3.0;
		var paapa  = 60.0 - shubha;

		switch (b)
		{
			case Tables.Body.Name.Sun:
			case Tables.Body.Name.Mars:
			case Tables.Body.Name.Saturn:
				return paapa;
			case Tables.Body.Name.Moon: return shubha * 2.0;
			default:
			case Tables.Body.Name.Mercury:
			case Tables.Body.Name.Jupiter:
			case Tables.Body.Name.Venus:
				return shubha;
		}
	}

	public double tribhaagaBala(Tables.Body.Name b)
	{
		var ret = Tables.Body.Name.Jupiter;
		verifyGraha(b);
		if (h.isDayBirth())
		{
			var length = (h.sunset - h.sunrise) / 3;
			var offset = h.info.tob.time - h.sunrise;
			var part   = (int) Math.Floor(offset / length);
			switch (part)
			{
				case 0:
					ret = Tables.Body.Name.Mercury;
					break;
				case 1:
					ret = Tables.Body.Name.Sun;
					break;
				case 2:
					ret = Tables.Body.Name.Saturn;
					break;
			}
		}
		else
		{
			var length = (h.next_sunrise + 24.0 - h.sunset) / 3;
			var offset = h.info.tob.time - h.sunset;
			if (offset < 0)
			{
				offset += 24;
			}

			var part = (int) Math.Floor(offset / length);
			switch (part)
			{
				case 0:
					ret = Tables.Body.Name.Moon;
					break;
				case 1:
					ret = Tables.Body.Name.Venus;
					break;
				case 2:
					ret = Tables.Body.Name.Mars;
					break;
			}
		}

		if (b == Tables.Body.Name.Jupiter || b == ret)
		{
			return 60;
		}

		return 0;
	}

	public double naisargikaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		switch (b)
		{
			case Tables.Body.Name.Sun:     return 60;
			case Tables.Body.Name.Moon:    return 51.43;
			case Tables.Body.Name.Mars:    return 17.14;
			case Tables.Body.Name.Mercury: return 25.70;
			case Tables.Body.Name.Jupiter: return 34.28;
			case Tables.Body.Name.Venus:   return 42.85;
			case Tables.Body.Name.Saturn:  return 8.57;
		}

		return 0;
	}

	public void kalaHelper(ref Tables.Body.Name yearLord, ref Tables.Body.Name monthLord)
	{
		var ut_arghana = sweph.JulDay(1827, 5, 2, -h.info.tz.toDouble() + 12.0 / 24.0);
		var ut_noon    = h.baseUT - h.info.tob.time / 24.0 + 12.0 / 24.0;

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

		yearLord  = Basics.weekdayRuler((Basics.Weekday) sweph.DayOfWeek(ut_noon - diff_year));
		monthLord = Basics.weekdayRuler((Basics.Weekday) sweph.DayOfWeek(ut_noon - diff_month));
	}

	public double abdaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		Tables.Body.Name yearLord = Tables.Body.Name.Sun, monthLord = Tables.Body.Name.Sun;
		kalaHelper(ref yearLord, ref monthLord);
		if (yearLord == b)
		{
			return 15.0;
		}

		return 0.0;
	}

	public double masaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		Tables.Body.Name yearLord = Tables.Body.Name.Sun, monthLord = Tables.Body.Name.Sun;
		kalaHelper(ref yearLord, ref monthLord);
		if (monthLord == b)
		{
			return 30.0;
		}

		return 0.0;
	}

	public double varaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		if (Basics.weekdayRuler(h.wday) == b)
		{
			return 45.0;
		}

		return 0.0;
	}

	public double horaBala(Tables.Body.Name b)
	{
		verifyGraha(b);
		if (h.calculateHora() == b)
		{
			return 60.0;
		}

		return 0.0;
	}
}