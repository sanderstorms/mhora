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


namespace Mhora.Elements;

public static class Karanas
{
	public static Karana Add(this Karana value, int i)
	{
		var tnum = ((int) value + i - 1).NormalizeInc(1, 60);
		return (Karana) tnum;
	}

	public static Karana AddReverse(this Karana value, int i)
	{
		var tnum = ((int) value - i + 1).NormalizeInc(1, 60);
		return (Karana) tnum;
	}

	public static Body.BodyType GetLord(this Karana value)
	{
		switch (value)
		{
			case Karana.Kimstughna:  return Body.BodyType.Moon;
			case Karana.Sakuna:      return Body.BodyType.Mars;
			case Karana.Chatushpada: return Body.BodyType.Sun;
			case Karana.Naga:        return Body.BodyType.Venus;
			default:
			{
				var vn = ((int) value - 1).NormalizeInc(1, 7);
				switch (vn)
				{
					case 1:  return Body.BodyType.Sun;
					case 2:  return Body.BodyType.Moon;
					case 3:  return Body.BodyType.Mars;
					case 4:  return Body.BodyType.Mercury;
					case 5:  return Body.BodyType.Jupiter;
					case 6:  return Body.BodyType.Venus;
					default: return Body.BodyType.Saturn;
				}
			}
		}
	}
}