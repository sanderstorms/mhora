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
using System.ComponentModel;
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Components.Property;
using Mhora.Util;
using Mhora.Varga;

namespace Mhora;

public class VimsottariDasa : NakshatraDasa, INakshatraDasa
{
    public Horoscope   horoscope;
    public UserOptions options;

    public VimsottariDasa(Horoscope h)
    {
        common    = this;
        options   = new UserOptions();
        horoscope = h;

        var fs_graha = new FindStronger(h, new Division(Basics.DivisionType.BhavaPada), FindStronger.RulesVimsottariGraha(h));
        var stronger = fs_graha.StrongerGraha(Body.Body.Name.Moon, Body.Body.Name.Lagna, false);

        if (stronger == Body.Body.Name.Lagna)
        {
            options.SeedBody = UserOptions.StartBodyType.Lagna;
        }
        else
        {
            options.SeedBody = UserOptions.StartBodyType.Moon;
        }

        h.Changed += ChangedHoroscope;
    }

    public override object GetOptions()
    {
        return options.Clone();
    }

    public override object SetOptions(object a)
    {
        var uo           = (UserOptions) a;
        var bRecalculate = false;
        if (options.SeedBody != uo.SeedBody)
        {
            options.SeedBody = uo.SeedBody;
            bRecalculate     = true;
        }

        if (options.div != uo.div)
        {
            options.div  = uo.div;
            bRecalculate = true;
        }

        if (bRecalculate && RecalculateEvent != null)
        {
            RecalculateEvent();
        }

        return options.Clone();
    }

    public ArrayList Dasa(int cycle)
    {
        return _Dasa(horoscope.getPosition(options.start_graha).extrapolateLongitude(options.div), options.nakshatra_offset, cycle);
    }

    public ArrayList AntarDasa(DasaEntry di)
    {
        return _AntarDasa(di);
    }

    public string Description()
    {
        return "Vimsottari Dasa Seeded from " + options.SeedBody;
    }

    public double paramAyus()
    {
        return 120.0;
    }

    public int numberOfDasaItems()
    {
        return 9;
    }

    public DasaEntry nextDasaLord(DasaEntry di)
    {
        return new DasaEntry(nextDasaLordHelper(di.graha), 0, 0, di.level, string.Empty);
    }

    public double lengthOfDasa(Body.Body.Name plt)
    {
        return LengthOfDasa(plt);
    }

    public Body.Body.Name lordOfNakshatra(Nakshatra n)
    {
        return LordOfNakshatra(n);
    }

    public new void DivisionChanged(Division div)
    {
        var uoNew = (UserOptions) options.Clone();
        uoNew.div = (Division) div.Clone();
        SetOptions(uoNew);
    }

    public void ChangedHoroscope(object o)
    {
        var h = (Horoscope) o;
        OnChanged();
    }

    private Body.Body.Name nextDasaLordHelper(Body.Body.Name b)
    {
        switch (b)
        {
            case Body.Body.Name.Sun:     return Body.Body.Name.Moon;
            case Body.Body.Name.Moon:    return Body.Body.Name.Mars;
            case Body.Body.Name.Mars:    return Body.Body.Name.Rahu;
            case Body.Body.Name.Rahu:    return Body.Body.Name.Jupiter;
            case Body.Body.Name.Jupiter: return Body.Body.Name.Saturn;
            case Body.Body.Name.Saturn:  return Body.Body.Name.Mercury;
            case Body.Body.Name.Mercury: return Body.Body.Name.Ketu;
            case Body.Body.Name.Ketu:    return Body.Body.Name.Venus;
            case Body.Body.Name.Venus:   return Body.Body.Name.Sun;
        }

        Trace.Assert(false, "VimsottariDasa::nextDasaLord");
        return Body.Body.Name.Lagna;
    }

    public static double LengthOfDasa(Body.Body.Name plt)
    {
        switch (plt)
        {
            case Body.Body.Name.Sun:     return 6;
            case Body.Body.Name.Moon:    return 10;
            case Body.Body.Name.Mars:    return 7;
            case Body.Body.Name.Rahu:    return 18;
            case Body.Body.Name.Jupiter: return 16;
            case Body.Body.Name.Saturn:  return 19;
            case Body.Body.Name.Mercury: return 17;
            case Body.Body.Name.Ketu:    return 7;
            case Body.Body.Name.Venus:   return 20;
        }

        Trace.Assert(false, "Vimsottari::lengthOfDasa");
        return 0;
    }

    public static Body.Body.Name LordOfNakshatra(Nakshatra n)
    {
        var lords = new Body.Body.Name[9]
        {
            Body.Body.Name.Mercury,
            Body.Body.Name.Ketu,
            Body.Body.Name.Venus,
            Body.Body.Name.Sun,
            Body.Body.Name.Moon,
            Body.Body.Name.Mars,
            Body.Body.Name.Rahu,
            Body.Body.Name.Jupiter,
            Body.Body.Name.Saturn
        };
        var nak_val = (int) n.value % 9;
        return lords[nak_val];
    }

    public class UserOptions : ICloneable
    {
        [TypeConverter(typeof(EnumDescConverter))]
        public enum StartBodyType
        {
            [Description("Lagna's sphuta")]
            Lagna,

            [Description("Moon's sphuta")]
            Moon,

            [Description("Jupiter's sphuta")]
            Jupiter,

            [Description("Utpanna - 5th tara from Moon's sphuta")]
            Utpanna,

            [Description("Kshema - 4th tara from Moon's sphuta")]
            Kshema,

            [Description("Adhana - 8th tara from Moon's sphuta")]
            Aadhaana,

            [Description("Mandi's sphuta")]
            Maandi,

            [Description("Gulika's sphuta")]
            Gulika
        }

        public Division       div = new(Basics.DivisionType.Rasi);
        public int            nakshatra_offset;
        public Body.Body.Name start_graha;
        public StartBodyType  user_start_graha;


        [PGDisplayName("Varga")]
        public Basics.DivisionType Varga
        {
            get =>
                div.MultipleDivisions[0].Varga;
            set =>
                div = new Division(value);
        }

        [PGDisplayName("Seed Nakshatra")]
        public StartBodyType SeedBody
        {
            get =>
                user_start_graha;
            set
            {
                user_start_graha = value;
                switch (value)
                {
                    case StartBodyType.Lagna:
                        start_graha      = Body.Body.Name.Lagna;
                        nakshatra_offset = 1;
                        break;
                    case StartBodyType.Jupiter:
                        start_graha      = Body.Body.Name.Jupiter;
                        nakshatra_offset = 1;
                        break;
                    case StartBodyType.Moon:
                        start_graha      = Body.Body.Name.Moon;
                        nakshatra_offset = 1;
                        break;
                    case StartBodyType.Utpanna:
                        start_graha      = Body.Body.Name.Moon;
                        nakshatra_offset = 5;
                        break;
                    case StartBodyType.Kshema:
                        start_graha      = Body.Body.Name.Moon;
                        nakshatra_offset = 4;
                        break;
                    case StartBodyType.Aadhaana:
                        start_graha      = Body.Body.Name.Moon;
                        nakshatra_offset = 8;
                        break;
                    case StartBodyType.Maandi:
                        start_graha      = Body.Body.Name.Maandi;
                        nakshatra_offset = 1;
                        break;
                    case StartBodyType.Gulika:
                        start_graha      = Body.Body.Name.Gulika;
                        nakshatra_offset = 1;
                        break;
                }
            }
        }

        public object Clone()
        {
            var options = new UserOptions();
            options.start_graha      = start_graha;
            options.nakshatra_offset = nakshatra_offset;
            options.SeedBody         = SeedBody;
            options.div              = (Division) div.Clone();
            return options;
        }
    }
}