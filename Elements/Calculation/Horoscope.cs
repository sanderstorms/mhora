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
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using Mhora.Components.Delegates;
using Mhora.Database.Settings;
using Mhora.SwissEph;
using Mhora.Tables;
using mhora.Util;
using Mhora.Util;

namespace Mhora.Elements.Calculation;

/// <summary>
///     Contains all the information for a horoscope. i.e. All ephemeris lookups
///     have been completed, sunrise/sunset has been calculated etc.
/// </summary>
public class Horoscope : ICloneable
{
	private static readonly string[] VarnadaStrs =
	{
		"VL",
		"V2",
		"V3",
		"V4",
		"V5",
		"V6",
		"V7",
		"V8",
		"V9",
		"V10",
		"V11",
		"V12"
	};

	public readonly Body.BodyType[] HoraOrder =
	{
		Body.BodyType.Sun,
		Body.BodyType.Venus,
		Body.BodyType.Mercury,
		Body.BodyType.Moon,
		Body.BodyType.Saturn,
		Body.BodyType.Jupiter,
		Body.BodyType.Mars
	};

	public HoraInfo Info { get; set;}

	public readonly Body.BodyType[] KalaOrder =
	{
		Body.BodyType.Sun,
		Body.BodyType.Mars,
		Body.BodyType.Jupiter,
		Body.BodyType.Mercury,
		Body.BodyType.Venus,
		Body.BodyType.Saturn,
		Body.BodyType.Moon,
		Body.BodyType.Rahu
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

	private Time _nextSunrise;
	public  Time NextSunrise => _nextSunrise;

	private Time _nextSunset;
	public Time NextSunset => _nextSunset;
	
	
	public HoroscopeOptions Options
	{
		get;
		private set;
	}

	public ArrayList PositionList
	{
		get;
		private set;
	}


	public StrengthOptions StrengthOptions
	{
		get;
		set;
	}

	private Time _sunrise;
	public  Time Sunrise => _sunrise;

	private Time        _sunset;
	public  Time        Sunset => _sunset;

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
		sweph.SetSidMode((int) HoroscopeOptions.AyanamsaType.TrueCitra, 0.0, 0.0);
		SwephHouseSystem = 'P';
		PopulateCache();
		MhoraGlobalOptions.CalculationPrefsChanged += OnGlobalCalcPrefsChanged;
	}

	public object Clone()
	{
		var h = new Horoscope((HoraInfo) Info.Clone(), (HoroscopeOptions) Options.Clone());
		if (StrengthOptions != null)
		{
			h.StrengthOptions = (StrengthOptions) StrengthOptions.Clone();
		}

		return h;
	}

	public event EvtChanged Changed;

	public Body.BodyType LordOfZodiacHouse(ZodiacHouse zh, Division dtype)
	{
		return LordOfZodiacHouse(zh.Sign, dtype);
	}

	public Body.BodyType LordOfZodiacHouse(ZodiacHouse.Rasi zh, Division dtype)
	{
		var fsColord = new FindStronger(this, dtype, FindStronger.RulesStrongerCoLord(this));

		switch (zh)
		{
			case ZodiacHouse.Rasi.Aqu: return fsColord.StrongerGraha(Body.BodyType.Rahu, Body.BodyType.Saturn, true);
			case ZodiacHouse.Rasi.Sco: return fsColord.StrongerGraha(Body.BodyType.Ketu, Body.BodyType.Mars, true);
			default:                   return zh.SimpleLordOfZodiacHouse();
		}
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

	public DivisionPosition CalculateDivisionPosition(Position bp, Division d)
	{
		return bp.ToDivisionPosition(d);
	}

	public ArrayList CalculateDivisionPositions(Division d)
	{
		var al = new ArrayList();
		foreach (Position bp in PositionList)
		{
			al.Add(CalculateDivisionPosition(bp, d));
		}

		return al;
	}

	private DivisionPosition CalculateGrahaArudhaDivisionPosition(Body.BodyType bn, ZodiacHouse zh, Division dtype)
	{
		var dp    = GetPosition(bn).ToDivisionPosition(dtype);
		var dpl   = GetPosition(Body.BodyType.Lagna).ToDivisionPosition(dtype);
		var rel   = dp.ZodiacHouse.NumHousesBetween(zh);
		var hse   = dpl.ZodiacHouse.NumHousesBetween(zh);
		var zhsum = zh.Add(rel);
		var rel2  = dp.ZodiacHouse.NumHousesBetween(zhsum);
		if (rel2 == 1 || rel2 == 7)
		{
			zhsum = zhsum.Add(10);
		}

		var dp2 = new DivisionPosition(Body.BodyType.Other, Body.Type.GrahaArudha, zhsum, 0, 0, 0);
		//dp2.Longitude   = zhsum.DivisionalLongitude(dp.Longitude, dpl.part);
		dp2.Description = string.Format("{0}{1}", bn.ToShortString(), hse);
		return dp2;
	}

	public ArrayList CalculateGrahaArudhaDivisionPositions(Division dtype)
	{
		object[][] parameters =
		{
			new object[]
			{
				ZodiacHouse.Rasi.Ari,
				Body.BodyType.Mars
			},
			new object[]
			{
				ZodiacHouse.Rasi.Tau,
				Body.BodyType.Venus
			},
			new object[]
			{
				ZodiacHouse.Rasi.Gem,
				Body.BodyType.Mercury
			},
			new object[]
			{
				ZodiacHouse.Rasi.Can,
				Body.BodyType.Moon
			},
			new object[]
			{
				ZodiacHouse.Rasi.Leo,
				Body.BodyType.Sun
			},
			new object[]
			{
				ZodiacHouse.Rasi.Vir,
				Body.BodyType.Mercury
			},
			new object[]
			{
				ZodiacHouse.Rasi.Lib,
				Body.BodyType.Venus
			},
			new object[]
			{
				ZodiacHouse.Rasi.Sco,
				Body.BodyType.Mars
			},
			new object[]
			{
				ZodiacHouse.Rasi.Sag,
				Body.BodyType.Jupiter
			},
			new object[]
			{
				ZodiacHouse.Rasi.Cap,
				Body.BodyType.Saturn
			},
			new object[]
			{
				ZodiacHouse.Rasi.Aqu,
				Body.BodyType.Saturn
			},
			new object[]
			{
				ZodiacHouse.Rasi.Pis,
				Body.BodyType.Jupiter
			},
			new object[]
			{
				ZodiacHouse.Rasi.Sco,
				Body.BodyType.Ketu
			},
			new object[]
			{
				ZodiacHouse.Rasi.Aqu,
				Body.BodyType.Rahu
			}
		};
		var al = new ArrayList(14);

		for (var i = 0; i < parameters.Length; i++)
		{
			al.Add(CalculateGrahaArudhaDivisionPosition((Body.BodyType) parameters[i][1], new ZodiacHouse((ZodiacHouse.Rasi) parameters[i][0]), dtype));
		}

		return al;
	}

	public ArrayList CalculateVarnadaDivisionPositions(Division dtype)
	{
		var al     = new ArrayList(12);
		var zhL  = GetPosition(Body.BodyType.Lagna).ToDivisionPosition(dtype).ZodiacHouse;
		var zhHl = GetPosition(Body.BodyType.HoraLagna).ToDivisionPosition(dtype).ZodiacHouse;

		var zhAri = new ZodiacHouse(ZodiacHouse.Rasi.Ari);
		var zhPis = new ZodiacHouse(ZodiacHouse.Rasi.Pis);
		for (var i = 1; i <= 12; i++)
		{
			zhL  = zhL.Add(i);
			zhHl = zhHl.Add(i);

			int iL = 0, iHl = 0;
			if (zhL.IsOdd())
			{
				iL = zhAri.NumHousesBetween(zhL);
			}
			else
			{
				iL = zhPis.NumHousesBetweenReverse(zhL);
			}

			if (zhHl.IsOdd())
			{
				iHl = zhAri.NumHousesBetween(zhHl);
			}
			else
			{
				iHl = zhPis.NumHousesBetweenReverse(zhHl);
			}

			var sum = 0;
			if (zhL.IsOdd() == zhHl.IsOdd())
			{
				sum = iL + iHl;
			}
			else
			{
				sum = Math.Max(iL, iHl) - Math.Min(iL, iHl);
			}

			ZodiacHouse zhV = null;
			if (zhL.IsOdd())
			{
				zhV = zhAri.Add(sum);
			}
			else
			{
				zhV = zhPis.AddReverse(sum);
			}

			var divPos = new DivisionPosition(Body.BodyType.Other, Body.Type.Varnada, zhV, 0, 0, 0);

			divPos.Description = VarnadaStrs[i - 1];
			al.Add(divPos);
		}

		return al;
	}

	private DivisionPosition CalculateArudhaDivisionPosition(ZodiacHouse zh, Body.BodyType bn, Body.BodyType aname, Division d, Body.Type btype)
	{
		var bp    = GetPosition(bn);
		var zhb   = CalculateDivisionPosition(bp, d).ZodiacHouse;
		var rel   = zh.NumHousesBetween(zhb);
		var zhsum = zhb.Add(rel);
		var rel2  = zh.NumHousesBetween(zhsum);
		if (rel2 == 1 || rel2 == 7)
		{
			zhsum = zhsum.Add(10);
		}

		var dp = new DivisionPosition(aname, btype, zhsum, 0, 0, 0);
		//dp.Longitude = zhsum.DivisionalLongitude(bp.longitude, dp.part);

		return dp;
	}

	public ArrayList CalculateArudhaDivisionPositions(Division d)
	{
		Body.BodyType[] bnlist =
		{
			Body.BodyType.Other,
			Body.BodyType.AL,
			Body.BodyType.A2,
			Body.BodyType.A3,
			Body.BodyType.A4,
			Body.BodyType.A5,
			Body.BodyType.A6,
			Body.BodyType.A7,
			Body.BodyType.A8,
			Body.BodyType.A9,
			Body.BodyType.A10,
			Body.BodyType.A11,
			Body.BodyType.UL
		};

		var              fsColord       = new FindStronger(this, d, FindStronger.RulesStrongerCoLord(this));
		var              arudhaDivList = new ArrayList(14);
		DivisionPosition first, second;
		for (var j = 1; j <= 12; j++)
		{
			var           zlagna     = CalculateDivisionPosition(GetPosition(Body.BodyType.Lagna), d).ZodiacHouse;
			var           zh         = zlagna.Add(j);
			var bnWeaker   = Body.BodyType.Other;
			var           bnStronger = zh.Sign.SimpleLordOfZodiacHouse();
			if (zh.Sign == ZodiacHouse.Rasi.Aqu)
			{
				bnStronger = fsColord.StrongerGraha(Body.BodyType.Rahu, Body.BodyType.Saturn, true);
				bnWeaker   = fsColord.WeakerGraha(Body.BodyType.Rahu, Body.BodyType.Saturn, true);
			}
			else if (zh.Sign == ZodiacHouse.Rasi.Sco)
			{
				bnStronger = fsColord.StrongerGraha(Body.BodyType.Ketu, Body.BodyType.Mars, true);
				bnWeaker   = fsColord.WeakerGraha(Body.BodyType.Ketu, Body.BodyType.Mars, true);
			}

			first = CalculateArudhaDivisionPosition(zh, bnStronger, bnlist[j], d, Body.Type.BhavaArudha);
			arudhaDivList.Add(first);
			if (zh.Sign == ZodiacHouse.Rasi.Aqu || zh.Sign == ZodiacHouse.Rasi.Sco)
			{
				second = CalculateArudhaDivisionPosition(zh, bnWeaker, bnlist[j], d, Body.Type.BhavaArudhaSecondary);
				if (first.ZodiacHouse.Sign != second.ZodiacHouse.Sign)
				{
					arudhaDivList.Add(second);
				}
			}
		}

		return arudhaDivList;
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
		//mhora.Log.Debug("LMT: {0}, {1}", lmt_offset_1, lmt_offset_2);

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
		Time sunriseUt   = 0.0;

		PopulateSunrisetCacheHelper(Info.Jd, ref _nextSunrise, ref _nextSunset, ref sunriseUt);
		PopulateSunrisetCacheHelper(sunriseUt - 1.0 - 1.0 / 24.0, ref _sunrise, ref _sunset, ref sunriseUt);
		//Debug.WriteLine("Sunrise[t]: " + this.sunrise.ToString() + " " + this.sunrise.ToString(), "Basics");
	}

	public void PopulateSunrisetCacheHelper(double ut, ref Time sr, ref Time ss, ref Time srUt)
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
				sweph.RevJul(tret, ref year, ref month, ref day, ref hour);
				sr = hour + Info.DstOffset.TotalHours;
				this.Set(tret, sweph.SE_SUN, srflag, geopos, 0.0, 0.0, ref tret);
				sweph.RevJul(tret, ref year, ref month, ref day, ref hour);
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

	public Body.BodyType CalculateKala()
	{
		var iBase = 0;
		return CalculateKala(ref iBase);
	}

	public Body.BodyType CalculateKala(ref int iBase)
	{
		int[] offsetsDay =
		{
			0,
			6,
			1,
			3,
			2,
			4,
			5
		};
		var b          = Wday.WeekdayRuler();
		var bdayBirth = IsDayBirth();

		var cusps = GetKalaCuspsUt();
		if (Options.KalaType == HoroscopeOptions.EHoraType.Lmt)
		{
			b          = LmtWday.WeekdayRuler();
			bdayBirth = Info.DateOfBirth.Time().TotalHours > LmtSunset || Info.DateOfBirth.Time().TotalHours < LmtSunrise;
		}

		var i = offsetsDay[(int) b];
		iBase = i;
		var j = 0;

		if (bdayBirth)
		{
			for (j = 0; j < 8; j++)
			{
				if (Info.Jd >= cusps[j] && Info.Jd < cusps[j + 1])
				{
					break;
				}
			}

			i += j;
			while (i >= 8)
			{
				i -= 8;
			}

			return KalaOrder[i];
		}

		//i+=4;
		for (j = 8; j < 16; j++)
		{
			if (Info.Jd >= cusps[j] && Info.Jd < cusps[j + 1])
			{
				break;
			}
		}

		i += j;
		while (i >= 8)
		{
			i -= 8;
		}

		return KalaOrder[i];
	}

	public Body.BodyType CalculateHora()
	{
		var iBody = 0;
		return CalculateHora(Info.Jd, ref iBody);
	}

	public Body.BodyType CalculateHora(double baseUt, ref int baseBody)
	{
		int[] offsets =
		{
			0,
			3,
			6,
			2,
			5,
			1,
			4
		};
		var b     = Wday.WeekdayRuler();
		var cusps = GetHoraCuspsUt();
		if (Options.HoraType == HoroscopeOptions.EHoraType.Lmt)
		{
			b = LmtWday.WeekdayRuler();
		}

		var i = offsets[(int) b];
		baseBody = i;
		var j = 0;
		//for (j=0; j<23; j++)
		//{
		//	Moment m1 = new Moment(cusps[j], this);
		//	Moment m2 = new Moment(cusps[j+1], this);
		//	mhora.Log.Debug ("Seeing if dob is between {0} and {1}", m1, m2);
		//}
		for (j = 0; j < 23; j++)
		{
			if (baseUt >= cusps[j] && baseUt < cusps[j + 1])
			{
				break;
			}
		}

		//mhora.Log.Debug ("Found hora in the {0}th hora", j);
		i += j;
		while (i >= 7)
		{
			i -= 7;
		}

		return HoraOrder[i];
	}

	private Body.BodyType CalculateUpagrahasStart()
	{
		if (IsDayBirth())
		{
			return Wday.WeekdayRuler();
		}

		switch (Wday)
		{
			default:
			case Tables.Hora.Weekday.Sunday: return Body.BodyType.Jupiter;
			case Tables.Hora.Weekday.Monday:    return Body.BodyType.Venus;
			case Tables.Hora.Weekday.Tuesday:   return Body.BodyType.Saturn;
			case Tables.Hora.Weekday.Wednesday: return Body.BodyType.Sun;
			case Tables.Hora.Weekday.Thursday:  return Body.BodyType.Moon;
			case Tables.Hora.Weekday.Friday:    return Body.BodyType.Mars;
			case Tables.Hora.Weekday.Saturday:  return Body.BodyType.Mercury;
		}
	}

	private void CalculateUpagrahasSingle(Body.BodyType b, double tjd)
	{
		var lon = new Longitude(0.0);
		lon.Value = this.Lagna(tjd);
		var bp = new Position(this, b, Body.Type.Upagraha, lon, 0, 0, 0, 0, 0);
		PositionList.Add(bp);
	}

	private void CalculateMaandiHelper(Body.BodyType b, HoroscopeOptions.EMaandiType mty, double[] jds, double dOffset, int[] bodyOffsets)
	{
		switch (mty)
		{
			case HoroscopeOptions.EMaandiType.SaturnBegin:
				CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.BodyType.Saturn]]);
				break;
			case HoroscopeOptions.EMaandiType.SaturnMid:
				CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.BodyType.Saturn]] + dOffset);
				break;
			case HoroscopeOptions.EMaandiType.SaturnEnd:
			case HoroscopeOptions.EMaandiType.LordlessBegin:
				var off1 = bodyOffsets[(int) Body.BodyType.Saturn]                      + 1;
				CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.BodyType.Saturn]] + dOffset * 2.0);
				break;
			case HoroscopeOptions.EMaandiType.LordlessMid:
				CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.BodyType.Saturn]] + dOffset * 3.0);
				break;
			case HoroscopeOptions.EMaandiType.LordlessEnd:
				CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.BodyType.Saturn]] + dOffset * 4.0);
				break;
		}
	}

	private void CalculateUpagrahas()
	{
		double dStart = 0, dEnd = 0;

		var m         = Info.DateOfBirth;
		dStart = dEnd = sweph.JulDay(m.Year, m.Month, m.Day, -Info.DstOffset.TotalHours);
		var bStart    = CalculateUpagrahasStart();

		if (IsDayBirth())
		{
			dStart += Sunrise / 24.0;
			dEnd   += Sunset  / 24.0;
		}
		else
		{
			dStart += Sunset / 24.0;
			dEnd   += 1.0 + Sunrise / 24.0;
		}

		var dPeriod = (dEnd - dStart) / 8.0;
		var dOffset = dPeriod         / 2.0;

		var jds = new double[8];
		for (var i = 0; i < 8; i++)
		{
			jds[i] = dStart + i * dPeriod + dOffset;
		}

		var bodyOffsets = new int[8];
		for (var i = 0; i < 8; i++)
		{
			var ib = (int) bStart + i;
			while (ib >= 8)
			{
				ib -= 8;
			}

			bodyOffsets[ib] = i;
		}

		double dUpagrahaOffset = 0;
		switch (Options.UpagrahaType)
		{
			case HoroscopeOptions.EUpagrahaType.Begin:
				dUpagrahaOffset = 0;
				break;
			case HoroscopeOptions.EUpagrahaType.Mid:
				dUpagrahaOffset = dOffset;
				break;
			case HoroscopeOptions.EUpagrahaType.End:
				dUpagrahaOffset = dPeriod;
				break;
		}

		CalculateUpagrahasSingle(Body.BodyType.Kala, jds[bodyOffsets[(int) Body.BodyType.Sun]]);
		CalculateUpagrahasSingle(Body.BodyType.Mrityu, jds[bodyOffsets[(int) Body.BodyType.Mars]]);
		CalculateUpagrahasSingle(Body.BodyType.ArthaPraharaka, jds[bodyOffsets[(int) Body.BodyType.Mercury]]);
		CalculateUpagrahasSingle(Body.BodyType.YamaGhantaka, jds[bodyOffsets[(int) Body.BodyType.Jupiter]]);


		CalculateMaandiHelper(Body.BodyType.Maandi, Options.MaandiType, jds, dOffset, bodyOffsets);
		CalculateMaandiHelper(Body.BodyType.Gulika, Options.GulikaType, jds, dOffset, bodyOffsets);
	}

	private void CalculateSunsUpagrahas()
	{
		var slon = GetPosition(Body.BodyType.Sun).Longitude;

		var bpDhuma = new Position(this, Body.BodyType.Dhuma, Body.Type.Upagraha, slon.Add(133.0 + 20.0 / 60.0), 0, 0, 0, 0, 0);

		var bpVyatipata = new Position(this, Body.BodyType.Vyatipata, Body.Type.Upagraha, new Longitude(360.0).Sub(bpDhuma.Longitude), 0, 0, 0, 0, 0);

		var bpParivesha = new Position(this, Body.BodyType.Parivesha, Body.Type.Upagraha, bpVyatipata.Longitude.Add(180), 0, 0, 0, 0, 0);

		var bpIndrachapa = new Position(this, Body.BodyType.Indrachapa, Body.Type.Upagraha, new Longitude(360.0).Sub(bpParivesha.Longitude), 0, 0, 0, 0, 0);

		var bpUpaketu = new Position(this, Body.BodyType.Upaketu, Body.Type.Upagraha, slon.Sub(30), 0, 0, 0, 0, 0);

		PositionList.Add(bpDhuma);
		PositionList.Add(bpVyatipata);
		PositionList.Add(bpParivesha);
		PositionList.Add(bpIndrachapa);
		PositionList.Add(bpUpaketu);
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

	private void AddChandraLagna(string desc, Longitude lon)
	{
		var bp = new Position(this, Body.BodyType.Other, Body.Type.ChandraLagna, lon, 0, 0, 0, 0, 0);
		bp.OtherString = desc;
		PositionList.Add(bp);
	}

	private void CalculateChandraLagnas()
	{
		var bpMoon  = GetPosition(Body.BodyType.Moon);
		var lonBase = new Longitude(bpMoon.ExtrapolateLongitude(new Division(Vargas.DivisionType.Navamsa)).ToZodiacHouseBase());
		lonBase = lonBase.Add(bpMoon.Longitude.ToZodiacHouseOffset());

		//mhora.Log.Debug ("Starting Chandra Ayur Lagna from {0}", lon_base);

		var istaGhati = (Info.DateOfBirth.Time().TotalHours - Sunrise).NormalizeExc(0.0, 24.0) * 2.5;
		var glLon     = lonBase.Add(new Longitude(istaGhati        * 30.0));
		var hlLon     = lonBase.Add(new Longitude(istaGhati * 30.0 / 2.5));
		var blLon     = lonBase.Add(new Longitude(istaGhati * 30.0 / 5.0));

		var vl = istaGhati * 5.0;
		while (istaGhati > 12.0)
		{
			istaGhati -= 12.0;
		}

		var vlLon = lonBase.Add(new Longitude(vl * 30.0));

		AddChandraLagna("Chandra Lagna - GL", glLon);
		AddChandraLagna("Chandra Lagna - HL", hlLon);
		AddChandraLagna("Chandra Ayur Lagna - BL", blLon);
		AddChandraLagna("Chandra Lagna - ViL", vlLon);
	}

	private void CalculateSl()
	{
		var mpos  = GetPosition(Body.BodyType.Moon).Longitude;
		var lpos  = GetPosition(Body.BodyType.Lagna).Longitude;
		var sldeg = mpos.ToNakshatraOffset() / (360.0 / 27.0) * 360.0;
		var slLon = lpos.Add(sldeg);
		var bp    = new Position(this, Body.BodyType.SreeLagna, Body.Type.SpecialLagna, slLon, 0, 0, 0, 0, 0);
		PositionList.Add(bp);
	}

	private void CalculatePranapada()
	{
		var spos   = GetPosition(Body.BodyType.Sun).Longitude;
		var offset = Info.DateOfBirth.Time().TotalHours - Sunrise;
		if (offset < 0)
		{
			offset += 24.0;
		}

		offset *= 60.0 * 60.0 / 6.0;
		Longitude ppos = null;
		switch ((int) spos.ToZodiacHouse().Sign % 3)
		{
			case 1:
				ppos = spos.Add(offset);
				break;
			case 2:
				ppos = spos.Add(offset + 8.0 * 30.0);
				break;
			default:
			case 0:
				ppos = spos.Add(offset + 4.0 * 30.0);
				break;
		}

		var bp = new Position(this, Body.BodyType.Pranapada, Body.Type.SpecialLagna, ppos, 0, 0, 0, 0, 0);
		PositionList.Add(bp);
	}

	private void AddOtherPoints()
	{
		var lagPos     = GetPosition(Body.BodyType.Lagna).Longitude;
		var sunPos     = GetPosition(Body.BodyType.Sun).Longitude;
		var moonPos    = GetPosition(Body.BodyType.Moon).Longitude;
		var marsPos    = GetPosition(Body.BodyType.Mars).Longitude;
		var jupPos     = GetPosition(Body.BodyType.Jupiter).Longitude;
		var venPos     = GetPosition(Body.BodyType.Venus).Longitude;
		var satPos     = GetPosition(Body.BodyType.Saturn).Longitude;
		var rahPos     = GetPosition(Body.BodyType.Rahu).Longitude;
		var mandiPos   = GetPosition(Body.BodyType.Maandi).Longitude;
		var gulikaPos  = GetPosition(Body.BodyType.Gulika).Longitude;
		var muhurtaPos = new Longitude(HoursAfterSunrise() / (NextSunrise + 24.0 - Sunrise) * 360.0);

		// add simple midpoints
		AddOtherPosition("User Specified", new Longitude(Options.CustomBodyLongitude.Value));
		AddOtherPosition("Brighu Bindu", rahPos.Add(moonPos.Sub(rahPos).Value / 2.0));
		AddOtherPosition("Muhurta Point", muhurtaPos);
		AddOtherPosition("Ra-Ke m.p", rahPos.Add(90));
		AddOtherPosition("Ke-Ra m.p", rahPos.Add(270));

		var l1Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse(), new Division(Vargas.DivisionType.Rasi))).Longitude;
		var l6Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), new Division(Vargas.DivisionType.Rasi))).Longitude;
		var l8Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), new Division(Vargas.DivisionType.Rasi))).Longitude;
		var l12Pos = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), new Division(Vargas.DivisionType.Rasi))).Longitude;

		var mritSatPos   = new Longitude(mandiPos.Value * 8.0 + satPos.Value   * 8.0);
		var mritJup2Pos  = new Longitude(satPos.Value   * 9.0 + mandiPos.Value * 18.0 + jupPos.Value  * 18.0);
		var mritSun2Pos  = new Longitude(satPos.Value   * 9.0 + mandiPos.Value * 18.0 + sunPos.Value  * 18.0);
		var mritMoon2Pos = new Longitude(satPos.Value   * 9.0 + mandiPos.Value * 18.0 + moonPos.Value * 18.0);

		if (IsDayBirth())
		{
			AddOtherPosition("Niryana: Su-Sa sum", sunPos.Add(satPos), Body.BodyType.MrityuPoint);
		}
		else
		{
			AddOtherPosition("Niryana: Mo-Ra sum", moonPos.Add(rahPos), Body.BodyType.MrityuPoint);
		}

		AddOtherPosition("Mrityu Sun: La-Mn sum", lagPos.Add(mandiPos), Body.BodyType.MrityuPoint);
		AddOtherPosition("Mrityu Moon: Mo-Mn sum", moonPos.Add(mandiPos), Body.BodyType.MrityuPoint);
		AddOtherPosition("Mrityu Lagna: La-Mo-Mn sum", lagPos.Add(moonPos).Add(mandiPos), Body.BodyType.MrityuPoint);
		AddOtherPosition("Mrityu Sat: Mn8-Sa8", mritSatPos, Body.BodyType.MrityuPoint);
		AddOtherPosition("6-8-12 sum", l6Pos.Add(l8Pos).Add(l12Pos), Body.BodyType.MrityuPoint);
		AddOtherPosition("Mrityu Jup: Sa9-Mn18-Ju18", mritJup2Pos, Body.BodyType.MrityuPoint);
		AddOtherPosition("Mrityu Sun: Sa9-Mn18-Su18", mritSun2Pos, Body.BodyType.MrityuPoint);
		AddOtherPosition("Mrityu Moon: Sa9-Mn18-Mo18", mritMoon2Pos, Body.BodyType.MrityuPoint);

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
		CalculateSl();
		CalculatePranapada();
		// Sun based Upagrahas (depends on sun)
		CalculateSunsUpagrahas();
		// Upagrahas (depends on sunrise)
		CalculateUpagrahas();
		// Weekday (depends on sunrise)
		CalculateWeekday();
		// Sahamas
		CalculateSahamas();
		// Prana sphuta etc. (depends on upagrahas)
		GetPrashnaMargaPositions();
		CalculateChandraLagnas();
		AddOtherPoints();
		// Add extrapolated special lagnas (depends on sunrise)
		AddSpecialLagnaPositions();
		// Hora (depends on weekday)
		CalculateHora();
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

	public void AddOtherPosition(string desc, Longitude lon, Body.BodyType name)
	{
		var bp = new Position(this, name, Body.Type.Other, lon, 0, 0, 0, 0, 0);
		bp.OtherString = desc;
		PositionList.Add(bp);
	}

	public void AddOtherPosition(string desc, Longitude lon)
	{
		AddOtherPosition(desc, lon, Body.BodyType.Other);
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
		var sunLon    = GetPosition(Body.BodyType.Sun).Longitude;
		var moonLon   = GetPosition(Body.BodyType.Moon).Longitude;
		var lagnaLon  = GetPosition(Body.BodyType.Lagna).Longitude;
		var gulikaLon = GetPosition(Body.BodyType.Gulika).Longitude;
		var rahuLon   = GetPosition(Body.BodyType.Rahu).Longitude;

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

	public Position GetPosition(Body.BodyType b)
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

		for (var i = (int) Body.BodyType.Lagna + 1; i < PositionList.Count; i++)
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

	private Position SahamaHelper(string sahama, Body.BodyType b, Body.BodyType a, Body.BodyType c)
	{
		var lonA = GetPosition(a).Longitude;
		var       lonB = GetPosition(b).Longitude;
		var       lonC = GetPosition(c).Longitude;
		return SahamaHelper(sahama, lonB, lonA, lonC);
	}

	private Position SahamaHelper(string sahama, Body.BodyType b, Body.BodyType a, Longitude lonC)
	{
		var lonA = GetPosition(a).Longitude;
		var       lonB = GetPosition(b).Longitude;
		return SahamaHelper(sahama, lonB, lonA, lonC);
	}

	private Position SahamaHelper(string sahama, Longitude lonB, Body.BodyType a, Body.BodyType c)
	{
		var lonA = GetPosition(a).Longitude;
		var       lonC = GetPosition(c).Longitude;
		return SahamaHelper(sahama, lonB, lonA, lonC);
	}

	private Position SahamaHelper(string sahama, Longitude lonB, Longitude lonA, Longitude lonC)
	{
		// b-a+c
		var bDay = IsDayBirth();

		var lonR = lonB.Sub(lonA).Add(lonC);
		if (lonB.Sub(lonA).Value <= lonC.Sub(lonA).Value)
		{
			lonR = lonR.Add(new Longitude(30.0));
		}

		var bp = new Position(this, Body.BodyType.Other, Body.Type.Sahama, lonR, 0.0, 0.0, 0.0, 0.0, 0.0);
		bp.OtherString = sahama;
		return bp;
	}

	private Position SahamaDnHelper(string sahama, Longitude lonB, Longitude lonA, Longitude lonC)
	{
		// b-a+c
		var       bDay = IsDayBirth();
		Longitude lonR;
		if (bDay)
		{
			lonR = lonB.Sub(lonA).Add(lonC);
		}
		else
		{
			lonR = lonA.Sub(lonB).Add(lonC);
		}

		if (lonB.Sub(lonA).Value <= lonC.Sub(lonA).Value)
		{
			lonR = lonR.Add(new Longitude(30.0));
		}

		var bp = new Position(this, Body.BodyType.Other, Body.Type.Sahama, lonR, 0.0, 0.0, 0.0, 0.0, 0.0);
		bp.OtherString = sahama;
		return bp;
	}

	private Position SahamaDnHelper(string sahama, Body.BodyType b, Longitude lonA, Body.BodyType c)
	{
		var lonB = GetPosition(b).Longitude;
		var       lonC = GetPosition(c).Longitude;
		return SahamaDnHelper(sahama, lonB, lonA, lonC);
	}

	private Position SahamaDnHelper(string sahama, Longitude lonB, Body.BodyType a, Body.BodyType c)
	{
		var lonA = GetPosition(a).Longitude;
		var       lonC = GetPosition(c).Longitude;
		return SahamaDnHelper(sahama, lonB, lonA, lonC);
	}

	private Position SahamaDnHelper(string sahama, Longitude lonB, Longitude lonA, Body.BodyType c)
	{
		var lonC = GetPosition(c).Longitude;
		return SahamaDnHelper(sahama, lonB, lonA, lonC);
	}

	private Position SahamaDnHelper(string sahama, Body.BodyType b, Body.BodyType a, Body.BodyType c)
	{
		var lonA = GetPosition(a).Longitude;
		var       lonB = GetPosition(b).Longitude;
		var       lonC = GetPosition(c).Longitude;
		return SahamaDnHelper(sahama, lonB, lonA, lonC);
	}

	private Position SahamaHelperNormalize(Position b, Body.BodyType lower, Body.BodyType higher)
	{
		var lonA = GetPosition(lower).Longitude;
		var lonB = GetPosition(higher).Longitude;
		if (b.Longitude.Sub(lonA).Value < lonB.Sub(lonA).Value)
		{
			return b;
		}

		b.Longitude = b.Longitude.Add(new Longitude(30.0));
		return b;
	}

	public ArrayList CalculateSahamas()
	{
		var bDay      = IsDayBirth();
		var al        = new ArrayList();
		var lonLagna = GetPosition(Body.BodyType.Lagna).Longitude;
		var lonBase  = new Longitude(lonLagna.ToZodiacHouseBase());
		var zhLagna  = lonLagna.ToZodiacHouse();
		var zhMoon   = GetPosition(Body.BodyType.Moon).Longitude.ToZodiacHouse();
		var zhSun    = GetPosition(Body.BodyType.Sun).Longitude.ToZodiacHouse();


		// Fixed positions. Relied on by other sahams
		al.Add(SahamaDnHelper("Punya", Body.BodyType.Moon, Body.BodyType.Sun, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Vidya", Body.BodyType.Sun, Body.BodyType.Moon, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Sastra", Body.BodyType.Jupiter, Body.BodyType.Saturn, Body.BodyType.Mercury));

		// Variable positions.
		al.Add(SahamaDnHelper("Yasas", Body.BodyType.Jupiter, ((Position) al[0]).Longitude, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Mitra", Body.BodyType.Jupiter, ((Position) al[0]).Longitude, Body.BodyType.Venus));
		al.Add(SahamaDnHelper("Mahatmya", ((Position) al[0]).Longitude, Body.BodyType.Mars, Body.BodyType.Lagna));

		var bLagnaLord = LordOfZodiacHouse(zhLagna, new Division(Vargas.DivisionType.Rasi));
		if (bLagnaLord != Body.BodyType.Mars)
		{
			al.Add(SahamaDnHelper("Samartha", Body.BodyType.Mars, bLagnaLord, Body.BodyType.Lagna));
		}
		else
		{
			al.Add(SahamaDnHelper("Samartha", Body.BodyType.Jupiter, Body.BodyType.Mars, Body.BodyType.Lagna));
		}

		al.Add(SahamaHelper("Bhratri", Body.BodyType.Jupiter, Body.BodyType.Saturn, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Gaurava", Body.BodyType.Jupiter, Body.BodyType.Moon, Body.BodyType.Sun));
		al.Add(SahamaDnHelper("Pitri", Body.BodyType.Saturn, Body.BodyType.Sun, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Rajya", Body.BodyType.Saturn, Body.BodyType.Sun, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Matri", Body.BodyType.Moon, Body.BodyType.Venus, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Putra", Body.BodyType.Jupiter, Body.BodyType.Moon, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Jeeva", Body.BodyType.Saturn, Body.BodyType.Jupiter, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Karma", Body.BodyType.Mars, Body.BodyType.Mercury, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Roga", Body.BodyType.Lagna, Body.BodyType.Moon, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Kali", Body.BodyType.Jupiter, Body.BodyType.Mars, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Bandhu", Body.BodyType.Mercury, Body.BodyType.Moon, Body.BodyType.Lagna));
		al.Add(SahamaHelper("Mrityu", lonBase.Add(8.0   * 30.0), Body.BodyType.Moon, Body.BodyType.Lagna));
		al.Add(SahamaHelper("Paradesa", lonBase.Add(9.0 * 30.0), LordOfZodiacHouse(zhLagna.Add(9), new Division(Vargas.DivisionType.Rasi)), Body.BodyType.Lagna));
		al.Add(SahamaHelper("Artha", lonBase.Add(2.0    * 30.0), LordOfZodiacHouse(zhLagna.Add(2), new Division(Vargas.DivisionType.Rasi)), Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Paradara", Body.BodyType.Venus, Body.BodyType.Sun, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Vanik", Body.BodyType.Moon, Body.BodyType.Mercury, Body.BodyType.Lagna));

		if (bDay)
		{
			al.Add(SahamaHelper("Karyasiddhi", Body.BodyType.Saturn, Body.BodyType.Sun, LordOfZodiacHouse(zhSun, new Division(Vargas.DivisionType.Rasi))));
		}
		else
		{
			al.Add(SahamaHelper("Karyasiddhi", Body.BodyType.Saturn, Body.BodyType.Moon, LordOfZodiacHouse(zhMoon, new Division(Vargas.DivisionType.Rasi))));
		}

		al.Add(SahamaDnHelper("Vivaha", Body.BodyType.Venus, Body.BodyType.Saturn, Body.BodyType.Lagna));
		al.Add(SahamaHelper("Santapa", Body.BodyType.Saturn, Body.BodyType.Moon, lonBase.Add(6.0 * 30.0)));
		al.Add(SahamaDnHelper("Sraddha", Body.BodyType.Venus, Body.BodyType.Mars, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Preeti", ((Position) al[2]).Longitude, ((Position) al[0]).Longitude, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Jadya", Body.BodyType.Mars, Body.BodyType.Saturn, Body.BodyType.Mercury));
		al.Add(SahamaHelper("Vyapara", Body.BodyType.Mars, Body.BodyType.Saturn, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Satru", Body.BodyType.Mars, Body.BodyType.Saturn, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Jalapatana", new Longitude(105.0), Body.BodyType.Saturn, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Bandhana", ((Position) al[0]).Longitude, Body.BodyType.Saturn, Body.BodyType.Lagna));
		al.Add(SahamaDnHelper("Apamrityu", lonBase.Add(8.0 * 30.0), Body.BodyType.Mars, Body.BodyType.Lagna));
		al.Add(SahamaHelper("Labha", lonBase.Add(11.0      * 30.0), LordOfZodiacHouse(zhLagna.Add(11), new Division(Vargas.DivisionType.Rasi)), Body.BodyType.Lagna));

		PositionList.AddRange(al);
		return al;
	}
}