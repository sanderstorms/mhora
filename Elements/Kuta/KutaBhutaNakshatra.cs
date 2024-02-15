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

using System.Diagnostics;
using Mhora.Definitions;

namespace Mhora.Elements.Kuta;

/// <summary>
///     Summary description for Kutas.
/// </summary>
public class KutaBhutaNakshatra
{
	public enum EType
	{
		Earth,
		Water,
		Fire,
		Air,
		Ether
	}

	public static int GetMaxScore()
	{
		return 1;
	}

	public static int GetScore(Nakshatra m, Nakshatra n)
	{
		var a = GetType(m);
		var b = GetType(n);
		if (a == b)
		{
			return 1;
		}

		if (a == EType.Fire && b == EType.Air || a == EType.Air && b == EType.Fire)
		{
			return 1;
		}

		if (a == EType.Earth || b == EType.Earth)
		{
			return 1;
		}

		return 0;
	}

	public static EType GetType(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Bharani:
			case Nakshatra.Krittika:
			case Nakshatra.Rohini:
			case Nakshatra.Mrigarirsa: return EType.Earth;
			case Nakshatra.Aridra:
			case Nakshatra.Punarvasu:
			case Nakshatra.Pushya:
			case Nakshatra.Aslesha:
			case Nakshatra.Makha:
			case Nakshatra.PoorvaPhalguni: return EType.Water;
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.Hasta:
			case Nakshatra.Chittra:
			case Nakshatra.Swati:
			case Nakshatra.Vishaka: return EType.Fire;
			case Nakshatra.Anuradha:
			case Nakshatra.Jyestha:
			case Nakshatra.Moola:
			case Nakshatra.PoorvaShada:
			case Nakshatra.UttaraShada:
			case Nakshatra.Sravana: return EType.Air;
			case Nakshatra.Dhanishta:
			case Nakshatra.Satabisha:
			case Nakshatra.PoorvaBhadra:
			case Nakshatra.UttaraBhadra:
			case Nakshatra.Revati: return EType.Ether;
		}

		Debug.Assert(false, "KutaBhutaNakshatra::getType");
		return EType.Air;
	}
}