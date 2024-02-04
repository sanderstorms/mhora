using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Yoga
{
	public static class Malika
	{
		//All seven planets occupying one house each from the lagna to seventh house starting with the 11th house.
		//Very competent, blessed with lovely women, of royal mien.
		public static bool LabhaMalika(this DivisionType varga)
		{
			foreach (var graha in Graha.Planets(varga))
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if ((graha.Bhava > Bhava.JayaBhava) && (graha.Bhava < Bhava.LabhaBhava))
				{
					return (false);
				}
			}

			return (true);
		}

		//All seven planets occupying one house each from the lagna to seventh house starting with the Second House.
		//A wealthy king, devoted to his parents, aggressive, resolute, unsympathetic, virtuous.
		public static bool DhanuMalika(this DivisionType varga)
		{
			var  planets = Graha.Planets(varga);
			byte yoga    = 0x00;
			foreach (var graha in planets)
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if ((graha.Bhava > Bhava.DhanaBhava) && (graha.Bhava < Bhava.JayaBhava))
				{
					return (false);
				}

				if ((graha.Bhava >= Bhava.JayaBhava) && (graha.Bhava <= Bhava.VyayaBhava))
				{
					yoga |= (byte) (1 << (graha.Bhava.Index() - 7));
				}
				else if (graha.Bhava == Bhava.LagnaBhava)
				{
					yoga |= 0x80;
				}
			}

			return (yoga == 0xF7);
		}



		//All seven planets occupying one house each from the lagna to seventh house starting with the 9th house.
		//Powerful well to do, saintly, a devout performer of sacrifices.
		public static bool BhagyaMalika(this DivisionType varga)
		{
			var  planets = Graha.Planets(varga);
			byte yoga    = 0x00;
			foreach (var graha in planets)
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if ((graha.Bhava > Bhava.JayaBhava) && (graha.Bhava < Bhava.DharmaBhava))
				{
					return (false);
				}

				if ((graha.Bhava >= Bhava.LagnaBhava) && (graha.Bhava <= Bhava.JayaBhava))
				{
					yoga |= (byte) (1 << graha.Bhava.Index());
				}
			}

			return (yoga == 0xF7);
		}


		//All seven planets occupying one house each from the lagna to seventh house starting with the Lagna.
		public static bool LagnaMalika(this DivisionType varga)
		{
			foreach (var graha in Graha.Planets(varga))
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if (graha.Bhava > Bhava.JayaBhava)
				{
					return (false);
				}
			}
			return (true);
		}

		//All seven planets occupying one house each from the lagna to seventh house starting with the 7th house.
		//Long-lived, a king associating with several wives (spouses), influential.
		public static bool KalatraMalika(this DivisionType varga)
		{
			var planets = Graha.Planets(varga);

			if (planets[0].Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			for (int index = 1; index < planets.Count; index++)
			{
				if (planets[index].Bhava.Index() != index + 6)
				{
					return (false);
				}
			}

			return (true);
		}

		//All seven planets occupying one house each from the lagna to seventh house starting with the 10th house.
		//Virtuous highly esteemed, given to good deeds.
		public static bool KarmaMalika(this DivisionType varga)
		{
			var  planets = Graha.Planets(varga);
			byte yoga    = 0x00;
			foreach (var graha in planets)
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if ((graha.Bhava > Bhava.JayaBhava) && (graha.Bhava < Bhava.KarmaBhava))
				{
					return (false);
				}

				if ((graha.Bhava >= Bhava.LagnaBhava) && (graha.Bhava <= Bhava.JayaBhava))
				{
					yoga |= (byte) (1 << graha.Bhava.Index());
				}
			}

			return (yoga == 0xF7);
		}
	}
}
