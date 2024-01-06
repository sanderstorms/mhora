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


namespace Mhora.Calculation;

public class SunMoonYoga
{
	public enum Name
	{
		Vishkambha = 1,
		Preeti,
		Aayushmaan,
		Saubhaagya,
		Sobhana,
		Atiganda,
		Sukarman,
		Dhriti,
		Shoola,
		Ganda,
		Vriddhi,
		Dhruva,
		Vyaaghaata,
		Harshana,
		Vajra,
		Siddhi,
		Vyatipaata,
		Variyan,
		Parigha,
		Shiva,
		Siddha,
		Saadhya,
		Subha,
		Sukla,
		Brahma,
		Indra,
		Vaidhriti
	}

	public SunMoonYoga(Name _mvalue)
	{
		value = _mvalue;
	}

	public Name value
	{
		get;
		set;
	}

	public int normalize()
	{
		return Basics.normalize_inc(1, 27, (int) value);
	}

	public SunMoonYoga add(int i)
	{
		var snum = Basics.normalize_inc(1, 27, (int) value + i - 1);
		return new SunMoonYoga((Name) snum);
	}

	public SunMoonYoga addReverse(int i)
	{
		var snum = Basics.normalize_inc(1, 27, (int) value - i + 1);
		return new SunMoonYoga((Name) snum);
	}

	public Tables.Body.Name getLord()
	{
		switch ((int) value % 9)
		{
			case 1:  return Tables.Body.Name.Saturn;
			case 2:  return Tables.Body.Name.Mercury;
			case 3:  return Tables.Body.Name.Ketu;
			case 4:  return Tables.Body.Name.Venus;
			case 5:  return Tables.Body.Name.Sun;
			case 6:  return Tables.Body.Name.Moon;
			case 7:  return Tables.Body.Name.Mars;
			case 8:  return Tables.Body.Name.Rahu;
			default: return Tables.Body.Name.Jupiter;
		}
	}

	public override string ToString()
	{
		return value.ToString();
	}
}