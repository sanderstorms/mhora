/******
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

/*****************************************************************
 * Module: EnumDescConverter.cs
 * Type: C# Source Code
 * Version: 1.0
 * Description: Enum Converter using Description Attributes
 *
 * Revisions
 * ------------------------------------------------
 * [F] 24/02/2004, Jcl - Shaping up
 * [B] 25/02/2004, Jcl - Made it much easier :-)
 *
 *****************************************************************/

using System;
using System.ComponentModel;
using System.Globalization;
using mhora.Util;

namespace Mhora.Util;

/// <summary>
///     EnumConverter supporting System.ComponentModel.DescriptionAttribute
/// </summary>
public class EnumDescConverter : EnumConverter
{
	protected Type myVal;

	public EnumDescConverter(Type type) : base(type)
	{
		myVal = type;
	}


	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (value is Enum && destinationType == typeof(string))
		{
			return ((Enum) value).GetEnumDescription();
		}

		if (value is string && destinationType == typeof(string))
		{
			return myVal.GetEnumDescription((string) value);
		}

		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			return myVal.GetEnumValue((string)value);
		}

		if (value is Enum)
		{
			return ((Enum)value).GetEnumDescription();
		}

		return base.ConvertFrom(context, culture, value);
	}
}