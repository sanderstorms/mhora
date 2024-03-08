using System;
using System.Text;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.SwissEph.Helpers;

/// <summary>
///     Обертка для вызова функиций swedll64.dll.
///     Создание и использование экземпляров в разных потоках может привести к ошибкам.
/// </summary>
internal class SweApi : IDisposable
{
	private const int DefaultStringLength = 256;

	public SweApi()
	{
		sweph.SetEphePath(null);
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public string GetVersion()
	{
		var buff = new byte[DefaultStringLength];

		sweph.Version(buff);
		return Encoding.ASCII.GetString(buff);
	}

	/// <summary>
	///     Вычисляет Юлианский день для восхода или захода Солнца.
	/// </summary>
	/// <param name="position">Географическая позиция наблюдателя.</param>
	/// <param name="pressure">Атмосферное давление в mbar/hPa.</param>
	/// <param name="temperature">Температура воздуха в градусах C.</param>
	/// <param name="purpose">
	///     Расчет времени захода или заката.
	///     Одно из значений: <see cref="sweph.SE_CALC_RISE" /> или <see cref="sweph.SE_CALC_SET" />.
	/// </param>
	/// <param name="date">Дата. UTC.</param>
	/// <returns>Время восхода/заката в формате Юлианского дня.</returns>
	public static double SunriseSunsetJulDay(GeoPosition position, double pressure, double temperature, DateTime date)
	{
		var tjd = date.ToJulian();

		var geopos = new[]
		{
			position.Longitude,
			position.Latitude,
			position.Altitude
		};
		var sterr = new StringBuilder();
		var riseTime = new double[6]
		{
			0,
			0,
			0,
			0,
			0,
			0
		};

		//var result = sweph.Rise(tjd, sweph.SE_SUN, sweph.SEFLG_SWIEPH, geopos, pressure, temperature, riseTime, sterr);

		var result = 0;
		if (result == -2)
		{
			throw new ArgumentOutOfRangeException(nameof(position), "Не удалось найти время восхода/захода");
		}

		if (result == sweph.ERR)
		{
			throw new SwedllException(sterr.ToString());
		}

		return riseTime[0];
	}

	/// <summary>
	///     Вычисляет позицию Солнца для указанного юлианского дня.
	/// </summary>
	/// <param name="jday">Юлианский день. UTC.</param>
	/// <param name="calcFlag">Тип вычислений. По умолчанию <see cref="sweph.SEFLG_EQUATORIAL" />.</param>
	/// <returns><see cref="BodyPosition" />.</returns>
	public static BodyPosition GetBodyPosition(Horoscope h, double jday, int body)
	{
		var sterr    = new StringBuilder();
		var position = new double[6];

		var result = h.CalcUT(jday, body, 0, position);

		//var result = h.CalcUT(jday, body, 0, position, sterr);
		if (result == sweph.ERR)
		{
			throw new SwedllException(sterr.ToString());
		}

		return new BodyPosition
		{
			Longitude      = position[0],
			Latitude       = position[1],
			Distance       = position[2],
			LongitudeSpeed = position[3],
			LatitudeSpeed  = position[4],
			DistanceSpeed  = position[5]
		};
	}

	/// <summary>
	///     Расчитывает горизонтальные координаты тела.
	/// </summary>
	/// <param name="jday">Юлианский день. UTC.</param>
	/// <param name="position">Позиция наблюдателя.</param>
	/// <param name="pressure">Атмосферное давление mbar/hPa.</param>
	/// <param name="temperature">Температура в градусах C</param>
	/// <param name="bodyPosition">
	///     Позиция небесного тела. Обязательны только поля <see cref="BodyPosition.Longitude" /> и
	///     <see cref="BodyPosition.Latitude" />.
	/// </param>
	/// <returns>Горизонтальные координаты тела.</returns>
	public static HorizontalCoordinates GetHorizontalCoordinates(double jday, GeoPosition position, double pressure, double temperature, BodyPosition bodyPosition)
	{
		var geopos = new[]
		{
			position.Longitude,
			position.Latitude,
			position.Altitude
		};
		var xin = new[]
		{
			(double) bodyPosition.Longitude,
			bodyPosition.Latitude,
			bodyPosition.Distance
		};
		var xout = new double[3];

		sweph.Azalt(jday, sweph.SE_ECL2HOR, geopos, pressure, temperature, xin, xout);

		return new HorizontalCoordinates
		{
			Azimuth          = xout[0],
			Altitude         = xout[1],
			ApparentAltitude = xout[2]
		};
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			sweph.Close();
		}
	}

	public static unsafe string PointerToString(byte* pointer, int lenght = DefaultStringLength)
	{
		var i = 0;
		while (i < lenght)
		{
			if (pointer[i] == 0)
			{
				break;
			}

			++i;
		}

		return Encoding.ASCII.GetString(pointer, i);
	}
}