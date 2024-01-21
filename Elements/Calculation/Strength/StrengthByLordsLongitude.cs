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

// Stronger rasi's lord has traversed larger longitude
public class StrengthByLordsLongitude : BaseStrength, IStrengthRasi
{
	public StrengthByLordsLongitude(Horoscope h, Division dtype, bool bSimpleLord) : base(h, dtype, bSimpleLord)
	{
	}

	public bool Stronger(ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb)
	{
		var lora = GetStrengthLord(za);
		var lorb = GetStrengthLord(zb);
		var offa = KarakaLongitude(lora);
		var offb = KarakaLongitude(lorb);
		if (offa > offb)
		{
			return true;
		}

		if (offb > offa)
		{
			return false;
		}

		throw new EqualStrength();
	}
}