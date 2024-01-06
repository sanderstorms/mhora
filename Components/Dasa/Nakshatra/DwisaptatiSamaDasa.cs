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
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Components.Dasa.Nakshatra;

public class DwisaptatiSamaDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope h;

	public DwisaptatiSamaDasa(Horoscope _h)
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
		return _Dasa(h.getPosition(Elements.Body.Name.Moon).longitude, 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Dwisaptati Sama Dasa";
	}

	public double paramAyus()
	{
		return 72.0;
	}

	public int numberOfDasaItems()
	{
		return 8;
	}

	public DasaEntry nextDasaLord(DasaEntry di)
	{
		return new DasaEntry(nextDasaLordHelper(di.graha), 0, 0, di.level, string.Empty);
	}

	public double lengthOfDasa(Elements.Body.Name plt)
	{
		switch (plt)
		{
			case Elements.Body.Name.Sun:     return 9;
			case Elements.Body.Name.Moon:    return 9;
			case Elements.Body.Name.Mars:    return 9;
			case Elements.Body.Name.Mercury: return 9;
			case Elements.Body.Name.Jupiter: return 9;
			case Elements.Body.Name.Venus:   return 9;
			case Elements.Body.Name.Saturn:  return 9;
			case Elements.Body.Name.Rahu:    return 9;
		}

		Trace.Assert(false, "DwisaptatiSamaDasa::lengthOfDasa");
		return 0;
	}

	public Elements.Body.Name lordOfNakshatra(Elements.Nakshatra n)
	{
		var lords = new Elements.Body.Name[8]
		{
			Elements.Body.Name.Sun,
			Elements.Body.Name.Moon,
			Elements.Body.Name.Mars,
			Elements.Body.Name.Mercury,
			Elements.Body.Name.Jupiter,
			Elements.Body.Name.Venus,
			Elements.Body.Name.Saturn,
			Elements.Body.Name.Rahu
		};
		var nak_val  = (int) n.value;
		var moo_val  = (int) Elements.Nakshatra.Name.Moola;
		var diff_val = Basics.normalize_inc((int) Elements.Nakshatra.Name.Aswini, (int) Elements.Nakshatra.Name.Revati, nak_val - moo_val);
		var diff_off = diff_val % 8;
		return lords[diff_off];
	}

	private Elements.Body.Name nextDasaLordHelper(Elements.Body.Name b)
	{
		switch (b)
		{
			case Elements.Body.Name.Sun:     return Elements.Body.Name.Moon;
			case Elements.Body.Name.Moon:    return Elements.Body.Name.Mars;
			case Elements.Body.Name.Mars:    return Elements.Body.Name.Mercury;
			case Elements.Body.Name.Mercury: return Elements.Body.Name.Jupiter;
			case Elements.Body.Name.Jupiter: return Elements.Body.Name.Venus;
			case Elements.Body.Name.Venus:   return Elements.Body.Name.Saturn;
			case Elements.Body.Name.Saturn:  return Elements.Body.Name.Rahu;
			case Elements.Body.Name.Rahu:    return Elements.Body.Name.Sun;
		}

		Trace.Assert(false, "DwisaptatiSamaDasa::nextDasaLord");
		return Elements.Body.Name.Lagna;
	}
}