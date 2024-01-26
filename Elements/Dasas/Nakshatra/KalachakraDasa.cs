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
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Elements.Dasas.Nakshatra;

public class KalachakraDasa : Dasa, IDasa
{
	public enum GroupType
	{
		Savya,
		SavyaMirrored,
		Apasavya,
		ApasavyaMirrored
	}

	private readonly Horoscope     _h;
	private readonly ZodiacHouse[] _mzhApasavya = new ZodiacHouse[24];

	private readonly ZodiacHouse[] _mzhSavya = new ZodiacHouse[24];

	public KalachakraDasa(Horoscope h)
	{
		this._h = h;

		var zAri = new ZodiacHouse(ZodiacHouse.Rasi.Ari);
		var zSag = new ZodiacHouse(ZodiacHouse.Rasi.Sag);
		for (var i = 0; i < 12; i++)
		{
			_mzhSavya[i] = zAri.Add(i + 1);
			_mzhSavya[i               + 12] = _mzhSavya[i].LordsOtherSign();
			_mzhApasavya[i]                 = zSag.Add(i + 1);
			_mzhApasavya[i                               + 12] = _mzhApasavya[i].LordsOtherSign();
		}
	}

	public double ParamAyus()
	{
		return 144;
	}

	public ArrayList Dasa(int cycle)
	{
		var dRasi = new Division(Vargas.DivisionType.Rasi);
		var mLon  = _h.GetPosition(Body.BodyType.Moon).ExtrapolateLongitude(dRasi);

		var           offset  = 0;
		ZodiacHouse[] zhOrder = null;
		InitHelper(mLon, ref zhOrder, ref offset);

		var al = new ArrayList();

		double dasaLengthSum = 0;
		for (var i = 0; i < 9; i++)
		{
			var zhCurr      = zhOrder[(int) Basics.NormalizeExcLower(offset + i, 0, 24)];
			var dasaLength = DasaLength(zhCurr);
			var de          = new DasaEntry(zhCurr.Sign, dasaLengthSum, dasaLength, 1, zhCurr.Sign.ToString());
			al.Add(de);
			dasaLengthSum += dasaLength;
		}

		var offsetLength = mLon.ToNakshatraPadaPercentage() / 100.0 * dasaLengthSum;

		foreach (DasaEntry de in al)
		{
			de.StartUt -= offsetLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		return new ArrayList();
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

	public GroupType NakshatraToGroup(Nakshatras.Nakshatra n)
	{
		switch (n)
		{
			case Nakshatras.Nakshatra.Aswini:
			case Nakshatras.Nakshatra.Krittika:
			case Nakshatras.Nakshatra.Punarvasu:
			case Nakshatras.Nakshatra.Aslesha:
			case Nakshatras.Nakshatra.Hasta:
			case Nakshatras.Nakshatra.Swati:
			case Nakshatras.Nakshatra.Moola:
			case Nakshatras.Nakshatra.UttaraShada:
			case Nakshatras.Nakshatra.PoorvaBhadra: return GroupType.Savya;
			case Nakshatras.Nakshatra.Bharani:
			case Nakshatras.Nakshatra.Pushya:
			case Nakshatras.Nakshatra.Chittra:
			case Nakshatras.Nakshatra.PoorvaShada:
			case Nakshatras.Nakshatra.Revati: return GroupType.SavyaMirrored;
			case Nakshatras.Nakshatra.Rohini:
			case Nakshatras.Nakshatra.Makha:
			case Nakshatras.Nakshatra.Vishaka:
			case Nakshatras.Nakshatra.Sravana: return GroupType.Apasavya;
			default: return GroupType.ApasavyaMirrored;
		}

		switch ((int) n % 6)
		{
			case 1:  return GroupType.Savya;
			case 2:  return GroupType.SavyaMirrored;
			case 3:  return GroupType.Savya;
			case 4:  return GroupType.Apasavya;
			case 5:  return GroupType.ApasavyaMirrored;
			default: return GroupType.ApasavyaMirrored;
		}
	}

	private void InitHelper(Longitude lon, ref ZodiacHouse[] mzhOrder, ref int offset)
	{
		var grp  = NakshatraToGroup(lon.ToNakshatra());
		var pada = lon.ToNakshatraPada();

		switch (grp)
		{
			case GroupType.Savya:
			case GroupType.SavyaMirrored:
				mzhOrder = _mzhSavya;
				break;
			default:
				mzhOrder = _mzhApasavya;
				break;
		}

		switch (grp)
		{
			case GroupType.Savya:
			case GroupType.Apasavya:
				offset = 0;
				break;
			default:
				offset = 12;
				break;
		}

		offset = (int) Basics.NormalizeExcLower((pada - 1) * 9 + offset, 0, 24);
	}

	public double DasaLength(ZodiacHouse zh)
	{
		switch (zh.Sign)
		{
			case ZodiacHouse.Rasi.Ari:
			case ZodiacHouse.Rasi.Sco: return 7;
			case ZodiacHouse.Rasi.Tau:
			case ZodiacHouse.Rasi.Lib: return 16;
			case ZodiacHouse.Rasi.Gem:
			case ZodiacHouse.Rasi.Vir: return 9;
			case ZodiacHouse.Rasi.Can: return 21;
			case ZodiacHouse.Rasi.Leo: return 5;
			case ZodiacHouse.Rasi.Sag:
			case ZodiacHouse.Rasi.Pis: return 10;
			case ZodiacHouse.Rasi.Cap:
			case ZodiacHouse.Rasi.Aqu: return 4;
			default: throw new Exception("KalachakraDasa::DasaLength");
		}
	}
}