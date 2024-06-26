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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Mhora.Calculation;
using Mhora.Definitions;

namespace Mhora.Components.Converter;

internal class OrderedGrahasConverter : ExpandableObjectConverter
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
		Trace.Assert(value is string, "OrderedGrahasConverter::ConvertFrom 1");
		var s = (string) value;

		var oz  = new OrderedGrahas();
		var al  = new List<Body>();
		var arr = s.Split('.', ' ', ':', ',');
		foreach (var szh_mixed in arr)
		{
			var szh = szh_mixed.ToLower();
			switch (szh)
			{
				case "as":
					al.Add(Body.Lagna);
					break;
				case "su":
					al.Add(Body.Sun);
					break;
				case "mo":
					al.Add(Body.Moon);
					break;
				case "ma":
					al.Add(Body.Mars);
					break;
				case "me":
					al.Add(Body.Mercury);
					break;
				case "ju":
					al.Add(Body.Jupiter);
					break;
				case "ve":
					al.Add(Body.Venus);
					break;
				case "sa":
					al.Add(Body.Saturn);
					break;
				case "ra":
					al.Add(Body.Rahu);
					break;
				case "ke":
					al.Add(Body.Ketu);
					break;
			}
		}

		oz.grahas = [..al];
		return oz;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
	{
		Trace.Assert(destType == typeof(string) && value is OrderedGrahas, "OrderedGrahas::ConvertTo 1");
		var oz = (OrderedGrahas) value;
		return oz.ToString();
	}
}