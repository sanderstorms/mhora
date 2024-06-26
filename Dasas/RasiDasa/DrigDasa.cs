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
using Mhora.Calculation;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Dasas.RasiDasa;

public class DrigDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	public DrigDasa(Horoscope h)
	{
		_h       = h;
		_options = new RasiDasaUserOptions(_h, _h.RulesNarayanaDasaRasi());
	}

	public double ParamAyus() => 144;

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		var rashis  = _h.FindRashis(DivisionType.Rasi);
		var alOrder = new List<ZodiacHouse>();
		var zhSeed  = _options.GetSeed().Add(9);

		for (var i = 1; i <= 4; i++)
		{
			DasaHelper(zhSeed.Add(i), alOrder);
		}

		var al = new List<DasaEntry> ();

		var    dasaLengthSum = 0.0;
		double dasaLength;
		for (var i = 0; i < 12; i++)
		{
			var zhDasa = alOrder[i];
			dasaLength = NarayanaDasaLength(zhDasa, rashis[zhDasa].Lord);
			var di = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}


		return al;
	}

	public List<DasaEntry> AntarDasa(DasaEntry pdi)
	{
		var nd = new NarayanaDasa(_h)
		{
			Options = _options
		};
		return nd.AntarDasa(pdi);
	}

	public string Description() => "Drig Dasa" + " seeded from " + _options.SeedZodiacHouse;

	public object GetOptions() => _options.Clone();

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
		return zh switch
		       {
			       ZodiacHouse.Aqu => _options.ColordAqu,
			       ZodiacHouse.Sco => _options.ColordSco,
			       _               => zh.SimpleLordOfZodiacHouse()
		       };
	}

	public void DasaHelper(ZodiacHouse zh, List<ZodiacHouse> al)
	{
		int[] orderMoveable =
		[
			5,
			8,
			11
		];
		int[] orderFixed =
		[
			3,
			6,
			9
		];
		int[] orderDual =
		[
			4,
			7,
			10
		];
		var backward = false;
		if (!zh.IsOddFooted())
		{
			backward = true;
		}

		int[] order = ((int) zh % 3) switch
		              {
			              1 => orderMoveable,
			              2 => orderFixed,
			              _ => orderDual
		              };

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