using Mhora.Elements;

namespace Mhora.SwissEph.Helpers;

/// <summary>
///     Позиция небесного тела.
/// </summary>
public class BodyPosition
{
	public Longitude Longitude { get; set; }

	public double Latitude { get; set; }

	public double Distance { get; set; }

	public double LongitudeSpeed { get; set; }

	public double LatitudeSpeed { get; set; }

	public double DistanceSpeed { get; set; }
}