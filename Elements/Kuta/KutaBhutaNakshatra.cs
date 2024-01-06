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

/// <summary>
///     Summary description for Kutas.
/// </summary>
public class KutaBhutaNakshatra
{
	public enum EType
	{
		IEarth,
		IWater,
		IFire,
		IAir,
		IEther
	}

	public static int getMaxScore()
	{
		return 1;
	}

	public static int getScore(Nakshatra m, Nakshatra n)
	{
		var a = getType(m);
		var b = getType(n);
		if (a == b)
		{
			return 1;
		}

		if ((a == EType.IFire && b == EType.IAir) || (a == EType.IAir && b == EType.IFire))
		{
			return 1;
		}

		if (a == EType.IEarth || b == EType.IEarth)
		{
			return 1;
		}

		return 0;
	}

	public static EType getType(Nakshatra n)
	{
		switch (n.value)
		{
			case Nakshatra.Name.Aswini:
			case Nakshatra.Name.Bharani:
			case Nakshatra.Name.Krittika:
			case Nakshatra.Name.Rohini:
			case Nakshatra.Name.Mrigarirsa: return EType.IEarth;
			case Nakshatra.Name.Aridra:
			case Nakshatra.Name.Punarvasu:
			case Nakshatra.Name.Pushya:
			case Nakshatra.Name.Aslesha:
			case Nakshatra.Name.Makha:
			case Nakshatra.Name.PoorvaPhalguni: return EType.IWater;
			case Nakshatra.Name.UttaraPhalguni:
			case Nakshatra.Name.Hasta:
			case Nakshatra.Name.Chittra:
			case Nakshatra.Name.Swati:
			case Nakshatra.Name.Vishaka: return EType.IFire;
			case Nakshatra.Name.Anuradha:
			case Nakshatra.Name.Jyestha:
			case Nakshatra.Name.Moola:
			case Nakshatra.Name.PoorvaShada:
			case Nakshatra.Name.UttaraShada:
			case Nakshatra.Name.Sravana: return EType.IAir;
			case Nakshatra.Name.Dhanishta:
			case Nakshatra.Name.Satabisha:
			case Nakshatra.Name.PoorvaBhadra:
			case Nakshatra.Name.UttaraBhadra:
			case Nakshatra.Name.Revati: return EType.IEther;
		}

		Debug.Assert(false, "KutaBhutaNakshatra::getType");
		return EType.IAir;
	}
}