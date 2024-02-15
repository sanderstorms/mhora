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

using Mhora.Definitions;

namespace Mhora.Elements.Kuta;

public class KutaNadi
{
	public enum EType
	{
		Vata,
		Pitta,
		Sleshma
	}

	public static int GetMaxScore()
	{
		return 2;
	}

	public static int GetScore(Nakshatra m, Nakshatra n)
	{
		var ea = GetType(m);
		var eb = GetType(n);
		if (ea != eb)
		{
			return 2;
		}

		if (ea == EType.Vata || ea == EType.Sleshma)
		{
			return 1;
		}

		return 0;
	}

	public static EType GetType(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Aridra:
			case Nakshatra.Punarvasu:
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.Hasta:
			case Nakshatra.Jyestha:
			case Nakshatra.Moola:
			case Nakshatra.Satabisha:
			case Nakshatra.PoorvaBhadra: return EType.Vata;
			case Nakshatra.Bharani:
			case Nakshatra.Mrigarirsa:
			case Nakshatra.Pushya:
			case Nakshatra.PoorvaPhalguni:
			case Nakshatra.Chittra:
			case Nakshatra.Anuradha:
			case Nakshatra.PoorvaShada:
			case Nakshatra.Dhanishta:
			case Nakshatra.UttaraBhadra: return EType.Pitta;
		}

		return EType.Sleshma;
	}
}