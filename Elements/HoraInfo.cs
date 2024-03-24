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
using System.IO;
using System.Linq;
using Mhora.Calculation;
using Mhora.Components.Property;
using Mhora.Database.Settings;
using Mhora.Database.World;
using Mhora.Util;
using Newtonsoft.Json;
using SqlNado.Query;

namespace Mhora.Elements;

/// <summary>
///     A class containing all required input from the user for a given chart
///     (e.g.) all the information contained in a .jhd file
/// </summary>
[JsonObject]
public class HoraInfo : MhoraSerializableOptions, ICloneable
{
	public enum ChartType
	{
		Birth,
		Progression,
		TithiPravesh,
		Transit,
		Dasa
	}

	public enum EFileType
	{
		JagannathaHora,
		MudgalaHora
	}

	private const string CatTob = "1: Birth Info";

	private const string CatEvt = "2: Events";
	private       double _altitude;
	
	private City     _city;
	private string   _country;
	private DmsPoint _latitude;
	private DmsPoint _longitude;
	private string   _name;

	//public double lon, lat, alt, tz;
	public double          DefaultYearCompression;
	public double          DefaultYearLength;
	public ToDate.DateType DefaultYearType = ToDate.DateType.FixedYear;

	private UserEvent[] _events;
	public  EFileType   FileType;

	private DateTime  _tob;
	private ChartType _type;

	public HoraInfo()
	{
		_tob       = DateTime.Now;
		Longitude  = MhoraGlobalOptions.Instance.Longitude;
		Latitude   = MhoraGlobalOptions.Instance.Latitude;
		Altitude   = 0.0;
		_type      = ChartType.Birth;
		FileType   = EFileType.MudgalaHora;
		_events     = new UserEvent[0];
	}

	public HoraInfo(HoraInfo h)
	{
		_tob                   = h._tob;
		City                   = h.City;
		Events                 = h.Events;
		Name                   = h.Name;
		Longitude              = h.Longitude;
		Latitude               = h.Latitude;
		Altitude               = h.Altitude;
		DefaultYearCompression = h.DefaultYearCompression;
		DefaultYearLength      = h.DefaultYearLength;
		DefaultYearType        = h.DefaultYearType;
	}

	public object Clone()
	{
		return new HoraInfo(this);
	}

	public ChartType Type
	{
		get => _type;
		set => _type = value;
	}

	[Category(CatTob)]
	[PropertyOrder(1)]
	[PGDisplayName("Time of Birth")]
	[Description("Date of Birth. Format is 'dd Mmm yyyy hh:mm:ss'\n Example 23 Mar 1979 23:11:00")]
	public DateTime DateOfBirth
	{
		get => _tob;
		set
		{
			_tob = value;
		}
	}

	[Category(CatTob)]
	[PropertyOrder(2)]
	[Description("Latitude. Format is 'hh D mm:ss mm:ss'\n Example 23 N 24:00")]
	public DmsPoint Latitude
	{
		get => _latitude;
		set => _latitude = value;
	}

	[Category(CatTob)]
	[PropertyOrder(3)]
	[Description("Longitude. Format is 'hh D mm:ss mm:ss'\n Example 23 E 24:00")]
	public DmsPoint Longitude
	{
		get => _longitude;
		set => _longitude = value;
	}

	[Category(CatTob)]
	[PropertyOrder(5)]
	public double Altitude
	{
		get => _altitude;
		set => _altitude = value;
	}

	[Category(CatTob)]
	[PropertyOrder(8)]
	[PGDisplayName("Name")]
	public string Name
	{
		get => _name;
		set => _name = value ?? string.Empty;
	}

	[Category(CatEvt)]
	[PropertyOrder(1)]
	[Description("Events")]
	public UserEvent[] Events
	{
		get => _events;
		set => _events = value;
	}

	[JsonProperty]
	private int CityId
	{
		get
		{
			return City.Id;
		}
		set
		{
			var query  = Query.From<City>().Where(city => city.Id == value).SelectAll();
			var cities = Application.WorldDb.Load<City>(query.ToString()).ToList();
			if (cities?.Count > 0)
			{
				_city = cities[0];
			}
		}
	}

	[JsonIgnore]
	public City City
	{
		get => _city;
		set => _city = value;
	}

	[JsonIgnore]
	public TimeSpan UtcOffset => City.Country.TimeZone.TimeZoneInfo.BaseUtcOffset;

	[JsonIgnore]
	public TimeSpan DstOffset => City.Country.TimeZone.TimeZoneInfo.GetUtcOffset(_tob);

	private JulianDate _jd = 0;
	[JsonIgnore]
	public JulianDate Jd
	{
		get
		{
			if (_jd.IsEmpty)
			{
				_jd = TimeZoneInfo.ConvertTimeToUtc(_tob, City.Country.TimeZone.TimeZoneInfo);
			}
			return _jd;
		}
	}
	public void Export(string filename)
	{
		FileType = EFileType.MudgalaHora;
		JsonSerializer serializer = new JsonSerializer
		{
			NullValueHandling = NullValueHandling.Ignore,
		};

		using (StreamWriter sw = new StreamWriter(filename))
		{
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				writer.Formatting = Formatting.Indented;
				serializer.Serialize(sw, this);
			}
		}
	}

	public static HoraInfo Import(string filename)
	{
		using (StreamReader sr = new StreamReader(filename))
		{
			var serializer = new JsonSerializer();
			try
			{
				var horaInfo = (HoraInfo) serializer.Deserialize(sr, typeof(HoraInfo));
				return horaInfo;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			return new HoraInfo();
		}
	}
}