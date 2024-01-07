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

public class KutaNadi
{
	public enum EType
	{
		IVata,
		IPitta,
		ISleshma
	}

	public static int getMaxScore()
	{
		return 2;
	}

	public static int getScore(Nakshatras.Nakshatra m, Nakshatras.Nakshatra n)
	{
		var ea = getType(m);
		var eb = getType(n);
		if (ea != eb)
		{
			return 2;
		}

		if (ea == EType.IVata || ea == EType.ISleshma)
		{
			return 1;
		}

		return 0;
	}

	public static EType getType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Aridra:
			case Nakshatras.Nakshatra.Punarvasu:
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.Jyestha:
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.Satabisha:
			case Nakshatras.Nakshatra.PoorvaBhadra: return EType.IVata;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Mrigarirsa:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.PoorvaPhalguni:
			case Nakshatras.Nakshatra.Chittra:
			case Nakshatras.Nakshatra.Anuradha:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.Dhanishta:
			case Nakshatras.Nakshatra.UttaraBhadra: return EType.IPitta;
		}

		return EType.ISleshma;
	}
}