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
using Mhora.Body;
using Mhora.SwissEph;

namespace Mhora.Calculation
{
    public class Retrogression
    {
        private readonly Body.Body.Name b;
        private readonly Horoscope h;

        public Retrogression(Horoscope _h, Body.Body.Name _b)
        {
            //Debug.Assert((int)_b >= (int)Body.Name.Moon &&
            //	(int)_b <= (int)Body.Name.Saturn, 
            //	string.Format("Retrogression::Retrogression. Invalid Body {0}", _b));
            h = _h;
            b = _b;
        }

        public void getRetroSolarCusps(ref Longitude start, ref Longitude end)
        {
            switch (b)
            {
                case Body.Body.Name.Mars:
                    start.value = 211;
                    end.value   = 232;
                    break;
                case Body.Body.Name.Jupiter:
                    start.value = 240;
                    end.value   = 248;
                    break;
                case Body.Body.Name.Saturn:
                    start.value = 248;
                    end.value   = 253;
                    break;
            }
        }

        public double gotoNextRetroSolarCusp(double ut)
        {
            return ut;
        #if DND
			Longitude cusp_start = new Longitude(0);
			Longitude cusp_end = new Longitude(0);
			BodyPosition bp_sun = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(Body.Name.Sun), Body.Name.Sun, BodyType.Name.Other);
			BodyPosition bp_b = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other);
			Longitude diff = bp_b.longitude.sub(bp_sun.longitude);
			if (Transit.CircLonLessThan(cusp_start, diff) &&
				Transit.CircLonLessThan(diff, cusp_end))
				return ut;

			Longitude diffIncrease = diff.sub(cusp_start);
			double ret = ut + (diffIncrease.value * 360.0/365.2425);
			return ret;
        #endif
        }

        public double FindClosestTransit(double ut, Longitude lonToFind)
        {
            var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
            while (Transit.CircLonLessThan(bp.longitude, lonToFind))
            {
                //mhora.Log.Debug("- {0} {1}", bp.longitude.value, lonToFind.value);
                ut++;
                bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
            }

            while (Transit.CircLonLessThan(lonToFind, bp.longitude))
            {
                //mhora.Log.Debug("+ {0} {1}", bp.longitude.value, lonToFind.value);
                ut--;
                bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
            }

            return ut;
        }

        public double GetTransitBackward(double ut, Longitude lonToFind)
        {
            if (b == Body.Body.Name.Lagna)
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

                var bp_next = Basics.CalculateSingleBodyPosition(ut_curr, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
                var bp_curr = Basics.CalculateSingleBodyPosition(ut_next, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

                //mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

                if (false                                  == becomesDirect &&
                    lonToFind.sub(bp_curr.longitude).value <= bp_next.longitude.sub(bp_curr.longitude).value)
                {
                    //mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
                    break;
                }

                if (becomesDirect &&
                    lonToFind.sub(bp_next.longitude).value <= bp_curr.longitude.sub(bp_next.longitude).value)
                {
                    //mhora.Log.Debug ("- Found {0} betweeen {1} and {2}", lonToFind, bp_next.longitude, bp_curr.longitude);
                    break;
                }
                //mhora.Log.Debug ("{3} Didn't find {0} betweeen {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude, ut_curr);
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

                int day   = 0,
                    month = 0,
                    year  = 0;
                double hour = 0;
                sweph.RevJul(ut_start, ref year, ref month, ref day, ref hour);
                var m = new Moment(year, month, day, hour);

                //mhora.Log.Debug ("F {3} Lagna search for {0} between {1} and {2}",
                //lonToFind, lon_start, lon_end, m);

                if (lonToFind.sub(lon_start).value <= lon_end.sub(lon_start).value)
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

                int day   = 0,
                    month = 0,
                    year  = 0;
                double hour = 0;
                sweph.RevJul(ut_start, ref year, ref month, ref day, ref hour);
                var m = new Moment(year, month, day, hour);

                //mhora.Log.Debug ("B {3} Lagna search for {0} between {1} and {2}",
                //lonToFind, lon_start, lon_end, m);

                if (lonToFind.sub(lon_end).value <= lon_start.sub(lon_end).value)
                {
                    break;
                }
            }

            return BinaryLonSearch(ut_end, ut_start, lonToFind, true);
        }

        public double GetTransitForward(double ut, Longitude lonToFind)
        {
            if (b == Body.Body.Name.Lagna)
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

                var bp_curr = Basics.CalculateSingleBodyPosition(ut_curr, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
                var bp_next = Basics.CalculateSingleBodyPosition(ut_next, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

                //mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

                if (false                                  == becomesDirect &&
                    lonToFind.sub(bp_curr.longitude).value <= bp_next.longitude.sub(bp_curr.longitude).value)
                {
                    //mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
                    break;
                }

                if (becomesDirect &&
                    lonToFind.sub(bp_next.longitude).value <= bp_curr.longitude.sub(bp_next.longitude).value)
                {
                    //mhora.Log.Debug ("- Found {0} betweeen {1} and {2}", lonToFind, bp_next.longitude, bp_curr.longitude);
                    break;
                }
                //mhora.Log.Debug ("{3} Didn't find {0} betweeen {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude, ut_curr);
            }

            if (false == becomesDirect)
            {
                return BinaryLonSearch(ut_curr, ut_next, lonToFind, true);
            }

            return BinaryLonSearch(ut_curr, ut_next, lonToFind, false);
        }

        public double GetSpeed(double ut)
        {
            var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
            return bp.speed_longitude;
        }

        public Longitude GetLon(double ut, ref bool bForward)
        {
            if (b == Body.Body.Name.Lagna)
            {
                return new Longitude(sweph.Lagna(ut));
            }

            var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
            bForward = bp.speed_longitude >= 0;
            return bp.longitude;
        }

        public Longitude GetLon(double ut)
        {
            var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
            return bp.longitude;
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
                if (lon_to_find.sub(lon_start).value <= lon_middle.sub(lon_start).value)
                {
                    return BinaryLonSearch(ut_start, ut_middle, lon_to_find, normal);
                }

                return BinaryLonSearch(ut_middle, ut_end, lon_to_find, normal);
            }

            if (lon_to_find.sub(lon_end).value <= lon_middle.sub(lon_end).value)
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

            //mhora.Log.Debug ("Speed BinarySearchNormal {0} UT: {2} Speed {1}", b, speed_middle, ut_middle);

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
            var bp      = Basics.CalculateSingleBodyPosition(start_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

            // Body is currently direct
            if (bp.speed_longitude >= 0)
            {
                start_ut = gotoNextRetroSolarCusp(start_ut);
                var lower_ut  = start_ut;
                var higher_ut = start_ut;
                becomesDirect = false;
                while (true)
                {
                    lower_ut  = higher_ut;
                    higher_ut = lower_ut - ut_step;

                    // Find speeds
                    var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
                    var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

                    //mhora.Log.Debug ("DChecking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
                    // If first one is retro, we're exactly at the cusp
                    // If higher is still direct, contine
                    if (bp_l.speed_longitude < 0 && bp_h.speed_longitude > 0)
                    {
                        break;
                    }

                    if (bp_l.speed_longitude > 0 && bp_h.speed_longitude < 0)
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
                    var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
                    var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

                    //mhora.Log.Debug ("R Checking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
                    if (bp_l.speed_longitude > 0 && bp_h.speed_longitude <= 0)
                    {
                        break;
                    }

                    if (bp_l.speed_longitude < 0 && bp_h.speed_longitude > 0)
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
            var bp      = Basics.CalculateSingleBodyPosition(start_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

            // Body is currently direct
            if (bp.speed_longitude >= 0)
            {
                start_ut = gotoNextRetroSolarCusp(start_ut);
                var lower_ut  = start_ut;
                var higher_ut = start_ut;
                becomesDirect = false;
                while (true)
                {
                    lower_ut  = higher_ut;
                    higher_ut = lower_ut + ut_step;

                    // Find speeds
                    var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
                    var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

                    //mhora.Log.Debug ("DChecking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
                    // If first one is retro, we're exactly at the cusp
                    // If higher is still direct, contine
                    if (bp_l.speed_longitude > 0 && bp_h.speed_longitude < 0)
                    {
                        break;
                    }

                    if (bp_l.speed_longitude < 0 && bp_h.speed_longitude < 0)
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
                    var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);
                    var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, BodyType.Name.Other, h);

                    //mhora.Log.Debug ("R Checking daily {0} UT: {1} {2} Speed {3} {4}", b, lower_ut, higher_ut, bp_l.speed_longitude, bp_h.speed_longitude);
                    if (bp_l.speed_longitude < 0 && bp_h.speed_longitude >= 0)
                    {
                        break;
                    }

                    if (bp_l.speed_longitude > 0 && bp_h.speed_longitude > 0)
                    {
                        return findNextCuspForward(lower_ut, ref becomesDirect);
                    }
                }

                // Within one day period
                return BinaryCuspSearch(lower_ut, higher_ut, false);
            }
        }
    }
}