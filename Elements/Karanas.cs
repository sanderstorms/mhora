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


using Mhora.Tables;

namespace Mhora.Elements;

public static class Karanas
{
	public enum Karana
	{
		Kimstughna = 1,
		Bava1,
		Balava1,
		Kaulava1,
		Taitula1,
		Garija1,
		Vanija1,
		Vishti1,
		Bava2,
		Balava2,
		Kaulava2,
		Taitula2,
		Garija2,
		Vanija2,
		Vishti2,
		Bava3,
		Balava3,
		Kaulava3,
		Taitula3,
		Garija3,
		Vanija3,
		Vishti3,
		Bava4,
		Balava4,
		Kaulava4,
		Taitula4,
		Garija4,
		Vanija4,
		Vishti4,
		Bava5,
		Balava5,
		Kaulava5,
		Taitula5,
		Garija5,
		Vanija5,
		Vishti5,
		Bava6,
		Balava6,
		Kaulava6,
		Taitula6,
		Garija6,
		Vanija6,
		Vishti6,
		Bava7,
		Balava7,
		Kaulava7,
		Taitula7,
		Garija7,
		Vanija7,
		Vishti7,
		Bava8,
		Balava8,
		Kaulava8,
		Taitula8,
		Garija8,
		Vanija8,
		Vishti8,
		Sakuna,
		Chatushpada,
		Naga
	}

	public static Karana add(this Karana value, int i)
	{
		var tnum = Basics.normalize_inc(1, 60, (int) value + i - 1);
		return (Karana) tnum;
	}

	public static Karana addReverse(this Karana value, int i)
	{
		var tnum = Basics.normalize_inc(1, 60, (int) value - i + 1);
		return (Karana) tnum;
	}

	public static Body.BodyType getLord(this Karana value)
	{
		switch (value)
		{
			case Karana.Kimstughna:  return Body.BodyType.Moon;
			case Karana.Sakuna:      return Body.BodyType.Mars;
			case Karana.Chatushpada: return Body.BodyType.Sun;
			case Karana.Naga:        return Body.BodyType.Venus;
			default:
			{
				var vn = Basics.normalize_inc(1, 7, (int) value - 1);
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