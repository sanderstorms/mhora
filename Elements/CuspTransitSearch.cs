using System;
using Mhora.Database.Settings;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements;

public static class CuspTransitSearch
{
	private static double DirectSpeed(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:   return 365.2425;
			case Body.BodyType.Moon:  return 28.0;
			case Body.BodyType.Lagna: return 1.0;
		}

		return 0.0;
	}

	public static double TransitSearchDirect(this Horoscope h, Body.BodyType SearchBody, DateTime StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
	{
		var bDiscard = true;

		var t        = new Transit(h, SearchBody);
		var ut_base  = h.UniversalTime(StartDate);
		var lon_curr = t.GenericLongitude(ut_base, ref bDiscard);

		double diff = 0;
		diff = TransitPoint.sub(lon_curr).value;

		if (false == Forward)
		{
			diff -= 360.0;
		}

		var ut_diff_approx = diff / 360.0 * DirectSpeed(SearchBody);
		double found_ut = 0;

		if (SearchBody == Body.BodyType.Lagna)
		{
			found_ut = t.LinearSearchBinary(ut_base + ut_diff_approx - 3.0 / 24.0, ut_base + ut_diff_approx + 3.0 / 24.0, TransitPoint, t.GenericLongitude);
		}
		else
		{
			found_ut = t.LinearSearch(ut_base + ut_diff_approx, TransitPoint, t.GenericLongitude);
		}

		FoundLon.value = t.GenericLongitude(found_ut, ref bForward).value;
		bForward       = true;
		return found_ut;
	}


	public static double TransitSearch(this Horoscope h, Body.BodyType SearchBody, DateTime StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
	{
		if (SearchBody == Body.BodyType.Sun || SearchBody == Body.BodyType.Moon)
		{
			return h.TransitSearchDirect(SearchBody, StartDate, Forward, TransitPoint, FoundLon, ref bForward);
		}

		if (((int) SearchBody <= (int) Body.BodyType.Moon || (int) SearchBody > (int) Body.BodyType.Saturn) && SearchBody != Body.BodyType.Lagna)
		{
			return StartDate.UniversalTime();
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

		FoundLon.value = r.GetLon(found_ut, ref bForward).value;

		return found_ut;
	}
}