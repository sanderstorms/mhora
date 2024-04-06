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
using Mhora.Elements.Extensions;

namespace Mhora.Calculation.Strength;

// StrengthByKendraConjunction rasi has larger number of grahas in kendras
// StrengthByKendraConjunction Graha is in such a rasi
public static class KendraConjunction
{
	public static int StrengthByKendraConjunction(this Grahas grahas, Body m, Body n)
	{
		return grahas.StrengthByKendraConjunction(grahas [m].Rashi, grahas [n].Rashi);
	}

	public static int StrengthByKendraConjunction(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var numa = grahas.KendraConjunctionStrength(za);
		var numb = grahas.KendraConjunctionStrength(zb);

		return numa.CompareTo(numb);
	}

	public static int KendraConjunctionStrength(this Grahas grahas, ZodiacHouse zodiacHouse)
	{
		int numGrahas = 0;

		foreach (var graha in grahas)
		{
			if (graha.Bhava.IsKendra())
			{
				numGrahas += graha.Conjunct.Count;
			}
		}

		return numGrahas;
	}
}