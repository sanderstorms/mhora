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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Mhora.Elements;
using Mhora.Elements.Dasas;

namespace Mhora.Components.Dasa;

internal class DasaEntryConverter : ExpandableObjectConverter
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
		Trace.Assert(value is string, "DasaEntryConverter::ConvertFrom 1");
		var s = (string) value;

		var de  = new DasaEntry(Body.Name.Lagna, 0.0, 0.0, 1, "None");
		var arr = s.Split(',');
		if (arr.Length >= 1)
		{
			de.shortDesc = arr[0];
		}

		if (arr.Length >= 2)
		{
			de.level = int.Parse(arr[1]);
		}

		if (arr.Length >= 3)
		{
			de.startUT = double.Parse(arr[2]);
		}

		if (arr.Length >= 4)
		{
			de.dasaLength = double.Parse(arr[3]);
		}

		if (arr.Length >= 5)
		{
			de.graha = (Body.Name) int.Parse(arr[4]);
		}

		if (arr.Length >= 6)
		{
			de.zodiacHouse = (ZodiacHouse.Name) int.Parse(arr[5]);
		}

		return de;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
	{
		Trace.Assert(destType == typeof(string) && value is DasaEntry, "DasaItem::ConvertTo 1");
		var de = (DasaEntry) value;
		return de.shortDesc + "," + de.level + "," + de.startUT + "," + de.dasaLength + "," + (int) de.graha + "," + (int) de.zodiacHouse;
	}
}