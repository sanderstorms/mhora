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
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Elements;

[Serializable]
[TypeConverter(typeof(LongitudeConverter))]
public class Longitude
{
	private double m_lon;

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

		m_lon = lon;
		//m_lon = Basics.normalize_exc (0, 360, lon);
	}

	public double value
	{
		get => m_lon;
		set
		{
			Trace.Assert(value >= 0 && value <= 360);
			m_lon = value;
		}
	}

	public Longitude add(Longitude b)
	{
		return new Longitude(Basics.normalize_exc_lower(0, 360, value + b.value));
	}

	public Longitude add(double b)
	{
		return add(new Longitude(b));
	}

	public Longitude sub(Longitude b)
	{
		return new Longitude(Basics.normalize_exc_lower(0, 360, value - b.value));
	}

	public Longitude sub(double b)
	{
		return sub(new Longitude(b));
	}

	public double normalize()
	{
		return Basics.normalize_exc_lower(0, 360, value);
	}

	public bool isBetween(Longitude cusp_lower, Longitude cusp_higher)
	{
		var diff1 = sub(cusp_lower).value;
		var diff2 = sub(cusp_higher).value;

		var bRet = cusp_higher.sub(cusp_lower).value <= 180 && diff1 <= diff2;

		Application.Log.Debug("Is it true that {0} < {1} < {2}? {3}", this, cusp_lower, cusp_higher, bRet);
		return bRet;
	}

	public SunMoonYoga toSunMoonYoga()
	{
		var smIndex = (int) (Math.Floor(value / (360.0 / 27.0)) + 1);
		var smYoga  = new SunMoonYoga((SunMoonYoga.Name) smIndex);
		return smYoga;
	}

	public double toSunMoonYogaBase()
	{
		var num  = (int) toSunMoonYoga().value;
		var cusp = (num - 1) * (360.0 / 27.0);
		return cusp;
	}

	public double toSunMoonYogaOffset()
	{
		return value - toSunMoonYogaBase();
	}

	public Tithi toTithi()
	{
		var tIndex = (int) (Math.Floor(value / (360.0 / 30.0)) + 1);
		var t      = new Tithi((Tables.Tithi.Value) tIndex);
		return t;
	}

	public Karana toKarana()
	{
		var kIndex = (int) (Math.Floor(value / (360.0 / 60.0)) + 1);
		var k      = new Karana((Karana.Name) kIndex);
		return k;
	}

	public double toKaranaBase()
	{
		var num  = (int) toKarana().value;
		var cusp = (num - 1) * (360.0 / 60.0);
		return cusp;
	}

	public double toKaranaOffset()
	{
		return value - toKaranaBase();
	}

	public double toTithiBase()
	{
		var num  = (int) toTithi().value;
		var cusp = (num - 1) * (360.0 / 30.0);
		return cusp;
	}

	public double toTithiOffset()
	{
		return value - toTithiBase();
	}

	public Nakshatra toNakshatra()
	{
		var snum = (int) (Math.Floor(value / (360.0 / 27.0)) + 1.0);
		return new Nakshatra((Nakshatra.Name) snum);
	}

	public double toNakshatraBase()
	{
		var num  = (int) toNakshatra().value;
		var cusp = (num - 1) * (360.0 / 27.0);
		return cusp;
	}

	public Nakshatra28 toNakshatra28()
	{
		var snum = (int) (Math.Floor(value / (360.0 / 27.0)) + 1.0);

		var ret = new Nakshatra28((Nakshatra28.Name) snum);
		if (snum >= (int) Nakshatra28.Name.Abhijit)
		{
			ret = ret.add(2);
		}

		if (value >= 270 + (6.0 + 40.0 / 60.0) && value <= 270 + (10.0 + 53.0 / 60.0 + 20.0 / 3600.0))
		{
			ret.value = Nakshatra28.Name.Abhijit;
		}

		return ret;
	}

	public ZodiacHouse toZodiacHouse()
	{
		var znum = (int) (Math.Floor(value / 30.0) + 1.0);
		return new ZodiacHouse((ZodiacHouse.Name) znum);
	}

	public double toZodiacHouseBase()
	{
		var znum = (int) toZodiacHouse().value;
		var cusp = (znum - 1) * 30.0;
		return cusp;
	}

	public double toZodiacHouseOffset()
	{
		var znum = (int) toZodiacHouse().value;
		var cusp = (znum - 1) * 30.0;
		var ret  = value - cusp;
		Trace.Assert(ret >= 0.0 && ret <= 30.0);
		return ret;
	}

	public double percentageOfZodiacHouse()
	{
		var offset = toZodiacHouseOffset();
		var perc   = offset / 30.0 * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public double toNakshatraOffset()
	{
		var znum = (int) toNakshatra().value;
		var cusp = (znum - 1) * (360.0 / 27.0);
		var ret  = value - cusp;
		Trace.Assert(ret >= 0.0 && ret <= 360.0 / 27.0);
		return ret;
	}

	public double percentageOfNakshatra()
	{
		var offset = toNakshatraOffset();
		var perc   = offset / (360.0 / 27.0) * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public int toNakshatraPada()
	{
		var offset = toNakshatraOffset();
		var val    = (int) Math.Floor(offset / (360.0 / (27.0 * 4.0))) + 1;
		Trace.Assert(val >= 1 && val <= 4);
		return val;
	}

	public int toAbsoluteNakshatraPada()
	{
		var n = (int) toNakshatra().value;
		var p = toNakshatraPada();
		return (n - 1) * 4 + p;
	}

	public double toNakshatraPadaOffset()
	{
		var pnum = toAbsoluteNakshatraPada();
		var cusp = (pnum - 1) * (360.0 / (27.0 * 4.0));
		var ret  = value - cusp;
		Trace.Assert(ret >= 0.0 && ret <= 360.0 / 27.0);
		return ret;
	}

	public double toNakshatraPadaPercentage()
	{
		var offset = toNakshatraPadaOffset();
		var perc   = offset / (360.0 / (27.0 * 4.0)) * 100;
		Trace.Assert(perc >= 0 && perc <= 100);
		return perc;
	}

	public override string ToString()
	{
		var lon     = this;
		var rasi    = lon.toZodiacHouse().value.ToString();
		var offset  = lon.toZodiacHouseOffset();
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