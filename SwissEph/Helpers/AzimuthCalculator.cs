using System;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.SwissEph.Helpers;

public interface IAzimuthCalculator
{
	/// <summary>
	///     Расчитывает азимут восхода/захода Солнца воспроизводя поряд вычислений из морского астрономического ежегодника.
	/// </summary>
	/// <param name="date">Дата после которой необходимо найти ближайший восход/заход.</param>
	/// <param name="position">Географическая позиция.</param>
	/// <param name="pressure">Атмосферное давление в mbar/hPa.</param>
	/// <param name="temperature">Температура воздуха в градусах C.</param>
	/// <param name="purpose">
	///     Расчет времени захода или заката.
	///     Одно из значений: <see cref="sweph.SE_CALC_RISE" /> или <see cref="sweph.SE_CALC_SET" />.
	/// </param>
	/// <param name="KP">Значение компасного пелинга.</param>
	/// <returns>
	///     Азумут верхнего и нижнего края солнка, а также промежуточные значения вычислений -
	///     <see cref="AzimuthCalcResult" />.
	/// </returns>
	AzimuthCalcResult CalculateSunriseSunsetAzimuth(DateTime date, GeoPosition position, double pressure, double temperature, int purpose, double KP);

	/// <summary>
	///     Расчитывает точное время восхода/захода и горизонтальные координаты солнца для заданных условий.
	/// </summary>
	/// <param name="date">Дата после которой необходимо найти ближайший восход/заход.</param>
	/// <param name="position">Географическая позиция.</param>
	/// <param name="pressure">Атмосферное давление в mbar/hPa.</param>
	/// <param name="temperature">Температура воздуха в градусах C.</param>
	/// <param name="purpose">
	///     Расчет времени захода или заката.
	///     Одно из значений: <see cref="sweph.SE_CALC_RISE" /> или <see cref="sweph.SE_CALC_SET" />.
	/// </param>
	/// <returns>Кординаты и точное время восхода/захода солнца.</returns>
	(HorizontalCoordinates Coordinates, double JulDay) GetSunriseSunsetAzimuthAndTime(DateTime date, GeoPosition position, double pressure, double temperature, int purpose);

	/// <summary>
	///     Расчитывает точное время восхода/захода и горизонтальные координаты солнца для заданных условий.
	/// </summary>
	/// <param name="date">Дата после которой необходимо найти ближайший восход/заход.</param>
	/// <param name="position">Географическая позиция.</param>
	/// <param name="pressure">Атмосферное давление в mbar/hPa.</param>
	/// <param name="temperature">Температура воздуха в градусах C.</param>
	/// <param name="purpose">
	///     Расчет времени захода или заката.
	///     Одно из значений: <see cref="sweph.SE_CALC_RISE" /> или <see cref="sweph.SE_CALC_SET" />.
	/// </param>
	/// <returns>Кординаты и точное время восхода/захода солнца.</returns>
	SunriseSunsetTimeCalcResult CalculateSunriseSunsetTime(DateTime date, GeoPosition position, double pressure, double temperature, int purpose);
}

public class AzimuthCalculator
{
	public AzimuthCalcResult CalculateSunriseSunsetAzimuth(Horoscope h, DateTime date, GeoPosition position, double pressure, double temperature, int purpose, double KP)
	{
		if (position.Latitude < -90 || position.Latitude > 90 || position.Altitude < 0 || position.Altitude > 50 || position.Longitude < -180 || position.Longitude > 180)
		{
			throw new ArgumentOutOfRangeException(nameof(position));
		}

		if (pressure < AstroCatalogue.DeltaP[0][0] || pressure > AstroCatalogue.DeltaP[0][^1])
		{
			throw new ArgumentOutOfRangeException(nameof(pressure));
		}

		if (temperature < AstroCatalogue.DeltaT[0][0] || temperature > AstroCatalogue.DeltaT[0][^1])
		{
			throw new ArgumentOutOfRangeException(nameof(temperature));
		}

		if (purpose != sweph.SE_CALC_SET && purpose != sweph.SE_CALC_RISE)
		{
			throw new ArgumentOutOfRangeException(nameof(purpose));
		}

		// 1. Расчет азимута по табличным значениям широты + вычисление поправки по широте.
		// За основу берем левое табличное значение азимута. Все поправки будут прибавляться к нему. 
		(HorizontalCoordinates Coordinates, double JulDay) baseAzimuthResult;
		double                                             baseLatitude;
		double?                                            latitudeCorrection = null;
		double?                                            K                  = null;
		double                                             dA;

		if (position.Latitude >= AstroCatalogue.TableLatitudeValues[0] && position.Latitude <= AstroCatalogue.TableLatitudeValues[^1])
		{
			var (left, right) = AstroCatalogue.GetBoundaryLatitudeValues(position.Latitude, position.Latitude >= 0);

			// Пожелание заказчика для южных широт брать бОльшую широту за основу
			if (position.Latitude < 0)
			{
				(right, left) = (left, right);
			}

			baseLatitude      = left;
			baseAzimuthResult = GetStandardSunriseSunsetAzimuthAndTime(h, date, left, purpose); // A_т

			// 2 Расчет поправки за широту (∆А_φ)
			var rAzimuthResult = GetStandardSunriseSunsetAzimuthAndTime(h, date, right, purpose);
			latitudeCorrection = GetLatitudeCorrection(baseAzimuthResult.Coordinates.Azimuth, rAzimuthResult.Coordinates.Azimuth, left, right, position.Latitude);
		}
		else
		{
			baseLatitude      = position.Latitude;
			baseAzimuthResult = GetStandardSunriseSunsetAzimuthAndTime(h, date, baseLatitude, purpose); // A_т
		}

		// Расчет поправки за долготу (∆А_λ)
		var longitudeCorrection = GetLongitudeCorrection(h, date, baseAzimuthResult.Coordinates.Azimuth, baseLatitude, position.Longitude, purpose);

		// Расчет поправки за высоту (Δh_d)
		var altitudeCorrection = GetAltitudeCorrection(position.Altitude);
		// Расчет поправки за температуру (∆h_t)
		var tempCorrection = AstroCatalogue.GetInterpolatedTemperatureCorrection(temperature);
		// Расчет поправки за давление (∆h_p)
		var pressureCorrextion = AstroCatalogue.GetInterpolatedPressureCorrection(pressure);
		// Суммарная поправка за условия (∆h)
		var dh = altitudeCorrection + tempCorrection + pressureCorrextion;

		if (Math.Abs(position.Latitude) < AstroCatalogue.Kt[0][^1])
		{
			// Вычисление аргумента K по широте и месяцу
			K = AstroCatalogue.GetInterpolatedK(position.Latitude, date.Month);

			// Вычисление ∆А по K и ∆h.
			dA = AstroCatalogue.GetInterpolatedACorrection(K.Value, dh);
		}
		else
		{
			dA = GetAzimuthCorrection(baseLatitude, dh, baseAzimuthResult.Coordinates.Azimuth);
		}

		// На восходе прибавляется, на заходе поправка вычитается.
		dA = purpose == sweph.SE_CALC_RISE ? dA : -dA;

		// Вычисление азимута верхнего края солнца.
		var azimuthTop = baseAzimuthResult.Coordinates.Azimuth + dA;
		azimuthTop += latitudeCorrection ?? 0;
		azimuthTop -= longitudeCorrection;

		// Вычисление азимута нижнего края солнца.
		var dAge       = GetSunBotAgeCorrection(position.Latitude, azimuthTop, purpose);
		var azimuthBot = azimuthTop + dAge;

		var result = new AzimuthCalcResult
		{
			At           = baseAzimuthResult.Coordinates.Azimuth,
			dLatitude    = latitudeCorrection,
			dLongitude   = longitudeCorrection,
			dAltitude    = altitudeCorrection,
			dTemperature = tempCorrection,
			dPressure    = pressureCorrextion,
			dh           = dh,
			K            = K,
			dAzimuth     = dA,
			AzimuthTop   = azimuthTop,
			AzimuthBot   = azimuthBot,
			dAzimuthAge  = dAge,
			KP           = KP,
			dKPTop       = azimuthTop - KP,
			dKPBot       = azimuthBot - KP,
			Purpose      = purpose
		};

		return result;
	}

	public static (HorizontalCoordinates Coordinates, double JulDay) GetSunriseSunsetAzimuthAndTime(Horoscope h, DateTime date, GeoPosition position, double pressure, double temperature, int purpose)
	{
		using var sweApi = new SweApi();
		return GetSunriseSunsetAzimuthAndTimeInternal(h, date, position, pressure, temperature, purpose, sweApi);
	}

	/// <summary>
	///     Расчитывает точное время восхода/захода и горизонтальные координаты солнца для заданных условий.
	/// </summary>
	/// <param name="date">Дата после которой необходимо найти ближайший восход/заход.</param>
	/// <param name="position">Географическая позиция.</param>
	/// <param name="pressure">Атмосферное давление в mbar/hPa.</param>
	/// <param name="temperature">Температура воздуха в градусах C.</param>
	/// <param name="purpose">
	///     Расчет времени захода или заката.
	///     Одно из значений: <see cref="sweph.SE_CALC_RISE" /> или <see cref="sweph.SE_CALC_SET" />.
	/// </param>
	/// <returns>Кординаты и точное время восхода/захода солнца.</returns>
	public static SunriseSunsetTimeCalcResult CalculateSunriseSunsetTime(Horoscope h, DateTime date, GeoPosition position, double pressure, double temperature, int purpose)
	{
		using var sweApi = new SweApi();

		var coord    = GetSunriseSunsetAzimuthAndTimeInternal(h, date, position, pressure, temperature, purpose, sweApi);
		var dT       = GetAltitudeCorrection(position.Altitude) / (15 * Math.Cos(ToRad(position.Latitude)) * Math.Sin(ToRad(coord.Coordinates.Azimuth)));
		var timeBase = SweApi.JulDayToDateTime(coord.JulDay);
		var time     = timeBase.AddMinutes(dT);
		var jday     = time.ToJulian();

		return new SunriseSunsetTimeCalcResult
		{
			dT       = dT,
			DateTime = timeBase.AddMinutes(dT),
			JulDay   = jday
		};
	}

	private static (HorizontalCoordinates Coordinates, double JulDay) GetSunriseSunsetAzimuthAndTimeInternal(Horoscope h, DateTime date, GeoPosition position, double pressure, double temperature, int purpose, SweApi sweApi)
	{
		var jday        = SweApi.SunriseSunsetJulDay(position, pressure, temperature, date);
		var sunPosition = SweApi.GetBodyPosition(h, jday, Body.Sun.SwephBody());
		var azimuth     = SweApi.GetHorizontalCoordinates(jday, position, pressure, temperature, sunPosition);

		// Перевод в круговой счет от N
		azimuth.Azimuth = azimuth.Azimuth >= 180 ? azimuth.Azimuth -= 180d : azimuth.Azimuth += 180d;

		return (azimuth, jday);
	}

	/// <summary>
	///     Расчитывает точное время восхода/захода и горизонтальные координаты солнца при стандартных условиях.
	/// </summary>
	/// <param name="date">Дата после которой необходимо найти ближайший восход/заход.</param>
	/// <param name="latitude">Табличная широта.</param>
	/// <param name="purpose">
	///     Расчет времени захода или заката.
	///     Одно из значений: <see cref="sweph.SE_CALC_RISE" /> или <see cref="sweph.SE_CALC_SET" />.
	/// </param>
	/// <param name="sweApi"><see cref="SweApi" />.</param>
	/// <returns>Кординаты и точное время восхода/захода солнца или null если произошла ошибка.</returns>
	private static (HorizontalCoordinates Coordinates, double JulDay) GetStandardSunriseSunsetAzimuthAndTime(Horoscope h, DateTime date, double latitude, int purpose)
	{
		var position = new GeoPosition
		{
			Longitude = 0,
			Latitude  = latitude,
			Altitude  = AstroCatalogue.StandardAltitude
		};

		return GetSunriseSunsetAzimuthAndTime(h, date, position, AstroCatalogue.StandardPressure, AstroCatalogue.StandardTemperature, purpose);
	}

#region Corrections

	/// <summary>
	///     Расчитывает поправку азимута по высоте.
	/// </summary>
	/// <param name="altitude">Высота над уровнем моря в метрах.</param>
	/// <returns>Поправка по высоте</returns>
	private static double GetAltitudeCorrection(double altitude)
	{
		return -1.76 * Math.Sqrt(altitude);
	}

	/// <summary>
	///     Расчитывает поправку по широте.
	/// </summary>
	/// <param name="rAzimuth">Азимут расчитанный по правой границе табличных значений широты.</param>
	/// <param name="lAzimuth">Азимут расчитанный по левой границе табличных значений широты.</param>
	/// <param name="rLatitude">Правое табличное значение широты.</param>
	/// <param name="lLatitude">Левое табличное значение широты.</param>
	/// <param name="latitude">Широта наблюдателя.</param>
	/// <returns>Поправку по широте для значения азимута.</returns>
	private double GetLatitudeCorrection(double lAzimuth, double rAzimuth, double lLatitude, double rLatitude, double latitude)
	{
		return (rAzimuth - lAzimuth) / (rLatitude - lLatitude) * (latitude - lLatitude);
	}

	/// <summary>
	///     Расчитывает поправку по долготе.
	/// </summary>
	/// <param name="date">Дата для которой происходит вычисление азимута. UTC.</param>
	/// <param name="azimuth">Азимут расчитанный по левой границе табличных значений широты.</param>
	/// <param name="latitude">Табличное значение широты.</param>
	/// <param name="longitude">Заданное значение долготы.</param>
	/// <param name="purpose">
	///     Расчет времени захода или заката.
	///     Одно из значений: <see cref="sweph.SE_CALC_RISE" /> или <see cref="sweph.SE_CALC_SET" />.
	/// </param>
	/// <returns>Поправку по широте для значения азимута.</returns>
	private double GetLongitudeCorrection(Horoscope h, DateTime date, double azimuth, double latitude, double longitude, int purpose)
	{
		if (longitude >= 0)
		{
			var result = GetStandardSunriseSunsetAzimuthAndTime(h, date.AddDays(-1), latitude, purpose);

			return (azimuth - result.Coordinates.Azimuth) / 360d * longitude;
		}
		else
		{
			var result = GetStandardSunriseSunsetAzimuthAndTime(h, date.AddDays(1), latitude, purpose);

			return (result.Coordinates.Azimuth - azimuth) / 360d * longitude;
		}
	}

	/// <summary>
	///     Расчитывает поправку азимута для широт за пределами [-60, 74].
	/// </summary>
	/// <param name="latitude">Широта.</param>
	/// <param name="dh">Сумма поправок за высоту, давление и температуру.</param>
	/// <param name="azimuth">Азимут.</param>
	/// <returns>Поправку по широте для значения азимута.</returns>
	private double GetAzimuthCorrection(double latitude, double dh, double azimuth)
	{
		return 0.017 * Math.Tan(ToRad(latitude)) * (dh / 60d) / Math.Sin(ToRad(azimuth));
	}

	/// <summary>
	///     Расчитывает поправку для расчета азимута нижнего края по азимуту верзнего Края солнца.
	/// </summary>
	/// <param name="latitude">Широта в градусах.</param>
	/// <param name="azimuth">Азимут верхнего края солнца в градусах. В круговом счете.</param>
	/// <param name="purpose">Восход или закат.</param>
	/// <returns>Поправку азимута верхренего края солнца.</returns>
	public static double GetSunBotAgeCorrection(double latitude, double azimuth, int purpose)
	{
		// 1. Перевод в полукруговой счет
		var az = azimuth > 180 ? 360 - azimuth : azimuth;

		// ΔA = 32' / 60 * tan(latitude) * cosec(azimuth)
		var correction = 32d / 60d * Math.Tan(ToRad(latitude)) / Math.Sin(ToRad(az));

		return purpose == sweph.SE_CALC_RISE ? correction : -correction;
	}

	private static double ToRad(double angle)
	{
		return angle * Math.PI / 180d;
	}

#endregion
}