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

using Mhora.Dasas;
using Mhora.Definitions;
using Mhora.Elements;

namespace Mhora.Calculation.Strength;

// StrengthByNarayanaDasaLength rasi has a larger narayana dasa length
// StrengthByNarayanaDasaLength Graha is in such a rasi
public static class NarayanaDasaLength
{
	public static int StrengthByNarayanaDasaLength(this Grahas grahas, Body m, Body n, bool simpleLord)
	{
		var a = grahas.Value(m, simpleLord);
		var b = grahas.Value(n, simpleLord);

		return a.CompareTo(b);
	}

	public static int StrengthByNarayanaDasaLength(this Grahas grahas, ZodiacHouse za, ZodiacHouse zb, bool simpleLord)
	{
		var a = grahas.Value(za, simpleLord);
		var b = grahas.Value(zb, simpleLord);

		return a.CompareTo(b);
	}

	private static int Value(this Grahas grahas, ZodiacHouse zh, bool simpleLord)
	{
		var bl = grahas.Horoscope.LordOfZodiacHouse(zh, grahas.Varga, simpleLord);
		return Dasa.NarayanaDasaLength(zh, grahas [bl]);
	}

	private static int Value(this Grahas grahas, Body bm, bool simpleLord)
	{
		return grahas.Value(grahas [bm].Rashi, simpleLord);
	}
}