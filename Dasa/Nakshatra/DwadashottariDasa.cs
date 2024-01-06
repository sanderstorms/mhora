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

public class DwadashottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope h;

	public DwadashottariDasa(Horoscope _h)
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
		return "Dwadashottari Dasa";
	}

	public double paramAyus()
	{
		return 112.0;
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
			case Tables.Body.Name.Sun:     return 7;
			case Tables.Body.Name.Jupiter: return 9;
			case Tables.Body.Name.Ketu:    return 11;
			case Tables.Body.Name.Mercury: return 13;
			case Tables.Body.Name.Rahu:    return 15;
			case Tables.Body.Name.Mars:    return 17;
			case Tables.Body.Name.Saturn:  return 19;
			case Tables.Body.Name.Moon:    return 21;
		}

		Trace.Assert(false, "Dwadashottari::lengthOfDasa");
		return 0;
	}

	public Tables.Body.Name lordOfNakshatra(Nakshatra n)
	{
		var lords = new Tables.Body.Name[8]
		{
			Tables.Body.Name.Sun,
			Tables.Body.Name.Jupiter,
			Tables.Body.Name.Ketu,
			Tables.Body.Name.Mercury,
			Tables.Body.Name.Rahu,
			Tables.Body.Name.Mars,
			Tables.Body.Name.Saturn,
			Tables.Body.Name.Moon
		};
		var nak_val  = (int) n.value;
		var rev_val  = (int) Nakshatra.Name.Revati;
		var diff_val = Basics.normalize_inc((int) Nakshatra.Name.Aswini, (int) Nakshatra.Name.Revati, rev_val - nak_val);
		var diff_off = diff_val % 8;
		return lords[diff_off];
	}

	private Tables.Body.Name nextDasaLordHelper(Tables.Body.Name b)
	{
		switch (b)
		{
			case Tables.Body.Name.Sun:     return Tables.Body.Name.Jupiter;
			case Tables.Body.Name.Jupiter: return Tables.Body.Name.Ketu;
			case Tables.Body.Name.Ketu:    return Tables.Body.Name.Mercury;
			case Tables.Body.Name.Mercury: return Tables.Body.Name.Rahu;
			case Tables.Body.Name.Rahu:    return Tables.Body.Name.Mars;
			case Tables.Body.Name.Mars:    return Tables.Body.Name.Saturn;
			case Tables.Body.Name.Saturn:  return Tables.Body.Name.Moon;
			case Tables.Body.Name.Moon:    return Tables.Body.Name.Sun;
		}

		Trace.Assert(false, "DwadashottariDasa::nextDasaLord");
		return Tables.Body.Name.Lagna;
	}
}