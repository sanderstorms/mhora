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

	public static int GetScore(Nakshatras.Nakshatra m, Nakshatras.Nakshatra n)
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

	public static EType GetType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.Swati: return EType.Marichi;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Vishaka:
			case Nakshatras.Nakshatra.Sravana: return EType.Vasishtha;
			case Nakshatras.Nakshatra.Krittika:
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.Anuradha:
			case Nakshatras.Nakshatra.Dhanishta: return EType.Angirasa;
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.PoorvaPhalguni:
			case Nakshatras.Nakshatra.Jyestha:
			case Nakshatras.Nakshatra.Satabisha: return EType.Atri;
			case Nakshatras.Nakshatra.Mrigarirsa:
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.PoorvaBhadra: return EType.Pulastya;
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.UttaraBhadra: return EType.Pulaha;
			case Nakshatras.Nakshatra.Punarvasu:
			case Nakshatras.Nakshatra.Chittra:
			case Nakshatras.Nakshatra.UttaraShada:
			case Nakshatras.Nakshatra.Revati: return EType.Kretu;
		}

		Debug.Assert(false, "KutaGotra::getType");
		return EType.Angirasa;
	}
}