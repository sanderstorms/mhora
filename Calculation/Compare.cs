using System;
using Mhora.Calculation.Strength;
using Mhora.Definitions;
using Mhora.Yoga;

namespace Mhora.Calculation
{
	public static class Compare
	{
		public static int GetStronger(this Grahas grahas, Body m, Body n, bool simpleLord, GrahaStrength strength)
		{
			switch (strength)
			{
				case GrahaStrength.Conjunction:
					return grahas.StrengthByConjunction(m, n);
				case GrahaStrength.Exaltation:
					return grahas.StrengthByExaltation(m, n);
				case GrahaStrength.Longitude:
					return grahas.StrengthByLongitude(m, n);
				case GrahaStrength.AtmaKaraka:
					return grahas.StrengthByAtmaKaraka(m, n);
				case GrahaStrength.RasisNature:
					return grahas.StrengthByRasisNature(m, n);
				case GrahaStrength.LordsNature:
					return grahas.StrengthByLordsNature(m, n);
				case GrahaStrength.AspectsRasi:
					return grahas.StrengthByAspectsRasi(m, n, simpleLord);
				case GrahaStrength.AspectsGraha:
					return grahas.StrengthByAspectsGraha(m, n, simpleLord);
				case GrahaStrength.NarayanaDasaLength:
					return grahas.StrengthByNarayanaDasaLength(m, n, simpleLord);
				case GrahaStrength.VimsottariDasaLength:
					return grahas.StrengthByVimsottariDasaLength(m, n);
				case GrahaStrength.MoolaTrikona:
					return grahas.StrengthByMoolaTrikona(m, n);
				case GrahaStrength.OwnHouse:
					return grahas.StrengthByOwnHouse(m, n);
				case GrahaStrength.NotInOwnHouse:
					return 0 - grahas.StrengthByOwnHouse(m, n);
				case GrahaStrength.LordInOwnHouse:
					return grahas.StrengthByLordInOwnHouse(m, n, simpleLord);
				case GrahaStrength.LordInDifferentOddity:
					return grahas.StrengthByLordInDifferentOddity(m, n, simpleLord);
				case GrahaStrength.KendraConjunction:
					return grahas.StrengthByKendraConjunction(m, n);
				case GrahaStrength.KarakaKendradiGrahaDasaLength:
					return grahas.StrengthByKarakaKendradiGrahaDasaLength(m, n);
				case GrahaStrength.First:
					return grahas.StrengthByFirst(m, n);
				default: 
					throw new Exception("Unknown Graha Strength Rule");
			}

		}

		public static int GetStronger(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord, RashiStrength strength)
		{
			switch (strength)
			{
				case RashiStrength.Conjunction:
					return grahas.StrengthByConjunction(za, zb);
				case RashiStrength.Exaltation:
					return grahas.StrengthByExaltation(za, zb);
				case RashiStrength.Longitude:
					return grahas.StrengthByLongitude(za, zb);
				case RashiStrength.AtmaKaraka:
					return grahas.StrengthByAtmaKaraka(za, zb);
				case RashiStrength.LordIsAtmaKaraka:
					return grahas.StrengthByLordIsAtmaKaraka(za, zb, simpleLord);
				case RashiStrength.RasisNature:
					return grahas.StrengthByRasisNature(za, zb);
				case RashiStrength.LordsNature:
					return grahas.StrengthByLordsNature(za, zb);
				case RashiStrength.AspectsRasi:
					return grahas.StrengthByAspectsRasi(za, zb, simpleLord);
				case RashiStrength.AspectsGraha:
					return grahas.StrengthByAspectsGraha(za, zb, simpleLord);
				case RashiStrength.LordInDifferentOddity:
					return grahas.StrengthByLordInDifferentOddity(za, zb, simpleLord);
				case RashiStrength.LordsLongitude:
					return grahas.StrengthByLordsLongitude(za, zb, simpleLord);
				case RashiStrength.NarayanaDasaLength:
					return grahas.StrengthByNarayanaDasaLength(za, zb, simpleLord);
				case RashiStrength.VimsottariDasaLength:
					return grahas.StrengthByVimsottariDasaLength(za, zb);
				case RashiStrength.MoolaTrikona:
					return grahas.StrengthByMoolaTrikona(za, zb);
				case RashiStrength.OwnHouse:
					return grahas.StrengthByOwnHouse(za, zb);
				case RashiStrength.KendraConjunction:
					return grahas.StrengthByKendraConjunction(za, zb);
				case RashiStrength.KarakaKendradiGrahaDasaLength:
					return grahas.StrengthByKarakaKendradiGrahaDasaLength(za, zb);
				case RashiStrength.First:
					return grahas.StrengthByFirst(za, zb);
				default: 
					throw new Exception("Unknown Rasi Strength Rule");
			}
		}
	}
}
