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

public class GhatakaLagnaOpp
{
	public static bool CheckLagna(ZodiacHouse janma, ZodiacHouse same)
	{
		var ja = janma.Sign;
		var gh = ZodiacHouse.Rasi.Ari;
		switch (ja)
		{
			case ZodiacHouse.Rasi.Ari:
				gh = ZodiacHouse.Rasi.Lib;
				break;
			case ZodiacHouse.Rasi.Tau:
				gh = ZodiacHouse.Rasi.Sco;
				break;
			case ZodiacHouse.Rasi.Gem:
				gh = ZodiacHouse.Rasi.Cap;
				break;
			case ZodiacHouse.Rasi.Can:
				gh = ZodiacHouse.Rasi.Ari;
				break;
			case ZodiacHouse.Rasi.Leo:
				gh = ZodiacHouse.Rasi.Can;
				break;
			case ZodiacHouse.Rasi.Vir:
				gh = ZodiacHouse.Rasi.Vir;
				break;
			case ZodiacHouse.Rasi.Lib:
				gh = ZodiacHouse.Rasi.Pis;
				break;
			case ZodiacHouse.Rasi.Sco:
				gh = ZodiacHouse.Rasi.Tau;
				break;
			case ZodiacHouse.Rasi.Sag:
				gh = ZodiacHouse.Rasi.Gem;
				break;
			case ZodiacHouse.Rasi.Cap:
				gh = ZodiacHouse.Rasi.Leo;
				break;
			case ZodiacHouse.Rasi.Aqu:
				gh = ZodiacHouse.Rasi.Sag;
				break;
			case ZodiacHouse.Rasi.Pis:
				gh = ZodiacHouse.Rasi.Aqu;
				break;
		}

		return same.Sign == gh;
	}
}