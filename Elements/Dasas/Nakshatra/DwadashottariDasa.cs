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
		return _Dasa(h.getPosition(Body.BodyType.Moon).longitude, 1, cycle);
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

	public double lengthOfDasa(Body.BodyType plt)
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

		Trace.Assert(false, "Dwadashottari::lengthOfDasa");
		return 0;
	}

	public Body.BodyType lordOfNakshatra(Nakshatras.Nakshatra n)
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
		var nak_val  = (int) n;
		var rev_val  = (int) Nakshatras.Nakshatra.Revati;
		var diff_val = Basics.normalize_inc((int) Nakshatras.Nakshatra.Aswini, (int) Nakshatras.Nakshatra.Revati, rev_val - nak_val);
		var diff_off = diff_val % 8;
		return lords[diff_off];
	}

	private Body.BodyType nextDasaLordHelper(Body.BodyType b)
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

		Trace.Assert(false, "DwadashottariDasa::nextDasaLord");
		return Body.BodyType.Lagna;
	}
}