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

namespace Mhora.Elements.Calculation.Strength;

// Stronger rasi has more planets in own house
// Stronger planet is in own house
public class StrengthByOwnHouse : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByOwnHouse(Horoscope h, Division dtype) : base(h, dtype, true)
	{
	}

	public bool stronger(Body.Name m, Body.Name n)
	{
		var valm = value(m);
		var valn = value(n);

		if (valm > valn)
		{
			return true;
		}

		if (valn > valm)
		{
			return false;
		}

		throw new EqualStrength();
	}

	public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
	{
		var vala = value(za);
		var valb = value(zb);

		if (vala > valb)
		{
			return true;
		}

		if (valb > vala)
		{
			return false;
		}

		throw new EqualStrength();
	}

	public int value(ZodiacHouse.Name zn)
	{
		var ret = 0;
		foreach (DivisionPosition dp in std_div_pos)
		{
			if (dp.type != Body.Type.Graha)
			{
				continue;
			}

			if (dp.zodiac_house.value != zn)
			{
				continue;
			}

			ret += value(dp.name);
		}

		return ret;
	}

	public int value(Body.Name b)
	{
		if (h.getPosition(b).toDivisionPosition(dtype).isInOwnHouse())
		{
			return 1;
		}

		return 0;
	}
}