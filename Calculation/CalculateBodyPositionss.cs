using System;
using System.Collections.Generic;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Calculation
{
	internal static class CalculateBodyPositionss
	{
		public static DivisionPosition CalculateDivisionPosition(this Position bp, DivisionType d)
		{
			return bp.ToDivisionPosition(d);
		}

		public static List<DivisionPosition> CalculateDivisionPositions(this List<Position> positionList, DivisionType d)
		{
			var al = new List<DivisionPosition>();
			foreach (Position bp in positionList)
			{
				if (bp.BodyType != BodyType.Other)
				{
					al.Add(CalculateDivisionPosition(bp, d));
				}
			}

			return al;
		}

		private static DivisionPosition CalculateGrahaArudhaDivisionPosition(this Horoscope h, Body bn, ZodiacHouse zh, DivisionType dtype)
		{
			var dp    = h.GetPosition(bn).ToDivisionPosition(dtype);
			var dpl   = h.GetPosition(Body.Lagna).ToDivisionPosition(dtype);
			var rel   = dp.ZodiacHouse.NumHousesBetween(zh);
			var hse   = dpl.ZodiacHouse.NumHousesBetween(zh);
			var zhsum = zh.Add(rel);
			var rel2  = dp.ZodiacHouse.NumHousesBetween(zhsum);
			if (rel2 == 1 || rel2 == 7)
			{
				zhsum = zhsum.Add(10);
			}

			var dp2 = new DivisionPosition(Body.Other, BodyType.GrahaArudha, zhsum, 0, 0, 0)
			{
				//dp2.Longitude   = zhsum.DivisionalLongitude(dp.Longitude, dpl.part);
				Description = string.Format("{0}{1}", bn.ToShortString(), hse)
			};
			return dp2;
		}

		public static List <DivisionPosition> CalculateGrahaArudhaDivisionPositions(this Horoscope h, DivisionType dtype)
		{
			object[][] parameters =
			{
				new object[]
				{
					ZodiacHouse.Ari,
					Body.Mars
				},
				new object[]
				{
					ZodiacHouse.Tau,
					Body.Venus
				},
				new object[]
				{
					ZodiacHouse.Gem,
					Body.Mercury
				},
				new object[]
				{
					ZodiacHouse.Can,
					Body.Moon
				},
				new object[]
				{
					ZodiacHouse.Leo,
					Body.Sun
				},
				new object[]
				{
					ZodiacHouse.Vir,
					Body.Mercury
				},
				new object[]
				{
					ZodiacHouse.Lib,
					Body.Venus
				},
				new object[]
				{
					ZodiacHouse.Sco,
					Body.Mars
				},
				new object[]
				{
					ZodiacHouse.Sag,
					Body.Jupiter
				},
				new object[]
				{
					ZodiacHouse.Cap,
					Body.Saturn
				},
				new object[]
				{
					ZodiacHouse.Aqu,
					Body.Saturn
				},
				new object[]
				{
					ZodiacHouse.Pis,
					Body.Jupiter
				},
				new object[]
				{
					ZodiacHouse.Sco,
					Body.Ketu
				},
				new object[]
				{
					ZodiacHouse.Aqu,
					Body.Rahu
				}
			};
			var al = new List <DivisionPosition> ();

			for (var i = 0; i < parameters.Length; i++)
			{
				al.Add(h.CalculateGrahaArudhaDivisionPosition((Body) parameters[i][1], ((ZodiacHouse) parameters[i][0]), dtype));
			}

			return al;
		}

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

		public static List <DivisionPosition> CalculateVarnadaDivisionPositions(this Horoscope h, DivisionType dtype)
		{
			var al   = new List <DivisionPosition> ();
			var zhL  = h.GetPosition(Body.Lagna).ToDivisionPosition(dtype).ZodiacHouse;
			var zhHl = h.GetPosition(Body.HoraLagna).ToDivisionPosition(dtype).ZodiacHouse;

			var zhAri = ZodiacHouse.Ari;
			var zhPis = ZodiacHouse.Pis;
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

				ZodiacHouse zhV;
				if (zhL.IsOdd())
				{
					zhV = zhAri.Add(sum);
				}
				else
				{
					zhV = zhPis.AddReverse(sum);
				}

				var divPos = new DivisionPosition(Body.Other, BodyType.Varnada, zhV, 0, 0, 0)
				{
					Description = VarnadaStrs[i - 1]
				};

				al.Add(divPos);
			}

			return al;
		}

		private static DivisionPosition CalculateArudhaDivisionPosition(this Horoscope h, ZodiacHouse zh, Body bn, Body aname, DivisionType d, BodyType btype)
		{
			var bp    = h.GetPosition(bn);
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

		public static List <DivisionPosition> CalculateArudhaDivisionPositions(this Horoscope h, DivisionType varga)
		{
			var grahas = h.FindGrahas(varga);

			Body[] bnlist =
			{
				Body.Other,
				Body.AL,
				Body.A2,
				Body.A3,
				Body.A4,
				Body.A5,
				Body.A6,
				Body.A7,
				Body.A8,
				Body.A9,
				Body.A10,
				Body.A11,
				Body.UL
			};

			var              fsColord      = h.RulesStrongerCoLord();
			var              arudhaDivList = new List <DivisionPosition> ();
			DivisionPosition first, second;
			for (var j = 1; j <= 12; j++)
			{
				var           zlagna     = CalculateDivisionPosition(h.GetPosition(Body.Lagna), varga).ZodiacHouse;
				var           zh         = zlagna.Add(j);
				var bnWeaker   = Body.Other;
				var           bnStronger = zh.SimpleLordOfZodiacHouse();
				if (zh == ZodiacHouse.Aqu)
				{
					bnStronger = grahas.Stronger(Body.Rahu, Body.Saturn, true, fsColord, out _);
					bnWeaker   = grahas.Weaker(Body.Rahu, Body.Saturn, true, fsColord, out _);
				}
				else if (zh == ZodiacHouse.Sco)
				{
					bnStronger = grahas.Stronger(Body.Ketu, Body.Mars, true, fsColord, out _);
					bnWeaker   = grahas.Weaker(Body.Ketu, Body.Mars, true, fsColord, out _);
				}

				first = h.CalculateArudhaDivisionPosition(zh, bnStronger, bnlist[j], varga, BodyType.BhavaArudha);
				arudhaDivList.Add(first);
				if (zh == ZodiacHouse.Aqu || zh == ZodiacHouse.Sco)
				{
					second = h.CalculateArudhaDivisionPosition(zh, bnWeaker, bnlist[j], varga, BodyType.BhavaArudhaSecondary);
					if (first.ZodiacHouse != second.ZodiacHouse)
					{
						arudhaDivList.Add(second);
					}
				}
			}

			return arudhaDivList;
		}



		public static Body CalculateKala(this Horoscope h, ref int iBase)
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
			var b          = h.Wday.WeekdayRuler();
			var bdayBirth = h.IsDayBirth();

			var cusps = h.GetKalaCuspsUt();
			if (h.Options.KalaType == HoroscopeOptions.EHoraType.Lmt)
			{
				b          = h.LmtWday.WeekdayRuler();
				bdayBirth = h.Info.DateOfBirth.Time().TotalHours > h.LmtSunset || h.Info.DateOfBirth.Time().TotalHours < h.LmtSunrise;
			}

			var i = offsetsDay[(int) b];
			iBase = i;
			var j = 0;

			if (bdayBirth)
			{
				for (j = 0; j < 8; j++)
				{
					if (h.Info.Jd >= cusps[j] && h.Info.Jd < cusps[j + 1])
					{
						break;
					}
				}

				i += j;
				while (i >= 8)
				{
					i -= 8;
				}

				return h.KalaOrder[i];
			}

			//i+=4;
			for (j = 8; j < 16; j++)
			{
				if (h.Info.Jd >= cusps[j] && h.Info.Jd < cusps[j + 1])
				{
					break;
				}
			}

			i += j;
			while (i >= 8)
			{
				i -= 8;
			}

			return h.KalaOrder[i];
		}

		public static Body CalculateHora(this Horoscope h)
		{
			var iBody = 0;
			return h.CalculateHora(h.Info.Jd, ref iBody);
		}

		public static Body CalculateHora(this Horoscope h, double baseUt, ref int baseBody)
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
			var b     = h.Wday.WeekdayRuler();
			var cusps = h.GetHoraCuspsUt();
			if (h.Options.HoraType == HoroscopeOptions.EHoraType.Lmt)
			{
				b = h.LmtWday.WeekdayRuler();
			}

			var i = offsets[(int) b];
			baseBody = i;
			var j = 0;
			//for (j=0; j<23; j++)
			//{
			//	Moment m1 = new Moment(cusps[j], this);
			//	Moment m2 = new Moment(cusps[j+1], this);
			//	Mhora.Log.Debug ("Seeing if dob is between {0} and {1}", m1, m2);
			//}
			for (j = 0; j < 23; j++)
			{
				if (baseUt >= cusps[j] && baseUt < cusps[j + 1])
				{
					break;
				}
			}

			//Mhora.Log.Debug ("Found hora in the {0}th hora", j);
			i += j;
			while (i >= 7)
			{
				i -= 7;
			}

			return h.HoraOrder[i];
		}

		private static Body CalculateUpagrahasStart(this Horoscope h)
		{
			if (h.IsDayBirth())
			{
				return h.Wday.WeekdayRuler();
			}

			switch (h.Wday)
			{
				default:
				case Weekday.Sunday: return Body.Jupiter;
				case Weekday.Monday:    return Body.Venus;
				case Weekday.Tuesday:   return Body.Saturn;
				case Weekday.Wednesday: return Body.Sun;
				case Weekday.Thursday:  return Body.Moon;
				case Weekday.Friday:    return Body.Mars;
				case Weekday.Saturday:  return Body.Mercury;
			}
		}

		private static Position CalculateUpagrahasSingle(this Horoscope h, Body b, double tjd)
		{
			var lon = new Longitude(0.0)
			{
				Value = h.Lagna(tjd)
			};
			return new Position(h, b, BodyType.Upagraha, lon, 0, 0, 0, 0, 0);
		}

		private static Position CalculateMaandiHelper(this Horoscope h, Body b, HoroscopeOptions.EMaandiType mty, double[] jds, double dOffset, int[] bodyOffsets)
		{
			switch (mty)
			{
				case HoroscopeOptions.EMaandiType.SaturnBegin:
					return (h.CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Saturn]]));
				case HoroscopeOptions.EMaandiType.SaturnMid:
					return (h.CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Saturn]] + dOffset));
				case HoroscopeOptions.EMaandiType.SaturnEnd:
				case HoroscopeOptions.EMaandiType.LordlessBegin:
					var off1 = bodyOffsets[(int) Body.Saturn]                      + 1;
					return (h.CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Saturn]] + dOffset * 2.0));
				case HoroscopeOptions.EMaandiType.LordlessMid:
					return (h.CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Saturn]] + dOffset * 3.0));
				case HoroscopeOptions.EMaandiType.LordlessEnd:
					return (h.CalculateUpagrahasSingle(b, jds[bodyOffsets[(int) Body.Saturn]] + dOffset * 4.0));
			}

			return (null);
		}

		public static List<Position> CalculateUpagrahas(this Horoscope h)
		{
			var positionList = new List<Position>();
			double dStart = 0, dEnd = 0;

			var m         = h.Info.DateOfBirth;
			dStart = dEnd = sweph.JulDay(m.Year, m.Month, m.Day, -h.Info.DstOffset.TotalHours);
			var bStart    = h.CalculateUpagrahasStart();

			if (h.IsDayBirth())
			{
				dStart += h.Sunrise / 24.0;
				dEnd   += h.Sunset  / 24.0;
			}
			else
			{
				dStart += h.Sunset / 24.0;
				dEnd   += 1.0 + h.Sunrise / 24.0;
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
			switch (h.Options.UpagrahaType)
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

			positionList.Add(h.CalculateUpagrahasSingle(Body.Kala, jds[bodyOffsets[(int) Body.Sun]]));
			positionList.Add(h.CalculateUpagrahasSingle(Body.Mrityu, jds[bodyOffsets[(int) Body.Mars]]));
			positionList.Add(h.CalculateUpagrahasSingle(Body.ArthaPraharaka, jds[bodyOffsets[(int) Body.Mercury]]));
			positionList.Add(h.CalculateUpagrahasSingle(Body.YamaGhantaka, jds[bodyOffsets[(int) Body.Jupiter]]));


			positionList.Add(h.CalculateMaandiHelper(Body.Maandi, h.Options.MaandiType, jds, dOffset, bodyOffsets));
			positionList.Add(h.CalculateMaandiHelper(Body.Gulika, h.Options.GulikaType, jds, dOffset, bodyOffsets));

			return (positionList);
		}

		public static List<Position> CalculateSunsUpagrahas(this Horoscope h)
		{
			var positionList = new List<Position>();
			var slon = h.GetPosition(Body.Sun).Longitude;

			var bpDhuma = new Position(h, Body.Dhuma, BodyType.Upagraha, slon.Add(133.0 + 20.0 / 60.0), 0, 0, 0, 0, 0);

			var bpVyatipata = new Position(h, Body.Vyatipata, BodyType.Upagraha, new Longitude(360.0).Sub(bpDhuma.Longitude), 0, 0, 0, 0, 0);

			var bpParivesha = new Position(h, Body.Parivesha, BodyType.Upagraha, bpVyatipata.Longitude.Add(180.0), 0, 0, 0, 0, 0);

			var bpIndrachapa = new Position(h, Body.Indrachapa, BodyType.Upagraha, new Longitude(360.0).Sub(bpParivesha.Longitude), 0, 0, 0, 0, 0);

			var bpUpaketu = new Position(h, Body.Upaketu, BodyType.Upagraha, slon.Sub(30.0), 0, 0, 0, 0, 0);

			positionList.Add(bpDhuma);
			positionList.Add(bpVyatipata);
			positionList.Add(bpParivesha);
			positionList.Add(bpIndrachapa);
			positionList.Add(bpUpaketu);

			return (positionList);
		}

		public static Body CalculateKala(this Horoscope h)
		{
			var iBase = 0;
			return h.CalculateKala(ref iBase);
		}


		public static List<Position> CalculateChandraLagnas(this Horoscope h)
		{
			var positionList = new List<Position>(); 
			var bpMoon       = h.GetPosition(Body.Moon);
			var lonBase      = new Longitude(bpMoon.ExtrapolateLongitude(DivisionType.Navamsa).ToZodiacHouseBase());
			lonBase = lonBase.Add(bpMoon.Longitude.ToZodiacHouseOffset());

			//Mhora.Log.Debug ("Starting Chandra Ayur Lagna from {0}", lon_base);

			var istaGhati = (h.Info.DateOfBirth.Time().TotalHours - h.Sunrise).NormalizeExc(0.0, 24.0) * 2.5;
			var glLon     = lonBase.Add(new Longitude(istaGhati        * 30.0));
			var hlLon     = lonBase.Add(new Longitude(istaGhati * 30.0 / 2.5));
			var blLon     = lonBase.Add(new Longitude(istaGhati * 30.0 / 5.0));

			var vl = istaGhati * 5.0;
			while (istaGhati > 12.0)
			{
				istaGhati -= 12.0;
			}

			var vlLon = lonBase.Add(new Longitude(vl * 30.0));

			positionList.Add(h.AddChandraLagna("Chandra Lagna - GL", glLon));
			positionList.Add(h.AddChandraLagna("Chandra Lagna - HL", hlLon));
			positionList.Add(h.AddChandraLagna("Chandra Ayur Lagna - BL", blLon));
			positionList.Add(h.AddChandraLagna("Chandra Lagna - ViL", vlLon));

			return (positionList);
		}

		public static Position CalculateSl(this Horoscope h)
		{
			var mpos  = h.GetPosition(Body.Moon).Longitude;
			var lpos  = h.GetPosition(Body.Lagna).Longitude;
			var sldeg = mpos.NakshatraOffset() / (360.0 / 27.0) * 360.0;
			var slLon = lpos.Add(sldeg);
			var bp    = new Position(h, Body.SreeLagna, BodyType.SpecialLagna, slLon, 0, 0, 0, 0, 0);
			return (bp);
		}

		public static Position CalculatePranapada(this Horoscope h)
		{
			var spos   = h.GetPosition(Body.Sun).Longitude;
			var offset = h.Info.DateOfBirth.Time().TotalHours - h.Sunrise;
			if (offset < 0)
			{
				offset += 24.0;
			}

			offset *= 60.0 * 60.0 / 6.0;
			Longitude ppos = null;
			switch ((int) spos.ToZodiacHouse() % 3)
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

			var bp = new Position(h, Body.Pranapada, BodyType.SpecialLagna, ppos, 0, 0, 0, 0, 0);
			return (bp);
		}

		private static Position AddChandraLagna(this Horoscope h, string desc, Longitude lon)
		{
			var bp = new Position(h, Body.Other, BodyType.ChandraLagna, lon, 0, 0, 0, 0, 0)
			{
				OtherString = desc
			};
			return (bp);
		}
	}
}
