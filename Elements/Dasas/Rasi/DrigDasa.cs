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
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Elements.Dasas.Rasi;

public class DrigDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	public DrigDasa(Horoscope h)
	{
		this._h       = h;
		_options = new RasiDasaUserOptions(this._h, FindStronger.RulesNarayanaDasaRasi(this._h));
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
			var dp      = _h.CalculateDivisionPosition(_h.GetPosition(GetLord(zhDasa)), new Division(Vargas.DivisionType.Rasi));
			dasaLength = NarayanaDasaLength(zhDasa, dp);
			var di = new DasaEntry(zhDasa.Sign, dasaLengthSum, dasaLength, 1, zhDasa.Sign.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}


		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var nd = new NarayanaDasa(_h);
		nd.Options = _options;
		return nd.AntarDasa(pdi);
	}

	public string Description()
	{
		return "Drig Dasa" + " seeded from " + _options.SeedRasi;
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

	private Body.BodyType GetLord(ZodiacHouse zh)
	{
		switch (zh.Sign)
		{
			case ZodiacHouse.Rasi.Aqu: return _options.ColordAqu;
			case ZodiacHouse.Rasi.Sco: return _options.ColordSco;
			default:                   return zh.Sign.SimpleLordOfZodiacHouse();
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
		switch ((int) zh.Sign % 3)
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