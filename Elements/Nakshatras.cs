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

using Mhora.Definitions;
using Mhora.Elements.Yoga;
using Mhora.Util;

namespace Mhora.Elements;

public static class Nakshatras
{

	public static readonly int[][] TaraAspects =
	{
		new[]
		{
			14,
			15
		},
		new[]
		{
			14,
			15
		},
		new[]
		{
			1,
			3,
			7,
			8,
			15
		},
		new[]
		{
			1,
			15
		},
		new[]
		{
			10,
			15,
			19
		},
		new[]
		{
			1,
			15
		},
		new[]
		{
			3,
			5,
			15,
			19
		},
		new int[]
		{
		}
	};

	public static string Name(Nakshatra value)
	{
		switch (value)
		{
			case Nakshatra.Aswini:         return "Aswini";
			case Nakshatra.Bharani:        return "Bharani";
			case Nakshatra.Krittika:       return "Krittika";
			case Nakshatra.Rohini:         return "Rohini";
			case Nakshatra.Mrigarirsa:     return "Mrigasira";
			case Nakshatra.Aridra:         return "Ardra";
			case Nakshatra.Punarvasu:      return "Punarvasu";
			case Nakshatra.Pushya:         return "Pushyami";
			case Nakshatra.Aslesha:        return "Aslesha";
			case Nakshatra.Makha:          return "Makha";
			case Nakshatra.PoorvaPhalguni: return "P.Phalguni";
			case Nakshatra.UttaraPhalguni: return "U.Phalguni";
			case Nakshatra.Hasta:          return "Hasta";
			case Nakshatra.Chittra:        return "Chitta";
			case Nakshatra.Swati:          return "Swati";
			case Nakshatra.Vishaka:        return "Visakha";
			case Nakshatra.Anuradha:       return "Anuradha";
			case Nakshatra.Jyestha:        return "Jyeshtha";
			case Nakshatra.Moola:          return "Moola";
			case Nakshatra.PoorvaShada:    return "P.Ashada";
			case Nakshatra.UttaraShada:    return "U.Ashada";
			case Nakshatra.Sravana:        return "Sravana";
			case Nakshatra.Dhanishta:      return "Dhanishta";
			case Nakshatra.Satabisha:      return "Shatabisha";
			case Nakshatra.PoorvaBhadra:   return "P.Bhadra";
			case Nakshatra.UttaraBhadra:   return "U.Bhadra";
			case Nakshatra.Revati:         return "Revati";
			default:                  return "---";
		}
	}

	public static string ToShortString(this Nakshatra value)
	{
		switch (value)
		{
			case Nakshatra.Aswini:         return "Asw";
			case Nakshatra.Bharani:        return "Bha";
			case Nakshatra.Krittika:       return "Kri";
			case Nakshatra.Rohini:         return "Roh";
			case Nakshatra.Mrigarirsa:     return "Mri";
			case Nakshatra.Aridra:         return "Ari";
			case Nakshatra.Punarvasu:      return "Pun";
			case Nakshatra.Pushya:         return "Pus";
			case Nakshatra.Aslesha:        return "Asl";
			case Nakshatra.Makha:          return "Mak";
			case Nakshatra.PoorvaPhalguni: return "P.Ph";
			case Nakshatra.UttaraPhalguni: return "U.Ph";
			case Nakshatra.Hasta:          return "Has";
			case Nakshatra.Chittra:        return "Chi";
			case Nakshatra.Swati:          return "Swa";
			case Nakshatra.Vishaka:        return "Vis";
			case Nakshatra.Anuradha:       return "Anu";
			case Nakshatra.Jyestha:        return "Jye";
			case Nakshatra.Moola:          return "Moo";
			case Nakshatra.PoorvaShada:    return "P.Ash";
			case Nakshatra.UttaraShada:    return "U.Ash";
			case Nakshatra.Sravana:        return "Sra";
			case Nakshatra.Dhanishta:      return "Dha";
			case Nakshatra.Satabisha:      return "Sat";
			case Nakshatra.PoorvaBhadra:   return "P.Bh";
			case Nakshatra.UttaraBhadra:   return "U.Bh";
			case Nakshatra.Revati:         return "Rev";
			default:                  return "---";
		}
	}

	public static Body Lord(this Nakshatra nakshatra)
	{
		switch ((nakshatra.Index() - 1) % 9)
		{
			case 0: return Body.Ketu;
			case 1: return Body.Venus;
			case 2: return Body.Sun;
			case 4: return Body.Moon;
			case 5: return Body.Mars;
			case 6: return Body.Rahu;
			case 7: return Body.Jupiter;
			case 8: return Body.Saturn;
			case 9: return Body.Mercury;
		}

		return (Body.Lagna); //not possible
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
}