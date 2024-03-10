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

using System.Collections;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

public class KalachakraDasa : Dasa, IDasa
{
	private readonly Horoscope _h;

	public class DasaInfo
	{
		public ZodiacHouse DasaPack { get;}
		public int         Cycle    { get;}
		public ZodiacHouse Dasa     { get; }

		public DasaInfo(ZodiacHouse dp, int cycle, ZodiacHouse dasa)
		{
			DasaPack = dp;
			Cycle    = cycle;
			Dasa     = dasa;
		}
	}

	public class BhuktiInfo
	{
		public DasaInfo    Dasa   { get;}
		public int         Cycle  { get;}
		public ZodiacHouse Bhukti { get;}

		public BhuktiInfo (DasaInfo dasa, int cycle, ZodiacHouse bhukti)
		{
			Dasa   = dasa;
			Cycle  = cycle;
			Bhukti = bhukti;
		}	
	}


	public KalachakraDasa(Horoscope h)
	{
		_h = h;
	}

	public double ParamAyus()
	{
		return 144;
	}

	//In any dasha there are bhukties of nine signs ruled by mine lords in the usual sequence starting from
	//a sign ruled  by the lord of the movable (chara) sign in trine to the dasha sign.
	//The first bhukti in any dasha will be of a sign that is ruled by the lord of the movable
	//sign occurring In trine to the dasha sign
	public ArrayList Dasa(int cycle)
	{
		var dRasi = new Division(DivisionType.Rasi);
		var mLon  = _h.GetPosition(Body.Moon).ExtrapolateLongitude(dRasi);

		var al = new ArrayList();

		var dasaLength = 0.0;

		for (var i = 0; i < 9; i++)
		{
			var zh  = mLon.DasaPeriod(i, out _);
			var len = zh.DasaLength();
			dasaLength += len;
		}

		/*
		Angle  nakshatraOffset = mLon.NakshatraOffset();
		Angle  unit            = new Angle(3,20, 0.0);
		double navamsa         = mLon.Value                / unit;
		Angle  begin           = Math.Floor(navamsa) * unit;
		var    left            = mLon.Value - begin;

		var bhogyaSarvayu = (left.TotalArcseconds * 100) / 12000;
		*/
		var offsetLength = mLon.NakshatraPadaPercentage() / 100.0 * dasaLength;

		var start   = -offsetLength;
		int skipped = 0;
		for (var i = 0; i < 9; i++)
		{
			var zh  = mLon.DasaPeriod(i, out var savya);
			var len = zh.DasaLength();

			if ((start + len) >= 0)
			{
				var de = new KalaChakraDasaEntry(zh, start, len, 1, savya, zh.ToString());
				al.Add(de);
			}
			else
			{
				skipped++;
			}

			start += len;
		}

		for (var i = 0; i < skipped; i++)
		{
			var zh  = mLon.DasaPeriod(i, out var savya);
			var len = zh.DasaLength();

			var de = new KalaChakraDasaEntry(zh, start, len, 1, savya, zh.ToString());
			al.Add(de);
			start += len;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		var         al = new ArrayList();
		ZodiacHouse zh;
		double      dasaLengthSum = 0;

		var entry = pdi as KalaChakraDasaEntry;

		for (var i = 0; i < 9; i++)
		{
			if (entry.Direct)
			{
				zh = pdi.ZHouse.BhuktiDirect(i, out _);
			}
			else
			{
				zh = pdi.ZHouse.BhuktiIndirect(i, out _);
				
			}
			var dasaLength = zh.DasaLength();
			dasaLengthSum += dasaLength;
		}

		dasaLengthSum = pdi.DasaLength.TotalYears / dasaLengthSum;

		var start = pdi.Start;
		for (var i = 0; i < 9; i++)
		{
			bool direct;
			if (entry.Direct)
			{
				zh = pdi.ZHouse.BhuktiDirect(i, out direct);
			}
			else
			{
				zh = pdi.ZHouse.BhuktiIndirect(i, out direct);
				
			}
			var dasaLength = zh.DasaLength() * dasaLengthSum;
			var name       = pdi.DasaName + " " + zh.Name();
			var de         = new KalaChakraDasaEntry(zh, start, dasaLength, pdi.Level + 1, direct, name);
			start += dasaLength;
			al.Add(de);
		}

		return (al);
	}

	public string Description()
	{
		return "Kalachakra Dasa";
	}

	public object GetOptions()
	{
		return new object();
	}

	public object SetOptions(object o)
	{
		return o;
	}

	public void RecalculateOptions()
	{
	}
}