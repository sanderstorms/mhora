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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;


//Lagna in Sun's hora in Sukla paksha or Lagna in Moon's hora in Krishna paksha
public class ShodashottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public ShodashottariDasa(Horoscope h)
	{
		Common  = this;
		_h = h;
	}

	public override object GetOptions() => new();

	public override object SetOptions(object a) => new();

	public List<DasaEntry> Dasa(int cycle) => _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);

	public List<DasaEntry> AntarDasa(DasaEntry di) => _AntarDasa(di);

	public string Description() => "Shodashottari Dasa";

	public double ParamAyus() => 116.0;

	public int NumberOfDasaItems() => 8;

	public DasaEntry NextDasaLord(DasaEntry di) => new(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);

	public TimeOffset LengthOfDasa(Body plt)
	{
		return plt switch
	       {
		       Body.Sun     => 11,
		       Body.Mars    => 12,
		       Body.Jupiter => 13,
		       Body.Saturn  => 14,
		       Body.Ketu    => 15,
		       Body.Moon    => 16,
		       Body.Mercury => 17,
		       Body.Venus   => 18,
		       _            => throw new ArgumentOutOfRangeException(nameof(plt), plt, null)
	       };
	}

	public static Body NakshatraLord(Nakshatra n)
	{
		var lords = new Body[8]
		{
			Body.Sun,
			Body.Mars,
			Body.Jupiter,
			Body.Saturn,
			Body.Ketu,
			Body.Moon,
			Body.Mercury,
			Body.Venus
		};
		var nakVal  = (int) n;
		var pusVal  = (int) Nakshatra.Pushya;
		var diffVal = (nakVal - pusVal).NormalizeInc((int) Nakshatra.Aswini, (int) Nakshatra.Revati);
		var diffOff = diffVal % 8;
		return lords[diffOff];
	}

	public Body LordOfNakshatra(Nakshatra n) => (NakshatraLord(n));

	private Body NextDasaLordHelper(Body b)
	{
		return b switch
	       {
		       Body.Sun     => Body.Mars,
		       Body.Mars    => Body.Jupiter,
		       Body.Jupiter => Body.Saturn,
		       Body.Saturn  => Body.Ketu,
		       Body.Ketu    => Body.Moon,
		       Body.Moon    => Body.Mercury,
		       Body.Mercury => Body.Venus,
		       Body.Venus   => Body.Sun,
		       _            => throw new ArgumentOutOfRangeException(nameof(b), b, null)
	       };
	}
}