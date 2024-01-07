using System;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;
using Mhora.Tables;

namespace Mhora.Elements;

/// <summary>
///     Summary description for Transits.
/// </summary>
public class NonLinearTransit
{
	private readonly Body.BodyType b;
	private readonly Horoscope h;

	public NonLinearTransit(Horoscope _h, Body.BodyType _b)
	{
		h = _h;
		b = _b;
	}

	public int BodyNameToSweph(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return sweph.SE_SUN;
			case Body.BodyType.Moon:    return sweph.SE_MOON;
			case Body.BodyType.Mars:    return sweph.SE_MARS;
			case Body.BodyType.Mercury: return sweph.SE_MERCURY;
			case Body.BodyType.Jupiter: return sweph.SE_JUPITER;
			case Body.BodyType.Venus:   return sweph.SE_VENUS;
			case Body.BodyType.Saturn:  return sweph.SE_SATURN;
			default:                throw new Exception();
		}
	}

	public Longitude GetLongitude(double ut, ref bool bForwardDir)
	{
		var swephBody = BodyNameToSweph(b);
		var bp        = Basics.CalculateSingleBodyPosition(ut, swephBody, b, Body.Type.Other, h);
		if (bp.speed_longitude >= 0)
		{
			bForwardDir = true;
		}
		else
		{
			bForwardDir = false;
		}

		return bp.longitude;
	}

	public double BinarySearchNormal(double ut_start, double ut_end, Longitude lon_to_find)
	{
		var bDiscard = true;

		if (Math.Abs(ut_end - ut_start) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			//mhora.Log.Debug ("BinarySearchNormal: Found {0} at {1}", lon_to_find, ut_start);
			if (Transit.CircLonLessThan(GetLongitude(ut_start, ref bDiscard), lon_to_find))
			{
				return ut_end;
			}

			return ut_start;
		}

		var ut_middle = (ut_start + ut_end) / 2.0;

		var lon = GetLongitude(ut_middle, ref bDiscard);
		//mhora.Log.Debug ("BinarySearchNormal {0} Find:{1} {2} curr:{3}", b, lon_to_find.value, ut_middle, lon.value);
		if (Transit.CircLonLessThan(lon, lon_to_find))
		{
			return BinarySearchNormal(ut_middle, ut_end, lon_to_find);
		}

		return BinarySearchNormal(ut_start, ut_middle, lon_to_find);
	}

	public double BinarySearchRetro(double ut_start, double ut_end, Longitude lon_to_find)
	{
		if (Math.Abs(ut_end - ut_start) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			//mhora.Log.Debug ("BinarySearchRetro: Found {0} at {1}", lon_to_find, ut_start);
			return ut_start;
		}

		var ut_middle = (ut_start + ut_end) / 2.0;
		var bDiscard  = true;
		var lon       = GetLongitude(ut_middle, ref bDiscard);
		//mhora.Log.Debug ("BinarySearchRetro {0} Find:{1} {2} curr:{3}", b, lon_to_find.value, ut_middle, lon.value);
		if (Transit.CircLonLessThan(lon, lon_to_find))
		{
			return BinarySearchRetro(ut_start, ut_middle, lon_to_find);
		}

		return BinarySearchRetro(ut_middle, ut_end, lon_to_find);
	}

	public double Forward(double ut, Longitude lonToFind)
	{
		while (true)
		{
			bool bForwardStart = true, bForwardEnd = true;
			var  lStart        = GetLongitude(ut, ref bForwardStart);
			var  lEnd          = GetLongitude(ut + 1.0, ref bForwardEnd);
			if (bForwardStart && bForwardEnd)
			{
				if (Transit.CircLonLessThan(lStart, lonToFind) && Transit.CircLonLessThan(lonToFind, lEnd))
				{
					//mhora.Log.Debug("2: (N) +1.0. {0} Curr:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
					return BinarySearchNormal(ut, ut + 1.0, lonToFind);
				}

				//mhora.Log.Debug("1: (N) +1.0. {0} Find:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
				ut += 10.0;
			}
			else if (bForwardStart == false && bForwardEnd == false)
			{
				if (Transit.CircLonLessThan(lEnd, lonToFind) && Transit.CircLonLessThan(lonToFind, lStart))
				{
					//mhora.Log.Debug("2: (R) +1.0. {0} Curr:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
					return BinarySearchRetro(ut, ut + 1.0, lonToFind);
				}

				//mhora.Log.Debug("1: (R) +1.0. {0} Find:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
				ut += 10.0;
			}
			else
			{
				//mhora.Log.Debug ("Retrograde Cusp date at {0}. Skipping for now.", ut);
				ut += 10.0;
			}
		}
	}
}