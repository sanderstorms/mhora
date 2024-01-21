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
		return _Dasa(h.GetPosition(Body.BodyType.Moon).Longitude, 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Yogini Dasa";
	}

	public double ParamAyus()
	{
		return 36.0;
	}

	public int NumberOfDasaItems()
	{
		return 8;
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return new DasaEntry(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);
	}

	public double LengthOfDasa(Body.BodyType plt)
	{
		switch (plt)
		{
			case Body.BodyType.Moon:    return 1;
			case Body.BodyType.Sun:     return 2;
			case Body.BodyType.Jupiter: return 3;
			case Body.BodyType.Mars:    return 4;
			case Body.BodyType.Mercury: return 5;
			case Body.BodyType.Saturn:  return 6;
			case Body.BodyType.Venus:   return 7;
			case Body.BodyType.Rahu:    return 8;
		}

		Trace.Assert(false, "YoginiDasa::LengthOfDasa");
		return 0;
	}

	public Body.BodyType LordOfNakshatra(Nakshatras.Nakshatra n)
	{
		Body.BodyType[] lords =
		{
			Body.BodyType.Moon,
			Body.BodyType.Sun,
			Body.BodyType.Jupiter,
			Body.BodyType.Mars,
			Body.BodyType.Mercury,
			Body.BodyType.Saturn,
			Body.BodyType.Venus,
			Body.BodyType.Rahu
		};

		var index = ((int) n + 3) % 8;
		if (index == 0)
		{
			index = 8;
		}

		index--;
		return lords[index];
	}

	private Body.BodyType NextDasaLordHelper(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Moon:    return Body.BodyType.Sun;
			case Body.BodyType.Sun:     return Body.BodyType.Jupiter;
			case Body.BodyType.Jupiter: return Body.BodyType.Mars;
			case Body.BodyType.Mars:    return Body.BodyType.Mercury;
			case Body.BodyType.Mercury: return Body.BodyType.Saturn;
			case Body.BodyType.Saturn:  return Body.BodyType.Venus;
			case Body.BodyType.Venus:   return Body.BodyType.Rahu;
			case Body.BodyType.Rahu:    return Body.BodyType.Moon;
		}

		Trace.Assert(false, "YoginiDasa::NextDasaLord");
		return Body.BodyType.Sun;
	}
}