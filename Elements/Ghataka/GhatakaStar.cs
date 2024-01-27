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

namespace Mhora.Elements.Ghataka;

public class GhatakaStar
{
	public static bool CheckStar(ZodiacHouse janmaZodiacHouse, Nakshatras.Nakshatra nak)
	{
		var ja = janmaZodiacHouse;
		var gh = Nakshatras.Nakshatra.Aswini;
		switch (ja)
		{
			case ZodiacHouse.Ari:
				gh = Nakshatras.Nakshatra.Makha;
				break;
			case ZodiacHouse.Tau:
				gh = Nakshatras.Nakshatra.Hasta;
				break;
			case ZodiacHouse.Gem:
				gh = Nakshatras.Nakshatra.Swati;
				break;
			case ZodiacHouse.Can:
				gh = Nakshatras.Nakshatra.Anuradha;
				break;
			case ZodiacHouse.Leo:
				gh = Nakshatras.Nakshatra.Moola;
				break;
			case ZodiacHouse.Vir:
				gh = Nakshatras.Nakshatra.Sravana;
				break;
			case ZodiacHouse.Lib:
				gh = Nakshatras.Nakshatra.Satabisha;
				break;
			case ZodiacHouse.Sco:
				gh = Nakshatras.Nakshatra.Revati;
				break;
			// FIXME dveja nakshatra?????
			case ZodiacHouse.Sag:
				gh = Nakshatras.Nakshatra.Revati;
				break;
			case ZodiacHouse.Cap:
				gh = Nakshatras.Nakshatra.Rohini;
				break;
			case ZodiacHouse.Aqu:
				gh = Nakshatras.Nakshatra.Aridra;
				break;
			case ZodiacHouse.Pis:
				gh = Nakshatras.Nakshatra.Aslesha;
				break;
		}

		return nak == gh;
	}
}