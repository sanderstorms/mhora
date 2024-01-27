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

namespace Mhora.Elements.Calculation.Strength;

// Stronger rasi has more conjunctions/rasi drishtis of Jupiter, Mercury and Lord
// Stronger Graha is in such a rasi
public class StrengthByAspectsRasi : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByAspectsRasi(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
	{
	}

	public bool Stronger(Body.BodyType m, Body.BodyType n)
	{
		var zm = H.GetPosition(m).ToDivisionPosition(Dtype).ZodiacHouse;
		var zn = H.GetPosition(n).ToDivisionPosition(Dtype).ZodiacHouse;
		return Stronger(zm, zn);
	}

	public bool Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var zj = H.GetPosition(Body.BodyType.Jupiter).ToDivisionPosition(Dtype).ZodiacHouse;
		var zm = H.GetPosition(Body.BodyType.Mercury).ToDivisionPosition(Dtype).ZodiacHouse;

		var a = Value(zj, zm, za);
		var b = Value(zj, zm, zb);
		if (a > b)
		{
			return true;
		}

		if (a < b)
		{
			return false;
		}

		throw new EqualStrength();
	}

	protected int Value(ZodiacHouse zj, ZodiacHouse zm, ZodiacHouse zx)
	{
		var ret = 0;

		var bl = GetStrengthLord(zx);
		var zl = H.GetPosition(bl).ToDivisionPosition(Dtype).ZodiacHouse;

		if (zj.RasiDristi(zx) || zj == zx)
		{
			ret++;
		}

		if (zm.RasiDristi(zx) || zm == zx)
		{
			ret++;
		}

		if (zl.RasiDristi(zx) || zl == zx)
		{
			ret++;
		}

		return ret;
	}
}