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

public class KutaNakshatraYoni
{
	public enum ESex
	{
		Male,
		Female
	}

	public enum EType
	{
		Horse,
		Elephant,
		Sheep,
		Serpent,
		Dog,
		Cat,
		Rat,
		Cow,
		Buffalo,
		Tiger,
		Hare,
		Monkey,
		Lion,
		Mongoose
	}

	public static ESex GetSex(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.Swati:
			case Nakshatras.Nakshatra.Vishaka:
			case Nakshatras.Nakshatra.Jyestha:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.PoorvaBhadra:
			case Nakshatras.Nakshatra.UttaraShada: return ESex.Male;
		}

		return ESex.Female;
	}

	public static EType GetType(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Satabisha: return EType.Horse;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Revati: return EType.Elephant;
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.Krittika: return EType.Sheep;
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Mrigarirsa: return EType.Serpent;
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.Aridra: return EType.Dog;
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Punarvasu: return EType.Cat;
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.PoorvaPhalguni: return EType.Rat;
			case Nakshatras.Nakshatra.UttaraPhalguni:
			case Nakshatras.Nakshatra.UttaraBhadra: return EType.Cow;
			case Nakshatras.Nakshatra.Swati:
			case Nakshatras.Nakshatra.Hasta: return EType.Buffalo;
			case Nakshatras.Nakshatra.Vishaka:
			case Nakshatras.Nakshatra.Chittra: return EType.Tiger;
			case Nakshatras.Nakshatra.Jyestha:
			case Nakshatras.Nakshatra.Anuradha: return EType.Hare;
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.Sravana: return EType.Monkey;
			case Nakshatras.Nakshatra.PoorvaBhadra:
			case Nakshatras.Nakshatra.Dhanishta: return EType.Lion;
			case Nakshatras.Nakshatra.UttaraShada: return EType.Mongoose;
		}


		Debug.Assert(false, "KutaNakshatraYoni::getType");
		return EType.Horse;
	}
}