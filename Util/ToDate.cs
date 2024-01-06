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
using System.Diagnostics;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;

namespace Mhora.Util;

public class ToDate
{
	public enum DateType
	{
		FixedYear,
		SolarYear,
		TithiYear,
		YogaYear,
		TithiPraveshYear,
		KaranaPraveshYear,
		YogaPraveshYear,
		NakshatraPraveshYear
	}

	private readonly double    baseUT;
	private readonly double    compression;
	private readonly Horoscope h;
	private readonly double    mpos;
	private readonly double    spos;
	private readonly DateType  type;
	private readonly double    yearLength;
	private          double    offset;

	public ToDate(double _baseUT, double _yearLength, double _compression, Horoscope _h)
	{
		baseUT      = _baseUT;
		yearLength  = _yearLength;
		type        = DateType.FixedYear;
		compression = _compression;
		h           = _h;
	}

	public ToDate(double _baseUT, DateType _type, double _yearLength, double _compression, Horoscope _h)
	{
		baseUT      = _baseUT;
		type        = _type;
		yearLength  = _yearLength;
		compression = _compression;
		h           = _h;
		spos        = h.getPosition(Elements.Body.Name.Sun).longitude.value;
		mpos        = h.getPosition(Elements.Body.Name.Moon).longitude.value;
	}

	public void SetOffset(double _offset)
	{
		offset = _offset;
	}


	public Moment AddPraveshYears(double years, ReturnLon returnLonFunc, int numMonths, int numDays)
	{
		var       jd       = 0.0;
		int       year     = 0, month  = 0, day    = 0;
		int       hour     = 0, minute = 0, second = 0;
		double    dhour    = 0, lon    = 0;
		double    soff     = 0;
		double    _years   = 0;
		double    tYears   = 0;
		double    tMonths  = 0;
		double    tDays    = 0;
		double    jd_st    = 0;
		var       bDiscard = true;
		Transit   t        = null;
		Longitude l        = null;

		Debug.Assert(years >= 0, "pravesh years only work in the future");
		t       = new Transit(h);
		soff    = h.getPosition(Elements.Body.Name.Sun).longitude.toZodiacHouseOffset();
		_years  = years;
		tYears  = 0;
		tMonths = 0;
		tDays   = 0;
		tYears  = Math.Floor(_years);
		_years  = (_years - Math.Floor(_years)) * numMonths;
		tMonths = Math.Floor(_years);
		_years  = (_years - Math.Floor(_years)) * numDays;
		tDays   = _years;

		//mhora.Log.Debug ("Searching for {0} {1} {2}", tYears, tMonths, tDays);
		lon = spos - soff;
		l   = new Longitude(lon);
		jd  = t.LinearSearch(h.baseUT + tYears * 365.2425, l, t.LongitudeOfSun);
		var yoga_start = returnLonFunc(jd, ref bDiscard).value;
		var yoga_end   = returnLonFunc(h.baseUT, ref bDiscard).value;
		jd_st = jd + (yoga_end - yoga_start) / 360.0 * 28.0;
		if (yoga_end < yoga_start)
		{
			jd_st += 28.0;
		}

		l  = new Longitude(yoga_end);
		jd = t.LinearSearch(jd_st, new Longitude(yoga_end), returnLonFunc);
		for (var i = 1; i <= tMonths; i++)
		{
			jd = t.LinearSearch(jd + 30.0, new Longitude(yoga_end), returnLonFunc);
		}

		l     =  l.add(new Longitude(tDays * (360.0 / numDays)));
		jd_st =  jd + tDays; // * 25.0/30.0;
		jd    =  t.LinearSearch(jd_st, l, returnLonFunc);
		jd    += h.info.tz.toDouble() / 24.0;
		jd    += offset;

		sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
		Moment.doubleToHMS(dhour, ref hour, ref minute, ref second);
		return new Moment(year, month, day, hour, minute, second);
	}

	public Moment AddYears(double years)
	{
		var       jd         = 0.0;
		int       year       = 0, month  = 0, day    = 0;
		int       hour       = 0, minute = 0, second = 0;
		double    dhour      = 0, lon    = 0;
		var       new_baseut = 0.0;
		Transit   t          = null;
		Longitude l          = null;
		if (compression > 0.0)
		{
			years *= compression;
		}

		switch (type)
		{
			case DateType.FixedYear:
				//mhora.Log.Debug("Finding {0} fixed years of length {1}", years, yearLength);
				jd = baseUT + years * yearLength;
				//mhora.Log.Debug("tz = {0}", (h.info.tz.toDouble()) / 24.0);
				jd += offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				Moment.doubleToHMS(dhour, ref hour, ref minute, ref second);
				return new Moment(year, month, day, hour, minute, second);
			case DateType.SolarYear:
				// Turn into years of 360 degrees, and then search
				years = years * yearLength / 360.0;
				t     = new Transit(h);
				if (years >= 0)
				{
					lon = (years - Math.Floor(years)) * 360.0;
				}
				else
				{
					lon = (years - Math.Ceiling(years)) * 360.0;
				}

				l  =  new Longitude(lon       + spos);
				jd =  t.LinearSearch(h.baseUT + years * 365.2425, l, t.LongitudeOfSun);
				jd += h.info.tz.toDouble() / 24.0;
				jd += offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				Moment.doubleToHMS(dhour, ref hour, ref minute, ref second);
				return new Moment(year, month, day, hour, minute, second);
			case DateType.TithiPraveshYear:
				t = new Transit(h);
				return AddPraveshYears(years, t.LongitudeOfTithiDir, 13, 30);
			case DateType.KaranaPraveshYear:
				t = new Transit(h);
				return AddPraveshYears(years, t.LongitudeOfTithiDir, 13, 60);
			case DateType.YogaPraveshYear:
				t = new Transit(h);
				return AddPraveshYears(years, t.LongitudeOfSunMoonYogaDir, 15, 27);
			case DateType.NakshatraPraveshYear:
				t = new Transit(h);
				return AddPraveshYears(years, t.LongitudeOfMoonDir, 13, 27);
			case DateType.TithiYear:
				jd -= h.info.tz.toDouble() / 24.0;
				t  =  new Transit(h);
				jd =  h.baseUT;
				var tithi_base = new Longitude(mpos - spos);
				var days       = years * yearLength;
				//mhora.Log.Debug("Find {0} tithi days", days);
				while (days >= 30 * 12.0)
				{
					jd   =  t.LinearSearch(jd + 29.52916 * 12.0, tithi_base, t.LongitudeOfTithiDir);
					days -= 30 * 12.0;
				}

				tithi_base = tithi_base.add(new Longitude(days * 12.0));
				//mhora.Log.Debug ("Searching from {0} for {1}", t.LongitudeOfTithiDir(jd+days*28.0/30.0), tithi_base);
				jd =  t.LinearSearch(jd + days * 28.0 / 30.0, tithi_base, t.LongitudeOfTithiDir);
				jd += h.info.tz.toDouble() / 24.0;
				jd += offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				Moment.doubleToHMS(dhour, ref hour, ref minute, ref second);
				return new Moment(year, month, day, hour, minute, second);
			case DateType.YogaYear:
				jd -= h.info.tz.toDouble() / 24.0;
				t  =  new Transit(h);
				jd =  h.baseUT;
				var yoga_base = new Longitude(mpos + spos);
				var yogaDays  = years * yearLength;
				//mhora.Log.Debug ("Find {0} yoga days", yogaDays);
				while (yogaDays >= 27 * 12)
				{
					jd       =  t.LinearSearch(jd + 305, yoga_base, t.LongitudeOfSunMoonYogaDir);
					yogaDays -= 27 * 12;
				}

				yoga_base =  yoga_base.add(new Longitude(yogaDays * (360.0 / 27.0)));
				jd        =  t.LinearSearch(jd + yogaDays * 28.0 / 30.0, yoga_base, t.LongitudeOfSunMoonYogaDir);
				jd        += h.info.tz.toDouble() / 24.0;
				jd        += offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				Moment.doubleToHMS(dhour, ref hour, ref minute, ref second);
				return new Moment(year, month, day, hour, minute, second);
			default:
				//years = years * yearLength;
				t = new Transit(h);
				if (years >= 0)
				{
					lon = (years - Math.Floor(years)) * 4320;
				}
				else
				{
					lon = (years - Math.Ceiling(years)) * 4320;
				}

				lon        *= yearLength / 360.0;
				new_baseut =  h.baseUT;
				var tithi = t.LongitudeOfTithi(new_baseut);
				l = tithi.add(new Longitude(lon));
				//mhora.Log.Debug("{0} {1} {2}", 354.35, 354.35*yearLength/360.0, yearLength);
				var tyear_approx = 354.35 * yearLength / 360.0; /*357.93765*/
				var lapp         = t.LongitudeOfTithi(new_baseut + years * tyear_approx).value;
				jd =  t.LinearSearch(new_baseut + years * tyear_approx, l, t.LongitudeOfTithiDir);
				jd += offset;
				//jd += (h.info.tz.toDouble() / 24.0);
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				Moment.doubleToHMS(dhour, ref hour, ref minute, ref second);
				return new Moment(year, month, day, hour, minute, second);
		}
	}
}