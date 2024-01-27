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

// Stronger rasi has larger number of exalted planets - debilitated planets
// Stronger planet is exalted or not debilitated
public class StrengthByExaltation : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByExaltation(Horoscope h, Division dtype) : base(h, dtype, true)
	{
	}

	public bool Stronger(Body.BodyType m, Body.BodyType n)
	{
		var valm = Value(m);
		var valn = Value(n);

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

	public bool Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var vala = Value(za);
		var valb = Value(zb);

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

	public int Value(ZodiacHouse zn)
	{
		var ret = 0;
		foreach (DivisionPosition dp in StdDivPos)
		{
			if (dp.Type != Body.Type.Graha)
			{
				continue;
			}

			if (dp.ZodiacHouse != zn)
			{
				continue;
			}

			if (dp.IsExaltedPhalita())
			{
				ret++;
			}
			else if (dp.IsDebilitatedPhalita())
			{
				ret--;
			}
		}

		return ret;
	}

	public int Value(Body.BodyType b)
	{
		if (H.GetPosition(b).ToDivisionPosition(Dtype).IsExaltedPhalita())
		{
			return 1;
		}

		if (H.GetPosition(b).ToDivisionPosition(Dtype).IsDebilitatedPhalita())
		{
			return -1;
		}

		return 0;
	}
}