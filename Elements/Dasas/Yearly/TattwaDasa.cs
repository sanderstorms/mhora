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
using Mhora.Components.Property;
using Mhora.Elements.Calculation;

namespace Mhora.Elements.Dasas.Yearly;

public class TattwaDasa : Dasa, IDasa
{
	private readonly Horoscope h;

	public TattwaDasa(Horoscope _h)
	{
		h = _h;
	}

	public double ParamAyus()
	{
		return 1.0 / 24.0 / 60.0;
	}

	public void RecalculateOptions()
	{
	}

	public ArrayList Dasa(int cycle)
	{
		var al = new ArrayList();

		var day_length = h.NextSunrise + 24.0 - h.Sunrise;
		var day_sr     = Math.Floor(h.Info.Jd)  + h.Sunrise / 24.0;

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		return new ArrayList();
	}

	public string Description()
	{
		return "Tattwa Dasa";
	}

	public object GetOptions()
	{
		return new object();
	}

	public object SetOptions(object o)
	{
		return o;
	}

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

		public Tattwa _startTattwa;

		[PGDisplayName("Seed Tattwa")]
		public Tattwa StartTattwa
		{
			get => _startTattwa;
			set => _startTattwa = value;
		}
	}
}