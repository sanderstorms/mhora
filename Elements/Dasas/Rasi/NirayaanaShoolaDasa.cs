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
using Mhora.Components.Dasa;
using Mhora.Database.Settings;
using Mhora.Elements.Calculation;

namespace Mhora.Elements.Dasas.Rasi;

public class NirayaanaShoolaDasa : Dasa, IDasa
{
	private readonly Horoscope           h;
	private readonly RasiDasaUserOptions options;

	public NirayaanaShoolaDasa(Horoscope _h)
	{
		h       = _h;
		options = new RasiDasaUserOptions(h, FindStronger.RulesNarayanaDasaRasi(h));
	}

	public double paramAyus()
	{
		return 96;
	}

	public void recalculateOptions()
	{
		options.recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var al      = new ArrayList();
		var zh_seed = options.getSeed().Add(2);
		zh_seed.Sign = options.findStrongerRasi(options.SeventhStrengths, zh_seed.Sign, zh_seed.Add(7).Sign);

		var bIsForward = zh_seed.IsOdd();

		var dasa_length_sum = 0.0;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zh_dasa = null;
			if (bIsForward)
			{
				zh_dasa = zh_seed.Add(i);
			}
			else
			{
				zh_dasa = zh_seed.AddReverse(i);
			}

			var dasa_length = getDasaLength(zh_dasa);
			var di          = new DasaEntry(zh_dasa.Sign, dasa_length_sum, dasa_length, 1, zh_dasa.Sign.ToString());
			al.Add(di);
			dasa_length_sum += dasa_length;
		}

		var cycle_length = cycle * paramAyus();
		foreach (DasaEntry di in al)
		{
			di.startUT += cycle_length;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var nd = new NarayanaDasa(h);
		nd.options = options;
		return nd.AntarDasa(pdi);
	}

	public string Description()
	{
		return "Niryaana Shoola Dasa" + " seeded from " + options.SeedRasi;
	}

	public object GetOptions()
	{
		return options.Clone();
	}

	public object SetOptions(object a)
	{
		var uo = (RasiDasaUserOptions) a;
		options.CopyFrom(uo);
		RecalculateEvent();
		return options.Clone();
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
	}

	public double getDasaLength(ZodiacHouse zh)
	{
		switch ((int) zh.Sign % 3)
		{
			case 1:  return 7.0;
			case 2:  return 8.0;
			case 0:  return 9.0;
			default: throw new Exception();
		}
	}
}