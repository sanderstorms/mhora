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

// Stronger rasi has a Graha which has traversed larger longitude
// Stronger Graha has traversed larger longitude in its house
public class StrengthByLongitude : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByLongitude(Grahas grahas) : base(grahas, true)
	{
	}

	public int Stronger(Body m, Body n)
	{
		var lonm = KarakaLongitude(m);
		var lonn = KarakaLongitude(n);

		return lonn.CompareTo(lonn);
	}

	public int Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		Body[] karakaBodies =
		{
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Jupiter,
			Body.Venus,
			Body.Saturn,
			Body.Rahu
		};

		double lona = 0.0, lonb = 0.0;
		foreach (var bn in karakaBodies)
		{
			var offset = KarakaLongitude(bn);
			if (_grahas [bn].Rashi == za && offset > lona)
			{
				lona = offset;
			}
			else if (_grahas [bn].Rashi == zb && offset > lonb)
			{
				lonb = offset;
			}
		}

		return lona.CompareTo(lonb);
	}
}