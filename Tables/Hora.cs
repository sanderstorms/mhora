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

	public static Hora.Weekday bodyToWeekday(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return Hora.Weekday.Sunday;
			case Body.BodyType.Moon:    return Hora.Weekday.Monday;
			case Body.BodyType.Mars:    return Hora.Weekday.Tuesday;
			case Body.BodyType.Mercury: return Hora.Weekday.Wednesday;
			case Body.BodyType.Jupiter: return Hora.Weekday.Thursday;
			case Body.BodyType.Venus:   return Hora.Weekday.Friday;
			case Body.BodyType.Saturn:  return Hora.Weekday.Saturday;
		}

		Debug.Assert(false, string.Format("bodyToWeekday({0})", b));
		throw new Exception();
	}

	public static Body.BodyType weekdayRuler(Hora.Weekday w)
	{
		switch (w)
		{
			case Hora.Weekday.Sunday:    return Body.BodyType.Sun;
			case Hora.Weekday.Monday:    return Body.BodyType.Moon;
			case Hora.Weekday.Tuesday:   return Body.BodyType.Mars;
			case Hora.Weekday.Wednesday: return Body.BodyType.Mercury;
			case Hora.Weekday.Thursday:  return Body.BodyType.Jupiter;
			case Hora.Weekday.Friday:    return Body.BodyType.Venus;
			case Hora.Weekday.Saturday:  return Body.BodyType.Saturn;
			default:
				Debug.Assert(false, "Basics::weekdayRuler");
				return Body.BodyType.Sun;
		}
	}

	public static string weekdayToShortString(Hora.Weekday w)
	{
		switch (w)
		{
			case Hora.Weekday.Sunday:    return "Su";
			case Hora.Weekday.Monday:    return "Mo";
			case Hora.Weekday.Tuesday:   return "Tu";
			case Hora.Weekday.Wednesday: return "We";
			case Hora.Weekday.Thursday:  return "Th";
			case Hora.Weekday.Friday:    return "Fr";
			case Hora.Weekday.Saturday:  return "Sa";
		}

		return string.Empty;
	}
}