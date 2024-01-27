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
using Mhora.Elements.Calculation;
using Mhora.Tables;
using Mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     Specifies a BodyPosition, i.e. the astronomical characteristics of a body like
///     longitude, speed etc. It has no notion of its "rasi".
///     The functions to convert this to a DivisionType (the various vargas)
///     are all implemented here
/// </summary>
public class Position : ICloneable
{
	private static bool      _mbNadiamsaCknCalculated;
	private static double[]  _mNadiamsaCusps;
	public         Horoscope H;
	public         Body.BodyType Name;
	public         string    OtherString;
	public         Body.Type Type;

	public Position(Horoscope h, Body.BodyType aname, Body.Type atype, Longitude lon, double lat, double dist, double splon, double splat, double spdist)
	{
		Longitude       = lon;
		Latitude        = lat;
		Distance        = dist;
		SpeedLongitude = splon;
		SpeedLatitude  = splat;
		SpeedDistance  = spdist;
		Name            = aname;
		Type            = atype;
		H               = h;
		//Mhora.Log.Debug ("{0} {1} {2}", aname.ToString(), lon.value, splon);
	}

	public Longitude Longitude
	{
		get;
		set;
	}

	public double Latitude
	{
		get;
		set;
	}

	public double Distance
	{
		get;
		set;
	}

	public double SpeedLongitude
	{
		get;
		set;
	}

	public double SpeedLatitude
	{
		get;
		set;
	}

	public double SpeedDistance
	{
		get;
		set;
	}

	public object Clone()
	{
		var bp = new Position(H, Name, Type, Longitude.Add(0), Latitude, Distance, SpeedLongitude, SpeedLatitude, SpeedDistance);
		bp.OtherString = OtherString;
		return bp;
	}

	public int PartOfZodiacHouse(int n)
	{
		var l      = Longitude;
		var offset = l.ToZodiacHouseOffset();
		var part   = (int) Math.Floor(offset / (30.0 / n)) + 1;
		Trace.Assert(part >= 1 && part <= n);
		return part;
	}

	private DivisionPosition PopulateRegularCusps(int n, DivisionPosition dp)
	{
		var part        = PartOfZodiacHouse(n);
		var cuspLength = 30.0 / n;
		dp.CuspLower  =  (part - 1) * cuspLength;
		dp.CuspLower  += Longitude.ToZodiacHouseBase();
		dp.CuspHigher =  dp.CuspLower + cuspLength;
		dp.Part        =  part;

		//if (dp.type == BodyType.Type.Graha || dp.type == BodyType.Type.Lagna)
		//Mhora.Log.Debug ("D: {0} {1} {2} {3} {4} {5}", 
		//	n, dp.name, cusp_length,
		//	dp.cusp_lower, m_lon.value, dp.cusp_higher);


		return dp;
	}

	/// <summary>
	///     Many of the varga divisions (like navamsa) are regular divisions,
	///     and can be implemented by a single method. We do this when possible.
	/// </summary>
	/// <param name="n">The number of parts a house is divided into</param>
	/// <returns>The DivisionPosition the body falls into</returns>
	private DivisionPosition ToRegularDivisionPosition(int n)
	{
		var zhouse   = Longitude.ToZodiacHouse().Index();
		var numParts = (zhouse - 1) * n + PartOfZodiacHouse(n);
		var divHouse = ZodiacHouse.Ari.Add(numParts);
		var dp       = new DivisionPosition(Name, Type, divHouse, 0, 0, 0);
		PopulateRegularCusps(n, dp);

		if (n > 1)
		{
			dp.Longitude = divHouse.DivisionalLongitude(Longitude, n);
		}
		else
		{
			dp.Longitude = Longitude;
		}

		return dp;
	}

	private DivisionPosition ToRegularDivisionPositionFromCurrentHouseOddEven(int n)
	{
		var zhouse    = (int) Longitude.ToZodiacHouse();
		var numParts = PartOfZodiacHouse(n);
		var divHouse = Longitude.ToZodiacHouse().Add(numParts);
		var dp        = new DivisionPosition(Name, Type, divHouse, 0, 0, 0);
		dp.Longitude = divHouse.DivisionalLongitude(Longitude, n);
		PopulateRegularCusps(n, dp);
		return dp;
	}


	private DivisionPosition ToBhavaDivisionPositionRasi(Longitude[] cusps)
	{
		Debug.Assert(cusps.Length == 13);
		cusps[12] = cusps[0];
		for (var i = 0; i < 12; i++)
		{
			if (Longitude.Sub(cusps[i]).Value <= cusps[i + 1].Sub(cusps[i]).Value)
			{
				return new DivisionPosition(Name, Type, (ZodiacHouse) i + 1, cusps[i].Value, cusps[i + 1].Value, 1);
			}
		}

		throw new Exception();
	}

	private DivisionPosition ToBhavaDivisionPositionHouse(Longitude[] cusps)
	{
		Debug.Assert(cusps.Length == 13);

		var zlagna = H.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(new Division(Vargas.DivisionType.Rasi)).ZodiacHouse;
		for (var i = 0; i < 12; i++)
		{
			if (Longitude.Sub(cusps[i]).Value < cusps[i + 1].Sub(cusps[i]).Value)
			{
				//Mhora.Log.Debug ("Found {4} - {0} in cusp {3} between {1} and {2}", this.m_lon.value,
				//	cusps[i].value, cusps[i+1].value, i+1, this.name.ToString());

				return new DivisionPosition(Name, Type, zlagna.Add(i + 1), cusps[i].Value, cusps[i + 1].Value, 1);
			}
		}

		return new DivisionPosition(Name, Type, zlagna.Add(1), cusps[0].Value, cusps[1].Value, 1);
	}

	private DivisionPosition ToDivisionPositionBhavaEqual()
	{
		var offset = H.GetPosition(Body.BodyType.Lagna).Longitude.ToZodiacHouseOffset();
		var cusps  = new Longitude[13];
		for (var i = 0; i < 12; i++)
		{
			cusps[i] = new Longitude(i * 30.0 + offset - 15.0);
		}

		return ToBhavaDivisionPositionRasi(cusps);
	}

	private DivisionPosition ToDivisionPositionBhavaPada()
	{
		var cusps       = new Longitude[13];
		var offset      = H.GetPosition(Body.BodyType.Lagna).Longitude.ToZodiacHouseOffset();
		var padasOffset = (int) Math.Floor(offset / (360.0 / 108.0));
		var startOffset = padasOffset * (360.0 / 108.0);

		for (var i = 0; i < 12; i++)
		{
			cusps[i] = new Longitude(i * 30.0 + startOffset - 15.0);
		}

		return ToBhavaDivisionPositionRasi(cusps);
	}

	private DivisionPosition ToDivisionPositionBhavaHelper(int hsys)
	{
		var cusps  = new Longitude[13];
		var dCusps = new double[13];
		var ascmc  = new double[10];

		if (hsys != H.SwephHouseSystem)
		{
			H.SwephHouseSystem = hsys;
			H.PopulateHouseCusps();
		}

		return ToBhavaDivisionPositionHouse(H.SwephHouseCusps);
	}

	private bool HoraSunDayNight()
	{
		var sign = (int) Longitude.ToZodiacHouse();
		var part = PartOfZodiacHouse(2);
		if (Longitude.ToZodiacHouse().IsDaySign())
		{
			if (part == 1)
			{
				return true;
			}

			return false;
		}

		if (part == 1)
		{
			return false;
		}

		return true;
	}

	private bool HoraSunOddEven()
	{
		var sign = (int) Longitude.ToZodiacHouse();
		var part = PartOfZodiacHouse(2);
		var mod  = sign % 2;
		switch (mod)
		{
			case 1:
				if (part == 1)
				{
					return true;
				}

				return false;
			default:
				if (part == 1)
				{
					return false;
				}

				return true;
		}
	}

	private DivisionPosition ToDivisionPositionHoraKashinath()
	{
		var daySigns = new int[13]
		{
			0,
			8,
			7,
			6,
			5,
			5,
			6,
			7,
			8,
			12,
			11,
			11,
			12
		};
		var nightSigns = new int[13]
		{
			0,
			1,
			2,
			3,
			4,
			4,
			3,
			2,
			1,
			9,
			10,
			10,
			9
		};

		ZodiacHouse zh;
		var        sign = (int) Longitude.ToZodiacHouse();
		if (HoraSunOddEven())
		{
			zh = (ZodiacHouse) daySigns[sign];
		}
		else
		{
			zh = (ZodiacHouse) nightSigns[sign];
		}

		var dp = new DivisionPosition(Name, Type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(Longitude, 2);
		PopulateRegularCusps(2, dp);
		return dp;
	}

	private DivisionPosition ToDivisionPositionHoraJagannath()
	{
		var zh = Longitude.ToZodiacHouse();

		Application.Log.Debug("{2} in {3}: OddEven is {0}, DayNight is {1}", HoraSunOddEven(), HoraSunDayNight(), Name, zh);

		if (HoraSunDayNight() && false == HoraSunOddEven())
		{
			zh = zh.Add(7);
		}
		else if (false == HoraSunDayNight() && HoraSunOddEven())
		{
			zh = zh.Add(7);
		}

		Application.Log.Debug("{0} ends in {1}", Name, zh);

		var dp = new DivisionPosition(Name, Type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(Longitude, 2);
		PopulateRegularCusps(2, dp);
		return dp;
	}

	private DivisionPosition ToDivisionPositionHoraParasara()
	{
		ZodiacHouse zh;
		var        rulerIndex = 0;
		if (HoraSunOddEven())
		{
			zh         = ZodiacHouse.Leo;
			rulerIndex = 1;
		}
		else
		{
			zh         = ZodiacHouse.Can;
			rulerIndex = 2;
		}

		var dp = new DivisionPosition(Name, Type, zh, 0, 0, 0);
		dp.Longitude   = zh.DivisionalLongitude(Longitude, 2);
		dp.RulerIndex = rulerIndex;
		return PopulateRegularCusps(2, dp);
	}

	private DivisionPosition ToDivisionPositionDrekanna(int n)
	{
		var offset = new int[4]
		{
			9,
			1,
			5,
			9
		};
		var zhouse = Longitude.ToZodiacHouse();
		var part   = PartOfZodiacHouse(n);
		var dhouse = zhouse.Add(offset[part % 3]);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, 3);
		PopulateRegularCusps(n, dp);
		if (n == 3)
		{
			var rulerIndex = (int) dp.ZodiacHouse % 3;
			if (rulerIndex == 0)
			{
				rulerIndex = 3;
			}

			dp.RulerIndex = rulerIndex;
		}

		return dp;
	}

	private DivisionPosition ToDivisionPositionDrekannaJagannath()
	{
		var         zh = Longitude.ToZodiacHouse();
		ZodiacHouse zhm;
		ZodiacHouse dhouse;
		var         mod = (int) Longitude.ToZodiacHouse() % 3;
		// Find moveable sign in trines
		switch (mod)
		{
			case 1:
				zhm = zh.Add(1);
				break;
			case 2:
				zhm = zh.Add(9);
				break;
			default:
				zhm = zh.Add(5);
				break;
		}

		// From moveable sign, 3 parts belong to the trines
		var part = PartOfZodiacHouse(3);
		switch (part)
		{
			case 1:
				dhouse = zhm.Add(1);
				break;
			case 2:
				dhouse = zhm.Add(5);
				break;
			default:
				dhouse = zhm.Add(9);
				break;
		}

		var dp = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, 3);
		return PopulateRegularCusps(3, dp);
	}

	private DivisionPosition ToDivisionPositionDrekkanaSomnath()
	{
		var mod  = (int) Longitude.ToZodiacHouse() % 2;
		var part = PartOfZodiacHouse(3);
		var zh   = Longitude.ToZodiacHouse();
		var p    = (int) zh;

		if (mod == 0)
		{
			p--;
		}

		p = (p - 1) / 2;
		var numDone = p * 3;

		var        zh1 = ZodiacHouse.Ari;
		ZodiacHouse zh2;
		if (mod == 1)
		{
			zh2 = zh1.Add(numDone + part);
		}
		else
		{
			zh2 = zh1.AddReverse(numDone + part + 1);
		}

		var dp = new DivisionPosition(Name, Type, zh2, 0, 0, 0);
		dp.Longitude = zh2.DivisionalLongitude(Longitude, 3);
		return PopulateRegularCusps(3, dp);
	}

	private DivisionPosition ToDivisionPositionChaturthamsa(int n)
	{
		var offset = new int[5]
		{
			10,
			1,
			4,
			7,
			10
		};
		var zhouse = Longitude.ToZodiacHouse();
		var part   = PartOfZodiacHouse(n);
		var dhouse = zhouse.Add(offset[part % 4]);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);
		if (n == 4)
		{
			dp.RulerIndex = part;
		}

		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionShashthamsa(int n)
	{
		var mod     = (int) Longitude.ToZodiacHouse() % 2;
		var dhousen = mod % 2 == 1 ? ZodiacHouse.Ari : ZodiacHouse.Lib;
		var dhouse  = dhousen.Add(PartOfZodiacHouse(n));
		var dp      = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);
		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionSaptamsa(int n)
	{
		var part = PartOfZodiacHouse(n);
		var zh   = Longitude.ToZodiacHouse();
		if (false == zh.IsOdd())
		{
			zh = zh.Add(7);
		}

		zh = zh.Add(part);
		var dp = new DivisionPosition(Name, Type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(Longitude, n);

		if (n == 7)
		{
			if (Longitude.ToZodiacHouse().IsOdd())
			{
				dp.RulerIndex = part;
			}
			else
			{
				dp.RulerIndex = 8 - part;
			}
		}

		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionNavamsa()
	{
		var part = PartOfZodiacHouse(9);
		var dp   = ToRegularDivisionPosition(9);
		switch ((int) Longitude.ToZodiacHouse() % 3)
		{
			case 1:
				dp.RulerIndex = part;
				break;
			case 2:
				dp.RulerIndex = part + 1;
				break;
			case 0:
				dp.RulerIndex = part + 2;
				break;
		}

		while (dp.RulerIndex > 3)
		{
			dp.RulerIndex -= 3;
		}

		return dp;
	}

	private DivisionPosition ToDivisionPositionAshtamsaRaman()
	{
		ZodiacHouse zstart;
		switch ((int) Longitude.ToZodiacHouse() % 3)
		{
			case 1:
				zstart = ZodiacHouse.Ari;
				break;
			case 2:
				zstart = ZodiacHouse.Leo;
				break;
			case 0:
			default:
				zstart = ZodiacHouse.Sag;
				break;
		}

		var dhouse = zstart.Add(PartOfZodiacHouse(8));
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, 8);
		return PopulateRegularCusps(8, dp);
	}

	private DivisionPosition ToDivisionPositionPanchamsa()
	{
		var offsetOdd = new []
		{
			ZodiacHouse.Ari,
			ZodiacHouse.Aqu,
			ZodiacHouse.Sag,
			ZodiacHouse.Gem,
			ZodiacHouse.Lib
		};
		var offsetEven = new []
		{
			ZodiacHouse.Tau,
			ZodiacHouse.Vir,
			ZodiacHouse.Pis,
			ZodiacHouse.Cap,
			ZodiacHouse.Sco
		};
		var part   = PartOfZodiacHouse(5);
		var mod    = (int) Longitude.ToZodiacHouse() % 2;
		var dhouse = mod % 2 == 1 ? offsetOdd[part - 1] : offsetEven[part - 1];
		var zh     = dhouse;
		var dp     = new DivisionPosition(Name, Type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(Longitude, 5);
		return PopulateRegularCusps(5, dp);
	}

	private DivisionPosition ToDivisionPositionRudramsa()
	{
		var zari   = ZodiacHouse.Ari;
		var zhouse = Longitude.ToZodiacHouse();
		var diff   = zari.NumHousesBetween(zhouse);
		var zstart = zari.AddReverse(diff);
		var part   = PartOfZodiacHouse(11);
		var zend   = zstart.Add(part);
		var dp     = new DivisionPosition(Name, Type, zend, 0, 0, 0);
		dp.Longitude = zend.DivisionalLongitude(Longitude, 11);
		return PopulateRegularCusps(11, dp);
	}

	private DivisionPosition ToDivisionPositionRudramsaRaman()
	{
		var zhstart = Longitude.ToZodiacHouse().Add(12);
		var part    = PartOfZodiacHouse(11);
		var zend    = zhstart.AddReverse(part);
		var dp      = new DivisionPosition(Name, Type, zend, 0, 0, 0);
		dp.Longitude = zend.DivisionalLongitude(Longitude, 11);
		return PopulateRegularCusps(11, dp);
	}

	private DivisionPosition ToDivisionPositionDasamsa(int n)
	{
		var offset = new int[2]
		{
			9,
			1
		};
		var zhouse = Longitude.ToZodiacHouse();
		var dhouse = zhouse.Add(offset[(int) zhouse % 2]);
		var part   = PartOfZodiacHouse(n);
		dhouse = dhouse.Add(part);
		var dp = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		if (n == 10)
		{
			if (Longitude.ToZodiacHouse().IsOdd())
			{
				dp.RulerIndex = part;
			}
			else
			{
				dp.RulerIndex = 11 - part;
			}
		}

		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);
		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionDwadasamsa(int n)
	{
		var zhouse = Longitude.ToZodiacHouse();
		var part   = PartOfZodiacHouse(n);
		var dhouse = zhouse.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		if (n == 12)
		{
			dp.RulerIndex = part.NormalizeInc(1, 4);
		}

		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);

		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionShodasamsa()
	{
		var part  = PartOfZodiacHouse(16);
		var dp    = ToRegularDivisionPosition(16);
		var ruler = part;
		if (Longitude.ToZodiacHouse().IsOdd())
		{
			ruler = part;
		}
		else
		{
			ruler = 17 - part;
		}

		dp.RulerIndex = ruler.NormalizeInc(1, 4);
		return dp;
	}

	private DivisionPosition ToDivisionPositionVimsamsa(int n)
	{
		var              mod = (int) Longitude.ToZodiacHouse() % 3;
		ZodiacHouse dhousename;
		switch (mod)
		{
			case 1:
				dhousename = ZodiacHouse.Ari;
				break;
			case 2:
				dhousename = ZodiacHouse.Sag;
				break;
			default:
				dhousename = ZodiacHouse.Leo;
				break;
		}

		var part   = PartOfZodiacHouse(n);
		var dhouse = dhousename.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);
		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionVimsamsa()
	{
		var part = PartOfZodiacHouse(20);
		var dp   = ToRegularDivisionPosition(20);
		if (Longitude.ToZodiacHouse().IsOdd())
		{
			dp.RulerIndex = part;
		}
		else
		{
			dp.RulerIndex = 20 + part;
		}

		return dp;
	}

	private DivisionPosition ToDivisionPositionChaturvimsamsa(int n)
	{
		var mod        = (int) Longitude.ToZodiacHouse() % 2;
		var dhousename = mod % 2 == 1 ? ZodiacHouse.Leo : ZodiacHouse.Can;
		var part       = PartOfZodiacHouse(n);
		var dhouse     = dhousename.Add(part);
		var dp         = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);
		if (n == 24)
		{
			if (Longitude.ToZodiacHouse().IsOdd())
			{
				dp.RulerIndex = part;
			}
			else
			{
				dp.RulerIndex = 25 - part;
			}

			dp.RulerIndex = dp.RulerIndex.NormalizeInc(1, 12);
		}

		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionNakshatramsa(int n)
	{
		var              mod = (int) Longitude.ToZodiacHouse() % 4;
		ZodiacHouse dhousename;
		switch (mod)
		{
			case 1:
				dhousename = ZodiacHouse.Ari;
				break;
			case 2:
				dhousename = ZodiacHouse.Can;
				break;
			case 3:
				dhousename = ZodiacHouse.Lib;
				break;
			default:
				dhousename = ZodiacHouse.Cap;
				break;
		}

		var part   = PartOfZodiacHouse(n);
		var dhouse = dhousename.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);
		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionNakshatramsa()
	{
		var dp = ToRegularDivisionPosition(27);
		dp.RulerIndex = PartOfZodiacHouse(27);
		return dp;
	}

	private DivisionPosition ToDivisionPositionTrimsamsaSimple()
	{
		var zhouse = Longitude.ToZodiacHouse();
		var part   = PartOfZodiacHouse(30);
		var dhouse = zhouse.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, 30);
		return PopulateRegularCusps(30, dp);
	}

	private DivisionPosition ToDivisionPositionTrimsamsa()
	{
		var         mod = (int) Longitude.ToZodiacHouse() % 2;
		var         off = Longitude.ToZodiacHouseOffset();
		ZodiacHouse dhouse;
		double      cuspLower  = 0;
		double      cuspHigher = 0;
		var         rulerIndex = 0;
		var         part        = 0;
		if (mod == 1)
		{
			if (off <= 5)
			{
				dhouse     = ZodiacHouse.Ari;
				cuspLower  = 0.0;
				cuspHigher = 5.0;
				rulerIndex = 1;
				part       = 1;
			}
			else if (off <= 10)
			{
				dhouse     = ZodiacHouse.Aqu;
				cuspLower  = 5.01;
				cuspHigher = 10.0;
				rulerIndex = 2;
				part       = 2;
			}
			else if (off <= 18)
			{
				dhouse     = ZodiacHouse.Sag;
				cuspLower  = 10.01;
				cuspHigher = 18.0;
				rulerIndex = 3;
				part       = 3;
			}
			else if (off <= 25)
			{
				dhouse     = ZodiacHouse.Gem;
				cuspLower  = 18.01;
				cuspHigher = 25.0;
				rulerIndex = 4;
				part       = 4;
			}
			else
			{
				dhouse     = ZodiacHouse.Lib;
				cuspLower  = 25.01;
				cuspHigher = 30.0;
				rulerIndex = 5;
				part       = 5;
			}
		}
		else
		{
			if (off <= 5)
			{
				dhouse     = ZodiacHouse.Tau;
				cuspLower  = 0.0;
				cuspHigher = 5.0;
				rulerIndex = 5;
				part       = 1;
			}
			else if (off <= 12)
			{
				dhouse     = ZodiacHouse.Vir;
				cuspLower  = 5.01;
				cuspHigher = 12.0;
				rulerIndex = 4;
				part       = 2;
			}
			else if (off <= 20)
			{
				dhouse     = ZodiacHouse.Pis;
				cuspLower  = 12.01;
				cuspHigher = 20.0;
				rulerIndex = 3;
				part       = 3;
			}
			else if (off <= 25)
			{
				dhouse     = ZodiacHouse.Cap;
				cuspLower  = 20.01;
				cuspHigher = 25.0;
				rulerIndex = 2;
				part       = 4;
			}
			else
			{
				dhouse     = ZodiacHouse.Sco;
				cuspLower  = 25.01;
				cuspHigher = 30.0;
				rulerIndex = 1;
				part       = 5;
			}
		}

		cuspLower  += Longitude.ToZodiacHouseBase();
		cuspHigher += Longitude.ToZodiacHouseBase();

		var dp = new DivisionPosition(Name, Type, dhouse, cuspLower, cuspHigher, 0);
		dp.Longitude   = dhouse.DivisionalLongitude(Longitude, 30);
		dp.RulerIndex = rulerIndex;
		dp.Part        = part;
		return dp;
	}

	private DivisionPosition ToDivisionPositionKhavedamsa()
	{
		var mod        = (int) Longitude.ToZodiacHouse() % 2;
		var dhousename = mod % 2 == 1 ? ZodiacHouse.Ari : ZodiacHouse.Lib;
		var part       = PartOfZodiacHouse(40);
		var dhouse     = dhousename.Add(part);
		var dp         = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude   = dhouse.DivisionalLongitude(Longitude, 40);
		dp.RulerIndex = part.NormalizeInc(1, 12);
		return PopulateRegularCusps(40, dp);
	}

	private DivisionPosition ToDivisionPositionAkshavedamsa(int n)
	{
		var              mod = (int) Longitude.ToZodiacHouse() % 3;
		ZodiacHouse dhousename;
		switch (mod)
		{
			case 1:
				dhousename = ZodiacHouse.Ari;
				break;
			case 2:
				dhousename = ZodiacHouse.Leo;
				break;
			default:
				dhousename = ZodiacHouse.Sag;
				break;
		}

		var part   = PartOfZodiacHouse(n);
		var dhouse = dhousename.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);
		if (n == 45)
		{
			switch ((int) Longitude.ToZodiacHouse() % 3)
			{
				case 1:
					dp.RulerIndex = part;
					break;
				case 2:
					dp.RulerIndex = part + 1;
					break;
				case 0:
					dp.RulerIndex = part + 2;
					break;
			}

			dp.RulerIndex = dp.RulerIndex.NormalizeInc(1, 3);
		}

		return PopulateRegularCusps(n, dp);
	}

	private DivisionPosition ToDivisionPositionShashtyamsa()
	{
		var zhouse = Longitude.ToZodiacHouse();
		var part   = PartOfZodiacHouse(60);
		var dhouse = zhouse.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, 60);
		if (Longitude.ToZodiacHouse().IsOdd())
		{
			dp.RulerIndex = part;
		}
		else
		{
			dp.RulerIndex = 61 - part;
		}

		return PopulateRegularCusps(60, dp);
	}

	private DivisionPosition ToDivisionPositionNadiamsa()
	{
#if DND
			Rasis.Rasi zhouse = m_lon.toZodiacHouse();
			int part = partOfZodiacHouse(150);
			Rasis.Rasi dhouse = null;
			switch ((int)zhouse.value % 3)
			{
				case 1:	dhouse = zhouse.add(part); break;
				case 2:	dhouse = zhouse.addReverse(part); break;
				default:
				case 0:
					dhouse = zhouse.add(part-75); break;
			}
			DivisionPosition dp = new DivisionPosition (name, type, dhouse, 0, 0, 0);
#endif
		var zhouse = Longitude.ToZodiacHouse();
		var part   = PartOfZodiacHouse(150);
		var dhouse = zhouse.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, 150);
		switch ((int) Longitude.ToZodiacHouse() % 3)
		{
			case 1:
				dp.RulerIndex = part;
				break;
			case 2:
				dp.RulerIndex = 151 - part;
				break;
			case 0:
				dp.RulerIndex = (75 + part).NormalizeInc(1, 150);
				break;
		}

		return PopulateRegularCusps(150, dp);
	}

	private void CalculateNadiamsaCusps()
	{
		if (_mbNadiamsaCknCalculated)
		{
			return;
		}

		int[] bases =
		{
			1,
			2,
			3,
			4,
			7,
			9,
			10,
			12,
			16,
			20,
			24,
			27,
			30,
			40,
			45,
			60
		};
		var alUnsorted = new ArrayList(150);
		foreach (var iVarga in bases)
		{
			for (var i = 0; i < iVarga; i++)
			{
				alUnsorted.Add(i / (double) iVarga * 30.0);
			}
		}

		alUnsorted.Add(30.0);
		alUnsorted.Sort();
		var alSorted = new ArrayList(150);

		alSorted.Add(0.0);
		for (var i = 0; i < alUnsorted.Count; i++)
		{
			if ((double) alUnsorted[i] != (double) alSorted[alSorted.Count - 1])
			{
				alSorted.Add(alUnsorted[i]);
			}
		}

		Debug.Assert(alSorted.Count == 151, string.Format("Found {0} Nadis. Expected 151.", alSorted.Count));

		_mNadiamsaCusps          = (double[]) alSorted.ToArray(typeof(double));
		_mbNadiamsaCknCalculated = true;
	}

	private DivisionPosition ToDivisionPositionNadiamsaCkn()
	{
		CalculateNadiamsaCusps();
		var part = PartOfZodiacHouse(150) - 10;
		if (part < 0)
		{
			part = 0;
		}

		for (; part < 149; part++)
		{
			var lonLow  = new Longitude(_mNadiamsaCusps[part]);
			var lonHigh = new Longitude(_mNadiamsaCusps[part + 1]);
			var offset  = new Longitude(Longitude.ToZodiacHouseOffset());

			if (offset.Sub(lonLow).Value <= lonHigh.Sub(lonLow).Value)
			{
				break;
			}
		}

		part++;

#if DND
			Rasis.Rasi zhouse = m_lon.toZodiacHouse();
			Rasis.Rasi dhouse = null;
			switch ((int)zhouse.value % 3)
			{
				case 1:	dhouse = zhouse.add(part); break;
				case 2:	dhouse = zhouse.addReverse(part); break;
				default:
				case 0:
					dhouse = zhouse.add(part-75); break;
			}
			DivisionPosition dp = new DivisionPosition (name, type, dhouse, 0, 0, 0);
#endif

		var zhouse = Longitude.ToZodiacHouse();
		var dhouse = zhouse.Add(part);
		var dp     = new DivisionPosition(Name, Type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(Longitude, 150);

		switch ((int) Longitude.ToZodiacHouse() % 3)
		{
			case 1:
				dp.RulerIndex = part;
				break;
			case 2:
				dp.RulerIndex = 151 - part;
				break;
			case 0:
				dp.RulerIndex = (75 + part).NormalizeInc(1, 150);
				break;
		}

		dp.CuspLower  = Longitude.ToZodiacHouseBase() + _mNadiamsaCusps[part - 1];
		dp.CuspHigher = Longitude.ToZodiacHouseBase() + _mNadiamsaCusps[part];
		dp.Part        = part;
		return dp;
	}

	private DivisionPosition ToDivisionPositionNavamsaDwadasamsa()
	{
		var bp = (Position) Clone();
		bp.Longitude = bp.ExtrapolateLongitude(new Division(Vargas.DivisionType.Navamsa));
		var dp = bp.ToDivisionPositionDwadasamsa(12);
		PopulateRegularCusps(108, dp);
		return dp;
	}

	private DivisionPosition ToDivisionPositionDwadasamsaDwadasamsa()
	{
		var bp = (Position) Clone();
		bp.Longitude = bp.ExtrapolateLongitude(new Division(Vargas.DivisionType.Dwadasamsa));
		var dp = bp.ToDivisionPositionDwadasamsa(12);
		PopulateRegularCusps(144, dp);
		return dp;
	}

	/// <summary>
	///     Calculated any known Vargas positions. Simply calls the appropriate
	///     helper function
	/// </summary>
	/// <param name="dtype">The requested DivisionType</param>
	/// <returns>A division Position</returns>
	public DivisionPosition ToDivisionPosition(Division d)
	{
		var              bp = (Position) Clone();
		DivisionPosition dp = null;

		foreach (var division in d.MultipleDivisions)
		{
			dp           = bp.ToDivisionPosition(division);
			bp.Longitude = bp.ExtrapolateLongitude(division);
		}
		return dp;
	}

	public DivisionPosition ToDivisionPosition(Division.SingleDivision d)
	{
		if (d.NumParts < 1)
		{
			d.NumParts = 1;
		}

		switch (d.Varga)
		{
			case Vargas.DivisionType.Rasi:                    return ToRegularDivisionPosition(1);
			case Vargas.DivisionType.BhavaPada:               return ToDivisionPositionBhavaPada();
			case Vargas.DivisionType.BhavaEqual:              return ToDivisionPositionBhavaEqual();
			case Vargas.DivisionType.BhavaSripati:            return ToDivisionPositionBhavaHelper('O');
			case Vargas.DivisionType.BhavaKoch:               return ToDivisionPositionBhavaHelper('K');
			case Vargas.DivisionType.BhavaPlacidus:           return ToDivisionPositionBhavaHelper('P');
			case Vargas.DivisionType.BhavaCampanus:           return ToDivisionPositionBhavaHelper('C');
			case Vargas.DivisionType.BhavaRegiomontanus:      return ToDivisionPositionBhavaHelper('R');
			case Vargas.DivisionType.BhavaAlcabitus:          return ToDivisionPositionBhavaHelper('B');
			case Vargas.DivisionType.BhavaAxial:              return ToDivisionPositionBhavaHelper('X');
			case Vargas.DivisionType.HoraParivrittiDwaya:     return ToRegularDivisionPosition(2);
			case Vargas.DivisionType.HoraKashinath:           return ToDivisionPositionHoraKashinath();
			case Vargas.DivisionType.HoraParasara:            return ToDivisionPositionHoraParasara();
			case Vargas.DivisionType.HoraJagannath:           return ToDivisionPositionHoraJagannath();
			case Vargas.DivisionType.DrekkanaParasara:        return ToDivisionPositionDrekanna(3);
			case Vargas.DivisionType.DrekkanaJagannath:       return ToDivisionPositionDrekannaJagannath();
			case Vargas.DivisionType.DrekkanaParivrittitraya: return ToRegularDivisionPosition(3);
			case Vargas.DivisionType.DrekkanaSomnath:         return ToDivisionPositionDrekkanaSomnath();
			case Vargas.DivisionType.Chaturthamsa:            return ToDivisionPositionChaturthamsa(4);
			case Vargas.DivisionType.Panchamsa:               return ToDivisionPositionPanchamsa();
			case Vargas.DivisionType.Shashthamsa:             return ToDivisionPositionShashthamsa(6);
			case Vargas.DivisionType.Saptamsa:                return ToDivisionPositionSaptamsa(7);
			case Vargas.DivisionType.Ashtamsa:                return ToRegularDivisionPosition(8);
			case Vargas.DivisionType.AshtamsaRaman:           return ToDivisionPositionAshtamsaRaman();
			case Vargas.DivisionType.Navamsa:                 return ToDivisionPositionNavamsa();
			case Vargas.DivisionType.Dasamsa:                 return ToDivisionPositionDasamsa(10);
			case Vargas.DivisionType.Rudramsa:                return ToDivisionPositionRudramsa();
			case Vargas.DivisionType.RudramsaRaman:           return ToDivisionPositionRudramsaRaman();
			case Vargas.DivisionType.Dwadasamsa:              return ToDivisionPositionDwadasamsa(12);
			case Vargas.DivisionType.Shodasamsa:              return ToDivisionPositionShodasamsa();
			case Vargas.DivisionType.Vimsamsa:                return ToDivisionPositionVimsamsa();
			case Vargas.DivisionType.Chaturvimsamsa:          return ToDivisionPositionChaturvimsamsa(24);
			case Vargas.DivisionType.Nakshatramsa:            return ToDivisionPositionNakshatramsa();
			case Vargas.DivisionType.Trimsamsa:               return ToDivisionPositionTrimsamsa();
			case Vargas.DivisionType.TrimsamsaParivritti:     return ToRegularDivisionPosition(30);
			case Vargas.DivisionType.TrimsamsaSimple:         return ToDivisionPositionTrimsamsaSimple();
			case Vargas.DivisionType.Khavedamsa:              return ToDivisionPositionKhavedamsa();
			case Vargas.DivisionType.Akshavedamsa:            return ToDivisionPositionAkshavedamsa(45);
			case Vargas.DivisionType.Shashtyamsa:             return ToDivisionPositionShashtyamsa();
			case Vargas.DivisionType.Ashtottaramsa:           return ToRegularDivisionPosition(108);
			case Vargas.DivisionType.Nadiamsa:                return ToDivisionPositionNadiamsa();
			case Vargas.DivisionType.NadiamsaCKN:             return ToDivisionPositionNadiamsaCkn();
			case Vargas.DivisionType.NavamsaDwadasamsa:       return ToDivisionPositionNavamsaDwadasamsa();
			case Vargas.DivisionType.DwadasamsaDwadasamsa:    return ToDivisionPositionDwadasamsaDwadasamsa();
			case Vargas.DivisionType.GenericParivritti:       return ToRegularDivisionPosition(d.NumParts);
			case Vargas.DivisionType.GenericShashthamsa:      return ToDivisionPositionShashthamsa(d.NumParts);
			case Vargas.DivisionType.GenericSaptamsa:         return ToDivisionPositionSaptamsa(d.NumParts);
			case Vargas.DivisionType.GenericDasamsa:          return ToDivisionPositionDasamsa(d.NumParts);
			case Vargas.DivisionType.GenericDwadasamsa:       return ToDivisionPositionDwadasamsa(d.NumParts);
			case Vargas.DivisionType.GenericChaturvimsamsa:   return ToDivisionPositionChaturvimsamsa(d.NumParts);
			case Vargas.DivisionType.GenericChaturthamsa:     return ToDivisionPositionChaturthamsa(d.NumParts);
			case Vargas.DivisionType.GenericNakshatramsa:     return ToDivisionPositionNakshatramsa(d.NumParts);
			case Vargas.DivisionType.GenericDrekkana:         return ToDivisionPositionDrekanna(d.NumParts);
			case Vargas.DivisionType.GenericShodasamsa:       return ToDivisionPositionAkshavedamsa(d.NumParts);
			case Vargas.DivisionType.GenericVimsamsa:         return ToDivisionPositionVimsamsa(d.NumParts);
		}

		Trace.Assert(false, "DivisionPosition Error");
		return new DivisionPosition(Name, Type, ZodiacHouse.Ari, 0, 0, 0);
	}

	public Longitude ExtrapolateLongitude(Division d)
	{
		var bp = (Position) Clone();
		foreach (var dSingle in d.MultipleDivisions)
		{
			bp.Longitude = ExtrapolateLongitude(dSingle);
		}

		return bp.Longitude;
	}

	public Longitude ExtrapolateLongitude(Division.SingleDivision d)
	{
		var dp      = ToDivisionPosition(d);
		var lOffset = Longitude.Sub(dp.CuspLower);
		var lRange  = new Longitude(dp.CuspHigher).Sub(dp.CuspLower);
		Trace.Assert(lOffset.Value <= lRange.Value, "Extrapolation internal error: Slice smaller than range. Weird.");

		var newOffset = lOffset.Value / lRange.Value      * 30.0;
		var newBase   = ((int) dp.ZodiacHouse - 1) * 30.0;
		return new Longitude(newOffset + newBase);
	}
}