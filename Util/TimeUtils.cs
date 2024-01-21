using System;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;

namespace Mhora.Util;

public static class TimeUtils
{
	public static TimeSpan SiderealYear => new TimeSpan(365, 6, 12, 36, 56);

	public static double SideralMonth => 27.32;

	public static TimeSpan Time (this DateTime dateTime)
	{
		return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
	}

	public static TimeSpan Mul(this TimeSpan timeSpan, double factor)
	{
		return TimeSpan.FromHours(timeSpan.TotalHours * factor);
	}

	public static TimeSpan Div(this TimeSpan timeSpan, double factor)
	{
		return TimeSpan.FromHours(timeSpan.TotalHours / factor);
	}

	public static DateTime Moment(this Horoscope h, double tjdUt)
	{
		double time  = 0;
		int    year  = 0;
		int    month = 0;
		int    day   = 0;

		tjdUt += h.Info.DstOffset.TotalDays;
		sweph.RevJul(tjdUt, ref year, ref month, ref day, ref time);
		return new DateTime(year, month, day).AddHours(time);
	}

	public static double UniversalTime(this DateTime dateTime)
	{
		return sweph.JulDay(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Time().TotalHours);
	}

	public static double UtcToJulian(this DateTime dateTime)
	{
		double jd;
		double u, u0, u1, u2;
		u = dateTime.Year;
		if (dateTime.Month < 3) u -= 1;
		u0 = u              + 4712.0;
		u1 = dateTime.Month + 1.0;
		if (u1 < 4) u1 += 12.0;
		jd = Math.Floor(u0 * SiderealYear.TotalDays)
			+ Math.Floor(30.6 * u1 + 0.000001)
			+ dateTime.Day + dateTime.Time().TotalHours / 24.0 - 63.5;

		return (jd);
	}

	public static DateTime UtcDateTime(this double jd)
	{
		double u0, u1, u2, u3, u4;
		u0 = jd + 32082.5;
		u2   = Math.Floor(u0 + 123.0);
		u3   = Math.Floor((u2 - 122.2)                   / 365.25);
		u4   = Math.Floor((u2 - Math.Floor(365.25 * u3)) / 30.6001);
		var jmon = (int)(u4 - 1.0);
		if (jmon > 12) jmon -= 12;
		var jday  = (int)(u2 - Math.Floor(SiderealYear.TotalHours * u3) - Math.Floor(30.6001 * u4));
		var jyear = (int)(u3 + Math.Floor((u4 - 2.0) / 12.0) - 4800);
		var jut   = (jd      - Math.Floor(jd + 0.5)          + 0.5) * 24.0;

		return new DateTime(jyear, jmon, jday).AddHours(jut);

	}

	public static double UniversalTime(this Horoscope h, DateTime dateTime)
	{
		return sweph.JulDay(dateTime.Year, dateTime.Month, dateTime.Day, (dateTime - h.Info.DstOffset).Time().TotalHours);
	}

	public static int FromStringMonth(this string s)
	{
		switch (s)
		{
			case "Jan": return 1;
			case "Feb": return 2;
			case "Mar": return 3;
			case "Apr": return 4;
			case "May": return 5;
			case "Jun": return 6;
			case "Jul": return 7;
			case "Aug": return 8;
			case "Sep": return 9;
			case "Oct": return 10;
			case "Nov": return 11;
			case "Dec": return 12;
		}

		return 1;
	}

	public static string ToStringMonth(this DateTime dateTime)
	{
		switch (dateTime.Month)
		{
			case 1:  return "Jan";
			case 2:  return "Feb";
			case 3:  return "Mar";
			case 4:  return "Apr";
			case 5:  return "May";
			case 6:  return "Jun";
			case 7:  return "Jul";
			case 8:  return "Aug";
			case 9:  return "Sep";
			case 10: return "Oct";
			case 11: return "Nov";
			case 12: return "Dec";
		}

		return string.Empty;
	}

	public static string ToString(this DateTime dateTime)
	{
		return (dateTime.Day < 10 ? "0" : string.Empty) + dateTime.Day + " " + ToStringMonth(dateTime) + " " + dateTime.Year + " " + (dateTime.Hour < 10 ? "0" : string.Empty) + dateTime.Hour + ":" + (dateTime.Minute < 10 ? "0" : string.Empty) + dateTime.Minute + ":" + (dateTime.Second < 10 ? "0" : string.Empty) + dateTime.Second;
	}

	public static string ToShortDateString(this DateTime dateTime)
	{
		var year = dateTime.Year % 100;
		return string.Format("{0:00}-{1:00}-{2:00}", dateTime.Day, dateTime.Month, year);
	}

	public static string ToDateString(this DateTime dateTime)
	{
		return string.Format("{0:00} {1} {2}", dateTime.Day, ToStringMonth(dateTime), dateTime.Year);
	}

	public static string ToTimeString(this DateTime dateTime)
	{
		return dateTime.ToTimeString(false);
	}

	public static string ToTimeString(this DateTime dateTime, bool bDisplaySeconds)
	{
		if (bDisplaySeconds)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", dateTime.Hour, dateTime.Minute, dateTime.Second);
		}

		return string.Format("{0:00}:{1:00}", dateTime.Hour, dateTime.Minute);
	}


	//*********************************************************************/

	// Convert radian angle to degrees

	public static double RadToDeg(double angleRad)
	{
		return 180.0 * angleRad / Math.PI;
	}

	//*********************************************************************/

	// Convert degree angle to radians

	public static double DegToRad(double angleDeg)
	{
		return Math.PI * angleDeg / 180.0;
	}


	/// <summary>
	///     calcJD
	///     Julian day from calendar day
	/// </summary>
	/// <param name="year"> 4 digit year</param>
	/// <param name="month">January = 1	</param>
	/// <param name="day">1 - 31</param>
	/// <returns>The Julian day corresponding to the date</returns>
	/// <remarks>Number is returned for start of day. Fractional days should be	added later.</remarks>
	public static double CalcJd(int year, int month, int day)
	{
		if (month <= 2)
		{
			year  -= 1;
			month += 12;
		}

		var a = Math.Floor(year / 100.0);
		var b = 2 - a + Math.Floor(a / 4);

		var jd = Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5;
		return jd;
	}

	public static double CalcJd(this DateTime date)
	{
		return CalcJd(date.Year, date.Month, date.Day);
	}

	//***********************************************************************/
	//* Name: calcTimeJulianCent	
	//* Type: Function	
	//* Purpose: convert Julian Day to centuries since J2000.0.	
	//* Arguments:	
	//* jd : the Julian Day to convert	
	//* Return value:	
	//* the T value corresponding to the Julian Day	
	//***********************************************************************/

	public static double CalcTimeJulianCent(double jd)
	{
		var T = (jd - 2451545.0) / 36525.0;
		return T;
	}


	//***********************************************************************/
	//* Name: calcJDFromJulianCent	
	//* Type: Function	
	//* Purpose: convert centuries since J2000.0 to Julian Day.	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* the Julian Day corresponding to the t value	
	//***********************************************************************/

	public static double CalcJdFromJulianCent(double t)
	{
		var jd = t * 36525.0 + 2451545.0;
		return jd;
	}


	//***********************************************************************/
	//* Name: calGeomMeanLongSun	
	//* Type: Function	
	//* Purpose: calculate the Geometric Mean Longitude of the Sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* the Geometric Mean Longitude of the Sun in degrees	
	//***********************************************************************/

	public static double CalcGeomMeanLongSun(double t)
	{
		var l0 = 280.46646 + t * (36000.76983 + 0.0003032 * t);
		while (l0 > 360.0)
		{
			l0 -= 360.0;
		}

		while (l0 < 0.0)
		{
			l0 += 360.0;
		}

		return l0; // in degrees
	}


	//***********************************************************************/
	//* Name: calGeomAnomalySun	
	//* Type: Function	
	//* Purpose: calculate the Geometric Mean Anomaly of the Sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* the Geometric Mean Anomaly of the Sun in degrees	
	//***********************************************************************/

	public static double CalcGeomMeanAnomalySun(double t)
	{
		var m = 357.52911 + t * (35999.05029 - 0.0001537 * t);
		return m; // in degrees
	}

	//***********************************************************************/
	//* Name: calcEccentricityEarthOrbit	
	//* Type: Function	
	//* Purpose: calculate the eccentricity of earth's orbit	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* the unitless eccentricity	
	//***********************************************************************/


	public static double CalcEccentricityEarthOrbit(double t)
	{
		var e = 0.016708634 - t * (0.000042037 + 0.0000001267 * t);
		return e; // unitless
	}

	//***********************************************************************/
	//* Name: calcSunEqOfCenter	
	//* Type: Function	
	//* Purpose: calculate the equation of center for the sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* in degrees	
	//***********************************************************************/


	public static double CalcSunEqOfCenter(double t)
	{
		var m = CalcGeomMeanAnomalySun(t);

		var mrad  = DegToRad(m);
		var sinm  = Math.Sin(mrad);
		var sin2M = Math.Sin(mrad + mrad);
		var sin3M = Math.Sin(mrad + mrad + mrad);

		var c = sinm * (1.914602 - t * (0.004817 + 0.000014 * t)) + sin2M * (0.019993 - 0.000101 * t) + sin3M * 0.000289;
		return c; // in degrees
	}

	//***********************************************************************/
	//* Name: calcSunTrueLong	
	//* Type: Function	
	//* Purpose: calculate the true longitude of the sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* sun's true longitude in degrees	
	//***********************************************************************/


	public static double CalcSunTrueLong(double t)
	{
		var l0 = CalcGeomMeanLongSun(t);
		var c  = CalcSunEqOfCenter(t);

		var o = l0 + c;
		return o; // in degrees
	}

	//***********************************************************************/
	//* Name: calcSunTrueAnomaly	
	//* Type: Function	
	//* Purpose: calculate the true anamoly of the sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* sun's true anamoly in degrees	
	//***********************************************************************/

	public static double CalcSunTrueAnomaly(double t)
	{
		var m = CalcGeomMeanAnomalySun(t);
		var c = CalcSunEqOfCenter(t);

		var v = m + c;
		return v; // in degrees
	}

	//***********************************************************************/
	//* Name: calcSunRadVector	
	//* Type: Function	
	//* Purpose: calculate the distance to the sun in AU	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* sun radius vector in AUs	
	//***********************************************************************/

	public static double CalcSunRadVector(double t)
	{
		var v = CalcSunTrueAnomaly(t);
		var e = CalcEccentricityEarthOrbit(t);

		var r = 1.000001018 * (1 - e * e) / (1 + e * Math.Cos(DegToRad(v)));
		return r; // in AUs
	}

	//***********************************************************************/
	//* Name: calcSunApparentLong	
	//* Type: Function	
	//* Purpose: calculate the apparent longitude of the sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* sun's apparent longitude in degrees	
	//***********************************************************************/

	public static double CalcSunApparentLong(double t)
	{
		var o = CalcSunTrueLong(t);

		var omega  = 125.04 - 1934.136          * t;
		var lambda = o      - 0.00569 - 0.00478 * Math.Sin(DegToRad(omega));
		return lambda; // in degrees
	}

	//***********************************************************************/
	//* Name: calcMeanObliquityOfEcliptic	
	//* Type: Function	
	//* Purpose: calculate the mean obliquity of the ecliptic	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* mean obliquity in degrees	
	//***********************************************************************/

	public static double CalcMeanObliquityOfEcliptic(double t)
	{
		var seconds = 21.448 - t * (46.8150 + t       * (0.00059 - t * 0.001813));
		var e0      = 23.0   + (26.0        + seconds / 60.0) / 60.0;
		return e0; // in degrees
	}

	//***********************************************************************/
	//* Name: calcObliquityCorrection	
	//* Type: Function	
	//* Purpose: calculate the corrected obliquity of the ecliptic	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* corrected obliquity in degrees	
	//***********************************************************************/

	public static double CalcObliquityCorrection(double t)
	{
		var e0 = CalcMeanObliquityOfEcliptic(t);

		var omega = 125.04 - 1934.136 * t;
		var e     = e0     + 0.00256  * Math.Cos(DegToRad(omega));
		return e; // in degrees
	}

	//***********************************************************************/
	//* Name: calcSunRtAscension	
	//* Type: Function	
	//* Purpose: calculate the right ascension of the sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* sun's right ascension in degrees	
	//***********************************************************************/

	public static double CalcSunRtAscension(double t)
	{
		var e      = CalcObliquityCorrection(t);
		var lambda = CalcSunApparentLong(t);

		var tananum   = Math.Cos(DegToRad(e)) * Math.Sin(DegToRad(lambda));
		var tanadenom = Math.Cos(DegToRad(lambda));
		var alpha     = RadToDeg(Math.Atan2(tananum, tanadenom));
		return alpha; // in degrees
	}

	//***********************************************************************/
	//* Name: calcSunDeclination	
	//* Type: Function	
	//* Purpose: calculate the declination of the sun	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* sun's declination in degrees	
	//***********************************************************************/

	public static double CalcSunDeclination(double t)
	{
		var e      = CalcObliquityCorrection(t);
		var lambda = CalcSunApparentLong(t);

		var sint  = Math.Sin(DegToRad(e)) * Math.Sin(DegToRad(lambda));
		var theta = RadToDeg(Math.Asin(sint));
		return theta; // in degrees
	}

	//***********************************************************************/
	//* Name: calcEquationOfTime	
	//* Type: Function	
	//* Purpose: calculate the difference between true solar time and mean	
	//*	 solar time	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* Return value:	
	//* equation of time in minutes of time	
	//***********************************************************************/

	public static double CalcEquationOfTime(double t)
	{
		var epsilon = CalcObliquityCorrection(t);
		var l0      = CalcGeomMeanLongSun(t);
		var e       = CalcEccentricityEarthOrbit(t);
		var m       = CalcGeomMeanAnomalySun(t);

		var y = Math.Tan(DegToRad(epsilon) / 2.0);
		y *= y;

		var sin2L0 = Math.Sin(2.0 * DegToRad(l0));
		var sinm   = Math.Sin(DegToRad(m));
		var cos2L0 = Math.Cos(2.0 * DegToRad(l0));
		var sin4L0 = Math.Sin(4.0 * DegToRad(l0));
		var sin2M  = Math.Sin(2.0 * DegToRad(m));

		var etime = y * sin2L0 - 2.0 * e * sinm + 4.0 * e * y * sinm * cos2L0 - 0.5 * y * y * sin4L0 - 1.25 * e * e * sin2M;

		return RadToDeg(etime) * 4.0; // in minutes of time
	}

	//***********************************************************************/
	//* Name: calcHourAngleSunrise	
	//* Type: Function	
	//* Purpose: calculate the hour angle of the sun at sunrise for the	
	//*	 latitude	
	//* Arguments:	
	//* lat : latitude of observer in degrees	
	//*	solarDec : declination angle of sun in degrees	
	//* Return value:	
	//* hour angle of sunrise in radians	
	//***********************************************************************/

	public static double CalcHourAngleSunrise(double lat, double solarDec)
	{
		var latRad = DegToRad(lat);
		var sdRad  = DegToRad(solarDec);

		var hAarg = Math.Cos(DegToRad(90.833)) / (Math.Cos(latRad) * Math.Cos(sdRad)) - Math.Tan(latRad) * Math.Tan(sdRad);

		var ha = Math.Acos(Math.Cos(DegToRad(90.833)) / (Math.Cos(latRad) * Math.Cos(sdRad)) - Math.Tan(latRad) * Math.Tan(sdRad));

		return ha; // in radians
	}

	//***********************************************************************/
	//* Name: calcHourAngleSunset	
	//* Type: Function	
	//* Purpose: calculate the hour angle of the sun at sunset for the	
	//*	 latitude	
	//* Arguments:	
	//* lat : latitude of observer in degrees	
	//*	solarDec : declination angle of sun in degrees	
	//* Return value:	
	//* hour angle of sunset in radians	
	//***********************************************************************/

	public static double CalcHourAngleSunset(double lat, double solarDec)
	{
		var latRad = DegToRad(lat);
		var sdRad  = DegToRad(solarDec);

		var hAarg = Math.Cos(DegToRad(90.833)) / (Math.Cos(latRad) * Math.Cos(sdRad)) - Math.Tan(latRad) * Math.Tan(sdRad);

		var ha = Math.Acos(Math.Cos(DegToRad(90.833)) / (Math.Cos(latRad) * Math.Cos(sdRad)) - Math.Tan(latRad) * Math.Tan(sdRad));

		return -ha; // in radians
	}


	//***********************************************************************/
	//* Name: calcSunriseUTC	
	//* Type: Function	
	//* Purpose: calculate the Universal Coordinated Time (UTC) of sunrise	
	//*	 for the given day at the given location on earth	
	//* Arguments:	
	//* JD : julian day	
	//* latitude : latitude of observer in degrees	
	//* longitude : longitude of observer in degrees	
	//* Return value:	
	//* time in minutes from zero Z	
	//***********************************************************************/

	[Obsolete("calcSunriseUTCWithFraction is deprecated, please use calcSunRiseUTC instead.", true)]
	public static double CalcSunriseUtc(double jd, double latitude, double longitude)
	{
		return CalcSunRiseUtcWithFraction(jd, latitude, longitude);
	}

	[Obsolete("calcSunRiseUTCWithFraction is deprecated because noo work yet :), please use calcSunRiseUTC instead.")]
	public static double CalcSunRiseUtcWithFraction(double jd, double latitude, double longitude)
	{
		//TODO: this method don't work!!! I have to fix it
		var t = CalcTimeJulianCent(jd);
		/*


		// *** Find the time of solar noon at the location, and use
		// that declination. This is better than start of the
		// Julian day

		double noonmin = calcSolNoonUTC(t, longitude);
		double tnoon = calcTimeJulianCent(JD + noonmin / 1440.0);

		// *** First pass to approximate sunrise (using solar noon)

		double eqTime = calcEquationOfTime(tnoon);
		double solarDec = calcSunDeclination(tnoon);
		double hourAngle = calcHourAngleSunrise(latitude, solarDec);

		double delta = longitude - radToDeg(hourAngle);
		double timeDiff = 4 * delta;	// in minutes of time

		double timeUTC = calcSunRiseUTC(JD, latitude, longitude); // 720 + timeDiff - eqTime;	// in minutes
		*/

		double eqTime    = 0;
		double solarDec  = 0;
		double hourAngle = 0;
		double delta     = 0;
		double timeDiff  = 0;
		var    timeUtc   = CalcSunRiseUtc(jd, latitude, longitude);
		// alert("eqTime = " + eqTime + "\nsolarDec = " + solarDec + "\ntimeUTC = " + timeUTC);

		// *** Second pass includes fractional jday in gamma calc
		//this is the good function for calculate the time UTC of the sunRise
		//var t = calcTimeJulianCent(JD);
		//var eqTime = calcEquationOfTime(t);
		//var solarDec = calcSunDeclination(t);
		//var hourAngle = calcHourAngleSunrise(latitude, solarDec);
		//hourAngle = -hourAngle;
		//var delta = longitude + radToDeg(hourAngle);
		//var timeUTC = 720 - (4.0 * delta) - eqTime;	// in minutes

		var newt = CalcTimeJulianCent(CalcJdFromJulianCent(t) + timeUtc / 1440.0);
		eqTime    = CalcEquationOfTime(newt);
		solarDec  = CalcSunDeclination(newt);
		hourAngle = CalcHourAngleSunrise(latitude, solarDec);
		hourAngle = -hourAngle;
		delta     = longitude - RadToDeg(hourAngle);
		timeDiff  = 4 * delta;
		timeUtc   = 720 + timeDiff - eqTime; // in minutes

		// alert("eqTime = " + eqTime + "\nsolarDec = " + solarDec + "\ntimeUTC = " + timeUTC);

		return timeUtc;
	}

	//***********************************************************************/
	//* Name: calcSolNoonUTC	
	//* Type: Function	
	//* Purpose: calculate the Universal Coordinated Time (UTC) of solar	
	//*	 noon for the given day at the given location on earth	
	//* Arguments:	
	//* t : number of Julian centuries since J2000.0	
	//* longitude : longitude of observer in degrees	
	//* Return value:	
	//* time in minutes from zero Z	
	//***********************************************************************/

	public static double CalcSolNoonUtc(double t, double longitude)
	{
		// First pass uses approximate solar noon to calculate eqtime
		var tnoon      = CalcTimeJulianCent(CalcJdFromJulianCent(t) + longitude / 360.0);
		var eqTime     = CalcEquationOfTime(tnoon);
		var solNoonUtc = 720 + longitude * 4 - eqTime; // min

		var newt = CalcTimeJulianCent(CalcJdFromJulianCent(t) - 0.5 + solNoonUtc / 1440.0);

		eqTime = CalcEquationOfTime(newt);
		// double solarNoonDec = calcSunDeclination(newt);
		solNoonUtc = 720 + longitude * 4 - eqTime; // min

		return solNoonUtc;
	}

	//***********************************************************************/
	//* Name: calcSunsetUTC	
	//* Type: Function	
	//* Purpose: calculate the Universal Coordinated Time (UTC) of sunset	
	//*	 for the given day at the given location on earth	
	//* Arguments:	
	//* JD : julian day	
	//* latitude : latitude of observer in degrees	
	//* longitude : longitude of observer in degrees	
	//* Return value:	
	//* time in minutes from zero Z	
	//***********************************************************************/

	public static double CalcSunSetUtc(double jd, double latitude, double longitude)
	{
		var t         = CalcTimeJulianCent(jd);
		var eqTime    = CalcEquationOfTime(t);
		var solarDec  = CalcSunDeclination(t);
		var hourAngle = CalcHourAngleSunrise(latitude, solarDec);
		hourAngle = -hourAngle;
		var delta   = longitude + RadToDeg(hourAngle);
		var timeUtc = 720       - 4.0 * delta - eqTime; // in minutes
		return timeUtc;
	}

	public static double CalcSunRiseUtc(double jd, double latitude, double longitude)
	{
		var t         = CalcTimeJulianCent(jd);
		var eqTime    = CalcEquationOfTime(t);
		var solarDec  = CalcSunDeclination(t);
		var hourAngle = CalcHourAngleSunrise(latitude, solarDec);
		var delta     = longitude + RadToDeg(hourAngle);
		var timeUtc   = 720       - 4.0 * delta - eqTime; // in minutes
		return timeUtc;
	}

	public static string GetTimeString(double time, int timezone, double jd, bool dst)
	{
		var timeLocal = time + timezone * 60.0;
		var riseT     = CalcTimeJulianCent(jd + time / 1440.0);
		timeLocal += dst ? 60.0 : 0.0;
		return GetTimeString(timeLocal);
	}

	public static DateTime? GetDateTime(double time, int timezone, DateTime date, bool dst)
	{
		var jd        = CalcJd(date);
		var timeLocal = time + timezone * 60.0;
		var riseT     = CalcTimeJulianCent(jd + time / 1440.0);
		timeLocal += dst ? 60.0 : 0.0;
		return GetDateTime(timeLocal, date);
	}

	private static string GetTimeString(double minutes)
	{
		var output = "";

		if (minutes >= 0 && minutes < 1440)
		{
			var floatHour   = minutes / 60.0;
			var hour        = Math.Floor(floatHour);
			var floatMinute = 60.0 * (floatHour - Math.Floor(floatHour));
			var minute      = Math.Floor(floatMinute);
			var floatSec    = 60.0 * (floatMinute - Math.Floor(floatMinute));
			var second      = Math.Floor(floatSec + 0.5);
			if (second > 59)
			{
				second =  0;
				minute += 1;
			}

			if (second >= 30)
			{
				minute++;
			}

			if (minute > 59)
			{
				minute =  0;
				hour   += 1;
			}

			output = string.Format("{0} : {1}", hour, minute);
		}
		else
		{
			return "error";
		}

		return output;
	}

	private static DateTime? GetDateTime(double minutes, DateTime date)
	{
		DateTime? retVal = null;

		if (minutes >= 0 && minutes < 1440)
		{
			var floatHour   = minutes / 60.0;
			var hour        = Math.Floor(floatHour);
			var floatMinute = 60.0 * (floatHour - Math.Floor(floatHour));
			var minute      = Math.Floor(floatMinute);
			var floatSec    = 60.0 * (floatMinute - Math.Floor(floatMinute));
			var second      = Math.Floor(floatSec + 0.5);
			if (second > 59)
			{
				second =  0;
				minute += 1;
			}

			if (second >= 30)
			{
				minute++;
			}

			if (minute > 59)
			{
				minute =  0;
				hour   += 1;
			}

			return new DateTime(date.Year, date.Month, date.Day, (int) hour, (int) minute, (int) second);
		}

		return retVal;
	}
}