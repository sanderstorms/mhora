using System;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Calculation;

public class Retrogression
{
	private readonly Body _b;
	private readonly Horoscope _h;

	public Retrogression(Horoscope h, Body b)
	{
		//Debug.Assert((int)_b >= (int)Body.Type.Moon &&
		//	(int)_b <= (int)Body.Type.Saturn, 
		//	string.Format("Retrogression::Retrogression. Invalid Body {0}", _b));
		_h       = h;
		_b = b;
	}

	public void GetRetroSolarCusps(ref Longitude start, ref Longitude end)
	{
		switch (_b)
		{
			case Body.Mars:
				start.Value = 211;
				end.Value   = 232;
				break;
			case Body.Jupiter:
				start.Value = 240;
				end.Value   = 248;
				break;
			case Body.Saturn:
				start.Value = 248;
				end.Value   = 253;
				break;
		}
	}

	public JulianDate GotoNextRetroSolarCusp(JulianDate ut)
	{
		return ut;
#if DND
			Longitude cusp_start = new Longitude(0);
			Longitude cusp_end = new Longitude(0);
			BodyPosition bp_sun = h.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(Body.Type.Sun), Body.Type.Sun, BodyType.Type.Other);
			BodyPosition bp_b = h.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Type.Other);
			Longitude diff = bp_b.longitude.sub(bp_sun.longitude);
			if (Transit.CircLonLessThan(cusp_start, diff) &&
				Transit.CircLonLessThan(diff, cusp_end))
				return ut;

			Longitude diffIncrease = diff.sub(cusp_start);
			double ret = ut + (diffIncrease.value * 360.0/TimeUtils.SiderealYear.TotalDays);
			return ret;
#endif
	}

	public JulianDate FindClosestTransit(JulianDate ut, Longitude lonToFind)
	{
		var bp = _h.CalculateSingleBodyPosition(ut, _b.SwephBody(), _b, BodyType.Other);
		while (bp.Longitude.CircLonLessThan(lonToFind))
		{
			//Mhora.Log.Debug("- {0} {1}", bp.longitude.value, lonToFind.value);
			ut++;
			bp = _h.CalculateSingleBodyPosition(ut, _b.SwephBody(), _b, BodyType.Other);
		}

		while (lonToFind.CircLonLessThan(bp.Longitude))
		{
			//Mhora.Log.Debug("+ {0} {1}", bp.longitude.value, lonToFind.value);
			ut--;
			bp = _h.CalculateSingleBodyPosition(ut, _b.SwephBody(), _b, BodyType.Other);
		}

		return ut;
	}

	public JulianDate GetTransitBackward(JulianDate ut, Longitude lonToFind)
	{
		if (_b == Body.Lagna)
		{
			return GetLagnaTransitBackward(ut, lonToFind);
		}

		Ref<bool> becomesDirect = new(true);
		var utCurr       = ut;
		var utNext       = ut;

		while (true)
		{
			utCurr = utNext;

			var utStart = utCurr;
			if (utCurr != ut)
			{
				utStart -= 5.0;
			}

			utNext = FindNextCuspBackward(utStart, becomesDirect);

			var bpNext = _h.CalculateSingleBodyPosition(utCurr, _b.SwephBody(), _b, BodyType.Other);
			var bpCurr = _h.CalculateSingleBodyPosition(utNext, _b.SwephBody(), _b, BodyType.Other);

			//Mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

			if (false == becomesDirect && lonToFind.Sub(bpCurr.Longitude).Value <= bpNext.Longitude.Sub(bpCurr.Longitude).Value)
			{
				//Mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
				break;
			}

			if (becomesDirect && lonToFind.Sub(bpNext.Longitude).Value <= bpCurr.Longitude.Sub(bpNext.Longitude).Value)
			{
				//Mhora.Log.Debug ("- Found {0} betweeen {1} and {2}", lonToFind, bp_next.longitude, bp_curr.longitude);
				break;
			}
			//Mhora.Log.Debug ("{3} Didn't find {0} betweeen {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude, ut_curr);
		}

		if (false == becomesDirect)
		{
			return BinaryLonSearch(utNext, utCurr, lonToFind, true);
		}

		return BinaryLonSearch(utNext, utCurr, lonToFind, false);
	}

	public double GetStep() => 5.0;

	public JulianDate GetLagnaTransitForward(JulianDate ut, Longitude lonToFind)
	{
		var utStart = ut;
		var utEnd   = ut;

		while (true)
		{
			utStart = utEnd;
			utEnd   = utStart + TimeSpan.FromHours(1);

			var lonStart = GetLon(utStart);
			var lonEnd   = GetLon(utEnd);

			//Mhora.Log.Debug ("F {3} Lagna search for {0} between {1} and {2}",
			//lonToFind, lon_start, lon_end, m);

			if (lonToFind.Sub(lonStart).Value <= lonEnd.Sub(lonStart).Value)
			{
				break;
			}
		}

		return BinaryLonSearch(utStart, utEnd, lonToFind, true);
	}

	public JulianDate GetLagnaTransitBackward(JulianDate ut, Longitude lonToFind)
	{
		var utStart = ut;
		var utEnd   = ut;

		while (true)
		{
			utStart = utEnd;
			utEnd   = utStart - TimeSpan.FromHours(1);

			var lonStart = GetLon(utStart);
			var lonEnd   = GetLon(utEnd);

			//Mhora.Log.Debug ("B {3} Lagna search for {0} between {1} and {2}",
			//lonToFind, lon_start, lon_end, m);

			if (lonToFind.Sub(lonEnd).Value <= lonStart.Sub(lonEnd).Value)
			{
				break;
			}
		}

		return BinaryLonSearch(utEnd, utStart, lonToFind, true);
	}

	public JulianDate GetTransitForward(JulianDate ut, Longitude lonToFind)
	{
		if (_b == Body.Lagna)
		{
			return GetLagnaTransitForward(ut, lonToFind);
		}


		Ref<bool> becomesDirect = new(true);
		var utCurr       = ut;
		var utNext       = ut;

		while (true)
		{
			utCurr = utNext;

			var utStart = utCurr;
			if (utCurr != ut)
			{
				utStart += GetStep();
			}

			utNext = FindNextCuspForward(utStart, becomesDirect);

			var bpCurr = _h.CalculateSingleBodyPosition(utCurr, _b.SwephBody(), _b, BodyType.Other);
			var bpNext = _h.CalculateSingleBodyPosition(utNext, _b.SwephBody(), _b, BodyType.Other);

			//Mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

			if (false == becomesDirect && lonToFind.Sub(bpCurr.Longitude).Value <= bpNext.Longitude.Sub(bpCurr.Longitude).Value)
			{
				//Mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
				break;
			}

			if (becomesDirect && lonToFind.Sub(bpNext.Longitude).Value <= bpCurr.Longitude.Sub(bpNext.Longitude).Value)
			{
				//Mhora.Log.Debug ("- Found {0} betweeen {1} and {2}", lonToFind, bp_next.longitude, bp_curr.longitude);
				break;
			}
			//Mhora.Log.Debug ("{3} Didn't find {0} betweeen {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude, ut_curr);
		}

		if (false == becomesDirect)
		{
			return BinaryLonSearch(utCurr, utNext, lonToFind, true);
		}

		return BinaryLonSearch(utCurr, utNext, lonToFind, false);
	}

	public double GetSpeed(double ut)
	{
		var bp = _h.CalculateSingleBodyPosition(ut, _b.SwephBody(), _b, BodyType.Other);
		return bp.SpeedLongitude;
	}

	public Longitude GetLon(JulianDate ut, Ref <bool> bForward)
	{
		if (_b == Body.Lagna)
		{
			return new Longitude(_h.Lagna(ut));
		}

		var bp = _h.CalculateSingleBodyPosition(ut, _b.SwephBody(), _b, BodyType.Other);
		bForward.Value = (bp.SpeedLongitude >= 0) ? true : false;
		return bp.Longitude;
	}

	public Longitude GetLon(JulianDate ut)
	{
		var bp = _h.CalculateSingleBodyPosition(ut, _b.SwephBody(), _b, BodyType.Other);
		return bp.Longitude;
	}

	public JulianDate BinaryLonSearch(JulianDate utStart, JulianDate utEnd, Longitude lonToFind, bool normal)
	{
		if (Math.Abs(utEnd - utStart) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0 * 60.0))
		{
			if (GetLon(utStart).CircLonLessThan(lonToFind))
			{
				if (normal)
				{
					return utEnd;
				}

				return utStart;
			}

			if (normal)
			{
				return utStart;
			}

			return utEnd;
		}

		var utMiddle = (utStart + utEnd) / 2.0;

		var lonStart  = GetLon(utStart);
		var lonMiddle = GetLon(utMiddle);
		var lonEnd    = GetLon(utEnd);

		if (normal)
		{
			if (lonToFind.Sub(lonStart).Value <= lonMiddle.Sub(lonStart).Value)
			{
				return BinaryLonSearch(utStart, utMiddle, lonToFind, normal);
			}

			return BinaryLonSearch(utMiddle, utEnd, lonToFind, normal);
		}

		if (lonToFind.Sub(lonEnd).Value <= lonMiddle.Sub(lonEnd).Value)
		{
			return BinaryLonSearch(utMiddle, utEnd, lonToFind, normal);
		}

		return BinaryLonSearch(utStart, utMiddle, lonToFind, normal);
	}

	public JulianDate BinaryCuspSearch(JulianDate utStart, JulianDate utEnd, bool normal)
	{
		if (Math.Abs(utEnd - utStart) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			return utStart;
		}

		var utMiddle    = (utStart + utEnd) / 2.0;
		var speedStart  = GetSpeed(utStart);
		var speedMiddle = GetSpeed(utMiddle);
		var speedEnd    = GetSpeed(utEnd);

		//Mhora.Log.Debug ("Speed BinarySearchNormal {0} UT: {2} Speed {1}", b, speed_middle, ut_middle);

		if (speedStart > 0 && speedEnd < 0)
		{
			if (speedMiddle > 0)
			{
				return BinaryCuspSearch(utMiddle, utEnd, normal);
			}

			return BinaryCuspSearch(utStart, utMiddle, normal);
		}

		if (speedStart < 0 && speedEnd > 0)
		{
			if (speedMiddle < 0)
			{
				return BinaryCuspSearch(utMiddle, utEnd, normal);
			}

			return BinaryCuspSearch(utStart, utMiddle, normal);
		}

		if (speedStart == 0)
		{
			return utStart;
		}

		return utEnd;
	}

	public JulianDate FindNextCuspBackward(JulianDate startUt, Ref <bool> becomesDirect)
	{
		var utStep = 5.0;
		var bp      = _h.CalculateSingleBodyPosition(startUt, _b.SwephBody(), _b, BodyType.Other);

		// Body is currently direct
		if (bp.SpeedLongitude >= 0)
		{
			startUt = GotoNextRetroSolarCusp(startUt);
			var lowerUt  = startUt;
			var higherUt = startUt;
			becomesDirect.Value = false;
			while (true)
			{
				lowerUt  = higherUt;
				higherUt = lowerUt - utStep;

				// Find speeds
				var bpL = _h.CalculateSingleBodyPosition(lowerUt, _b.SwephBody(), _b, BodyType.Other);
				var bpH = _h.CalculateSingleBodyPosition(higherUt, _b.SwephBody(), _b, BodyType.Other);

				//Mhora.Log.Debug ("DChecking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				// If first one is retro, we're exactly at the cusp
				// If higher is still direct, contine
				if (bpL.SpeedLongitude < 0 && bpH.SpeedLongitude > 0)
				{
					break;
				}

				if (bpL.SpeedLongitude > 0 && bpH.SpeedLongitude < 0)
				{
					break;
				}
				//if (bp_l.speed_longitude < 0 && bp_h.speed_longitude < 0) 
				//	return findNextCuspBackward (lower_ut, ref becomesDirect);
			}

			// Within one day period
			return BinaryCuspSearch(higherUt, lowerUt, true);
		}

		// Body is current retrograde
		else
		{
			var lowerUt  = startUt;
			var higherUt = startUt;
			becomesDirect.Value = true;
			while (true)
			{
				lowerUt  = higherUt;
				higherUt = lowerUt - utStep;
				// Find speeds
				var bpL = _h.CalculateSingleBodyPosition(lowerUt, _b.SwephBody(), _b, BodyType.Other);
				var bpH = _h.CalculateSingleBodyPosition(higherUt, _b.SwephBody(), _b, BodyType.Other);

				//Mhora.Log.Debug ("R Checking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				if (bpL.SpeedLongitude > 0 && bpH.SpeedLongitude <= 0)
				{
					break;
				}

				if (bpL.SpeedLongitude < 0 && bpH.SpeedLongitude > 0)
				{
					break;
				}
				//if (bp_l.speed_longitude > 0 && bp_h.speed_longitude > 0)
				//	return findNextCuspBackward (lower_ut, ref becomesDirect);
			}

			// Within one day period
			return BinaryCuspSearch(higherUt, lowerUt, false);
		}
	}

	public JulianDate FindNextCuspForward(JulianDate startUt, Ref <bool> becomesDirect)
	{
		var utStep = 1.0;
		var bp      = _h.CalculateSingleBodyPosition(startUt, _b.SwephBody(), _b, BodyType.Other);

		// Body is currently direct
		if (bp.SpeedLongitude >= 0)
		{
			startUt = GotoNextRetroSolarCusp(startUt);
			var lowerUt  = startUt;
			var higherUt = startUt;
			becomesDirect.Value = false;
			while (true)
			{
				lowerUt  = higherUt;
				higherUt = lowerUt + utStep;

				// Find speeds
				var bpL = _h.CalculateSingleBodyPosition(lowerUt, _b.SwephBody(), _b, BodyType.Other);
				var bpH = _h.CalculateSingleBodyPosition(higherUt, _b.SwephBody(), _b, BodyType.Other);

				//Mhora.Log.Debug ("DChecking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				// If first one is retro, we're exactly at the cusp
				// If higher is still direct, contine
				if (bpL.SpeedLongitude > 0 && bpH.SpeedLongitude < 0)
				{
					break;
				}

				if (bpL.SpeedLongitude < 0 && bpH.SpeedLongitude < 0)
				{
					return FindNextCuspForward(lowerUt, becomesDirect);
				}
			}

			// Within one day period
			return BinaryCuspSearch(lowerUt, higherUt, true);
		}

		// Body is current retrograde
		else
		{
			var lowerUt  = startUt;
			var higherUt = startUt;
			becomesDirect.Value = true;
			while (true)
			{
				lowerUt  = higherUt;
				higherUt = lowerUt + utStep;
				// Find speeds
				var bpL = _h.CalculateSingleBodyPosition(lowerUt, _b.SwephBody(), _b, BodyType.Other);
				var bpH = _h.CalculateSingleBodyPosition(higherUt, _b.SwephBody(), _b, BodyType.Other);

				//Mhora.Log.Debug ("R Checking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				if (bpL.SpeedLongitude < 0 && bpH.SpeedLongitude >= 0)
				{
					break;
				}

				if (bpL.SpeedLongitude > 0 && bpH.SpeedLongitude > 0)
				{
					return FindNextCuspForward(lowerUt, becomesDirect);
				}
			}

			// Within one day period
			return BinaryCuspSearch(lowerUt, higherUt, false);
		}
	}
}