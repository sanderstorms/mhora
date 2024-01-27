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
using System.ComponentModel;
using System.Diagnostics;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     A compile-time list of every body we will use in this program
/// </summary>
public static class Body
{
	[TypeConverter(typeof(EnumDescConverter))]
	public enum BodyType
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

	public enum Relationship
	{
		BitterEnemy = -2,
		Enemy       = -1,
		Neutral     =  0,
		Friend      =  1,
		BestFriend  =  2,
	}


	public static readonly string[] Karakas =
	{
		"Atma",
		"Amatya",
		"Bhratri",
		"Matri",
		"Pitri",
		"Putra",
		"Jnaati",
		"Dara"
	};

	public static readonly string[] KarakasS =
	{
		"AK",
		"AmK",
		"BK",
		"MK",
		"PiK",
		"PuK",
		"JK",
		"DK"
	};

	public static readonly string[] Karakas7 =
	{
		"Atma",
		"Amatya",
		"Bhratri",
		"Matri",
		"Pitri",
		"Jnaati",
		"Dara"
	};

	public static readonly string[] KarakasS7 =
	{
		"AK",
		"AmK",
		"BK",
		"MK",
		"PiK",
		"JK",
		"DK"
	};

	public static readonly int[] LattaAspects =
	{
		12,
		22,
		3,
		7,
		6,
		5,
		8,
		9
	};

	public static int SwephBody(this BodyType b)
	{
		switch (b)
		{
			case BodyType.Sun:     return sweph.SE_SUN;
			case BodyType.Moon:    return sweph.SE_MOON;
			case BodyType.Mars:    return sweph.SE_MARS;
			case BodyType.Mercury: return sweph.SE_MERCURY;
			case BodyType.Jupiter: return sweph.SE_JUPITER;
			case BodyType.Venus:   return sweph.SE_VENUS;
			case BodyType.Saturn:  return sweph.SE_SATURN;
			case BodyType.Lagna:   return sweph.SE_BIT_NO_REFRACTION;
			default:                    throw new Exception();
		}
	}

	public static Longitude ExaltationDegree(this BodyType body)
	{
		var b = (int) body;
		Debug.Assert(b >= (int) BodyType.Sun && b <= (int) BodyType.Saturn);
		double d = 0;
		switch (body)
		{
			case BodyType.Sun:
				d = 10;
				break;
			case BodyType.Moon:
				d = 33;
				break;
			case BodyType.Mars:
				d = 298;
				break;
			case BodyType.Mercury:
				d = 165;
				break;
			case BodyType.Jupiter:
				d = 95;
				break;
			case BodyType.Venus:
				d = 357;
				break;
			case BodyType.Saturn:
				d = 200;
				break;
		}

		return new Longitude(d);
	}

	public static Longitude DebilitationDegree(this BodyType b)
	{
		return ExaltationDegree(b).Add(180);
	}

	public static string Name(this BodyType b)
	{
		switch (b)
		{
			case BodyType.Lagna:   return "Lagna";
			case BodyType.Sun:     return "Sun";
			case BodyType.Moon:    return "Moon";
			case BodyType.Mars:    return "Mars";
			case BodyType.Mercury: return "Mercury";
			case BodyType.Jupiter: return "Jupiter";
			case BodyType.Venus:   return "Venus";
			case BodyType.Saturn:  return "Saturn";
			case BodyType.Rahu:    return "Rahu";
			case BodyType.Ketu:    return "Ketu";
		}

		return string.Empty;
	}

	public static string ToShortString(this BodyType b)
	{
		switch (b)
		{
			case BodyType.Lagna:        return "As";
			case BodyType.Sun:          return "Su";
			case BodyType.Moon:         return "Mo";
			case BodyType.Mars:         return "Ma";
			case BodyType.Mercury:      return "Me";
			case BodyType.Jupiter:      return "Ju";
			case BodyType.Venus:        return "Ve";
			case BodyType.Saturn:       return "Sa";
			case BodyType.Rahu:         return "Ra";
			case BodyType.Ketu:         return "Ke";
			case BodyType.AL:           return "AL";
			case BodyType.A2:           return "A2";
			case BodyType.A3:           return "A3";
			case BodyType.A4:           return "A4";
			case BodyType.A5:           return "A5";
			case BodyType.A6:           return "A6";
			case BodyType.A7:           return "A7";
			case BodyType.A8:           return "A8";
			case BodyType.A9:           return "A9";
			case BodyType.A10:          return "A10";
			case BodyType.A11:          return "A11";
			case BodyType.UL:           return "UL";
			case BodyType.GhatiLagna:   return "GL";
			case BodyType.BhavaLagna:   return "BL";
			case BodyType.HoraLagna:    return "HL";
			case BodyType.VighatiLagna: return "ViL";
			case BodyType.SreeLagna:    return "SL";
			case BodyType.Pranapada:    return "PL";
		}

		Trace.Assert(false, "Basics.Body.toShortString");
		return "   ";
	}

	public static bool IsFriend(this BodyType graha, BodyType other)
	{
		switch (graha)
		{
			case BodyType.Sun:
				switch (other)
				{
					case BodyType.Moon:
					case BodyType.Mars:
					case BodyType.Jupiter:
						return true;
				}
				break;
			case BodyType.Moon:
				switch (other)
				{
					case BodyType.Moon:
					case BodyType.Mercury:
						return true;
				}
				break;
			case BodyType.Mars:
				switch (other)
				{
					case BodyType.Sun:
					case BodyType.Moon:
					case BodyType.Jupiter:
					case BodyType.Ketu:
						return true;
				}
				break;

			case BodyType.Mercury:
				switch (other)
				{
					case BodyType.Sun:
					case BodyType.Venus:
						return true;
				}
				break;

			case BodyType.Jupiter:
				switch (other)
				{
					case BodyType.Sun:
					case BodyType.Moon:
					case BodyType.Mars:
					case BodyType.Rahu:
						return true;
				}
				break;

			case BodyType.Venus:
				switch (other)
				{
					case BodyType.Saturn:
					case BodyType.Rahu:
					case BodyType.Ketu:
						return true;
				}
				break;

			case BodyType.Saturn:
				switch (other)
				{
					case BodyType.Mercury:
					case BodyType.Venus:
					case BodyType.Rahu:
						return true;
				}
				break;

			case BodyType.Rahu:
				switch (other)
				{
					case BodyType.Jupiter:
					case BodyType.Venus:
					case BodyType.Saturn:
					case BodyType.Ketu:
						return true;
				}
				break;

			case BodyType.Ketu:
				switch (other)
				{
					case BodyType.Mars:
					case BodyType.Jupiter:
					case BodyType.Venus:
						return true;
				}
				break;
		}
		return false;
	}

	public static bool IsEnemy(this BodyType graha, BodyType other)
	{
		switch (graha)
		{
			case BodyType.Sun:
				switch (other)
				{
					case BodyType.Venus:
					case BodyType.Saturn:
					case BodyType.Rahu:
					case BodyType.Ketu:
						return true;
				}
				break;
			case BodyType.Moon:
				switch (other)
				{
					case BodyType.Rahu:
					case BodyType.Ketu:
						return true;
				}
				break;
			case BodyType.Mars:
				switch (other)
				{
					case BodyType.Mercury:
					case BodyType.Rahu:
						return true;
				}
				break;

			case BodyType.Mercury:
				switch (other)
				{
					case BodyType.Moon:
						return true;
				}
				break;

			case BodyType.Jupiter:
				switch (other)
				{
					case BodyType.Mercury:
					case BodyType.Venus:
						return true;
				}
				break;

			case BodyType.Venus:
				switch (other)
				{
					case BodyType.Sun:
					case BodyType.Moon:
						return true;
				}
				break;

			case BodyType.Saturn:
				switch (other)
				{
					case BodyType.Sun:
					case BodyType.Moon:
					case BodyType.Mars:
					case BodyType.Ketu:
						return true;
				}
				break;

			case BodyType.Rahu:
				switch (other)
				{
					case BodyType.Sun:
					case BodyType.Moon:
					case BodyType.Mars:
						return true;
				}
				break;

			case BodyType.Ketu:
				switch (other)
				{
					case BodyType.Moon:
						return true;
				}
				break;
		}
		return false;
	}

}