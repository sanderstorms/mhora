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

public class CharaDasa : Dasa, IDasa
{
	private readonly Horoscope           h;
	private readonly RasiDasaUserOptions options;

	public CharaDasa(Horoscope _h)
	{
		h       = _h;
		options = new RasiDasaUserOptions(h, FindStronger.RulesNavamsaDasaRasi(h));
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
		var al      = new ArrayList(12);
		var zh_seed = options.getSeed();
		zh_seed.Sign = options.findStrongerRasi(options.SeventhStrengths, zh_seed.Sign, zh_seed.Add(7).Sign);

		var bIsZodiacal = zh_seed.Add(9).IsOddFooted();

		var dasa_length_sum = 0.0;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zh_dasa = null;
			if (bIsZodiacal)
			{
				zh_dasa = zh_seed.Add(i);
			}
			else
			{
				zh_dasa = zh_seed.AddReverse(i);
			}

			double dasa_length = NarayanaDasaLength(zh_dasa, getLordsPosition(zh_dasa));


			var di = new DasaEntry(zh_dasa.Sign, dasa_length_sum, dasa_length, 1, zh_dasa.Sign.ToString());
			al.Add(di);
			dasa_length_sum += dasa_length;
		}

		for (var i = 0; i < 12; i++)
		{
			var df          = (DasaEntry) al[i];
			var dasa_length = 12.0 - df.dasaLength;
			var di          = new DasaEntry(df.zodiacHouse, dasa_length_sum, dasa_length, 1, df.shortDesc);
			al.Add(di);
			dasa_length_sum += dasa_length;
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
		var nd = new NarayanaDasa(h);
		nd.options = options;
		return nd.AntarDasa(pdi);
	}

	public string Description()
	{
		return "Chara Dasa seeded from " + options.SeedRasi;
	}

	public object GetOptions()
	{
		return options.Clone();
	}

	public object SetOptions(object a)
	{
		var uo = (RasiDasaUserOptions) a;
		options.CopyFrom(uo);
		RecalculateEvent();
		return options.Clone();
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
	}

	public DivisionPosition getLordsPosition(ZodiacHouse zh)
	{
		Body.BodyType b;
		if (zh.Sign == ZodiacHouse.Rasi.Sco)
		{
			b = options.ColordSco;
		}
		else if (zh.Sign == ZodiacHouse.Rasi.Aqu)
		{
			b = options.ColordAqu;
		}
		else
		{
			b = zh.Sign.SimpleLordOfZodiacHouse();
		}

		return h.getPosition(b).toDivisionPosition(options.Division);
	}
}