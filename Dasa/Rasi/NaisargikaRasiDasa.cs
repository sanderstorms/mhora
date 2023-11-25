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

using System;
using System.Collections;
using mhora.Calculation;
using mhora.Components.Property;

namespace mhora
{
    public class NaisargikaRasiDasa : Dasa, IDasa
    {
        private readonly Horoscope   h;
        private readonly UserOptions options;

        public NaisargikaRasiDasa(Horoscope _h)
        {
            h       = _h;
            options = new UserOptions();
        }

        public void recalculateOptions()
        {
        }

        public double paramAyus()
        {
            switch (options.ParamAyus)
            {
                case UserOptions.ParamAyusType.Long:   return 120.0;
                case UserOptions.ParamAyusType.Middle: return 108.0;
                default:                               return 96.0;
            }
        }

        public ArrayList Dasa(int cycle)
        {
            int[] order =
            {
                4,
                2,
                8,
                10,
                12,
                6,
                5,
                11,
                1,
                7,
                9,
                3
            };
            int[] short_length =
            {
                9,
                7,
                8
            };
            var al = new ArrayList(9);

            var    cycle_start = paramAyus() * cycle;
            var    curr        = 0.0;
            double dasa_length;
            var    zlagna = h.getPosition(Body.Body.Name.Lagna).longitude.toZodiacHouse();
            for (var i = 0; i < 12; i++)
            {
                var zh = zlagna.add(order[i]);
                switch (options.ParamAyus)
                {
                    case UserOptions.ParamAyusType.Long:
                        dasa_length = 10.0;
                        break;
                    case UserOptions.ParamAyusType.Middle:
                        dasa_length = 9.0;
                        break;
                    default:
                        var mod = (int)zh.value % 3;
                        dasa_length = short_length[mod];
                        break;
                }

                al.Add(new DasaEntry(zh.value, cycle_start + curr, dasa_length, 1, zh.value.ToString()));
                curr += dasa_length;
            }

            return al;
        }

        public ArrayList AntarDasa(DasaEntry pdi)
        {
            return new ArrayList();
        }

        public string Description()
        {
            return "Naisargika Rasi Dasa";
        }

        public object GetOptions()
        {
            return options.Clone();
        }

        public object SetOptions(object a)
        {
            var uo = (UserOptions)a;
            options.ParamAyus = uo.ParamAyus;
            RecalculateEvent();
            return options.Clone();
        }

        public class UserOptions : ICloneable
        {
            [PGDisplayName("Life Expectancy")]
            public enum ParamAyusType
            {
                Short,
                Middle,
                Long
            }

            public UserOptions()
            {
                ParamAyus = ParamAyusType.Middle;
            }

            [PGDisplayName("Total Param Ayus")]
            public ParamAyusType ParamAyus
            {
                get;
                set;
            }

            public object Clone()
            {
                var uo = new UserOptions();
                uo.ParamAyus = ParamAyus;
                return uo;
            }
        }
    }
}