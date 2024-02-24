using System;
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Util;
using Newtonsoft.Json.Linq;

namespace Mhora.Elements.Yoga
{
	public static class Yoga
	{

		//All grahas in Sthira Sign and several other grahas are also in Sthira sign.
		//One born in this yoga is proud, learned, wealthy, liked by the ruler, famous, of a stable nature and blessed with several sons.
		public static bool AashrayaMusala(this Grahas grahaList)
		{
			var grahas = grahaList.Planets;

			foreach (var graha in grahas)
			{
				if (graha.Rashi.ZodiacHouse.IsFixedSign() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All grahas in Dwiswabhava Sign and several other grahas are also in Dwiswabhava sign.
		//One born in this yoga is defective of a limb, resolute, very clever, of fluctuating wealth,
		//good to look at and fond of his near and dear ones.
		public static bool AashrayaNala(this Grahas grahaList)
		{
			var grahas = grahaList.Planets;

			foreach (var graha in grahas)
			{
				if (graha.Rashi.ZodiacHouse.IsDualSign() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//All grahas in Chara Sign and several other grahas are also in Chara sign.
		//One born in this yoga is fond of travel, of good looks, ambitious, cruel and delights to frequent alien lands in pursuit of wealth.
		public static bool AashrayaRajju(this Grahas grahaList)
		{
			var grahas = grahaList.Planets;

			foreach (var graha in grahas)
			{
				if (graha.Rashi.ZodiacHouse.IsMoveableSign() == false)
				{
					return (false);
				}
			}
			return (true);
		}

		//This yoga is caused by the presence of a natural benefic in the 10 houses from lagna or Moon.
		//A native with this yoga is revered by the ruler, blessed with physical pleasures, likeable and helpful and enjoys a lasting fame.
		public static bool AmalaKirti(this Grahas grahaList)
		{
			var grahas = grahaList.Planets;
			var moon    = grahas.Find(graha => graha.Body == Body.Moon);

			foreach (var graha in grahas)
			{
				if (graha.IsNaturalBenefic)
				{
					if (graha.Bhava == Bhava.KarmaBhava)
					{
						return (true);
					}

					if (graha.Bhava.HousesFrom(moon.Bhava) == 10)
					{
						return (true);
					}
				}
			}

			return (false);
		}

		//All Benefic in kendras
		//The person will achieve the ownership of vast lands. The person will gain abundant wealth.
		public static bool AmaraYogaBenefics(this Grahas grahaList)
		{
			var grahas = grahaList.Planets;

			bool positiveYoga = true;

			foreach (var graha in grahas)
			{
				if (graha.IsNaturalBenefic)
				{
					positiveYoga &= graha.Bhava.IsKendra();
				}
			}

			return (positiveYoga);

		}

		//All malefic in kendras.
		//The person will achieve the ownership of vast lands. The person will gain abundant wealth.
		public static bool AmaraYogaMalefics (this Grahas grahaList)
		{
			var grahas = grahaList.Planets;

			bool negativeYoga = true;

			foreach (var graha in grahas)
			{
				if (graha.IsNaturalMalefic)
				{
					negativeYoga &= graha.Bhava.IsKendra();
				}
			}

			return (negativeYoga);
		}

		//The lord of the 9 being strong AND the lagna lord as also Jupiter and Venus occupy kendras. OR
		//all planets occupy the lagna, the 2, 7 and 12th houses only
		//One born in this yoga is of a royal bearing, of noble birth, well- behaved, well with wife, sons and fame, bereft of disease.
		public static bool Bheri(this Grahas grahaList)
		{
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if (lord9.IsStrong)
			{
				return (true);
			}

			bool yoga= true;
			foreach (var g in grahaList.Planets)
			{
				if ((g.Bhava != Bhava.DhanaBhava) && (g.Bhava != Bhava.JayaBhava) && (g.Bhava != Bhava.VyayaBhava))
				{
					yoga = false;
					break;
				}
			}

			if (yoga)
			{
				return (true);
			}


			var lord1 = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			if (lord1.Bhava.IsKendra() == false)
			{
				return (false);
			}

			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava.IsKendra() == false)
			{
				return (false);
			}

			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava.IsKendra() == false)
			{
				return (false);
			}

			return (false);
		}


		//Exalted lagna lord occupying a kendra and aspected by Jupiter.
		//The Chaamara yoga confers on the native kingship or honor through a king, eloquence,
		//wisdom, knowledge of several subjects and a longevity of 71 years.
		public static bool Chaamara1(this Grahas grahaList)
		{
			var lagnaLord = grahaList.Find(Body.Lagna).HouseLord;
			if (lagnaLord.IsExalted == false)
			{
				return (false);
			}

			if (lagnaLord.Bhava.IsKendra() == false)
			{
				return (false);
			}

			return lagnaLord.IsAspectedBy(Body.Jupiter);
		}

		//Two benefices conjoined either in the Lagna, or in the 7 house or in the 9th or the 10 th house.
		//The Chaamara yoga confers on the native kingship or honor through a king, eloquence, wisdom,
		//knowledge of several subjects and a longevity of 71 years.
		public static bool Chaamara2(this Grahas grahaList)
		{
			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if (graha.Conjunct.Count > 0)
					{
						foreach (var conj in graha.Conjunct)
						{
							if (conj.IsNaturalBenefic == false)
							{
								return (false);
							}
						}

						switch (graha.Bhava)
						{
							case Bhava.LagnaBhava:
							case Bhava.JayaBhava:
							case Bhava.DhanaBhava:
							case Bhava.KarmaBhava:
								return true;
						}
					}
				}
			}

			return (false);
		}

		//All planets occupy the four Kendras
		//This Yoga destroys numerous afflictions in a chart and ensures wealth and high status to the person.
		public static bool ChatussagaraKendra(this Grahas grahaList)
		{
			byte kendra = 0;
			foreach (var graha in grahaList.Planets)
			{
				if (graha.Bhava.IsKendra() == false)
				{
					return (false);
				}

				if (graha.Bhava == Bhava.LagnaBhava)
				{
					kendra |= 0x01;
				}
				else if (graha.Bhava == Bhava.SukhaBhava)
				{
					kendra |= 0x02;
				}
				else if (graha.Bhava == Bhava.JayaBhava)
				{
					kendra |= 0x04;
				}
				else
				{
					kendra |= 0x08;
				}

			}
			return (kendra == 0x0F);
		}

		//All the planets occupy the four Chara signs
		//This Yoga destroys numerous afflictions in a chart and ensures wealth and high status to the person.
		public static bool ChatussagaraChara(this Grahas grahaList)
		{
			byte chara = 0;

			foreach (var graha in grahaList.Planets)
			{
				if (graha.Rashi.ZodiacHouse.IsMoveableSign() == false)
				{
					return (false);
				}

				if (graha.Rashi.ZodiacHouse == ZodiacHouse.Ari)
				{
					chara |= 0x01;
				}
				else if (graha.Rashi.ZodiacHouse == ZodiacHouse.Can)
				{
					chara |= 0x02;
				}
				else if (graha.Rashi.ZodiacHouse == ZodiacHouse.Lib)
				{
					chara |= 0x04;
				}
				else
				{
					chara |= 0x08;
				}
			}
			return (chara == 0x0F);
		}

		//All benefices in three kendras, Functional malefics not in kendras.
		//One born in this yoga is blessed with constant enjoyments, vehicles, good food and clothes and association with lovely women.
		public static bool DalaMala(this Grahas grahaList)
		{
			int yoga    = 0;
			byte kendra = 0;

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsFunctionalMalefic)
				{
					if (graha.Bhava.IsKendra())
					{
						return (false);
					}
				}
				else if (graha.IsNaturalBenefic)
				{
					if (graha.Bhava.IsKendra() == false)
					{
						return (false);
					}

					if (graha.Bhava == Bhava.LagnaBhava)
					{
						if ((kendra & 0x01) == 0)
						{
							kendra |= 0x01;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else if (graha.Bhava == Bhava.SukhaBhava)
					{
						if ((kendra & 0x02) == 0)
						{
							kendra |= 0x02;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else if (graha.Bhava == Bhava.JayaBhava)
					{
						if ((kendra & 0x04) == 0)
						{
							kendra |= 0x04;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else
					{
						if ((kendra & 0x08) == 0)
						{
							kendra |= 0x08;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
				}
			}

			return (yoga == 3);
		}

		//All Functional Malefic in three kendras, Benefic not in kendras
		//One born in this yoga is scheming, wicked, miserable, destitute, and dependent upon others for his subsistence.
		public static bool DalaSarpa(this Grahas grahaList)
		{
			int  yoga   = 0;
			byte kendra = 0;

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if (graha.Bhava.IsKendra())
					{
						return (false);
					}
				}
				else if (graha.IsFunctionalMalefic)
				{
					if (graha.Bhava.IsKendra() == false)
					{
						return (false);
					}

					if (graha.Bhava == Bhava.LagnaBhava)
					{
						if ((kendra & 0x01) == 0)
						{
							kendra |= 0x01;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else if (graha.Bhava == Bhava.SukhaBhava)
					{
						if ((kendra & 0x02) == 0)
						{
							kendra |= 0x02;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else if (graha.Bhava == Bhava.JayaBhava)
					{
						if ((kendra & 0x04) == 0)
						{
							kendra |= 0x04;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else
					{
						if ((kendra & 0x08) == 0)
						{
							kendra |= 0x08;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
				}
			}

			return (yoga == 3);

		}

		//All benefices in three kendras malefics not in kendras.
		//One born in this yoga is blessed with constant enjoyments, vehicles, good food and clothes and association with lovely women.
		public static bool DalaMaala(this Grahas grahaList)
		{
			int  yoga   = 0;
			byte kendra = 0;

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if (graha.Bhava.IsKendra() == false)
					{
						return (false);
					}

					if (graha.Bhava == Bhava.LagnaBhava)
					{
						if ((kendra & 0x01) == 0)
						{
							kendra |= 0x01;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else if (graha.Bhava == Bhava.SukhaBhava)
					{
						if ((kendra & 0x02) == 0)
						{
							kendra |= 0x02;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else if (graha.Bhava == Bhava.JayaBhava)
					{
						if ((kendra & 0x04) == 0)
						{
							kendra |= 0x04;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
					else
					{
						if ((kendra & 0x08) == 0)
						{
							kendra |= 0x08;
							yoga++;
						}
						else
						{
							return (false);
						}
					}
				}
				else
				{
					if (graha.Bhava.IsKendra())
					{
						return (false);
					}
				}
			}

			return (yoga == 3);

		}

		//Dhana ($$$) Yogas are formed when ANY of the two Lords of Houses1, 2, 5, 9 or 11
		//are in Association (in a house together) or in Mutual Aspect
		//(especially if these Lords are First Tier Strength or “wellplaced”
		public static bool Dhana(this Grahas grahaList)
		{
			foreach (var rashi in grahaList.Rashis)
			{
				if (rashi.Bhava.IsDhana())
				{
					foreach (var graha in rashi.Lord.MutualAspect)
					{
						if (graha.Bhava.IsDhana())
						{
							return true;
						}
					}
					foreach (var graha in rashi.Lord.Association)
					{
						if (graha.Bhava.IsDhana())
						{
							return true;
						}
					}

				}
			}

			return false;
		}

		//If the native born in Night time and the Moon located in its own navamsha, or that of a friend's house and aspected by Venus.
		//The native is very wealthy. According to one interpretation, irrespective of the birth being during daytime or night-time,
		//the placement of the Moon in a favorable navamsha, under the aspect of Jupiter or Venus or both, is a combination of great wealth.
		public static bool DhanadhanaNight(this Horoscope h, Grahas grahaList)
		{
			if (h.IsDayBirth())
			{
				return (false);
			}

			var navamsha = Grahas.Find(h, DivisionType.Navamsa);
			var moon     = navamsha.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			moon = grahaList.Find(Body.Moon);
			if (moon.IsInOwnHouse)
			{
				return (true);
			}

			if (moon.FriendlySign)
			{
				return (true);
			}

			if (moon.IsAspectedBy(Body.Venus))
			{
				return (true);
			}

			return (false);
		}

		//If the native born in Day time and the Moon located in its own navamsha, or that of a friend's house and aspected by Jupiter.
		//The native is very wealthy. According to one interpretation, irrespective of the birth being during daytime or night-time,
		//the placement of the Moon in a favorable navamsha, under the aspect of Jupiter or Venus or both, is a combination of great wealth.
		public static bool DhanadhanaDay(this Horoscope h, Grahas grahaList)
		{
			if (h.IsDayBirth() == false)
			{
				return (false);
			}

			var navamsha = Grahas.Find(h, DivisionType.Navamsa);
			var moon     = navamsha.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			moon = grahaList.Find(Body.Moon);
			if (moon.IsInOwnHouse)
			{
				return (true);
			}

			if (moon.FriendlySign)
			{
				return (true);
			}

			if (moon.IsAspectedBy(Body.Jupiter))
			{
				return (true);
			}

			return (false);
		}

		//All Malefics in the 8th house and All Benefic in the Lagna.
		//The person is a commander one whose orders others follow.
		public static bool Dhwaja(this Grahas grahaList)
		{
			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if (graha.Bhava != Bhava.LagnaBhava)
					{
						return (false);
					}
				}
				else if (graha.IsNaturalMalefic)
				{
					if (graha.Bhava != Bhava.MrtyuBhava)
					{
						return (false);
					}
				}
			}
			return (true);
		}

		//All planets in the Lagna, 5th, 7th and in the 9th house.
		//The native takes over the sustenance of his fellow beings.
		public static bool Hamsa(this Grahas grahaList)
		{
			foreach (var graha in grahaList.Planets)
			{
				if ((graha.Bhava != Bhava.PutraBhava) && 
				    (graha.Bhava != Bhava.JayaBhava) && 
				    (graha.Bhava != Bhava.DharmaBhava))
				{
					return (false);
				}
			}
			return (true);
		}

		//This yoga arises when the Moon occupies the 11th house from the lagna.
		//The native suffers death as a consequence of some stupidly egotistical action of his.
		public static bool HathaHantaa(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			return (moon.Bhava == Bhava.LabhaBhava);
		}

		public static int HousesFrom(this Grahas grahaList, Body body, Body from)
		{
			var graha  = grahaList.Find(body);
			var graha2 = grahaList.Find(from);
			return (graha.Bhava.HousesFrom(graha2.Bhava));
		}

		//Mars is 3rd from Moon, Saturn is 7th from Mars, Venus is 7th from Saturn Jupiter is 7th from Venus.
		//The person is celebrated a king or his equal amiable eloquent wealthy and fortunate.
		public static bool Indra(this Grahas grahaList)
		{
			if (grahaList.HousesFrom(Body.Mars, Body.Moon) != 7)
			{
				return (false);
			}

			if (grahaList.HousesFrom(Body.Saturn, Body.Mars) != 7)
			{
				return (false);
			}

			if (grahaList.HousesFrom(Body.Venus, Body.Saturn) != 7)
			{
				return (false);
			}

			if (grahaList.HousesFrom(Body.Jupiter, Body.Venus) != 7)
			{
				return (false);
			}

			return (true);
		}

		//The 4 lord exalted or in its own house, associated with or aspected by the 10 lord.
		//The native with this yoga is aggressive, courageous, ignorant, commander of an army, owner (ruler) of several villages.
		public static bool Kaahala1(this Grahas grahaList)
		{
			var lord4 = grahaList.Rashis.Find(Bhava.SukhaBhava).Lord;
			if (lord4.IsExalted == false)
			{
				return (false);
			}

			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;
			if (lord4.IsAssociatedWith(lord10))
			{
				return (true);
			}

			if (lord4.IsAspectedBy(lord10))
			{
				return (true);
			}
			return (false);
		}

		//Lords of the 4 and 9 in mutual kendras and lord of the lagna are being strong.
		//The native with this yoga is aggressive, courageous, ignorant, commander of an army, owner (ruler) of several villages.
		public static bool Kaahala2(this Grahas grahaList)
		{
			var lord1 = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			var lord4 = grahaList.Rashis.Find(Bhava.SukhaBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;

			if (lord4.Bhava.IsKendra() == false)
			{
				return (false);
			}

			if (lord9.Bhava.IsKendra() == false)
			{
				return (false);
			}

			if (lord4.Exchange != lord9)
			{
				return (false);
			}

			if (lord1.Strength < 2)
			{
				return (false);
			}

			return (true);
		}

		//All planets in the 7th, 10th and 11th houses.
		//The person even if born in an ordinary ambiance
		public static bool Karika(this Grahas grahaList)
		{
			foreach (var graha in grahaList.Planets)
			{
				if ((graha.Bhava != Bhava.JayaBhava) &&
					(graha.Bhava !=Bhava.KarmaBhava) &&
					(graha.Bhava != Bhava.LabhaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//When natural malefics occupy houses 2 and 12 from the lagna.
		//This yoga produces criminal tendencies, ill health, unwholesome food, excessive sexual urge, intention to grab the wealth of others.
		public static bool PaapaKartari(this Grahas grahaList)
		{
			bool yoga  = false;
			var  rashi = grahaList.Rashis.Find(Bhava.DhanaBhava);
			foreach (var graha in rashi.Grahas)
			{
				if (graha.IsNaturalBenefic)
				{
					return (false);
				}
				if (graha.IsNaturalMalefic)
				{
					yoga = true;
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			yoga  = false;
			rashi = grahaList.Rashis.Find(Bhava.VyayaBhava);
			foreach (var graha in rashi.Grahas)
			{
				if (graha.IsNaturalBenefic)
				{
					return (false);
				}
				if (graha.IsNaturalMalefic)
				{
					yoga = true;
				}
			}

			return (yoga);
		}

		//When natural benefices occupy houses 2 and 12 from the lagna.
		//This yoga confers on the native eloquence, good health, handsome appearance, much wealth and fame.
		public static bool ShubhaKartari(this Grahas grahaList)
		{
			bool yoga  = false;
			var  rashi = grahaList.Rashis.Find(Bhava.DhanaBhava);
			foreach (var graha in rashi.Grahas)
			{
				if (graha.IsNaturalMalefic)
				{
					return (false);
				}
				if (graha.IsNaturalBenefic)
				{
					yoga = true;
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			yoga  = false;
			rashi = grahaList.Rashis.Find(Bhava.VyayaBhava);
			foreach (var graha in rashi.Grahas)
			{
				if (graha.IsNaturalMalefic)
				{
					return (false);
				}
				if (graha.IsNaturalBenefic)
				{
					yoga = true;
				}
			}

			return (yoga);

		}

		//Occupation of the 2 house by the 9 lord, and of the 9th house by the 2nd lord, and lagna lord occupying a kendra or a trikona house.
		//The native born in this yoga is immersed in the study of sacred scriptures, dignified, skillful, wealthy, blessed with wisdom, kindness
		public static bool Khadga(this Grahas grahaList)
		{
			var lord2 = grahaList.Rashis.Find(Bhava.DhanaBhava).Lord;
			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;
			if (lord2.Rashi.ZodiacHouse.LordOfSign() != lord9)
			{
				return (false);
			}

			if (lord9.Rashi.ZodiacHouse.LordOfSign() != lord2)
			{
				return (false);
			}

			var lord1 = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			if (lord1.Bhava.IsKendra() || lord1.Bhava.IsTrikona())
			{
				return (true);
			}

			return (false);
		}

		//When benefices occupy houses 7 and 8 from the lagna.
		//One born in this yoga is learned and blessed with comforts.
		public static bool Lagnadhi(this Grahas grahaList)
		{
			byte yoga = 0x00;
			foreach (var graha in grahaList.Planets)
			{
				if (graha.Bhava == Bhava.DhanaBhava)
				{
					if (graha.IsNaturalBenefic)
					{
						yoga |= 0x01;
					}
					else
					{
						return (false);
					}

				}
				else if (graha.Bhava == Bhava.MrtyuBhava)
				{
					if (graha.IsNaturalBenefic)
					{
						yoga |= 0x02;
					}
					else
					{
						return (false);
					}
				}
				else
				{
					continue;
				}
			}
			return (yoga == 0x03);
		}


		//when benefices occupy houses 7th & 8th from the Lagna, bereft of malefic association or aspect.
		//One born in this yoga is learned and blessed with comforts.
		public static bool Lagnadhi1(this Grahas grahaList)
		{
			if (grahaList.Lagnadhi() == false)
			{
				return (false);
			}

			foreach (var graha in grahaList.Planets)
			{
				if ((graha.Bhava == Bhava.DhanaBhava) || (graha.Bhava == Bhava.MrtyuBhava))
				{
					if (graha.IsNaturalBenefic)
					{
						if (graha.IsAspectedByMalefics)
						{
							return (false);
						}

						if (graha.IsAssociatedWithMalefics)
						{
							return (false);
						}
					}
				}
			}
			return (true);
		}

		//It results when the lagna lord is abundantly strong and the lord of 9th house occupies a kendra
		//identical with his own house, his sign of exaltation, or his mooltrikona.
		//One born in this, is good in looks, virtuous, very wealthy, owns vast lands, learned,
		//an illustrious king, widely renowned, blessed with wives and children.
		public static bool Lakshmi(this Grahas grahaList)
		{
			var lord1 = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			if (lord1.Strength < 2)
			{
				return false;
			}

			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;
			if (lord9.Bhava.IsKendra())
			{
				return (true);
			}

			if (lord9.IsInOwnHouse)
			{
				return (true);
			}

			if (lord9.IsMoolTrikona)
			{
				return (true);
			}

			return (false);
		}

		//Birth during nighttime, Even Sign rising in lagna, Sun & Moon occupies even sign.
		//The native is blessed with all feminine qualities, grace good fortune, good character, wealth and progeny.
		public static bool MahaBhagyaNight(this Horoscope h, Grahas grahaList)
		{
			if (h.IsDayBirth())
			{
				return (false);
			}

			var lagna = grahaList.Find(Body.Lagna);
			if ((lagna.Rashi.ZodiacHouse.Index() % 2) != 0)
			{
				return (false);
			}
			var sun = grahaList.Find(Body.Sun);
			if ((sun.Rashi.ZodiacHouse.Index() % 2) != 0)
			{
				return (false);
			}
			var moon = grahaList.Find(Body.Moon);
			if ((moon.Rashi.ZodiacHouse.Index() % 2) != 0)
			{
				return (false);
			}

			return (true);
		}

		//Birth during the daytime, Odd sign rising in the lagna, Sun & Moon occupying odd sign.
		//On born in this, is pleasant to look at, liberal, widely renowned, lord of lands.
		public static bool MahaBhagyaDay(this Horoscope h, Grahas grahaList)
		{
			if (h.IsDayBirth() == false)
			{
				return (false);
			}

			var lagna = grahaList.Find(Body.Lagna);
			if ((lagna.Rashi.ZodiacHouse.Index() % 2) == 0)
			{
				return (false);
			}
			var sun = grahaList.Find(Body.Sun);
			if ((sun.Rashi.ZodiacHouse.Index() % 2) == 0)
			{
				return (false);
			}
			var moon = grahaList.Find(Body.Moon);
			if ((moon.Rashi.ZodiacHouse.Index() % 2) == 0)
			{
				return (false);
			}

			return (true);
		}

		//Moon conjoined with Rahu, aspected by Jupiter 'which itself is under melefic association.
		//The person is a heinous sinner.
		public static bool MahaPataka(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.IsConjuctWith(Body.Rahu) == false)
			{
				return (false);
			}

			if (moon.IsAspectedBy(Body.Jupiter) == false)
			{
				return (false);
			}

			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.IsAssociatedWithMalefics)
			{
				return (true);
			}

			return (false);
		}

		//Benefic Planets occupying the kendras and 6th & 8th house either vacant or occupied by benefic only.
		//One born with either of the above combinations in the horoscope is renowned, illustrious, fortunate,
		//wealthy, an orator, charitable, learned.
		public static bool ParvataKendra(this Grahas grahaList)
		{
			int yoga = 0;
			foreach (var rashi in grahaList.Rashis)
			{
				switch (rashi.Bhava)
				{
					case Bhava.ShatruBhava:
					case Bhava.MrtyuBhava:
					{
						if (rashi.Grahas.Count > 0)
						{
							return (false);
						}
					}
					break;

					case Bhava.LagnaBhava:
					case Bhava.SukhaBhava:
					case Bhava.JayaBhava:
					case Bhava.KarmaBhava:
					{
						if (rashi.Grahas.Count > 0)
						{
							foreach (var graha in rashi.Grahas)
							{
								if (graha.Body == Body.Lagna)
								{
									continue;
								}

								if (graha.IsNaturalMalefic || graha.IsFunctionalMalefic)
								{
									return (false);
								}

								if (graha.IsNaturalBenefic)
								{
									yoga |= (1 << graha.Bhava.Index());
								}
							}
						}
					}
					break;
				}
			}

			return (yoga.NumberOfSetBits () >= 3);
		}

		//The lord of the Lagna and that of the 12th lord placed in mutual kendras, and aspected by benefices.
		//One born with either of the above combinations in the horoscope is renowned, illustrious, fortunate,
		//wealthy, an orator, charitable, learned, very lustful.
		public static bool ParvataLagna(this Grahas grahaList)
		{
			var lord1  = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			var lord12 = grahaList.Rashis.Find(Bhava.VyayaBhava).Lord;

			if (lord1.Bhava.IsKendra() == false)
			{
				return (false);
			}

			if (lord12.Bhava.IsKendra() == false)
			{
				return (false);
			}

			if (lord1.Exchange != lord12)
			{
				return (false);
			}

			if (lord1.Bhava.HousesFrom(lord12.Bhava) != 7)
			{
				return (false);
			}

			if (lord1.IsAspectedByBenefics == false)
			{
				return (false);
			}

			if (lord12.IsAspectedByBenefics == false)
			{
				return (false);
			}

			return (true);
		}

		//Mercury, Jupiter and Venus are in the Kendras or Trikonas or in the 2nd house,
		//and Jupiter is strong occupying own sign friends sign or exalted.
		//The person is highly learned, scholarly very well versed in prose and poetry
		//as also in sacred scriptures and higher mathematics.
		public static bool Saraswati(this Grahas grahaList)
		{
			var grahas = new List<Graha>()
			{
				grahaList.Find(Body.Mercury),
				grahaList.Find(Body.Jupiter),
				grahaList.Find(Body.Venus),
			};

			foreach (var graha in grahas)
			{
				if ((graha.Bhava.IsKendra() == false) && 
				    (graha.Bhava.IsTrikona() == false) && 
				    (graha.Bhava != Bhava.DhanaBhava))
				{
					return (false);
				}
			}

			var jupiter = grahaList.Find(Body.Jupiter);
			if ((jupiter.IsInOwnHouse) || (jupiter.IsExalted) || (jupiter.FriendlySign))
			{
				return (true);
			}
			return (false);
		}

		//When Jupiter, located in a house other than a kendra, occupies the 6, 8, 12th house from moon.
		//It produces a native who is destitute, indigent, ever toiling, disliked by all.
		public static bool Shakata(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava.IsKendra())
			{
				return (false);
			}
			switch (jupiter.HouseFrom(Body.Moon))
			{
				case Bhava.ShatruBhava:
				case Bhava.MrtyuBhava:
				case Bhava.VyayaBhava:
					return (true);
			}

			return (false);
		}

		//The 5 lord and 6 lord in mutual kendras and the lagna lord is strong OR
		//The lord of the lagna as well as that of the 10 house occupy a Chara(movable) sign and 9 lord is strong
		//One born in the Shankha yoga is kind-hearted, virtuous, learned, blessed with wife and children,
		//morally sound, owns lands, lives long(upto 81)
		public static bool Shankha(this Grahas grahaList)
		{
			var lagna = grahaList.Find(Body.Lagna);
			var lord5 = grahaList.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord6 = grahaList.Rashis.Find(Bhava.ShatruBhava).Lord;

			if (lord5.Bhava.IsKendra() && lord6.Bhava.IsKendra())
			{
				if (lord5.Exchange == lord6)
				{
					return (lagna.HouseLord.Strength >= 2);
				}
			}

			var lord1  = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;
			var lord9  = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;
			var lord10 = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;

			if (lord1.Rashi.ZodiacHouse.IsMoveableSign() == false)
			{
				return (false);
			}
			if (lord10.Rashi.ZodiacHouse.IsMoveableSign() == false)
			{
				return (false);
			}

			return (lord9.Strength >= 2);
		}

		//Exalted lord of the 7th occupying the 10th house and the 9th lord also in the 10th house.
		//The person is glorious like Indra the king of gods.
		public static bool Shrinatha(this Grahas grahaList)
		{
			var lord7 = grahaList.Rashis.Find(Bhava.JayaBhava).Lord;
			if (lord7.IsExalted == false)
			{
				return (false);
			}

			if (lord7.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			var lord9 = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;
			if (lord9.Bhava == Bhava.KarmaBhava)
			{
				return (true);
			}

			return (false);
		}

		//All planets occupying houses 2, 6, 8 and 12.
		//The person acquires the ‘Simhasana’ or the royal throne.
		public static bool Simhasana(this Grahas grahaList)
		{
			foreach (var graha in grahaList.Planets)
			{
				switch (graha.Bhava)
				{
					case Bhava.DhanaBhava:
					case Bhava.ShatruBhava:
					case Bhava.MrtyuBhava:
					case Bhava.VyayaBhava: 
						break;
					default:
						return (false);
				}
			}

			return (true);
		}

		//Location of the Moon in a kendra(1, 4, 7, 10) from the sun.
		//This produces ordinary wealth, learning, efficiency and fame of native.
		public static bool AlpaUttamadi(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			return (moon.HouseFrom(Body.Sun).IsKendra());
		}

		//Location of the Moon in a Panaphara ( 2, 5, 8, 11) from the Sun.
		//This produces medium wealth, learning, efficiency and fame of native.
		public static bool MadhyaUttamadi(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			return (moon.HouseFrom(Body.Sun).IsPanaphara());
		}

		//Location of the Moon in a Apoklima house ( 3, 6, 9, 12 ) from the Sun.
		//The wealth, learning, efficiency and fame of the native are plenteous.
		// The manasagari ascribes : Many sons, trouble from daughters.
		public static bool UttamaUttamadi(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			return (moon.HouseFrom(Body.Sun).IsApoklima());
		}

		//It is produced When all the benefices occupy the Upachaya houses (3, 6, 10, 11) from the Moon.
		//This yoga produces an individual who is extremely wealthy and enjoys comforts while staying at home.
		public static bool Vasumna(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			return (moon.HouseFrom(Body.Sun).IsUpachay());
		}

		//The 6th lord is in the 6th, 8th or 12th house.
		//This is supposed to confer happiness health and fame. The person will conquer his/her enemies
		//and will hesitate in indulging in sinful deeds. Friends will be illustrious and with class.
		public static bool ViparitaHarshaRaja(this Grahas grahaList)
		{
			var lord6 = grahaList.Rashis.Find(Bhava.ShatruBhava).Lord;
			return lord6.Bhava.IsDushtana();
		}

		//The 8th lord is in the 6th, 8th or 12th house.
		//This confers learning longevity and prosperity. The person will be successful
		//in all ventures conqueror of foes and a great celebrity.
		public static bool ViparitaSaralaRaja(this Grahas grahaList)
		{
			var lord8 = grahaList.Rashis.Find(Bhava.MrtyuBhava).Lord;
			return lord8.Bhava.IsDushtana();
		}

		//The 12th lord is in the 6th, 8th or 12th house.
		//This makes the person virtuous and contented. The person will be equipped with good behavior
		//towards others will enjoy happiness will be independent following a respectable profession or
		//conduct and will be known for good qualities.
		public static bool ViparitaVimalaRaja(this Grahas grahaList)
		{
			var lord12 = grahaList.Rashis.Find(Bhava.VyayaBhava).Lord;
			return lord12.Bhava.IsDushtana();
		}
	}
}
