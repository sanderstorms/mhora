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

// StrengthByRasisNature rasi by nature (moveable, fixed, dual)
// StrengthByRasisNature Graha in such a rasi
public static class RasisNature
{
	public static int StrengthByRasisNature(this Grahas grahas, Body m, Body n)
	{
		var za = grahas [m].Rashi;
		var zb = grahas [n].Rashi;
		return grahas.StrengthByRasisNature(za, zb);
	}

	public static int StrengthByRasisNature(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		int[] vals =
		{
			3,
			1,
			2
		}; // dual, move, fix
		var a = NaturalValueForRasi(za);
		var b = NaturalValueForRasi(zb);

		return a.CompareTo(b);
	}

	public static int NaturalValueForRasi(this ZodiacHouse zha)
	{
		int[] vals =
		{
			3,
			1,
			2
		}; // dual, move, fix
		return vals[(int) zha % 3];
	}
}