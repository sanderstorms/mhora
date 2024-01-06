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

using System.ComponentModel;
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Util;

namespace Mhora.Tables;

/// <summary>
///     A compile-time list of every body we will use in this program
/// </summary>
public class Body
{
	public enum Type
	{
		Lagna,
		Graha,
		NonLunarNode,
		SpecialLagna,
		ChandraLagna,
		BhavaArudha,
		BhavaArudhaSecondary,
		GrahaArudha,
		Varnada,
		Upagraha,
		Sahama,
		Other
	}

	[TypeConverter(typeof(EnumDescConverter))]
    public enum Name
    {
        // Do NOT CHANGE ORDER WITHOUT CHANING NARAYANA DASA ETC
        // RELY ON EXPLICIT EQUAL CONVERSION FOR STRONGER CO_LORD ETC
        Sun     = 0,
        Moon    = 1,
        Mars    = 2,
        Mercury = 3,
        Jupiter = 4,
        Venus   = 5,
        Saturn  = 6,
        Rahu    = 7,
        Ketu    = 8,
        Lagna   = 9,

        // And now, we're no longer uptight about the ordering :-)
        [Description("Bhava Lagna")]
        BhavaLagna,

        [Description("Hora Lagna")]
        HoraLagna,

        [Description("Ghati Lagna")]
        GhatiLagna,

        [Description("Sree Lagna")]
        SreeLagna,
        Pranapada,

        [Description("Vighati Lagna")]
        VighatiLagna,
        Dhuma,
        Vyatipata,
        Parivesha,
        Indrachapa,
        Upaketu,
        Kala,
        Mrityu,
        ArthaPraharaka,
        YamaGhantaka,
        Gulika,
        Maandi,

        [Description("Chandra Ayur Lagna")]
        ChandraAyurLagna,
        MrityuPoint,
        Other,
        AL,
        A2,
        A3,
        A4,
        A5,
        A6,
        A7,
        A8,
        A9,
        A10,
        A11,
        UL
    }

    public static int toInt(Name b)
    {
        return (int) b;
    }

    public static Longitude exaltationDegree(Name b)
    {
        var _b = (int) b;
        Debug.Assert(_b >= (int) Name.Sun && _b <= (int) Name.Saturn);
        double d = 0;
        switch (b)
        {
            case Name.Sun:
                d = 10;
                break;
            case Name.Moon:
                d = 33;
                break;
            case Name.Mars:
                d = 298;
                break;
            case Name.Mercury:
                d = 165;
                break;
            case Name.Jupiter:
                d = 95;
                break;
            case Name.Venus:
                d = 357;
                break;
            case Name.Saturn:
                d = 200;
                break;
        }

        return new Longitude(d);
    }

    public static Longitude debilitationDegree(Name b)
    {
        return exaltationDegree(b).add(180);
    }

    public static string toString(Name b)
    {
        switch (b)
        {
            case Name.Lagna:   return "Lagna";
            case Name.Sun:     return "Sun";
            case Name.Moon:    return "Moon";
            case Name.Mars:    return "Mars";
            case Name.Mercury: return "Mercury";
            case Name.Jupiter: return "Jupiter";
            case Name.Venus:   return "Venus";
            case Name.Saturn:  return "Saturn";
            case Name.Rahu:    return "Rahu";
            case Name.Ketu:    return "Ketu";
        }

        return string.Empty;
    }

    public static string toShortString(Name b)
    {
        switch (b)
        {
            case Name.Lagna:        return "As";
            case Name.Sun:          return "Su";
            case Name.Moon:         return "Mo";
            case Name.Mars:         return "Ma";
            case Name.Mercury:      return "Me";
            case Name.Jupiter:      return "Ju";
            case Name.Venus:        return "Ve";
            case Name.Saturn:       return "Sa";
            case Name.Rahu:         return "Ra";
            case Name.Ketu:         return "Ke";
            case Name.AL:           return "AL";
            case Name.A2:           return "A2";
            case Name.A3:           return "A3";
            case Name.A4:           return "A4";
            case Name.A5:           return "A5";
            case Name.A6:           return "A6";
            case Name.A7:           return "A7";
            case Name.A8:           return "A8";
            case Name.A9:           return "A9";
            case Name.A10:          return "A10";
            case Name.A11:          return "A11";
            case Name.UL:           return "UL";
            case Name.GhatiLagna:   return "GL";
            case Name.BhavaLagna:   return "BL";
            case Name.HoraLagna:    return "HL";
            case Name.VighatiLagna: return "ViL";
            case Name.SreeLagna:    return "SL";
            case Name.Pranapada:    return "PL";
        }

        Trace.Assert(false, "Basics.Body.toShortString");
        return "   ";
    }
}