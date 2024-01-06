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
using Mhora.Components.Dasa;
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Elements.Dasas.Nakshatra;

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
		return _Dasa(h.getPosition(Body.Name.Moon).longitude, 1, cycle);
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

	public double lengthOfDasa(Body.Name plt)
	{
		switch (plt)
		{
			case Body.Name.Sun:     return 7;
			case Body.Name.Jupiter: return 9;
			case Body.Name.Ketu:    return 11;
			case Body.Name.Mercury: return 13;
			case Body.Name.Rahu:    return 15;
			case Body.Name.Mars:    return 17;
			case Body.Name.Saturn:  return 19;
			case Body.Name.Moon:    return 21;
		}

		Trace.Assert(false, "Dwadashottari::lengthOfDasa");
		return 0;
	}

	public Body.Name lordOfNakshatra(Elements.Nakshatra n)
	{
		var lords = new Body.Name[8]
		{
			Body.Name.Sun,
			Body.Name.Jupiter,
			Body.Name.Ketu,
			Body.Name.Mercury,
			Body.Name.Rahu,
			Body.Name.Mars,
			Body.Name.Saturn,
			Body.Name.Moon
		};
		var nak_val  = (int) n.value;
		var rev_val  = (int) Elements.Nakshatra.Name.Revati;
		var diff_val = Basics.normalize_inc((int) Elements.Nakshatra.Name.Aswini, (int) Elements.Nakshatra.Name.Revati, rev_val - nak_val);
		var diff_off = diff_val % 8;
		return lords[diff_off];
	}

	private Body.Name nextDasaLordHelper(Body.Name b)
	{
		switch (b)
		{
			case Body.Name.Sun:     return Body.Name.Jupiter;
			case Body.Name.Jupiter: return Body.Name.Ketu;
			case Body.Name.Ketu:    return Body.Name.Mercury;
			case Body.Name.Mercury: return Body.Name.Rahu;
			case Body.Name.Rahu:    return Body.Name.Mars;
			case Body.Name.Mars:    return Body.Name.Saturn;
			case Body.Name.Saturn:  return Body.Name.Moon;
			case Body.Name.Moon:    return Body.Name.Sun;
		}

		Trace.Assert(false, "DwadashottariDasa::nextDasaLord");
		return Body.Name.Lagna;
	}
}