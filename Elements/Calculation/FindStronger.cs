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
using Mhora.Definitions;
using Mhora.Elements.Calculation.Strength;
using Mhora.Elements.Yoga;
using Mhora.Util;

namespace Mhora.Elements.Calculation;

public class FindStronger
{

	private readonly Grahas    _grahas;
	private readonly ArrayList _rules;

	private bool _bUseSimpleLords;


	public FindStronger(Grahas grahas, ArrayList rules, bool useSimpleLords = false)
	{
		_bUseSimpleLords = useSimpleLords;
		_grahas          = grahas;
		_rules           = rules;
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
		var rules = new ArrayList
		{
			RashiStrength.AtmaKaraka,
			RashiStrength.Conjunction,
			RashiStrength.Exaltation,
			RashiStrength.MoolaTrikona,
			RashiStrength.OwnHouse,
			RashiStrength.RasisNature,
			RashiStrength.LordIsAtmaKaraka,
			RashiStrength.LordsLongitude,
			RashiStrength.LordInDifferentOddity
		};
		return rules;
	}

	public static ArrayList RulesJaiminiSecondRasi(Horoscope h)
	{
		var rules = new ArrayList
		{
			RashiStrength.AspectsRasi
		};
		return rules;
	}

	public static ArrayList RulesNaisargikaDasaGraha(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).NaisargikaDasaGraha);
	}

	public static ArrayList RulesVimsottariGraha(Horoscope h)
	{
		var rules = new ArrayList
		{
			GrahaStrength.KendraConjunction,
			GrahaStrength.First
		};
		return rules;
	}

	public static ArrayList RulesStrongerCoLord(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).Colord);
	}

	public OrderedZodiacHouses[] ResultsZodiacKendras(ZodiacHouse zodiacHouse)
	{
		var zRet = new OrderedZodiacHouses[3];
		var zh1 = new ZodiacHouse[4]
		{
			zodiacHouse.Add(1),
			zodiacHouse.Add(4),
			zodiacHouse.Add(7),
			zodiacHouse.Add(10)
		};
		var zh2 = new ZodiacHouse[4]
		{
			zodiacHouse.Add(2),
			zodiacHouse.Add(5),
			zodiacHouse.Add(8),
			zodiacHouse.Add(11)
		};
		var zh3 = new ZodiacHouse[4]
		{
			zodiacHouse.Add(3),
			zodiacHouse.Add(6),
			zodiacHouse.Add(9),
			zodiacHouse.Add(12)
		};
		zRet[0] = GetOrderedHouses(zh1);
		zRet[1] = GetOrderedHouses(zh2);
		zRet[2] = GetOrderedHouses(zh3);
		return zRet;
	}

	public ZodiacHouse[] ResultsKendraRasis(ZodiacHouse zodiacHouse)
	{
		var zRet = new ZodiacHouse[12];
		var zh1 = new ZodiacHouse[4]
		{
			zodiacHouse.Add(1),
			zodiacHouse.Add(4),
			zodiacHouse.Add(7),
			zodiacHouse.Add(10)
		};
		var zh2 = new ZodiacHouse[4]
		{
			zodiacHouse.Add(2),
			zodiacHouse.Add(5),
			zodiacHouse.Add(8),
			zodiacHouse.Add(11)
		};
		var zh3 = new ZodiacHouse[4]
		{
			zodiacHouse.Add(3),
			zodiacHouse.Add(6),
			zodiacHouse.Add(9),
			zodiacHouse.Add(12)
		};
		GetOrderedRasis(zh1).CopyTo(zRet, 0);
		GetOrderedRasis(zh2).CopyTo(zRet, 4);
		GetOrderedRasis(zh3).CopyTo(zRet, 8);
		return zRet;
	}

	public ZodiacHouse[] ResultsFirstSeventhRasis()
	{
		var zRet = new ZodiacHouse[12];
		GetOrderedRasis(new[]
		{
			ZodiacHouse.Ari,
			ZodiacHouse.Lib
		}).CopyTo(zRet, 0);
		GetOrderedRasis(new[]
		{
			ZodiacHouse.Tau,
			ZodiacHouse.Sco
		}).CopyTo(zRet, 2);
		GetOrderedRasis(new[]
		{
			ZodiacHouse.Gem,
			ZodiacHouse.Sag
		}).CopyTo(zRet, 4);
		GetOrderedRasis(new[]
		{
			ZodiacHouse.Can,
			ZodiacHouse.Cap
		}).CopyTo(zRet, 6);
		GetOrderedRasis(new[]
		{
			ZodiacHouse.Leo,
			ZodiacHouse.Aqu
		}).CopyTo(zRet, 8);
		GetOrderedRasis(new[]
		{
			ZodiacHouse.Vir,
			ZodiacHouse.Pis
		}).CopyTo(zRet, 10);
		return zRet;
	}


	public Body[] GetOrderedGrahas()
	{
		Body[] grahas =
		{
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Jupiter,
			Body.Venus,
			Body.Saturn,
			Body.Rahu,
			Body.Ketu
		};
		return GetOrderedGrahas(grahas);
	}

	public Body[] GetOrderedGrahas(Body[] grahas)
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

	public ZodiacHouse[] GetOrderedRasis()
	{
		ZodiacHouse[] rasis =
		{
			ZodiacHouse.Ari,
			ZodiacHouse.Tau,
			ZodiacHouse.Gem,
			ZodiacHouse.Can,
			ZodiacHouse.Leo,
			ZodiacHouse.Vir,
			ZodiacHouse.Lib,
			ZodiacHouse.Sco,
			ZodiacHouse.Sag,
			ZodiacHouse.Cap,
			ZodiacHouse.Aqu,
			ZodiacHouse.Pis
		};
		return GetOrderedRasis(rasis);
	}

	public OrderedZodiacHouses GetOrderedHouses(ZodiacHouse[] rasis)
	{
		var zhOrdered = GetOrderedRasis(rasis);
		var oz         = new OrderedZodiacHouses();
		foreach (var zn in zhOrdered)
		{
			oz.houses.Add(zn);
		}

		return oz;
	}

	public ZodiacHouse[] GetOrderedRasis(ZodiacHouse[] rasis)
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
				//System.Mhora.Log.Debug ("Comparing {0} and {1}", i, j);
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

	public ZodiacHouse StrongerRasi(ZodiacHouse za, ZodiacHouse zb, bool bSimpleLord, ref int winner)
	{
		if (CmpRasi(za, zb, bSimpleLord, ref winner))
		{
			return za;
		}

		return zb;
	}

	public ZodiacHouse StrongerRasi(ZodiacHouse za, ZodiacHouse zb, bool bSimpleLord)
	{
		var winner = 0;
		return StrongerRasi(za, zb, bSimpleLord, ref winner);
	}

	public Body StrongerGraha(Body m, Body n, bool bSimpleLord)
	{
		var winner = 0;
		return StrongerGraha(m, n, bSimpleLord, ref winner);
	}

	public Body StrongerGraha(Body m, Body n, bool bSimpleLord, ref int winner)
	{
		if (CmpGraha(m, n, bSimpleLord, ref winner))
		{
			return m;
		}

		return n;
	}

	public ZodiacHouse WeakerRasi(ZodiacHouse za, ZodiacHouse zb, bool bSimpleLord)
	{
		if (CmpRasi(za, zb, bSimpleLord))
		{
			return zb;
		}

		return za;
	}

	public Body WeakerGraha(Body m, Body n, bool bSimpleLord)
	{
		if (CmpGraha(m, n, bSimpleLord))
		{
			return n;
		}

		return m;
	}

	public bool CmpRasi(ZodiacHouse za, ZodiacHouse zb, bool bSimpleLord)
	{
		var winner = 0;
		return CmpRasi(za, zb, bSimpleLord, ref winner);
	}

	public int CmpRasi(ZodiacHouse za, ZodiacHouse zb, bool simpleLord, RashiStrength rule)
	{
		switch (rule)
		{
			case RashiStrength.Conjunction:
				return new StrengthByConjunction(_grahas).Stronger(za, zb);
			case RashiStrength.Exaltation:
				return new StrengthByExaltation(_grahas).Stronger(za, zb);
			case RashiStrength.Longitude:
				return new StrengthByLongitude(_grahas).Stronger(za, zb);
			case RashiStrength.AtmaKaraka:
				return new StrengthByAtmaKaraka(_grahas).Stronger(za, zb);
			case RashiStrength.LordIsAtmaKaraka:
				return new StrengthByLordIsAtmaKaraka(_grahas, simpleLord).Stronger(za, zb);
			case RashiStrength.RasisNature:
				return new StrengthByRasisNature(_grahas).Stronger(za, zb);
			case RashiStrength.LordsNature:
				return new StrengthByLordsNature(_grahas).Stronger(za, zb);
			case RashiStrength.AspectsRasi:
				return new StrengthByAspectsRasi(_grahas, simpleLord).Stronger(za, zb);
			case RashiStrength.AspectsGraha:
				return new StrengthByAspectsGraha(_grahas, simpleLord).Stronger(za, zb);
			case RashiStrength.LordInDifferentOddity:
				return new StrengthByLordInDifferentOddity(_grahas, simpleLord).Stronger(za, zb);
			case RashiStrength.LordsLongitude:
				return new StrengthByLordsLongitude(_grahas, simpleLord).Stronger(za, zb);
			case RashiStrength.NarayanaDasaLength:
				return new StrengthByNarayanaDasaLength(_grahas, simpleLord).Stronger(za, zb);
			case RashiStrength.VimsottariDasaLength:
				return new StrengthByVimsottariDasaLength(_grahas).Stronger(za, zb);
			case RashiStrength.MoolaTrikona:
				return new StrengthByMoolaTrikona(_grahas).Stronger(za, zb);
			case RashiStrength.OwnHouse:
				return new StrengthByOwnHouse(_grahas).Stronger(za, zb);
			case RashiStrength.KendraConjunction:
				return new StrengthByKendraConjunction(_grahas).Stronger(za, zb);
			case RashiStrength.KarakaKendradiGrahaDasaLength:
				return new StrengthByKarakaKendradiGrahaDasaLength(_grahas).Stronger(za, zb);
			case RashiStrength.First:
				return new StrengthByFirst(_grahas).Stronger(za, zb);
			default: 
				throw new Exception("Unknown Rasi Strength Rule");
		}
	}



	public bool CmpRasi(ZodiacHouse za, ZodiacHouse zb, bool bSimpleLord, ref int winner)
	{
		foreach (RashiStrength s in _rules)
		{
			var result = CmpRasi(za, zb, bSimpleLord, s);
			if (result == 0)
			{
				winner++;
			}
			else
			{
				return (result > 0);
			}
		}
		return true;
	}

	public bool CmpGraha(Body m, Body n, bool bSimpleLord)
	{
		var winner = 0;
		return CmpGraha(m, n, bSimpleLord, ref winner);
	}

	public int CmpGraha(Body m, Body n, bool bSimpleLord, GrahaStrength strength)
	{
		switch (strength)
		{
			case GrahaStrength.Conjunction:
				return new StrengthByConjunction(_grahas).Stronger(m, n);
			case GrahaStrength.Exaltation:
				return new StrengthByExaltation(_grahas).Stronger(m, n);
			case GrahaStrength.Longitude:
				return new StrengthByLongitude(_grahas).Stronger(m, n);
			case GrahaStrength.AtmaKaraka:
				return new StrengthByAtmaKaraka(_grahas).Stronger(m, n);
			case GrahaStrength.RasisNature:
				return new StrengthByRasisNature(_grahas).Stronger(m, n);
			case GrahaStrength.LordsNature:
				return new StrengthByLordsNature(_grahas).Stronger(m, n);
			case GrahaStrength.AspectsRasi:
				return new StrengthByAspectsRasi(_grahas, bSimpleLord).Stronger(m, n);
			case GrahaStrength.AspectsGraha:
				return new StrengthByAspectsGraha(_grahas, bSimpleLord).Stronger(m, n);
			case GrahaStrength.NarayanaDasaLength:
				return new StrengthByNarayanaDasaLength(_grahas, bSimpleLord).Stronger(m, n);
			case GrahaStrength.VimsottariDasaLength:
				return new StrengthByVimsottariDasaLength(_grahas).Stronger(m, n);
			case GrahaStrength.MoolaTrikona:
				return new StrengthByMoolaTrikona(_grahas).Stronger(m, n);
			case GrahaStrength.OwnHouse:
				return new StrengthByOwnHouse(_grahas).Stronger(m, n);
			case GrahaStrength.NotInOwnHouse:
				return 0 - new StrengthByOwnHouse(_grahas).Stronger(m, n);
			case GrahaStrength.LordInOwnHouse:
				return new StrengthByLordInOwnHouse(_grahas, bSimpleLord).Stronger(m, n);
			case GrahaStrength.LordInDifferentOddity:
				return new StrengthByLordInDifferentOddity(_grahas, bSimpleLord).Stronger(m, n);
			case GrahaStrength.KendraConjunction:
				return new StrengthByKendraConjunction(_grahas).Stronger(m, n);
			case GrahaStrength.KarakaKendradiGrahaDasaLength:
				return new StrengthByKarakaKendradiGrahaDasaLength(_grahas).Stronger(m, n);
			case GrahaStrength.First:
				return new StrengthByFirst(_grahas).Stronger(m, n);
			default: 
				throw new Exception("Unknown Graha Strength Rule");
		}
	}


	public bool CmpGraha(Body m, Body n, bool bSimpleLord, ref int winner)
	{
		winner = 0;
		foreach (GrahaStrength s in _rules)
		{
			var result = CmpGraha(m, n, bSimpleLord, s);
			if (result == 0)
			{
				winner++;
			}
			else
			{
				return (result > 0);
			}
		}
		return true;
	}
}