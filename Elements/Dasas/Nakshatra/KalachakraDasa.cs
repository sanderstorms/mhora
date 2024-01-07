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
using Mhora.Components.Dasa;
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

	private readonly Horoscope     h;
	private readonly ZodiacHouse[] mzhApasavya = new ZodiacHouse[24];

	private readonly ZodiacHouse[] mzhSavya = new ZodiacHouse[24];

	public KalachakraDasa(Horoscope _h)
	{
		h = _h;

		var zAri = new ZodiacHouse(ZodiacHouse.Rasi.Ari);
		var zSag = new ZodiacHouse(ZodiacHouse.Rasi.Sag);
		for (var i = 0; i < 12; i++)
		{
			mzhSavya[i] = zAri.Add(i + 1);
			mzhSavya[i               + 12] = mzhSavya[i].LordsOtherSign();
			mzhApasavya[i]                 = zSag.Add(i + 1);
			mzhApasavya[i                               + 12] = mzhApasavya[i].LordsOtherSign();
		}
	}

	public double paramAyus()
	{
		return 144;
	}

	public ArrayList Dasa(int cycle)
	{
		var dRasi = new Division(Vargas.DivisionType.Rasi);
		var mLon  = h.getPosition(Body.BodyType.Moon).extrapolateLongitude(dRasi);

		var           offset  = 0;
		ZodiacHouse[] zhOrder = null;
		initHelper(mLon, ref zhOrder, ref offset);

		var al = new ArrayList();

		double dasa_length_sum = 0;
		for (var i = 0; i < 9; i++)
		{
			var zhCurr      = zhOrder[(int) Basics.normalize_exc_lower(0, 24, offset + i)];
			var dasa_length = DasaLength(zhCurr);
			var de          = new DasaEntry(zhCurr.Sign, dasa_length_sum, dasa_length, 1, zhCurr.Sign.ToString());
			al.Add(de);
			dasa_length_sum += dasa_length;
		}

		var offsetLength = mLon.toNakshatraPadaPercentage() / 100.0 * dasa_length_sum;

		foreach (DasaEntry de in al)
		{
			de.startUT -= offsetLength;
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

	public void recalculateOptions()
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

	private void initHelper(Longitude lon, ref ZodiacHouse[] mzhOrder, ref int offset)
	{
		var grp  = NakshatraToGroup(lon.toNakshatra());
		var pada = lon.toNakshatraPada();

		switch (grp)
		{
			case GroupType.Savya:
			case GroupType.SavyaMirrored:
				mzhOrder = mzhSavya;
				break;
			default:
				mzhOrder = mzhApasavya;
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

		offset = (int) Basics.normalize_exc_lower(0, 24, (pada - 1) * 9 + offset);
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