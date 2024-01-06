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

using Mhora.Body;
using Mhora.Tables;
using Mhora.Varga;

namespace Mhora.Calculation.Strength;

// Stronger rasi has larger number of exalted planets - debilitated planets
// Stronger planet is exalted or not debilitated
public class StrengthByExaltation : BaseStrength, IStrengthRasi, IStrengthGraha
{
    public StrengthByExaltation(Horoscope h, Division dtype) : base(h, dtype, true)
    {
    }

    public bool stronger(Tables.Body.Name m, Tables.Body.Name n)
    {
        var valm = value(m);
        var valn = value(n);

        if (valm > valn)
        {
            return true;
        }

        if (valn > valm)
        {
            return false;
        }

        throw new EqualStrength();
    }

    public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
    {
        var vala = value(za);
        var valb = value(zb);

        if (vala > valb)
        {
            return true;
        }

        if (valb > vala)
        {
            return false;
        }

        throw new EqualStrength();
    }

    public int value(ZodiacHouse.Name zn)
    {
        var ret = 0;
        foreach (DivisionPosition dp in std_div_pos)
        {
            if (dp.type != Tables.Body.Type.Graha)
            {
                continue;
            }

            if (dp.zodiac_house.value != zn)
            {
                continue;
            }

            if (dp.isExaltedPhalita())
            {
                ret++;
            }
            else if (dp.isDebilitatedPhalita())
            {
                ret--;
            }
        }

        return ret;
    }

    public int value(Tables.Body.Name b)
    {
        if (h.getPosition(b).toDivisionPosition(dtype).isExaltedPhalita())
        {
            return 1;
        }

        if (h.getPosition(b).toDivisionPosition(dtype).isDebilitatedPhalita())
        {
            return -1;
        }

        return 0;
    }
}