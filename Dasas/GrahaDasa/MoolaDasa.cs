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
using System.Collections.Generic;
using System.ComponentModel;
using Mhora.Calculation;
using Mhora.Components.Property;
using Mhora.Dasas.NakshatraDasa;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Dasas.GrahaDasa;

public class MoolaDasa : Dasa, IDasa
{
	private readonly Horoscope      _h;
	private readonly VimsottariDasa _vd;
	private          UserOptions    _options;

	public MoolaDasa(Horoscope h)
	{
		_h = h;
		_options = new UserOptions(_h);
		_vd      = new VimsottariDasa(_h);
	}

	public double ParamAyus() => 120.0;

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		var cycleStart = ParamAyus() * cycle;
		TimeOffset curr        = 0.0;
		var al          = new List<DasaEntry> ();
		foreach (var b in _options.GrahaStrengths.grahas)
		{
			var dasaLength = LengthOfDasa(b);
			al.Add(new DasaEntry(b, cycleStart + curr, dasaLength, 1, b.ToShortString()));
			curr += dasaLength;
		}

		var numDasas = al.Count;
		for (var i = 0; i < numDasas; i++)
		{
			var de         = al[i];
			var dasaLength = de.DasaLength - _vd.LengthOfDasa(de.Graha);
			al.Add(new DasaEntry(de.Graha, cycleStart + curr, dasaLength, 1, de.Graha.ToShortString()));
			curr += dasaLength;
		}

		return al;
	}

	public List<DasaEntry> AntarDasa(DasaEntry pdi)
	{
		var al   = new List<DasaEntry> ();
		var curr = pdi.Start;

		var bOrder = new List <Body> ();
		var bFound = false;
		foreach (var b in _options.GrahaStrengths.grahas)
		{
			if (b != pdi.Graha && bFound == false)
			{
				continue;
			}

			bFound = true;
			bOrder.Add(b);
		}

		foreach (var b in _options.GrahaStrengths.grahas)
		{
			if (b == pdi.Graha)
			{
				break;
			}

			bOrder.Add(b);
		}


		foreach (var b in bOrder)
		{
			var dasaLength = _vd.LengthOfDasa(b) / _vd.ParamAyus() * pdi.DasaLength;
			al.Add(new DasaEntry(b, curr, dasaLength, pdi.Level + 1, pdi.DasaName + " " + b.ToShortString()));
			curr += dasaLength;
		}

		return al;
	}

	public string Description() => string.Format("Moola Dasa seeded from {0}", _options.SeedBody);

	public object GetOptions() => _options.Clone();

	public object SetOptions(object a)
	{
		var newOpts = (UserOptions) a;
		_options.CompareAndRecalculate(newOpts);
		_options = newOpts;
		RecalculateEvent();
		return _options.Clone();
	}

	public TimeOffset LengthOfDasa(Body plt)
	{
		TimeOffset length = 0;

		// Count to moola trikona - 1.
		// Use Aqu / Sco as MT houses for Rahu / Ketu
		var dpPlt = _h.GetPosition(plt).ToDivisionPosition(_options.Dtype);
		var zhPlt = dpPlt.ZodiacHouse;
		var zhMt  = plt.MooltrikonaSign();
		if (plt == Body.Rahu)
		{
			zhMt = ZodiacHouse.Aqu;
		}

		if (plt == Body.Ketu)
		{
			zhMt = ZodiacHouse.Sco;
		}

		var diff = zhPlt.NumHousesBetween(zhMt);
		length = diff - 1;

		// exaltation / debilitation correction
		if (dpPlt.IsExaltedPhalita())
		{
			length += 1.0;
		}
		else if (dpPlt.IsDebilitatedPhalita())
		{
			length -= 1.0;
		}

		// subtract this length from the vimsottari lengths
		length = _vd.LengthOfDasa(plt) - length;

		// Zero length = full vimsottari length.
		// If negative, make it positive
		if (length == 0)
		{
			length = _vd.LengthOfDasa(plt);
		}
		else if (length < 0)
		{
			length *= -1;
		}

		return length;
	}

	public class UserOptions : ICloneable
	{
		private readonly Horoscope _h;
		public           DivisionType  Dtype = DivisionType.Rasi;
		protected        Body      MSeedBody;
		private          Grahas    _grahas;

		public UserOptions(Horoscope h)
		{
			_h     = h;
			MSeedBody   = Body.Lagna;
			CalculateRasiStrengths();
			CalculateGrahaStrengths();
		}

		[Category("Strengths1 Seed")]
		[PGDisplayName("Seed Body")]
		public Body SeedBody
		{
			get => MSeedBody;
			set => MSeedBody = value;
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
			var uo = new UserOptions(_h)
			{
				GrahaStrengths = (OrderedGrahas) GrahaStrengths.Clone(),
				RasiStrengths = new OrderedZodiacHouses[3],
				MSeedBody = MSeedBody
			};
			for (var i = 0; i < 3; i++)
			{
				uo.RasiStrengths[i] = (OrderedZodiacHouses) RasiStrengths[i].Clone();
			}

			return uo;
		}

		public void Recalculate()
		{
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
					if (newOpts.RasiStrengths[i].houses[j] != RasiStrengths[i].houses[j])
					{
						newOpts.CalculateGrahaStrengths();
						return;
					}
				}
			}
		}

		public void CalculateRasiStrengths()
		{
			var zRet   = new OrderedZodiacHouses[3];
			var grahas = _h.FindGrahas(Dtype);
			var zh     = grahas[SeedBody].Rashi.ZodiacHouse;

			var zhK = new ZodiacHouse[4]
			{
				zh.Add(1),
				zh.Add(4),
				zh.Add(7),
				zh.Add(10)
			};
			var zhP = new ZodiacHouse[4]
			{
				zh.Add(2),
				zh.Add(5),
				zh.Add(8),
				zh.Add(11)
			};
			var zhA = new ZodiacHouse[4]
			{
				zh.Add(3),
				zh.Add(6),
				zh.Add(9),
				zh.Add(12)
			};

			var rules = _h.RulesMoolaDasaRasi();
			zRet[0] = grahas.GetOrderedHouses(zhK, rules);
			zRet[1] = grahas.GetOrderedHouses(zhP, rules);
			zRet[2] = grahas.GetOrderedHouses(zhA, rules);

			grahas = _h.FindGrahas(Dtype);

			var zhSat = grahas [Body.Saturn].Rashi;
			var zhKet = grahas [Body.Ketu].Rashi;

			var bIsForward = zh.IsOdd();
			if (zhSat != zhKet && zhSat == zh)
			{
				bIsForward = true;
			}
			else if (zhSat != zhKet && zhKet == zh)
			{
				bIsForward = false;
			}
			else if (zhSat == zhKet && zhSat == zh)
			{
				var rule = new List<GrahaStrength>()
				{
					GrahaStrength.Longitude
				};
				bIsForward = grahas.Compare(Body.Saturn, Body.Ketu, false, rule, out _) > 0;
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
			var grahas = _h.FindGrahas(Dtype);
			var rules  = _h.RulesNaisargikaDasaGraha();
			GrahaStrengths = new OrderedGrahas();
			foreach (var oz in RasiStrengths)
			{
				foreach (var zn in oz.houses)
				{
					var rashi   = grahas.Rashis[zn];
					var tempArr = new Body[rashi.Grahas.Count];
					for (var i = 0; i < rashi.Grahas.Count; i++)
					{
						tempArr[i] = rashi.Grahas[i];
					}

					var sorted = grahas.GetOrderedGrahas(tempArr, rules);
					foreach (var bn in sorted)
					{
						GrahaStrengths.grahas.Add(bn);
					}
				}
			}
		}
	}
}