using Mhora.Definitions;
using Mhora.Util;
using System;
using Mhora.Elements.Extensions;
using Mhora.SwissEph;

namespace Mhora.Elements
{
	public class Vara
	{
		// Vara means Vedic Day. Every day of the week is associated with a planet,
		// Sunday – Sun, Monday – Moon and so on, similarly every hour is ruled by a planet and this is called ‘Hora’.
		public Vara(Horoscope h)
		{
			Horoscope = h;
			Jd        = new JulianDate(h.Info.UtcTob);

			JulianDate nextSunrise = h.CalcNextSolarEvent(sweph.EventType.SOLAR_EVENT_SUNRISE, Jd);

			IsDayBirth        = CalcSunriseSunset(h, h.Info.Jd, out var sunrise, out var sunset, out var noon, out var midnight);
			Sunrise           = sunrise.Lmt(h);
			Sunset            = sunset.Lmt(h);
			Noon              = noon.Lmt(h);
			Midnight          = midnight.Lmt(h);
			NextSunrise       = nextSunrise.Lmt(h);
			HoursAfterSunrise = Jd.Date - sunrise;
			HoursAfterSunRiseSet = IsDayBirth ? Jd.Date - sunrise : Jd.Date - sunset;

			LmtSunrise = Sunrise.Date.Lstm(h).Time();
			LmtSunset  = Sunset.Date.Lstm(h).Time();
			
			Length    = (nextSunrise.Date - sunrise.Date);
			DayTime   = (sunset.Time - sunrise.Time);
			NightTime = (Length - DayTime);

			var date = sunrise.Date;
			DayLord  = date.DayLord();
			WeekDay  = date.DayOfWeek.WeekDay();

			HoraLord = Jd.Date.HoraLord();
			KalaLord = Jd.Date.KalaLord();
		}

		public Horoscope  Horoscope            {get;}
		public JulianDate Jd                   {get;}
		public Time       LmtSunrise           {get;}
		public Time       LmtSunset            {get;}
		public bool       IsDayBirth           {get;}
		public JulianDate Sunrise              {get;}
		public JulianDate Sunset               {get;}
		public JulianDate Noon                 {get;}
		public JulianDate Midnight             {get;}
		public JulianDate NextSunrise          {get;}
		public Time       HoursAfterSunrise    {get;}
		public Time       HoursAfterSunRiseSet {get;}
		public Time       Length               {get;}
		public Time       DayTime              {get;}
		public Time       NightTime            {get;}
		public Body       DayLord              {get;}
		public Weekday    WeekDay              {get;}
		public Body       HoraLord             {get;}
		public Body       KalaLord             {get;}

		public bool CalcSunriseSunset(Horoscope h, JulianDate jd, out JulianDate sunrise, out JulianDate sunset, out JulianDate noon, out JulianDate midnight)
		{
			bool      daybirth = true;
			double [] r        = new double[6];
			double[]  cusp     = new double[13];
			double [] ascmc    = new double [10];
			double[]  rsmi     = new double [3];

			h.Calc(jd, Body.Sun.SwephBody(), 0, r);
			h.HousesEx(jd, sweph.SEFLG_SIDEREAL, h.Info.Latitude, h.Info.Longitude, h.SwephHouseSystem, cusp, ascmc);

			var diff_ascsun = new Angle(ascmc[0] -  r[0] ); // Sun and AC
			diff_ascsun.Reduce();
			if (diff_ascsun > 180.0)
			{
				daybirth = false;
			}

			var diff_icsun = new Angle(ascmc[1] + 180 - r[0]); // Sun and IC
			diff_icsun.Reduce();

			var startjdrise     = jd;
			var startjdset      = jd;
			var startjdnoon     = jd;
			var startjdmidnight = jd;

			if ( daybirth )
			{
				if ( diff_icsun < 180M)
				{
					// forenoon
					startjdrise--;
				}
				else
				{
					// afternoon
					startjdrise--;
					startjdnoon--;
				}
			}
			else
			{
				if ( diff_icsun < 180M)
				{
					// morning before sunrise
					startjdrise--;
					startjdset--;
					startjdnoon--;
					startjdmidnight--;
				}
				else
				{
					// evening after sunset
					startjdrise--;
					startjdset--;
					startjdnoon--;
				}
			}

			rsmi[0] = h.Info.Longitude;
			rsmi[1] = h.Info.Latitude;
			rsmi[2] = h.Info.Altitude;

			sunrise  = h.CalcNextSolarEvent(sweph.EventType.SOLAR_EVENT_SUNRISE, startjdrise);
			sunset   = h.CalcNextSolarEvent(sweph.EventType.SOLAR_EVENT_SUNSET, startjdset);
			midnight = h.CalcNextSolarEvent(sweph.EventType.SOLAR_EVENT_MIDNIGHT, startjdmidnight);
			noon     = h.CalcNextSolarEvent(sweph.EventType.SOLAR_EVENT_NOON, startjdnoon);

			return (daybirth);
		}

		public JulianDate[] GetSunrisetCuspsUt(int dayParts)
		{
			var ret = new JulianDate[dayParts * 2 + 1];

			Time daySpan   = DayTime.TotalHours / dayParts;
			Time nightSpan = NightTime.TotalHours / dayParts;

			for (var i = 0; i < dayParts; i++)
			{
				ret[i] = Sunrise + (daySpan * i);
			}

			for (var i = 0; i <= dayParts; i++)
			{
				ret[i + dayParts] = Sunset + nightSpan * i;
			}

			return ret;
		}

		public JulianDate[] GetSunrisetEqualCuspsUt(int dayParts)
		{
			var ret = new JulianDate[dayParts * 2 + 1];

			Time span = Length.TotalHours / (dayParts * 2);

			for (var i = 0; i <= dayParts * 2; i++)
			{
				ret[i] = Sunrise + span * i;
			}

			return ret;
		}

		public JulianDate[] GetLmtCuspsUt(int dayParts)
		{
			var ret         = new JulianDate[dayParts * 2 + 1];
			var sr          = Jd.Date - HoursAfterSunrise - Sunrise.Time + LmtSunrise;
			var srLmtUt     = new JulianDate(sr - TimeSpan.FromDays(1));
			var srLmtNextUt = new JulianDate(sr);
			//double sr_lmt_ut = this.info.Jd - this.info.DateOfBirth.time / 24.0 + 6.0 / 24.0;
			//double sr_lmt_next_ut = sr_lmt_ut + 1.0;

			if (srLmtUt > Jd)
			{
				srLmtUt--;
				srLmtNextUt--;
			}


			var span = 24.0 / (dayParts * 2);

			for (var i = 0; i <= dayParts * 2; i++)
			{
				ret[i] = srLmtUt + span * i;
			}

			return ret;
		}
	}
}
