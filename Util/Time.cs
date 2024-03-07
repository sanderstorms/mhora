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

		public Time(TimeSpan timeSpan)
		{
			_timeSpan = timeSpan;
		}

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

		public static implicit operator TimeSpan(Time time)
		{
			return time._timeSpan;
		}

		public static implicit operator Time(TimeSpan timeSpan)
		{
			return new Time(timeSpan);
		}

		public static implicit operator double(Time time)
		{
			return time._timeSpan.TotalHours;
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
