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
		IDeva,
		INara,
		IRakshasa
	}

	public static int getScore(Nakshatras.Nakshatra m, Nakshatras.Nakshatra f)
	{
		var em = getType(m);
		var ef = getType(f);

		if (em == ef)
		{
			return 5;
		}

		if (em == EType.IDeva && ef == EType.INara)
		{
			return 4;
		}

		if (em == EType.IRakshasa && ef == EType.INara)
		{
			return 3;
		}

		if (em == EType.INara && ef == EType.IDeva)
		{
			return 2;
		}

		return 1;
	}

	public static int getMaxScore()
	{
		return 5;
	}

	public static EType getType(Nakshatras.Nakshatra n)
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
			case Nakshatras.Nakshatra.Revati: return EType.IDeva;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.PoorvaPhalguni:
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.UttaraShada:
			case Nakshatras.Nakshatra.PoorvaBhadra:
			case Nakshatras.Nakshatra.UttaraBhadra: return EType.INara;
		}

		return EType.IRakshasa;
	}
}