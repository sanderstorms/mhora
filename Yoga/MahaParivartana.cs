using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Yoga
{
	public static class MahaParivartana
	{
		//The 9th house lord exchanges houses with the 10th lord or the 11th lord.
		//This combination promises wealth status and physical enjoyments plus beneficial influences from the houses involved.
		public static bool MahaParivartana9(this Grahas grahas)
		{
			var lord9 = grahas.Rashis[Bhava.DharmaBhava].Lord;
			var lord10 = grahas.Rashis[Bhava.KarmaBhava].Lord;
			var lord11 = grahas.Rashis[Bhava.LabhaBhava].Lord;

			if (lord9.Exchange == lord10)
			{
				return (true);
			}

			if (lord9.Exchange == lord11)
			{
				return (true);
			}

			return (false);
		}

		//The 10th house lord exchanges houses with the 11th lord.
		//This combination promises wealth status and physical enjoyments plus beneficial influences from the houses involved.
		public static bool MahaParivartana10(this Grahas grahas)
		{
			var lord10 = grahas.Rashis[Bhava.KarmaBhava].Lord;
			var lord11 = grahas.Rashis[Bhava.LabhaBhava].Lord;
			if (lord10.Exchange == lord11)
			{
				return (true);
			}

			return (false);
		}

		//The Lagna lord exchanges houses with 2nd or 4th or 5th or 7th or 9th or 10th or the 11th lord.
		//This combination promises wealth status and physical enjoyments plus beneficial influences from the houses involved.
		public static bool MahaParivartana1(this Grahas grahas)
		{
			var lord1 = grahas.Rashis[Bhava.LagnaBhava].Lord;

			foreach (var rashi in grahas.Rashis)
			{
				switch (rashi.Bhava)
				{
					case Bhava.DhanaBhava:
					case Bhava.SukhaBhava:
					case Bhava.PutraBhava:
					case Bhava.JayaBhava:
					case Bhava.DharmaBhava:
					case Bhava.KarmaBhava:
					case Bhava.LabhaBhava:
					{
						if (lord1.Exchange == rashi.Lord)
						{
							return (true);
						}
					}
					break;
				}
			}
			return (false);
		}

		//The 2nd house lord exchanges houses with the 4th or the 5th or the 7th or the 9th or the 10th or the 11th lord.
		public static bool MahaParivartana2(this Grahas grahas)
		{
			var lord2 = grahas.Rashis[Bhava.DhanaBhava].Lord;
			foreach (var rashi in grahas.Rashis)
			{
				switch (rashi.Bhava)
				{
					case Bhava.SukhaBhava:
					case Bhava.PutraBhava:
					case Bhava.JayaBhava:
					case Bhava.DharmaBhava:
					case Bhava.KarmaBhava:
					case Bhava.LabhaBhava:
					{
						if (lord2.Exchange == rashi.Lord)
						{
							return (true);
						}
					}
					break;
				}
			}
			return (false);
		}

		//The 4nd house lord exchanges houses with the 5th lord or the 7th lord or the 9th lord or the 10th lord or the 11th lord .
		public static bool MahaParivartana4(this Grahas grahas)
		{
			var lord4 = grahas.Rashis[Bhava.SukhaBhava].Lord;
			foreach (var rashi in grahas.Rashis)
			{
				switch (rashi.Bhava)
				{
					case Bhava.PutraBhava:
					case Bhava.JayaBhava:
					case Bhava.DharmaBhava:
					case Bhava.KarmaBhava:
					case Bhava.LabhaBhava:
					{
						if (lord4.Exchange == rashi.Lord)
						{
							return (true);
						}
					}
						break;
				}
			}
			return (false);
		}


		//The 5th house lord exchanges houses with the 7th lord or the 9th lord or the 10th lord or the 11th lord.
		//This combination promises wealth status and physical enjoyments plus beneficial influences from the houses involved.
		public static bool MahaParivartana5(this Grahas grahas)
		{
			var lord5 = grahas.Rashis[Bhava.PutraBhava].Lord;
			foreach (var rashi in grahas.Rashis)
			{
				switch (rashi.Bhava)
				{
					case Bhava.JayaBhava:
					case Bhava.DharmaBhava:
					case Bhava.KarmaBhava:
					case Bhava.LabhaBhava:
					{
						if (lord5.Exchange == rashi.Lord)
						{
							return (true);
						}
					}
						break;
				}
			}
			return (false);

		}

		//The 7th house lord exchanges houses with the 9th lord or the 10th lord or the 11th lord.
		//This combination promises wealth status and physical enjoyments plus beneficial influences from the houses involved.
		public static bool MahaParivartana7(this Grahas grahas)
		{
			var lord7 = grahas.Rashis[Bhava.JayaBhava].Lord;
			foreach (var rashi in grahas.Rashis)
			{
				switch (rashi.Bhava)
				{
					case Bhava.DharmaBhava:
					case Bhava.KarmaBhava:
					case Bhava.LabhaBhava:
					{
						if (lord7.Exchange == rashi.Lord)
						{
							return (true);
						}
					}
					break;
				}
			}
			return (false);
		}
	}
}
