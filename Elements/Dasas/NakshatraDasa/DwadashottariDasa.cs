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
using Mhora.Definitions;

namespace Mhora.Elements.Dasas.NakshatraDasa;

public class DwadashottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public DwadashottariDasa(Horoscope h)
	{
		Common  = this;
		_h = h;
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
		return _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Dwadashottari Dasa";
	}

	public double ParamAyus()
	{
		return 112.0;
	}

	public int NumberOfDasaItems()
	{
		return 8;
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return new DasaEntry(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);
	}

	public double LengthOfDasa(Body plt)
	{
		switch (plt)
		{
			case Body.Sun:     return 7;
			case Body.Jupiter: return 9;
			case Body.Ketu:    return 11;
			case Body.Mercury: return 13;
			case Body.Rahu:    return 15;
			case Body.Mars:    return 17;
			case Body.Saturn:  return 19;
			case Body.Moon:    return 21;
		}

		Trace.Assert(false, "Dwadashottari::LengthOfDasa");
		return 0;
	}

	public Body LordOfNakshatra(Nakshatra n)
	{
		var lords = new Body[8]
		{
			Body.Sun,
			Body.Jupiter,
			Body.Ketu,
			Body.Mercury,
			Body.Rahu,
			Body.Mars,
			Body.Saturn,
			Body.Moon
		};
		var nakVal  = (int) n;
		var revVal  = (int) Nakshatra.Revati;
		var diffVal = (revVal - nakVal).NormalizeInc((int) Nakshatra.Aswini, (int) Nakshatra.Revati);
		var diffOff = diffVal % 8;
		return lords[diffOff];
	}

	private Body NextDasaLordHelper(Body b)
	{
		switch (b)
		{
			case Body.Sun:     return Body.Jupiter;
			case Body.Jupiter: return Body.Ketu;
			case Body.Ketu:    return Body.Mercury;
			case Body.Mercury: return Body.Rahu;
			case Body.Rahu:    return Body.Mars;
			case Body.Mars:    return Body.Saturn;
			case Body.Saturn:  return Body.Moon;
			case Body.Moon:    return Body.Sun;
		}

		Trace.Assert(false, "DwadashottariDasa::NextDasaLord");
		return Body.Lagna;
	}
}