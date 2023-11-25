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
    // Stronger rasi has its lord in a house of different oddity
    // Stronger graha in such a rasi
    public class StrengthByLordInDifferentOddity : BaseStrength, IStrengthRasi, IStrengthGraha
    {
        public StrengthByLordInDifferentOddity(Horoscope h, Division dtype, bool bSimpleLord)
            : base(h, dtype, bSimpleLord)
        {
        }

        public bool stronger(Body.Body.Name ba, Body.Body.Name bb)
        {
            var za = h.getPosition(ba).toDivisionPosition(dtype).zodiac_house.value;
            var zb = h.getPosition(bb).toDivisionPosition(dtype).zodiac_house.value;
            return stronger(za, zb);
        }

        public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
        {
            var a = oddityValueForZodiacHouse(za);
            var b = oddityValueForZodiacHouse(zb);
            if (a > b)
            {
                return true;
            }

            if (a < b)
            {
                return false;
            }

            throw new EqualStrength();
        }

        protected int oddityValueForZodiacHouse(ZodiacHouse.Name zh)
        {
            var lname  = GetStrengthLord(zh);
            var lbpos  = h.getPosition(lname);
            var ldpos  = h.CalculateDivisionPosition(lbpos, dtype);
            var zh_lor = ldpos.zodiac_house;

            //System.Console.WriteLine("   DiffOddity {0} {1} {2}", zh.ToString(), zh_lor.value.ToString(), (int)zh %2==(int)zh_lor.value%2);
            if ((int)zh % 2 == (int)zh_lor.value % 2)
            {
                return 0;
            }

            return 1;
        }
    }
}