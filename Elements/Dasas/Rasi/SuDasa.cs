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

namespace Mhora.Elements.Dasas.Rasi;

public class SuDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	private readonly int[] _order =
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

	public SuDasa(Horoscope h)
	{
		this._h       = h;
		_options = new RasiDasaUserOptions(this._h, FindStronger.RulesNarayanaDasaRasi(this._h));
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) _options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
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
		var al      = new ArrayList();
		var bpSl   = _h.GetPosition(Body.BodyType.SreeLagna);
		var zhSeed = bpSl.ToDivisionPosition(_options.Division).ZodiacHouse;
		zhSeed.Sign = _options.FindStrongerRasi(_options.SeventhStrengths, zhSeed.Sign, zhSeed.Add(7).Sign);

		var bIsForward = zhSeed.IsOdd();

		var dasaLengthSum = 0.0;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zhDasa = null;
			if (bIsForward)
			{
				zhDasa = zhSeed.Add(_order[i]);
			}
			else
			{
				zhDasa = zhSeed.AddReverse(_order[i]);
			}

			var    bl          = GetLord(zhDasa);
			var    dp          = _h.GetPosition(bl).ToDivisionPosition(_options.Division);
			double dasaLength = NarayanaDasaLength(zhDasa, dp);
			var    di          = new DasaEntry(zhDasa.Sign, dasaLengthSum, dasaLength, 1, zhDasa.Sign.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		for (var i = 0; i < 12; i++)
		{
			var diFirst    = (DasaEntry) al[i];
			var dasaLength = 12.0 - diFirst.DasaLength;
			var di          = new DasaEntry(diFirst.ZHouse, dasaLengthSum, dasaLength, 1, diFirst.ZHouse.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		var cycleLength  = cycle                                        * ParamAyus();
		var offsetLength = bpSl.Longitude.ToZodiacHouseOffset() / 30.0 * ((DasaEntry) al[0]).DasaLength;

		//mhora.Log.Debug ("Completed {0}, going back {1} of {2} years", bp_sl.longitude.toZodiacHouseOffset() / 30.0,
		//	offset_length, ((DasaEntry)al[0]).DasaLength);

		cycleLength -= offsetLength;

		foreach (DasaEntry di in al)
		{
			di.StartUt += cycleLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var al      = new ArrayList();
		var zhSeed = new ZodiacHouse(pdi.ZHouse);

		var dasaLength     = pdi.DasaLength / 12.0;
		var dasaLengthSum = pdi.StartUt;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zhDasa = null;
			zhDasa = zhSeed.AddReverse(_order[i]);

			var di = new DasaEntry(zhDasa.Sign, dasaLengthSum, dasaLength, pdi.Level + 1, pdi.DasaName + " " + zhDasa.Sign);
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		return al;
	}

	public string Description()
	{
		return "Sudasa";
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

	private Body.BodyType GetLord(ZodiacHouse zh)
	{
		switch (zh.Sign)
		{
			case ZodiacHouse.Rasi.Aqu: return _options.ColordAqu;
			case ZodiacHouse.Rasi.Sco: return _options.ColordSco;
			default:                   return zh.Sign.SimpleLordOfZodiacHouse();
		}
	}
}