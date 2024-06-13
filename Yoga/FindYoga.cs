using System;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Yoga
{
	public static class FindYoga
	{
		public struct Yogas
		{
			public AakritiYoga         AakritiYoga;
			public ChandraYoga         ChandraYoga;
			public DhanaYoga           DhanaYoga;
			public GenericYoga         GenericYoga;
			public MahaParivartanaYoga MahaParivartanaYoga;
			public MahapurushaYoga     MahapurushaYoga;
			public MalikaYoga          MalikaYoga;
			public RajaYoga            RajaYoga;
			public CauseOfDeath        CauseOfDeath;
		}

		public static Yogas FindYogas(this Grahas grahas)
		{
			var yogas = new Yogas();
			foreach (AakritiYoga yoga in Enum.GetValues(typeof(AakritiYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.AakritiYoga |= yoga;
				}
			}

			foreach (ChandraYoga yoga in Enum.GetValues(typeof(ChandraYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.ChandraYoga |= yoga;
				}
			}

			foreach (DhanaYoga yoga in Enum.GetValues(typeof(DhanaYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.DhanaYoga |= yoga;
				}
			}

			foreach (GenericYoga yoga in Enum.GetValues(typeof(GenericYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.GenericYoga |= yoga;
				}
			}

			foreach (MahaParivartanaYoga yoga in Enum.GetValues(typeof(MahaParivartanaYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.MahaParivartanaYoga |= yoga;
				}
			}

			foreach (MahapurushaYoga yoga in Enum.GetValues(typeof(MahapurushaYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.MahapurushaYoga |= yoga;
				}
			}

			foreach (MalikaYoga yoga in Enum.GetValues(typeof(MalikaYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.MalikaYoga |= yoga;
				}
			}

			foreach (RajaYoga yoga in Enum.GetValues(typeof(RajaYoga)))
			{
				if (grahas.Find(yoga))
				{
					yogas.RajaYoga |= yoga;
				}
			}

			foreach (CauseOfDeath yoga in Enum.GetValues(typeof(CauseOfDeath)))
			{
				if (grahas.Find(yoga))
				{
					yogas.CauseOfDeath |= yoga;
				}
			}
			return (yogas);
		}

		public static bool Find(this Grahas grahas, AakritiYoga yoga)
		{
			return yoga switch
			       {
				       AakritiYoga.AakritiArdhachandra => grahas.AakritiArdhachandra(),
				       AakritiYoga.AakritiChakra       => grahas.AakritiChakra(),
				       AakritiYoga.AakritiChhatra      => grahas.AakritiChhatra(),
				       AakritiYoga.AakritiDanda        => grahas.AakritiDanda(),
				       AakritiYoga.AakritiDhanush      => grahas.AakritiDhanush(),
				       AakritiYoga.AakritiGada         => grahas.AakritiGada(),
				       AakritiYoga.AakritiHalaArtha    => grahas.AakritiHalaArtha(),
				       AakritiYoga.AakritiHalaKama     => grahas.AakritiHalaKama(),
				       AakritiYoga.AakritiHalaMoksha   => grahas.AakritiHalaMoksha(),
				       AakritiYoga.AakritiKoota        => grahas.AakritiKoota(),
				       AakritiYoga.AakritiNauka        => grahas.AakritiNauka(),
				       AakritiYoga.AakritiPakshi       => grahas.AakritiPakshi(),
				       AakritiYoga.AakritiSamudra      => grahas.AakritiSamudra(),
				       AakritiYoga.AakritiShakata      => grahas.AakritiShakata(),
				       AakritiYoga.AakritiShakti       => grahas.AakritiShakti(),
				       AakritiYoga.AakritiShara        => grahas.AakritiShara(),
				       AakritiYoga.AakritiShringataka  => grahas.AakritiShringataka(),
				       AakritiYoga.AakritiVaapi        => grahas.AakritiVaapi(),
				       AakritiYoga.AakritiVajra        => grahas.AakritiVajra(),
				       AakritiYoga.AakritiYava         => grahas.AakritiYava(),
				       AakritiYoga.AakritiYoopa        => grahas.AakritiYoopa(),
				       _                               => (false)
			       };
		}

		public static bool Find(this Grahas grahas, ChandraYoga yoga)
		{
			return yoga switch
			       {
				       ChandraYoga.ChandraAdhi         => grahas.ChandraAdhi(),
				       ChandraYoga.ChandraAnapha       => grahas.ChandraAnapha(),
				       ChandraYoga.ChandraDurudhara    => grahas.ChandraDurudhara(),
				       ChandraYoga.ChandraKalpadruma1  => grahas.ChandraKalpadruma1(),
				       ChandraYoga.ChandraKalpadruma2  => grahas.ChandraKalpadruma2(),
				       ChandraYoga.ChandraKalpadruma3  => grahas.ChandraKalpadruma3(),
				       ChandraYoga.ChandraKalpadruma4  => grahas.ChandraKalpadruma4(),
				       ChandraYoga.ChandraKalpadruma5  => grahas.ChandraKalpadruma5(),
				       ChandraYoga.ChandraKalpadruma6  => grahas.ChandraKalpadruma6(),
				       ChandraYoga.ChandraKalpadruma7  => grahas.ChandraKalpadruma7(),
				       ChandraYoga.ChandraKalpadruma8  => grahas.ChandraKalpadruma8(),
				       ChandraYoga.ChandraKalpadruma9  => grahas.ChandraKalpadruma9(),
				       ChandraYoga.ChandraKalpadruma10 => grahas.ChandraKalpadruma10(),
				       ChandraYoga.ChandraKalpadruma11 => grahas.ChandraKalpadruma11(),
				       ChandraYoga.ChandraKalpadruma12 => grahas.ChandraKalpadruma12(),
				       ChandraYoga.ChandraKalpadruma13 => grahas.ChandraKalpadruma13(),
				       ChandraYoga.ChandraKalpadruma14 => grahas.ChandraKalpadruma14(),
				       ChandraYoga.ChandraKalpadruma15 => grahas.ChandraKalpadruma15(),
				       ChandraYoga.ChandraKalpadruma16 => grahas.ChandraKalpadruma16(),
				       ChandraYoga.ChandraKalpadruma17 => grahas.ChandraKalpadruma17(),
				       ChandraYoga.ChandraKalpadruma18 => grahas.ChandraKalpadruma18(),
				       ChandraYoga.ChandraKalpadruma19 => grahas.ChandraKalpadruma19(),
				       ChandraYoga.ChandraKalpadruma20 => grahas.ChandraKalpadruma20(),
				       ChandraYoga.ChandraSunapha      => grahas.ChandraSunapha(),
				       _                               => (false)
			       };
		}

		public static bool Find(this Grahas grahas, DhanaYoga yoga)
		{
			return yoga switch
			       {
				       DhanaYoga.DhanaSurya        => grahas.DhanaSurya(),
				       DhanaYoga.DhanaGuru5th      => grahas.DhanaGuru5th(),
				       DhanaYoga.DhanaLagna        => grahas.DhanaLagna(),
				       DhanaYoga.DhanaChandra      => grahas.DhanaChandra(),
				       DhanaYoga.DanaShukraLagna   => grahas.DanaShukraLagna(),
				       DhanaYoga.Dhana2ndHouseLord => grahas.Dhana2ndHouseLord(),
				       DhanaYoga.Dhana5thLord      => grahas.Dhana5thLord(),
				       DhanaYoga.Dhana9thLord      => grahas.Dhana9thLord(),
				       DhanaYoga.DhanaBhava        => grahas.DhanaBhava(),
				       DhanaYoga.DhanaBuddh5th     => grahas.DhanaBuddh5th(),
				       DhanaYoga.DhanaBuddhLagna   => grahas.DhanaBuddhLagna(),
				       DhanaYoga.DhanaChandra5th   => grahas.DhanaChandra5th(),
				       DhanaYoga.DhanaDay          => grahas.DhanaDay(),
				       DhanaYoga.DhanaGuruLagna    => grahas.DhanaGuruLagna(),
				       DhanaYoga.DhanaGuru9th      => grahas.DhanaGuru9th(),
				       DhanaYoga.DhanaMangal5th    => grahas.DhanaMangal5th(),
				       DhanaYoga.DhanaMangalLagna  => grahas.DhanaMangalLagna(),
				       DhanaYoga.DhanaShukra5th    => grahas.DhanaShukra5th(),
				       DhanaYoga.DhanaShani5th     => grahas.DhanaShani5th(),
				       DhanaYoga.DhanaShaniLagna   => grahas.DhanaShaniLagna(),
				       DhanaYoga.DhanaSurya5th     => grahas.DhanaSurya5th(),
				       DhanaYoga.DhanaNight        => grahas.DhanaNight(),
				       _                           => (false)
			       };
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
					for (var count = 1; count <= 7;count++)
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
			return yoga switch
			       {
				       MahaParivartanaYoga.MahaParivartana1  => grahas.MahaParivartana1(),
				       MahaParivartanaYoga.MahaParivartana2  => grahas.MahaParivartana2(),
				       MahaParivartanaYoga.MahaParivartana4  => grahas.MahaParivartana5(),
				       MahaParivartanaYoga.MahaParivartana5  => grahas.MahaParivartana5(),
				       MahaParivartanaYoga.MahaParivartana7  => grahas.MahaParivartana7(),
				       MahaParivartanaYoga.MahaParivartana9  => grahas.MahaParivartana9(),
				       MahaParivartanaYoga.MahaParivartana10 => grahas.MahaParivartana10(),
				       _                                     => (false)
			       };
		}

		public static bool Find(this Grahas grahas, MahapurushaYoga yoga)
		{
			return yoga switch
			       {
				       MahapurushaYoga.MahapurushaBhadraBuddh => grahas.MahapurushaBhadraBuddh(),
				       MahapurushaYoga.MahapurushaHamsa       => grahas.MahapurushaHamsa(),
				       MahapurushaYoga.MahapurushaMalavya     => grahas.MahapurushaMalavya(),
				       MahapurushaYoga.MahapurushaRuchaka     => grahas.MahapurushaRuchaka(),
				       MahapurushaYoga.MahapurushaSasha       => grahas.MahapurushaSasha(),
				       _                                      => (false)
			       };
		}

		public static bool Find(this Grahas grahas, MalikaYoga yoga)
		{
			return yoga switch
			       {
				       MalikaYoga.BhagyaMalika  => grahas.BhagyaMalika(),
				       MalikaYoga.DhanuMalika   => grahas.DhanuMalika(),
				       MalikaYoga.KalatraMalika => grahas.KalatraMalika(),
				       MalikaYoga.KarmaMalika   => grahas.KarmaMalika(),
				       MalikaYoga.LabhaMalika   => grahas.LabhaMalika(),
				       MalikaYoga.LagnaMalika   => grahas.LagnaMalika(),
				       MalikaYoga.PutraMalika   => grahas.PutraMalika(),
				       MalikaYoga.RandhraMalika => grahas.RandhraMalika(),
				       MalikaYoga.SatruMalika   => grahas.SatruMalika(),
				       MalikaYoga.SukhaMalika   => grahas.SukhaMalika(),
				       MalikaYoga.VrayaMalika   => grahas.VrayaMalika(),
				       _                        => (false)
			       };
		}

		public static bool Find(this Grahas grahas, RajaYoga yoga)
		{
			return yoga switch
			       {
				       RajaYoga.Raja1              => grahas.Raja1(),
				       RajaYoga.Raja2              => grahas.Raja2(),
				       RajaYoga.Raja3              => grahas.Raja3(),
				       RajaYoga.Raja4              => grahas.Raja4(),
				       RajaYoga.Raja5              => grahas.Raja5(),
				       RajaYoga.Raja6              => grahas.Raja6(),
				       RajaYoga.Raja7              => grahas.Raja7(),
				       RajaYoga.Raja8              => grahas.Raja8(),
				       RajaYoga.Raja9              => grahas.Raja9(),
				       RajaYoga.Raja10             => grahas.Raja10(),
				       RajaYoga.Raja11             => grahas.Raja11(),
				       RajaYoga.Raja12             => grahas.Raja12(),
				       RajaYoga.Raja13             => grahas.Raja13(),
				       RajaYoga.Raja14             => grahas.Raja14(),
				       RajaYoga.Raja15             => grahas.Raja15(),
				       RajaYoga.Raja16             => grahas.Raja16(),
				       RajaYoga.Raja17             => grahas.Raja17(),
				       RajaYoga.Raja18             => grahas.Raja18(),
				       RajaYoga.Raja19             => grahas.Raja19(),
				       RajaYoga.Raja20             => grahas.Raja20(),
				       RajaYoga.Raja21             => grahas.Raja21(),
				       RajaYoga.Raja22             => grahas.Raja22(),
				       RajaYoga.Raja23             => grahas.Raja23(),
				       RajaYoga.ViparitaHarshaRaja => grahas.ViparitaHarshaRaja(),
				       RajaYoga.ViparitaSaralaRaja => grahas.ViparitaSaralaRaja(),
				       RajaYoga.ViparitaVimalaRaja => grahas.ViparitaVimalaRaja(),
				       _                           => (false)
			       };
		}

		public static bool Find(this Grahas grahas, CauseOfDeath causeOfDeath)
		{
			return causeOfDeath switch
			{
				CauseOfDeath.FallFromMountain      => grahas.FallFromMountain (),
				CauseOfDeath.BeatenWithWoodenClub  => grahas.BeatenWithWoodenClub(),
				CauseOfDeath.BeatenWithWoodenClubs => grahas.BeatenWithWoodenClubs(),
				CauseOfDeath.Captivity             => grahas.Captivity(),
				CauseOfDeath.Consumption           => grahas.Consumption(),
				CauseOfDeath.Dropsy                => grahas.Dropsy(),
				CauseOfDeath.FallFromAVehicle      => grahas.FallFromAVehicle(),
				CauseOfDeath.FallIntoWell          => grahas.FallIntoWell(),
				CauseOfDeath.FilthAndNightSoil     => grahas.FilthAndNightSoil(),
				CauseOfDeath.FilthAndNightSoil2    => grahas.FilthAndNightSoil2(),
				CauseOfDeath.FallOfWall            => grahas.FallOfWall(),
				CauseOfDeath.Hanging_Fire_or_Fall  => grahas.Hanging_Fire_or_Fall(),
				CauseOfDeath.Impalement            => grahas.Impalement(),
				CauseOfDeath.InWater               => grahas.InWater(),
				CauseOfDeath.Machine               => grahas.Machine(),
				CauseOfDeath.OnAaccountOfAWoman    => grahas.OnAaccountOfAWoman(),
				CauseOfDeath.Poison                => grahas.Poison(),
				CauseOfDeath.Poisoning             => grahas.Poisoning(),
				CauseOfDeath.Prison                => grahas.Prison(),
				CauseOfDeath.RoyalDispleasure      => grahas.RoyalDispleasure(),
				CauseOfDeath.Suffocation           => grahas.Suffocation(),
				CauseOfDeath.Suffocation2          => grahas.Suffocation2(),
				CauseOfDeath.Tumour                => grahas.Tumour(),
				CauseOfDeath.WateryGrave           => grahas.WateryGrave(),
				CauseOfDeath.WeaponsOrFire         => grahas.WeaponsOrFire(),
				CauseOfDeath.WoundsOrWorms         => grahas.WoundsOrWorms(),
				_                                  => false,
			};
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
