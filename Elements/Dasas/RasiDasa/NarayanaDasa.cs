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

public class NarayanaDasa : Dasa, IDasa
{
	private readonly Horoscope           _h;
	public           bool                BSama;
	public           RasiDasaUserOptions Options;

	public NarayanaDasa(Horoscope h)
	{
		_h       = h;
		BSama   = false;
		Options = new RasiDasaUserOptions(_h, FindStronger.RulesNarayanaDasaRasi(_h));
	}

	public void RecalculateOptions()
	{
		Options.Recalculate();
	}

	public double ParamAyus()
	{
		return 144;
	}

	public ArrayList Dasa(int cycle)
	{
		int[] orderMoveable =
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12
		};
		int[] orderFixed =
		{
			1,
			6,
			11,
			4,
			9,
			2,
			7,
			12,
			5,
			10,
			3,
			8
		};
		int[] orderDual =
		{
			1,
			5,
			9,
			10,
			2,
			6,
			7,
			11,
			3,
			4,
			8,
			12
		};

		var al       = new ArrayList(24);
		var backward = true;

		int[] order;
		switch ((int) Options.SeedZodiacHouse % 3)
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

		var zhSeed = Options.GetSeed();
		zhSeed = Options.FindStrongerRasi(Options.SeventhStrengths, zhSeed, zhSeed.Add(7));

		if (zhSeed.Add(9).IsOddFooted())
		{
			backward = false;
		}

		if (Options.SaturnExceptionApplies(zhSeed))
		{
			order    = orderMoveable;
			backward = false;
		}
		else if (Options.KetuExceptionApplies(zhSeed))
		{
			backward = !backward;
		}

		var dasaLengthSum = 0.0;
		for (var i = 0; i < 12; i++)
		{
			ZodiacHouse zhDasa;
			if (backward)
			{
				zhDasa = zhSeed.AddReverse(order[i]);
			}
			else
			{
				zhDasa = zhSeed.Add(order[i]);
			}

			var dasaLord = GetLord(zhDasa);
			//gs.strongerForNarayanaDasa(zh_dasa);
			var    dlordDpos  = _h.CalculateDivisionPosition(_h.GetPosition(dasaLord), Options.Division);
			double dasaLength = DasaLength(zhDasa, dlordDpos);

			var di = new DasaEntry(zhDasa, dasaLengthSum, dasaLength, 1, zhDasa.ToString());
			al.Add(di);
			dasaLengthSum += dasaLength;
		}

		if (BSama == false)
		{
			for (var i = 0; i < 12; i++)
			{
				var di = (DasaEntry) al[i];
				var dn = new DasaEntry(di.ZHouse, dasaLengthSum, 12.0 - di.DasaLength, 1, di.ZHouse.ToString());
				dasaLengthSum += dn.DasaLength;
				al.Add(dn);
			}
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

		var zhFirst    = pdi.ZHouse;
		var zhStronger = zhFirst.Add(1);
		zhStronger = Options.FindStrongerRasi(Options.SeventhStrengths, zhStronger, zhStronger.Add(7));

		var b        = GetLord(zhStronger);
		var dp       = _h.CalculateDivisionPosition(_h.GetPosition(b), Options.Division);
		var first    = dp.ZodiacHouse;
		var backward = false;
		if ((int) first % 2 == 0)
		{
			backward = true;
		}

		var dasaStart = pdi.StartUt;
		for (var i = 1; i <= 12; i++)
		{
			ZodiacHouse zhDasa;
			if (!backward)
			{
				zhDasa = first.Add(i);
			}
			else
			{
				zhDasa = first.AddReverse(i);
			}

			var di = new DasaEntry(zhDasa, dasaStart, pdi.DasaLength / 12.0, pdi.Level + 1, pdi.DasaName + " " + zhDasa);
			al.Add(di);
			dasaStart += pdi.DasaLength / 12.0;
		}

		return al;
	}

	public string Description()
	{
		return "Narayana Dasa for " + Options.Division + " seeded from " + Options.SeedZodiacHouse;
	}

	public object GetOptions()
	{
		return Options.Clone();
	}

	public object SetOptions(object a)
	{
		Options.CopyFrom(a);
		RecalculateEvent();
		return Options.Clone();
	}

	public new void DivisionChanged(Division div)
	{
		var newOpts = (RasiDasaUserOptions) Options.Clone();
		newOpts.Division = (Division) div.Clone();
		SetOptions(newOpts);
	}

	private Body GetLord(ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Aqu: return Options.ColordAqu;
			case ZodiacHouse.Sco: return Options.ColordSco;
			default:                   return zh.SimpleLordOfZodiacHouse();
		}
	}

	public int DasaLength(ZodiacHouse zh, DivisionPosition dp)
	{
		if (BSama)
		{
			return 12;
		}

		return NarayanaDasaLength(zh, dp);
	}
}