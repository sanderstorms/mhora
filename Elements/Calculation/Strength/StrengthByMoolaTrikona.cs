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

// StrengthByMoolaTrikona rasi has more planets in moola trikona
// StrengthByMoolaTrikona planet is in moola trikona rasi
public static class MoolaTrikona
{
	public static int StrengthByMoolaTrikona(this Grahas grahas, Body m, Body n)
	{
		var valm = grahas.MoolaTrikonaValue(m);
		var valn = grahas.MoolaTrikonaValue(n);

		return valm.CompareTo(valn);
	}

	public static int StrengthByMoolaTrikona(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var vala = grahas.MoolaTrikonaValue(za);
		var valb = grahas.MoolaTrikonaValue(zb);

		return vala.CompareTo(valb);
	}

	public static int MoolaTrikonaValue(this Grahas grahas, ZodiacHouse zn)
	{
		var ret = 0;
		foreach (var graha in grahas.NavaGrahas)
		{
			if (graha.Rashi.ZodiacHouse != zn)
			{
				continue;
			}

			ret += grahas.MoolaTrikonaValue(graha);
		}

		return ret;
	}

	public static int MoolaTrikonaValue(this Grahas grahas, Body b)
	{
		if (grahas [b].IsMoolTrikona)
		{
			return 1;
		}

		return 0;
	}
}