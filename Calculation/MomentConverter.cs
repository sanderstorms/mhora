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

namespace Mhora.Calculation;

internal class MomentConverter : ExpandableObjectConverter
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
		Trace.Assert(value is string, "MomentConverter::ConvertFrom 1");
		var s = (string) value;

		int day = 1, month = 1, year = 1, hour = 1, min = 1, sec = 1;

		var _arr = s.Split(' ', ':');
		var arr  = new ArrayList(_arr);

		if ((string) arr[arr.Count - 1] == string.Empty)
		{
			arr[arr.Count - 1] = "0";
		}

		if (arr.Count >= 3)
		{
			while (arr.Count < 6)
			{
				arr.Add("0");
			}

			day   = int.Parse((string) arr[0]);
			month = Moment.FromStringMonth((string) arr[1]);
			year  = int.Parse((string) arr[2]);
			hour  = int.Parse((string) arr[3]);
			min   = int.Parse((string) arr[4]);
			sec   = int.Parse((string) arr[5]);
		}

		//if (day < 1 || day > 31) day = 1;
		if (hour < 0 || hour > 23)
		{
			hour = 12;
		}

		if (min < 0 || min > 120)
		{
			min = 30;
		}

		if (sec < 0 || sec > 120)
		{
			sec = 30;
		}

		var m = new Moment(year, month, day, hour, min, sec);
		return m;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
	{
		Application.Log.Debug("Foo: destType is {0}", destType);
		// Trace.Assert (destType == typeof(string) && value is Moment, "MomentConverter::ConvertTo 1");
		var m = (Moment) value;
		return m.ToString();
	}
}