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
using Mhora.Components.Delegates;
using Mhora.Elements.Calculation;

namespace Mhora.Elements.Dasas.Nakshatra;

// Wrapper around vimsottari dasa that starts the initial dasa
// based on the yoga
public class YogaVimsottariDasa : NakshatraDasa, INakshatraDasa, INakshatraYogaDasa
{
	private readonly Horoscope      h;
	private readonly VimsottariDasa vd;
	private          UserOptions    options;

	public YogaVimsottariDasa(Horoscope _h)
	{
		options    = new UserOptions();
		common     = this;
		yogaCommon = this;
		h          = _h;
		vd         = new VimsottariDasa(h);
	}

	public override object GetOptions()
	{
		return options.Clone();
	}

	public override object SetOptions(object a)
	{
		options = (UserOptions) options.SetOptions(a);
		RecalculateEvent?.Invoke();

		return options.Clone();
	}

	public ArrayList Dasa(int cycle)
	{
		var t = new Transit(h);
		var l = t.LongitudeOfSunMoonYoga(h.Info.Jd);
		return _YogaDasa(l, 1, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Yoga Vimsottari Dasa";
	}

	public double ParamAyus()
	{
		return vd.ParamAyus();
	}

	public int NumberOfDasaItems()
	{
		return vd.NumberOfDasaItems();
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return vd.NextDasaLord(di);
	}

	public double LengthOfDasa(Body.BodyType plt)
	{
		return vd.LengthOfDasa(plt);
	}

	public Body.BodyType LordOfNakshatra(Nakshatras.Nakshatra n)
	{
		throw new Exception();
		return Body.BodyType.Lagna;
	}

	public Body.BodyType lordOfYoga(Longitude l)
	{
		return l.ToSunMoonYoga().getLord();
	}

	public class UserOptions : ICloneable
	{
		public bool bExpungeTravelled = true;

		public UserOptions()
		{
			bExpungeTravelled = true;
		}

		[PGNotVisible]
		public bool UseYogaRemainder
		{
			get => bExpungeTravelled;
			set => bExpungeTravelled = value;
		}

		public object Clone()
		{
			var options = new UserOptions();
			options.bExpungeTravelled = bExpungeTravelled;
			return options;
		}

		public object SetOptions(object b)
		{
			if (b is UserOptions)
			{
				var uo = (UserOptions) b;
				bExpungeTravelled = uo.bExpungeTravelled;
			}

			return Clone();
		}
	}
}