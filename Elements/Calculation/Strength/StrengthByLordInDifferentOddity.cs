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

// StrengthByLordInDifferentOddity rasi has its lord in a house of different oddity
// StrengthByLordInDifferentOddity Graha in such a rasi
public static class LordInDifferentOddity
{
	public static int StrengthByLordInDifferentOddity(this Grahas grahas, Body ba, Body bb, bool simpleLord)
	{
		var za = grahas [ba].Rashi;
		var zb = grahas [bb].Rashi;
		return grahas.StrengthByLordInDifferentOddity(za, zb, simpleLord);
	}

	public static int StrengthByLordInDifferentOddity(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord)
	{
		var a = grahas.OddityValueForZodiacHouse(za, simpleLord);
		var b = grahas.OddityValueForZodiacHouse(zb, simpleLord);

		return a.CompareTo(b);
	}

	private static int OddityValueForZodiacHouse(this Grahas grahas, ZodiacHouse zh, bool simpleLord)
	{
		var lname = grahas.Horoscope.LordOfZodiacHouse(zh, new Division(grahas.Varga), simpleLord);
		var lbpos = grahas[lname];

		//System.Mhora.Log.Debug("   DiffOddity {0} {1} {2}", zh.ToString(), zh_lor.value.ToString(), (int)zh %2==(int)zh_lor.value%2);
		if ((int) zh % 2 == (int) lbpos.Rashi.ZodiacHouse % 2)
		{
			return 0;
		}

		return 1;
	}
}