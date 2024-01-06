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

public class ShatTrimshaSamaDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope h;

	public ShatTrimshaSamaDasa(Horoscope _h)
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
		return "ShatTrimsha Sama Dasa";
	}

	public double paramAyus()
	{
		return 36.0;
	}

	public int numberOfDasaItems()
	{
		return 8;
	}

	public DasaEntry nextDasaLord(DasaEntry di)
	{
		return new DasaEntry(nextDasaLordHelper(di.graha), 0, 0, di.level, string.Empty);
	}

	public double lengthOfDasa(Tables.Body.Name plt)
	{
		switch (plt)
		{
			case Tables.Body.Name.Moon:    return 1;
			case Tables.Body.Name.Sun:     return 2;
			case Tables.Body.Name.Jupiter: return 3;
			case Tables.Body.Name.Mars:    return 4;
			case Tables.Body.Name.Mercury: return 5;
			case Tables.Body.Name.Saturn:  return 6;
			case Tables.Body.Name.Venus:   return 7;
			case Tables.Body.Name.Rahu:    return 8;
		}

		Trace.Assert(false, "ShatTrimshaSamaDasa::lengthOfDasa");
		return 0;
	}

	public Tables.Body.Name lordOfNakshatra(Nakshatra n)
	{
		var lords = new Tables.Body.Name[8]
		{
			Tables.Body.Name.Moon,
			Tables.Body.Name.Sun,
			Tables.Body.Name.Jupiter,
			Tables.Body.Name.Mars,
			Tables.Body.Name.Mercury,
			Tables.Body.Name.Saturn,
			Tables.Body.Name.Venus,
			Tables.Body.Name.Rahu
		};
		var nak_val  = (int) n.value;
		var shr_val  = (int) Nakshatra.Name.Sravana;
		var diff_val = Basics.normalize_inc((int) Nakshatra.Name.Aswini, (int) Nakshatra.Name.Revati, nak_val - shr_val);
		var diff_off = diff_val % 8;
		return lords[diff_off];
	}

	private Tables.Body.Name nextDasaLordHelper(Tables.Body.Name b)
	{
		switch (b)
		{
			case Tables.Body.Name.Moon:    return Tables.Body.Name.Sun;
			case Tables.Body.Name.Sun:     return Tables.Body.Name.Jupiter;
			case Tables.Body.Name.Jupiter: return Tables.Body.Name.Mars;
			case Tables.Body.Name.Mars:    return Tables.Body.Name.Mercury;
			case Tables.Body.Name.Mercury: return Tables.Body.Name.Saturn;
			case Tables.Body.Name.Saturn:  return Tables.Body.Name.Venus;
			case Tables.Body.Name.Venus:   return Tables.Body.Name.Rahu;
			case Tables.Body.Name.Rahu:    return Tables.Body.Name.Moon;
		}

		Trace.Assert(false, "ShatTrimshaSamaDasa::nextDasaLord");
		return Tables.Body.Name.Lagna;
	}
}