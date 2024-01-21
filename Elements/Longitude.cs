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
using System.ComponentModel;
using System.Diagnostics;
using Mhora.Components.Converter;
using Mhora.Elements.Calculation;
using mhora.Util;

namespace Mhora.Elements;

[Serializable]
[TypeConverter(typeof(LongitudeConverter))]
public class Longitude
{
	private double _lon;

	public Longitude(double lon)
	{
		while (lon > 360.0)
		{
			lon -= 360.0;
		}

		while (lon < 0)
		{
			lon += 360.0;
		}

		_lon = lon;
		//m_lon = Basics.NormalizeExc (0, 360, lon);
	}

	public double Value
	{
		get => _lon;
		set
		{
			Trace.Assert(value >= 0 && value <= 360);
			_lon = value;
		}
	}

	public Longitude Add(Longitude b)
	{
		return new Longitude(Basics.NormalizeExcLower(Value + b.Value, 0, 360));
	}

	public Longitude Add(double b)
	{
		return Add(new Longitude(b));
	}

	public Longitude Sub(Longitude b)
	{
		return new Longitude(Basics.NormalizeExcLower(Value - b.Value, 0, 360));
	}

	public Longitude Sub(double b)
	{
		return Sub(new Longitude(b));
	}

	public double Normalize()
	{
		return Value.NormalizeExcLower(0, 360);
	}

	public bool IsBetween(Longitude cuspLower, Longitude cuspHigher)
	{
		var diff1 = Sub(cuspLower).Value;
		var diff2 = Sub(cuspHigher).Value;

		var bRet = cuspHigher.Sub(cuspLower).Value <= 180 && diff1 <= diff2;

		Application.Log.Debug("Is it true that {0} < {1} < {2}? {3}", this, cuspLower, cuspHigher, bRet);
		return bRet;
	}

	public SunMoonYoga ToSunMoonYoga()
	{
		var smIndex = (int) (Math.Floor(Value / (360.0 / 27.0)) + 1);
		var smYoga  = new SunMoonYoga((SunMoonYoga.Name) smIndex);
		return smYoga;
	}

	public double ToSunMoonYogaBase()
	{
		var num  = (int) ToSunMoonYoga().value;
		var cusp = (num - 1) * (360.0 / 27.0);
		return cusp;
	}

	public double ToSunMoonYogaOffset()
	{
		return Value - ToSunMoonYogaBase();
	}

	public Tithis.Tithi ToTithi()
	{
		var tIndex = (int) (Math.Floor(Value / (360.0 / 30.0)) + 1);
		var t      = tIndex.ToTithi();
		return t;
	}

	public Karanas.Karana ToKarana()
	{
		var kIndex = (int) (Math.Floor(Value / (360.0 / 60.0)) + 1);
		var k      = (Karanas.Karana) kIndex;
		return k;
	}

	public double ToKaranaBase()
	{
		var num  = (int) ToKarana();
		var cusp = (num - 1) * (360.0 / 60.0);
		return cusp;
	}

	public double ToKaranaOffset()
	{
		return Value - ToKaranaBase();
	}

	public double ToTithiBase()
	{
		var num  = ToTithi().Index();
		var cusp = (num - 1) * (360.0 / 30.0);
		return cusp;
	}

	public double ToTithiOffset()
	{
		return Value - ToTithiBase();
	}

	public Nakshatras.Nakshatra ToNakshatra()
	{
		var snum = (int) (Math.Floor(Value / (360.0 / 27.0)) + 1.0);
		return (Nakshatras.Nakshatra) snum;
	}

	public double ToNakshatraBase()
	{
		var num  = ToNakshatra().Index();
		var cusp = (num - 1) * (360.0 / 27.0);
		return cusp;
	}

	public Nakshatras.Nakshatra28 ToNakshatra28()
	{
		var snum = (int) (Math.Floor(Value / (360.0 / 27.0)) + 1.0);

		var ret = (Nakshatras.Nakshatra28) snum;
		if (snum >= (int) Nakshatras.Nakshatra28.Abhijit)
		{
			ret = ret.Add(2);
		}

		if (Value >= 270 + (6.0 + 40.0 / 60.0) && Value <= 270 + (10.0 + 53.0 / 60.0 + 20.0 / 3600.0))
		{
			ret = Nakshatras.Nakshatra28.Abhijit;
		}

		return ret;
	}

	public ZodiacHouse ToZodiacHouse()
	{
		var znum = (int) (Math.Floor(Value / 30.0) + 1.0);
		return new ZodiacHouse((ZodiacHouse.Rasi) znum);
	}

	public double ToZodiacHouseBase()
	{
		var znum = ToZodiacHouse().Sign.Index ();
		var cusp = (znum - 1) * 30.0;
		return cusp;
	}

	public double ToZodiacHouseOffset()
	{
		var znum = ToZodiacHouse().Sign.Index ();
		var cusp = (znum - 1) * 30.0;
		var ret  = Value - cusp;
		Trace.Assert(ret >= 0.0 && ret <= 30.0);
		return ret;
	}

	public double PercentageOfZodiacHouse()
	{
		var offset = ToZodiacHouseOffset();
		var perc   = offset / 30.0 * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public double ToNakshatraOffset()
	{
		var znum = ToNakshatra().Index();
		var cusp = (znum - 1) * (360.0 / 27.0);
		var ret  = Value - cusp;
		Trace.Assert(ret >= 0.0 && ret <= 360.0 / 27.0);
		return ret;
	}

	public double PercentageOfNakshatra()
	{
		var offset = ToNakshatraOffset();
		var perc   = offset / (360.0 / 27.0) * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public int ToNakshatraPada()
	{
		var offset = ToNakshatraOffset();
		var val    = (int) Math.Floor(offset / (360.0 / (27.0 * 4.0))) + 1;
		Trace.Assert(val >= 1 && val <= 4);
		return val;
	}

	public int ToAbsoluteNakshatraPada()
	{
		var n = ToNakshatra().Index();
		var p = ToNakshatraPada();
		return (n - 1) * 4 + p;
	}

	public double ToNakshatraPadaOffset()
	{
		var pnum = ToAbsoluteNakshatraPada();
		var cusp = (pnum - 1) * (360.0 / (27.0 * 4.0));
		var ret  = Value - cusp;
		Trace.Assert(ret >= 0.0 && ret <= 360.0 / 27.0);
		return ret;
	}

	public double ToNakshatraPadaPercentage()
	{
		var offset = ToNakshatraPadaOffset();
		var perc   = offset / (360.0 / (27.0 * 4.0)) * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public override string ToString()
	{
		var lon     = this;
		var rasi    = lon.ToZodiacHouse().Sign.ToString();
		var offset  = lon.ToZodiacHouseOffset();
		var minutes = Math.Floor(offset);
		offset = (offset - minutes) * 60.0;
		var seconds = Math.Floor(offset);
		offset = (offset - seconds) * 60.0;
		var subsecs = Math.Floor(offset);
		offset = (offset - subsecs) * 60.0;
		var subsubsecs = Math.Floor(offset);

		return string.Format("{0:00} {1} {2:00}:{3:00}:{4:00}", minutes, rasi, seconds, subsecs, subsubsecs);
	}
}