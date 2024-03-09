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

// StrengthByAtmaKaraka rasi contains AK
// StrengthByAtmaKaraka Graha is AK
public static class AtmaKaraka
{
	public static int StrengthByAtmaKaraka(this Grahas grahas, Body m, Body n)
	{
		var ak = grahas.Karaka8[0];
		if (m == ak)
		{
			return 1;
		}

		if (n == ak)
		{
			return -1;
		}

		return (0);
	}

	public static int StrengthByAtmaKaraka(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var ala = grahas.Rashis[za].Grahas;
		var alb = grahas.Rashis[zb].Grahas;
		var ak  = grahas.Karaka8[0];
		foreach (Body ba in ala)
		{
			if (ba == ak)
			{
				return 1;
			}
		}

		foreach (Body bb in alb)
		{
			if (bb == ak)
			{
				return -1;
			}
		}

		return 0;
	}
}