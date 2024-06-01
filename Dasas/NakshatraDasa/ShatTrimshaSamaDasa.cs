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

//Lagna in Sun's hora in daytime or Lagna in Moon's hora in night time~
public class ShatTrimshaSamaDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public ShatTrimshaSamaDasa(Horoscope h)
	{
		Common  = this;
		_h = h;
	}

	public override object GetOptions() => new();

	public override object SetOptions(object a) => new();

	public List<DasaEntry> Dasa(int cycle) => _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);

	public List<DasaEntry> AntarDasa(DasaEntry di) => _AntarDasa(di);

	public string Description() => "ShatTrimsha Sama Dasa";

	public double ParamAyus() => 36.0;

	public int NumberOfDasaItems() => 8;

	public DasaEntry NextDasaLord(DasaEntry di) => new(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);

	public TimeOffset LengthOfDasa(Body plt)
	{
		return plt switch
	       {
		       Body.Moon    => 1,
		       Body.Sun     => 2,
		       Body.Jupiter => 3,
		       Body.Mars    => 4,
		       Body.Mercury => 5,
		       Body.Saturn  => 6,
		       Body.Venus   => 7,
		       Body.Rahu    => 8,
		       _            => throw new ArgumentOutOfRangeException(nameof(plt), plt, null)
	       };
	}

	public static Body NakshatraLord(Nakshatra n)
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

	public Body LordOfNakshatra(Nakshatra n) => NakshatraLord(n);

	private Body NextDasaLordHelper(Body b)
	{
		return b switch
	       {
		       Body.Moon    => Body.Sun,
		       Body.Sun     => Body.Jupiter,
		       Body.Jupiter => Body.Mars,
		       Body.Mars    => Body.Mercury,
		       Body.Mercury => Body.Saturn,
		       Body.Saturn  => Body.Venus,
		       Body.Venus   => Body.Rahu,
		       Body.Rahu    => Body.Moon,
		       _            => throw new ArgumentOutOfRangeException(nameof(b), b, null)
	       };
	}
}