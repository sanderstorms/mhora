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
using Mhora.Calculation;
using Mhora.SwissEph;
using Mhora.Tables;

namespace Mhora;

public delegate Longitude ReturnLon(double ut, ref bool dirForward);

/// <summary>
///     Summary description for Transits.
/// </summary>
public class NonLinearTransit
{
    private readonly Tables.Body.Name b;
    private readonly Horoscope        h;

    public NonLinearTransit(Horoscope _h, Tables.Body.Name _b)
    {
        h = _h;
        b = _b;
    }

    public int BodyNameToSweph(Tables.Body.Name b)
    {
        switch (b)
        {
            case Tables.Body.Name.Sun:     return sweph.SE_SUN;
            case Tables.Body.Name.Moon:    return sweph.SE_MOON;
            case Tables.Body.Name.Mars:    return sweph.SE_MARS;
            case Tables.Body.Name.Mercury: return sweph.SE_MERCURY;
            case Tables.Body.Name.Jupiter: return sweph.SE_JUPITER;
            case Tables.Body.Name.Venus:   return sweph.SE_VENUS;
            case Tables.Body.Name.Saturn:  return sweph.SE_SATURN;
            default:                       throw new Exception();
        }
    }

    public Longitude GetLongitude(double ut, ref bool bForwardDir)
    {
        var swephBody = BodyNameToSweph(b);
        var bp        = Basics.CalculateSingleBodyPosition(ut, swephBody, b, Tables.Body.Type.Other, h);
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

public class Retrogression
{
    private readonly Tables.Body.Name b;
    private readonly Horoscope        h;

    public Retrogression(Horoscope _h, Tables.Body.Name _b)
    {
        //Debug.Assert((int)_b >= (int)Body.Type.Moon &&
        //	(int)_b <= (int)Body.Type.Saturn, 
        //	string.Format("Retrogression::Retrogression. Invalid Body {0}", _b));
        h = _h;
        b = _b;
    }

    public void getRetroSolarCusps(ref Longitude start, ref Longitude end)
    {
        switch (b)
        {
            case Tables.Body.Name.Mars:
                start.value = 211;
                end.value   = 232;
                break;
            case Tables.Body.Name.Jupiter:
                start.value = 240;
                end.value   = 248;
                break;
            case Tables.Body.Name.Saturn:
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
			BodyPosition bp_sun = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(Body.Type.Sun), Body.Type.Sun, BodyType.Type.Other);
			BodyPosition bp_b = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, BodyType.Type.Other);
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
        var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
        while (Transit.CircLonLessThan(bp.longitude, lonToFind))
        {
            //mhora.Log.Debug("- {0} {1}", bp.longitude.value, lonToFind.value);
            ut++;
            bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
        }

        while (Transit.CircLonLessThan(lonToFind, bp.longitude))
        {
            //mhora.Log.Debug("+ {0} {1}", bp.longitude.value, lonToFind.value);
            ut--;
            bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
        }

        return ut;
    }

    public double GetTransitBackward(double ut, Longitude lonToFind)
    {
        if (b == Tables.Body.Name.Lagna)
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

            var bp_next = Basics.CalculateSingleBodyPosition(ut_curr, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
            var bp_curr = Basics.CalculateSingleBodyPosition(ut_next, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

            //mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

            if (false == becomesDirect && lonToFind.sub(bp_curr.longitude).value <= bp_next.longitude.sub(bp_curr.longitude).value)
            {
                //mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
                break;
            }

            if (becomesDirect && lonToFind.sub(bp_next.longitude).value <= bp_curr.longitude.sub(bp_next.longitude).value)
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

            int    day  = 0, month = 0, year = 0;
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

            int    day  = 0, month = 0, year = 0;
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
        if (b == Tables.Body.Name.Lagna)
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

            var bp_curr = Basics.CalculateSingleBodyPosition(ut_curr, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
            var bp_next = Basics.CalculateSingleBodyPosition(ut_next, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

            //mhora.Log.Debug ("{0}, {1}, {2}", becomesDirect, bp_curr.longitude, bp_next.longitude);

            if (false == becomesDirect && lonToFind.sub(bp_curr.longitude).value <= bp_next.longitude.sub(bp_curr.longitude).value)
            {
                //mhora.Log.Debug ("+ Found {0} between {1} and {2}", lonToFind, bp_curr.longitude, bp_next.longitude);
                break;
            }

            if (becomesDirect && lonToFind.sub(bp_next.longitude).value <= bp_curr.longitude.sub(bp_next.longitude).value)
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
        var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
        return bp.speed_longitude;
    }

    public Longitude GetLon(double ut, ref bool bForward)
    {
        if (b == Tables.Body.Name.Lagna)
        {
            return new Longitude(sweph.Lagna(ut));
        }

        var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
        bForward = bp.speed_longitude >= 0;
        return bp.longitude;
    }

    public Longitude GetLon(double ut)
    {
        var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
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
        var bp      = Basics.CalculateSingleBodyPosition(start_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

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
                var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
                var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

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
                var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
                var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

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
        var bp      = Basics.CalculateSingleBodyPosition(start_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

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
                var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
                var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

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
                var bp_l = Basics.CalculateSingleBodyPosition(lower_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
                var bp_h = Basics.CalculateSingleBodyPosition(higher_ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);

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

public class Transit
{
    private readonly Tables.Body.Name b;
    private readonly Horoscope        h;

    public Transit(Horoscope _h)
    {
        h = _h;
        b = Tables.Body.Name.Other;
    }

    public Transit(Horoscope _h, Tables.Body.Name _b)
    {
        h = _h;
        b = _b;
    }


    public Longitude LongitudeOfSun(double ut, ref bool bDirRetro)
    {
        var bp = Basics.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Tables.Body.Name.Sun, Tables.Body.Type.Graha, h);
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
        if (b == Tables.Body.Name.Lagna)
        {
            return new Longitude(sweph.Lagna(ut));
        }

        var bp = Basics.CalculateSingleBodyPosition(ut, sweph.BodyNameToSweph(b), b, Tables.Body.Type.Other, h);
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
        var bp_sun  = Basics.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Tables.Body.Name.Sun, Tables.Body.Type.Graha, h);
        var bp_moon = Basics.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Tables.Body.Name.Moon, Tables.Body.Type.Graha, h);
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
        var bp_moon = Basics.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Tables.Body.Name.Moon, Tables.Body.Type.Graha, h);
        return bp_moon.longitude.add(0);
    }

    public Longitude LongitudeOfSunMoonYogaDir(double ut, ref bool bDirRetro)
    {
        bDirRetro = false;
        return LongitudeOfSunMoonYoga(ut);
    }

    public Longitude LongitudeOfSunMoonYoga(double ut)
    {
        var bp_sun  = Basics.CalculateSingleBodyPosition(ut, sweph.SE_SUN, Tables.Body.Name.Sun, Tables.Body.Type.Graha, h);
        var bp_moon = Basics.CalculateSingleBodyPosition(ut, sweph.SE_MOON, Tables.Body.Name.Moon, Tables.Body.Type.Graha, h);
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

    public double NonLinearSearch(double ut, Tables.Body.Name b, Longitude lon_to_find, ReturnLon func)
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

internal class CuspTransitSearch
{
    private readonly Horoscope h;

    public CuspTransitSearch(Horoscope _h)
    {
        h = _h;
    }

    private double DirectSpeed(Tables.Body.Name b)
    {
        switch (b)
        {
            case Tables.Body.Name.Sun:   return 365.2425;
            case Tables.Body.Name.Moon:  return 28.0;
            case Tables.Body.Name.Lagna: return 1.0;
        }

        return 0.0;
    }

    public double TransitSearchDirect(Tables.Body.Name SearchBody, Moment StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
    {
        var bDiscard = true;

        sweph.obtainLock(h);
        var t        = new Transit(h, SearchBody);
        var ut_base  = StartDate.toUniversalTime() - h.info.TimeZone.toDouble() / 24.0;
        var lon_curr = t.GenericLongitude(ut_base, ref bDiscard);
        sweph.releaseLock(h);

        double diff = 0;
        diff = TransitPoint.sub(lon_curr).value;

        if (false == Forward)
        {
            diff -= 360.0;
        }

        var ut_diff_approx = diff / 360.0 * DirectSpeed(SearchBody);
        sweph.obtainLock(h);
        double found_ut = 0;

        if (SearchBody == Tables.Body.Name.Lagna)
        {
            found_ut = t.LinearSearchBinary(ut_base + ut_diff_approx - 3.0 / 24.0, ut_base + ut_diff_approx + 3.0 / 24.0, TransitPoint, t.GenericLongitude);
        }
        else
        {
            found_ut = t.LinearSearch(ut_base + ut_diff_approx, TransitPoint, t.GenericLongitude);
        }

        FoundLon.value = t.GenericLongitude(found_ut, ref bForward).value;
        bForward       = true;
        sweph.releaseLock(h);
        return found_ut;
    }


    public double TransitSearch(Tables.Body.Name SearchBody, Moment StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
    {
        if (SearchBody == Tables.Body.Name.Sun || SearchBody == Tables.Body.Name.Moon)
        {
            return TransitSearchDirect(SearchBody, StartDate, Forward, TransitPoint, FoundLon, ref bForward);
        }

        if (((int) SearchBody <= (int) Tables.Body.Name.Moon || (int) SearchBody > (int) Tables.Body.Name.Saturn) && SearchBody != Tables.Body.Name.Lagna)
        {
            return StartDate.toUniversalTime();
        }

        sweph.obtainLock(h);

        var r = new Retrogression(h, SearchBody);

        var julday_ut = StartDate.toUniversalTime() - h.info.tz.toDouble() / 24.0;
        var found_ut  = julday_ut;

        if (Forward)
        {
            found_ut = r.GetTransitForward(julday_ut, TransitPoint);
        }
        else
        {
            found_ut = r.GetTransitBackward(julday_ut, TransitPoint);
        }

        FoundLon.value = r.GetLon(found_ut, ref bForward).value;

        sweph.releaseLock(h);
        return found_ut;
    }
}