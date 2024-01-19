namespace Mhora.SwissEph;

public static partial class sweph
{
	public const double SE_AUNIT_TO_KM        = 149597870.700;
	public const double SE_AUNIT_TO_LIGHTYEAR = 1.0 / 63241.07708427;
	public const double SE_AUNIT_TO_PARSEC    = 1.0 / 206264.8062471;

	/* values for gregflag in swe_julday() and swe_revjul() */
	public const int SE_JUL_CAL  = 0;
	public const int SE_GREG_CAL = 1;

	/*
	 * planet numbers for the ipl parameter in swe_calc()
	 */
	public const int SE_ECL_NUT = -1;

	public const int SE_SUN       = 0;
	public const int SE_MOON      = 1;
	public const int SE_MERCURY   = 2;
	public const int SE_VENUS     = 3;
	public const int SE_MARS      = 4;
	public const int SE_JUPITER   = 5;
	public const int SE_SATURN    = 6;
	public const int SE_URANUS    = 7;
	public const int SE_NEPTUNE   = 8;
	public const int SE_PLUTO     = 9;
	public const int SE_MEAN_NODE = 10;
	public const int SE_TRUE_NODE = 11;
	public const int SE_MEAN_APOG = 12;
	public const int SE_OSCU_APOG = 13;
	public const int SE_EARTH     = 14;
	public const int SE_CHIRON    = 15;
	public const int SE_PHOLUS    = 16;
	public const int SE_CERES     = 17;
	public const int SE_PALLAS    = 18;
	public const int SE_JUNO      = 19;
	public const int SE_VESTA     = 20;
	public const int SE_INTP_APOG = 21;
	public const int SE_INTP_PERG = 22;

	public const int SE_NPLANETS = 23;

	public const int SE_AST_OFFSET = 10000;
	public const int SE_VARUNA     = SE_AST_OFFSET + 20000;

	public const int SE_FICT_OFFSET   = 40;
	public const int SE_FICT_OFFSET_1 = 39;
	public const int SE_FICT_MAX      = 999;
	public const int SE_NFICT_ELEM    = 15;

	public const int SE_COMET_OFFSET = 1000;

	public const int SE_NALL_NAT_POINTS = SE_NPLANETS + SE_NFICT_ELEM;

	/* Hamburger or Uranian "planets" */
	public const int SE_CUPIDO   = 40;
	public const int SE_HADES    = 41;
	public const int SE_ZEUS     = 42;
	public const int SE_KRONOS   = 43;
	public const int SE_APOLLON  = 44;
	public const int SE_ADMETOS  = 45;
	public const int SE_VULKANUS = 46;

	public const int SE_POSEIDON = 47;

	/* other fictitious bodies */
	public const int SE_ISIS              = 48;
	public const int SE_NIBIRU            = 49;
	public const int SE_HARRINGTON        = 50;
	public const int SE_NEPTUNE_LEVERRIER = 51;
	public const int SE_NEPTUNE_ADAMS     = 52;
	public const int SE_PLUTO_LOWELL      = 53;
	public const int SE_PLUTO_PICKERING   = 54;
	public const int SE_VULCAN            = 55;
	public const int SE_WHITE_MOON        = 56;
	public const int SE_PROSERPINA        = 57;
	public const int SE_WALDEMATH         = 58;

	public const int SE_FIXSTAR = -10;

	public const int SE_ASC    = 0;
	public const int SE_MC     = 1;
	public const int SE_ARMC   = 2;
	public const int SE_VERTEX = 3;
	public const int SE_EQUASC = 4; /* "equatorial ascendant" */
	public const int SE_COASC1 = 5; /* "co-ascendant" (W. Koch) */
	public const int SE_COASC2 = 6; /* "co-ascendant" (M. Munkasey) */
	public const int SE_POLASC = 7; /* "polar ascendant" (M. Munkasey) */
	public const int SE_NASCMC = 8;

	/*
	 * flag bits for parameter iflag in function swe_calc()
	 * The flag bits are defined in such a way that iflag = 0 delivers what one
	 * usually wants:
	 *    - the default ephemeris (SWISS EPHEMERIS) is used,
	 *    - apparent geocentric positions referring to the true equinox of date
	 *      are returned.
	 * If not only coordinates, but also speed values are required, use
	 * flag = SEFLG_SPEED.
	 *
	 * The 'L' behind the number indicates that 32-bit integers (Long) are used.
	 */
	public const int SEFLG_JPLEPH = 1; /* use JPL ephemeris */
	public const int SEFLG_SWIEPH = 2; /* use SWISSEPH ephemeris */
	public const int SEFLG_MOSEPH = 4; /* use Moshier ephemeris */

	public const int SEFLG_EPHMASK  = SEFLG_JPLEPH     | SEFLG_SWIEPH | SEFLG_MOSEPH;
	public const int SEFLG_COORDSYS = SEFLG_EQUATORIAL | SEFLG_XYZ    | SEFLG_RADIANS;

	public const int SEFLG_HELCTR  = 8;  /* heliocentric position */
	public const int SEFLG_TRUEPOS = 16; /* true/geometric position, not apparent position */
	public const int SEFLG_J2000   = 32; /* no precession, i.e. give J2000 equinox */
	public const int SEFLG_NONUT   = 64; /* no nutation, i.e. mean equinox of date */

	public const int SEFLG_SPEED3 = 128; /* speed from 3 positions (do not use it,
	                                      * SEFLG_SPEED is faster and more precise.) */

	public const int SEFLG_SPEED   = 256;  /* high precision speed  */
	public const int SEFLG_NOGDEFL = 512;  /* turn off gravitational deflection */
	public const int SEFLG_NOABERR = 1024; /* turn off 'annual' aberration of light */

	public const int SEFLG_ASTROMETRIC = SEFLG_NOABERR | SEFLG_NOGDEFL; /* astrometric position,
	                                                                     * i.e. with light-time, but without aberration and
	                                                                     * light deflection */

	public const int SEFLG_EQUATORIAL = 2  * 1024; /* equatorial positions are wanted */
	public const int SEFLG_XYZ        = 4  * 1024; /* cartesian, not polar, coordinates */
	public const int SEFLG_RADIANS    = 8  * 1024; /* coordinates in radians, not degrees */
	public const int SEFLG_BARYCTR    = 16 * 1024; /* barycentric position */
	public const int SEFLG_TOPOCTR    = 32 * 1024; /* topocentric position */

	public const int SEFLG_ORBEL_AA = SEFLG_TOPOCTR; /* used for Astronomical Almanac mode in
	                                                  * calculation of Kepler elipses */

	public const int SEFLG_SIDEREAL = 64  * 1024; /* sidereal position */
	public const int SEFLG_ICRS     = 128 * 1024; /* ICRS (DE406 reference frame) */

	public const int SEFLG_DPSIDEPS_1980 = 256 * 1024; /* reproduce JPL Horizons
	                                                        1962 - today to 0.002 arcsec. */

	public const int SEFLG_JPLHOR        = SEFLG_DPSIDEPS_1980;
	public const int SEFLG_JPLHOR_APPROX = 512 * 1024; /* approximate JPL Horizons 1962 - today */

	public const int SE_SIDBITS = 256;

	/* for projection onto ecliptic of t0 */
	public const int SE_SIDBIT_ECL_T0 = 256;

	/* for projection onto solar system plane */
	public const int SE_SIDBIT_SSY_PLANE = 512;

	/* with user-defined ayanamsha, t0 is UT */
	public const int SE_SIDBIT_USER_UT = 1024;

	/* sidereal modes (ayanamsas) */
	public const int SE_SIDM_FAGAN_BRADLEY        = 0;
	public const int SE_SIDM_LAHIRI               = 1;
	public const int SE_SIDM_DELUCE               = 2;
	public const int SE_SIDM_RAMAN                = 3;
	public const int SE_SIDM_USHASHASHI           = 4;
	public const int SE_SIDM_KRISHNAMURTI         = 5;
	public const int SE_SIDM_DJWHAL_KHUL          = 6;
	public const int SE_SIDM_YUKTESHWAR           = 7;
	public const int SE_SIDM_JN_BHASIN            = 8;
	public const int SE_SIDM_BABYL_KUGLER1        = 9;
	public const int SE_SIDM_BABYL_KUGLER2        = 10;
	public const int SE_SIDM_BABYL_KUGLER3        = 11;
	public const int SE_SIDM_BABYL_HUBER          = 12;
	public const int SE_SIDM_BABYL_ETPSC          = 13;
	public const int SE_SIDM_ALDEBARAN_15TAU      = 14;
	public const int SE_SIDM_HIPPARCHOS           = 15;
	public const int SE_SIDM_SASSANIAN            = 16;
	public const int SE_SIDM_GALCENT_0SAG         = 17;
	public const int SE_SIDM_J2000                = 18;
	public const int SE_SIDM_J1900                = 19;
	public const int SE_SIDM_B1950                = 20;
	public const int SE_SIDM_SURYASIDDHANTA       = 21;
	public const int SE_SIDM_SURYASIDDHANTA_MSUN  = 22;
	public const int SE_SIDM_ARYABHATA            = 23;
	public const int SE_SIDM_ARYABHATA_MSUN       = 24;
	public const int SE_SIDM_SS_REVATI            = 25;
	public const int SE_SIDM_SS_CITRA             = 26;
	public const int SE_SIDM_TRUE_CITRA           = 27;
	public const int SE_SIDM_TRUE_REVATI          = 28;
	public const int SE_SIDM_TRUE_PUSHYA          = 29;
	public const int SE_SIDM_GALCENT_RGILBRAND    = 30;
	public const int SE_SIDM_GALEQU_IAU1958       = 31;
	public const int SE_SIDM_GALEQU_TRUE          = 32;
	public const int SE_SIDM_GALEQU_MULA          = 33;
	public const int SE_SIDM_GALALIGN_MARDYKS     = 34;
	public const int SE_SIDM_TRUE_MULA            = 35;
	public const int SE_SIDM_GALCENT_MULA_WILHELM = 36;
	public const int SE_SIDM_ARYABHATA_522        = 37;
	public const int SE_SIDM_BABYL_BRITTON        = 38;
	public const int SE_SIDM_TRUE_SHEORAN         = 39;
	public const int SE_SIDM_GALCENT_COCHRANE     = 40;
	public const int SE_SIDM_GALEQU_FIORENZA      = 41;

	public const int SE_SIDM_VALENS_MOON = 42;

	////#define SE_SIDM_MANJULA         43
	public const int SE_SIDM_USER = 255; /* user-defined ayanamsha, t0 is TT */

	public const int SE_NSIDM_PREDEF = 43;

	/* used for swe_nod_aps(): */
	public const int SE_NODBIT_MEAN     = 1;   /* mean nodes/apsides */
	public const int SE_NODBIT_OSCU     = 2;   /* osculating nodes/apsides */
	public const int SE_NODBIT_OSCU_BAR = 4;   /* same, but motion about solar system barycenter is considered */
	public const int SE_NODBIT_FOPOINT  = 256; /* focal point of orbit instead of aphelion */

	/* default ephemeris used when no ephemeris flagbit is set */
	public const int SEFLG_DEFAULTEPH = SEFLG_SWIEPH;

	public const int SE_MAX_STNAME = 256; /* maximum size of fixstar name;
	                                       * the parameter star in swe_fixstar
	                                       * must allow twice this space for
	                                       * the returned star name.
	                                       */

	/* defines for eclipse computations */

	public const int SE_ECL_CENTRAL           = 1;
	public const int SE_ECL_NONCENTRAL        = 2;
	public const int SE_ECL_TOTAL             = 4;
	public const int SE_ECL_ANNULAR           = 8;
	public const int SE_ECL_PARTIAL           = 16;
	public const int SE_ECL_ANNULAR_TOTAL     = 32;
	public const int SE_ECL_PENUMBRAL         = 64;
	public const int SE_ECL_ALLTYPES_SOLAR    = SE_ECL_CENTRAL | SE_ECL_NONCENTRAL | SE_ECL_TOTAL | SE_ECL_ANNULAR | SE_ECL_PARTIAL | SE_ECL_ANNULAR_TOTAL;
	public const int SE_ECL_ALLTYPES_LUNAR    = SE_ECL_TOTAL   | SE_ECL_PARTIAL    | SE_ECL_PENUMBRAL;
	public const int SE_ECL_VISIBLE           = 128;
	public const int SE_ECL_MAX_VISIBLE       = 256;
	public const int SE_ECL_1ST_VISIBLE       = 512;   /* begin of partial eclipse */
	public const int SE_ECL_PARTBEG_VISIBLE   = 512;   /* begin of partial eclipse */
	public const int SE_ECL_2ND_VISIBLE       = 1024;  /* begin of total eclipse */
	public const int SE_ECL_TOTBEG_VISIBLE    = 1024;  /* begin of total eclipse */
	public const int SE_ECL_3RD_VISIBLE       = 2048;  /* end of total eclipse */
	public const int SE_ECL_TOTEND_VISIBLE    = 2048;  /* end of total eclipse */
	public const int SE_ECL_4TH_VISIBLE       = 4096;  /* end of partial eclipse */
	public const int SE_ECL_PARTEND_VISIBLE   = 4096;  /* end of partial eclipse */
	public const int SE_ECL_PENUMBBEG_VISIBLE = 8192;  /* begin of penumbral eclipse */
	public const int SE_ECL_PENUMBEND_VISIBLE = 16384; /* end of penumbral eclipse */
	public const int SE_ECL_OCC_BEG_DAYLIGHT  = 8192;  /* occultation begins during the day */
	public const int SE_ECL_OCC_END_DAYLIGHT  = 16384; /* occultation ends during the day */

	public const int SE_ECL_ONE_TRY = 32 * 1024;
	/* check if the next conjunction of the moon with
	 * a planet is an occultation; don't search further */

	/* for swe_rise_transit() */
	public const int SE_CALC_RISE     = 1;
	public const int SE_CALC_SET      = 2;
	public const int SE_CALC_MTRANSIT = 4;
	public const int SE_CALC_ITRANSIT = 8;

	public const int SE_BIT_DISC_CENTER = 256; /* to be or'ed to SE_CALC_RISE/SET,
	                                            * if rise or set of disc center is
	                                            * required */

	public const int SE_BIT_DISC_BOTTOM = 8192; /* to be or'ed to SE_CALC_RISE/SET,
	                                             * if rise or set of lower limb of
	                                             * disc is requried */

	public const int SE_BIT_GEOCTR_NO_ECL_LAT = 128; /* use geocentric rather than topocentric
	                                                    position of object and
	                                                    ignore its ecliptic latitude */

	public const int SE_BIT_NO_REFRACTION = 512; /* to be or'ed to SE_CALC_RISE/SET,
	                                              * if refraction is to be ignored */

	public const int SE_BIT_CIVIL_TWILIGHT  = 1024; /* to be or'ed to SE_CALC_RISE/SET */
	public const int SE_BIT_NAUTIC_TWILIGHT = 2048; /* to be or'ed to SE_CALC_RISE/SET */
	public const int SE_BIT_ASTRO_TWILIGHT  = 4096; /* to be or'ed to SE_CALC_RISE/SET */

	public const int SE_BIT_FIXED_DISC_SIZE = 16384; /* or'ed to SE_CALC_RISE/SET:
	                                                  * neglect the effect of distance on
	                                                  * disc size */

	public const int SE_BIT_FORCE_SLOW_METHOD = 32768; /* This is only an Astrodienst in-house
	                                                    * test flag.It forces the usage
	                                                    * of the old, slow calculation of
	                                                    * risings and settings. */

	public const int SE_BIT_HINDU_RISING = SE_BIT_DISC_CENTER | SE_BIT_NO_REFRACTION | SE_BIT_GEOCTR_NO_ECL_LAT;


	/* for swe_azalt() and swe_azalt_rev() */
	public const int SE_ECL2HOR = 0;
	public const int SE_EQU2HOR = 1;
	public const int SE_HOR2ECL = 0;
	public const int SE_HOR2EQU = 1;

	/* for swe_refrac() */
	public const int SE_TRUE_TO_APP = 0;
	public const int SE_APP_TO_TRUE = 1;

	/*
	 * only used for experimenting with various JPL ephemeris files
	 * which are available at Astrodienst's internal network
	 */
	public const int    SE_DE_NUMBER    = 431;
	public const string SE_FNAME_DE200  = "de200.eph";
	public const string SE_FNAME_DE403  = "de403.eph";
	public const string SE_FNAME_DE404  = "de404.eph";
	public const string SE_FNAME_DE405  = "de405.eph";
	public const string SE_FNAME_DE406  = "de406.eph";
	public const string SE_FNAME_DE431  = "de431.eph";
	public const string SE_FNAME_DFT    = SE_FNAME_DE431;
	public const string SE_FNAME_DFT2   = SE_FNAME_DE406;
	public const string SE_STARFILE_OLD = "fixstars.cat";
	public const string SE_STARFILE     = "sefstars.txt";
	public const string SE_ASTNAMFILE   = "seasnam.txt";
	public const string SE_FICTFILE     = "seorbel.txt";


   /// <summary>
    /// 2000 January 1.5
    /// </summary>
    public const double J2000 = 2451545.0;
    /// <summary>
    /// 1950 January 0.923 
    /// </summary>
    public const double B1950 = 2433282.42345905;
    /// <summary>
    /// 1900 January 0.5
    /// </summary>
    public const double J1900 = 2415020.0;

    public const int MPC_CERES = 1;
    public const int MPC_PALLAS = 2;
    public const int MPC_JUNO = 3;
    public const int MPC_VESTA = 4;
    public const int MPC_CHIRON = 2060;
    public const int MPC_PHOLUS = 5145;

    public const string SE_NAME_SUN = "Sun";
    public const string SE_NAME_MOON = "Moon";
    public const string SE_NAME_MERCURY = "Mercury";
    public const string SE_NAME_VENUS = "Venus";
    public const string SE_NAME_MARS = "Mars";
    public const string SE_NAME_JUPITER = "Jupiter";
    public const string SE_NAME_SATURN = "Saturn";
    public const string SE_NAME_URANUS = "Uranus";
    public const string SE_NAME_NEPTUNE = "Neptune";
    public const string SE_NAME_PLUTO = "Pluto";
    public const string SE_NAME_MEAN_NODE = "mean Node";
    public const string SE_NAME_TRUE_NODE = "true Node";
    public const string SE_NAME_MEAN_APOG = "mean Apogee";
    public const string SE_NAME_OSCU_APOG = "osc. Apogee";
    public const string SE_NAME_INTP_APOG = "intp. Apogee";
    public const string SE_NAME_INTP_PERG = "intp. Perigee";
    public const string SE_NAME_EARTH = "Earth";
    public const string SE_NAME_CERES = "Ceres";
    public const string SE_NAME_PALLAS = "Pallas";
    public const string SE_NAME_JUNO = "Juno";
    public const string SE_NAME_VESTA = "Vesta";
    public const string SE_NAME_CHIRON = "Chiron";
    public const string SE_NAME_PHOLUS = "Pholus";


    public const string SE_NAME_CUPIDO = "Cupido";
    public const string SE_NAME_HADES = "Hades";
    public const string SE_NAME_ZEUS = "Zeus";
    public const string SE_NAME_KRONOS = "Kronos";
    public const string SE_NAME_APOLLON = "Apollon";
    public const string SE_NAME_ADMETOS = "Admetos";
    public const string SE_NAME_VULKANUS = "Vulkanus";
    public const string SE_NAME_POSEIDON = "Poseidon";
    public const string SE_NAME_ISIS = "Isis";
    public const string SE_NAME_NIBIRU = "Nibiru";
    public const string SE_NAME_HARRINGTON = "Harrington";
    public const string SE_NAME_NEPTUNE_LEVERRIER = "Leverrier";
    public const string SE_NAME_NEPTUNE_ADAMS = "Adams";
    public const string SE_NAME_PLUTO_LOWELL = "Lowell";
    public const string SE_NAME_PLUTO_PICKERING = "Pickering";
    public const string SE_NAME_VULCAN = "Vulcan";
    public const string SE_NAME_WHITE_MOON = "White Moon";

	/*
	 * ephemeris path
	 * this defines where ephemeris files are expected if the function
	 * swe_set_ephe_path() is not called by the application.
	 * Normally, every application should make this call to define its
	 * own place for the ephemeris files.
	 */
	/// <summary>
	///     SweNet : We create a pseudo constant for detect ephemeris path when loading
	/// </summary>
	public const string SE_EPHE_PATH = "[ephe]";


	/* defines for function swe_split_deg() (in swephlib.c) */
	public const int SE_SPLIT_DEG_ROUND_SEC = 1;
	public const int SE_SPLIT_DEG_ROUND_MIN = 2;
	public const int SE_SPLIT_DEG_ROUND_DEG = 4;
	public const int SE_SPLIT_DEG_ZODIACAL  = 8;
	public const int SE_SPLIT_DEG_NAKSHATRA = 1024;

	public const int SE_SPLIT_DEG_KEEP_SIGN = 16; /* don't round to next sign,
	                                               * e.g. 29.9999999 will be rounded
	                                               * to 29d59'59" (or 29d59' or 29d) */

	public const int SE_SPLIT_DEG_KEEP_DEG = 32; /* don't round to next degree
	                                              * e.g. 13.9999999 will be rounded
	                                              * to 13d59'59" (or 13d59' or 13d) */

	/* for heliacal functions */
	public const int SE_HELIACAL_RISING    = 1;
	public const int SE_HELIACAL_SETTING   = 2;
	public const int SE_MORNING_FIRST      = SE_HELIACAL_RISING;
	public const int SE_EVENING_LAST       = SE_HELIACAL_SETTING;
	public const int SE_EVENING_FIRST      = 3;
	public const int SE_MORNING_LAST       = 4;
	public const int SE_ACRONYCHAL_RISING  = 5; /* still not implemented */
	public const int SE_ACRONYCHAL_SETTING = 6; /* still not implemented */
	public const int SE_COSMICAL_SETTING   = SE_ACRONYCHAL_SETTING;

	public const int SE_HELFLAG_LONG_SEARCH     = 128;
	public const int SE_HELFLAG_HIGH_PRECISION  = 256;
	public const int SE_HELFLAG_OPTICAL_PARAMS  = 512;
	public const int SE_HELFLAG_NO_DETAILS      = 1024;
	public const int SE_HELFLAG_SEARCH_1_PERIOD = 1 << 11; /*  2048 */
	public const int SE_HELFLAG_VISLIM_DARK     = 1 << 12; /*  4096 */

	public const int SE_HELFLAG_VISLIM_NOMOON = 1 << 13; /*  8192 */

	/* the following undocumented defines are for test reasons only */
	public const int    SE_HELFLAG_VISLIM_PHOTOPIC = 1 << 14; /* 16384 */
	public const int    SE_HELFLAG_VISLIM_SCOTOPIC = 1 << 15; /* 32768 */
	public const int    SE_HELFLAG_AV              = 1 << 16; /* 65536 */
	public const int    SE_HELFLAG_AVKIND_VR       = 1 << 16; /* 65536 */
	public const int    SE_HELFLAG_AVKIND_PTO      = 1 << 17;
	public const int    SE_HELFLAG_AVKIND_MIN7     = 1 << 18;
	public const int    SE_HELFLAG_AVKIND_MIN9     = 1 << 19;
	public const int    SE_HELFLAG_AVKIND          = SE_HELFLAG_AVKIND_VR | SE_HELFLAG_AVKIND_PTO | SE_HELFLAG_AVKIND_MIN7 | SE_HELFLAG_AVKIND_MIN9;
	public const double TJD_INVALID                = 99999999.0;
	public const bool   SIMULATE_VICTORVB          = true;

#if FALSE // unused and redundant
        public const int SE_HELIACAL_LONG_SEARCH = 128;
        public const int SE_HELIACAL_HIGH_PRECISION = 256;
        public const int SE_HELIACAL_OPTICAL_PARAMS = 512;
        public const int SE_HELIACAL_NO_DETAILS = 1024;
        public const int SE_HELIACAL_SEARCH_1_PERIOD = (1 << 11);  /*  2048 */
        public const int SE_HELIACAL_VISLIM_DARK = (1 << 12);  /*  4096 */
        public const int SE_HELIACAL_VISLIM_NOMOON = (1 << 13);  /*  8192 */
        public const int SE_HELIACAL_VISLIM_PHOTOPIC = (1 << 14);  /* 16384 */
        public const int SE_HELIACAL_AVKIND_VR = (1 << 15);  /* 32768 */
        public const int SE_HELIACAL_AVKIND_PTO = (1 << 16);
        public const int SE_HELIACAL_AVKIND_MIN7 = (1 << 17);
        public const int SE_HELIACAL_AVKIND_MIN9 = (1 << 18);
        public const int SE_HELIACAL_AVKIND = (SE_HELFLAG_AVKIND_VR | SE_HELFLAG_AVKIND_PTO | SE_HELFLAG_AVKIND_MIN7 | SE_HELFLAG_AVKIND_MIN9);
#endif

	public const int SE_PHOTOPIC_FLAG  = 0;
	public const int SE_SCOTOPIC_FLAG  = 1;
	public const int SE_MIXEDOPIC_FLAG = 2;

	/* for swe_set_tid_acc() and ephemeris-dependent delta t:
	 * intrinsic tidal acceleration in the mean motion of the moon,
	 * not given in the parameters list of the ephemeris files but computed
	 * by Chapront/Chapront-TouzÃ©/Francou A&A 387 (2002), p. 705.
	 */
	public const double SE_TIDAL_DE200           = -23.8946;
	public const double SE_TIDAL_DE403           = -25.580; /* was (-25.8) until V. 1.76.2 */
	public const double SE_TIDAL_DE404           = -25.580; /* was (-25.8) until V. 1.76.2 */
	public const double SE_TIDAL_DE405           = -25.826; /* was (-25.7376) until V. 1.76.2 */
	public const double SE_TIDAL_DE406           = -25.826; /* was (-25.7376) until V. 1.76.2 */
	public const double SE_TIDAL_DE421           = -25.85;  /* JPL Interoffice Memorandum 14-mar-2008 on DE421 Lunar Orbit */
	public const double SE_TIDAL_DE422           = -25.85;  /* JPL Interoffice Memorandum 14-mar-2008 on DE421 (sic!) Lunar Orbit */
	public const double SE_TIDAL_DE430           = -25.82;  /* JPL Interoffice Memorandum 9-jul-2013 on DE430 Lunar Orbit */
	public const double SE_TIDAL_DE431           = -25.80;  /* IPN Progress Report 42-196 â€¢ February 15, 2014, p. 15; was (-25.82) in V. 2.00.00 */
	public const double SE_TIDAL_26              = -26.0;
	public const double SE_TIDAL_STEPHENSON_2016 = -25.85;
	public const double SE_TIDAL_DEFAULT         = SE_TIDAL_DE431;
	public const double SE_TIDAL_AUTOMATIC       = 999999;
	public const double SE_TIDAL_MOSEPH          = SE_TIDAL_DE404;
	public const double SE_TIDAL_SWIEPH          = SE_TIDAL_DEFAULT;
	public const double SE_TIDAL_JPLEPH          = SE_TIDAL_DEFAULT;

	/* for function swe_set_delta_t_userdef() */
	public const double SE_DELTAT_AUTOMATIC = -1E-10;

	public const int SE_MODEL_DELTAT         = 0;
	public const int SE_MODEL_PREC_LONGTERM  = 1;
	public const int SE_MODEL_PREC_SHORTTERM = 2;
	public const int SE_MODEL_NUT            = 3;
	public const int SE_MODEL_BIAS           = 4;
	public const int SE_MODEL_JPLHOR_MODE    = 5;
	public const int SE_MODEL_JPLHORA_MODE   = 6;
	public const int SE_MODEL_SIDT           = 7;
	public const int NSE_MODELS              = 8;

	/* precession models */
	public const int SEMOD_NPREC               = 10;
	public const int SEMOD_PREC_IAU_1976       = 1;
	public const int SEMOD_PREC_LASKAR_1986    = 2;
	public const int SEMOD_PREC_WILL_EPS_LASK  = 3;
	public const int SEMOD_PREC_WILLIAMS_1994  = 4;
	public const int SEMOD_PREC_SIMON_1994     = 5;
	public const int SEMOD_PREC_IAU_2000       = 6;
	public const int SEMOD_PREC_BRETAGNON_2003 = 7;
	public const int SEMOD_PREC_IAU_2006       = 8;
	public const int SEMOD_PREC_VONDRAK_2011   = 9;
	public const int SEMOD_PREC_OWEN_1990      = 10;

	public const int SEMOD_PREC_DEFAULT = SEMOD_PREC_VONDRAK_2011;

	/* SE versions before 1.70 used IAU 1976 precession for
	 * a limited time range of 2 centuries in combination with
	 * the long-term precession Simon 1994.
	 */
	public const int SEMOD_PREC_DEFAULT_SHORT = SEMOD_PREC_VONDRAK_2011;

	/* nutation models */
	public const int SEMOD_NNUT         = 4;
	public const int SEMOD_NUT_IAU_1980 = 1;

	public const int SEMOD_NUT_IAU_CORR_1987 = 2; /* Herring's (1987) corrections to IAU 1980
	                                               * nutation series. AA (1996) neglects them.*/

	public const int SEMOD_NUT_IAU_2000A = 3;                   /* very time consuming ! */
	public const int SEMOD_NUT_IAU_2000B = 4;                   /* fast, but precision of milli-arcsec */
	public const int SEMOD_NUT_DEFAULT   = SEMOD_NUT_IAU_2000B; /* fast, but precision of milli-arcsec */

	/* methods for sidereal time */
	public const int SEMOD_NSIDT               = 4;
	public const int SEMOD_SIDT_IAU_1976       = 1;
	public const int SEMOD_SIDT_IAU_2006       = 2;
	public const int SEMOD_SIDT_IERS_CONV_2010 = 3;
	public const int SEMOD_SIDT_LONGTERM       = 4;
	public const int SEMOD_SIDT_DEFAULT        = SEMOD_SIDT_LONGTERM;
	//#define SEMOD_SIDT_DEFAULT          SEMOD_SIDT_IERS_CONV_2010

	/* frame bias methods */
	public const int SEMOD_NBIAS        = 3;
	public const int SEMOD_BIAS_NONE    = 1; /* ignore frame bias */
	public const int SEMOD_BIAS_IAU2000 = 2; /* use frame bias matrix IAU 2000 */
	public const int SEMOD_BIAS_IAU2006 = 3; /* use frame bias matrix IAU 2006 */
	public const int SEMOD_BIAS_DEFAULT = SEMOD_BIAS_IAU2006;

	/* methods of JPL Horizons (iflag & SEFLG_JPLHOR),
	 * using daily dpsi, deps;  see explanations below */
	public const int SEMOD_NJPLHOR = 2;

	public const int SEMOD_JPLHOR_LONG_AGREEMENT = 1; /* daily dpsi and deps from file are
	                                                   * limited to 1962 - today. JPL uses the
	                                                   * first and last value for all  dates
	                                                   * beyond this time range. */

	public const int SEMOD_JPLHOR_DEFAULT = SEMOD_JPLHOR_LONG_AGREEMENT;
	/* Note, currently this is the only option for SEMOD_JPLHOR..*/
	/* SEMOD_JPLHOR_LONG_AGREEMENT, if combined with SEFLG_JPLHOR provides good
	 * agreement with JPL Horizons for 9998 BC (-9997) until 9999 CE.
	 * - After 20-jan-1962 until today, Horizons uses correct dpsi and deps.
	 * - For dates before that, it uses dpsi and deps of 20-jan-1962, which
	 *   provides a continuous ephemeris, but does not make sense otherwise.
	 * - Before 1.1.1799 and after 1.1.2202, the precession model Owen 1990
	 *   is used, as in Horizons.
	 * An agreement with Horizons to a couple of milli arc seconds is achieved
	 * for the whole time range of Horizons. (BC 9998-Mar-20 to AD 9999-Dec-31 TT.)
	 */

	/* methods of approximation of JPL Horizons (iflag & SEFLG_JPLHORA),
	 * without dpsi, deps; see explanations below */
	public const int SEMOD_NJPLHORA        = 3;
	public const int SEMOD_JPLHORA_1       = 1;
	public const int SEMOD_JPLHORA_2       = 2;
	public const int SEMOD_JPLHORA_3       = 3;
	public const int SEMOD_JPLHORA_DEFAULT = SEMOD_JPLHORA_3;

	/* With SEMOD_JPLHORA_1, planetary positions are always calculated
	 * using a recent precession/nutation model. Frame bias matrix is applied
	 * with some correction to RA and another correction added to epsilon.
	 * This provides a very good approximation of JPL Horizons positions.
	 *
	 * With SEMOD_JPLHORA_2, frame bias as recommended by IERS Conventions 2003
	 * and 2010 is *not* applied. Instead, dpsi_bias and deps_bias are added to
	 * nutation. This procedure is found in some older astronomical software.
	 * Equatorial apparent positions will be close to JPL Horizons
	 * (within a few mas) between 1962 and current years. Ecl. longitude
	 * will be good, latitude bad.
	 *
	 * With SEMOD_JPLHORA_3 works like SEMOD_JPLHORA_3 after 1962, but like
	 * SEFLG_JPLHOR before that. This allows EXTREMELY good agreement with JPL
	 * Horizons over its whole time range.
	 */

	public const int SEMOD_NDELTAT                         = 5;
	public const int SEMOD_DELTAT_STEPHENSON_MORRISON_1984 = 1;
	public const int SEMOD_DELTAT_STEPHENSON_1997          = 2;
	public const int SEMOD_DELTAT_STEPHENSON_MORRISON_2004 = 3;
	public const int SEMOD_DELTAT_ESPENAK_MEEUS_2006       = 4;

	public const int SEMOD_DELTAT_STEPHENSON_ETC_2016 = 5;

	//#define SEMOD_DELTAT_DEFAULT   SEMOD_DELTAT_ESPENAK_MEEUS_2006
	public const int SEMOD_DELTAT_DEFAULT = SEMOD_DELTAT_STEPHENSON_ETC_2016;


	public const int OK  = 0;
	public const int ERR = -1;
}