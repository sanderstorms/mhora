using System;

namespace mhora.Util;

public static class LocationConverter
{
	public enum PointType
	{
		Lat,
		Lon
	}

	public static int ExtractDegrees(this double value)
	{
		return (int) value;
	}

	public static int ExtractMinutes(this double value)
	{
		value = Math.Abs(value);
		return (int) ((value - ExtractDegrees(value)) * 60);
	}

	public static int ExtractSeconds(this double value)
	{
		value = Math.Abs(value);
		var minutes = (value - ExtractDegrees(value)) * 60;
		return (int) Math.Round((minutes - ExtractMinutes(value)) * 60);
	}

	public class DecimalLocation
	{
		public double Latitude  { get; set; }
		public double Longitude { get; set; }

		public override string ToString()
		{
			return $"{Latitude:f5}, {Longitude:f5}";
		}

		public static implicit operator DmsLocation(DecimalLocation location)
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
		public DmsLocation(double longitude, double latitude)
		{
			Longitude = new DmsPoint(longitude, PointType.Lon);
			Latitude  = new DmsPoint(latitude, PointType.Lat);
		}

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

			return new DmsLocation(location.Longitude, location.Latitude);
		}
	}

	public class DmsPoint
	{
		public DmsPoint(double degrees, PointType type)
		{
			Degrees = degrees.ExtractDegrees();
			Minutes = degrees.ExtractMinutes();
			Seconds = degrees.ExtractSeconds();
			Type    = type;
		}

		public int       Degrees { get; set; }
		public int       Minutes { get; set; }
		public int       Seconds { get; set; }
		public PointType Type    { get; set; }

		public string Direction => Type == PointType.Lat ? Degrees < 0 ? "S" : "N" : Degrees < 0 ? "W" : "E";

		public override string ToString()
		{
			return $"{Math.Abs(Degrees):00} {Direction} {Minutes:00}'{Seconds:00}";
		}

		public static implicit operator double(DmsPoint point)
		{
			if (point == null)
			{
				return default;
			}

			return point.Degrees + (double) point.Minutes / 60 + (double) point.Seconds / 3600;
		}
	}
}