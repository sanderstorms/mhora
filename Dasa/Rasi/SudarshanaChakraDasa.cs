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

using System.Collections;
using mhora.Calculation;
using mhora.Varga;

namespace mhora
{
    public class SudarshanaChakraDasa : Dasa, IDasa
    {
        private readonly Horoscope h;

        public SudarshanaChakraDasa(Horoscope _h)
        {
            h = _h;
        }

        public double paramAyus()
        {
            return 12;
        }

        public string Description()
        {
            return "Sudarshana Chakra Dasa";
        }

        public object GetOptions()
        {
            return new object();
        }

        public object SetOptions(object o)
        {
            return o;
        }

        public void recalculateOptions()
        {
        }

        public ArrayList Dasa(int cycle)
        {
            var al    = new ArrayList(12);
            var start = cycle * paramAyus();
            var lzh   = h.getPosition(Body.Body.Name.Lagna).toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house;
            for (var i = 1; i <= 12; i++)
            {
                var czh = lzh.add(i);
                al.Add(new DasaEntry(czh.value, start, 1, 1, czh.value.ToString()));
                start += 1;
            }

            return al;
        }

        public ArrayList AntarDasa(DasaEntry de)
        {
            var al     = new ArrayList(12);
            var start  = de.StartUT;
            var length = de.DasaLength / 12.0;
            var zh     = new ZodiacHouse(de.zodiacHouse);
            for (var i = 1; i <= 12; i++)
            {
                var czh = zh.add(i);
                al.Add(new DasaEntry(czh.value,
                                     start,
                                     length,
                                     de.level     + 1,
                                     de.shortDesc + " " + czh.value));
                start += length;
            }

            return al;
        }
    }
}