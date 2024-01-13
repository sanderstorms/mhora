using System;

namespace Mhora.SwissEph;

public struct JulDay
{
	/// <summary>
	///     Юлианский день. UTC.
	/// </summary>
	public double Day { get; }

	/// <summary>
	///     Дата соответсвующая юлианскому дню. UTC.
	/// </summary>
	public DateTime Date { get; }

	internal JulDay(double day, DateTime date)
	{
		Day  = day;
		Date = date;
	}
}