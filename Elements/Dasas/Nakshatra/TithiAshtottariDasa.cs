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
using System.Diagnostics;
using Mhora.Components.Dasa;
using Mhora.Components.Delegates;
using Mhora.Elements.Calculation;

namespace Mhora.Elements.Dasas.Nakshatra;

// Wrapper around ashtottari dasa that starts the initial dasa
// based on the tithi. We do not reimplement ashtottari dasa 
// semantics here.
public class TithiAshtottariDasa : NakshatraDasa, INakshatraDasa, INakshatraTithiDasa
{
	private readonly AshtottariDasa ad;
	private readonly Horoscope      h;
	private          UserOptions    options;

	public TithiAshtottariDasa(Horoscope _h)
	{
		options     = new UserOptions();
		tithiCommon = this;
		common      = this;
		h           = _h;
		ad          = new AshtottariDasa(h);
	}

	public override object GetOptions()
	{
		return options.Clone();
	}

	public override object SetOptions(object a)
	{
		options = (UserOptions) options.SetOptions(a);
		if (RecalculateEvent != null)
		{
			RecalculateEvent();
		}

		return options.Clone();
	}

	public ArrayList Dasa(int cycle)
	{
		var mpos = h.getPosition(Body.BodyType.Moon).longitude;
		var spos = h.getPosition(Body.BodyType.Sun).longitude;

		var tithi = mpos.sub(spos);
		if (options.UseTithiRemainder == false)
		{
			var offset = tithi.value;
			while (offset >= 12.0)
			{
				offset -= 12.0;
			}

			tithi = tithi.sub(new Longitude(offset));
		}

		return _TithiDasa(tithi, options.TithiOffset, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return string.Format("({0}) Tithis Ashtottari Dasa", options.TithiOffset);
	}

	public double paramAyus()
	{
		return ad.paramAyus();
	}

	public int numberOfDasaItems()
	{
		return ad.numberOfDasaItems();
	}

	public DasaEntry nextDasaLord(DasaEntry di)
	{
		return ad.nextDasaLord(di);
	}

	public double lengthOfDasa(Body.BodyType plt)
	{
		return ad.lengthOfDasa(plt);
	}

	public Body.BodyType lordOfNakshatra(Nakshatras.Nakshatra n)
	{
		Debug.Assert(false, "TithiAshtottari::lordOfNakshatra");
		return Body.BodyType.Sun;
	}

	public Body.BodyType lordOfTithi(Longitude l)
	{
		return l.toTithi().GetLord();
	}

	public class UserOptions : ICloneable
	{
		public bool bExpungeTravelled = true;
		public int  mTithiOffset      = 1;

		public UserOptions()
		{
			mTithiOffset      = 1;
			bExpungeTravelled = true;
		}

		[PGNotVisible]
		public bool UseTithiRemainder
		{
			get => bExpungeTravelled;
			set => bExpungeTravelled = value;
		}

		public int TithiOffset
		{
			get => mTithiOffset;
			set
			{
				if (value >= 1 && value <= 30)
				{
					mTithiOffset = value;
				}
			}
		}

		public object Clone()
		{
			var options = new UserOptions();
			options.mTithiOffset      = mTithiOffset;
			options.bExpungeTravelled = bExpungeTravelled;
			return options;
		}

		public object SetOptions(object b)
		{
			if (b is UserOptions)
			{
				var uo = (UserOptions) b;
				mTithiOffset      = uo.mTithiOffset;
				bExpungeTravelled = uo.bExpungeTravelled;
			}

			return Clone();
		}
	}
}