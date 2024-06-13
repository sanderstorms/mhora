using System;
using Mhora.Components;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Dasas
{
	public static class Dasas
	{
		public static bool IsValid(this Horoscope h, NakshatraLord dasa)
		{
			var grahas  = h.FindGrahas(DivisionType.Rasi);

			switch (dasa)
			{
				case NakshatraLord.Ashtottari:
				{
					var rahu = grahas[Body.Rahu];
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
				//Lagna is in Venusian Amsa
				case NakshatraLord.Dwadashottari:
				{
					var navamsa = h.FindGrahas(DivisionType.Navamsa);
					var lagna   = navamsa[Body.Lagna];
					if (lagna.Rashi.Lord == Body.Venus)
					{
						return (true);
					}
				}
				return (false);
				//langa has Cancer in D1 and Cancer is rising in D12 divisional chart as well
				case NakshatraLord.Panchottari:
				{
					var dasamsa = h.FindGrahas(DivisionType.Dasamsa);
					var lagna   = dasamsa[Body.Lagna];
					if (lagna.Rashi == ZodiacHouse.Can)
					{
						return (true);
					}
				}
				return (false);
				//Lagna is Virgottam. Meaning the same sign rising in the ascendant and the Navamsa. 
				case NakshatraLord.Shatabdika:
				{
					var navamsa = h.FindGrahas(DivisionType.Navamsa);
					var lagnaD1 = grahas[Body.Lagna];
					var lagnaD9 = navamsa[Body.Lagna];

					if (lagnaD1.Rashi == lagnaD9.Rashi)
					{
						return (true);
					}
				}
				return (false);
				//10th lord is in the 10th house
				case NakshatraLord.ChaturashitiSama:
				{
					var rashi = grahas.Rashis[Bhava.KarmaBhava];
					if (rashi.Lord.Bhava == Bhava.KarmaBhava)
					{
						return (true);
					}
				}
				return (false);
				//Lagna lord is in the 7th house OR 7th lord is in the Lagna
				case NakshatraLord.DwisaptatiSama:
				{
					var lagna = grahas [Body.Lagna];
					if (lagna.Bhava == Bhava.JayaBhava)
					{
						return (true);
					}

					var lord = grahas.Rashis[Bhava.JayaBhava].Lord;
					if (lord.Bhava == Bhava.LagnaBhava)
					{
						return (true);
					}
				}
				return (false);
				//daytime in Sun’s Hora or at night time in Moon’s Hora.
				//Hora = hour lord or the planet that rules the hour of birth.
				case NakshatraLord.ShatTrimshaSama:
				{
					var hora = h.FindGrahas(DivisionType.HoraParasara);
					var sun  = hora[Body.Sun];
					if (sun.Rashi == ZodiacHouse.Leo)
					{
						return h.Vara.IsDayBirth;
					}

					return (h.Vara.IsDayBirth == false);
				}
			}

			return (true);
		}

		public static MhoraViewType View(this NakshatraLord nakshatraLord)
		{
			return nakshatraLord switch
			{
				NakshatraLord.Ashtottari       => MhoraViewType.DasaAshtottari,
				NakshatraLord.ChaturashitiSama => MhoraViewType.DasaChaturashitiSama,
				NakshatraLord.Dwadashottari    => MhoraViewType.DasaDwadashottari,
				NakshatraLord.DwisaptatiSama   => MhoraViewType.DasaDwisaptatiSama,
				NakshatraLord.Panchottari      => MhoraViewType.DasaPanchottari,
				NakshatraLord.ShatTrimshaSama  => MhoraViewType.DasaShatTrimshaSama,
				NakshatraLord.Shatabdika       => MhoraViewType.DasaShatabdika,
				NakshatraLord.Shodashottari    => MhoraViewType.DasaShodashottari,
				NakshatraLord.Vimsottari       => MhoraViewType.DasaVimsottari,
				NakshatraLord.Yogini           => MhoraViewType.DasaYogini,
				_                              => throw new IndexOutOfRangeException()
			};
		}

	}
}
