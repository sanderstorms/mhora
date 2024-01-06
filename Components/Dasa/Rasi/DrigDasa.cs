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

public class DrigDasa : Dasa, IDasa
{
	private readonly Horoscope           h;
	private readonly RasiDasaUserOptions options;

	public DrigDasa(Horoscope _h)
	{
		h       = _h;
		options = new RasiDasaUserOptions(h, FindStronger.RulesNarayanaDasaRasi(h));
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
		var al_order = new ArrayList(12);
		var zh_seed  = options.getSeed().add(9);

		for (var i = 1; i <= 4; i++)
		{
			DasaHelper(zh_seed.add(i), al_order);
		}

		var al = new ArrayList(12);

		var    dasa_length_sum = 0.0;
		double dasa_length;
		for (var i = 0; i < 12; i++)
		{
			var zh_dasa = (ZodiacHouse) al_order[i];
			var dp      = h.CalculateDivisionPosition(h.getPosition(GetLord(zh_dasa)), new Division(Basics.DivisionType.Rasi));
			dasa_length = NarayanaDasaLength(zh_dasa, dp);
			var di = new DasaEntry(zh_dasa.value, dasa_length_sum, dasa_length, 1, zh_dasa.value.ToString());
			al.Add(di);
			dasa_length_sum += dasa_length;
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
		return "Drig Dasa" + " seeded from " + options.SeedRasi;
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

	public void DasaHelper(ZodiacHouse zh, ArrayList al)
	{
		int[] order_moveable =
		{
			5,
			8,
			11
		};
		int[] order_fixed =
		{
			3,
			6,
			9
		};
		int[] order_dual =
		{
			4,
			7,
			10
		};
		var backward = false;
		if (!zh.isOddFooted())
		{
			backward = true;
		}

		int[] order;
		switch ((int) zh.value % 3)
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

		al.Add(zh.add(1));
		if (!backward)
		{
			for (var i = 0; i < 3; i++)
			{
				al.Add(zh.add(order[i]));
			}
		}
		else
		{
			for (var i = 2; i >= 0; i--)
			{
				al.Add(zh.add(order[i]));
			}
		}
	}
}