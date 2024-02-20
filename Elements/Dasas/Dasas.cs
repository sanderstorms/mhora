using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mhora.Definitions;
using Mhora.Elements.Yoga;

namespace Mhora.Elements.Dasas
{
	public static class Dasas
	{
		public enum NakshatraLord
		{
			Vimsottari,
			Ashtottari,
			Yogini,
			Shodashottari,
			Dwadashottari,
			Panchottari,
			Shatabdika,
			ChaturashitiSama,
			DwisaptatiSama,
			ShatTrimshaSama
		}

		public static bool IsValid(this Horoscope h, NakshatraLord dasa)
		{
			switch (dasa)
			{
				case NakshatraLord.Ashtottari:
				{
					var rahu = Graha.Find(Body.Rahu, DivisionType.Rasi);
					if (rahu.Bhava == Bhava.LagnaBhava)
					{
						return (false);
					}

					if (rahu.Bhava.IsKendra() || rahu.Bhava.IsTrikona())
					{
						return (true);
					}

				}
				return (false);

				case NakshatraLord.Dwadashottari:
				{
					var lagna = Graha.Find(Body.Lagna, DivisionType.Navamsa);
					if (lagna.Rashi.Lord == Body.Venus)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.Panchottari:
				{
					var lagna = Graha.Find(Body.Lagna, DivisionType.Dasamsa);
					if (lagna.Rashi == ZodiacHouse.Can)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.Shatabdika:
				{
					var lagnaD1 = Graha.Find(Body.Lagna, DivisionType.Rasi);
					var lagnaD9 = Graha.Find(Body.Lagna, DivisionType.Navamsa);

					if (lagnaD1.Rashi == lagnaD9.Rashi)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.ChaturashitiSama:
				{
					var rashi = Rashi.Find(Bhava.KarmaBhava, DivisionType.Rasi);
					if (rashi.Lord.Bhava == Bhava.KarmaBhava)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.DwisaptatiSama:
				{
					var lagna = Graha.Find(Body.Lagna, DivisionType.Rasi);
					if (lagna.Bhava == Bhava.JayaBhava)
					{
						return (true);
					}

					var lord = Rashi.Find(Bhava.JayaBhava, DivisionType.Rasi).Lord;
					if (lord.Bhava == Bhava.LagnaBhava)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.ShatTrimshaSama:
				{
					var sun = Graha.Find(Body.Sun, DivisionType.HoraParasara);
					if (sun.Rashi == ZodiacHouse.Leo)
					{
						return h.IsDayBirth();
					}
					else
					{
						return (h.IsDayBirth() == false);
					}
				}
				return (false);
			}

			return (true);
		}
	}
}
