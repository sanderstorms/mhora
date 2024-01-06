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
using Mhora.Elements;
using Mhora.Elements.Calculation;

namespace Mhora.Components.Dasa.Nakshatra;

public class AshtottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope h;

	public AshtottariDasa(Horoscope _h)
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
		return "Ashtottari Dasa";
	}

	public double paramAyus()
	{
		return 108.0;
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
			case Body.Name.Sun:     return 6;
			case Body.Name.Moon:    return 15;
			case Body.Name.Mars:    return 8;
			case Body.Name.Mercury: return 17;
			case Body.Name.Saturn:  return 10;
			case Body.Name.Jupiter: return 19;
			case Body.Name.Rahu:    return 12;
			case Body.Name.Venus:   return 21;
		}

		Trace.Assert(false, "Ashtottari::lengthOfDasa");
		return 0;
	}

	public Body.Name lordOfNakshatra(Elements.Nakshatra n)
	{
		switch (n.value)
		{
			case Elements.Nakshatra.Name.Aswini:         return Body.Name.Rahu;
			case Elements.Nakshatra.Name.Bharani:        return Body.Name.Rahu;
			case Elements.Nakshatra.Name.Krittika:       return Body.Name.Venus;
			case Elements.Nakshatra.Name.Rohini:         return Body.Name.Venus;
			case Elements.Nakshatra.Name.Mrigarirsa:     return Body.Name.Venus;
			case Elements.Nakshatra.Name.Aridra:         return Body.Name.Sun;
			case Elements.Nakshatra.Name.Punarvasu:      return Body.Name.Sun;
			case Elements.Nakshatra.Name.Pushya:         return Body.Name.Sun;
			case Elements.Nakshatra.Name.Aslesha:        return Body.Name.Sun;
			case Elements.Nakshatra.Name.Makha:          return Body.Name.Moon;
			case Elements.Nakshatra.Name.PoorvaPhalguni: return Body.Name.Moon;
			case Elements.Nakshatra.Name.UttaraPhalguni: return Body.Name.Moon;
			case Elements.Nakshatra.Name.Hasta:          return Body.Name.Mars;
			case Elements.Nakshatra.Name.Chittra:        return Body.Name.Mars;
			case Elements.Nakshatra.Name.Swati:          return Body.Name.Mars;
			case Elements.Nakshatra.Name.Vishaka:        return Body.Name.Mars;
			case Elements.Nakshatra.Name.Anuradha:       return Body.Name.Mercury;
			case Elements.Nakshatra.Name.Jyestha:        return Body.Name.Mercury;
			case Elements.Nakshatra.Name.Moola:          return Body.Name.Mercury;
			case Elements.Nakshatra.Name.PoorvaShada:    return Body.Name.Saturn;
			case Elements.Nakshatra.Name.UttaraShada:    return Body.Name.Saturn;
			case Elements.Nakshatra.Name.Sravana:        return Body.Name.Saturn;
			case Elements.Nakshatra.Name.Dhanishta:      return Body.Name.Jupiter;
			case Elements.Nakshatra.Name.Satabisha:      return Body.Name.Jupiter;
			case Elements.Nakshatra.Name.PoorvaBhadra:   return Body.Name.Jupiter;
			case Elements.Nakshatra.Name.UttaraBhadra:   return Body.Name.Rahu;
			case Elements.Nakshatra.Name.Revati:         return Body.Name.Rahu;
		}

		Trace.Assert(false, "AshtottariDasa::LordOfNakshatra");
		return Body.Name.Lagna;
	}

	private Body.Name nextDasaLordHelper(Body.Name b)
	{
		switch (b)
		{
			case Body.Name.Sun:     return Body.Name.Moon;
			case Body.Name.Moon:    return Body.Name.Mars;
			case Body.Name.Mars:    return Body.Name.Mercury;
			case Body.Name.Mercury: return Body.Name.Saturn;
			case Body.Name.Saturn:  return Body.Name.Jupiter;
			case Body.Name.Jupiter: return Body.Name.Rahu;
			case Body.Name.Rahu:    return Body.Name.Venus;
			case Body.Name.Venus:   return Body.Name.Sun;
		}

		Trace.Assert(false, "AshtottariDasa::nextDasaLord");
		return Body.Name.Lagna;
	}
}