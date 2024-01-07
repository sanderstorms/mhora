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
		IEqual,
		IMale,
		IFemale
	}

	public enum EType
	{
		IBharandhaka,
		IPingala,
		ICrow,
		ICock,
		IPeacock
	}

	public static EDominator getDominator(Nakshatras.Nakshatra m, Nakshatras.Nakshatra n)
	{
		var em = getType(m);
		var en = getType(n);

		EType[] order =
		{
			EType.IPeacock,
			EType.ICock,
			EType.ICrow,
			EType.IPingala
		};
		if (em == en)
		{
			return EDominator.IEqual;
		}

		for (var i = 0; i < order.Length; i++)
		{
			if (em == order[i])
			{
				return EDominator.IMale;
			}

			if (en == order[i])
			{
				return EDominator.IFemale;
			}
		}

		return EDominator.IEqual;
	}

	public static EType getType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Krittika:
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Mrigarirsa: return EType.IBharandhaka;
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.Punarvasu:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.PoorvaPhalguni: return EType.IPingala;
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.Chittra:
			case Nakshatras.Nakshatra.Swati:
			case Nakshatras.Nakshatra.Vishaka:
			case Nakshatras.Nakshatra.Anuradha: return EType.ICrow;
			case Nakshatras.Nakshatra.Jyestha:
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.UttaraShada:
			case Nakshatras.Nakshatra.Sravana: return EType.ICock;
			case Nakshatras.Nakshatra.Dhanishta:
			case Nakshatras.Nakshatra.Satabisha:
			case Nakshatras.Nakshatra.PoorvaBhadra:
			case Nakshatras.Nakshatra.UttaraBhadra:
			case Nakshatras.Nakshatra.Revati: return EType.IPeacock;
		}

		Debug.Assert(false, "KutaVibhanga::getType");
		return EType.IBharandhaka;
	}
}