using System;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Yoga
{
	public static class FindYoga
	{
		public static bool Find(this Grahas grahas, AakritiYoga yoga)
		{
			switch (yoga)
			{
				case AakritiYoga.AakritiArdhachandra: return grahas.AakritiArdhachandra();
				case AakritiYoga.AakritiChakra:       return grahas.AakritiChakra();
				case AakritiYoga.AakritiChhatra:      return grahas.AakritiChhatra();
				case AakritiYoga.AakritiDanda:        return grahas.AakritiDanda();
				case AakritiYoga.AakritiDhanush:      return grahas.AakritiDhanush();
				case AakritiYoga.AakritiGada:         return grahas.AakritiGada();
				case AakritiYoga.AakritiHalaArtha:    return grahas.AakritiHalaArtha();
				case AakritiYoga.AakritiHalaKama:     return grahas.AakritiHalaKama();
				case AakritiYoga.AakritiHalaMoksha:   return grahas.AakritiHalaMoksha();
				case AakritiYoga.AakritiKoota:        return grahas.AakritiKoota();
				case AakritiYoga.AakritiNauka:        return grahas.AakritiNauka();
				case AakritiYoga.AakritiPakshi:       return grahas.AakritiPakshi();
				case AakritiYoga.AakritiSamudra:      return grahas.AakritiSamudra();
				case AakritiYoga.AakritiShakata:      return grahas.AakritiShakata();
				case AakritiYoga.AakritiShakti:       return grahas.AakritiShakti();
				case AakritiYoga.AakritiShara:        return grahas.AakritiShara();
				case AakritiYoga.AakritiShringataka:  return grahas.AakritiShringataka();
				case AakritiYoga.AakritiVaapi:        return grahas.AakritiVaapi();
				case AakritiYoga.AakritiVajra:        return grahas.AakritiVajra();
				case AakritiYoga.AakritiYava:         return grahas.AakritiYava();
				case AakritiYoga.AakritiYoopa:        return grahas.AakritiYoopa();
			}
			return (false);
		}

		public static bool Find(this Grahas grahas, ChandraYoga yoga)
		{
			switch (yoga)
			{
				case ChandraYoga.ChandraAdhi:         return grahas.ChandraAdhi();
				case ChandraYoga.ChandraAnapha:       return grahas.ChandraAnapha();
				case ChandraYoga.ChandraDurudhara:    return grahas.ChandraDurudhara();
				case ChandraYoga.ChandraKalpadruma1:  return grahas.ChandraKalpadruma1();
				case ChandraYoga.ChandraKalpadruma2:  return grahas.ChandraKalpadruma2();
				case ChandraYoga.ChandraKalpadruma3:  return grahas.ChandraKalpadruma3();
				case ChandraYoga.ChandraKalpadruma4:  return grahas.ChandraKalpadruma4();
				case ChandraYoga.ChandraKalpadruma5:  return grahas.ChandraKalpadruma5();
				case ChandraYoga.ChandraKalpadruma6:  return grahas.ChandraKalpadruma6();
				case ChandraYoga.ChandraKalpadruma7:  return grahas.ChandraKalpadruma7();
				case ChandraYoga.ChandraKalpadruma8:  return grahas.ChandraKalpadruma8();
				case ChandraYoga.ChandraKalpadruma9:  return grahas.ChandraKalpadruma9();
				case ChandraYoga.ChandraKalpadruma10: return grahas.ChandraKalpadruma10();
				case ChandraYoga.ChandraKalpadruma11: return grahas.ChandraKalpadruma11();
				case ChandraYoga.ChandraKalpadruma12: return grahas.ChandraKalpadruma12();
				case ChandraYoga.ChandraKalpadruma13: return grahas.ChandraKalpadruma13();
				case ChandraYoga.ChandraKalpadruma14: return grahas.ChandraKalpadruma14();
				case ChandraYoga.ChandraKalpadruma15: return grahas.ChandraKalpadruma15();
				case ChandraYoga.ChandraKalpadruma16: return grahas.ChandraKalpadruma16();
				case ChandraYoga.ChandraKalpadruma17: return grahas.ChandraKalpadruma17();
				case ChandraYoga.ChandraKalpadruma18: return grahas.ChandraKalpadruma18();
				case ChandraYoga.ChandraKalpadruma19: return grahas.ChandraKalpadruma19();
				case ChandraYoga.ChandraKalpadruma20: return grahas.ChandraKalpadruma20();
				case ChandraYoga.ChandraSunapha:      return grahas.ChandraSunapha();
			}
			return (false);
		}

		public static bool Find(this Grahas grahas, DhanaYoga yoga)
		{
			switch (yoga)
			{
				case DhanaYoga.DhanaSurya:        return grahas.DhanaSurya();
				case DhanaYoga.DhanaGuru5th:      return grahas.DhanaGuru5th();
				case DhanaYoga.DhanaLagna:        return grahas.DhanaLagna();
				case DhanaYoga.DhanaChandra:      return grahas.DhanaChandra();
				case DhanaYoga.DanaShukraLagna:   return grahas.DanaShukraLagna();
				case DhanaYoga.Dhana2ndHouseLord: return grahas.Dhana2ndHouseLord();
				case DhanaYoga.Dhana5thLord:      return grahas.Dhana5thLord();
				case DhanaYoga.Dhana9thLord:      return grahas.Dhana9thLord();
				case DhanaYoga.DhanaBhava:        return grahas.DhanaBhava();
				case DhanaYoga.DhanaBuddh5th:     return grahas.DhanaBuddh5th();
				case DhanaYoga.DhanaBuddhLagna:   return grahas.DhanaBuddhLagna();
				case DhanaYoga.DhanaChandra5th:   return grahas.DhanaChandra5th();
				case DhanaYoga.DhanaDay:          return grahas.DhanaDay();
				case DhanaYoga.DhanaGuruLagna:    return grahas.DhanaGuruLagna();
				case DhanaYoga.DhanaGuru9th:      return grahas.DhanaGuru9th();
				case DhanaYoga.DhanaMangal5th:    return grahas.DhanaMangal5th();
				case DhanaYoga.DhanaMangalLagna:  return grahas.DhanaMangalLagna();
				case DhanaYoga.DhanaShukra5th:    return grahas.DhanaShukra5th();
				case DhanaYoga.DhanaShani5th:     return grahas.DhanaShani5th();
				case DhanaYoga.DhanaShaniLagna:   return grahas.DhanaShaniLagna();
				case DhanaYoga.DhanaSurya5th:     return grahas.DhanaSurya5th();
				case DhanaYoga.DhanaNight:        return grahas.DhanaNight();
			}
			return (false);
		}

		public static bool Find(this Grahas grahas, GenericYoga yoga)
		{
			switch (yoga)
			{
				case GenericYoga.GajaKesariChandra:  return grahas.GajaKesariChandra();
				case GenericYoga.AashrayaMusala:     return grahas.AashrayaMusala();
				case GenericYoga.AashrayaNala:       return grahas.AashrayaNala();
				case GenericYoga.AashrayaRajju:      return grahas.AashrayaRajju();
				case GenericYoga.AlpaUttamadi:       return grahas.AlpaUttamadi();
				case GenericYoga.AmalaKirti:         return grahas.AmalaKirti();
				case GenericYoga.AmaraYogaBenefics:  return grahas.AmaraYogaBenefics();
				case GenericYoga.AmaraYogaMalefics:  return grahas.AmaraYogaMalefics();
				case GenericYoga.Bheri:              return grahas.Bheri();
				case GenericYoga.Chaamara1:          return grahas.Chaamara1();
				case GenericYoga.Chaamara2:          return grahas.Chaamara2();
				case GenericYoga.ChatussagaraChara:  return grahas.ChatussagaraChara();
				case GenericYoga.ChatussagaraKendra: return grahas.ChatussagaraKendra();
				case GenericYoga.DalaMaala:          return grahas.DalaMaala();
				case GenericYoga.DalaMala:           return grahas.DalaMala();
				case GenericYoga.DalaSarpa:          return grahas.DalaSarpa();
				case GenericYoga.Dhwaja:             return grahas.Dhwaja();
				case GenericYoga.Hamsa:              return grahas.Hamsa();
				case GenericYoga.HariHaraBramha1st:  return grahas.HariHaraBramha1st();
				case GenericYoga.HariHaraBramha2nd:  return grahas.HariHaraBramha2nd();
				case GenericYoga.HariHaraBramha7th:  return grahas.HariHaraBramha7th();
				case GenericYoga.HathaHantaa:        return grahas.HathaHantaa();
				case GenericYoga.Indra:              return grahas.Indra();
				case GenericYoga.Kaahala1:           return grahas.Kaahala1();
				case GenericYoga.Kaahala2:           return grahas.Kaahala2();
				case GenericYoga.KalsarpaYoga:       return grahas.KalsarpaYoga();
				case GenericYoga.Karika:             return grahas.Karika();
				case GenericYoga.Khadga:             return grahas.Khadga();
				case GenericYoga.Lagnadhi:           return grahas.Lagnadhi();
				case GenericYoga.Lakshmi:            return grahas.Lakshmi();
				case GenericYoga.MadhyaUttamadi:     return grahas.MadhyaUttamadi();
				case GenericYoga.MahaBhagyaDay:      return grahas.MahaBhagyaDay();
				case GenericYoga.MahaBhagyaNight:    return grahas.MahaBhagyaNight();
				case GenericYoga.MahaPataka:         return grahas.MahaPataka();
				case GenericYoga.PaapaKartari:       return grahas.PaapaKartari();
				case GenericYoga.ShubhaKartari:      return grahas.ShubhaKartari();
				case GenericYoga.ParvataKendra:      return grahas.ParvataKendra();
				case GenericYoga.ParvataLagna:       return grahas.ParvataLagna();
				case GenericYoga.Saraswati:          return grahas.Saraswati();
				case GenericYoga.Shakata:            return grahas.Shakata();
				case GenericYoga.Shankha:            return grahas.Shankha();
				case GenericYoga.Shrinatha:          return grahas.Shrinatha();
				case GenericYoga.Simhasana:          return grahas.Simhasana();
				case GenericYoga.UttamaUttamadi:     return grahas.UttamaUttamadi();
				case GenericYoga.Vasumna:            return grahas.Vasumna();
				case GenericYoga.GrahaYoga:
				{
					foreach (var graha in grahas)
					{
						if (graha.Conjunct.Count > 0)
						{
							return (true);
						}
					}
				}
				break;

				case GenericYoga.RaviVeshiYoga:
				{
					foreach (var graha in grahas)
					{
						if (grahas.VeshiYoga(graha))
						{
							return (true);
						}
					}
				}
				break;

				case GenericYoga.RaviVoshiYoga:
				{
					foreach (var graha in grahas)
					{
						if (grahas.VoshiYoga(graha))
						{
							return (true);
						}
					}
				}
				break;

				case GenericYoga.SankhyaYoga:
				{
					for (int count = 1; count <= 7;count++)
					{
						if (grahas.SankhyaYoga(count))
						{
							return (true);
						}
					}
				}
				break;

			}
			return (false);
		}

		public static bool Find(this Grahas grahas, MahaParivartanaYoga yoga)
		{
			switch (yoga)
			{
				case MahaParivartanaYoga.MahaParivartana1:  return grahas.MahaParivartana1();
				case MahaParivartanaYoga.MahaParivartana2:  return grahas.MahaParivartana2();
				case MahaParivartanaYoga.MahaParivartana4:  return grahas.MahaParivartana5();
				case MahaParivartanaYoga.MahaParivartana5:  return grahas.MahaParivartana5();
				case MahaParivartanaYoga.MahaParivartana7:  return grahas.MahaParivartana7();
				case MahaParivartanaYoga.MahaParivartana9:  return grahas.MahaParivartana9();
				case MahaParivartanaYoga.MahaParivartana10: return grahas.MahaParivartana10();
			}
			return (false);
		}

		public static bool Find(this Grahas grahas, MahapurushaYoga yoga)
		{
			switch (yoga)
			{
				case MahapurushaYoga.MahapurushaBhadraBuddh: return grahas.MahapurushaBhadraBuddh();
				case MahapurushaYoga.MahapurushaHamsa:       return grahas.MahapurushaHamsa();
				case MahapurushaYoga.MahapurushaMalavya:     return grahas.MahapurushaMalavya();
				case MahapurushaYoga.MahapurushaRuchaka:     return grahas.MahapurushaRuchaka();
				case MahapurushaYoga.MahapurushaSasha:       return grahas.MahapurushaSasha();
			}
			return (false);
		}

		public static bool Find(this Grahas grahas, MalikaYoga yoga)
		{
			switch (yoga)
			{
				case MalikaYoga.BhagyaMalika:  return grahas.BhagyaMalika();
				case MalikaYoga.DhanuMalika:   return grahas.DhanuMalika();
				case MalikaYoga.KalatraMalika: return grahas.KalatraMalika();
				case MalikaYoga.KarmaMalika:   return grahas.KarmaMalika();
				case MalikaYoga.LabhaMalika:   return grahas.LabhaMalika();
				case MalikaYoga.LagnaMalika:   return grahas.LagnaMalika();
				case MalikaYoga.PutraMalika:   return grahas.PutraMalika();
				case MalikaYoga.RandhraMalika: return grahas.RandhraMalika();
				case MalikaYoga.SatruMalika:   return grahas.SatruMalika();
				case MalikaYoga.SukhaMalika:   return grahas.SukhaMalika();
				case MalikaYoga.VrayaMalika:   return grahas.VrayaMalika();
			}
			return (false);
		}

		public static bool Find(this Grahas grahas, RajaYoga yoga)
		{
			switch (yoga)
			{
				case RajaYoga.Raja1:              return grahas.Raja1();
				case RajaYoga.Raja2:              return grahas.Raja2();
				case RajaYoga.Raja3:              return grahas.Raja3();
				case RajaYoga.Raja4:              return grahas.Raja4();
				case RajaYoga.Raja5:              return grahas.Raja5();
				case RajaYoga.Raja6:              return grahas.Raja6();
				case RajaYoga.Raja7:              return grahas.Raja7();
				case RajaYoga.Raja8:              return grahas.Raja8();
				case RajaYoga.Raja9:              return grahas.Raja9();
				case RajaYoga.Raja10:             return grahas.Raja10();
				case RajaYoga.Raja11:             return grahas.Raja11();
				case RajaYoga.Raja12:             return grahas.Raja12();
				case RajaYoga.Raja13:             return grahas.Raja13();
				case RajaYoga.Raja14:             return grahas.Raja14();
				case RajaYoga.Raja15:             return grahas.Raja15();
				case RajaYoga.Raja16:             return grahas.Raja16();
				case RajaYoga.Raja17:             return grahas.Raja17();
				case RajaYoga.Raja18:             return grahas.Raja18();
				case RajaYoga.Raja19:             return grahas.Raja19();
				case RajaYoga.Raja20:             return grahas.Raja20();
				case RajaYoga.Raja21:             return grahas.Raja21();
				case RajaYoga.Raja22:             return grahas.Raja22();
				case RajaYoga.Raja23:             return grahas.Raja23();
				case RajaYoga.ViparitaHarshaRaja: return grahas.ViparitaHarshaRaja();
				case RajaYoga.ViparitaSaralaRaja: return grahas.ViparitaSaralaRaja();
				case RajaYoga.ViparitaVimalaRaja: return grahas.ViparitaVimalaRaja();
			}
			return (false);
		}

		public static uint FindAakritiYoga (this Grahas grahas)
		{
			uint yoga = 0;

			foreach (AakritiYoga aakritiYoga in Enum.GetValues(typeof(AakritiYoga)))
			{
				if (grahas.Find(aakritiYoga))
				{
					yoga |= (uint) (1 << aakritiYoga.Index());
				}
			}

			return (yoga);
		}

		public static uint FindChandraYoga(this Grahas grahas)
		{
			uint yoga = 0;

			foreach (ChandraYoga chandraYoga in Enum.GetValues(typeof(ChandraYoga)))
			{
				if (grahas.Find(chandraYoga))
				{
					yoga |= (uint) (1 << chandraYoga.Index());
				}
			}

			return (yoga);
		}

		public static uint FindDhanaYoga(this Grahas grahas)
		{
			uint yoga = 0;

			foreach (DhanaYoga dhanaYoga in Enum.GetValues(typeof(DhanaYoga)))
			{
				if (grahas.Find(dhanaYoga))
				{
					yoga |= (uint) (1 << dhanaYoga.Index());
				}
			}

			return (yoga);
		}

		public static ulong FindGenericYoga(this Grahas grahas)
		{
			ulong yoga = 0;

			foreach (GenericYoga genericYoga in Enum.GetValues(typeof(GenericYoga)))
			{
				if (grahas.Find(genericYoga))
				{
					yoga |= (uint) (1 << genericYoga.Index());
				}
			}

			return (yoga);
		}

		public static uint FindMahaParivartanaYoga(this Grahas grahas)
		{
			uint yoga = 0;

			foreach (MahaParivartanaYoga mahaParivartanaYoga in Enum.GetValues(typeof(MahaParivartanaYoga)))
			{
				if (grahas.Find(mahaParivartanaYoga))
				{
					yoga |= (uint) (1 << mahaParivartanaYoga.Index());
				}
			}

			return (yoga);
		}

		public static uint FindMahapurushaYoga(this Grahas grahas)
		{
			uint yoga = 0;

			foreach (MahapurushaYoga mahapurushaYoga in Enum.GetValues(typeof(MahapurushaYoga)))
			{
				if (grahas.Find(mahapurushaYoga))
				{
					yoga |= (uint) (1 << mahapurushaYoga.Index());
				}
			}

			return (yoga);
		}

		public static uint FindMalikaYoga(this Grahas grahas)
		{
			uint yoga = 0;

			foreach (MalikaYoga malikaYoga in Enum.GetValues(typeof(MalikaYoga)))
			{
				if (grahas.Find(malikaYoga))
				{
					yoga |= (uint) (1 << malikaYoga.Index());
				}
			}

			return (yoga);
		}

		public static uint FindRajaYoga(this Grahas grahas)
		{
			uint yoga = 0;

			foreach (RajaYoga rajaYoga in Enum.GetValues(typeof(RajaYoga)))
			{
				if (grahas.Find(rajaYoga))
				{
					yoga |= (uint) (1 << rajaYoga.Index());
				}
			}

			return (yoga);
		}
	}
}
