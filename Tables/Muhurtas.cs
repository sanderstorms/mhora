using System.Diagnostics;
using Mhora.Elements;

namespace Mhora.Tables
{
	public static class Muhurtas
	{
		public enum Muhurta
		{
			Rudra = 1,
			Ahi,
			Mitra,
			Pitri,
			Vasu,
			Ambu,
			Visvadeva,
			Abhijit,
			Vidhata,
			Puruhuta,
			Indragni,
			Nirriti,
			Varuna,
			Aryaman,
			Bhaga,
			Girisa,
			Ajapada,
			Ahirbudhnya,
			Pushan,
			Asvi,
			Yama,
			Agni,
			Vidhaatri,
			Chanda,
			Aditi,
			Jiiva,
			Vishnu,
			Arka,
			Tvashtri,
			Maruta
		}

		public static Nakshatras.Nakshatra28 NakLordOfMuhurta(Muhurta m)
		{
			switch (m)
			{
				case Muhurta.Rudra:       return Nakshatras.Nakshatra28.Aridra;
				case Muhurta.Ahi:         return Nakshatras.Nakshatra28.Aslesha;
				case Muhurta.Mitra:       return Nakshatras.Nakshatra28.Anuradha;
				case Muhurta.Pitri:       return Nakshatras.Nakshatra28.Makha;
				case Muhurta.Vasu:        return Nakshatras.Nakshatra28.Dhanishta;
				case Muhurta.Ambu:        return Nakshatras.Nakshatra28.PoorvaShada;
				case Muhurta.Visvadeva:   return Nakshatras.Nakshatra28.UttaraShada;
				case Muhurta.Abhijit:     return Nakshatras.Nakshatra28.Abhijit;
				case Muhurta.Vidhata:     return Nakshatras.Nakshatra28.Rohini;
				case Muhurta.Puruhuta:    return Nakshatras.Nakshatra28.Jyestha;
				case Muhurta.Indragni:    return Nakshatras.Nakshatra28.Vishaka;
				case Muhurta.Nirriti:     return Nakshatras.Nakshatra28.Moola;
				case Muhurta.Varuna:      return Nakshatras.Nakshatra28.Satabisha;
				case Muhurta.Aryaman:     return Nakshatras.Nakshatra28.UttaraPhalguni;
				case Muhurta.Bhaga:       return Nakshatras.Nakshatra28.PoorvaPhalguni;
				case Muhurta.Girisa:      return Nakshatras.Nakshatra28.Aridra;
				case Muhurta.Ajapada:     return Nakshatras.Nakshatra28.PoorvaBhadra;
				case Muhurta.Ahirbudhnya: return Nakshatras.Nakshatra28.UttaraBhadra;
				case Muhurta.Pushan:      return Nakshatras.Nakshatra28.Revati;
				case Muhurta.Asvi:        return Nakshatras.Nakshatra28.Aswini;
				case Muhurta.Yama:        return Nakshatras.Nakshatra28.Bharani;
				case Muhurta.Agni:        return Nakshatras.Nakshatra28.Krittika;
				case Muhurta.Vidhaatri:   return Nakshatras.Nakshatra28.Rohini;
				case Muhurta.Chanda:      return Nakshatras.Nakshatra28.Mrigarirsa;
				case Muhurta.Aditi:       return Nakshatras.Nakshatra28.Punarvasu;
				case Muhurta.Jiiva:       return Nakshatras.Nakshatra28.Pushya;
				case Muhurta.Vishnu:      return Nakshatras.Nakshatra28.Sravana;
				case Muhurta.Arka:        return Nakshatras.Nakshatra28.Hasta;
				case Muhurta.Tvashtri:    return Nakshatras.Nakshatra28.Chittra;
				case Muhurta.Maruta:      return Nakshatras.Nakshatra28.Swati;
			}

			Debug.Assert(false, string.Format("Basics::NakLordOfMuhurta Unknown Muhurta {0}", m));
			return Nakshatras.Nakshatra28.Aswini;
		}
	}
}
