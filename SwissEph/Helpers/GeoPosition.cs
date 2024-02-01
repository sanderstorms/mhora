namespace Mhora.SwissEph.Helpers;

public class GeoPosition
{
	/// <summary>
	///     Долгота в градусах. Восточная долгота положительная, западня отрицательная.
	/// </summary>
	public double Longitude { get; set; }

	/// <summary>
	///     Широта в градусах. Северная широта положительная, южная отрицательная.
	/// </summary>
	public double Latitude { get; set; }

	/// <summary>
	///     Высота над уровнем моря в метрах.
	/// </summary>
	public double Altitude { get; set; }
}