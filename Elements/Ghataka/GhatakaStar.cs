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

namespace Mhora.Elements.Ghataka;

public class GhatakaStar
{
	public static bool CheckStar(ZodiacHouse janmaZodiacHouse, Nakshatra nak)
	{
		var ja = janmaZodiacHouse;
		var gh = Nakshatra.Aswini;
		switch (ja)
		{
			case ZodiacHouse.Ari:
				gh = Nakshatra.Makha;
				break;
			case ZodiacHouse.Tau:
				gh = Nakshatra.Hasta;
				break;
			case ZodiacHouse.Gem:
				gh = Nakshatra.Swati;
				break;
			case ZodiacHouse.Can:
				gh = Nakshatra.Anuradha;
				break;
			case ZodiacHouse.Leo:
				gh = Nakshatra.Moola;
				break;
			case ZodiacHouse.Vir:
				gh = Nakshatra.Sravana;
				break;
			case ZodiacHouse.Lib:
				gh = Nakshatra.Satabisha;
				break;
			case ZodiacHouse.Sco:
				gh = Nakshatra.Revati;
				break;
			// FIXME dveja nakshatra?????
			case ZodiacHouse.Sag:
				gh = Nakshatra.Revati;
				break;
			case ZodiacHouse.Cap:
				gh = Nakshatra.Rohini;
				break;
			case ZodiacHouse.Aqu:
				gh = Nakshatra.Aridra;
				break;
			case ZodiacHouse.Pis:
				gh = Nakshatra.Aslesha;
				break;
		}

		return nak == gh;
	}
}