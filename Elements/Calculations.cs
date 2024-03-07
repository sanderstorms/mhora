using System;
using System.Diagnostics;
using Mhora.Definitions;
using Mhora.Elements.Calculation;
using Mhora.Util;

namespace Mhora.Elements
{
	public static class Calculations
	{
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

		public static Tithi ToTithi(this Longitude l)
		{
			var tIndex = (int) (Math.Floor(l.Value / (360.0 / 30.0)) + 1);
			var t      = tIndex.ToTithi();
			return t;
		}

		public static double ToTithiBase(this Longitude l)
		{
			var num  = l.ToTithi().Index();
			var cusp = (num - 1) * (360.0 / 30.0);
			return cusp;
		}

		public static double ToTithiOffset(this Longitude l)
		{
			return l.Value - l.ToTithiBase();
		}


		public static Karana ToKarana(this Longitude l)
		{
			var kIndex = (int) (Math.Floor(l.Value / (360.0 / 60.0)) + 1);
			var k      = (Karana) kIndex;
			return k;
		}

		public static double ToKaranaBase(this Longitude l)
		{
			var num  = (int) l.ToKarana();
			var cusp = (num - 1) * (360.0 / 60.0);
			return cusp;
		}

		public static double ToKaranaOffset(this Longitude l)
		{
			return l.Value - l.ToKaranaBase();
		}

		public static ZodiacHouse ToZodiacHouse(this Longitude l)
		{
			var znum = (int) (Math.Floor(l.Value / 30.0) + 1.0);
			return (ZodiacHouse) znum;
		}

		public static double ToZodiacHouseBase(this Longitude l)
		{
			var znum = l.ToZodiacHouse().Index ();
			var cusp = (znum - 1) * 30.0;
			return cusp;
		}

		public static double ToZodiacHouseOffset(this Longitude l)
		{
			var znum = l.ToZodiacHouse().Index ();
			var cusp = (znum - 1) * 30.0;
			var ret  = l.Value - cusp;
			Trace.Assert(ret >= 0.0 && ret <= 30.0);
			return ret;
		}

		public static double PercentageOfZodiacHouse(this Longitude l)
		{
			var offset = l.ToZodiacHouseOffset();
			var perc   = offset / 30.0 * 100;
			Trace.Assert(perc >= 0 && perc <= 100);
			return perc;
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
