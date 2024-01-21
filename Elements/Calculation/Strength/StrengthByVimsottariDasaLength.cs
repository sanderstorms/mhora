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
using Mhora.Elements.Dasas.Nakshatra;

namespace Mhora.Elements.Calculation.Strength;

public class StrengthByVimsottariDasaLength : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByVimsottariDasaLength(Horoscope h, Division dtype) : base(h, dtype, false)
	{
	}

	public bool Stronger(Body.BodyType m, Body.BodyType n)
	{
		var a = VimsottariDasa.DasaLength(m);
		var b = VimsottariDasa.DasaLength(n);
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

	public bool Stronger(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb)
	{
		var a = Value(za);
		var b = Value(zb);
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

	protected double Value(ZodiacHouse.Rasi zh)
	{
		double length = 0;
		foreach (Position bp in H.PositionList)
		{
			if (bp.Type == Body.Type.Graha)
			{
				length = Math.Max(length, VimsottariDasa.DasaLength(bp.Name));
			}
		}

		return length;
	}
}