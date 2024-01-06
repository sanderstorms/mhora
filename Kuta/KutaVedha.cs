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
using Mhora.Tables.Nakshatra;

namespace Mhora.Kuta;

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

	public static int getScore(Nakshatra m, Nakshatra n)
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

	public static EType getType(Nakshatra n)
	{
		switch (n.value)
		{
			case Nakshatra.Name.Aswini:
			case Nakshatra.Name.Jyestha:
				return EType.IAswJye;
			case Nakshatra.Name.Bharani:
			case Nakshatra.Name.Anuradha:
				return EType.IBhaAnu;
			case Nakshatra.Name.Krittika:
			case Nakshatra.Name.Vishaka:
				return EType.IKriVis;
			case Nakshatra.Name.Rohini:
			case Nakshatra.Name.Swati:
				return EType.IRohSwa;
			case Nakshatra.Name.Aridra:
			case Nakshatra.Name.Sravana:
				return EType.IAriSra;
			case Nakshatra.Name.Punarvasu:
			case Nakshatra.Name.UttaraShada:
				return EType.IPunUsh;
			case Nakshatra.Name.Pushya:
			case Nakshatra.Name.PoorvaShada:
				return EType.IPusPsh;
			case Nakshatra.Name.Aslesha:
			case Nakshatra.Name.Moola:
				return EType.IAslMoo;
			case Nakshatra.Name.Makha:
			case Nakshatra.Name.Revati:
				return EType.IMakRev;
			case Nakshatra.Name.PoorvaPhalguni:
			case Nakshatra.Name.UttaraBhadra:
				return EType.IPphUbh;
			case Nakshatra.Name.UttaraPhalguni:
			case Nakshatra.Name.PoorvaBhadra:
				return EType.IUphPbh;
			case Nakshatra.Name.Hasta:
			case Nakshatra.Name.Satabisha:
				return EType.IHasSat;
			case Nakshatra.Name.Mrigarirsa:
			case Nakshatra.Name.Dhanishta:
				return EType.IMriDha;
			case Nakshatra.Name.Chittra: return EType.IChi;
		}

		Debug.Assert(false, "KutaVedha::getType");
		return EType.IAriSra;
	}
}