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
			var grahas  = h.FindGrahas(DivisionType.Rasi);

			switch (dasa)
			{
				case NakshatraLord.Ashtottari:
				{
					var rahu = grahas.Find(Body.Rahu);
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
					var navamsa = h.FindGrahas(DivisionType.Navamsa);
					var lagna   = navamsa.Find(Body.Lagna);
					if (lagna.Rashi.Lord == Body.Venus)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.Panchottari:
				{
					var dasamsa = h.FindGrahas(DivisionType.Dasamsa);
					var lagna   = dasamsa.Find(Body.Lagna);
					if (lagna.Rashi == ZodiacHouse.Can)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.Shatabdika:
				{
					var navamsa = h.FindGrahas(DivisionType.Navamsa);
					var lagnaD1 = grahas.Find(Body.Lagna);
					var lagnaD9 = navamsa.Find(Body.Lagna);

					if (lagnaD1.Rashi == lagnaD9.Rashi)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.ChaturashitiSama:
				{
					var rashi = grahas.Rashis.Find(Bhava.KarmaBhava);
					if (rashi.Lord.Bhava == Bhava.KarmaBhava)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.DwisaptatiSama:
				{
					var lagna = grahas.Find(Body.Lagna);
					if (lagna.Bhava == Bhava.JayaBhava)
					{
						return (true);
					}

					var lord = grahas.Rashis.Find(Bhava.JayaBhava).Lord;
					if (lord.Bhava == Bhava.LagnaBhava)
					{
						return (true);
					}
				}
				return (false);

				case NakshatraLord.ShatTrimshaSama:
				{
					var hora = h.FindGrahas(DivisionType.HoraParasara);
					var sun  = hora.Find(Body.Sun);
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
