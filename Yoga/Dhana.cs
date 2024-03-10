using System;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Yoga
{
	public static class Dhana
	{
		//The Sun in Simha Lagna, under the association or aspect of Mars and Jupiter.
		//Strong Lagna should be there. This combination lead to excessive wealth. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts.
		public static bool DhanaSurya(this Grahas grahas)
		{
			byte yoga = 0;
			var sun  = grahas.Find(Body.Sun);
			if (sun.Rashi.ZodiacHouse != ZodiacHouse.Leo)
			{
				return (false);
			}

			foreach (var graha in sun.AspectFrom)
			{
				if (graha.Body == Body.Jupiter)
				{
					yoga |= 0x01;
				}

				if (graha.Body == Body.Mars)
				{
					yoga |= 0x02;
				}
			}

			if (sun.IsAssociatedWith(Body.Jupiter))
			{
				yoga |= 0x01;
			}

			if (sun.IsAssociatedWith(Body.Mars))
			{
				yoga |= 0x02;
			}

			return (yoga == 0x03);
		}

		//The Jupiter occupying the 5th house identical with its own rashi (Dhanu/Meena), and Mercury in the 11th house.
		//This combination lead to excessive wealth. This yoga generally related to one’s profession, it may be desirable to examine the
		//dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool DhanaGuru5th(this Grahas grahas)
		{
			var jupiter = grahas.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			if (jupiter.IsInOwnHouse == false)
			{
				return (false);
			}

			var mercury = grahas.Find(Body.Mercury);
			if (mercury.Bhava == Bhava.LabhaBhava)
			{
				return (true);
			}

			return (false);
		}

		//The Lagna lord is associtating with the 2nd or 5th or 9th or the 11th lord.
		//This indicates a promise of rise in status associated with increased inflow of money. The native will be benefited
		//specially when the participating planet’s is in it’s mahadasha.
		public static bool DhanaLagna(this Grahas grahas)
		{
			var lagnaLord = grahas.Rashis.Find(Bhava.LagnaBhava).Lord;
			var graha     = grahas.Rashis.Find(Bhava.DhanaBhava).Lord;
			if (lagnaLord.IsAssociatedWith(graha) == false)
			{
				return (true);
			}

			graha = grahas.Rashis.Find(Bhava.PutraBhava).Lord;
			if (lagnaLord.IsAssociatedWith(graha) == false)
			{
				return (true);
			}

			graha = grahas.Rashis.Find(Bhava.DhanaBhava).Lord;
			if (lagnaLord.IsAssociatedWith(graha) == false)
			{
				return (true);
			}

			graha = grahas.Rashis.Find(Bhava.LabhaBhava).Lord;
			if (lagnaLord.IsAssociatedWith(graha) == false)
			{
				return (true);
			}

			return (false);
		}

		//The Moon in Karka Lagna, under the influence of Mercury and Jupiter.
		//Strong Lagna should be there. This combination lead to excessive wealth. This yoga generally related
		//to one’s profession, it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts.
		public static bool DhanaChandra(this Grahas grahas)
		{
			var moon = grahas.Find(Body.Moon);
			if (moon.Rashi.ZodiacHouse != ZodiacHouse.Can)
			{
				return (false);
			}

			var mercury = grahas.Find(Body.Mercury);
			if (moon.IsUnderInfluenceOf(mercury) == false)
			{
				return (false);
			}

			var jupiter = grahas.Find(Body.Jupiter);
			if (moon.IsUnderInfluenceOf(jupiter) == false)
			{
				return (false);
			}

			return (true);
		}

		//The 2nd lord is associtating with the 5th or the 9th or the 11th lord.
		//This Yoga is for wealth and financial prosperity. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool Dhana2ndHouseLord(this Grahas grahas)
		{
			var lord  = grahas.Rashis.Find(Bhava.DhanaBhava).Lord;
			var graha = grahas.Rashis.Find(Bhava.PutraBhava).Lord;
			if (lord.IsAssociatedWith(graha) == false)
			{
				return (false);
			}
			graha = grahas.Rashis.Find(Bhava.DharmaBhava).Lord;
			if (lord.IsAssociatedWith(graha) == false)
			{
				return (false);
			}
			graha = grahas.Rashis.Find(Bhava.LabhaBhava).Lord;
			if (lord.IsAssociatedWith(graha) == false)
			{
				return (false);
			}

			return (false);
		}

		//The Venus in its own rashi (Vrischika/Tula) in the lagna, under the influence of Mercury and Saturn.
		//Strong lagna should be there. This combination lead to excessive wealth. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts.
		public static bool DanaShukraLagna(this Grahas grahas)
		{
			var  venus = grahas.Find(Body.Venus);
			if (venus.IsInOwnHouse == false)
			{
				return (false);
			}

			if (venus.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (venus.IsUnderInfluenceOf(Body.Mercury) == false)
			{
				return (false);
			}
			
			if (venus.IsUnderInfluenceOf(Body.Saturn) == false)
			{
				return (false);
			}

			return (true);
		}

		//The Rashis of jupiter falling in the 9 house and Jupiter- Venus or the 5 lord aspecting them produce a potent dhana yoga
		//This indicates a promise of rise in status associated with increased inflow of money. The native will be benefited
		//specially when the participating planet’s is in it’s Mahadasha.
		public static bool DhanaGuru9th(this Grahas grahas)
		{
			var jupiter = grahas.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			//Todo: Examine all dhana yoga's
			var venus = grahas.Find(Body.Venus);
			var lord  = grahas.Rashis.Find(Bhava.PutraBhava).Lord;

			if (grahas.Dhana5thLord ())
			{
				return (true);
			}

			if (grahas.Dhana9thLord ())
			{
				return (true);
			}

			if (grahas.DhanaShukra5th ())
			{
				return (true);
			}

			return (false);
		}

		//The Mercury occupying the 5th house identical with its own rashi(Mithuna/Kanya), with the Moon, Mars and Jupiter in the 11th house.
		//This combination lead to excessive wealth. This yoga generally related to one’s profession, it may be desirable to examine the
		//dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool DhanaBuddh5th(this Grahas grahas)
		{
			var mercury = grahas.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			if (mercury.IsInOwnHouse == false)
			{
				return (false);
			}

			byte yoga  = 0x00;
			var  rashi = grahas.Rashis.Find(Bhava.LabhaBhava);
			foreach (var graha in grahas.Planets)
			{
				if (graha.Body == Body.Moon)
				{
					yoga |= 0x01;
				}

				if (graha.Body == Body.Mars)
				{
					yoga |= 0x02;
				}

				if (graha.Body == Body.Jupiter)
				{
					yoga |= 0x04;
				}
			}

			return (yoga == 0x07);
		}

		//The Mercury in its own rashi(Mithuna/Kanya) in the Lagna, under the influence of Jupiter and Saturn.
		//Strong lagna should be there. This combination lead to excessive wealth. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts
		public static bool DhanaBuddhLagna(this Grahas grahas)
		{
			var mercury = grahas.Find(Body.Mercury);
			if (mercury.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (mercury.IsInOwnHouse == false)
			{
				return (false);
			}

			if (mercury.IsUnderInfluenceOf(Body.Jupiter) == false)
			{
				return (false);
			}

			if (mercury.IsUnderInfluenceOf(Body.Saturn) == false)
			{
				return (false);
			}

			return (true);
		}

		//The Venus occupying the 5th house identical with its own rashi(Vrisha/Tula), and Mars placed in the Lagna.
		//This combination lead to excessive wealth. This yoga generally related to one’s profession, it may be desirable
		//to examine the dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool DhanaShukra5th (this Grahas grahas)
		{
			var venus = grahas.Find(Body.Venus);
			if (venus.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			if (venus.IsInOwnHouse == false)
			{
				return (false);
			}
			var mars = grahas.Find(Body.Mars);
			if (mars.Bhava == Bhava.LagnaBhava)
			{
				return (true);
			}

			return (false);
		}

		//The Jupiter in its own rashi(Dhanu/Meena) in the lagna, under the influence of Mars and Mercury.
		//Strong lagna should be there. This combination lead to excessive wealth. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts.
		public static bool DhanaGuruLagna(this Grahas grahas)
		{
			var jupiter = grahas.Find(Body.Jupiter);
			if (jupiter.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (jupiter.IsInOwnHouse == false)
			{
				return (false);
			}

			if (jupiter.IsUnderInfluenceOf(Body.Mars) == false)
			{
				return (false);
			}

			if (jupiter.IsUnderInfluenceOf(Body.Mercury) == false)
			{
				return (false);
			}

			return (true);
		}

		//The Saturn in its own rashi (Makara/Kumbha) in the lagna, under the influence of Mars and Jupiter.
		//Strong lagna should be there. This combination lead to excessive wealth. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts.
		public static bool DhanaShaniLagna(this Grahas grahas)
		{
			var saturn = grahas.Find(Body.Saturn);
			if (saturn.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (saturn.IsInOwnHouse == false)
			{
				return (false);
			}

			if (saturn.IsUnderInfluenceOf(Body.Mars) == false)
			{
				return (false);
			}

			if (saturn.IsUnderInfluenceOf(Body.Jupiter) == false)
			{
				return (false);
			}
			return (true);
		}

		//The Saturn occupying the 5th house identical with its own sign (Makara/Kumbha), and the /Sun and the Moon in the 11th house.
		//This combination lead to excessive wealth. This yoga generally related to one’s profession, it may be desirable to examine the
		//dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool DhanaShani5th(this Grahas grahas)
		{
			var saturn = grahas.Find(Body.Saturn);
			if (saturn.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			if (saturn.IsInOwnHouse == false)
			{
				return (false);
			}

			byte yoga  = 0x00;
			var  rashi = grahas.Rashis.Find(Bhava.LabhaBhava);
			foreach (var graha in rashi.Grahas)
			{
				if (graha == Body.Sun)
				{
					yoga |= 0x01;
				}

				if (graha == Body.Moon)
				{
					yoga |= 0x02;
				}
			}

			return (yoga == 0x03);
		}

		//The Mars occupying the 5th house identical with its own rashi(Mesha/Vrischika), and Venus in the 11th house.
		//This combination lead to excessive wealth. This yoga generally related to one’s profession, it may be desirable
		//to examine the dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool DhanaMangal5th (this Grahas grahas)
		{
			var mars = grahas.Find(Body.Mars);
			if (mars.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			if (mars.IsInOwnHouse == false)
			{
				return (false);
			}

			var venus = grahas.Find(Body.Venus);
			if (venus.Bhava == Bhava.LabhaBhava)
			{
				return (true);
			}

			return (false);
		}

		//The Moon in its own rashi (Karka) in the 5th house, and Saturn in the 11th house.
		//This combination lead to excessive wealth. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool DhanaChandra5th(this Grahas grahas)
		{
			var moon = grahas.Find(Body.Moon);
			if (moon.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			if (moon.IsInOwnHouse == false)
			{
				return (false);
			}
			var saturn = grahas.Find(Body.Saturn);
			if (saturn.Bhava != Bhava.LabhaBhava)
			{
				return (false);
			}

			return (true);
		}

		//The Sun occupying the 5th house identical with its own rashi (Simha), and the Moon, Jupiter and Saturn in the 11th house.
		//This combination lead to excessive wealth. This yoga generally related to one’s profession, it may be desirable to examine the
		//dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there
		public static bool DhanaSurya5th(this Grahas grahas)
		{
			var sun = grahas.Find(Body.Sun);
			if (sun.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (sun.IsInOwnHouse == false)
			{
				return (false);
			}

			byte yoga = 0x00;
			var  rash = grahas.Rashis.Find(Bhava.PutraBhava);
			foreach (var graha in rash.Grahas)
			{
				if (graha == Body.Moon)
				{
					yoga |= 0x01;
				}

				if (graha == Body.Jupiter)
				{
					yoga |= 0x02;
				}

				if (graha == Body.Saturn)
				{
					yoga |= 0x04;
				}
			}

			return (yoga == 0x07);
		}

		//The 9th lord is associtating with the 11th lord.
		//This Yoga is for wealth and financial prosperity. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there
		public static bool Dhana9thLord(this Grahas grahas)
		{
			var lord9  = grahas.Rashis.Find(Bhava.DharmaBhava).Lord;
			var lord11 = grahas.Rashis.Find(Bhava.LabhaBhava).Lord;

			return (lord9.IsAssociatedWith(lord11));
		}

		//The 5th lord is associtating with the 9th or the 11th lord.
		//This Yoga is for wealth and financial prosperity. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts and also strong Lagna should be there.
		public static bool Dhana5thLord(this Grahas grahas)
		{
			var lord5  = grahas.Rashis.Find(Bhava.PutraBhava).Lord;
			var lord9  = grahas.Rashis.Find(Bhava.DharmaBhava).Lord;
			var lord11 = grahas.Rashis.Find(Bhava.LabhaBhava).Lord;

			if (lord5.IsAssociatedWith(lord9))
			{
				return (true);
			}

			if (lord5.IsAssociatedWith(lord11))
			{
				return (true);
			}
			return (false);
		}

		//The Mars in its own rashi(Mesha/Vrischika) in the Lagna, under the influence of Mercury, Venus and Saturn.
		//Strong Lagna should be there. This combination lead to excessive wealth. This yoga generally related to one’s profession,
		//it may be desirable to examine the dashamamsha chart along with the rashi & the navamsha charts.
		public static bool DhanaMangalLagna(this Grahas grahas)
		{
			var mars = grahas.Find(Body.Mars);
			if (mars.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			if (mars.IsInOwnHouse == false)
			{
				return (false);
			}

			if (mars.IsUnderInfluenceOf(Body.Mercury) == false)
			{
				return (false);
			}
			if (mars.IsUnderInfluenceOf(Body.Venus) == false)
			{
				return (false);
			}
			if (mars.IsUnderInfluenceOf(Body.Saturn) == false)
			{
				return (false);
			}
			return (true);
		}

		//Dhana ($$$) Yogas are formed when ANY of the two Lords of Houses1, 2, 5, 9 or 11
		//are in Association (in a house together) or in Mutual Aspect
		//(especially if these Lords are First Tier Strength or “wellplaced”
		public static bool DhanaBhava(this Grahas grahas)
		{
			foreach (var rashi in grahas.Rashis)
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
		public static bool DhanaNight(this Grahas grahas)
		{
			if (grahas.Horoscope.IsDayBirth())
			{
				return (false);
			}

			var navamsha = grahas.Horoscope.FindGrahas( DivisionType.Navamsa);
			var moon     = navamsha.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			moon = grahas.Find(Body.Moon);
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
		public static bool DhanaDay(this Grahas grahas)
		{
			if (grahas.Horoscope.IsDayBirth() == false)
			{
				return (false);
			}

			var navamsha = grahas.Horoscope.FindGrahas( DivisionType.Navamsa);
			var moon     = navamsha.Find(Body.Moon);
			if (moon == null)
			{
				throw new Exception("Navamsha not calculated!");
			}

			moon = grahas.Find(Body.Moon);
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

	}
}
