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
    public class GhatakaMoon
    {
        public static bool checkGhataka(ZodiacHouse janmaRasi, ZodiacHouse chandraRasi)
        {
            var ja = janmaRasi.value;
            var ch = chandraRasi.value;

            var gh = ZodiacHouse.Name.Ari;

            switch (ja)
            {
                case ZodiacHouse.Name.Ari:
                    gh = ZodiacHouse.Name.Ari;
                    break;
                case ZodiacHouse.Name.Tau:
                    gh = ZodiacHouse.Name.Vir;
                    break;
                case ZodiacHouse.Name.Gem:
                    gh = ZodiacHouse.Name.Aqu;
                    break;
                case ZodiacHouse.Name.Can:
                    gh = ZodiacHouse.Name.Leo;
                    break;
                case ZodiacHouse.Name.Leo:
                    gh = ZodiacHouse.Name.Cap;
                    break;
                case ZodiacHouse.Name.Vir:
                    gh = ZodiacHouse.Name.Gem;
                    break;
                case ZodiacHouse.Name.Lib:
                    gh = ZodiacHouse.Name.Sag;
                    break;
                case ZodiacHouse.Name.Sco:
                    gh = ZodiacHouse.Name.Tau;
                    break;
                case ZodiacHouse.Name.Sag:
                    gh = ZodiacHouse.Name.Pis;
                    break;
                case ZodiacHouse.Name.Cap:
                    gh = ZodiacHouse.Name.Leo;
                    break;
                case ZodiacHouse.Name.Aqu:
                    gh = ZodiacHouse.Name.Sag;
                    break;
                case ZodiacHouse.Name.Pis:
                    gh = ZodiacHouse.Name.Aqu;
                    break;
            }

            return ch == gh;
        }
    }
}