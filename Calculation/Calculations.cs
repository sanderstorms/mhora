﻿using System;
using System.Diagnostics;
using Mhora.Definitions;
using Mhora.Elements;

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

		/// <summary>
		///     Normalize a number between bounds
		/// </summary>
		/// <param name="x">The value to be normalized</param>
		/// <param name="lower">The lower bound of normalization</param>
		/// <param name="upper">The upper bound of normalization</param>
		/// <returns>
		///     The normalized value of x, where lower = x <= upper </returns>
		public static double NormalizeExc(this double x, double lower, double upper)
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

		public static double NormalizeExcLower(this double x, double lower, double upper)
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

		public static double Normalize(this Longitude l)
		{
			return l.Value.NormalizeExcLower(0, 360);
		}

		public static bool IsBetween(this Longitude l, Longitude cuspLower, Longitude cuspHigher)
		{
			var diff1 = l.Sub(cuspLower).Value;
			var diff2 = l.Sub(cuspHigher).Value;

			var bRet = cuspHigher.Sub(cuspLower).Value <= 180 && diff1 <= diff2;

			Application.Log.Debug("Is it true that {0} < {1} < {2}? {3}", l, cuspLower, cuspHigher, bRet);
			return bRet;
		}

		public static Longitude Add(this Longitude l, Longitude b)
		{
			return new Longitude((l.Value + b.Value).NormalizeExcLower(0, 360));
		}

		public static Longitude Add(this Longitude l, double b)
		{
			return l.Add(new Longitude(b));
		}

		public static Longitude Sub(this Longitude l, Longitude b)
		{
			return new Longitude((l.Value - b.Value).NormalizeExcLower(0, 360));
		}

		public static Longitude Sub(this Longitude l, double b)
		{
			return l.Sub(new Longitude(b));
		}

		public static SunMoonYoga ToSunMoonYoga(this Longitude l)
		{
			var smIndex = (int) (Math.Floor(l.Value / (360.0 / 27.0)) + 1);
			var smYoga  = (SunMoonYoga) smIndex;
			return smYoga;
		}

		public static double ToSunMoonYogaBase(this Longitude l)
		{
			var num  = (int) l.ToSunMoonYoga();
			var cusp = (num - 1) * (360.0 / 27.0);
			return cusp;
		}

		public static double ToSunMoonYogaOffset(this Longitude l)
		{
			return l.Value - l.ToSunMoonYogaBase();
		}

		public static SunMoonYoga Add(this SunMoonYoga value, int i)
		{
			var snum = ((int) value + i - 1).NormalizeInc(1, 27);
			return (SunMoonYoga) snum;
		}

		public static SunMoonYoga AddReverse(this SunMoonYoga value, int i)
		{
			var snum = ((int) value - i + 1).NormalizeInc(1, 27);
			return (SunMoonYoga)snum;
		}

		public static Body Lord(this SunMoonYoga value)
		{
			switch ((int) value % 9)
			{
				case 1:  return Body.Saturn;
				case 2:  return Body.Mercury;
				case 3:  return Body.Ketu;
				case 4:  return Body.Venus;
				case 5:  return Body.Sun;
				case 6:  return Body.Moon;
				case 7:  return Body.Mars;
				case 8:  return Body.Rahu;
				default: return Body.Jupiter;
			}
		}

		public static bool CircularLonLessThan(this Longitude a, Longitude b)
		{
			return a.CircLonLessThan(b);
		}

		public static bool CircLonLessThan(this Longitude a, Longitude b)
		{
			var bounds = 40.0;

			if (a.Value > 360.0 - bounds && b.Value < bounds)
			{
				return true;
			}

			if (a.Value < bounds && b.Value > 360.0 - bounds)
			{
				return false;
			}

			return a.Value < b.Value;
		}
	}
}