using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class MahaParivartana
	{
		//The 9th house lord exchanges houses with the 10th lord or the 11th lord.
		//This combination promises wealth status and physical enjoyments plus beneficial influences from the houses involved.
		public static bool MahaParivartana9(this DivisionType varga)
		{
			var lord9 = Rashi.Find(Bhava.DharmaBhava, varga).Lord;
			var lord10 = Rashi.Find(Bhava.KarmaBhava, varga).Lord;
			var lord11 = Rashi.Find(Bhava.LabhaBhava, varga).Lord;

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
		public static bool MahaParivartana10(this DivisionType varga)
		{
			var lord10 = Rashi.Find(Bhava.KarmaBhava, varga).Lord;
			var lord11 = Rashi.Find(Bhava.LabhaBhava, varga).Lord;
			if (lord10.Exchange == lord11)
			{
				return (true);
			}

			return (false);
		}

		//The Lagna lord exchanges houses with 2nd or 4th or 5th or 7th or 9th or 10th or the 11th lord.
		//This combination promises wealth status and physical enjoyments plus beneficial influences from the houses involved.
		public static bool MahaParivartana1(this DivisionType varga)
		{
			var lord1 = Rashi.Find(Bhava.LagnaBhava, varga).Lord;

			foreach (var rashi in Rashi.Rashis(varga))
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
		public static bool MahaParivartana2(this DivisionType varga)
		{
			var lord2 = Rashi.Find(Bhava.DhanaBhava, varga).Lord;
			foreach (var rashi in Rashi.Rashis(varga))
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
		public static bool MahaParivartana4(this DivisionType varga)
		{
			var lord4 = Rashi.Find(Bhava.SukhaBhava, varga).Lord;
			foreach (var rashi in Rashi.Rashis(varga))
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
		public static bool MahaParivartana5(this DivisionType varga)
		{
			var lord5 = Rashi.Find(Bhava.PutraBhava, varga).Lord;
			foreach (var rashi in Rashi.Rashis(varga))
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
		public static bool MahaParivartana7(this DivisionType varga)
		{
			var lord7 = Rashi.Find(Bhava.JayaBhava, varga).Lord;
			foreach (var rashi in Rashi.Rashis(varga))
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
