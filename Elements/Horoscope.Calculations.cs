using System.Diagnostics;
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements
{
	public partial class Horoscope
	{
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
					sr = Calculations.NormalizeExc(sr, 0.0, 24.0);
					ss = Calculations.NormalizeExc(ss, 0.0, 24.0);
					break;
			}
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
			var bp = PositionList[b.Index()];
			if (bp.Name == b)
			{
				return bp;
			}

			for (var i = (int) Body.Lagna + 1; i < PositionList.Count; i++)
			{
				var position = PositionList[i];
				if (b == position.Name)
				{
					return position;
				}
			}

			Trace.Assert(false, "Basics::GetPosition. Unable to find body");
			return PositionList[0];
		}
	}
}
