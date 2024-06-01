using System;
using Mhora.SwissEph;
using Newtonsoft.Json;

namespace Mhora.Util
{
	[JsonObject]
	public class JulianDate : ICloneable
	{
		private readonly int      _yearsBC;
		private readonly double   _value;
		private readonly DateTime _date;

		// The Julian day number is a system of numbering all days continously
		// within the time range of known human history. It should be familiar
		// to every astrological or astronomical programmer. The time variable
		// in astronomical theories is usually expressed in Julian days or
		// Julian centuries (36525 days per century) relative to some start day;
		// the start day is called 'the epoch'.
		// The Julian day number is a double representing the number of
		// days since JD = 0.0 on 1 Jan -4712, 12:00 noon (in the Julian calendar).
		// 
		// Midnight has always a JD with fraction .5, because traditionally
		// the astronomical day started at noon. This was practical because
		// then there was no change of date during a night at the telescope.
		// From this comes also the fact the noon ephemerides were printed
		// before midnight ephemerides were introduced early in the 20th century.
		//
		// Be aware the we use astronomical year numbering for the years
		// before Christ, not the historical year numbering.
		// Astronomical years are done with negative numbers, historical
		// years with indicators BC or BCE (before common era).
		// Year  0 (astronomical)  	= 1 BC historical year
		public JulianDate(double value)
		{
			_value = value;
			sweph.RevJul(value, out var year, out var month, out var day, out var hours);
			if (year < 1)
			{
				_yearsBC = 1 - year;
				year  = 1;
			}

			_date = new DateTime(year, month, day).AddHours(hours);
		}

		public bool IsEmpty => (_value == 0);

		public JulianDate(DateTime dateTime, int yearsBC = 0)
		{
			_yearsBC = yearsBC;
			_date    = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);

			var year = _date.Year + _yearsBC;

			_value = sweph.JulDay(year, dateTime.Month, dateTime.Day, dateTime.Time().TotalHours);
		}

		[JsonProperty]
		public int YearsBC => _yearsBC;
		[JsonProperty]
		public DateTime Date     => _date;
		[JsonIgnore]
		public Time     Time     => _date.Time();
		[JsonIgnore]
		public double Value => _value;

		public override string ToString()
		{
			var dateTime = $"{_date}";
			if (_yearsBC != 0)
			{
				return $"{_yearsBC}+{dateTime}";
			}
			return dateTime;
		}

		public object Clone() => new JulianDate(_value);

		public JulianDate Add(Time offset)
		{
			DateTime dateTime = this;
			return new JulianDate(dateTime.Add(offset));
		}

		public JulianDate Sub(Time offset)
		{
			DateTime dateTime = this;
			return new JulianDate(dateTime.Subtract(offset));
		}
		
		public static implicit operator double(JulianDate jd) => jd._value;

		public static implicit operator JulianDate(double jd) => new(jd);

		public static implicit operator JulianDate(DateTime dateTime) => new(dateTime);

		public static implicit operator DateTime(JulianDate jd) => jd.Date;

		public static JulianDate operator +(JulianDate jd, Time time) => (jd.Add(time));

		public static JulianDate operator -(JulianDate jd, Time time) => (jd.Sub(time));

		public static JulianDate operator -(JulianDate jd, double days) => (new(jd._value - days));

		public static JulianDate operator +(JulianDate jd, double days) => (new(jd._value + days));

		public static bool operator < (JulianDate  jd, JulianDate value) => jd._value < value._value; 
		public static bool operator > (JulianDate  jd, JulianDate value) => jd._value > value._value; 
		public static bool operator == (JulianDate jd, JulianDate value) => jd?._value == value?._value; 
		public static bool operator != (JulianDate jd, JulianDate value) => jd?._value != value?._value; 
		public static bool operator >= (JulianDate jd, JulianDate value) => jd._value >= value._value; 
		public static bool operator <= (JulianDate jd, JulianDate value) => jd._value <= value._value; 
	}
}
