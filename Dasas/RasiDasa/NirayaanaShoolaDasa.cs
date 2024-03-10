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
using Mhora.Calculation;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Dasas.RasiDasa;

public class NirayaanaShoolaDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	public NirayaanaShoolaDasa(Horoscope h)
	{
		_h       = h;
		_options = new RasiDasaUserOptions(_h, _h.RulesNarayanaDasaRasi());
	}

	public double ParamAyus()
	{
		return 96;
	}

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var al      = new ArrayList();
		var zhSeed = _options.GetSeed().Add(2);
		zhSeed = _options.FindStrongerRasi(_options.SeventhStrengths, zhSeed, zhSeed.Add(7));

		var bIsForward = zhSeed.IsOdd();

		var dasaLengthSum = 0.0;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zhDasa;
			if (bIsForward)
			{
				zhDasa = zhSeed.Add(i);
			}
			else
			{
				zhDasa = zhSeed.AddReverse(i);
			}

			var dasaLength = GetDasaLength(zhDasa);
			var di          = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
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
		return "Niryaana Shoola Dasa" + " seeded from " + _options.SeedZodiacHouse;
	}

	public object GetOptions()
	{
		return _options.Clone();
	}

	public object SetOptions(object a)
	{
		var uo = (RasiDasaUserOptions) a;
		_options.CopyFrom(uo);
		RecalculateEvent();
		return _options.Clone();
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) _options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
	}

	public double GetDasaLength(ZodiacHouse zh)
	{
		switch ((int) zh % 3)
		{
			case 1:  return 7.0;
			case 2:  return 8.0;
			case 0:  return 9.0;
			default: throw new Exception();
		}
	}
}