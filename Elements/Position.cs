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
using System.Collections.Generic;
using System.Diagnostics;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Divisions;
using Mhora.Divisions.D2;
using Mhora.Elements.Extensions;
using Mhora.SwissEph.Helpers;
using Mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     Specifies a BodyPosition, i.e. the astronomical characteristics of a body like
///     longitude, speed etc. It has no notion of its "rasi".
///     The functions to convert this to a DivisionType (the various vargas)
///     are all implemented here
/// </summary>
public class Position
{
	private readonly Horoscope _h;
	private static   bool      _mbNadiamsaCknCalculated;
	private static   double[]  _mNadiamsaCusps;
	public           string    OtherString;

	public readonly  BodyType              BodyType;
	public readonly  Body                  Name;
	public readonly  HorizontalCoordinates HorizontalCoordinates;

	public Position(Horoscope h, Body body, BodyType bodyType, Longitude lon, double lat = 0, double dist = 0, double splon = 0, double splat = 0, double spdist = 0)
	{
		Longitude      = lon;
		Latitude       = lat;
		Distance       = dist;
		SpeedLongitude = splon;
		SpeedLatitude  = splat;
		SpeedDistance  = spdist;
		Name           = body;
		BodyType       = bodyType;
		_h             = h;

		if (BodyType == BodyType.Graha)
		{
			var geoPosition = new GeoPosition()
			{
				Longitude = h.Info.Longitude,
				Latitude  = h.Info.Latitude,
				Altitude  = h.Info.Altitude,
			};

			HorizontalCoordinates = SweApi.GetHorizontalCoordinates(h.Info.Jd, geoPosition, 0, 0,this);
		}
		else
		{
			HorizontalCoordinates = new HorizontalCoordinates();
		}
		//Mhora.Log.Debug ("{0} {1} {2}", body.ToString(), lon.value, splon);
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

	public Position Clone()
	{
		var bp = new Position(_h, Name, BodyType, Longitude, Latitude, Distance, SpeedLongitude, SpeedLatitude, SpeedDistance)
		{
			OtherString = OtherString
		};
		return bp;
	}

	/// <summary>
	///     Many of the varga divisions (like navamsa) are regular divisions,
	///     and can be implemented by a single method. We do this when possible.
	/// </summary>
	/// <param name="n">The number of parts a house is divided into</param>
	/// <returns>The DivisionPosition the body falls into</returns>
	private DivisionPosition ToRegularDivisionPosition(int n)
	{
		var cusp     = new Cusp(Longitude, n);
		var zhouse   = Longitude.ToZodiacHouse().Index();
		var numParts = (zhouse - 1) * n + Longitude.PartOfZodiacHouse(n);
		var divHouse = ZodiacHouse.Ari.Add(numParts);
		var dp       = new DivisionPosition(Name, BodyType, divHouse, cusp);

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
		var zhouse   = (int) Longitude.ToZodiacHouse();
		var numParts = Longitude.PartOfZodiacHouse(n);
		var divHouse = Longitude.ToZodiacHouse().Add(numParts);
		var cusp     = new Cusp(Longitude, n);
		var dp       = new DivisionPosition(Name, BodyType, divHouse, cusp)
		{
			Longitude = divHouse.DivisionalLongitude(Longitude, n)
		};
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
				var cusp2 = new Cusps2(cusps[i], cusps[i + 1], 1, 1);
				return new DivisionPosition(Name, BodyType, (ZodiacHouse) i + 1, cusp2);
			}
		}

		throw new Exception();
	}

	private DivisionPosition ToBhavaDivisionPositionHouse(Longitude[] cusps)
	{
		Debug.Assert(cusps.Length == 13);

		var zlagna = _h.GetPosition(Body.Lagna).ToDivisionPosition(DivisionType.Rasi).ZodiacHouse;
		for (var i = 0; i < 12; i++)
		{
			if (Longitude.Sub(cusps[i]).Value < cusps[i + 1].Sub(cusps[i]).Value)
			{
				//Mhora.Log.Debug ("Found {4} - {0} in cusp {3} between {1} and {2}", this.m_lon.value,
				//	cusps[i].value, cusps[i+1].value, i+1, this.name.ToString());

				var cusp2 = new Cusps2(cusps[i], cusps[i + 1], 1, 1);
				return new DivisionPosition(Name, BodyType, zlagna.Add(i + 1), cusp2);
			}
		}

		return (null);
	}

	private DivisionPosition ToDivisionPositionBhavaEqual()
	{
		var offset = _h.GetPosition(Body.Lagna).Longitude.ToZodiacHouseOffset();
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
		var offset      = _h.GetPosition(Body.Lagna).Longitude.ToZodiacHouseOffset();
		var padasOffset = (int) (offset / (360.0 / 108.0)).Floor();
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

		if (hsys != _h.SwephHouseSystem)
		{
			_h.SwephHouseSystem = hsys;
			_h.PopulateHouseCusps();
		}

		return ToBhavaDivisionPositionHouse(_h.SwephHouseCusps);
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
		if (Longitude.HoraSunOddEven())
		{
			zh = (ZodiacHouse) daySigns[sign];
		}
		else
		{
			zh = (ZodiacHouse) nightSigns[sign];
		}

		var cusp = new Cusp(Longitude, 2);
		var dp = new DivisionPosition(Name, BodyType, zh, cusp)
		{
			Longitude = zh.DivisionalLongitude(Longitude, 2)
		};
		return dp;
	}

	private DivisionPosition ToDivisionPositionHoraJagannath()
	{
		var zh = Longitude.ToZodiacHouse();

		Application.Log.Debug("{2} in {3}: OddEven is {0}, DayNight is {1}", Longitude.HoraSunOddEven(), Longitude.HoraSunDayNight(), Name, zh);

		if (Longitude.HoraSunDayNight() && false == Longitude.HoraSunOddEven())
		{
			zh = zh.Add(7);
		}
		else if (false == Longitude.HoraSunDayNight() && Longitude.HoraSunOddEven())
		{
			zh = zh.Add(7);
		}

		Application.Log.Debug("{0} ends in {1}", Name, zh);

		var cusp = new Cusp(Longitude, 2);
		var dp = new DivisionPosition(Name, BodyType, zh, cusp)
		{
			Longitude = zh.DivisionalLongitude(Longitude, 2)
		};
		return dp;
	}

	private DivisionPosition ToDivisionPositionHoraParasara()
	{
		ZodiacHouse zh;
		var        rulerIndex = 0;
		if (Longitude.HoraSunOddEven())
		{
			zh         = ZodiacHouse.Leo;
			rulerIndex = 1;
		}
		else
		{
			zh         = ZodiacHouse.Can;
			rulerIndex = 2;
		}

		var cusp = new Cusp(Longitude, 2);
		var dp = new DivisionPosition(Name, BodyType, zh, cusp)
		{
			Longitude = zh.DivisionalLongitude(Longitude, 2),
			RulerIndex = rulerIndex
		};
		return dp;
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
		var part   = Longitude.PartOfZodiacHouse(n);
		var dhouse = zhouse.Add(offset[part % 3]);
		var cusp   = new Cusp(Longitude, n);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 3)
		};
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
		zhm = mod switch
		      {
			      1 => zh.Add(1),
			      2 => zh.Add(9),
			      _ => zh.Add(5)
		      };

		// From moveable sign, 3 parts belong to the trines
		var part = Longitude.PartOfZodiacHouse(3);
		dhouse = part switch
		         {
			         1 => zhm.Add(1),
			         2 => zhm.Add(5),
			         _ => zhm.Add(9)
		         };

		var cusp = new Cusp(Longitude, 3);
		var dp = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 3)
		};
		return dp;
	}

	private DivisionPosition ToDivisionPositionDrekkanaSomnath()
	{
		var mod  = (int) Longitude.ToZodiacHouse() % 2;
		var part = Longitude.PartOfZodiacHouse(3);
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

		var cusp = new Cusp(Longitude, 3);
		var dp = new DivisionPosition(Name, BodyType, zh2, cusp)
		{
			Longitude = zh2.DivisionalLongitude(Longitude, 3)
		};
		return dp;
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
		var part   = Longitude.PartOfZodiacHouse(n);
		var dhouse = zhouse.Add(offset[part % 4]);
		var cusp   = new Cusp(Longitude, n);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, n)
		};
		if (n == 4)
		{
			dp.RulerIndex = part;
		}

		return (dp);
	}

	private DivisionPosition ToDivisionPositionShashthamsa(int n)
	{
		var mod     = (int) Longitude.ToZodiacHouse() % 2;
		var dhousen = mod % 2 == 1 ? ZodiacHouse.Ari : ZodiacHouse.Lib;
		var dhouse  = dhousen.Add(Longitude.PartOfZodiacHouse(n));
		var cusp    = new Cusp(Longitude, n);
		var dp      = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, n)
		};
		return (dp);
	}

	private DivisionPosition ToDivisionPositionSaptamsa(int n)
	{
		var part = Longitude.PartOfZodiacHouse(n);
		var zh   = Longitude.ToZodiacHouse();
		if (false == zh.IsOdd())
		{
			zh = zh.Add(7);
		}

		zh = zh.Add(part);
		var cusp = new Cusp(Longitude, n);
		var dp = new DivisionPosition(Name, BodyType, zh, cusp)
		{
			Longitude = zh.DivisionalLongitude(Longitude, n)
		};

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

		return (dp);
	}

	private DivisionPosition ToDivisionPositionNavamsa(int parts)
	{
		DivisionPosition dp;
		if (parts > 9)
		{
			var bp = Clone();
			bp.Longitude = bp.ExtrapolateLongitude(DivisionType.Navamsa);
			dp = bp.ToDivisionPositionNavamsa(parts / 9);
			var cusp = new Cusp(Longitude, parts);
			dp = new DivisionPosition(dp.Body, dp.BodyType, dp.ZodiacHouse, cusp);
			return (dp);
		}

		var part = Longitude.PartOfZodiacHouse(9);
		dp   = ToRegularDivisionPosition(9);

		dp.RulerIndex = ((int) Longitude.ToZodiacHouse() % 3) switch
		                {
			                1 => part,
			                2 => part + 1,
			                0 => part + 2,
			                _ => dp.RulerIndex
		                };

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

		var dhouse = zstart.Add(Longitude.PartOfZodiacHouse(8));
		var cusp   = new Cusp(Longitude, 8);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 8)
		};
		return (dp);
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
		var part   = Longitude.PartOfZodiacHouse(5);
		var mod    = (int) Longitude.ToZodiacHouse() % 2;
		var dhouse = mod % 2 == 1 ? offsetOdd[part - 1] : offsetEven[part - 1];
		var zh     = dhouse;
		var cusp   = new Cusp(Longitude, 5);
		var dp     = new DivisionPosition(Name, BodyType, zh, cusp)
		{
			Longitude = zh.DivisionalLongitude(Longitude, 5)
		};
		return (dp);
	}

	private DivisionPosition ToDivisionPositionRudramsa()
	{
		var zari   = ZodiacHouse.Ari;
		var zhouse = Longitude.ToZodiacHouse();
		var diff   = zari.NumHousesBetween(zhouse);
		var zstart = zari.AddReverse(diff);
		var part   = Longitude.PartOfZodiacHouse(11);
		var zend   = zstart.Add(part);
		var cusp   = new Cusp(Longitude, 11);
		var dp     = new DivisionPosition(Name, BodyType, zend, cusp)
		{
			Longitude = zend.DivisionalLongitude(Longitude, 11)
		};
		return dp;
	}

	private DivisionPosition ToDivisionPositionRudramsaRaman()
	{
		var zhstart = Longitude.ToZodiacHouse().Add(12);
		var part    = Longitude.PartOfZodiacHouse(11);
		var zend    = zhstart.AddReverse(part);
		var cusp    = new Cusp(Longitude, 11);
		var dp      = new DivisionPosition(Name, BodyType, zend, cusp)
		{
			Longitude = zend.DivisionalLongitude(Longitude, 11)
		};

		return dp;
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
		var part   = Longitude.PartOfZodiacHouse(n);
		dhouse = dhouse.Add(part);
		var cusp = new Cusp(Longitude, n);
		var dp    = new DivisionPosition(Name, BodyType, dhouse, cusp);
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
		return (dp);
	}

	private DivisionPosition ToDivisionPositionDwadasamsa(int n)
	{
		var zhouse = Longitude.ToZodiacHouse();
		var part   = Longitude.PartOfZodiacHouse(n);
		var dhouse = zhouse.Add(part);
		var cusp   = new Cusp(Longitude, n);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp);
		if (n == 12)
		{
			dp.RulerIndex = part.NormalizeInc(1, 4);
		}

		dp.Longitude = dhouse.DivisionalLongitude(Longitude, n);

		return (dp);
	}

	private DivisionPosition ToDivisionPositionShodasamsa()
	{
		var part  = Longitude.PartOfZodiacHouse(16);
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
		ZodiacHouse dhousename = mod switch
		                         {
			                         1 => ZodiacHouse.Ari,
			                         2 => ZodiacHouse.Sag,
			                         _ => ZodiacHouse.Leo
		                         };

		var part   = Longitude.PartOfZodiacHouse(n);
		var dhouse = dhousename.Add(part);
		var cusp   = new Cusp(Longitude, n);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, n)
		};
		return (dp);
	}

	private DivisionPosition ToDivisionPositionVimsamsa()
	{
		var part = Longitude.PartOfZodiacHouse(20);
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
		var part       = Longitude.PartOfZodiacHouse(n);
		var dhouse     = dhousename.Add(part);
		var cusp       = new Cusp(Longitude, n);
		var dp         = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, n)
		};
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

		return dp;
	}

	private DivisionPosition ToDivisionPositionNakshatramsa(int n)
	{
		var              mod = (int) Longitude.ToZodiacHouse() % 4;
		ZodiacHouse dhousename = mod switch
		                         {
			                         1 => ZodiacHouse.Ari,
			                         2 => ZodiacHouse.Can,
			                         3 => ZodiacHouse.Lib,
			                         _ => ZodiacHouse.Cap
		                         };

		var part   = Longitude.PartOfZodiacHouse(n);
		var dhouse = dhousename.Add(part);
		var cusp   = new Cusp(Longitude, n);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, n)
		};

		return (dp);
	}

	private DivisionPosition ToDivisionPositionNakshatramsa()
	{
		var dp = ToRegularDivisionPosition(27);
		dp.RulerIndex = Longitude.PartOfZodiacHouse(27);
		return dp;
	}

	private DivisionPosition ToDivisionPositionTrimsamsaSimple()
	{
		var zhouse = Longitude.ToZodiacHouse();
		var part   = Longitude.PartOfZodiacHouse(30);
		var dhouse = zhouse.Add(part);
		var cusp   = new Cusp(Longitude, 30);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 30)
		};
		return (dp);
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
			switch (off)
			{
				case <= 5:
					dhouse     = ZodiacHouse.Ari;
					cuspLower  = 0.0;
					cuspHigher = 5.0;
					rulerIndex = 1;
					part       = 1;
					break;
				case <= 10:
					dhouse     = ZodiacHouse.Aqu;
					cuspLower  = 5.01;
					cuspHigher = 10.0;
					rulerIndex = 2;
					part       = 2;
					break;
				case <= 18:
					dhouse     = ZodiacHouse.Sag;
					cuspLower  = 10.01;
					cuspHigher = 18.0;
					rulerIndex = 3;
					part       = 3;
					break;
				case <= 25:
					dhouse     = ZodiacHouse.Gem;
					cuspLower  = 18.01;
					cuspHigher = 25.0;
					rulerIndex = 4;
					part       = 4;
					break;
				default:
					dhouse     = ZodiacHouse.Lib;
					cuspLower  = 25.01;
					cuspHigher = 30.0;
					rulerIndex = 5;
					part       = 5;
					break;
			}
		}
		else
		{
			switch (off)
			{
				case <= 5:
					dhouse     = ZodiacHouse.Tau;
					cuspLower  = 0.0;
					cuspHigher = 5.0;
					rulerIndex = 5;
					part       = 1;
					break;
				case <= 12:
					dhouse     = ZodiacHouse.Vir;
					cuspLower  = 5.01;
					cuspHigher = 12.0;
					rulerIndex = 4;
					part       = 2;
					break;
				case <= 20:
					dhouse     = ZodiacHouse.Pis;
					cuspLower  = 12.01;
					cuspHigher = 20.0;
					rulerIndex = 3;
					part       = 3;
					break;
				case <= 25:
					dhouse     = ZodiacHouse.Cap;
					cuspLower  = 20.01;
					cuspHigher = 25.0;
					rulerIndex = 2;
					part       = 4;
					break;
				default:
					dhouse     = ZodiacHouse.Sco;
					cuspLower  = 25.01;
					cuspHigher = 30.0;
					rulerIndex = 1;
					part       = 5;
					break;
			}
		}

		cuspLower  += Longitude.ToZodiacHouseBase();
		cuspHigher += Longitude.ToZodiacHouseBase();

		var cusps = new Cusps2(cuspLower, cuspHigher, part, 30);
		var dp = new DivisionPosition(Name, BodyType, dhouse, cusps)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 30),
			RulerIndex = rulerIndex,
		};
		return dp;
	}

	private DivisionPosition ToDivisionPositionKhavedamsa()
	{
		var mod        = (int) Longitude.ToZodiacHouse() % 2;
		var dhousename = mod % 2 == 1 ? ZodiacHouse.Ari : ZodiacHouse.Lib;
		var part       = Longitude.PartOfZodiacHouse(40);
		var dhouse     = dhousename.Add(part);
		var cusp       = new Cusp(Longitude, 40);
		var dp         = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 40),
			RulerIndex = part.NormalizeInc(1, 12)
		};
		return (dp);
	}

	private DivisionPosition ToDivisionPositionAkshavedamsa(int n)
	{
		var              mod = (int) Longitude.ToZodiacHouse() % 3;
		ZodiacHouse dhousename = mod switch
		                         {
			                         1 => ZodiacHouse.Ari,
			                         2 => ZodiacHouse.Leo,
			                         _ => ZodiacHouse.Sag
		                         };

		var part   = Longitude.PartOfZodiacHouse(n);
		var dhouse = dhousename.Add(part);
		var cusp   = new Cusp(Longitude, n);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, n)
		};
		if (n == 45)
		{
			dp.RulerIndex = ((int) Longitude.ToZodiacHouse() % 3) switch
			                {
				                1 => part,
				                2 => part + 1,
				                0 => part + 2,
				                _ => dp.RulerIndex
			                };

			dp.RulerIndex = dp.RulerIndex.NormalizeInc(1, 3);
		}

		return (dp);
	}

	private DivisionPosition ToDivisionPositionShashtyamsa()
	{
		var zhouse = Longitude.ToZodiacHouse();
		var part   = Longitude.PartOfZodiacHouse(60);
		var dhouse = zhouse.Add(part);
		var cusp   = new Cusp(Longitude, 60);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 60)
		};
		if (Longitude.ToZodiacHouse().IsOdd())
		{
			dp.RulerIndex = part;
		}
		else
		{
			dp.RulerIndex = 61 - part;
		}

		return (dp);
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
		var part   = Longitude.PartOfZodiacHouse(150);
		var dhouse = zhouse.Add(part);
		var cusp   = new Cusp(Longitude, 150);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 150)
		};
		dp.RulerIndex = ((int) Longitude.ToZodiacHouse() % 3) switch
		                {
			                1 => part,
			                2 => 151 - part,
			                0 => (75 + part).NormalizeInc(1, 150),
			                _ => dp.RulerIndex
		                };

		return (dp);
	}

	private void CalculateNadiamsaCusps()
	{
		if (_mbNadiamsaCknCalculated)
		{
			return;
		}

		int[] bases =
		[
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
		];
		var alUnsorted = new List<double> ();
		foreach (var iVarga in bases)
		{
			for (var i = 0; i < iVarga; i++)
			{
				alUnsorted.Add(i / (double) iVarga * 30.0);
			}
		}

		alUnsorted.Add(30.0);
		alUnsorted.Sort();
		var alSorted = new List<double>()
		{
			0.0
		};

		for (var i = 0; i < alUnsorted.Count; i++)
		{
			if (alUnsorted[i] != alSorted[alSorted.Count - 1])
			{
				alSorted.Add(alUnsorted[i]);
			}
		}

		Debug.Assert(alSorted.Count == 151, string.Format("Found {0} Nadis. Expected 151.", alSorted.Count));

		_mNadiamsaCusps          = alSorted.ToArray();
		_mbNadiamsaCknCalculated = true;
	}

	private DivisionPosition ToDivisionPositionNadiamsaCkn()
	{
		CalculateNadiamsaCusps();
		var part = Longitude.PartOfZodiacHouse(150) - 10;
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
		var cusp   = new Cusp(Longitude, 150);
		var dp     = new DivisionPosition(Name, BodyType, dhouse, cusp)
		{
			Longitude = dhouse.DivisionalLongitude(Longitude, 150)
		};

		dp.RulerIndex = ((int) Longitude.ToZodiacHouse() % 3) switch
		                {
			                1 => part,
			                2 => 151 - part,
			                0 => (75 + part).NormalizeInc(1, 150),
			                _ => dp.RulerIndex
		                };

		var lower = Longitude.ToZodiacHouseBase() + _mNadiamsaCusps[part - 1];
		var upper = Longitude.ToZodiacHouseBase() + _mNadiamsaCusps[part];
		var cusp2 = new Cusps2(lower, upper, part, 150);
		dp = new DivisionPosition(dp.Body, dp.BodyType, dp.ZodiacHouse, cusp2);
		return dp;
	}

	private DivisionPosition ToDivisionPositionNavamsaDwadasamsa()
	{
		var bp = Clone();
		bp.Longitude = bp.ExtrapolateLongitude(DivisionType.Navamsa);
		var dp   = bp.ToDivisionPositionDwadasamsa(12);
		var cusp = new Cusp(Longitude, 108);
		dp = new DivisionPosition(dp.Body, dp.BodyType, dp.ZodiacHouse, cusp);
		return dp;
	}

	private DivisionPosition ToDivisionPositionDwadasamsaDwadasamsa()
	{
		var bp = Clone();
		bp.Longitude = bp.ExtrapolateLongitude(DivisionType.Dwadasamsa);
		var dp   = bp.ToDivisionPositionDwadasamsa(12);
		var cusp = new Cusp(Longitude, 144);
		dp = new DivisionPosition(dp.Body, dp.BodyType, dp.ZodiacHouse, cusp);
		return dp;
	}

	public DivisionPosition ToDivisionPosition(DivisionType varga)
	{
		return ToDivisionPosition(new Division(varga));
	}


	/// <summary>
	///     Calculated any known Vargas positions. Simply calls the appropriate
	///     helper function
	/// </summary>
	/// <param name="dtype">The requested DivisionType</param>
	/// <returns>A division Position</returns>
	public DivisionPosition ToDivisionPosition(Division d)
	{
		var              bp = Clone();
		DivisionPosition dp = null;

		foreach (var division in d.MultipleDivisions)
		{
			dp           = bp.ToDivisionPosition(division);
			dp.Longitude = bp.ExtrapolateLongitude(dp);
		}
		return dp;
	}

	public DivisionPosition ToDivisionPosition(Division.SingleDivision d)
	{
		if (d.NumParts < 1)
		{
			d.NumParts = 1;
		}

		return d.Varga switch
		{
			DivisionType.Rasi                    => ToRegularDivisionPosition(1),
			DivisionType.BhavaPada               => ToDivisionPositionBhavaPada(),
			DivisionType.BhavaEqual              => ToDivisionPositionBhavaEqual(),
			DivisionType.BhavaSripati            => ToDivisionPositionBhavaHelper('O'),
			DivisionType.BhavaKoch               => ToDivisionPositionBhavaHelper('K'),
			DivisionType.BhavaPlacidus           => ToDivisionPositionBhavaHelper('P'),
			DivisionType.BhavaCampanus           => ToDivisionPositionBhavaHelper('C'),
			DivisionType.BhavaRegiomontanus      => ToDivisionPositionBhavaHelper('R'),
			DivisionType.BhavaAlcabitus          => ToDivisionPositionBhavaHelper('B'),
			DivisionType.BhavaAxial              => ToDivisionPositionBhavaHelper('X'),
			DivisionType.HoraParivrittiDwaya     => ToRegularDivisionPosition(2),
			DivisionType.HoraKashinath           => ToDivisionPositionHoraKashinath(),
			DivisionType.HoraParasara            => ToDivisionPositionHoraParasara(),
			DivisionType.HoraJagannath           => ToDivisionPositionHoraJagannath(),
			DivisionType.DrekkanaParasara        => ToDivisionPositionDrekanna(3),
			DivisionType.DrekkanaJagannath       => ToDivisionPositionDrekannaJagannath(),
			DivisionType.DrekkanaParivrittitraya => ToRegularDivisionPosition(3),
			DivisionType.DrekkanaSomnath         => ToDivisionPositionDrekkanaSomnath(),
			DivisionType.Chaturthamsa            => ToDivisionPositionChaturthamsa(4),
			DivisionType.Panchamsa               => ToDivisionPositionPanchamsa(),
			DivisionType.Shashthamsa             => ToDivisionPositionShashthamsa(6),
			DivisionType.Saptamsa                => ToDivisionPositionSaptamsa(7),
			DivisionType.Ashtamsa                => ToRegularDivisionPosition(8),
			DivisionType.AshtamsaRaman           => ToDivisionPositionAshtamsaRaman(),
			DivisionType.Navamsa                 => ToDivisionPositionNavamsa(9),
			DivisionType.Dasamsa                 => ToDivisionPositionDasamsa(10),
			DivisionType.Rudramsa                => ToDivisionPositionRudramsa(),
			DivisionType.RudramsaRaman           => ToDivisionPositionRudramsaRaman(),
			DivisionType.Dwadasamsa              => ToDivisionPositionDwadasamsa(12),
			DivisionType.Shodasamsa              => ToDivisionPositionShodasamsa(),
			DivisionType.Vimsamsa                => ToDivisionPositionVimsamsa(),
			DivisionType.Chaturvimsamsa          => ToDivisionPositionChaturvimsamsa(24),
			DivisionType.Nakshatramsa            => ToDivisionPositionNakshatramsa(),
			DivisionType.Trimsamsa               => ToDivisionPositionTrimsamsa(),
			DivisionType.TrimsamsaParivritti     => ToRegularDivisionPosition(30),
			DivisionType.TrimsamsaSimple         => ToDivisionPositionTrimsamsaSimple(),
			DivisionType.Khavedamsa              => ToDivisionPositionKhavedamsa(),
			DivisionType.Akshavedamsa            => ToDivisionPositionAkshavedamsa(45),
			DivisionType.NavaNavamsa             => ToDivisionPositionNavamsa(81),
			DivisionType.Shashtyamsa             => ToDivisionPositionShashtyamsa(),
			DivisionType.Ashtottaramsa           => ToRegularDivisionPosition(108),
			DivisionType.Nadiamsa                => ToDivisionPositionNadiamsa(),
			DivisionType.NadiamsaCKN             => ToDivisionPositionNadiamsaCkn(),
			DivisionType.NavamsaDwadasamsa       => ToDivisionPositionNavamsaDwadasamsa(),
			DivisionType.DwadasamsaDwadasamsa    => ToDivisionPositionDwadasamsaDwadasamsa(),
			DivisionType.GenericParivritti       => ToRegularDivisionPosition(d.NumParts),
			DivisionType.GenericShashthamsa      => ToDivisionPositionShashthamsa(d.NumParts),
			DivisionType.GenericSaptamsa         => ToDivisionPositionSaptamsa(d.NumParts),
			DivisionType.GenericDasamsa          => ToDivisionPositionDasamsa(d.NumParts),
			DivisionType.GenericDwadasamsa       => ToDivisionPositionDwadasamsa(d.NumParts),
			DivisionType.GenericChaturvimsamsa   => ToDivisionPositionChaturvimsamsa(d.NumParts),
			DivisionType.GenericChaturthamsa     => ToDivisionPositionChaturthamsa(d.NumParts),
			DivisionType.GenericNakshatramsa     => ToDivisionPositionNakshatramsa(d.NumParts),
			DivisionType.GenericDrekkana         => ToDivisionPositionDrekanna(d.NumParts),
			DivisionType.GenericShodasamsa       => ToDivisionPositionAkshavedamsa(d.NumParts),
			DivisionType.GenericVimsamsa         => ToDivisionPositionVimsamsa(d.NumParts),
			_                                    => throw new ArgumentOutOfRangeException(),
		};
		Trace.Assert(false, "DivisionPosition Error");
		return (null);
	}

	public Longitude ExtrapolateLongitude(DivisionType varga)
	{
		return ExtrapolateLongitude(new Division(varga));
	}

	public Longitude ExtrapolateLongitude(Division d)
	{
		var bp = Clone();
		foreach (var dSingle in d.MultipleDivisions)
		{
			var dp       = bp.ToDivisionPosition(dSingle);
			bp.Longitude = ExtrapolateLongitude(dp);
		}

		return bp.Longitude;
	}

	public Longitude ExtrapolateLongitude(DivisionPosition dp)
	{
		var lOffset = Longitude.Sub(dp.Cusp.Lower);
		var lRange  = dp.Cusp.Upper.Sub(dp.Cusp.Lower);
		Trace.Assert(lOffset.Value <= lRange.Value, "Extrapolation internal error: Slice smaller than range. Weird.");

		var newOffset = lOffset.Value / lRange.Value * 30;
		var newBase   = ((int) dp.ZodiacHouse - 1) * 30M;
		return new Longitude(newOffset + newBase);
	}
}