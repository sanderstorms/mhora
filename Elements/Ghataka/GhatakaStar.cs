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
	public static bool checkStar(ZodiacHouse janmaRasi, Nakshatras.Nakshatra nak)
	{
		var ja = janmaRasi.Sign;
		var gh = Nakshatras.Nakshatra.Aswini;
		switch (ja)
		{
			case ZodiacHouse.Rasi.Ari:
				gh = Nakshatras.Nakshatra.Makha;
				break;
			case ZodiacHouse.Rasi.Tau:
				gh = Nakshatras.Nakshatra.Hasta;
				break;
			case ZodiacHouse.Rasi.Gem:
				gh = Nakshatras.Nakshatra.Swati;
				break;
			case ZodiacHouse.Rasi.Can:
				gh = Nakshatras.Nakshatra.Anuradha;
				break;
			case ZodiacHouse.Rasi.Leo:
				gh = Nakshatras.Nakshatra.Moola;
				break;
			case ZodiacHouse.Rasi.Vir:
				gh = Nakshatras.Nakshatra.Sravana;
				break;
			case ZodiacHouse.Rasi.Lib:
				gh = Nakshatras.Nakshatra.Satabisha;
				break;
			case ZodiacHouse.Rasi.Sco:
				gh = Nakshatras.Nakshatra.Revati;
				break;
			// FIXME dveja nakshatra?????
			case ZodiacHouse.Rasi.Sag:
				gh = Nakshatras.Nakshatra.Revati;
				break;
			case ZodiacHouse.Rasi.Cap:
				gh = Nakshatras.Nakshatra.Rohini;
				break;
			case ZodiacHouse.Rasi.Aqu:
				gh = Nakshatras.Nakshatra.Aridra;
				break;
			case ZodiacHouse.Rasi.Pis:
				gh = Nakshatras.Nakshatra.Aslesha;
				break;
		}

		return nak == gh;
	}
}