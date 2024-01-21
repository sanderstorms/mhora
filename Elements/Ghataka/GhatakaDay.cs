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

public class GhatakaDay
{
	public static bool CheckDay(ZodiacHouse janmaRasi, Tables.Hora.Weekday wd)
	{
		var ja = janmaRasi.Sign;
		var gh = Tables.Hora.Weekday.Sunday;
		switch (ja)
		{
			case ZodiacHouse.Rasi.Ari:
				gh = Tables.Hora.Weekday.Sunday;
				break;
			case ZodiacHouse.Rasi.Tau:
				gh = Tables.Hora.Weekday.Saturday;
				break;
			case ZodiacHouse.Rasi.Gem:
				gh = Tables.Hora.Weekday.Monday;
				break;
			case ZodiacHouse.Rasi.Can:
				gh = Tables.Hora.Weekday.Wednesday;
				break;
			case ZodiacHouse.Rasi.Leo:
				gh = Tables.Hora.Weekday.Saturday;
				break;
			case ZodiacHouse.Rasi.Vir:
				gh = Tables.Hora.Weekday.Saturday;
				break;
			case ZodiacHouse.Rasi.Lib:
				gh = Tables.Hora.Weekday.Thursday;
				break;
			case ZodiacHouse.Rasi.Sco:
				gh = Tables.Hora.Weekday.Friday;
				break;
			case ZodiacHouse.Rasi.Sag:
				gh = Tables.Hora.Weekday.Friday;
				break;
			case ZodiacHouse.Rasi.Cap:
				gh = Tables.Hora.Weekday.Tuesday;
				break;
			case ZodiacHouse.Rasi.Aqu:
				gh = Tables.Hora.Weekday.Thursday;
				break;
			case ZodiacHouse.Rasi.Pis:
				gh = Tables.Hora.Weekday.Friday;
				break;
		}

		return wd == gh;
	}
}