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
			if (Rashi.Find(Bhava.LabhaBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

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
			if (Rashi.Find(Bhava.DhanaBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

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
			if (Rashi.Find(Bhava.DharmaBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

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
			if (Rashi.Find(Bhava.LagnaBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

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
			if (Rashi.Find(Bhava.JayaBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

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
			if (Rashi.Find(Bhava.KarmaBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

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

		//All seven planets occupying one house each from the lagna to seventh house starting with the 5th house.
		//Famous religious, performing sacrificial rituals.
		public static bool PutraMalika(this DivisionType varga)
		{
			if (Rashi.Find(Bhava.PutraBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

			var  planets = Graha.Planets(varga);
			byte yoga    = 0x00;
			foreach (var graha in planets)
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if ((graha.Bhava > Bhava.LagnaBhava) && (graha.Bhava < Bhava.PutraBhava))
				{
					return (false);
				}

				if ((graha.Bhava >= Bhava.JayaBhava) || (graha.Bhava == Bhava.LagnaBhava))
				{
					yoga |= (byte) (1 << graha.Bhava.Index());
				}
			}

			return (yoga == 0xF7);

		}

		//All seven planets occupying one house each from the lagna to seventh house starting with the 8th house.
		//Long-lived, distinguished, poor and henpecked.
		public static bool RandhraMalika(this DivisionType varga)
		{
			if (Rashi.Find(Bhava.MrtyuBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

			var  planets = Graha.Planets(varga);
			byte yoga    = 0x00;
			foreach (var graha in planets)
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if ((graha.Bhava >= Bhava.LagnaBhava) || (graha.Bhava <= Bhava.JayaBhava))
				{
					yoga |= (byte) (1 << graha.Bhava.Index());
				}
			}

			return (yoga == 0xF7);
		}

		//All seven planets occupying one house each from the lagna to seventh house starting with the 6th house.
		public static bool SatruMalika(this DivisionType varga)
		{
			if (Rashi.Find(Bhava.ShatruBhava, varga).Grahas.Count != 1)
			{
				return (false);
			}

			var  planets = Graha.Planets(varga);
			byte yoga    = 0x00;
			foreach (var graha in planets)
			{
				if (graha.IsConjunctWithPlanet)
				{
					return (false);
				}

				if ((graha.Bhava > Bhava.LagnaBhava) && (graha.Bhava < Bhava.ShatruBhava))
				{
					return (false);
				}

				if ((graha.Bhava >= Bhava.JayaBhava) || (graha.Bhava == Bhava.LagnaBhava))
				{
					yoga |= (byte) (1 << graha.Bhava.Index());
				}
			}

			return (yoga == 0xF7);

		}
	}
}
