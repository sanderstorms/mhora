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

namespace Mhora.Elements.Dasas.Nakshatra;

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
		var numItems = common.NumberOfDasaItems();
		var ditems   = new ArrayList(numItems);
		var curr     = new DasaEntry(pdi.Graha, pdi.StartUT, 0, pdi.Level + 1, string.Empty);
		for (var i = 0; i < numItems; i++)
		{
			var dlength = common.LengthOfDasa(curr.Graha) / common.ParamAyus() * pdi.DasaLength;
			var desc    = pdi.DasaName + " " + curr.Graha.ToShortString();
			var di      = new DasaEntry(curr.Graha, curr.StartUT, dlength, curr.Level, desc);
			ditems.Add(di);
			curr         = common.NextDasaLord(di);
			curr.StartUT = di.StartUT + dlength;
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
		var ditems         = new ArrayList(common.NumberOfDasaItems());
		var n              = lon.ToNakshatra().Add(offset);
		var g              = common.LordOfNakshatra(n);
		var perc_traversed = lon.PercentageOfNakshatra();
		var start          = cycle * common.ParamAyus() - perc_traversed / 100.0 * common.LengthOfDasa(g);
		//System.mhora.Log.Debug ("{0} {1} {2}", common.LengthOfDasa(g), perc_traversed, start);

		// Calculate a "seed" dasaItem, to make use of the AntarDasa function
		var di = new DasaEntry(g, start, common.ParamAyus(), 0, string.Empty);
		return _AntarDasa(di);
	}

	protected ArrayList _TithiDasa(Longitude lon, int offset, int cycle)
	{
		//ArrayList ditems = new ArrayList(tithiCommon.numberOfDasaItems());
		lon = lon.Add(new Longitude(cycle * (offset - 1) * 12.0));
		var g = tithiCommon.lordOfTithi(lon);

		var tithiOffset = lon.Value;
		while (tithiOffset >= 12.0)
		{
			tithiOffset -= 12.0;
		}

		var perc_traversed = tithiOffset / 12.0;
		var start          = cycle * tithiCommon.ParamAyus() - perc_traversed * tithiCommon.LengthOfDasa(g);
		var di             = new DasaEntry(g, start, tithiCommon.ParamAyus(), 0, string.Empty);
		return _AntarDasa(di);
	}

	protected ArrayList _YogaDasa(Longitude lon, int offset, int cycle)
	{
		lon = lon.Add(new Longitude(cycle * (offset - 1) * (360.0 / 27.0)));
		var g = yogaCommon.lordOfYoga(lon);

		var yogaOffset     = lon.ToSunMoonYogaOffset();
		var perc_traversed = yogaOffset / (360.0 / 27.0);
		var start          = cycle * common.ParamAyus() - perc_traversed * common.LengthOfDasa(g);
		var di             = new DasaEntry(g, start, common.ParamAyus(), 0, string.Empty);
		return _AntarDasa(di);
	}

	protected ArrayList _KaranaDasa(Longitude lon, int offset, int cycle)
	{
		lon = lon.Add(new Longitude(cycle * (offset - 1) * (360.0 / 60.0)));
		var g = karanaCommon.LordOfKarana(lon);

		var karanaOffset   = lon.ToKaranaOffset();
		var perc_traversed = karanaOffset / (360.0 / 60.0);
		var start          = cycle * common.ParamAyus() - perc_traversed * common.LengthOfDasa(g);
		var di             = new DasaEntry(g, start, common.ParamAyus(), 0, string.Empty);
		return _AntarDasa(di);
	}

	public void RecalculateOptions()
	{
	}
}