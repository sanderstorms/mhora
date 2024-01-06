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

using Mhora.Tables.Nakshatra;

namespace Mhora.Kuta;

public class KutaRajju
{
	public enum EType
	{
		IKantha,
		IKati,
		IPada,
		ISiro,
		IKukshi
	}

	public static int getScore(Nakshatra m, Nakshatra n)
	{
		if (getType(m) != getType(n))
		{
			return 1;
		}

		return 0;
	}

	public static int getMaxScore()
	{
		return 1;
	}

	public static EType getType(Nakshatra n)
	{
		switch (n.value)
		{
			case Nakshatra.Name.Rohini:
			case Nakshatra.Name.Aridra:
			case Nakshatra.Name.Hasta:
			case Nakshatra.Name.Swati:
			case Nakshatra.Name.Sravana:
			case Nakshatra.Name.Satabisha:
				return EType.IKantha;
			case Nakshatra.Name.Bharani:
			case Nakshatra.Name.Pushya:
			case Nakshatra.Name.PoorvaPhalguni:
			case Nakshatra.Name.Anuradha:
			case Nakshatra.Name.PoorvaShada:
			case Nakshatra.Name.UttaraBhadra:
				return EType.IKati;
			case Nakshatra.Name.Aswini:
			case Nakshatra.Name.Aslesha:
			case Nakshatra.Name.Makha:
			case Nakshatra.Name.Jyestha:
			case Nakshatra.Name.Moola:
			case Nakshatra.Name.Revati:
				return EType.IPada;
			case Nakshatra.Name.Mrigarirsa:
			case Nakshatra.Name.Dhanishta:
			case Nakshatra.Name.Chittra:
				return EType.ISiro;
		}

		return EType.IKukshi;
	}
}