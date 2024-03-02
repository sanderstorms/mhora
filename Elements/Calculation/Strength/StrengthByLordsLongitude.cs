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

// Stronger rasi's lord has traversed larger longitude
public static class LordsLongitude
{
	public static int StrengthByLordsLongitude(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord)
	{
		var lora = grahas.Horoscope.LordOfZodiacHouse(za, new Division(grahas.Varga), simpleLord);
		var lorb = grahas.Horoscope.LordOfZodiacHouse(zb, new Division(grahas.Varga), simpleLord);
		var offa = grahas [lora].HouseOffset;
		var offb = grahas [lorb].HouseOffset;

		return offa.CompareTo(offb);
	}
}