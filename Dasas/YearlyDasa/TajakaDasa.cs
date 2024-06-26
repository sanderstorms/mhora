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
using System.Collections.Generic;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Dasas.YearlyDasa;

public class TajakaDasa : Dasa, IDasa
{
	private Horoscope _h;

	public TajakaDasa(Horoscope h)
	{
		_h = h;
	}

	public object GetOptions() => new();

	public object SetOptions(object a) => new();

	public void RecalculateOptions()
	{
	}

	public double ParamAyus() => 60.0;

	public List<DasaEntry> Dasa(int cycle)
	{
		var al         = new List<DasaEntry> ();
		var cycleStart = cycle * ParamAyus();
		for (var i = 0; i < 60; i++)
		{
			var start = cycleStart + i;
			var di    = new DasaEntry(Body.Other, start, 1.0, 1, "Tajaka Year");
			al.Add(di);
		}

		return al;
	}

	public List<DasaEntry> AntarDasa(DasaEntry pdi)
	{
		string[] desc =
		[
			"  Tajaka Month",
			"    Tajaka 60 hour",
			"      Tajaka 5 hour",
			"        Tajaka 25 minute",
			"          Tajaka 2 minute"
		];
		if (pdi.Level == 6)
		{
			return [];
		}

		TimeOffset start  = 0.0;
		TimeOffset length = 0.0;
		var        level  = 0;

		var al = new List<DasaEntry> ();
		start  = pdi.Start;
		level  = pdi.Level + 1;
		length = pdi.DasaLength / 12.0;
		for (var i = 0; i < 12; i++)
		{
			var di = new DasaEntry(Body.Other, start, length, level, desc[level - 2]);
			al.Add(di);
			start += length;
		}

		return al;
	}

	public string Description() => "Tajaka Chart Dasa";
}