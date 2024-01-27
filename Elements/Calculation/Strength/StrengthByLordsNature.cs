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

// Stronger rasi's lord by nature (moveable, fixed, dual)
// Stronger Graha's dispositor in such a rasi
public class StrengthByLordsNature : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByLordsNature(Horoscope h, Division dtype) : base(h, dtype, true)
	{
	}

	public bool Stronger(Body.BodyType m, Body.BodyType n)
	{
		var za = H.GetPosition(m).ToDivisionPosition(Dtype).ZodiacHouse;
		var zb = H.GetPosition(n).ToDivisionPosition(Dtype).ZodiacHouse;
		return Stronger(za, zb);
	}

	public bool Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		int[] vals =
		{
			3,
			1,
			2
		}; // dual, move, fix
		var a = NaturalValueForRasi(za);
		var b = NaturalValueForRasi(zb);
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

	public int NaturalValueForRasi(ZodiacHouse zha)
	{
		var bl  = H.LordOfZodiacHouse(zha, Dtype);
		var zhl = H.GetPosition(bl).ToDivisionPosition(Dtype).ZodiacHouse;

		int[] vals =
		{
			3,
			1,
			2
		}; // dual, move, fix
		return vals[(int) zhl % 3];
	}
}