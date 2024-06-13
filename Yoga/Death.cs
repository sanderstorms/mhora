using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Yoga
{
	public static class Death
	{
		//The Sun in 10th and Mars in 4th
		//Fall from the top of a mountain.
		public static bool FallFromMountain(this Grahas grahas)
		{
			var sun  = grahas[Body.Sun];
			var mars = grahas[Body.Mars];

			if (sun.Bhava != Bhava.KarmaBhava)
			{
				return false;
			}

			if (mars.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Saturn in 4th, the Moon in 7th and Mars in 10th
		//Falling into a well.
		public static bool FallIntoWell(this Grahas grahas)
		{
			var saturn = grahas [Body.Saturn];
			var moon   = grahas[Body.Moon];
			var mars   = grahas[Body.Mars];

			if (saturn.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			if (moon.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			if (mars.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Both the luminaries in Virgo aspected by malefics
		//Own Kinsmen/ Poisoning.
		public static bool Poisoning(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			var sun  = grahas[Body.Sun];

			if (moon.Rashi != ZodiacHouse.Vir)
			{
				return (false);
			}

			if (sun.Rashi != ZodiacHouse.Vir)
			{
				return (false);
			}

			if (sun.AspectFrom.Count == 0)
			{
				return (false);
			}

			foreach (var graha in sun.AspectFrom)
			{
				if (graha.IsNaturalBenefic)
				{
					return (false);
				}
			}

			return (true);
		}

		//Both the luminaries in Lagna happened to be a dual sign
		//Watery grave.
		public static bool WateryGrave(this Grahas grahas)
		{
			var moon  = grahas[Body.Moon];
			var sun   = grahas[Body.Sun];
			var lagna = grahas[Body.Lagna];

			if (lagna.Rashi.ZodiacHouse.IsDualSign() == false)
			{
				return (false);
			}

			if (moon.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (sun.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Saturn in Cancer, the Moon in Capricorn
		//Dropsy
		public static bool Dropsy(this Grahas grahas)
		{
			var saturn = grahas[Body.Saturn];
			var moon   = grahas[Body.Moon];

			if (saturn.Rashi != ZodiacHouse.Can)
			{
				return (false);
			}

			if (moon.Rashi != ZodiacHouse.Cap)
			{
				return (false);
			}

			return (true);
		}

		//The Moon in Martian sign hemmed in between malefics
		//Weapons or Fire.
		public static bool WeaponsOrFire(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Rashi.Lord != Body.Mars)
			{
				return (false);
			}

			if (moon.Before.IsNaturalMalefic)
			{
				if (moon.After.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//The Moon in Virgo betwixt malefics
		//Vitiation of blood or consumption.
		public static bool Consumption(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Rashi != ZodiacHouse.Vir)
			{
				return (false);
			}

			if (moon.Before.IsNaturalMalefic)
			{
				if (moon.After.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//The Moon in Saturnine signs in betwixt malefics
		//Hanging, Fire or fall.
		public static bool Hanging_Fire_or_Fall(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Rashi.Lord != Body.Saturn)
			{
				return (false);
			}

			if (moon.Before.IsNaturalMalefic)
			{
				if (moon.After.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//5th and 9th occupied by malefics without benefic aspect
		//Captivity/ Prison.
		public static bool Prison(this Grahas grahas)
		{
			var bhava5 = grahas.Rashis[Bhava.PutraBhava];
			var bhava9 = grahas.Rashis[Bhava.DharmaBhava];

			if (bhava5.Grahas.Count == 0)
			{
				return (false);
			}

			if (bhava9.Grahas.Count == 0)
			{
				return (false);
			}

			foreach (var graha in bhava5.Grahas)
			{
				if (graha.IsNaturalMalefic == false)
				{
					return (false);
				}

				foreach (var aspect in graha.AspectFrom)
				{
					if (aspect.IsFunctionalBenefic)
					{
						return (false);
					}
				}
			}

			foreach (var graha in bhava9.Grahas)
			{
				if (graha.IsNaturalMalefic == false)
				{
					return (false);
				}

				foreach (var aspect in graha.AspectFrom)
				{
					if (aspect.IsFunctionalBenefic)
					{
						return (false);
					}
				}
			}

			return (true);
		}

		//Decanate of 8th house be Sarpa (Serpent) or Pasa (Noose) or Nigada (Fetters)
		public static bool Captivity(this Grahas grahas)
		{
			return (false);
		}

		//Virgo happened to be 7th (Pisces Lagna) occupied by the Moon along with malefic,
		//Venus in Aries(2nd), Sun in the Lagna
		//Die on account of a woman.
		public static bool OnAaccountOfAWoman(this Grahas grahas)
		{
			var lagna = grahas[Body.Lagna];
			if (lagna.Rashi == ZodiacHouse.Pis)
			{
				var moon = grahas [Body.Moon];
				if (moon.Bhava != Bhava.JayaBhava)
				{
					return (false);
				}

				if (moon.Conjunct.Count == 0)
				{
					return (false);
				}

				foreach (var graha in moon.Conjunct)
				{
					if (graha.IsNaturalMalefic == false)
					{
						return (false);
					}
				}

				var venus = grahas [Body.Venus];
				if (venus.Bhava != Bhava.DhanaBhava)
				{
					return (false);
				}

				var sun = grahas [Body.Sun];
				if (sun.Bhava != Bhava.LagnaBhava)
				{
					return (false);
				}

				return (true);
			}

			return (false);
		}

		//Sun or Mars in 4th, Saturn in 10th, weak Moon conjoined with malefics in the 1st or 5th or 9th
		public static bool Impalement(this Grahas grahas)
		{
			var sun    = grahas [Body.Sun];
			var mars   = grahas [Body.Mars];
			var saturn = grahas [Body.Saturn];
			var moon   = grahas [Body.Moon];

			if (sun.Bhava != Bhava.SukhaBhava)
			{
				if (mars.Bhava != Bhava.SukhaBhava)
				{
					return (false);
				}
			}

			if (saturn.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			if (moon.Bhava.IsTrikona() == false)
			{
				return (false);
			}

			if (moon.Conjunct.Count == 0)
			{
				return (false);
			}
	
			foreach (var graha in moon.Conjunct)
			{
				if (graha.IsFunctionalMalefic == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//The Sun in the 4th, Mars conjoined with weak Moon in the 10th aspected by Saturn
		//Four planets mentioned above in 8th, 10th, 1st and the 4th
		public static bool BeatenWithWoodenClubs(this Grahas grahas)
		{
			var moon = grahas [Body.Moon];
			if (moon.IsAspectedBy(Body.Saturn) == false)
			{
				return (false);
			}

			if (moon.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			if (moon.Strength > 0)
			{
				return (false);
			}

			var sun  = grahas [Body.Sun];
			if (sun.Bhava == Bhava.SukhaBhava)
			{
				if (moon.Bhava == Bhava.KarmaBhava)
				{
					return (true);
				}
			}
			return (false);
		}

		//Four planets mentioned above in 8th, 10th, 1st and the 4th
		public static bool BeatenWithWoodenClub(this Grahas grahas)
		{
			var yoga = 0x00;

			foreach (var graha in grahas)
			{
				switch (graha.Bhava)
				{
					case Bhava.LagnaBhava:
					case Bhava.SukhaBhava:
					case Bhava.MrtyuBhava:
					case Bhava.KarmaBhava:
					{
						switch (graha.Body)
						{
							case Body.Mars:
							case Body.Moon:
							case Body.Sun:
							case Body.Saturn:
							{
								yoga |= (1 << (int) graha.Bhava);
							}
							break;
						}
					}
					break;
				}
			}

			return yoga.NumberOfSetBits() == 4;
		}

		//Above four planets in the 10th, 9th, 1st and 5th
		//Suffocation, Fire, Imprisonment, Beating.
		public static bool Suffocation(this Grahas grahas)
		{
			var yoga = 0x00;

			foreach (var graha in grahas)
			{
				switch (graha.Bhava)
				{
					case Bhava.LagnaBhava:
					case Bhava.PutraBhava:
					case Bhava.DharmaBhava:
					case Bhava.KarmaBhava:
					{
						switch (graha.Body)
						{
							case Body.Mars:
							case Body.Moon:
							case Body.Sun:
							case Body.Saturn:
							{
								yoga |= (1 << (int) graha.Bhava);
							}
							break;
						}
					}
					break;
				}
			}

			return yoga.NumberOfSetBits() == 4;
		}

		//Mars in 4th, the Sun in 7th and Saturn in the 10th
		//Weapon, Fire, Royal displeasure.
		public static bool RoyalDispleasure(this Grahas grahas)
		{
			if (grahas[Body.Mars].Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			if (grahas[Body.Sun].Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			if (grahas[Body.Saturn].Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Saturn in 2nd, the Moon in 4th, Mars in 10th
		public static bool WoundsOrWorms(this Grahas grahas)
		{
			if (grahas[Body.Saturn].Bhava != Bhava.DhanaBhava)
			{
				return (false);
			}

			if (grahas[Body.Moon].Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			if (grahas[Body.Mars].Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Sun in 10th, Mars in 4th
		//Fall from a Vehicle/ Injuries received from stone throwing.
		public static bool FallFromAVehicle(this Grahas grahas)
		{
			if (grahas[Body.Sun].Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			if (grahas[Body.Mars].Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Mars in 7th, the sun, Moon and Saturn in the Lagna
		public static bool Machine(this Grahas grahas)
		{
			if (grahas[Body.Mars].Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			if (grahas[Body.Sun].Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (grahas[Body.Moon].Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (grahas[Body.Saturn].Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Mars in Libra, Saturn in Aries, the Moon in Saturnine sign
		//Death in between filth and night soil.
		public static bool FilthAndNightSoil(this Grahas grahas)
		{
			if (grahas[Body.Mars].Rashi != ZodiacHouse.Lib)
			{
				return (false);
			}

			if (grahas[Body.Saturn].Rashi != ZodiacHouse.Ari)
			{
				return (false);
			}

			if (grahas[Body.Moon].Rashi.Lord != Body.Saturn)
			{
				return (false);
			}

			return (true);
		}

		//Weak Moon in 10th, the Sun in 7th and Mars in 4th
		public static bool FilthAndNightSoil2(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Strength > 0)
			{
				return (false);
			}

			if (moon.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			if (grahas[Body.Sun].Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			if (grahas[Body.Mars].Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Weak Moon aspected by strong Mars, Saturn in 8th
		//Worms\ Tumour\ Instruments\ Fire\ Disease of private parts.
		public static bool Tumour(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Strength > 0)
			{
				return (false);
			}

			var mars = grahas[Body.Mars];
			if (mars.Strength < 2)
			{
				return (false);
			}

			if (moon.IsAspectedBy(mars) == false)
			{
				return (false);
			}

			if (grahas[Body.Saturn].Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			return (true);
		}

		//The Sun in Lagna, Mars in 8th, Saturn in 5th, the Moon in 9th
		//Fall from a precipice\ Fall of thunderbolt or of a wall.
		public static bool FallOfWall(this Grahas grahas)
		{
			if (grahas[Body.Sun].Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (grahas[Body.Mars].Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			if (grahas[Body.Saturn].Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			if (grahas[Body.Moon].Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return (true);
		}

		//Mars and Saturn afflicted
		public static bool Suffocation2(this Grahas grahas)
		{
			var mars   = grahas[Body.Mars];
			var saturn = grahas [Body.Saturn];

			if (mars.Strength > 0)
			{
				return (false);
			}

			if (saturn.Strength > 0)
			{
				return (false);
			}

			return (true);
		}

		//Lagna be a watery sign or Amsa aspected by the Moon and Venus, 8th and 12th occupied by malefics
		public static bool InWater(this Grahas grahas)
		{
			var lagna = grahas[Body.Lagna];
			if (lagna.Rashi.ZodiacHouse.Element() != Element.Water)
			{
				var navamsa = grahas[DivisionType.Navamsa];
				var amsa    = navamsa [Body.Lagna];
				if (amsa.IsAspectedBy(Body.Moon) == false)
				{
					return (false);
				}

				if (amsa.IsAspectedBy(Body.Venus) == false)
				{
					return (false);
				}
			}

			foreach (var graha in grahas.Rashis[Bhava.MrtyuBhava].Grahas)
			{
				if (graha.IsFunctionalMalefic == false)
				{
					return (false);
				}
			}
			foreach (var graha in grahas.Rashis[Bhava.VyayaBhava].Grahas)
			{
				if (graha.IsFunctionalMalefic == false)
				{
					return (false);
				}
			}

			return (true);
		}

		//Birth during the Visha ghatis, 8th occupied by malefic
		//Poison/ Fire/ Weapons.
		public static bool Poison(this Grahas grahas)
		{
			var lagna = grahas[Body.Lagna];
			if (lagna.VishaGhati == false)
			{
				return (false);
			}

			var bhava8 = grahas.Rashis[Bhava.MrtyuBhava];
			if (bhava8.Grahas.Count == 0)
			{
				return (false);
			}

			foreach (var graha in bhava8.Grahas)
			{
				if (graha.IsNaturalMalefic == false)
				{
					return (false);
				}
			}

			return (true);
		}

		//The Moon is capable of inflicting death, if she is in any of the following degrees at birth:
		//Aquarius 21, Leo 5,Taurus 9, Scorpio 23, Aries 8, Cancer 12, Libra 4, Capricorn 20, Virgo 1,
		//Sagittarius 18, Pisces 10 and Gemini 22. If the Moon is in such degrees at birth,
		//death is indicated in the corresponding year and the native cannot be protected by even God Yama.


		//The Moon in the fatal degree of Lagna or 8th or 12th
		//Degree of the ascendant + degree of the 8th house - degree of the Moon <= 3degrees
		public static bool WaterOrMachinery(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Bhava == Bhava.MrtyuBhava)
			{
				if (moon.HouseOffset <= 3.0)
				{
					return (true);
				}
			}
			return (false);
		}

		//8th lord in the Navamsa which is termed as visa, and be conjoined with malefics
		//Snake poison/ by vultures/by wild boar according to the name of particular Navamsa.
		//The first Navamsa of Aries, Taurus, Virgo and Sagittarius called snake poison;
		//the middle of Gemini, Leo Libra and Aquarius is called Vulture poison
		public static bool SnakeOrBoar(this Grahas grahas)
		{
			var navamsa = grahas[DivisionType.Navamsa].Rashis;
			var lord8   = navamsa[Bhava.MrtyuBhava].Lord;

			var graha  = grahas[lord8];
			var offset = graha.Position.Longitude.ToZodiacHouseOffset();

			if (lord8.Conjunct.Count == 0)
			{
				return (false);
			}

			foreach (var conjunct in lord8.Conjunct)
			{
				if (conjunct.IsNaturalMalefic == false)
				{
					return (false);
				}
			}

			if (offset < 3.20)
			{
				switch (graha.Rashi.ZodiacHouse)
				{
					case ZodiacHouse.Ari:
					case ZodiacHouse.Tau:
					case ZodiacHouse.Vir:
					case ZodiacHouse.Sag:
						return true;
				}
			}
			else if ((offset > 14.0) && (offset < 16.0))
			{
				switch (graha.Rashi.ZodiacHouse)
				{
					case ZodiacHouse.Gem:
						case ZodiacHouse.Leo:
						case ZodiacHouse.Lib:
						case ZodiacHouse.Aqu:
							return true;
				}
			}

			return (false);
		}


		//The Sun and Mars in each other’s sign and also Kendra from the 8th lord
		//Impalement on account of royal displeasure. 
		public static bool Impalement2(this Grahas grahas)
		{
			var sun   = grahas[Body.Sun];
			var mars  = grahas[Body.Mars];
			var lord8 = grahas.Rashis[Bhava.MrtyuBhava].Lord;

			if (sun.Exchange != mars)
			{
				return (false);
			}

			if (sun.HouseFrom(lord8).IsKendra() == false)
			{
				return (false);
			}

			if (mars.HouseFrom(lord8).IsKendra() == false)
			{
				return (false);
			}

			return (true);
		}

		//[This also happens when Mars and Saturn are in each other’s sign or Amsa, in the sign
		//or Amsa of the 8th house (or in the fatal degree) and at the same time in Kendra from the 8th lord]
		public static bool Impalement3(this Grahas grahas)
		{
			var saturn   = grahas[Body.Saturn];
			var mars  = grahas[Body.Mars];
			var lord8 = grahas.Rashis[Bhava.MrtyuBhava].Lord;

			if (saturn.Exchange != mars)
			{
				return (false);
			}

			if (saturn.HouseFrom(lord8).IsKendra() == false)
			{
				return (false);
			}

			if (mars.HouseFrom(lord8).IsKendra() == false)
			{
				return (false);
			}

			return (true);
		}

		//The Moon in Lagna, weak Sun in 8th, Jupiter in 12th and a malefic in the 4th
		//Fall from the couch/Attack by hunters at night.
		public static bool FallFromCouch(this Grahas grahas)
		{
			if (grahas[Body.Moon].Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (grahas[Body.Sun].Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			if (grahas[Body.Sun].Strength >= 2)
			{
				return (false);
			}

			if (grahas[Body.Jupiter].Bhava != Bhava.VyayaBhava)
			{
				return (false);
			}

			var house4 = grahas.Rashis[Bhava.SukhaBhava];
			foreach (var graha in house4.Grahas)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//Lagna lord in the Navamsa of the 8th house, is eclipsed or in the 6th
		//Hunger in a place far away from kith and kin
		public static bool Hunger(this Grahas grahas)
		{
			var lord    = grahas[Body.Lagna].HouseLord;
			var navamsa = grahas[DivisionType.Navamsa];
			if (navamsa[lord].Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			if (lord.IsEcliped)
			{
				return (true);
			}

			if (lord.Bhava == Bhava.ShatruBhava)
			{
				return (true);
			}

			return (false);
		}

		//1st and 8th lords weak, Mars conjoined with 6th lord
		//In battle/ Weapon.
		public static bool Weapon(this Grahas grahas)
		{
			if (grahas.Rashis[Bhava.LagnaBhava].Lord.Strength >= 2)
			{
				return (false);
			}

			if (grahas.Rashis[Bhava.MrtyuBhava].Lord.Strength >= 2)
			{
				return (false);
			}

			if (grahas.Rashis[Bhava.ShatruBhava].Lord.IsConjuctWith(Body.Mars))
			{
				return (true);
			}

			return (false);
		}

		//Lagna or 7th lord conjoined with lords of 2nd and 4th
		public static bool Indigestion(this Grahas grahas)
		{
			var lord1 = grahas.Rashis[Bhava.LagnaBhava].Lord;
			var lord7 = grahas.Rashis[Bhava.JayaBhava].Lord;

			if (grahas.Rashis[Bhava.DhanaBhava].Lord.IsConjuctWith(lord1))
			{
				return (grahas.Rashis[Bhava.SukhaBhava].Lord.IsConjuctWith(lord1));
			}

			if (grahas.Rashis[Bhava.DhanaBhava].Lord.IsConjuctWith(lord7))
			{
				return (grahas.Rashis[Bhava.SukhaBhava].Lord.IsConjuctWith(lord7));
			}

			return (false);
		}

		//Lord of Navamsa of the 4th house in dusthana or conjoined with Saturn
		public static bool Poisoning2(this Grahas grahas)
		{
			var navamsa = grahas[DivisionType.Navamsa];
			var lord4   = navamsa.Rashis[Bhava.SukhaBhava].Lord;
			if (lord4.IsConjuctWith(Body.Saturn))
			{
				return (true);
			}

			if (lord4.Bhava.IsDushtana())
			{
				return (true);
			}

			return (false);
		}

		//Above lord with Rahu or Ketu
		public static bool Hanging(this Grahas grahas)
		{
			var navamsa = grahas[DivisionType.Navamsa];
			var lord4   = navamsa.Rashis[Bhava.SukhaBhava].Lord;
			if (lord4.IsConjuctWith(Body.Ketu))
			{
				return (true);
			}

			if (lord4.IsConjuctWith(Body.Rahu))
			{
				return (true);
			}

			return (false);
		}

		//Weak Moon conjoined with Mars or Saturn or Rahu in the 8th
		public static bool GhostFireWater(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Strength >= 2)
			{
				return (false);
			}

			if (moon.Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			if (moon.IsConjuctWith(Body.Mars))
			{
				return (true);
			}

			if (moon.IsConjuctWith(Body.Rahu))
			{
				return (true);
			}

			return (false);
		}

		//Weak Moon conjoined with Mars or Saturn or Rahu in other dusthana
		public static bool Epilepsy(this Grahas grahas)
		{
			var moon = grahas[Body.Moon];
			if (moon.Strength >= 2)
			{
				return (false);
			}
			if (moon.IsConjuctWith(Body.Saturn) || moon.IsConjuctWith(Body.Rahu))
			{
				return (moon.Bhava.IsDushtana());
			}

			return (false);
		}

		//Weak Sun or Mars in the 8th, Malefics in 2nd
		public static bool BiliousDisease(this Grahas grahas)
		{
			var house2 = grahas[Bhava.DhanaBhava];
			var house8 = grahas[Bhava.MrtyuBhava];

			if (house2.Count == 0)
			{
				return (false);
			}

			foreach (var graha in house8)
			{
				if ((graha.Body == Body.Sun) || (graha.Body == Body.Mars))
				{
					return (graha.Strength < 2);
				}
			}

			foreach (var graha in house2)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//The Moon or Jupiter in the 8th (Watery sign), aspected by malefic
		public static bool Consumption2(this Grahas grahas)
		{
			var house8 = grahas.Rashis[Bhava.MrtyuBhava];
			if (house8.ZodiacHouse.Element() != Element.Water)
			{
				return (false);
			}

			foreach (var graha in house8.Grahas)
			{
				if ((graha.Body == Body.Moon) || (graha.Body == Body.Jupiter))
				{
					foreach (var aspect in graha.AspectFrom)
					{
						if (aspect.IsNaturalMalefic)
						{
							return (true);
						}
					}
				}
			}

			return (false);
		}

		//Venus in 8th aspected by malefics
		//Rheumatism, Consumption or Diabetes.
		public static bool Rheumatism(this Grahas grahas)
		{
			var venus = grahas[Body.Venus];
			if (venus.Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			if (venus.AspectFrom.Count == 0)
			{
				return (false);
			}

			foreach (var graha in venus.AspectFrom)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//Rahu in 8th aspected by malefics
		//Heat blisters/ Snake bite/ Small pox/ Mental Disease
		public static bool SmallPox(this Grahas grahas)
		{
			var rahu = grahas[Body.Rahu];
			if (rahu.Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			if (rahu.AspectFrom.Count == 0)
			{
				return (false);
			}

			foreach (var graha in rahu.AspectFrom)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//8th lord aspected by Venus, the Sun or Saturn conjoined with Rahu posited
		//in a Krura Shastiyamsa
		public static bool HeadChoppedOff(this Grahas grahas)
		{
			return (false);
		}
	}

}
