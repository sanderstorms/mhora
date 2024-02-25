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
using Mhora.Elements.Yoga;

namespace Mhora.Elements.Calculation.Strength;

public abstract class BaseStrength
{
	protected readonly Grahas _grahas;
	protected          bool   BUseSimpleLords;

	protected BaseStrength(Grahas grahas, bool bUseSimpleLords)
	{
		_grahas = grahas;
		BUseSimpleLords = bUseSimpleLords;
	}

	protected Body GetStrengthLord(ZodiacHouse zh)
	{
		if (BUseSimpleLords)
		{
			return zh.SimpleLordOfZodiacHouse();
		}

		var rashi = _grahas.Rashis.Find(zh);
		return (rashi.Lord);
	}

	protected int NumGrahasInZodiacHouse(ZodiacHouse zh)
	{
		return (_grahas.Rashis.Find(zh).Grahas.Count);
	}

	protected double KarakaLongitude(Body b)
	{
		return (_grahas.Find(b).HouseOffset);
	}

	protected Body FindAtmaKaraka()
	{
		Body[] karakaBodies =
		{
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Jupiter,
			Body.Venus,
			Body.Saturn,
			Body.Rahu
		};
		var lon = 0.0;
		var ret = Body.Sun;
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
}