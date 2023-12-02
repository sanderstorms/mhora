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

using Mhora.Calculation;

namespace Mhora.Ghata
{
    public class GhatakaLagnaOpp
    {
        public static bool checkLagna(ZodiacHouse janma, ZodiacHouse same)
        {
            var ja = janma.value;
            var gh = ZodiacHouse.Name.Ari;
            switch (ja)
            {
                case ZodiacHouse.Name.Ari:
                    gh = ZodiacHouse.Name.Lib;
                    break;
                case ZodiacHouse.Name.Tau:
                    gh = ZodiacHouse.Name.Sco;
                    break;
                case ZodiacHouse.Name.Gem:
                    gh = ZodiacHouse.Name.Cap;
                    break;
                case ZodiacHouse.Name.Can:
                    gh = ZodiacHouse.Name.Ari;
                    break;
                case ZodiacHouse.Name.Leo:
                    gh = ZodiacHouse.Name.Can;
                    break;
                case ZodiacHouse.Name.Vir:
                    gh = ZodiacHouse.Name.Vir;
                    break;
                case ZodiacHouse.Name.Lib:
                    gh = ZodiacHouse.Name.Pis;
                    break;
                case ZodiacHouse.Name.Sco:
                    gh = ZodiacHouse.Name.Tau;
                    break;
                case ZodiacHouse.Name.Sag:
                    gh = ZodiacHouse.Name.Gem;
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

            return same.value == gh;
        }
    }
}