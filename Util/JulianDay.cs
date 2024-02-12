using System;
using Mhora.SwissEph;
using Newtonsoft.Json;

namespace Mhora.Util
{
	[JsonObject]
	public class JulianDay
	{
		private readonly double   _value;
		private readonly DateTime _date;
		private readonly Time     _time;

		public JulianDay(double value)
		{
			_value = value;
			sweph.RevJul(value, out var year, out var month, out var day, out var hours);
			_date = new DateTime(year, month, day);
			_time = hours;
		}

		public JulianDay(DateTime dateTime)
		{
			_date  = dateTime.Date;
			_time  = dateTime.Time();
			_value = sweph.JulDay(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Time().TotalHours);
		}

		[JsonProperty]
		public DateTime Date     => _date;
		[JsonProperty]
		public Time     Time     => _time;

		public override string ToString()
		{
			return $"{_date.Add(_time)}";
		}

		public JulianDay Add(Time offset)
		{
			DateTime dateTime = this;
			return new JulianDay(dateTime.Add(offset));
		}

		public JulianDay Sub(Time offset)
		{
			DateTime dateTime = this;
			return new JulianDay(dateTime.Subtract(offset));
		}
		
		public static implicit operator double(JulianDay jd)
		{
			return jd._value;
		}
		
		public static implicit operator JulianDay(double jd)
		{
			return new JulianDay(jd);
		}

		public static implicit operator DateTime(JulianDay jd)
		{
			return jd.Date.Add(jd.Time);
		}

		public static JulianDay operator +(JulianDay jd, Time      offset) => jd.Add (offset);
		public static JulianDay operator -(JulianDay jd, Time      offset) => jd.Sub (offset);
		public static bool operator < (JulianDay     jd, JulianDay value)  => jd._value < value._value; 
		public static bool operator > (JulianDay     jd, JulianDay value)  => jd._value > value._value; 
		public static bool operator == (JulianDay    jd, JulianDay value)  => jd._value == value._value; 
		public static bool operator != (JulianDay    jd, JulianDay value)  => jd._value != value._value; 
		public static bool operator >= (JulianDay    jd, JulianDay value)  => jd._value >= value._value; 
		public static bool operator <= (JulianDay    jd, JulianDay value)  => jd._value <= value._value; 
	}
}
