using System;

namespace mhora.Util
{
    public static class LocationConverter
    {
        public class DecimalLocation
        {
            public decimal Latitude  { get; set; }
            public decimal Longitude { get; set; }

            public override string ToString()
            {
                return $"{Latitude:f5}, {Longitude:f5}";
            }

            public static implicit operator DmsLocation (DecimalLocation location)
            {
                if (location == null)
                {
                    return null;
                }

                return new DecimalLocation
                {
                    Latitude  = location.Latitude,
                    Longitude = location.Longitude
                };
            }
        }

        public class DmsLocation
        {
            public DmsPoint Latitude  { get; set; }
            public DmsPoint Longitude { get; set; }

            public override string ToString()
            {
                return $"{Latitude}, {Longitude}";
            }

            public static implicit operator DecimalLocation(DmsLocation location)
            {
                if (location == null)
                {
                    return null;
                }

                return new DmsLocation
                {
                    Latitude  = new DmsPoint(location.Latitude, PointType.Lat),
                    Longitude = new DmsPoint(location.Longitude, PointType.Lon)
                };
            }
        }

        public class DmsPoint
        {
            public int       Degrees { get; set; }
            public int       Minutes { get; set; }
            public int       Seconds { get; set; }
            public PointType Type    { get; set; }

            public DmsPoint(decimal degrees, PointType type)
            {
                Degrees = degrees.ExtractDegrees();
                Minutes = degrees.ExtractMinutes();
                Seconds = degrees.ExtractSeconds();
                Type    = type;
            }

            public override string ToString()
            {
                return $"{Math.Abs(Degrees)} {Minutes} {Seconds} " +
                       $"{(Type == PointType.Lat ? 
                           Degrees < 0 ? "S" : "N" : 
                           Degrees < 0 ? "W" : "E")}";
            }

            public static implicit operator decimal(DmsPoint point)
            {
                if (point == null)
                {
                    return default(decimal);
                }

                return point.Degrees + (decimal)point.Minutes / 60 + (decimal)point.Seconds / 3600;
            }
        }

        public enum PointType
        {
            Lat,
            Lon
        }

        public static int ExtractDegrees(this decimal value)
        {
            return (int) value;
        }

        public static int ExtractMinutes(this decimal value)
        {
            value = Math.Abs(value);
            return (int) ((value - ExtractDegrees(value)) * 60);
        }

        public static int ExtractSeconds(this decimal value)
        {
            value = Math.Abs(value);
            decimal minutes = (value - ExtractDegrees(value)) * 60;
            return (int) Math.Round((minutes - ExtractMinutes(value)) * 60);
        }
    }
}
