using System;
using Mhora.SwissEph;
using Newtonsoft.Json;

namespace Mhora.Util
{
	[JsonObject]
	public class JulianDay
	{
		private readonly DateTime _date;
		private readonly Time     _time;

		public JulianDay(double jd)
		{
			sweph.RevJul(jd, out var year, out var month, out var day, out var hours);
			_date = new DateTime(year, month, day);
			_time = hours;
		}

		public JulianDay(DateTime dateTime)
		{
			_date = dateTime.Date;
			_time = dateTime.Time();
		}

		[JsonIgnore]
		public DateTime Date     => _date;
		[JsonIgnore]
		public Time     Time     => _time;
		[JsonProperty]
		public DateTime DateTime => _date.Add(_time);

		public override string ToString()
		{
			return $"{_date} {_time}";
		}

		public static implicit operator double(JulianDay jd)
		{
			return jd.DateTime.ToJulian();
		}

		public static implicit operator JulianDay(double jd)
		{
			return new JulianDay(jd);
		}

		public static implicit operator DateTime(JulianDay jd)
		{
			return jd.DateTime;
		}
	}
}
