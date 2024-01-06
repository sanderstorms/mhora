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
		return _Dasa(h.getPosition(Elements.Body.Name.Moon).longitude, 1, cycle);
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

	public double lengthOfDasa(Elements.Body.Name plt)
	{
		switch (plt)
		{
			case Elements.Body.Name.Sun:     return 6;
			case Elements.Body.Name.Moon:    return 15;
			case Elements.Body.Name.Mars:    return 8;
			case Elements.Body.Name.Mercury: return 17;
			case Elements.Body.Name.Saturn:  return 10;
			case Elements.Body.Name.Jupiter: return 19;
			case Elements.Body.Name.Rahu:    return 12;
			case Elements.Body.Name.Venus:   return 21;
		}

		Trace.Assert(false, "Ashtottari::lengthOfDasa");
		return 0;
	}

	public Elements.Body.Name lordOfNakshatra(Elements.Nakshatra n)
	{
		switch (n.value)
		{
			case Elements.Nakshatra.Name.Aswini:         return Elements.Body.Name.Rahu;
			case Elements.Nakshatra.Name.Bharani:        return Elements.Body.Name.Rahu;
			case Elements.Nakshatra.Name.Krittika:       return Elements.Body.Name.Venus;
			case Elements.Nakshatra.Name.Rohini:         return Elements.Body.Name.Venus;
			case Elements.Nakshatra.Name.Mrigarirsa:     return Elements.Body.Name.Venus;
			case Elements.Nakshatra.Name.Aridra:         return Elements.Body.Name.Sun;
			case Elements.Nakshatra.Name.Punarvasu:      return Elements.Body.Name.Sun;
			case Elements.Nakshatra.Name.Pushya:         return Elements.Body.Name.Sun;
			case Elements.Nakshatra.Name.Aslesha:        return Elements.Body.Name.Sun;
			case Elements.Nakshatra.Name.Makha:          return Elements.Body.Name.Moon;
			case Elements.Nakshatra.Name.PoorvaPhalguni: return Elements.Body.Name.Moon;
			case Elements.Nakshatra.Name.UttaraPhalguni: return Elements.Body.Name.Moon;
			case Elements.Nakshatra.Name.Hasta:          return Elements.Body.Name.Mars;
			case Elements.Nakshatra.Name.Chittra:        return Elements.Body.Name.Mars;
			case Elements.Nakshatra.Name.Swati:          return Elements.Body.Name.Mars;
			case Elements.Nakshatra.Name.Vishaka:        return Elements.Body.Name.Mars;
			case Elements.Nakshatra.Name.Anuradha:       return Elements.Body.Name.Mercury;
			case Elements.Nakshatra.Name.Jyestha:        return Elements.Body.Name.Mercury;
			case Elements.Nakshatra.Name.Moola:          return Elements.Body.Name.Mercury;
			case Elements.Nakshatra.Name.PoorvaShada:    return Elements.Body.Name.Saturn;
			case Elements.Nakshatra.Name.UttaraShada:    return Elements.Body.Name.Saturn;
			case Elements.Nakshatra.Name.Sravana:        return Elements.Body.Name.Saturn;
			case Elements.Nakshatra.Name.Dhanishta:      return Elements.Body.Name.Jupiter;
			case Elements.Nakshatra.Name.Satabisha:      return Elements.Body.Name.Jupiter;
			case Elements.Nakshatra.Name.PoorvaBhadra:   return Elements.Body.Name.Jupiter;
			case Elements.Nakshatra.Name.UttaraBhadra:   return Elements.Body.Name.Rahu;
			case Elements.Nakshatra.Name.Revati:         return Elements.Body.Name.Rahu;
		}

		Trace.Assert(false, "AshtottariDasa::LordOfNakshatra");
		return Elements.Body.Name.Lagna;
	}

	private Elements.Body.Name nextDasaLordHelper(Elements.Body.Name b)
	{
		switch (b)
		{
			case Elements.Body.Name.Sun:     return Elements.Body.Name.Moon;
			case Elements.Body.Name.Moon:    return Elements.Body.Name.Mars;
			case Elements.Body.Name.Mars:    return Elements.Body.Name.Mercury;
			case Elements.Body.Name.Mercury: return Elements.Body.Name.Saturn;
			case Elements.Body.Name.Saturn:  return Elements.Body.Name.Jupiter;
			case Elements.Body.Name.Jupiter: return Elements.Body.Name.Rahu;
			case Elements.Body.Name.Rahu:    return Elements.Body.Name.Venus;
			case Elements.Body.Name.Venus:   return Elements.Body.Name.Sun;
		}

		Trace.Assert(false, "AshtottariDasa::nextDasaLord");
		return Elements.Body.Name.Lagna;
	}
}