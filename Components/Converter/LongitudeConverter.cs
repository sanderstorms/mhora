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

namespace Mhora.Components.Converter;

/// <summary>
///     A package of longitude related functions. These are useful enough that
///     I have justified using an object instead of a simple double value type
/// </summary>
internal class LongitudeConverter : ExpandableObjectConverter
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
		Trace.Assert(value is string, "LongitudeConverter::ConvertFrom 1");
		var s = (string) value;

		var arr = s.Split('.', ' ', ':');

		double lonValue = 0;
		if (arr.Length >= 1)
		{
			lonValue = int.Parse(arr[0]);
		}

		if (arr.Length >= 2)
		{
			switch (arr[1].ToLower())
			{
				case "ari":
					lonValue += 0.0;
					break;
				case "tau":
					lonValue += 30.0;
					break;
				case "gem":
					lonValue += 60.0;
					break;
				case "can":
					lonValue += 90.0;
					break;
				case "leo":
					lonValue += 120.0;
					break;
				case "vir":
					lonValue += 150.0;
					break;
				case "lib":
					lonValue += 180.0;
					break;
				case "sco":
					lonValue += 210.0;
					break;
				case "sag":
					lonValue += 240.0;
					break;
				case "cap":
					lonValue += 270.0;
					break;
				case "aqu":
					lonValue += 300.0;
					break;
				case "pis":
					lonValue += 330.0;
					break;
			}
		}

		double divider = 60;
		for (var i = 2; i < arr.Length; i++)
		{
			lonValue += double.Parse(arr[i]) / divider;
			divider  *= 60.0;
		}

		return new Longitude(lonValue);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
	{
		Trace.Assert(destType == typeof(string) && value is Longitude, "Longitude::ConvertTo 1");
		var lon = (Longitude) value;
		return lon.ToString();
	}
}