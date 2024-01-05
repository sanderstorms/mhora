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

using System;
using System.Diagnostics;
using mhora.Util;

namespace Mhora.Calculation;

/// <summary>
///     A package related to a ZodiacHouse
/// </summary>
public class ZodiacHouse : ICloneable
{
    public enum Name
    {
        Ari = 1,
        Tau = 2,
        Gem = 3,
        Can = 4,
        Leo = 5,
        Vir = 6,
        Lib = 7,
        Sco = 8,
        Sag = 9,
        Cap = 10,
        Aqu = 11,
        Pis = 12
    }

    public enum RiseType
    {
        RisesWithHead,
        RisesWithFoot,
        RisesWithBoth
    }

    public static Name[] AllNames =
    {
        Name.Ari,
        Name.Tau,
        Name.Gem,
        Name.Can,
        Name.Leo,
        Name.Vir,
        Name.Lib,
        Name.Sco,
        Name.Sag,
        Name.Cap,
        Name.Aqu,
        Name.Pis
    };

    public ZodiacHouse(Name zhouse) { value = zhouse; }

    public Name value
    {
        get;
        set;
    }

    public object Clone()
    {
        return new ZodiacHouse(value);
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public int normalize()
    {
        return Basics.normalize_inc(1, 12, (int) value);
    }

    public ZodiacHouse add(int i)
    {
        var znum = Basics.normalize_inc(1, 12, (int) value + i - 1);
        return new ZodiacHouse((Name) znum);
    }

    public ZodiacHouse addReverse(int i)
    {
        var znum = Basics.normalize_inc(1, 12, (int) value - i + 1);
        return new ZodiacHouse((Name) znum);
    }

    public int numHousesBetweenReverse(ZodiacHouse zrel)
    {
        return Basics.normalize_inc(1, 12, 14 - numHousesBetween(zrel));
    }

    public int numHousesBetween(ZodiacHouse zrel)
    {
        var ret = Basics.normalize_inc(1, 12, (int) zrel.value - (int) value + 1);
        Trace.Assert(ret >= 1 && ret <= 12, "ZodiacHouse.numHousesBetween failed");
        return ret;
    }

    public Longitude Origin
    {
	    get
	    {
		    return new Longitude((value.Index() - 1) * 30.0);
	    }
    }

    public Longitude DivisionalLongitude(Longitude longitude, int nrOfDivisions)
    {
	    var houseBase = Origin;
	    var div       = (30.0 / nrOfDivisions);
	    var offset    = (longitude.value % div);

	    return new Longitude(houseBase.value + offset * nrOfDivisions);
    }

	public bool isDaySign()
    {
        switch (value)
        {
            case Name.Ari:
            case Name.Tau:
            case Name.Gem:
            case Name.Can:
                return false;

            case Name.Leo:
            case Name.Vir:
            case Name.Lib:
            case Name.Sco:
                return true;

            case Name.Sag:
            case Name.Cap:
                return false;

            case Name.Aqu:
            case Name.Pis:
                return true;

            default:
                Trace.Assert(false, "isDaySign internal error");
                return true;
        }
    }

    public bool isOdd()
    {
        switch (value)
        {
            case Name.Ari:
            case Name.Gem:
            case Name.Leo:
            case Name.Lib:
            case Name.Sag:
            case Name.Aqu:
                return true;

            case Name.Tau:
            case Name.Can:
            case Name.Vir:
            case Name.Sco:
            case Name.Cap:
            case Name.Pis:
                return false;

            default:
                Trace.Assert(false, "isOdd internal error");
                return true;
        }
    }

    public bool isOddFooted()
    {
        switch (value)
        {
            case Name.Ari: return true;
            case Name.Tau: return true;
            case Name.Gem: return true;
            case Name.Can: return false;
            case Name.Leo: return false;
            case Name.Vir: return false;
            case Name.Lib: return true;
            case Name.Sco: return true;
            case Name.Sag: return true;
            case Name.Cap: return false;
            case Name.Aqu: return false;
            case Name.Pis: return false;
        }

        Trace.Assert(false, "ZOdiacHouse::isOddFooted");
        return false;
    }

    public bool RasiDristi(ZodiacHouse b)
    {
        var ma = (int) value   % 3;
        var mb = (int) b.value % 3;

        switch (ma)
        {
            case 1:
                if (mb == 2 && add(2).value != b.value)
                {
                    return true;
                }

                return false;
            case 2:
                if (mb == 1 && addReverse(2).value != b.value)
                {
                    return true;
                }

                return false;
            case 0:
                if (mb == 0)
                {
                    return true;
                }

                return false;
        }

        Trace.Assert(false, "ZodiacHouse.RasiDristi");
        return false;
    }

    public RiseType RisesWith()
    {
        switch (value)
        {
            case Name.Ari:
            case Name.Tau:
            case Name.Can:
            case Name.Sag:
            case Name.Cap:
                return RiseType.RisesWithFoot;
            case Name.Gem:
            case Name.Leo:
            case Name.Vir:
            case Name.Lib:
            case Name.Sco:
            case Name.Aqu:
                return RiseType.RisesWithHead;
            default: return RiseType.RisesWithBoth;
        }
    }

    public ZodiacHouse LordsOtherSign()
    {
        var ret = Name.Ari;
        switch (value)
        {
            case Name.Ari:
                ret = Name.Sco;
                break;
            case Name.Tau:
                ret = Name.Lib;
                break;
            case Name.Gem:
                ret = Name.Vir;
                break;
            case Name.Can:
                ret = Name.Can;
                break;
            case Name.Leo:
                ret = Name.Leo;
                break;
            case Name.Vir:
                ret = Name.Gem;
                break;
            case Name.Lib:
                ret = Name.Tau;
                break;
            case Name.Sco:
                ret = Name.Ari;
                break;
            case Name.Sag:
                ret = Name.Pis;
                break;
            case Name.Cap:
                ret = Name.Aqu;
                break;
            case Name.Aqu:
                ret = Name.Cap;
                break;
            case Name.Pis:
                ret = Name.Sag;
                break;
            default:
                Debug.Assert(false, "ZodiacHouse::KalachakraMirrorSign");
                break;
        }

        return new ZodiacHouse(ret);
    }

    public ZodiacHouse AdarsaSign()
    {
        var ret = Name.Ari;
        switch (value)
        {
            case Name.Ari:
                ret = Name.Sco;
                break;
            case Name.Tau:
                ret = Name.Lib;
                break;
            case Name.Gem:
                ret = Name.Vir;
                break;
            case Name.Can:
                ret = Name.Aqu;
                break;
            case Name.Leo:
                ret = Name.Cap;
                break;
            case Name.Vir:
                ret = Name.Gem;
                break;
            case Name.Lib:
                ret = Name.Tau;
                break;
            case Name.Sco:
                ret = Name.Ari;
                break;
            case Name.Sag:
                ret = Name.Pis;
                break;
            case Name.Cap:
                ret = Name.Leo;
                break;
            case Name.Aqu:
                ret = Name.Can;
                break;
            case Name.Pis:
                ret = Name.Sag;
                break;
            default:
                Debug.Assert(false, "ZodiacHouse::AdarsaSign");
                break;
        }

        return new ZodiacHouse(ret);
    }

    public ZodiacHouse AbhimukhaSign()
    {
        var ret = Name.Ari;
        switch (value)
        {
            case Name.Ari:
                ret = Name.Sco;
                break;
            case Name.Tau:
                ret = Name.Lib;
                break;
            case Name.Gem:
                ret = Name.Sag;
                break;
            case Name.Can:
                ret = Name.Aqu;
                break;
            case Name.Leo:
                ret = Name.Cap;
                break;
            case Name.Vir:
                ret = Name.Pis;
                break;
            case Name.Lib:
                ret = Name.Tau;
                break;
            case Name.Sco:
                ret = Name.Ari;
                break;
            case Name.Sag:
                ret = Name.Gem;
                break;
            case Name.Cap:
                ret = Name.Leo;
                break;
            case Name.Aqu:
                ret = Name.Can;
                break;
            case Name.Pis:
                ret = Name.Vir;
                break;
            default:
                Debug.Assert(false, "ZodiacHouse::AbhimukhaSign");
                break;
        }

        return new ZodiacHouse(ret);
    }

    public static string ToShortString(Name z)
    {
        switch (z)
        {
            case Name.Ari: return "Ar";
            case Name.Tau: return "Ta";
            case Name.Gem: return "Ge";
            case Name.Can: return "Cn";
            case Name.Leo: return "Le";
            case Name.Vir: return "Vi";
            case Name.Lib: return "Li";
            case Name.Sco: return "Sc";
            case Name.Sag: return "Sg";
            case Name.Cap: return "Cp";
            case Name.Aqu: return "Aq";
            case Name.Pis: return "Pi";
            default:       return string.Empty;
        }
    }
}