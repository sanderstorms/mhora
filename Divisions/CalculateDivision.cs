using System.Diagnostics;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Divisions
{
	public static class CalculateDivision
	{
		public static int PartOfZodiacHouse(this Longitude longitude, int n)
		{
			var offset = longitude.ToZodiacHouseOffset();
			var part   = (int) (offset / (30.0 / n)).Floor() + 1;
			Trace.Assert(part >= 1 && part <= n);
			return part;
		}
	}
}
