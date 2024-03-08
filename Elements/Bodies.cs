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
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.SwissEph;

namespace Mhora.Elements;

/// <summary>
///     A compile-time list of every body we will use in this program
/// </summary>
public static class Bodies
{

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

	public static int SwephBody(this Body b)
	{
		switch (b)
		{
			case Body.Sun:     return sweph.SE_SUN;
			case Body.Moon:    return sweph.SE_MOON;
			case Body.Mars:    return sweph.SE_MARS;
			case Body.Mercury: return sweph.SE_MERCURY;
			case Body.Jupiter: return sweph.SE_JUPITER;
			case Body.Venus:   return sweph.SE_VENUS;
			case Body.Saturn:  return sweph.SE_SATURN;
			case Body.Lagna:   return sweph.SE_BIT_NO_REFRACTION;
			case Body.Rahu:    return sweph.SE_MEAN_NODE;
			case Body.Ketu:    return sweph.SE_MEAN_NODE;

			default:                    throw new Exception();
		}
	}

	public static Longitude ExaltationDegree(this Body body)
	{
		var b = (int) body;
		Debug.Assert(b >= (int) Body.Sun && b <= (int) Body.Saturn);
		double d = 0;
		switch (body)
		{
			case Body.Sun:
				d = 10;
				break;
			case Body.Moon:
				d = 33;
				break;
			case Body.Mars:
				d = 298;
				break;
			case Body.Mercury:
				d = 165;
				break;
			case Body.Jupiter:
				d = 95;
				break;
			case Body.Venus:
				d = 357;
				break;
			case Body.Saturn:
				d = 200;
				break;
		}

		return new Longitude(d);
	}

	public static Longitude DebilitationDegree(this Body b)
	{
		return ExaltationDegree(b).Add(180.0);
	}

	public static string Name(this Body b)
	{
		switch (b)
		{
			case Body.Lagna:   return "Lagna";
			case Body.Sun:     return "Sun";
			case Body.Moon:    return "Moon";
			case Body.Mars:    return "Mars";
			case Body.Mercury: return "Mercury";
			case Body.Jupiter: return "Jupiter";
			case Body.Venus:   return "Venus";
			case Body.Saturn:  return "Saturn";
			case Body.Rahu:    return "Rahu";
			case Body.Ketu:    return "Ketu";
		}

		return string.Empty;
	}

	public static string ToShortString(this Body b)
	{
		switch (b)
		{
			case Body.Lagna:        return "As";
			case Body.Sun:          return "Su";
			case Body.Moon:         return "Mo";
			case Body.Mars:         return "Ma";
			case Body.Mercury:      return "Me";
			case Body.Jupiter:      return "Ju";
			case Body.Venus:        return "Ve";
			case Body.Saturn:       return "Sa";
			case Body.Rahu:         return "Ra";
			case Body.Ketu:         return "Ke";
			case Body.AL:           return "AL";
			case Body.A2:           return "A2";
			case Body.A3:           return "A3";
			case Body.A4:           return "A4";
			case Body.A5:           return "A5";
			case Body.A6:           return "A6";
			case Body.A7:           return "A7";
			case Body.A8:           return "A8";
			case Body.A9:           return "A9";
			case Body.A10:          return "A10";
			case Body.A11:          return "A11";
			case Body.UL:           return "UL";
			case Body.GhatiLagna:   return "GL";
			case Body.BhavaLagna:   return "BL";
			case Body.HoraLagna:    return "HL";
			case Body.VighatiLagna: return "ViL";
			case Body.SreeLagna:    return "SL";
			case Body.Pranapada:    return "PL";
		}

		Trace.Assert(false, "Basics.Body.toShortString");
		return "   ";
	}

	public static bool IsFriend(this Body graha, Body other)
	{
		switch (graha)
		{
			case Body.Sun:
				switch (other)
				{
					case Body.Moon:
					case Body.Mars:
					case Body.Jupiter:
						return true;
				}
				break;
			case Body.Moon:
				switch (other)
				{
					case Body.Moon:
					case Body.Mercury:
						return true;
				}
				break;
			case Body.Mars:
				switch (other)
				{
					case Body.Sun:
					case Body.Moon:
					case Body.Jupiter:
					case Body.Ketu:
						return true;
				}
				break;

			case Body.Mercury:
				switch (other)
				{
					case Body.Sun:
					case Body.Venus:
						return true;
				}
				break;

			case Body.Jupiter:
				switch (other)
				{
					case Body.Sun:
					case Body.Moon:
					case Body.Mars:
					case Body.Rahu:
						return true;
				}
				break;

			case Body.Venus:
				switch (other)
				{
					case Body.Saturn:
					case Body.Rahu:
					case Body.Ketu:
						return true;
				}
				break;

			case Body.Saturn:
				switch (other)
				{
					case Body.Mercury:
					case Body.Venus:
					case Body.Rahu:
						return true;
				}
				break;

			case Body.Rahu:
				switch (other)
				{
					case Body.Jupiter:
					case Body.Venus:
					case Body.Saturn:
					case Body.Ketu:
						return true;
				}
				break;

			case Body.Ketu:
				switch (other)
				{
					case Body.Mars:
					case Body.Jupiter:
					case Body.Venus:
						return true;
				}
				break;
		}
		return false;
	}

	public static bool IsEnemy(this Body graha, Body other)
	{
		switch (graha)
		{
			case Body.Sun:
				switch (other)
				{
					case Body.Venus:
					case Body.Saturn:
					case Body.Rahu:
					case Body.Ketu:
						return true;
				}
				break;
			case Body.Moon:
				switch (other)
				{
					case Body.Rahu:
					case Body.Ketu:
						return true;
				}
				break;
			case Body.Mars:
				switch (other)
				{
					case Body.Mercury:
					case Body.Rahu:
						return true;
				}
				break;

			case Body.Mercury:
				switch (other)
				{
					case Body.Moon:
						return true;
				}
				break;

			case Body.Jupiter:
				switch (other)
				{
					case Body.Mercury:
					case Body.Venus:
						return true;
				}
				break;

			case Body.Venus:
				switch (other)
				{
					case Body.Sun:
					case Body.Moon:
						return true;
				}
				break;

			case Body.Saturn:
				switch (other)
				{
					case Body.Sun:
					case Body.Moon:
					case Body.Mars:
					case Body.Ketu:
						return true;
				}
				break;

			case Body.Rahu:
				switch (other)
				{
					case Body.Sun:
					case Body.Moon:
					case Body.Mars:
						return true;
				}
				break;

			case Body.Ketu:
				switch (other)
				{
					case Body.Moon:
						return true;
				}
				break;
		}
		return false;
	}

	public static ZodiacHouse ExaltationSign(this Body body)
	{
		switch (body)
		{
			case Body.Sun:     return ZodiacHouse.Ari;
			case Body.Moon:    return ZodiacHouse.Tau;
			case Body.Mars:    return ZodiacHouse.Cap;
			case Body.Mercury: return ZodiacHouse.Vir;
			case Body.Jupiter: return ZodiacHouse.Can;
			case Body.Venus:   return ZodiacHouse.Pis;
			case Body.Saturn:  return ZodiacHouse.Lib;
			case Body.Rahu:    return ZodiacHouse.Gem;
			case Body.Ketu:    return ZodiacHouse.Sag;
		}

		throw new Exception("Not a graha");
	}

	public static ZodiacHouse DebilitationSign(this Body body)
	{
		switch (body)
		{
			case Body.Sun:     return ZodiacHouse.Lib;
			case Body.Moon:    return ZodiacHouse.Sco;
			case Body.Mars:    return ZodiacHouse.Can;
			case Body.Mercury: return ZodiacHouse.Pis;
			case Body.Jupiter: return ZodiacHouse.Cap;
			case Body.Venus:   return ZodiacHouse.Vir;
			case Body.Saturn:  return ZodiacHouse.Ari;
			case Body.Rahu:    return ZodiacHouse.Sag;
			case Body.Ketu:    return ZodiacHouse.Gem;
		}

		throw new Exception("Not a graha");
	}

	public static ZodiacHouse MooltrikonaSign(this Body body)
	{
		switch (body)
		{
			case Body.Sun:     return ZodiacHouse.Leo;
			case Body.Moon:    return ZodiacHouse.Tau;
			case Body.Mars:    return ZodiacHouse.Ari;
			case Body.Mercury: return ZodiacHouse.Vir;
			case Body.Jupiter: return ZodiacHouse.Sag;
			case Body.Venus:   return ZodiacHouse.Lib;
			case Body.Saturn:  return ZodiacHouse.Aqu;
			case Body.Rahu:    return ZodiacHouse.Vir;
			case Body.Ketu:    return ZodiacHouse.Pis;
		}

		throw new Exception("Not a graha");
	}
}