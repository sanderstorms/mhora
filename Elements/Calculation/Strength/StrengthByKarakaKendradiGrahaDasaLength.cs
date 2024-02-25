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
using Mhora.Definitions;
using Mhora.Elements.Dasas.GrahaDasa;
using Mhora.Elements.Yoga;

namespace Mhora.Elements.Calculation.Strength;

// Stronger Graha has longer length
// Stronger rasi has a Graha with longer length placed therein
public class StrengthByKarakaKendradiGrahaDasaLength : BaseStrength, IStrengthRasi, IStrengthGraha
{
	public StrengthByKarakaKendradiGrahaDasaLength(Grahas grahas) : base(grahas, false)
	{
	}

	public int Stronger(Body m, Body n)
	{
		var a = Value(m);
		var b = Value(n);

		return a.CompareTo(b);
	}

	public int Stronger(ZodiacHouse za, ZodiacHouse zb)
	{
		var a = Value(za);
		var b = Value(zb);

		return a.CompareTo(b);
	}

	protected double Value(ZodiacHouse zh)
	{
		double length = 0;
		foreach (var graha in _grahas.NavaGrahas)
		{
			length = Math.Max(length, KarakaKendradiGrahaDasa.LengthOfDasa(graha));
		}

		return length;
	}

	protected double Value(Body b)
	{
		return KarakaKendradiGrahaDasa.LengthOfDasa(_grahas [b]);
	}
}