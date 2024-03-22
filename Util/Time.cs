using System;
using Newtonsoft.Json;

namespace Mhora.Util
{
	[JsonObject]
	public class Time : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>
	{
		[JsonProperty]
		private TimeSpan _timeSpan;

		public Time()
		{
			_timeSpan = TimeSpan.Zero;
		}

		public Time(int days, double hours)
		{
			_timeSpan = TimeSpan.FromDays(days).Add(TimeSpan.FromHours(hours));
		}

		public Time(double hours)
		{
			_timeSpan = TimeSpan.FromHours(hours);
		}

		public Time(int hours, int minutes, double seconds)
		{
			_timeSpan = new TimeSpan(hours, minutes, 0).Add(TimeSpan.FromSeconds(seconds));
		}

		public Time(TimeSpan timeSpan)
		{
			_timeSpan = timeSpan;
		}

		public Time Add(double hours)
		{
			return new Time(_timeSpan.TotalHours + hours);;
		}

		public Time Sub(double hours)
		{
			return new Time(_timeSpan.TotalHours - hours);;
		}

		public Time Mul(double hours)
		{
			return new Time(_timeSpan.TotalHours * hours);
		}

		public Time Div(double hours)
		{
			return new Time(_timeSpan.TotalHours / hours);
		}

		public double TotalDays  => _timeSpan.TotalDays;
		public double TotalHours => _timeSpan.TotalHours;

		//1 day or 24 hours = 60 Ghatis
		public double Ghati => _timeSpan.TotalDays * 60;
		// 1 Ghati = 60 Vighati (also called Pala or Kala)
		public double Vighati => Ghati * 60;
		// 1 Vighati = 60 Linta or (also called Vipala or Vikala)
		public double Vipala => Vighati * 60;
		// 1 Lipta = 60 Vilipta
		public double Vilipta => Vipala * 60;
		// 1 Vilipta = 60 Para
		public double Para => Vilipta * 60;
		// 1 Para = 60 Tatpara
		public double Tatpara => Para * 60;

		public override string ToString()
		{
			return _timeSpan.ToString();
		}

		public static Time operator +(Time time, Time other)
		{
			return time.Add(other.TotalHours);
		}

		public static Time operator -(Time time, Time other)
		{
			return time.Sub(other.TotalHours);
		}

		public static Time operator /(Time time, Time other)
		{
			return time.Div(other.TotalHours);
		}

		public static Time operator *(Time time, Time other)
		{
			return time.Mul(other.TotalHours);
		}

		public static bool operator < (Time time, Time other)
		{
			return time.TotalHours < other.TotalHours;
		}

		public static bool operator > (Time time, Time other)
		{
			return time.TotalHours > other.TotalHours;
		}

		public static Time operator +(Time time, double hours)
		{
			return time.Add(hours);
		}

		public static Time operator -(Time time, double hours)
		{
			return time.Sub(hours);
		}

		public static Time operator /(Time time, double hours)
		{
			return time.Div(hours);
		}

		public static Time operator *(Time time, double hours)
		{
			return time.Mul(hours);
		}

		public static bool operator < (Time time, double hours)
		{
			return time.TotalHours < hours;
		}

		public static bool operator > (Time time, double hours)
		{
			return time.TotalHours > hours;
		}

		public static implicit operator TimeSpan(Time time)
		{
			return time._timeSpan;
		}

		public static implicit operator Time(TimeSpan timeSpan)
		{
			return new Time(timeSpan);
		}

		public static implicit operator Time(double hours)
		{
			return TimeSpan.FromHours(hours);
		}

		public int CompareTo(object   obj)
		{
			return _timeSpan.CompareTo(obj);
		}

		public int CompareTo(TimeSpan other)
		{
			return _timeSpan.CompareTo(other);
		}

		public bool Equals(TimeSpan other)
		{
			return _timeSpan.Equals(other);
		}
    }
}
