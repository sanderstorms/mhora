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
using Mhora.Util;

namespace Mhora.Dasas.RasiDasa;

public class SuDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	private readonly int[] _order =
	[
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
	];

	public SuDasa(Horoscope h)
	{
		_h       = h;
		_options = new RasiDasaUserOptions(_h, _h.RulesNarayanaDasaRasi());
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

	public List<DasaEntry> Dasa(int cycle)
	{
		var rashis = _h.FindRashis(_options.Division);
		var al     = new List<DasaEntry> ();
		var bpSl   = _h.GetPosition(Body.SreeLagna);
		var zhSeed = bpSl.ToDivisionPosition(_options.Division).ZodiacHouse;
		zhSeed = _options.FindStrongerRasi(_options.SeventhStrengths, zhSeed, zhSeed.Add(7));

		var bIsForward = zhSeed.IsOdd();

		TimeOffset dasaLengthSum = 0.0;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zhDasa;
			if (bIsForward)
			{
				zhDasa = zhSeed.Add(_order[i]);
			}
			else
			{
				zhDasa = zhSeed.AddReverse(_order[i]);
			}

			double dasaLength = NarayanaDasaLength(zhDasa, rashis [zhDasa].Lord);
			var    di          = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		for (var i = 0; i < 12; i++)
		{
			var diFirst    = al[i];
			var dasaLength = 12.0 - diFirst.DasaLength;
			var di         = new DasaEntry(diFirst.ZHouse, dasaLengthSum, dasaLength, 1, diFirst.ZHouse.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		TimeOffset cycleLength  = cycle                                        * ParamAyus();
		var offsetLength = bpSl.Longitude.ToZodiacHouseOffset() / 30.0 * al[0].DasaLength;

		//Mhora.Log.Debug ("Completed {0}, going back {1} of {2} years", bp_sl.longitude.toZodiacHouseOffset() / 30.0,
		//	offset_length, ((DasaEntry)al[0]).DasaLength);

		cycleLength -= offsetLength;

		foreach (DasaEntry di in al)
		{
			di.Start += cycleLength;
		}

		return al;
	}

	public List<DasaEntry> AntarDasa(DasaEntry pdi)
	{
		var al     = new List<DasaEntry> ();
		var zhSeed = pdi.ZHouse;

		var dasaLength     = pdi.DasaLength / 12.0;
		var dasaLengthSum = pdi.Start;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zhDasa;
			zhDasa = zhSeed.AddReverse(_order[i]);

			var di = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, pdi.Level + 1, pdi.DasaName + " " + zhDasa);
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

	private Body GetLord(Rashi rashi)
	{
		return rashi.ZodiacHouse switch
		       {
			       ZodiacHouse.Aqu => _options.ColordAqu,
			       ZodiacHouse.Sco => _options.ColordSco,
			       _               => rashi.Lord
		       };
	}
}