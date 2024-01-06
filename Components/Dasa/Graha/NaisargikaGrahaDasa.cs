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
using System.Diagnostics;
using Mhora.Components.Property;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Elements.Calculation.Strength;
using Mhora.Tables;

namespace Mhora.Components.Dasa.Graha;

public class NaisargikaGrahaDasa : Dasa, IDasa
{
	private readonly Horoscope   h;
	private          UserOptions options;

	public NaisargikaGrahaDasa(Horoscope _h)
	{
		h       = _h;
		options = new UserOptions(h);
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
		var al = new ArrayList(36);
		Body.Name[] order =
		{
			Body.Name.Moon,
			Body.Name.Mars,
			Body.Name.Mercury,
			Body.Name.Venus,
			Body.Name.Jupiter,
			Body.Name.Sun,
			Body.Name.Saturn,
			Body.Name.Lagna
		};

		var cycle_start = paramAyus() * cycle;
		var curr        = 0.0;
		foreach (var bn in order)
		{
			var dasaLength = lengthOfDasa(bn);
			al.Add(new DasaEntry(bn, cycle_start + curr, dasaLength, 1, Body.toShortString(bn)));
			curr += dasaLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var orderedAntar = new OrderedGrahas();
		var lzh          = h.getPosition(pdi.graha).toDivisionPosition(options.dtype).zodiac_house;
		var kendra_start = (int) Basics.normalize_exc_lower(0, 3, (int) lzh.value % 3);
		for (var i = kendra_start; i <= 2; i++)
		{
			foreach (Body.Name b in options.GrahaStrengths[i].grahas)
			{
				orderedAntar.grahas.Add(b);
			}
		}

		for (var i = 0; i < kendra_start; i++)
		{
			foreach (Body.Name b in options.GrahaStrengths[i].grahas)
			{
				orderedAntar.grahas.Add(b);
			}
		}

		var size              = orderedAntar.grahas.Count;
		var antarLengths      = new double[size];
		var totalAntarLengths = 0.0;
		var ret               = new ArrayList(size - 1);


		for (var i = 0; i < size; i++)
		{
			if (ExcludeGraha(pdi, (Body.Name) orderedAntar.grahas[i]))
			{
				continue;
			}

			var diff = lzh.numHousesBetween(h.getPosition((Body.Name) orderedAntar.grahas[i]).toDivisionPosition(options.dtype).zodiac_house);
			switch (diff)
			{
				case 7:
					antarLengths[i]   =  12.0;
					totalAntarLengths += antarLengths[i];
					break;
				case 1:
					antarLengths[i]   =  42.0;
					totalAntarLengths += antarLengths[i];
					break;
				case 4:
				case 8:
					antarLengths[i]   =  21.0;
					totalAntarLengths += antarLengths[i];
					break;
				case 5:
				case 9:
					antarLengths[i]   =  28.0;
					totalAntarLengths += antarLengths[i];
					break;
				case 2:
				case 3:
				case 6:
				case 10:
				case 11:
				case 12:
					antarLengths[i]   =  84.0;
					totalAntarLengths += antarLengths[i];
					break;
				default:
					Trace.Assert(false, "Naisargika Dasa Antardasa lengths Internal Error 1");
					break;
			}
		}

		var curr = pdi.startUT;
		for (var i = 0; i < size; i++)
		{
			var bn = (Body.Name) orderedAntar.grahas[i];

			if (ExcludeGraha(pdi, bn))
			{
				continue;
			}

			var length = antarLengths[i] / totalAntarLengths * pdi.dasaLength;
			var desc   = pdi.shortDesc + " " + Body.toShortString(bn);
			ret.Add(new DasaEntry(bn, curr, length, pdi.level + 1, desc));
			curr += length;
		}

		return ret;
	}

	public string Description()
	{
		return "Naisargika Graha Dasa (SR)";
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
		switch (plt)
		{
			case Body.Name.Sun:     return 20;
			case Body.Name.Moon:    return 1;
			case Body.Name.Mars:    return 2;
			case Body.Name.Mercury: return 9;
			case Body.Name.Jupiter: return 18;
			case Body.Name.Venus:   return 20;
			case Body.Name.Saturn:  return 50;
			case Body.Name.Lagna:   return 0;
		}

		Trace.Assert(false, "NaisargikaGrahaDasa::lengthOfDasa");
		return 0;
	}

	private bool ExcludeGraha(DasaEntry pdi, Body.Name graha)
	{
		if (options.ExcludeDasaLord && graha == pdi.graha)
		{
			return true;
		}

		if (options.ExcludeNodes && (graha == Body.Name.Rahu || graha == Body.Name.Ketu))
		{
			return true;
		}

		var diff = 0;
		if (options.Exclude_3_10 || options.Exclude_2_6_11_12)
		{
			var zhDasa  = h.getPosition(pdi.graha).toDivisionPosition(options.dtype).zodiac_house;
			var zhAntar = h.getPosition(graha).toDivisionPosition(options.dtype).zodiac_house;
			diff = zhDasa.numHousesBetween(zhAntar);
		}

		if (options.Exclude_3_10 && (diff == 3 || diff == 10))
		{
			return true;
		}

		if (options.Exclude_2_6_11_12 && (diff == 2 || diff == 6 || diff == 11 || diff == 12))
		{
			return true;
		}

		return false;
	}

	public class UserOptions : ICloneable
	{
		private readonly Horoscope h;
		public           Division  dtype = new(Basics.DivisionType.Rasi);
		protected        Body.Name mLordAqu;
		protected        Body.Name mLordSco;
		private          ArrayList std_div_pos;

		public UserOptions(Horoscope _h)
		{
			h           = _h;
			std_div_pos = h.CalculateDivisionPositions(dtype);
			var fs = new FindStronger(h, dtype, FindStronger.RulesStrongerCoLord(h));
			mLordSco          = fs.StrongerGraha(Body.Name.Mars, Body.Name.Ketu, true);
			mLordAqu          = fs.StrongerGraha(Body.Name.Saturn, Body.Name.Rahu, true);
			ExcludeNodes      = true;
			ExcludeDasaLord   = true;
			Exclude_3_10      = false;
			Exclude_2_6_11_12 = false;
			CalculateRasiStrengths();
			CalculateGrahaStrengths();
		}

		[Category("1: Colord")]
		[PropertyOrder(1)]
		[PGDisplayName("Colord")]
		[Description("Is Ketu or Mars the stronger lord of Scorpio?")]
		public Body.Name Lord_Sco
		{
			get => mLordSco;
			set => mLordSco = value;
		}

		[Category("1: Colord")]
		[PropertyOrder(2)]
		[PGDisplayName("Lord of Aquarius")]
		[Description("Is Rahu or Saturn the stronger lord of Aquarius?")]
		public Body.Name Lord_Aqu
		{
			get => mLordAqu;
			set => mLordAqu = value;
		}

		[Category("2: Strengths")]
		[PropertyOrder(2)]
		[PGDisplayName("Graha strength order")]
		public OrderedGrahas[] GrahaStrengths
		{
			get;
			set;
		}

		[Category("2: Strengths")]
		[PropertyOrder(1)]
		[PGDisplayName("Rasi strength order")]
		public OrderedZodiacHouses[] RasiStrengths
		{
			get;
			set;
		}

		[Category("4: Exclude Antardasas")]
		[PGDisplayName("Exclude Rahu / Ketu")]
		public bool ExcludeNodes
		{
			get;
			set;
		}

		[Category("4: Exclude Antardasas")]
		[PGDisplayName("Exclude dasa lord")]
		public bool ExcludeDasaLord
		{
			get;
			set;
		}

		[Category("4: Exclude Antardasas")]
		[PGDisplayName("Exclude grahas in 3rd & 10th")]
		public bool Exclude_3_10
		{
			get;
			set;
		}

		[Category("4: Exclude Antardasas")]
		[PGDisplayName("Exclude grahas in 2nd, 6th, 11th & 12th")]
		public bool Exclude_2_6_11_12
		{
			get;
			set;
		}

		public object Clone()
		{
			var uo = new UserOptions(h);
			uo.mLordSco      = mLordSco;
			uo.mLordAqu      = mLordAqu;
			uo.RasiStrengths = new OrderedZodiacHouses[3];
			for (var i = 0; i < 3; i++)
			{
				uo.RasiStrengths[i]  = (OrderedZodiacHouses) RasiStrengths[i].Clone();
				uo.GrahaStrengths[i] = (OrderedGrahas) GrahaStrengths[i].Clone();
			}

			uo.ExcludeDasaLord   = ExcludeDasaLord;
			uo.ExcludeNodes      = ExcludeNodes;
			uo.Exclude_2_6_11_12 = Exclude_2_6_11_12;
			uo.Exclude_3_10      = Exclude_3_10;
			return uo;
		}

		public void recalculate()
		{
			var fs = new FindStronger(h, dtype, FindStronger.RulesStrongerCoLord(h));
			mLordSco = fs.StrongerGraha(Body.Name.Mars, Body.Name.Ketu, true);
			mLordAqu = fs.StrongerGraha(Body.Name.Saturn, Body.Name.Rahu, true);
			CalculateRasiStrengths();
			CalculateGrahaStrengths();
		}

		public void CompareAndRecalculate(UserOptions newOpts)
		{
			if (newOpts.mLordAqu != mLordAqu || newOpts.mLordSco != mLordSco)
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

		public void CalculateRasiStrengths()
		{
			var fs = new FindStronger(h, dtype, FindStronger.RulesNaisargikaDasaRasi(h));
			RasiStrengths = fs.ResultsZodiacKendras(h.CalculateDivisionPosition(h.getPosition(Body.Name.Lagna), dtype).zodiac_house.value);
		}

		public void CalculateGrahaStrengths()
		{
			var fs_temp = new StrengthByConjunction(h, dtype);
			var fs      = new FindStronger(h, dtype, FindStronger.RulesNaisargikaDasaGraha(h));
			GrahaStrengths = new OrderedGrahas[3];
			for (var i = 0; i < RasiStrengths.Length; i++)
			{
				GrahaStrengths[i] = new OrderedGrahas();
				var oz = RasiStrengths[i];
				foreach (ZodiacHouse.Name zn in oz.houses)
				{
					var temp     = fs_temp.findGrahasInHouse(zn);
					var temp_arr = new Body.Name[temp.Count];
					for (var j = 0; j < temp.Count; j++)
					{
						temp_arr[j] = (Body.Name) temp[j];
					}

					var sorted = fs.getOrderedGrahas(temp_arr);
					foreach (var bn in sorted)
					{
						GrahaStrengths[i].grahas.Add(bn);
					}
				}
			}
		}
	}
}