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
using Mhora.Util;

namespace Mhora.Elements.Extensions;

/// <summary>
///     A compile-time list of every body we will use in this program
/// </summary>
public static class Bodies
{
	public static readonly Time[] Cycle = new Time[10];

	static Bodies()
	{
		Cycle[Body.Lagna.Index()]   = TimeSpan.FromDays(1);
		Cycle[Body.Moon.Index()]    = TimeSpan.FromDays(27);
		Cycle[Body.Sun.Index()]     = TimeSpan.FromDays(360);
		Cycle[Body.Mercury.Index()] = Cycle[Body.Moon.Index()] * 12;
		Cycle[Body.Venus.Index()]   = Cycle[Body.Moon.Index()] * 12;
		Cycle[Body.Mars.Index()]    = Cycle[Body.Moon.Index()] * 18;
		Cycle[Body.Mars.Index()]    = Cycle[Body.Moon.Index()] * 18;
		Cycle[Body.Jupiter.Index()] = Cycle[Body.Sun.Index()]  * 13;
		Cycle[Body.Rahu.Index()]    = Cycle[Body.Sun.Index()]  * 18;
		Cycle[Body.Ketu.Index()]    = Cycle[Body.Sun.Index()]  * 18;
		Cycle[Body.Saturn.Index()]  = Cycle[Body.Sun.Index()]  * 30;
	}

	public static Time TransitPeriod(this Body body, Body other)
	{
		var transitTime = Cycle[body.Index()];

		if (transitTime > Cycle[other.Index()])
		{
			var period = transitTime / Cycle[other.Index()];
			return Cycle[other.Index()] + period;
		}
		return other.TransitPeriod(body);
	}

	public static TimeSpan ConvertTimeTo(this Body body, Time time, Body other)
	{
		var from = Cycle[body.Index()];
		var to   = Cycle[other.Index()];
		time /= from;
		time *= to;
		return time;
	}

	public static TimeSpan ConvertTimeFrom(this Body body, Time time, Body other)
	{
		var to   = Cycle[body.Index()];
		var from = Cycle[other.Index()];
		time /= from;
		time *= to;
		return time;
	}


	public static readonly Body[] HoraOrder =
	[
		Body.Sun,
		Body.Venus,
		Body.Mercury,
		Body.Moon,
		Body.Saturn,
		Body.Jupiter,
		Body.Mars
	];

	public static readonly Body[] KalaOrder =
	[
		Body.Sun,
		Body.Mars,
		Body.Jupiter,
		Body.Mercury,
		Body.Venus,
		Body.Saturn,
		Body.Moon,
		Body.Rahu
	];


	public static readonly string[] Karakas =
	[
		"Atma",
		"Amatya",
		"Bhratri",
		"Matri",
		"Pitri",
		"Putra",
		"Jnaati",
		"Dara"
	];

	public static readonly string[] KarakasS =
	[
		"AK",
		"AmK",
		"BK",
		"MK",
		"PiK",
		"PuK",
		"JK",
		"DK"
	];

	public static readonly string[] Karakas7 =
	[
		"Atma",
		"Amatya",
		"Bhratri",
		"Matri",
		"Pitri",
		"Jnaati",
		"Dara"
	];

	public static readonly string[] KarakasS7 =
	[
		"AK",
		"AmK",
		"BK",
		"MK",
		"PiK",
		"JK",
		"DK"
	];

	public static readonly int[] LattaAspects =
	[
		12,
		22,
		3,
		7,
		6,
		5,
		8,
		9
	];

	public static int SwephBody(this Body b)
	{
		return b switch
		       {
			       Body.Sun     => sweph.SE_SUN,
			       Body.Moon    => sweph.SE_MOON,
			       Body.Mars    => sweph.SE_MARS,
			       Body.Mercury => sweph.SE_MERCURY,
			       Body.Jupiter => sweph.SE_JUPITER,
			       Body.Venus   => sweph.SE_VENUS,
			       Body.Saturn  => sweph.SE_SATURN,
			       Body.Lagna   => sweph.SE_BIT_NO_REFRACTION,
			       Body.Rahu    => sweph.SE_MEAN_NODE,
			       Body.Ketu    => sweph.SE_MEAN_NODE,
			       _            => throw new Exception()
		       };
	}

	public static Longitude ExaltationDegree(this Body body)
	{
		var b = (int) body;
		Debug.Assert(b >= (int) Body.Sun && b <= (int) Body.Saturn);
		double d = body switch
		           {
			           Body.Sun     => 10,
			           Body.Moon    => 33,
			           Body.Mars    => 298,
			           Body.Mercury => 165,
			           Body.Jupiter => 95,
			           Body.Venus   => 357,
			           Body.Saturn  => 200,
			           _            => 0
		           };

		return new Longitude(d);
	}

	public static Longitude DebilitationDegree(this Body b)
	{
		return ExaltationDegree(b).Add(180.0);
	}

	public static string Name(this Body b)
	{
		return b switch
		       {
			       Body.Lagna   => "Lagna",
			       Body.Sun     => "Sun",
			       Body.Moon    => "Moon",
			       Body.Mars    => "Mars",
			       Body.Mercury => "Mercury",
			       Body.Jupiter => "Jupiter",
			       Body.Venus   => "Venus",
			       Body.Saturn  => "Saturn",
			       Body.Rahu    => "Rahu",
			       Body.Ketu    => "Ketu",
			       _            => string.Empty
		       };
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
		return body switch
		       {
			       Body.Sun     => ZodiacHouse.Ari,
			       Body.Moon    => ZodiacHouse.Tau,
			       Body.Mars    => ZodiacHouse.Cap,
			       Body.Mercury => ZodiacHouse.Vir,
			       Body.Jupiter => ZodiacHouse.Can,
			       Body.Venus   => ZodiacHouse.Pis,
			       Body.Saturn  => ZodiacHouse.Lib,
			       Body.Rahu    => ZodiacHouse.Gem,
			       Body.Ketu    => ZodiacHouse.Sag,
			       _            => throw new Exception("Not a graha")
		       };
	}

	public static ZodiacHouse DebilitationSign(this Body body)
	{
		return body switch
		       {
			       Body.Sun     => ZodiacHouse.Lib,
			       Body.Moon    => ZodiacHouse.Sco,
			       Body.Mars    => ZodiacHouse.Can,
			       Body.Mercury => ZodiacHouse.Pis,
			       Body.Jupiter => ZodiacHouse.Cap,
			       Body.Venus   => ZodiacHouse.Vir,
			       Body.Saturn  => ZodiacHouse.Ari,
			       Body.Rahu    => ZodiacHouse.Sag,
			       Body.Ketu    => ZodiacHouse.Gem,
			       _            => throw new Exception("Not a graha")
		       };
	}

	public static ZodiacHouse MooltrikonaSign(this Body body)
	{
		return body switch
		       {
			       Body.Sun     => ZodiacHouse.Leo,
			       Body.Moon    => ZodiacHouse.Tau,
			       Body.Mars    => ZodiacHouse.Ari,
			       Body.Mercury => ZodiacHouse.Vir,
			       Body.Jupiter => ZodiacHouse.Sag,
			       Body.Venus   => ZodiacHouse.Lib,
			       Body.Saturn  => ZodiacHouse.Aqu,
			       Body.Rahu    => ZodiacHouse.Vir,
			       Body.Ketu    => ZodiacHouse.Pis,
			       _            => throw new Exception("Not a graha")
		       };
	}
}