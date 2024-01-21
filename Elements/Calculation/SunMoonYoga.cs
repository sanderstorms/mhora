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


namespace Mhora.Elements.Calculation;

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
		return Basics.NormalizeInc((int) value, 1, 27);
	}

	public SunMoonYoga add(int i)
	{
		var snum = Basics.NormalizeInc((int) value + i - 1, 1, 27);
		return new SunMoonYoga((Name) snum);
	}

	public SunMoonYoga addReverse(int i)
	{
		var snum = Basics.NormalizeInc((int) value - i + 1, 1, 27);
		return new SunMoonYoga((Name) snum);
	}

	public Body.BodyType getLord()
	{
		switch ((int) value % 9)
		{
			case 1:  return Body.BodyType.Saturn;
			case 2:  return Body.BodyType.Mercury;
			case 3:  return Body.BodyType.Ketu;
			case 4:  return Body.BodyType.Venus;
			case 5:  return Body.BodyType.Sun;
			case 6:  return Body.BodyType.Moon;
			case 7:  return Body.BodyType.Mars;
			case 8:  return Body.BodyType.Rahu;
			default: return Body.BodyType.Jupiter;
		}
	}

	public override string ToString()
	{
		return value.ToString();
	}
}