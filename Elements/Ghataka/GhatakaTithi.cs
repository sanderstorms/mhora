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

public class GhatakaTithi
{
	public static bool CheckTithi(ZodiacHouse janmaZodiacHouse, Tithi t)
	{
		var ja = janmaZodiacHouse;
		var gh = Tithis.NandaType.Nanda;
		switch (ja)
		{
			case ZodiacHouse.Ari:
				gh = Tithis.NandaType.Nanda;
				break;
			case ZodiacHouse.Tau:
				gh = Tithis.NandaType.Purna;
				break;
			case ZodiacHouse.Gem:
				gh = Tithis.NandaType.Bhadra;
				break;
			case ZodiacHouse.Can:
				gh = Tithis.NandaType.Bhadra;
				break;
			case ZodiacHouse.Leo:
				gh = Tithis.NandaType.Jaya;
				break;
			case ZodiacHouse.Vir:
				gh = Tithis.NandaType.Purna;
				break;
			case ZodiacHouse.Lib:
				gh = Tithis.NandaType.Rikta;
				break;
			case ZodiacHouse.Sco:
				gh = Tithis.NandaType.Nanda;
				break;
			case ZodiacHouse.Sag:
				gh = Tithis.NandaType.Jaya;
				break;
			case ZodiacHouse.Cap:
				gh = Tithis.NandaType.Rikta;
				break;
			case ZodiacHouse.Aqu:
				gh = Tithis.NandaType.Jaya;
				break;
			case ZodiacHouse.Pis:
				gh = Tithis.NandaType.Purna;
				break;
		}

		return t.ToNandaType() == gh;
	}
}