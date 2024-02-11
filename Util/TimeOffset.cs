
using System;

namespace Mhora.Util
{
	public class TimeOffset
	{
		private readonly int      _years;
		private readonly TimeSpan _remainder;

	   public TimeOffset()
	   {
		   _years = 0;
		   _remainder = new Time();
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

	   //public static implicit operator double(TimeOffset offset) => offset.TotalYears;

	   public static implicit operator TimeOffset (double totalYears) => new TimeOffset (totalYears);

	   public static TimeOffset Zero => new TimeOffset(0);

	   public double TotalYears
	   {
		   get
		   {
			   var result = (double) _years;
			   result += (_remainder.TotalDays / TimeUtils.SiderealYear.TotalDays);
			   return (result);
		   }
	   }

	   public int      Years     => _years;
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

	   public static TimeOffset operator +(TimeOffset start, double years)  => start.Add(new TimeOffset(years));
	   public static TimeOffset operator -(TimeOffset start, double years)  => start.Sub(new TimeOffset(years));
	   public static TimeOffset operator /(TimeOffset start, double factor) => start.Div(factor);
	   public static TimeOffset operator *(TimeOffset start, double factor) => start.Mul(factor);

	   public static TimeOffset operator +(TimeOffset start, TimeOffset offset) => start.Add(offset);
	   public static TimeOffset operator -(TimeOffset start, TimeOffset offset) => start.Sub(offset);
	   public static TimeOffset operator /(TimeOffset start, TimeOffset offset) => start.Div(offset.TotalYears);
	   public static TimeOffset operator *(TimeOffset start, TimeOffset offset) => start.Mul(offset.TotalYears);
	}
}
