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
using Mhora.Tables;
using Mhora.Util;
using Newtonsoft.Json;
using SqlNado;
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
	private       string _city;

	private string  _country;
	private HMSInfo _latitude;
	private HMSInfo _longitude;
	private string  _name;
	private HMSInfo _timezone;

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

	public HoraInfo(Moment atob, HMSInfo alat, HMSInfo alon, HMSInfo atz)
	{
		tob       = atob;
		Longitude = alon;
		Latitude  = alat;
		Timezone  = atz;
		Altitude  = 0.0;
		type      = ChartType.Birth;
		FileType  = EFileType.MudgalaHora;
		events    = new UserEvent[0];
	}

	public HoraInfo()
	{
		var t = DateTime.Now;
		tob       = new Moment(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second);
		Longitude = (HMSInfo) MhoraGlobalOptions.Instance.Longitude.Clone();
		Latitude  = (HMSInfo) MhoraGlobalOptions.Instance.Latitude.Clone();
		Timezone  = (HMSInfo) MhoraGlobalOptions.Instance.TimeZone.Clone();
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
	public HMSInfo Latitude
	{
		get => _latitude;
		set => _latitude = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(3)]
	[Description("Longitude. Format is 'hh D mm:ss mm:ss'\n Example 23 E 24:00")]
	public HMSInfo Longitude
	{
		get => _longitude;
		set => _longitude = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(4)]
	[PGDisplayName("Time zone")]
	[Description("Time Zone. Format is 'hh D mm:ss mm:ss'\n Example 3 E 00:00")]
	public HMSInfo Timezone
	{
		get => _timezone;
		set => _timezone = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(5)]
	public double Altitude
	{
		get => _altitude;
		set => _altitude = value;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(6)]
	[PGDisplayName("Country")]
	public string Country
	{
		get => _country;
		set => _country = value ?? string.Empty;
	}

	[Category(CAT_TOB)]
	[PropertyOrder(7)]
	[PGDisplayName("City")]
	public string City
	{
		get => _city;
		set => _city = value ?? string.Empty;
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

	private City _worldCity;
	public City WorldCity
	{
		get
		{
			if (_worldCity == null)
			{
				using var db     = new SQLiteDatabase("world.db");
				var       query  = Query.From<City>().Where(city => city.Name == _city).SelectAll();
				var       cities = db.Load<City>(query.ToString()).ToList();

				if (cities?.Count > 0)
				{
					foreach (var city in cities)
					{
						if (city.Country.Name.Equals(_country, StringComparison.OrdinalIgnoreCase))
						{
							_worldCity = city;
							break;
						}
					}
				}
			}

			return (_worldCity);
		}
	}

	public object Clone()
	{
		var hi = new HoraInfo((Moment) tob.Clone(), (HMSInfo) Latitude.Clone(), (HMSInfo) Longitude.Clone(), (HMSInfo) Timezone.Clone())
		{
			City                   = City,
			Country                = Country,
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