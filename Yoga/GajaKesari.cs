using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Yoga
{
	public static class GajaKesari
	{
		public static bool GajaKesariChandra(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.IsConjuctWith(Body.Moon))
			{
				if (jupiter.Bhava.IsKendra())
				{
					return (true);
				}

				if (jupiter.Bhava.IsTrikona())
				{
					return (true);
				}
			}

			return (false);

		}

		//Jupiter and Moon are Conjunct at 1th house.
		//The person is good-looking blessed with friends spouse and progeny.
		// The person will enjoy health is dignified and impressive
		public static bool GajaKesariChandra1(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.IsConjuctWith(Body.Moon))
			{
				return (jupiter.Bhava == Bhava.LagnaBhava);
			}

			return (false);
		}

		//Jupiter and Moon are Conjunct at 4th house.
  		//This combination should be very favorable for domestic comforts and the mother of the person.
		//But according to Mansagari this Yoga deprives one of comforts at home leads to troubles in respect of the mother
		public static bool GajaKesariChandra4(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.IsConjuctWith(Body.Moon))
			{
				return (jupiter.Bhava == Bhava.SukhaBhava);
			}

			return (false);

		}
		//Jupiter and Moon are Conjunct at 7th house.
		//The person is learned skilled a trader wealthy and will experience conjugal bliss.
		//The person will have luck and obtain wealth through business partnerships.
		public static bool GajaKesariChandra7(this Grahas grahaList)
		{

			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.IsConjuctWith(Body.Moon))
			{
				return (jupiter.Bhava == Bhava.JayaBhava);
			}

			return (false);
		}

		//Jupiter and Moon are Conjunct at 9th house.
		//The person is a scholar wealthy, haughty, respectable etc.
		//This is a good combination for career prospects high status and financial prosperity.
		public static bool GajaKesariChandra9(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.IsConjuctWith(Body.Moon))
			{
				return (jupiter.Bhava == Bhava.DharmaBhava);
			}

			return (false);

		}

		//Jupiter and Moon are Conjunct at 10th house.
		//The person is distinguished fortunate wealthy contented and will have great fortune.
		//There will be good luck as well as virtuous activities and religious pursuits.
		public static bool GajaKesariChandra10(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			if (jupiter.IsConjuctWith(Body.Moon))
			{
				return (jupiter.Bhava == Bhava.KarmaBhava);
			}

			return (false);
		}

		//Jupiter 7th house away from Moon.
		//The person will have good health and a long life respected by his family members and an inclination to be frugal.
		//Mansagari adds that the person will become impotent and suffer from jaundice but in general this Yoga confers conjugal harmony.
		public static bool GajaKesari7Chandra(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			var moon    = grahaList.Find(Body.Moon);
			if (jupiter.Bhava.HousesFrom(moon.Bhava) == 7)
			{
				return (true);
			}
			return (false);
		}

		//Jupiter 10th house away from Moon.
		//The person will have a very favorable career attains a dignified position and wealth. There could also be strong idealism and spiritual pursuits.
		//Mansagari maintains that a person with this Yoga will give up a spouse and children to become an ascetic.
		public static bool GajaKesari10Chandra(this Grahas grahaList)
		{
			var jupiter = grahaList.Find(Body.Jupiter);
			var moon    = grahaList.Find(Body.Moon);
			if (jupiter.Bhava.HousesFrom(moon.Bhava) == 10)
			{
				return (true);
			}
			return (false);
		}
	}
}
