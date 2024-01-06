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
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Tables.Nakshatra;

namespace Mhora;

public class PanchottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope h;

	public PanchottariDasa(Horoscope _h)
	{
		common = this;
		h      = _h;
	}

	public override object GetOptions()
	{
		return new object();
	}

	public override object SetOptions(object a)
	{
		return new object();
	}

	public ArrayList Dasa(int cycle)
	{
		return _Dasa(h.getPosition(Tables.Body.Name.Moon).longitude, 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Panchottari Dasa";
	}

	public double paramAyus()
	{
		return 105.0;
	}

	public int numberOfDasaItems()
	{
		return 7;
	}

	public DasaEntry nextDasaLord(DasaEntry di)
	{
		return new DasaEntry(nextDasaLordHelper(di.graha), 0, 0, di.level, string.Empty);
	}

	public double lengthOfDasa(Tables.Body.Name plt)
	{
		switch (plt)
		{
			case Tables.Body.Name.Sun:     return 12;
			case Tables.Body.Name.Mercury: return 13;
			case Tables.Body.Name.Saturn:  return 14;
			case Tables.Body.Name.Mars:    return 15;
			case Tables.Body.Name.Venus:   return 16;
			case Tables.Body.Name.Moon:    return 17;
			case Tables.Body.Name.Jupiter: return 18;
		}

		Trace.Assert(false, "Panchottari::lengthOfDasa");
		return 0;
	}

	public Tables.Body.Name lordOfNakshatra(Nakshatra n)
	{
		var lords = new Tables.Body.Name[7]
		{
			Tables.Body.Name.Sun,
			Tables.Body.Name.Mercury,
			Tables.Body.Name.Saturn,
			Tables.Body.Name.Mars,
			Tables.Body.Name.Venus,
			Tables.Body.Name.Moon,
			Tables.Body.Name.Jupiter
		};
		var nak_val  = (int) n.value;
		var anu_val  = (int) Nakshatra.Name.Anuradha;
		var diff_val = Basics.normalize_inc((int) Nakshatra.Name.Aswini, (int) Nakshatra.Name.Revati, nak_val - anu_val);
		var diff_off = diff_val % 7;
		return lords[diff_off];
	}

	private Tables.Body.Name nextDasaLordHelper(Tables.Body.Name b)
	{
		switch (b)
		{
			case Tables.Body.Name.Sun:     return Tables.Body.Name.Mercury;
			case Tables.Body.Name.Mercury: return Tables.Body.Name.Saturn;
			case Tables.Body.Name.Saturn:  return Tables.Body.Name.Mars;
			case Tables.Body.Name.Mars:    return Tables.Body.Name.Venus;
			case Tables.Body.Name.Venus:   return Tables.Body.Name.Moon;
			case Tables.Body.Name.Moon:    return Tables.Body.Name.Jupiter;
			case Tables.Body.Name.Jupiter: return Tables.Body.Name.Sun;
		}

		Trace.Assert(false, "DwadashottariDasa::nextDasaLord");
		return Tables.Body.Name.Lagna;
	}
}