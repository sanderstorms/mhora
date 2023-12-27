using System.Collections.Generic;

namespace mhora.Database
{
    public class TimeZone
    {
        public string id { get; set; }
        public List<string> aliases { get; set; }
        public Dictionary<string, string> location { get; set; }
        public List<string> offset { get; set; }
        public string currentOfset { get; set; }
    }

    public class TimeZones 
    {
        public List<TimeZone> zones { get; set; }

        public TimeZone FindId(string id)
        {
            TimeZone timeZone = null;
            var zoneId = id.Split('/');
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
}
