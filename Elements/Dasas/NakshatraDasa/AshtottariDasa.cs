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
using Mhora.Util;

namespace Mhora.Elements.Dasas.NakshatraDasa;

public class AshtottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public AshtottariDasa(Horoscope horoscope)
	{
		Common = this;
		_h      = horoscope;
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
		return "Ashtottari Dasa";
	}

	public double ParamAyus()
	{
		return 108.0;
	}

	public int NumberOfDasaItems()
	{
		return 8;
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return new DasaEntry(NextDasaLordHelper(di.Graha), TimeOffset.Zero, 0, di.Level, string.Empty);
	}

	public TimeOffset LengthOfDasa(Body plt)
	{
		switch (plt)
		{
			case Body.Sun:     return 6;
			case Body.Moon:    return 15;
			case Body.Mars:    return 8;
			case Body.Mercury: return 17;
			case Body.Saturn:  return 10;
			case Body.Jupiter: return 19;
			case Body.Rahu:    return 12;
			case Body.Venus:   return 21;
		}

		Trace.Assert(false, "Ashtottari::LengthOfDasa");
		return 0;
	}

	public Body LordOfNakshatra(Nakshatra n)
	{
		switch (n)
		{
			case Nakshatra.Aswini:         return Body.Rahu;
			case Nakshatra.Bharani:        return Body.Rahu;
			case Nakshatra.Krittika:       return Body.Venus;
			case Nakshatra.Rohini:         return Body.Venus;
			case Nakshatra.Mrigarirsa:     return Body.Venus;
			case Nakshatra.Aridra:         return Body.Sun;
			case Nakshatra.Punarvasu:      return Body.Sun;
			case Nakshatra.Pushya:         return Body.Sun;
			case Nakshatra.Aslesha:        return Body.Sun;
			case Nakshatra.Makha:          return Body.Moon;
			case Nakshatra.PoorvaPhalguni: return Body.Moon;
			case Nakshatra.UttaraPhalguni: return Body.Moon;
			case Nakshatra.Hasta:          return Body.Mars;
			case Nakshatra.Chittra:        return Body.Mars;
			case Nakshatra.Swati:          return Body.Mars;
			case Nakshatra.Vishaka:        return Body.Mars;
			case Nakshatra.Anuradha:       return Body.Mercury;
			case Nakshatra.Jyestha:        return Body.Mercury;
			case Nakshatra.Moola:          return Body.Mercury;
			case Nakshatra.PoorvaShada:    return Body.Saturn;
			case Nakshatra.UttaraShada:    return Body.Saturn;
			case Nakshatra.Sravana:        return Body.Saturn;
			case Nakshatra.Dhanishta:      return Body.Jupiter;
			case Nakshatra.Satabisha:      return Body.Jupiter;
			case Nakshatra.PoorvaBhadra:   return Body.Jupiter;
			case Nakshatra.UttaraBhadra:   return Body.Rahu;
			case Nakshatra.Revati:         return Body.Rahu;
		}

		Trace.Assert(false, "AshtottariDasa::NakshatraLord");
		return Body.Lagna;
	}

	private Body NextDasaLordHelper(Body b)
	{
		switch (b)
		{
			case Body.Sun:     return Body.Moon;
			case Body.Moon:    return Body.Mars;
			case Body.Mars:    return Body.Mercury;
			case Body.Mercury: return Body.Saturn;
			case Body.Saturn:  return Body.Jupiter;
			case Body.Jupiter: return Body.Rahu;
			case Body.Rahu:    return Body.Venus;
			case Body.Venus:   return Body.Sun;
		}

		Trace.Assert(false, "AshtottariDasa::NextDasaLord");
		return Body.Lagna;
	}
}