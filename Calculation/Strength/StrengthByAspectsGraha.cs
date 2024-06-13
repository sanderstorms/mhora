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

namespace Mhora.Calculation.Strength;

// StrengthByAspectsGraha rasi has more Graha drishtis of Jupiter, Mercury and Lord
// StrengthByAspectsGraha Graha is in such a rasi
public static class AspectsGraha
{
	public static int StrengthByAspectsGraha(this Grahas grahas, Body m, Body n, bool simpleLord)
	{
		var a = grahas.AspectsGrahaStrength(m, simpleLord);
		var b = grahas.AspectsGrahaStrength(n, simpleLord);

		return a.CompareTo(b);
	}

	public static int StrengthByAspectsGraha(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord)
	{
		var a = grahas.AspectsGrahaStrength(za, simpleLord);
		var b = grahas.AspectsGrahaStrength(zb, simpleLord);

		return a.CompareTo(b);
	}

	private static int AspectsGrahaStrength(this Grahas grahas, ZodiacHouse zodiacHouse, bool simpleLord)
	{
		var val = 0;
		var bl  = grahas.Horoscope.LordOfZodiacHouse(zodiacHouse, grahas.Varga, simpleLord);
		var dl  = grahas [bl];
		var dj  = grahas [Body.Jupiter];
		var dm  = grahas [Body.Mercury];

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

	private static int AspectsGrahaStrength(this Grahas grahas, Body bm, bool simpleLord) => grahas.AspectsGrahaStrength(grahas[bm].Rashi, simpleLord);
}