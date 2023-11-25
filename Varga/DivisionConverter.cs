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
using System.Globalization;
using mhora.Calculation;

namespace mhora.Varga
{
    internal class DivisionConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            if (t == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo            info,
            object                 value)
        {
            return new Division(Basics.DivisionType.Rasi);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo            culture,
            object                 value,
            Type                   destType)
        {
            //Trace.Assert (destType == typeof(string) && value is Division, "DivisionConverter::ConvertTo 1");
            return "Varga";
        }
    }
}