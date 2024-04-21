using Mhora.Definitions;
using Mhora.Util;
using System;
using Mhora.Calculation;
using Mhora.Database.Settings;
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
			Jd        = h.Info.Jd;

			JulianDate nextSunrise = h.CalcNextSolarEvent(sweph.EventType.SOLAR_EVENT_SUNRISE, Jd);

			IsDayBirth   = CalcSunriseSunset(h, h.Info.Jd, out var sunrise, out var sunset, out var noon, out var midnight);
			Sunrise      = sunrise.Lmt(h);
			Sunset       = sunset.Lmt(h);
			Noon         = noon.Lmt(h);
			Midnight     = midnight.Lmt(h);
			NextSunrise  = nextSunrise.Lmt(h);
			Isthaghati   = Jd.Date - sunrise;
			JanmaVighati = IsDayBirth ? sunset - Jd.Date : Jd.Date - sunset;

			LmtSunrise = Sunrise.Date.Lstm(h).Time();
			LmtSunset  = Sunset.Date.Lstm(h).Time();
			
			Length    = (nextSunrise.Date - sunrise.Date);
			DayTime   = (sunset.Time - sunrise.Time);
			NightTime = (Length - DayTime);

			var date = sunrise.Date;
			WeekDay  = date.DayOfWeek.WeekDay();
			DayLord  = WeekDay.Ruler();
			HoraLord = Jd.Date.HoraLord();
			KalaLord = CalculateKalaLord(Isthaghati);

			//	Janma Vighatika Graha
			// 1. The moment of birth is always considered from the Sunrise or Sunset. So the start of the vighatikas
			//    for finding the ruler of the moment of birth has to be reckoned from the Sunrise or Sunset based on birth
			//    happening before of after the Sunset.
			// 2. The time of birth elapsed after Sunrise of Sunset is converted into Vighatis. (1 Hr = 2.5 Ghati = 150 vighati).
			// 3. The ruler of the vighatikas is based on the sequence of the lords of the weekdays
			//    (Su-1, Mo-2, Mars- 3, Me-4, Ju- 5, Ve- 6, Sa- 7, Ra- 8, Ke- 9), starting from Sun.
			//    To arrive at the lord of the ruler, divide the Vighatis arrived at the previous step by 9
			//    and round it up to the higher interger and count that number from Sun.

			VighatikaGraha = (Body) (int) (JanmaVighati.Vighati % 9);

			BirthTatva           = CalculateBirthTatva();
			Gulika               = CalculateUpgraha(Body.Saturn);
			Maandi               = CalculateUpgraha(Body.Saturn, true, HoroscopeOptions.EUpagrahaType.End);
			(YamaLord, YamaSpan) = CalculateYamaLord();

			(RahuKalam, Yamagandam, GulikaKalam) = CalculateKalam();
		}

		public Horoscope  Horoscope      {get;}
		public JulianDate Jd             {get;}
		public Time       LmtSunrise     {get;}
		public Time       LmtSunset      {get;}
		public bool       IsDayBirth     {get;}
		public JulianDate Sunrise        {get;}
		public JulianDate Sunset         {get;}
		public JulianDate Noon           {get;}
		public JulianDate Midnight       {get;}
		public JulianDate NextSunrise    {get;}
		public JulianDate RahuKalam      {get;}
		public JulianDate Yamagandam     {get;}
		public JulianDate GulikaKalam    {get;}
		public Time       Isthaghati     {get;} //Hours after sunrise
		public Time       JanmaVighati   {get;} //Hours after sunrise or sunset
		public Time       Length         {get;}
		public Time       DayTime        {get;}
		public Time       NightTime      {get;}
		public Body       DayLord        {get;}
		public Weekday    WeekDay        {get;}
		public Body       HoraLord       {get;}
		public Body       KalaLord       {get;}
		public Body       YamaLord       {get;}
		public Body       VighatikaGraha {get;}
		public Yama       YamaSpan       {get;}
		public BirthTatva BirthTatva     {get;}
		public Longitude  Gulika         {get;}
		public Longitude  Maandi         {get;}

		public Longitude BhriguBindu
		{
			get
			{
				var moon = Horoscope.GetPosition(Body.Moon).Longitude;
				var rahu = Horoscope.GetPosition(Body.Rahu).Longitude;
				var bb = rahu.Value;
				if (rahu > moon)
				{
					bb -= ((rahu + 360.0 - moon) / 2.0);
				}
				else
				{
					bb += ((rahu - moon) / 2.0);
				}

				return new Longitude(bb);

			}
		}

		// Find the time of sunrise and sunset of a day in your city.
		// Divide this duration in 8 equal parts.
		// On Mondays, 2nd part; on Tuesdays, 7th part; on Wednesdays, 5th part; on Thursdays, 6th part;
		// on Fridays, 4th part; on Saturdays, 3rd part; and on Sundays, 8th part is called Rahukalam.
		// Yamagandam: 4 -3 - 2 - 1 - 7 - 6- 5
		// Gulika	 : 6 - 5 - 4 - 3 - 2 - 1 - 7
		//  			Rahu Kaal		Yamagandam		Gulika
		// Monday		7:30 – 9:00		10:30 - 12:00	13:30 – 15:00
		// Tuesday		15:00 – 16:30	9:00 – 10:30	12:00 – 13:30
		// Wednesday	12:00 – 13:30	7:30 – 9:00		10:30 – 12:00
		// Thursday		13:30 – 15:00	6:00 – 7:30		9:00 – 10:30
		// Friday		10:30 – 12:00	15:00 – 16:30	7:30 – 9:00
		// Saturday		9:00 – 10:30	13:30 – 15:00	6:00 – 7:30
		// Sunday		16:30 – 18:00	12:00 – 13:30	15:00 – 16:30
		private (JulianDate, JulianDate, JulianDate) CalculateKalam()
		{
			var rahu   = new [] {2, 7, 5, 6, 4, 3, 8};
			var yama   = new [] {4, 3, 2, 1, 7, 5, 5};
			var gulika = new [] {6, 5, 4, 3, 2, 1, 7};

			var cusps = GetSunrisetCuspsUt(8);

			var rahuKalam   = cusps[rahu[WeekDay.Index()] - 1];
			var yamagandam  = cusps[yama[WeekDay.Index()] - 1];
			var gulikaKalam = cusps[gulika[WeekDay.Index()] - 1];

			return (rahuKalam, yamagandam, gulikaKalam);
		}

		//The 24 hours starting from the Sun’s movement from Sangyā are divided into 8 yamas,
		//each spanning for 3 hours. Each half of a yama is known as a kāla, measuring 1½ hours,
		//thereby creating 16 kālas in a day.  Each kāla is ruled by a planet starting with the day lord
		//and subsequently it follows the order of the Kāla Cakra from Sun to Rāhu.
		//The 8 kālas which exist from sunset to sunrise begin with the 7th planet from the vāra lord in the Kāla Cakra.
		public Body CalculateKalaLord(Time hoursAfterSunrise, bool equalParts = true)
		{
			var index = Array.IndexOf(Bodies.KalaOrder, DayLord);
			var hour  = hoursAfterSunrise.TotalHours;
			int part  = 0;

			if (equalParts)
			{
				var yama = (Length / 16);
				part = (int) (hour / yama.TotalHours);
			}
			else
			{
				if (hoursAfterSunrise > DayTime)
				{
					var yama = NightTime / 8;
					hour -= DayTime.TotalHours;
					part =  (int) (hour / yama.TotalHours) + 8;
				}
				else
				{
					var yama = DayTime / 8;
					part = (int) (hour  / yama.TotalHours);
				}
			}

			if (part >= 8)
			{
				part  -= 8;
				index += 4;
			}

			var lord = (index + part);
			lord %= Bodies.KalaOrder.Length;

			return Bodies.KalaOrder[lord];
		}

		//Yama of Sun = Kala Span (of time)
		//Yama of Moon = Paridhi Span (of time)
		//Yama of Mars = Dhooma Span (of time)
		//Yama of Mercury = Arthaprahara Span (of time)
		//Yama of Jupiter = Yemakandaka Span (of time)
		//Yama of Venus = Yamasukra Span (of time)
		//Yama of Saturn = Gulika Span (of time)
		public JulianDate FindKalaCusp(Body body, bool equalParts, HoroscopeOptions.EUpagrahaType upagrahaType)
		{
			var part  = 0;
			var cusps = equalParts ? GetSunrisetEqualCuspsUt(8) : GetSunrisetCuspsUt(8);

			var offset = upagrahaType switch
			             {
				             HoroscopeOptions.EUpagrahaType.Begin => 0,
				             HoroscopeOptions.EUpagrahaType.Mid   => (cusps[1].Time - cusps[0].Time).TotalHours / 2,
				             HoroscopeOptions.EUpagrahaType.End   => (cusps[1].Time - cusps[0].Time),
				             _                                    => Time.Zero
			             };

			if (IsDayBirth == false)
			{
				part = 8;
			}

			for (var dayPart = part; dayPart < part + 8; dayPart++)
			{
				var hours = (cusps[dayPart].Date - Sunrise);
				if (CalculateKalaLord (hours, equalParts) == body)
				{
					return cusps[dayPart] + offset;
				}
			}

			return cusps[0];
		}

		// The position of Gulika is different for daytime (from sunrise to sunset) and night - time
		// (from sunset to sunrise). The duration of the day or of the night (as the case may be) is
		// divided into eight parts.The segment belonging to Saturn is known as Gulika
		// The cusp of the sign rising at the beginning of the Gulika segment is
		// considered as Gulika. From this, the chart must be analyzed.
		//
		// Mandi Nadi are 26, 22, 18, 14, 10, 6, 2 for each day starting from Sunday. The
		// differences in duration of day should be considered.
		// To find the days Mandi Udaya time, The Dina Pramana (day or night) should be
		// multiplied by Mani Udaya Ghati and expunged by 30. The sign thus arrived is
		// the Rising sign of Mandi
		public Longitude CalculateUpgraha(Body body, bool equalParts = true, HoroscopeOptions.EUpagrahaType upagrahaType = HoroscopeOptions.EUpagrahaType.Begin)
		{
			var upgraha = FindKalaCusp(body, equalParts, upagrahaType);
			return new Longitude(Horoscope.Lagna(upgraha));
		}

		private double[] _houseCusps;
		public bool CalcSunriseSunset(Horoscope h, JulianDate jd, out JulianDate sunrise, out JulianDate sunset, out JulianDate noon, out JulianDate midnight)
		{
			bool      daybirth = true;
			double [] r        = new double[6];
			double[]  cusps    = new double[13];
			double [] ascmc    = new double [10];
			double[]  rsmi     = new double [3];

			h.Calc(jd, Body.Sun.SwephBody(), 0, r);
			h.HousesEx(jd, sweph.SEFLG_SIDEREAL, h.Info.Latitude, h.Info.Longitude, h.SwephHouseSystem, cusps, ascmc);
			_houseCusps ??= cusps;

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

		private JulianDate [] _horaCusps;
		public JulianDate[] HoraCuspsUt
		{
			get
			{
				_horaCusps ??= Horoscope.Options.HoraType switch
				{
					HoroscopeOptions.EHoraType.Sunriset      => GetSunrisetCuspsUt(12),
					HoroscopeOptions.EHoraType.SunrisetEqual => GetSunrisetEqualCuspsUt(12),
					HoroscopeOptions.EHoraType.Lmt           => GetLmtCuspsUt(12),
					_                                        => null
				};
				return _horaCusps;
			}
		}

		private JulianDate[] _kalaCusps;
		public JulianDate[] KalaCuspsUt
		{
			get
			{
				_kalaCusps ??= Horoscope.Options.KalaType switch
                 {
                     HoroscopeOptions.EHoraType.Sunriset      => GetSunrisetCuspsUt(8),
                     HoroscopeOptions.EHoraType.SunrisetEqual => GetSunrisetEqualCuspsUt(8),
                     HoroscopeOptions.EHoraType.Lmt           => GetLmtCuspsUt(8),
                     _                                        => null
                 };

				return _kalaCusps;
			}
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
			var sr          = Jd.Date - Isthaghati - Sunrise.Time + LmtSunrise;
			var srLmtUt     = new JulianDate(sr - TimeSpan.FromDays(1));
			var srLmtNextUt = new JulianDate(sr);
			//double sr_lmt_ut = this.info.Jd - this.info.DateOfBirth.time / 24.0 + 6.0 / 24.0;
			//double sr_lmt_next_ut = sr_lmt_ut + 1.0;

			if (srLmtUt > Jd)
			{
				srLmtUt--;
				srLmtNextUt--;
			}


			Time span = 24.0 / (dayParts * 2);

			for (var i = 0; i <= dayParts * 2; i++)
			{
				ret[i] = srLmtUt + (span * i);
			}

			return ret;
		}

		private (Body, Yama) CalculateYamaLord()
		{
			var dayLord  = DayLord;
			var yamaLen  = Length.TotalHours / 8;
			var yama     = (int) (Isthaghati.TotalHours / yamaLen);
			var yamaSpan = (Yama) yama;
			var yamaLord = (Body) (dayLord.Index() + yama).NormalizeInc(0, 7);
			return (yamaLord, yamaSpan);
		}
		
		//The total time for all the five tatwas adds up to 90 minutes or 1 ½ hours. Generally, after sunrise
		//first 1 ½ hours tatwa cycle will be in the ascending order which is called Aroha cycle. The
		//descending cycle of 1 ½ hours is called Avaroha cycle and the tatwas climb down top tatwa to the
		//bottom tatwa.
		// The special feature is each day starts with specific tatwa according to the week day and continues
		//in succession in Aroha and Avaroha cycle.
		// On Sunday or Tuesday the first tatwa is Tejas tatwa of 18 mins and goes in the ascending order 18,
		//24, 30, 6 and 12 which makes 1 ½ hours. While descending it starts from 12, 6, 30, 24, and 18.
		// On Wednesday the first tatwa is Prithvi tatwa. On Thursday the first tatwa is Akash tatwa, on
		//Mondays and Fridays he first tatwa is Jala tatwa, on Saturdays the first tatwa is Vayu tatwa .
		private BirthTatva CalculateBirthTatva()
		{
			var tatva = Tatvas.DayTatva[WeekDay.Index()];

			var  yamaSpan  = Length / 16;
			var  part      = (int) (Isthaghati.TotalHours / yamaSpan.TotalHours);
			Time subPeriod = (Isthaghati.TotalHours % yamaSpan.TotalHours);

			var yama = (Yama) (part / 2); //Yama is 1/8th of a day

			if ((part % 2) == 1)
			{
				tatva = (Tatva) (4 - tatva.Index());
			}

			bool reverse    = (part % 2) == 1;
			(subPeriod, yamaSpan, part) = CalculateTatva(ref tatva, yamaSpan, subPeriod, reverse);

			reverse = (part % 2) == 1;
			var antaraTatva = tatva;
			CalculateTatva(ref antaraTatva, yamaSpan, subPeriod, reverse);

			return new BirthTatva(yama, tatva, antaraTatva);
		}

		private (Time, Time, int) CalculateTatva(ref Tatva tatva, Time span, Time subPeriod, bool reverse)
		{
			Time tatvaPeriod = subPeriod;
			int  index = 0;
			for (index = 0; index < 5; index++)
			{
				tatvaPeriod = (span / 90) * Tatvas.Duration[tatva.Index()];

				if (subPeriod < tatvaPeriod)
				{
					break;
				}
				subPeriod -= tatvaPeriod;

				if (reverse)
				{
					var bt = (tatva.Index() - 1);
					if (bt < 0)
					{
						bt += 5;
					}

					tatva = (Tatva) bt;
				}
				else
				{
					tatva = (Tatva) ((tatva.Index() + 1) % 5);
				}

			}
			return (subPeriod, tatvaPeriod, index);
		}
	}
}
