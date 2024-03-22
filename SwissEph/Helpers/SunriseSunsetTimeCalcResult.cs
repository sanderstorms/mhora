using System;
using Mhora.Util;

namespace Mhora.SwissEph.Helpers;

public class SunriseSunsetTimeCalcResult
{
	/// <summary>
	///     Поправка ко времени восхода/захода за высоту.
	/// </summary>
	public double dT { get; init; }

	/// <summary>
	///     Время восхода/захода в формате юлианского дня (UTC).
	/// </summary>
	public JulianDate JulDay { get; init; }

	/// <summary>
	///     Время восхода/захода (UTC).
	/// </summary>
	public DateTime DateTime { get; init; }
}