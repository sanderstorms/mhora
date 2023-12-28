using System;

namespace mhora.Util
{
    public class LocationConverter
    {
        public class DecimalLocation
        {
            public decimal Latitude  { get; set; }
            public decimal Longitude { get; set; }

            public override string ToString()
            {
                return $"{Latitude:f5}, {Longitude:f5}";
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
        }

        public class DmsPoint
        {
            public int       Degrees { get; set; }
            public int       Minutes { get; set; }
            public int       Seconds { get; set; }
            public PointType Type    { get; set; }

            public override string ToString()
            {
                return $"{Math.Abs(Degrees)} {Minutes} {Seconds} {(Type == PointType.Lat ? Degrees < 0 ? "S" : "N" : Degrees < 0 ? "W" : "E")}";
            }
        }

        public enum PointType
        {
            Lat,
            Lon
        }

        public static DecimalLocation Convert(DmsLocation dmsLocation)
        {
            if (dmsLocation == null)
            {
                return null;
            }

            return new DecimalLocation
            {
                Latitude  = CalculateDecimal(dmsLocation.Latitude),
                Longitude = CalculateDecimal(dmsLocation.Longitude)
            };
        }

        public static DmsLocation Convert(DecimalLocation decimalLocation)
        {
            if (decimalLocation == null)
            {
                return null;
            }

            return new DmsLocation
            {
                Latitude = new DmsPoint
                {
                    Degrees = ExtractDegrees(decimalLocation.Latitude),
                    Minutes = ExtractMinutes(decimalLocation.Latitude),
                    Seconds = ExtractSeconds(decimalLocation.Latitude),
                    Type    = PointType.Lat
                },
                Longitude = new DmsPoint
                {
                    Degrees = ExtractDegrees(decimalLocation.Longitude),
                    Minutes = ExtractMinutes(decimalLocation.Longitude),
                    Seconds = ExtractSeconds(decimalLocation.Longitude),
                    Type    = PointType.Lon
                }
            };
        }

        public static decimal CalculateDecimal(DmsPoint point)
        {
            if (point == null)
            {
                return default(decimal);
            }

            return point.Degrees + (decimal) point.Minutes / 60 + (decimal) point.Seconds / 3600;
        }

        public static int ExtractDegrees(decimal value)
        {
            return (int) value;
        }

        public static int ExtractMinutes(decimal value)
        {
            value = Math.Abs(value);
            return (int) ((value - ExtractDegrees(value)) * 60);
        }

        public static int ExtractSeconds(decimal value)
        {
            value = Math.Abs(value);
            decimal minutes = (value - ExtractDegrees(value)) * 60;
            return (int) Math.Round((minutes - ExtractMinutes(value)) * 60);
        }
    }
}
