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

using Mhora.Tables;

namespace Mhora.Elements.Calculation.Strength;

// Stronger rasi has a graha which has traversed larger longitude
// Stronger graha has traversed larger longitude in its house
public class StrengthByLongitude : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByLongitude(Horoscope h, Division dtype) : base(h, dtype, true)
	{
	}

	public bool stronger(Body.BodyType m, Body.BodyType n)
	{
		var lonm = karakaLongitude(m);
		var lonn = karakaLongitude(n);
		if (lonm > lonn)
		{
			return true;
		}

		if (lonn > lonm)
		{
			return false;
		}

		throw new EqualStrength();
	}

	public bool stronger(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb)
	{
		Body.BodyType[] karakaBodies =
		{
			Body.BodyType.Sun,
			Body.BodyType.Moon,
			Body.BodyType.Mars,
			Body.BodyType.Mercury,
			Body.BodyType.Jupiter,
			Body.BodyType.Venus,
			Body.BodyType.Saturn,
			Body.BodyType.Rahu
		};

		double lona = 0.0, lonb = 0.0;
		foreach (var bn in karakaBodies)
		{
			var div    = h.getPosition(bn).toDivisionPosition(new Division(Vargas.DivisionType.Rasi));
			var offset = karakaLongitude(bn);
			if (div.zodiac_house.Sign == za && offset > lona)
			{
				lona = offset;
			}
			else if (div.zodiac_house.Sign == zb && offset > lonb)
			{
				lonb = offset;
			}
		}

		if (lona > lonb)
		{
			return true;
		}

		if (lonb > lona)
		{
			return false;
		}

		throw new EqualStrength();
	}
}