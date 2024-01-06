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

public class NarayanaDasa : Dasa, IDasa
{
	private readonly Horoscope           h;
	public           bool                bSama;
	public           RasiDasaUserOptions options;

	public NarayanaDasa(Horoscope _h)
	{
		h       = _h;
		bSama   = false;
		options = new RasiDasaUserOptions(h, FindStronger.RulesNarayanaDasaRasi(h));
	}

	public void recalculateOptions()
	{
		options.recalculate();
	}

	public double paramAyus()
	{
		return 144;
	}

	public ArrayList Dasa(int cycle)
	{
		int[] order_moveable =
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12
		};
		int[] order_fixed =
		{
			1,
			6,
			11,
			4,
			9,
			2,
			7,
			12,
			5,
			10,
			3,
			8
		};
		int[] order_dual =
		{
			1,
			5,
			9,
			10,
			2,
			6,
			7,
			11,
			3,
			4,
			8,
			12
		};

		var al       = new ArrayList(24);
		var backward = true;

		int[] order;
		switch ((int) options.SeedRasi % 3)
		{
			case 1:
				order = order_moveable;
				break;
			case 2:
				order = order_fixed;
				break;
			default:
				order = order_dual;
				break;
		}

		var zh_seed = options.getSeed();
		zh_seed.value = options.findStrongerRasi(options.SeventhStrengths, zh_seed.value, zh_seed.add(7).value);

		if (zh_seed.add(9).isOddFooted())
		{
			backward = false;
		}

		if (options.saturnExceptionApplies(zh_seed.value))
		{
			order    = order_moveable;
			backward = false;
		}
		else if (options.ketuExceptionApplies(zh_seed.value))
		{
			backward = !backward;
		}

		var dasa_length_sum = 0.0;
		for (var i = 0; i < 12; i++)
		{
			ZodiacHouse zh_dasa;
			if (backward)
			{
				zh_dasa = zh_seed.addReverse(order[i]);
			}
			else
			{
				zh_dasa = zh_seed.add(order[i]);
			}

			var dasa_lord = GetLord(zh_dasa);
			//gs.strongerForNarayanaDasa(zh_dasa);
			var    dlord_dpos  = h.CalculateDivisionPosition(h.getPosition(dasa_lord), options.Division);
			double dasa_length = DasaLength(zh_dasa, dlord_dpos);

			var di = new DasaEntry(zh_dasa.value, dasa_length_sum, dasa_length, 1, zh_dasa.value.ToString());
			al.Add(di);
			dasa_length_sum += dasa_length;
		}

		if (bSama == false)
		{
			for (var i = 0; i < 12; i++)
			{
				var di = (DasaEntry) al[i];
				var dn = new DasaEntry(di.zodiacHouse, dasa_length_sum, 12.0 - di.dasaLength, 1, di.zodiacHouse.ToString());
				dasa_length_sum += dn.dasaLength;
				al.Add(dn);
			}
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
		zh_stronger.value = options.findStrongerRasi(options.SeventhStrengths, zh_stronger.value, zh_stronger.add(7).value);

		var b        = GetLord(zh_stronger);
		var dp       = h.CalculateDivisionPosition(h.getPosition(b), options.Division);
		var first    = dp.zodiac_house;
		var backward = false;
		if ((int) first.value % 2 == 0)
		{
			backward = true;
		}

		var dasa_start = pdi.startUT;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zh_dasa;
			if (!backward)
			{
				zh_dasa = first.add(i);
			}
			else
			{
				zh_dasa = first.addReverse(i);
			}

			var di = new DasaEntry(zh_dasa.value, dasa_start, pdi.dasaLength / 12.0, pdi.level + 1, pdi.shortDesc + " " + zh_dasa.value);
			al.Add(di);
			dasa_start += pdi.dasaLength / 12.0;
		}

		return al;
	}

	public string Description()
	{
		return "Narayana Dasa for " + options.Division + " seeded from " + options.SeedRasi;
	}

	public object GetOptions()
	{
		return options.Clone();
	}

	public object SetOptions(object a)
	{
		options.CopyFrom(a);
		RecalculateEvent();
		return options.Clone();
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
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

	public int DasaLength(ZodiacHouse zh, DivisionPosition dp)
	{
		if (bSama)
		{
			return 12;
		}

		return NarayanaDasaLength(zh, dp);
	}
}