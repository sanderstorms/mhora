using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Yoga
{
	public static class Kalsarpa
	{
		public static bool KalsarpaYoga(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			var ketuBhava = rahu.Bhava.Add(7);

			bool left  = false;
			bool right = false;

			foreach (var graha in grahaList.Planets)
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
		public static bool AnantKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.LagnaBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 8th house and Ketu is in 2th house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of a person will become a Scientist, Detective, Geologist or a Tantrik. But he will be wicked in nature.
		public static bool KanktokKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.MrtyuBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 2nd house and Ketu is in 8th house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of a person will become a Scientist, Detective, Geologist or a Tantrik. But he will be wicked in nature
		public static bool KulikKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.DhanaBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 6th house and Ketu is in 12th house and all the planets fall on one side of Rahu/Ketu axis.
		//The person will become a Jyotishi or an Advocate, but he will have a dangerous disease.
		public static bool MahapadmKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.ShatruBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 5th house and Ketu is in 11th house and all the planets fall on one side of Rahu/Ketu axis
		//This person will get child after some troubles and will have obstacles in education as well,
		//will earn in shares or lottries. Earns name in the field of art.
		public static bool PadmaKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 10th house and Ketu is in 4th house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of persons luck rises outside the place the place of his birth, he will be physically weak.
		public static bool PatakKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 9th house and Ketu is in 3rd house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of a person will go on a world tour. He will be a good warrior, a renouned writer or a good player.
		//But these type of people are having some mental troubles and there luck rises after the death of there father.
		public static bool ShankhchudKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 4th house and Ketu is in 10th house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of persons luck rises outside the place the place of his birth, they will be physically weak.
		public static bool ShankhpatKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.KarmaBhava)
			{
				return (false);
			}
			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 12th house and Ketu is in 6th house and all the planets fall on one side of Rahu/Ketu axis.
		//The person will become a Jyotishi or an Advocate, but he will have a dangerous disease.
		public static bool SheshnagKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.VyayaBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 7th house and Ketu is in 1st house and all the planets fall on one side of Rahu/Ketu axis.
		//This is considered to be a good kalsarpa yoga, if this person is a Politician, Journalist, Writer or a Philosopher,
		//then he will earn name and fame. But this person would not have a good married life.
		public static bool TakshakKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.JayaBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();
		}

		//Rahu is in 3rd house and Ketu is in 9th house and all the planets fall on one side of Rahu/Ketu axis.
		//This type of a person will go on a world tour. He will be a good warrior, or a renouned writer or a good player.
		//But these type of people are having some mental troubles, and there luck rises after the death of there father.
		public static bool VasukiKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.DharmaBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();

		}

		//Rahu is in 11th house and Ketu is in 5th house and all the planets fall on one side of Rahu/Ketu axis.
		//This person will get child after some troubles and will have obstacles in education as well,
		//will earn in shares or lottries. Earns name in the field of art.
		public static bool VishaktaKalsarpa(this Grahas grahaList)
		{
			var rahu = grahaList.Find(Body.Rahu);
			if (rahu.Bhava != Bhava.PutraBhava)
			{
				return (false);
			}

			return grahaList.KalsarpaYoga();

		}
	}

}
