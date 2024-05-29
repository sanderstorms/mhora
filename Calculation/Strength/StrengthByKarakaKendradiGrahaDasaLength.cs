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
using Mhora.Dasas.GrahaDasa;
using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Calculation.Strength;

// StrengthByKarakaKendradiGrahaDasaLength Graha has longer length
// StrengthByKarakaKendradiGrahaDasaLength rasi has a Graha with longer length placed therein
public static class KarakaKendradiGrahaDasaLength
{
	public static int StrengthByKarakaKendradiGrahaDasaLength(this Grahas grahas, Body m, Body n)
	{
		var a = grahas.KarakaKendradiGrahaDasaLengthStrength(m);
		var b = grahas.KarakaKendradiGrahaDasaLengthStrength(n);

		return a.CompareTo(b);
	}

	public static int StrengthByKarakaKendradiGrahaDasaLength(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var a = grahas.KarakaKendradiGrahaDasaLengthStrength(za);
		var b = grahas.KarakaKendradiGrahaDasaLengthStrength(zb);

		return a.CompareTo(b);
	}

	private static double KarakaKendradiGrahaDasaLengthStrength(this Grahas grahas, ZodiacHouse zh)
	{
		double length = 0;
		foreach (var graha in grahas.NavaGrahas)
		{
			length = Math.Max(length, KarakaKendradiGrahaDasa.LengthOfDasa(graha));
		}

		return length;
	}

	private static double KarakaKendradiGrahaDasaLengthStrength(this Grahas grahas, Body b) => KarakaKendradiGrahaDasa.LengthOfDasa(grahas [b]);
}