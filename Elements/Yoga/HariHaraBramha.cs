
using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class HariHaraBramha
	{
		//Benefices placed in house4 8 9 from the lord of the 7nd house.
		public static bool HariHaraBramha7th(this Grahas grahaList)
		{
			var lord = grahaList.Rashis.Find(Bhava.JayaBhava).Lord;

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if ((graha.Bhava.HousesFrom(lord.Bhava) != 4) && 
					    (graha.Bhava.HousesFrom(lord.Bhava) != 8) && 
					    (graha.Bhava.HousesFrom(lord.Bhava) != 9))
					{
						return (false);
					}
				}
			}
			return true;
		}

		//Benefices placed in house 2 8 12 from the lord of the 2nd house.
		public static bool HariHaraBramha2nd(this Grahas grahaList)
		{
			var lord = grahaList.Rashis.Find(Bhava.DhanaBhava).Lord;

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if ((graha.Bhava.HousesFrom(lord.Bhava) != 2) && 
					    (graha.Bhava.HousesFrom(lord.Bhava) != 8) && 
					    (graha.Bhava.HousesFrom(lord.Bhava) != 12))
					{
						return (false);
					}
				}
			}
			return true;

		}

		//Benefices placed in house 4 10 11 from the lord of the Lagna.
		public static bool HariHaraBramha1nd(this Grahas grahaList)
		{
			var lord = grahaList.Rashis.Find(Bhava.LagnaBhava).Lord;

			foreach (var graha in grahaList.Planets)
			{
				if (graha.IsNaturalBenefic)
				{
					if ((graha.Bhava.HousesFrom(lord.Bhava) != 4) && 
					    (graha.Bhava.HousesFrom(lord.Bhava) != 8) && 
					    (graha.Bhava.HousesFrom(lord.Bhava) != 10))
					{
						return (false);
					}
				}
			}
			return true;

		}
	}
}
