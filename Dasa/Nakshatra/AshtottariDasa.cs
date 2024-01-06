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
		return _Dasa(h.getPosition(Tables.Body.Name.Moon).longitude, 1, cycle);
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

	public double lengthOfDasa(Tables.Body.Name plt)
	{
		switch (plt)
		{
			case Tables.Body.Name.Sun:     return 6;
			case Tables.Body.Name.Moon:    return 15;
			case Tables.Body.Name.Mars:    return 8;
			case Tables.Body.Name.Mercury: return 17;
			case Tables.Body.Name.Saturn:  return 10;
			case Tables.Body.Name.Jupiter: return 19;
			case Tables.Body.Name.Rahu:    return 12;
			case Tables.Body.Name.Venus:   return 21;
		}

		Trace.Assert(false, "Ashtottari::lengthOfDasa");
		return 0;
	}

	public Tables.Body.Name lordOfNakshatra(Nakshatra n)
	{
		switch (n.value)
		{
			case Nakshatra.Name.Aswini:         return Tables.Body.Name.Rahu;
			case Nakshatra.Name.Bharani:        return Tables.Body.Name.Rahu;
			case Nakshatra.Name.Krittika:       return Tables.Body.Name.Venus;
			case Nakshatra.Name.Rohini:         return Tables.Body.Name.Venus;
			case Nakshatra.Name.Mrigarirsa:     return Tables.Body.Name.Venus;
			case Nakshatra.Name.Aridra:         return Tables.Body.Name.Sun;
			case Nakshatra.Name.Punarvasu:      return Tables.Body.Name.Sun;
			case Nakshatra.Name.Pushya:         return Tables.Body.Name.Sun;
			case Nakshatra.Name.Aslesha:        return Tables.Body.Name.Sun;
			case Nakshatra.Name.Makha:          return Tables.Body.Name.Moon;
			case Nakshatra.Name.PoorvaPhalguni: return Tables.Body.Name.Moon;
			case Nakshatra.Name.UttaraPhalguni: return Tables.Body.Name.Moon;
			case Nakshatra.Name.Hasta:          return Tables.Body.Name.Mars;
			case Nakshatra.Name.Chittra:        return Tables.Body.Name.Mars;
			case Nakshatra.Name.Swati:          return Tables.Body.Name.Mars;
			case Nakshatra.Name.Vishaka:        return Tables.Body.Name.Mars;
			case Nakshatra.Name.Anuradha:       return Tables.Body.Name.Mercury;
			case Nakshatra.Name.Jyestha:        return Tables.Body.Name.Mercury;
			case Nakshatra.Name.Moola:          return Tables.Body.Name.Mercury;
			case Nakshatra.Name.PoorvaShada:    return Tables.Body.Name.Saturn;
			case Nakshatra.Name.UttaraShada:    return Tables.Body.Name.Saturn;
			case Nakshatra.Name.Sravana:        return Tables.Body.Name.Saturn;
			case Nakshatra.Name.Dhanishta:      return Tables.Body.Name.Jupiter;
			case Nakshatra.Name.Satabisha:      return Tables.Body.Name.Jupiter;
			case Nakshatra.Name.PoorvaBhadra:   return Tables.Body.Name.Jupiter;
			case Nakshatra.Name.UttaraBhadra:   return Tables.Body.Name.Rahu;
			case Nakshatra.Name.Revati:         return Tables.Body.Name.Rahu;
		}

		Trace.Assert(false, "AshtottariDasa::LordOfNakshatra");
		return Tables.Body.Name.Lagna;
	}

	private Tables.Body.Name nextDasaLordHelper(Tables.Body.Name b)
	{
		switch (b)
		{
			case Tables.Body.Name.Sun:     return Tables.Body.Name.Moon;
			case Tables.Body.Name.Moon:    return Tables.Body.Name.Mars;
			case Tables.Body.Name.Mars:    return Tables.Body.Name.Mercury;
			case Tables.Body.Name.Mercury: return Tables.Body.Name.Saturn;
			case Tables.Body.Name.Saturn:  return Tables.Body.Name.Jupiter;
			case Tables.Body.Name.Jupiter: return Tables.Body.Name.Rahu;
			case Tables.Body.Name.Rahu:    return Tables.Body.Name.Venus;
			case Tables.Body.Name.Venus:   return Tables.Body.Name.Sun;
		}

		Trace.Assert(false, "AshtottariDasa::nextDasaLord");
		return Tables.Body.Name.Lagna;
	}
}