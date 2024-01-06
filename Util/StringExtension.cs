using System.Text.RegularExpressions;

namespace Mhora.Util;

public static class StringExtension
{
	public static string ToCamelCase(this string str)
	{
		if (!string.IsNullOrEmpty(str) && str.Length > 1)
		{
			var subStr = str.Split(' ', '_', '-');
			if (subStr.Length > 1)
			{
				str = string.Empty;
				foreach (var s in subStr)
				{
					str += char.ToUpperInvariant(s[0]) + s.Substring(1).ToLower();
				}
			}
		}

		return str;
	}


	public static string CamelCase(this string s)
	{
		var x = s.Replace("_", "");
		if (x.Length == 0)
		{
			return "null";
		}

		x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])", m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
		return char.ToLower(x[0]) + x.Substring(1);
	}
}