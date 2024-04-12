using System.Diagnostics;
using Mhora.Definitions;

namespace Mhora.Elements.Extensions
{
	public static class Kutas
	{
		public static Kuta.BhutaNakshatra BhutaNakshatra(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Bharani:
				case Nakshatra.Krittika:
				case Nakshatra.Rohini:
				case Nakshatra.Mrigarirsa: return Kuta.BhutaNakshatra.Earth;
				case Nakshatra.Aridra:
				case Nakshatra.Punarvasu:
				case Nakshatra.Pushya:
				case Nakshatra.Aslesha:
				case Nakshatra.Makha:
				case Nakshatra.PoorvaPhalguni: return Kuta.BhutaNakshatra.Water;
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.Hasta:
				case Nakshatra.Chittra:
				case Nakshatra.Swati:
				case Nakshatra.Vishaka: return Kuta.BhutaNakshatra.Fire;
				case Nakshatra.Anuradha:
				case Nakshatra.Jyestha:
				case Nakshatra.Moola:
				case Nakshatra.PoorvaShada:
				case Nakshatra.UttaraShada:
				case Nakshatra.Sravana: return Kuta.BhutaNakshatra.Air;
				case Nakshatra.Dhanishta:
				case Nakshatra.Satabisha:
				case Nakshatra.PoorvaBhadra:
				case Nakshatra.UttaraBhadra:
				case Nakshatra.Revati: return Kuta.BhutaNakshatra.Ether;
			}

			Debug.Assert(false, "KutaBhutaNakshatra::getType");
			return Kuta.BhutaNakshatra.Air;
		}

		public static Kuta.Gana KutaGana(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Mrigarirsa:
				case Nakshatra.Punarvasu:
				case Nakshatra.Pushya:
				case Nakshatra.Hasta:
				case Nakshatra.Swati:
				case Nakshatra.Anuradha:
				case Nakshatra.Sravana:
				case Nakshatra.Revati: return Kuta.Gana.Deva;
				case Nakshatra.Bharani:
				case Nakshatra.Rohini:
				case Nakshatra.Aridra:
				case Nakshatra.PoorvaPhalguni:
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.PoorvaShada:
				case Nakshatra.UttaraShada:
				case Nakshatra.PoorvaBhadra:
				case Nakshatra.UttaraBhadra: return Kuta.Gana.Nara;
			}

			return Kuta.Gana.Rakshasa;
		}

		public static Kuta.Gotra KutaGotra(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Pushya:
				case Nakshatra.Swati: return Kuta.Gotra.Marichi;
				case Nakshatra.Bharani:
				case Nakshatra.Aslesha:
				case Nakshatra.Vishaka:
				case Nakshatra.Sravana: return Kuta.Gotra.Vasishtha;
				case Nakshatra.Krittika:
				case Nakshatra.Makha:
				case Nakshatra.Anuradha:
				case Nakshatra.Dhanishta: return Kuta.Gotra.Angirasa;
				case Nakshatra.Rohini:
				case Nakshatra.PoorvaPhalguni:
				case Nakshatra.Jyestha:
				case Nakshatra.Satabisha: return Kuta.Gotra.Atri;
				case Nakshatra.Mrigarirsa:
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.Moola:
				case Nakshatra.PoorvaBhadra: return Kuta.Gotra.Pulastya;
				case Nakshatra.Aridra:
				case Nakshatra.Hasta:
				case Nakshatra.PoorvaShada:
				case Nakshatra.UttaraBhadra: return Kuta.Gotra.Pulaha;
				case Nakshatra.Punarvasu:
				case Nakshatra.Chittra:
				case Nakshatra.UttaraShada:
				case Nakshatra.Revati: return Kuta.Gotra.Kretu;
			}

			Debug.Assert(false, "KutaGotra::getType");
			return Kuta.Gotra.Angirasa;
		}

		public static Kuta.Nadi KutaNadi(Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Aridra:
				case Nakshatra.Punarvasu:
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.Hasta:
				case Nakshatra.Jyestha:
				case Nakshatra.Moola:
				case Nakshatra.Satabisha:
				case Nakshatra.PoorvaBhadra: return Kuta.Nadi.Vata;
				case Nakshatra.Bharani:
				case Nakshatra.Mrigarirsa:
				case Nakshatra.Pushya:
				case Nakshatra.PoorvaPhalguni:
				case Nakshatra.Chittra:
				case Nakshatra.Anuradha:
				case Nakshatra.PoorvaShada:
				case Nakshatra.Dhanishta:
				case Nakshatra.UttaraBhadra: return Kuta.Nadi.Pitta;
			}

			return Kuta.Nadi.Sleshma;
		}

		public static Kuta.Sex GetSex(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Bharani:
				case Nakshatra.Pushya:
				case Nakshatra.Rohini:
				case Nakshatra.Moola:
				case Nakshatra.Aslesha:
				case Nakshatra.Makha:
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.Swati:
				case Nakshatra.Vishaka:
				case Nakshatra.Jyestha:
				case Nakshatra.PoorvaShada:
				case Nakshatra.PoorvaBhadra:
				case Nakshatra.UttaraShada: return Kuta.Sex.Male;
			}

			return Kuta.Sex.Female;
		}

		public static Kuta.NakshatraYoni KutaNakshatraYoni(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Satabisha: return Kuta.NakshatraYoni.Horse;
				case Nakshatra.Bharani:
				case Nakshatra.Revati: return Kuta.NakshatraYoni.Elephant;
				case Nakshatra.Pushya:
				case Nakshatra.Krittika: return Kuta.NakshatraYoni.Sheep;
				case Nakshatra.Rohini:
				case Nakshatra.Mrigarirsa: return Kuta.NakshatraYoni.Serpent;
				case Nakshatra.Moola:
				case Nakshatra.Aridra: return Kuta.NakshatraYoni.Dog;
				case Nakshatra.Aslesha:
				case Nakshatra.Punarvasu: return Kuta.NakshatraYoni.Cat;
				case Nakshatra.Makha:
				case Nakshatra.PoorvaPhalguni: return Kuta.NakshatraYoni.Rat;
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.UttaraBhadra: return Kuta.NakshatraYoni.Cow;
				case Nakshatra.Swati:
				case Nakshatra.Hasta: return Kuta.NakshatraYoni.Buffalo;
				case Nakshatra.Vishaka:
				case Nakshatra.Chittra: return Kuta.NakshatraYoni.Tiger;
				case Nakshatra.Jyestha:
				case Nakshatra.Anuradha: return Kuta.NakshatraYoni.Hare;
				case Nakshatra.PoorvaShada:
				case Nakshatra.Sravana: return Kuta.NakshatraYoni.Monkey;
				case Nakshatra.PoorvaBhadra:
				case Nakshatra.Dhanishta: return Kuta.NakshatraYoni.Lion;
				case Nakshatra.UttaraShada: return Kuta.NakshatraYoni.Mongoose;
			}


			Debug.Assert(false, "KutaNakshatraYoni::getType");
			return Kuta.NakshatraYoni.Horse;
		}

		public static Kuta.Rajju KutaRajju(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Rohini:
				case Nakshatra.Aridra:
				case Nakshatra.Hasta:
				case Nakshatra.Swati:
				case Nakshatra.Sravana:
				case Nakshatra.Satabisha: return Kuta.Rajju.Kantha;
				case Nakshatra.Bharani:
				case Nakshatra.Pushya:
				case Nakshatra.PoorvaPhalguni:
				case Nakshatra.Anuradha:
				case Nakshatra.PoorvaShada:
				case Nakshatra.UttaraBhadra: return Kuta.Rajju.Kati;
				case Nakshatra.Aswini:
				case Nakshatra.Aslesha:
				case Nakshatra.Makha:
				case Nakshatra.Jyestha:
				case Nakshatra.Moola:
				case Nakshatra.Revati: return Kuta.Rajju.Pada;
				case Nakshatra.Mrigarirsa:
				case Nakshatra.Dhanishta:
				case Nakshatra.Chittra: return Kuta.Rajju.Siro;
			}

			return Kuta.Rajju.Kukshi;
		}

		public static Kuta.RasiYoni KutaRasiYoni(this ZodiacHouse z)
		{
			switch (z)
			{
				case ZodiacHouse.Cap:
				case ZodiacHouse.Pis: return Kuta.RasiYoni.Pakshi;
				case ZodiacHouse.Can:
				case ZodiacHouse.Sco: return Kuta.RasiYoni.Reptile;
				case ZodiacHouse.Ari:
				case ZodiacHouse.Tau:
				case ZodiacHouse.Leo: return Kuta.RasiYoni.Pasu;
			}

			return Kuta.RasiYoni.Nara;
		}

		public static Kuta.Varna KutaVarna(this Nakshatra n)
		{
			switch ((int) n % 6)
			{
				case 1: return Kuta.Varna.Brahmana;
				case 2: return Kuta.Varna.Kshatriya;
				case 3: return Kuta.Varna.Vaishya;
				case 4: return Kuta.Varna.Sudra;
				case 5: return Kuta.Varna.Anuloma;
				case 0: return Kuta.Varna.Pratiloma;
				case 6: return Kuta.Varna.Pratiloma;
			}

			Debug.Assert(false, "KutaVarna::getType");
			return Kuta.Varna.Anuloma;
		}

		public static Kuta.Vedha KutaVedha(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Jyestha: return Kuta.Vedha.AswJye;
				case Nakshatra.Bharani:
				case Nakshatra.Anuradha: return Kuta.Vedha.BhaAnu;
				case Nakshatra.Krittika:
				case Nakshatra.Vishaka: return Kuta.Vedha.KriVis;
				case Nakshatra.Rohini:
				case Nakshatra.Swati: return Kuta.Vedha.RohSwa;
				case Nakshatra.Aridra:
				case Nakshatra.Sravana: return Kuta.Vedha.AriSra;
				case Nakshatra.Punarvasu:
				case Nakshatra.UttaraShada: return Kuta.Vedha.PunUsh;
				case Nakshatra.Pushya:
				case Nakshatra.PoorvaShada: return Kuta.Vedha.PusPsh;
				case Nakshatra.Aslesha:
				case Nakshatra.Moola: return Kuta.Vedha.AslMoo;
				case Nakshatra.Makha:
				case Nakshatra.Revati: return Kuta.Vedha.MakRev;
				case Nakshatra.PoorvaPhalguni:
				case Nakshatra.UttaraBhadra: return Kuta.Vedha.PphUbh;
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.PoorvaBhadra: return Kuta.Vedha.UphPbh;
				case Nakshatra.Hasta:
				case Nakshatra.Satabisha: return Kuta.Vedha.HasSat;
				case Nakshatra.Mrigarirsa:
				case Nakshatra.Dhanishta: return Kuta.Vedha.MriDha;
				case Nakshatra.Chittra: return Kuta.Vedha.Chi;
			}

			Debug.Assert(false, "KutaVedha::getType");
			return Kuta.Vedha.AriSra;
		}

		public static Kuta.Vihanga KutaVihanga(this Nakshatra n)
		{
			switch (n)
			{
				case Nakshatra.Aswini:
				case Nakshatra.Bharani:
				case Nakshatra.Krittika:
				case Nakshatra.Rohini:
				case Nakshatra.Mrigarirsa: return Kuta.Vihanga.Bharandhaka;
				case Nakshatra.Aridra:
				case Nakshatra.Punarvasu:
				case Nakshatra.Pushya:
				case Nakshatra.Aslesha:
				case Nakshatra.Makha:
				case Nakshatra.PoorvaPhalguni: return Kuta.Vihanga.Pingala;
				case Nakshatra.UttaraPhalguni:
				case Nakshatra.Hasta:
				case Nakshatra.Chittra:
				case Nakshatra.Swati:
				case Nakshatra.Vishaka:
				case Nakshatra.Anuradha: return Kuta.Vihanga.Crow;
				case Nakshatra.Jyestha:
				case Nakshatra.Moola:
				case Nakshatra.PoorvaShada:
				case Nakshatra.UttaraShada:
				case Nakshatra.Sravana: return Kuta.Vihanga.Cock;
				case Nakshatra.Dhanishta:
				case Nakshatra.Satabisha:
				case Nakshatra.PoorvaBhadra:
				case Nakshatra.UttaraBhadra:
				case Nakshatra.Revati: return Kuta.Vihanga.Peacock;
			}

			Debug.Assert(false, "KutaVibhanga::getType");
			return Kuta.Vihanga.Bharandhaka;
		}

		public static Kuta.Dominator GetDominator(Nakshatra m, Nakshatra n)
		{
			var em = KutaVihanga(m);
			var en = KutaVihanga(n);

			Kuta.Vihanga[] order =
			[
				Kuta.Vihanga.Peacock,
				Kuta.Vihanga.Cock,
				Kuta.Vihanga.Crow,
				Kuta.Vihanga.Pingala
			];
			if (em == en)
			{
				return Kuta.Dominator.Equal;
			}

			for (var i = 0; i < order.Length; i++)
			{
				if (em == order[i])
				{
					return Kuta.Dominator.Male;
				}

				if (en == order[i])
				{
					return Kuta.Dominator.Female;
				}
			}

			return Kuta.Dominator.Equal;
		}

		public static class  Score
		{
			public const int BhutaNakshatraMaxScore = 1;
			public const int GanaMaxScore           = 5;
			public const int GotraMaxScore          = 1;
			public const int NadiMaxScore           = 2;
			public const int RajjuMaxScore          = 1;
			public const int VarnaMaxScore          = 2;
			public const int VedhaMaxScore          = 1;

			public static int BhutaNakshatra(Nakshatra m, Nakshatra n)
			{
				var a = m.BhutaNakshatra();
				var b = n.BhutaNakshatra();
				if (a == b)
				{
					return 1;
				}

				if (a == Kuta.BhutaNakshatra.Fire && b == Kuta.BhutaNakshatra.Air || a == Kuta.BhutaNakshatra.Air && b == Kuta.BhutaNakshatra.Fire)
				{
					return 1;
				}

				if (a == Kuta.BhutaNakshatra.Earth || b == Kuta.BhutaNakshatra.Earth)
				{
					return 1;
				}

				return 0;
			}

			public static int Gana(Nakshatra m, Nakshatra f)
			{
				var em = m.KutaGana();
				var ef = f.KutaGana();

				if (em == ef)
				{
					return 5;
				}

				if (em == Kuta.Gana.Deva && ef == Kuta.Gana.Nara)
				{
					return 4;
				}

				if (em == Kuta.Gana.Rakshasa && ef == Kuta.Gana.Nara)
				{
					return 3;
				}

				if (em == Kuta.Gana.Nara && ef == Kuta.Gana.Deva)
				{
					return 2;
				}

				return 1;
			}

			
			public static int Gotra(Nakshatra m, Nakshatra n)
			{
				if (m.KutaGotra() == n.KutaGotra())
				{
					return 0;
				}

				return 1;
			}

			public static int Nadi(Nakshatra m, Nakshatra n)
			{
				var ea = KutaNadi(m);
				var eb = KutaNadi(n);
				if (ea != eb)
				{
					return 2;
				}

				if (ea == Kuta.Nadi.Vata || ea == Kuta.Nadi.Sleshma)
				{
					return 1;
				}

				return 0;
			}

			public static int Rajju(Nakshatra m, Nakshatra n)
			{
				if (m.KutaRajju() != n.KutaRajju())
				{
					return 1;
				}

				return 0;
			}

			public static int Varna(Nakshatra m, Nakshatra f)
			{
				var em = m.KutaVarna();
				var ef = f.KutaVarna();
				if (em == ef)
				{
					return 2;
				}

				if (em == Kuta.Varna.Brahmana && (ef == Kuta.Varna.Kshatriya || ef == Kuta.Varna.Vaishya || ef == Kuta.Varna.Sudra))
				{
					return 1;
				}

				if (em == Kuta.Varna.Kshatriya && (ef == Kuta.Varna.Vaishya || ef == Kuta.Varna.Sudra))
				{
					return 1;
				}

				if (em == Kuta.Varna.Vaishya && ef == Kuta.Varna.Sudra)
				{
					return 1;
				}

				if (em == Kuta.Varna.Anuloma && ef != Kuta.Varna.Pratiloma)
				{
					return 1;
				}

				if (ef == Kuta.Varna.Anuloma && em != Kuta.Varna.Anuloma)
				{
					return 1;
				}

				return 0;
			}

			public static int Vedha(Nakshatra m, Nakshatra n)
			{
				if (m.KutaVedha() == n.KutaVedha())
				{
					return 0;
				}

				return 1;
			}

		}
	}
}
