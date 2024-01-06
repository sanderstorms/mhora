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
using Mhora.Components.Dasa;
using Mhora.Components.Property;
using Mhora.Elements.Calculation;
using Mhora.Elements.Calculation.Strength;
using Mhora.Elements.Dasas.Nakshatra;
using Mhora.Tables;

namespace Mhora.Elements.Dasas.Graha;

public class KarakaKendradiGrahaDasa : Dasa, IDasa
{
	private readonly Horoscope      h;
	private readonly VimsottariDasa vd;
	private          UserOptions    options;

	public KarakaKendradiGrahaDasa(Horoscope _h)
	{
		h       = _h;
		options = new UserOptions(h);
		vd      = new VimsottariDasa(h);
	}

	public double paramAyus()
	{
		return 120.0;
	}

	public void recalculateOptions()
	{
		options.recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var cycle_start = paramAyus() * cycle;
		var curr        = 0.0;
		var al          = new ArrayList(24);
		foreach (Body.Name b in options.GrahaStrengths.grahas)
		{
			var dasaLength = lengthOfDasa(b);
			al.Add(new DasaEntry(b, cycle_start + curr, dasaLength, 1, Body.toShortString(b)));
			curr += dasaLength;
		}

		var numDasas = al.Count;
		for (var i = 0; i < numDasas; i++)
		{
			var de         = (DasaEntry) al[i];
			var dasaLength = de.dasaLength - vd.lengthOfDasa(de.graha);
			if (dasaLength < 0)
			{
				dasaLength *= -1;
			}

			al.Add(new DasaEntry(de.graha, cycle_start + curr, dasaLength, 1, Body.toShortString(de.graha)));
			curr += dasaLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var al   = new ArrayList();
		var curr = pdi.startUT;

		var bOrder = new ArrayList();
		var bFound = false;
		foreach (Body.Name b in options.GrahaStrengths.grahas)
		{
			if (b != pdi.graha && bFound == false)
			{
				continue;
			}

			bFound = true;
			bOrder.Add(b);
		}

		foreach (Body.Name b in options.GrahaStrengths.grahas)
		{
			if (b == pdi.graha)
			{
				break;
			}

			bOrder.Add(b);
		}


		var dasaLength = pdi.dasaLength / 9.0;
		foreach (Body.Name b in bOrder)
		{
			al.Add(new DasaEntry(b, curr, dasaLength, pdi.level + 1, pdi.shortDesc + " " + Body.toShortString(b)));
			curr += dasaLength;
		}

		return al;
	}

	public string Description()
	{
		return string.Format("Karaka Kendradi Graha Dasa seeded from {0}", options.SeedBody);
	}

	public object GetOptions()
	{
		return options.Clone();
	}

	public object SetOptions(object a)
	{
		var newOpts = (UserOptions) a;
		options.CompareAndRecalculate(newOpts);
		options = newOpts;
		RecalculateEvent();
		return options.Clone();
	}

	public double lengthOfDasa(Body.Name plt)
	{
		var dp_plt = h.getPosition(plt).toDivisionPosition(new Division(Basics.DivisionType.Rasi));
		return LengthOfDasa(h, options.dtype, plt, dp_plt);
	}

	public static double LengthOfDasa(Horoscope h, Division dtype, Body.Name plt, DivisionPosition dp_plt)
	{
		double length = 0;

		// Count to moola trikona - 1.
		// Use Aqu / Sco as MT houses for Rahu / Ketu
		//DivisionPosition dp_plt = h.getPosition(plt).toDivisionPosition(new Division(Basics.DivisionType.Rasi));
		var zh_plt = dp_plt.zodiac_house;
		var zh_mt  = Basics.getMoolaTrikonaRasi(plt);

		if (plt == Body.Name.Rahu)
		{
			zh_mt.value = ZodiacHouse.Name.Aqu;
		}

		if (plt == Body.Name.Ketu)
		{
			zh_mt.value = ZodiacHouse.Name.Sco;
		}

		var diff = zh_plt.numHousesBetween(zh_mt);
		length = diff - 1;

		// exaltation / debilitation correction
		if (dp_plt.isExaltedPhalita())
		{
			length += 1.0;
		}
		else if (dp_plt.isDebilitatedPhalita())
		{
			length -= 1.0;
		}

		if (plt == h.LordOfZodiacHouse(zh_plt, dtype))
		{
			length = 12.0;
		}

		// subtract this length from the vimsottari lengths
		length = VimsottariDasa.LengthOfDasa(plt) - length;

		// Zero length = full vimsottari length.
		// If negative, make it positive
		if (length == 0)
		{
			length = VimsottariDasa.LengthOfDasa(plt);
		}
		else if (length < 0)
		{
			length *= -1;
		}

		return length;
	}

	public class UserOptions : ICloneable
	{
		private readonly Horoscope h;
		public           Division  dtype = new(Basics.DivisionType.Rasi);
		protected        Body.Name mSeedBody;
		private          ArrayList std_div_pos;

		public UserOptions(Horoscope _h)
		{
			h           = _h;
			std_div_pos = h.CalculateDivisionPositions(dtype);
			recalculate();
		}

		[Category("Strengths1 Seed")]
		[PGDisplayName("Seed Body")]
		public Body.Name SeedBody
		{
			get => mSeedBody;
			set => mSeedBody = value;
		}


		[Category("Strengths3 Grahas")]
		[PGDisplayName("Graha strength order")]
		public OrderedGrahas GrahaStrengths
		{
			get;
			set;
		}

		[Category("Strengths2 Rasis")]
		[PGDisplayName("Rasi strength order")]
		public OrderedZodiacHouses[] RasiStrengths
		{
			get;
			set;
		}


		public object Clone()
		{
			var uo = new UserOptions(h);
			uo.GrahaStrengths = (OrderedGrahas) GrahaStrengths.Clone();
			uo.RasiStrengths  = new OrderedZodiacHouses[3];
			uo.mSeedBody      = mSeedBody;
			for (var i = 0; i < 3; i++)
			{
				uo.RasiStrengths[i] = (OrderedZodiacHouses) RasiStrengths[i].Clone();
			}

			return uo;
		}

		public void recalculate()
		{
			CalculateSeedBody();
			CalculateRasiStrengths();
			CalculateGrahaStrengths();
		}

		public void CompareAndRecalculate(UserOptions newOpts)
		{
			if (newOpts.SeedBody != SeedBody)
			{
				newOpts.CalculateRasiStrengths();
				newOpts.CalculateGrahaStrengths();
				return;
			}

			for (var i = 0; i < 3; i++)
			{
				if (newOpts.RasiStrengths[i].houses.Count != RasiStrengths[i].houses.Count)
				{
					newOpts.CalculateGrahaStrengths();
					return;
				}

				for (var j = 0; j < newOpts.RasiStrengths[i].houses.Count; j++)
				{
					if ((ZodiacHouse.Name) newOpts.RasiStrengths[i].houses[j] != (ZodiacHouse.Name) RasiStrengths[i].houses[j])
					{
						newOpts.CalculateGrahaStrengths();
						return;
					}
				}
			}
		}

		public void CalculateSeedBody()
		{
			var al_k = new ArrayList();
			for (var i = (int) Body.Name.Sun; i <= (int) Body.Name.Rahu; i++)
			{
				var b   = (Body.Name) i;
				var bp  = h.getPosition(b);
				var bkc = new KarakaComparer(bp);
				al_k.Add(bkc);
			}

			al_k.Sort();

			var bp_ak = ((KarakaComparer) al_k[0]).GetPosition;
			SeedBody = bp_ak.name;
		}

		public void CalculateRasiStrengths()
		{
			var zRet = new OrderedZodiacHouses[3];
			var zh   = h.getPosition(SeedBody).toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house;

			var zh_k = new ZodiacHouse.Name[4]
			{
				zh.add(1).value,
				zh.add(4).value,
				zh.add(7).value,
				zh.add(10).value
			};
			var zh_p = new ZodiacHouse.Name[4]
			{
				zh.add(2).value,
				zh.add(5).value,
				zh.add(8).value,
				zh.add(11).value
			};
			var zh_a = new ZodiacHouse.Name[4]
			{
				zh.add(3).value,
				zh.add(6).value,
				zh.add(9).value,
				zh.add(12).value
			};

			var fs = new FindStronger(h, dtype, FindStronger.RulesKarakaKendradiGrahaDasaRasi(h));
			zRet[0] = fs.getOrderedHouses(zh_k);
			zRet[1] = fs.getOrderedHouses(zh_p);
			zRet[2] = fs.getOrderedHouses(zh_a);

			var zh_sat = h.getPosition(Body.Name.Saturn).toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house.value;
			var zh_ket = h.getPosition(Body.Name.Ketu).toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house.value;

			var bIsForward = zh.isOdd();
			if (zh_sat != zh_ket && zh_sat == zh.value)
			{
				bIsForward = true;
			}
			else if (zh_sat != zh_ket && zh_ket == zh.value)
			{
				bIsForward = false;
			}
			else if (zh_sat == zh_ket && zh_sat == zh.value)
			{
				var rule = new ArrayList();
				rule.Add(FindStronger.EGrahaStrength.Longitude);
				var fs2 = new FindStronger(h, new Division(Basics.DivisionType.Rasi), rule);
				bIsForward = fs2.CmpGraha(Body.Name.Saturn, Body.Name.Ketu, false);
			}


			RasiStrengths    = new OrderedZodiacHouses[3];
			RasiStrengths[0] = zRet[0];
			if (bIsForward)
			{
				RasiStrengths[1] = zRet[1];
				RasiStrengths[2] = zRet[2];
			}
			else
			{
				RasiStrengths[1] = zRet[2];
				RasiStrengths[2] = zRet[1];
			}
		}

		public void CalculateGrahaStrengths()
		{
			var fs_temp = new StrengthByConjunction(h, dtype);
			var fs      = new FindStronger(h, dtype, FindStronger.RulesKarakaKendradiGrahaDasaGraha(h));
			GrahaStrengths = new OrderedGrahas();
			foreach (var oz in RasiStrengths)
			{
				foreach (ZodiacHouse.Name zn in oz.houses)
				{
					var temp     = fs_temp.findGrahasInHouse(zn);
					var temp_arr = new Body.Name[temp.Count];
					for (var i = 0; i < temp.Count; i++)
					{
						temp_arr[i] = (Body.Name) temp[i];
					}

					var sorted = fs.getOrderedGrahas(temp_arr);
					foreach (var bn in sorted)
					{
						GrahaStrengths.grahas.Add(bn);
					}
				}
			}
		}
	}
}