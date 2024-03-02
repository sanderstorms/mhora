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

using System.Collections;
using System.Collections.Generic;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements.Calculation;
using Mhora.Elements.Yoga;
using Mhora.Util;

namespace Mhora.Elements.Dasas.RasiDasa;

public class TrikonaDasa : Dasa, IDasa
{
	private readonly Horoscope   _h;
	private readonly UserOptions _options;

	private readonly int[] _order =
	{
		1,
		5,
		9,
		2,
		6,
		10,
		3,
		7,
		11,
		4,
		8,
		12
	};

	public TrikonaDasa(Horoscope h)
	{
		_h       = h;
		_options = new UserOptions(_h, FindStronger.RulesNavamsaDasaRasi(_h));
	}

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public double ParamAyus()
	{
		return 144;
	}

	public ArrayList Dasa(int cycle)
	{
		var rashis = _h.FindRashis(_options.Division);
		var al     = new ArrayList(12);
		var zhSeed = _options.GetSeed();
		if (_options.TrikonaStrengths.houses.Count >= 1)
		{
			zhSeed = (ZodiacHouse) _options.TrikonaStrengths.houses[0];
		}

		zhSeed = _options.FindStrongerRasi(_options.SeventhStrengths, zhSeed, zhSeed.Add(7));

		var bIsZodiacal = zhSeed.IsOdd();

		TimeOffset dasaLengthSum = TimeOffset.Zero;
		for (var i = 0; i < 12; i++)
		{
			ZodiacHouse zhDasa;
			if (bIsZodiacal)
			{
				zhDasa = zhSeed.Add(_order[i]);
			}
			else
			{
				zhDasa = zhSeed.AddReverse(_order[i]);
			}

			double dasaLength = NarayanaDasaLength(zhDasa, GetLordsPosition(rashis [zhDasa]));


			var di = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		for (var i = 0; i < 12; i++)
		{
			var df          = (DasaEntry) al[i];
			var dasaLength = 12.0 - df.DasaLength;
			var di          = new DasaEntry(df.ZHouse, dasaLengthSum, dasaLength, 1, df.DasaName);
			al.Add(di);
			dasaLengthSum += dasaLength;
		}


		var cycleLength = cycle * ParamAyus();
		foreach (DasaEntry di in al)
		{
			di.Start += cycleLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var nd = new NarayanaDasa(_h)
		{
			Options = _options
		};
		return nd.AntarDasa(pdi);
	}

	public string Description()
	{
		return "Trikona Dasa seeded from " + _options.SeedZodiacHouse;
	}

	public object GetOptions()
	{
		return _options.Clone();
	}

	public object SetOptions(object a)
	{
		var uo = (UserOptions) a;
		_options.CopyFrom(uo);
		RecalculateEvent();
		return _options.Clone();
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (UserOptions) _options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
	}

	public Graha GetLordsPosition(Rashi rashi)
	{
		Graha graha;
		if (rashi == ZodiacHouse.Sco)
		{
			graha = rashi.GrahaList [_options.ColordSco];
		}
		else if (rashi == ZodiacHouse.Aqu)
		{
			graha = rashi.GrahaList[_options.ColordAqu];
		}
		else
		{
			graha = rashi.Lord;
		}

		return graha;
	}

	private class UserOptions : RasiDasaUserOptions
	{
		protected OrderedZodiacHouses MTrikonaStrengths;

		public UserOptions(Horoscope h, List<RashiStrength> rules) : base(h, rules)
		{
			CalculateTrikonaStrengths();
		}

		public OrderedZodiacHouses TrikonaStrengths
		{
			get => MTrikonaStrengths;
			set => MTrikonaStrengths = value;
		}

		private void CalculateTrikonaStrengths()
		{
			var grahas = H.FindGrahas(Division);
			var zh     = GetSeed();
			var zhT = new ZodiacHouse[3]
			{
				zh.Add(1),
				zh.Add(5),
				zh.Add(9)
			};
			MTrikonaStrengths = grahas.GetOrderedHouses(zhT, MRules);
		}

		public override object Clone()
		{
			var uo = new UserOptions(H, MRules);
			CopyFromNoClone(this);
			uo.MTrikonaStrengths = (OrderedZodiacHouses) MTrikonaStrengths.Clone();
			return uo;
		}

		public override object CopyFrom(object userOptions)
		{
			var uo = (UserOptions) userOptions;
			if (Division != uo.Division || ColordAqu != uo.ColordAqu || ColordSco != uo.ColordSco)
			{
				CalculateTrikonaStrengths();
				CalculateSeed();
				CalculateExceptions();
				CalculateSeventhStrengths();
				CalculateCoLords();
			}

			base.CopyFromNoClone(userOptions);
			return Clone();
		}

		public new void Recalculate()
		{
			CalculateTrikonaStrengths();
			CalculateSeed();
			CalculateExceptions();
			CalculateSeventhStrengths();
			CalculateCoLords();
		}
	}
}