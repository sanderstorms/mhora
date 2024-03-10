using Mhora.Elements.Extensions;

namespace Mhora.Definitions
{
	public static class Ghataka
	{
		public static bool Day(ZodiacHouse janmaZodiacHouse, Weekday wd)
		{
			var ja = janmaZodiacHouse;
			var gh = Weekday.Sunday;
			switch (ja)
			{
				case ZodiacHouse.Ari:
					gh = Weekday.Sunday;
					break;
				case ZodiacHouse.Tau:
					gh = Weekday.Saturday;
					break;
				case ZodiacHouse.Gem:
					gh = Weekday.Monday;
					break;
				case ZodiacHouse.Can:
					gh = Weekday.Wednesday;
					break;
				case ZodiacHouse.Leo:
					gh = Weekday.Saturday;
					break;
				case ZodiacHouse.Vir:
					gh = Weekday.Saturday;
					break;
				case ZodiacHouse.Lib:
					gh = Weekday.Thursday;
					break;
				case ZodiacHouse.Sco:
					gh = Weekday.Friday;
					break;
				case ZodiacHouse.Sag:
					gh = Weekday.Friday;
					break;
				case ZodiacHouse.Cap:
					gh = Weekday.Tuesday;
					break;
				case ZodiacHouse.Aqu:
					gh = Weekday.Thursday;
					break;
				case ZodiacHouse.Pis:
					gh = Weekday.Friday;
					break;
			}

			return wd == gh;
		}

		public static bool Tithi(ZodiacHouse janmaZodiacHouse, Tithi t)
		{
			var ja = janmaZodiacHouse;
			var gh = Tithis.NandaType.Nanda;
			switch (ja)
			{
				case ZodiacHouse.Ari:
					gh = Tithis.NandaType.Nanda;
					break;
				case ZodiacHouse.Tau:
					gh = Tithis.NandaType.Purna;
					break;
				case ZodiacHouse.Gem:
					gh = Tithis.NandaType.Bhadra;
					break;
				case ZodiacHouse.Can:
					gh = Tithis.NandaType.Bhadra;
					break;
				case ZodiacHouse.Leo:
					gh = Tithis.NandaType.Jaya;
					break;
				case ZodiacHouse.Vir:
					gh = Tithis.NandaType.Purna;
					break;
				case ZodiacHouse.Lib:
					gh = Tithis.NandaType.Rikta;
					break;
				case ZodiacHouse.Sco:
					gh = Tithis.NandaType.Nanda;
					break;
				case ZodiacHouse.Sag:
					gh = Tithis.NandaType.Jaya;
					break;
				case ZodiacHouse.Cap:
					gh = Tithis.NandaType.Rikta;
					break;
				case ZodiacHouse.Aqu:
					gh = Tithis.NandaType.Jaya;
					break;
				case ZodiacHouse.Pis:
					gh = Tithis.NandaType.Purna;
					break;
			}

			return t.ToNandaType() == gh;
		}

		public static bool LagnaOpp(ZodiacHouse janma, ZodiacHouse same)
		{
			var ja = janma;
			var gh = ZodiacHouse.Ari;
			switch (ja)
			{
				case ZodiacHouse.Ari:
					gh = ZodiacHouse.Lib;
					break;
				case ZodiacHouse.Tau:
					gh = ZodiacHouse.Sco;
					break;
				case ZodiacHouse.Gem:
					gh = ZodiacHouse.Cap;
					break;
				case ZodiacHouse.Can:
					gh = ZodiacHouse.Ari;
					break;
				case ZodiacHouse.Leo:
					gh = ZodiacHouse.Can;
					break;
				case ZodiacHouse.Vir:
					gh = ZodiacHouse.Vir;
					break;
				case ZodiacHouse.Lib:
					gh = ZodiacHouse.Pis;
					break;
				case ZodiacHouse.Sco:
					gh = ZodiacHouse.Tau;
					break;
				case ZodiacHouse.Sag:
					gh = ZodiacHouse.Gem;
					break;
				case ZodiacHouse.Cap:
					gh = ZodiacHouse.Leo;
					break;
				case ZodiacHouse.Aqu:
					gh = ZodiacHouse.Sag;
					break;
				case ZodiacHouse.Pis:
					gh = ZodiacHouse.Aqu;
					break;
			}

			return same == gh;
		}

		public static bool LagnaSame(ZodiacHouse janma, ZodiacHouse same)
		{
			var ja = janma;
			var gh = ZodiacHouse.Ari;
			switch (ja)
			{
				case ZodiacHouse.Ari:
					gh = ZodiacHouse.Ari;
					break;
				case ZodiacHouse.Tau:
					gh = ZodiacHouse.Tau;
					break;
				case ZodiacHouse.Gem:
					gh = ZodiacHouse.Can;
					break;
				case ZodiacHouse.Can:
					gh = ZodiacHouse.Lib;
					break;
				case ZodiacHouse.Leo:
					gh = ZodiacHouse.Cap;
					break;
				case ZodiacHouse.Vir:
					gh = ZodiacHouse.Pis;
					break;
				case ZodiacHouse.Lib:
					gh = ZodiacHouse.Vir;
					break;
				case ZodiacHouse.Sco:
					gh = ZodiacHouse.Sco;
					break;
				case ZodiacHouse.Sag:
					gh = ZodiacHouse.Sag;
					break;
				case ZodiacHouse.Cap:
					gh = ZodiacHouse.Aqu;
					break;
				case ZodiacHouse.Aqu:
					gh = ZodiacHouse.Gem;
					break;
				case ZodiacHouse.Pis:
					gh = ZodiacHouse.Leo;
					break;
			}

			return same == gh;
		}

		public static bool Moon(ZodiacHouse janmaZodiacHouse, ZodiacHouse chandraZodiacHouse)
		{
			var ja = janmaZodiacHouse;
			var ch = chandraZodiacHouse;

			var gh = ZodiacHouse.Ari;

			switch (ja)
			{
				case ZodiacHouse.Ari:
					gh = ZodiacHouse.Ari;
					break;
				case ZodiacHouse.Tau:
					gh = ZodiacHouse.Vir;
					break;
				case ZodiacHouse.Gem:
					gh = ZodiacHouse.Aqu;
					break;
				case ZodiacHouse.Can:
					gh = ZodiacHouse.Leo;
					break;
				case ZodiacHouse.Leo:
					gh = ZodiacHouse.Cap;
					break;
				case ZodiacHouse.Vir:
					gh = ZodiacHouse.Gem;
					break;
				case ZodiacHouse.Lib:
					gh = ZodiacHouse.Sag;
					break;
				case ZodiacHouse.Sco:
					gh = ZodiacHouse.Tau;
					break;
				case ZodiacHouse.Sag:
					gh = ZodiacHouse.Pis;
					break;
				case ZodiacHouse.Cap:
					gh = ZodiacHouse.Leo;
					break;
				case ZodiacHouse.Aqu:
					gh = ZodiacHouse.Sag;
					break;
				case ZodiacHouse.Pis:
					gh = ZodiacHouse.Aqu;
					break;
			}

			return ch == gh;
		}

		public static bool Star(ZodiacHouse janmaZodiacHouse, Nakshatra nak)
		{
			var ja = janmaZodiacHouse;
			var gh = Nakshatra.Aswini;
			switch (ja)
			{
				case ZodiacHouse.Ari:
					gh = Nakshatra.Makha;
					break;
				case ZodiacHouse.Tau:
					gh = Nakshatra.Hasta;
					break;
				case ZodiacHouse.Gem:
					gh = Nakshatra.Swati;
					break;
				case ZodiacHouse.Can:
					gh = Nakshatra.Anuradha;
					break;
				case ZodiacHouse.Leo:
					gh = Nakshatra.Moola;
					break;
				case ZodiacHouse.Vir:
					gh = Nakshatra.Sravana;
					break;
				case ZodiacHouse.Lib:
					gh = Nakshatra.Satabisha;
					break;
				case ZodiacHouse.Sco:
					gh = Nakshatra.Revati;
					break;
				// FIXME dveja nakshatra?????
				case ZodiacHouse.Sag:
					gh = Nakshatra.Revati;
					break;
				case ZodiacHouse.Cap:
					gh = Nakshatra.Rohini;
					break;
				case ZodiacHouse.Aqu:
					gh = Nakshatra.Aridra;
					break;
				case ZodiacHouse.Pis:
					gh = Nakshatra.Aslesha;
					break;
			}

			return nak == gh;
		}
	}
}
