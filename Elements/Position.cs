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

namespace Mhora.Elements;

/// <summary>
///     Specifies a BodyPosition, i.e. the astronomical characteristics of a body like
///     longitude, speed etc. It has no notion of its "rasi".
///     The functions to convert this to a DivisionType (the various vargas)
///     are all implemented here
/// </summary>
public class Position : ICloneable
{
	private static bool      mbNadiamsaCKNCalculated;
	private static double[]  mNadiamsaCusps;
	public         Horoscope h;
	public         Body.Name name;
	public         string    otherString;
	public         Body.Type type;

	public Position(Horoscope _h, Body.Name aname, Body.Type atype, Longitude lon, double lat, double dist, double splon, double splat, double spdist)
	{
		longitude       = lon;
		latitude        = lat;
		distance        = dist;
		speed_longitude = splon;
		speed_latitude  = splat;
		speed_distance  = spdist;
		name            = aname;
		type            = atype;
		h               = _h;
		//mhora.Log.Debug ("{0} {1} {2}", aname.ToString(), lon.value, splon);
	}

	public Longitude longitude
	{
		get;
		set;
	}

	public double latitude
	{
		get;
		set;
	}

	public double distance
	{
		get;
		set;
	}

	public double speed_longitude
	{
		get;
		set;
	}

	public double speed_latitude
	{
		get;
		set;
	}

	public double speed_distance
	{
		get;
		set;
	}

	public object Clone()
	{
		var bp = new Position(h, name, type, longitude.add(0), latitude, distance, speed_longitude, speed_latitude, speed_distance);
		bp.otherString = otherString;
		return bp;
	}

	public int partOfZodiacHouse(int n)
	{
		var l      = longitude;
		var offset = l.toZodiacHouseOffset();
		var part   = (int) Math.Floor(offset / (30.0 / n)) + 1;
		Trace.Assert(part >= 1 && part <= n);
		return part;
	}

	private DivisionPosition populateRegularCusps(int n, DivisionPosition dp)
	{
		var part        = partOfZodiacHouse(n);
		var cusp_length = 30.0 / n;
		dp.cusp_lower  =  (part - 1) * cusp_length;
		dp.cusp_lower  += longitude.toZodiacHouseBase();
		dp.cusp_higher =  dp.cusp_lower + cusp_length;
		dp.part        =  part;

		//if (dp.type == BodyType.Type.Graha || dp.type == BodyType.Type.Lagna)
		//mhora.Log.Debug ("D: {0} {1} {2} {3} {4} {5}", 
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
	private DivisionPosition toRegularDivisionPosition(int n)
	{
		var zhouse    = (int) longitude.toZodiacHouse().value;
		var num_parts = (zhouse - 1) * n + partOfZodiacHouse(n);
		var div_house = new ZodiacHouse(ZodiacHouse.Name.Ari).add(num_parts);
		var dp        = new DivisionPosition(name, type, div_house, 0, 0, 0);
		populateRegularCusps(n, dp);

		if (n > 1)
		{
			dp.Longitude = div_house.DivisionalLongitude(longitude, n);
		}
		else
		{
			dp.Longitude = longitude;
		}

		return dp;
	}

	private DivisionPosition toRegularDivisionPositionFromCurrentHouseOddEven(int n)
	{
		var zhouse    = (int) longitude.toZodiacHouse().value;
		var num_parts = partOfZodiacHouse(n);
		var div_house = longitude.toZodiacHouse().add(num_parts);
		var dp        = new DivisionPosition(name, type, div_house, 0, 0, 0);
		dp.Longitude = div_house.DivisionalLongitude(longitude, n);
		populateRegularCusps(n, dp);
		return dp;
	}


	private DivisionPosition toBhavaDivisionPositionRasi(Longitude[] cusps)
	{
		Debug.Assert(cusps.Length == 13);
		cusps[12] = cusps[0];
		for (var i = 0; i < 12; i++)
		{
			if (longitude.sub(cusps[i]).value <= cusps[i + 1].sub(cusps[i]).value)
			{
				return new DivisionPosition(name, type, new ZodiacHouse((ZodiacHouse.Name) i + 1), cusps[i].value, cusps[i + 1].value, 1);
			}
		}

		throw new Exception();
	}

	private DivisionPosition toBhavaDivisionPositionHouse(Longitude[] cusps)
	{
		Debug.Assert(cusps.Length == 13);

		var zlagna = h.getPosition(Body.Name.Lagna).toDivisionPosition(new Division(Basics.DivisionType.Rasi)).zodiac_house;
		for (var i = 0; i < 12; i++)
		{
			if (longitude.sub(cusps[i]).value < cusps[i + 1].sub(cusps[i]).value)
			{
				//mhora.Log.Debug ("Found {4} - {0} in cusp {3} between {1} and {2}", this.m_lon.value,
				//	cusps[i].value, cusps[i+1].value, i+1, this.name.ToString());

				return new DivisionPosition(name, type, zlagna.add(i + 1), cusps[i].value, cusps[i + 1].value, 1);
			}
		}

		return new DivisionPosition(name, type, zlagna.add(1), cusps[0].value, cusps[1].value, 1);
	}

	private DivisionPosition toDivisionPositionBhavaEqual()
	{
		var offset = h.getPosition(Body.Name.Lagna).longitude.toZodiacHouseOffset();
		var cusps  = new Longitude[13];
		for (var i = 0; i < 12; i++)
		{
			cusps[i] = new Longitude(i * 30.0 + offset - 15.0);
		}

		return toBhavaDivisionPositionRasi(cusps);
	}

	private DivisionPosition toDivisionPositionBhavaPada()
	{
		var cusps       = new Longitude[13];
		var offset      = h.getPosition(Body.Name.Lagna).longitude.toZodiacHouseOffset();
		var padasOffset = (int) Math.Floor(offset / (360.0 / 108.0));
		var startOffset = padasOffset * (360.0 / 108.0);

		for (var i = 0; i < 12; i++)
		{
			cusps[i] = new Longitude(i * 30.0 + startOffset - 15.0);
		}

		return toBhavaDivisionPositionRasi(cusps);
	}

	private DivisionPosition toDivisionPositionBhavaHelper(int hsys)
	{
		var cusps  = new Longitude[13];
		var dCusps = new double[13];
		var ascmc  = new double[10];

		if (hsys != h.swephHouseSystem)
		{
			h.swephHouseSystem = hsys;
			h.populateHouseCusps();
		}

		return toBhavaDivisionPositionHouse(h.swephHouseCusps);
	}

	private bool HoraSunDayNight()
	{
		var sign = (int) longitude.toZodiacHouse().value;
		var part = partOfZodiacHouse(2);
		if (longitude.toZodiacHouse().isDaySign())
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
		var sign = (int) longitude.toZodiacHouse().value;
		var part = partOfZodiacHouse(2);
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

	private DivisionPosition toDivisionPositionHoraKashinath()
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
		var         sign = (int) longitude.toZodiacHouse().value;
		if (HoraSunOddEven())
		{
			zh = new ZodiacHouse((ZodiacHouse.Name) daySigns[sign]);
		}
		else
		{
			zh = new ZodiacHouse((ZodiacHouse.Name) nightSigns[sign]);
		}

		var dp = new DivisionPosition(name, type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(longitude, 2);
		populateRegularCusps(2, dp);
		return dp;
	}

	private DivisionPosition toDivisionPositionHoraJagannath()
	{
		var zh = longitude.toZodiacHouse();

		Application.Log.Debug("{2} in {3}: OddEven is {0}, DayNight is {1}", HoraSunOddEven(), HoraSunDayNight(), name, zh.value);

		if (HoraSunDayNight() && false == HoraSunOddEven())
		{
			zh = zh.add(7);
		}
		else if (false == HoraSunDayNight() && HoraSunOddEven())
		{
			zh = zh.add(7);
		}

		Application.Log.Debug("{0} ends in {1}", name, zh.value);

		var dp = new DivisionPosition(name, type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(longitude, 2);
		populateRegularCusps(2, dp);
		return dp;
	}

	private DivisionPosition toDivisionPositionHoraParasara()
	{
		ZodiacHouse zh;
		var         ruler_index = 0;
		if (HoraSunOddEven())
		{
			zh          = new ZodiacHouse(ZodiacHouse.Name.Leo);
			ruler_index = 1;
		}
		else
		{
			zh          = new ZodiacHouse(ZodiacHouse.Name.Can);
			ruler_index = 2;
		}

		var dp = new DivisionPosition(name, type, zh, 0, 0, 0);
		dp.Longitude   = zh.DivisionalLongitude(longitude, 2);
		dp.ruler_index = ruler_index;
		return populateRegularCusps(2, dp);
	}

	private DivisionPosition toDivisionPositionDrekanna(int n)
	{
		var offset = new int[4]
		{
			9,
			1,
			5,
			9
		};
		var zhouse = longitude.toZodiacHouse();
		var part   = partOfZodiacHouse(n);
		var dhouse = zhouse.add(offset[part % 3]);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, 3);
		populateRegularCusps(n, dp);
		if (n == 3)
		{
			var ruler_index = (int) dp.zodiac_house.value % 3;
			if (ruler_index == 0)
			{
				ruler_index = 3;
			}

			dp.ruler_index = ruler_index;
		}

		return dp;
	}

	private DivisionPosition toDivisionPositionDrekannaJagannath()
	{
		var         zh = longitude.toZodiacHouse();
		ZodiacHouse zhm;
		ZodiacHouse dhouse;
		var         mod = (int) longitude.toZodiacHouse().value % 3;
		// Find moveable sign in trines
		switch (mod)
		{
			case 1:
				zhm = zh.add(1);
				break;
			case 2:
				zhm = zh.add(9);
				break;
			default:
				zhm = zh.add(5);
				break;
		}

		// From moveable sign, 3 parts belong to the trines
		var part = partOfZodiacHouse(3);
		switch (part)
		{
			case 1:
				dhouse = zhm.add(1);
				break;
			case 2:
				dhouse = zhm.add(5);
				break;
			default:
				dhouse = zhm.add(9);
				break;
		}

		var dp = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, 3);
		return populateRegularCusps(3, dp);
	}

	private DivisionPosition toDivisionPositionDrekkanaSomnath()
	{
		var mod  = (int) longitude.toZodiacHouse().value % 2;
		var part = partOfZodiacHouse(3);
		var zh   = longitude.toZodiacHouse();
		var p    = (int) zh.value;

		if (mod == 0)
		{
			p--;
		}

		p = (p - 1) / 2;
		var num_done = p * 3;

		var         zh1 = new ZodiacHouse(ZodiacHouse.Name.Ari);
		ZodiacHouse zh2;
		if (mod == 1)
		{
			zh2 = zh1.add(num_done + part);
		}
		else
		{
			zh2 = zh1.addReverse(num_done + part + 1);
		}

		var dp = new DivisionPosition(name, type, zh2, 0, 0, 0);
		dp.Longitude = zh2.DivisionalLongitude(longitude, 3);
		return populateRegularCusps(3, dp);
	}

	private DivisionPosition toDivisionPositionChaturthamsa(int n)
	{
		var offset = new int[5]
		{
			10,
			1,
			4,
			7,
			10
		};
		var zhouse = longitude.toZodiacHouse();
		var part   = partOfZodiacHouse(n);
		var dhouse = zhouse.add(offset[part % 4]);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);
		if (n == 4)
		{
			dp.ruler_index = part;
		}

		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionShashthamsa(int n)
	{
		var mod     = (int) longitude.toZodiacHouse().value % 2;
		var dhousen = mod % 2 == 1 ? ZodiacHouse.Name.Ari : ZodiacHouse.Name.Lib;
		var dhouse  = new ZodiacHouse(dhousen).add(partOfZodiacHouse(n));
		var dp      = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);
		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionSaptamsa(int n)
	{
		var part = partOfZodiacHouse(n);
		var zh   = longitude.toZodiacHouse();
		if (false == zh.isOdd())
		{
			zh = zh.add(7);
		}

		zh = zh.add(part);
		var dp = new DivisionPosition(name, type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(longitude, n);

		if (n == 7)
		{
			if (longitude.toZodiacHouse().isOdd())
			{
				dp.ruler_index = part;
			}
			else
			{
				dp.ruler_index = 8 - part;
			}
		}

		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionNavamsa()
	{
		var part = partOfZodiacHouse(9);
		var dp   = toRegularDivisionPosition(9);
		switch ((int) longitude.toZodiacHouse().value % 3)
		{
			case 1:
				dp.ruler_index = part;
				break;
			case 2:
				dp.ruler_index = part + 1;
				break;
			case 0:
				dp.ruler_index = part + 2;
				break;
		}

		while (dp.ruler_index > 3)
		{
			dp.ruler_index -= 3;
		}

		return dp;
	}

	private DivisionPosition toDivisionPositionAshtamsaRaman()
	{
		ZodiacHouse zstart = null;
		switch ((int) longitude.toZodiacHouse().value % 3)
		{
			case 1:
				zstart = new ZodiacHouse(ZodiacHouse.Name.Ari);
				break;
			case 2:
				zstart = new ZodiacHouse(ZodiacHouse.Name.Leo);
				break;
			case 0:
			default:
				zstart = new ZodiacHouse(ZodiacHouse.Name.Sag);
				break;
		}

		var dhouse = zstart.add(partOfZodiacHouse(8));
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, 8);
		return populateRegularCusps(8, dp);
	}

	private DivisionPosition toDivisionPositionPanchamsa()
	{
		var offset_odd = new ZodiacHouse.Name[5]
		{
			ZodiacHouse.Name.Ari,
			ZodiacHouse.Name.Aqu,
			ZodiacHouse.Name.Sag,
			ZodiacHouse.Name.Gem,
			ZodiacHouse.Name.Lib
		};
		var offset_even = new ZodiacHouse.Name[5]
		{
			ZodiacHouse.Name.Tau,
			ZodiacHouse.Name.Vir,
			ZodiacHouse.Name.Pis,
			ZodiacHouse.Name.Cap,
			ZodiacHouse.Name.Sco
		};
		var part   = partOfZodiacHouse(5);
		var mod    = (int) longitude.toZodiacHouse().value % 2;
		var dhouse = mod % 2 == 1 ? offset_odd[part - 1] : offset_even[part - 1];
		var zh     = new ZodiacHouse(dhouse);
		var dp     = new DivisionPosition(name, type, zh, 0, 0, 0);
		dp.Longitude = zh.DivisionalLongitude(longitude, 5);
		return populateRegularCusps(5, dp);
	}

	private DivisionPosition toDivisionPositionRudramsa()
	{
		var zari   = new ZodiacHouse(ZodiacHouse.Name.Ari);
		var zhouse = longitude.toZodiacHouse();
		var diff   = zari.numHousesBetween(zhouse);
		var zstart = zari.addReverse(diff);
		var part   = partOfZodiacHouse(11);
		var zend   = zstart.add(part);
		var dp     = new DivisionPosition(name, type, zend, 0, 0, 0);
		dp.Longitude = zend.DivisionalLongitude(longitude, 11);
		return populateRegularCusps(11, dp);
	}

	private DivisionPosition toDivisionPositionRudramsaRaman()
	{
		var zhstart = longitude.toZodiacHouse().add(12);
		var part    = partOfZodiacHouse(11);
		var zend    = zhstart.addReverse(part);
		var dp      = new DivisionPosition(name, type, zend, 0, 0, 0);
		dp.Longitude = zend.DivisionalLongitude(longitude, 11);
		return populateRegularCusps(11, dp);
	}

	private DivisionPosition toDivisionPositionDasamsa(int n)
	{
		var offset = new int[2]
		{
			9,
			1
		};
		var zhouse = longitude.toZodiacHouse();
		var dhouse = zhouse.add(offset[(int) zhouse.value % 2]);
		var part   = partOfZodiacHouse(n);
		dhouse = dhouse.add(part);
		var dp = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		if (n == 10)
		{
			if (longitude.toZodiacHouse().isOdd())
			{
				dp.ruler_index = part;
			}
			else
			{
				dp.ruler_index = 11 - part;
			}
		}

		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);
		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionDwadasamsa(int n)
	{
		var zhouse = longitude.toZodiacHouse();
		var part   = partOfZodiacHouse(n);
		var dhouse = zhouse.add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		if (n == 12)
		{
			dp.ruler_index = Basics.normalize_inc(1, 4, part);
		}

		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);

		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionShodasamsa()
	{
		var part  = partOfZodiacHouse(16);
		var dp    = toRegularDivisionPosition(16);
		var ruler = part;
		if (longitude.toZodiacHouse().isOdd())
		{
			ruler = part;
		}
		else
		{
			ruler = 17 - part;
		}

		dp.ruler_index = Basics.normalize_inc(1, 4, ruler);
		return dp;
	}

	private DivisionPosition toDivisionPositionVimsamsa(int n)
	{
		var              mod = (int) longitude.toZodiacHouse().value % 3;
		ZodiacHouse.Name dhousename;
		switch (mod)
		{
			case 1:
				dhousename = ZodiacHouse.Name.Ari;
				break;
			case 2:
				dhousename = ZodiacHouse.Name.Sag;
				break;
			default:
				dhousename = ZodiacHouse.Name.Leo;
				break;
		}

		var part   = partOfZodiacHouse(n);
		var dhouse = new ZodiacHouse(dhousename).add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);
		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionVimsamsa()
	{
		var part = partOfZodiacHouse(20);
		var dp   = toRegularDivisionPosition(20);
		if (longitude.toZodiacHouse().isOdd())
		{
			dp.ruler_index = part;
		}
		else
		{
			dp.ruler_index = 20 + part;
		}

		return dp;
	}

	private DivisionPosition toDivisionPositionChaturvimsamsa(int n)
	{
		var mod        = (int) longitude.toZodiacHouse().value % 2;
		var dhousename = mod % 2 == 1 ? ZodiacHouse.Name.Leo : ZodiacHouse.Name.Can;
		var part       = partOfZodiacHouse(n);
		var dhouse     = new ZodiacHouse(dhousename).add(part);
		var dp         = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);
		if (n == 24)
		{
			if (longitude.toZodiacHouse().isOdd())
			{
				dp.ruler_index = part;
			}
			else
			{
				dp.ruler_index = 25 - part;
			}

			dp.ruler_index = Basics.normalize_inc(1, 12, dp.ruler_index);
		}

		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionNakshatramsa(int n)
	{
		var              mod = (int) longitude.toZodiacHouse().value % 4;
		ZodiacHouse.Name dhousename;
		switch (mod)
		{
			case 1:
				dhousename = ZodiacHouse.Name.Ari;
				break;
			case 2:
				dhousename = ZodiacHouse.Name.Can;
				break;
			case 3:
				dhousename = ZodiacHouse.Name.Lib;
				break;
			default:
				dhousename = ZodiacHouse.Name.Cap;
				break;
		}

		var part   = partOfZodiacHouse(n);
		var dhouse = new ZodiacHouse(dhousename).add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);
		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionNakshatramsa()
	{
		var dp = toRegularDivisionPosition(27);
		dp.ruler_index = partOfZodiacHouse(27);
		return dp;
	}

	private DivisionPosition toDivisionPositionTrimsamsaSimple()
	{
		var zhouse = longitude.toZodiacHouse();
		var part   = partOfZodiacHouse(30);
		var dhouse = zhouse.add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, 30);
		return populateRegularCusps(30, dp);
	}

	private DivisionPosition toDivisionPositionTrimsamsa()
	{
		var         mod = (int) longitude.toZodiacHouse().value % 2;
		var         off = longitude.toZodiacHouseOffset();
		ZodiacHouse dhouse;
		double      cusp_lower  = 0;
		double      cusp_higher = 0;
		var         ruler_index = 0;
		var         part        = 0;
		if (mod == 1)
		{
			if (off <= 5)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Ari);
				cusp_lower  = 0.0;
				cusp_higher = 5.0;
				ruler_index = 1;
				part        = 1;
			}
			else if (off <= 10)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Aqu);
				cusp_lower  = 5.01;
				cusp_higher = 10.0;
				ruler_index = 2;
				part        = 2;
			}
			else if (off <= 18)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Sag);
				cusp_lower  = 10.01;
				cusp_higher = 18.0;
				ruler_index = 3;
				part        = 3;
			}
			else if (off <= 25)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Gem);
				cusp_lower  = 18.01;
				cusp_higher = 25.0;
				ruler_index = 4;
				part        = 4;
			}
			else
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Lib);
				cusp_lower  = 25.01;
				cusp_higher = 30.0;
				ruler_index = 5;
				part        = 5;
			}
		}
		else
		{
			if (off <= 5)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Tau);
				cusp_lower  = 0.0;
				cusp_higher = 5.0;
				ruler_index = 5;
				part        = 1;
			}
			else if (off <= 12)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Vir);
				cusp_lower  = 5.01;
				cusp_higher = 12.0;
				ruler_index = 4;
				part        = 2;
			}
			else if (off <= 20)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Pis);
				cusp_lower  = 12.01;
				cusp_higher = 20.0;
				ruler_index = 3;
				part        = 3;
			}
			else if (off <= 25)
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Cap);
				cusp_lower  = 20.01;
				cusp_higher = 25.0;
				ruler_index = 2;
				part        = 4;
			}
			else
			{
				dhouse      = new ZodiacHouse(ZodiacHouse.Name.Sco);
				cusp_lower  = 25.01;
				cusp_higher = 30.0;
				ruler_index = 1;
				part        = 5;
			}
		}

		cusp_lower  += longitude.toZodiacHouseBase();
		cusp_higher += longitude.toZodiacHouseBase();

		var dp = new DivisionPosition(name, type, dhouse, cusp_lower, cusp_higher, 0);
		dp.Longitude   = dhouse.DivisionalLongitude(longitude, 30);
		dp.ruler_index = ruler_index;
		dp.part        = part;
		return dp;
	}

	private DivisionPosition toDivisionPositionKhavedamsa()
	{
		var mod        = (int) longitude.toZodiacHouse().value % 2;
		var dhousename = mod % 2 == 1 ? ZodiacHouse.Name.Ari : ZodiacHouse.Name.Lib;
		var part       = partOfZodiacHouse(40);
		var dhouse     = new ZodiacHouse(dhousename).add(part);
		var dp         = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude   = dhouse.DivisionalLongitude(longitude, 40);
		dp.ruler_index = Basics.normalize_inc(1, 12, part);
		return populateRegularCusps(40, dp);
	}

	private DivisionPosition toDivisionPositionAkshavedamsa(int n)
	{
		var              mod = (int) longitude.toZodiacHouse().value % 3;
		ZodiacHouse.Name dhousename;
		switch (mod)
		{
			case 1:
				dhousename = ZodiacHouse.Name.Ari;
				break;
			case 2:
				dhousename = ZodiacHouse.Name.Leo;
				break;
			default:
				dhousename = ZodiacHouse.Name.Sag;
				break;
		}

		var part   = partOfZodiacHouse(n);
		var dhouse = new ZodiacHouse(dhousename).add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, n);
		if (n == 45)
		{
			switch ((int) longitude.toZodiacHouse().value % 3)
			{
				case 1:
					dp.ruler_index = part;
					break;
				case 2:
					dp.ruler_index = part + 1;
					break;
				case 0:
					dp.ruler_index = part + 2;
					break;
			}

			dp.ruler_index = Basics.normalize_inc(1, 3, dp.ruler_index);
		}

		return populateRegularCusps(n, dp);
	}

	private DivisionPosition toDivisionPositionShashtyamsa()
	{
		var zhouse = longitude.toZodiacHouse();
		var part   = partOfZodiacHouse(60);
		var dhouse = zhouse.add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, 60);
		if (longitude.toZodiacHouse().isOdd())
		{
			dp.ruler_index = part;
		}
		else
		{
			dp.ruler_index = 61 - part;
		}

		return populateRegularCusps(60, dp);
	}

	private DivisionPosition toDivisionPositionNadiamsa()
	{
#if DND
			ZodiacHouse zhouse = m_lon.toZodiacHouse();
			int part = partOfZodiacHouse(150);
			ZodiacHouse dhouse = null;
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
		var zhouse = longitude.toZodiacHouse();
		var part   = partOfZodiacHouse(150);
		var dhouse = zhouse.add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, 150);
		switch ((int) longitude.toZodiacHouse().value % 3)
		{
			case 1:
				dp.ruler_index = part;
				break;
			case 2:
				dp.ruler_index = 151 - part;
				break;
			case 0:
				dp.ruler_index = Basics.normalize_inc(1, 150, 75 + part);
				break;
		}

		return populateRegularCusps(150, dp);
	}

	private void calculateNadiamsaCusps()
	{
		if (mbNadiamsaCKNCalculated)
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

		mNadiamsaCusps          = (double[]) alSorted.ToArray(typeof(double));
		mbNadiamsaCKNCalculated = true;
	}

	private DivisionPosition toDivisionPositionNadiamsaCKN()
	{
		calculateNadiamsaCusps();
		var part = partOfZodiacHouse(150) - 10;
		if (part < 0)
		{
			part = 0;
		}

		for (; part < 149; part++)
		{
			var lonLow  = new Longitude(mNadiamsaCusps[part]);
			var lonHigh = new Longitude(mNadiamsaCusps[part + 1]);
			var offset  = new Longitude(longitude.toZodiacHouseOffset());

			if (offset.sub(lonLow).value <= lonHigh.sub(lonLow).value)
			{
				break;
			}
		}

		part++;

#if DND
			ZodiacHouse zhouse = m_lon.toZodiacHouse();
			ZodiacHouse dhouse = null;
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

		var zhouse = longitude.toZodiacHouse();
		var dhouse = zhouse.add(part);
		var dp     = new DivisionPosition(name, type, dhouse, 0, 0, 0);
		dp.Longitude = dhouse.DivisionalLongitude(longitude, 150);

		switch ((int) longitude.toZodiacHouse().value % 3)
		{
			case 1:
				dp.ruler_index = part;
				break;
			case 2:
				dp.ruler_index = 151 - part;
				break;
			case 0:
				dp.ruler_index = Basics.normalize_inc(1, 150, 75 + part);
				break;
		}

		dp.cusp_lower  = longitude.toZodiacHouseBase() + mNadiamsaCusps[part - 1];
		dp.cusp_higher = longitude.toZodiacHouseBase() + mNadiamsaCusps[part];
		dp.part        = part;
		return dp;
	}

	private DivisionPosition toDivisionPositionNavamsaDwadasamsa()
	{
		var bp = (Position) Clone();
		bp.longitude = bp.extrapolateLongitude(new Division(Basics.DivisionType.Navamsa));
		var dp = bp.toDivisionPositionDwadasamsa(12);
		populateRegularCusps(108, dp);
		return dp;
	}

	private DivisionPosition toDivisionPositionDwadasamsaDwadasamsa()
	{
		var bp = (Position) Clone();
		bp.longitude = bp.extrapolateLongitude(new Division(Basics.DivisionType.Dwadasamsa));
		var dp = bp.toDivisionPositionDwadasamsa(12);
		populateRegularCusps(144, dp);
		return dp;
	}

	/// <summary>
	///     Calculated any known Varga positions. Simply calls the appropriate
	///     helper function
	/// </summary>
	/// <param name="dtype">The requested DivisionType</param>
	/// <returns>A division Position</returns>
	public DivisionPosition toDivisionPosition(Division d)
	{
		var              bp = (Position) Clone();
		DivisionPosition dp = null;
		for (var i = 0; i < d.MultipleDivisions.Length; i++)
		{
			dp           = bp.toDivisionPosition(d.MultipleDivisions[i]);
			bp.longitude = bp.extrapolateLongitude(d.MultipleDivisions[i]);
		}

		return dp;
	}

	public DivisionPosition toDivisionPosition(Division.SingleDivision d)
	{
		if (d.NumParts < 1)
		{
			d.NumParts = 1;
		}

		switch (d.Varga)
		{
			case Basics.DivisionType.Rasi:                    return toRegularDivisionPosition(1);
			case Basics.DivisionType.BhavaPada:               return toDivisionPositionBhavaPada();
			case Basics.DivisionType.BhavaEqual:              return toDivisionPositionBhavaEqual();
			case Basics.DivisionType.BhavaSripati:            return toDivisionPositionBhavaHelper('O');
			case Basics.DivisionType.BhavaKoch:               return toDivisionPositionBhavaHelper('K');
			case Basics.DivisionType.BhavaPlacidus:           return toDivisionPositionBhavaHelper('P');
			case Basics.DivisionType.BhavaCampanus:           return toDivisionPositionBhavaHelper('C');
			case Basics.DivisionType.BhavaRegiomontanus:      return toDivisionPositionBhavaHelper('R');
			case Basics.DivisionType.BhavaAlcabitus:          return toDivisionPositionBhavaHelper('B');
			case Basics.DivisionType.BhavaAxial:              return toDivisionPositionBhavaHelper('X');
			case Basics.DivisionType.HoraParivrittiDwaya:     return toRegularDivisionPosition(2);
			case Basics.DivisionType.HoraKashinath:           return toDivisionPositionHoraKashinath();
			case Basics.DivisionType.HoraParasara:            return toDivisionPositionHoraParasara();
			case Basics.DivisionType.HoraJagannath:           return toDivisionPositionHoraJagannath();
			case Basics.DivisionType.DrekkanaParasara:        return toDivisionPositionDrekanna(3);
			case Basics.DivisionType.DrekkanaJagannath:       return toDivisionPositionDrekannaJagannath();
			case Basics.DivisionType.DrekkanaParivrittitraya: return toRegularDivisionPosition(3);
			case Basics.DivisionType.DrekkanaSomnath:         return toDivisionPositionDrekkanaSomnath();
			case Basics.DivisionType.Chaturthamsa:            return toDivisionPositionChaturthamsa(4);
			case Basics.DivisionType.Panchamsa:               return toDivisionPositionPanchamsa();
			case Basics.DivisionType.Shashthamsa:             return toDivisionPositionShashthamsa(6);
			case Basics.DivisionType.Saptamsa:                return toDivisionPositionSaptamsa(7);
			case Basics.DivisionType.Ashtamsa:                return toRegularDivisionPosition(8);
			case Basics.DivisionType.AshtamsaRaman:           return toDivisionPositionAshtamsaRaman();
			case Basics.DivisionType.Navamsa:                 return toDivisionPositionNavamsa();
			case Basics.DivisionType.Dasamsa:                 return toDivisionPositionDasamsa(10);
			case Basics.DivisionType.Rudramsa:                return toDivisionPositionRudramsa();
			case Basics.DivisionType.RudramsaRaman:           return toDivisionPositionRudramsaRaman();
			case Basics.DivisionType.Dwadasamsa:              return toDivisionPositionDwadasamsa(12);
			case Basics.DivisionType.Shodasamsa:              return toDivisionPositionShodasamsa();
			case Basics.DivisionType.Vimsamsa:                return toDivisionPositionVimsamsa();
			case Basics.DivisionType.Chaturvimsamsa:          return toDivisionPositionChaturvimsamsa(24);
			case Basics.DivisionType.Nakshatramsa:            return toDivisionPositionNakshatramsa();
			case Basics.DivisionType.Trimsamsa:               return toDivisionPositionTrimsamsa();
			case Basics.DivisionType.TrimsamsaParivritti:     return toRegularDivisionPosition(30);
			case Basics.DivisionType.TrimsamsaSimple:         return toDivisionPositionTrimsamsaSimple();
			case Basics.DivisionType.Khavedamsa:              return toDivisionPositionKhavedamsa();
			case Basics.DivisionType.Akshavedamsa:            return toDivisionPositionAkshavedamsa(45);
			case Basics.DivisionType.Shashtyamsa:             return toDivisionPositionShashtyamsa();
			case Basics.DivisionType.Ashtottaramsa:           return toRegularDivisionPosition(108);
			case Basics.DivisionType.Nadiamsa:                return toDivisionPositionNadiamsa();
			case Basics.DivisionType.NadiamsaCKN:             return toDivisionPositionNadiamsaCKN();
			case Basics.DivisionType.NavamsaDwadasamsa:       return toDivisionPositionNavamsaDwadasamsa();
			case Basics.DivisionType.DwadasamsaDwadasamsa:    return toDivisionPositionDwadasamsaDwadasamsa();
			case Basics.DivisionType.GenericParivritti:       return toRegularDivisionPosition(d.NumParts);
			case Basics.DivisionType.GenericShashthamsa:      return toDivisionPositionShashthamsa(d.NumParts);
			case Basics.DivisionType.GenericSaptamsa:         return toDivisionPositionSaptamsa(d.NumParts);
			case Basics.DivisionType.GenericDasamsa:          return toDivisionPositionDasamsa(d.NumParts);
			case Basics.DivisionType.GenericDwadasamsa:       return toDivisionPositionDwadasamsa(d.NumParts);
			case Basics.DivisionType.GenericChaturvimsamsa:   return toDivisionPositionChaturvimsamsa(d.NumParts);
			case Basics.DivisionType.GenericChaturthamsa:     return toDivisionPositionChaturthamsa(d.NumParts);
			case Basics.DivisionType.GenericNakshatramsa:     return toDivisionPositionNakshatramsa(d.NumParts);
			case Basics.DivisionType.GenericDrekkana:         return toDivisionPositionDrekanna(d.NumParts);
			case Basics.DivisionType.GenericShodasamsa:       return toDivisionPositionAkshavedamsa(d.NumParts);
			case Basics.DivisionType.GenericVimsamsa:         return toDivisionPositionVimsamsa(d.NumParts);
		}

		Trace.Assert(false, "DivisionPosition Error");
		return new DivisionPosition(name, type, new ZodiacHouse(ZodiacHouse.Name.Ari), 0, 0, 0);
	}

	public Longitude extrapolateLongitude(Division d)
	{
		var bp = (Position) Clone();
		foreach (var dSingle in d.MultipleDivisions)
		{
			bp.longitude = extrapolateLongitude(dSingle);
		}

		return bp.longitude;
	}

	public Longitude extrapolateLongitude(Division.SingleDivision d)
	{
		var dp      = toDivisionPosition(d);
		var lOffset = longitude.sub(dp.cusp_lower);
		var lRange  = new Longitude(dp.cusp_higher).sub(dp.cusp_lower);
		Trace.Assert(lOffset.value <= lRange.value, "Extrapolation internal error: Slice smaller than range. Weird.");

		var newOffset = lOffset.value / lRange.value      * 30.0;
		var newBase   = ((int) dp.zodiac_house.value - 1) * 30.0;
		return new Longitude(newOffset + newBase);
	}
}