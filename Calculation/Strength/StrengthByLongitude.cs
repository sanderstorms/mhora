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

using mhora.Varga;

namespace mhora.Calculation.Strength
{
    // Stronger rasi has a graha which has traversed larger longitude
    // Stronger graha has traversed larger longitude in its house
    public class StrengthByLongitude : BaseStrength, IStrengthRasi, IStrengthGraha
    {
        public StrengthByLongitude(Horoscope h, Division dtype)
            : base(h, dtype, true)
        {
        }

        public bool stronger(Body.Body.Name m, Body.Body.Name n)
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

        public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
        {
            Body.Body.Name[] karakaBodies =
            {
                Body.Body.Name.Sun,
                Body.Body.Name.Moon,
                Body.Body.Name.Mars,
                Body.Body.Name.Mercury,
                Body.Body.Name.Jupiter,
                Body.Body.Name.Venus,
                Body.Body.Name.Saturn,
                Body.Body.Name.Rahu
            };

            double lona = 0.0,
                   lonb = 0.0;
            foreach (var bn in karakaBodies)
            {
                var div    = h.getPosition(bn).toDivisionPosition(new Division(Basics.DivisionType.Rasi));
                var offset = karakaLongitude(bn);
                if (div.zodiac_house.value == za && offset > lona)
                {
                    lona = offset;
                }
                else if (div.zodiac_house.value == zb && offset > lonb)
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
}