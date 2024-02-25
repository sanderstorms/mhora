using System;
using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class Chandra
	{
		//Conjuntion between moon and another graha
		public static bool ChandraGraha(this Grahas grahaList, Body body)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Conjunct.Count > 0)
			{
				foreach (var graha in moon.Conjunct)
				{
					if (graha.Body == body)
					{
						return (true);
					}
				}
			}

			return (false);

		}

		//when the benefic planets (Mercury, Jupiter, and Venus) occupy the 6, 7, 8 houses from Moon.
		//They may sepatately or jointly occupy all or any of the houses sixth, seventh, and eighth from the Moon.
		//The native born with this yoga is a king, minister or commander. This yoga confers on the native prosperity,
		//health, status, Govt. recognition and dominance over opponents.
		public static bool ChandraAdhi(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);

			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.IsNaturalMalefic)
			{
				return (false);
			}

			var houseFromMoon = mercury.Bhava.HousesFrom(moon.Bhava);

			if ((houseFromMoon < 6) || (houseFromMoon > 8))
			{
				return (false);
			}

			var jupiter = grahaList.Find(Body.Jupiter);
			houseFromMoon = jupiter.Bhava.HousesFrom(moon.Bhava);

			if ((houseFromMoon < 6) || (houseFromMoon > 8))
			{
				return (false);
			}

			var venus = grahaList.Find(Body.Venus);
			houseFromMoon = venus.Bhava.HousesFrom(moon.Bhava);

			if ((houseFromMoon < 6) || (houseFromMoon > 8))
			{
				return (false);
			}

			return (true);
		}

		//When a planet other than the Sun occupies the 12th house from Moon is called Anapha Yoga.
		//One born in the Anapha yoga is a king, healthy, affable, renowned, capable, pleasant looks and happy.
		public static bool ChandraAnapha(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			foreach (var graha in grahaList.Planets)
			{
				if (graha.Body == Body.Sun)
				{
					continue;
				}

				if (graha.Bhava.HousesFrom(moon.Bhava) == 12)
				{
					return (true);
				}
			}

			return (false);
		}

		//When Saturn occupies the 12th house from Moon is called Anapha Yoga.
		//Owner of vast lands, forests and cattle, of long arms, honoring his words, enjoying the wealth of others, associated with wicked woman.
		public static bool ChandraAnaphaShani(this Grahas grahaList)
		{
			var moon   = grahaList.Find(Body.Moon);
			var saturn = grahaList.Find(Body.Saturn);

			if (saturn.Bhava.HousesFrom(moon.Bhava) == 12)
			{
				return (true);
			}

			return (false);
		}

		//When Mercury occupies the 12th house from Moon is called Anapha Yoga.
		//Eloquent, a poet, honored by the ruler, versed in music, dance and writing, handsome and renowned. Adverse for the career of one’s progeny.
		public static bool ChandraAnaphaBuddha(this Grahas grahaList)
		{
			var moon    = grahaList.Find(Body.Moon);
			var mercury = grahaList.Find(Body.Mercury);

			if (mercury.Bhava.HousesFrom(moon.Bhava) == 12)
			{
				return (true);
			}

			return (false);
		}

		//When Mars occupies the 12th house from Moon is called Anapha Yoga.
		//Leader of band of thieves, haughty, wrathful, bold, good looking and hurtful to everyone including his mother.
		public static bool ChandraAnaphaMangal(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			var mars = grahaList.Find(Body.Mars);

			if (mars.Bhava.HousesFrom(moon.Bhava) == 12)
			{
				return (true);
			}

			return (false);
		}

		//When Jupiter occupies the 12th house from Moon is called Anapha Yoga.
		//Endowed with strength and virtue, energetic, learned, honored by the king, a poet, wealthy,
		//opposed to his near & dear. If Jupiter aspects the sixth house from the Lagna, the native is happy & contented.
		public static bool ChandraAnaphaGuru(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			var mars = grahaList.Find(Body.Mars);

			if (mars.Bhava.HousesFrom(moon.Bhava) == 12)
			{
				return (true);
			}

			return (false);
		}

		//Conjunction between Moon and Mercury.
		//Pleasant looks, sweet tongued, engaged in virtues deeds, pious, blessed, a poet, kind-hearted, and deeply attached to his wife.
		public static bool ChandraBuddh(this Grahas grahaList) => grahaList.ChandraGraha(Body.Mercury);


		//Moon and Mercury are Conjunct in lagna.
		//Comfortable, wise, strong, fortunate, clever, good in looks, very talkative.
		public static bool ChandraBuddh1(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraBuddh());
		}

		//Moon and Mercury are Conjunct in 4th House.
		//Blessed with his friends, children, comforts, fame and fortune.
		public static bool ChandraBuddh4(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraBuddh());
		}

		//Moon and Mercury are Conjunct in 7th House.
		//llustrious, equivalent to a king, good looking, a poet.
		public static bool ChandraBuddh7(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraBuddh());
		}

		//Moon and Mercury are Conjunct in 9th House.
		//Learned in scriptures, bereft of peace, famous, very talkative.
		public static bool ChandraBuddh9(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraBuddh());
		}

		//Moon and Mercury are Conjunct in 10th House.
		//Wealthy, haughty. famous, minister, suffers in old age.
		public static bool ChandraBuddh10(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraBuddh());
		}


		//When planets other than the Sun occupy both 2 & 12 houses from Moon.
		//One born in this earns fame through his good speech, learning, virtue. It confers upon the native immense wealth, vehicles, lands.
		public static bool ChandraDurudhara(this Grahas grahaList, Body body1, Body body2)
		{
			var moon   = grahaList.Find(Body.Moon);
			var graha1 = grahaList.Find(body1);
			var graha2 = grahaList.Find(body2);

			var graha1Moon = graha1.Bhava.HousesFrom(moon.Bhava);
			var graha2Moon = graha2.Bhava.HousesFrom(moon.Bhava);

			if ((graha1Moon > 1) && (graha1Moon < 12))
			{
				return false;
			}

			if ((graha2Moon > 1) && (graha2Moon < 12))
			{
				return false;
			}

			if (graha1Moon != graha2Moon)
			{
				return (true);
			}

			return (false);
		}

		//When planets other than the Sun occupy both 2 & 12 houses from Moon.
		//One born in this earns fame through his good speech, learning, virtue. It confers upon the native immense wealth, vehicles, lands.
		public static bool ChandraDurudhara(this Grahas grahaList)
		{
			int second  = 0;
			int twelfth = 0;

			var moon = grahaList.Find(Body.Moon);
			foreach (var graha in grahaList.Planets)
			{
				if (graha.Body == Body.Sun)
				{
					continue;
				}

				if (graha.Bhava.HousesFrom(moon.Bhava) == 2)
				{
					second++;
				}

				if (graha.Bhava.HousesFrom(moon.Bhava) == 12)
				{
					twelfth++;
				}
			}

			if ((second == 1) && (twelfth == 1))
			{
				return (true);
			}

			return (false);
		}

		//When Jupiter and Saturn occupies the 2 and 12th house from Moon.
		//Blessed with comforts, humble, sweet-tongued, very learned, wealthy, of beautiful looks and a quiet temperament.
		public static bool ChandraDurudhara1(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Jupiter, Body.Saturn);

		//When Venus and Mars occupies the 2 and 12th house from Moon.
		//Handsome, Valorous, athletic, argumentative, pious, wealthy, very efficient, blessed with a lovely wife.
		public static bool ChandraDurudhara2(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Venus, Body.Mars);

		//When Venus and Jupiter occupies the 2 and 12th house from Moon.
		//Blessed with wisdom and valour, steadfast, prosperous, of a royal mien, very renowned, and guiltless
		public static bool ChandraDurudhara3(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Venus, Body.Jupiter);

		//When Venus and Saturn occupies the 2 and 12th house from Moon.
		//Clever and wealthy, Favored by the king, of mature thinking, head of his family, liked by women.
		public static bool ChandraDurudhara4(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Venus, Body.Saturn);

		//When Mercury and Mars occupies the 2 and 12th house from Moon.	
		//Untruthful, rich, clever, wicked, faultfinding, avaricious, respected in his own family, and addicted to elderly unchaste women.
		public static bool ChandraDurudhara5(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Mercury, Body.Mars);

		//When Mars and Saturn occupies the 2 and 12th house from Moon.	
		//Addicted to unchaste women, engaged in wicked deeds, easily angered, treacherous, rich, annihilator of enemies, a hoarder, without remorse.
		public static bool ChandraDurudhara6(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Saturn, Body.Mars);

		//When Mercury and Venus occupies the 2 and 12th house from Moon.	
		//Sweet tongued, good in looks, fond of music and dance, heroic in temperament, a minister, commanding respect.
		public static bool ChandraDurudhara7(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Mercury, Body.Venus);

		//When Mercury and Saturn occupies the 2 and 12th house from Moon.	
		//Goes to one country to another country to earn money, revered of poor or moderate learning, opposed to his kith and kin.
		public static bool ChandraDurudhara8(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Mercury, Body.Saturn);

		//Religiously inclined, versed in scriptures, eloquent, wealthy, a poet, a renunciant, highly renowned.
		public static bool ChandraDurudhara9(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Mercury, Body.Jupiter);

		//Renowned and wealthy, easily angered, generally contended, protector of his dear ones, harassed by his opponents,
		//accumulator of fortune earned through his own efforts.
		public static bool ChandraDurudhara10(this Grahas grahaList) => grahaList.ChandraDurudhara(Body.Mars, Body.Jupiter);


		//Conjunction between Moon and Jupiter.
		//Overpowering, virtuous, famous, intelligent profoundly versed, many friends, engaged in virtuous pursuits, doing good to others,
		//wealthy, c//onsistent in love, soft spoken, chief of family, fickle- minded.
		public static bool ChandraGuru(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Conjunct.Count > 0)
			{
				foreach (var graha in moon.Conjunct)
				{
					if (graha.Body == Body.Jupiter)
					{
						return (true);
					}
				}
			}

			return (false);
		}

		//Moon and Jupiter are Conjunct in Lagna.
		//Broad-chested, good-looking, blessed with wife, friends and children.
		public static bool ChandraGuru1(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraGuru());
		}

		//Moon and Jupiter are Conjunct in 4th house
		//Equivalent to a king, a minister, illustrious, highly learned.
		public static bool ChandraGuru4(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraGuru());
		}

		//Moon and Jupiter are Conjunct in 7th house
		//Learned, equal to king, very skilled, a trader, very wealthy.
		public static bool ChandraGuru7(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraGuru());
		}

		//Moon and Jupiter are Conjunct in 9th house
		//Distinguished, fortunate, wealthy, contented in all circumstances.
		public static bool ChandraGuru9(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.DhanaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraGuru());
		}

		//Moon and Jupiter are Conjunct in 10th house
		//Scholar, wealthy, haughty, renowned, respected by all.
		public static bool ChandraGuru10(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return (grahaList.ChandraGuru());
		}

		//All planets aspecting the Moon.
		//When this happens, the adverse Kemadruma yield place to a highly benefic kalpadruma yoga which bestows all comforts on the native.
		public static bool ChandraKalpadruma1(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			foreach (var graha in grahaList.Planets)
			{
				if (graha.Body == Body.Moon)
				{
					continue;
				}

				if (graha.AspectTo.Contains(moon) == false)
				{
					return (false);
				}
			}

			return (true);
		}

		public static Tithi GetTithi(this Grahas grahaList)
		{
			var moon     = grahaList.Find(Body.Moon);
			var sun      = grahaList.Find(Body.Sun);
			var distance = (Longitude) moon.DistanceFrom(sun);
			return (distance.ToTithi());
		}

		//A full Moon occupying the lagna in conjunction with a benefic planet.
		//When this happens, the adverse Kemadruma yield place to a highly benefic kalpadruma yoga which bestows all comforts on the native
		public static bool ChandraKalpadruma2(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);

			if (grahaList.GetTithi() != Tithi.Paurnami)
			{
				return (false);
			}

			if (moon.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (moon.Conjunct.Count > 0)
			{
				foreach (var graha in moon.Conjunct)
				{
					if (graha.IsNaturalBenefic == false)
					{
						return (false);
					}
				}

				return (true);
			}

			return (false);
		}

		//A strong Moon in a kendra associated with or aspected by benefices.
		//When this happens, the adverse Kemadruma yield place to a highly benefic kalpadruma yoga which bestows all comforts on the native.
		public static bool ChandraKalpadruma3(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava.IsKendra() == false)
			{
				return (false);
			}

			if (moon.Strength < 2)
			{
				return (false);
			}

			int yoga = 0;
			foreach (var graha in grahaList.Planets)
			{
				if (moon.IsAssociatedWith(graha))
				{
					if (graha.IsNaturalBenefic)
					{
						yoga++;
					}
					else
					{
						yoga--;
					}
				}
			}

			if (moon.AspectFrom.Count > 0)
			{
				foreach (var graha in moon.AspectFrom)
				{
					if (graha.IsNaturalBenefic)
					{
						yoga++;
					}
					else
					{
						yoga--;
					}
				}

			}

			return (yoga > 0);
		}

		//The Moon associated with a benefic planet or located between two benefices and aspected by Jupiter.
		//When this happens, the adverse Kemadruma yield place to a highly benefic kalpadruma yoga which bestows all comforts on the native.
		public static bool ChandraKalpadruma4(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			int yoga = 0;

			foreach (var graha in grahaList.Planets)
			{
				if (moon.IsAssociatedWith(graha))
				{
					if (graha.IsNaturalBenefic)
					{
						yoga++;
					}
					else
					{
						yoga--;
					}
				}
			}

			if (yoga > 0)
			{
				return true;
			}

			if (moon.Before.IsNaturalBenefic == false)
			{
				return (false);
			}

			if (moon.After.IsNaturalBenefic == false)
			{
				return (false);
			}

			if (moon.AspectFrom.Count > 0)
			{
				foreach (var graha in moon.AspectFrom)
				{
					if (graha.Body == Body.Jupiter)
					{
						return (true);
					}
				}
			}

			return (false);
		}

		//The Moon occupying in the navamsha chart its exaltation sign or the house of a very friendly planet, aspected by Jupiter.
		//When this happens, the adverse Kemadruma yield place to a highly benefic kalpadruma yoga which bestows all comforts on the native.
		public static bool ChandraKalpadruma5(this Grahas grahaList)
		{
			var navamsha = grahaList.Horoscope.FindGrahas(DivisionType.Navamsa);
			var moon     = navamsha.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			if ((moon.IsExalted == false) && (moon.FriendlySign == false))
			{
				return (false);
			}

			moon = grahaList.Find(Body.Moon);
			foreach (var graha in moon.AspectFrom)
			{
				if (graha.Body == Body.Jupiter)
				{
					return (true);
				}
			}

			return (false);
		}

		//Presence of planets in kendras from the Moon.
		//When this happens, the adverse Kemadruma yield place to a highly benefic kalpadruma yoga which bestows all comforts on the native.
		public static bool ChandraKalpadruma6(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);

			foreach (var graha in grahaList.Planets)
			{
				if (graha.Body == Body.Moon)
				{
					continue;
				}

				var bhava = (Bhava) graha.Bhava.HousesFrom(moon.Bhava);
				if (bhava.IsKendra())
				{
					return (true);
				}
			}

			return (false);
		}

		//The Moon exalted in the 10 house and aspected by a benefic.
		//When this happens, the adverse Kemadruma yield place to a highly benefic kalpadruma yoga which bestows all comforts on the native.
		public static bool ChandraKalpadruma7(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);

			if (moon.IsExalted == false)
			{
				return (false);
			}

			if (moon.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			foreach (var graha in moon.AspectFrom)
			{
				if (graha.IsNaturalBenefic)
				{
					return (true);
				}
			}

			return (false);
		}


		//If at birth, Mars and Jupiter be in Tula, the Sun in Kanya and Moon in Mesha,
		//even if the other planets do not aspect the Moon, the Kemadruma stands cancelled.
		public static bool ChandraKalpadruma8(this Grahas grahaList)
		{
			var moon    = grahaList.Find(Body.Moon);
			var mars    = grahaList.Find(Body.Mars);
			var jupiter = grahaList.Find(Body.Jupiter);
			var sun     = grahaList.Find(Body.Sun);

			if (moon.Rashi.ZodiacHouse != ZodiacHouse.Ari)
			{
				return (false);
			}


			if (mars.Rashi.ZodiacHouse != ZodiacHouse.Lib)
			{
				return (false);
			}

			if (jupiter.Rashi.ZodiacHouse != ZodiacHouse.Lib)
			{
				return (false);
			}

			if (sun.Rashi.ZodiacHouse != ZodiacHouse.Vir)
			{
				return (false);
			}

			return (true);
		}

		//A waning Moon in debilitation, with the native born at night time.
		//One born in this yoga is bereft of health, wealth, learning, wisdom,
		//wife and mental peace, such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma9(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.IsDebilitated == false)
			{
				return (false);
			}

			if (grahaList.GetTithi() < Tithi.KrishnaPratipada)
			{
				return (false);
			}

			if (grahaList.Horoscope.IsDayBirth())
			{
				return (false);
			}

			return (true);
		}

		//Placement of the lord of the 11th house in the 12th house, and a weak 12th lord in the 2nd house, with malefics in the 3rd house.
		//Such a native lives on food given by others, indulges in base acts, is poor and ever engaged in adultery.
		public static bool ChandraKalpadruma10(this Grahas grahaList)
		{
			var lord = grahaList.Rashis.Find(Bhava.LabhaBhava).Lord;
			if (lord.Bhava != Bhava.VyayaBhava)
			{
				return (false);
			}

			lord = grahaList.Rashis.Find(Bhava.VyayaBhava).Lord;
			if (lord.Bhava != Bhava.DhanaBhava)
			{
				return (false);
			}

			var rashi = grahaList.Rashis.Find(Bhava.SahajaBhava);
			foreach (var graha in rashi.Grahas)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);

		}

		//A waning Moon, occupying the 8th house from the lagna, aspected by or associated with a malefic planet,
		//in a case where birth takes place during the night time.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace,
		//such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma11(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);

			if (moon.Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			if (grahaList.GetTithi() < Tithi.KrishnaPratipada)
			{
				return (false);
			}

			if (grahaList.Horoscope.IsDayBirth())
			{
				return (false);
			}

			foreach (var graha in grahaList.Planets)
			{
				if (moon.IsAssociatedWith(graha))
				{
					if (graha.IsNaturalMalefic)
					{
						return (true);
					}
				}
			}

			foreach (var graha in moon.AspectFrom)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//This yoga arises when there is no planet in the 2 or 12 from Moon.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace,
		//such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma12(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);

			var zh    = moon.Rashi.ZodiacHouse.Add(2);
			var rashi = grahaList.Rashis.Find(zh);
			if (rashi.Grahas.Count != 0)
			{
				return (false);
			}

			zh    = moon.Rashi.ZodiacHouse.Add(12);
			rashi = grahaList.Rashis.Find(zh);
			if (rashi.Grahas.Count != 0)
			{
				return (false);
			}

			return (true);
		}

		//The Moon in the lagna or the 7 house sans Jupiter’s aspect.
		//One born in this yoga is bereft of health, wealth, learning, wisdom,
		//wife and mental peace such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma13(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if ((moon.Bhava != Bhava.LagnaBhava) && (moon.Bhava != Bhava.JayaBhava))
			{
				return (false);
			}

			foreach (var graha in moon.AspectFrom)
			{
				if (graha.Body == Body.Jupiter)
				{
					return (false);
				}
			}

			return (true);
		}

		//The Moon is in conjunction with the Sun, aspected by a debilitated planet and occupying a malefic navamsha.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace
		//such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma14(this Grahas grahaList)
		{
			bool yoga = false;
			var  moon = grahaList.Find(Body.Moon);
			foreach (var graha in moon.Conjunct)
			{
				if (graha.Body == Body.Sun)
				{
					yoga = true;
					break;
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			yoga = false;
			foreach (var graha in moon.AspectFrom)
			{
				if (graha.IsDebilitated)
				{
					yoga = true;
					break;
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			var navamsa = grahaList.Horoscope.FindGrahas( DivisionType.Navamsa);

			moon = navamsa.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsa not calculated!");
			}

			if (moon.EnemySign)
			{
				return (true);
			}

			return (false);
		}

		//The Moon in the Rahu-Ketu Axis, aspected by a malefic planet.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace, such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma15(this Grahas grahaList)
		{
			bool yoga = false;
			var  moon = grahaList.Find(Body.Moon);
			if (moon.Conjunct.Count > 0)
			{
				foreach (var graha in moon.Conjunct)
				{
					if ((graha.Body == Body.Rahu) || (graha.Body == Body.Ketu))
					{
						yoga = true;
						break;
					}
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			foreach (var graha in moon.AspectFrom)
			{
				if ((graha.Body == Body.Rahu) || (graha.Body == Body.Ketu))
				{
					continue;
				}

				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//The 4 house from the Lagna or the Moon occupied by a malefic planet.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace, such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma16(this Grahas grahaList)
		{
			var moon  = grahaList.Find(Body.Moon);
			var rashi = grahaList.Rashis.Find(Bhava.SukhaBhava);
			foreach (var graha in rashi.Grahas)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			var zh = moon.Rashi.ZodiacHouse.Add(4);
			rashi = grahaList.Rashis.Find(zh);
			foreach (var graha in rashi.Grahas)
			{
				if (graha.IsNaturalMalefic)
				{
					return (true);
				}
			}

			return (false);
		}

		//The moon in Tula, in the varga of an inimical planet, aspected by an inimical or debilitated planet.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace, such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma17(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Rashi.ZodiacHouse != ZodiacHouse.Lib)
			{
				return (false);
			}

			if (moon.IsDebilitated == false)
			{
				return (false);
			}

			foreach (var graha in moon.AspectFrom)
			{
				if (graha.IsDebilitated)
				{
					return (true);
				}

				if (graha.Body.IsEnemy(Body.Moon))
				{
					return (true);
				}
			}

			return (false);
		}

		//The Moon in Chara rashi and Chara navamsha, aspected by an inimical planet, unaspected by Jupiter.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace, such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma18(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Rashi.ZodiacHouse.IsMoveableSign() == false)
			{
				return (false);
			}

			bool yoga = false;
			foreach (var graha in moon.AspectFrom)
			{
				if (graha.Body == Body.Jupiter)
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
			

			var navamsa = grahaList.Horoscope.FindGrahas( DivisionType.Navamsa);
			moon = navamsa.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			if (moon.Rashi.ZodiacHouse.IsMoveableSign())
			{
				return (true);
			}

			return (false);
		}

		//A weak Moon conjunct with a malefic planet and occupying, in a night birth, a malefic house or a navamsha, aspected by the lord of the 10th house.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace, such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma19(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Strength > 0)
			{
				return (false);
			}

			if (grahaList.Horoscope.IsDayBirth())
			{
				return (false);
			}

			bool yoga = false;
			foreach (var graha in moon.Conjunct)
			{
				if (graha.IsNaturalMalefic)
				{
					yoga = true;
					break;
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			var lord = grahaList.Rashis.Find(Bhava.KarmaBhava).Lord;

			yoga = false;
			foreach (var graha in moon.AspectFrom)
			{
				if (graha == lord)
				{
					yoga = true;
					break;
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			if (moon.EnemySign)
			{
				return (true);
			}

			var navamsa = grahaList.Horoscope.FindGrahas( DivisionType.Navamsa);
			moon = navamsa.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			if (moon.EnemySign)
			{
				return (true);
			}

			return (false);
		}

		//A waning Moon debilitated in navamsha, associated with a malefic and aspected by the 9 lord.
		//One born in this yoga is bereft of health, wealth, learning, wisdom, wife and mental peace, such a native suffers misery, failures, physical illness.
		public static bool ChandraKalpadruma20(this Grahas grahaList)
		{
			var navamsa = grahaList.Horoscope.FindGrahas( DivisionType.Navamsa);
			var moon    = navamsa.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			var  lord = grahaList.Rashis.Find(Bhava.DharmaBhava).Lord;
			bool yoga = false;
			foreach (var graha in moon.AspectFrom)
			{
				if (graha == lord)
				{
					yoga = true;
					break;
				}
			}

			if (yoga == false)
			{
				return (false);
			}

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalMalefic)
				{
					if (moon.IsAssociatedWith(graha))
					{
						return (true);
					}
				}
			}

			return (false);
		}


		//Conjunction between Moon and Mars.
		//Wealthy, brave, winner in combat, dealer of women, wines and earthenware,
		//adept in metal craft, suffering from blood disorders, hostile to mother.
		public static bool ChandraMangal(this Grahas grahaList) => grahaList.ChandraGraha(Body.Mars);

		//Moon and mars are Conjunct in Lagna.
		//Aggressive, suffers from blood and bile disorders.
		public static bool ChandraMangal1(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return grahaList.ChandraMangal();
		}

		//Moon and mars are Conjunct in 4th house.
		//Quarrelsome, poor, bereft of home comforts and mental peace.
		public static bool ChandraMangal4(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return grahaList.ChandraMangal();

		}

		//Moon and mars are Conjunct in 7th house.
		//Talkative, desires others wealth and possessions.
		public static bool ChandraMangal7(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return grahaList.ChandraMangal();
		}

		//Moon and mars are Conjunct in 9th house.
		//Hostile to mother, bereft of peace, of injured body, wealthy
		public static bool ChandraMangal9(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return grahaList.ChandraMangal();
		}

		//Moon and mars are Conjunct in 10th house.
		//Valorous, blessed with vehicles and material possessions
		public static bool ChandraMangal10(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return grahaList.ChandraMangal();
		}

		//Conjunction between Moon and Saturn.
		//Born of a widow remarried, attached to an old woman, given to pleasures of the flesh,
		//bereft of grace, wealth and velour, tends horses and elephants.
		public static bool ChandraShani(this Grahas grahaList) => grahaList.ChandraGraha(Body.Saturn);

		//Moon and Saturn are Conjunct in Lagna.
		//Servile, ugly in looks, greedy, lazy, a sinner.
		public static bool ChandraShani1(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.LabhaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShani();
		}

		//Moon and Saturn are Conjunct in 4th House.
		//Profession related to water, precious stones and boating or ships, honored by others.
		public static bool ChandraShani4(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShani();
		}

		//Moon and Saturn are Conjunct in 7th House.
		//Headman of a village or a town, bereft of wife.
		public static bool ChandraShani7(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShani();
		}

		//Moon and Saturn are Conjunct in 9th House.
		//A sinner, follows blemished faith, gives up his mother.
		public static bool ChandraShani9(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShani();
		}

		//Moon and Saturn are Conjunct in 10th House.
		//Valorous, blessed with vehicles and material possessions.
		public static bool ChandraShani10(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShani();
		}

		//Conjunction between Moon and Venus.
		//Clever in buying and selling, adept in tailoring, weaving and trading of clothes,
		//Quarrelsome, fond of flowers and perfumes, lazy, sinful, a poet.
		public static bool ChandraShukra(this Grahas grahaList) => grahaList.ChandraGraha(Body.Venus);

		//Moon and Venus are Conjunct in Lagna
		//Good in looks, devoted to teachers and elders, blessed with good cloths and perfumes, comforted by base women.
		public static bool ChandraShukra1(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShukra();
		}

		//Moon and Venus are Conjunct in 4th house
		//Comforted by women, earns from sea travel, very likeable.
		public static bool ChandraShukra4(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShukra();
		}

		//Moon and Venus are Conjunct in 7th house
		//Associates with many women, little wealth, more daughters and few sons, equal to a king, very wise.
		public static bool ChandraShukra7(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShukra();
		}

		//Moon and Venus are Conjunct in 9th house
		//Ailing, husband of a base woman, subservient to one in high position.
		public static bool ChandraShukra9(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShukra();
		}

		//Moon and Venus are Conjunct in 10th house
		//Very distinguished, high status, forgiving.
		public static bool ChandraShukra10(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			if (moon.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return grahaList.ChandraShukra();
		}

		//When a planet other than the Sun occupies the second house from the moon, the resulting yoga is called as Sunapha yoga.
		//This confers on the native a status equivalent to that of a king, immense wealth, capacity to earn his fortune through his own efforts,
		//wide renown, inclination towards virtuous deeds, quietude and contentment.
		public static bool ChandraSunapha(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			foreach (var graha in grahaList.Planets)
			{
				if (graha.Body == Body.Sun)
				{
					continue;
				}

				if (graha.Bhava.HousesFrom(moon.Bhava) == 2)
				{
					return (true);
				}
			}
			return (false);
		}

		//When Jupiter occupies the second house from the moon, the resulting yoga is called as Sunapha yoga.
		//Excelling in every branch of learning, preceptor, widely renowned, very wealthy, favored by the ruler,
		//and blessed with a good family. The native is sinless and long-lived.
		public static bool ChandraSunaphaGuru(this Grahas grahaList)
		{
			var moon    = grahaList.Find(Body.Moon);
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.Bhava.HousesFrom(moon.Bhava) == 2)
			{
				return (true);
			}

			return (false);
		}

		//When Mars occupies the second house from the moon, the resulting yoga is called as Sunapha yoga.
		//The native is valorous, cruel, fierce, wealthy, a king, or a commander, averse to hypocrisy, his son takes to agriculture.
		public static bool ChandraSunaphaMangal(this Grahas grahaList)
		{
			var moon    = grahaList.Find(Body.Moon);
			var mars = grahaList.Find(Body.Mars);
			if (mars.Bhava.HousesFrom(moon.Bhava) == 2)
			{
				return (true);
			}

			return (false);

		}

		//When Venus occupies the second house from the moon, the resulting yoga is called as Sunapha yoga.
		//The native is very efficient, brave, good in looks, and honored by the  ruler. He is learned, and blessed with wife,
		//houses, lands, vehicles, quadrupeds and splendor.
		public static bool ChandraSunaphaShukra(this Grahas grahaList)
		{
			var moon = grahaList.Find(Body.Moon);
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava.HousesFrom(moon.Bhava) == 2)
			{
				return (true);
			}

			return (false);

		}

		//When Mercury occupies the second house from the moon, the resulting yoga is called as Sunapha yoga.
		//Well-versed in scriptures, fine arts and music, immersed in religious pursuits, of good looks and agreeable speech, highly intelligent,
		//and doing good to others. He earns well and dies of ailments arising from cold.
		public static bool ChandraSunaphaBuddh(this Grahas grahaList)
		{
			var moon  = grahaList.Find(Body.Moon);
			var mercury = grahaList.Find(Body.Mercury);
			if (mercury.Bhava.HousesFrom(moon.Bhava) == 2)
			{
				return (true);
			}

			return (false);

		}

		//When Saturn occupies the second house from the moon, the resulting yoga is called as Sunapha yoga.
		//Clever and skillful, held in esteem by the rural and the urban folk alike, wealthy and contented.
		//The native lives on Goat’s milk. This is not favorable for the mother of the native.
		public static bool ChandraSunaphaShani(this Grahas grahaList)
		{
			var moon    = grahaList.Find(Body.Moon);
			var saturn = grahaList.Find(Body.Saturn);
			if (saturn.Bhava.HousesFrom(moon.Bhava) == 2)
			{
				return (true);
			}

			return (false);

		}

		//Conjunction between Moon, Mercury, and Jupiter.
		//Learned, Famous, eloquent, rich, virtuous, sickly, the favorite of king.
		public static bool ChandraBuddhGuru(this Grahas grahaList) => grahaList.ChandraBuddh() && grahaList.ChandraGuru();

		//Conjunction between Moon, Mercury, Jupiter and Saturn.
		//Virtuous, charitable, learned, famous, exceedingly wealthy, counselor of a king.
		public static bool ChandraBuddhGuruShani(this Grahas grahaList) => grahaList.ChandraBuddhGuru() && grahaList.ChandraShani();

		//Moon, Mercury, Jupiter, Venus, and Saturn are conjunct.
		//Highly respectable, a minister, virtuous, leader of many people.
		public static bool ChandraBuddhGuruShukraShani(this Grahas grahaList) => grahaList.ChandraBuddhGuruShani() && grahaList.ChandraShukra();

		//Conjunction between Moon, Mercury, and Saturn.
		//Learned, worthy, honored by the ruler, eloquent, sickly, a leader.
		public static bool ChandraBuddhShani(this Grahas grahaList) => grahaList.ChandraBuddh() && grahaList.ChandraShani();

		//Conjunction between Moon, Mercury, and Venus.
		//Good learning, honorable, mean nature, highly covetous, jealous of others.
		public static bool ChandraBuddhShukra(this Grahas grahaList) => grahaList.ChandraBuddh() && grahaList.ChandraShukra();

		//Conjunction between Moon, Mercury, Venus and Saturn.
		//Adulterous, husband of a wicked woman, learned, hostile to many, of diseased eyes.
		public static bool ChandraBuddhShukraShani(this Grahas grahaList) => grahaList.ChandraBuddhShukra() && grahaList.ChandraShani();

		//Conjunction between Moon, Jupiter, and Saturn.
		//Versed in scriptures, liked by the ruler, clever, renowned, bereft of illness, leader of a village or a town, attached to older women.
		public static bool ChandraGuruShani(this Grahas grahaList) => grahaList.ChandraGuru() && grahaList.ChandraShani();

		//Conjunction between Moon, Jupiter, and Venus.
		//Very good looking, learned, born of a virtues mother, proficient in several arts.
		public static bool ChandraGuruShukra(this Grahas grahaList) => grahaList.ChandraGuru() && grahaList.ChandraShukra();

		//Conjunction between Moon, Jupiter, Venus, and Saturn.
		//Deprived of mother, truthful, adulterous, uncomfortable, a wanderer, has skin disease.
		public static bool ChandraGuruShukraShani(this Grahas grahaList) => grahaList.ChandraGuruShukra() && grahaList.ChandraShani();

		//Conjunction between Moon, Mars, and Mercury.
		//Wicked, humiliated by his own people, bereft of virtue and wealth, without friends throughout his life, a glutton.
		public static bool ChandraMangalBuddh(this Grahas grahaList) => grahaList.ChandraMangal() && grahaList.ChandraBuddh();

		//Conjunction between Moon, Mars, Mercury and Jupiter.
		//Versed in sacred scriptures, a king or a minister, highly renowned.
		public static bool ChandraMangalBuddhGuru(this Grahas grahaList) => grahaList.ChandraMangalBuddh() && grahaList.ChandraGuru();

		//Moon, Mars, Mercury, Jupiter, and Saturn are conjunct.
		//Wicked, poor, living on begged food, suffers night-blindness.
		public static bool ChandraMangalBuddGuruShani(this Grahas grahaList) => grahaList.ChandraMangalBuddhGuru() && grahaList.ChandraShani();

		//Moon, Mars, Mercury, Jupiter, and Venus are conjunct.
		//Virtuous, learned, wealthy, bereft of ailments, with many friends.
		public static bool ChandraMangalBuddhGuruShukra(this Grahas grahaList) => grahaList.ChandraMangalBuddhGuru() && grahaList.ChandraShukra();

		//Moon, Mars, Mercury, Jupiter, Venus and Saturn are conjunct.
		//Pious, famous, lazy, wealthy, a king’s counselor, blessed with many women, undertakes pilgrimages, of ascetic habits.
		public static bool ChandraMangalBuddhGuruShukraShani(this Grahas grahaList) => grahaList.ChandraMangalBuddhGuruShukra() && grahaList.ChandraShani();

		//Conjunction between Moon, Mars, Mercury and Saturn.
		//Brave, bereft of comforts from his friends, blessed with wife, children and friends.
		public static bool ChandraMangalBuddhShani(this Grahas grahaList) => grahaList.ChandraMangalBuddh() && grahaList.ChandraShani();

		//Conjunction between Moon, Mars, Mercury and Venus.
		//Quarrelsome, lazy, wicked, hostile to his own people, good in looks, husband of a wicked woman.
		public static bool ChandraMangalBuddhShukra(this Grahas grahaList) => grahaList.ChandraMangalBuddh() && grahaList.ChandraShukra();

		//Moon, Mars, Mercury, Venus, and Saturn are conjunct.
		//Ugly in looks, foolish, a pauper, a eunuch, haughty, serving others.
		public static bool ChandraMangalBuddhShukraShani(this Grahas grahaList) => grahaList.ChandraMangalBuddhShukra() && grahaList.ChandraShani();

		//Conjunction between Moon, Mars, and Jupiter.
		//Good in looks, lovely face, love-sick, likeable, easily angered, his body is scarred with injuries.
		public static bool ChandraMangalGuru(this Grahas grahaList) => grahaList.ChandraMangal() && grahaList.ChandraGuru();

		//Conjunction between Moon, Mars, Jupiter, and Saturn
		//Learned, generous, brave, mentally stable, rich, of defective hearing.
		public static bool ChandraMangalGuruShani(this Grahas grahaList) => grahaList.ChandraMangalGuru() && grahaList.ChandraShani();

		//Conjunction between Moon, Mars, Jupiter and Venus.
		//Valorous, wealthy, learned, blessed with friends and a lovely wife, defective of a limb.
		public static bool ChandraMangalGuruShukra(this Grahas grahaList) => grahaList.ChandraMangalGuru() && grahaList.ChandraShukra();

		//Moon, Mars, Jupiter, Venus, and Saturn are conjunct
		//A menial servant, foolish, thievish, living on begged food, shabbily dressed, of disease in the eye.
		public static bool ChandraMangalGuruShukraShani(this Grahas grahaList) => grahaList.ChandraMangalGuruShukra() && grahaList.ChandraShani();

		//Conjunction between Moon, Mars and Saturn.
		//Bereft of comforts from mother right from childhood, wicked, fickle- minded, indulges in prohibited deeds.
		public static bool ChandraMangalShani(this Grahas grahaList) => grahaList.ChandraMangal() && grahaList.ChandraShani();
		
		 //Conjunction between Moon, Mars, and Venus.
		 //Master of an ill-mannered woman, ever a wanderer, fickle-minded, in dread of cold.
		public static bool ChandraMangalShukra(this Grahas grahaList) => grahaList.ChandraMangal() && grahaList.ChandraShukra();

		//Conjunction between Moon, Mars, Venus, and Saturn.
		//Husband of an immoral woman, ever miserable, courageous, fearless, with eyes like those of a serpent.
		public static bool ChandraMangalShukraShani(this Grahas grahaList) => grahaList.ChandraMangalShukra() && grahaList.ChandraShani();

		//Conjunction between Moon, Venus, and Saturn.
		//Proficient writer, coming from a good family, engaged in virtues pursuits, very likeable.
		public static bool ChandraShukraShani(this Grahas grahaList) => grahaList.ChandraShukra() && grahaList.ChandraShani();
	}
}
