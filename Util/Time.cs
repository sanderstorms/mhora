using System;

namespace Mhora.Util
{
	public class Time : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>
	{
		private TimeSpan _timeSpan;

		public Time()
		{
			_timeSpan = TimeSpan.Zero;
		}

		public Time(TimeSpan timeSpan)
		{
			_timeSpan = timeSpan;
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
