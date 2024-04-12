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
using Mhora.Util;

namespace Mhora.Elements.Extensions;

public static class Nakshatras
{

	public static readonly int[][] TaraAspects =
	[
		[
			14,
			15
		],
		[
			14,
		15
		],
		[
			1,
		3,
		7,
		8,
		15
		],
		[
			1,
		15
		],
		[
			10,
		15,
		19
		],
		[
			1,
		15
		],
		[
			3,
		5,
		15,
		19
		],
		[
		]
	];

	public static string Name(Nakshatra value)
	{
		return value switch
		       {
			       Nakshatra.Aswini         => "Aswini",
			       Nakshatra.Bharani        => "Bharani",
			       Nakshatra.Krittika       => "Krittika",
			       Nakshatra.Rohini         => "Rohini",
			       Nakshatra.Mrigarirsa     => "Mrigasira",
			       Nakshatra.Aridra         => "Ardra",
			       Nakshatra.Punarvasu      => "Punarvasu",
			       Nakshatra.Pushya         => "Pushyami",
			       Nakshatra.Aslesha        => "Aslesha",
			       Nakshatra.Makha          => "Makha",
			       Nakshatra.PoorvaPhalguni => "P.Phalguni",
			       Nakshatra.UttaraPhalguni => "U.Phalguni",
			       Nakshatra.Hasta          => "Hasta",
			       Nakshatra.Chittra        => "Chitta",
			       Nakshatra.Swati          => "Swati",
			       Nakshatra.Vishaka        => "Visakha",
			       Nakshatra.Anuradha       => "Anuradha",
			       Nakshatra.Jyestha        => "Jyeshtha",
			       Nakshatra.Moola          => "Moola",
			       Nakshatra.PoorvaShada    => "P.Ashada",
			       Nakshatra.UttaraShada    => "U.Ashada",
			       Nakshatra.Sravana        => "Sravana",
			       Nakshatra.Dhanishta      => "Dhanishta",
			       Nakshatra.Satabisha      => "Shatabisha",
			       Nakshatra.PoorvaBhadra   => "P.Bhadra",
			       Nakshatra.UttaraBhadra   => "U.Bhadra",
			       Nakshatra.Revati         => "Revati",
			       _                        => "---"
		       };
	}

	public static string ToShortString(this Nakshatra value)
	{
		return value switch
		       {
			       Nakshatra.Aswini         => "Asw",
			       Nakshatra.Bharani        => "Bha",
			       Nakshatra.Krittika       => "Kri",
			       Nakshatra.Rohini         => "Roh",
			       Nakshatra.Mrigarirsa     => "Mri",
			       Nakshatra.Aridra         => "Ari",
			       Nakshatra.Punarvasu      => "Pun",
			       Nakshatra.Pushya         => "Pus",
			       Nakshatra.Aslesha        => "Asl",
			       Nakshatra.Makha          => "Mak",
			       Nakshatra.PoorvaPhalguni => "P.Ph",
			       Nakshatra.UttaraPhalguni => "U.Ph",
			       Nakshatra.Hasta          => "Has",
			       Nakshatra.Chittra        => "Chi",
			       Nakshatra.Swati          => "Swa",
			       Nakshatra.Vishaka        => "Vis",
			       Nakshatra.Anuradha       => "Anu",
			       Nakshatra.Jyestha        => "Jye",
			       Nakshatra.Moola          => "Moo",
			       Nakshatra.PoorvaShada    => "P.Ash",
			       Nakshatra.UttaraShada    => "U.Ash",
			       Nakshatra.Sravana        => "Sra",
			       Nakshatra.Dhanishta      => "Dha",
			       Nakshatra.Satabisha      => "Sat",
			       Nakshatra.PoorvaBhadra   => "P.Bh",
			       Nakshatra.UttaraBhadra   => "U.Bh",
			       Nakshatra.Revati         => "Rev",
			       _                        => "---"
		       };
	}

	public static Body Lord(this Nakshatra nakshatra)
	{
		return ((nakshatra.Index() - 1) % 9) switch
		       {
			       0 => Body.Ketu,
			       1 => Body.Venus,
			       2 => Body.Sun,
			       4 => Body.Moon,
			       5 => Body.Mars,
			       6 => Body.Rahu,
			       7 => Body.Jupiter,
			       8 => Body.Saturn,
			       9 => Body.Mercury,
			       _ => (Body.Lagna) //not possible
		       };
	}

	public static ZodiacHouse Pada(this Nakshatra nakshatra, int pada)
	{
		var index = ((nakshatra.Index() -1) * 4) + (pada - 1);
		return ((ZodiacHouse) ((index % 12) + 1));
	}


	public static int Normalize(this Nakshatra value)
	{
		return ((int) value).NormalizeInc(1, 27);
	}

	public static Nakshatra Add(this Nakshatra value, int i)
	{
		var snum = ((int) value + i - 1).NormalizeInc(1, 27);
		return (Nakshatra) snum;
	}

	public static Nakshatra AddReverse(this Nakshatra value, int i)
	{
		var snum = ((int) value - i + 1).NormalizeInc(1, 27);
		return (Nakshatra) snum;
	}

	public static int Normalize(this Nakshatra28 value)
	{
		return ((int)value).NormalizeInc(1, 28);
	}

	public static Nakshatra28 Add(this Nakshatra28 value, int i)
	{
		var snum = ((int)value + i - 1).NormalizeInc(1, 28);
		return (Nakshatra28) snum;
	}

	public static Nakshatra ToNakshatra(this Longitude l)
	{
		var snum = (int) (Math.Floor(l.Value / (360M / 27)) + 1);
		return (Nakshatra) snum;
	}

	public static Nakshatra28 ToNakshatra28(this Longitude l)
	{
		var snum = (int) (Math.Floor(l.Value / (360M / 27)) + 1);

		var ret = (Nakshatra28) snum;
		if (snum >= (int) Nakshatra28.Abhijit)
		{
			ret = ret.Add(2);
		}

		if (l.Value >= 270 + (6 + 40 / 60M) && l.Value <= 270 + (10 + 53 / 60M + 20 / 3600M))
		{
			ret = Nakshatra28.Abhijit;
		}

		return ret;
	}

	public static double NakshatraBase(this Longitude l)
	{
		var num  = l.ToNakshatra().Index();
		var cusp = (num - 1) * (360.0 / 27.0);
		return cusp;
	}

	public static double NakshatraOffset(this Longitude l)
	{
		var znum = l.ToNakshatra().Index();
		var cusp = (znum - 1) * (360 / 27M);
		var ret  = l.Value - cusp;
		Trace.Assert(ret >= 0 && ret <= 360 / 27M);
		return (double) ret;
	}

	public static double PercentageOfNakshatra(this Longitude l)
	{
		var offset = l.NakshatraOffset();
		var perc   = offset / (360.0 / 27.0) * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public static int NakshatraPada(this Longitude l)
	{
		var offset = l.NakshatraOffset();
		var val    = (int) (offset / (360.0 / (27.0 * 4.0))).Floor() + 1;
		Trace.Assert(val >= 1 && val <= 4);
		return val;
	}

	public static int AbsoluteNakshatraPada(this Longitude l)
	{
		var n = l.ToNakshatra().Index();
		var p = l.NakshatraPada();
		return (n - 1) * 4 + p;
	}

	public static double NakshatraPadaOffset(this Longitude l)
	{
		var pnum = l.AbsoluteNakshatraPada();
		var cusp = (pnum - 1) * (360 / (27M * 4));
		var ret  = l.Value - cusp;
		Trace.Assert(ret >= 0 && ret <= 360 / 27M);
		return (double)ret;
	}

	public static double NakshatraPadaPercentage(this Longitude l)
	{
		var offset = l.NakshatraPadaOffset();
		var perc   = offset / (360.0 / (27.0 * 4.0)) * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public static (Nakshatra, int) NakshatraPada(this ZodiacHouse zh)
	{
		var total     = (((zh.Index() -1) * 9.0) / 4) + 1;
		var nakshatra = ((int) (total)).NormalizeInc (1, 27);
		var pada      = ((int) (total - nakshatra) * 4);

		return ((Nakshatra) nakshatra, pada);
	}

	public static (Nakshatra, int) AddPada(this Nakshatra nakshatra, int nrOfPadas)
	{
		var pada = 1 + nrOfPadas;
		while (pada > 4)
		{
			nakshatra =  nakshatra.Add(2);
			pada      -= 4;
		}
		return (nakshatra, pada);
	}

	public static (Nakshatra, int) AddPada(this Longitude lon, int nrOfPadas)
	{
		var nakshatra = lon.ToNakshatra();
		var pada      = lon.NakshatraPada();

		return nakshatra.AddPada(pada + nrOfPadas);
	}
}