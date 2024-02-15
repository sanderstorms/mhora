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

	public static ESex GetSex(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Bharani:
			case Nakshatra.Pushya:
			case Nakshatra.Rohini:
			case Nakshatra.Moola:
			case Nakshatra.Aslesha:
			case Nakshatra.Makha:
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.Swati:
			case Nakshatra.Vishaka:
			case Nakshatra.Jyestha:
			case Nakshatra.PoorvaShada:
			case Nakshatra.PoorvaBhadra:
			case Nakshatra.UttaraShada: return ESex.Male;
		}

		return ESex.Female;
	}

	public static EType GetType(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Satabisha: return EType.Horse;
			case Nakshatra.Bharani:
			case Nakshatra.Revati: return EType.Elephant;
			case Nakshatra.Pushya:
			case Nakshatra.Krittika: return EType.Sheep;
			case Nakshatra.Rohini:
			case Nakshatra.Mrigarirsa: return EType.Serpent;
			case Nakshatra.Moola:
			case Nakshatra.Aridra: return EType.Dog;
			case Nakshatra.Aslesha:
			case Nakshatra.Punarvasu: return EType.Cat;
			case Nakshatra.Makha:
			case Nakshatra.PoorvaPhalguni: return EType.Rat;
			case Nakshatra.UttaraPhalguni:
			case Nakshatra.UttaraBhadra: return EType.Cow;
			case Nakshatra.Swati:
			case Nakshatra.Hasta: return EType.Buffalo;
			case Nakshatra.Vishaka:
			case Nakshatra.Chittra: return EType.Tiger;
			case Nakshatra.Jyestha:
			case Nakshatra.Anuradha: return EType.Hare;
			case Nakshatra.PoorvaShada:
			case Nakshatra.Sravana: return EType.Monkey;
			case Nakshatra.PoorvaBhadra:
			case Nakshatra.Dhanishta: return EType.Lion;
			case Nakshatra.UttaraShada: return EType.Mongoose;
		}


		Debug.Assert(false, "KutaNakshatraYoni::getType");
		return EType.Horse;
	}
}