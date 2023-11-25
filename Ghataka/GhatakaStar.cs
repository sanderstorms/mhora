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

using mhora.Calculation;

namespace mhora.Ghata
{
    public class GhatakaStar
    {
        public static bool checkStar(ZodiacHouse janmaRasi, Nakshatra nak)
        {
            var ja = janmaRasi.value;
            var gh = Nakshatra.Name.Aswini;
            switch (ja)
            {
                case ZodiacHouse.Name.Ari:
                    gh = Nakshatra.Name.Makha;
                    break;
                case ZodiacHouse.Name.Tau:
                    gh = Nakshatra.Name.Hasta;
                    break;
                case ZodiacHouse.Name.Gem:
                    gh = Nakshatra.Name.Swati;
                    break;
                case ZodiacHouse.Name.Can:
                    gh = Nakshatra.Name.Anuradha;
                    break;
                case ZodiacHouse.Name.Leo:
                    gh = Nakshatra.Name.Moola;
                    break;
                case ZodiacHouse.Name.Vir:
                    gh = Nakshatra.Name.Sravana;
                    break;
                case ZodiacHouse.Name.Lib:
                    gh = Nakshatra.Name.Satabisha;
                    break;
                case ZodiacHouse.Name.Sco:
                    gh = Nakshatra.Name.Revati;
                    break;
                // FIXME dveja nakshatra?????
                case ZodiacHouse.Name.Sag:
                    gh = Nakshatra.Name.Revati;
                    break;
                case ZodiacHouse.Name.Cap:
                    gh = Nakshatra.Name.Rohini;
                    break;
                case ZodiacHouse.Name.Aqu:
                    gh = Nakshatra.Name.Aridra;
                    break;
                case ZodiacHouse.Name.Pis:
                    gh = Nakshatra.Name.Aslesha;
                    break;
            }

            return nak.value == gh;
        }
    }
}