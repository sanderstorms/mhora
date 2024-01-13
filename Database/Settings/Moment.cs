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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using Mhora.Components.Converter;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;

namespace Mhora.Database.Settings;

/// <summary>
///     Specified a Moment. This can be used for charts, dasa times etc
/// </summary>
[Serializable]
[TypeConverter(typeof(MomentConverter))]
public class Moment : MhoraSerializableOptions, ICloneable, ISerializable
{
	private int m_day, m_month, m_year, m_hour, m_minute, m_second;

	protected Moment(SerializationInfo info, StreamingContext context) : this()
	{
		Constructor(GetType(), info, context);
	}

	public Moment()
	{
		var t = DateTime.Now;
		day    = t.Day;
		month  = t.Month;
		year   = t.Year;
		hour   = t.Hour;
		minute = t.Minute;
		second = t.Second;
	}

	public static implicit operator DateTime(Moment m)
	{
		return new DateTime(m.year, m.month, m.day, m.hour, m.minute, m.second);
	}

	public Moment(int year, int month, int day, double time)
	{
		m_day   = day;
		m_month = month;
		m_year  = year;
		doubleToHMS(time, ref m_hour, ref m_minute, ref m_second);
	}

	public Moment(int year, int month, int day, int hour, int minute, int second)
	{
		m_day    = day;
		m_month  = month;
		m_year   = year;
		m_hour   = hour;
		m_minute = minute;
		m_second = second;
	}

	public Moment(double tjd_ut, Horoscope h)
	{
		double time = 0;
		tjd_ut += h.info.Timezone.toDouble() / 24.0;
		sweph.RevJul(tjd_ut, ref m_year, ref m_month, ref m_day, ref time);
		doubleToHMS(time, ref m_hour, ref m_minute, ref m_second);
	}

	public double time => m_hour + m_minute / 60.0 + m_second / 3600.0;

	public int day
	{
		get => m_day;
		set => m_day = value;
	}

	public int month
	{
		get => m_month;
		set => m_month = value;
	}

	public int year
	{
		get => m_year;
		set => m_year = value;
	}

	public int hour
	{
		get => m_hour;
		set => m_hour = value;
	}

	public int minute
	{
		get => m_minute;
		set => m_minute = value;
	}

	public int second
	{
		get => m_second;
		set => m_second = value;
	}

	public object Clone()
	{
		return new Moment(m_year, m_month, m_day, m_hour, m_minute, m_second);
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		GetObjectData(GetType(), info, context);
	}

	public static void doubleToHMS(double h, ref int hour, ref int minute, ref int second)
	{
		hour   = (int) Math.Floor(h);
		h      = (h - hour) * 60.0;
		minute = (int) Math.Floor(h);
		h      = (h - minute) * 60.0;
		second = (int) Math.Floor(h);
	}

	public double toUniversalTime()
	{
		return sweph.JulDay(m_year, m_month, m_day, time);
	}

	public double toUniversalTime(Horoscope h)
	{
		var local_ut = sweph.JulDay(year, month, day, time);
		return local_ut - h.info.Timezone.toDouble() / 24.0;
	}

	public static int FromStringMonth(string s)
	{
		switch (s)
		{
			case "Jan": return 1;
			case "Feb": return 2;
			case "Mar": return 3;
			case "Apr": return 4;
			case "May": return 5;
			case "Jun": return 6;
			case "Jul": return 7;
			case "Aug": return 8;
			case "Sep": return 9;
			case "Oct": return 10;
			case "Nov": return 11;
			case "Dec": return 12;
		}

		return 1;
	}

	public string ToStringMonth(int i)
	{
		switch (i)
		{
			case 1:  return "Jan";
			case 2:  return "Feb";
			case 3:  return "Mar";
			case 4:  return "Apr";
			case 5:  return "May";
			case 6:  return "Jun";
			case 7:  return "Jul";
			case 8:  return "Aug";
			case 9:  return "Sep";
			case 10: return "Oct";
			case 11: return "Nov";
			case 12: return "Dec";
		}

		Trace.Assert(false, "Moment::ToStringMonth");
		return string.Empty;
	}

	public override string ToString()
	{
		return (m_day < 10 ? "0" : string.Empty) + m_day + " " + ToStringMonth(m_month) + " " + m_year + " " + (m_hour < 10 ? "0" : string.Empty) + m_hour + ":" + (m_minute < 10 ? "0" : string.Empty) + m_minute + ":" + (m_second < 10 ? "0" : string.Empty) + m_second;
	}

	public string ToShortDateString()
	{
		var year = m_year % 100;
		return string.Format("{0:00}-{1:00}-{2:00}", m_day, m_month, year);
	}

	public string ToDateString()
	{
		return string.Format("{0:00} {1} {2}", m_day, ToStringMonth(m_month), m_year);
	}

	public string ToTimeString()
	{
		return ToTimeString(false);
	}

	public string ToTimeString(bool bDisplaySeconds)
	{
		if (bDisplaySeconds)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", m_hour, m_minute, m_second);
		}

		return string.Format("{0:00}:{1:00}", m_hour, m_minute);
	}
}