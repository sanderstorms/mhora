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

using System.Diagnostics;

namespace mhora.Kuta
{
    public class KutaVarna
    {
        public enum EType
        {
            IBrahmana,
            IKshatriya,
            IVaishya,
            ISudra,
            IAnuloma,
            IPratiloma
        }

        public static int getMaxScore()
        {
            return 2;
        }

        public static int getScore(Nakshatra m, Nakshatra f)
        {
            var em = getType(m);
            var ef = getType(f);
            if (em == ef)
            {
                return 2;
            }

            if (em == EType.IBrahmana &&
                (ef == EType.IKshatriya || ef == EType.IVaishya || ef == EType.ISudra))
            {
                return 1;
            }

            if (em == EType.IKshatriya &&
                (ef == EType.IVaishya || ef == EType.ISudra))
            {
                return 1;
            }

            if (em == EType.IVaishya && ef == EType.ISudra)
            {
                return 1;
            }

            if (em == EType.IAnuloma && ef != EType.IPratiloma)
            {
                return 1;
            }

            if (ef == EType.IAnuloma && em != EType.IAnuloma)
            {
                return 1;
            }

            return 0;
        }

        public static EType getType(Nakshatra n)
        {
            switch ((int)n.value % 6)
            {
                case 1: return EType.IBrahmana;
                case 2: return EType.IKshatriya;
                case 3: return EType.IVaishya;
                case 4: return EType.ISudra;
                case 5: return EType.IAnuloma;
                case 0: return EType.IPratiloma;
                case 6: return EType.IPratiloma;
            }

            Debug.Assert(false, "KutaVarna::getType");
            return EType.IAnuloma;
        }
    }
}