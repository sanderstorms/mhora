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
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

// Wrapper around ChaturashitiSamaDasa
public class KaranaChaturashitiSamaDasa : NakshatraDasa, INakshatraDasa, INakshatraKaranaDasa
{
	private readonly ChaturashitiSamaDasa _cd;
	private readonly Horoscope            _h;

	public KaranaChaturashitiSamaDasa(Horoscope h)
	{
		Common       = this;
		KaranaCommon = this;
		_h      = h;
		_cd          = new ChaturashitiSamaDasa(_h);
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
		var mMoon = _h.GetPosition(Body.Moon).Longitude;
		var mSun  = _h.GetPosition(Body.Sun).Longitude;
		return _KaranaDasa(mMoon.Sub(mSun), 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Karana Chaturashiti-Sama Dasa";
	}

	public double ParamAyus()
	{
		return _cd.ParamAyus();
	}

	public int NumberOfDasaItems()
	{
		return _cd.NumberOfDasaItems();
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return _cd.NextDasaLord(di);
	}

	public TimeOffset LengthOfDasa(Body plt)
	{
		return _cd.LengthOfDasa(plt);
	}

	public Body LordOfNakshatra(Nakshatra n)
	{
		return _cd.LordOfNakshatra(n);
	}

	public Body LordOfKarana(Longitude l)
	{
		return l.ToKarana().GetLord();
	}
}