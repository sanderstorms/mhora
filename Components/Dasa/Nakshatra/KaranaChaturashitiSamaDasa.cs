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
using Mhora.Elements;
using Mhora.Elements.Calculation;

namespace Mhora.Components.Dasa.Nakshatra;

// Wrapper around ChaturashitiSamaDasa
public class KaranaChaturashitiSamaDasa : NakshatraDasa, INakshatraDasa, INakshatraKaranaDasa
{
	private readonly ChaturashitiSamaDasa cd;
	private readonly Horoscope            h;

	public KaranaChaturashitiSamaDasa(Horoscope _h)
	{
		common       = this;
		karanaCommon = this;
		h            = _h;
		cd           = new ChaturashitiSamaDasa(h);
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
		var mMoon = h.getPosition(Elements.Body.Name.Moon).longitude;
		var mSun  = h.getPosition(Elements.Body.Name.Sun).longitude;
		return _KaranaDasa(mMoon.sub(mSun), 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Karana Chaturashiti-Sama Dasa";
	}

	public double paramAyus()
	{
		return cd.paramAyus();
	}

	public int numberOfDasaItems()
	{
		return cd.numberOfDasaItems();
	}

	public DasaEntry nextDasaLord(DasaEntry di)
	{
		return cd.nextDasaLord(di);
	}

	public double lengthOfDasa(Elements.Body.Name plt)
	{
		return cd.lengthOfDasa(plt);
	}

	public Elements.Body.Name lordOfNakshatra(Elements.Nakshatra n)
	{
		return cd.lordOfNakshatra(n);
	}

	public Elements.Body.Name lordOfKarana(Longitude l)
	{
		return l.toKarana().getLord();
	}
}