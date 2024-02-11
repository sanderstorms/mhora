using System;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements;

public static class CuspTransitSearch
{
	private static double DirectSpeed(Body b)
	{
		switch (b)
		{
			case Body.Sun:   return TimeUtils.SiderealYear.TotalDays;
			case Body.Moon:  return 28.0;
			case Body.Lagna: return 1.0;
		}

		return 0.0;
	}

	public static double TransitSearchDirect(this Horoscope h, Body SearchBody, DateTime StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
	{
		var bDiscard = true;

		var t        = new Transit(h, SearchBody);
		var ut_base  = h.UniversalTime(StartDate);
		var lon_curr = t.GenericLongitude(ut_base, ref bDiscard);

		double diff = 0;
		diff = TransitPoint.Sub(lon_curr).Value;

		if (false == Forward)
		{
			diff -= 360.0;
		}

		var ut_diff_approx = diff / 360.0 * DirectSpeed(SearchBody);
		double found_ut = 0;

		if (SearchBody == Body.Lagna)
		{
			found_ut = t.LinearSearchBinary(ut_base + ut_diff_approx - 3.0 / 24.0, ut_base + ut_diff_approx + 3.0 / 24.0, TransitPoint, t.GenericLongitude);
		}
		else
		{
			found_ut = t.LinearSearch(ut_base + ut_diff_approx, TransitPoint, t.GenericLongitude);
		}

		FoundLon.Value = t.GenericLongitude(found_ut, ref bForward).Value;
		bForward       = true;
		return found_ut;
	}


	public static double TransitSearch(this Horoscope h, Body SearchBody, DateTime StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
	{
		if (SearchBody == Body.Sun || SearchBody == Body.Moon)
		{
			return h.TransitSearchDirect(SearchBody, StartDate, Forward, TransitPoint, FoundLon, ref bForward);
		}

		if (((int) SearchBody <= (int) Body.Moon || (int) SearchBody > (int) Body.Saturn) && SearchBody != Body.Lagna)
		{
			return StartDate.ToJulian();
		}

		var r = new Retrogression(h, SearchBody);

		var julday_ut = h.UniversalTime(StartDate);
		var found_ut  = julday_ut;

		if (Forward)
		{
			found_ut = r.GetTransitForward(julday_ut, TransitPoint);
		}
		else
		{
			found_ut = r.GetTransitBackward(julday_ut, TransitPoint);
		}

		FoundLon.Value = r.GetLon(found_ut, ref bForward).Value;

		return found_ut;
	}
}