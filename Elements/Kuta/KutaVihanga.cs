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

	public static EDominator GetDominator(Nakshatra m, Nakshatra n)
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

	public static EType GetType(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Bharani:
			case Nakshatra.Krittika:
			case Nakshatra.Rohini:
			case Nakshatra.Mrigarirsa: return EType.Bharandhaka;
			case Nakshatra.Aridra:
			case Nakshatra.Punarvasu:
			case Nakshatra.Pushya:
			case Nakshatra.Aslesha:
			case Nakshatra.Makha:
			case Nakshatra.PoorvaPhalguni: return EType.Pingala;
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.Hasta:
			case Nakshatra.Chittra:
			case Nakshatra.Swati:
			case Nakshatra.Vishaka:
			case Nakshatra.Anuradha: return EType.Crow;
			case Nakshatra.Jyestha:
			case Nakshatra.Moola:
			case Nakshatra.PoorvaShada:
			case Nakshatra.UttaraShada:
			case Nakshatra.Sravana: return EType.Cock;
			case Nakshatra.Dhanishta:
			case Nakshatra.Satabisha:
			case Nakshatra.PoorvaBhadra:
			case Nakshatra.UttaraBhadra:
			case Nakshatra.Revati: return EType.Peacock;
		}

		Debug.Assert(false, "KutaVibhanga::getType");
		return EType.Bharandhaka;
	}
}