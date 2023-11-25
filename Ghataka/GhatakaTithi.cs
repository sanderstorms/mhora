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
    public class GhatakaTithi
    {
        public static bool checkTithi(ZodiacHouse janmaRasi, Tithi t)
        {
            var ja = janmaRasi.value;
            var gh = Tithi.NandaType.Nanda;
            switch (ja)
            {
                case ZodiacHouse.Name.Ari:
                    gh = Tithi.NandaType.Nanda;
                    break;
                case ZodiacHouse.Name.Tau:
                    gh = Tithi.NandaType.Purna;
                    break;
                case ZodiacHouse.Name.Gem:
                    gh = Tithi.NandaType.Bhadra;
                    break;
                case ZodiacHouse.Name.Can:
                    gh = Tithi.NandaType.Bhadra;
                    break;
                case ZodiacHouse.Name.Leo:
                    gh = Tithi.NandaType.Jaya;
                    break;
                case ZodiacHouse.Name.Vir:
                    gh = Tithi.NandaType.Purna;
                    break;
                case ZodiacHouse.Name.Lib:
                    gh = Tithi.NandaType.Rikta;
                    break;
                case ZodiacHouse.Name.Sco:
                    gh = Tithi.NandaType.Nanda;
                    break;
                case ZodiacHouse.Name.Sag:
                    gh = Tithi.NandaType.Jaya;
                    break;
                case ZodiacHouse.Name.Cap:
                    gh = Tithi.NandaType.Rikta;
                    break;
                case ZodiacHouse.Name.Aqu:
                    gh = Tithi.NandaType.Jaya;
                    break;
                case ZodiacHouse.Name.Pis:
                    gh = Tithi.NandaType.Purna;
                    break;
            }

            return t.toNandaType() == gh;
        }
    }
}