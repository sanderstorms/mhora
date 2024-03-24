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
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Dasas.YearlyDasa;

public class YogaPraveshDasa : Dasa, IDasa
{
	private Horoscope _h;

	public YogaPraveshDasa(Horoscope h)
	{
		_h = h;
	}

	public object GetOptions()
	{
		return new object();
	}

	public object SetOptions(object a)
	{
		return new object();
	}

	public void RecalculateOptions()
	{
	}

	public double ParamAyus()
	{
		return 60.0;
	}

	public List<DasaEntry> Dasa(int cycle)
	{
		var al         = new List<DasaEntry>();
		var cycleStart = cycle * ParamAyus();
		for (var i = 0; i < 60; i++)
		{
			var start = cycleStart + i;
			var di    = new DasaEntry(Body.Other, start, 1.0, 1, "Yoga Pravesh Year");
			al.Add(di);
		}

		return al;
	}

	public List<DasaEntry> AntarDasa(DasaEntry pdi)
	{
		string[] desc =
		{
			"  Month: ",
			"    Yoga: "
		};
		if (pdi.Level == 3)
		{
			return new List<DasaEntry>();
		}

		TimeOffset start  = TimeOffset.Zero; 
		TimeOffset length = 0.0;
		var        level  = 0;

		List<DasaEntry> al = new ();
		start = pdi.Start;
		level = pdi.Level + 1;

		switch (pdi.Level)
		{
			case 1:
				length = pdi.DasaLength / 15.0;
				//Mhora.Log.Debug("AD length is {0}", length);
				for (var i = 0; i < 15; i++)
				{
					var di = new DasaEntry(Body.Other, start, length, level, desc[level - 2]);
					al.Add(di);
					start += length;
				}

				return al;
			case 2:
				length = pdi.DasaLength / 27.0;
				//Mhora.Log.Debug("PD length is {0}", length);
				for (var i = 0; i < 27; i++)
				{
					var di = new DasaEntry(Body.Other, start, length, level, desc[level - 2]);
					//Mhora.Log.Debug ("PD: Starg {0}, length {1}", start, length);
					al.Add(di);
					start += length;
				}

				return al;
		}

		return al;
	}

	public new string EntryDescription(DasaEntry pdi, DateTime start, DateTime end)
	{
		if (pdi.Level == 2)
		{
			var l  = _h.CalculateBodyLongitude(start, Body.Sun.SwephBody());
			var zh = l.ToZodiacHouse();
			return zh.ToString();
		}

		if (pdi.Level == 3)
		{
			var lSun  = _h.CalculateBodyLongitude(start, Body.Sun.SwephBody());
			var lMoon = _h.CalculateBodyLongitude(start, Body.Moon.SwephBody());
			var l     = lMoon.Add(lSun);

			// this seems wrong. Why should we need to go to the next yoga here?
			var y = l.ToSunMoonYoga().Add(2);
			return y.ToString();
		}

		return string.Empty;
	}

	public string Description()
	{
		return "Yoga Pravesh Chart Dasa";
	}
}