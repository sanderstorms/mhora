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
using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Dasas.GrahaDasa;

public class NaisargikaGrahaDasaSp : Dasa, IDasa
{
	private readonly UserOptions _options;
	private          Horoscope   _h;

	public NaisargikaGrahaDasaSp(Horoscope h)
	{
		_h       = h;
		_options = new UserOptions();
	}

	public double ParamAyus()
	{
		return 108.0;
	}

	public void RecalculateOptions()
	{
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		var al = new List<DasaEntry> ();
		Body[] order =
		[
			Body.Moon,
			Body.Mercury,
			Body.Mars,
			Body.Venus,
			Body.Jupiter,
			Body.Sun,
			Body.Ketu,
			Body.Rahu,
			Body.Saturn
		];

		var cycleStart = ParamAyus() * cycle;
		var curr        = 0.0;
		for (var i = 0; i < 3; i++)
		{
			foreach (var bn in order)
			{
				al.Add(new DasaEntry(bn, cycleStart + curr, 4.0, 1, bn.ToString()));
				curr += 4.0;
			}
		}

		return al;
	}

	public List<DasaEntry> AntarDasa(DasaEntry pdi)
	{
		return [];
	}

	public string Description()
	{
		return "Naisargika Graha Dasa (SP)";
	}

	public object GetOptions()
	{
		return _options.Clone();
	}

	public object SetOptions(object a)
	{
		var uo = (UserOptions) a;
		RecalculateEvent?.Invoke();

		return _options.Clone();
	}

	public class UserOptions : ICloneable
	{
		public object Clone()
		{
			var uo = new UserOptions();
			return uo;
		}
	}
}