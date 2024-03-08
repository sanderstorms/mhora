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

namespace Mhora.Calculation.Strength;

// StrengthByLongitude rasi has a Graha which has traversed larger longitude
// StrengthByLongitude Graha has traversed larger longitude in its house
public static class Longitude
{
	public static int StrengthByLongitude(this Grahas grahas, Body m, Body n)
	{
		var lonm = grahas[m].HouseOffset;
		var lonn = grahas[n].HouseOffset;

		return lonn.CompareTo(lonn);
	}

	public static int StrengthByLongitude(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		double lona = 0.0, lonb = 0.0;
		foreach (var graha in grahas.NavaGrahas)
		{
			if (graha.Rashi == za && graha.HouseOffset > lona)
			{
				lona = graha.HouseOffset;
			}
			else if (graha.Rashi == zb && graha.HouseOffset > lonb)
			{
				lonb = graha.HouseOffset;
			}
		}

		return lona.CompareTo(lonb);
	}
}