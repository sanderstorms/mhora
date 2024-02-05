﻿using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class Kalsarpa
	{
		public static bool KalsarpaYoga(this DivisionType varga)
		{
			var rahu = Graha.Find(Body.Rahu, varga);
			var ketuBhava = rahu.Bhava.Add(7);

			bool left  = false;
			bool right = false;

			foreach (var graha in Graha.Planets(varga))
			{
				if (graha.Bhava == rahu.Bhava)
				{
					if (graha.IsBefore(rahu))
					{
						left = true;
					}
					else
					{
						right = true;
					}
				}
				else if (graha.Bhava == ketuBhava)
				{
					if (graha.IsBefore(Body.Ketu))
					{
						right = true;
					}
					else
					{
						left = true;
					}
				}
				else
				{
					if (graha.Bhava.HousesFrom(rahu.Bhava) < 7)
					{
						left = true;
					}
					else
					{
						right = true;
					}
				}
			}

			return (left != right);
		}


		//Rahu is in 1st house and Ketu is in 7th house and all the grahas fall on one side of Rahu/Ketu axis
		public static bool AnantKalsarpa(this DivisionType varga)
		{
			var rahu = Graha.Find(Body.Rahu, varga);
			if (rahu.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return varga.KalsarpaYoga();
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

			return varga.KalsarpaYoga();
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

			return varga.KalsarpaYoga();
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

			return varga.KalsarpaYoga();
		}

		//Rahu is in 5th house and Ketu is in 11th house and all the planets fall on one side of Rahu/Ketu axis
		//This person will get child after some troubles and will have obstacles in education as well,
		//will earn in shares or lottries. Earns name in the field of art.
		public static bool PadmaKalsarpa(this DivisionType varga)
		{
			var rahu = Graha.Find(Body.Rahu, varga);
			if (rahu.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			return varga.KalsarpaYoga();
		}
	}

}
