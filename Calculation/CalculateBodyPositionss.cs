using System;
using System.Collections.Generic;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Calculation
{
	internal static class CalculateBodyPositionss
	{
		public static List<DivisionPosition> CalculateDivisionPositions(this List<Position> positionList, DivisionType d)
		{
			var al = new List<DivisionPosition>();
			foreach (Position bp in positionList)
			{
				if (bp.BodyType != BodyType.Other)
				{
					al.Add(bp.ToDivisionPosition(d));
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
			var al = new List <DivisionPosition> ();

			foreach (var lordship in Arudha.Lord)
			{
				al.Add(h.CalculateGrahaArudhaDivisionPosition(lordship.Body, lordship.ZodiacHouse, dtype));
			}

			return al;
		}

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
					Description = Varnada.Name[i - 1]
				};

				al.Add(divPos);
			}

			return al;
		}

		private static DivisionPosition CalculateArudhaDivisionPosition(this Horoscope h, ZodiacHouse zh, Body bn, Body aname, DivisionType d, BodyType btype)
		{
			var bp    = h.GetPosition(bn);
			var zhb   = bp.ToDivisionPosition(d).ZodiacHouse;
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

			var              fsColord      = h.RulesStrongerCoLord();
			var              arudhaDivList = new List <DivisionPosition> ();
			DivisionPosition first, second;
			for (var j = 1; j <= 12; j++)
			{
				var zlagna     = h.GetPosition(Body.Lagna).ToDivisionPosition(varga).ZodiacHouse;
				var zh         = zlagna.Add(j);
				var bnWeaker   = Body.Other;
				var bnStronger = zh.SimpleLordOfZodiacHouse();
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

				first = h.CalculateArudhaDivisionPosition(zh, bnStronger, Arudha.Position[j], varga, BodyType.BhavaArudha);
				arudhaDivList.Add(first);
				if (zh == ZodiacHouse.Aqu || zh == ZodiacHouse.Sco)
				{
					second = h.CalculateArudhaDivisionPosition(zh, bnWeaker, Arudha.Position[j], varga, BodyType.BhavaArudhaSecondary);
					if (first.ZodiacHouse != second.ZodiacHouse)
					{
						arudhaDivList.Add(second);
					}
				}
			}

			return arudhaDivList;
		}

		public static Body CalculateKala(this Horoscope h, out int iBase)
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
			var b          = h.Wday.Ruler();
			var bdayBirth = h.Vara.IsDayBirth;

			var cusps = h.GetKalaCuspsUt();
			if (h.Options.KalaType == HoroscopeOptions.EHoraType.Lmt)
			{
				b          = h.Info.DateOfBirth.Lstm(h).DayLord();
				bdayBirth = h.Info.DateOfBirth > h.Vara.Sunset.Date.Lstm(h) || h.Info.DateOfBirth < h.Vara.Sunset.Date.Lstm(h);
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

				return Bodies.KalaOrder[i];
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

			return Bodies.KalaOrder[i];
		}

		public static Body CalculateHora(this Horoscope h, JulianDate baseUt, out int baseBody)
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
			var b     = h.Wday.Ruler();
			var cusps = h.GetHoraCuspsUt();
			if (h.Options.HoraType == HoroscopeOptions.EHoraType.Lmt)
			{
				b = h.Info.DateOfBirth.Lstm(h).DayLord();
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

			return Bodies.HoraOrder[i];
		}

		private static Position CalculateUpagrahasSingle(this Horoscope h, Body b, JulianDate tjd)
		{
			var lon = new Longitude(0.0)
			{
				Value = h.Lagna(tjd)
			};
			return new Position(h, b, BodyType.Upagraha, lon, 0, 0, 0, 0, 0);
		}

		private static Position CalculateMaandiHelper(this Horoscope h, Body b, HoroscopeOptions.EMaandiType mty, JulianDate[] jds, double dOffset, int[] bodyOffsets)
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
			JulianDate dStart , dEnd;

			var m         = h.Info.DateOfBirth;
			var bStart    = h.UpagrahasStart();

			if (h.Vara.IsDayBirth)
			{
				dStart = h.Vara.Sunrise;
				dEnd   = h.Vara.Sunset;
			}
			else
			{
				dStart = h.Vara.Sunset;
				dEnd   = h.Vara.Sunrise;
			}

			Time dPeriod = (dEnd - dStart) / 8.0;
			Time dOffset = dPeriod         / 2.0;

			var jds = new JulianDate[8];
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
					dUpagrahaOffset = dOffset.TotalHours;
					break;
				case HoroscopeOptions.EUpagrahaType.End:
					dUpagrahaOffset = dPeriod.TotalHours;
					break;
			}

			positionList.Add(h.CalculateUpagrahasSingle(Body.Kala, jds[bodyOffsets[(int) Body.Sun]]));
			positionList.Add(h.CalculateUpagrahasSingle(Body.Mrityu, jds[bodyOffsets[(int) Body.Mars]]));
			positionList.Add(h.CalculateUpagrahasSingle(Body.ArthaPraharaka, jds[bodyOffsets[(int) Body.Mercury]]));
			positionList.Add(h.CalculateUpagrahasSingle(Body.YamaGhantaka, jds[bodyOffsets[(int) Body.Jupiter]]));


			positionList.Add(h.CalculateMaandiHelper(Body.Maandi, h.Options.MaandiType, jds, dOffset.TotalHours, bodyOffsets));
			positionList.Add(h.CalculateMaandiHelper(Body.Gulika, h.Options.GulikaType, jds, dOffset.TotalHours, bodyOffsets));

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

		public static List<Position> CalculateChandraLagnas(this Horoscope h)
		{
			var positionList = new List<Position>(); 
			var bpMoon       = h.GetPosition(Body.Moon);
			var lonBase      = new Longitude(bpMoon.ExtrapolateLongitude(DivisionType.Navamsa).ToZodiacHouseBase());
			lonBase = lonBase.Add(bpMoon.Longitude.ToZodiacHouseOffset());

			//Mhora.Log.Debug ("Starting Chandra Ayur Lagna from {0}", lon_base);

			var time      =  (h.Info.DateOfBirth - h.Vara.Sunrise);
			var istaGhati = time.TotalHours.NormalizeExc(0, 24) * 2.5;
			var glLon     = lonBase.Add(new Longitude(istaGhati        * 30));
			var hlLon     = lonBase.Add(new Longitude(istaGhati * 30 / 2.5));
			var blLon     = lonBase.Add(new Longitude(istaGhati * 30 / 5));

			var vl = istaGhati * 5;
			while (istaGhati > 12)
			{
				istaGhati -= 12;
			}

			var vlLon = lonBase.Add(new Longitude(vl * 30));

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
			double offset = (h.Info.DateOfBirth - h.Vara.Sunrise).TotalHours;

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
