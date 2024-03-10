using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Yoga
{
	public static class Shukra
	{
		//Conjunction between Venus and Mars.
		//A cheat, a liar or gambler, addicted to other's wives, opposed to all, skilled in math’s,
		//a shepherd, a wrestler, distinguished among men because of his virtues.
		public static bool ShukraMangal(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			return venus.IsConjuctWith(Body.Mars);
		}

		//Addicted to base women, indulges in probated deeds, wastes money on women, short-lived.
		public static bool ShukraMangal1(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Mars);
		}

		//Worried, miserable, bereft of relatives, friends and children.
		public static bool ShukraMangal4(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Mars);
		}

		//Greedy, immoral, tormented by women.
		public static bool ShukraMangal7(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Mars);
		}

		//Quarrelsome, lives in a foreign land, harsh, not loyal to his wife, cunning, fond of metallurgy.
		public static bool ShukraMangal9(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Mars);
		}

		//A distinguished teacher for use of weapons, wise, blessed, with learning, wealth and fine clothes, famous, a minister.
		public static bool ShukraMangal10(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Mars);
		}

		//A Fighter, a wanderer, a fine sculptor, a writer or painter, an athlete, looks after herds of cattle, shortsighted,
		//his marriage holds the key to his financial prosperity.
		public static bool ShukraShani(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			return venus.IsConjuctWith(Body.Saturn);

		}

		//Associates with many women, handsome, blessed with physical comforts, suffers from mental torment financially.
		public static bool ShukraShani1(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Saturn);

		}

		//Helped by friends, honored by the ruler.
		public static bool ShukraShani4(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.SukhaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Saturn);

		}

		//Blessed with women, wealth, comforts and fame.
		public static bool ShukraShani7(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Saturn);

		}

		//Ailing, liked by the ruler, famous, amiable, blessed with progeny and wealth.
		public static bool ShukraShani9(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Saturn);

		}

		//Widely renowned, of a high status, without worries
		public static bool ShukraShani10(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return venus.IsConjuctWith(Body.Saturn);

		}

		//Renowned, famous, of clear intellect, equal to a king even if born under ordinary circumstances.
		public static bool ShukraGuruShani(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.IsConjuctWith(Body.Jupiter) == false)
			{
				return (false);
			}

			return venus.IsConjuctWith(Body.Saturn);
		}

		//Blessed with wife, children and comforts, liked by the ruler, associates with good people.
		public static bool ShukraMangalGuru(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			return venus.IsConjuctWith(Body.Jupiter);
		}

		//Foreign residence, bad children, suffers humiliation at the hands of a pretty woman.
		public static bool ShukraMangalShani(this Grahas grahaList)
		{
			var venus = grahaList.Find(Body.Venus);
			if (venus.IsConjuctWith(Body.Mars) == false)
			{
				return (false);
			}

			return venus.IsConjuctWith(Body.Saturn);
		}
	}
}
