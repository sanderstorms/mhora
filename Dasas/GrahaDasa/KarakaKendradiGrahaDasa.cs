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
using Mhora.Util;
using Mhora.Yoga;

namespace Mhora.Dasas.GrahaDasa;

public class KarakaKendradiGrahaDasa : Dasa, IDasa
{
	private readonly Horoscope      _h;
	private readonly VimsottariDasa _vd;
	private          UserOptions    _options;

	public KarakaKendradiGrahaDasa(Horoscope h)
	{
		_h = h;
		_options = new UserOptions(_h);
		_vd      = new VimsottariDasa(_h);
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
		var        grahas     = _h.FindGrahas(DivisionType.Rasi);
		var        cycleStart = ParamAyus() * cycle;
		TimeOffset curr       = 0.0;
		var        al         = new ArrayList(24);
		foreach (Body b in _options.GrahaStrengths.grahas)
		{
			var dasaLength = LengthOfDasa(grahas [b]);
			al.Add(new DasaEntry(b, cycleStart + curr, dasaLength, 1, b.ToShortString()));
			curr += dasaLength;
		}

		var numDasas = al.Count;
		for (var i = 0; i < numDasas; i++)
		{
			var de         = (DasaEntry) al[i];
			var dasaLength = de.DasaLength - _vd.LengthOfDasa(de.Graha);
			if (dasaLength < 0)
			{
				dasaLength *= -1;
			}

			al.Add(new DasaEntry(de.Graha, cycleStart + curr, dasaLength, 1, de.Graha.ToShortString()));
			curr += dasaLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var al   = new ArrayList();
		var curr = pdi.Start;

		var bOrder = new ArrayList();
		var bFound = false;
		foreach (Body b in _options.GrahaStrengths.grahas)
		{
			if (b != pdi.Graha && bFound == false)
			{
				continue;
			}

			bFound = true;
			bOrder.Add(b);
		}

		foreach (Body b in _options.GrahaStrengths.grahas)
		{
			if (b == pdi.Graha)
			{
				break;
			}

			bOrder.Add(b);
		}


		var dasaLength = pdi.DasaLength / 9.0;
		foreach (Body b in bOrder)
		{
			al.Add(new DasaEntry(b, curr, dasaLength, pdi.Level + 1, pdi.DasaName + " " + b.ToShortString()));
			curr += dasaLength;
		}

		return al;
	}

	public string Description()
	{
		return string.Format("Karakas Kendradi Graha Dasa seeded from {0}", _options.SeedBody);
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

	public static double LengthOfDasa(Graha graha)
	{
		double length = 0;

		// Count to moola trikona - 1.
		// Use Aqu / Sco as MT houses for Rahu / Ketu
		//DivisionPosition dp_plt = h.getPosition(plt).toDivisionPosition(new Division(DivisionType.Rasi));
		var zhMt  = graha.Body.MooltrikonaSign();

		if (graha == Body.Rahu)
		{
			zhMt = ZodiacHouse.Aqu;
		}

		if (graha == Body.Ketu)
		{
			zhMt = ZodiacHouse.Sco;
		}

		var diff = graha.Rashi.ZodiacHouse.NumHousesBetween(zhMt);
		length = diff - 1;

		// exaltation / debilitation correction
		if (graha.IsExalted)
		{
			length += 1.0;
		}
		else if (graha.IsDebilitated)
		{
			length -= 1.0;
		}

		if (graha.IsInOwnHouse)
		{
			length = 12.0;
		}

		// subtract this length from the vimsottari lengths
		length = VimsottariDasa.DasaLength(graha) - length;

		// Zero length = full vimsottari length.
		// If negative, make it positive
		if (length == 0)
		{
			length = VimsottariDasa.DasaLength(graha);
		}
		else if (length < 0)
		{
			length *= -1;
		}

		return length;
	}

	public class UserOptions : ICloneable
	{
		private readonly Horoscope               _h;
		protected        Body                    MSeedBody;
		private          List <DivisionPosition> _stdDivPos;

		public UserOptions(Horoscope h)
		{
			_h           = h;
			_stdDivPos = h.PositionList.CalculateDivisionPositions(DivisionType.Rasi);
			Recalculate();
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
		public List <ZodiacHouse []> RasiStrengths
		{
			get;
			set;
		}


		public object Clone()
		{
			var uo = new UserOptions(_h)
			{
				GrahaStrengths = (OrderedGrahas) GrahaStrengths.Clone(),
				RasiStrengths = new List <ZodiacHouse []> (),
				MSeedBody = MSeedBody
			};
			for (var i = 0; i < 3; i++)
			{
				uo.RasiStrengths[i] = (ZodiacHouse []) RasiStrengths[i].Clone();
			}

			return uo;
		}

		public void Recalculate()
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
				if (newOpts.RasiStrengths[i].Length != RasiStrengths[i].Length)
				{
					newOpts.CalculateGrahaStrengths();
					return;
				}

				for (var j = 0; j < newOpts.RasiStrengths[i].Length; j++)
				{
					if (newOpts.RasiStrengths[i][j] != RasiStrengths[i][j])
					{
						newOpts.CalculateGrahaStrengths();
						return;
					}
				}
			}
		}

		public void CalculateSeedBody()
		{
			var alK = new ArrayList();
			for (var i = (int) Body.Sun; i <= (int) Body.Rahu; i++)
			{
				var b   = (Body) i;
				var bp  = _h.GetPosition(b);
				var bkc = new KarakaComparer(bp);
				alK.Add(bkc);
			}

			alK.Sort();

			var bpAk = ((KarakaComparer) alK[0]).GetPosition;
			SeedBody = bpAk.Name;
		}

		public void CalculateRasiStrengths()
		{
			var grahasD1 = _h.FindGrahas(DivisionType.Rasi);
			var zRet   = new List<ZodiacHouse[]>();
			var zh     = _h.GetPosition(SeedBody).ToDivisionPosition(DivisionType.Rasi).ZodiacHouse;

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

			var grahas = _h.FindGrahas(DivisionType.Rasi);
			var rules  = _h.RulesKarakaKendradiGrahaDasaRasi();
			zRet.Add(grahas.GetOrderedRasis(zhK, rules));
			zRet.Add(grahas.GetOrderedRasis(zhP, rules));
			zRet.Add(grahas.GetOrderedRasis(zhA, rules));

			var zhSat = _h.GetPosition(Body.Saturn).ToDivisionPosition(DivisionType.Rasi).ZodiacHouse;
			var zhKet = _h.GetPosition(Body.Ketu).ToDivisionPosition(DivisionType.Rasi).ZodiacHouse;

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
				var rule = new ArrayList
				{
					GrahaStrength.Longitude
				};
				bIsForward = grahasD1.Compare(Body.Saturn, Body.Ketu, false, rule, out _) > 0;
			}


			RasiStrengths    = new List<ZodiacHouse[]>();
			RasiStrengths.Add(zRet[0]);
			if (bIsForward)
			{
				RasiStrengths.Add(zRet[1]);
				RasiStrengths.Add(zRet[2]);
			}
			else
			{
				RasiStrengths.Add(zRet[2]);
				RasiStrengths.Add(zRet[1]);
			}
		}

		public void CalculateGrahaStrengths()
		{
			var grahas = _h.FindGrahas(DivisionType.Rasi);
			var rules  =  _h.RulesKarakaKendradiGrahaDasaGraha();
			GrahaStrengths = new OrderedGrahas();
			foreach (var oz in RasiStrengths)
			{
				foreach (ZodiacHouse zn in oz)
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