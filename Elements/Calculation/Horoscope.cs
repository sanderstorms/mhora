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
using Mhora.Elements.Hora;
using Mhora.SwissEph;
using Mhora.Tables;

namespace Mhora.Elements.Calculation;

/// <summary>
///     Contains all the information for a horoscope. i.e. All ephemeris lookups
///     have been completed, sunrise/sunset has been calculated etc.
/// </summary>
public class Horoscope : ICloneable
{
	private readonly string[] varnada_strs =
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

	public double ayanamsa;
	public double baseUT;


	public Body.Name[] horaOrder =
	{
		Body.Name.Sun,
		Body.Name.Venus,
		Body.Name.Mercury,
		Body.Name.Moon,
		Body.Name.Saturn,
		Body.Name.Jupiter,
		Body.Name.Mars
	};

	public HoraInfo info;

	public Body.Name[] kalaOrder =
	{
		Body.Name.Sun,
		Body.Name.Mars,
		Body.Name.Jupiter,
		Body.Name.Mercury,
		Body.Name.Venus,
		Body.Name.Saturn,
		Body.Name.Moon,
		Body.Name.Rahu
	};

	public double           lmt_offset;
	public double           lmt_sunrise;
	public double           lmt_sunset;
	public Basics.Weekday   lmt_wday;
	public double           next_sunrise;
	public double           next_sunset;
	public HoroscopeOptions options;
	public ArrayList        positionList;
	public StrengthOptions  strength_options;
	public double           sunrise;
	public double           sunset;
	public Longitude[]      swephHouseCusps;
	public int              swephHouseSystem;

	public Basics.Weekday wday;

	public Horoscope(HoraInfo _info, HoroscopeOptions _options)
	{
		options          = _options;
		info             = _info;
		swephHouseSystem = 'P';
		populateCache();
		MhoraGlobalOptions.CalculationPrefsChanged += OnGlobalCalcPrefsChanged;
	}

	public object Clone()
	{
		var h = new Horoscope((HoraInfo) info.Clone(), (HoroscopeOptions) options.Clone());
		if (strength_options != null)
		{
			h.strength_options = (StrengthOptions) strength_options.Clone();
		}

		return h;
	}

	public event EvtChanged Changed;

	public Body.Name LordOfZodiacHouse(ZodiacHouse zh, Division dtype)
	{
		return LordOfZodiacHouse(zh.value, dtype);
	}

	public Body.Name LordOfZodiacHouse(ZodiacHouse.Name zh, Division dtype)
	{
		var fs_colord = new FindStronger(this, dtype, FindStronger.RulesStrongerCoLord(this));

		switch (zh)
		{
			case ZodiacHouse.Name.Aqu: return fs_colord.StrongerGraha(Body.Name.Rahu, Body.Name.Saturn, true);
			case ZodiacHouse.Name.Sco: return fs_colord.StrongerGraha(Body.Name.Ketu, Body.Name.Mars, true);
			default:                   return Basics.SimpleLordOfZodiacHouse(zh);
		}
	}

	public void OnGlobalCalcPrefsChanged(object o)
	{
		var ho = (HoroscopeOptions) o;
		options.Copy(ho);
		strength_options = null;
		OnChanged();
	}

	public void OnlySignalChanged()
	{
		if (Changed != null)
		{
			Changed(this);
		}
	}

	public void OnChanged()
	{
		populateCache();
		if (Changed != null)
		{
			Changed(this);
		}
	}

	public DivisionPosition CalculateDivisionPosition(Position bp, Division d)
	{
		return bp.toDivisionPosition(d);
	}

	public ArrayList CalculateDivisionPositions(Division d)
	{
		var al = new ArrayList();
		foreach (Position bp in positionList)
		{
			al.Add(CalculateDivisionPosition(bp, d));
		}

		return al;
	}

	private DivisionPosition CalculateGrahaArudhaDivisionPosition(Body.Name bn, ZodiacHouse zh, Division dtype)
	{
		var dp    = getPosition(bn).toDivisionPosition(dtype);
		var dpl   = getPosition(Body.Name.Lagna).toDivisionPosition(dtype);
		var rel   = dp.zodiac_house.numHousesBetween(zh);
		var hse   = dpl.zodiac_house.numHousesBetween(zh);
		var zhsum = zh.add(rel);
		var rel2  = dp.zodiac_house.numHousesBetween(zhsum);
		if (rel2 == 1 || rel2 == 7)
		{
			zhsum = zhsum.add(10);
		}

		var dp2 = new DivisionPosition(Body.Name.Other, Body.Type.GrahaArudha, zhsum, 0, 0, 0);
		//dp2.Longitude   = zhsum.DivisionalLongitude(dp.Longitude, dpl.part);
		dp2.Description = string.Format("{0}{1}", Body.toShortString(bn), hse);
		return dp2;
	}

	public ArrayList CalculateGrahaArudhaDivisionPositions(Division dtype)
	{
		object[][] parameters =
		{
			new object[]
			{
				ZodiacHouse.Name.Ari,
				Body.Name.Mars
			},
			new object[]
			{
				ZodiacHouse.Name.Tau,
				Body.Name.Venus
			},
			new object[]
			{
				ZodiacHouse.Name.Gem,
				Body.Name.Mercury
			},
			new object[]
			{
				ZodiacHouse.Name.Can,
				Body.Name.Moon
			},
			new object[]
			{
				ZodiacHouse.Name.Leo,
				Body.Name.Sun
			},
			new object[]
			{
				ZodiacHouse.Name.Vir,
				Body.Name.Mercury
			},
			new object[]
			{
				ZodiacHouse.Name.Lib,
				Body.Name.Venus
			},
			new object[]
			{
				ZodiacHouse.Name.Sco,
				Body.Name.Mars
			},
			new object[]
			{
				ZodiacHouse.Name.Sag,
				Body.Name.Jupiter
			},
			new object[]
			{
				ZodiacHouse.Name.Cap,
				Body.Name.Saturn
			},
			new object[]
			{
				ZodiacHouse.Name.Aqu,
				Body.Name.Saturn
			},
			new object[]
			{
				ZodiacHouse.Name.Pis,
				Body.Name.Jupiter
			},
			new object[]
			{
				ZodiacHouse.Name.Sco,
				Body.Name.Ketu
			},
			new object[]
			{
				ZodiacHouse.Name.Aqu,
				Body.Name.Rahu
			}
		};
		var al = new ArrayList(14);

		for (var i = 0; i < parameters.Length; i++)
		{
			al.Add(CalculateGrahaArudhaDivisionPosition((Body.Name) parameters[i][1], new ZodiacHouse((ZodiacHouse.Name) parameters[i][0]), dtype));
		}

		return al;
	}

	public ArrayList CalculateVarnadaDivisionPositions(Division dtype)
	{
		var al     = new ArrayList(12);
		var _zh_l  = getPosition(Body.Name.Lagna).toDivisionPosition(dtype).zodiac_house;
		var _zh_hl = getPosition(Body.Name.HoraLagna).toDivisionPosition(dtype).zodiac_house;

		var zh_ari = new ZodiacHouse(ZodiacHouse.Name.Ari);
		var zh_pis = new ZodiacHouse(ZodiacHouse.Name.Pis);
		for (var i = 1; i <= 12; i++)
		{
			var zh_l  = _zh_l.add(i);
			var zh_hl = _zh_hl.add(i);

			int i_l = 0, i_hl = 0;
			if (zh_l.isOdd())
			{
				i_l = zh_ari.numHousesBetween(zh_l);
			}
			else
			{
				i_l = zh_pis.numHousesBetweenReverse(zh_l);
			}

			if (zh_hl.isOdd())
			{
				i_hl = zh_ari.numHousesBetween(zh_hl);
			}
			else
			{
				i_hl = zh_pis.numHousesBetweenReverse(zh_hl);
			}

			var sum = 0;
			if (zh_l.isOdd() == zh_hl.isOdd())
			{
				sum = i_l + i_hl;
			}
			else
			{
				sum = Math.Max(i_l, i_hl) - Math.Min(i_l, i_hl);
			}

			ZodiacHouse zh_v = null;
			if (zh_l.isOdd())
			{
				zh_v = zh_ari.add(sum);
			}
			else
			{
				zh_v = zh_pis.addReverse(sum);
			}

			var div_pos = new DivisionPosition(Body.Name.Other, Body.Type.Varnada, zh_v, 0, 0, 0);

			div_pos.Description = varnada_strs[i - 1];
			al.Add(div_pos);
		}

		return al;
	}

	private DivisionPosition CalculateArudhaDivisionPosition(ZodiacHouse zh, Body.Name bn, Body.Name aname, Division d, Body.Type btype)
	{
		var bp    = getPosition(bn);
		var zhb   = CalculateDivisionPosition(bp, d).zodiac_house;
		var rel   = zh.numHousesBetween(zhb);
		var zhsum = zhb.add(rel);
		var rel2  = zh.numHousesBetween(zhsum);
		if (rel2 == 1 || rel2 == 7)
		{
			zhsum = zhsum.add(10);
		}

		var dp = new DivisionPosition(aname, btype, zhsum, 0, 0, 0);
		//dp.Longitude = zhsum.DivisionalLongitude(bp.longitude, dp.part);

		return dp;
	}

	public ArrayList CalculateArudhaDivisionPositions(Division d)
	{
		Body.Name[] bnlist =
		{
			Body.Name.Other,
			Body.Name.AL,
			Body.Name.A2,
			Body.Name.A3,
			Body.Name.A4,
			Body.Name.A5,
			Body.Name.A6,
			Body.Name.A7,
			Body.Name.A8,
			Body.Name.A9,
			Body.Name.A10,
			Body.Name.A11,
			Body.Name.UL
		};

		var              fs_colord       = new FindStronger(this, d, FindStronger.RulesStrongerCoLord(this));
		var              arudha_div_list = new ArrayList(14);
		DivisionPosition first, second;
		for (var j = 1; j <= 12; j++)
		{
			var       zlagna                 = CalculateDivisionPosition(getPosition(Body.Name.Lagna), d).zodiac_house;
			var       zh                     = zlagna.add(j);
			Body.Name bn_stronger, bn_weaker = Body.Name.Other;
			bn_stronger = Basics.SimpleLordOfZodiacHouse(zh.value);
			if (zh.value == ZodiacHouse.Name.Aqu)
			{
				bn_stronger = fs_colord.StrongerGraha(Body.Name.Rahu, Body.Name.Saturn, true);
				bn_weaker   = fs_colord.WeakerGraha(Body.Name.Rahu, Body.Name.Saturn, true);
			}
			else if (zh.value == ZodiacHouse.Name.Sco)
			{
				bn_stronger = fs_colord.StrongerGraha(Body.Name.Ketu, Body.Name.Mars, true);
				bn_weaker   = fs_colord.WeakerGraha(Body.Name.Ketu, Body.Name.Mars, true);
			}

			first = CalculateArudhaDivisionPosition(zh, bn_stronger, bnlist[j], d, Body.Type.BhavaArudha);
			arudha_div_list.Add(first);
			if (zh.value == ZodiacHouse.Name.Aqu || zh.value == ZodiacHouse.Name.Sco)
			{
				second = CalculateArudhaDivisionPosition(zh, bn_weaker, bnlist[j], d, Body.Type.BhavaArudhaSecondary);
				if (first.zodiac_house.value != second.zodiac_house.value)
				{
					arudha_div_list.Add(second);
				}
			}
		}

		return arudha_div_list;
	}

	public object UpdateHoraInfo(object o)
	{
		var i = (HoraInfo) o;
		info.DateOfBirth = i.DateOfBirth;
		info.Altitude    = i.Altitude;
		info.Latitude    = i.Latitude;
		info.Longitude   = i.Longitude;
		info.tz          = i.tz;
		info.Events      = (UserEvent[]) i.Events.Clone();
		OnChanged();
		return info.Clone();
	}

	private void populateLmt()
	{
		lmt_offset  = getLmtOffset(info, baseUT);
		lmt_sunrise = 6.0  + lmt_offset * 24.0;
		lmt_sunset  = 18.0 + lmt_offset * 24.0;
	}

	public double getLmtOffsetDays(HoraInfo info, double _baseUT)
	{
		var ut_lmt_noon = getLmtOffset(info, _baseUT);
		var ut_noon     = baseUT - info.tob.time / 24.0 + 12.0 / 24.0;
		return ut_lmt_noon - ut_noon;
	}

	public double getLmtOffset(HoraInfo _info, double _baseUT)
	{
		var geopos = new double[3]
		{
			_info.lon.toDouble(),
			_info.lat.toDouble(),
			_info.alt
		};
		var tret = new double[6]
		{
			0,
			0,
			0,
			0,
			0,
			0
		};
		var midnight_ut = _baseUT - _info.tob.time / 24.0;
		sweph.Lmt(midnight_ut, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, tret);
		var lmt_noon_1   = tret[0];
		var lmt_offset_1 = lmt_noon_1 - (midnight_ut + 12.0 / 24.0);
		sweph.Lmt(midnight_ut, sweph.SE_SUN, sweph.SE_CALC_MTRANSIT, geopos, 0.0, 0.0, tret);
		var lmt_noon_2   = tret[0];
		var lmt_offset_2 = lmt_noon_2 - (midnight_ut + 12.0 / 24.0);

		var ret_lmt_offset = (lmt_offset_1 + lmt_offset_2) / 2.0;
		//mhora.Log.Debug("LMT: {0}, {1}", lmt_offset_1, lmt_offset_2);

		return ret_lmt_offset;
#if DND
			// determine offset from ephemeris time
			lmt_offset = 0;
			double tjd_et = baseUT + sweph.swe_deltat(baseUT);
			System.Text.StringBuilder s = new System.Text.StringBuilder(256);
			int ret = sweph.swe_time_equ(tjd_et, ref lmt_offset, s);
#endif
	}

	private void populateSunrisetCache()
	{
		var sunrise_ut = 0.0;
		populateSunrisetCacheHelper(baseUT, ref next_sunrise, ref next_sunset, ref sunrise_ut);
		populateSunrisetCacheHelper(sunrise_ut - 1.0 - 1.0 / 24.0, ref sunrise, ref sunset, ref sunrise_ut);
		//Debug.WriteLine("Sunrise[t]: " + this.sunrise.ToString() + " " + this.sunrise.ToString(), "Basics");
	}

	public void populateSunrisetCacheHelper(double ut, ref double sr, ref double ss, ref double sr_ut)
	{
		var srflag = 0;
		switch (options.sunrisePosition)
		{
			case HoroscopeOptions.SunrisePositionType.Lmt:
				sr = 6.0  + lmt_offset * 24.0;
				ss = 18.0 + lmt_offset * 24.0;
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
					info.lon.toDouble(),
					info.lat.toDouble(),
					info.alt
				};
				var tret = new double[6]
				{
					0,
					0,
					0,
					0,
					0,
					0
				};

				if (sweph.Rise(ut, sweph.SE_SUN, srflag, geopos, 0.0, 0.0, tret) < 0)
				{
					MessageBox.Show("Invalid data");
					return;
				}

				sr_ut = tret[0];
				sweph.RevJul(tret[0], ref year, ref month, ref day, ref hour);
				sr = hour + info.tz.toDouble();
				sweph.Set(tret[0], sweph.SE_SUN, srflag, geopos, 0.0, 0.0, tret);
				sweph.RevJul(tret[0], ref year, ref month, ref day, ref hour);
				ss = hour + info.tz.toDouble();
				sr = Basics.normalize_exc(0.0, 24.0, sr);
				ss = Basics.normalize_exc(0.0, 24.0, ss);
				break;
		}
	}


	public double[] getHoraCuspsUt()
	{
		double[] cusps = null;
		switch (options.HoraType)
		{
			case HoroscopeOptions.EHoraType.Sunriset:
				cusps = getSunrisetCuspsUt(12);
				break;
			case HoroscopeOptions.EHoraType.SunrisetEqual:
				cusps = getSunrisetEqualCuspsUt(12);
				break;
			case HoroscopeOptions.EHoraType.Lmt:
				sweph.obtainLock(this);
				cusps = getLmtCuspsUt(12);
				sweph.releaseLock(this);
				break;
		}

		return cusps;
	}

	public double[] getKalaCuspsUt()
	{
		double[] cusps = null;
		switch (options.KalaType)
		{
			case HoroscopeOptions.EHoraType.Sunriset:
				cusps = getSunrisetCuspsUt(8);
				break;
			case HoroscopeOptions.EHoraType.SunrisetEqual:
				cusps = getSunrisetEqualCuspsUt(8);
				break;
			case HoroscopeOptions.EHoraType.Lmt:
				sweph.obtainLock(this);
				cusps = getLmtCuspsUt(8);
				sweph.releaseLock(this);
				break;
		}

		return cusps;
	}

	public double[] getSunrisetCuspsUt(int dayParts)
	{
		var ret = new double[dayParts * 2 + 1];

		var sr_ut      = baseUT                 - hoursAfterSunrise() / 24.0;
		var ss_ut      = sr_ut - sunrise / 24.0 + sunset              / 24.0;
		var sr_next_ut = sr_ut - sunrise / 24.0 + next_sunrise        / 24.0 + 1.0;

		var day_span   = (ss_ut      - sr_ut) / dayParts;
		var night_span = (sr_next_ut - ss_ut) / dayParts;

		for (var i = 0; i < dayParts; i++)
		{
			ret[i] = sr_ut + day_span * i;
		}

		for (var i = 0; i <= dayParts; i++)
		{
			ret[i + dayParts] = ss_ut + night_span * i;
		}

		return ret;
	}

	public double[] getSunrisetEqualCuspsUt(int dayParts)
	{
		var ret = new double[dayParts * 2 + 1];

		var sr_ut      = baseUT                                  - hoursAfterSunrise() / 24.0;
		var sr_next_ut = sr_ut - sunrise                  / 24.0 + next_sunrise        / 24.0 + 1.0;
		var span       = (sr_next_ut - sr_ut) / (dayParts * 2);

		for (var i = 0; i <= dayParts * 2; i++)
		{
			ret[i] = sr_ut + span * i;
		}

		return ret;
	}

	public double[] getLmtCuspsUt(int dayParts)
	{
		var ret            = new double[dayParts * 2 + 1];
		var sr_lmt_ut      = baseUT                  - hoursAfterSunrise() / 24.0 - sunrise / 24.0 + 6.0 / 24.0;
		var sr_lmt_next_ut = sr_lmt_ut                                                             + 1.0;
		//double sr_lmt_ut = this.baseUT - this.info.tob.time / 24.0 + 6.0 / 24.0;
		//double sr_lmt_next_ut = sr_lmt_ut + 1.0;

		var lmt_offset = getLmtOffset(info, baseUT);
		sr_lmt_ut      += lmt_offset;
		sr_lmt_next_ut += lmt_offset;

		if (sr_lmt_ut > baseUT)
		{
			sr_lmt_ut--;
			sr_lmt_next_ut--;
		}


		var span = (sr_lmt_next_ut - sr_lmt_ut) / (dayParts * 2);

		for (var i = 0; i <= dayParts * 2; i++)
		{
			ret[i] = sr_lmt_ut + span * i;
		}

		return ret;
	}

	public Body.Name calculateKala()
	{
		var iBase = 0;
		return calculateKala(ref iBase);
	}

	public Body.Name calculateKala(ref int iBase)
	{
		int[] offsets_day =
		{
			0,
			6,
			1,
			3,
			2,
			4,
			5
		};
		var b          = Basics.weekdayRuler(wday);
		var bday_birth = isDayBirth();

		var cusps = getKalaCuspsUt();
		if (options.KalaType == HoroscopeOptions.EHoraType.Lmt)
		{
			b          = Basics.weekdayRuler(lmt_wday);
			bday_birth = info.tob.time > lmt_sunset || info.tob.time < lmt_sunrise;
		}

		var i = offsets_day[(int) b];
		iBase = i;
		var j = 0;

		if (bday_birth)
		{
			for (j = 0; j < 8; j++)
			{
				if (baseUT >= cusps[j] && baseUT < cusps[j + 1])
				{
					break;
				}
			}

			i += j;
			while (i >= 8)
			{
				i -= 8;
			}

			return kalaOrder[i];
		}

		//i+=4;
		for (j = 8; j < 16; j++)
		{
			if (baseUT >= cusps[j] && baseUT < cusps[j + 1])
			{
				break;
			}
		}

		i += j;
		while (i >= 8)
		{
			i -= 8;
		}

		return kalaOrder[i];
	}

	public Body.Name calculateHora()
	{
		var iBody = 0;
		return calculateHora(baseUT, ref iBody);
	}

	public Body.Name calculateHora(double _baseUT, ref int baseBody)
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
		var b     = Basics.weekdayRuler(wday);
		var cusps = getHoraCuspsUt();
		if (options.HoraType == HoroscopeOptions.EHoraType.Lmt)
		{
			b = Basics.weekdayRuler(lmt_wday);
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
			if (_baseUT >= cusps[j] && _baseUT < cusps[j + 1])
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

		return horaOrder[i];
	}

	private Body.Name calculateUpagrahasStart()
	{
		if (isDayBirth())
		{
			return Basics.weekdayRuler(wday);
		}

		switch (wday)
		{
			default:
			case Basics.Weekday.Sunday: return Body.Name.Jupiter;
			case Basics.Weekday.Monday:    return Body.Name.Venus;
			case Basics.Weekday.Tuesday:   return Body.Name.Saturn;
			case Basics.Weekday.Wednesday: return Body.Name.Sun;
			case Basics.Weekday.Thursday:  return Body.Name.Moon;
			case Basics.Weekday.Friday:    return Body.Name.Mars;
			case Basics.Weekday.Saturday:  return Body.Name.Mercury;
		}
	}

	private void calculateUpagrahasSingle(Body.Name b, double tjd)
	{
		var lon = new Longitude(0);
		lon.value = sweph.Lagna(tjd);
		var bp = new Position(this, b, Body.Type.Upagraha, lon, 0, 0, 0, 0, 0);
		positionList.Add(bp);
	}

	private void calculateMaandiHelper(Body.Name b, HoroscopeOptions.EMaandiType mty, double[] jds, double dOffset, int[] bodyOffsets)
	{
		switch (mty)
		{
			case HoroscopeOptions.EMaandiType.SaturnBegin:
				calculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Name.Saturn]]);
				break;
			case HoroscopeOptions.EMaandiType.SaturnMid:
				calculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Name.Saturn]] + dOffset);
				break;
			case HoroscopeOptions.EMaandiType.SaturnEnd:
			case HoroscopeOptions.EMaandiType.LordlessBegin:
				var _off1 = bodyOffsets[(int) Body.Name.Saturn]                      + 1;
				calculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Name.Saturn]] + dOffset * 2.0);
				break;
			case HoroscopeOptions.EMaandiType.LordlessMid:
				calculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Name.Saturn]] + dOffset * 3.0);
				break;
			case HoroscopeOptions.EMaandiType.LordlessEnd:
				calculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Name.Saturn]] + dOffset * 4.0);
				break;
		}
	}

	private void calculateUpagrahas()
	{
		double dStart = 0, dEnd = 0;

		var m         = info.tob;
		dStart = dEnd = sweph.JulDay(m.year, m.month, m.day, -info.tz.toDouble());
		var bStart    = calculateUpagrahasStart();

		if (isDayBirth())
		{
			dStart += sunrise / 24.0;
			dEnd   += sunset  / 24.0;
		}
		else
		{
			dStart += sunset / 24.0;
			dEnd   += 1.0 + sunrise / 24.0;
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
			var _ib = (int) bStart + i;
			while (_ib >= 8)
			{
				_ib -= 8;
			}

			bodyOffsets[_ib] = i;
		}

		double dUpagrahaOffset = 0;
		switch (options.UpagrahaType)
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

		sweph.obtainLock(this);
		calculateUpagrahasSingle(Body.Name.Kala, jds[bodyOffsets[(int) Body.Name.Sun]]);
		calculateUpagrahasSingle(Body.Name.Mrityu, jds[bodyOffsets[(int) Body.Name.Mars]]);
		calculateUpagrahasSingle(Body.Name.ArthaPraharaka, jds[bodyOffsets[(int) Body.Name.Mercury]]);
		calculateUpagrahasSingle(Body.Name.YamaGhantaka, jds[bodyOffsets[(int) Body.Name.Jupiter]]);


		calculateMaandiHelper(Body.Name.Maandi, options.MaandiType, jds, dOffset, bodyOffsets);
		calculateMaandiHelper(Body.Name.Gulika, options.GulikaType, jds, dOffset, bodyOffsets);
		sweph.releaseLock(this);
	}

	private void calculateSunsUpagrahas()
	{
		var slon = getPosition(Body.Name.Sun).longitude;

		var bpDhuma = new Position(this, Body.Name.Dhuma, Body.Type.Upagraha, slon.add(133.0 + 20.0 / 60.0), 0, 0, 0, 0, 0);

		var bpVyatipata = new Position(this, Body.Name.Vyatipata, Body.Type.Upagraha, new Longitude(360.0).sub(bpDhuma.longitude), 0, 0, 0, 0, 0);

		var bpParivesha = new Position(this, Body.Name.Parivesha, Body.Type.Upagraha, bpVyatipata.longitude.add(180), 0, 0, 0, 0, 0);

		var bpIndrachapa = new Position(this, Body.Name.Indrachapa, Body.Type.Upagraha, new Longitude(360.0).sub(bpParivesha.longitude), 0, 0, 0, 0, 0);

		var bpUpaketu = new Position(this, Body.Name.Upaketu, Body.Type.Upagraha, slon.sub(30), 0, 0, 0, 0, 0);

		positionList.Add(bpDhuma);
		positionList.Add(bpVyatipata);
		positionList.Add(bpParivesha);
		positionList.Add(bpIndrachapa);
		positionList.Add(bpUpaketu);
	}

	private void calculateWeekday()
	{
		var m  = info.tob;
		var jd = sweph.JulDay(m.year, m.month, m.day, 12.0);
		if (info.tob.time < sunrise)
		{
			jd -= 1;
		}

		wday = (Basics.Weekday) sweph.DayOfWeek(jd);

		jd = sweph.JulDay(m.year, m.month, m.day, 12.0);
		if (info.tob.time < lmt_sunrise)
		{
			jd -= 1;
		}

		lmt_wday = (Basics.Weekday) sweph.DayOfWeek(jd);
	}

	private void addChandraLagna(string desc, Longitude lon)
	{
		var bp = new Position(this, Body.Name.Other, Body.Type.ChandraLagna, lon, 0, 0, 0, 0, 0);
		bp.otherString = desc;
		positionList.Add(bp);
	}

	private void calculateChandraLagnas()
	{
		var bp_moon  = getPosition(Body.Name.Moon);
		var lon_base = new Longitude(bp_moon.extrapolateLongitude(new Division(Basics.DivisionType.Navamsa)).toZodiacHouseBase());
		lon_base = lon_base.add(bp_moon.longitude.toZodiacHouseOffset());

		//mhora.Log.Debug ("Starting Chandra Ayur Lagna from {0}", lon_base);

		var ista_ghati = Basics.normalize_exc(0.0, 24.0, info.tob.time - sunrise) * 2.5;
		var gl_lon     = lon_base.add(new Longitude(ista_ghati        * 30.0));
		var hl_lon     = lon_base.add(new Longitude(ista_ghati * 30.0 / 2.5));
		var bl_lon     = lon_base.add(new Longitude(ista_ghati * 30.0 / 5.0));

		var vl = ista_ghati * 5.0;
		while (ista_ghati > 12.0)
		{
			ista_ghati -= 12.0;
		}

		var vl_lon = lon_base.add(new Longitude(vl * 30.0));

		addChandraLagna("Chandra Lagna - GL", gl_lon);
		addChandraLagna("Chandra Lagna - HL", hl_lon);
		addChandraLagna("Chandra Ayur Lagna - BL", bl_lon);
		addChandraLagna("Chandra Lagna - ViL", vl_lon);
	}

	private void calculateSL()
	{
		var mpos  = getPosition(Body.Name.Moon).longitude;
		var lpos  = getPosition(Body.Name.Lagna).longitude;
		var sldeg = mpos.toNakshatraOffset() / (360.0 / 27.0) * 360.0;
		var slLon = lpos.add(sldeg);
		var bp    = new Position(this, Body.Name.SreeLagna, Body.Type.SpecialLagna, slLon, 0, 0, 0, 0, 0);
		positionList.Add(bp);
	}

	private void calculatePranapada()
	{
		var spos   = getPosition(Body.Name.Sun).longitude;
		var offset = info.tob.time - sunrise;
		if (offset < 0)
		{
			offset += 24.0;
		}

		offset *= 60.0 * 60.0 / 6.0;
		Longitude ppos = null;
		switch ((int) spos.toZodiacHouse().value % 3)
		{
			case 1:
				ppos = spos.add(offset);
				break;
			case 2:
				ppos = spos.add(offset + 8.0 * 30.0);
				break;
			default:
			case 0:
				ppos = spos.add(offset + 4.0 * 30.0);
				break;
		}

		var bp = new Position(this, Body.Name.Pranapada, Body.Type.SpecialLagna, ppos, 0, 0, 0, 0, 0);
		positionList.Add(bp);
	}

	private void addOtherPoints()
	{
		var lag_pos     = getPosition(Body.Name.Lagna).longitude;
		var sun_pos     = getPosition(Body.Name.Sun).longitude;
		var moon_pos    = getPosition(Body.Name.Moon).longitude;
		var mars_pos    = getPosition(Body.Name.Mars).longitude;
		var jup_pos     = getPosition(Body.Name.Jupiter).longitude;
		var ven_pos     = getPosition(Body.Name.Venus).longitude;
		var sat_pos     = getPosition(Body.Name.Saturn).longitude;
		var rah_pos     = getPosition(Body.Name.Rahu).longitude;
		var mandi_pos   = getPosition(Body.Name.Maandi).longitude;
		var gulika_pos  = getPosition(Body.Name.Gulika).longitude;
		var muhurta_pos = new Longitude(hoursAfterSunrise() / (next_sunrise + 24.0 - sunrise) * 360.0);

		// add simple midpoints
		addOtherPosition("User Specified", options.CustomBodyLongitude);
		addOtherPosition("Brighu Bindu", rah_pos.add(moon_pos.sub(rah_pos).value / 2.0));
		addOtherPosition("Muhurta Point", muhurta_pos);
		addOtherPosition("Ra-Ke m.p", rah_pos.add(90));
		addOtherPosition("Ke-Ra m.p", rah_pos.add(270));

		var l1pos  = getPosition(LordOfZodiacHouse(lag_pos.toZodiacHouse(), new Division(Basics.DivisionType.Rasi))).longitude;
		var l6pos  = getPosition(LordOfZodiacHouse(lag_pos.toZodiacHouse().add(6), new Division(Basics.DivisionType.Rasi))).longitude;
		var l8pos  = getPosition(LordOfZodiacHouse(lag_pos.toZodiacHouse().add(6), new Division(Basics.DivisionType.Rasi))).longitude;
		var l12pos = getPosition(LordOfZodiacHouse(lag_pos.toZodiacHouse().add(6), new Division(Basics.DivisionType.Rasi))).longitude;

		var mrit_sat_pos   = new Longitude(mandi_pos.value * 8.0 + sat_pos.value   * 8.0);
		var mrit_jup2_pos  = new Longitude(sat_pos.value   * 9.0 + mandi_pos.value * 18.0 + jup_pos.value  * 18.0);
		var mrit_sun2_pos  = new Longitude(sat_pos.value   * 9.0 + mandi_pos.value * 18.0 + sun_pos.value  * 18.0);
		var mrit_moon2_pos = new Longitude(sat_pos.value   * 9.0 + mandi_pos.value * 18.0 + moon_pos.value * 18.0);

		if (isDayBirth())
		{
			addOtherPosition("Niryana: Su-Sa sum", sun_pos.add(sat_pos), Body.Name.MrityuPoint);
		}
		else
		{
			addOtherPosition("Niryana: Mo-Ra sum", moon_pos.add(rah_pos), Body.Name.MrityuPoint);
		}

		addOtherPosition("Mrityu Sun: La-Mn sum", lag_pos.add(mandi_pos), Body.Name.MrityuPoint);
		addOtherPosition("Mrityu Moon: Mo-Mn sum", moon_pos.add(mandi_pos), Body.Name.MrityuPoint);
		addOtherPosition("Mrityu Lagna: La-Mo-Mn sum", lag_pos.add(moon_pos).add(mandi_pos), Body.Name.MrityuPoint);
		addOtherPosition("Mrityu Sat: Mn8-Sa8", mrit_sat_pos, Body.Name.MrityuPoint);
		addOtherPosition("6-8-12 sum", l6pos.add(l8pos).add(l12pos), Body.Name.MrityuPoint);
		addOtherPosition("Mrityu Jup: Sa9-Mn18-Ju18", mrit_jup2_pos, Body.Name.MrityuPoint);
		addOtherPosition("Mrityu Sun: Sa9-Mn18-Su18", mrit_sun2_pos, Body.Name.MrityuPoint);
		addOtherPosition("Mrityu Moon: Sa9-Mn18-Mo18", mrit_moon2_pos, Body.Name.MrityuPoint);

		addOtherPosition("Su-Mo sum", sun_pos.add(moon_pos));
		addOtherPosition("Ju-Mo-Ma sum", jup_pos.add(moon_pos).add(mars_pos));
		addOtherPosition("Su-Ve-Ju sum", sun_pos.add(ven_pos).add(jup_pos));
		addOtherPosition("Sa-Mo-Ma sum", sat_pos.add(moon_pos).add(mars_pos));
		addOtherPosition("La-Gu-Sa sum", lag_pos.add(gulika_pos).add(sat_pos));
		addOtherPosition("L-MLBase sum", l1pos.add(moon_pos.toZodiacHouseBase()));
	}

	public void populateHouseCusps()
	{
		swephHouseCusps = new Longitude[13];
		var dCusps = new double[13];
		var ascmc  = new double[10];

		sweph.obtainLock(this);
		sweph.HousesEx(baseUT, sweph.SEFLG_SIDEREAL, info.lat.toDouble(), info.lon.toDouble(), swephHouseSystem, dCusps, ascmc);
		sweph.releaseLock(this);
		for (var i = 0; i < 12; i++)
		{
			swephHouseCusps[i] = new Longitude(dCusps[i + 1]);
		}

		if (options.BhavaType == HoroscopeOptions.EBhavaType.Middle)
		{
			var middle = new Longitude((dCusps[1] + dCusps[2]) / 2.0);
			var offset = middle.sub(swephHouseCusps[0]).value;
			for (var i = 0; i < 12; i++)
			{
				swephHouseCusps[i] = swephHouseCusps[i].sub(offset);
			}
		}


		swephHouseCusps[12] = swephHouseCusps[0];
	}

	private void populateCache()
	{
		// The stuff here is largely order sensitive
		// Try to add new definitions to the end

		baseUT = sweph.JulDay(info.tob.year, info.tob.month, info.tob.day, info.tob.time - info.tz.toDouble());


		sweph.obtainLock(this);
		sweph.SetPath(MhoraGlobalOptions.Instance.HOptions.EphemerisPath);
		// Find LMT offset
		populateLmt();
		// Sunrise (depends on lmt)
		populateSunrisetCache();
		// Basic grahas + Special lagnas (depend on sunrise)
		positionList = Basics.CalculateBodyPositions(this, sunrise);
		sweph.releaseLock(this);
		// Srilagna etc
		calculateSL();
		calculatePranapada();
		// Sun based Upagrahas (depends on sun)
		calculateSunsUpagrahas();
		// Upagrahas (depends on sunrise)
		calculateUpagrahas();
		// Weekday (depends on sunrise)
		calculateWeekday();
		// Sahamas
		calculateSahamas();
		// Prana sphuta etc. (depends on upagrahas)
		getPrashnaMargaPositions();
		calculateChandraLagnas();
		addOtherPoints();
		// Add extrapolated special lagnas (depends on sunrise)
		addSpecialLagnaPositions();
		// Hora (depends on weekday)
		calculateHora();
		// Populate house cusps on options refresh
		populateHouseCusps();
	}

	public double lengthOfDay()
	{
		return next_sunrise + 24.0 - sunrise;
	}

	public double hoursAfterSunrise()
	{
		var ret = info.tob.time - sunrise;
		if (ret < 0)
		{
			ret += 24.0;
		}

		return ret;
	}

	public double hoursAfterSunRiseSet()
	{
		double ret = 0;
		if (isDayBirth())
		{
			ret = info.tob.time - sunrise;
		}
		else
		{
			ret = info.tob.time - sunset;
		}

		if (ret < 0)
		{
			ret += 24.0;
		}

		return ret;
	}

	public bool isDayBirth()
	{
		if (info.tob.time >= sunrise && info.tob.time < sunset)
		{
			return true;
		}

		return false;
	}

	public void addOtherPosition(string desc, Longitude lon, Body.Name name)
	{
		var bp = new Position(this, name, Body.Type.Other, lon, 0, 0, 0, 0, 0);
		bp.otherString = desc;
		positionList.Add(bp);
	}

	public void addOtherPosition(string desc, Longitude lon)
	{
		addOtherPosition(desc, lon, Body.Name.Other);
	}

	public void addSpecialLagnaPositions()
	{
		var diff = info.tob.time - sunrise;
		if (diff < 0)
		{
			diff += 24.0;
		}

		sweph.obtainLock(this);
		for (var i = 1; i <= 12; i++)
		{
			var specialDiff = diff * (i - 1);
			var tjd         = baseUT + specialDiff / 24.0;
			var asc         = sweph.Lagna(tjd);
			var desc        = string.Format("Special Lagna ({0:00})", i);
			addOtherPosition(desc, new Longitude(asc));
		}

		sweph.releaseLock(this);
	}

	public void getPrashnaMargaPositions()
	{
		var sunLon    = getPosition(Body.Name.Sun).longitude;
		var moonLon   = getPosition(Body.Name.Moon).longitude;
		var lagnaLon  = getPosition(Body.Name.Lagna).longitude;
		var gulikaLon = getPosition(Body.Name.Gulika).longitude;
		var rahuLon   = getPosition(Body.Name.Rahu).longitude;

		var trisLon    = lagnaLon.add(moonLon).add(gulikaLon);
		var chatusLon  = trisLon.add(sunLon);
		var panchasLon = chatusLon.add(rahuLon);
		var pranaLon   = new Longitude(lagnaLon.value  * 5.0).add(gulikaLon);
		var dehaLon    = new Longitude(moonLon.value   * 8.0).add(gulikaLon);
		var mrityuLon  = new Longitude(gulikaLon.value * 7.0).add(sunLon);

		addOtherPosition("Trih Sphuta", trisLon);
		addOtherPosition("Chatuh Sphuta", chatusLon);
		addOtherPosition("Panchah Sphuta", panchasLon);
		addOtherPosition("Pranah Sphuta", pranaLon);
		addOtherPosition("Deha Sphuta", dehaLon);
		addOtherPosition("Mrityu Sphuta", mrityuLon);
	}

	public Position getPosition(Body.Name b)
	{
		var index = Body.toInt(b);
		var t     = positionList[index].GetType();
		var s     = t.ToString();
		Trace.Assert(index >= 0 && index < positionList.Count, "Horoscope::getPosition 1");
		Trace.Assert(positionList[index].GetType() == typeof(Position), "Horoscope::getPosition 2");
		var bp = (Position) positionList[Body.toInt(b)];
		if (bp.name == b)
		{
			return bp;
		}

		for (var i = (int) Body.Name.Lagna + 1; i < positionList.Count; i++)
		{
			if (b == ((Position) positionList[i]).name)
			{
				return (Position) positionList[i];
			}
		}

		Trace.Assert(false, "Basics::GetPosition. Unable to find body");
		return (Position) positionList[0];
	}

	private Position sahamaHelper(string sahama, Body.Name b, Body.Name a, Body.Name c)
	{
		Longitude lonA, lonB, lonC;
		lonA = getPosition(a).longitude;
		lonB = getPosition(b).longitude;
		lonC = getPosition(c).longitude;
		return sahamaHelper(sahama, lonB, lonA, lonC);
	}

	private Position sahamaHelper(string sahama, Body.Name b, Body.Name a, Longitude lonC)
	{
		Longitude lonA, lonB;
		lonA = getPosition(a).longitude;
		lonB = getPosition(b).longitude;
		return sahamaHelper(sahama, lonB, lonA, lonC);
	}

	private Position sahamaHelper(string sahama, Longitude lonB, Body.Name a, Body.Name c)
	{
		Longitude lonA, lonC;
		lonA = getPosition(a).longitude;
		lonC = getPosition(c).longitude;
		return sahamaHelper(sahama, lonB, lonA, lonC);
	}

	private Position sahamaHelper(string sahama, Longitude lonB, Longitude lonA, Longitude lonC)
	{
		// b-a+c
		var bDay = isDayBirth();

		Longitude lonR;
		lonR = lonB.sub(lonA).add(lonC);
		if (lonB.sub(lonA).value <= lonC.sub(lonA).value)
		{
			lonR = lonR.add(new Longitude(30.0));
		}

		var bp = new Position(this, Body.Name.Other, Body.Type.Sahama, lonR, 0.0, 0.0, 0.0, 0.0, 0.0);
		bp.otherString = sahama;
		return bp;
	}

	private Position sahamaDNHelper(string sahama, Longitude lonB, Longitude lonA, Longitude lonC)
	{
		// b-a+c
		var       bDay = isDayBirth();
		Longitude lonR;
		if (bDay)
		{
			lonR = lonB.sub(lonA).add(lonC);
		}
		else
		{
			lonR = lonA.sub(lonB).add(lonC);
		}

		if (lonB.sub(lonA).value <= lonC.sub(lonA).value)
		{
			lonR = lonR.add(new Longitude(30.0));
		}

		var bp = new Position(this, Body.Name.Other, Body.Type.Sahama, lonR, 0.0, 0.0, 0.0, 0.0, 0.0);
		bp.otherString = sahama;
		return bp;
	}

	private Position sahamaDNHelper(string sahama, Body.Name b, Longitude lonA, Body.Name c)
	{
		Longitude lonB, lonC;
		lonB = getPosition(b).longitude;
		lonC = getPosition(c).longitude;
		return sahamaDNHelper(sahama, lonB, lonA, lonC);
	}

	private Position sahamaDNHelper(string sahama, Longitude lonB, Body.Name a, Body.Name c)
	{
		Longitude lonA, lonC;
		lonA = getPosition(a).longitude;
		lonC = getPosition(c).longitude;
		return sahamaDNHelper(sahama, lonB, lonA, lonC);
	}

	private Position sahamaDNHelper(string sahama, Longitude lonB, Longitude lonA, Body.Name c)
	{
		Longitude lonC;
		lonC = getPosition(c).longitude;
		return sahamaDNHelper(sahama, lonB, lonA, lonC);
	}

	private Position sahamaDNHelper(string sahama, Body.Name b, Body.Name a, Body.Name c)
	{
		Longitude lonA, lonB, lonC;
		lonA = getPosition(a).longitude;
		lonB = getPosition(b).longitude;
		lonC = getPosition(c).longitude;
		return sahamaDNHelper(sahama, lonB, lonA, lonC);
	}

	private Position sahamaHelperNormalize(Position b, Body.Name lower, Body.Name higher)
	{
		var lonA = getPosition(lower).longitude;
		var lonB = getPosition(higher).longitude;
		if (b.longitude.sub(lonA).value < lonB.sub(lonA).value)
		{
			return b;
		}

		b.longitude = b.longitude.add(new Longitude(30));
		return b;
	}

	public ArrayList calculateSahamas()
	{
		var bDay      = isDayBirth();
		var al        = new ArrayList();
		var lon_lagna = getPosition(Body.Name.Lagna).longitude;
		var lon_base  = new Longitude(lon_lagna.toZodiacHouseBase());
		var zh_lagna  = lon_lagna.toZodiacHouse();
		var zh_moon   = getPosition(Body.Name.Moon).longitude.toZodiacHouse();
		var zh_sun    = getPosition(Body.Name.Sun).longitude.toZodiacHouse();


		// Fixed positions. Relied on by other sahams
		al.Add(sahamaDNHelper("Punya", Body.Name.Moon, Body.Name.Sun, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Vidya", Body.Name.Sun, Body.Name.Moon, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Sastra", Body.Name.Jupiter, Body.Name.Saturn, Body.Name.Mercury));

		// Variable positions.
		al.Add(sahamaDNHelper("Yasas", Body.Name.Jupiter, ((Position) al[0]).longitude, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Mitra", Body.Name.Jupiter, ((Position) al[0]).longitude, Body.Name.Venus));
		al.Add(sahamaDNHelper("Mahatmya", ((Position) al[0]).longitude, Body.Name.Mars, Body.Name.Lagna));

		var bLagnaLord = LordOfZodiacHouse(zh_lagna, new Division(Basics.DivisionType.Rasi));
		if (bLagnaLord != Body.Name.Mars)
		{
			al.Add(sahamaDNHelper("Samartha", Body.Name.Mars, bLagnaLord, Body.Name.Lagna));
		}
		else
		{
			al.Add(sahamaDNHelper("Samartha", Body.Name.Jupiter, Body.Name.Mars, Body.Name.Lagna));
		}

		al.Add(sahamaHelper("Bhratri", Body.Name.Jupiter, Body.Name.Saturn, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Gaurava", Body.Name.Jupiter, Body.Name.Moon, Body.Name.Sun));
		al.Add(sahamaDNHelper("Pitri", Body.Name.Saturn, Body.Name.Sun, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Rajya", Body.Name.Saturn, Body.Name.Sun, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Matri", Body.Name.Moon, Body.Name.Venus, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Putra", Body.Name.Jupiter, Body.Name.Moon, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Jeeva", Body.Name.Saturn, Body.Name.Jupiter, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Karma", Body.Name.Mars, Body.Name.Mercury, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Roga", Body.Name.Lagna, Body.Name.Moon, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Kali", Body.Name.Jupiter, Body.Name.Mars, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Bandhu", Body.Name.Mercury, Body.Name.Moon, Body.Name.Lagna));
		al.Add(sahamaHelper("Mrityu", lon_base.add(8.0   * 30.0), Body.Name.Moon, Body.Name.Lagna));
		al.Add(sahamaHelper("Paradesa", lon_base.add(9.0 * 30.0), LordOfZodiacHouse(zh_lagna.add(9), new Division(Basics.DivisionType.Rasi)), Body.Name.Lagna));
		al.Add(sahamaHelper("Artha", lon_base.add(2.0    * 30.0), LordOfZodiacHouse(zh_lagna.add(2), new Division(Basics.DivisionType.Rasi)), Body.Name.Lagna));
		al.Add(sahamaDNHelper("Paradara", Body.Name.Venus, Body.Name.Sun, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Vanik", Body.Name.Moon, Body.Name.Mercury, Body.Name.Lagna));

		if (bDay)
		{
			al.Add(sahamaHelper("Karyasiddhi", Body.Name.Saturn, Body.Name.Sun, LordOfZodiacHouse(zh_sun, new Division(Basics.DivisionType.Rasi))));
		}
		else
		{
			al.Add(sahamaHelper("Karyasiddhi", Body.Name.Saturn, Body.Name.Moon, LordOfZodiacHouse(zh_moon, new Division(Basics.DivisionType.Rasi))));
		}

		al.Add(sahamaDNHelper("Vivaha", Body.Name.Venus, Body.Name.Saturn, Body.Name.Lagna));
		al.Add(sahamaHelper("Santapa", Body.Name.Saturn, Body.Name.Moon, lon_base.add(6.0 * 30.0)));
		al.Add(sahamaDNHelper("Sraddha", Body.Name.Venus, Body.Name.Mars, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Preeti", ((Position) al[2]).longitude, ((Position) al[0]).longitude, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Jadya", Body.Name.Mars, Body.Name.Saturn, Body.Name.Mercury));
		al.Add(sahamaHelper("Vyapara", Body.Name.Mars, Body.Name.Saturn, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Satru", Body.Name.Mars, Body.Name.Saturn, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Jalapatana", new Longitude(105), Body.Name.Saturn, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Bandhana", ((Position) al[0]).longitude, Body.Name.Saturn, Body.Name.Lagna));
		al.Add(sahamaDNHelper("Apamrityu", lon_base.add(8.0 * 30.0), Body.Name.Mars, Body.Name.Lagna));
		al.Add(sahamaHelper("Labha", lon_base.add(11.0      * 30.0), LordOfZodiacHouse(zh_lagna.add(11), new Division(Basics.DivisionType.Rasi)), Body.Name.Lagna));

		positionList.AddRange(al);
		return al;
	}
}