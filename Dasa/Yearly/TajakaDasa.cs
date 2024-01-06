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
using Mhora.Calculation;

namespace Mhora;

public class TajakaDasa : Dasa, IDasa
{
    private Horoscope h;

    public TajakaDasa(Horoscope _h)
    {
        h = _h;
    }

    public object GetOptions()
    {
        return new object();
    }

    public object SetOptions(object a)
    {
        return new object();
    }

    public void recalculateOptions()
    {
    }

    public double paramAyus()
    {
        return 60.0;
    }

    public ArrayList Dasa(int cycle)
    {
        var al          = new ArrayList(60);
        var cycle_start = cycle * paramAyus();
        for (var i = 0; i < 60; i++)
        {
            var start = cycle_start + i;
            var di    = new DasaEntry(Tables.Body.Name.Other, start, 1.0, 1, "Tajaka Year");
            al.Add(di);
        }

        return al;
    }

    public ArrayList AntarDasa(DasaEntry pdi)
    {
        string[] desc =
        {
            "  Tajaka Month",
            "    Tajaka 60 hour",
            "      Tajaka 5 hour",
            "        Tajaka 25 minute",
            "          Tajaka 2 minute"
        };
        if (pdi.level == 6)
        {
            return new ArrayList();
        }

        ArrayList al;
        double    start = 0.0, length = 0.0;
        var       level = 0;

        al     = new ArrayList(12);
        start  = pdi.startUT;
        level  = pdi.level + 1;
        length = pdi.dasaLength / 12.0;
        for (var i = 0; i < 12; i++)
        {
            var di = new DasaEntry(Tables.Body.Name.Other, start, length, level, desc[level - 2]);
            al.Add(di);
            start += length;
        }

        return al;
    }

    public string Description()
    {
        return "Tajaka Chart Dasa";
    }
}