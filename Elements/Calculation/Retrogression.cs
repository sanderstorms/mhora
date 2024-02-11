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
using Mhora.Definitions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements.Calculation;

public class Retrogression
{
	private readonly Body b;
	private readonly Horoscope h;

	public Retrogression(Horoscope _h, Body _b)
	{
		//Debug.Assert((int)_b >= (int)Body.Type.Moon &&
		//	(int)_b <= (int)Body.Type.Saturn, 
		//	string.Format("Retrogression::Retrogression. Invalid Body {0}", _b));
		h = _h;
		b = _b;
	}

	public void GetRetroSolarCusps(ref Longitude start, ref Longitude end)
	{
		switch (b)
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

	public double GotoNextRetroSolarCusp(double ut)
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

	public double FindClosestTransit(double ut, Longitude lonToFind)
	{
		var bp = h.CalculateSingleBodyPosition(ut, b.SwephBody(), b, BodyType.Other);
		while (Transit.CircLonLessThan(bp.Longitude, lonToFind))
		{
			//Mhora.Log.Debug("- {0} {1}", bp.longitude.value, lonToFind.value);
			ut++;
			bp = h.CalculateSingleBodyPosition(ut, b.SwephBody(), b, BodyType.Other);
		}

		while (Transit.CircLonLessThan(lonToFind, bp.Longitude))
		{
			//Mhora.Log.Debug("+ {0} {1}", bp.longitude.value, lonToFind.value);
			ut--;
			bp = h.CalculateSingleBodyPosition(ut, b.SwephBody(), b, BodyType.Other);
		}

		return ut;
	}

	public double GetTransitBackward(double ut, Longitude lonToFind)
	{
		if (b == Body.Lagna)
		{
			return GetLagnaTransitBackward(ut, lonToFind);
		}

		var becomesDirect = true;
		var ut_curr       = ut;
		var ut_next       = ut;

		while (true)
		{
			ut_curr = ut_next;

			var ut_start = ut_curr;
			if (ut_curr != ut)
			{
				ut_start -= 5.0;
			}

			ut_next = findNextCuspBackward(ut_start, ref becomesDirect);

			var bp_next = h.CalculateSingleBodyPosition(ut_curr, b.SwephBody(), b, BodyType.Other);
			var bp_curr = h.CalculateSingleBodyPosition(ut_next, b.SwephBody(), b, BodyType.Other);

			//Mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

			if (false == becomesDirect && lonToFind.Sub(bp_curr.Longitude).Value <= bp_next.Longitude.Sub(bp_curr.Longitude).Value)
			{
				//Mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
				break;
			}

			if (becomesDirect && lonToFind.Sub(bp_next.Longitude).Value <= bp_curr.Longitude.Sub(bp_next.Longitude).Value)
			{
				//Mhora.Log.Debug ("- Found {0} betweeen {1} and {2}", lonToFind, bp_next.longitude, bp_curr.longitude);
				break;
			}
			//Mhora.Log.Debug ("{3} Didn't find {0} betweeen {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude, ut_curr);
		}

		if (false == becomesDirect)
		{
			return BinaryLonSearch(ut_next, ut_curr, lonToFind, true);
		}

		return BinaryLonSearch(ut_next, ut_curr, lonToFind, false);
	}

	public double GetStep()
	{
		return 5.0;
	}

	public double GetLagnaTransitForward(double ut, Longitude lonToFind)
	{
		var ut_start = ut;
		var ut_end   = ut;

		while (true)
		{
			ut_start = ut_end;
			ut_end   = ut_start + 1.0 / 24.0;

			var lon_start = GetLon(ut_start);
			var lon_end   = GetLon(ut_end);

			var m = ut_start.ToUtc();

			//Mhora.Log.Debug ("F {3} Lagna search for {0} between {1} and {2}",
			//lonToFind, lon_start, lon_end, m);

			if (lonToFind.Sub(lon_start).Value <= lon_end.Sub(lon_start).Value)
			{
				break;
			}
		}

		return BinaryLonSearch(ut_start, ut_end, lonToFind, true);
	}

	public double GetLagnaTransitBackward(double ut, Longitude lonToFind)
	{
		var ut_start = ut;
		var ut_end   = ut;

		while (true)
		{
			ut_start = ut_end;
			ut_end   = ut_start - 1.0 / 24.0;

			var lon_start = GetLon(ut_start);
			var lon_end   = GetLon(ut_end);

			var m = ut_start.ToUtc();

			//Mhora.Log.Debug ("B {3} Lagna search for {0} between {1} and {2}",
			//lonToFind, lon_start, lon_end, m);

			if (lonToFind.Sub(lon_end).Value <= lon_start.Sub(lon_end).Value)
			{
				break;
			}
		}

		return BinaryLonSearch(ut_end, ut_start, lonToFind, true);
	}

	public double GetTransitForward(double ut, Longitude lonToFind)
	{
		if (b == Body.Lagna)
		{
			return GetLagnaTransitForward(ut, lonToFind);
		}


		var becomesDirect = true;
		var ut_curr       = ut;
		var ut_next       = ut;

		while (true)
		{
			ut_curr = ut_next;

			var ut_start = ut_curr;
			if (ut_curr != ut)
			{
				ut_start += GetStep();
			}

			ut_next = findNextCuspForward(ut_start, ref becomesDirect);

			var bp_curr = h.CalculateSingleBodyPosition(ut_curr, b.SwephBody(), b, BodyType.Other);
			var bp_next = h.CalculateSingleBodyPosition(ut_next, b.SwephBody(), b, BodyType.Other);

			//Mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

			if (false == becomesDirect && lonToFind.Sub(bp_curr.Longitude).Value <= bp_next.Longitude.Sub(bp_curr.Longitude).Value)
			{
				//Mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
				break;
			}

			if (becomesDirect && lonToFind.Sub(bp_next.Longitude).Value <= bp_curr.Longitude.Sub(bp_next.Longitude).Value)
			{
				//Mhora.Log.Debug ("- Found {0} betweeen {1} and {2}", lonToFind, bp_next.longitude, bp_curr.longitude);
				break;
			}
			//Mhora.Log.Debug ("{3} Didn't find {0} betweeen {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude, ut_curr);
		}

		if (false == becomesDirect)
		{
			return BinaryLonSearch(ut_curr, ut_next, lonToFind, true);
		}

		return BinaryLonSearch(ut_curr, ut_next, lonToFind, false);
	}

	public double GetSpeed(double ut)
	{
		var bp = h.CalculateSingleBodyPosition(ut, b.SwephBody(), b, BodyType.Other);
		return bp.SpeedLongitude;
	}

	public Longitude GetLon(double ut, ref bool bForward)
	{
		if (b == Body.Lagna)
		{
			return new Longitude(h.Lagna(ut));
		}

		var bp = h.CalculateSingleBodyPosition(ut, b.SwephBody(), b, BodyType.Other);
		bForward = bp.SpeedLongitude >= 0;
		return bp.Longitude;
	}

	public Longitude GetLon(double ut)
	{
		var bp = h.CalculateSingleBodyPosition(ut, b.SwephBody(), b, BodyType.Other);
		return bp.Longitude;
	}

	public double BinaryLonSearch(double ut_start, double ut_end, Longitude lon_to_find, bool normal)
	{
		if (Math.Abs(ut_end - ut_start) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0 * 60.0))
		{
			if (Transit.CircLonLessThan(GetLon(ut_start), lon_to_find))
			{
				if (normal)
				{
					return ut_end;
				}

				return ut_start;
			}

			if (normal)
			{
				return ut_start;
			}

			return ut_end;
		}

		var ut_middle = (ut_start + ut_end) / 2.0;

		var lon_start  = GetLon(ut_start);
		var lon_middle = GetLon(ut_middle);
		var lon_end    = GetLon(ut_end);

		if (normal)
		{
			if (lon_to_find.Sub(lon_start).Value <= lon_middle.Sub(lon_start).Value)
			{
				return BinaryLonSearch(ut_start, ut_middle, lon_to_find, normal);
			}

			return BinaryLonSearch(ut_middle, ut_end, lon_to_find, normal);
		}

		if (lon_to_find.Sub(lon_end).Value <= lon_middle.Sub(lon_end).Value)
		{
			return BinaryLonSearch(ut_middle, ut_end, lon_to_find, normal);
		}

		return BinaryLonSearch(ut_start, ut_middle, lon_to_find, normal);
	}

	public double BinaryCuspSearch(double ut_start, double ut_end, bool normal)
	{
		if (Math.Abs(ut_end - ut_start) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
		{
			return ut_start;
		}

		var ut_middle    = (ut_start + ut_end) / 2.0;
		var speed_start  = GetSpeed(ut_start);
		var speed_middle = GetSpeed(ut_middle);
		var speed_end    = GetSpeed(ut_end);

		//Mhora.Log.Debug ("Speed BinarySearchNormal {0} UT: {2} Speed {1}", b, speed_middle, ut_middle);

		if (speed_start > 0 && speed_end < 0)
		{
			if (speed_middle > 0)
			{
				return BinaryCuspSearch(ut_middle, ut_end, normal);
			}

			return BinaryCuspSearch(ut_start, ut_middle, normal);
		}

		if (speed_start < 0 && speed_end > 0)
		{
			if (speed_middle < 0)
			{
				return BinaryCuspSearch(ut_middle, ut_end, normal);
			}

			return BinaryCuspSearch(ut_start, ut_middle, normal);
		}

		if (speed_start == 0)
		{
			return ut_start;
		}

		return ut_end;
	}

	public double findNextCuspBackward(double start_ut, ref bool becomesDirect)
	{
		var ut_step = 5.0;
		var bp      = h.CalculateSingleBodyPosition(start_ut, b.SwephBody(), b, BodyType.Other);

		// Body is currently direct
		if (bp.SpeedLongitude >= 0)
		{
			start_ut = GotoNextRetroSolarCusp(start_ut);
			var lower_ut  = start_ut;
			var higher_ut = start_ut;
			becomesDirect = false;
			while (true)
			{
				lower_ut  = higher_ut;
				higher_ut = lower_ut - ut_step;

				// Find speeds
				var bp_l = h.CalculateSingleBodyPosition(lower_ut, b.SwephBody(), b, BodyType.Other);
				var bp_h = h.CalculateSingleBodyPosition(higher_ut, b.SwephBody(), b, BodyType.Other);

				//Mhora.Log.Debug ("DChecking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				// If first one is retro, we're exactly at the cusp
				// If higher is still direct, contine
				if (bp_l.SpeedLongitude < 0 && bp_h.SpeedLongitude > 0)
				{
					break;
				}

				if (bp_l.SpeedLongitude > 0 && bp_h.SpeedLongitude < 0)
				{
					break;
				}
				//if (bp_l.speed_longitude < 0 && bp_h.speed_longitude < 0) 
				//	return findNextCuspBackward (lower_ut, ref becomesDirect);
			}

			// Within one day period
			return BinaryCuspSearch(higher_ut, lower_ut, true);
		}

		// Body is current retrograde
		else
		{
			var lower_ut  = start_ut;
			var higher_ut = start_ut;
			becomesDirect = true;
			while (true)
			{
				lower_ut  = higher_ut;
				higher_ut = lower_ut - ut_step;
				// Find speeds
				var bp_l = h.CalculateSingleBodyPosition(lower_ut, b.SwephBody(), b, BodyType.Other);
				var bp_h = h.CalculateSingleBodyPosition(higher_ut, b.SwephBody(), b, BodyType.Other);

				//Mhora.Log.Debug ("R Checking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				if (bp_l.SpeedLongitude > 0 && bp_h.SpeedLongitude <= 0)
				{
					break;
				}

				if (bp_l.SpeedLongitude < 0 && bp_h.SpeedLongitude > 0)
				{
					break;
				}
				//if (bp_l.speed_longitude > 0 && bp_h.speed_longitude > 0)
				//	return findNextCuspBackward (lower_ut, ref becomesDirect);
			}

			// Within one day period
			return BinaryCuspSearch(higher_ut, lower_ut, false);
		}
	}

	public double findNextCuspForward(double start_ut, ref bool becomesDirect)
	{
		var ut_step = 1.0;
		var bp      = h.CalculateSingleBodyPosition(start_ut, b.SwephBody(), b, BodyType.Other);

		// Body is currently direct
		if (bp.SpeedLongitude >= 0)
		{
			start_ut = GotoNextRetroSolarCusp(start_ut);
			var lower_ut  = start_ut;
			var higher_ut = start_ut;
			becomesDirect = false;
			while (true)
			{
				lower_ut  = higher_ut;
				higher_ut = lower_ut + ut_step;

				// Find speeds
				var bp_l = h.CalculateSingleBodyPosition(lower_ut, b.SwephBody(), b, BodyType.Other);
				var bp_h = h.CalculateSingleBodyPosition(higher_ut, b.SwephBody(), b, BodyType.Other);

				//Mhora.Log.Debug ("DChecking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				// If first one is retro, we're exactly at the cusp
				// If higher is still direct, contine
				if (bp_l.SpeedLongitude > 0 && bp_h.SpeedLongitude < 0)
				{
					break;
				}

				if (bp_l.SpeedLongitude < 0 && bp_h.SpeedLongitude < 0)
				{
					return findNextCuspForward(lower_ut, ref becomesDirect);
				}
			}

			// Within one day period
			return BinaryCuspSearch(lower_ut, higher_ut, true);
		}

		// Body is current retrograde
		else
		{
			var lower_ut  = start_ut;
			var higher_ut = start_ut;
			becomesDirect = true;
			while (true)
			{
				lower_ut  = higher_ut;
				higher_ut = lower_ut + ut_step;
				// Find speeds
				var bp_l = h.CalculateSingleBodyPosition(lower_ut, b.SwephBody(), b, BodyType.Other);
				var bp_h = h.CalculateSingleBodyPosition(higher_ut, b.SwephBody(), b, BodyType.Other);

				//Mhora.Log.Debug ("R Checking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
				if (bp_l.SpeedLongitude < 0 && bp_h.SpeedLongitude >= 0)
				{
					break;
				}

				if (bp_l.SpeedLongitude > 0 && bp_h.SpeedLongitude > 0)
				{
					return findNextCuspForward(lower_ut, ref becomesDirect);
				}
			}

			// Within one day period
			return BinaryCuspSearch(lower_ut, higher_ut, false);
		}
	}
}