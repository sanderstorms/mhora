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

using System;
using System.Collections;
using System.ComponentModel;
using Mhora.Components.Converter;
using Mhora.Definitions;

namespace Mhora.Calculation;

[TypeConverter(typeof(OrderedZodiacHousesConverter))]
public class OrderedZodiacHouses : ICloneable
{
	public ArrayList houses;

	public OrderedZodiacHouses()
	{
		houses = new ArrayList();
	}

	public OrderedZodiacHouses(ZodiacHouse[] rashis)
	{
		houses = new () { rashis };
	}


	public object Clone()
	{
		var oz = new OrderedZodiacHouses
		{
			houses = (ArrayList) houses.Clone()
		};
		return oz;
	}

	public override string ToString()
	{
		var s     = string.Empty;
		var names = (ZodiacHouse[]) houses.ToArray(typeof(ZodiacHouse));
		foreach (var zn in names)
		{
			s += zn + " ";
		}

		return s;
	}
}