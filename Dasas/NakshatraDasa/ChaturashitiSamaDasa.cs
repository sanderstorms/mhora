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

//The 10th lord in 10th
public class ChaturashitiSamaDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public ChaturashitiSamaDasa(Horoscope h)
	{
		Common  = this;
		_h = h;
	}

	public override object GetOptions() => new();

	public override object SetOptions(object a) => new();

	public List<DasaEntry> Dasa(int cycle) => _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);

	public List<DasaEntry> AntarDasa(DasaEntry di) => _AntarDasa(di);

	public string Description() => "Chaturashiti-Sama Dasa";

	public double ParamAyus() => 84.0;

	public int NumberOfDasaItems() => 7;

	public DasaEntry NextDasaLord(DasaEntry di) => new(NextDasaLordHelper(di.Graha), TimeOffset.Zero, 0, di.Level, string.Empty);

	public TimeOffset LengthOfDasa(Body plt)
	{
		return plt switch
		       {
			       Body.Sun     => 12,
			       Body.Moon    => 12,
			       Body.Mars    => 12,
			       Body.Mercury => 12,
			       Body.Jupiter => 12,
			       Body.Venus   => 12,
			       Body.Saturn  => 12,
			       _            => throw new IndexOutOfRangeException()
		       };
	}

	public static Body NakshatraLord(Nakshatra n)
	{
		var lords = new Body[7]
		{
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Jupiter,
			Body.Venus,
			Body.Saturn
		};
		var nakVal  = (int) n;
		var svaVal  = (int) Nakshatra.Swati;
		var diffVal = (nakVal - svaVal).NormalizeInc((int) Nakshatra.Aswini, (int) Nakshatra.Revati);
		var diffOff = diffVal % 7;
		return lords[diffOff];
	}

	public Body LordOfNakshatra(Nakshatra n) => (NakshatraLord(n));

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
		       Body.Saturn  => Body.Sun,
		       _            => throw new ArgumentOutOfRangeException(nameof(b), b, null)
	       };
	}
}