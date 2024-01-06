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

// Stronger rasi is the first one
// Stronger graha is the first one
public class StrengthByFirst : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByFirst(Horoscope h, Division dtype) : base(h, dtype, true)
	{
	}

	public bool stronger(Body.Name m, Body.Name n)
	{
		return true;
	}

	public bool stronger(ZodiacHouse.Name za, ZodiacHouse.Name zb)
	{
		return true;
	}
}