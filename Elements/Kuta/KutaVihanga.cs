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

public class KutaVihanga
{
	public enum EDominator
	{
		Equal,
		Male,
		Female
	}

	public enum EType
	{
		Bharandhaka,
		Pingala,
		Crow,
		Cock,
		Peacock
	}

	public static EDominator GetDominator(Nakshatras.Nakshatra m, Nakshatras.Nakshatra n)
	{
		var em = GetType(m);
		var en = GetType(n);

		EType[] order =
		{
			EType.Peacock,
			EType.Cock,
			EType.Crow,
			EType.Pingala
		};
		if (em == en)
		{
			return EDominator.Equal;
		}

		for (var i = 0; i < order.Length; i++)
		{
			if (em == order[i])
			{
				return EDominator.Male;
			}

			if (en == order[i])
			{
				return EDominator.Female;
			}
		}

		return EDominator.Equal;
	}

	public static EType GetType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Krittika:
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Mrigarirsa: return EType.Bharandhaka;
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.Punarvasu:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.PoorvaPhalguni: return EType.Pingala;
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.Chittra:
			case Nakshatras.Nakshatra.Swati:
			case Nakshatras.Nakshatra.Vishaka:
			case Nakshatras.Nakshatra.Anuradha: return EType.Crow;
			case Nakshatras.Nakshatra.Jyestha:
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.UttaraShada:
			case Nakshatras.Nakshatra.Sravana: return EType.Cock;
			case Nakshatras.Nakshatra.Dhanishta:
			case Nakshatras.Nakshatra.Satabisha:
			case Nakshatras.Nakshatra.PoorvaBhadra:
			case Nakshatras.Nakshatra.UttaraBhadra:
			case Nakshatras.Nakshatra.Revati: return EType.Peacock;
		}

		Debug.Assert(false, "KutaVibhanga::getType");
		return EType.Bharandhaka;
	}
}