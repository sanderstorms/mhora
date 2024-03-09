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
using Mhora.Yoga;

namespace Mhora.Calculation.Strength;

// StrengthByOwnHouse rasi has more planets in own house
// StrengthByOwnHouse planet is in own house
public static class OwnHouse
{
	public static int StrengthByOwnHouse(this Grahas grahas, Body m, Body n)
	{
		var valm = grahas.OwnHouseValue(m);
		var valn = grahas.OwnHouseValue(n);

		return valm.CompareTo(valn);
	}

	public static int StrengthByOwnHouse(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var vala = grahas.OwnHouseValue(za);
		var valb = grahas.OwnHouseValue(zb);

		return vala.CompareTo(valb);
	}

	public static int OwnHouseValue(this Grahas grahas, ZodiacHouse zn)
	{
		var ret = 0;
		foreach (var graha in grahas.NavaGrahas)
		{
			if (graha.Rashi.ZodiacHouse != zn)
			{
				continue;
			}

			ret += grahas.OwnHouseValue(graha);
		}

		return ret;
	}

	public static int OwnHouseValue(this Grahas grahas, Body b)
	{
		if (grahas [b].IsInOwnHouse)
		{
			return 1;
		}

		return 0;
	}
}