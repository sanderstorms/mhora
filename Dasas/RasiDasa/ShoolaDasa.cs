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
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Dasas.RasiDasa;

public class ShoolaDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	public ShoolaDasa(Horoscope h)
	{
		_h       = h;
		_options = new RasiDasaUserOptions(_h, _h.RulesNarayanaDasaRasi());
	}

	public double ParamAyus() => 108;

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		var al     = new List<DasaEntry> ();
		var zhSeed = _options.GetSeed();
		zhSeed = _options.FindStrongerRasi(_options.SeventhStrengths, zhSeed, zhSeed.Add(7));

		var dasaLengthSum = 0.0;
		var dasaLength     = 9.0;
		for (var i = 1; i <= 12; i++)
		{
			var zhDasa = zhSeed.Add(i);
			var di      = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
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

	public List<DasaEntry> AntarDasa(DasaEntry pdi)
	{
		var al = new List<DasaEntry> ();

		var zhFirst    = pdi.ZHouse;
		var zhStronger = zhFirst.Add(1);
		zhStronger = _options.FindStrongerRasi(_options.SeventhStrengths, zhFirst, zhFirst.Add(7));

		var dasaStart = pdi.Start;

		for (var i = 1; i <= 12; i++)
		{
			var zhDasa = zhStronger.Add(i);
			var di      = new DasaEntry(zhDasa, dasaStart, pdi.DasaLength / 12.0, pdi.Level + 1, pdi.DasaName + " " + zhDasa);
			al.Add(di);
			dasaStart += pdi.DasaLength / 12.0;
		}

		return al;
	}

	public string Description() => "Shoola Dasa" + " seeded from " + _options.SeedZodiacHouse;

	public object GetOptions() => _options.Clone();

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
}