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
using System.Runtime.Serialization;
using Mhora.Components.Property;
using Mhora.Database.World;
using Mhora.Util;
using Newtonsoft.Json;
using SqlNado.Query;

namespace Mhora.Database.Settings;

/// <summary>
///     A class containing all required input from the user for a given chart
///     (e.g.) all the information contained in a .jhd file
/// </summary>
[Serializable]
[JsonObject]
public class HoraInfo : MhoraSerializableOptions, ICloneable, ISerializable
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

	private const string CAT_TOB = "1: Birth Info";

	private const string CAT_EVT = "2: Events";
	private       double _altitude;
	
	private City   _city;
	private string _country;
	private Angle  _latitude;
	private Angle  _longitude;
	private string _name;

	//public double lon, lat, alt, tz;
	public double          defaultYearCompression;
	public double          defaultYearLength;
	public ToDate.DateType defaultYearType = ToDate.DateType.FixedYear;

	private UserEvent[] events;
	public  EFileType   FileType;

	public Moment    tob;
	public ChartType type;

	protected HoraInfo(SerializationInfo info, StreamingContext context) : this()
	{
		Constructor(GetType(), info, context);
	}

	public HoraInfo(Moment atob, Angle alat, Angle alon)
	{
		tob       = atob;
		Longitude = alon;
		Latitude  = alat;
		Altitude  = 0.0;
		type      = ChartType.Birth;
		FileType  = EFileType.MudgalaHora;
		events    = new UserEvent[0];
	}

	public HoraInfo()
	{
		var t = DateTime.Now;
		tob       = new Moment(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
		Longitude = MhoraGlobalOptions.Instance.Longitude;
		Latitude  = MhoraGlobalOptions.Instance.Latitude;
		Altitude  = 0.0;
		type      = ChartType.Birth;
		FileType  = EFileType.MudgalaHora;
		events    = new UserEvent[0];
	}

	[Category(CAT_TOB)]
	[PropertyOrder(1)]
	[PGDisplayName("Time of Birth")]
	[Description("Date of Birth. Format is 'dd Mmm yyyy hh:mm:ss'\n Example 23 Mar 1979 23:11:00")]
	public Moment DateOfBirth
	{
		get => tob;
		set => tob = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(2)]
	[Description("Latitude. Format is 'hh D mm:ss mm:ss'\n Example 23 N 24:00")]
	public Angle Latitude
	{
		get => _latitude;
		set => _latitude = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(3)]
	[Description("Longitude. Format is 'hh D mm:ss mm:ss'\n Example 23 E 24:00")]
	public Angle Longitude
	{
		get => _longitude;
		set => _longitude = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(5)]
	public double Altitude
	{
		get => _altitude;
		set => _altitude = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(8)]
	[PGDisplayName("Name")]
	public string Name
	{
		get => _name;
		set => _name = value ?? string.Empty;
	}

	[Category(CAT_EVT)]
	[PropertyOrder(1)]
	[Description("Events")]
	public UserEvent[] Events
	{
		get => events;
		set => events = value;
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
	public DateTime UtcTime => TimeZoneInfo.ConvertTimeToUtc(tob, City.Country.TimeZoneInfo);

	[JsonIgnore]
	public TimeSpan UtcOffset => City.Country.TimeZoneInfo.BaseUtcOffset;

	[JsonIgnore]
	public TimeSpan DstOffset => City.Country.TimeZoneInfo.GetUtcOffset(tob);

	[JsonIgnore]
	public TimeSpan TimeFromMidnight => new TimeSpan(0, tob.hour, tob.minute, tob.second);

	public object Clone()
	{
		var hi = new HoraInfo((Moment) tob.Clone(), Latitude, Longitude)
		{
			City			       = City,
			Events                 = Events,
			Name                   = Name,
			defaultYearCompression = defaultYearCompression,
			defaultYearLength      = defaultYearLength,
			defaultYearType        = defaultYearType
		};
		return hi;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		GetObjectData(GetType(), info, context);
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
			return (new HoraInfo());
		}
	}
}