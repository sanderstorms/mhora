using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	//Shubh Sankhya Yoga: This yoga is a part of Nabhas Yoga combination where Shubh Sankhya Yoga is formed
	//when the planets in both paired houses are natural benefics, i.e., Moon, Mercury, Venus, or Jupiter.
	// 
	// Papa Sankhya Yoga: It is formed if the planets in both paired houses are natural malefics i.e.,
	// Sun, Mars, Saturn, Rahu, or Ketu. Mishra Sankhya Yoga forms with one or more benefic planet,
	// and in the other paired house there is only one or more malefics.
	public static class Sankhya
	{
		public static bool SankhyaYoga(this DivisionType varga, int count)
		{
			int yoga = 0;

			foreach (var graha in Graha.Planets(varga))
			{
				yoga |= (ushort) (1 << graha.Bhava.Index());
			}

			return (yoga.NumberOfSetBits() == count);
		}

		//The native is wealthy, liberal, renowned, learned, contented.
		public static bool SankhyaDaama(this DivisionType varga) => varga.SankhyaYoga(6);

		//The native with such a yoga is destitute, illiterate, wicked, miserable, ever wandering.
		public static bool SankhyaGola(this DivisionType varga) => varga.SankhyaYoga(1);

		//The native with this yoga is truthful, wealthy, virtues, doing good to others, resorts to agriculture.
		public static bool SankhyaKedaara(this DivisionType varga) => varga.SankhyaYoga(4);

		//One born in this yoga has a large family, is adept in work, skillful in earning wealth,
		//impolite, fond of dwelling forests, drawbacks.
		public static bool SankhyaPaasha(this DivisionType varga) => varga.SankhyaYoga(5);

		//The native is lazy, cruel, socially rejected, injured, and scarred, a fighter.
		public static bool SankhyaShoola(this DivisionType varga) => varga.SankhyaYoga(3);

		//The native is prosperous, versed in scared scriptures, fond of dance and music, a leader,
		//looks after the sustenance of many.
		public static bool SankhyaVeena(this DivisionType varga) => varga.SankhyaYoga(7);

		//The native is poor, heretic, shameless, socially rejected, devoid of mother, father and virtue.
		public static bool SankhyaYuga(this DivisionType varga) => varga.SankhyaYoga(2);
	}
}
