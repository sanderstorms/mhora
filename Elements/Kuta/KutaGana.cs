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

namespace Mhora.Elements.Kuta;

public class KutaGana
{
	public enum EType
	{
		Deva,
		Nara,
		Rakshasa
	}

	public static int GetScore(Nakshatra m, Nakshatra f)
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

	public static EType GetType(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Mrigarirsa:
			case Nakshatra.Punarvasu:
			case Nakshatra.Pushya:
			case Nakshatra.Hasta:
			case Nakshatra.Swati:
			case Nakshatra.Anuradha:
			case Nakshatra.Sravana:
			case Nakshatra.Revati: return EType.Deva;
			case Nakshatra.Bharani:
			case Nakshatra.Rohini:
			case Nakshatra.Aridra:
			case Nakshatra.PoorvaPhalguni:
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.PoorvaShada:
			case Nakshatra.UttaraShada:
			case Nakshatra.PoorvaBhadra:
			case Nakshatra.UttaraBhadra: return EType.Nara;
		}

		return EType.Rakshasa;
	}
}