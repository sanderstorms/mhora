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

public class NavamsaDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	public NavamsaDasa(Horoscope h)
	{
		this._h       = h;
		_options = new RasiDasaUserOptions(this._h, FindStronger.RulesNavamsaDasaRasi(this._h));
	}

	public double ParamAyus()
	{
		return 108;
	}

	public void RecalculateOptions()
	{
		_options.Recalculate();
	}

	public ArrayList Dasa(int cycle)
	{
		var al      = new ArrayList(12);
		var zhSeed = _h.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(new Division(Vargas.DivisionType.Rasi)).ZodiacHouse;

		if (!zhSeed.IsOdd())
		{
			zhSeed = zhSeed.AdarsaSign();
		}

		var dasaLengthSum = 0.0;
		var dasaLength     = 9.0;
		for (var i = 1; i <= 12; i++)
		{
			var zhDasa = zhSeed.Add(i);
			var di      = new DasaEntry(zhDasa.Sign, dasaLengthSum, dasaLength, 1, zhDasa.Sign.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		var cycleLength = cycle * ParamAyus();
		foreach (DasaEntry di in al)
		{
			di.StartUt += cycleLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var al = new ArrayList(12);

		var zhFirst    = new ZodiacHouse(pdi.ZHouse);
		var zhStronger = zhFirst.Add(1);
		if (!zhStronger.IsOdd())
		{
			zhStronger = zhStronger.AdarsaSign();
		}

		var dasaStart = pdi.StartUt;

		for (var i = 1; i <= 12; i++)
		{
			var zhDasa = zhStronger.Add(i);
			var di      = new DasaEntry(zhDasa.Sign, dasaStart, pdi.DasaLength / 12.0, pdi.Level + 1, pdi.DasaName + " " + zhDasa.Sign);
			al.Add(di);
			dasaStart += pdi.DasaLength / 12.0;
		}

		return al;
	}

	public string Description()
	{
		return "Navamsa Dasa";
	}

	public object GetOptions()
	{
		return _options.Clone();
	}

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