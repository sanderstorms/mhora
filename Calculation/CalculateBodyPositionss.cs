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

			dp.Cusp.ZodiacHouse = zhsum;
			var dp2 = new DivisionPosition(Body.Other, BodyType.GrahaArudha, zhsum, dp.Cusp)
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
			var al    = new List <DivisionPosition> ();
			var lagna = h.GetPosition(Body.Lagna).ToDivisionPosition(dtype);
			var zhL   = lagna.ZodiacHouse;
			var hl    = h.GetPosition(Body.HoraLagna).ToDivisionPosition(dtype);
			var zhHl  = hl.ZodiacHouse;

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

				var cusp = new Cusp(lagna.Longitude, Vargas.NumPartsInDivision(dtype));
				var divPos = new DivisionPosition(Body.Other, BodyType.Varnada, zhV, cusp)
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

			var cusp = new Cusp(bp.Longitude, Vargas.NumPartsInDivision(d));
			var dp    = new DivisionPosition(aname, btype, zhsum, cusp);
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

		private static Position CalculateUpagrahasSingle(this Horoscope h, Body b, JulianDate tjd)
		{
			var lon = new Longitude(0.0)
			{
				Value = h.Lagna(tjd)
			};
			return new Position(h, b, BodyType.Upagraha, lon);
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

		//There are nine Upagrahas linked to the nine Grahas (planets) which are recognized by Jyotish rules. These are:
		// 
		// 1. Kaala  --- Sun
		// 2. Dhoom --- Moon
		// 3. Parivesh --- Mars
		// 4. Ardha Prahar --- Mercury
		// 5. Yamkantaka --- Jupiter
		// 6. Vyatipat --- Venus
		// 7. Gulika --- Saturn
		// 8. Indradhanush --- Rahu
		// 9. Upaketu --- Ketu
		// 
		// These nine upagrahas are categorised in two categories. The first category has Gulika, Ardha Prahar, Yamkantaka and Kaala.
		// The second category consists of Dhoom, Vyatipat, Indradhanush and Upaketu.
		// These two categories of upagrahas are not enemical to each other and they are categorised on the basis of their method of calculation.
		// 
		// The Upagrahas in first category are calculated on basis of the points based on set time difference from the actual sunrise and sunset.
		// Also, these points keep on changing with every day of the week. The second category Upagrahas are also based on set time intervals
		// from actual sunrise and sunset but their base points always remain constant throughout the week. 
		// They based on Sun's longitude. All these upagrahas are very malefic in nature.
		// Any houses occupied by them in rasi chart or divisional charts are spoiled by them.
		public static List<Position> CalculateUpagrahas(this Horoscope h)
		{
			var positionList = new List<Position>
			{
				new (h, Body.Kala, BodyType.Upagraha, h.Vara.CalculateUpgraha(Body.Sun)),
				new (h, Body.ArthaPraharaka, BodyType.Upagraha, h.Vara.CalculateUpgraha(Body.Mercury)),
				new (h, Body.YamaGhantaka, BodyType.Upagraha, h.Vara.CalculateUpgraha(Body.Jupiter)),
				new (h, Body.Gulika, BodyType.Upagraha, h.Vara.CalculateUpgraha(Body.Saturn)),
				new (h, Body.Maandi, BodyType.Upagraha, h.Vara.CalculateUpgraha(Body.Saturn, true, HoroscopeOptions.EUpagrahaType.End)),
				new (h, Body.Mrityu, BodyType.Upagraha, h.Vara.CalculateUpgraha(Body.Mars))
			};

			return (positionList);
		}

		// Upagraha Longitude Formula
		// Dhuma Sun's longitude + 13320'
		// Vyatipaata 360º – Dhuma’s longitude
		// Parivesha Vyatipata's longitude + 180
		// Indrachaapa 360º – Parivesha’s longitude
		// Upaketu Indrachaapa’s longitude + 1640'= Sun's longitude – 30
		// It may be noted that Dhuma and Indrachaapa are apart by 180 and Vyatipaata and Parivesha are apart by 180.
		public static List<Position> CalculateSunsUpagrahas(this Horoscope h)
		{
			var slon         = h.GetPosition(Body.Sun).Longitude;
			var positionList = new List<Position>();

			var bpDhuma      = new Position(h, Body.Dhuma, BodyType.Upagraha, slon.Add(133.0 + 20.0 / 60.0));
			var bpVyatipata  = new Position(h, Body.Vyatipata, BodyType.Upagraha, new Longitude(360.0).Sub(bpDhuma.Longitude));
			var bpParivesha  = new Position(h, Body.Parivesha, BodyType.Upagraha, bpVyatipata.Longitude.Add(180.0));
			var bpIndrachapa = new Position(h, Body.Indrachapa, BodyType.Upagraha, new Longitude(360.0).Sub(bpParivesha.Longitude));
			var bpUpaketu    = new Position(h, Body.Upaketu, BodyType.Upagraha, slon.Sub(30.0));

			positionList.Add(bpDhuma);
			positionList.Add(bpVyatipata);
			positionList.Add(bpParivesha);
			positionList.Add(bpIndrachapa);
			positionList.Add(bpUpaketu);

			return (positionList);
		}


		//Bhaav    Lagna: changes sign every 5 Ghatees (every 2 hours)
		//Horaa Lagna: changes sign every 2.5 Ghatees (each hour)
		//Ghatee   Lagna: changes sign every Ghatee (every 24 min)
		//Vighatee Lagna: changes sign every Vighatee (every 24 sec)
		public static List<Position> CalculateChandraLagnas(this Horoscope h)
		{
			var positionList = new List<Position>(); 
			var bpMoon       = h.GetPosition(Body.Moon);
			var lonBase      = new Longitude(bpMoon.ExtrapolateLongitude(DivisionType.Navamsa).ToZodiacHouseBase());
			lonBase = lonBase.Add(bpMoon.Longitude.ToZodiacHouseOffset());

			//Mhora.Log.Debug ("Starting Chandra Ayur Lagna from {0}", lon_base);

			var istaGhati = h.Vara.Isthaghati.Ghati;
			var blLon     = lonBase.Add(new Longitude(istaGhati * 30 / 5));
			var hlLon     = lonBase.Add(new Longitude(istaGhati * 30 / 2.5));
			var glLon     = lonBase.Add(new Longitude(istaGhati      * 30));
			var vlLon     = lonBase.Add(new Longitude(istaGhati      * 30 * 15));

			positionList.Add(h.AddChandraLagna("Chandra Lagna - GL", glLon));
			positionList.Add(h.AddChandraLagna("Chandra Lagna - HL", hlLon));
			positionList.Add(h.AddChandraLagna("Chandra Ayur Lagna - BL", blLon));
			positionList.Add(h.AddChandraLagna("Chandra Lagna - ViL", vlLon));

			return (positionList);
		}

		//Calculation of Shree Lagna - The birth Nakshatra of a native (where Moon is placed in)
		//is divided into 12 parts, then the distance from the beginning of the Nakshatra to the Moon
		//is converted into signs and added to the natal Lagna.
		public static Position CalculateSl(this Horoscope h)
		{
			var mpos  = h.GetPosition(Body.Moon).Longitude;
			var lpos  = h.GetPosition(Body.Lagna).Longitude;

			var nakshattra = (360.0 / 27.0); //Nakshattra length
			var distance   = (mpos.NakshatraOffset() / nakshattra) * 12;
			var sign       = lpos + (distance  * 30.0);
			sign.Reduce();
			var shreeLagna = new Longitude (sign.Value);

			var bp    = new Position(h, Body.SreeLagna, BodyType.SpecialLagna, shreeLagna);
			return (bp);
		}

		// The difference between Birth time and Sunrise (also called Ishtagati) should be calculated in
		// Vighatikas and then divided by 15.
		// If at the time of birth Sun is in a Movable Sign consider the same sign and longitude, if in a fixed
		// sign, consider the 9th sign with the same longitude and if in a dual sign consider the 5th sign from it
		// with the same longitude.
		//
		// PP = Ishtagati [in minutes] X Rate of PP + Arka-kona
		// Rate of PP = 1 rasi/6 minutes = 5 degrees/minute
		//
		// Sun 6 aqu 54
		// Sunrise Time on the day of birth of Thakur = 6:31:24 hrs
		// Time of birth of Thakur = 6:44:00 hrs
		// Ishtagati = 12:36 minutes = 12.6 minutes
		// Ishtagati X Rate of PP = 63 degrees = 2s + 3
		// Arka-kona = 7 degrees of Libra = 6s + 7
		// Thus, PP = 2s + 3 + 6s + 7 = 8s + 10 = 10 degrees of Sagittarius
		//
		// Sun 29 Sg 26
		// Sunrise Time on the day of birth = 6:42:47 hrs
		// Time of birth = 6:33:00 hrs
		// Ishtagati = - 10 minutes
		// Ishtagati X Rate of PP = - 50 degrees = -1s - 20
		// Arka-kona = 29 degrees = 12s + 29
		// Thus, PP = 12s + 29 – 1s – 20 = 9 degrees of Pisces
		public static Position CalculatePranapada(this Horoscope h)
		{
			var sun       = h.FindGrahas(DivisionType.Rasi) [Body.Sun];
			var ppMinutes = (h.Vara.Isthaghati.TotalMinutes * 5); //degrees
			var arkaKopa  = sun.Position.Longitude;
			var zh        = sun.Rashi.ZodiacHouse;

			if (zh.IsMoveableSign())
			{
				//same;
			}
			if (zh.IsFixedSign())
			{
				arkaKopa = arkaKopa.Add(30.0 * 8);
			}
			else if (zh.IsDualSign())
			{
				arkaKopa = arkaKopa.Add(30.0 * 4);
			}

			var pp = arkaKopa.Add(ppMinutes);

			var bp = new Position(h, Body.Pranapada, BodyType.SpecialLagna, pp);
			return (bp);
		}

		private static Position AddChandraLagna(this Horoscope h, string desc, Longitude lon)
		{
			var bp = new Position(h, Body.Other, BodyType.ChandraLagna, lon)
			{
				OtherString = desc
			};
			return (bp);
		}
	}
}
