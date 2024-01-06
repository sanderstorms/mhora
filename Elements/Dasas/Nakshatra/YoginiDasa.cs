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

namespace Mhora.Elements.Dasas.Nakshatra;

public class YoginiDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope h;

	public YoginiDasa(Horoscope _h)
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
		return "Yogini Dasa";
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

	public double lengthOfDasa(Body.Name plt)
	{
		switch (plt)
		{
			case Body.Name.Moon:    return 1;
			case Body.Name.Sun:     return 2;
			case Body.Name.Jupiter: return 3;
			case Body.Name.Mars:    return 4;
			case Body.Name.Mercury: return 5;
			case Body.Name.Saturn:  return 6;
			case Body.Name.Venus:   return 7;
			case Body.Name.Rahu:    return 8;
		}

		Trace.Assert(false, "YoginiDasa::lengthOfDasa");
		return 0;
	}

	public Body.Name lordOfNakshatra(Elements.Nakshatra n)
	{
		Body.Name[] lords =
		{
			Body.Name.Moon,
			Body.Name.Sun,
			Body.Name.Jupiter,
			Body.Name.Mars,
			Body.Name.Mercury,
			Body.Name.Saturn,
			Body.Name.Venus,
			Body.Name.Rahu
		};

		var index = ((int) n.value + 3) % 8;
		if (index == 0)
		{
			index = 8;
		}

		index--;
		return lords[index];
	}

	private Body.Name nextDasaLordHelper(Body.Name b)
	{
		switch (b)
		{
			case Body.Name.Moon:    return Body.Name.Sun;
			case Body.Name.Sun:     return Body.Name.Jupiter;
			case Body.Name.Jupiter: return Body.Name.Mars;
			case Body.Name.Mars:    return Body.Name.Mercury;
			case Body.Name.Mercury: return Body.Name.Saturn;
			case Body.Name.Saturn:  return Body.Name.Venus;
			case Body.Name.Venus:   return Body.Name.Rahu;
			case Body.Name.Rahu:    return Body.Name.Moon;
		}

		Trace.Assert(false, "YoginiDasa::nextDasaLord");
		return Body.Name.Sun;
	}
}