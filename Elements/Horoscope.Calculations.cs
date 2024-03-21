using System.Diagnostics;
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements.Extensions;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Elements
{
	public partial class Horoscope
	{
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
			var muhurtaPos = new Longitude((Vara.HoursAfterSunrise / (Vara.Length) * 360.0).TotalHours);

			// add simple midpoints
			AddOtherPosition("User Specified", new Longitude(Options.CustomBodyLongitude.Value));
			AddOtherPosition("Brighu Bindu", rahPos.Add((double) (moonPos.Sub(rahPos).Value / 2M)));
			AddOtherPosition("Muhurta Point", muhurtaPos);
			AddOtherPosition("Ra-Ke m.p", rahPos.Add(90.0));
			AddOtherPosition("Ke-Ra m.p", rahPos.Add(270.0));

			var l1Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse(), DivisionType.Rasi, false)).Longitude;
			var l6Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), DivisionType.Rasi, false)).Longitude;
			var l8Pos  = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), DivisionType.Rasi, false)).Longitude;
			var l12Pos = GetPosition(LordOfZodiacHouse(lagPos.ToZodiacHouse().Add(6), DivisionType.Rasi, false)).Longitude;

			var mritSatPos   = new Longitude(mandiPos.Value * 8 + satPos.Value   * 8);
			var mritJup2Pos  = new Longitude(satPos.Value   * 9 + mandiPos.Value * 18 + jupPos.Value  * 18);
			var mritSun2Pos  = new Longitude(satPos.Value   * 9 + mandiPos.Value * 18 + sunPos.Value  * 18);
			var mritMoon2Pos = new Longitude(satPos.Value   * 9 + mandiPos.Value * 18 + moonPos.Value * 18);

			if (Vara.IsDayBirth)
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
					SwephHouseCusps[i] = SwephHouseCusps[i].Sub((double) offset);
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
			// Basic grahas + Special lagnas (depend on sunrise)
			PositionList = this.CalculateBodyPositions(Vara.Sunrise.Time.TotalHours);
			// Srilagna etc
			PositionList.Add(this.CalculateSl());
			PositionList.Add(this.CalculatePranapada());
			// Sun based Upagrahas (depends on sun)
			PositionList.AddRange(this.CalculateSunsUpagrahas());
			// Upagrahas (depends on sunrise)
			PositionList.AddRange(this.CalculateUpagrahas());
			// Sahamas
			_ = FindGrahas(DivisionType.Rasi);
			PositionList.AddRange (this.CalculateSahamas());
			// Prana sphuta etc. (depends on upagrahas)
			GetPrashnaMargaPositions();
			PositionList.AddRange(this.CalculateChandraLagnas());
			AddOtherPoints();
			// Add extrapolated special lagnas (depends on sunrise)
			AddSpecialLagnaPositions();
			// Populate house cusps on options refresh
			PopulateHouseCusps();
		}

		public void AddSpecialLagnaPositions()
		{
			Time diff = (Info.DateOfBirth - Vara.Sunrise);

			for (var i = 1; i <= 12; i++)
			{
				var specialDiff = diff * (i - 1);
				var tjd         = Info.Jd + specialDiff.TotalDays;
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
			var pranaLon   = new Longitude(lagnaLon.Value  * 5).Add(gulikaLon);
			var dehaLon    = new Longitude(moonLon.Value   * 8).Add(gulikaLon);
			var mrityuLon  = new Longitude(gulikaLon.Value * 7).Add(sunLon);

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
