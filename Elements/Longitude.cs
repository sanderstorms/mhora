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
using System.ComponentModel;
using System.Diagnostics;
using Mhora.Components.Converter;

namespace Mhora.Elements;

[Serializable]
[TypeConverter(typeof(LongitudeConverter))]
public class Longitude
{
	private double _lon;

	public Longitude(double lon)
	{
		while (lon > 360.0)
		{
			lon -= 360.0;
		}

		while (lon < 0)
		{
			lon += 360.0;
		}

		_lon = lon;
		//m_lon = Basics.NormalizeExc (0, 360, lon);
	}

	public double Value
	{
		get => _lon;
		set
		{
			Trace.Assert(value >= 0 && value <= 360);
			_lon = value;
		}
	}

	public override string ToString()
	{
		var lon     = this;
		var rasi    = lon.ToZodiacHouse().Sign.ToString();
		var offset  = lon.ToZodiacHouseOffset();
		var minutes = Math.Floor(offset);
		offset = (offset - minutes) * 60.0;
		var seconds = Math.Floor(offset);
		offset = (offset - seconds) * 60.0;
		var subsecs = Math.Floor(offset);
		offset = (offset - subsecs) * 60.0;
		var subsubsecs = Math.Floor(offset);

		return string.Format("{0:00} {1} {2:00}:{3:00}:{4:00}", minutes, rasi, seconds, subsecs, subsubsecs);
	}
}