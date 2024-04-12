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

using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Calculation;

public static class FindStronger
{
	public static List<RashiStrength> RulesNaisargikaDasaRasi(this Horoscope h)
	{
		return [..h.StrengthOptions.NaisargikaDasaRasi];
	}

	public static List<RashiStrength> RulesNarayanaDasaRasi(this Horoscope h)
	{
		return [..h.StrengthOptions.NarayanaDasaRasi];
	}

	public static List<RashiStrength> RulesKarakaKendradiGrahaDasaRasi(this Horoscope h)
	{
		return [..h.StrengthOptions.KarakaKendradiGrahaDasaRasi];
	}

	public static List<GrahaStrength> RulesKarakaKendradiGrahaDasaGraha(this Horoscope h)
	{
		return [..h.StrengthOptions.KarakaKendradiGrahaDasaGraha];
	}

	public static List<GrahaStrength> RulesKarakaKendradiGrahaDasaColord(this Horoscope h)
	{
		return [..h.StrengthOptions.KarakaKendradiGrahaDasaColord];
	}

	public static List <RashiStrength> RulesMoolaDasaRasi(this Horoscope h)
	{
		return [..h.StrengthOptions.MoolaDasaRasi];
	}

	public static List <RashiStrength> RulesNavamsaDasaRasi(this Horoscope h)
	{
		return [..h.StrengthOptions.NavamsaDasaRasi];
	}

	public static List<RashiStrength> RulesJaiminiFirstRasi(this Horoscope h)
	{
		var rules = new List<RashiStrength>
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

	public static List< RashiStrength> RulesJaiminiSecondRasi(this Horoscope h)
	{
		var rules = new List< RashiStrength> 
		{
			RashiStrength.AspectsRasi
		};
		return rules;
	}

	public static List<GrahaStrength> RulesNaisargikaDasaGraha(this Horoscope h)
	{
		return [..h.StrengthOptions.NaisargikaDasaGraha];
	}

	public static List<GrahaStrength> RulesVimsottariGraha(this Horoscope h)
	{
		var rules = new List<GrahaStrength>
		{
			GrahaStrength.KendraConjunction,
			GrahaStrength.First
		};
		return rules;
	}

	public static List<GrahaStrength> RulesStrongerCoLord(this Horoscope h)
	{
		return [..h.StrengthOptions.Colord];
	}

	public static OrderedZodiacHouses[] ResultsZodiacKendras(this Grahas grahas, ZodiacHouse zodiacHouse, List<RashiStrength> rules)
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
		zRet[0] = grahas.GetOrderedHouses(zh1, rules);
		zRet[1] = grahas.GetOrderedHouses(zh2, rules);
		zRet[2] = grahas.GetOrderedHouses(zh3, rules);
		return zRet;
	}

	public static ZodiacHouse[] ResultsKendraRasis(this Grahas grahas, ZodiacHouse zodiacHouse, List<RashiStrength> rules)
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
		grahas.GetOrderedRasis(zh1, rules).CopyTo(zRet, 0);
		grahas.GetOrderedRasis(zh2, rules).CopyTo(zRet, 4);
		grahas.GetOrderedRasis(zh3, rules).CopyTo(zRet, 8);
		return zRet;
	}

	public static ZodiacHouse[] ResultsFirstSeventhRasis(this Grahas grahas, List<RashiStrength> rules)
	{
		var zRet = new ZodiacHouse[12];
		grahas.GetOrderedRasis([
			                       ZodiacHouse.Ari,
			                       ZodiacHouse.Lib
		                       ], rules).CopyTo(zRet, 0);
		grahas.GetOrderedRasis([
			                       ZodiacHouse.Tau,
			                       ZodiacHouse.Sco
		                       ], rules).CopyTo(zRet, 2);
		grahas.GetOrderedRasis([
			                       ZodiacHouse.Gem,
			                       ZodiacHouse.Sag
		                       ], rules).CopyTo(zRet, 4);
		grahas.GetOrderedRasis([
			                       ZodiacHouse.Can,
			                       ZodiacHouse.Cap
		                       ], rules).CopyTo(zRet, 6);
		grahas.GetOrderedRasis([
			                       ZodiacHouse.Leo,
			                       ZodiacHouse.Aqu
		                       ], rules).CopyTo(zRet, 8);
		grahas.GetOrderedRasis([
			                       ZodiacHouse.Vir,
			                       ZodiacHouse.Pis
		                       ], rules).CopyTo(zRet, 10);
		return zRet;
	}


	public static Body[] GetOrderedGrahas(this Grahas grahas, List<GrahaStrength> rules)
	{
		Body[] bodies =
		[
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Jupiter,
			Body.Venus,
			Body.Saturn,
			Body.Rahu,
			Body.Ketu
		];
		return grahas.GetOrderedGrahas(bodies, rules);
	}

	public static Body[] GetOrderedGrahas(this Grahas grahas, Body[] bodies, List<GrahaStrength> rules)
	{
		if (bodies.Length <= 1)
		{
			return bodies;
		}

		for (var i = 0; i < bodies.Length - 1; i++)
		{
			for (var j = 0; j < bodies.Length - 1; j++)
			{
				if (grahas.Compare(bodies[j], bodies[j + 1], false, rules, out _)  < 0)
				{
					var temp = bodies[j];
					bodies[j] = bodies[j + 1];
					bodies[j             + 1] = temp;
				}
			}
		}

		return bodies;
	}

	public static ZodiacHouse[] GetOrderedRasis(this Grahas grahas, List<RashiStrength> rules)
	{
		ZodiacHouse[] rasis =
		[
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
		];
		return grahas.GetOrderedRasis(rasis, rules);
	}

	public static OrderedZodiacHouses GetOrderedHouses(this Grahas grahas, ZodiacHouse[] rasis, List<RashiStrength> rules)
	{
		var zhOrdered = grahas.GetOrderedRasis(rasis, rules);
		var oz         = new OrderedZodiacHouses();
		foreach (var zn in zhOrdered)
		{
			oz.houses.Add(zn);
		}

		return oz;
	}

	public static ZodiacHouse[] GetOrderedRasis(this Grahas grahas, ZodiacHouse[] rasis, List<RashiStrength> rules)
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
				if (grahas.Rashis.Compare(rasis[j], rasis[j + 1], false, rules, out _) < 0)
				{
					var temp = rasis[j];
					rasis[j] = rasis[j + 1];
					rasis[j            + 1] = temp;
				}
			}
		}

		return rasis;
	}
}