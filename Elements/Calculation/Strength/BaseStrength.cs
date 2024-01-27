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

using System.Collections;

namespace Mhora.Elements.Calculation.Strength;

public abstract class BaseStrength
{
	protected bool      BUseSimpleLords;
	protected Division  Dtype;
	protected Horoscope H;
	protected ArrayList StdDivPos;
	protected ArrayList StdGrahas;

	protected BaseStrength(Horoscope h, Division dtype, bool bUseSimpleLords)
	{
		H               = h;
		Dtype           = dtype;
		BUseSimpleLords = bUseSimpleLords;
		StdDivPos     = H.CalculateDivisionPositions(Dtype);
	}

	protected Body.BodyType GetStrengthLord(ZodiacHouse zh)
	{
		if (BUseSimpleLords)
		{
			return zh.SimpleLordOfZodiacHouse();
		}

		return H.LordOfZodiacHouse(zh, Dtype);
	}

	protected int NumGrahasInZodiacHouse(ZodiacHouse zh)
	{
		var num = 0;
		foreach (DivisionPosition dp in StdDivPos)
		{
			if (dp.Type != Body.Type.Graha)
			{
				continue;
			}

			if (dp.ZodiacHouse == zh)
			{
				num = num + 1;
			}
		}

		return num;
	}

	protected double KarakaLongitude(Body.BodyType b)
	{
		var lon = H.GetPosition(b).Longitude.ToZodiacHouseOffset();
		if (b == Body.BodyType.Rahu || b == Body.BodyType.Ketu)
		{
			lon = 30.0 - lon;
		}

		return lon;
	}

	protected Body.BodyType FindAtmaKaraka()
	{
		Body.BodyType[] karakaBodies =
		{
			Body.BodyType.Sun,
			Body.BodyType.Moon,
			Body.BodyType.Mars,
			Body.BodyType.Mercury,
			Body.BodyType.Jupiter,
			Body.BodyType.Venus,
			Body.BodyType.Saturn,
			Body.BodyType.Rahu
		};
		var lon = 0.0;
		var ret = Body.BodyType.Sun;
		foreach (var bn in karakaBodies)
		{
			var offset = KarakaLongitude(bn);
			if (offset > lon)
			{
				lon = offset;
			}

			ret = bn;
		}

		return ret;
	}

	public ArrayList FindGrahasInHouse(ZodiacHouse zh)
	{
		var ret = new ArrayList();
		foreach (DivisionPosition dp in StdDivPos)
		{
			if (dp.Type == Body.Type.Graha && dp.ZodiacHouse == zh)
			{
				ret.Add(dp.Name);
			}
		}

		return ret;
	}
}