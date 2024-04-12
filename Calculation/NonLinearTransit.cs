using System;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Calculation;

/// <summary>
///     Summary description for Transits.
/// </summary>
public class NonLinearTransit
{
	private readonly Body _b;
	private readonly Horoscope _h;

	public NonLinearTransit(Horoscope h, Body b)
	{
		_h       = h;
		_b = b;
	}

	public int BodyNameToSweph(Body b)
	{
		return b switch
		       {
			       Body.Sun     => sweph.SE_SUN,
			       Body.Moon    => sweph.SE_MOON,
			       Body.Mars    => sweph.SE_MARS,
			       Body.Mercury => sweph.SE_MERCURY,
			       Body.Jupiter => sweph.SE_JUPITER,
			       Body.Venus   => sweph.SE_VENUS,
			       Body.Saturn  => sweph.SE_SATURN,
			       _            => throw new Exception()
		       };
	}

	public Longitude GetLongitude(JulianDate ut, ref bool bForwardDir)
	{
		var swephBody = BodyNameToSweph(_b);
		var bp        = _h.CalculateSingleBodyPosition(ut, swephBody, _b, BodyType.Other);
		if (bp.SpeedLongitude >= 0)
		{
			bForwardDir = true;
		}
		else
		{
			bForwardDir = false;
		}

		return bp.Longitude;
	}

	public double BinarySearchNormal(JulianDate utStart, double utEnd, Longitude lonToFind)
	{
		var bDiscard = true;

		if (Math.Abs(utEnd - utStart) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			//Mhora.Log.Debug ("BinarySearchNormal: Found {0} at {1}", lon_to_find, ut_start);
			if (GetLongitude(utStart, ref bDiscard).CircLonLessThan(lonToFind))
			{
				return utEnd;
			}

			return utStart;
		}

		var utMiddle = (utStart + utEnd) / 2.0;

		var lon = GetLongitude(utMiddle, ref bDiscard);
		//Mhora.Log.Debug ("BinarySearchNormal {0} Find:{1} {2} curr:{3}", b, lon_to_find.value, ut_middle, lon.value);
		if (lon.CircLonLessThan(lonToFind))
		{
			return BinarySearchNormal(utMiddle, utEnd, lonToFind);
		}

		return BinarySearchNormal(utStart, utMiddle, lonToFind);
	}

	public double BinarySearchRetro(JulianDate utStart, double utEnd, Longitude lonToFind)
	{
		if (Math.Abs(utEnd - utStart) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			//Mhora.Log.Debug ("BinarySearchRetro: Found {0} at {1}", lon_to_find, ut_start);
			return utStart;
		}

		var utMiddle = (utStart + utEnd) / 2.0;
		var bDiscard  = true;
		var lon       = GetLongitude(utMiddle, ref bDiscard);
		//Mhora.Log.Debug ("BinarySearchRetro {0} Find:{1} {2} curr:{3}", b, lon_to_find.value, ut_middle, lon.value);
		if (lon.CircLonLessThan(lonToFind))
		{
			return BinarySearchRetro(utStart, utMiddle, lonToFind);
		}

		return BinarySearchRetro(utMiddle, utEnd, lonToFind);
	}

	public double Forward(JulianDate ut, Longitude lonToFind)
	{
		while (true)
		{
			bool bForwardStart = true, bForwardEnd = true;
			var  lStart        = GetLongitude(ut, ref bForwardStart);
			var  lEnd          = GetLongitude(ut + 1.0, ref bForwardEnd);
			if (bForwardStart && bForwardEnd)
			{
				if (lStart.CircLonLessThan(lonToFind) && lonToFind.CircLonLessThan(lEnd))
				{
					//Mhora.Log.Debug("2: (N) +1.0. {0} Curr:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
					return BinarySearchNormal(ut, ut + 1.0, lonToFind);
				}

				//Mhora.Log.Debug("1: (N) +1.0. {0} Find:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
				ut += 10.0;
			}
			else if (bForwardStart == false && bForwardEnd == false)
			{
				if (lEnd.CircLonLessThan(lonToFind) && lonToFind.CircLonLessThan(lStart))
				{
					//Mhora.Log.Debug("2: (R) +1.0. {0} Curr:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
					return BinarySearchRetro(ut, ut + 1.0, lonToFind);
				}

				//Mhora.Log.Debug("1: (R) +1.0. {0} Find:{1} Start:{2} End:{3}", b, lonToFind.value, lStart.value, lEnd.value);
				ut += 10.0;
			}
			else
			{
				//Mhora.Log.Debug ("Retrograde Cusp date at {0}. Skipping for now.", ut);
				ut += 10.0;
			}
		}
	}
}