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
using Mhora.Dasas.NakshatraDasa;
using Mhora.Definitions;
using Mhora.Elements.Yoga;

namespace Mhora.Calculation.Strength;

public static class VimsottariDasaLength
{
	public static int StrengthByVimsottariDasaLength(this Grahas grahas, Body m, Body n)
	{
		var a = VimsottariDasa.DasaLength(m);
		var b = VimsottariDasa.DasaLength(n);

		return a.CompareTo(b);
	}

	public static int StrengthByVimsottariDasaLength(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb)
	{
		var a = grahas.StrengthByVimsottariDasaLengthValue(za);
		var b = grahas.StrengthByVimsottariDasaLengthValue(zb);

		return a.CompareTo(b);
	}

	private static double StrengthByVimsottariDasaLengthValue(this Grahas grahas, ZodiacHouse zh)
	{
		double length = 0;
		foreach (var graha in grahas.NavaGrahas)
		{
			length = Math.Max(length, VimsottariDasa.DasaLength(graha));
		}

		return length;
	}
}