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
using Mhora.Components.Property;
using Mhora.Elements;

namespace Mhora.Dasas.YearlyDasa;

public class TattwaDasa : Dasa, IDasa
{
	private readonly Horoscope _h;

	public TattwaDasa(Horoscope h)
	{
		_h = h;
	}

	public double ParamAyus() => 1.0 / 24.0 / 60.0;

	public void RecalculateOptions()
	{
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		var al = new List<DasaEntry> ();

		//var dayLength = _h.NextSunrise + 24.0 - _h.Sunrise;
		//var daySr     = Math.Floor(_h.Info.Jd)  + _h.Sunrise / 24.0;

		return al;
	}

	public List<DasaEntry> AntarDasa(DasaEntry pdi) => [];

	public string Description() => "Tattwa Dasa";

	public object GetOptions() => new();

	public object SetOptions(object o) => o;

	public class UserOptions
	{
		public enum Tattwa
		{
			Bhoomi,
			Jala,
			Agni,
			Vayu,
			Akasha
		}

		private Tattwa _startTattwa;

		[PGDisplayName("Seed Tattwa")]
		public Tattwa StartTattwa
		{
			get => _startTattwa;
			set => _startTattwa = value;
		}
	}
}