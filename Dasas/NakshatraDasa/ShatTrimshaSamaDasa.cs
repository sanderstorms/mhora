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
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

//Lagna in Sun's hora in daytime or Lagna in Moon's hora in night time~
public class ShatTrimshaSamaDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public ShatTrimshaSamaDasa(Horoscope h)
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

	public ArrayList Dasa(int cycle)
	{
		return _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "ShatTrimsha Sama Dasa";
	}

	public double ParamAyus()
	{
		return 36.0;
	}

	public int NumberOfDasaItems()
	{
		return 8;
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return new DasaEntry(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);
	}

	public TimeOffset LengthOfDasa(Body plt)
	{
		switch (plt)
		{
			case Body.Moon:    return 1;
			case Body.Sun:     return 2;
			case Body.Jupiter: return 3;
			case Body.Mars:    return 4;
			case Body.Mercury: return 5;
			case Body.Saturn:  return 6;
			case Body.Venus:   return 7;
			case Body.Rahu:    return 8;
		}

		Trace.Assert(false, "ShatTrimshaSamaDasa::LengthOfDasa");
		return 0;
	}

	public Body LordOfNakshatra(Nakshatra n)
	{
		var lords = new Body[8]
		{
			Body.Moon,
			Body.Sun,
			Body.Jupiter,
			Body.Mars,
			Body.Mercury,
			Body.Saturn,
			Body.Venus,
			Body.Rahu
		};
		var nakVal  = (int) n;
		var shrVal  = (int) Nakshatra.Sravana;
		var diffVal = (nakVal - shrVal).NormalizeInc((int) Nakshatra.Aswini, (int) Nakshatra.Revati);
		var diffOff = diffVal % 8;
		return lords[diffOff];
	}

	private Body NextDasaLordHelper(Body b)
	{
		switch (b)
		{
			case Body.Moon:    return Body.Sun;
			case Body.Sun:     return Body.Jupiter;
			case Body.Jupiter: return Body.Mars;
			case Body.Mars:    return Body.Mercury;
			case Body.Mercury: return Body.Saturn;
			case Body.Saturn:  return Body.Venus;
			case Body.Venus:   return Body.Rahu;
			case Body.Rahu:    return Body.Moon;
		}

		Trace.Assert(false, "ShatTrimshaSamaDasa::NextDasaLord");
		return Body.Lagna;
	}
}