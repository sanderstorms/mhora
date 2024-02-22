using System;

using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Dasas.NakshatraDasa
{
	public static class KalaChakra
	{
		[Flags]
		public enum Gati : byte
		{
			None = 0,
			SF   = 0x01, // Systemic feature
			QJ   = 0x02, // Qwantum Jump (Simhavalokan)
			FF   = 0x04, // Fast forward (Mandooki)
			Re   = 0x08, // Reorientation (Markati)
			Co   = 0x10, // Corporal
			Ps   = 0x20, // Psychical
			Mark = 0x40, // Mark next Gati
		}

		public static Gati DasaGati(ZodiacHouse dasa, int cycle)
		{
			var gati = Gati.None;

			var dp = (1 << (dasa.Index() - 1));

			switch (cycle)
			{
				case 1: //deva
				{
					gati = Gati.Co;
					if ((dp & 0b100110011001) == 0)
					{
						gati |= Gati.QJ;
					}
					else
					{
						gati |= Gati.FF;
					}
				}
					break;

				case 2:
				{
					if (dasa == ZodiacHouse.Sco)
					{
						gati = Gati.Re;
					}
				}
					break;

				case 3:
				{
					switch (dasa)
					{
						case ZodiacHouse.Tau:
						case ZodiacHouse.Leo:
						case ZodiacHouse.Vir:
						case ZodiacHouse.Cap:
							return Gati.Mark;

						case ZodiacHouse.Sco:
							return Gati.FF;
					}
				}
					break;

				case 4:
				{
					switch (dasa)
					{
						case ZodiacHouse.Tau:
						case ZodiacHouse.Vir:
						case ZodiacHouse.Cap:
							return Gati.QJ;
						case ZodiacHouse.Leo:
							return Gati.FF;
					}
				}
					break;

				case 6:
				{
					switch (dasa)
					{
						case ZodiacHouse.Tau:
						case ZodiacHouse.Gem:
						case ZodiacHouse.Lib:
						case ZodiacHouse.Cap:
							return Gati.Mark;
					}
				}
					break;

				case 7:
				{
					switch (dasa)
					{
						case ZodiacHouse.Tau:
						case ZodiacHouse.Cap:
							return Gati.FF;
						case ZodiacHouse.Gem:
						case ZodiacHouse.Lib:
							return Gati.QJ;
					}

					break;
				}

				case 8:
				{
					switch (dasa)
					{
						case ZodiacHouse.Tau:
						case ZodiacHouse.Cap:
							return Gati.Re;
					}
				}
					break;

				case 9: //jeeva
				{
					gati = Gati.Ps;
					if ((dp & 0b100110011001) == 0)
					{
						gati |= Gati.QJ;
					}
					else
					{
						gati |= Gati.FF;
					}

					if ((dp & 0b100101111011) == 0)
					{
						gati |= Gati.Mark;

					}

				}
					break;
			}

			return (gati);
		}

	public static ZodiacHouse DasaPeriod (Longitude lon, int cycle, out bool savya)
	{
		var nakshatra = lon.ToNakshatra();
		var pada = lon.ToNakshatraPada();
		var zh   = NavamsaRasi(nakshatra, pada, out _);

		(nakshatra, pada) = zh.NakshatraPada();
		(nakshatra, pada) = nakshatra.AddPada(pada + cycle);

		return NavamsaRasi(nakshatra, pada, out savya);
	}

	public static ZodiacHouse NavamsaRasi(Nakshatra nakshatra, int pada, out bool savya)
	{
		var dp       = nakshatra.Index().NormalizeInc(1, 6);
		var dpOffset = dp.NormalizeInc(1, 3) - 1;
		var zh       = (ZodiacHouse) (dpOffset * 4 + pada);

		switch (dp)
		{
			case 1:
			case 2:
			case 3:
				savya = true;
				return (zh); //Clockwise (Savya)
			default:
				savya = false;
				return zh.LordsOtherSign();
		}
	}

	public static ZodiacHouse Indirect(ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Leo: return ZodiacHouse.Can;
			case ZodiacHouse.Can: return ZodiacHouse.Leo;
		}

		return zh.LordsOtherSign();
	}

	public static ZodiacHouse Invert(ZodiacHouse zh)
	{
		return (ZodiacHouse) (12 - (zh.Index() - 1));
	}

	public static int DasaPackDirect(ZodiacHouse zh, int cycle, out ZodiacHouse bhukti)
	{
		var progression = ((zh.Index() - 1) * 9)                 + 1 + cycle;
		var dp          = (int) Math.Ceiling(progression / 12.0) - 1;
		
		bhukti      = (ZodiacHouse) progression.NormalizeInc(1, 12);
		return (dp);
	}

	public static ZodiacHouse BhuktiDirect(ZodiacHouse zh, int cycle, out bool direct)
	{
		var dp = DasaPackDirect(zh, cycle, out ZodiacHouse bhukti);
		if ((dp % 2) == 1)
		{
			direct = false;
			return bhukti.LordsOtherSign();
		}

		direct = true;
		return bhukti;

	}

	public static int DasaPackIndirect(ZodiacHouse zh, int cycle, out ZodiacHouse bhukti)
	{
		var index       = Indirect(zh).Index();
		var progression = ((index - 1) * 9)  + 1 + cycle;

		var dp          = (int) Math.Ceiling(progression / 12.0) - 1;
		bhukti = (ZodiacHouse)progression.NormalizeInc(1, 12);
		bhukti = Invert(bhukti);
		return (dp);
	}

	public static ZodiacHouse BhuktiIndirect(ZodiacHouse zh, int cycle, out bool direct)
	{
		var dp = DasaPackIndirect(zh, cycle, out var bhukti);

		if (dp >= 6)
		{
			dp++; //switch in direction
		}

		if ((dp % 2) == 0)
		{
			direct = false;
			return bhukti.LordsOtherSign();
		}
		direct = true;
		return bhukti;
	}

	}
}
