using System;
using Mhora.Components.Delegates;
using Mhora.Definitions;
using Mhora.SwissEph;

namespace Mhora.Elements.Calculation;

public class Transit
{
	private readonly Body _b;
	private readonly Horoscope _h;

	public Transit(Horoscope h)
	{
		_h = h;
		_b = Body.Other;
	}

	public Transit(Horoscope h, Body b)
	{
		_h = h;
		_b = b;
	}


	public Longitude LongitudeOfSun(double ut, ref bool bDirRetro)
	{
		var bp = _h.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Body.Sun, BodyType.Graha);
		if (bp.SpeedLongitude >= 0)
		{
			bDirRetro = false;
		}
		else
		{
			bDirRetro = true;
		}

		return bp.Longitude;
	}

	public Longitude GenericLongitude(double ut, ref bool bDirRetro)
	{
		if (_b == Body.Lagna)
		{
			return new Longitude(_h.Lagna(ut));
		}

		var bp = _h.CalculateSingleBodyPosition(ut, _b.SwephBody(), _b, BodyType.Other);
		if (bp.SpeedLongitude >= 0)
		{
			bDirRetro = false;
		}
		else
		{
			bDirRetro = true;
		}

		return bp.Longitude;
	}

	public Longitude LongitudeOfTithiDir(double ut, ref bool bDirRetro)
	{
		bDirRetro = false;
		return LongitudeOfTithi(ut);
	}

	public Longitude LongitudeOfTithi(double ut)
	{
		var bpSun  = _h.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Body.Sun, BodyType.Graha);
		var bpMoon = _h.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Body.Moon, BodyType.Graha);
		var rel     = bpMoon.Longitude.Sub(bpSun.Longitude);
		return rel;
	}

	public Longitude LongitudeOfMoonDir(double ut, ref bool bDirRetro)
	{
		bDirRetro = false;
		return LongitudeOfMoon(ut);
	}

	public Longitude LongitudeOfMoon(double ut)
	{
		var bpMoon = _h.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Body.Moon, BodyType.Graha);
		return bpMoon.Longitude;
	}

	public Longitude LongitudeOfSunMoonYogaDir(double ut, ref bool bDirRetro)
	{
		bDirRetro = false;
		return LongitudeOfSunMoonYoga(ut);
	}

	public Longitude LongitudeOfSunMoonYoga(double ut)
	{
		var bpSun  = _h.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Body.Sun, BodyType.Graha);
		var bpMoon = _h.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Body.Moon, BodyType.Graha);
		var rel     = bpMoon.Longitude.Add(bpSun.Longitude);
		return rel;
	}


	public double LinearSearch(double approxUt, Longitude lonToFind, ReturnLon func)
	{
		var dayStart = LinearSearchApprox(approxUt, lonToFind, func);
		var dayFound = LinearSearchBinary(dayStart, dayStart + 1.0, lonToFind, func);
		return dayFound;
	}

	public double LinearSearchBinary(double utStart, double utEnd, Longitude lonToFind, ReturnLon func)
	{
		var bDiscard = true;
		if (Math.Abs(utEnd - utStart) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			if (Calculations.CircLonLessThan(func(utStart, ref bDiscard), lonToFind))
			{
				return utEnd;
			}

			return utStart;
		}

		var utMiddle = (utStart + utEnd) / 2.0;
		var lon       = func(utMiddle, ref bDiscard);
		if (lon.CircularLonLessThan(lonToFind))
		{
			return LinearSearchBinary(utMiddle, utEnd, lonToFind, func);
		}

		return LinearSearchBinary(utStart, utMiddle, lonToFind, func);
	}

	public double NonLinearSearch(double ut, Body b, Longitude lonToFind, ReturnLon func)
	{
		var rDirStart = false;
		var rDirEnd   = false;
		var bDayFound  = false;
		ut -= 1.0;
		do
		{
			ut += 1.0;
			var lStart = func(ut, ref rDirStart);
			var lEnd   = func(ut + 1.0, ref rDirEnd);
			if (lStart.CircularLonLessThan(lonToFind) && lonToFind.CircularLonLessThan(lEnd))
			{
				bDayFound = true;
			}
		}
		while (bDayFound == false);

		if (rDirStart == false && rDirEnd == false)
		{
			LinearSearchBinary(ut, ut + 1.0, lonToFind, LongitudeOfSun);
		}

		return ut;
	}

	public double LinearSearchApprox(double approxUt, Longitude lonToFind, ReturnLon func)
	{
		var bDiscard = true;
		var ut       = Math.Floor(approxUt);
		var lon      = func(ut, ref bDiscard);

		if (lon.CircularLonLessThan(lonToFind))
		{
			while (lon.CircularLonLessThan(lonToFind))
			{
				ut  += 1.0;
				lon =  func(ut, ref bDiscard);
			}

			ut -= 1.0;
		}
		else
		{
			while (!lon.CircularLonLessThan(lonToFind))
			{
				ut  -= 1.0;
				lon =  func(ut, ref bDiscard);
			}
		}

		var l = func(ut, ref bDiscard);
		return ut;
	}
}