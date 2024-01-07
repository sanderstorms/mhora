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

// Stronger rasi has more graha drishtis of Jupiter, Mercury and Lord
// Stronger graha is in such a rasi
public class StrengthByAspectsGraha : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByAspectsGraha(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
	{
	}

	public bool stronger(Body.BodyType m, Body.BodyType n)
	{
		var a = value(m);
		var b = value(n);
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
		var val = 0;
		var bl  = GetStrengthLord(_zh);
		var dl  = h.getPosition(bl).toDivisionPosition(dtype);
		var dj  = h.getPosition(Body.BodyType.Jupiter).toDivisionPosition(dtype);
		var dm  = h.getPosition(Body.BodyType.Mercury).toDivisionPosition(dtype);

		var zh = new ZodiacHouse(_zh);
		if (dl.GrahaDristi(zh) || dl.zodiac_house.Sign == _zh)
		{
			val++;
		}

		if (dj.GrahaDristi(zh) || dj.zodiac_house.Sign == _zh)
		{
			val++;
		}

		if (dm.GrahaDristi(zh) || dm.zodiac_house.Sign == _zh)
		{
			val++;
		}

		return val;
	}

	protected int value(Body.BodyType bm)
	{
		return value(h.getPosition(bm).toDivisionPosition(dtype).zodiac_house.Sign);
	}
}