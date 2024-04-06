using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Yoga
{
	public static class Mangal
	{
		//Conjunction between Mars and Jupiter.
		//Learned, revered, wealthy, very intelligent, skillful lecturer,
		//a sculpture, skilled in the use of weapons, memories by mere listening, a leader.
		public static bool MangalGuru(this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			return (mars.IsConjuctWith(Body.Jupiter));
		}

		//Mars and Jupiter are conjunct in Lagna.
		//Distinguished, talented, virtuous, courageous.
		public static bool MangalGuru1(this Grahas grahaList)
		{
			if (grahaList.MangalGuru () == false)
			{
				return (false);
			}

			if (grahaList.Find(Body.Mars).Bhava != Bhava.LagnaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Jupiter are conjunct in 4th House.
		//Blessed with friends and relatives, consistent, devoted to gods and Brahmins.
		public static bool MangalGuru4(this Grahas grahaList)
		{
			if (grahaList.MangalGuru () == false)
			{
				return (false);
			}

			if (grahaList.Find(Body.Mars).Bhava != Bhava.SukhaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Jupiter are conjunct in 7th House.
		public static bool MangalGuru7(this Grahas grahaList)
		{
			if (grahaList.MangalGuru () == false)
			{
				return (false);
			}

			if (grahaList.Find(Body.Mars).Bhava != Bhava.JayaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Jupiter are conjunct in 9th House.
		//Worship-worthy, royal, ailing, harsh, scarred with family.
		public static bool MangalGuru9(this Grahas grahaList)
		{
			if (grahaList.MangalGuru() == false)
			{
				return (false);
			}

			if (grahaList.Find(Body.Mars).Bhava != Bhava.DharmaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Jupiter are conjunct in 10th House.
		//A Highly renowned, very wealthy, wise, with a large family.
		public static bool MangalGuru10(this Grahas grahaList)
		{
			if (grahaList.MangalGuru() == false)
			{
				return (false);
			}

			if (grahaList.Find(Body.Mars).Bhava != Bhava.KarmaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Conjunction between Mars and Saturn.
		//Miserable, quarrelsome, condemnable, a betrayer, clever in talking, versed in the use of weapons,
		//follower of a faith other than his own, has danger from poison or injury.
		public static bool MangalShani(this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			return (mars.IsConjuctWith(Body.Saturn));

		}

		//Mars and Saturn are conjunct in Lagna.
		//Winner in battle, hostile to mother, short-lived.
		public static bool MangalShani1(this Grahas grahaList)
		{
			if (grahaList.MangalShani() == false)
			{
				return (false);
			}
			if (grahaList.Find(Body.Mars).Bhava != Bhava.LagnaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Saturn are conjunct in 4th housoe.
		//Bereft of home comforts, given up by near and dear ones, a sinner.
		public static bool MangalShani4(this Grahas grahaList)
		{
			if (grahaList.MangalShani() == false)
			{
				return (false);
			}
			if (grahaList.Find(Body.Mars).Bhava != Bhava.SukhaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Saturn are conjunct in 7th house.
		//Devoid of comforts from wife and children, ailing, indulges in sinful deeds.
		public static bool MangalShani7(this Grahas grahaList)
		{
			if (grahaList.MangalShani() == false)
			{
				return (false);
			}
			if (grahaList.Find(Body.Mars).Bhava != Bhava.JayaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Saturn are conjunct in 9th house.
		//Earns from the ruler, suffers punishment(for a crime).
		public static bool MangalShani9(this Grahas grahaList)
		{
			if (grahaList.MangalShani() == false)
			{
				return (false);
			}
			if (grahaList.Find(Body.Mars).Bhava != Bhava.DharmaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Mars and Saturn are conjunct in 10th house.
		public static bool MangalShani10(this Grahas grahaList)
		{
			if (grahaList.MangalShani() == false)
			{
				return (false);
			}
			if (grahaList.Find(Body.Mars).Bhava != Bhava.KarmaBhava)
			{
				return (true);
			}

			return (false);
		}

		//Conjunction between Mars, Mercury, Jupiter, and Saturn.
		//Valorous, learned, eloquent, truthful, pious, poor.
		public static bool MangalBuddhGuruShani(this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			if (mars.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return (true);
		}


		//Conjunction between Mars, Mercury, Jupiter and Venus.
		//Rich, healthy, highly esteemed, involved in quarrels with women.
		public static bool MangalBuddhGuruShukra(this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			if (mars.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			return (true);
		}

		//Mars, Mercury, Jupiter, Venus and Saturn are conjunct.
		//Easily angered, authorized to render imprisonment and capital punishment, liked by the king, lazy, sickly, suffers from madness.
		public static bool MangalBuddhGuruShukraShani(this Grahas grahaList)
		{
			if (grahaList.MangalBuddhGuruShukra() == false)
			{
				return (false);
			}

			return grahaList.MangalShani();
		}

		//Conjunction between Mars, Mercury, Venus, and Saturn.
		//Skilled in battle, a wrestler, very healthy, renowned, keeps dogs as pets.
		public static bool MangalBuddhShukraShani(this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			if (mars.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return (true);
		}

		//Conjunction between Mars, Jupiter, and Saturn.
		//Lean-bodied, cruel, wicked, bereft of friends, conceited, favored by the ruler.
		public static bool MangalGuruShani(this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			if (mars.IsConjuctWith(Body.Mercury) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return (true);

		}

		//Conjunction between Mars, Jupiter, Venus, and Saturn.
		//Illustrious, wealthy, courageous, addicted to other people’s wives.
		public static bool MangalGuruShukraShani(this Grahas grahaList)
		{
			var mars = grahaList.Find(Body.Mars);
			if (mars.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Venus) == false)
			{
				return (false);
			}

			if (mars.IsConjuctWith(Body.Saturn) == false)
			{
				return (false);
			}

			return (true);
		}

	}
}
