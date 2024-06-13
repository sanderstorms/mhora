using System;
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Elements;

public static class Hora
{
	// This matches the sweph definitions for easy conversion

	public static readonly string[] Weekdays =
	[
		"Monday",
		"Tuesday",
		"Wednesday",
		"Thursday",
		"Friday",
		"Saturday",
		"Sunday"
	];

	public static Weekday ToWeekday(this Body b)
	{
		switch (b)
		{
			case Body.Sun:     return Weekday.Sunday;
			case Body.Moon:    return Weekday.Monday;
			case Body.Mars:    return Weekday.Tuesday;
			case Body.Mercury: return Weekday.Wednesday;
			case Body.Jupiter: return Weekday.Thursday;
			case Body.Venus:   return Weekday.Friday;
			case Body.Saturn:  return Weekday.Saturday;
		}

		Debug.Assert(false, string.Format("bodyToWeekday({0})", b));
		throw new Exception();
	}

	public static Body Ruler(this Weekday w)
	{
		switch (w)
		{
			case Weekday.Sunday:    return Body.Sun;
			case Weekday.Monday:    return Body.Moon;
			case Weekday.Tuesday:   return Body.Mars;
			case Weekday.Wednesday: return Body.Mercury;
			case Weekday.Thursday:  return Body.Jupiter;
			case Weekday.Friday:    return Body.Venus;
			case Weekday.Saturday:  return Body.Saturn;
			default:
				Debug.Assert(false, "Basics::weekdayRuler");
				return Body.Sun;
		}
	}

	public static Body UpagrahasStart(this Horoscope h)
	{
		if (h.Vara.IsDayBirth)
		{
			return h.Vara.DayLord;
		}

		switch (h.Vara.WeekDay)
		{
			default:
			case Weekday.Sunday:    return Body.Jupiter;
			case Weekday.Monday:    return Body.Venus;
			case Weekday.Tuesday:   return Body.Saturn;
			case Weekday.Wednesday: return Body.Sun;
			case Weekday.Thursday:  return Body.Moon;
			case Weekday.Friday:    return Body.Mars;
			case Weekday.Saturday:  return Body.Mercury;
		}
	}


	public static string ShortString(this Weekday w)
	{
		return w switch
		       {
			       Weekday.Sunday    => "Su",
			       Weekday.Monday    => "Mo",
			       Weekday.Tuesday   => "Tu",
			       Weekday.Wednesday => "We",
			       Weekday.Thursday  => "Th",
			       Weekday.Friday    => "Fr",
			       Weekday.Saturday  => "Sa",
			       _                 => string.Empty
		       };
	}

	public static DayOfWeek DayOfWeek (this Weekday weekday)
	{
		return weekday switch
		       {
			       Weekday.Sunday    => System.DayOfWeek.Sunday,
			       Weekday.Monday    => System.DayOfWeek.Monday,
			       Weekday.Tuesday   => System.DayOfWeek.Tuesday,
			       Weekday.Wednesday => System.DayOfWeek.Wednesday,
			       Weekday.Thursday  => System.DayOfWeek.Thursday,
			       Weekday.Friday    => System.DayOfWeek.Friday,
			       Weekday.Saturday  => System.DayOfWeek.Saturday,
			       _                 => System.DayOfWeek.Sunday
		       };
	}

	public static Weekday WeekDay (this DayOfWeek dayOfWeek)
	{
		return dayOfWeek switch
		       {
			       System.DayOfWeek.Sunday    => Weekday.Sunday,
			       System.DayOfWeek.Monday    => Weekday.Monday,
			       System.DayOfWeek.Tuesday   => Weekday.Tuesday,
			       System.DayOfWeek.Wednesday => Weekday.Wednesday,
			       System.DayOfWeek.Thursday  => Weekday.Thursday,
			       System.DayOfWeek.Friday    => Weekday.Friday,
			       System.DayOfWeek.Saturday  => Weekday.Saturday,
			       _                          => Weekday.Sunday
		       };
	}

	public static Body MonthLord (this Month month)
	{
		return month switch
		       {
			       Month.January   => Body.Sun,
			       Month.February  => Body.Moon,
			       Month.March     => Body.Jupiter,
			       Month.April     => Body.Rahu,
			       Month.May       => Body.Mercury,
			       Month.June      => Body.Venus,
			       Month.July      => Body.Ketu,
			       Month.August    => Body.Saturn,
			       Month.September => Body.Mars,
			       Month.October   => Body.Sun,
			       Month.November  => Body.Sun,
			       Month.December  => Body.Jupiter,
			       _               => (Body.Sun)
		       };
	}

	public static Body DayLord(this DateTime dateTime)
	{
		if (dateTime.Hour < 6)
		{
			dateTime -= TimeSpan.FromDays(1);
		}
		return dateTime.DayOfWeek.WeekDay().Ruler();
	}

	public static Body HoraLord(this DateTime dateTime)
	{
		var dayLord = dateTime.DayLord();
		var index   = Array.IndexOf(Bodies.HoraOrder, dayLord);

		var hour = (dateTime.Hour - 6);
		if (hour < 0)
		{
			hour += 24;
		}

		var lord = index + (hour % Bodies.HoraOrder.Length);
		lord %= Bodies.HoraOrder.Length;

		return Bodies.HoraOrder[lord];

	}

	public static Body YearLord(this DateTime dateTime)
	{
		Body[] lords =
		[
			Body.Rahu, //2011
			Body.Mercury,
			Body.Venus,
			Body.Ketu,
			Body.Saturn,
			Body.Mars,
			Body.Sun,
			Body.Moon,
			Body.Jupiter,
			Body.Moon,
			Body.Mercury,
			Body.Moon,
			Body.Ketu,
			Body.Saturn,
			Body.Mars,
			Body.Sun,
			Body.Moon,
			Body.Jupiter,
			Body.Rahu,
			Body.Mercury //2030
		];

		return Body.Sun;
	}
}