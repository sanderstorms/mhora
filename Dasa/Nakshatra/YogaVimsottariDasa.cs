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
using Mhora.Calculation;
using Mhora.Delegates;

namespace Mhora
{
    // Wrapper around vimsottari dasa that starts the initial dasa
    // based on the yoga
    public class YogaVimsottariDasa : NakshatraDasa, INakshatraDasa, INakshatraYogaDasa
    {
        private readonly Horoscope      h;
        private          UserOptions    options;
        private readonly VimsottariDasa vd;

        public YogaVimsottariDasa(Horoscope _h)
        {
            options    = new UserOptions();
            common     = this;
            yogaCommon = this;
            h          = _h;
            vd         = new VimsottariDasa(h);
        }

        public override object GetOptions()
        {
            return options.Clone();
        }

        public override object SetOptions(object a)
        {
            options = (UserOptions)options.SetOptions(a);
            if (RecalculateEvent != null)
            {
                RecalculateEvent();
            }

            return options.Clone();
        }

        public ArrayList Dasa(int cycle)
        {
            var t = new Transit(h);
            var l = t.LongitudeOfSunMoonYoga(h.baseUT);
            return _YogaDasa(l, 1, cycle);
        }

        public ArrayList AntarDasa(DasaEntry di)
        {
            return _AntarDasa(di);
        }

        public string Description()
        {
            return "Yoga Vimsottari Dasa";
        }

        public double paramAyus()
        {
            return vd.paramAyus();
        }

        public int numberOfDasaItems()
        {
            return vd.numberOfDasaItems();
        }

        public DasaEntry nextDasaLord(DasaEntry di)
        {
            return vd.nextDasaLord(di);
        }

        public double lengthOfDasa(Body.Body.Name plt)
        {
            return vd.lengthOfDasa(plt);
        }

        public Body.Body.Name lordOfNakshatra(Nakshatra n)
        {
            throw new Exception();
            return Body.Body.Name.Lagna;
        }

        public Body.Body.Name lordOfYoga(Longitude l)
        {
            return l.toSunMoonYoga().getLord();
        }

        public class UserOptions : ICloneable
        {
            public bool bExpungeTravelled = true;

            public UserOptions()
            {
                bExpungeTravelled = true;
            }

            [PGNotVisible]
            public bool UseYogaRemainder
            {
                get =>
                    bExpungeTravelled;
                set =>
                    bExpungeTravelled = value;
            }

            public object Clone()
            {
                var options = new UserOptions();
                options.bExpungeTravelled = bExpungeTravelled;
                return options;
            }

            public object SetOptions(object b)
            {
                if (b is UserOptions)
                {
                    var uo = (UserOptions)b;
                    bExpungeTravelled = uo.bExpungeTravelled;
                }

                return Clone();
            }
        }
    }
}