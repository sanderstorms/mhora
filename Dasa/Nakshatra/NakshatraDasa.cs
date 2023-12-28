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

/// <summary>
///     Base class to be implemented by Vimsottari/Ashtottari like dasas
/// </summary>
public abstract class NakshatraDasa : Dasa
{
    protected       INakshatraDasa       common;
    protected       INakshatraKaranaDasa karanaCommon;
    protected       INakshatraTithiDasa  tithiCommon;
    protected       INakshatraYogaDasa   yogaCommon;
    public abstract object               GetOptions();
    public abstract object               SetOptions(object a);

    /// <summary>
    ///     Returns the antardasas
    /// </summary>
    /// <param name="pdi">The current dasa item whose antardasas should be calculated</param>
    /// <returns></returns>
    protected ArrayList _AntarDasa(DasaEntry pdi)
    {
        var numItems = common.numberOfDasaItems();
        var ditems   = new ArrayList(numItems);
        var curr     = new DasaEntry(pdi.graha, pdi.startUT, 0, pdi.level + 1, string.Empty);
        for (var i = 0; i < numItems; i++)
        {
            var dlength = common.lengthOfDasa(curr.graha) / common.paramAyus() * pdi.dasaLength;
            var desc    = pdi.shortDesc + " " + Body.Body.toShortString(curr.graha);
            var di      = new DasaEntry(curr.graha, curr.startUT, dlength, curr.level, desc);
            ditems.Add(di);
            curr         = common.nextDasaLord(di);
            curr.startUT = di.startUT + dlength;
        }

        return ditems;
    }

    /// <summary>
    ///     Given a Lontigude, a Nakshatra Offset and Cycle number, calculate the maha dasa
    /// </summary>
    /// <param name="lon">The seed longitude (eg. Moon for Utpanna)</param>
    /// <param name="offset">The seed start (eg. 5 for Utpanna)</param>
    /// <param name="cycle">The cycle number. eg which 120 year cycle? 0 for "current"</param>
    /// <returns></returns>
    protected ArrayList _Dasa(Longitude lon, int offset, int cycle)
    {
        var ditems         = new ArrayList(common.numberOfDasaItems());
        var n              = lon.toNakshatra().add(offset);
        var g              = common.lordOfNakshatra(n);
        var perc_traversed = lon.percentageOfNakshatra();
        var start          = cycle * common.paramAyus() - perc_traversed / 100.0 * common.lengthOfDasa(g);
        //System.mhora.Log.Debug ("{0} {1} {2}", common.lengthOfDasa(g), perc_traversed, start);

        // Calculate a "seed" dasaItem, to make use of the AntarDasa function
        var di = new DasaEntry(g, start, common.paramAyus(), 0, string.Empty);
        return _AntarDasa(di);
    }

    protected ArrayList _TithiDasa(Longitude lon, int offset, int cycle)
    {
        //ArrayList ditems = new ArrayList(tithiCommon.numberOfDasaItems());
        lon = lon.add(new Longitude(cycle * (offset - 1) * 12.0));
        var g = tithiCommon.lordOfTithi(lon);

        var tithiOffset = lon.value;
        while (tithiOffset >= 12.0)
        {
            tithiOffset -= 12.0;
        }

        var perc_traversed = tithiOffset / 12.0;
        var start          = cycle * tithiCommon.paramAyus() - perc_traversed * tithiCommon.lengthOfDasa(g);
        var di             = new DasaEntry(g, start, tithiCommon.paramAyus(), 0, string.Empty);
        return _AntarDasa(di);
    }

    protected ArrayList _YogaDasa(Longitude lon, int offset, int cycle)
    {
        lon = lon.add(new Longitude(cycle * (offset - 1) * (360.0 / 27.0)));
        var g = yogaCommon.lordOfYoga(lon);

        var yogaOffset     = lon.toSunMoonYogaOffset();
        var perc_traversed = yogaOffset / (360.0 / 27.0);
        var start          = cycle * common.paramAyus() - perc_traversed * common.lengthOfDasa(g);
        var di             = new DasaEntry(g, start, common.paramAyus(), 0, string.Empty);
        return _AntarDasa(di);
    }

    protected ArrayList _KaranaDasa(Longitude lon, int offset, int cycle)
    {
        lon = lon.add(new Longitude(cycle * (offset - 1) * (360.0 / 60.0)));
        var g = karanaCommon.lordOfKarana(lon);

        var karanaOffset   = lon.toKaranaOffset();
        var perc_traversed = karanaOffset / (360.0 / 60.0);
        var start          = cycle * common.paramAyus() - perc_traversed * common.lengthOfDasa(g);
        var di             = new DasaEntry(g, start, common.paramAyus(), 0, string.Empty);
        return _AntarDasa(di);
    }

    public void recalculateOptions()
    {
    }
}