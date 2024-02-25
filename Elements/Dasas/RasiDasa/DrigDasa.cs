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
using Mhora.Definitions;
using Mhora.Elements.Calculation;

namespace Mhora.Elements.Dasas.RasiDasa;

public class DrigDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	public DrigDasa(Horoscope h)
	{
		_h       = h;
		_options = new RasiDasaUserOptions(_h, FindStronger.RulesNarayanaDasaRasi(_h));
	}

	public double ParamAyus()
	{
		return 144;
	}

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var rashis  = _h.FindRashis(DivisionType.Rasi);
		var alOrder = new ArrayList(12);
		var zhSeed  = _options.GetSeed().Add(9);

		for (var i = 1; i <= 4; i++)
		{
			DasaHelper(zhSeed.Add(i), alOrder);
		}

		var al = new ArrayList(12);

		var    dasaLengthSum = 0.0;
		double dasaLength;
		for (var i = 0; i < 12; i++)
		{
			var zhDasa = (ZodiacHouse) alOrder[i];
			dasaLength = NarayanaDasaLength(zhDasa, rashis[zhDasa].Lord);
			var di = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
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
		return "Drig Dasa" + " seeded from " + _options.SeedZodiacHouse;
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

	private Body GetLord(ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Aqu: return _options.ColordAqu;
			case ZodiacHouse.Sco: return _options.ColordSco;
			default:                   return zh.SimpleLordOfZodiacHouse();
		}
	}

	public void DasaHelper(ZodiacHouse zh, ArrayList al)
	{
		int[] orderMoveable =
		{
			5,
			8,
			11
		};
		int[] orderFixed =
		{
			3,
			6,
			9
		};
		int[] orderDual =
		{
			4,
			7,
			10
		};
		var backward = false;
		if (!zh.IsOddFooted())
		{
			backward = true;
		}

		int[] order;
		switch ((int) zh % 3)
		{
			case 1:
				order = orderMoveable;
				break;
			case 2:
				order = orderFixed;
				break;
			default:
				order = orderDual;
				break;
		}

		al.Add(zh.Add(1));
		if (!backward)
		{
			for (var i = 0; i < 3; i++)
			{
				al.Add(zh.Add(order[i]));
			}
		}
		else
		{
			for (var i = 2; i >= 0; i--)
			{
				al.Add(zh.Add(order[i]));
			}
		}
	}
}