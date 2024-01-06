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


namespace Mhora.Calculation;

public class Karana
{
	public enum Name
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

	public Karana(Name _mValue)
	{
		value = (Name) Basics.normalize_inc(1, 60, (int) _mValue);
	}

	public Name value
	{
		get;
		set;
	}

	public Karana add(int i)
	{
		var tnum = Basics.normalize_inc(1, 60, (int) value + i - 1);
		return new Karana((Name) tnum);
	}

	public Karana addReverse(int i)
	{
		var tnum = Basics.normalize_inc(1, 60, (int) value - i + 1);
		return new Karana((Name) tnum);
	}

	public override string ToString()
	{
		return value.ToString();
	}

	public Tables.Body.Name getLord()
	{
		switch (value)
		{
			case Name.Kimstughna:  return Tables.Body.Name.Moon;
			case Name.Sakuna:      return Tables.Body.Name.Mars;
			case Name.Chatushpada: return Tables.Body.Name.Sun;
			case Name.Naga:        return Tables.Body.Name.Venus;
			default:
			{
				var vn = Basics.normalize_inc(1, 7, (int) value - 1);
				switch (vn)
				{
					case 1:  return Tables.Body.Name.Sun;
					case 2:  return Tables.Body.Name.Moon;
					case 3:  return Tables.Body.Name.Mars;
					case 4:  return Tables.Body.Name.Mercury;
					case 5:  return Tables.Body.Name.Jupiter;
					case 6:  return Tables.Body.Name.Venus;
					default: return Tables.Body.Name.Saturn;
				}
			}
		}
	}
}