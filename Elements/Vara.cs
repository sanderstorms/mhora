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
			Jd = new JulianDate(h.Info.UtcTob);

			JulianDate nextSunrise = h.CalcNextSolarEvent(sweph.EventType.SOLAR_EVENT_SUNRISE, Jd);

			IsDayBirth        = CalcSunriseSunset(h, out var sunrise, out var sunset, out var noon, out var midnight);
			Sunrise           = sunrise.Lmt(h);
			Sunset            = sunset.Lmt(h);
			Noon              = noon.Lmt(h);
			Midnight          = midnight.Lmt(h);
			NextSunrise       = nextSunrise.Lmt(h);
			HoursAfterSunrise = Jd.Date - sunrise;

			LmtOffset  = h.GetLmtOffset(Jd);
			LmtSunrise = 6.0  + LmtOffset * 24.0;
			LmtSunset  = 18.0 + LmtOffset * 24.0;
			
			Length    = (nextSunrise.Date - sunrise.Date);
			DayTime   = (sunset.Time - sunrise.Time);
			NightTime = (Length - DayTime);

			var date = sunrise.Date;
			DayLord  = date.DayLord();
			WeekDay  = date.DayOfWeek.WeekDay();

			HoraLord = Jd.Date.HoraLord();
			KalaLord = Jd.Date.KalaLord();
		}

		public JulianDate Jd                {get;}
		public Time       LmtOffset         {get;}
		public Time       LmtSunrise        {get;}
		public Time       LmtSunset         {get;}
		public bool       IsDayBirth        {get;}
		public JulianDate Sunrise           {get;}
		public JulianDate Sunset            {get;}
		public JulianDate Noon              {get;}
		public JulianDate Midnight          {get;}
		public JulianDate NextSunrise       {get;}
		public Time       HoursAfterSunrise {get;}
		public Time       Length            {get;}
		public Time       DayTime           {get;}
		public Time       NightTime         {get;}
		public Body       DayLord           {get;}
		public Weekday    WeekDay           {get;}
		public Body       HoraLord          {get;}
		public Body       KalaLord          {get;}

		public bool CalcSunriseSunset(Horoscope h, out JulianDate sunrise, out JulianDate sunset, out JulianDate noon, out JulianDate midnight)
		{
			bool      daybirth = true;
			double [] r        = new double[6];
			double[]  cusp     = new double[13];
			double [] ascmc    = new double [10];
			double[]  rsmi     = new double [3];
			double    startjdrise, startjdset, startjdnoon, startjdmidnight;

			h.Calc(h.Info.Jd, Body.Sun.SwephBody(), 0, r);
			h.HousesEx(h.Info.Jd, sweph.SEFLG_SIDEREAL, h.Info.Latitude, h.Info.Longitude, h.SwephHouseSystem, cusp, ascmc);

			var diff_ascsun = new Angle(ascmc[0] -  r[0] ); // Sun and AC
			if (diff_ascsun > 180.0)
			{
				daybirth = false;
			}

			double diff_icsun = new Angle(ascmc[1] + 180 - r[0]); // Sun and IC

			startjdrise = startjdset = startjdnoon = startjdmidnight = h.Info.Jd;

			if ( daybirth )
			{
				if ( diff_icsun < 180 )
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
				if ( diff_icsun < 180 )
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

		public double[] GetSunrisetCuspsUt(int dayParts)
		{
			var ret = new double[dayParts * 2 + 1];

			var srUt      = Jd  - HoursAfterSunrise / 24.0;
			var ssUt      = srUt - Sunrise / 24.0 + Sunset              / 24.0;
			var srNextUt = srUt - Sunrise / 24.0 + NextSunrise        / 24.0 + 1.0;

			var daySpan   = (ssUt      - srUt) / dayParts;
			var nightSpan = (srNextUt - ssUt) / dayParts;

			for (var i = 0; i < dayParts; i++)
			{
				ret[i] = srUt + daySpan * i;
			}

			for (var i = 0; i <= dayParts; i++)
			{
				ret[i + dayParts] = ssUt + nightSpan * i;
			}

			return ret;
		}

		public double[] GetSunrisetEqualCuspsUt(int dayParts)
		{
			var ret = new double[dayParts * 2 + 1];

			var srUt      = Jd - HoursAfterSunrise / 24.0;
			var srNextUt = srUt - Sunrise         / 24.0 + NextSunrise        / 24.0 + 1.0;
			var span       = (srNextUt - srUt) / (dayParts * 2);

			for (var i = 0; i <= dayParts * 2; i++)
			{
				ret[i] = srUt + span * i;
			}

			return ret;
		}

		public double[] GetLmtCuspsUt(int dayParts)
		{
			var ret         = new double[dayParts * 2 + 1];
			var srLmtUt     = Jd                 - HoursAfterSunrise / 24.0 - Sunrise / 24.0 + 6.0 / 24.0;
			var srLmtNextUt = srLmtUt                                                               + 1.0;
			//double sr_lmt_ut = this.info.Jd - this.info.DateOfBirth.time / 24.0 + 6.0 / 24.0;
			//double sr_lmt_next_ut = sr_lmt_ut + 1.0;

			srLmtUt     += LmtOffset;
			srLmtNextUt += LmtOffset;

			if (srLmtUt > Jd)
			{
				srLmtUt--;
				srLmtNextUt--;
			}


			var span = (srLmtNextUt - srLmtUt) / (dayParts * 2);

			for (var i = 0; i <= dayParts * 2; i++)
			{
				ret[i] = srLmtUt + span * i;
			}

			return ret;
		}
	}
}
