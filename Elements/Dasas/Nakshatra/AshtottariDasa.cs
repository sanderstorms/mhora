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
		return _Dasa(_h.GetPosition(Body.BodyType.Moon).Longitude, 1, cycle);
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
		return new DasaEntry(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);
	}

	public double LengthOfDasa(Body.BodyType plt)
	{
		switch (plt)
		{
			case Body.BodyType.Sun:     return 6;
			case Body.BodyType.Moon:    return 15;
			case Body.BodyType.Mars:    return 8;
			case Body.BodyType.Mercury: return 17;
			case Body.BodyType.Saturn:  return 10;
			case Body.BodyType.Jupiter: return 19;
			case Body.BodyType.Rahu:    return 12;
			case Body.BodyType.Venus:   return 21;
		}

		Trace.Assert(false, "Ashtottari::LengthOfDasa");
		return 0;
	}

	public Body.BodyType LordOfNakshatra(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:         return Body.BodyType.Rahu;
			case Nakshatras.Nakshatra.Bharani:        return Body.BodyType.Rahu;
			case Nakshatras.Nakshatra.Krittika:       return Body.BodyType.Venus;
			case Nakshatras.Nakshatra.Rohini:         return Body.BodyType.Venus;
			case Nakshatras.Nakshatra.Mrigarirsa:     return Body.BodyType.Venus;
			case Nakshatras.Nakshatra.Aridra:         return Body.BodyType.Sun;
			case Nakshatras.Nakshatra.Punarvasu:      return Body.BodyType.Sun;
			case Nakshatras.Nakshatra.Pushya:         return Body.BodyType.Sun;
			case Nakshatras.Nakshatra.Aslesha:        return Body.BodyType.Sun;
			case Nakshatras.Nakshatra.Makha:          return Body.BodyType.Moon;
			case Nakshatras.Nakshatra.PoorvaPhalguni: return Body.BodyType.Moon;
			case Nakshatras.Nakshatra.UttaraPhalguni: return Body.BodyType.Moon;
			case Nakshatras.Nakshatra.Hasta:          return Body.BodyType.Mars;
			case Nakshatras.Nakshatra.Chittra:        return Body.BodyType.Mars;
			case Nakshatras.Nakshatra.Swati:          return Body.BodyType.Mars;
			case Nakshatras.Nakshatra.Vishaka:        return Body.BodyType.Mars;
			case Nakshatras.Nakshatra.Anuradha:       return Body.BodyType.Mercury;
			case Nakshatras.Nakshatra.Jyestha:        return Body.BodyType.Mercury;
			case Nakshatras.Nakshatra.Moola:          return Body.BodyType.Mercury;
			case Nakshatras.Nakshatra.PoorvaShada:    return Body.BodyType.Saturn;
			case Nakshatras.Nakshatra.UttaraShada:    return Body.BodyType.Saturn;
			case Nakshatras.Nakshatra.Sravana:        return Body.BodyType.Saturn;
			case Nakshatras.Nakshatra.Dhanishta:      return Body.BodyType.Jupiter;
			case Nakshatras.Nakshatra.Satabisha:      return Body.BodyType.Jupiter;
			case Nakshatras.Nakshatra.PoorvaBhadra:   return Body.BodyType.Jupiter;
			case Nakshatras.Nakshatra.UttaraBhadra:   return Body.BodyType.Rahu;
			case Nakshatras.Nakshatra.Revati:         return Body.BodyType.Rahu;
		}

		Trace.Assert(false, "AshtottariDasa::NakshatraLord");
		return Body.BodyType.Lagna;
	}

	private Body.BodyType NextDasaLordHelper(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return Body.BodyType.Moon;
			case Body.BodyType.Moon:    return Body.BodyType.Mars;
			case Body.BodyType.Mars:    return Body.BodyType.Mercury;
			case Body.BodyType.Mercury: return Body.BodyType.Saturn;
			case Body.BodyType.Saturn:  return Body.BodyType.Jupiter;
			case Body.BodyType.Jupiter: return Body.BodyType.Rahu;
			case Body.BodyType.Rahu:    return Body.BodyType.Venus;
			case Body.BodyType.Venus:   return Body.BodyType.Sun;
		}

		Trace.Assert(false, "AshtottariDasa::NextDasaLord");
		return Body.BodyType.Lagna;
	}
}