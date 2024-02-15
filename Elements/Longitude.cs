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
using Mhora.Components.Converter;
using Mhora.Util;

namespace Mhora.Elements;

[Serializable]
[TypeConverter(typeof(LongitudeConverter))]
public class Longitude : DmsPoint
{
	public Longitude(double lon) : base(lon)
	{
	}

	public Longitude(decimal lon) : base((double) lon)
	{

	}

	public static implicit operator Longitude (double lon) => new Longitude(lon);

	public override string ToString()
	{
		var lon     = this;
		var rasi    = lon.ToZodiacHouse().ToString();
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