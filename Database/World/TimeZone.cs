using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Mhora.Util;
using Newtonsoft.Json;

namespace Mhora.Database.World;

public class TimeZone
{
	private static TimeZoneCollection _timeZoneCollection;

	private static List<TimeZoneInfo>         _timezones;
	private        TimeZoneInfo               _timeZoneInfo;
	public         string                     id       { get; set; }
	public         List<string>               aliases  { get; set; }
	public         Dictionary<string, string> location { get; set; }
	public         List<string>               offsets  { get; set; }

	public static TimeZoneCollection TimeZones
	{
		get
		{
			if (_timezones == null)
			{
				var jsonPath = Path.Combine(Application.WorkingDir, "DataBase", "TimeZones.json");

				// deserialize JSON directly from a file
				using (var file = File.OpenText(jsonPath))
				{
					var serializer = new JsonSerializer();
					try
					{
						_timeZoneCollection = (TimeZoneCollection) serializer.Deserialize(file, typeof(TimeZoneCollection));
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}
			}

			return _timeZoneCollection;
		}
	}

	public Angle Latitude
	{
		get
		{
			var str = "0.0";
			if (location.TryGetValue("latitude", out str))
			{
				if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var latitude))
				{
					return latitude;
				}
			}

			return 0.0;
		}
	}

	public Angle Longitude
	{
		get
		{
			var str = "0.0";
			if (location.TryGetValue("longitude", out str))
			{
				if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var longitude))
				{
					return longitude;
				}
			}

			return 0.0;

		}
	}

	public int Offset
	{
		get
		{
			var hours = float.Parse(offsets[0]);
			return (int) (hours * 60);
		}
	}

	public TimeSpan DstOffset
	{
		get
		{
			TimeSpan time = TimeSpan.Zero;
			if (offsets.Count == 2)
			{
				var str  = offsets[1].Split(':');

				if (float.TryParse(str[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var hours))
				{
					time = TimeSpan.FromHours(hours);
				}

				if (str.Length == 2)
				{
					if (float.TryParse(str[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var minutes))
					{
						if (hours < 0)
						{
							minutes = -minutes;
						}
						time = time.Add(TimeSpan.FromMinutes(minutes));
					}
				}
			}

			return (time);
		}
	}

	public TimeZoneInfo TimeZoneInfo
	{
		get
		{
			try
			{
				if (_timeZoneInfo == null)
				{
					var name = id.Split('/');
					_timezones ??= TimeZoneInfo.GetSystemTimeZones().ToList();
					var regions = _timezones.FindAll(zone => zone.Id.Contains(name[0]));
					if (regions.Count > 0)
					{
						_timeZoneInfo = regions.Find(zone => zone.DisplayName.Contains(name[1]));
					}

					foreach (var alias in aliases)
					{
						name          = id.Split('/');
						_timeZoneInfo = _timezones.Find(zone => zone.DisplayName.Contains(name[1]));
						if (_timeZoneInfo != null)
						{
							break;
						}
					}
				}

				return _timeZoneInfo ??= CreateTimezone();
			}
			catch (Exception e)
			{
			}

			return _timeZoneInfo;
		}
	}

	private TimeZoneInfo CreateTimezone()
	{
		try
		{
			var name         = id.Split('/');
			var minutes      = Offset;
			var dst          = DstOffset;
			var displayName  = id;
			var standardName = name.Last();
			var offset       = new TimeSpan(minutes / 60, minutes % 60, 0);

			if (dst.TotalHours == 0)
			{
				return TimeZoneInfo.CreateCustomTimeZone(id, offset, displayName, standardName);
			}

			var endTransition   = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 4, 0, 0), 10, 2, DayOfWeek.Sunday);
			var startTransition = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 3, 0, 0), 3, 2, DayOfWeek.Sunday);
			// Define adjustment rule
			var delta      = new TimeSpan(1, 0, 0);
			var adjustment = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(new DateTime(1999, 10, 1), DateTime.MaxValue.Date, delta, startTransition, endTransition);

			TimeZoneInfo.AdjustmentRule[] adjustments =
			[
				adjustment
			];
			return TimeZoneInfo.CreateCustomTimeZone(id, offset, displayName, standardName, standardName + " (DST)", adjustments);
		}
		catch (Exception e)
		{
			return (TimeZoneInfo.Local);
		}
	}
}

public class TimeZoneCollection
{
	public List<TimeZone> zones { get; set; }

	public TimeZone FindId(string id)
	{
		TimeZone timeZone = null;
		var      zoneId   = id.Split('/');
		if (zoneId.Length == 2)
		{
			var regions = zones.FindAll(zone => zone.id.Contains(zoneId[0]));
			timeZone = regions.Find(zone => zone.id.Contains(zoneId[1]));

			if (timeZone == null)
			{
				foreach (var region in regions)
				{
					if (region.aliases.Find(alias => alias.Contains(zoneId[1])) != null)
					{
						timeZone = region;
						break;
					}
				}
			}
		}

		return timeZone;
	}
}