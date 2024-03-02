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

// StrengthByExaltation rasi has larger number of exalted planets - debilitated planets
// StrengthByExaltation planet is exalted or not debilitated
public static class Exaltation
{
	public static int StrengthByExaltation(this Grahas grahas, Body m, Body n)
	{
		var valm = grahas.ExaltationStrength(m);
		var valn = grahas.ExaltationStrength(n);

		return valm.CompareTo(valn);
	}

	public static int StrengthByExaltation(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var vala = grahas.ExaltationStrength(za);
		var valb = grahas.ExaltationStrength(zb);

		return vala.CompareTo(valb);
	}

	public static int ExaltationStrength(this Grahas grahas, ZodiacHouse zn)
	{
		var ret = 0;
		foreach (var graha in grahas.NavaGrahas)
		{
			if (graha.Rashi != zn)
			{
				continue;
			}

			if (graha.IsExalted)
			{
				ret++;
			}
			else if (graha.IsDebilitated)
			{
				ret--;
			}
		}

		return ret;
	}

	public static int ExaltationStrength(this Grahas grahas, Body b)
	{
		if (grahas [b].IsExalted)
		{
			return 1;
		}

		if (grahas [b].IsDebilitated)
		{
			return -1;
		}

		return 0;
	}
}