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

	private readonly Division  _dtype;
	private readonly Horoscope _h;
	private readonly ArrayList _rules;

	private bool _bUseSimpleLords;


	public FindStronger(Horoscope h, Division dtype, ArrayList rules, bool useSimpleLords)
	{
		this._h         = h;
		this._dtype     = dtype;
		this._rules     = rules;
		_bUseSimpleLords = useSimpleLords;
	}

	public FindStronger(Horoscope h, Division dtype, ArrayList rules)
	{
		this._h         = h;
		this._dtype     = dtype;
		this._rules     = rules;
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
		rules.Add(ERasiStrength.AtmaKaraka);
		rules.Add(ERasiStrength.Conjunction);
		rules.Add(ERasiStrength.Exaltation);
		rules.Add(ERasiStrength.MoolaTrikona);
		rules.Add(ERasiStrength.OwnHouse);
		rules.Add(ERasiStrength.RasisNature);
		rules.Add(ERasiStrength.LordIsAtmaKaraka);
		rules.Add(ERasiStrength.LordsLongitude);
		rules.Add(ERasiStrength.LordInDifferentOddity);
		return rules;
	}

	public static ArrayList RulesJaiminiSecondRasi(Horoscope h)
	{
		var rules = new ArrayList();
		rules.Add(ERasiStrength.AspectsRasi);
		return rules;
	}

	public static ArrayList RulesNaisargikaDasaGraha(Horoscope h)
	{
		return new ArrayList(GetStrengthOptions(h).NaisargikaDasaGraha);
	}

	public static ArrayList RulesVimsottariGraha(Horoscope h)
	{
		var rules = new ArrayList();
		rules.Add(EGrahaStrength.KendraConjunction);
		rules.Add(EGrahaStrength.First);
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


	public Body.BodyType[] GetOrderedGrahas()
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
		return GetOrderedGrahas(grahas);
	}

	public Body.BodyType[] GetOrderedGrahas(Body.BodyType[] grahas)
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

	public ZodiacHouse WeakerRasi(ZodiacHouse za, ZodiacHouse zb, bool bSimpleLord)
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

		//System.mhora.Log.Debug("Rasi: {0} {1}", za.ToString(), zb.ToString());
		foreach (ERasiStrength s in _rules)
		{
			//System.mhora.Log.Debug("Rasi::{0}", s);
			switch (s)
			{
				case ERasiStrength.Conjunction:
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
				case ERasiStrength.Exaltation:
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
				case ERasiStrength.Longitude:
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
				case ERasiStrength.AtmaKaraka:
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
				case ERasiStrength.LordIsAtmaKaraka:
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
				case ERasiStrength.RasisNature:
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
				case ERasiStrength.LordsNature:
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
				case ERasiStrength.AspectsRasi:
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
				case ERasiStrength.AspectsGraha:
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
				case ERasiStrength.LordInDifferentOddity:
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
				case ERasiStrength.LordsLongitude:
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
				case ERasiStrength.NarayanaDasaLength:
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
				case ERasiStrength.VimsottariDasaLength:
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
				case ERasiStrength.MoolaTrikona:
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
				case ERasiStrength.OwnHouse:
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
				case ERasiStrength.KendraConjunction:
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
				case ERasiStrength.KarakaKendradiGrahaDasaLength:
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
				case ERasiStrength.First:
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
		foreach (EGrahaStrength s in _rules)
		{
			//mhora.Log.Debug("Trying {0}. Curr is {1}", s, winner);
			switch (s)
			{
				case EGrahaStrength.Conjunction:
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
				case EGrahaStrength.Exaltation:
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
				case EGrahaStrength.Longitude:
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
				case EGrahaStrength.AtmaKaraka:
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
				case EGrahaStrength.RasisNature:
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
				case EGrahaStrength.LordsNature:
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
				case EGrahaStrength.AspectsRasi:
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
				case EGrahaStrength.AspectsGraha:
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
				case EGrahaStrength.NarayanaDasaLength:
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
				case EGrahaStrength.VimsottariDasaLength:
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
				case EGrahaStrength.MoolaTrikona:
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
				case EGrahaStrength.OwnHouse:
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
				case EGrahaStrength.NotInOwnHouse:
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
				case EGrahaStrength.LordInOwnHouse:
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
				case EGrahaStrength.LordInDifferentOddity:
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
				case EGrahaStrength.KendraConjunction:
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
				case EGrahaStrength.KarakaKendradiGrahaDasaLength:
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
				case EGrahaStrength.First:
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