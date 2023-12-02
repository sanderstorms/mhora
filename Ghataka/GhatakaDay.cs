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

using Mhora.Calculation;

namespace Mhora.Ghata
{
    public class GhatakaDay
    {
        public static bool checkDay(ZodiacHouse janmaRasi, Basics.Weekday wd)
        {
            var ja = janmaRasi.value;
            var gh = Basics.Weekday.Sunday;
            switch (ja)
            {
                case ZodiacHouse.Name.Ari:
                    gh = Basics.Weekday.Sunday;
                    break;
                case ZodiacHouse.Name.Tau:
                    gh = Basics.Weekday.Saturday;
                    break;
                case ZodiacHouse.Name.Gem:
                    gh = Basics.Weekday.Monday;
                    break;
                case ZodiacHouse.Name.Can:
                    gh = Basics.Weekday.Wednesday;
                    break;
                case ZodiacHouse.Name.Leo:
                    gh = Basics.Weekday.Saturday;
                    break;
                case ZodiacHouse.Name.Vir:
                    gh = Basics.Weekday.Saturday;
                    break;
                case ZodiacHouse.Name.Lib:
                    gh = Basics.Weekday.Thursday;
                    break;
                case ZodiacHouse.Name.Sco:
                    gh = Basics.Weekday.Friday;
                    break;
                case ZodiacHouse.Name.Sag:
                    gh = Basics.Weekday.Friday;
                    break;
                case ZodiacHouse.Name.Cap:
                    gh = Basics.Weekday.Tuesday;
                    break;
                case ZodiacHouse.Name.Aqu:
                    gh = Basics.Weekday.Thursday;
                    break;
                case ZodiacHouse.Name.Pis:
                    gh = Basics.Weekday.Friday;
                    break;
            }

            return wd == gh;
        }
    }
}