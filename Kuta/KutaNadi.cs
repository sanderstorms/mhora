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

namespace Mhora.Kuta;

public class KutaNadi
{
    public enum EType
    {
        IVata,
        IPitta,
        ISleshma
    }

    public static int getMaxScore()
    {
        return 2;
    }

    public static int getScore(Nakshatra m, Nakshatra n)
    {
        var ea = getType(m);
        var eb = getType(n);
        if (ea != eb)
        {
            return 2;
        }

        if (ea == EType.IVata || ea == EType.ISleshma)
        {
            return 1;
        }

        return 0;
    }

    public static EType getType(Nakshatra n)
    {
        switch (n.value)
        {
            case Nakshatra.Name.Aswini:
            case Nakshatra.Name.Aridra:
            case Nakshatra.Name.Punarvasu:
            case Nakshatra.Name.UttaraPhalguni:
            case Nakshatra.Name.Hasta:
            case Nakshatra.Name.Jyestha:
            case Nakshatra.Name.Moola:
            case Nakshatra.Name.Satabisha:
            case Nakshatra.Name.PoorvaBhadra:
                return EType.IVata;
            case Nakshatra.Name.Bharani:
            case Nakshatra.Name.Mrigarirsa:
            case Nakshatra.Name.Pushya:
            case Nakshatra.Name.PoorvaPhalguni:
            case Nakshatra.Name.Chittra:
            case Nakshatra.Name.Anuradha:
            case Nakshatra.Name.PoorvaShada:
            case Nakshatra.Name.Dhanishta:
            case Nakshatra.Name.UttaraBhadra:
                return EType.IPitta;
        }

        return EType.ISleshma;
    }
}