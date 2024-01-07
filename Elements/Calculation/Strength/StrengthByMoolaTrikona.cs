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

// Stronger rasi has more planets in moola trikona
// Stronger planet is in moola trikona rasi
public class StrengthByMoolaTrikona : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByMoolaTrikona(Horoscope h, Division dtype) : base(h, dtype, true)
	{
	}

	public bool stronger(Body.BodyType m, Body.BodyType n)
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

	public bool stronger(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb)
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

	public int value(ZodiacHouse.Rasi zn)
	{
		var ret = 0;
		foreach (DivisionPosition dp in std_div_pos)
		{
			if (dp.type != Body.Type.Graha)
			{
				continue;
			}

			if (dp.zodiac_house.Sign != zn)
			{
				continue;
			}

			ret += value(dp.name);
		}

		return ret;
	}

	public int value(Body.BodyType b)
	{
		if (h.getPosition(b).toDivisionPosition(dtype).isInMoolaTrikona())
		{
			return 1;
		}

		return 0;
	}
}