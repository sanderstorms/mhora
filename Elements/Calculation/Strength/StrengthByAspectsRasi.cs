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
// Stronger graha is in such a rasi
public class StrengthByAspectsRasi : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByAspectsRasi(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
	{
	}

	public bool stronger(Body.BodyType m, Body.BodyType n)
	{
		var zm = h.getPosition(m).toDivisionPosition(dtype).zodiac_house.Sign;
		var zn = h.getPosition(n).toDivisionPosition(dtype).zodiac_house.Sign;
		return stronger(zm, zn);
	}

	public bool stronger(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb)
	{
		var zj = h.getPosition(Body.BodyType.Jupiter).toDivisionPosition(dtype).zodiac_house;
		var zm = h.getPosition(Body.BodyType.Mercury).toDivisionPosition(dtype).zodiac_house;

		var a = value(zj, zm, za);
		var b = value(zj, zm, zb);
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

	protected int value(ZodiacHouse zj, ZodiacHouse zm, ZodiacHouse.Rasi zx)
	{
		var ret = 0;
		var zh  = new ZodiacHouse(zx);

		var bl = GetStrengthLord(zx);
		var zl = h.getPosition(bl).toDivisionPosition(dtype).zodiac_house;

		if (zj.RasiDristi(zh) || zj.Sign == zh.Sign)
		{
			ret++;
		}

		if (zm.RasiDristi(zh) || zm.Sign == zh.Sign)
		{
			ret++;
		}

		if (zl.RasiDristi(zh) || zl.Sign == zh.Sign)
		{
			ret++;
		}

		return ret;
	}
}