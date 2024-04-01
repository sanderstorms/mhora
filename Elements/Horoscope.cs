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

	public HoraInfo Info
	{
		get;
		private set;
	}

	public Vara Vara
	{
		get;
		private set;
	}

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

	public Horoscope(HoraInfo info, HoroscopeOptions options)
	{
		Options = options;
		sweph.SetSidMode((int) options.Ayanamsa, 0.0, 0.0);
		SwephHouseSystem = 'P';
		SetInfo(info);
		MhoraGlobalOptions.CalculationPrefsChanged += OnGlobalCalcPrefsChanged;
	}

	public void SetInfo (HoraInfo info)
	{
		Info = info;
		Vara = new Vara(this);
		PopulateCache();
		CheckBirthTime();
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

	public void UpdateHoraInfo(HoraInfo info)
	{
		SetInfo(info);
		Info.Events = (UserEvent[]) info.Events.Clone();
		OnChanged();
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

	#region birth time rectification
	int CheckBirthTime()
	{
		int correct = 0;

		if (CheckBirthTime1())
		{
			correct++;
		}

		if (CheckBirthTime2())
		{
			correct++;
		}

		if (CheckBirthTime3())
		{
			correct++;
		}

		if (GetSex1() == GetSex2())
		{
			correct++;
		}

		//Pranapada lagna will be trine or 7th to lagna.
		var lagna = _grahas[DivisionType.Rasi][Body.Lagna];
		var pp    = _grahas[DivisionType.Rasi][Body.Pranapada];

		if (lagna.HouseFrom(pp) == Bhava.JayaBhava)
		{
			correct++;
		}
		else if (lagna.HouseFrom(pp).IsTrikona())
		{
			correct++;
		}

		//Pranapada lagna in Navamsa in trines or 7th to swamsa or navamsa Moon
		pp =  FindGrahas(DivisionType.Navamsa)[Body.Pranapada];
		var m9 = FindGrahas(DivisionType.Navamsa)[Body.Moon];

		if (pp.HouseFrom(m9) == Bhava.JayaBhava)
		{
			correct++;
		}
		else if (pp.HouseFrom(m9).IsTrikona())
		{
			correct++;
		}

		//Check what sign AtmaKaraka planet is in in the Navamsa chart
		var ak = FindGrahas(DivisionType.Rasi).Find(Karaka8.Atma);
		var sv = FindGrahas(DivisionType.Navamsa)[ak];

		if (sv.HouseFrom(m9) == Bhava.JayaBhava)
		{
			correct++;
		}
		else if (sv.HouseFrom(m9).IsTrikona())
		{
			correct++;
		}

		if (CheckKetu60())
		{
			correct++;
		}

		if (CheckKundaLagna())
		{
			correct++;
		}

		if (CheckKundaMoon())
		{
			correct++;
		}

		if (Vara.BirthTatva.Tatva == Tatva.Jal)
		{
			correct++;
		}
		else if (Vara.BirthTatva.AntaraTatva == Tatva.Jal)
		{
			correct++;
		}

		return correct;
	}

	//The Kunda should be placed in trines (dharma) or in seventh (dwara) from the Rasi Lagna, to cause creation.
	//Hence for rectification purposes Kunda should be in trines or seventh from Lagna. 
	public bool CheckKundaLagna()
	{
		var kunda = FindGrahas(DivisionType.NavaNavamsa)[Body.Lagna];
		var lagna = FindGrahas(DivisionType.Rasi)[Body.Lagna];

		if (lagna.HouseFrom(kunda) == Bhava.JayaBhava)
		{
			return (true);
		}

		if (lagna.HouseFrom(kunda).IsTrikona())
		{
			return (true);
		}

		return (false);
	}

	//if the Kunda (Navamsa-Navamsa) longitude of Lagna conjunct with the Nakshatra of Natal Moon
	//or its trine Nakshatras the given BT can be considered correct 
	public bool CheckKundaMoon()
	{
		var kunda = FindGrahas(DivisionType.NavaNavamsa)[Body.Lagna];
		var moon  = FindGrahas(DivisionType.Rasi)[Body.Moon];

		if (moon.Position.Longitude.ToNakshatra() == kunda.Position.Longitude.ToNakshatra())
		{
			return (true);
		}

		if (moon.HouseFrom(kunda).IsTrikona())
		{
			return (true);
		}

		return (false);
	}

	// Ketu is our past life karma, dropping us in this incarnation. 
	// Check what sign Ketu is in, and see where the lord has gone. The lord must have rasi-aspect on the Ascendant. 
	public bool CheckKetu60()
	{
		var ketu  = FindGrahas(DivisionType.Shashtyamsa)[Body.Ketu];
		var lagna = FindGrahas(DivisionType.Shashtyamsa)[Body.Lagna];

		if (ketu.HouseLord.Rashi.ZodiacHouse.RasiDristi(lagna.Rashi))
		{
			return (true);
		}

		return (false);
	}


	bool IsBirthNakshatra (Nakshatra nakshatra, int offset)
	{
		if (nakshatra == Nakshatra.Aswini.Add(offset))
		{
			return (true);
		}

		if (nakshatra == Nakshatra.Makha.Add(offset))
		{
			return (true);
		}

		if (nakshatra == Nakshatra.Moola.Add(offset))
		{
			return (true);
		}

		return (false);
	}


	// From the time of sun � rise (LMT) to the given LMT of
	// birth, note Ghati and Vighati that have elapsed. Convert
	// this duration of time into Vighatis, multiply by 4 and divide
	// by 9. The remainder counted from Aswini, Magha and
	// Moola should give constellation at the time of birth.
	bool CheckBirthTime1()
	{
		var vighati = (int) Vara.HoursAfterSunrise.Vighati * 4;
		var offset  = (vighati % 9);

		var lagna     = _grahas[DivisionType.Rasi][Body.Lagna];
		var nakshatra = lagna.Position.Longitude.ToNakshatra();

		if (IsBirthNakshatra(nakshatra, offset))
		{
			return (true);
		}

		return (false);
	}

	// Convert Isht kala into Vighatis, multiply it by 2, add 5 to
	// the result if Lagna sign is movable, 10 if fixed and 15 if
	// dual. Multiply result by 4 and divide by 9 and count the
	// remainder from Aswini, Magha or Moola to get the birth
	// constellation.
	bool CheckBirthTime2()
	{
		var vighati = (int) Vara.HoursAfterSunrise.Vighati;
		vighati *= 2;

		var lagna     = _grahas[DivisionType.Rasi][Body.Lagna];
		var nakshatra = lagna.Position.Longitude.ToNakshatra();

		if (lagna.Rashi.ZodiacHouse.IsMoveableSign())
		{
			vighati += 5;
		}
		else if (lagna.Rashi.ZodiacHouse.IsFixedSign())
		{
			vighati += 10;
		}
		else
		{
			vighati += 15;
		}

		vighati *= 4;
		var offset = vighati % 9;

		if (IsBirthNakshatra(nakshatra, offset))
		{
			return (true);
		}

		return (false);
	}

	// Multiply Isht kal into Vighatis by 3 and divide by 7. The
	// remainder counted from Sunday in order of weak days must
	// agree with day of birth.
	bool CheckBirthTime3()
	{
		var vighati = (int) Vara.HoursAfterSunrise.Vighati;
		vighati *= 3;

		var offset    = vighati % 7;
		var dayOfWeek = (Weekday.Sunday.Index () + offset).NormalizeInc(0, 6);

		if (Vara.WeekDay.Index() == dayOfWeek)
		{
			return (true);
		}
		return (false);
	}


	// From the time of sun � rise (LMT) to the given LMT of
	// birth, note Ghati and Vighati that have elapsed. Convert
	// this duration of time into Vighatis, multiply by 4 and divide
	// by 9. The remainder counted from Aswini, Magha and
	// Moola should give constellation at the time of birth.
	public Kuta.Sex GetSex1()
	{
		var vighati = (int) Vara.HoursAfterSunrise.Vighati;
		vighati %= 225;

		if (vighati <= 15)
		{
			return Kuta.Sex.Male;
		}

		if (vighati <= 45)
		{
			return Kuta.Sex.Female;
		}

		if (vighati <= 90)
		{
			return Kuta.Sex.Male;
		}

		if (vighati <= 150)
		{
			return Kuta.Sex.Female;
		}

		return Kuta.Sex.Male;
	}

	// The sex of a person is determined by the element ruling at the time of birth.
	// The five elements repeat after every 90 minutes on any day.
	// Thus 16 such cyclical repetitions take place in one day. The duration of each
	//  element is fixed. The sequence and duration of elements is as under:
	//  Prithvi (6 mnts.), Jal (12 mnts.), Agni (18 mnts.), Vayu (24 mnts.), and Akash (30 mnts.)
	public Kuta.Sex GetSex2 ()
	{
		var hoursAfterBirth = Vara.HoursAfterSunrise.TotalMinutes;
		var offset          = hoursAfterBirth % 90;

		var element = 6;
		if (offset < element)
		{
			return (Kuta.Sex.Neutral);
		}

		element += 12;
		if (offset < element)
		{
			return (Kuta.Sex.Female);
		}

		element += 18;
		if (offset < element)
		{
			return (Kuta.Sex.Male);
		}
		element += 24;
		if (offset < element)
		{
			return (Kuta.Sex.Neutral);
		}

		return (Kuta.Sex.Male);
	}
	#endregion
}