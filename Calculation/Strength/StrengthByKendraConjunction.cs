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
    // Stronger rasi has larger number of grahas in kendras
    // Stronger graha is in such a rasi
    public class StrengthByKendraConjunction : BaseStrength, IStrengthRasi, IStrengthGraha
    {
        public StrengthByKendraConjunction(Horoscope h, Division dtype)
            : base(h, dtype, true)
        {
        }

        public bool stronger(Body.Body.Name m, Body.Body.Name n)
        {
            return stronger(h.getPosition(m).toDivisionPosition(dtype).zodiac_house.value,
                            h.getPosition(n).toDivisionPosition(dtype).zodiac_house.value);
        }

        public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
        {
            var numa = value(za);
            var numb = value(zb);
            if (numa > numb)
            {
                return true;
            }

            if (numb > numa)
            {
                return false;
            }

            throw new EqualStrength();
        }

        public int value(ZodiacHouse.Name _zh)
        {
            var kendras = new int[4]
            {
                1,
                4,
                7,
                10
            };
            var numGrahas = 0;
            var zh        = new ZodiacHouse(_zh);
            foreach (var i in kendras)
            {
                numGrahas += numGrahasInZodiacHouse(zh.add(i).value);
            }

            return numGrahas;
        }
    }
}