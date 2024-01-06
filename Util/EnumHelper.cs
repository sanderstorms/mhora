using System;

namespace mhora.Util;

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
}