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
using Mhora.Calculation;
using Mhora.SwissEph;

namespace Mhora
{
    public class NakshatraPraveshDasa : Dasa, IDasa
    {
        private Horoscope h;

        public NakshatraPraveshDasa(Horoscope _h)
        {
            h = _h;
        }

        public object GetOptions()
        {
            return new object();
        }

        public object SetOptions(object a)
        {
            return new object();
        }

        public void recalculateOptions()
        {
        }

        public double paramAyus()
        {
            return 60.0;
        }

        public ArrayList Dasa(int cycle)
        {
            var al          = new ArrayList(60);
            var cycle_start = cycle * paramAyus();
            for (var i = 0; i < 60; i++)
            {
                var start = cycle_start + i;
                var di    = new DasaEntry(Body.Body.Name.Other, start, 1.0, 1, "Nakshatra Pravesh Year");
                al.Add(di);
            }

            return al;
        }

        public ArrayList AntarDasa(DasaEntry pdi)
        {
            string[] desc =
            {
                "  Month: ",
                "    Yoga: "
            };
            if (pdi.level == 3)
            {
                return new ArrayList();
            }

            ArrayList al;
            double start  = 0.0,
                   length = 0.0;
            var level = 0;

            al    = null;
            start = pdi.startUT;
            level = pdi.level + 1;

            switch (pdi.level)
            {
                case 1:
                    al     = new ArrayList(13);
                    length = pdi.dasaLength / 13.0;
                    //mhora.Log.Debug("AD length is {0}", length);
                    for (var i = 0; i < 15; i++)
                    {
                        var di = new DasaEntry(Body.Body.Name.Other, start, length, level, desc[level - 2]);
                        al.Add(di);
                        start += length;
                    }

                    return al;
                case 2:
                    al     = new ArrayList(27);
                    length = pdi.dasaLength / 27.0;
                    //mhora.Log.Debug("PD length is {0}", length);
                    for (var i = 0; i < 27; i++)
                    {
                        var di = new DasaEntry(Body.Body.Name.Other, start, length, level, desc[level - 2]);
                        //mhora.Log.Debug ("PD: Starg {0}, length {1}", start, length);
                        al.Add(di);
                        start += length;
                    }

                    return al;
            }

            return new ArrayList();
            ;
        }

        public new string EntryDescription(DasaEntry pdi, Moment start, Moment end)
        {
            if (pdi.level == 2)
            {
                var l  = Basics.CalculateBodyLongitude(start.toUniversalTime(), sweph.BodyNameToSweph(Body.Body.Name.Sun));
                var zh = l.toZodiacHouse();
                return zh.ToString();
            }

            if (pdi.level == 3)
            {
                var l = Basics.CalculateBodyLongitude(start.toUniversalTime(), sweph.BodyNameToSweph(Body.Body.Name.Moon));
                var n = l.toNakshatra();
                return n.toShortString();
            }

            return string.Empty;
        }

        public string Description()
        {
            return "Nakshatra Pravesh Chart Dasa";
        }
    }
}