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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Mhora.Database.Settings;
using Mhora.Database.World;
using Mhora.Elements.Calculation;
using Mhora.Elements.Hora;
using SqlNado;
using SqlNado.Query;

namespace Mhora.Components.Jhora;

public class Jhd : IFileToHoraInfo
{
	private readonly string fname;

	public Jhd(string fileName)
	{
		fname = fileName;
	}

	public HoraInfo toHoraInfo()
	{
		var  sr      = File.OpenText(fname);
		var  m       = readMomentLine(sr);
		var  tz      = readHmsLineInfo(sr, true, HMSInfo.dir_type.EW);
		var  lon     = readHmsLineInfo(sr, true, HMSInfo.dir_type.EW);
		var  lat     = readHmsLineInfo(sr, false, HMSInfo.dir_type.NS);
		var  alt     = readHmsLineInfo(sr, false, HMSInfo.dir_type.EW);
		var  est     = readHmsLineInfo(sr, false, HMSInfo.dir_type.EW);
		var  dst     = readHmsLineInfo(sr, false, HMSInfo.dir_type.EW);
		var  i1      = readIntLine(sr);
		var  i2      = readIntLine(sr);
		var  cityName = sr.ReadLine();
		var  country = sr.ReadLine();
		City worldCity = null;

		var       query  = Query.From<City>().Where(city => city.Name.ToLower() == cityName.ToLower()).SelectAll();
		var       cities = Application.WorldDb.Load<City>(query.ToString()).ToList();

		if (cities?.Count > 0)
		{
			worldCity = cities[0];
			foreach (var city in cities)
			{
				if (city.Country.Name.Equals(country, StringComparison.OrdinalIgnoreCase))
				{
					worldCity = city;
					break;
				}
			}
		}

		var hi = new HoraInfo()
		{
			DateOfBirth = m,
			Latitude    = (double) lat,
			Longitude   = (double) lon,
			City        = worldCity,
			FileType    = HoraInfo.EFileType.JagannathaHora
		};
		hi.Name = Path.GetFileNameWithoutExtension(fname);
		return hi;
	}

	public void ToFile(HoraInfo h)
	{
		var sw = new StreamWriter(fname, false);
		writeMomentLine(sw, h.DateOfBirth);
		writeHMSInfoLine(sw, h.City.Country.TimeZone.TimeZoneInfo.BaseUtcOffset.TotalHours);
		writeHMSInfoLine(sw, (double) h.Longitude);
		writeHMSInfoLine(sw, (double) h.Latitude);
		sw.WriteLine("0.000000");
		sw.Flush();
		sw.Close();
	}

	private static int readIntLine(StreamReader sr)
	{
		var s = sr.ReadLine();
		return int.Parse(s);
	}

	private static void writeHMSInfoLine(StreamWriter sw, HMSInfo hi)
	{
		string q;
		if (hi.direction == HMSInfo.dir_type.NS && hi.degree >= 0)
		{
			q = string.Empty;
		}
		else if (hi.direction == HMSInfo.dir_type.NS)
		{
			q = "-";
		}
		else if (hi.direction == HMSInfo.dir_type.EW && hi.degree >= 0)
		{
			q = "-";
		}
		else
		{
			q = string.Empty;
		}

		var thour = hi.degree >= 0 ? hi.degree : -hi.degree;
		var w     = q + thour + "." + numToString(hi.minute) + numToString(hi.second) + "00";
		sw.WriteLine(w);
	}

	private static HMSInfo readHmsLineInfo(StreamReader sr, bool negate, HMSInfo.dir_type dir)
	{
		int h = 0, m = 0, s = 0;
		readHmsLine(sr, ref h, ref m, ref s);
		if (negate)
		{
			h *= -1;
		}

		return new HMSInfo(h, m, s, dir);
	}

	private static void readHmsLine(StreamReader sr, ref int hour, ref int minute, ref int second)
	{
		var s  = sr.ReadLine();
		var re = new Regex("[0-9]*$");
		var m  = re.Match(s);
		var s2 = m.Value;

		if (s[0] == '|')
		{
			s = new string(s.ToCharArray(1, s.Length - 1));
		}

		var dhour = double.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
		dhour  = dhour < 0 ? Math.Ceiling(dhour) : Math.Floor(dhour);
		hour   = (int) dhour;
		minute = int.Parse(s2.Substring(0, 2));
		var _second = 0.0;
		if (s2.Length > 5)
		{
			_second = double.Parse(s2.Substring(2, 4)) / 10000.0 * 60.0;
		}

		second = (int) _second;
	}

	private static DateTime readMomentLine(StreamReader sr)
	{
		var month = readIntLine(sr);
		var day   = readIntLine(sr);
		var year  = readIntLine(sr);

		int hour = 0, minute = 0, second = 0;
		readHmsLine(sr, ref hour, ref minute, ref second);
		return new DateTime(year, month, day, hour, minute, second);
	}

	private static string numToString(int _n)
	{
		var    n = _n < 0 ? -_n : _n;
		string s;
		if (n < 10)
		{
			s = "0" + n;
		}
		else
		{
			s = n.ToString();
		}

		return s;
	}

	private static void writeMomentLine(StreamWriter sw, DateTime m)
	{
		sw.WriteLine(m.Month);
		sw.WriteLine(m.Day);
		sw.WriteLine(m.Year);

		sw.WriteLine(m.Hour + "." + numToString(m.Minute) + numToString(m.Second) + "00");
	}
}