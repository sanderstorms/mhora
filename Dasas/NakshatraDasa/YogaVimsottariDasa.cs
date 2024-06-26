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
using Mhora.Calculation;
using Mhora.Components.Delegates;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

// Wrapper around vimsottari dasa that starts the initial dasa
// based on the yoga
public class YogaVimsottariDasa : NakshatraDasa, INakshatraDasa, INakshatraYogaDasa
{
	private readonly Horoscope      _h;
	private readonly VimsottariDasa _vd;
	private          UserOptions    _options;

	public YogaVimsottariDasa(Horoscope h)
	{
		_options    = new UserOptions();
		Common     = this;
		YogaCommon = this;
		_h    = h;
		_vd         = new VimsottariDasa(_h);
	}

	public override object GetOptions() => _options.Clone();

	public override object SetOptions(object a)
	{
		_options = (UserOptions) _options.SetOptions(a);
		RecalculateEvent?.Invoke();

		return _options.Clone();
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		var grahas = _h.FindGrahas(DivisionType.Rasi);
		var l      = grahas.Calc(_h.Info.Jd, Body.Moon, Body.Sun, false);
		return _YogaDasa(l, 1, cycle);
	}

	public List<DasaEntry> AntarDasa(DasaEntry di) => _AntarDasa(di);

	public string Description() => "Yoga Vimsottari Dasa";

	public double ParamAyus() => _vd.ParamAyus();

	public int NumberOfDasaItems() => _vd.NumberOfDasaItems();

	public DasaEntry NextDasaLord(DasaEntry di) => _vd.NextDasaLord(di);

	public TimeOffset LengthOfDasa(Body plt) => _vd.LengthOfDasa(plt);

	public Body LordOfNakshatra(Nakshatra n)
	{
		throw new Exception();
		return Body.Lagna;
	}

	public Body LordOfYoga(Longitude l) => l.ToSunMoonYoga().Lord();

	public class UserOptions : ICloneable
	{
		public bool BExpungeTravelled = true;

		public UserOptions()
		{
			BExpungeTravelled = true;
		}

		[PGNotVisible]
		public bool UseYogaRemainder
		{
			get => BExpungeTravelled;
			set => BExpungeTravelled = value;
		}

		public object Clone()
		{
			var options = new UserOptions
			{
				BExpungeTravelled = BExpungeTravelled
			};
			return options;
		}

		public object SetOptions(object b)
		{
			if (b is UserOptions)
			{
				var uo = (UserOptions) b;
				BExpungeTravelled = uo.BExpungeTravelled;
			}

			return Clone();
		}
	}
}