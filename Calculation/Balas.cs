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
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Calculation;

/// <summary>
///     Summary description for Balas.
/// </summary>
public static class ShadBalas
{
	private static void VerifyGraha(this Horoscope h, Body b)
	{
		var _b = (int) b;
		Debug.Assert(_b >= (int) Body.Sun && _b <= (int) Body.Saturn);
	}

	public static decimal UcchaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		var debLon = b.DebilitationDegree();
		var posLon = h.GetPosition(b).Longitude;
		var diff   = posLon.Sub(debLon).Value;
		if (diff > 180)
		{
			diff = 360 - diff;
		}

		return diff / 180M * 60;
	}

	public static bool GetsOjaBala(this Body b)
	{
		switch (b)
		{
			case Body.Moon:
			case Body.Venus: return false;
			default: return true;
		}
	}

	public static double OjaYugmaHelper(this Body b, ZodiacHouse zh)
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

	public static double OjaYugmaRasyAmsaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		var    bp      = h.GetPosition(b);
		var    zh_rasi = bp.ToDivisionPosition(DivisionType.Rasi).ZodiacHouse;
		var    zh_amsa = bp.ToDivisionPosition(DivisionType.Navamsa).ZodiacHouse;
		double s       = 0;
		s += b.OjaYugmaHelper(zh_rasi);
		s += b.OjaYugmaHelper(zh_amsa);
		return s;
	}

	public static double KendraBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		var zh_b = h.GetPosition(b).ToDivisionPosition(DivisionType.Rasi).ZodiacHouse;
		var zh_l = h.GetPosition(Body.Lagna).ToDivisionPosition(DivisionType.Rasi).ZodiacHouse;
		var diff = zh_l.NumHousesBetween(zh_b);
		switch (diff % 3)
		{
			case 1: return 60;
			case 2: return 30.0;
			case 0:
			default: return 15.0;
		}
	}

	public static double DrekkanaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		var part = h.GetPosition(b).PartOfZodiacHouse(3);
		if (part == 1 && (b == Body.Sun || b == Body.Jupiter || b == Body.Mars))
		{
			return 15.0;
		}

		if (part == 2 && (b == Body.Saturn || b == Body.Mercury))
		{
			return 15.0;
		}

		if (part == 3 && (b == Body.Moon || b == Body.Venus))
		{
			return 15.0;
		}

		return 0;
	}

	public static decimal DigBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		int[] powerlessHouse =
		[
			4,
			10,
			4,
			7,
			7,
			10,
			1
		];
		var lagLon = h.GetPosition(Body.Lagna).Longitude;
		var debLon = new Longitude(lagLon.ToZodiacHouseBase());
		debLon = debLon.Add(powerlessHouse[(int) b] * 30.0 + 15.0);
		var posLon = h.GetPosition(b).Longitude;

		Application.Log.Debug("digBala {0} {1} {2}", b, posLon.Value, debLon.Value);

		var diff = posLon.Sub(debLon).Value;
		if (diff > 180)
		{
			diff = 360 - diff;
		}

		return diff / 180M * 60;
	}

	public static double NathonnathaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);

		if (b == Body.Mercury)
		{
			return 60;
		}

		var    lmt_midnight = h.Vara.Midnight.Date.Lstm(h).Time();
		var    lmt_noon     = h.Vara.Noon.Date.Lstm(h).Time();
		Time   diff;
		var    time = h.Info.DateOfBirth.Time();
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

		if (b == Body.Moon || b == Body.Mars || b == Body.Saturn)
		{
			diff = 60 - diff;
		}

		return diff.TotalHours;
	}

	public static decimal PakshaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);

		var mlon = h.GetPosition(Body.Moon).Longitude;
		var slon = h.GetPosition(Body.Sun).Longitude;

		var diff = mlon.Sub(slon).Value;
		if (diff > 180)
		{
			diff = 360 - diff;
		}

		var shubha = diff / 3M;
		var paapa  = 60 - shubha;

		switch (b)
		{
			case Body.Sun:
			case Body.Mars:
			case Body.Saturn: return paapa;
			case Body.Moon: return shubha * 2;
			default:
			case Body.Mercury:
			case Body.Jupiter:
			case Body.Venus: return shubha;
		}
	}

	public static double TribhaagaBala(this Horoscope h, Body b)
	{
		var ret = Body.Jupiter;
		h.VerifyGraha(b);
		if (h.Vara.IsDayBirth)
		{
			var length = h.Vara.DayTime / 3;
			var offset = (Time) h.Info.DateOfBirth.Time () - h.Vara.Sunrise.Time;
			var part   = (int) ((offset / length).TotalHours).Floor();
			ret = part switch
			      {
				      0 => Body.Mercury,
				      1 => Body.Sun,
				      2 => Body.Saturn,
				      _ => ret
			      };
		}
		else
		{
			var length = h.Vara.NightTime / 3;
			var offset = h.Info.DateOfBirth - h.Vara.Sunset;

			var part = (int) ((offset / length).TotalHours).Floor();
			ret = part switch
			      {
				      0 => Body.Moon,
				      1 => Body.Venus,
				      2 => Body.Mars,
				      _ => ret
			      };
		}

		if (b == Body.Jupiter || b == ret)
		{
			return 60;
		}

		return 0;
	}

	public static double NaisargikaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		return b switch
		       {
			       Body.Sun     => 60,
			       Body.Moon    => 51.43,
			       Body.Mars    => 17.14,
			       Body.Mercury => 25.70,
			       Body.Jupiter => 34.28,
			       Body.Venus   => 42.85,
			       Body.Saturn  => 8.57,
			       _            => 0
		       };
	}

	public static void KalaHelper(this Horoscope h, ref Body yearLord, ref Body monthLord)
	{
		var date       = new DateTime(1827, 5, 2);
		var dstOffset  = h.Info.TimeZone.TimeZoneInfo.GetUtcOffset(date);
		var ut_arghana = sweph.JulDay(1827, 5, 2, -dstOffset.TotalHours + 12.0 / 24.0);
		var ut_noon    = h.Info.Jd - h.Info.DateOfBirth.Time ().TotalDays + 12.0 / 24.0;

		var diff = ut_noon - ut_arghana;
		if (diff >= 0)
		{
			int quo = (int) (diff / 360.0).Floor();
			diff -= quo * 360.0;
		}
		else
		{
			var pdiff = -diff;
			var quo   = (pdiff / 360.0).Ceil();
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

		yearLord  = ((Weekday) sweph.DayOfWeek(ut_noon - diff_year)).Ruler();
		monthLord = ((Weekday) sweph.DayOfWeek(ut_noon - diff_month)).Ruler();
	}

	public static double AbdaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		Body yearLord = Body.Sun, monthLord = Body.Sun;
		h.KalaHelper(ref yearLord, ref monthLord);
		if (yearLord == b)
		{
			return 15.0;
		}

		return 0.0;
	}

	public static double MasaBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		Body yearLord = Body.Sun, monthLord = Body.Sun;
		h.KalaHelper(ref yearLord, ref monthLord);
		if (monthLord == b)
		{
			return 30.0;
		}

		return 0.0;
	}

	public static double VaraBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		if (h.Vara.DayLord == b)
		{
			return 45.0;
		}

		return 0.0;
	}

	public static double HoraBala(this Horoscope h, Body b)
	{
		h.VerifyGraha(b);
		if (h.Vara.HoraLord == b)
		{
			return 60.0;
		}

		return 0.0;
	}
}