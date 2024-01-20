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
using Mhora.Components.Delegates;
using Mhora.SwissEph;
using Mhora.Tables;

namespace Mhora.Elements.Calculation;

public class Transit
{
	private readonly Body.BodyType b;
	private readonly Horoscope h;

	public Transit(Horoscope _h)
	{
		h = _h;
		b = Body.BodyType.Other;
	}

	public Transit(Horoscope _h, Body.BodyType _b)
	{
		h = _h;
		b = _b;
	}


	public Longitude LongitudeOfSun(double ut, ref bool bDirRetro)
	{
		var bp = h.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Body.BodyType.Sun, Body.Type.Graha);
		if (bp.speed_longitude >= 0)
		{
			bDirRetro = false;
		}
		else
		{
			bDirRetro = true;
		}

		return bp.longitude;
	}

	public Longitude GenericLongitude(double ut, ref bool bDirRetro)
	{
		if (b == Body.BodyType.Lagna)
		{
			return new Longitude(h.Lagna(ut));
		}

		var bp = h.CalculateSingleBodyPosition(ut, b.SwephBody(), b, Body.Type.Other);
		if (bp.speed_longitude >= 0)
		{
			bDirRetro = false;
		}
		else
		{
			bDirRetro = true;
		}

		return bp.longitude;
	}

	public Longitude LongitudeOfTithiDir(double ut, ref bool bDirRetro)
	{
		bDirRetro = false;
		return LongitudeOfTithi(ut);
	}

	public Longitude LongitudeOfTithi(double ut)
	{
		var bp_sun  = h.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Body.BodyType.Sun, Body.Type.Graha);
		var bp_moon = h.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Body.BodyType.Moon, Body.Type.Graha);
		var rel     = bp_moon.longitude.sub(bp_sun.longitude);
		return rel;
	}

	public Longitude LongitudeOfMoonDir(double ut, ref bool bDirRetro)
	{
		bDirRetro = false;
		return LongitudeOfMoon(ut);
	}

	public Longitude LongitudeOfMoon(double ut)
	{
		var bp_moon = h.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Body.BodyType.Moon, Body.Type.Graha);
		return bp_moon.longitude.add(0);
	}

	public Longitude LongitudeOfSunMoonYogaDir(double ut, ref bool bDirRetro)
	{
		bDirRetro = false;
		return LongitudeOfSunMoonYoga(ut);
	}

	public Longitude LongitudeOfSunMoonYoga(double ut)
	{
		var bp_sun  = h.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Body.BodyType.Sun, Body.Type.Graha);
		var bp_moon = h.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Body.BodyType.Moon, Body.Type.Graha);
		var rel     = bp_moon.longitude.add(bp_sun.longitude);
		return rel;
	}

	public bool CircularLonLessThan(Longitude a, Longitude b)
	{
		return CircLonLessThan(a, b);
	}

	public static bool CircLonLessThan(Longitude a, Longitude b)
	{
		var bounds = 40.0;

		if (a.value > 360.0 - bounds && b.value < bounds)
		{
			return true;
		}

		if (a.value < bounds && b.value > 360.0 - bounds)
		{
			return false;
		}

		return a.value < b.value;
	}

	public double LinearSearch(double approx_ut, Longitude lon_to_find, ReturnLon func)
	{
		var day_start = LinearSearchApprox(approx_ut, lon_to_find, func);
		var day_found = LinearSearchBinary(day_start, day_start + 1.0, lon_to_find, func);
		return day_found;
	}

	public double LinearSearchBinary(double ut_start, double ut_end, Longitude lon_to_find, ReturnLon func)
	{
		var bDiscard = true;
		if (Math.Abs(ut_end - ut_start) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			if (CircLonLessThan(func(ut_start, ref bDiscard), lon_to_find))
			{
				return ut_end;
			}

			return ut_start;
		}

		var ut_middle = (ut_start + ut_end) / 2.0;
		var lon       = func(ut_middle, ref bDiscard);
		if (CircularLonLessThan(lon, lon_to_find))
		{
			return LinearSearchBinary(ut_middle, ut_end, lon_to_find, func);
		}

		return LinearSearchBinary(ut_start, ut_middle, lon_to_find, func);
	}

	public double NonLinearSearch(double ut, Body.BodyType b, Longitude lon_to_find, ReturnLon func)
	{
		var rDir_start = false;
		var rDir_end   = false;
		var bDayFound  = false;
		ut -= 1.0;
		do
		{
			ut += 1.0;
			var l_start = func(ut, ref rDir_start);
			var l_end   = func(ut + 1.0, ref rDir_end);
			if (CircularLonLessThan(l_start, lon_to_find) && CircularLonLessThan(lon_to_find, l_end))
			{
				bDayFound = true;
			}
		}
		while (bDayFound == false);

		if (rDir_start == false && rDir_end == false)
		{
			LinearSearchBinary(ut, ut + 1.0, lon_to_find, LongitudeOfSun);
		}

		return ut;
	}

	public double LinearSearchApprox(double approx_ut, Longitude lon_to_find, ReturnLon func)
	{
		var bDiscard = true;
		var ut       = Math.Floor(approx_ut);
		var lon      = func(ut, ref bDiscard);

		if (CircularLonLessThan(lon, lon_to_find))
		{
			while (CircularLonLessThan(lon, lon_to_find))
			{
				ut  += 1.0;
				lon =  func(ut, ref bDiscard);
			}

			ut -= 1.0;
		}
		else
		{
			while (!CircularLonLessThan(lon, lon_to_find))
			{
				ut  -= 1.0;
				lon =  func(ut, ref bDiscard);
			}
		}

		var l = func(ut, ref bDiscard);
		return ut;
	}
}