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
using Mhora.Util;

namespace Mhora.Elements.Dasas.RasiDasa;

public class LagnaKendradiRasiDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly Division            _mDtype = new(DivisionType.Rasi);
	private readonly RasiDasaUserOptions _options;

	public LagnaKendradiRasiDasa(Horoscope h)
	{
		_h       = h;
		_options = new RasiDasaUserOptions(_h, _h.RulesNarayanaDasaRasi());
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
		var rashis = _h.FindRashis(_mDtype);
		var al     = new ArrayList(24);
		int[] order =
		{
			1,
			4,
			7,
			10,
			2,
			5,
			8,
			11,
			3,
			6,
			9,
			12
		};
		TimeOffset dasaLengthSum = TimeOffset.Zero;

		var zhStart = _options.GetSeed();
		zhStart = _options.FindStrongerRasi(_options.SeventhStrengths, zhStart, zhStart.Add(7));

		var bIsZodiacal = IsZodiacal();
		for (var i = 0; i < 12; i++)
		{
			var zh = zhStart.Add(1);
			if (bIsZodiacal)
			{
				zh = zh.Add(order[i]);
			}
			else
			{
				zh = zh.AddReverse(order[i]);
			}

			double dasaLength = NarayanaDasaLength(zh, rashis [zh].Lord);
			var    de          = new DasaEntry(zh, dasaLengthSum, dasaLength, 1, zh.ToString());
			al.Add(de);
			dasaLengthSum += dasaLength;
		}

		for (var i = 0; i < 12; i++)
		{
			var deFirst    = (DasaEntry) al[i];
			var dasaLength = 12.0 - deFirst.DasaLength;
			var de          = new DasaEntry(deFirst.ZHouse, dasaLengthSum, dasaLength, 1, deFirst.DasaName);
			dasaLengthSum += dasaLength;
			al.Add(de);
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
		return "Lagna Kendradi Rasi Dasa seeded from" + " seeded from " + _options.SeedZodiacHouse;
	}

	public object GetOptions()
	{
		return _options.Clone();
	}

	public object SetOptions(object a)
	{
		_options.CopyFrom(a);
		RecalculateEvent();
		return _options.Clone();
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) _options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
	}

	private bool IsZodiacal()
	{
		var zhStart = _options.GetSeed();
		zhStart = _options.FindStrongerRasi(_options.SeventhStrengths, zhStart, zhStart.Add(7));

		var forward = zhStart.IsOdd();
		if (_options.SaturnExceptionApplies(zhStart))
		{
			return forward;
		}

		if (_options.KetuExceptionApplies(zhStart))
		{
			forward = !forward;
		}

		return forward;
	}
}