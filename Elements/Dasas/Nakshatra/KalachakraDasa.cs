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

		var zAri = new ZodiacHouse(ZodiacHouse.Name.Ari);
		var zSag = new ZodiacHouse(ZodiacHouse.Name.Sag);
		for (var i = 0; i < 12; i++)
		{
			mzhSavya[i] = zAri.add(i + 1);
			mzhSavya[i               + 12] = mzhSavya[i].LordsOtherSign();
			mzhApasavya[i]                 = zSag.add(i + 1);
			mzhApasavya[i                               + 12] = mzhApasavya[i].LordsOtherSign();
		}
	}

	public double paramAyus()
	{
		return 144;
	}

	public ArrayList Dasa(int cycle)
	{
		var dRasi = new Division(Basics.DivisionType.Rasi);
		var mLon  = h.getPosition(Body.Name.Moon).extrapolateLongitude(dRasi);

		var           offset  = 0;
		ZodiacHouse[] zhOrder = null;
		initHelper(mLon, ref zhOrder, ref offset);

		var al = new ArrayList();

		double dasa_length_sum = 0;
		for (var i = 0; i < 9; i++)
		{
			var zhCurr      = zhOrder[(int) Basics.normalize_exc_lower(0, 24, offset + i)];
			var dasa_length = DasaLength(zhCurr);
			var de          = new DasaEntry(zhCurr.value, dasa_length_sum, dasa_length, 1, zhCurr.value.ToString());
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

	public GroupType NakshatraToGroup(Elements.Nakshatra n)
	{
		switch (n.value)
		{
			case Elements.Nakshatra.Name.Aswini:
			case Elements.Nakshatra.Name.Krittika:
			case Elements.Nakshatra.Name.Punarvasu:
			case Elements.Nakshatra.Name.Aslesha:
			case Elements.Nakshatra.Name.Hasta:
			case Elements.Nakshatra.Name.Swati:
			case Elements.Nakshatra.Name.Moola:
			case Elements.Nakshatra.Name.UttaraShada:
			case Elements.Nakshatra.Name.PoorvaBhadra: return GroupType.Savya;
			case Elements.Nakshatra.Name.Bharani:
			case Elements.Nakshatra.Name.Pushya:
			case Elements.Nakshatra.Name.Chittra:
			case Elements.Nakshatra.Name.PoorvaShada:
			case Elements.Nakshatra.Name.Revati: return GroupType.SavyaMirrored;
			case Elements.Nakshatra.Name.Rohini:
			case Elements.Nakshatra.Name.Makha:
			case Elements.Nakshatra.Name.Vishaka:
			case Elements.Nakshatra.Name.Sravana: return GroupType.Apasavya;
			default: return GroupType.ApasavyaMirrored;
		}

		switch ((int) n.value % 6)
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
		switch (zh.value)
		{
			case ZodiacHouse.Name.Ari:
			case ZodiacHouse.Name.Sco: return 7;
			case ZodiacHouse.Name.Tau:
			case ZodiacHouse.Name.Lib: return 16;
			case ZodiacHouse.Name.Gem:
			case ZodiacHouse.Name.Vir: return 9;
			case ZodiacHouse.Name.Can: return 21;
			case ZodiacHouse.Name.Leo: return 5;
			case ZodiacHouse.Name.Sag:
			case ZodiacHouse.Name.Pis: return 10;
			case ZodiacHouse.Name.Cap:
			case ZodiacHouse.Name.Aqu: return 4;
			default: throw new Exception("KalachakraDasa::DasaLength");
		}
	}
}