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
using Mhora.Components.Dasa;
using Mhora.Database.Settings;
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Elements.Dasas.Rasi;

public class MandookaDasa : Dasa, IDasa
{
	private readonly Horoscope           h;
	private readonly RasiDasaUserOptions options;

	public MandookaDasa(Horoscope _h)
	{
		h       = _h;
		options = new RasiDasaUserOptions(h, FindStronger.RulesNavamsaDasaRasi(h));
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
		int[] sequence =
		{
			1,
			3,
			5,
			7,
			9,
			11,
			2,
			4,
			6,
			8,
			10,
			12
		};
		var al      = new ArrayList(12);
		var zh_seed = options.getSeed();

		if (zh_seed.IsOdd())
		{
			zh_seed = zh_seed.Add(3);
		}
		else
		{
			zh_seed = zh_seed.AddReverse(3);
		}

		var bDirZodiacal = true;
		if (!zh_seed.IsOdd())
		{
			//zh_seed = zh_seed.AdarsaSign();
			bDirZodiacal = false;
		}

		var dasa_length_sum = 0.0;
		for (var i = 0; i < 12; i++)
		{
			ZodiacHouse zh_dasa = null;
			if (bDirZodiacal)
			{
				zh_dasa = zh_seed.Add(sequence[i]);
			}
			else
			{
				zh_dasa = zh_seed.AddReverse(sequence[i]);
			}

			double dasa_length = DasaLength(zh_dasa);
			var    di          = new DasaEntry(zh_dasa.Sign, dasa_length_sum, dasa_length, 1, zh_dasa.Sign.ToString());
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
		var al = new ArrayList(12);

		var zh_first    = new ZodiacHouse(pdi.zodiacHouse);
		var zh_stronger = zh_first.Add(1);
		if (!zh_stronger.IsOdd())
		{
			zh_stronger = zh_stronger.AdarsaSign();
		}

		var dasa_start = pdi.startUT;

		for (var i = 1; i <= 12; i++)
		{
			var zh_dasa = zh_stronger.Add(i);
			var di      = new DasaEntry(zh_dasa.Sign, dasa_start, pdi.dasaLength / 12.0, pdi.level + 1, pdi.shortDesc + " " + zh_dasa.Sign);
			al.Add(di);
			dasa_start += pdi.dasaLength / 12.0;
		}

		return al;
	}

	public string Description()
	{
		return "Mandooka Dasa (seeded from) " + options.Division.NumPartsInDivisionString();
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

	public int DasaLength(ZodiacHouse zh)
	{
		switch ((int) zh.Sign % 3)
		{
			case 1:  return 7;
			case 2:  return 8;
			default: return 9;
		}
	}
}