using System;
using Mhora.Calculation.Strength;
using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Calculation
{
	public static class Compare
	{
		public static int GetStronger(this Grahas grahas, Body m, Body n, bool simpleLord, GrahaStrength strength)
		{
			return strength switch
			       {
				       GrahaStrength.Conjunction                   => grahas.StrengthByConjunction(m, n),
				       GrahaStrength.Exaltation                    => grahas.StrengthByExaltation(m, n),
				       GrahaStrength.Longitude                     => grahas.StrengthByLongitude(m, n),
				       GrahaStrength.AtmaKaraka                    => grahas.StrengthByAtmaKaraka(m, n),
				       GrahaStrength.RasisNature                   => grahas.StrengthByRasisNature(m, n),
				       GrahaStrength.LordsNature                   => grahas.StrengthByLordsNature(m, n),
				       GrahaStrength.AspectsRasi                   => grahas.StrengthByAspectsRasi(m, n, simpleLord),
				       GrahaStrength.AspectsGraha                  => grahas.StrengthByAspectsGraha(m, n, simpleLord),
				       GrahaStrength.NarayanaDasaLength            => grahas.StrengthByNarayanaDasaLength(m, n, simpleLord),
				       GrahaStrength.VimsottariDasaLength          => grahas.StrengthByVimsottariDasaLength(m, n),
				       GrahaStrength.MoolaTrikona                  => grahas.StrengthByMoolaTrikona(m, n),
				       GrahaStrength.OwnHouse                      => grahas.StrengthByOwnHouse(m, n),
				       GrahaStrength.NotInOwnHouse                 => 0 - grahas.StrengthByOwnHouse(m, n),
				       GrahaStrength.LordInOwnHouse                => grahas.StrengthByLordInOwnHouse(m, n, simpleLord),
				       GrahaStrength.LordInDifferentOddity         => grahas.StrengthByLordInDifferentOddity(m, n, simpleLord),
				       GrahaStrength.KendraConjunction             => grahas.StrengthByKendraConjunction(m, n),
				       GrahaStrength.KarakaKendradiGrahaDasaLength => grahas.StrengthByKarakaKendradiGrahaDasaLength(m, n),
				       GrahaStrength.First                         => grahas.StrengthByFirst(m, n),
				       _                                           => throw new Exception("Unknown Graha Strength Rule")
			       };
		}

		public static int GetStronger(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord, RashiStrength strength)
		{
			return strength switch
			       {
				       RashiStrength.Conjunction                   => grahas.StrengthByConjunction(za, zb),
				       RashiStrength.Exaltation                    => grahas.StrengthByExaltation(za, zb),
				       RashiStrength.Longitude                     => grahas.StrengthByLongitude(za, zb),
				       RashiStrength.AtmaKaraka                    => grahas.StrengthByAtmaKaraka(za, zb),
				       RashiStrength.LordIsAtmaKaraka              => grahas.StrengthByLordIsAtmaKaraka(za, zb, simpleLord),
				       RashiStrength.RasisNature                   => grahas.StrengthByRasisNature(za, zb),
				       RashiStrength.LordsNature                   => grahas.StrengthByLordsNature(za, zb),
				       RashiStrength.AspectsRasi                   => grahas.StrengthByAspectsRasi(za, zb, simpleLord),
				       RashiStrength.AspectsGraha                  => grahas.StrengthByAspectsGraha(za, zb, simpleLord),
				       RashiStrength.LordInDifferentOddity         => grahas.StrengthByLordInDifferentOddity(za, zb, simpleLord),
				       RashiStrength.LordsLongitude                => grahas.StrengthByLordsLongitude(za, zb, simpleLord),
				       RashiStrength.NarayanaDasaLength            => grahas.StrengthByNarayanaDasaLength(za, zb, simpleLord),
				       RashiStrength.VimsottariDasaLength          => grahas.StrengthByVimsottariDasaLength(za, zb),
				       RashiStrength.MoolaTrikona                  => grahas.StrengthByMoolaTrikona(za, zb),
				       RashiStrength.OwnHouse                      => grahas.StrengthByOwnHouse(za, zb),
				       RashiStrength.KendraConjunction             => grahas.StrengthByKendraConjunction(za, zb),
				       RashiStrength.KarakaKendradiGrahaDasaLength => grahas.StrengthByKarakaKendradiGrahaDasaLength(za, zb),
				       RashiStrength.First                         => grahas.StrengthByFirst(za, zb),
				       _                                           => throw new Exception("Unknown Rasi Strength Rule")
			       };
		}
	}
}
