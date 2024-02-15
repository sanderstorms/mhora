using System;
using System.ComponentModel;

namespace Mhora.Util;

public static class EnumHelper
{
	public static int Index(this Enum value)
	{
		if (value == null)
		{
			return -1;
		}

		return value.GetHashCode();
	}

	public static string Name(this Enum value)
	{
		var type = value.GetType();
		var name = Enum.GetName(type, value);
		return name;
	}

	/// <summary>
	///     Gets Enum Value's Description Attribute
	/// </summary>
	/// <param name="value">The value you want the description attribute for</param>
	/// <returns>The description, if any, else it's .ToString()</returns>
	public static string GetEnumDescription(this Enum value)
	{
		var fi         = value.GetType().GetField(value.ToString());
		var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
		return attributes.Length > 0 ? attributes[0].Description : value.ToString();
	}

	/// <summary>
	///     Gets the description for certaing named value in an Enumeration
	/// </summary>
	/// <param name="value">The type of the Enumeration</param>
	/// <param name="name">The name of the Enumeration value</param>
	/// <returns>The description, if any, else the passed name</returns>
	public static string GetEnumDescription(this Type value, string name)
	{
		var fi         = value.GetField(name);
		var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
		return attributes.Length > 0 ? attributes[0].Description : name;
	}

	/// <summary>
	///     Gets the value of an Enum, based on it's Description Attribute or named value
	/// </summary>
	/// <param name="value">The Enum type</param>
	/// <param name="description">The description or name of the element</param>
	/// <returns>The value, or the passed in description, if it was not found</returns>
	public static object GetEnumValue(this Type value, string description)
	{
		var fis = value.GetFields();
		foreach (var fi in fis)
		{
			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
			{
				if (attributes[0].Description == description)
				{
					return fi.GetValue(fi.Name);
				}
			}

			if (fi.Name == description)
			{
				return fi.GetValue(fi.Name);
			}
		}

		return description;
	}

}