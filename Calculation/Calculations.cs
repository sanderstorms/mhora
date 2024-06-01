using System;
using System.Diagnostics;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Calculation
{
	public static class Calculations
	{
		/// <summary>
		///     Normalize a number between bounds
		/// </summary>
		/// <param name="x">The value to be normalized</param>
		/// <param name="lower">The lower bound of normalization</param>
		/// <param name="upper">The upper bound of normalization</param>
		/// <returns>
		///     The normalized value of x, where lower <= x <= upper </returns>
		public static int NormalizeInc(this int x, int lower, int upper)
		{
			var size = upper - lower + 1;
			while (x > upper)
			{
				x -= size;
			}

			while (x < lower)
			{
				x += size;
			}

			Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
			return x;
		}


		public static double NormalizeExc(this double x, double lower, double upper) => (double) NormalizeExc((decimal) x,  (decimal) lower, (decimal) upper);

		/// <summary>
		///     Normalize a number between bounds
		/// </summary>
		/// <param name="x">The value to be normalized</param>
		/// <param name="lower">The lower bound of normalization</param>
		/// <param name="upper">The upper bound of normalization</param>
		/// <returns>
		///     The normalized value of x, where lower = x <= upper </returns>
		public static decimal NormalizeExc(this decimal x, decimal lower, decimal upper)
		{
			var size = upper - lower;
			while (x > upper)
			{
				x -= size;
			}

			while (x <= lower)
			{
				x += size;
			}

			Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
			return x;
		}

		public static decimal NormalizeExcLower(this decimal x, decimal lower, decimal upper)
		{
			var size = upper - lower;
			while (x >= upper)
			{
				x -= size;
			}

			while (x < lower)
			{
				x += size;
			}

			Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
			return x;
		}

		public static decimal Normalize(this Longitude l) => l.Value.NormalizeExcLower(0, 360);

		public static bool IsBetween(this Longitude l, Longitude cuspLower, Longitude cuspHigher)
		{
			var diff1 = l.Sub(cuspLower).Value;
			var diff2 = l.Sub(cuspHigher).Value;

			var bRet = cuspHigher.Sub(cuspLower).Value <= 180 && diff1 <= diff2;

			Application.Log.Debug("Is it true that {0} < {1} < {2}? {3}", l, cuspLower, cuspHigher, bRet);
			return bRet;
		}

		public static Longitude Add(this Longitude l, Longitude b) => new((l.Value + b.Value).NormalizeExcLower(0, 360));

		public static Longitude Add(this Longitude l, double b) => l.Add(new Longitude(b));

		public static Longitude Sub(this Longitude l, Longitude b) => new((l.Value - b.Value).NormalizeExcLower(0, 360));

		public static Longitude Sub(this Longitude l, double b) => l.Sub(new Longitude(b));

		public static SunMoonYoga ToSunMoonYoga(this Longitude l)
		{
			var smIndex = (int) (l.Value / (360M / 27)).Floor () + 1;
			var smYoga  = (SunMoonYoga) smIndex;
			return smYoga;
		}

		public static decimal ToSunMoonYogaBase(this Longitude l)
		{
			var num  = (int) l.ToSunMoonYoga();
			var cusp = (num - 1) * (360M / 27);
			return cusp;
		}

		public static decimal ToSunMoonYogaOffset(this Longitude l) => l.Value - l.ToSunMoonYogaBase();

		public static SunMoonYoga Add(this SunMoonYoga value, int i)
		{
			var snum = (value.Index() + i - 1).NormalizeInc(1, 27);
			return (SunMoonYoga) snum;
		}

		public static SunMoonYoga AddReverse(this SunMoonYoga value, int i)
		{
			var snum = (value.Index() - i + 1).NormalizeInc(1, 27);
			return (SunMoonYoga)snum;
		}

		public static Body Lord(this SunMoonYoga value)
		{
			return ((int) value % 9) switch
			       {
				       1 => Body.Saturn,
				       2 => Body.Mercury,
				       3 => Body.Ketu,
				       4 => Body.Venus,
				       5 => Body.Sun,
				       6 => Body.Moon,
				       7 => Body.Mars,
				       8 => Body.Rahu,
				       _ => Body.Jupiter
			       };
		}

		public static bool CircularLonLessThan(this Longitude a, Longitude b) => a.CircLonLessThan(b);

		public static bool CircLonLessThan(this Longitude a, Longitude b)
		{
			var bounds = 40;

			if (a.Value > 360 - bounds && b.Value < bounds)
			{
				return true;
			}

			if (a.Value < bounds && b.Value > 360 - bounds)
			{
				return false;
			}

			return a.Value < b.Value;
		}
	}
}
