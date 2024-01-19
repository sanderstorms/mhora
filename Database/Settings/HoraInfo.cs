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
using Mhora.SwissEph;
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
	private Angle  _latitude  = Angle.Empty;
	private Angle  _longitude = Angle.Empty;
	private string _name;

	//public double lon, lat, alt, tz;
	public double          defaultYearCompression;
	public double          defaultYearLength;
	public ToDate.DateType defaultYearType = ToDate.DateType.FixedYear;

	private UserEvent[] events;
	public  EFileType   FileType;

	private DateTime  _tob;
	private ChartType _type;

	protected HoraInfo(SerializationInfo info, StreamingContext context) : this()
	{
		Constructor(GetType(), info, context);
	}

	public HoraInfo()
	{
		_tob       = DateTime.Now;
		_lmtOffset = null;
		_jd        = double.NaN;
		Longitude  = MhoraGlobalOptions.Instance.Longitude;
		Latitude   = MhoraGlobalOptions.Instance.Latitude;
		Altitude   = 0.0;
		_type      = ChartType.Birth;
		FileType   = EFileType.MudgalaHora;
		events     = new UserEvent[0];
	}

	public HoraInfo(HoraInfo h)
	{
		City                   = h.City;
		Events                 = h.Events;
		Name                   = h.Name;
		Longitude              = h.Longitude;
		Latitude               = h.Latitude;
		Altitude               = h.Altitude;
		defaultYearCompression = h.defaultYearCompression;
		defaultYearLength      = h.defaultYearLength;
		defaultYearType        = h.defaultYearType;
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

	[Category(CAT_TOB)]
	[PropertyOrder(1)]
	[PGDisplayName("Time of Birth")]
	[Description("Date of Birth. Format is 'dd Mmm yyyy hh:mm:ss'\n Example 23 Mar 1979 23:11:00")]
	public DateTime DateOfBirth
	{
		get => _tob;
		set
		{
			_tob       = value;
			_lmtOffset = null;
			_jd        = double.NaN;
		}
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
	public DateTime UtcTob => TimeZoneInfo.ConvertTimeToUtc(_tob, City.Country.TimeZone.TimeZoneInfo);

	[JsonIgnore]
	public TimeSpan UtcOffset => City.Country.TimeZone.TimeZoneInfo.BaseUtcOffset;

	[JsonIgnore]
	public TimeSpan DstOffset => City.Country.TimeZone.TimeZoneInfo.GetUtcOffset(_tob);

	private Time _lmtOffset = null;
	[JsonIgnore]
	public Time LmtOffset
	{
		get
		{
			if (_lmtOffset == null)
			{
				var jd = UtcTob.UniversalTime();
				_lmtOffset = (GetLmtOffset(jd));
			}
			return _lmtOffset;
		}
	
	}

	private double _jd = double.NaN;
	[JsonIgnore]
	public double Jd
	{
		get
		{
			if (double.IsNaN(_jd))
			{
				_jd = UtcTob.UniversalTime();
				//_jd += GetLmtOffset(_jd);
			}
			return (_jd);
		}
	}

	public Time GetLmtOffset(double baseUT)
	{
		var geopos = new double[3]
		{
			Longitude,
			Latitude,
			Altitude
		};
		var tret = new double[6]
		{
			0,
			0,
			0,
			0,
			0,
			0
		};
		var midnight_ut = baseUT - _tob.Time ().TotalDays;
		sweph.Lmt(midnight_ut, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, tret);
		var lmt_noon_1   = tret[0];
		var lmt_offset_1 = lmt_noon_1 - (midnight_ut + 12.0 / 24.0);
		sweph.Lmt(midnight_ut, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, tret);
		var lmt_noon_2   = tret[0];
		var lmt_offset_2 = lmt_noon_2 - (midnight_ut + 12.0 / 24.0);

		var ret_lmt_offset = (lmt_offset_1 + lmt_offset_2) / 2.0;
		//mhora.Log.Debug("LMT: {0}, {1}", lmt_offset_1, lmt_offset_2);

		return ret_lmt_offset;
#if DND
			// determine offset from ephemeris time
			lmt_offset = 0;
			double tjd_et = baseUT + sweph.swe_deltat(baseUT);
			System.Text.StringBuilder s = new System.Text.StringBuilder(256);
			int ret = sweph.swe_time_equ(tjd_et, ref lmt_offset, s);
#endif
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