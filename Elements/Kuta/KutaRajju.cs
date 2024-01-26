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

namespace Mhora.Elements.Kuta;

public class KutaRajju
{
	public enum EType
	{
		Kantha,
		Kati,
		Pada,
		Siro,
		Kukshi
	}

	public static int GetScore(Nakshatras.Nakshatra m, Nakshatras.Nakshatra n)
	{
		if (GetType(m) != GetType(n))
		{
			return 1;
		}

		return 0;
	}

	public static int GetMaxScore()
	{
		return 1;
	}

	public static EType GetType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.Swati:
			case Nakshatras.Nakshatra.Sravana:
			case Nakshatras.Nakshatra.Satabisha: return EType.Kantha;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.PoorvaPhalguni:
			case Nakshatras.Nakshatra.Anuradha:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.UttaraBhadra: return EType.Kati;
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.Jyestha:
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.Revati: return EType.Pada;
			case Nakshatras.Nakshatra.Mrigarirsa:
			case Nakshatras.Nakshatra.Dhanishta:
			case Nakshatras.Nakshatra.Chittra: return EType.Siro;
		}

		return EType.Kukshi;
	}
}