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

	public static int getScore(Nakshatra m, Nakshatra f)
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

	public static EType getType(Nakshatra n)
	{
		switch (n.value)
		{
			case Nakshatra.Name.Aswini:
			case Nakshatra.Name.Mrigarirsa:
			case Nakshatra.Name.Punarvasu:
			case Nakshatra.Name.Pushya:
			case Nakshatra.Name.Hasta:
			case Nakshatra.Name.Swati:
			case Nakshatra.Name.Anuradha:
			case Nakshatra.Name.Sravana:
			case Nakshatra.Name.Revati: return EType.IDeva;
			case Nakshatra.Name.Bharani:
			case Nakshatra.Name.Rohini:
			case Nakshatra.Name.Aridra:
			case Nakshatra.Name.PoorvaPhalguni:
			case Nakshatra.Name.UttaraPhalguni:
			case Nakshatra.Name.PoorvaShada:
			case Nakshatra.Name.UttaraShada:
			case Nakshatra.Name.PoorvaBhadra:
			case Nakshatra.Name.UttaraBhadra: return EType.INara;
		}

		return EType.IRakshasa;
	}
}