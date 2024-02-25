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
using Mhora.Elements.Yoga;

namespace Mhora.Elements.Calculation.Strength;

// Stronger rasi has more conjunctions/rasi drishtis of Jupiter, Mercury and Lord
// Stronger Graha is in such a rasi
public class StrengthByAspectsRasi : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByAspectsRasi(Grahas grahas, bool bSimpleLord) : base(grahas, bSimpleLord)
	{
	}

	public int Stronger(Body m, Body n)
	{
		var zm = _grahas [m].Rashi;
		var zn = _grahas [n].Rashi;
		return Stronger(zm, zn);
	}

	public int Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var zj = _grahas [Body.Jupiter].Rashi;
		var zm = _grahas [Body.Mercury].Rashi;

		var a = Value(zj, zm, za);
		var b = Value(zj, zm, zb);

		return a.CompareTo(b);
	}

	protected int Value(ZodiacHouse zj, ZodiacHouse zm, ZodiacHouse zx)
	{
		var ret = 0;

		var bl = GetStrengthLord(zx);
		var zl = _grahas [bl].Rashi;

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