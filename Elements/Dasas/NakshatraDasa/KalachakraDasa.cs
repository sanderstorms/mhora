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
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Dasas.NakshatraDasa;

public class KalachakraDasa : Dasa, IDasa
{
	private readonly Horoscope _h;
	private          Nakshatra _nakshatra;
	private          double    _dasaLength;

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

		if (cycle == 0)
		{
			_dasaLength = 0;
		}


		for (var i = 0; i < 9; i++)
		{
			var zh  = DasaPeriod(mLon.Value, i, out _);
			var len = DasaLength(zh);
			_dasaLength += len;
		}

		/*
		Angle  nakshatraOffset = mLon.ToNakshatraOffset();
		Angle  unit            = new Angle(3,20, 0.0);
		double navamsa         = mLon.Value                / unit;
		Angle  begin           = Math.Floor(navamsa) * unit;
		var    left            = mLon.Value - begin;

		var bhogyaSarvayu = (left.TotalArcseconds * 100) / 12000;
		*/
		var offsetLength = mLon.ToNakshatraPadaPercentage() / 100.0 * _dasaLength;

		var start = -offsetLength;
		for (var i = 0; i < 9; i++)
		{
			var zh  = DasaPeriod(mLon.Value, i, out var savya);
			var len = DasaLength(zh);
			var de  = new KalaChakraDasaEntry(zh, start, len, 1, savya, zh.ToString());
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
				zh = BhuktiDirect(pdi.ZHouse, i, out _);
			}
			else
			{
				zh = BhuktiIndirect(pdi.ZHouse, i, out _);
				
			}
			var dasaLength = DasaLength(zh);
			dasaLengthSum += dasaLength;
		}

		dasaLengthSum = pdi.DasaLength.TotalYears / dasaLengthSum;

		var start = pdi.Start;
		for (var i = 0; i < 9; i++)
		{
			bool direct;
			if (entry.Direct)
			{
				zh = BhuktiDirect(pdi.ZHouse, i, out direct);
			}
			else
			{
				zh = BhuktiIndirect(pdi.ZHouse, i, out direct);
				
			}
			var dasaLength = DasaLength(zh) * dasaLengthSum;
			var de         = new KalaChakraDasaEntry(zh, start, dasaLength, pdi.Level + 1, direct, "  " + pdi.DasaName + " " + zh);
			start += dasaLength;
			al.Add(de);
		}

		return (al);
	}


	public ZodiacHouse DasaPeriod (Longitude lon, int cycle, out bool savya)
	{
		_nakshatra = lon.ToNakshatra();
		var pada = lon.ToNakshatraPada();
		var zh   = NavamsaRasi(_nakshatra, pada, out _);

		(var nakshatra, pada) = zh.NakshatraPada();
		(nakshatra, pada)     = nakshatra.AddPada(pada + cycle);

		return NavamsaRasi(nakshatra, pada, out savya);
	}

	public ZodiacHouse NavamsaRasi(Nakshatra nakshatra, int pada, out bool savya)
	{
		var dp       = nakshatra.Index().NormalizeInc(1, 6);
		var dpOffset = dp.NormalizeInc(1, 3) - 1;
		var zh       = (ZodiacHouse) (dpOffset * 4 + pada);

		switch (dp)
		{
			case 1:
			case 2:
			case 3:
				savya = true;
				return (zh); //Clockwise (Savya)
			default:
				savya = false;
				return zh.LordsOtherSign();
		}
	}

	public bool IsDirect(Nakshatra nakshatra)
	{
		switch (nakshatra)
		{
			case Nakshatra.Aswini:
			case Nakshatra.Punarvasu:
			case Nakshatra.Hasta:
			case Nakshatra.Moola:
			case Nakshatra.PoorvaBhadra: 
				return true;

			case Nakshatra.Bharani:
			case Nakshatra.Pushya:
			case Nakshatra.Chittra:
			case Nakshatra.PoorvaShada:
			case Nakshatra.UttaraBhadra:
				return true;
				
			case Nakshatra.Krittika:
			case Nakshatra.Aslesha:
			case Nakshatra.Swati:
			case Nakshatra.UttaraShada:
			case Nakshatra.Revati: 
				return true;
		}

		return (false);
	}

	public ZodiacHouse BhuktiDirect(ZodiacHouse zh, int cycle, out bool direct)
	{
		var progression = ((zh.Index() - 1) * 9) + 1 + cycle;
		var bhukti      = (ZodiacHouse) progression.NormalizeInc(1, 12);
		var dp          = (int) Math.Ceiling(progression / 12.0) - 1;
		if ((dp % 2) == 1)
		{
			direct = false;
			return bhukti.LordsOtherSign();
		}

		direct = true;
		return bhukti;

	}

	public ZodiacHouse Indirect(ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Leo: return ZodiacHouse.Can;
			case ZodiacHouse.Can: return ZodiacHouse.Leo;
		}

		return zh.LordsOtherSign();
	}

	public ZodiacHouse Invert(ZodiacHouse zh)
	{
		return (ZodiacHouse) (12 - (zh.Index() - 1));
	}

	public ZodiacHouse BhuktiIndirect(ZodiacHouse zh, int cycle, out bool direct)
	{
		var index       = Indirect(zh).Index();
		var progression = ((index - 1) * 9) + 1 + cycle;
		var bhukti      = (ZodiacHouse) progression.NormalizeInc(1, 12);
		bhukti = Invert(bhukti);
		var dp          = (int) Math.Ceiling(progression / 12.0) - 1;

		if (dp >= 6)
		{
			dp++; //switch in direction
		}

		if ((dp % 2) == 0)
		{
			direct = false;
			return bhukti.LordsOtherSign();
		}
		direct = true;
		return bhukti;
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

	public double DasaLength(ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Ari:
			case ZodiacHouse.Sco: return 7;
			case ZodiacHouse.Tau:
			case ZodiacHouse.Lib: return 16;
			case ZodiacHouse.Gem:
			case ZodiacHouse.Vir: return 9;
			case ZodiacHouse.Can: return 21;
			case ZodiacHouse.Leo: return 5;
			case ZodiacHouse.Sag:
			case ZodiacHouse.Pis: return 10;
			case ZodiacHouse.Cap:
			case ZodiacHouse.Aqu: return 4;
			default: throw new Exception("KalachakraDasa::DasaLength");
		}
	}
}