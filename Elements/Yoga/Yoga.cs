using System;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public static class Yoga
	{

		//All grahas in Sthira Sign and several other grahas are also in Sthira sign.
		//One born in this yoga is proud, learned, wealthy, liked by the ruler, famous, of a stable nature and blessed with several sons.
		public static bool AashrayaMusala(this DivisionType varga)
		{
			var grahas = Graha.Planets(varga);

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
		public static bool AashrayaNala(this DivisionType varga)
		{
			var grahas = Graha.Planets(varga);

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
		public static bool AashrayaRajju(this DivisionType varga)
		{
			var grahas = Graha.Planets(varga);

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
		public static bool AmalaKirti(this DivisionType varga)
		{
			var grahas = Graha.Planets(varga);
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
		public static bool AmaraYogaBenefics(this DivisionType varga)
		{
			var grahas = Graha.Planets(varga);

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
		public static bool AmaraYogaMalefics (this DivisionType varga)
		{
			var grahas = Graha.Planets(varga);

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
		public static bool Bheri(this DivisionType varga)
		{
			var ninthRashi = Rashi.Find(Bhava.DharmaBhava, varga);
			var ninthLord  = ninthRashi.ZodiacHouse.LordOfSign();
			var graha = Graha.Find(ninthLord, varga);

			if (graha.IsStrong)
			{
				return (true);
			}

			bool yoga= true;
			foreach (var g in Graha.Planets(varga))
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


			var lagnaRashi = Rashi.Find(Bhava.LagnaBhava, varga);
			var lagnaLord  = lagnaRashi.ZodiacHouse.LordOfSign();
			graha      = Graha.Find(lagnaLord, varga);
			if (graha.Rashi.Bhava.IsKendra() == false)
			{
				return (false);
			}

			graha = Graha.Find(Body.Jupiter, varga);
			if (graha.Rashi.Bhava.IsKendra() == false)
			{
				return (false);
			}

			graha = Graha.Find(Body.Venus, varga);
			if (graha.Rashi.Bhava.IsKendra() == false)
			{
				return (false);
			}

			return (false);
		}


		//Exalted lagna lord occupying a kendra and aspected by Jupiter.
		//The Chaamara yoga confers on the native kingship or honor through a king, eloquence,
		//wisdom, knowledge of several subjects and a longevity of 71 years.
		public static bool Chaamara1(this DivisionType varga)
		{
			var lagnaLord = Graha.Find(Body.Lagna, varga).HouseLord;
			if (lagnaLord.IsExalted == false)
			{
				return (false);
			}

			if (lagnaLord.Bhava.IsKendra() == false)
			{
				return (false);
			}

			if (lagnaLord.AspectFrom.Count > 0)
			{
				foreach (var graha in lagnaLord.AspectFrom)
				{
					if (graha.Body == Body.Jupiter)
					{
						return (true);
					}
				}
			}
			return (false);
		}

		//Two benefices conjoined either in the Lagna, or in the 7 house or in the 9th or the 10 th house.
		//The Chaamara yoga confers on the native kingship or honor through a king, eloquence, wisdom,
		//knowledge of several subjects and a longevity of 71 years.
		public static bool Chaamara2(this DivisionType varga)
		{
			foreach (var graha in Graha.Planets(varga))
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
		public static bool ChatussagaraKendra(this DivisionType varga)
		{
			byte kendra = 0;
			foreach (var graha in Graha.Planets(varga))
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
		public static bool ChatussagaraChara(this DivisionType varga)
		{
			byte chara = 0;

			foreach (var graha in Graha.Planets(varga))
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
		public static bool DalaMala(this DivisionType varga)
		{
			int yoga    = 0;
			byte kendra = 0;

			foreach (var graha in Graha.Planets(varga))
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
		public static bool DalaSarpa(this DivisionType varga)
		{
			int  yoga   = 0;
			byte kendra = 0;

			foreach (var graha in Graha.Planets(varga))
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
		public static bool DalaMaala(this DivisionType varga)
		{
			int  yoga   = 0;
			byte kendra = 0;

			foreach (var graha in Graha.Planets(varga))
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

		//If the native born in Night time and the Moon located in its own navamsha, or that of a friend's house and aspected by Venus.
		//The native is very wealthy. According to one interpretation, irrespective of the birth being during daytime or night-time,
		//the placement of the Moon in a favorable navamsha, under the aspect of Jupiter or Venus or both, is a combination of great wealth.
		public static bool DhanadhanaNight(this Horoscope h, DivisionType varga)
		{
			if (h.IsDayBirth())
			{
				return (false);
			}

			var moon = Graha.Find(Body.Moon, DivisionType.Navamsa);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			moon = Graha.Find(Body.Moon, varga);
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
		public static bool DhanadhanaDay(this Horoscope h, DivisionType varga)
		{
			if (h.IsDayBirth() == false)
			{
				return (false);
			}

			var moon = Graha.Find(Body.Moon, DivisionType.Navamsa);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			moon = Graha.Find(Body.Moon, varga);
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
		public static bool Dhwaja(this DivisionType varga)
		{
			foreach (var graha in Graha.Planets(varga))
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
		public static bool Hamsa(this DivisionType varga)
		{
			foreach (var graha in Graha.Planets(varga))
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
		public static bool HathaHantaa(this DivisionType varga)
		{
			var moon = Graha.Find(Body.Moon, varga);
			return (moon.Bhava == Bhava.LabhaBhava);
		}

		public static int HousesFrom(this DivisionType varga, Body body, Body from)
		{
			var graha  = Graha.Find(body, varga);
			var graha2 = Graha.Find(from, varga);
			return (graha.Bhava.HousesFrom(graha2.Bhava));
		}

		//Mars is 3rd from Moon, Saturn is 7th from Mars, Venus is 7th from Saturn Jupiter is 7th from Venus.
		//The person is celebrated a king or his equal amiable eloquent wealthy and fortunate.
		public static bool Indra(this DivisionType varga)
		{
			if (varga.HousesFrom(Body.Mars, Body.Moon) != 7)
			{
				return (false);
			}

			if (varga.HousesFrom(Body.Saturn, Body.Mars) != 7)
			{
				return (false);
			}

			if (varga.HousesFrom(Body.Venus, Body.Saturn) != 7)
			{
				return (false);
			}

			if (varga.HousesFrom(Body.Jupiter, Body.Venus) != 7)
			{
				return (false);
			}

			return (true);
		}

		//The 4 lord exalted or in its own house, associated with or aspected by the 10 lord.
		//The native with this yoga is aggressive, courageous, ignorant, commander of an army, owner (ruler) of several villages.
		public static bool Kaahala1(this DivisionType varga)
		{
			var lord4 = Rashi.Find(Bhava.SukhaBhava, varga).Lord;
			if (lord4.IsExalted == false)
			{
				return (false);
			}

			var lord10 = Rashi.Find(Bhava.KarmaBhava, varga).Lord;
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
		public static bool Kaahala2(this DivisionType varga)
		{
			var lord1 = Rashi.Find(Bhava.LagnaBhava, varga).Lord;
			var lord4 = Rashi.Find(Bhava.SukhaBhava, varga).Lord;
			var lord9 = Rashi.Find(Bhava.DharmaBhava, varga).Lord;

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
		public static bool Karika(this DivisionType varga)
		{
			foreach (var graha in Graha.Planets(varga))
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
		public static bool PaapaKartari(this DivisionType varga)
		{
			bool yoga  = false;
			var  rashi = Rashi.Find(Bhava.DhanaBhava, varga);
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
			rashi = Rashi.Find(Bhava.VyayaBhava, varga);
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
		public static bool ShubhaKartari(this DivisionType varga)
		{
			bool yoga  = false;
			var  rashi = Rashi.Find(Bhava.DhanaBhava, varga);
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
			rashi = Rashi.Find(Bhava.VyayaBhava, varga);
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
		public static bool Khadga(this DivisionType varga)
		{
			var lord2 = Rashi.Find(Bhava.DhanaBhava, varga).Lord;
			var lord9 = Rashi.Find(Bhava.DharmaBhava, varga).Lord;
			if (lord2.Rashi.ZodiacHouse.LordOfSign() != lord9)
			{
				return (false);
			}

			if (lord9.Rashi.ZodiacHouse.LordOfSign() != lord2)
			{
				return (false);
			}

			var lord1 = Rashi.Find(Bhava.LagnaBhava, varga).Lord;
			if (lord1.Bhava.IsKendra() || lord1.Bhava.IsTrikona())
			{
				return (true);
			}

			return (false);
		}

		//When benefices occupy houses 7 and 8 from the lagna.
		//One born in this yoga is learned and blessed with comforts.
		public static bool Lagnadhi(this DivisionType varga)
		{
			byte yoga = 0x00;
			foreach (var graha in Graha.Planets(varga))
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
		public static bool Lagnadhi1(this DivisionType varga)
		{
			if (varga.Lagnadhi() == false)
			{
				return (false);
			}

			foreach (var graha in Graha.Planets(varga))
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
		public static bool Lakshmi(this DivisionType varga)
		{
			var lord1 = Rashi.Find(Bhava.LagnaBhava, varga).Lord;
			if (lord1.Strength < 2)
			{
				return false;
			}

			var lord9 = Rashi.Find(Bhava.DharmaBhava, varga).Lord;
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
		public static bool MahaBhagyaNight(this Horoscope h, DivisionType varga)
		{
			if (h.IsDayBirth())
			{
				return (false);
			}

			var lagna = Graha.Find(Body.Lagna, varga);
			if ((lagna.Rashi.ZodiacHouse.Index() % 2) != 0)
			{
				return (false);
			}
			var sun = Graha.Find(Body.Sun, varga);
			if ((sun.Rashi.ZodiacHouse.Index() % 2) != 0)
			{
				return (false);
			}
			var moon = Graha.Find(Body.Moon, varga);
			if ((moon.Rashi.ZodiacHouse.Index() % 2) != 0)
			{
				return (false);
			}

			return (true);
		}

		//Birth during the daytime, Odd sign rising in the lagna, Sun & Moon occupying odd sign.
		//On born in this, is pleasant to look at, liberal, widely renowned, lord of lands.
		public static bool MahaBhagyaDay(this Horoscope h, DivisionType varga)
		{
			if (h.IsDayBirth() == false)
			{
				return (false);
			}

			var lagna = Graha.Find(Body.Lagna, varga);
			if ((lagna.Rashi.ZodiacHouse.Index() % 2) == 0)
			{
				return (false);
			}
			var sun = Graha.Find(Body.Sun, varga);
			if ((sun.Rashi.ZodiacHouse.Index() % 2) == 0)
			{
				return (false);
			}
			var moon = Graha.Find(Body.Moon, varga);
			if ((moon.Rashi.ZodiacHouse.Index() % 2) == 0)
			{
				return (false);
			}

			return (true);
		}

		//Moon conjoined with Rahu, aspected by Jupiter 'which itself is under melefic association.
		//The person is a heinous sinner.
		public static bool MahaPataka(this DivisionType varga)
		{
			var moon = Graha.Find(Body.Moon, varga);
			if (moon.IsConjuctWith(Body.Rahu) == false)
			{
				return (false);
			}

			if (moon.IsAspectedBy(Body.Jupiter) == false)
			{
				return (false);
			}

			var jupiter = Graha.Find(Body.Jupiter, varga);
			if (jupiter.IsAssociatedWithMalefics)
			{
				return (true);
			}

			return (false);
		}

	}

}
