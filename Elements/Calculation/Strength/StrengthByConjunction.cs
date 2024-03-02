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

// StrengthByConjunction rasi has larger number of grahas
// StrengthByConjunction Graha is in such a rasi
public static class Conjunction
{
	public static int StrengthByConjunction(this Grahas grahas, Body m, Body n)
	{
		var gm = grahas.Find(m);
		var gn = grahas.Find(n);

		return gm.Conjunct.Count.CompareTo(gn.Conjunct.Count);
	}

	public static int StrengthByConjunction(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var rashi1 = grahas.Rashis.Find(za);
		var rashi2 = grahas.Rashis.Find(zb);

		return rashi1.Grahas.Count.CompareTo(rashi2.Grahas.Count);
	}
}