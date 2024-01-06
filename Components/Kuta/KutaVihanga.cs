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
using Mhora.Elements;

namespace Mhora.Components.Kuta;

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

	public static EDominator getDominator(Nakshatra m, Nakshatra n)
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

	public static EType getType(Nakshatra n)
	{
		switch (n.value)
		{
			case Nakshatra.Name.Aswini:
			case Nakshatra.Name.Bharani:
			case Nakshatra.Name.Krittika:
			case Nakshatra.Name.Rohini:
			case Nakshatra.Name.Mrigarirsa:
				return EType.IBharandhaka;
			case Nakshatra.Name.Aridra:
			case Nakshatra.Name.Punarvasu:
			case Nakshatra.Name.Pushya:
			case Nakshatra.Name.Aslesha:
			case Nakshatra.Name.Makha:
			case Nakshatra.Name.PoorvaPhalguni:
				return EType.IPingala;
			case Nakshatra.Name.UttaraPhalguni:
			case Nakshatra.Name.Hasta:
			case Nakshatra.Name.Chittra:
			case Nakshatra.Name.Swati:
			case Nakshatra.Name.Vishaka:
			case Nakshatra.Name.Anuradha:
				return EType.ICrow;
			case Nakshatra.Name.Jyestha:
			case Nakshatra.Name.Moola:
			case Nakshatra.Name.PoorvaShada:
			case Nakshatra.Name.UttaraShada:
			case Nakshatra.Name.Sravana:
				return EType.ICock;
			case Nakshatra.Name.Dhanishta:
			case Nakshatra.Name.Satabisha:
			case Nakshatra.Name.PoorvaBhadra:
			case Nakshatra.Name.UttaraBhadra:
			case Nakshatra.Name.Revati:
				return EType.IPeacock;
		}

		Debug.Assert(false, "KutaVibhanga::getType");
		return EType.IBharandhaka;
	}
}