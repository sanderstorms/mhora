using Mhora.Elements.Extensions;

namespace Mhora.Definitions
{
	public static class Ghataka
	{
		public static bool Day(ZodiacHouse janmaZodiacHouse, Weekday wd)
		{
			var ja = janmaZodiacHouse;
			var gh = ja switch
			         {
				         ZodiacHouse.Ari => Weekday.Sunday,
				         ZodiacHouse.Tau => Weekday.Saturday,
				         ZodiacHouse.Gem => Weekday.Monday,
				         ZodiacHouse.Can => Weekday.Wednesday,
				         ZodiacHouse.Leo => Weekday.Saturday,
				         ZodiacHouse.Vir => Weekday.Saturday,
				         ZodiacHouse.Lib => Weekday.Thursday,
				         ZodiacHouse.Sco => Weekday.Friday,
				         ZodiacHouse.Sag => Weekday.Friday,
				         ZodiacHouse.Cap => Weekday.Tuesday,
				         ZodiacHouse.Aqu => Weekday.Thursday,
				         ZodiacHouse.Pis => Weekday.Friday,
				         _               => Weekday.Sunday
			         };

			return wd == gh;
		}

		public static bool Tithi(ZodiacHouse janmaZodiacHouse, Tithi t)
		{
			var ja = janmaZodiacHouse;
			var gh = ja switch
			         {
				         ZodiacHouse.Ari => Tithis.NandaType.Nanda,
				         ZodiacHouse.Tau => Tithis.NandaType.Purna,
				         ZodiacHouse.Gem => Tithis.NandaType.Bhadra,
				         ZodiacHouse.Can => Tithis.NandaType.Bhadra,
				         ZodiacHouse.Leo => Tithis.NandaType.Jaya,
				         ZodiacHouse.Vir => Tithis.NandaType.Purna,
				         ZodiacHouse.Lib => Tithis.NandaType.Rikta,
				         ZodiacHouse.Sco => Tithis.NandaType.Nanda,
				         ZodiacHouse.Sag => Tithis.NandaType.Jaya,
				         ZodiacHouse.Cap => Tithis.NandaType.Rikta,
				         ZodiacHouse.Aqu => Tithis.NandaType.Jaya,
				         ZodiacHouse.Pis => Tithis.NandaType.Purna,
				         _               => Tithis.NandaType.Nanda
			         };

			return t.ToNandaType() == gh;
		}

		public static bool LagnaOpp(ZodiacHouse janma, ZodiacHouse same)
		{
			var ja = janma;
			var gh = ja switch
			         {
				         ZodiacHouse.Ari => ZodiacHouse.Lib,
				         ZodiacHouse.Tau => ZodiacHouse.Sco,
				         ZodiacHouse.Gem => ZodiacHouse.Cap,
				         ZodiacHouse.Can => ZodiacHouse.Ari,
				         ZodiacHouse.Leo => ZodiacHouse.Can,
				         ZodiacHouse.Vir => ZodiacHouse.Vir,
				         ZodiacHouse.Lib => ZodiacHouse.Pis,
				         ZodiacHouse.Sco => ZodiacHouse.Tau,
				         ZodiacHouse.Sag => ZodiacHouse.Gem,
				         ZodiacHouse.Cap => ZodiacHouse.Leo,
				         ZodiacHouse.Aqu => ZodiacHouse.Sag,
				         ZodiacHouse.Pis => ZodiacHouse.Aqu,
				         _               => ZodiacHouse.Ari
			         };

			return same == gh;
		}

		public static bool LagnaSame(ZodiacHouse janma, ZodiacHouse same)
		{
			var ja = janma;
			var gh = ja switch
			         {
				         ZodiacHouse.Ari => ZodiacHouse.Ari,
				         ZodiacHouse.Tau => ZodiacHouse.Tau,
				         ZodiacHouse.Gem => ZodiacHouse.Can,
				         ZodiacHouse.Can => ZodiacHouse.Lib,
				         ZodiacHouse.Leo => ZodiacHouse.Cap,
				         ZodiacHouse.Vir => ZodiacHouse.Pis,
				         ZodiacHouse.Lib => ZodiacHouse.Vir,
				         ZodiacHouse.Sco => ZodiacHouse.Sco,
				         ZodiacHouse.Sag => ZodiacHouse.Sag,
				         ZodiacHouse.Cap => ZodiacHouse.Aqu,
				         ZodiacHouse.Aqu => ZodiacHouse.Gem,
				         ZodiacHouse.Pis => ZodiacHouse.Leo,
				         _               => ZodiacHouse.Ari
			         };

			return same == gh;
		}

		public static bool Moon(ZodiacHouse janmaZodiacHouse, ZodiacHouse chandraZodiacHouse)
		{
			var ja = janmaZodiacHouse;
			var ch = chandraZodiacHouse;

			var gh = ja switch
			         {
				         ZodiacHouse.Ari => ZodiacHouse.Ari,
				         ZodiacHouse.Tau => ZodiacHouse.Vir,
				         ZodiacHouse.Gem => ZodiacHouse.Aqu,
				         ZodiacHouse.Can => ZodiacHouse.Leo,
				         ZodiacHouse.Leo => ZodiacHouse.Cap,
				         ZodiacHouse.Vir => ZodiacHouse.Gem,
				         ZodiacHouse.Lib => ZodiacHouse.Sag,
				         ZodiacHouse.Sco => ZodiacHouse.Tau,
				         ZodiacHouse.Sag => ZodiacHouse.Pis,
				         ZodiacHouse.Cap => ZodiacHouse.Leo,
				         ZodiacHouse.Aqu => ZodiacHouse.Sag,
				         ZodiacHouse.Pis => ZodiacHouse.Aqu,
				         _               => ZodiacHouse.Ari
			         };

			return ch == gh;
		}

		public static bool Star(ZodiacHouse janmaZodiacHouse, Nakshatra nak)
		{
			var ja = janmaZodiacHouse;
			var gh = ja switch
			         {
				         ZodiacHouse.Ari => Nakshatra.Makha,
				         ZodiacHouse.Tau => Nakshatra.Hasta,
				         ZodiacHouse.Gem => Nakshatra.Swati,
				         ZodiacHouse.Can => Nakshatra.Anuradha,
				         ZodiacHouse.Leo => Nakshatra.Moola,
				         ZodiacHouse.Vir => Nakshatra.Sravana,
				         ZodiacHouse.Lib => Nakshatra.Satabisha,
				         ZodiacHouse.Sco => Nakshatra.Revati,
				         // FIXME dveja nakshatra?????
				         ZodiacHouse.Sag => Nakshatra.Revati,
				         ZodiacHouse.Cap => Nakshatra.Rohini,
				         ZodiacHouse.Aqu => Nakshatra.Aridra,
				         ZodiacHouse.Pis => Nakshatra.Aslesha,
				         _               => Nakshatra.Aswini
			         };

			return nak == gh;
		}
	}
}
