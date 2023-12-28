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

using Mhora.Calculation;

namespace Mhora;

/// <summary>
///     A list of nakshatras, and related helper functions
/// </summary>
public class Nakshatra28
{
    public enum Name
    {
        Aswini         = 1,
        Bharani        = 2,
        Krittika       = 3,
        Rohini         = 4,
        Mrigarirsa     = 5,
        Aridra         = 6,
        Punarvasu      = 7,
        Pushya         = 8,
        Aslesha        = 9,
        Makha          = 10,
        PoorvaPhalguni = 11,
        UttaraPhalguni = 12,
        Hasta          = 13,
        Chittra        = 14,
        Swati          = 15,
        Vishaka        = 16,
        Anuradha       = 17,
        Jyestha        = 18,
        Moola          = 19,
        PoorvaShada    = 20,
        UttaraShada    = 21,
        Abhijit        = 22,
        Sravana        = 23,
        Dhanishta      = 24,
        Satabisha      = 25,
        PoorvaBhadra   = 26,
        UttaraBhadra   = 27,
        Revati         = 28
    }

    public Nakshatra28(Name nak)
    {
        value = nak;
    }

    public Name value
    {
        get;
        set;
    }

    public int normalize()
    {
        return Basics.normalize_inc(1, 28, (int) value);
    }

    public Nakshatra28 add(int i)
    {
        var snum = Basics.normalize_inc(1, 28, (int) value + i - 1);
        return new Nakshatra28((Name) snum);
    }
}