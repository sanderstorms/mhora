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

// StrengthByLordInOwnHouse rasi has its Lord in its house
// StrengthByLordInOwnHouse Graha is in its own house
public static class LordInOwnHouse
{
	public static int StrengthByLordInOwnHouse(this Grahas grahas, Body m, Body n, bool simpleLord)
	{
		var zm = grahas [m].Rashi;
		var zn = grahas [n].Rashi;

		return grahas.StrengthByLordInOwnHouse(zm, zn, simpleLord);
	}

	public static int StrengthByLordInOwnHouse(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord)
	{
		var a = grahas.Value(za, simpleLord);
		var b = grahas.Value(zb, simpleLord);

		return a.CompareTo(b);
	}

	private static int Value(this Grahas grahas, ZodiacHouse zodiacHouse, bool simpleLord)
	{
		var ret = 0;

		var zh = (zodiacHouse);
		var bl = grahas.Horoscope.LordOfZodiacHouse(zh, grahas.Varga, simpleLord);
		var pl = grahas [bl];
		var pj = grahas [Body.Jupiter];
		var pm = grahas [Body.Mercury];

		if (pl.HasDrishtiOn(zh))
		{
			ret++;
		}

		if (pj.HasDrishtiOn(zh))
		{
			ret++;
		}

		if (pm.HasDrishtiOn(zh))
		{
			ret++;
		}

		return ret;
	}
}