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

public class GhatakaTithi
{
	public static bool checkTithi(ZodiacHouse janmaRasi, Tithi t)
	{
		var ja = janmaRasi.Sign;
		var gh = Tables.Tithis.NandaType.Nanda;
		switch (ja)
		{
			case ZodiacHouse.Rasi.Ari:
				gh = Tables.Tithis.NandaType.Nanda;
				break;
			case ZodiacHouse.Rasi.Tau:
				gh = Tables.Tithis.NandaType.Purna;
				break;
			case ZodiacHouse.Rasi.Gem:
				gh = Tables.Tithis.NandaType.Bhadra;
				break;
			case ZodiacHouse.Rasi.Can:
				gh = Tables.Tithis.NandaType.Bhadra;
				break;
			case ZodiacHouse.Rasi.Leo:
				gh = Tables.Tithis.NandaType.Jaya;
				break;
			case ZodiacHouse.Rasi.Vir:
				gh = Tables.Tithis.NandaType.Purna;
				break;
			case ZodiacHouse.Rasi.Lib:
				gh = Tables.Tithis.NandaType.Rikta;
				break;
			case ZodiacHouse.Rasi.Sco:
				gh = Tables.Tithis.NandaType.Nanda;
				break;
			case ZodiacHouse.Rasi.Sag:
				gh = Tables.Tithis.NandaType.Jaya;
				break;
			case ZodiacHouse.Rasi.Cap:
				gh = Tables.Tithis.NandaType.Rikta;
				break;
			case ZodiacHouse.Rasi.Aqu:
				gh = Tables.Tithis.NandaType.Jaya;
				break;
			case ZodiacHouse.Rasi.Pis:
				gh = Tables.Tithis.NandaType.Purna;
				break;
		}

		return t.toNandaType() == gh;
	}
}