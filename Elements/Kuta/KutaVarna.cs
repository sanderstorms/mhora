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

using System.Diagnostics;
using Mhora.Definitions;

namespace Mhora.Elements.Kuta;

public class KutaVarna
{
	public enum EType
	{
		Brahmana,
		Kshatriya,
		Vaishya,
		Sudra,
		Anuloma,
		Pratiloma
	}

	public static int GetMaxScore()
	{
		return 2;
	}

	public static int GetScore(Nakshatra m, Nakshatra f)
	{
		var em = GetType(m);
		var ef = GetType(f);
		if (em == ef)
		{
			return 2;
		}

		if (em == EType.Brahmana && (ef == EType.Kshatriya || ef == EType.Vaishya || ef == EType.Sudra))
		{
			return 1;
		}

		if (em == EType.Kshatriya && (ef == EType.Vaishya || ef == EType.Sudra))
		{
			return 1;
		}

		if (em == EType.Vaishya && ef == EType.Sudra)
		{
			return 1;
		}

		if (em == EType.Anuloma && ef != EType.Pratiloma)
		{
			return 1;
		}

		if (ef == EType.Anuloma && em != EType.Anuloma)
		{
			return 1;
		}

		return 0;
	}

	public static EType GetType(Nakshatra n)
	{
		switch ((int) n % 6)
		{
			case 1: return EType.Brahmana;
			case 2: return EType.Kshatriya;
			case 3: return EType.Vaishya;
			case 4: return EType.Sudra;
			case 5: return EType.Anuloma;
			case 0: return EType.Pratiloma;
			case 6: return EType.Pratiloma;
		}

		Debug.Assert(false, "KutaVarna::getType");
		return EType.Anuloma;
	}
}