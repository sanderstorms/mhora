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
using Mhora.Elements.Yoga;
using Mhora.Util;

namespace Mhora.Elements.Dasas.RasiDasa;

public class CharaDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	private readonly RasiDasaUserOptions _options;

	public CharaDasa(Horoscope h)
	{
		_h       = h;
		_options = new RasiDasaUserOptions(_h, FindStronger.RulesNavamsaDasaRasi(_h));
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
		var rashis = _h.FindRashis(_options.Division);
		var al     = new ArrayList(12);
		var zhSeed = _options.GetSeed();
		zhSeed = _options.FindStrongerRasi(_options.SeventhStrengths, zhSeed, zhSeed.Add(7));

		var bIsZodiacal = zhSeed.Add(9).IsOddFooted();

		TimeOffset dasaLengthSum = TimeOffset.Zero;
		;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zhDasa;
			if (bIsZodiacal)
			{
				zhDasa = zhSeed.Add(i);
			}
			else
			{
				zhDasa = zhSeed.AddReverse(i);
			}

			double dasaLength = NarayanaDasaLength(zhDasa, GetLordsPosition(rashis [zhDasa]));


			var di = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		for (var i = 0; i < 12; i++)
		{
			var df          = (DasaEntry) al[i];
			var dasaLength = 12.0 - df.DasaLength;
			var di          = new DasaEntry(df.ZHouse, dasaLengthSum, dasaLength, 1, df.DasaName);
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
		return "Chara Dasa seeded from " + _options.SeedZodiacHouse;
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

	public Graha GetLordsPosition(Rashi rashi)
	{
		Graha lord;
		if (rashi == ZodiacHouse.Sco)
		{
			lord = rashi.GrahaList [_options.ColordSco];
		}
		else if (rashi == ZodiacHouse.Aqu)
		{
			lord = rashi.GrahaList[_options.ColordAqu];
		}
		else
		{
			lord = rashi.Lord;
		}

		return lord;
	}
}