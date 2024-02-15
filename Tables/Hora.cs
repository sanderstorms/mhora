using System;
using System.Diagnostics;
using Mhora.Definitions;

namespace Mhora.Tables;

public static class Hora
{
	// This matches the sweph definitions for easy conversion

	public enum Weekday
	{
		Monday    = 0,
		Tuesday   = 1,
		Wednesday = 2,
		Thursday  = 3,
		Friday    = 4,
		Saturday  = 5,
		Sunday    = 6
	}

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

	public static Weekday BodyToWeekday(this Body b)
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

	public static Body WeekdayRuler(this Weekday w)
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

	public static string weekdayToShortString(this Weekday w)
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
}