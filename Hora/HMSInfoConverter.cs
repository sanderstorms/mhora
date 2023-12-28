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

namespace Mhora.Hora;

internal class HMSInfoConverter : ExpandableObjectConverter
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
        Trace.Assert(value is string, "HMSInfoConverter::ConvertFrom 1");
        var s = (string) value;

        int hour = 1, min = 1, sec = 1;

        var dir  = HMSInfo.dir_type.NS;
        var _arr = s.Split('.', ' ', ':');
        var arr  = new ArrayList(_arr);

        if (arr.Count >= 2)
        {
            if ((string) arr[arr.Count - 1] == string.Empty)
            {
                arr[arr.Count - 1] = "0";
            }

            while (arr.Count < 4)
            {
                arr.Add("0");
            }

            hour = int.Parse((string) arr[0]);
            var sdir = (string) arr[1];
            if (sdir == "W" || sdir == "w" || sdir == "S" || sdir == "s")
            {
                hour *= -1;
            }

            if (sdir == "W" || sdir == "w" || sdir == "E" || sdir == "e")
            {
                dir = HMSInfo.dir_type.EW;
            }
            else
            {
                dir = HMSInfo.dir_type.NS;
            }

            if (int.TryParse((string) arr[2], out min) == false)
            {
                //todo: warning
            }

            if (int.TryParse((string) arr[3], out sec) == false)
            {
                //todo: warning
            }
        }

        if (hour < -180 || hour > 180)
        {
            hour = 29;
        }

        if (min < 0 || min > 60)
        {
            min = 20;
        }

        if (sec < 0 || sec > 60)
        {
            sec = 30;
        }

        var hi = new HMSInfo(hour, min, sec, dir);
        return hi;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
    {
        Trace.Assert(destType == typeof(string) && value is HMSInfo, "HMSInfo::ConvertTo 1");
        var hi = (HMSInfo) value;
        return hi.ToString();
    }
}