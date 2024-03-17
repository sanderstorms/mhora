using System.Runtime.InteropServices;
using System.Text;

namespace Mhora.SwissEph;

public static class SwephDll
{
	public static class Swe64
	{
		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_close();

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern byte[] swe_version(byte[] svers);

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_set_ephe_path(string path);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_set_sid_mode")]
		public static extern void swe_set_sid_mode(int sid_mode, double t0, double ayan_t0);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_julday")]
		public static extern double swe_julday(int year, int month, int day, double hour, int gregflag);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_revjul")]
		public static extern double swe_revjul(double tjd, int gregflag, out int year, out int month, out int day, out double hour);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_glob")]
		public static extern int swe_sol_eclipse_when_glob(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		/* planets, moon, nodes etc. */
		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_calc")]
		public static extern int swe_calc(double tjd, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_calc_ut")]
		public static extern int swe_calc_ut(double tjd_ut, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_loc")]
		public static extern int swe_sol_eclipse_when_loc(double tjd_ut, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_eclipse_when")]
		public static extern int swe_lun_eclipse_when(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_occult_when_loc")]
		public static extern int swe_lun_occult_when_loc(double tjd_ut, int ipl, ref string starname, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_get_ayanamsa_ut")]
		public static extern double swe_get_ayanamsa_ut(double tjd_ut);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_rise_trans")]
		public static extern int swe_rise_trans(double tjd_ut, int ipl, string starname, int epheflag, int rsmi, double[] geopos, double atpress, double attemp, ref double tret, StringBuilder serr);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_houses")]
		public static extern int swe_houses(double tjd_ut, double geolat, double geolon, int hsys, double [] cusps, double[] ascmc);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_houses_ex")]
		public static extern int swe_houses_ex(double tjd_ut, int iflag, double lat, double lon, int hsys, double[] cusps, double[] ascmc);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_day_of_week")]
		public static extern int swe_day_of_week(double jd);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_deltat")]
		public static extern double swe_deltat(double tjd_et);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_set_tid_acc")]
		public static extern void swe_set_tid_acc(double t_acc);

		[DllImport("swedll64", CharSet = CharSet.Ansi, EntryPoint = "swe_time_equ")]
		public static extern int swe_time_equ(double tjd_et, ref double e, StringBuilder s);

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_set_topo(double geolon, double geolat, double altitude);

		/* sidereal time */
		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern double swe_sidtime0(double tjd_ut, double eps, double nut);

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern double swe_sidtime(double tjd_ut);

		[DllImport("swedll64", CharSet = CharSet.Ansi)]
		public static extern void swe_azalt(double   tjd_ut,    // UT
		                                    int      calc_flag, // SE_ECL2HOR or SE_EQU2HOR
		                                    double[] geopos,    // array of 3 doubles: geograph. long., lat., height
		                                    double   atpress,   // atmospheric pressure in mbar (hPa)
		                                    double   attemp,    // atmospheric temperature in degrees Celsius
		                                    double[] xin,       // array of 3 doubles: position of body in either ecliptical or equatorial coordinates, depending on calc_flag
		                                    double[] xaz); // return array of 3 doubles, containing azimuth, true altitude, apparent altitude;
	}

	public static class Swe32
	{
		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_close();

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern byte[] swe_version(byte[] svers);

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_set_ephe_path(string path);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_set_sid_mode")]
		public static extern void swe_set_sid_mode(int sid_mode, double t0, double ayan_t0);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_julday")]
		public static extern double swe_julday(int year, int month, int day, double hour, int gregflag);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_revjul")]
		public static extern double swe_revjul(double tjd, int gregflag, out int year, out int month, out int day, out double hour);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_glob")]
		public static extern int swe_sol_eclipse_when_glob(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_calc")]
		public static extern int swe_calc(double tjd, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_calc_ut")]
		public static extern int swe_calc_ut(double tjd_ut, int ipl, int iflag, double[] xx, StringBuilder serr);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_sol_eclipse_when_loc")]
		public static extern int swe_sol_eclipse_when_loc(double tjd_ut, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_eclipse_when")]
		public static extern int swe_lun_eclipse_when(double tjd_ut, int iflag, int ifltype, double[] tret, bool backward, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_lun_occult_when_loc")]
		public static extern int swe_lun_occult_when_loc(double tjd_ut, int ipl, ref string starname, int iflag, double[] geopos, double[] tret, double[] attr, bool backward, StringBuilder s);


		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_get_ayanamsa_ut")]
		public static extern double swe_get_ayanamsa_ut(double tjd_ut);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_rise_trans")]
		public static extern int swe_rise_trans(double tjd_ut, int ipl, string starname, int epheflag, int rsmi, double[] geopos, double atpress, double attemp, ref double tret, StringBuilder serr);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_houses_ex")]
		public static extern int swe_houses_ex(double tjd_ut, int iflag, double lat, double lon, int hsys, double[] cusps, double[] ascmc);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_houses")]
		public static extern int swe_houses(double tjd_ut, double geolat, double geolon, int hsys, double [] cusps,  double[] ascmc);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_day_of_week")]
		public static extern int swe_day_of_week(double jd);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_deltat")]
		public static extern double swe_deltat(double tjd_et);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_set_tid_acc")]
		public static extern void swe_set_tid_acc(double t_acc);

		[DllImport("swedll32", CharSet = CharSet.Ansi, EntryPoint = "swe_time_equ")]
		public static extern int swe_time_equ(double tjd_et, ref double e, StringBuilder s);

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_set_topo(double geolon, double geolat, double altitude);

		/* sidereal time */
		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern double swe_sidtime0(double tjd_ut, double eps, double nut);

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern double swe_sidtime(double tjd_ut);

		[DllImport("swedll32", CharSet = CharSet.Ansi)]
		public static extern void swe_azalt(double   tjd_ut,    // UT
		                                    int      calc_flag, // SE_ECL2HOR or SE_EQU2HOR
		                                    double[] geopos,    // array of 3 doubles: geograph. long., lat., height
		                                    double   atpress,   // atmospheric pressure in mbar (hPa)
		                                    double   attemp,    // atmospheric temperature in degrees Celsius
		                                    double[] xin,       // array of 3 doubles: position of body in either ecliptical or equatorial coordinates, depending on calc_flag
		                                    double[] xaz); // return array of 3 doubles, containing azimuth, true altitude, apparent altitude;
	}
}