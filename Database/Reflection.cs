using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using SujaySarma.Data.Files.TokenLimitedFiles.Attributes;

namespace Mhora.Database;

public static class Reflection
{
	private static MemberInfo GetField(this IEnumerable<FieldInfo> fields, string attributeName)
	{
		foreach (var field in fields)
		{
			var attributes = field.GetCustomAttributes(typeof(FileFieldAttribute), true);

			if (attributes.Length > 0)
			{
				var xmlTokenAttribute = (FileFieldAttribute) attributes[0];
				if (string.Compare(xmlTokenAttribute.Name, attributeName, StringComparison.Ordinal) == 0)
				{
					return field;
				}
			}
		}

		return null;
	}

	private static MemberInfo GetProperty(this IEnumerable<PropertyInfo> properties, string attributeName)
	{
		foreach (var property in properties)
		{
			var attributes = property.GetCustomAttributes(typeof(FileFieldAttribute), true);

			if (attributes.Length > 0)
			{
				var xmlTokenAttribute = (FileFieldAttribute) attributes[0];
				if (string.Compare(xmlTokenAttribute.Name, attributeName, StringComparison.Ordinal) == 0)
				{
					return property;
				}
			}
		}

		return null;
	}

	private static MethodInfo GetMethod(this IEnumerable<MethodInfo> methods, string attributeName)
	{
		foreach (var method in methods)
		{
			var attributes = method.GetCustomAttributes(typeof(FileFieldAttribute), true);

			if (attributes.Length > 0)
			{
				var xmlTokenAttribute = (FileFieldAttribute) attributes[0];
				if (string.Compare(xmlTokenAttribute.Name, attributeName, StringComparison.Ordinal) == 0)
				{
					return method;
				}
			}
		}

		return null;
	}

	public static MemberInfo GetFieldByAttribute(this Type type, string attributeName)
	{
		var fields     = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		var memberInfo = GetField(fields, attributeName);

		if (memberInfo == null)
		{
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
			memberInfo = GetProperty(properties, attributeName);
		}

		return memberInfo;
	}

	public static string GetToken(this MemberInfo memberInfo)
	{
		var attributes = memberInfo.GetCustomAttributes(typeof(FileFieldAttribute), true);

		if (attributes.Length > 0)
		{
			var field = (FileFieldAttribute) attributes[0];
			return field.Name;
		}

		return null;
	}

	public static void SetValue(this MemberInfo member, object instance, object value)
	{
		switch (member.MemberType)
		{
			case MemberTypes.Field:
			{
				((FieldInfo) member).SetValue(instance, value);
			}
				break;

			case MemberTypes.Property:
			{
				((PropertyInfo) member).SetValue(instance, value, null);
			}
				break;
		}
	}

	public static Type Type(this MemberInfo member)
	{
		Type type = null;
		switch (member.MemberType)
		{
			case MemberTypes.Field:
			{
				type = ((FieldInfo) member).FieldType;
			}
				break;

			case MemberTypes.Property:
			{
				type = ((PropertyInfo) member).PropertyType;
			}
				break;
		}

		return type;
	}

	public static bool Is(this MemberInfo member, Type type)
	{
		if (type.IsGenericType)
		{
			try
			{
				var typeParameters = member.Type().GetGenericArguments();
				var listType       = type.MakeGenericType(typeParameters);

				if (member.Type() == listType)
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		return member.Type() == type;
	}

	public static bool IsStruct(this Type type)
	{
		return type.IsValueType && type.IsEnum == false && type.IsPrimitive == false;
	}


	public static (string, string) ParseTuple(string value)
	{
		var valueX = string.Empty;
		var valueY = string.Empty;
		var tokens = value.Split(new[]
		{
			",",
			";",
			" "
		}, StringSplitOptions.RemoveEmptyEntries);
		if (tokens.Length == 2)
		{
			if (value.Contains("="))
			{
				valueX = tokens[0].Split('=')[1];
				valueY = tokens[1].Split('=')[1].TrimEnd('}');
			}
			else
			{
				valueX = tokens[0];
				valueY = tokens[1];
			}
		}

		return (valueX, valueY);
	}


	public static bool SetValue(this object instance, string fieldName, string value)
	{
		object fieldValue = null;
		try
		{
			var type = instance.GetType();

			var field = type.GetFieldByAttribute(fieldName);

			if (field == null)
			{
				return false;
			}

			if (field.Type().IsClass)
			{
				var bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				var arguments = new[]
				{
					typeof(string)
				};
				var parse = type.GetMethod(" Parse", bindingFlags, null, arguments, null);
				if (parse != null)
				{
					parse.Invoke(instance, new object[]
					{
						value
					});
					return true;
				}
			}

			if (field.Is(typeof(DateTime)))
			{
				field.SetValue(instance, DateTime.FromBinary(long.Parse(value)));
				return true;
			}

			if (field.Is(typeof(Color)))
			{
				field.SetValue(instance, Color.FromArgb(int.Parse(value)));
				return true;
			}

			if (field.Type().IsEnum)
			{
				var enumType = Enum.GetUnderlyingType(field.Type());
				if (enumType == typeof(byte))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), byte.Parse(value)));
				}
				else if (enumType == typeof(sbyte))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), sbyte.Parse(value)));
				}
				else if (enumType == typeof(short))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), short.Parse(value)));
				}
				else if (enumType == typeof(ushort))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), ushort.Parse(value)));
				}
				else if (enumType == typeof(int))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), int.Parse(value)));
				}
				else if (enumType == typeof(uint))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), uint.Parse(value)));
				}
				else if (enumType == typeof(long))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), long.Parse(value)));
				}
				else if (enumType == typeof(ulong))
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), ulong.Parse(value)));
				}
				else
				{
					field.SetValue(instance, Enum.ToObject(field.Type(), value[0]));
				}

				return true;
			}

			if (field.Is(typeof(decimal)))
			{
				field.SetValue(instance, decimal.Parse(value, CultureInfo.InvariantCulture));
				return true;
			}

			if (field.Type() == typeof(string))
			{
				field.SetValue(instance, value);
				return true;
			}

			if (field.Type() == typeof(PointF))
			{
				var (valueX, valueY) = ParseTuple(value);
				if (float.TryParse(valueX, out var x) && float.TryParse(valueY, out var y))
				{
					fieldValue = new PointF(x, y);
				}
			}

			if (fieldValue == null)
			{
				try
				{
					var convert = TypeDescriptor.GetConverter(field.Type());
					fieldValue = convert.ConvertFromString(value);
				}
				catch (Exception e)
				{
					return false;
				}
			}

			if (fieldValue != null)
			{
				field.SetValue(instance, fieldValue);
				return true;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}

		return false;
	}
}