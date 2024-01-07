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
using Mhora.Elements.Dasas.Graha;

namespace Mhora.Elements.Calculation.Strength;

// Stronger graha has longer length
// Stronger rasi has a graha with longer length placed therein
public class StrengthByKarakaKendradiGrahaDasaLength : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByKarakaKendradiGrahaDasaLength(Horoscope h, Division dtype) : base(h, dtype, false)
	{
	}

	public bool stronger(Body.BodyType m, Body.BodyType n)
	{
		var a = value(m);
		var b = value(n);
		if (a > b)
		{
			return true;
		}

		if (a < b)
		{
			return false;
		}

		throw new EqualStrength();
	}

	public bool stronger(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb)
	{
		var a = value(za);
		var b = value(zb);
		if (a > b)
		{
			return true;
		}

		if (a < b)
		{
			return false;
		}

		throw new EqualStrength();
	}

	protected double value(ZodiacHouse.Rasi zh)
	{
		double length = 0;
		foreach (Position bp in h.positionList)
		{
			if (bp.type == Body.Type.Graha)
			{
				var dp = bp.toDivisionPosition(dtype);
				length = Math.Max(length, KarakaKendradiGrahaDasa.LengthOfDasa(h, dtype, bp.name, dp));
			}
		}

		return length;
	}

	protected double value(Body.BodyType b)
	{
		var dp = h.getPosition(b).toDivisionPosition(dtype);
		return KarakaKendradiGrahaDasa.LengthOfDasa(h, dtype, b, dp);
	}
}