using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class Kalsarpa
	{
		//Rahu is in 1st house and Ketu is in 7th house and all the grahas fall on one side of Rahu/Ketu axis
		public static bool AnantKalsarpa(this DivisionType varga)
		{
			var  grahas = Graha.Planets(varga);
			bool left   = true;
			bool right  = true;

			var rahu = Graha.Find(Body.Rahu, varga);
			if (rahu.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			foreach (var graha in Graha.Planets(varga))
			{
				left  |= (graha.Bhava <= Bhava.JayaBhava);
				right |= (graha.Bhava > Bhava.JayaBhava);
			}

			return (left |right);
		}

		//Rahu is in 8th house and Ketu is in 2th house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of a person will become a Scientist, Detective, Geologist or a Tantrik. But he will be wicked in nature.
		public static bool KanktokKalsarpa(this DivisionType varga)
		{
			var rahu = Graha.Find(Body.Rahu, varga);
			if (rahu.Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			bool yoga = true;
			foreach (var graha in Graha.Planets(varga))
			{
				if ((graha.Bhava <= Bhava.DhanaBhava) || (graha.Bhava >= Bhava.MrtyuBhava))
				{
					if (graha.Bhava != Bhava.LagnaBhava)
					{
						yoga = false;
						break;
					}
				}
			}

			if (yoga)
			{
				return (true);
			}

			yoga = true;
			foreach (var graha in Graha.Planets(varga))
			{
				if ((graha.Bhava >= Bhava.DhanaBhava) || (graha.Bhava <= Bhava.MrtyuBhava))
				{
					yoga = false;
					break;
				}
			}

			return (yoga);
		}

		//Rahu is in 2nd house and Ketu is in 8th house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of a person will become a Scientist, Detective, Geologist or a Tantrik. But he will be wicked in nature
		public static bool KulikKalsarpa(this DivisionType varga)
		{
			var rahu = Graha.Find(Body.Rahu, varga);
			if (rahu.Bhava != Bhava.DhanaBhava)
			{
				return (false);
			}

			bool yoga = true;
			foreach (var graha in Graha.Planets(varga))
			{
				if ((graha.Bhava <= Bhava.DhanaBhava) || (graha.Bhava >= Bhava.MrtyuBhava))
				{
					if (graha.Bhava != Bhava.LagnaBhava)
					{
						yoga = false;
						break;
					}
				}
			}

			if (yoga)
			{
				return (true);
			}

			yoga = true;
			foreach (var graha in Graha.Planets(varga))
			{
				if ((graha.Bhava >= Bhava.DhanaBhava) || (graha.Bhava <= Bhava.MrtyuBhava))
				{
					yoga = false;
					break;
				}
			}

			return (yoga);
		}

		//Rahu is in 6th house and Ketu is in 12th house and all the planets fall on one side of Rahu/Ketu axis.
		//The person will become a Jyotishi or an Advocate, but he will have a dangerous disease.
		public static bool MahapadmKalsarpa(this DivisionType varga)
		{
			var rahu = Graha.Find(Body.Rahu, varga);
			if (rahu.Bhava != Bhava.ShatruBhava)
			{
				return (false);
			}

			bool yoga = true;
			foreach (var graha in Graha.Planets(varga))
			{
				if ((graha.Bhava <= Bhava.ShatruBhava) || (graha.Bhava >= Bhava.LabhaBhava))
				{
					if (graha.Bhava != Bhava.LagnaBhava)
					{
						yoga = false;
						break;
					}
				}
			}

			if (yoga)
			{
				return (true);
			}

			yoga = true;
			foreach (var graha in Graha.Planets(varga))
			{
				if ((graha.Bhava >= Bhava.ShatruBhava) || (graha.Bhava <= Bhava.LabhaBhava))
				{
					yoga = false;
					break;
				}
			}

			return (yoga);
		}
	}

}
