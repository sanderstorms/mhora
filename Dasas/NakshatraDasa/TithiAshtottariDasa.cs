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
using Mhora.Calculation;
using Mhora.Components.Delegates;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

// Wrapper around ashtottari dasa that starts the initial dasa
// based on the tithi. We do not reimplement ashtottari dasa 
// semantics here.
public class TithiAshtottariDasa : NakshatraDasa, INakshatraDasa, INakshatraTithiDasa
{
	private readonly AshtottariDasa _ad;
	private readonly Horoscope      _h;
	private          UserOptions    _options;

	public TithiAshtottariDasa(Horoscope h)
	{
		_options     = new UserOptions();
		TithiCommon = this;
		Common      = this;
		_h     = h;
		_ad         = new AshtottariDasa(_h);
	}

	public override object GetOptions()
	{
		return _options.Clone();
	}

	public override object SetOptions(object a)
	{
		_options = (UserOptions) _options.SetOptions(a);
		RecalculateEvent?.Invoke();

		return _options.Clone();
	}

	public ArrayList Dasa(int cycle)
	{
		var mpos = _h.GetPosition(Body.Moon).Longitude;
		var spos = _h.GetPosition(Body.Sun).Longitude;

		var tithi = mpos.Sub(spos);
		if (_options.UseTithiRemainder == false)
		{
			var offset = tithi.Value;
			while (offset >= 12.0)
			{
				offset -= 12.0;
			}

			tithi = tithi.Sub(new Longitude(offset));
		}

		return _TithiDasa(tithi, _options.TithiOffset, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return string.Format("({0}) Tithis Ashtottari Dasa", _options.TithiOffset);
	}

	public double ParamAyus()
	{
		return _ad.ParamAyus();
	}

	public int NumberOfDasaItems()
	{
		return _ad.NumberOfDasaItems();
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return _ad.NextDasaLord(di);
	}

	public TimeOffset LengthOfDasa(Body plt)
	{
		return _ad.LengthOfDasa(plt);
	}

	public Body LordOfNakshatra(Nakshatra n)
	{
		Debug.Assert(false, "TithiAshtottari::lordOfNakshatra");
		return Body.Sun;
	}

	public Body LordOfTithi(Longitude l)
	{
		return l.ToTithi().GetLord();
	}

	public class UserOptions : ICloneable
	{
		public bool BExpungeTravelled = true;
		public int  MTithiOffset      = 1;

		public UserOptions()
		{
			MTithiOffset      = 1;
			BExpungeTravelled = true;
		}

		[PGNotVisible]
		public bool UseTithiRemainder
		{
			get => BExpungeTravelled;
			set => BExpungeTravelled = value;
		}

		public int TithiOffset
		{
			get => MTithiOffset;
			set
			{
				if (value >= 1 && value <= 30)
				{
					MTithiOffset = value;
				}
			}
		}

		public object Clone()
		{
			var options = new UserOptions
			{
				MTithiOffset = MTithiOffset,
				BExpungeTravelled = BExpungeTravelled
			};
			return options;
		}

		public object SetOptions(object b)
		{
			if (b is UserOptions)
			{
				var uo = (UserOptions) b;
				MTithiOffset      = uo.MTithiOffset;
				BExpungeTravelled = uo.BExpungeTravelled;
			}

			return Clone();
		}
	}
}