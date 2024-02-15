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

namespace Mhora.Elements.Kuta;

public class KutaRasiYoni
{
	public enum EType
	{
		Pakshi,
		Reptile,
		Pasu,
		Nara
	}

	public static EType GetType(ZodiacHouse z)
	{
		switch (z)
		{
			case ZodiacHouse.Cap:
			case ZodiacHouse.Pis: return EType.Pakshi;
			case ZodiacHouse.Can:
			case ZodiacHouse.Sco: return EType.Reptile;
			case ZodiacHouse.Ari:
			case ZodiacHouse.Tau:
			case ZodiacHouse.Leo: return EType.Pasu;
		}

		return EType.Nara;
	}
}