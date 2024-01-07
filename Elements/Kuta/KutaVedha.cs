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

public class KutaVedha
{
	public enum EType
	{
		IAswJye,
		IBhaAnu,
		IKriVis,
		IRohSwa,
		IAriSra,
		IPunUsh,
		IPusPsh,
		IAslMoo,
		IMakRev,
		IPphUbh,
		IUphPbh,
		IHasSat,
		IMriDha,
		IChi
	}

	public static int getScore(Nakshatras.Nakshatra m, Nakshatras.Nakshatra n)
	{
		if (getType(m) == getType(n))
		{
			return 0;
		}

		return 1;
	}

	public static int getMaxScore()
	{
		return 1;
	}

	public static EType getType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Jyestha: return EType.IAswJye;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Anuradha: return EType.IBhaAnu;
			case Nakshatras.Nakshatra.Krittika:
			case Nakshatras.Nakshatra.Vishaka: return EType.IKriVis;
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Swati: return EType.IRohSwa;
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.Sravana: return EType.IAriSra;
			case Nakshatras.Nakshatra.Punarvasu:
			case Nakshatras.Nakshatra.UttaraShada: return EType.IPunUsh;
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.PoorvaShada: return EType.IPusPsh;
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Moola: return EType.IAslMoo;
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.Revati: return EType.IMakRev;
			case Nakshatras.Nakshatra.PoorvaPhalguni:
			case Nakshatras.Nakshatra.UttaraBhadra: return EType.IPphUbh;
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.PoorvaBhadra: return EType.IUphPbh;
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.Satabisha: return EType.IHasSat;
			case Nakshatras.Nakshatra.Mrigarirsa:
			case Nakshatras.Nakshatra.Dhanishta: return EType.IMriDha;
			case Nakshatras.Nakshatra.Chittra: return EType.IChi;
		}

		Debug.Assert(false, "KutaVedha::getType");
		return EType.IAriSra;
	}
}