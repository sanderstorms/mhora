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

using mhora.Body;
using mhora.Calculation;

namespace mhora.Varga
{
    /// <summary>
    ///     Specifies a DivisionPosition, i.e. a position in a varga chart like Rasi
    ///     or Navamsa. It has no notion of "longitude".
    /// </summary>
    public class DivisionPosition
    {
        public double        cusp_higher;
        public double        cusp_lower;
        public Body.Body.Name     name;
        public string        otherString;
        public int           part;
        public int           ruler_index;
        public BodyType.Name type;
        public ZodiacHouse   zodiac_house;


        public DivisionPosition(Body.Body.Name     _name,
                                BodyType.Name _type,
                                ZodiacHouse   _zodiac_house,
                                double        _cusp_lower,
                                double        _cusp_higher,
                                int           _part)
        {
            name         = _name;
            type         = _type;
            zodiac_house = _zodiac_house;
            cusp_lower   = _cusp_lower;
            cusp_higher  = _cusp_higher;
            part         = _part;
            ruler_index  = 0;
        }

        public bool isInMoolaTrikona()
        {
            switch (name)
            {
                case Body.Body.Name.Sun:
                    if (zodiac_house.value == ZodiacHouse.Name.Leo)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Moon:
                    if (zodiac_house.value == ZodiacHouse.Name.Tau)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mars:
                    if (zodiac_house.value == ZodiacHouse.Name.Ari)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mercury:
                    if (zodiac_house.value == ZodiacHouse.Name.Vir)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Jupiter:
                    if (zodiac_house.value == ZodiacHouse.Name.Sag)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Venus:
                    if (zodiac_house.value == ZodiacHouse.Name.Lib)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Saturn:
                    if (zodiac_house.value == ZodiacHouse.Name.Aqu)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Rahu:
                    if (zodiac_house.value == ZodiacHouse.Name.Vir)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Ketu:
                    if (zodiac_house.value == ZodiacHouse.Name.Pis)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        public bool isInOwnHouse()
        {
            var zh = zodiac_house.value;
            switch (name)
            {
                case Body.Body.Name.Sun:
                    if (zh == ZodiacHouse.Name.Leo)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Moon:
                    if (zh == ZodiacHouse.Name.Tau)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mars:
                    if (zh == ZodiacHouse.Name.Ari || zh == ZodiacHouse.Name.Sco)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mercury:
                    if (zh == ZodiacHouse.Name.Gem || zh == ZodiacHouse.Name.Vir)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Jupiter:
                    if (zh == ZodiacHouse.Name.Sag || zh == ZodiacHouse.Name.Pis)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Venus:
                    if (zh == ZodiacHouse.Name.Tau || zh == ZodiacHouse.Name.Lib)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Saturn:
                    if (zh == ZodiacHouse.Name.Cap || zh == ZodiacHouse.Name.Aqu)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Rahu:
                    if (zh == ZodiacHouse.Name.Aqu)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Ketu:
                    if (zh == ZodiacHouse.Name.Sco)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        public bool isExaltedPhalita()
        {
            switch (name)
            {
                case Body.Body.Name.Sun:
                    if (zodiac_house.value == ZodiacHouse.Name.Ari)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Moon:
                    if (zodiac_house.value == ZodiacHouse.Name.Tau)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mars:
                    if (zodiac_house.value == ZodiacHouse.Name.Cap)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mercury:
                    if (zodiac_house.value == ZodiacHouse.Name.Vir)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Jupiter:
                    if (zodiac_house.value == ZodiacHouse.Name.Can)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Venus:
                    if (zodiac_house.value == ZodiacHouse.Name.Pis)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Saturn:
                    if (zodiac_house.value == ZodiacHouse.Name.Lib)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Rahu:
                    if (zodiac_house.value == ZodiacHouse.Name.Gem)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Ketu:
                    if (zodiac_house.value == ZodiacHouse.Name.Sag)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        public bool isDebilitatedPhalita()
        {
            switch (name)
            {
                case Body.Body.Name.Sun:
                    if (zodiac_house.value == ZodiacHouse.Name.Lib)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Moon:
                    if (zodiac_house.value == ZodiacHouse.Name.Sco)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mars:
                    if (zodiac_house.value == ZodiacHouse.Name.Can)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Mercury:
                    if (zodiac_house.value == ZodiacHouse.Name.Pis)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Jupiter:
                    if (zodiac_house.value == ZodiacHouse.Name.Cap)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Venus:
                    if (zodiac_house.value == ZodiacHouse.Name.Vir)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Saturn:
                    if (zodiac_house.value == ZodiacHouse.Name.Ari)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Rahu:
                    if (zodiac_house.value == ZodiacHouse.Name.Sag)
                    {
                        return true;
                    }

                    break;
                case Body.Body.Name.Ketu:
                    if (zodiac_house.value == ZodiacHouse.Name.Gem)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        public bool GrahaDristi(ZodiacHouse h)
        {
            var num = zodiac_house.numHousesBetween(h);
            if (num == 7)
            {
                return true;
            }

            if (name == Body.Body.Name.Jupiter && (num == 5 || num == 9))
            {
                return true;
            }

            if (name == Body.Body.Name.Rahu && (num == 5 || num == 9 || num == 2))
            {
                return true;
            }

            if (name == Body.Body.Name.Mars && (num == 4 || num == 8))
            {
                return true;
            }

            if (name == Body.Body.Name.Saturn && (num == 3 || num == 10))
            {
                return true;
            }

            return false;
        }
    }
}