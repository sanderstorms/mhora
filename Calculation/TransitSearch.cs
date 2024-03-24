using System;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Calculation
{
	public static class TransitSearch
	{
		public static double LinearSearch(this double approxUt, Longitude lonToFind, Func<JulianDate, Ref<bool>, Longitude> func)
		{
			var dayStart = LinearSearchApprox(approxUt, lonToFind, func);
			var dayFound = LinearSearchBinary(dayStart, dayStart + 1.0, lonToFind, func);
			return dayFound;
		}

		public static double LinearSearchBinary(this double utStart, double utEnd, Longitude lonToFind, Func<JulianDate, Ref<bool>, Longitude> func)
		{
			Ref<bool> bDiscard = new(true);
			if (Math.Abs(utEnd - utStart) < 1.0 / (24.0 * 60.0 * 60.0 * 60.0))
			{
				var l = func(utStart, bDiscard);
				if (l.CircLonLessThan(lonToFind))
				{
					return utEnd;
				}

				return utStart;
			}

			var utMiddle = (utStart + utEnd) / 2.0;
			var lon      = func(utMiddle, bDiscard);
			if (lon.CircularLonLessThan(lonToFind))
			{
				return LinearSearchBinary(utMiddle, utEnd, lonToFind, func);
			}

			return LinearSearchBinary(utStart, utMiddle, lonToFind, func);
		}

		public static double LinearSearchApprox(this double approxUt, Longitude lonToFind, Func<JulianDate, Ref<bool>, Longitude> func)
		{
			Ref<bool> bDiscard = new(true);
			var       ut       = approxUt.Floor();
			var       lon      = func(ut, bDiscard);

			if (lon.CircularLonLessThan(lonToFind))
			{
				while (lon.CircularLonLessThan(lonToFind))
				{
					ut  += 1.0;
					lon =  func(ut, bDiscard);
				}

				ut -= 1.0;
			}
			else
			{
				while (!lon.CircularLonLessThan(lonToFind))
				{
					ut  -= 1.0;
					lon =  func(ut, bDiscard);
				}
			}

			var l = func(ut, bDiscard);
			return ut;
		}

	}
}
