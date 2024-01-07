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
		foreach (Body.BodyType b in options.GrahaStrengths.grahas)
		{
			var dasaLength = lengthOfDasa(b);
			al.Add(new DasaEntry(b, cycle_start + curr, dasaLength, 1, b.ToShortString()));
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

			al.Add(new DasaEntry(de.graha, cycle_start + curr, dasaLength, 1, de.graha.ToShortString()));
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
		foreach (Body.BodyType b in options.GrahaStrengths.grahas)
		{
			if (b != pdi.graha && bFound == false)
			{
				continue;
			}

			bFound = true;
			bOrder.Add(b);
		}

		foreach (Body.BodyType b in options.GrahaStrengths.grahas)
		{
			if (b == pdi.graha)
			{
				break;
			}

			bOrder.Add(b);
		}


		var dasaLength = pdi.dasaLength / 9.0;
		foreach (Body.BodyType b in bOrder)
		{
			al.Add(new DasaEntry(b, curr, dasaLength, pdi.level + 1, pdi.shortDesc + " " + b.ToShortString()));
			curr += dasaLength;
		}

		return al;
	}

	public string Description()
	{
		return string.Format("Karakas Kendradi Graha Dasa seeded from {0}", options.SeedBody);
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

	public double lengthOfDasa(Body.BodyType plt)
	{
		var dp_plt = h.getPosition(plt).toDivisionPosition(new Division(Vargas.DivisionType.Rasi));
		return LengthOfDasa(h, options.dtype, plt, dp_plt);
	}

	public static double LengthOfDasa(Horoscope h, Division dtype, Body.BodyType plt, DivisionPosition dp_plt)
	{
		double length = 0;

		// Count to moola trikona - 1.
		// Use Aqu / Sco as MT houses for Rahu / Ketu
		//DivisionPosition dp_plt = h.getPosition(plt).toDivisionPosition(new Division(Vargas.DivisionType.Rasi));
		var zh_plt = dp_plt.zodiac_house;
		var zh_mt  = plt.GetMoolaTrikonaRasi();

		if (plt == Body.BodyType.Rahu)
		{
			zh_mt.Sign = ZodiacHouse.Rasi.Aqu;
		}

		if (plt == Body.BodyType.Ketu)
		{
			zh_mt.Sign = ZodiacHouse.Rasi.Sco;
		}

		var diff = zh_plt.NumHousesBetween(zh_mt);
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
		public           Division  dtype = new(Vargas.DivisionType.Rasi);
		protected        Body.BodyType mSeedBody;
		private          ArrayList std_div_pos;

		public UserOptions(Horoscope _h)
		{
			h           = _h;
			std_div_pos = h.CalculateDivisionPositions(dtype);
			recalculate();
		}

		[Category("Strengths1 Seed")]
		[PGDisplayName("Seed Body")]
		public Body.BodyType SeedBody
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
					if ((ZodiacHouse.Rasi) newOpts.RasiStrengths[i].houses[j] != (ZodiacHouse.Rasi) RasiStrengths[i].houses[j])
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
			for (var i = (int) Body.BodyType.Sun; i <= (int) Body.BodyType.Rahu; i++)
			{
				var b   = (Body.BodyType) i;
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
			var zh   = h.getPosition(SeedBody).toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house;

			var zh_k = new ZodiacHouse.Rasi[4]
			{
				zh.Add(1).Sign,
				zh.Add(4).Sign,
				zh.Add(7).Sign,
				zh.Add(10).Sign
			};
			var zh_p = new ZodiacHouse.Rasi[4]
			{
				zh.Add(2).Sign,
				zh.Add(5).Sign,
				zh.Add(8).Sign,
				zh.Add(11).Sign
			};
			var zh_a = new ZodiacHouse.Rasi[4]
			{
				zh.Add(3).Sign,
				zh.Add(6).Sign,
				zh.Add(9).Sign,
				zh.Add(12).Sign
			};

			var fs = new FindStronger(h, dtype, FindStronger.RulesKarakaKendradiGrahaDasaRasi(h));
			zRet[0] = fs.getOrderedHouses(zh_k);
			zRet[1] = fs.getOrderedHouses(zh_p);
			zRet[2] = fs.getOrderedHouses(zh_a);

			var zh_sat = h.getPosition(Body.BodyType.Saturn).toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house.Sign;
			var zh_ket = h.getPosition(Body.BodyType.Ketu).toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house.Sign;

			var bIsForward = zh.IsOdd();
			if (zh_sat != zh_ket && zh_sat == zh.Sign)
			{
				bIsForward = true;
			}
			else if (zh_sat != zh_ket && zh_ket == zh.Sign)
			{
				bIsForward = false;
			}
			else if (zh_sat == zh_ket && zh_sat == zh.Sign)
			{
				var rule = new ArrayList();
				rule.Add(FindStronger.EGrahaStrength.Longitude);
				var fs2 = new FindStronger(h, new Division(Vargas.DivisionType.Rasi), rule);
				bIsForward = fs2.CmpGraha(Body.BodyType.Saturn, Body.BodyType.Ketu, false);
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
				foreach (ZodiacHouse.Rasi zn in oz.houses)
				{
					var temp     = fs_temp.findGrahasInHouse(zn);
					var temp_arr = new Body.BodyType[temp.Count];
					for (var i = 0; i < temp.Count; i++)
					{
						temp_arr[i] = (Body.BodyType) temp[i];
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