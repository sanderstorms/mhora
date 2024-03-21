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
using Mhora.Calculation;
using Mhora.Components.Delegates;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements.Extensions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     Contains all the information for a horoscope. i.e. All ephemeris lookups
///     have been completed, sunrise/sunset has been calculated etc.
/// </summary>
public partial class Horoscope : ICloneable
{
	private readonly Dictionary<DivisionType, Grahas> _grahas = new ();

	public readonly int      Iflag = sweph.SEFLG_SWIEPH | sweph.SEFLG_SPEED | sweph.SEFLG_SIDEREAL;

	public HoraInfo Info { get;}
	public Vara     Vara { get; }

	public HoroscopeOptions Options { get; }

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

	public Weekday Wday
	{
		get; 
		private set;
	}

	public Horoscope(HoraInfo info, HoroscopeOptions options)
	{
		Options = options;
		Info    = info;
		Vara    = new Vara(this);
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

		double     tret       = 0;
		JulianDate midnightUt = baseUt - Info.DateOfBirth.Time ().TotalDays;
		this.Lmt(midnightUt, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, ref tret);
		JulianDate lmtNoon1   = tret;
		Time lmtOffset1 = lmtNoon1 - (midnightUt + 12.0 / 24.0);
		this.Lmt(midnightUt, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, ref tret);
		JulianDate lmtNoon2   = tret;
		Time lmtOffset2 = lmtNoon2 - (midnightUt + 12.0 / 24.0);

		Time retLmtOffset = (lmtOffset1 + lmtOffset2) / 2.0;
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



	public JulianDate[] GetHoraCuspsUt()
	{
		JulianDate[] cusps = null;
		switch (Options.HoraType)
		{
			case HoroscopeOptions.EHoraType.Sunriset:
				cusps = Vara.GetSunrisetCuspsUt(12);
				break;
			case HoroscopeOptions.EHoraType.SunrisetEqual:
				cusps = Vara.GetSunrisetEqualCuspsUt(12);
				break;
			case HoroscopeOptions.EHoraType.Lmt:
				cusps = Vara.GetLmtCuspsUt(12);
				break;
		}

		return cusps;
	}

	public JulianDate[] GetKalaCuspsUt()
	{
		JulianDate[] cusps = null;
		switch (Options.KalaType)
		{
			case HoroscopeOptions.EHoraType.Sunriset:
				cusps = Vara.GetSunrisetCuspsUt(8);
				break;
			case HoroscopeOptions.EHoraType.SunrisetEqual:
				cusps = Vara.GetSunrisetEqualCuspsUt(8);
				break;
			case HoroscopeOptions.EHoraType.Lmt:
				cusps = Vara.GetLmtCuspsUt(8);
				break;
		}

		return cusps;
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


}