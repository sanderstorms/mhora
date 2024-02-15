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
			var smYoga  = new SunMoonYoga((SunMoonYoga.Name) smIndex);
			return smYoga;
		}

		public static double ToSunMoonYogaBase(this Longitude l)
		{
			var num  = (int) l.ToSunMoonYoga().value;
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

		public static Nakshatra ToNakshatra(this Longitude l)
		{
			var snum = (int) (Math.Floor(l.Value / (360.0 / 27.0)) + 1.0);
			return (Nakshatra) snum;
		}

		public static double ToNakshatraBase(this Longitude l)
		{
			var num  = l.ToNakshatra().Index();
			var cusp = (num - 1) * (360.0 / 27.0);
			return cusp;
		}

		public static Nakshatra28 ToNakshatra28(this Longitude l)
		{
			var snum = (int) (Math.Floor(l.Value / (360.0 / 27.0)) + 1.0);

			var ret = (Nakshatra28) snum;
			if (snum >= (int) Nakshatra28.Abhijit)
			{
				ret = ret.Add(2);
			}

			if (l.Value >= 270 + (6.0 + 40.0 / 60.0) && l.Value <= 270 + (10.0 + 53.0 / 60.0 + 20.0 / 3600.0))
			{
				ret = Nakshatra28.Abhijit;
			}

			return ret;
		}

		public static double ToNakshatraOffset(this Longitude l)
		{
			var znum = l.ToNakshatra().Index();
			var cusp = (znum - 1) * (360.0 / 27.0);
			var ret  = l.Value - cusp;
			Trace.Assert(ret >= 0.0 && ret <= 360.0 / 27.0);
			return ret;
		}

		public static double PercentageOfNakshatra(this Longitude l)
		{
			var offset = l.ToNakshatraOffset();
			var perc   = offset / (360.0 / 27.0) * 100;
			Trace.Assert(perc >= 0 && perc <= 100);
			return perc;
		}

		public static int ToNakshatraPada(this Longitude l)
		{
			var offset = l.ToNakshatraOffset();
			var val    = (int) Math.Floor(offset / (360.0 / (27.0 * 4.0))) + 1;
			Trace.Assert(val >= 1 && val <= 4);
			return val;
		}

		public static int ToAbsoluteNakshatraPada(this Longitude l)
		{
			var n = l.ToNakshatra().Index();
			var p = l.ToNakshatraPada();
			return (n - 1) * 4 + p;
		}

		public static double ToNakshatraPadaOffset(this Longitude l)
		{
			var pnum = l.ToAbsoluteNakshatraPada();
			var cusp = (pnum - 1) * (360.0 / (27.0 * 4.0));
			var ret  = l.Value - cusp;
			Trace.Assert(ret >= 0.0 && ret <= 360.0 / 27.0);
			return ret;
		}

		public static double ToNakshatraPadaPercentage(this Longitude l)
		{
			var offset = l.ToNakshatraPadaOffset();
			var perc   = offset / (360.0 / (27.0 * 4.0)) * 100;
			Trace.Assert(perc >= 0 && perc <= 100);
			return perc;
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
