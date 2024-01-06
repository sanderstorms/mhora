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

using System.Diagnostics;
using Mhora.Tables;
using Mhora.Util;

namespace Mhora.Elements;

public class Tithi
{
	public Tithi(Tables.Tithi.Value _mValue)
	{
		value = (Tables.Tithi.Value) Basics.normalize_inc(1, 30, (int) _mValue);
	}

	public Tables.Tithi.Value value
	{
		get;
		set;
	}


	public override string ToString()
	{
		return EnumDescConverter.GetEnumDescription(value);
	}

	public Tithi add(int i)
	{
		var tnum = Basics.normalize_inc(1, 30, (int) value + i - 1);
		return new Tithi((Tables.Tithi.Value) tnum);
	}

	public Tithi addReverse(int i)
	{
		var tnum = Basics.normalize_inc(1, 30, (int) value - i + 1);
		return new Tithi((Tables.Tithi.Value) tnum);
	}

	public Body.Name getLord()
	{
		// 1 based index starting with prathama
		var t = (int) value;

		//mhora.Log.Debug ("Looking for lord of tithi {0}", t);
		// check for new moon and full moon 
		if (t == 30)
		{
			return Body.Name.Rahu;
		}

		if (t == 15)
		{
			return Body.Name.Saturn;
		}

		// coalesce pakshas
		if (t >= 16)
		{
			t -= 15;
		}

		switch (t)
		{
			case 1:
			case 9: return Body.Name.Sun;
			case 2:
			case 10: return Body.Name.Moon;
			case 3:
			case 11: return Body.Name.Mars;
			case 4:
			case 12: return Body.Name.Mercury;
			case 5:
			case 13: return Body.Name.Jupiter;
			case 6:
			case 14: return Body.Name.Venus;
			case 7: return Body.Name.Saturn;
			case 8: return Body.Name.Rahu;
		}

		Debug.Assert(false, "Tithi::getLord");
		return Body.Name.Sun;
	}

	public Tables.Tithi.NandaType toNandaType()
	{
		// 1 based index starting with prathama
		var t = (int) value;

		// check for new moon and full moon 

		if (t == 30 || t == 15)
		{
			return Tables.Tithi.NandaType.Purna;
		}

		// coalesce pakshas
		if (t >= 16)
		{
			t -= 15;
		}

		switch (t)
		{
			case 1:
			case 6:
			case 11: return Tables.Tithi.NandaType.Nanda;
			case 2:
			case 7:
			case 12: return Tables.Tithi.NandaType.Bhadra;
			case 3:
			case 8:
			case 13: return Tables.Tithi.NandaType.Jaya;
			case 4:
			case 9:
			case 14: return Tables.Tithi.NandaType.Rikta;
			case 5:
			case 10: return Tables.Tithi.NandaType.Purna;
		}

		Debug.Assert(false, "Tithi::toNandaType");
		return Tables.Tithi.NandaType.Nanda;
	}
}