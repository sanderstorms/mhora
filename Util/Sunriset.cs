using System;

namespace Mhora.Util;

public static class Sunriset
{
	private const double SunriseSunsetAltitude        = -35d / 60d;
	private const double CivilTwilightAltitude        = -6d;
	private const double NauticalTwilightAltitude     = -12d;
	private const double AstronomicalTwilightAltitude = -18d;

	/* Some conversion factors between radians and degrees */
	private const double RadDeg = 180.0   / Math.PI;
	private const double DegRad = Math.PI / 180.0;

	private const double Inv360 = 1.0d / 360.0d;

	/// <summary>
	///     Compute sunrise/sunset times UTC
	/// </summary>
	/// <param name="year">The year</param>
	/// <param name="month">The month of year</param>
	/// <param name="day">The day of month</param>
	/// <param name="lat">The latitude</param>
	/// <param name="lng">The longitude</param>
	/// <param name="tsunrise">The computed sunrise time (in seconds)</param>
	/// <param name="tsunset">The computed sunset time (in seconds)</param>
	public static void SunriseSunset(int year, int month, int day, double lat, double lng, out double tsunrise, out double tsunset)
	{
		SunriseSunset(year, month, day, lng, lat, SunriseSunsetAltitude, true, out tsunrise, out tsunset);
	}

	/// <summary>
	///     Compute civil twilight times UTC
	/// </summary>
	/// <param name="year">The year</param>
	/// <param name="month">The month of year</param>
	/// <param name="day">The day of month</param>
	/// <param name="lat">The latitude</param>
	/// <param name="lng">The longitude</param>
	/// <param name="tsunrise">The computed civil twilight time at sunrise (in seconds)</param>
	/// <param name="tsunset">The computed civil twilight time at sunset (in seconds)</param>
	public static void CivilTwilight(int year, int month, int day, double lat, double lng, out double tsunrise, out double tsunset)
	{
		SunriseSunset(year, month, day, lng, lat, CivilTwilightAltitude, false, out tsunrise, out tsunset);
	}

	/// <summary>
	///     Compute nautical twilight times UTC
	/// </summary>
	/// <param name="year">The year</param>
	/// <param name="month">The month of year</param>
	/// <param name="day">The day of month</param>
	/// <param name="lat">The latitude</param>
	/// <param name="lng">The longitude</param>
	/// <param name="tsunrise">The computed nautical twilight time at sunrise (in seconds)</param>
	/// <param name="tsunset">The computed nautical twilight time at sunset (in seconds)</param>
	public static void NauticalTwilight(int year, int month, int day, double lat, double lng, out double tsunrise, out double tsunset)
	{
		SunriseSunset(year, month, day, lng, lat, NauticalTwilightAltitude, false, out tsunrise, out tsunset);
	}

	/// <summary>
	///     Compute astronomical twilight times UTC
	/// </summary>
	/// <param name="year">The year</param>
	/// <param name="month">The month of year</param>
	/// <param name="day">The day of month</param>
	/// <param name="lat">The latitude</param>
	/// <param name="lng">The longitude</param>
	/// <param name="tsunrise">The computed astronomical twilight time at sunrise (in seconds)</param>
	/// <param name="tsunset">The computed astronomical twilight time at sunset (in seconds)</param>
	public static void AstronomicalTwilight(int year, int month, int day, double lat, double lng, out double tsunrise, out double tsunset)
	{
		SunriseSunset(year, month, day, lng, lat, AstronomicalTwilightAltitude, false, out tsunrise, out tsunset);
	}

	/* +++Date last modified: 05-Jul-1997 */
	/* Updated comments, 05-Aug-2013 */

	/*
	SUNRISET.C - computes Sun rise/set times, start/end of twilight, and
	the length of the day at any date and latitude
	Written as DAYLEN.C, 1989-08-16
	Modified to SUNRISET.C, 1992-12-01
	(c) Paul Schlyter, 1989, 1992
	Released to the public domain by Paul Schlyter, December 1992
*/

	/* Converted to C# by Mursaat 05-Feb-2017 */

	/// <summary>
	///     A function to compute the number of days elapsed since 2000 Jan 0.0
	///     (which is equal to 1999 Dec 31, 0h UT)
	/// </summary>
	/// <param name="y"></param>
	/// <param name="m"></param>
	/// <param name="d"></param>
	/// <returns></returns>
	private static long DaysSince2000Jan0(int y, int m, int d)
	{
		return 367L * y - 7 * (y + (m + 9) / 12) / 4 + 275 * m / 9 + d - 730530L;
	}

	/* The trigonometric functions in degrees */
	private static double Sind(double x)
	{
		return Math.Sin(x * DegRad);
	}

	private static double Cosd(double x)
	{
		return Math.Cos(x * DegRad);
	}

	private static double Tand(double x)
	{
		return Math.Tan(x * DegRad);
	}

	private static double Atand(double x)
	{
		return RadDeg * Math.Atan(x);
	}

	private static double Asind(double x)
	{
		return RadDeg * Math.Asin(x);
	}

	private static double Acosd(double x)
	{
		return RadDeg * Math.Acos(x);
	}

	private static double Atan2d(double y, double x)
	{
		return RadDeg * Math.Atan2(y, x);
	}

	/// <summary>
	///     The "workhorse" function for sun rise/set times
	///     Note: year,month,date = calendar date, 1801-2099 only.
	///     Eastern longitude positive, Western longitude negative
	///     Northern latitude positive, Southern latitude negative
	///     The longitude value IS critical in this function!
	/// </summary>
	/// <param name="year"></param>
	/// <param name="month"></param>
	/// <param name="day"></param>
	/// <param name="lon"></param>
	/// <param name="lat"></param>
	/// <param name="altit">
	///     the altitude which the Sun should cross
	///     Set to -35/60 degrees for rise/set, -6 degrees
	///     for civil, -12 degrees for nautical and -18
	///     degrees for astronomical twilight.
	/// </param>
	/// <param name="upper_limb">
	///     true -> upper limb, false -> center
	///     Set to true (e.g. 1) when computing rise/set
	///     times, and to false when computing start/end of twilight.
	/// </param>
	/// <param name="trise">where to store the rise time</param>
	/// <param name="tset">where to store the set time</param>
	/// <returns>
	///     0	=	sun rises/sets this day, times stored at trise and tset
	///     +1	=	sun above the specified "horizon" 24 hours.
	///     trise set to time when the sun is at south,
	///     minus 12 hours while *tset is set to the south
	///     time plus 12 hours. "Day" length = 24 hours
	///     -1	=	sun is below the specified "horizon" 24 hours
	///     "Day" length = 0 hours, *trise and *tset are
	///     both set to the time when the sun is at south.
	/// </returns>
	private static int SunriseSunset(int year, int month, int day, double lon, double lat, double altit, bool upperLimb, out double trise, out double tset)
	{
		double sr;      /* Solar distance, astronomical units */
		double sRa;     /* Sun's Right Ascension */
		double sdec;    /* Sun's declination */
		double t;       /* Diurnal arc */

		var rc = 0; /* Return cde from function - usually 0 */

		/* Compute d of 12h local mean solar time */
		var d /* Days since 2000 Jan 0.0 (negative before) */ = DaysSince2000Jan0(year, month, day) + 0.5 - lon / 360.0;

		/* Compute the local sidereal time of this moment */
		var sidtime /* Local sidereal time */ = Revolution(Gmst0(d) + 180.0 + lon);

		/* Compute Sun's RA, Decl and distance at this moment */
		sun_RA_dec(d, out sRa, out sdec, out sr);

		/* Compute time when Sun is at south - in hours UT */
		var tsouth /* Time when Sun is at south */ = 12.0 - Rev180(sidtime - sRa) / 15.0;

		/* Compute the Sun's apparent radius in degrees */
		var sradius /* Sun's apparent radius */ = 0.2666 / sr;

		/* Do correction to upper limb, if necessary */
		if (upperLimb)
		{
			altit -= sradius;
		}

		/* Compute the diurnal arc that the Sun traverses to reach */
		/* the specified altitude altit: */
		{
			var cost = (Sind(altit) - Sind(lat) * Sind(sdec)) / (Cosd(lat) * Cosd(sdec));
			if (cost >= 1.0) /* Sun always below altit */
			{
				rc = -1;
				t  = 0.0;
			}
			else if (cost <= -1.0) /* Sun always above altit */
			{
				rc = +1;
				t  = 12.0;
			}
			else
			{
				t = Acosd(cost) / 15.0; /* The diurnal arc, hours */
			}
		}

		/* Store rise and set times - in hours UT */
		trise = tsouth - t;
		tset  = tsouth + t;

		return rc;
	}

	/// <summary>
	///     Note: year,month,date = calendar date, 1801-2099 only.
	///     Eastern longitude positive, Western longitude negative
	///     Northern latitude positive, Southern latitude negative
	///     The longitude value is not critical. Set it to the correct
	///     The latitude however IS critical - be sure to get it correct
	/// </summary>
	/// <param name="year">
	///     altit = the altitude which the Sun should cross
	///     Set to -35/60 degrees for rise/set, -6 degrees
	///     for civil, -12 degrees for nautical and -18
	///     degrees for astronomical twilight.
	/// </param>
	/// <param name="month"></param>
	/// <param name="day"></param>
	/// <param name="lon"></param>
	/// <param name="lat"></param>
	/// <param name="altit"></param>
	/// <param name="upper_limb">
	///     true -> upper limb, true -> center
	///     Set to true (e.g. 1) when computing day length
	///     and to false when computing day+twilight length.
	/// </param>
	/// <returns></returns>
	public static double DayLen(int year, int month, int day, double lon, double lat, double altit, bool upperLimb)
	{
		double sr;   /* Solar distance, astronomical units */
		double slon; /* True solar longitude */
		double t;    /* Diurnal arc */

		/* Compute d of 12h local mean solar time */
		var d /* Days since 2000 Jan 0.0 (negative before) */ = DaysSince2000Jan0(year, month, day) + 0.5 - lon / 360.0;

		/* Compute obliquity of ecliptic (inclination of Earth's axis) */
		var oblEcl /* Obliquity (inclination) of Earth's axis */ = 23.4393 - 3.563E-7 * d;

		/* Compute Sun's ecliptic longitude and distance */
		Sunpos(d, out slon, out sr);

		/* Compute sine and cosine of Sun's declination */
		var sinSdecl /* Sine of Sun's declination */   = Sind(oblEcl) * Sind(slon);
		var cosSdecl /* Cosine of Sun's declination */ = Math.Sqrt(1.0 - sinSdecl * sinSdecl);

		/* Compute the Sun's apparent radius, degrees */
		var sradius /* Sun's apparent radius */ = 0.2666 / sr;

		/* Do correction to upper limb, if necessary */
		if (upperLimb)
		{
			altit -= sradius;
		}

		/* Compute the diurnal arc that the Sun traverses to reach */
		/* the specified altitude altit: */
		var cost = (Sind(altit) - Sind(lat) * sinSdecl) / (Cosd(lat) * cosSdecl);

		/* Sun always below altit */
		if (cost >= 1.0)
		{
			t = 0.0;
		}
		/* Sun always above altit */
		else if (cost <= -1.0)
		{
			t = 24.0;
		}
		/* The diurnal arc, hours */
		else
		{
			t = 2.0 / 15.0 * Acosd(cost);
		}

		return t;
	}

	/// <summary>
	///     Computes the Sun's ecliptic longitude and distance
	///     at an instant given in d, number of days since
	///     2000 Jan 0.0.  The Sun's ecliptic latitude is not
	///     computed, since it's always very near 0.
	/// </summary>
	/// <param name="d"></param>
	/// <param name="lon"></param>
	/// <param name="r"></param>
	private static void Sunpos(double d, out double lon, out double r)
	{
		var m /* Mean anomaly of the Sun */ =
			/* Note: Sun's mean longitude = M + w */
			/* Compute mean elements */
			Revolution(356.0470 + 0.9856002585 * d);
		var    w /* Mean longitude of perihelion */  = 282.9404 + 4.70935E-5 * d;
		var    e /* Eccentricity of Earth's orbit */ = 0.016709 - 1.151E-9   * d;

		/* Compute true longitude and radius vector */
		var ea /* Eccentric anomaly */        = m        + e * RadDeg * Sind(m) * (1.0 + e * Cosd(m));
		var x /* x, y coordinates in orbit */ = Cosd(ea) - e;
		var y                                 = Math.Sqrt(1.0 - e * e) * Sind(ea);
		r   = Math.Sqrt(x * x + y * y); /* Solar distance */
		var v /* True anomaly */ = Atan2d(y, x) /* True anomaly */;
		lon = v + w; /* True solar longitude */
		if (lon >= 360.0)
		{
			lon -= 360.0; /* Make it 0..360 degrees */
		}
	}

	/// <summary>
	///     Computes the Sun's equatorial coordinates RA, Decl
	///     and also its distance, at an instant given in d,
	///     the number of days since 2000 Jan 0.0.
	/// </summary>
	/// <param name="d"></param>
	/// <param name="ra"></param>
	/// <param name="dec"></param>
	/// <param name="r"></param>
	private static void sun_RA_dec(double d, out double ra, out double dec, out double r)
	{
		double lon;

		/* Compute Sun's ecliptical coordinates */
		Sunpos(d, out lon, out r);

		/* Compute ecliptic rectangular coordinates (z=0) */
		var x = r * Cosd(lon);
		var    y = r * Sind(lon);

		/* Compute obliquity of ecliptic (inclination of Earth's axis) */
		var oblEcl = 23.4393 - 3.563E-7 * d;

		/* Convert to equatorial rectangular coordinates - x is unchanged */
		var z = y * Sind(oblEcl);
		y = y * Cosd(oblEcl);

		/* Convert to spherical coordinates */
		ra  = Atan2d(y, x);
		dec = Atan2d(z, Math.Sqrt(x * x + y * y));
	}

	/// <summary>
	///     This function reduces any angle to within the first revolution
	///     by subtracting or adding even multiples of 360.0 until the
	///     result is >= 0.0 and < 360.0
	/// </summary>
	/// <param name="x"></param>
	/// <returns></returns>
	private static double Revolution(double x)
	{
		return x - 360.0 * Math.Floor(x * Inv360);
	}

	/// <summary>
	///     Reduce angle to within +180..+180 degrees
	/// </summary>
	/// <param name="x"></param>
	/// <returns></returns>
	private static double Rev180(double x)
	{
		return x - 360.0 * Math.Floor(x * Inv360 + 0.5);
	}

	/// <summary>
	///     This function computes GMST0, the Greenwich Mean Sidereal Time
	///     at 0h UT (i.e. the sidereal time at the Greenwhich meridian at
	///     0h UT).  GMST is then the sidereal time at Greenwich at any
	///     time of the day.  I've generalized GMST0 as well, and define it
	///     as:  GMST0 = GMST - UT  --  this allows GMST0 to be computed at
	///     other times than 0h UT as well.
	///     While this sounds somewhat contradictory, it is very practical:
	///     instead of computing  GMST like:
	///     GMST = (GMST0) + UT * (366.2422/365.2422)
	///     where (GMST0) is the GMST last time UT was 0 hours, one simply
	///     computes: GMST = GMST0 + UT
	///     where GMST0 is the GMST "at 0h UT" but at the current moment!
	///     Defined in this way, GMST0 will increase with about 4 min a
	///     day.  It also happens that GMST0 (in degrees, 1 hr = 15 degr)
	///     is equal to the Sun's mean longitude plus/minus 180 degrees!
	///     (if we neglect aberration, which amounts to 20 seconds of arc
	///     or 1.33 seconds of time)
	/// </summary>
	/// <param name="d"></param>
	/// <returns></returns>
	private static double Gmst0(double d)
	{
		var sidtim0 =
			/* Sidtime at 0h UT = L (Sun's mean longitude) + 180.0 degr  */
			/* L = M + w, as defined in sunpos().  Since I'm too lazy to */
			/* add these numbers, I'll let the C compiler do it for me.  */
			/* Any decent C compiler will add the constants at compile   */
			/* time, imposing no runtime or code overhead.               */
			Revolution(180.0 + 356.0470 + 282.9404 + (0.9856002585 + 4.70935E-5) * d);
		return sidtim0;
	}
}