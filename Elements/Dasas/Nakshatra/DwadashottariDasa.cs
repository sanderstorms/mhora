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

public class DwadashottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public DwadashottariDasa(Horoscope h)
	{
		Common  = this;
		this._h = h;
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
		return _Dasa(_h.GetPosition(Body.BodyType.Moon).Longitude, 1, cycle);
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

	public double LengthOfDasa(Body.BodyType plt)
	{
		switch (plt)
		{
			case Body.BodyType.Sun:     return 7;
			case Body.BodyType.Jupiter: return 9;
			case Body.BodyType.Ketu:    return 11;
			case Body.BodyType.Mercury: return 13;
			case Body.BodyType.Rahu:    return 15;
			case Body.BodyType.Mars:    return 17;
			case Body.BodyType.Saturn:  return 19;
			case Body.BodyType.Moon:    return 21;
		}

		Trace.Assert(false, "Dwadashottari::LengthOfDasa");
		return 0;
	}

	public Body.BodyType LordOfNakshatra(Nakshatras.Nakshatra n)
	{
		var lords = new Body.BodyType[8]
		{
			Body.BodyType.Sun,
			Body.BodyType.Jupiter,
			Body.BodyType.Ketu,
			Body.BodyType.Mercury,
			Body.BodyType.Rahu,
			Body.BodyType.Mars,
			Body.BodyType.Saturn,
			Body.BodyType.Moon
		};
		var nakVal  = (int) n;
		var revVal  = (int) Nakshatras.Nakshatra.Revati;
		var diffVal = (revVal - nakVal).NormalizeInc((int) Nakshatras.Nakshatra.Aswini, (int) Nakshatras.Nakshatra.Revati);
		var diffOff = diffVal % 8;
		return lords[diffOff];
	}

	private Body.BodyType NextDasaLordHelper(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return Body.BodyType.Jupiter;
			case Body.BodyType.Jupiter: return Body.BodyType.Ketu;
			case Body.BodyType.Ketu:    return Body.BodyType.Mercury;
			case Body.BodyType.Mercury: return Body.BodyType.Rahu;
			case Body.BodyType.Rahu:    return Body.BodyType.Mars;
			case Body.BodyType.Mars:    return Body.BodyType.Saturn;
			case Body.BodyType.Saturn:  return Body.BodyType.Moon;
			case Body.BodyType.Moon:    return Body.BodyType.Sun;
		}

		Trace.Assert(false, "DwadashottariDasa::NextDasaLord");
		return Body.BodyType.Lagna;
	}
}