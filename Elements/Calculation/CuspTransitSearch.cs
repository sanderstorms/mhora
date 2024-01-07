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

using Mhora.SwissEph;

namespace Mhora.Elements.Calculation;

internal class CuspTransitSearch
{
	private readonly Horoscope h;

	public CuspTransitSearch(Horoscope _h)
	{
		h = _h;
	}

	private double DirectSpeed(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:   return 365.2425;
			case Body.BodyType.Moon:  return 28.0;
			case Body.BodyType.Lagna: return 1.0;
		}

		return 0.0;
	}

	public double TransitSearchDirect(Body.BodyType SearchBody, Moment StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
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
		sweph.releaseLock(h);
		return found_ut;
	}


	public double TransitSearch(Body.BodyType SearchBody, Moment StartDate, bool Forward, Longitude TransitPoint, Longitude FoundLon, ref bool bForward)
	{
		if (SearchBody == Body.BodyType.Sun || SearchBody == Body.BodyType.Moon)
		{
			return TransitSearchDirect(SearchBody, StartDate, Forward, TransitPoint, FoundLon, ref bForward);
		}

		if (((int) SearchBody <= (int) Body.BodyType.Moon || (int) SearchBody > (int) Body.BodyType.Saturn) && SearchBody != Body.BodyType.Lagna)
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