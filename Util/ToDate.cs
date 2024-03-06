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
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Calculation;

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
	private          TimeSpan  _offset;

	public ToDate(double jd, double yearLength, double compression, Horoscope h)
	{
		_h          = h;
		_baseUt     = jd;
		_yearLength = yearLength;
		_type       = DateType.FixedYear;
		if (compression > 0)
		{
			_compression = compression;
		}
		else
		{
			_compression = 1;
		}
	}

	public ToDate(double jd, DateType type, double yearLength, double compression, Horoscope h)
	{
		_baseUt      = jd;
		_type        = type;
		_yearLength  = yearLength;
		_h           = h;
		_spos        = _h.GetPosition(Body.Sun).Longitude.Value;
		_mpos        = _h.GetPosition(Body.Moon).Longitude.Value;

		if (compression > 0)
		{
			_compression = (100 / compression);
		}
		else
		{
			_compression = 1;
		}
	}

	public void SetOffset(TimeSpan offset)
	{
		_offset = offset;
	}


	public DateTime AddPraveshYears(double y, Func<double, Ref<bool>, Longitude> returnLonFunc, int numMonths, int numDays)
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
		Ref<bool> bDiscard = new(true);
		Longitude l        = null;

		Debug.Assert(y >= 0, "pravesh years only work in the future");
		var sun = _h.FindGrahas(DivisionType.Rasi) [Body.Sun];
		soff    = _h.GetPosition(Body.Sun).Longitude.ToZodiacHouseOffset();
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
		var ut = _h.Info.Jd + tYears * TimeUtils.SiderealYear.TotalDays;
		jd  = ut.LinearSearch(l, sun.CalculateLongitude);
		var yogaStart = returnLonFunc(jd, bDiscard).Value;
		var yogaEnd   = returnLonFunc(_h.Info.Jd, bDiscard).Value;
		jdSt = jd + (yogaEnd - yogaStart) / 360.0 * 28.0;
		if (yogaEnd < yogaStart)
		{
			jdSt += 28.0;
		}

		l  = new Longitude(yogaEnd);
		jd = ut.LinearSearch(new Longitude(yogaEnd), returnLonFunc);
		for (var i = 1; i <= tMonths; i++)
		{
			ut = jd + 30.0;
			jd = ut.LinearSearch(new Longitude(yogaEnd), returnLonFunc);
		}

		l    =  l.Add(new Longitude(tDays * (360.0 / numDays)));
		jdSt =  jd + tDays; // * 25.0/30.0;
		jd   =  jdSt.LinearSearch(l, returnLonFunc);
		jd   += _h.Info.DstOffset.TotalDays;

		return jd.ToUtc();
	}

	public DateTime AddYears(TimeOffset years)
	{
		DateTime dateTime;
		years *= _compression;

		switch (_type)
		{
			case DateType.FixedYear:
			case DateType.SolarYear:
			{
				dateTime =  AddYearsInternal(0).AddYears(years.Years);
				dateTime += years.Remainder;
			}
			break;

			default:
			{
				dateTime = AddYearsInternal(years.TotalYears);
			}
			break;
		}
		return (dateTime.Add(_offset));
	}

	public DateTime AddYears(double years)
	{
		var timeOffset = new TimeOffset(years);
		return AddYears(timeOffset);
	}

	private DateTime _AddYearsInternal(double ut, double years)
	{
		years *= _yearLength;
		return TimeUtils.CalculateDate(ut, years).Add(_offset);
	}

	private DateTime AddYearsInternal(double years)
	{
		DateTime  start;
		double    jd;
		double    lon    = 0;
		var       ut     = 0.0;
		Longitude l      = null;
		var       grahas = _h.FindGrahas(DivisionType.Rasi);
		var       sun    = grahas [Body.Sun];
		var       moon   = grahas[Body.Moon];

		switch (_type)
		{
			case DateType.FixedYear:
				//Mhora.Log.Debug("Finding {0} fixed years of length {1}", years, yearLength);
				jd = _baseUt;
				//Mhora.Log.Debug("tz = {0}", (h.info.tz.toDouble()) / 24.0);
				break;
			case DateType.SolarYear:
				// Turn into years of 360 degrees, and then search
				l  = new Longitude(_spos);
				jd = _baseUt.LinearSearch(l, sun.CalculateLongitude);
				break;
			case DateType.TithiPraveshYear:
				return AddPraveshYears(years, grahas.Calc(Body.Moon, Body.Sun, true), 13, 30);
			case DateType.KaranaPraveshYear:
				return AddPraveshYears(years, grahas.Calc(Body.Moon, Body.Sun, true), 13, 60);
			case DateType.YogaPraveshYear:
				return AddPraveshYears(years, grahas.Calc(Body.Moon, Body.Sun, false), 15, 27);
			case DateType.NakshatraPraveshYear:
				return AddPraveshYears(years, moon.CalculateLongitude, 13, 27);
			case DateType.TithiYear:
				jd =  _h.Info.Jd;
				var tithiBase = new Longitude(_mpos - _spos);
				var days       = years * _yearLength;
				//Mhora.Log.Debug("Find {0} tithi days", days);
				while (days >= 30 * 12.0)
				{
					ut   =  jd + 29.52916 * 12.0;
					jd   =  ut.LinearSearch(tithiBase, grahas.Calc(Body.Moon, Body.Sun, true));
					days -= 30 * 12.0;
				}

				tithiBase = tithiBase.Add(new Longitude(days * 12.0));
				//Mhora.Log.Debug ("Searching from {0} for {1}", t.LongitudeOfTithiDir(jd+days*28.0/30.0), tithi_base);
				ut = jd + days * 28.0 / 30.0;
				jd =  ut.LinearSearch(tithiBase, grahas.Calc(Body.Moon, Body.Sun, true));
				break;
			case DateType.YogaYear:
				jd =  _h.Info.Jd;
				var yogaBase = new Longitude(_mpos + _spos);
				var yogaDays  = years * _yearLength;
				//Mhora.Log.Debug ("Find {0} yoga days", yogaDays);
				while (yogaDays >= 27 * 12)
				{
					ut       =  jd + 305;
					jd       =  ut.LinearSearch(yogaBase, grahas.Calc(Body.Moon, Body.Sun, false));
					yogaDays -= 27 * 12;
				}

				yogaBase =  yogaBase.Add(new Longitude(yogaDays * (360.0 / 27.0)));
				ut       =  jd + yogaDays * 28.0 / 30.0;
				jd       =  ut.LinearSearch(yogaBase, grahas.Calc(Body.Moon, Body.Sun, false));
				jd       += _h.Info.DstOffset.TotalDays;
				break;
			default:
				//years = years * yearLength;
				if (years >= 0)
				{
					lon = (years - Math.Floor(years)) * 4320;
				}
				else
				{
					lon = (years - Math.Ceiling(years)) * 4320;
				}

				lon        *= _yearLength / 360.0;
				var newBaseut = _h.Info.Jd;
				var tithi     = grahas.Calc(newBaseut, Body.Moon, Body.Sun, true);
				l = tithi.Add(new Longitude(lon));
				//Mhora.Log.Debug("{0} {1} {2}", 354.35, 354.35*yearLength/360.0, yearLength);
				var tyearApprox = 354.35 * _yearLength / 360.0; /*357.93765*/
				var lapp        = grahas.Calc(newBaseut + years * tyearApprox, Body.Moon, Body.Sun, true).Value;
				ut = newBaseut + years * tyearApprox;
				jd =  ut.LinearSearch(l, grahas.Calc(Body.Moon, Body.Sun, true));
				break;
		}

		return jd.ToUtc();
	}
}