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

// Stronger rasi has its lord in a house of different oddity
// Stronger Graha in such a rasi
public class StrengthByLordInDifferentOddity : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByLordInDifferentOddity(Grahas grahas, bool bSimpleLord) : base(grahas, bSimpleLord)
	{
	}

	public int Stronger(Body ba, Body bb)
	{
		var za = _grahas [ba].Rashi;
		var zb = _grahas [bb].Rashi;
		return Stronger(za, zb);
	}

	public int Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var a = OddityValueForZodiacHouse(za);
		var b = OddityValueForZodiacHouse(zb);

		return a.CompareTo(b);
	}

	protected int OddityValueForZodiacHouse(ZodiacHouse zh)
	{
		var lname = GetStrengthLord(zh);
		var lbpos = _grahas[lname];

		//System.Mhora.Log.Debug("   DiffOddity {0} {1} {2}", zh.ToString(), zh_lor.value.ToString(), (int)zh %2==(int)zh_lor.value%2);
		if ((int) zh % 2 == (int) lbpos.Rashi.ZodiacHouse % 2)
		{
			return 0;
		}

		return 1;
	}
}