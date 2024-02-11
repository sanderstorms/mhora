using System;

namespace Mhora.Util
{
	public class JulianDay
	{
		private readonly DateTime _dateTime;

		public JulianDay(double jd)
		{
			_dateTime = jd.ToUtc();
		}

		public JulianDay(DateTime dateTime)
		{
			_dateTime = dateTime;
		}

		public override string ToString()
		{
			return $"{_dateTime}";
		}

		public static implicit operator double(JulianDay jd)
		{
			return jd._dateTime.ToJulian();
		}

		public static implicit operator JulianDay(double jd)
		{
			return new JulianDay(jd);
		}

		public static implicit operator DateTime(JulianDay jd)
		{
			return jd._dateTime;
		}
	}
}
