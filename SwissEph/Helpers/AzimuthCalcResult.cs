namespace Mhora.SwissEph.Helpers;

public class AzimuthCalcResult
{
	/// <summary>
	///     Табличная величина Азимута верхнего края солнца.
	///     Вычисляется для левой границы табличных значений широты.
	///     При стандартный условиях: долгота - 0; высота - 0; давление - 1013.25 мб; температура 10 C°.
	/// </summary>
	public double At { get; init; }

	/// <summary>
	///     Поправка азимута за широту в градусах (∆А_φ).
	///     Null для широт за пределами
	/// </summary>
	public double? dLatitude { get; init; }

	/// <summary>
	///     Поправка азимута за долготу в градусах (∆А_λ).
	/// </summary>
	public double dLongitude { get; init; }

	/// <summary>
	///     Поправка за высоту наблюдателя в минутах градусов (Δh_d).
	/// </summary>
	public double dAltitude { get; init; }

	/// <summary>
	///     Поправка за температуру в минутах градусов (Δh_t).
	/// </summary>
	public double dTemperature { get; init; }

	/// <summary>
	///     Поправка за давление в минутах градусов (∆h_p).
	/// </summary>
	public double dPressure { get; init; }

	/// <summary>
	///     Суммарная поправка за условия: ∆h = Δh_d + Δh_t + ∆h_p.
	/// </summary>
	public double dh { get; init; }

	/// <summary>
	///     Аргумант K для вычисления поправки азимута.
	///     Для широт за пределами [-60,74] имеет значение null.
	/// </summary>
	public double? K { get; init; }

	/// <summary>
	///     Итоговая поправка Азимута (∆A) расчитывается по таблице с помощью <see cref="K" /> и <see cref="dh" />.
	///     Для широт за пределами [-60,74] по специальной формуле.
	///     Величина в градусах.
	/// </summary>
	public double dAzimuth { get; init; }

	/// <summary>
	///     Азимут верхнего края солнца в градусах.
	/// </summary>
	public double AzimuthTop { get; init; }

	/// <summary>
	///     Азимут нижнего края солнца в градусах.
	/// </summary>
	public double AzimuthBot { get; init; }

	/// <summary>
	///     Разница между азимутами верхнего и нижнего края солнца
	/// </summary>
	public double dAzimuthAge { get; init; }

	/// <summary>
	///     Компасный пелинг.
	/// </summary>
	public double KP { get; init; }

	/// <summary>
	///     Разница между азимутом верхнего края и компасным пелингом.
	/// </summary>
	public double dKPTop { get; init; }

	/// <summary>
	///     Разница между азимутом нижнего края и компасным пелингом.
	/// </summary>
	public double dKPBot { get; init; }

	/// <summary>
	///     Восход (<see cref="sweph.SE_CALC_RISE" />) или Заход (<see cref="sweph.SE_CALC_SET" />).
	/// </summary>
	public int Purpose { get; set; }
}