
using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class HariHaraBramha
	{
		//Benefices placed in house4 8 9 from the lord of the 7nd house.
		public static bool HariHaraBramha7th(this DivisionType varga)
		{
			var lord = Rashi.Find(Bhava.JayaBhava, varga).Lord;

			foreach (var graha in Graha.Planets(varga))
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
		public static bool HariHaraBramha2nd(this DivisionType varga)
		{
			var lord = Rashi.Find(Bhava.DhanaBhava, varga).Lord;

			foreach (var graha in Graha.Planets(varga))
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
		public static bool HariHaraBramha1nd(this DivisionType varga)
		{
			var lord = Rashi.Find(Bhava.LagnaBhava, varga).Lord;

			foreach (var graha in Graha.Planets(varga))
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
