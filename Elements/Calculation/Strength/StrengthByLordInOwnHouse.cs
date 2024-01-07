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

// Stronger rasi has its Lord in its house
// Stronger graha is in its own house
public class StrengthByLordInOwnHouse : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByLordInOwnHouse(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
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
		var a = value(za);
		var b = value(zb);
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

	protected int value(ZodiacHouse.Rasi _zh)
	{
		var ret = 0;

		var zh = new ZodiacHouse(_zh);
		var bl = GetStrengthLord(zh);
		var pl = h.getPosition(bl).toDivisionPosition(dtype);
		var pj = h.getPosition(Body.BodyType.Jupiter).toDivisionPosition(dtype);
		var pm = h.getPosition(Body.BodyType.Mercury).toDivisionPosition(dtype);

		if (pl.GrahaDristi(zh))
		{
			ret++;
		}

		if (pj.GrahaDristi(zh))
		{
			ret++;
		}

		if (pm.GrahaDristi(zh))
		{
			ret++;
		}

		return ret;
	}
}