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
using Mhora.Components.Delegates;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;
using Transit = Mhora.Elements.Transit;

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

	private readonly double    _baseUt;
	private readonly double    _compression;
	private readonly Horoscope _h;
	private readonly double    _mpos;
	private readonly double    _spos;
	private readonly DateType  _type;
	private readonly double    _yearLength;
	private          double    _offset;

	public ToDate(double jd, double yearLength, double compression, Horoscope h)
	{
		_baseUt           = jd;
		_yearLength  = yearLength;
		_type             = DateType.FixedYear;
		_compression = compression;
		_h           = h;
	}

	public ToDate(double jd, DateType type, double yearLength, double compression, Horoscope h)
	{
		_baseUt           = jd;
		_type        = type;
		_yearLength  = yearLength;
		_compression = compression;
		_h           = h;
		_spos             = _h.GetPosition(Body.BodyType.Sun).Longitude.Value;
		_mpos             = _h.GetPosition(Body.BodyType.Moon).Longitude.Value;
	}

	public void SetOffset(double offset)
	{
		_offset = offset;
	}


	public DateTime AddPraveshYears(double y, ReturnLon returnLonFunc, int numMonths, int numDays)
	{
		var       jd       = 0.0;
		int       year     = 0, month  = 0, day    = 0;
		int       hour     = 0, minute = 0, second = 0;
		double    dhour    = 0, lon    = 0;
		double    soff     = 0;
		double    years   = 0;
		double    tYears   = 0;
		double    tMonths  = 0;
		double    tDays    = 0;
		double    jdSt    = 0;
		var       bDiscard = true;
		Transit   t        = null;
		Longitude l        = null;

		Debug.Assert(y >= 0, "pravesh years only work in the future");
		t       = new Transit(_h);
		soff    = _h.GetPosition(Body.BodyType.Sun).Longitude.ToZodiacHouseOffset();
		years  = y;
		tYears  = 0;
		tMonths = 0;
		tDays   = 0;
		tYears  = Math.Floor(years);
		years  = (years - Math.Floor(years)) * numMonths;
		tMonths = Math.Floor(years);
		years  = (years - Math.Floor(years)) * numDays;
		tDays   = years;

		//Mhora.Log.Debug ("Searching for {0} {1} {2}", tYears, tMonths, tDays);
		lon = _spos - soff;
		l   = new Longitude(lon);
		jd  = t.LinearSearch(_h.Info.Jd + tYears * 365.2425, l, t.LongitudeOfSun);
		var yogaStart = returnLonFunc(jd, ref bDiscard).Value;
		var yogaEnd   = returnLonFunc(_h.Info.Jd, ref bDiscard).Value;
		jdSt = jd + (yogaEnd - yogaStart) / 360.0 * 28.0;
		if (yogaEnd < yogaStart)
		{
			jdSt += 28.0;
		}

		l  = new Longitude(yogaEnd);
		jd = t.LinearSearch(jdSt, new Longitude(yogaEnd), returnLonFunc);
		for (var i = 1; i <= tMonths; i++)
		{
			jd = t.LinearSearch(jd + 30.0, new Longitude(yogaEnd), returnLonFunc);
		}

		l     =  l.Add(new Longitude(tDays * (360.0 / numDays)));
		jdSt =  jd + tDays; // * 25.0/30.0;
		jd    =  t.LinearSearch(jdSt, l, returnLonFunc);
		jd    += _h.Info.DstOffset.TotalDays;
		jd    += _offset;

		sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
		return new DateTime(year, month, day).Add(TimeSpan.FromHours(dhour));
	}

	public DateTime AddYears(double years)
	{
		var       jd         = 0.0;
		int       year       = 0, month  = 0, day    = 0;
		int       hour       = 0, minute = 0, second = 0;
		double    dhour      = 0, lon    = 0;
		var       newBaseut = 0.0;
		Transit   t          = null;
		Longitude l          = null;
		if (_compression > 0.0)
		{
			years *= _compression;
		}

		switch (_type)
		{
			case DateType.FixedYear:
				//Mhora.Log.Debug("Finding {0} fixed years of length {1}", years, yearLength);
				jd = _baseUt + years * _yearLength;
				//Mhora.Log.Debug("tz = {0}", (h.info.tz.toDouble()) / 24.0);
				jd += _offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				return new DateTime(year, month, day).AddHours(dhour);
			case DateType.SolarYear:
				// Turn into years of 360 degrees, and then search
				years = years * _yearLength / 360.0;
				t     = new Transit(_h);
				if (years >= 0)
				{
					lon = (years - Math.Floor(years)) * 360.0;
				}
				else
				{
					lon = (years - Math.Ceiling(years)) * 360.0;
				}

				l  =  new Longitude(lon       + _spos);
				jd =  t.LinearSearch(_h.Info.Jd + years * 365.2425, l, t.LongitudeOfSun);
				jd += _h.Info.DstOffset.TotalDays;
				jd += _offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				return new DateTime(year, month, day).AddHours(dhour);
			case DateType.TithiPraveshYear:
				t = new Transit(_h);
				return AddPraveshYears(years, t.LongitudeOfTithiDir, 13, 30);
			case DateType.KaranaPraveshYear:
				t = new Transit(_h);
				return AddPraveshYears(years, t.LongitudeOfTithiDir, 13, 60);
			case DateType.YogaPraveshYear:
				t = new Transit(_h);
				return AddPraveshYears(years, t.LongitudeOfSunMoonYogaDir, 15, 27);
			case DateType.NakshatraPraveshYear:
				t = new Transit(_h);
				return AddPraveshYears(years, t.LongitudeOfMoonDir, 13, 27);
			case DateType.TithiYear:
				jd -= _h.Info.DstOffset.TotalDays;
				t  =  new Transit(_h);
				jd =  _h.Info.Jd;
				var tithiBase = new Longitude(_mpos - _spos);
				var days       = years * _yearLength;
				//Mhora.Log.Debug("Find {0} tithi days", days);
				while (days >= 30 * 12.0)
				{
					jd   =  t.LinearSearch(jd + 29.52916 * 12.0, tithiBase, t.LongitudeOfTithiDir);
					days -= 30 * 12.0;
				}

				tithiBase = tithiBase.Add(new Longitude(days * 12.0));
				//Mhora.Log.Debug ("Searching from {0} for {1}", t.LongitudeOfTithiDir(jd+days*28.0/30.0), tithi_base);
				jd =  t.LinearSearch(jd + days * 28.0 / 30.0, tithiBase, t.LongitudeOfTithiDir);
				jd += _h.Info.DstOffset.TotalDays;
				jd += _offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				return new DateTime(year, month, day).AddHours(dhour);
			case DateType.YogaYear:
				jd -= _h.Info.DstOffset.TotalDays;
				t  =  new Transit(_h);
				jd =  _h.Info.Jd;
				var yogaBase = new Longitude(_mpos + _spos);
				var yogaDays  = years * _yearLength;
				//Mhora.Log.Debug ("Find {0} yoga days", yogaDays);
				while (yogaDays >= 27 * 12)
				{
					jd       =  t.LinearSearch(jd + 305, yogaBase, t.LongitudeOfSunMoonYogaDir);
					yogaDays -= 27 * 12;
				}

				yogaBase =  yogaBase.Add(new Longitude(yogaDays * (360.0 / 27.0)));
				jd        =  t.LinearSearch(jd + yogaDays * 28.0 / 30.0, yogaBase, t.LongitudeOfSunMoonYogaDir);
				jd        += _h.Info.DstOffset.TotalDays;
				jd        += _offset;
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				return new DateTime(year, month, day).AddHours(dhour);
			default:
				//years = years * yearLength;
				t = new Transit(_h);
				if (years >= 0)
				{
					lon = (years - Math.Floor(years)) * 4320;
				}
				else
				{
					lon = (years - Math.Ceiling(years)) * 4320;
				}

				lon        *= _yearLength / 360.0;
				newBaseut =  _h.Info.Jd;
				var tithi = t.LongitudeOfTithi(newBaseut);
				l = tithi.Add(new Longitude(lon));
				//Mhora.Log.Debug("{0} {1} {2}", 354.35, 354.35*yearLength/360.0, yearLength);
				var tyearApprox = 354.35 * _yearLength / 360.0; /*357.93765*/
				var lapp         = t.LongitudeOfTithi(newBaseut + years * tyearApprox).Value;
				jd =  t.LinearSearch(newBaseut + years * tyearApprox, l, t.LongitudeOfTithiDir);
				jd += _offset;
				//jd += (h.info.tz.toDouble() / 24.0);
				sweph.RevJul(jd, ref year, ref month, ref day, ref dhour);
				return new DateTime(year, month, day).AddHours(dhour);
		}
	}
}