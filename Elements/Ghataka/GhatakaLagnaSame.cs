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

public class GhatakaLagnaSame
{
	public static bool CheckLagna(ZodiacHouse janma, ZodiacHouse same)
	{
		var ja = janma;
		var gh = ZodiacHouse.Ari;
		switch (ja)
		{
			case ZodiacHouse.Ari:
				gh = ZodiacHouse.Ari;
				break;
			case ZodiacHouse.Tau:
				gh = ZodiacHouse.Tau;
				break;
			case ZodiacHouse.Gem:
				gh = ZodiacHouse.Can;
				break;
			case ZodiacHouse.Can:
				gh = ZodiacHouse.Lib;
				break;
			case ZodiacHouse.Leo:
				gh = ZodiacHouse.Cap;
				break;
			case ZodiacHouse.Vir:
				gh = ZodiacHouse.Pis;
				break;
			case ZodiacHouse.Lib:
				gh = ZodiacHouse.Vir;
				break;
			case ZodiacHouse.Sco:
				gh = ZodiacHouse.Sco;
				break;
			case ZodiacHouse.Sag:
				gh = ZodiacHouse.Sag;
				break;
			case ZodiacHouse.Cap:
				gh = ZodiacHouse.Aqu;
				break;
			case ZodiacHouse.Aqu:
				gh = ZodiacHouse.Gem;
				break;
			case ZodiacHouse.Pis:
				gh = ZodiacHouse.Leo;
				break;
		}

		return same == gh;
	}
}