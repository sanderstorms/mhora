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

// Stronger rasi has more Graha drishtis of Jupiter, Mercury and Lord
// Stronger Graha is in such a rasi
public class StrengthByAspectsGraha : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByAspectsGraha(Grahas grahas, bool bSimpleLord) : base(grahas, bSimpleLord)
	{
	}

	public int Stronger(Body m, Body n)
	{
		var a = Value(m);
		var b = Value(n);

		return a.CompareTo(b);
	}

	public int Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var a = Value(za);
		var b = Value(zb);

		return a.CompareTo(b);
	}

	protected int Value(ZodiacHouse zodiacHouse)
	{
		var val = 0;
		var bl  = GetStrengthLord(zodiacHouse);
		var dl  = _grahas [bl];
		var dj  = _grahas [Body.Jupiter];
		var dm  = _grahas [Body.Mercury];

		var zh = (zodiacHouse);
		if (dl.HasDrishtiOn(zh) || dl.Rashi == zodiacHouse)
		{
			val++;
		}

		if (dj.HasDrishtiOn(zh) || dj.Rashi == zodiacHouse)
		{
			val++;
		}

		if (dm.HasDrishtiOn(zh) || dm.Rashi == zodiacHouse)
		{
			val++;
		}

		return val;
	}

	protected int Value(Body bm)
	{
		return Value(_grahas.Find(bm).Rashi);
	}
}