using Mhora.Definitions;

namespace Mhora.Yoga
{
	public static class Ravi
	{
		//This yoga is caused when planets other than the Moon occupy the 2 and 12 houses from the Sun.
		//The resultant native is of a strong physique, a king or equal to a king, great learning, balanced outlook,
		//wealthy, handsome and blessed with numerous objects of pleasure.
		public static bool UbhaychariYoga(this Grahas grahaList, Body body1, Body body2)
		{
			byte yoga   = 0x00;
			var  graha1   = grahaList.Find(body1);
			var  graha2 = grahaList.Find(body2);

			if ((graha1.HouseFrom(Body.Sun) == Bhava.DhanaBhava) ||
			    (graha2.HouseFrom(Body.Sun)  == Bhava.DhanaBhava))
			{
				yoga |= 0x01;
			}
			else if ((graha1.HouseFrom(Body.Sun) == Bhava.VyayaBhava) ||
			         (graha2.HouseFrom(Body.Sun)  == Bhava.VyayaBhava))
			{
				yoga |= 0x02;
			}

			return (yoga == 0x03);
		}

		//This arises when any planet other than the Moon occupies the 2nd house from the Sun.
		//A native with this yoga is truthful, lazy, kind-hearted, virtuously disposed,
		//having a tall stature and a balanced outlook, good memory.
		public static bool VeshiYoga(this Grahas grahaList, Body body)
		{
			var graha = grahaList.Find(body);
			return (graha.HouseFrom(Body.Sun) == Bhava.DhanaBhava);
		}

		//This yoga arises when any planet, other than Moon, occupies the 12 house from the Sun.
		//One born in this yoga exercises no restraint on his speech. He has good learning,
		//wide renown, sharp memory, a charitable nature.
		public static bool VoshiYoga(this Grahas grahaList, Body body)
		{
			var graha = grahaList.Find(body);
			return (graha.HouseFrom(Body.Sun) == Bhava.VyayaBhava);
		}

		//When Mars and Saturn occupy the 2 and 12 houses from the Sun.
		//the native is sickly, servile, destitute and of a wicked disposition.
		public static bool RaviUbhaychari1(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Mars, Body.Saturn);

		//When Mercury and Venus occupy the 2 and 12 houses from the Sun.
		//Native earns Name and fame in foreign countries, and is blessed with all comforts.
		public static bool RaviUbhaychari2(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Mercury, Body.Venus);

		//When Mars and Jupiter occupy the 2 and 12 houses from the Sun.
		//The resultant native is of a leader, very intelligent, skilled in use of weapons, wealthy,
		//handsome and blessed with numerous objects of pleasure.
		public static bool RaviUbhaychari3(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Mercury, Body.Jupiter);

		//When Venus and Saturn occupy the 2 and 12 houses from the Sun
		//The resultant native is of a strong physique, a king or equal to a king, great learning,
		//balanced outlook, wealthy, handsome and blessed with numerous objects of pleasure.
		public static bool RaviUbhaychari5(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Venus, Body.Saturn);

		//When Venus and Mars occupy the 2 and 12 houses from the Sun.
		//The resultant native is of a strong physique, balanced outlook, wealthy, handsome
		//and distinguished among men because of his virtues and blessed with numerous objects of pleasure.
		public static bool RaviUbhaychari6(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Venus, Body.Mars);

		//When Jupiter and Saturn occupy the 2 and 12 houses from the Sun
		//The resultant native is of a Strong, famous leader of a group or in army, wealthy,
		//and blessed with numerous objects of pleasure.
		public static bool RaviUbhaychari7(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Jupiter, Body.Saturn);

		//When Mercury and Mars occupy the 2 and 12 houses from the Sun.
		//The resultant native is of a strong physique, a king or equal to a king, great learning,
		//balanced outlook, wealthy, expert in Mathematics and blessed with numerous objects of pleasure.
		public static bool RaviUbhaychari8(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Mercury, Body.Mars);

		//When Mercury and Jupiter occupy the 2 and 12 houses from the Sun.
		//The resultant native is of a strong physique, a king or equal to a king, highly learned, honoured by king,
		//wealthy, Sharp memory, known among people and blessed with numerous objects of pleasure.
		public static bool RaviUbhaychari9(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Mercury, Body.Jupiter);

		//When Mercury and Saturn occupy the 2 and 12 houses from the Sun.
		//The resultant native earns money by technical job, great learning, fickle minded, adept in several arts
		//disobedient to his elders, a cheat, wealthy, and blessed with numerous objects of pleasure.
		public static bool RaviUbhaychari10(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Mercury, Body.Saturn);

		//When Venus and Jupiter occupy the 2 and 12 houses from the Sun.
		//Very learned blessed with virtuous wife, wealthy, religious,
		//earns his living through the use of his learning, and blessed with all comforts.
		public static bool RaviUbhaychari11(this Grahas grahaList) => grahaList.UbhaychariYoga(Body.Venus, Body.Jupiter);

		//Of a steady nature, truthful, wise, undaunted in battle.
		public static bool RaviVeshi1 (this Grahas grahaList) => grahaList.VeshiYoga(Body.Jupiter);

		//Valorous in battle, a charioteer, renowned.
		public static bool RaviVeshi2(this Grahas grahaList) => grahaList.VeshiYoga(Body.Mars);
			
		//Renowned, Respectable, with many virtues, intrepid.
		public static bool RaviVeshi3(this Grahas grahaList) => grahaList.VeshiYoga(Body.Venus);

		//Sweet tongued, handsome, capable of befooling others.
		public static bool RaviVeshi4(this Grahas grahaList) => grahaList.VeshiYoga(Body.Mercury);

		//Interested in Business, inclined to cheat others of their wealth, with malice towards his preceptors.	
		public static bool RaviVeshi5(this Grahas grahaList) => grahaList.VeshiYoga(Body.Saturn);

		//Hostile to mother, doing good to other’s.
		public static bool RaviVoshi1 (this Grahas grahaList) => grahaList.VoshiYoga(Body.Mars);

		//A hoarder, illustrious like the sunlit day.
		public static bool RaviVoshi2 (this Grahas grahaList) => grahaList.VoshiYoga(Body.Jupiter);

		//To all appearance poor, devoid of physical strength, shameless
		public static bool RaviVoshi3 (this Grahas grahaList) => grahaList.VoshiYoga(Body.Mercury);

		//Addicted to women of other’s, kind- hearted, exhausted, of mature looks
		public static bool RaviVoshi4 (this Grahas grahaList) => grahaList.VoshiYoga(Body.Saturn);

		//Cowardly, lustful, without enthusiasm, servile.
		public static bool RaviVoshi5 (this Grahas grahaList) => grahaList.VoshiYoga(Body.Venus);
	}
}
