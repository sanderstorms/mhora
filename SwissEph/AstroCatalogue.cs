using System;
using System.Collections.Generic;
using System.Linq;

namespace Mhora.SwissEph;

public static class AstroCatalogue
{
	/// <summary>
	///     Стандартные условия температуры в C°.
	/// </summary>
	public const double StandardTemperature = 10;

	/// <summary>
	///     Стандартные условия атмосферного давления (hPa).
	/// </summary>
	public const double StandardPressure = 1013.25;

	/// <summary>
	///     Стандартные условия высоты над уровнем моря в метрах.
	/// </summary>
	public const double StandardAltitude = 0;

	/// <summary>
	///     Табличные значения широты для которых приводиться азимут.
	/// </summary>
	public static readonly IReadOnlyList<double> TableLatitudeValues = new double[]
	{
		-60,
		-58,
		-56,
		-54,
		-52,
		-50,
		-45,
		-40,
		-30,
		-20,
		-10,
		0,
		10,
		20,
		30,
		40,
		45,
		50,
		52,
		54,
		56,
		58,
		60,
		62,
		64,
		66,
		68,
		70,
		72,
		74
	};

	/// <summary>
	///     Поправка высоты светила за температуру (C°) воздуха при видимой высоте 0.
	/// </summary>
	public static IReadOnlyList<IReadOnlyList<double>> DeltaT = new double[2][]
	{
		new[]
		{
			-40.0,
			-35.0,
			-30.0,
			-25.0,
			-20.0,
			-15.0,
			-10.0,
			-5.0,
			0.00,
			5.00,
			10.0,
			15.0,
			20.0,
			25.0,
			30.0,
			35.0,
			40.0
		},
		new[]
		{
			-13.6,
			-11.9,
			-10.2,
			-8.70,
			-7.30,
			-5.90,
			-4.60,
			-3.4,
			-2.2,
			-1.1,
			0.00,
			1.00,
			2.00,
			2.90,
			3.80,
			4.70,
			5.50
		}
	};

	/// <summary>
	///     Поправка высоты светила за давление (мб) воздуха при видимой высоте 0.
	/// </summary>
	public static IReadOnlyList<IReadOnlyList<double>> DeltaP = new double[2][]
	{
		new[]
		{
			960.0,
			967.0,
			973.0,
			980.0,
			987.0,
			993.0,
			1000.0,
			1007.0,
			1013.0,
			1020.0,
			1027.0,
			1033.0,
			1040.0,
			1047.0,
			1053.0
		},
		new[]
		{
			2.000,
			1.700,
			1.500,
			1.200,
			1.000,
			0.700,
			0.5000,
			0.2000,
			0.0000,
			-0.200,
			-0.500,
			-0.700,
			-1.000,
			-1.300,
			-1.600
		}
	};

	/// <summary>
	///     Оригинальная таблица определения аргумента K по дате и широте из ежегодника.
	///     ось x - месяц
	///     ось y - широта
	/// </summary>
	public static IReadOnlyList<IReadOnlyList<double>> K = new double[12][]
	{
		new[]
		{
			60,
			0.04,
			0.03,
			0.03,
			0.03,
			0.04,
			0.05,
			0.05,
			0.04,
			0.03,
			0.03,
			0.03,
			0.04
		},
		new[]
		{
			58,
			0.04,
			0.03,
			0.03,
			0.03,
			0.03,
			0.04,
			0.04,
			0.03,
			0.03,
			0.03,
			0.03,
			0.04
		},
		new[]
		{
			56,
			0.03,
			0.03,
			0.03,
			0.03,
			0.03,
			0.03,
			0.04,
			0.03,
			0.03,
			0.02,
			0.03,
			0.03
		},
		new[]
		{
			54,
			0.03,
			0.03,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03,
			0.03,
			0.02,
			0.02,
			0.02,
			0.03
		},
		new[]
		{
			52,
			0.03,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03
		},
		new[]
		{
			50,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02
		},
		new[]
		{
			45,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02
		},
		new[]
		{
			40,
			0.02,
			0.02,
			0.01,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.01,
			0.01,
			0.02
		},
		new[]
		{
			30,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01
		},
		new[]
		{
			20,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01,
			0.01
		},
		new[]
		{
			10,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00
		},
		new[]
		{
			00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00
		}
	};

	/// <summary>
	///     Транспанированная таблица определения аргумента K по дате и широте из ежегодника.
	///     ось x - широта
	///     ось y - месяц
	/// </summary>
	public static IReadOnlyList<IReadOnlyList<double>> Kt = new double[13][]
	{
		new[]
		{
			0.0,
			10,
			20,
			30,
			40,
			45,
			50,
			52,
			54,
			56,
			58,
			60
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03,
			0.04,
			0.04
		}, // январь
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03,
			0.03
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03,
			0.04
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03,
			0.04,
			0.05
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.02,
			0.03,
			0.03,
			0.03,
			0.03,
			0.04,
			0.04,
			0.05
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03,
			0.04
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03
		},
		new[]
		{
			0.0,
			0.0,
			0.01,
			0.01,
			0.02,
			0.02,
			0.02,
			0.03,
			0.03,
			0.03,
			0.04,
			0.04
		} // декабрь
	};

	/// <summary>
	///     Поправка азимута по K и ∆h.
	///     ось x - ∆h
	///     ocь y - K
	/// </summary>
	public static IReadOnlyList<IReadOnlyList<double>> DeltaA = new double[7][]
	{
		new[]
		{
			0.0,
			1.0,
			1.5,
			2.0,
			2.5,
			3.0,
			3.5,
			4.0,
			4.5,
			5.0,
			5.5,
			6.0,
			6.5,
			7.0,
			8.0,
			9.0,
			10.0,
			11.0,
			12.0,
			13.0,
			14.0,
			15.0,
			16.0,
			17.0,
			18.0,
			19.0,
			20.0,
			21.0,
			22.0,
			23.0,
			24.0,
			25.0,
			26.0
		},
		/* K = 0.00 */
		new[]
		{
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00,
			0.00
		},
		/* K = 0.01 */
		new[]
		{
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.10,
			0.10,
			0.10,
			0.10,
			0.10,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20,
			0.20
		},
		/* K = 0.02 */
		new[]
		{
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.2,
			0.1,
			0.2,
			0.20,
			0.20,
			0.20,
			0.30,
			0.30,
			0.30,
			0.30,
			0.30,
			0.40,
			0.40,
			0.40,
			0.40,
			0.40,
			0.50,
			0.50,
			0.50,
			0.50
		},
		/* K = 0.03 */
		new[]
		{
			0.0,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.2,
			0.2,
			0.2,
			0.2,
			0.3,
			0.30,
			0.30,
			0.40,
			0.40,
			0.40,
			0.40,
			0.50,
			0.50,
			0.50,
			0.60,
			0.60,
			0.60,
			0.70,
			0.70,
			0.70,
			0.80,
			0.80
		},
		/* K = 0.04 */
		new[]
		{
			0.0,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.1,
			0.2,
			0.2,
			0.2,
			0.2,
			0.2,
			0.3,
			0.3,
			0.3,
			0.4,
			0.40,
			0.40,
			0.50,
			0.50,
			0.60,
			0.60,
			0.70,
			0.70,
			0.70,
			0.80,
			0.80,
			0.80,
			0.90,
			0.90,
			1.00,
			1.00,
			1.00
		},
		/* K = 0.05 */
		new[]
		{
			0.0,
			0.1,
			0.1,
			0.1,
			0.1,
			0.2,
			0.2,
			0.2,
			0.2,
			0.2,
			0.2,
			0.3,
			0.3,
			0.4,
			0.4,
			0.4,
			0.50,
			0.60,
			0.60,
			0.60,
			0.70,
			0.80,
			0.80,
			0.80,
			0.90,
			1.00,
			1.00,
			1.00,
			1.20,
			1.20,
			1.20,
			1.20,
			1.20
		}
	};

	/// <summary>
	///     Вычисляет ближайшие табличные значения широты.
	/// </summary>
	/// <param name="latitude">Широта ближайшие значения к которой надо найти.</param>
	/// <param name="leftshift">
	///     Если <paramref name="value" /> выпало ровно на элемент массива, то какой он должен быть
	///     границей левой (true) или правой (false).
	/// </param>
	/// <returns>Левое и правое табличные значения.</returns>
	public static (double Left, double Right) GetBoundaryLatitudeValues(double latitude, bool leftshift = true)
	{
		var (left, right) = GetBoundaryIndexes(latitude, TableLatitudeValues, leftshift);
		return (TableLatitudeValues[left], TableLatitudeValues[right]);
	}

	/// <summary>
	///     Вычисляет поправку высоты светила солнца за температуру интерполируя между ближайшими табличными значениями.
	/// </summary>
	/// <param name="temperature">Заданная темперетура (C°).</param>
	/// <returns>Поправку высоты за температуру.</returns>
	public static double GetInterpolatedTemperatureCorrection(double temperature)
	{
		var (l, r) = GetBoundaryIndexes(temperature, DeltaT[0]);
		return InterpolateLinearly(temperature, l, r, DeltaT);
	}

	/// <summary>
	///     Вычисляет поправку высоты светила солнца за давления интерполируя между ближайшими табличными значениями.
	/// </summary>
	/// <param name="pressure">Заданное давление (мб).</param>
	/// <returns>Поправку высоты за давление.</returns>
	public static double GetInterpolatedPressureCorrection(double pressure)
	{
		var (l, r) = GetBoundaryIndexes(pressure, DeltaP[0]);
		return InterpolateLinearly(pressure, l, r, DeltaP);
	}

	/// <summary>
	///     Вычисляет аргумент K интерполируя между ближайшими табличными значениями широты.
	/// </summary>
	/// <param name="latitude">Заданное давление (мб).</param>
	/// <returns>Поправку высоты за давление.</returns>
	public static double GetInterpolatedK(double latitude, int month)
	{
		latitude   = Math.Abs(latitude);
		var (l, r) = GetBoundaryIndexes(latitude, Kt[0]);
		return InterpolateLinearly(latitude, l, r, new[]
		{
			Kt[0],
			Kt[month]
		});
	}

	/// <summary>
	///     Вычисляет аргумент K интерполируя между ближайшими табличными значениями широты.
	/// </summary>
	/// <param name="latitude">Заданное давление (мб).</param>
	/// <returns>Поправку высоты за давление.</returns>
	public static double GetInterpolatedACorrection(double K, double dh)
	{
		if (K < 0 || K > 0.05)
		{
			throw new ArgumentOutOfRangeException(nameof(K), $"K ({K}) находиться за пределами табличных значений.");
		}

		var dhAbs = Math.Abs(dh);
		if (dhAbs < DeltaA[0][0] || dhAbs > DeltaA[0].Last())
		{
			throw new ArgumentOutOfRangeException(nameof(dh), $"dh ({dh}) находиться за пределами табличных значений.");
		}

		var lK = (int) Math.Truncate(K * 100) + 1; // + 1 т.к. 0я строчка это значения dh.
		var rK = lK                           + 1;
		var (ldh, rdh) = GetBoundaryIndexes(dhAbs, DeltaA[0]);
		var lInterpolated = InterpolateLinearly(dhAbs, ldh, rdh, new[]
		{
			DeltaA[0],
			DeltaA[lK]
		});
		var rInterpolated = InterpolateLinearly(dhAbs, ldh, rdh, new[]
		{
			DeltaA[0],
			DeltaA[rK]
		});

		var result = lInterpolated + (rInterpolated - lInterpolated) / 0.01 * (K - (lK - 1) / 100d);

		return dh < 0 ? -result : result;
	}

	/// <summary>
	///     Вычисляет ближайшие табличные значения к заданному.
	/// </summary>
	/// <param name="value">Заданное значение ближайшие к которому надо найти.</param>
	/// <param name="array">Массив чисел для нахождения левой и правой границы.</param>
	/// <param name="leftshift">
	///     Если <paramref name="value" /> выпало ровно на элемент массива, то какой он должен быть
	///     границей левой (true) или правой (false).
	/// </param>
	/// <returns>Левое и правое табличные значения.</returns>
	private static (int Left, int Right) GetBoundaryIndexes(double value, IReadOnlyList<double> array, bool leftshift = true)
	{
		var                        l       = 0;
		var                        r       = array.Count - 1;
		Func<double, double, bool> compare = leftshift ? (v1, v2) => v1 >= v2 : (v1, v2) => v1 > v2;

		if (value > array[r] || value < array[l])
		{
			throw new ArgumentOutOfRangeException(nameof(value));
		}

		while (r - l > 1)
		{
			if (compare(value, array[(r + l) / 2]))
			{
				l = (r + l) / 2;
			}
			else
			{
				r = (r + l) / 2;
			}
		}

		return (l, r);
	}

	private static double InterpolateLinearly(double value, int l, int r, IReadOnlyList<IReadOnlyList<double>> table)
	{
		var dTemp  = table[0][l]                     - table[0][r];
		var dValue = table[1][l]                     - table[1][r];
		return table[1][l] + dValue / dTemp * (value - table[0][l]);
	}
}