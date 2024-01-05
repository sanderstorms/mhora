using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace mhora.Database.World;

public class TimeZone
{
    public string                     id       { get; set; }
    public List<string>               aliases  { get; set; }
    public Dictionary<string, string> location { get; set; }
    public List<string>               offsets  { get; set; }

    private static TimeZoneCollection _timeZoneCollection;

    public static TimeZoneCollection TimeZones
    {
	    get
	    {
		    if (_timezones == null)
		    {
			    var jsonPath = Path.Combine(Mhora.Application.WorkingDir, "DataBase", "TimeZones.json");

			    // deserialize JSON directly from a file
			    using (var file = File.OpenText(jsonPath))
			    {
				    var serializer = new JsonSerializer();
				    try
				    {
					    _timeZoneCollection = (TimeZoneCollection)serializer.Deserialize(file, typeof(TimeZoneCollection));
				    }
				    catch (Exception e)
				    {
					    Console.WriteLine(e);
				    }
				    finally
				    {
					    _timezones = new List<TimeZoneInfo>();
				    }
			    }
			}

			return (_timeZoneCollection);
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

    public int DstOffset
    {
        get
        {
            if (offsets.Count == 2)
            {
                var hours = float.Parse(offsets[1]);
                return (int)(hours * 60);
            }
            return 0;
        }
    }

    private static List<TimeZoneInfo> _timezones;
    private        TimeZoneInfo       _timeZoneInfo;
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

            return (_timeZoneInfo);
        }
    }

    private TimeZoneInfo CreateTimezone()
    {
        var name         = id.Split('/');
        var minutes      = Offset;
        var dst          = DstOffset;
        var displayName  = id;
        var standardName = name.Last();
        var offset       = new TimeSpan(minutes / 60, minutes % 60, 0);

        if (dst == 0)
        {
           return TimeZoneInfo.CreateCustomTimeZone(id, offset, displayName, standardName);
        }

        var startTransition = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 4, 0, 0),
                                                                                 10, 2, DayOfWeek.Sunday);
        var endTransition = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 3, 0, 0),
                                                                               3, 2, DayOfWeek.Sunday);
        // Define adjustment rule
        var delta      = new TimeSpan(1, 0, 0);
        var adjustment = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(new DateTime(1999, 10, 1), DateTime.MaxValue.Date, delta, startTransition, endTransition);

        TimeZoneInfo.AdjustmentRule[] adjustments = { adjustment };
        return TimeZoneInfo.CreateCustomTimeZone(id, offset, displayName, standardName, standardName + " (DST)", adjustments);
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