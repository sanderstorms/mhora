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

		public static Time Zero => new Time(0);


		public Time AddDays(double days)
		{
			return new Time(_timeSpan + TimeSpan.FromDays(days));
		}

		public Time Add(double hours)
		{
			return new Time(_timeSpan.TotalHours + hours);
		}

		public Time Sub(double hours)
		{
			return new Time(_timeSpan.TotalHours - hours);
		}

		public Time Mul(double hours)
		{
			return new Time(_timeSpan.TotalHours * hours);
		}

		public Time Div(double hours)
		{
			return new Time(_timeSpan.TotalHours / hours);
		}

		// 1 Chatur Yuga = 4,320,000 sidereal years
		//				 = 1577917828 days
		// 1 Krita Yuga	 = 1,728,000 sidereal years
		// 1 Manu		 = 71 Chatur-Yugas + 1 Krita Yuga = 308,448,000 sidereal years
		// 1 Kalpa		 = 14 Manus + 1 Krita Yuga = 4,320,000,000 sidereal years
		// 
		//Kalpa = 30000 sampāta
		//Anonamalitic motion 387 revolutions per Kalpa
		public const  int  YugaYears           = 4320000;    //Revolutions sun around the earth
		public const  int  YugaDays            = 1577917828; //nr of sunrises
		public static Time SuryasiddhānticYear = TimeSpan.FromDays((double) YugaDays / YugaYears); //365.25875648148144
		public static Time SiderealYear    => TimeSpan.FromDays(365.256363004);
		public static Time TropicalYear    => TimeSpan.FromDays(365.24219);
		public static Time AnomalisticYear => TimeSpan.FromDays(365.259636);

		//365.2563583796296
		public static Time CalculatedYear
		{
			get
			{
				double years    = 4320000000;
				var    kalpa    = 30000;
				var    anonmaly = 387;
				var    period   = (years / (kalpa - anonmaly));
				var    factor   = (double) kalpa / (kalpa - anonmaly);
				
				var  diff       = (SuryasiddhānticYear.TotalDays / period);
				Time totalDays  = TimeSpan.FromDays(SuryasiddhānticYear.TotalDays - diff);

				var days       = new Time(diff);
				var correction = (days.TotalHours * factor);
				totalDays += correction;
				return totalDays;
			}
		}

		public bool IsZero => _timeSpan.TotalHours == 0;
		public int Years        => (int) (_timeSpan.TotalDays / 360);
		public int Days         => _timeSpan.Days % 360;
		public int Hours        => _timeSpan.Hours;
		public int Minutes      => _timeSpan.Minutes;
		public int Seconds      => _timeSpan.Seconds;
		public int MilliSeconds => _timeSpan.Milliseconds;

		public double TotalSeconds => _timeSpan.TotalSeconds;
		public double TotalMinutes => _timeSpan.TotalMinutes;
		public double TotalHours   => _timeSpan.TotalHours;
		public double TotalDays    => _timeSpan.TotalDays;

		public double TotalMonths   => TotalDays / 30;
		public double SolarYears    => TotalDays / 360;
		public double SiderealYears => TotalDays / SiderealYear.TotalDays;

		// Nimesh = Blink of an Eye or 34000th of a second
		// 15 Nimesh = 1 Kashtha
		// 30 Kashtha = 1 Kala
		// 30 Kala = 1 Kshana
		// 30 Kshana = 1 Vipal
		// 60 Vipal = 1 Pal
		// 60 Pal = 1 Ghati or Nazhika or 24 Minutes
		// 2.5 Ghati = 1 Hora or 1 Hour
		// 24 Hora or 60 Ghati = 1 Diva-Ratri or 1 day and night

		//1 day or 24 hours = 60 Ghatis
		public double Ghati => _timeSpan.TotalDays * 60;
		// 1 Ghati = 60 Vighati (also called Pala or Kala)
		public double Vighati => Ghati * 60;
		// 1 Vighati = 60 Linta or (also called Vipala or Vikala)
		public double Vipala => Vighati * 60;

		public override string ToString()
		{
			var    sign = Math.Sign(TotalHours) < 0 ? "-" : "";
			string str = string.Empty;
			if (Hours != 0)
			{
				str = _timeSpan.ToString("h'h 'm'm 's's'");
			}
			else if (Minutes != 0)
			{
				str = _timeSpan.ToString("m'm 's's'");
			}
			else if (Seconds != 0)
			{
				str = _timeSpan.ToString("s's'");
			}

			if (MilliSeconds != 0)
			{
				var ms = Math.Abs(MilliSeconds);
				str += $" {ms}ms";
			}

			if (Days != 0)
			{
				var days = Math.Abs(Days);
				str = $"{days}d " + str;
			}
			if (Years != 0)
			{
				var years = Math.Abs(Years);
				str = $"{years}y " + str;
			}
			return $"{sign}" + str;
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
