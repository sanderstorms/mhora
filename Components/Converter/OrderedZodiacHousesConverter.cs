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
using System.Diagnostics;
using System.Globalization;
using Mhora.Elements;
using Mhora.Elements.Calculation;

namespace Mhora.Components.Converter;

internal class OrderedZodiacHousesConverter : ExpandableObjectConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
	{
		if (t == typeof(string))
		{
			return true;
		}

		return base.CanConvertFrom(context, t);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
	{
		Trace.Assert(value is string, "OrderedZodiacHousesConverter::ConvertFrom 1");
		var s = (string) value;

		var oz  = new OrderedZodiacHouses();
		var al  = new ArrayList();
		var arr = s.Split('.', ' ', ':', ',');
		foreach (var szh_mixed in arr)
		{
			var szh = szh_mixed.ToLower();
			switch (szh)
			{
				case "ari":
					al.Add(ZodiacHouse.Rasi.Ari);
					break;
				case "tau":
					al.Add(ZodiacHouse.Rasi.Tau);
					break;
				case "gem":
					al.Add(ZodiacHouse.Rasi.Gem);
					break;
				case "can":
					al.Add(ZodiacHouse.Rasi.Can);
					break;
				case "leo":
					al.Add(ZodiacHouse.Rasi.Leo);
					break;
				case "vir":
					al.Add(ZodiacHouse.Rasi.Vir);
					break;
				case "lib":
					al.Add(ZodiacHouse.Rasi.Lib);
					break;
				case "sco":
					al.Add(ZodiacHouse.Rasi.Sco);
					break;
				case "sag":
					al.Add(ZodiacHouse.Rasi.Sag);
					break;
				case "cap":
					al.Add(ZodiacHouse.Rasi.Cap);
					break;
				case "aqu":
					al.Add(ZodiacHouse.Rasi.Aqu);
					break;
				case "pis":
					al.Add(ZodiacHouse.Rasi.Pis);
					break;
			}
		}

		oz.houses = (ArrayList) al.Clone();
		return oz;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
	{
		Trace.Assert(destType == typeof(string) && value is OrderedZodiacHouses, "HMSInfo::ConvertTo 1");
		var oz = (OrderedZodiacHouses) value;
		return oz.ToString();
	}
}