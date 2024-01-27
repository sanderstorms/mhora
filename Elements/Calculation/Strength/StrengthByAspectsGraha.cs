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

// Stronger rasi has more Graha drishtis of Jupiter, Mercury and Lord
// Stronger Graha is in such a rasi
public class StrengthByAspectsGraha : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByAspectsGraha(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
	{
	}

	public bool Stronger(Body.BodyType m, Body.BodyType n)
	{
		var a = Value(m);
		var b = Value(n);
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

	public bool Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var a = Value(za);
		var b = Value(zb);
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

	protected int Value(ZodiacHouse zodiacHouse)
	{
		var val = 0;
		var bl  = GetStrengthLord(zodiacHouse);
		var dl  = H.GetPosition(bl).ToDivisionPosition(Dtype);
		var dj  = H.GetPosition(Body.BodyType.Jupiter).ToDivisionPosition(Dtype);
		var dm  = H.GetPosition(Body.BodyType.Mercury).ToDivisionPosition(Dtype);

		var zh = (zodiacHouse);
		if (dl.GrahaDristi(zh) || dl.ZodiacHouse == zodiacHouse)
		{
			val++;
		}

		if (dj.GrahaDristi(zh) || dj.ZodiacHouse == zodiacHouse)
		{
			val++;
		}

		if (dm.GrahaDristi(zh) || dm.ZodiacHouse == zodiacHouse)
		{
			val++;
		}

		return val;
	}

	protected int Value(Body.BodyType bm)
	{
		return Value(H.GetPosition(bm).ToDivisionPosition(Dtype).ZodiacHouse);
	}
}