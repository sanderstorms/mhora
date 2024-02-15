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
using Mhora.Definitions;
using Mhora.Elements.Calculation;
using Mhora.Elements.Calculation.Strength;
using Mhora.Util;

namespace Mhora.Elements.Dasas.GrahaDasa;

public class NaisargikaGrahaDasa : Dasa, IDasa
{
	private readonly Horoscope   _h;
	private          UserOptions _options;

	public NaisargikaGrahaDasa(Horoscope h)
	{
		_h       = h;
		_options = new UserOptions(_h);
	}

	public double ParamAyus()
	{
		return 120.0;
	}

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var al = new ArrayList(36);
		Body[] order =
		{
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Venus,
			Body.Jupiter,
			Body.Sun,
			Body.Saturn,
			Body.Lagna
		};

		var cycleStart  = new TimeOffset(ParamAyus() * cycle);
		var curr        = new TimeOffset();
		foreach (var bn in order)
		{
			var dasaLength = LengthOfDasa(bn);
			al.Add(new DasaEntry(bn, cycleStart + curr, dasaLength, 1, bn.ToShortString()));
			curr += dasaLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var orderedAntar = new OrderedGrahas();
		var lzh          = _h.GetPosition(pdi.Graha).ToDivisionPosition(_options.Dtype).ZodiacHouse;
		var kendraStart = (int) Basics.NormalizeExcLower((int) lzh % 3, 0, 3);
		for (var i = kendraStart; i <= 2; i++)
		{
			foreach (Body b in _options.GrahaStrengths[i].grahas)
			{
				orderedAntar.grahas.Add(b);
			}
		}

		for (var i = 0; i < kendraStart; i++)
		{
			foreach (Body b in _options.GrahaStrengths[i].grahas)
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
			if (ExcludeGraha(pdi, (Body) orderedAntar.grahas[i]))
			{
				continue;
			}

			var diff = lzh.NumHousesBetween(_h.GetPosition((Body) orderedAntar.grahas[i]).ToDivisionPosition(_options.Dtype).ZodiacHouse);
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

		var curr = pdi.Start;
		for (var i = 0; i < size; i++)
		{
			var bn = (Body) orderedAntar.grahas[i];

			if (ExcludeGraha(pdi, bn))
			{
				continue;
			}

			var length = antarLengths[i] / totalAntarLengths * pdi.DasaLength;
			var desc   = pdi.DasaName + " " + bn.ToShortString();
			ret.Add(new DasaEntry(bn, curr, length, pdi.Level + 1, desc));
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
		return _options.Clone();
	}

	public object SetOptions(object a)
	{
		var newOpts = (UserOptions) a;
		_options.CompareAndRecalculate(newOpts);
		_options = newOpts;
		RecalculateEvent();
		return _options.Clone();
	}

	public double LengthOfDasa(Body plt)
	{
		switch (plt)
		{
			case Body.Sun:     return 20;
			case Body.Moon:    return 1;
			case Body.Mars:    return 2;
			case Body.Mercury: return 9;
			case Body.Jupiter: return 18;
			case Body.Venus:   return 20;
			case Body.Saturn:  return 50;
			case Body.Lagna:   return 0;
		}

		Trace.Assert(false, "NaisargikaGrahaDasa::LengthOfDasa");
		return 0;
	}

	private bool ExcludeGraha(DasaEntry pdi, Body graha)
	{
		if (_options.ExcludeDasaLord && graha == pdi.Graha)
		{
			return true;
		}

		if (_options.ExcludeNodes && (graha == Body.Rahu || graha == Body.Ketu))
		{
			return true;
		}

		var diff = 0;
		if (_options.Exclude310 || _options.Exclude261112)
		{
			var zhDasa  = _h.GetPosition(pdi.Graha).ToDivisionPosition(_options.Dtype).ZodiacHouse;
			var zhAntar = _h.GetPosition(graha).ToDivisionPosition(_options.Dtype).ZodiacHouse;
			diff = zhDasa.NumHousesBetween(zhAntar);
		}

		if (_options.Exclude310 && (diff == 3 || diff == 10))
		{
			return true;
		}

		if (_options.Exclude261112 && (diff == 2 || diff == 6 || diff == 11 || diff == 12))
		{
			return true;
		}

		return false;
	}

	public class UserOptions : ICloneable
	{
		private readonly Horoscope _h;
		public           Division  Dtype = new(DivisionType.Rasi);
		protected        Body MLordAqu;
		protected        Body MLordSco;
		private          ArrayList _stdDivPos;

		public UserOptions(Horoscope h)
		{
			_h           = h;
			_stdDivPos = _h.CalculateDivisionPositions(Dtype);
			var fs = new FindStronger(_h, Dtype, FindStronger.RulesStrongerCoLord(_h));
			MLordSco          = fs.StrongerGraha(Body.Mars, Body.Ketu, true);
			MLordAqu          = fs.StrongerGraha(Body.Saturn, Body.Rahu, true);
			ExcludeNodes      = true;
			ExcludeDasaLord   = true;
			Exclude310      = false;
			Exclude261112 = false;
			CalculateRasiStrengths();
			CalculateGrahaStrengths();
		}

		[Category("1: Colord")]
		[PropertyOrder(1)]
		[PGDisplayName("Colord")]
		[Description("Is Ketu or Mars the stronger lord of Scorpio?")]
		public Body LordSco
		{
			get => MLordSco;
			set => MLordSco = value;
		}

		[Category("1: Colord")]
		[PropertyOrder(2)]
		[PGDisplayName("Lord of Aquarius")]
		[Description("Is Rahu or Saturn the stronger lord of Aquarius?")]
		public Body LordAqu
		{
			get => MLordAqu;
			set => MLordAqu = value;
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
		public bool Exclude310
		{
			get;
			set;
		}

		[Category("4: Exclude Antardasas")]
		[PGDisplayName("Exclude grahas in 2nd, 6th, 11th & 12th")]
		public bool Exclude261112
		{
			get;
			set;
		}

		public object Clone()
		{
			var uo = new UserOptions(_h)
			{
				MLordSco = MLordSco,
				MLordAqu = MLordAqu,
				RasiStrengths = new OrderedZodiacHouses[3]
			};
			for (var i = 0; i < 3; i++)
			{
				uo.RasiStrengths[i]  = (OrderedZodiacHouses) RasiStrengths[i].Clone();
				uo.GrahaStrengths[i] = (OrderedGrahas) GrahaStrengths[i].Clone();
			}

			uo.ExcludeDasaLord   = ExcludeDasaLord;
			uo.ExcludeNodes      = ExcludeNodes;
			uo.Exclude261112 = Exclude261112;
			uo.Exclude310      = Exclude310;
			return uo;
		}

		public void Recalculate()
		{
			var fs = new FindStronger(_h, Dtype, FindStronger.RulesStrongerCoLord(_h));
			MLordSco = fs.StrongerGraha(Body.Mars, Body.Ketu, true);
			MLordAqu = fs.StrongerGraha(Body.Saturn, Body.Rahu, true);
			CalculateRasiStrengths();
			CalculateGrahaStrengths();
		}

		public void CompareAndRecalculate(UserOptions newOpts)
		{
			if (newOpts.MLordAqu != MLordAqu || newOpts.MLordSco != MLordSco)
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
					if ((ZodiacHouse) newOpts.RasiStrengths[i].houses[j] != (ZodiacHouse) RasiStrengths[i].houses[j])
					{
						newOpts.CalculateGrahaStrengths();
						return;
					}
				}
			}
		}

		public void CalculateRasiStrengths()
		{
			var fs = new FindStronger(_h, Dtype, FindStronger.RulesNaisargikaDasaRasi(_h));
			RasiStrengths = fs.ResultsZodiacKendras(_h.CalculateDivisionPosition(_h.GetPosition(Body.Lagna), Dtype).ZodiacHouse);
		}

		public void CalculateGrahaStrengths()
		{
			var fsTemp = new StrengthByConjunction(_h, Dtype);
			var fs      = new FindStronger(_h, Dtype, FindStronger.RulesNaisargikaDasaGraha(_h));
			GrahaStrengths = new OrderedGrahas[3];
			for (var i = 0; i < RasiStrengths.Length; i++)
			{
				GrahaStrengths[i] = new OrderedGrahas();
				var oz = RasiStrengths[i];
				foreach (ZodiacHouse zn in oz.houses)
				{
					var temp     = fsTemp.FindGrahasInHouse(zn);
					var tempArr = new Body[temp.Count];
					for (var j = 0; j < temp.Count; j++)
					{
						tempArr[j] = (Body) temp[j];
					}

					var sorted = fs.GetOrderedGrahas(tempArr);
					foreach (var bn in sorted)
					{
						GrahaStrengths[i].grahas.Add(bn);
					}
				}
			}
		}
	}
}