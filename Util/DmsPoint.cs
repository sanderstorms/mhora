using System;

namespace Mhora.Util
{
	public class DmsPoint : Angle
	{
		public DmsPoint(double degrees, bool isLongitude = true) : base(degrees)
		{
			IsLongitude = isLongitude;
		}

		public DmsPoint(int degrees, int arcminute, decimal arcsecond, bool isLongitude = true) : base(degrees, arcminute, arcsecond)
		{
			IsLongitude =isLongitude;
		}

		public static implicit operator double (DmsPoint dmsPoint) => (double)dmsPoint.Value;
		public static implicit operator DmsPoint (double degrees) => new DmsPoint(degrees);

		public bool IsLongitude
		{
			get;
		}

		public string Direction => IsLongitude ? Degrees < 0 ? "W" : "E" : Degrees < 0 ? "S" : "N" ;

		public string String => $"{Math.Abs(Degrees):00} {Direction} {Arcminute:00}'{Arcsecond:00}"; 
	}
}
