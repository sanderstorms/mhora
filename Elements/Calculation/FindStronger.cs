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
using Mhora.Util;

namespace Mhora.Elements.Calculation;

public class FindStronger
{

	private readonly Division  _dtype;
	private readonly Horoscope _h;
	private readonly ArrayList _rules;

	private bool _bUseSimpleLords;


	public FindStronger(Horoscope h, Division dtype, ArrayList rules, bool useSimpleLords)
	{
		_h         = h;
		_dtype     = dtype;
		_rules     = rules;
		_bUseSimpleLords = useSimpleLords;
	}

	public FindStronger(Horoscope h, Division dtype, ArrayList rules)
	{
		_h         = h;
		_dtype     = dtype;
		_rules     = rules;
		_bUseSimpleLords = false;
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
		var rules = new ArrayList();
		rules.Add(RashiStrength.AtmaKaraka);
		rules.Add(RashiStrength.Conjunction);
		rules.Add(RashiStrength.Exaltation);
		rules.Add(RashiStrength.MoolaTrikona);
		rules.Add(RashiStrength.OwnHouse);
		rules.Add(RashiStrength.RasisNature);
		rules.Add(RashiStrength.LordIsAtmaKaraka);
		rules.Add(RashiStrength.LordsLongitude);
		rules.Add(RashiStrength.LordInDifferentOddity);
		return rules;
	}

	public static ArrayList RulesJaiminiSecondRasi(Horoscope h)
	{
		var rules = new ArrayList();
		rules.Add(RashiStrength.AspectsRasi);
		return rules;
	}

	public static ArrayList RulesNaisargikaDasaGraha(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).NaisargikaDasaGraha);
	}

	public static ArrayList RulesVimsottariGraha(Horoscope h)
	{
		var rules = new ArrayList();
		rules.Add(GrahaStrength.KendraConjunction);
		rules.Add(GrahaStrength.First);
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

	public bool CmpRasi(ZodiacHouse za, ZodiacHouse zb, bool bSimpleLord, ref int winner)
	{
		var bRet   = false;
		var bFound = true;
		winner = 0;

		//System.Mhora.Log.Debug("Rasi: {0} {1}", za.ToString(), zb.ToString());
		foreach (RashiStrength s in _rules)
		{
			//System.Mhora.Log.Debug("Rasi::{0}", s);
			switch (s)
			{
				case RashiStrength.Conjunction:
					try
					{
						bRet = new StrengthByConjunction(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.Exaltation:
					try
					{
						bRet = new StrengthByExaltation(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.Longitude:
					try
					{
						bRet = new StrengthByLongitude(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.AtmaKaraka:
					try
					{
						bRet = new StrengthByAtmaKaraka(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.LordIsAtmaKaraka:
					try
					{
						bRet = new StrengthByLordIsAtmaKaraka(_h, _dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.RasisNature:
					try
					{
						bRet = new StrengthByRasisNature(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.LordsNature:
					try
					{
						bRet = new StrengthByLordsNature(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.AspectsRasi:
					try
					{
						bRet = new StrengthByAspectsRasi(_h, _dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.AspectsGraha:
					try
					{
						bRet = new StrengthByAspectsGraha(_h, _dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.LordInDifferentOddity:
					try
					{
						bRet = new StrengthByLordInDifferentOddity(_h, _dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.LordsLongitude:
					try
					{
						bRet = new StrengthByLordsLongitude(_h, _dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.NarayanaDasaLength:
					try
					{
						bRet = new StrengthByNarayanaDasaLength(_h, _dtype, bSimpleLord).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.VimsottariDasaLength:
					try
					{
						bRet = new StrengthByVimsottariDasaLength(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.MoolaTrikona:
					try
					{
						bRet = new StrengthByMoolaTrikona(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.OwnHouse:
					try
					{
						bRet = new StrengthByOwnHouse(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.KendraConjunction:
					try
					{
						bRet = new StrengthByKendraConjunction(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.KarakaKendradiGrahaDasaLength:
					try
					{
						bRet = new StrengthByKarakaKendradiGrahaDasaLength(_h, _dtype).Stronger(za, zb);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case RashiStrength.First:
					try
					{
						bRet = new StrengthByFirst(_h, _dtype).Stronger(za, zb);
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

	public bool CmpGraha(Body m, Body n, bool bSimpleLord)
	{
		var winner = 0;
		return CmpGraha(m, n, bSimpleLord, ref winner);
	}

	public bool CmpGraha(Body m, Body n, bool bSimpleLord, ref int winner)
	{
		var bRet   = false;
		var bFound = true;
		winner = 0;
		foreach (GrahaStrength s in _rules)
		{
			//Mhora.Log.Debug("Trying {0}. Curr is {1}", s, winner);
			switch (s)
			{
				case GrahaStrength.Conjunction:
					try
					{
						bRet = new StrengthByConjunction(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.Exaltation:
					try
					{
						bRet = new StrengthByExaltation(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.Longitude:
					try
					{
						bRet = new StrengthByLongitude(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.AtmaKaraka:
					try
					{
						bRet = new StrengthByAtmaKaraka(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.RasisNature:
					try
					{
						bRet = new StrengthByRasisNature(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.LordsNature:
					try
					{
						bRet = new StrengthByLordsNature(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.AspectsRasi:
					try
					{
						bRet = new StrengthByAspectsRasi(_h, _dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.AspectsGraha:
					try
					{
						bRet = new StrengthByAspectsGraha(_h, _dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.NarayanaDasaLength:
					try
					{
						bRet = new StrengthByNarayanaDasaLength(_h, _dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.VimsottariDasaLength:
					try
					{
						bRet = new StrengthByVimsottariDasaLength(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.MoolaTrikona:
					try
					{
						bRet = new StrengthByMoolaTrikona(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.OwnHouse:
					try
					{
						bRet = new StrengthByOwnHouse(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.NotInOwnHouse:
					try
					{
						bRet = !new StrengthByOwnHouse(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.LordInOwnHouse:
					try
					{
						bRet = new StrengthByLordInOwnHouse(_h, _dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.LordInDifferentOddity:
					try
					{
						bRet = new StrengthByLordInDifferentOddity(_h, _dtype, bSimpleLord).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.KendraConjunction:
					try
					{
						bRet = new StrengthByKendraConjunction(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.KarakaKendradiGrahaDasaLength:
					try
					{
						bRet = new StrengthByKarakaKendradiGrahaDasaLength(_h, _dtype).Stronger(m, n);
						goto found;
					}
					catch
					{
						winner++;
					}

					break;
				case GrahaStrength.First:
					try
					{
						bRet = new StrengthByFirst(_h, _dtype).Stronger(m, n);
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