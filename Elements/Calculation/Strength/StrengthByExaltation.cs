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

using Mhora.Definitions;
using Mhora.Elements.Yoga;

namespace Mhora.Elements.Calculation.Strength;

// Stronger rasi has larger number of exalted planets - debilitated planets
// Stronger planet is exalted or not debilitated
public class StrengthByExaltation : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByExaltation(Grahas grahas) : base(grahas, true)
	{
	}

	public int Stronger(Body m, Body n)
	{
		var valm = Value(m);
		var valn = Value(n);

		return valm.CompareTo(valn);
	}

	public int Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var vala = Value(za);
		var valb = Value(zb);

		return vala.CompareTo(valb);
	}

	public int Value(ZodiacHouse zn)
	{
		var ret = 0;
		foreach (var graha in _grahas.NavaGrahas)
		{
			if (graha.Rashi != zn)
			{
				continue;
			}

			if (graha.IsExalted)
			{
				ret++;
			}
			else if (graha.IsDebilitated)
			{
				ret--;
			}
		}

		return ret;
	}

	public int Value(Body b)
	{
		if (_grahas [b].IsExalted)
		{
			return 1;
		}

		if (_grahas [b].IsDebilitated)
		{
			return -1;
		}

		return 0;
	}
}