/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Calculation.Strength;

// StrengthByAspectsRasi rasi has more conjunctions/rasi drishtis of Jupiter, Mercury and Lord
// StrengthByAspectsRasi Graha is in such a rasi
public static class AspectsRasi
{
	public static int StrengthByAspectsRasi(this Grahas grahas, Body m, Body n, bool simpleLord)
	{
		var zm = grahas [m].Rashi;
		var zn = grahas [n].Rashi;
		return StrengthByAspectsRasi(grahas, zm, zn, simpleLord);
	}

	public static int StrengthByAspectsRasi(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord)
	{
		var zj = grahas [Body.Jupiter].Rashi;
		var zm = grahas [Body.Mercury].Rashi;

		var a = grahas.Value(zj, zm, za, simpleLord);
		var b = grahas.Value(zj, zm, zb, simpleLord);

		return a.CompareTo(b);
	}

	private static int Value (this Grahas grahas, ZodiacHouse zj, ZodiacHouse zm, ZodiacHouse zx, bool simpleLord)
	{
		var ret = 0;

		var bl = grahas.Horoscope.LordOfZodiacHouse(zx, grahas.Varga, simpleLord);
		var zl = grahas [bl].Rashi;

		if (zj.RasiDristi(zx) || zj == zx)
		{
			ret++;
		}

		if (zm.RasiDristi(zx) || zm == zx)
		{
			ret++;
		}

		if (zl.HasDirshtiOn(zx) || zl == zx)
		{
			ret++;
		}

		return ret;
	}
}