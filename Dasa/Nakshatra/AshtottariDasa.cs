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
        return _Dasa(h.getPosition(Body.Body.Name.Moon).longitude, 1, cycle);
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

    public double lengthOfDasa(Body.Body.Name plt)
    {
        switch (plt)
        {
            case Body.Body.Name.Sun:     return 6;
            case Body.Body.Name.Moon:    return 15;
            case Body.Body.Name.Mars:    return 8;
            case Body.Body.Name.Mercury: return 17;
            case Body.Body.Name.Saturn:  return 10;
            case Body.Body.Name.Jupiter: return 19;
            case Body.Body.Name.Rahu:    return 12;
            case Body.Body.Name.Venus:   return 21;
        }

        Trace.Assert(false, "Ashtottari::lengthOfDasa");
        return 0;
    }

    public Body.Body.Name lordOfNakshatra(Nakshatra n)
    {
        switch (n.value)
        {
            case Nakshatra.Name.Aswini:         return Body.Body.Name.Rahu;
            case Nakshatra.Name.Bharani:        return Body.Body.Name.Rahu;
            case Nakshatra.Name.Krittika:       return Body.Body.Name.Venus;
            case Nakshatra.Name.Rohini:         return Body.Body.Name.Venus;
            case Nakshatra.Name.Mrigarirsa:     return Body.Body.Name.Venus;
            case Nakshatra.Name.Aridra:         return Body.Body.Name.Sun;
            case Nakshatra.Name.Punarvasu:      return Body.Body.Name.Sun;
            case Nakshatra.Name.Pushya:         return Body.Body.Name.Sun;
            case Nakshatra.Name.Aslesha:        return Body.Body.Name.Sun;
            case Nakshatra.Name.Makha:          return Body.Body.Name.Moon;
            case Nakshatra.Name.PoorvaPhalguni: return Body.Body.Name.Moon;
            case Nakshatra.Name.UttaraPhalguni: return Body.Body.Name.Moon;
            case Nakshatra.Name.Hasta:          return Body.Body.Name.Mars;
            case Nakshatra.Name.Chittra:        return Body.Body.Name.Mars;
            case Nakshatra.Name.Swati:          return Body.Body.Name.Mars;
            case Nakshatra.Name.Vishaka:        return Body.Body.Name.Mars;
            case Nakshatra.Name.Anuradha:       return Body.Body.Name.Mercury;
            case Nakshatra.Name.Jyestha:        return Body.Body.Name.Mercury;
            case Nakshatra.Name.Moola:          return Body.Body.Name.Mercury;
            case Nakshatra.Name.PoorvaShada:    return Body.Body.Name.Saturn;
            case Nakshatra.Name.UttaraShada:    return Body.Body.Name.Saturn;
            case Nakshatra.Name.Sravana:        return Body.Body.Name.Saturn;
            case Nakshatra.Name.Dhanishta:      return Body.Body.Name.Jupiter;
            case Nakshatra.Name.Satabisha:      return Body.Body.Name.Jupiter;
            case Nakshatra.Name.PoorvaBhadra:   return Body.Body.Name.Jupiter;
            case Nakshatra.Name.UttaraBhadra:   return Body.Body.Name.Rahu;
            case Nakshatra.Name.Revati:         return Body.Body.Name.Rahu;
        }

        Trace.Assert(false, "AshtottariDasa::LordOfNakshatra");
        return Body.Body.Name.Lagna;
    }

    private Body.Body.Name nextDasaLordHelper(Body.Body.Name b)
    {
        switch (b)
        {
            case Body.Body.Name.Sun:     return Body.Body.Name.Moon;
            case Body.Body.Name.Moon:    return Body.Body.Name.Mars;
            case Body.Body.Name.Mars:    return Body.Body.Name.Mercury;
            case Body.Body.Name.Mercury: return Body.Body.Name.Saturn;
            case Body.Body.Name.Saturn:  return Body.Body.Name.Jupiter;
            case Body.Body.Name.Jupiter: return Body.Body.Name.Rahu;
            case Body.Body.Name.Rahu:    return Body.Body.Name.Venus;
            case Body.Body.Name.Venus:   return Body.Body.Name.Sun;
        }

        Trace.Assert(false, "AshtottariDasa::nextDasaLord");
        return Body.Body.Name.Lagna;
    }
}