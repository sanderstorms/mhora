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

public class DwisaptatiSamaDasa : NakshatraDasa, INakshatraDasa
{
    private readonly Horoscope h;

    public DwisaptatiSamaDasa(Horoscope _h)
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
        return "Dwisaptati Sama Dasa";
    }

    public double paramAyus()
    {
        return 72.0;
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
            case Body.Body.Name.Sun:     return 9;
            case Body.Body.Name.Moon:    return 9;
            case Body.Body.Name.Mars:    return 9;
            case Body.Body.Name.Mercury: return 9;
            case Body.Body.Name.Jupiter: return 9;
            case Body.Body.Name.Venus:   return 9;
            case Body.Body.Name.Saturn:  return 9;
            case Body.Body.Name.Rahu:    return 9;
        }

        Trace.Assert(false, "DwisaptatiSamaDasa::lengthOfDasa");
        return 0;
    }

    public Body.Body.Name lordOfNakshatra(Nakshatra n)
    {
        var lords = new Body.Body.Name[8]
        {
            Body.Body.Name.Sun,
            Body.Body.Name.Moon,
            Body.Body.Name.Mars,
            Body.Body.Name.Mercury,
            Body.Body.Name.Jupiter,
            Body.Body.Name.Venus,
            Body.Body.Name.Saturn,
            Body.Body.Name.Rahu
        };
        var nak_val  = (int) n.value;
        var moo_val  = (int) Nakshatra.Name.Moola;
        var diff_val = Basics.normalize_inc((int) Nakshatra.Name.Aswini, (int) Nakshatra.Name.Revati, nak_val - moo_val);
        var diff_off = diff_val % 8;
        return lords[diff_off];
    }

    private Body.Body.Name nextDasaLordHelper(Body.Body.Name b)
    {
        switch (b)
        {
            case Body.Body.Name.Sun:     return Body.Body.Name.Moon;
            case Body.Body.Name.Moon:    return Body.Body.Name.Mars;
            case Body.Body.Name.Mars:    return Body.Body.Name.Mercury;
            case Body.Body.Name.Mercury: return Body.Body.Name.Jupiter;
            case Body.Body.Name.Jupiter: return Body.Body.Name.Venus;
            case Body.Body.Name.Venus:   return Body.Body.Name.Saturn;
            case Body.Body.Name.Saturn:  return Body.Body.Name.Rahu;
            case Body.Body.Name.Rahu:    return Body.Body.Name.Sun;
        }

        Trace.Assert(false, "DwisaptatiSamaDasa::nextDasaLord");
        return Body.Body.Name.Lagna;
    }
}