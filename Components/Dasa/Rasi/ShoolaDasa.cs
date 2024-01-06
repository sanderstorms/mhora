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
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;

namespace Mhora.Components.Dasa.Rasi;

public class ShoolaDasa : Dasa, IDasa
{
	private readonly Horoscope           h;
	private readonly RasiDasaUserOptions options;

	public ShoolaDasa(Horoscope _h)
	{
		h       = _h;
		options = new RasiDasaUserOptions(h, FindStronger.RulesNarayanaDasaRasi(h));
	}

	public double paramAyus()
	{
		return 108;
	}

	public void recalculateOptions()
	{
		options.recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var al      = new ArrayList(12);
		var zh_seed = options.getSeed();
		zh_seed.value = options.findStrongerRasi(options.SeventhStrengths, zh_seed.value, zh_seed.add(7).value);

		var dasa_length_sum = 0.0;
		var dasa_length     = 9.0;
		for (var i = 1; i <= 12; i++)
		{
			var zh_dasa = zh_seed.add(i);
			var di      = new DasaEntry(zh_dasa.value, dasa_length_sum, dasa_length, 1, zh_dasa.value.ToString());
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
		var zh_stronger = zh_first.add(1);
		zh_stronger.value = options.findStrongerRasi(options.SeventhStrengths, zh_first.value, zh_first.add(7).value);

		var dasa_start = pdi.startUT;

		for (var i = 1; i <= 12; i++)
		{
			var zh_dasa = zh_stronger.add(i);
			var di      = new DasaEntry(zh_dasa.value, dasa_start, pdi.dasaLength / 12.0, pdi.level + 1, pdi.shortDesc + " " + zh_dasa.value);
			al.Add(di);
			dasa_start += pdi.dasaLength / 12.0;
		}

		return al;
	}

	public string Description()
	{
		return "Shoola Dasa" + " seeded from " + options.SeedRasi;
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
}