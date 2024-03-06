using System.Collections;
using System.Collections.Generic;
using Mhora.Definitions;

namespace Mhora.Elements
{
	public static class Sahama
	{
		public static Position SahamaHelper(this Horoscope h,  string sahama, Body b, Body a, Body c)
		{
			var lonA = h.GetPosition(a).Longitude;
			var       lonB = h.GetPosition(b).Longitude;
			var       lonC = h.GetPosition(c).Longitude;
			return h.SahamaHelper(sahama, lonB, lonA, lonC);
		}

		public static Position SahamaHelper(this Horoscope h,  string sahama, Body b, Body a, Longitude lonC)
		{
			var lonA = h.GetPosition(a).Longitude;
			var       lonB = h.GetPosition(b).Longitude;
			return h.SahamaHelper(sahama, lonB, lonA, lonC);
		}

		public static Position SahamaHelper(this Horoscope h,  string sahama, Longitude lonB, Body a, Body c)
		{
			var lonA = h.GetPosition(a).Longitude;
			var       lonC = h.GetPosition(c).Longitude;
			return h.SahamaHelper(sahama, lonB, lonA, lonC);
		}

		public static Position SahamaHelper(this Horoscope h,  string sahama, Longitude lonB, Longitude lonA, Longitude lonC)
		{
			// b-a+c
			var bDay = h.IsDayBirth();

			var lonR = lonB.Sub(lonA).Add(lonC);
			if (lonB.Sub(lonA).Value <= lonC.Sub(lonA).Value)
			{
				lonR = lonR.Add(new Longitude(30.0));
			}

			var bp = new Position(h, Body.Other, BodyType.Sahama, lonR, 0.0, 0.0, 0.0, 0.0, 0.0)
			{
				OtherString = sahama
			};
			return bp;
		}

		public static Position SahamaDnHelper(this Horoscope h,  string sahama, Longitude lonB, Longitude lonA, Longitude lonC)
		{
			// b-a+c
			var       bDay = h.IsDayBirth();
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

			var bp = new Position(h, Body.Other, BodyType.Sahama, lonR, 0.0, 0.0, 0.0, 0.0, 0.0)
			{
				OtherString = sahama
			};
			return bp;
		}

		public static Position SahamaDnHelper(this Horoscope h,  string sahama, Body b, Longitude lonA, Body c)
		{
			var lonB = h.GetPosition(b).Longitude;
			var       lonC = h.GetPosition(c).Longitude;
			return h.SahamaDnHelper(sahama, lonB, lonA, lonC);
		}

		public static Position SahamaDnHelper(this Horoscope h,  string sahama, Longitude lonB, Body a, Body c)
		{
			var lonA = h.GetPosition(a).Longitude;
			var       lonC = h.GetPosition(c).Longitude;
			return h.SahamaDnHelper(sahama, lonB, lonA, lonC);
		}

		public static Position SahamaDnHelper(this Horoscope h,  string sahama, Longitude lonB, Longitude lonA, Body c)
		{
			var lonC = h.GetPosition(c).Longitude;
			return h.SahamaDnHelper(sahama, lonB, lonA, lonC);
		}

		public static Position SahamaDnHelper(this Horoscope h,  string sahama, Body b, Body a, Body c)
		{
			var lonA = h.GetPosition(a).Longitude;
			var       lonB = h.GetPosition(b).Longitude;
			var       lonC = h.GetPosition(c).Longitude;
			return h.SahamaDnHelper(sahama, lonB, lonA, lonC);
		}

		public static Position SahamaHelperNormalize(this Horoscope h, Position b, Body lower, Body higher)
		{
			var lonA = h.GetPosition(lower).Longitude;
			var lonB = h.GetPosition(higher).Longitude;
			if (b.Longitude.Sub(lonA).Value < lonB.Sub(lonA).Value)
			{
				return b;
			}

			b.Longitude = b.Longitude.Add(new Longitude(30.0));
			return b;
		}

		public static List <Position> CalculateSahamas(this Horoscope h)
		{
			var bDay     = h.IsDayBirth();
			var al       = new List <Position>();
			var lonLagna = h.GetPosition(Body.Lagna).Longitude;
			var lonBase  = new Longitude(lonLagna.ToZodiacHouseBase());
			var zhLagna  = lonLagna.ToZodiacHouse();
			var zhMoon   = h.GetPosition(Body.Moon).Longitude.ToZodiacHouse();
			var zhSun    = h.GetPosition(Body.Sun).Longitude.ToZodiacHouse();


			// Fixed positions. Relied on by other sahams
			al.Add(h.SahamaDnHelper("Punya", Body.Moon, Body.Sun, Body.Lagna));
			al.Add(h.SahamaDnHelper("Vidya", Body.Sun, Body.Moon, Body.Lagna));
			al.Add(h.SahamaDnHelper("Sastra", Body.Jupiter, Body.Saturn, Body.Mercury));

			// Variable positions.
			al.Add(h.SahamaDnHelper("Yasas", Body.Jupiter, ((Position) al[0]).Longitude, Body.Lagna));
			al.Add(h.SahamaDnHelper("Mitra", Body.Jupiter, ((Position) al[0]).Longitude, Body.Venus));
			al.Add(h.SahamaDnHelper("Mahatmya", ((Position) al[0]).Longitude, Body.Mars, Body.Lagna));

			var bLagnaLord = h.LordOfZodiacHouse(zhLagna, DivisionType.Rasi, false);
			if (bLagnaLord != Body.Mars)
			{
				al.Add(h.SahamaDnHelper("Samartha", Body.Mars, bLagnaLord, Body.Lagna));
			}
			else
			{
				al.Add(h.SahamaDnHelper("Samartha", Body.Jupiter, Body.Mars, Body.Lagna));
			}

			al.Add(h.SahamaHelper("Bhratri", Body.Jupiter, Body.Saturn, Body.Lagna));
			al.Add(h.SahamaDnHelper("Gaurava", Body.Jupiter, Body.Moon, Body.Sun));
			al.Add(h.SahamaDnHelper("Pitri", Body.Saturn, Body.Sun, Body.Lagna));
			al.Add(h.SahamaDnHelper("Rajya", Body.Saturn, Body.Sun, Body.Lagna));
			al.Add(h.SahamaDnHelper("Matri", Body.Moon, Body.Venus, Body.Lagna));
			al.Add(h.SahamaDnHelper("Putra", Body.Jupiter, Body.Moon, Body.Lagna));
			al.Add(h.SahamaDnHelper("Jeeva", Body.Saturn, Body.Jupiter, Body.Lagna));
			al.Add(h.SahamaDnHelper("Karma", Body.Mars, Body.Mercury, Body.Lagna));
			al.Add(h.SahamaDnHelper("Roga", Body.Lagna, Body.Moon, Body.Lagna));
			al.Add(h.SahamaDnHelper("Kali", Body.Jupiter, Body.Mars, Body.Lagna));
			al.Add(h.SahamaDnHelper("Bandhu", Body.Mercury, Body.Moon, Body.Lagna));
			al.Add(h.SahamaHelper("Mrityu", lonBase.Add(8.0   * 30.0), Body.Moon, Body.Lagna));
			al.Add(h.SahamaHelper("Paradesa", lonBase.Add(9.0 * 30.0), h.LordOfZodiacHouse(zhLagna.Add(9), DivisionType.Rasi, false), Body.Lagna));
			al.Add(h.SahamaHelper("Artha", lonBase.Add(2.0    * 30.0), h.LordOfZodiacHouse(zhLagna.Add(2), DivisionType.Rasi, false), Body.Lagna));
			al.Add(h.SahamaDnHelper("Paradara", Body.Venus, Body.Sun, Body.Lagna));
			al.Add(h.SahamaDnHelper("Vanik", Body.Moon, Body.Mercury, Body.Lagna));

			if (bDay)
			{
				al.Add(h.SahamaHelper("Karyasiddhi", Body.Saturn, Body.Sun, h.LordOfZodiacHouse(zhSun, DivisionType.Rasi, false)));
			}
			else
			{
				al.Add(h.SahamaHelper("Karyasiddhi", Body.Saturn, Body.Moon, h.LordOfZodiacHouse(zhMoon, DivisionType.Rasi, false)));
			}

			al.Add(h.SahamaDnHelper("Vivaha", Body.Venus, Body.Saturn, Body.Lagna));
			al.Add(h.SahamaHelper("Santapa", Body.Saturn, Body.Moon, lonBase.Add(6.0 * 30.0)));
			al.Add(h.SahamaDnHelper("Sraddha", Body.Venus, Body.Mars, Body.Lagna));
			al.Add(h.SahamaDnHelper("Preeti", ((Position) al[2]).Longitude, ((Position) al[0]).Longitude, Body.Lagna));
			al.Add(h.SahamaDnHelper("Jadya", Body.Mars, Body.Saturn, Body.Mercury));
			al.Add(h.SahamaHelper("Vyapara", Body.Mars, Body.Saturn, Body.Lagna));
			al.Add(h.SahamaDnHelper("Satru", Body.Mars, Body.Saturn, Body.Lagna));
			al.Add(h.SahamaDnHelper("Jalapatana", new Longitude(105.0), Body.Saturn, Body.Lagna));
			al.Add(h.SahamaDnHelper("Bandhana", ((Position) al[0]).Longitude, Body.Saturn, Body.Lagna));
			al.Add(h.SahamaDnHelper("Apamrityu", lonBase.Add(8.0 * 30.0), Body.Mars, Body.Lagna));
			al.Add(h.SahamaHelper("Labha", lonBase.Add(11.0      * 30.0), h.LordOfZodiacHouse(zhLagna.Add(11), DivisionType.Rasi, false), Body.Lagna));

			return al;
		}
	}
}
