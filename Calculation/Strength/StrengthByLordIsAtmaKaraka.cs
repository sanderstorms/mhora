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

// StrengthByLordIsAtmaKaraka rasi's lord is AK
public static class LordIsAtmaKaraka
{
	public static int StrengthByLordIsAtmaKaraka(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord)
	{
		var lora = grahas.Horoscope.LordOfZodiacHouse(za, grahas.Varga, simpleLord);
		var lorb = grahas.Horoscope.LordOfZodiacHouse(zb, grahas.Varga, simpleLord);
		var ak   = grahas.Karaka8[0];
		if (lora == ak)
		{
			return 1;
		}

		if (lorb == ak)
		{
			return -1;
		}

		return 0;
	}
}