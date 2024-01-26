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

public class KutaGana
{
	public enum EType
	{
		Deva,
		Nara,
		Rakshasa
	}

	public static int GetScore(Nakshatras.Nakshatra m, Nakshatras.Nakshatra f)
	{
		var em = GetType(m);
		var ef = GetType(f);

		if (em == ef)
		{
			return 5;
		}

		if (em == EType.Deva && ef == EType.Nara)
		{
			return 4;
		}

		if (em == EType.Rakshasa && ef == EType.Nara)
		{
			return 3;
		}

		if (em == EType.Nara && ef == EType.Deva)
		{
			return 2;
		}

		return 1;
	}

	public static int GetMaxScore()
	{
		return 5;
	}

	public static EType GetType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Mrigarirsa:
			case Nakshatras.Nakshatra.Punarvasu:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.Swati:
			case Nakshatras.Nakshatra.Anuradha:
			case Nakshatras.Nakshatra.Sravana:
			case Nakshatras.Nakshatra.Revati: return EType.Deva;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.PoorvaPhalguni:
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.UttaraShada:
			case Nakshatras.Nakshatra.PoorvaBhadra:
			case Nakshatras.Nakshatra.UttaraBhadra: return EType.Nara;
		}

		return EType.Rakshasa;
	}
}