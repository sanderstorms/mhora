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
	public class DasaInfo
	{
		public ZodiacHouse DasaPack { get;}
		public int         Cycle    { get;}
		public ZodiacHouse Dasa   { get; }

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
			Dasa = dasa;
			Cycle = cycle;
			Bhukti = bhukti;
		}	
	}

	private static Dictionary<DasaInfo, Gati> _directGati   = new (); 
	private static Dictionary<DasaInfo, Gati> _indirectGati = new (); 

	public KalachakraDasa(Horoscope h)
	{
		_h = h;
		if (_directGati.Count == 0)
		{
			_directGati.Add(new DasaInfo(ZodiacHouse.Ari, 1, ZodiacHouse.Ari), Gati.Co | Gati.FF);
			_directGati.Add(new DasaInfo(ZodiacHouse.Ari, 9, ZodiacHouse.Ari), Gati.Ps);

			_directGati.Add(new DasaInfo(ZodiacHouse.Tau, 1, ZodiacHouse.Cap), Gati.Co | Gati.QJ);
			_directGati.Add(new DasaInfo(ZodiacHouse.Tau, 4, ZodiacHouse.Sco), Gati.QJ);
			_directGati.Add(new DasaInfo(ZodiacHouse.Tau, 7, ZodiacHouse.Can), Gati.FF);
			_directGati.Add(new DasaInfo(ZodiacHouse.Tau, 8, ZodiacHouse.Leo), Gati.Re);
			_directGati.Add(new DasaInfo(ZodiacHouse.Tau, 9, ZodiacHouse.Gem), Gati.Ps | Gati.FF);
		}
	}

	public Gati DasaGati(ZodiacHouse dasa, int cycle)
	{
		var gati = Gati.None;

		var dp = (1 << (dasa.Index() - 1));

		switch (cycle)
		{
			case 1:	//deva
			{
				gati = Gati.Co;
				if ((dp & 0b100110011001) == 0)
				{
					gati |= Gati.QJ;
				}
				else
				{
					gati |= Gati.FF;
				}
			}
			break;

			case 2:
			{
				if (dasa == ZodiacHouse.Sco)
				{
					gati = Gati.Re;
				}
			}
			break;

			case 3:
			{
				switch (dasa)
				{
					case ZodiacHouse.Tau:
					case ZodiacHouse.Leo:
					case ZodiacHouse.Vir:
					case ZodiacHouse.Cap:
						return Gati.Mark;

					case ZodiacHouse.Sco:
						return Gati.FF;
				}
			}
			break;

			case 4:
			{
				switch (dasa)
				{
					case ZodiacHouse.Tau:
					case ZodiacHouse.Vir:
					case ZodiacHouse.Cap:
						return Gati.QJ;
					case ZodiacHouse.Leo:
						return Gati.FF;
				}
			}
			break;

			case 6:
			{
				switch (dasa)
				{
					case ZodiacHouse.Tau:
					case ZodiacHouse.Gem:
					case ZodiacHouse.Lib:
					case ZodiacHouse.Cap:
						return Gati.Mark;
				}
			}
			break;

			case 7:
			{
				switch (dasa)
				{
					case ZodiacHouse.Tau:
					case ZodiacHouse.Cap:
						return Gati.FF;
					case ZodiacHouse.Gem:
					case ZodiacHouse.Lib:
						return Gati.QJ;
				}

				break;
			}

			case 8:
			{
				switch (dasa)
				{
					case ZodiacHouse.Tau:
					case ZodiacHouse.Cap:
						return Gati.Re;
				}
			}
			break;

			case 9: //jeeva
			{
				gati = Gati.Ps;
				if ((dp & 0b100110011001) == 0)
				{
					gati |= Gati.QJ;
				}
				else
				{
					gati |= Gati.FF;
				}

				if ((dp & 0b100101111011) == 0)
				{
					gati |= Gati.Mark;

				}

			}
			break;
		}

		return (gati);
	}

	public double ParamAyus()
	{
		return 144;
	}

	[Flags]
	public enum Gati : byte
	{
		None = 0,
		SF   = 0x01, // Systemic feature
		QJ   = 0x02, // Qwantum Jump (Simhavalokan)
		FF   = 0x04, // Fast forward (Mandooki)
		Re   = 0x08, // Reorientation (Markati)
		Co   = 0x10, // Corporal
		Ps   = 0x20, // Psychical
		Mark = 0x40, // Mark next Gati
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
			var zh  = DasaPeriod(mLon.Value, i, out _);
			var len = DasaLength(zh);
			dasaLength += len;
		}

		/*
		Angle  nakshatraOffset = mLon.ToNakshatraOffset();
		Angle  unit            = new Angle(3,20, 0.0);
		double navamsa         = mLon.Value                / unit;
		Angle  begin           = Math.Floor(navamsa) * unit;
		var    left            = mLon.Value - begin;

		var bhogyaSarvayu = (left.TotalArcseconds * 100) / 12000;
		*/
		var offsetLength = mLon.ToNakshatraPadaPercentage() / 100.0 * dasaLength;

		var start   = -offsetLength;
		int skipped = 0;
		for (var i = 0; i < 9; i++)
		{
			var zh  = DasaPeriod(mLon.Value, i, out var savya);
			var len = DasaLength(zh);

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
			var zh  = DasaPeriod(mLon.Value, i, out var savya);
			var len = DasaLength(zh);

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
			var name       = pdi.DasaName + " " + zh.Name();
			var de         = new KalaChakraDasaEntry(zh, start, dasaLength, pdi.Level + 1, direct, name);
			start += dasaLength;
			al.Add(de);
		}

		return (al);
	}

	public static ZodiacHouse DasaPeriod (Longitude lon, int cycle, out bool savya)
	{
		var nakshatra = lon.ToNakshatra();
		var pada = lon.ToNakshatraPada();
		var zh   = NavamsaRasi(nakshatra, pada, out _);

		(nakshatra, pada) = zh.NakshatraPada();
		(nakshatra, pada) = nakshatra.AddPada(pada + cycle);

		return NavamsaRasi(nakshatra, pada, out savya);
	}

	public static ZodiacHouse NavamsaRasi(Nakshatra nakshatra, int pada, out bool savya)
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

	public static ZodiacHouse Indirect(ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Leo: return ZodiacHouse.Can;
			case ZodiacHouse.Can: return ZodiacHouse.Leo;
		}

		return zh.LordsOtherSign();
	}

	public static ZodiacHouse Invert(ZodiacHouse zh)
	{
		return (ZodiacHouse) (12 - (zh.Index() - 1));
	}

	public static int DasaPackDirect(ZodiacHouse zh, int cycle, out ZodiacHouse bhukti)
	{
		var progression = ((zh.Index() - 1) * 9)                 + 1 + cycle;
		var dp          = (int) Math.Ceiling(progression / 12.0) - 1;
		
		bhukti      = (ZodiacHouse) progression.NormalizeInc(1, 12);
		return (dp);
	}

	public static ZodiacHouse BhuktiDirect(ZodiacHouse zh, int cycle, out bool direct)
	{
		var dp = DasaPackDirect(zh, cycle, out ZodiacHouse bhukti);
		if ((dp % 2) == 1)
		{
			direct = false;
			return bhukti.LordsOtherSign();
		}

		direct = true;
		return bhukti;

	}

	public static int DasaPackIndirect(ZodiacHouse zh, int cycle, out ZodiacHouse bhukti)
	{
		var index       = Indirect(zh).Index();
		var progression = ((index - 1) * 9)  + 1 + cycle;

		var dp          = (int) Math.Ceiling(progression / 12.0) - 1;
		bhukti = (ZodiacHouse)progression.NormalizeInc(1, 12);
		bhukti = Invert(bhukti);
		return (dp);
	}

	public static ZodiacHouse BhuktiIndirect(ZodiacHouse zh, int cycle, out bool direct)
	{
		var dp = DasaPackIndirect(zh, cycle, out var bhukti);

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