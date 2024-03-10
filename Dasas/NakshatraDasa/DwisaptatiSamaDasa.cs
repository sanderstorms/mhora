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
using System.Collections.Generic;
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

//sama dasa	Lagna lord in 7th or 7th lord in lagna
public class DwisaptatiSamaDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public DwisaptatiSamaDasa(Horoscope h)
	{
		Common  = this;
		_h = h;
	}

	public override object GetOptions()
	{
		return new object();
	}

	public override object SetOptions(object a)
	{
		return new object();
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		return _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);
	}

	public List<DasaEntry> AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Dwisaptati Sama Dasa";
	}

	public double ParamAyus()
	{
		return 72.0;
	}

	public int NumberOfDasaItems()
	{
		return 8;
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return new DasaEntry(NextDasaLordHelper(di.Graha), TimeOffset.Zero, 0, di.Level, string.Empty);
	}

	public TimeOffset LengthOfDasa(Body plt)
	{
		switch (plt)
		{
			case Body.Sun:     return 9;
			case Body.Moon:    return 9;
			case Body.Mars:    return 9;
			case Body.Mercury: return 9;
			case Body.Jupiter: return 9;
			case Body.Venus:   return 9;
			case Body.Saturn:  return 9;
			case Body.Rahu:    return 9;
		}

		Trace.Assert(false, "DwisaptatiSamaDasa::LengthOfDasa");
		return 0;
	}

	public Body LordOfNakshatra(Nakshatra n)
	{
		var lords = new Body[8]
		{
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Jupiter,
			Body.Venus,
			Body.Saturn,
			Body.Rahu
		};
		var nakVal  = (int) n;
		var mooVal  = (int) Nakshatra.Moola;
		var diffVal = (nakVal - mooVal).NormalizeInc((int) Nakshatra.Aswini, (int) Nakshatra.Revati);
		var diffOff = diffVal % 8;
		return lords[diffOff];
	}

	private Body NextDasaLordHelper(Body b)
	{
		switch (b)
		{
			case Body.Sun:     return Body.Moon;
			case Body.Moon:    return Body.Mars;
			case Body.Mars:    return Body.Mercury;
			case Body.Mercury: return Body.Jupiter;
			case Body.Jupiter: return Body.Venus;
			case Body.Venus:   return Body.Saturn;
			case Body.Saturn:  return Body.Rahu;
			case Body.Rahu:    return Body.Sun;
		}

		Trace.Assert(false, "DwisaptatiSamaDasa::NextDasaLord");
		return Body.Lagna;
	}
}