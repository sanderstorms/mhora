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

public class KutaVedha
{
	public enum EType
	{
		AswJye,
		BhaAnu,
		KriVis,
		RohSwa,
		AriSra,
		PunUsh,
		PusPsh,
		AslMoo,
		MakRev,
		PphUbh,
		UphPbh,
		HasSat,
		MriDha,
		Chi
	}

	public static int GetScore(Nakshatra m, Nakshatra n)
	{
		if (GetType(m) == GetType(n))
		{
			return 0;
		}

		return 1;
	}

	public static int GetMaxScore()
	{
		return 1;
	}

	public static EType GetType(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Jyestha: return EType.AswJye;
			case Nakshatra.Bharani:
			case Nakshatra.Anuradha: return EType.BhaAnu;
			case Nakshatra.Krittika:
			case Nakshatra.Vishaka: return EType.KriVis;
			case Nakshatra.Rohini:
			case Nakshatra.Swati: return EType.RohSwa;
			case Nakshatra.Aridra:
			case Nakshatra.Sravana: return EType.AriSra;
			case Nakshatra.Punarvasu:
			case Nakshatra.UttaraShada: return EType.PunUsh;
			case Nakshatra.Pushya:
			case Nakshatra.PoorvaShada: return EType.PusPsh;
			case Nakshatra.Aslesha:
			case Nakshatra.Moola: return EType.AslMoo;
			case Nakshatra.Makha:
			case Nakshatra.Revati: return EType.MakRev;
			case Nakshatra.PoorvaPhalguni:
			case Nakshatra.UttaraBhadra: return EType.PphUbh;
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.PoorvaBhadra: return EType.UphPbh;
			case Nakshatra.Hasta:
			case Nakshatra.Satabisha: return EType.HasSat;
			case Nakshatra.Mrigarirsa:
			case Nakshatra.Dhanishta: return EType.MriDha;
			case Nakshatra.Chittra: return EType.Chi;
		}

		Debug.Assert(false, "KutaVedha::getType");
		return EType.AriSra;
	}
}