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
	{
		"Monday",
		"Tuesday",
		"Wednesday",
		"Thursday",
		"Friday",
		"Saturday",
		"Sunday"
	};

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
			return h.Wday.Ruler();
		}

		switch (h.Wday)
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
		switch (w)
		{
			case Weekday.Sunday:    return "Su";
			case Weekday.Monday:    return "Mo";
			case Weekday.Tuesday:   return "Tu";
			case Weekday.Wednesday: return "We";
			case Weekday.Thursday:  return "Th";
			case Weekday.Friday:    return "Fr";
			case Weekday.Saturday:  return "Sa";
		}

		return string.Empty;
	}

	public static DayOfWeek DayOfWeek (this Weekday weekday)
	{
		switch (weekday)
		{
			case Weekday.Sunday:  return System.DayOfWeek.Sunday;
			case Weekday.Monday : return System.DayOfWeek.Monday;
			case Weekday.Tuesday: return System.DayOfWeek.Tuesday;
			case Weekday.Wednesday: return System.DayOfWeek.Wednesday;
			case Weekday.Thursday:return System.DayOfWeek.Thursday;
			case Weekday.Friday:return System.DayOfWeek.Friday;
			case Weekday.Saturday:return System.DayOfWeek.Saturday;
		}
		return System.DayOfWeek.Sunday;
	}

	public static Weekday WeekDay (this DayOfWeek dayOfWeek)
	{
		switch (dayOfWeek)
		{
			case System.DayOfWeek.Sunday : return Weekday.Sunday;
			case System.DayOfWeek.Monday : return Weekday.Monday;
			case System.DayOfWeek.Tuesday : return Weekday.Tuesday;
			case System.DayOfWeek.Wednesday : return Weekday.Wednesday;
			case System.DayOfWeek.Thursday : return Weekday.Thursday;
			case System.DayOfWeek.Friday : return Weekday.Friday;
			case System.DayOfWeek.Saturday : return Weekday.Saturday;
		}

		return Weekday.Sunday;
	}

	public static Body MonthLord (this Month month)
	{
		switch (month)
		{
			case Month.January:   return Body.Sun;
			case Month.February:  return Body.Moon;
			case Month.March:     return Body.Jupiter;
			case Month.April:     return Body.Rahu;
			case Month.May:       return Body.Mercury;
			case Month.June:      return Body.Venus;
			case Month.July:      return Body.Ketu;
			case Month.August:    return Body.Saturn;
			case Month.September: return Body.Mars;
			case Month.October:   return Body.Sun;
			case Month.November:  return Body.Sun;
			case Month.December:  return Body.Jupiter;
		}

		return (Body.Sun);
	}

	public static Body DayLord(this DateTime dateTime)
	{
		if (dateTime.Hour < 6)
		{
			dateTime -= TimeSpan.FromDays(1);
		}
		return dateTime.DayOfWeek.WeekDay().Ruler();
	}

	//The 24 hours starting from the Sun’s movement from Sangyā are divided into 8 yamas,
	//each spanning for 3 hours. Each half of a yama is known as a kāla, measuring 1½ hours,
	//thereby creating 16 kālas in a day.  Each kāla is ruled by a planet starting with the day lord
	//and subsequently it follows the order of the Kāla Cakra from Sun to Rāhu.
	//The 8 kālas which exist from sunset to sunrise begin with the 7th planet from the vāra lord in the Kāla Cakra.
	public static Body KalaLord(this DateTime dateTime)
	{
		var dayLord = dateTime.DayLord();
		var index   = Array.IndexOf(Bodies.KalaOrder, dayLord);
		var hour    = dateTime.Time().TotalHours;
		var part    = (int) (hour / 1.5).Floor();

		if (hour > 18)
		{
			part += 5;
		}
		else if (hour >= 6)
		{
			part -= 4;
		}

		var lord = (index + part);
		lord %= Bodies.KalaOrder.Length;

		return Bodies.KalaOrder[lord];
	}

	public static Body HoraLord(this DateTime dateTime)
	{
		var dayLord = dateTime.DayLord();
		var index   = Array.IndexOf(Bodies.HoraOrder, dayLord);

		int hour = (dateTime.Hour - 6);
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
		{
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
		};

		return Body.Sun;
	}
}