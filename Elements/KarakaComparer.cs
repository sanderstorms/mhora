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
using System.Diagnostics;

namespace Mhora.Elements;

public class KarakaComparer : IComparable
{
	public KarakaComparer(Position bp)
	{
		GetPosition = bp;
	}

	public Position GetPosition
	{
		get;
		set;
	}

	public int CompareTo(object obj)
	{
		Debug.Assert(obj is KarakaComparer);
		var offa = GetOffset();
		var offb = ((KarakaComparer) obj).GetOffset();
		return offb.CompareTo(offa);
	}

	public double GetOffset()
	{
		var off = GetPosition.Longitude.ToZodiacHouseOffset();
		if (GetPosition.Name == Body.BodyType.Rahu)
		{
			off = 30.0 - off;
		}

		return off;
	}
}