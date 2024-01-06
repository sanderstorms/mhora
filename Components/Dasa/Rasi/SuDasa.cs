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
using Mhora.Tables;

namespace Mhora.Components.Dasa.Rasi;

public class SuDasa : Dasa, IDasa
{
	private readonly Horoscope           h;
	private readonly RasiDasaUserOptions options;

	private readonly int[] order =
	{
		0,
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

	public SuDasa(Horoscope _h)
	{
		h       = _h;
		options = new RasiDasaUserOptions(h, FindStronger.RulesNarayanaDasaRasi(h));
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
	}

	public double paramAyus()
	{
		return 144;
	}

	public void recalculateOptions()
	{
		options.recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var al      = new ArrayList();
		var bp_sl   = h.getPosition(Body.Name.SreeLagna);
		var zh_seed = bp_sl.toDivisionPosition(options.Division).zodiac_house;
		zh_seed.value = options.findStrongerRasi(options.SeventhStrengths, zh_seed.value, zh_seed.add(7).value);

		var bIsForward = zh_seed.isOdd();

		var dasa_length_sum = 0.0;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zh_dasa = null;
			if (bIsForward)
			{
				zh_dasa = zh_seed.add(order[i]);
			}
			else
			{
				zh_dasa = zh_seed.addReverse(order[i]);
			}

			var    bl          = GetLord(zh_dasa);
			var    dp          = h.getPosition(bl).toDivisionPosition(options.Division);
			double dasa_length = NarayanaDasaLength(zh_dasa, dp);
			var    di          = new DasaEntry(zh_dasa.value, dasa_length_sum, dasa_length, 1, zh_dasa.value.ToString());
			al.Add(di);
			dasa_length_sum += dasa_length;
		}

		for (var i = 0; i < 12; i++)
		{
			var di_first    = (DasaEntry) al[i];
			var dasa_length = 12.0 - di_first.dasaLength;
			var di          = new DasaEntry(di_first.zodiacHouse, dasa_length_sum, dasa_length, 1, di_first.zodiacHouse.ToString());
			al.Add(di);
			dasa_length_sum += dasa_length;
		}

		var cycle_length  = cycle                                        * paramAyus();
		var offset_length = bp_sl.longitude.toZodiacHouseOffset() / 30.0 * ((DasaEntry) al[0]).dasaLength;

		//mhora.Log.Debug ("Completed {0}, going back {1} of {2} years", bp_sl.longitude.toZodiacHouseOffset() / 30.0,
		//	offset_length, ((DasaEntry)al[0]).dasaLength);

		cycle_length -= offset_length;

		foreach (DasaEntry di in al)
		{
			di.startUT += cycle_length;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var al      = new ArrayList();
		var zh_seed = new ZodiacHouse(pdi.zodiacHouse);

		var dasa_length     = pdi.dasaLength / 12.0;
		var dasa_length_sum = pdi.startUT;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zh_dasa = null;
			zh_dasa = zh_seed.addReverse(order[i]);

			var di = new DasaEntry(zh_dasa.value, dasa_length_sum, dasa_length, pdi.level + 1, pdi.shortDesc + " " + zh_dasa.value);
			al.Add(di);
			dasa_length_sum += dasa_length;
		}

		return al;
	}

	public string Description()
	{
		return "Sudasa";
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

	private Body.Name GetLord(ZodiacHouse zh)
	{
		switch (zh.value)
		{
			case ZodiacHouse.Name.Aqu: return options.ColordAqu;
			case ZodiacHouse.Name.Sco: return options.ColordSco;
			default:                   return Basics.SimpleLordOfZodiacHouse(zh.value);
		}
	}
}