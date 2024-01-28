using System.Diagnostics;
using Mhora.Definitions;

namespace Mhora.Elements
{
	public static class Muhurtas
	{
		public static Nakshatra28 NakLordOfMuhurta(this Muhurta m)
		{
			switch (m)
			{
				case Muhurta.Rudra:       return Nakshatra28.Aridra;
				case Muhurta.Ahi:         return Nakshatra28.Aslesha;
				case Muhurta.Mitra:       return Nakshatra28.Anuradha;
				case Muhurta.Pitri:       return Nakshatra28.Makha;
				case Muhurta.Vasu:        return Nakshatra28.Dhanishta;
				case Muhurta.Ambu:        return Nakshatra28.PoorvaShada;
				case Muhurta.Visvadeva:   return Nakshatra28.UttaraShada;
				case Muhurta.Abhijit:     return Nakshatra28.Abhijit;
				case Muhurta.Vidhata:     return Nakshatra28.Rohini;
				case Muhurta.Puruhuta:    return Nakshatra28.Jyestha;
				case Muhurta.Indragni:    return Nakshatra28.Vishaka;
				case Muhurta.Nirriti:     return Nakshatra28.Moola;
				case Muhurta.Varuna:      return Nakshatra28.Satabisha;
				case Muhurta.Aryaman:     return Nakshatra28.UttaraPhalguni;
				case Muhurta.Bhaga:       return Nakshatra28.PoorvaPhalguni;
				case Muhurta.Girisa:      return Nakshatra28.Aridra;
				case Muhurta.Ajapada:     return Nakshatra28.PoorvaBhadra;
				case Muhurta.Ahirbudhnya: return Nakshatra28.UttaraBhadra;
				case Muhurta.Pushan:      return Nakshatra28.Revati;
				case Muhurta.Asvi:        return Nakshatra28.Aswini;
				case Muhurta.Yama:        return Nakshatra28.Bharani;
				case Muhurta.Agni:        return Nakshatra28.Krittika;
				case Muhurta.Vidhaatri:   return Nakshatra28.Rohini;
				case Muhurta.Chanda:      return Nakshatra28.Mrigarirsa;
				case Muhurta.Aditi:       return Nakshatra28.Punarvasu;
				case Muhurta.Jiiva:       return Nakshatra28.Pushya;
				case Muhurta.Vishnu:      return Nakshatra28.Sravana;
				case Muhurta.Arka:        return Nakshatra28.Hasta;
				case Muhurta.Tvashtri:    return Nakshatra28.Chittra;
				case Muhurta.Maruta:      return Nakshatra28.Swati;
			}

			Debug.Assert(false, string.Format("Basics::NakLordOfMuhurta Unknown Muhurta {0}", m));
			return Nakshatra28.Aswini;
		}
	}
}
