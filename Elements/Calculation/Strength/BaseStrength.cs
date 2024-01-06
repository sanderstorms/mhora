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
using Mhora.Tables;

namespace Mhora.Elements.Calculation.Strength;

public abstract class BaseStrength
{
	protected bool      bUseSimpleLords;
	protected Division  dtype;
	protected Horoscope h;
	protected ArrayList std_div_pos;
	protected ArrayList std_grahas;

	protected BaseStrength(Horoscope _h, Division _dtype, bool _bUseSimpleLords)
	{
		h               = _h;
		dtype           = _dtype;
		bUseSimpleLords = _bUseSimpleLords;
		std_div_pos     = h.CalculateDivisionPositions(dtype);
	}

	protected Body.Name GetStrengthLord(ZodiacHouse.Name zh)
	{
		if (bUseSimpleLords)
		{
			return Basics.SimpleLordOfZodiacHouse(zh);
		}

		return h.LordOfZodiacHouse(new ZodiacHouse(zh), dtype);
	}

	protected Body.Name GetStrengthLord(ZodiacHouse zh)
	{
		return GetStrengthLord(zh.value);
	}

	protected int numGrahasInZodiacHouse(ZodiacHouse.Name zh)
	{
		var num = 0;
		foreach (DivisionPosition dp in std_div_pos)
		{
			if (dp.type != Body.Type.Graha)
			{
				continue;
			}

			if (dp.zodiac_house.value == zh)
			{
				num = num + 1;
			}
		}

		return num;
	}

	protected double karakaLongitude(Body.Name b)
	{
		var lon = h.getPosition(b).longitude.toZodiacHouseOffset();
		if (b == Body.Name.Rahu || b == Body.Name.Ketu)
		{
			lon = 30.0 - lon;
		}

		return lon;
	}

	protected Body.Name findAtmaKaraka()
	{
		Body.Name[] karakaBodies =
		{
			Body.Name.Sun,
			Body.Name.Moon,
			Body.Name.Mars,
			Body.Name.Mercury,
			Body.Name.Jupiter,
			Body.Name.Venus,
			Body.Name.Saturn,
			Body.Name.Rahu
		};
		var lon = 0.0;
		var ret = Body.Name.Sun;
		foreach (var bn in karakaBodies)
		{
			var offset = karakaLongitude(bn);
			if (offset > lon)
			{
				lon = offset;
			}

			ret = bn;
		}

		return ret;
	}

	public ArrayList findGrahasInHouse(ZodiacHouse.Name zh)
	{
		var ret = new ArrayList();
		foreach (DivisionPosition dp in std_div_pos)
		{
			if (dp.type == Body.Type.Graha && dp.zodiac_house.value == zh)
			{
				ret.Add(dp.name);
			}
		}

		return ret;
	}
}