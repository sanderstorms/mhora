using System;
using System.Diagnostics;
using Mhora.Elements;

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

	public static Weekday BodyToWeekday(this Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return Weekday.Sunday;
			case Body.BodyType.Moon:    return Weekday.Monday;
			case Body.BodyType.Mars:    return Weekday.Tuesday;
			case Body.BodyType.Mercury: return Weekday.Wednesday;
			case Body.BodyType.Jupiter: return Weekday.Thursday;
			case Body.BodyType.Venus:   return Weekday.Friday;
			case Body.BodyType.Saturn:  return Weekday.Saturday;
		}

		Debug.Assert(false, string.Format("bodyToWeekday({0})", b));
		throw new Exception();
	}

	public static Body.BodyType WeekdayRuler(this Weekday w)
	{
		switch (w)
		{
			case Weekday.Sunday:    return Body.BodyType.Sun;
			case Weekday.Monday:    return Body.BodyType.Moon;
			case Weekday.Tuesday:   return Body.BodyType.Mars;
			case Weekday.Wednesday: return Body.BodyType.Mercury;
			case Weekday.Thursday:  return Body.BodyType.Jupiter;
			case Weekday.Friday:    return Body.BodyType.Venus;
			case Weekday.Saturday:  return Body.BodyType.Saturn;
			default:
				Debug.Assert(false, "Basics::weekdayRuler");
				return Body.BodyType.Sun;
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