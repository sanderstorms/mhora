/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Mhora.Components.Delegates;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements.Calculation;
using Mhora.Elements.Yoga;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     Contains all the information for a horoscope. i.e. All ephemeris lookups
///     have been completed, sunrise/sunset has been calculated etc.
/// </summary>
public class Horoscope : ICloneable
{
	private readonly Dictionary<DivisionType, Grahas> _grahas = new ();

	private          Time                             _sunrise;
	private          Time                             _sunset;
	private          Time                             _nextSunrise;
	private          Time                             _nextSunset;

	public readonly Body[] HoraOrder =
	{
		Body.Sun,
		Body.Venus,
		Body.Mercury,
		Body.Moon,
		Body.Saturn,
		Body.Jupiter,
		Body.Mars
	};

	public HoraInfo Info { get;}

	public readonly Body[] KalaOrder =
	{
		Body.Sun,
		Body.Mars,
		Body.Jupiter,
		Body.Mercury,
		Body.Venus,
		Body.Saturn,
		Body.Moon,
		Body.Rahu
	};

	public readonly int Iflag = sweph.SEFLG_SWIEPH | sweph.SEFLG_SPEED | sweph.SEFLG_SIDEREAL;

	public Time LmtOffset
	{
		get;
		private set;
	}

	public Time  LmtSunrise
	{
		get;
		private set;
	}

	public Time LmtSunset
	{
		get;
		private set;
	}


	public Tables.Hora.Weekday LmtWday
	{
		get;
		private set;
	}

	public Time Sunrise => _sunrise;
	public Time Sunset  => _sunset;

	public Time NextSunrise => _nextSunrise;
	public Time NextSunset  => _nextSunset;
	
	
	public HoroscopeOptions Options
	{
		get;
		private set;
	}

	public List<Position> PositionList
	{
		get;
		private set;
	}


	private StrengthOptions _strengthOptions;
	public StrengthOptions StrengthOptions
	{
		get => _strengthOptions ??= MhoraGlobalOptions.Instance.SOptions.Clone();
		set => _strengthOptions = value;
	}

	public Longitude[] SwephHouseCusps
	{
		get;
		private set;
	}

	public int SwephHouseSystem
	{
		get;
		set;
	}

	public Tables.Hora.Weekday Wday
	{
		get; 
		private set;
	}

	public Horoscope(HoraInfo info, HoroscopeOptions options)
	{
		Options          = options;
		Info             = info;
		sweph.SetSidMode((int) options.Ayanamsa, 0.0, 0.0);
		SwephHouseSystem = 'P';
		PopulateCache();
		MhoraGlobalOptions.CalculationPrefsChanged += OnGlobalCalcPrefsChanged;
	}

	public object Clone()
	{
		var h = new Horoscope((HoraInfo) Info.Clone(), (HoroscopeOptions) Options.Clone());
		h.StrengthOptions = StrengthOptions.Clone();
	
		return h;
	}

	public Rashis FindRashis(Division division)
	{
		return (FindRashis(division.MultipleDivisions[0].Varga));
	}

	public Rashis FindRashis(DivisionType varga)
	{
		return FindGrahas(varga);
	}

	public Grahas FindGrahas(Division division)
	{
		return (FindGrahas(division.MultipleDivisions[0].Varga));
	}

	public Grahas FindGrahas(DivisionType varga)
	{
		if (_grahas.TryGetValue(varga, out var grahas) == false)
		{
			grahas = new Grahas(this, varga);
			_grahas.Add(varga, grahas);
		}
		try
		{
			grahas.Examine();
			return (grahas);
		}
		catch (Exception e)
		{
			Application.Log.Exception(e);
		}

		return null;
	}


	public event EvtChanged Changed;

	public Body LordOfZodiacHouse(ZodiacHouse zh, DivisionType varga, bool simpleLord)
	{
		if (simpleLord == false)
		{
			var grahas = FindGrahas(varga);
			var rules = this.RulesStrongerCoLord();

			switch (zh)
			{
				case ZodiacHouse.Aqu: return grahas.Stronger(Body.Rahu, Body.Saturn, true, rules, out _);
				case ZodiacHouse.Sco: return grahas.Stronger(Body.Ketu, Body.Mars, true, rules, out _);
			}
		}
		return zh.SimpleLordOfZodiacHouse();
	}

	public void OnGlobalCalcPrefsChanged(object o)
	{
		var ho = (HoroscopeOptions) o;
		Options.Copy(ho);
		StrengthOptions = null;
		OnChanged();
	}

	public void OnlySignalChanged()
	{
		Changed?.Invoke(this);
	}

	public void OnChanged()
	{
		PopulateCache();
		OnlySignalChanged();
	}

	public object UpdateHoraInfo(object o)
	{
		var i = (HoraInfo) o;
		Info.DateOfBirth = i.DateOfBirth;
		Info.Altitude    = i.Altitude;
		Info.Latitude    = i.Latitude;
		Info.Longitude   = i.Longitude;
		Info.Events      = (UserEvent[]) i.Events.Clone();
		OnChanged();
		return Info.Clone();
	}

	public Time GetLmtOffset(double baseUt)
	{
		var geopos = new double[3]
		{
			Info.Longitude,
			Info.Latitude,
			Info.Altitude
		};
		double tret        = 0;
		var    midnightUt = baseUt - Info.DateOfBirth.Time ().TotalDays;
		this.Lmt(midnightUt, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, ref tret);
		var lmtNoon1   = tret;
		var lmtOffset1 = lmtNoon1 - (midnightUt + 12.0 / 24.0);
		this.Lmt(midnightUt, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, ref tret);
		var lmtNoon2   = tret;
		var lmtOffset2 = lmtNoon2 - (midnightUt + 12.0 / 24.0);

		var retLmtOffset = (lmtOffset1 + lmtOffset2) / 2.0;
		//Mhora.Log.Debug("LMT: {0}, {1}", lmt_offset_1, lmt_offset_2);

		return retLmtOffset;
#if DND
			// determine offset from ephemeris time
			lmt_offset = 0;
			double tjd_et = baseUT + sweph.swe_deltat(baseUT);
			System.Text.StringBuilder s = new System.Text.StringBuilder(256);
			int ret = sweph.swe_time_equ(tjd_et, ref lmt_offset, s);
#endif
	}


	private void PopulateLmt()
	{
		LmtOffset  = GetLmtOffset(Info.Jd);
		LmtSunrise = 6.0  + LmtOffset * 24.0;
		LmtSunset  = 18.0 + LmtOffset * 24.0;
	}

	public double GetLmtOffsetDays(HoraInfo info, double baseUt)
	{
		var utLmtNoon = GetLmtOffset(info.Jd);
		var utNoon     = info.Jd - info.DateOfBirth.Time ().TotalDays + 12.0 / 24.0;
		return utLmtNoon - utNoon;
	}

	private void PopulateSunrisetCache()
	{
		double sunriseUt   = 0.0;

		PopulateSunrisetCacheHelper(Info.Jd, ref _nextSunrise, ref _nextSunset, ref sunriseUt);
		PopulateSunrisetCacheHelper(sunriseUt - 1.0 - 1.0 / 24.0, ref _sunrise, ref _sunset, ref sunriseUt);
		//Debug.WriteLine("Sunrise[t]: " + this.sunrise.ToString() + " " + this.sunrise.ToString(), "Basics");
	}

	public void PopulateSunrisetCacheHelper(double ut, ref Time sr, ref Time ss, ref double srUt)
	{
		var srflag = 0;
		switch (Options.SunrisePosition)
		{
			case HoroscopeOptions.SunrisePositionType.Lmt:
				sr = 6.0  + LmtOffset * 24.0;
				ss = 18.0 + LmtOffset * 24.0;
				break;
			case HoroscopeOptions.SunrisePositionType.TrueDiscEdge:
				srflag = sweph.SE_BIT_NO_REFRACTION;
				goto default;
			case HoroscopeOptions.SunrisePositionType.TrueDiscCenter:
				srflag = sweph.SE_BIT_NO_REFRACTION | sweph.SE_BIT_DISC_CENTER;
				goto default;
			case HoroscopeOptions.SunrisePositionType.ApparentDiscCenter:
				srflag = sweph.SE_BIT_DISC_CENTER;
				goto default;
			case HoroscopeOptions.SunrisePositionType.ApparentDiscEdge:
			default:
				//int sflag = 0;
				//if (options.sunrisePosition == HoroscopeOptions.SunrisePositionType.DiscCenter)
				//	sflag += 256;
				int year = 0, month = 0, day = 0;
				var hour = 0.0;

				var geopos = new double[3]
				{
					Info.Longitude,
					Info.Latitude,
					Info.Altitude
				};
				double tret = 0;

				if (this.Rise(ut, sweph.SE_SUN, srflag, geopos, 0.0, 0.0, ref tret) < 0)
				{
					MessageBox.Show("Invalid data");
					return;
				}

				srUt = tret;
				var sunrise = srUt.ToUtc();
				sweph.RevJul(tret, out year, out month, out day, out hour);
				sr = hour + Info.DstOffset.TotalHours;
				this.Set(tret, sweph.SE_SUN, srflag, geopos, 0.0, 0.0, ref tret);
				sweph.RevJul(tret, out year, out month, out day, out hour);
				ss = hour + Info.DstOffset.TotalHours;
				sr = Basics.NormalizeExc(sr, 0.0, 24.0);
				ss = Basics.NormalizeExc(ss, 0.0, 24.0);
				break;
		}
	}


	public double[] GetHoraCuspsUt()
	{
		double[] cusps = null;
		switch (Options.HoraType)
		{
			case HoroscopeOptions.EHoraType.Sunriset:
				cusps = GetSunrisetCuspsUt(12);
				break;
			case HoroscopeOptions.EHoraType.SunrisetEqual:
				cusps = GetSunrisetEqualCuspsUt(12);
				break;
			case HoroscopeOptions.EHoraType.Lmt:
				cusps = GetLmtCuspsUt(12);
				break;
		}

		return cusps;
	}

	public double[] GetKalaCuspsUt()
	{
		double[] cusps = null;
		switch (Options.KalaType)
		{
			case HoroscopeOptions.EHoraType.Sunriset:
				cusps = GetSunrisetCuspsUt(8);
				break;
			case HoroscopeOptions.EHoraType.SunrisetEqual:
				cusps = GetSunrisetEqualCuspsUt(8);
				break;
			case HoroscopeOptions.EHoraType.Lmt:
				cusps = GetLmtCuspsUt(8);
				break;
		}

		return cusps;
	}

	public double[] GetSunrisetCuspsUt(int dayParts)
	{
		var ret = new double[dayParts * 2 + 1];

		var srUt      = Info.Jd                 - HoursAfterSunrise() / 24.0;
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

		var srUt      = Info.Jd                                  - HoursAfterSunrise() / 24.0;
		var srNextUt = srUt - Sunrise                  / 24.0 + NextSunrise        / 24.0 + 1.0;
		var span       = (srNextUt - srUt) / (dayParts * 2);

		for (var i = 0; i <= dayParts * 2; i++)
		{
			ret[i] = srUt + span * i;
		}

		return ret;
	}

	public double[] GetLmtCuspsUt(int dayParts)
	{
		var ret            = new double[dayParts * 2 + 1];
		var srLmtUt      = Info.Jd                  - HoursAfterSunrise() / 24.0 - Sunrise / 24.0 + 6.0 / 24.0;
		var srLmtNextUt = srLmtUt                                                             + 1.0;
		//double sr_lmt_ut = this.info.Jd - this.info.DateOfBirth.time / 24.0 + 6.0 / 24.0;
		//double sr_lmt_next_ut = sr_lmt_ut + 1.0;

		srLmtUt      += LmtOffset;
		srLmtNextUt += LmtOffset;

		if (srLmtUt > Info.Jd)
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

	private void CalculateWeekday()
	{
		var m  = Info.DateOfBirth;
		var jd = sweph.JulDay(m.Year, m.Month, m.Day, 12.0);
		if (Info.DateOfBirth.Time().TotalHours < Sunrise)
		{
			jd -= 1;
		}

		Wday = (Tables.Hora.Weekday) sweph.DayOfWeek(jd);

		jd = sweph.JulDay(m.Year, m.Month, m.Day, 12.0);
		if (Info.DateOfBirth.Time().TotalHours < LmtSunrise)
		{
			jd -= 1;
		}

		LmtWday = (Tables.Hora.Weekday) sweph.DayOfWeek(jd);
	}

	
	private void AddOtherPoints()
	{
		var lagPos     = GetPosition(Body.Lagna).Longitude;
		var sunPos     = GetPosition(Body.Sun).Longitude;
		var moonPos    = GetPosition(Body.Moon).Longitude;
		var marsPos    = GetPosition(Body.Mars).Longitude;
		var jupPos     = GetPosition(Body.Jupiter).Longitude;
		var venPos     = GetPosition(Body.Venus).Longitude;
		var satPos     = GetPosition(Body.Saturn).Longitude;
		var rahPos     = GetPosition(Body.Rahu).Longitude;
		var mandiPos   = GetPosition(Body.Maandi).Longitude;
		var gulikaPos  = GetPosition(Body.Gulika).Longitude;
		var muhurtaPos = new Longitude(HoursAfterSunrise() / (NextSunrise + 24.0 - Sunrise) * 360.0);

		// add simple midpoints
		AddOtherPosition("User Specified", new Longitude(Options.CustomBodyLongitude.Value));
		AddOtherPosition("Brighu Bindu", rahPos.Add(moonPos.Sub(rahPos).Value / 2.0));
		AddOtherPosition("Muhurta Point", muhurtaPos);
		AddOtherPosition("Ra-Ke m.p", rahPos.Add(90.0));
		AddOtherPosition("Ke-Ra m.p", rahPos.Add(270.0));

		var l1Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse(), DivisionType.Rasi, false)).Longitude;
		var l6Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), DivisionType.Rasi, false)).Longitude;
		var l8Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), DivisionType.Rasi, false)).Longitude;
		var l12Pos = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), DivisionType.Rasi, false)).Longitude;

		var mritSatPos   = new Longitude(mandiPos.Value * 8.0 + satPos.Value   * 8.0);
		var mritJup2Pos  = new Longitude(satPos.Value   * 9.0 + mandiPos.Value * 18.0 + jupPos.Value  * 18.0);
		var mritSun2Pos  = new Longitude(satPos.Value   * 9.0 + mandiPos.Value * 18.0 + sunPos.Value  * 18.0);
		var mritMoon2Pos = new Longitude(satPos.Value   * 9.0 + mandiPos.Value * 18.0 + moonPos.Value * 18.0);

		if (IsDayBirth())
		{
			AddOtherPosition("Niryana: Su-Sa sum", sunPos.Add(satPos), Body.MrityuPoint);
		}
		else
		{
			AddOtherPosition("Niryana: Mo-Ra sum", moonPos.Add(rahPos), Body.MrityuPoint);
		}

		AddOtherPosition("Mrityu Sun: La-Mn sum", lagPos.Add(mandiPos), Body.MrityuPoint);
		AddOtherPosition("Mrityu Moon: Mo-Mn sum", moonPos.Add(mandiPos), Body.MrityuPoint);
		AddOtherPosition("Mrityu Lagna: La-Mo-Mn sum", lagPos.Add(moonPos).Add(mandiPos), Body.MrityuPoint);
		AddOtherPosition("Mrityu Sat: Mn8-Sa8", mritSatPos, Body.MrityuPoint);
		AddOtherPosition("6-8-12 sum", l6Pos.Add(l8Pos).Add(l12Pos), Body.MrityuPoint);
		AddOtherPosition("Mrityu Jup: Sa9-Mn18-Ju18", mritJup2Pos, Body.MrityuPoint);
		AddOtherPosition("Mrityu Sun: Sa9-Mn18-Su18", mritSun2Pos, Body.MrityuPoint);
		AddOtherPosition("Mrityu Moon: Sa9-Mn18-Mo18", mritMoon2Pos, Body.MrityuPoint);

		AddOtherPosition("Su-Mo sum", sunPos.Add(moonPos));
		AddOtherPosition("Ju-Mo-Ma sum", jupPos.Add(moonPos).Add(marsPos));
		AddOtherPosition("Su-Ve-Ju sum", sunPos.Add(venPos).Add(jupPos));
		AddOtherPosition("Sa-Mo-Ma sum", satPos.Add(moonPos).Add(marsPos));
		AddOtherPosition("La-Gu-Sa sum", lagPos.Add(gulikaPos).Add(satPos));
		AddOtherPosition("L-MLBase sum", l1Pos.Add(moonPos.ToZodiacHouseBase()));
	}

	public void PopulateHouseCusps()
	{
		SwephHouseCusps = new Longitude[13];
		var dCusps = new double[13];
		var ascmc  = new double[10];

		this.HousesEx(Info.Jd, sweph.SEFLG_SIDEREAL, Info.Latitude, Info.Longitude, (char) SwephHouseSystem, dCusps, ascmc);
		for (var i = 0; i < 12; i++)
		{
			SwephHouseCusps[i] = new Longitude(dCusps[i + 1]);
		}

		if (Options.BhavaType == HoroscopeOptions.EBhavaType.Middle)
		{
			var middle = new Longitude((dCusps[1] + dCusps[2]) / 2.0);
			var offset = middle.Sub(SwephHouseCusps[0]).Value;
			for (var i = 0; i < 12; i++)
			{
				SwephHouseCusps[i] = SwephHouseCusps[i].Sub(offset);
			}
		}


		SwephHouseCusps[12] = SwephHouseCusps[0];
	}

	private void PopulateCache()
	{
		// The stuff here is largely order sensitive
		// Try to add new definitions to the end
		sweph.SetEphePath(MhoraGlobalOptions.Instance.HOptions.EphemerisPath);
		// Find LMT offset
		PopulateLmt();
		// Sunrise (depends on lmt)
		PopulateSunrisetCache();
		// Basic grahas + Special lagnas (depend on sunrise)
		PositionList = this.CalculateBodyPositions(Sunrise);
		// Srilagna etc
		PositionList.Add(this.CalculateSl());
		PositionList.Add(this.CalculatePranapada());
		// Sun based Upagrahas (depends on sun)
		PositionList.AddRange(this.CalculateSunsUpagrahas());
		// Upagrahas (depends on sunrise)
		PositionList.AddRange(this.CalculateUpagrahas());
		// Weekday (depends on sunrise)
		CalculateWeekday();
		// Sahamas
		_ = FindGrahas(DivisionType.Rasi);
		PositionList.AddRange (this.CalculateSahamas());
		// Prana sphuta etc. (depends on upagrahas)
		GetPrashnaMargaPositions();
		PositionList.AddRange(this.CalculateChandraLagnas());
		AddOtherPoints();
		// Add extrapolated special lagnas (depends on sunrise)
		AddSpecialLagnaPositions();
		// Hora (depends on weekday)
		var hora = this.CalculateHora();
		// Populate house cusps on options refresh
		PopulateHouseCusps();
	}

	public double LengthOfDay()
	{
		return NextSunrise + 24.0 - Sunrise;
	}

	public double HoursAfterSunrise()
	{
		var ret = Info.DateOfBirth.Time ().TotalHours - Sunrise;
		if (ret < 0)
		{
			ret += 24.0;
		}

		return ret;
	}

	public double HoursAfterSunRiseSet()
	{
		double ret = 0;
		if (IsDayBirth())
		{
			ret = Info.DateOfBirth.Time ().TotalHours - Sunrise;
		}
		else
		{
			ret = Info.DateOfBirth.Time ().TotalHours - Sunset;
		}

		if (ret < 0)
		{
			ret += 24.0;
		}

		return ret;
	}

	public bool IsDayBirth()
	{
		if (Info.DateOfBirth.Time().TotalHours >= Sunrise && Info.DateOfBirth.Time().TotalHours < Sunset)
		{
			return true;
		}

		return false;
	}

	public void AddOtherPosition(string desc, Longitude lon, Body name)
	{
		var bp = new Position(this, name, BodyType.Other, lon, 0, 0, 0, 0, 0)
		{
			OtherString = desc
		};
		PositionList.Add(bp);
	}

	public void AddOtherPosition(string desc, Longitude lon)
	{
		AddOtherPosition(desc, lon, Body.Other);
	}

	public void AddSpecialLagnaPositions()
	{
		var diff = Info.DateOfBirth.Time().TotalHours - Sunrise;
		if (diff < 0)
		{
			diff += 24.0;
		}

		for (var i = 1; i <= 12; i++)
		{
			var specialDiff = diff * (i - 1);
			var tjd         = Info.Jd + specialDiff / 24.0;
			var asc         = this.Lagna(tjd);
			var desc        = string.Format("Special Lagna ({0:00})", i);
			AddOtherPosition(desc, new Longitude(asc));
		}
	}

	public void GetPrashnaMargaPositions()
	{
		var sunLon    = GetPosition(Body.Sun).Longitude;
		var moonLon   = GetPosition(Body.Moon).Longitude;
		var lagnaLon  = GetPosition(Body.Lagna).Longitude;
		var gulikaLon = GetPosition(Body.Gulika).Longitude;
		var rahuLon   = GetPosition(Body.Rahu).Longitude;

		var trisLon    = lagnaLon.Add(moonLon).Add(gulikaLon);
		var chatusLon  = trisLon.Add(sunLon);
		var panchasLon = chatusLon.Add(rahuLon);
		var pranaLon   = new Longitude(lagnaLon.Value  * 5.0).Add(gulikaLon);
		var dehaLon    = new Longitude(moonLon.Value   * 8.0).Add(gulikaLon);
		var mrityuLon  = new Longitude(gulikaLon.Value * 7.0).Add(sunLon);

		AddOtherPosition("Trih Sphuta", trisLon);
		AddOtherPosition("Chatuh Sphuta", chatusLon);
		AddOtherPosition("Panchah Sphuta", panchasLon);
		AddOtherPosition("Pranah Sphuta", pranaLon);
		AddOtherPosition("Deha Sphuta", dehaLon);
		AddOtherPosition("Mrityu Sphuta", mrityuLon);
	}

	public Position GetPosition(Body b)
	{
		var index = b.Index();
		var t     = PositionList[index].GetType();
		var s     = t.ToString();
		Trace.Assert(index >= 0 && index < PositionList.Count, "Horoscope::getPosition 1");
		Trace.Assert(PositionList[index].GetType() == typeof(Position), "Horoscope::getPosition 2");
		var bp = (Position) PositionList[b.Index()];
		if (bp.Name == b)
		{
			return bp;
		}

		for (var i = (int) Body.Lagna + 1; i < PositionList.Count; i++)
		{
			var position = (Position) PositionList[i];
			if (b == position.Name)
			{
				return position;
			}
		}

		Trace.Assert(false, "Basics::GetPosition. Unable to find body");
		return (Position) PositionList[0];
	}

}