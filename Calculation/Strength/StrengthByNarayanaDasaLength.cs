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

using Mhora.Varga;

namespace Mhora.Calculation.Strength;

// Stronger rasi has a larger narayana dasa length
// Stronger graha is in such a rasi
public class StrengthByNarayanaDasaLength : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByNarayanaDasaLength(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
	{
	}

	public bool stronger(Tables.Body.Name m, Tables.Body.Name n)
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

	public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
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

	protected int value(ZodiacHouse.Name _zh)
	{
		var bl = GetStrengthLord(_zh);
		var pl = h.getPosition(bl).toDivisionPosition(dtype);
		return Dasa.NarayanaDasaLength(new ZodiacHouse(_zh), pl);
	}

	protected int value(Tables.Body.Name bm)
	{
		var zm = h.getPosition(bm).toDivisionPosition(dtype).zodiac_house.value;
		return value(zm);
	}
}