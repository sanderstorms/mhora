
using System;
using Newtonsoft.Json;

namespace Mhora.Util
{
	[JsonObject]
	public class TimeOffset : IEquatable<TimeOffset>
	{
		private readonly int      _years;
		private readonly TimeSpan _remainder;

	   public TimeOffset()
	   {
		   _years = 0;
		   _remainder = new Time();
	   }

	   public TimeOffset (DateTime dateTime)
	   {
		   _years     = dateTime.Year;
		   _remainder = dateTime.StartNextYear() - dateTime;
	   }

	   public TimeOffset (double years)
	   {
			_years     = (int) Math.Truncate (years);
			_remainder = TimeUtils.RemainingDays (years); 
	   }

	   public TimeOffset(TimeOffset offset)
	   {
		   _years     = offset._years;
		   _remainder = offset._remainder;
	   }

	   public TimeOffset(int years, TimeSpan remainder)
	   {
		   _years     = Years;
		   _remainder = remainder;
	   }

	   public TimeOffset Clone()
	   {
		   return (new TimeOffset(this));
	   }

	   public override string ToString()
	   {
		   return $"{_years}.{_remainder}";
	   }

	   //public static implicit operator double(TimeOffset offset) => offset.TotalYears;

	   public static implicit operator TimeOffset (double totalYears) => new TimeOffset (totalYears);
	   public static implicit operator TimeOffset (DateTime dateTime) => new TimeOffset (dateTime);


	   public static TimeOffset Zero => new TimeOffset(0);

	   [JsonIgnore]
	   public double TotalYears
	   {
		   get
		   {
			   var result = (double) _years;
			   result += (_remainder.TotalDays / TimeUtils.SiderealYear.TotalDays);
			   return (result);
		   }
	   }

	   [JsonProperty]
	   public int      Years     => _years;
	   [JsonProperty]
	   public TimeSpan Remainder => _remainder;

	   public TimeOffset Add(TimeOffset offset)
	   {
		   var timeOffset = offset.TotalYears + TotalYears;
		   return (new TimeOffset(timeOffset));
	   }

	   public TimeOffset Sub(TimeOffset offset)
	   {
		   var timeOffset = TotalYears - offset.TotalYears;
		   return (new TimeOffset(timeOffset));
	   }

	   public TimeOffset Div(double factor)
	   {
		   var timeOffset = TotalYears / factor;
		   return (new TimeOffset(timeOffset));
	   }

	   public TimeOffset Mul(double factor)
	   {
		   var timeOffset = TotalYears * factor;
		   return (new TimeOffset(timeOffset));
	   }

	   public static TimeOffset operator +(TimeOffset start,  double years)  => start.Add(new TimeOffset(years));
	   public static TimeOffset operator -(TimeOffset start,  double years)  => start.Sub(new TimeOffset(years));
	   public static TimeOffset operator /(TimeOffset start,  double factor) => start.Div(factor);
	   public static TimeOffset operator *(TimeOffset start,  double factor) => start.Mul(factor);
	   public static bool operator < (TimeOffset      offset, double value)  => offset.TotalYears < value; 
	   public static bool operator > (TimeOffset      offset, double value)  => offset.TotalYears > value; 
	   public static bool operator == (TimeOffset     offset, double value)  => offset.TotalYears == value; 
	   public static bool operator != (TimeOffset     offset, double value)  => offset.TotalYears != value; 

	   public static TimeOffset operator +(TimeOffset start,  TimeOffset offset) => start.Add(offset);
	   public static TimeOffset operator -(TimeOffset start,  TimeOffset offset) => start.Sub(offset);
	   public static TimeOffset operator /(TimeOffset start,  TimeOffset offset) => start.Div(offset.TotalYears);
	   public static TimeOffset operator *(TimeOffset start,  TimeOffset offset) => start.Mul(offset.TotalYears);
	   public static bool operator < (TimeOffset      offset, TimeOffset other)  => offset.TotalYears < other.TotalYears; 
	   public static bool operator > (TimeOffset      offset, TimeOffset other)  => offset.TotalYears > other.TotalYears; 

	   public bool Equals(TimeOffset other)
	   {
		   if (ReferenceEquals(null, other))
		   {
			   return false;
		   }

		   if (ReferenceEquals(this, other))
		   {
			   return true;
		   }

		   return _years == other._years && _remainder.Equals(other._remainder);
	   }

	   public override bool Equals(object obj)
	   {
		   if (ReferenceEquals(null, obj))
		   {
			   return false;
		   }

		   if (ReferenceEquals(this, obj))
		   {
			   return true;
		   }

		   if (obj.GetType() != this.GetType())
		   {
			   return false;
		   }

		   return Equals((TimeOffset) obj);
	   }

	   public override int GetHashCode()
	   {
		   unchecked
		   {
			   return (_years * 397) ^ _remainder.GetHashCode();
		   }
	   }
	}
}
