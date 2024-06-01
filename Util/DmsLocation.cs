namespace Mhora.Util
{
	public class DmsLocation
	{
		public DmsLocation(double longitude, double latitude)
		{
			Longitude = new DmsPoint(longitude);
			Latitude  = new DmsPoint(latitude, false);
		}

		public DmsPoint Latitude  { get; set; }
		public DmsPoint Longitude { get; set; }

		public override string ToString() => $"{Latitude.String}, {Longitude.String}";
	}
}
