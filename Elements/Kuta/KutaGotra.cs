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

public class KutaGotra
{
	public enum EType
	{
		Marichi,
		Vasishtha,
		Angirasa,
		Atri,
		Pulastya,
		Pulaha,
		Kretu
	}

	public static int GetScore(Nakshatra m, Nakshatra n)
	{
		if (GetType(m) == GetType(n))
		{
			return 0;
		}

		return 1;
	}

	public static int GetMaxScore()
	{
		return 1;
	}

	public static EType GetType(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Pushya:
			case Nakshatra.Swati: return EType.Marichi;
			case Nakshatra.Bharani:
			case Nakshatra.Aslesha:
			case Nakshatra.Vishaka:
			case Nakshatra.Sravana: return EType.Vasishtha;
			case Nakshatra.Krittika:
			case Nakshatra.Makha:
			case Nakshatra.Anuradha:
			case Nakshatra.Dhanishta: return EType.Angirasa;
			case Nakshatra.Rohini:
			case Nakshatra.PoorvaPhalguni:
			case Nakshatra.Jyestha:
			case Nakshatra.Satabisha: return EType.Atri;
			case Nakshatra.Mrigarirsa:
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.Moola:
			case Nakshatra.PoorvaBhadra: return EType.Pulastya;
			case Nakshatra.Aridra:
			case Nakshatra.Hasta:
			case Nakshatra.PoorvaShada:
			case Nakshatra.UttaraBhadra: return EType.Pulaha;
			case Nakshatra.Punarvasu:
			case Nakshatra.Chittra:
			case Nakshatra.UttaraShada:
			case Nakshatra.Revati: return EType.Kretu;
		}

		Debug.Assert(false, "KutaGotra::getType");
		return EType.Angirasa;
	}
}