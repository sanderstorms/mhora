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

// Stronger rasi's lord is AK
public class StrengthByLordIsAtmaKaraka : BaseStrength, IStrengthRasi
{
	public StrengthByLordIsAtmaKaraka(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
	{
	}

	public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
	{
		var lora = GetStrengthLord(za);
		var lorb = GetStrengthLord(zb);
		var ak   = findAtmaKaraka();
		if (lora == ak)
		{
			return true;
		}

		if (lorb == ak)
		{
			return false;
		}

		throw new EqualStrength();
	}
}