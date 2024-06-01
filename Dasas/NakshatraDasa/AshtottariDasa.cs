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
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

//Rahu in a quadrant/trine from lagna lord, but not in lagna	Ardra
public class AshtottariDasa : NakshatraDasa, INakshatraDasa
{
	private readonly Horoscope _h;

	public AshtottariDasa(Horoscope horoscope)
	{
		Common = this;
		_h      = horoscope;
	}

	public override object GetOptions() => new();

	public override object SetOptions(object a) => new();

	public List<DasaEntry> Dasa(int cycle) => _Dasa(_h.GetPosition(Body.Moon).Longitude, 1, cycle);

	public List<DasaEntry> AntarDasa(DasaEntry di) => _AntarDasa(di);

	public string Description() => "Ashtottari Dasa";

	public double ParamAyus() => 108.0;

	public int NumberOfDasaItems() => 8;

	public DasaEntry NextDasaLord(DasaEntry di) => new(NextDasaLordHelper(di.Graha), TimeOffset.Zero, 0, di.Level, string.Empty);

	public TimeOffset LengthOfDasa(Body plt)
	{
		switch (plt)
		{
			case Body.Sun:     return 6;
			case Body.Moon:    return 15;
			case Body.Mars:    return 8;
			case Body.Mercury: return 17;
			case Body.Saturn:  return 10;
			case Body.Jupiter: return 19;
			case Body.Rahu:    return 12;
			case Body.Venus:   return 21;
			default:           throw new IndexOutOfRangeException();
		}
	}

	public static Body NakshatraLord(Nakshatra n)
	{
		return n switch
		       {
			       Nakshatra.Aswini         => Body.Rahu,
			       Nakshatra.Bharani        => Body.Rahu,
			       Nakshatra.Krittika       => Body.Venus,
			       Nakshatra.Rohini         => Body.Venus,
			       Nakshatra.Mrigarirsa     => Body.Venus,
			       Nakshatra.Aridra         => Body.Sun,
			       Nakshatra.Punarvasu      => Body.Sun,
			       Nakshatra.Pushya         => Body.Sun,
			       Nakshatra.Aslesha        => Body.Sun,
			       Nakshatra.Makha          => Body.Moon,
			       Nakshatra.PoorvaPhalguni => Body.Moon,
			       Nakshatra.UttaraPhalguni => Body.Moon,
			       Nakshatra.Hasta          => Body.Mars,
			       Nakshatra.Chittra        => Body.Mars,
			       Nakshatra.Swati          => Body.Mars,
			       Nakshatra.Vishaka        => Body.Mars,
			       Nakshatra.Anuradha       => Body.Mercury,
			       Nakshatra.Jyestha        => Body.Mercury,
			       Nakshatra.Moola          => Body.Mercury,
			       Nakshatra.PoorvaShada    => Body.Saturn,
			       Nakshatra.UttaraShada    => Body.Saturn,
			       Nakshatra.Sravana        => Body.Saturn,
			       Nakshatra.Dhanishta      => Body.Jupiter,
			       Nakshatra.Satabisha      => Body.Jupiter,
			       Nakshatra.PoorvaBhadra   => Body.Jupiter,
			       Nakshatra.UttaraBhadra   => Body.Rahu,
			       Nakshatra.Revati         => Body.Rahu,
			       _                        => throw new IndexOutOfRangeException()
		       };
	}

	public Body LordOfNakshatra(Nakshatra n) => NakshatraLord(n);

	private Body NextDasaLordHelper(Body b)
	{
		return b switch
	       {
		       Body.Sun     => Body.Moon,
		       Body.Moon    => Body.Mars,
		       Body.Mars    => Body.Mercury,
		       Body.Mercury => Body.Saturn,
		       Body.Saturn  => Body.Jupiter,
		       Body.Jupiter => Body.Rahu,
		       Body.Rahu    => Body.Venus,
		       Body.Venus   => Body.Sun,
		       _            => throw new ArgumentOutOfRangeException(nameof(b), b, null)
	       };
	}
}