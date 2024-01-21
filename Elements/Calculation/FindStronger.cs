/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.Collections;
using System.ComponentModel;
using Mhora.Database.Settings;
using Mhora.Elements.Calculation.Strength;
using Mhora.Util;

namespace Mhora.Elements.Calculation;

public class FindStronger
{
	// Maintain numerical values for forward compatibility
	[TypeConverter(typeof(EnumDescConverter))]
	public enum EGrahaStrength
	{
		[Description("Giving up: Arbitrarily choosing one")]
		First,

		[Description("Graha is conjunct more grahas")]
		Conjunction,

		[Description("Graha is exalted")]
		Exaltation,

		[Description("Graha has higher longitude offset")]
		Longitude,

		[Description("Graha is Atma Karakas")]
		AtmaKaraka,

		[Description("Graha is in a rasi with stronger nature")]
		RasisNature,

		[Description("Graha has more rasi drishti of dispositor, Jup, Mer")]
		AspectsRasi,

		[Description("Graha has more Graha drishti of dispositor, Jup, Mer")]
		AspectsGraha,

		[Description("Graha has a larger narayana dasa length")]
		NarayanaDasaLength,

		[Description("Graha is in its moola trikona rasi")]
		MoolaTrikona,

		[Description("Graha is in own house")]
		OwnHouse,

		[Description("Graha is not in own house")]
		NotInOwnHouse,

		[Description("Graha's dispositor is in own house")]
		LordInOwnHouse,

		[Description("Graha has more grahas in kendras")]
		KendraConjunction,

		[Description("Graha's dispositor is in a rasi with stronger nature")]
		LordsNature,

		[Description("Graha's dispositor is in a rasi with different oddify")]
		LordInDifferentOddity,

		[Description("Graha has a larger Karakas Kendradi Graha Dasa length")]
		KarakaKendradiGrahaDasaLength,

		[Description("Graha has a larger Vimsottari Dasa length")]
		VimsottariDasaLength
	}

	// Maintain numerical values for forward compatibility
	[TypeConverter(typeof(EnumDescConverter))]
	public enum ERasiStrength
	{
		[Description("Giving up: Arbitrarily choosing one")]
		First,

		[Description("Rasi has more grahas in it")]
		Conjunction,

		[Description("Rasi contains more exalted grahas")]
		Exaltation,

		[Description("Rasi has a Graha with higher longitude offset")]
		Longitude,

		[Description("Rasi contains Atma Karakas")]
		AtmaKaraka,

		[Description("Rasi's lord is Atma Karakas")]
		LordIsAtmaKaraka,

		[Description("Rasi is stronger by nature")]
		RasisNature,

		[Description("Rasi has more rasi drishtis of lord, Mer, Jup")]
		AspectsRasi,

		[Description("Rasi has more Graha drishtis of lord, Mer, Jup")]
		AspectsGraha,

		[Description("Rasi's lord is in a rasi of different oddity")]
		LordInDifferentOddity,

		[Description("Rasi's lord has a higher longitude offset")]
		LordsLongitude,

		[Description("Rasi has longer narayana dasa length")]
		NarayanaDasaLength,

		[Description("Rasi has a Graha in moolatrikona")]
		MoolaTrikona,

		[Description("Rasi's lord is place there")]
		OwnHouse,

		[Description("Rasi has more grahas in kendras")]
		KendraConjunction,

		[Description("Rasi's dispositor is stronger by nature")]
		LordsNature,

		[Description("Rasi has a Graha with longer karaka kendradi Graha dasa length")]
		KarakaKendradiGrahaDasaLength,

		[Description("Rasi has a Graha with longer vimsottari dasa length")]
		VimsottariDasaLength
	}

	private readonly Division  dtype;
	private readonly Horoscope h;
	private readonly ArrayList rules;

	private bool bUseSimpleLords;


	public FindStronger(Horoscope _h, Division _dtype, ArrayList _rules, bool _UseSimpleLords)
	{
		h               = _h;
		dtype           = _dtype;
		rules           = _rules;
		bUseSimpleLords = _UseSimpleLords;
	}

	public FindStronger(Horoscope _h, Division _dtype, ArrayList _rules)
	{
		h               = _h;
		dtype           = _dtype;
		rules           = _rules;
		bUseSimpleLords = false;
	}

	private static StrengthOptions GetStrengthOptions(Horoscope h)
	{
		if (h.StrengthOptions == null)
		{
			return MhoraGlobalOptions.Instance.SOptions;
		}

		return h.StrengthOptions;
	}

	public static ArrayList RulesNaisargikaDasaRasi(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).NaisargikaDasaRasi);
	}

	public static ArrayList RulesNarayanaDasaRasi(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).NarayanaDasaRasi);
	}

	public static ArrayList RulesKarakaKendradiGrahaDasaRasi(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).KarakaKendradiGrahaDasaRasi);
	}

	public static ArrayList RulesKarakaKendradiGrahaDasaGraha(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).KarakaKendradiGrahaDasaGraha);
	}

	public static ArrayList RulesKarakaKendradiGrahaDasaColord(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).KarakaKendradiGrahaDasaColord);
	}

	public static ArrayList RulesMoolaDasaRasi(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).MoolaDasaRasi);
	}

	public static ArrayList RulesNavamsaDasaRasi(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).NavamsaDasaRasi);
	}

	public static ArrayList RulesJaiminiFirstRasi(Horoscope h)
	{
		var Rules = new ArrayList();
		Rules.Add(ERasiStrength.AtmaKaraka);
		Rules.Add(ERasiStrength.Conjunction);
		Rules.Add(ERasiStrength.Exaltation);
		Rules.Add(ERasiStrength.MoolaTrikona);
		Rules.Add(ERasiStrength.OwnHouse);
		Rules.Add(ERasiStrength.RasisNature);
		Rules.Add(ERasiStrength.LordIsAtmaKaraka);
		Rules.Add(ERasiStrength.LordsLongitude);
		Rules.Add(ERasiStrength.LordInDifferentOddity);
		return Rules;
	}

	public static ArrayList RulesJaiminiSecondRasi(Horoscope h)
	{
		var Rules = new ArrayList();
		Rules.Add(ERasiStrength.AspectsRasi);
		return Rules;
	}

	public static ArrayList RulesNaisargikaDasaGraha(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).NaisargikaDasaGraha);
	}

	public static ArrayList RulesVimsottariGraha(Horoscope h)
	{
		var Rules = new ArrayList();
		Rules.Add(EGrahaStrength.KendraConjunction);
		Rules.Add(EGrahaStrength.First);
		return Rules;
	}

	public static ArrayList RulesStrongerCoLord(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).Colord);
	}

	public OrderedZodiacHouses[] ResultsZodiacKendras(ZodiacHouse.Rasi _zh)
	{
		var zRet = new OrderedZodiacHouses[3];
		var zh   = new ZodiacHouse(_zh);
		var zh1 = new ZodiacHouse.Rasi[4]
		{
			zh.Add(1).Sign,
			zh.Add(4).Sign,
			zh.Add(7).Sign,
			zh.Add(10).Sign
		};
		var zh2 = new ZodiacHouse.Rasi[4]
		{
			zh.Add(2).Sign,
			zh.Add(5).Sign,
			zh.Add(8).Sign,
			zh.Add(11).Sign
		};
		var zh3 = new ZodiacHouse.Rasi[4]
		{
			zh.Add(3).Sign,
			zh.Add(6).Sign,
			zh.Add(9).Sign,
			zh.Add(12).Sign
		};
		zRet[0] = getOrderedHouses(zh1);
		zRet[1] = getOrderedHouses(zh2);
		zRet[2] = getOrderedHouses(zh3);
		return zRet;
	}

	public ZodiacHouse.Rasi[] ResultsKendraRasis(ZodiacHouse.Rasi _zh)
	{
		var zRet = new ZodiacHouse.Rasi[12];
		var zh   = new ZodiacHouse(_zh);
		var zh1 = new ZodiacHouse.Rasi[4]
		{
			zh.Add(1).Sign,
			zh.Add(4).Sign,
			zh.Add(7).Sign,
			zh.Add(10).Sign
		};
		var zh2 = new ZodiacHouse.Rasi[4]
		{
			zh.Add(2).Sign,
			zh.Add(5).Sign,
			zh.Add(8).Sign,
			zh.Add(11).Sign
		};
		var zh3 = new ZodiacHouse.Rasi[4]
		{
			zh.Add(3).Sign,
			zh.Add(6).Sign,
			zh.Add(9).Sign,
			zh.Add(12).Sign
		};
		getOrderedRasis(zh1).CopyTo(zRet, 0);
		getOrderedRasis(zh2).CopyTo(zRet, 4);
		getOrderedRasis(zh3).CopyTo(zRet, 8);
		return zRet;
	}

	public ZodiacHouse.Rasi[] ResultsFirstSeventhRasis()
	{
		var zRet = new ZodiacHouse.Rasi[12];
		getOrderedRasis(new[]
		{
			ZodiacHouse.Rasi.Ari,
			ZodiacHouse.Rasi.Lib
		}).CopyTo(zRet, 0);
		getOrderedRasis(new[]
		{
			ZodiacHouse.Rasi.Tau,
			ZodiacHouse.Rasi.Sco
		}).CopyTo(zRet, 2);
		getOrderedRasis(new[]
		{
			ZodiacHouse.Rasi.Gem,
			ZodiacHouse.Rasi.Sag
		}).CopyTo(zRet, 4);
		getOrderedRasis(new[]
		{
			ZodiacHouse.Rasi.Can,
			ZodiacHouse.Rasi.Cap
		}).CopyTo(zRet, 6);
		getOrderedRasis(new[]
		{
			ZodiacHouse.Rasi.Leo,
			ZodiacHouse.Rasi.Aqu
		}).CopyTo(zRet, 8);
		getOrderedRasis(new[]
		{
			ZodiacHouse.Rasi.Vir,
			ZodiacHouse.Rasi.Pis
		}).CopyTo(zRet, 10);
		return zRet;
	}


	public Body.BodyType[] getOrderedGrahas()
	{
		Body.BodyType[] grahas =
		{
			Body.BodyType.Sun,
			Body.BodyType.Moon,
			Body.BodyType.Mars,
			Body.BodyType.Mercury,
			Body.BodyType.Jupiter,
			Body.BodyType.Venus,
			Body.BodyType.Saturn,
			Body.BodyType.Rahu,
			Body.BodyType.Ketu
		};
		return getOrderedGrahas(grahas);
	}

	public Body.BodyType[] getOrderedGrahas(Body.BodyType[] grahas)
	{
		if (grahas.Length <= 1)
		{
			return grahas;
		}

		for (var i = 0; i < grahas.Length - 1; i++)
		{
			for (var j = 0; j < grahas.Length - 1; j++)
			{
				if (false == CmpGraha(grahas[j], grahas[j + 1], false))
				{
					var temp = grahas[j];
					grahas[j] = grahas[j + 1];
					grahas[j             + 1] = temp;
				}
			}
		}

		return grahas;
	}

	public ZodiacHouse.Rasi[] getOrderedRasis()
	{
		ZodiacHouse.Rasi[] rasis =
		{
			ZodiacHouse.Rasi.Ari,
			ZodiacHouse.Rasi.Tau,
			ZodiacHouse.Rasi.Gem,
			ZodiacHouse.Rasi.Can,
			ZodiacHouse.Rasi.Leo,
			ZodiacHouse.Rasi.Vir,
			ZodiacHouse.Rasi.Lib,
			ZodiacHouse.Rasi.Sco,
			ZodiacHouse.Rasi.Sag,
			ZodiacHouse.Rasi.Cap,
			ZodiacHouse.Rasi.Aqu,
			ZodiacHouse.Rasi.Pis
		};
		return getOrderedRasis(rasis);
	}

	public OrderedZodiacHouses getOrderedHouses(ZodiacHouse.Rasi[] rasis)
	{
		var zh_ordered = getOrderedRasis(rasis);
		var oz         = new OrderedZodiacHouses();
		foreach (var zn in zh_ordered)
		{
			oz.houses.Add(zn);
		}

		return oz;
	}

	public ZodiacHouse.Rasi[] getOrderedRasis(ZodiacHouse.Rasi[] rasis)
	{
		if (rasis.Length < 2)
		{
			return rasis;
		}

		var length = rasis.Length;
		for (var i = 0; i < length; i++)
		{
			for (var j = 0; j < length - 1; j++)
			{
				//System.mhora.Log.Debug ("Comparing {0} and {1}", i, j);
				if (false == CmpRasi(rasis[j], rasis[j + 1], false))
				{
					var temp = rasis[j];
					rasis[j] = rasis[j + 1];
					rasis[j            + 1] = temp;
				}
			}
		}

		return rasis;
	}

	public ZodiacHouse.Rasi StrongerRasi(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb, bool bSimpleLord, ref int winner)
	{
		if (CmpRasi(za, zb, bSimpleLord, ref winner))
		{
			return za;
		}

		return zb;
	}

	public ZodiacHouse.Rasi StrongerRasi(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb, bool bSimpleLord)
	{
		var winner = 0;
		return StrongerRasi(za, zb, bSimpleLord, ref winner);
	}

	public Body.BodyType StrongerGraha(Body.BodyType m, Body.BodyType n, bool bSimpleLord)
	{
		var winner = 0;
		return StrongerGraha(m, n, bSimpleLord, ref winner);
	}

	public Body.BodyType StrongerGraha(Body.BodyType m, Body.BodyType n, bool bSimpleLord, ref int winner)
	{
		if (CmpGraha(m, n, bSimpleLord, ref winner))
		{
			return m;
		}

		return n;
	}

	public ZodiacHouse.Rasi WeakerRasi(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb, bool bSimpleLord)
	{
		if (CmpRasi(za, zb, bSimpleLord))
		{
			return zb;
		}

		return za;
	}

	public Body.BodyType WeakerGraha(Body.BodyType m, Body.BodyType n, bool bSimpleLord)
	{
		if (CmpGraha(m, n, bSimpleLord))
		{
			return n;
		}

		return m;
	}

	public bool CmpRasi(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb, bool bSimpleLord)
	{
		var winner = 0;
		return CmpRasi(za, zb, bSimpleLord, ref winner);
	}

	public bool CmpRasi(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb, bool bSimpleLord, ref int winner)
	{
		var bRet   = false;
		var bFound = true;
		winner = 0;

		//System.mhora.Log.Debug("Rasi: {0} {1}", za.ToString(), zb.ToString());
		foreach (ERasiStrength s in rules)
		{
			//System.mhora.Log.Debug("Rasi::{0}", s);
			switch (s)
			{
				case ERasiStrength.Conjunction:
					try
					{
						bRet = new StrengthByConjunction(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.Exaltation:
					try
					{
						bRet = new StrengthByExaltation(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.Longitude:
					try
					{
						bRet = new StrengthByLongitude(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.AtmaKaraka:
					try
					{
						bRet = new StrengthByAtmaKaraka(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.LordIsAtmaKaraka:
					try
					{
						bRet = new StrengthByLordIsAtmaKaraka(h, dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.RasisNature:
					try
					{
						bRet = new StrengthByRasisNature(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.LordsNature:
					try
					{
						bRet = new StrengthByLordsNature(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.AspectsRasi:
					try
					{
						bRet = new StrengthByAspectsRasi(h, dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.AspectsGraha:
					try
					{
						bRet = new StrengthByAspectsGraha(h, dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.LordInDifferentOddity:
					try
					{
						bRet = new StrengthByLordInDifferentOddity(h, dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.LordsLongitude:
					try
					{
						bRet = new StrengthByLordsLongitude(h, dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.NarayanaDasaLength:
					try
					{
						bRet = new StrengthByNarayanaDasaLength(h, dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.VimsottariDasaLength:
					try
					{
						bRet = new StrengthByVimsottariDasaLength(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.MoolaTrikona:
					try
					{
						bRet = new StrengthByMoolaTrikona(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.OwnHouse:
					try
					{
						bRet = new StrengthByOwnHouse(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.KendraConjunction:
					try
					{
						bRet = new StrengthByKendraConjunction(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.KarakaKendradiGrahaDasaLength:
					try
					{
						bRet = new StrengthByKarakaKendradiGrahaDasaLength(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case ERasiStrength.First:
					try
					{
						bRet = new StrengthByFirst(h, dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				default: throw new Exception("Unknown Rasi Strength Rule");
			}
		}

		bFound = false;

	found:
		if (bFound)
		{
			return bRet;
		}

		return true;
	}

	public bool CmpGraha(Body.BodyType m, Body.BodyType n, bool bSimpleLord)
	{
		var winner = 0;
		return CmpGraha(m, n, bSimpleLord, ref winner);
	}

	public bool CmpGraha(Body.BodyType m, Body.BodyType n, bool bSimpleLord, ref int winner)
	{
		var bRet   = false;
		var bFound = true;
		winner = 0;
		foreach (EGrahaStrength s in rules)
		{
			//mhora.Log.Debug("Trying {0}. Curr is {1}", s, winner);
			switch (s)
			{
				case EGrahaStrength.Conjunction:
					try
					{
						bRet = new StrengthByConjunction(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.Exaltation:
					try
					{
						bRet = new StrengthByExaltation(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.Longitude:
					try
					{
						bRet = new StrengthByLongitude(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.AtmaKaraka:
					try
					{
						bRet = new StrengthByAtmaKaraka(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.RasisNature:
					try
					{
						bRet = new StrengthByRasisNature(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.LordsNature:
					try
					{
						bRet = new StrengthByLordsNature(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.AspectsRasi:
					try
					{
						bRet = new StrengthByAspectsRasi(h, dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.AspectsGraha:
					try
					{
						bRet = new StrengthByAspectsGraha(h, dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.NarayanaDasaLength:
					try
					{
						bRet = new StrengthByNarayanaDasaLength(h, dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.VimsottariDasaLength:
					try
					{
						bRet = new StrengthByVimsottariDasaLength(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.MoolaTrikona:
					try
					{
						bRet = new StrengthByMoolaTrikona(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.OwnHouse:
					try
					{
						bRet = new StrengthByOwnHouse(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.NotInOwnHouse:
					try
					{
						bRet = !new StrengthByOwnHouse(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.LordInOwnHouse:
					try
					{
						bRet = new StrengthByLordInOwnHouse(h, dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.LordInDifferentOddity:
					try
					{
						bRet = new StrengthByLordInDifferentOddity(h, dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.KendraConjunction:
					try
					{
						bRet = new StrengthByKendraConjunction(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.KarakaKendradiGrahaDasaLength:
					try
					{
						bRet = new StrengthByKarakaKendradiGrahaDasaLength(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case EGrahaStrength.First:
					try
					{
						bRet = new StrengthByFirst(h, dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				default: throw new Exception("Unknown Graha Strength Rule");
			}
		}

		bFound = false;

	found:
		if (bFound)
		{
			return bRet;
		}

		winner++;
		return true;
	}
}