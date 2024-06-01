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

//sama dasa	Lagna lord in 7th or 7th lord in lagna
public class DwisaptatiSamaDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public DwisaptatiSamaDasa(Horoscope h)
	{
		Common  = this;
		_h = h;
	}

	public override object GetOptions() => new();

	public override object SetOptions(object a) => new();

	public List<DasaEntry> Dasa(int cycle) => _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);

	public List<DasaEntry> AntarDasa(DasaEntry di) => _AntarDasa(di);

	public string Description() => "Dwisaptati Sama Dasa";

	public double ParamAyus() => 72.0;

	public int NumberOfDasaItems() => 8;

	public DasaEntry NextDasaLord(DasaEntry di) => new(NextDasaLordHelper(di.Graha), TimeOffset.Zero, 0, di.Level, string.Empty);

	public TimeOffset LengthOfDasa(Body plt)
	{
		return plt switch
	       {
		       Body.Sun     => 9,
		       Body.Moon    => 9,
		       Body.Mars    => 9,
		       Body.Mercury => 9,
		       Body.Jupiter => 9,
		       Body.Venus   => 9,
		       Body.Saturn  => 9,
		       Body.Rahu    => 9,
		       _            => throw new ArgumentOutOfRangeException(nameof(plt), plt, null)
	       };
	}

	public static Body NakshatraLord(Nakshatra n)
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

	public Body LordOfNakshatra(Nakshatra n) => NakshatraLord(n);

	private Body NextDasaLordHelper(Body b)
	{
		return b switch
	       {
		       Body.Sun     => Body.Moon,
		       Body.Moon    => Body.Mars,
		       Body.Mars    => Body.Mercury,
		       Body.Mercury => Body.Jupiter,
		       Body.Jupiter => Body.Venus,
		       Body.Venus   => Body.Saturn,
		       Body.Saturn  => Body.Rahu,
		       Body.Rahu    => Body.Sun,
		       _            => throw new ArgumentOutOfRangeException(nameof(b), b, null)
	       };
	}
}