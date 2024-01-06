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
using System.ComponentModel;
using System.Diagnostics;
using Mhora.Components.Property;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Tables;
using Mhora.Util;

namespace Mhora.Components.Dasa.Nakshatra;

public class VimsottariDasa : NakshatraDasa, INakshatraDasa
{
	public Horoscope   horoscope;
	public UserOptions options;

	public VimsottariDasa(Horoscope h)
	{
		common    = this;
		options   = new UserOptions();
		horoscope = h;

		var fs_graha = new FindStronger(h, new Division(Basics.DivisionType.BhavaPada), FindStronger.RulesVimsottariGraha(h));
		var stronger = fs_graha.StrongerGraha(Elements.Body.Name.Moon, Elements.Body.Name.Lagna, false);

		if (stronger == Elements.Body.Name.Lagna)
		{
			options.SeedBody = UserOptions.StartBodyType.Lagna;
		}
		else
		{
			options.SeedBody = UserOptions.StartBodyType.Moon;
		}

		h.Changed += ChangedHoroscope;
	}

	public override object GetOptions()
	{
		return options.Clone();
	}

	public override object SetOptions(object a)
	{
		var uo           = (UserOptions) a;
		var bRecalculate = false;
		if (options.SeedBody != uo.SeedBody)
		{
			options.SeedBody = uo.SeedBody;
			bRecalculate     = true;
		}

		if (options.div != uo.div)
		{
			options.div  = uo.div;
			bRecalculate = true;
		}

		if (bRecalculate && RecalculateEvent != null)
		{
			RecalculateEvent();
		}

		return options.Clone();
	}

	public ArrayList Dasa(int cycle)
	{
		return _Dasa(horoscope.getPosition(options.start_graha).extrapolateLongitude(options.div), options.nakshatra_offset, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Vimsottari Dasa Seeded from " + options.SeedBody;
	}

	public double paramAyus()
	{
		return 120.0;
	}

	public int numberOfDasaItems()
	{
		return 9;
	}

	public DasaEntry nextDasaLord(DasaEntry di)
	{
		return new DasaEntry(nextDasaLordHelper(di.graha), 0, 0, di.level, string.Empty);
	}

	public double lengthOfDasa(Elements.Body.Name plt)
	{
		return LengthOfDasa(plt);
	}

	public Elements.Body.Name lordOfNakshatra(Elements.Nakshatra n)
	{
		return LordOfNakshatra(n);
	}

	public new void DivisionChanged(Division div)
	{
		var uoNew = (UserOptions) options.Clone();
		uoNew.div = (Division) div.Clone();
		SetOptions(uoNew);
	}

	public void ChangedHoroscope(object o)
	{
		var h = (Horoscope) o;
		OnChanged();
	}

	private Elements.Body.Name nextDasaLordHelper(Elements.Body.Name b)
	{
		switch (b)
		{
			case Elements.Body.Name.Sun:     return Elements.Body.Name.Moon;
			case Elements.Body.Name.Moon:    return Elements.Body.Name.Mars;
			case Elements.Body.Name.Mars:    return Elements.Body.Name.Rahu;
			case Elements.Body.Name.Rahu:    return Elements.Body.Name.Jupiter;
			case Elements.Body.Name.Jupiter: return Elements.Body.Name.Saturn;
			case Elements.Body.Name.Saturn:  return Elements.Body.Name.Mercury;
			case Elements.Body.Name.Mercury: return Elements.Body.Name.Ketu;
			case Elements.Body.Name.Ketu:    return Elements.Body.Name.Venus;
			case Elements.Body.Name.Venus:   return Elements.Body.Name.Sun;
		}

		Trace.Assert(false, "VimsottariDasa::nextDasaLord");
		return Elements.Body.Name.Lagna;
	}

	public static double LengthOfDasa(Elements.Body.Name plt)
	{
		switch (plt)
		{
			case Elements.Body.Name.Sun:     return 6;
			case Elements.Body.Name.Moon:    return 10;
			case Elements.Body.Name.Mars:    return 7;
			case Elements.Body.Name.Rahu:    return 18;
			case Elements.Body.Name.Jupiter: return 16;
			case Elements.Body.Name.Saturn:  return 19;
			case Elements.Body.Name.Mercury: return 17;
			case Elements.Body.Name.Ketu:    return 7;
			case Elements.Body.Name.Venus:   return 20;
		}

		Trace.Assert(false, "Vimsottari::lengthOfDasa");
		return 0;
	}

	public static Elements.Body.Name LordOfNakshatra(Elements.Nakshatra n)
	{
		var lords = new Elements.Body.Name[9]
		{
			Elements.Body.Name.Mercury,
			Elements.Body.Name.Ketu,
			Elements.Body.Name.Venus,
			Elements.Body.Name.Sun,
			Elements.Body.Name.Moon,
			Elements.Body.Name.Mars,
			Elements.Body.Name.Rahu,
			Elements.Body.Name.Jupiter,
			Elements.Body.Name.Saturn
		};
		var nak_val = (int) n.value % 9;
		return lords[nak_val];
	}

	public class UserOptions : ICloneable
	{
		[TypeConverter(typeof(EnumDescConverter))]
		public enum StartBodyType
		{
			[Description("Lagna's sphuta")]
			Lagna,

			[Description("Moon's sphuta")]
			Moon,

			[Description("Jupiter's sphuta")]
			Jupiter,

			[Description("Utpanna - 5th tara from Moon's sphuta")]
			Utpanna,

			[Description("Kshema - 4th tara from Moon's sphuta")]
			Kshema,

			[Description("Adhana - 8th tara from Moon's sphuta")]
			Aadhaana,

			[Description("Mandi's sphuta")]
			Maandi,

			[Description("Gulika's sphuta")]
			Gulika
		}

		public Division           div = new(Basics.DivisionType.Rasi);
		public int                nakshatra_offset;
		public Elements.Body.Name start_graha;
		public StartBodyType      user_start_graha;


		[PGDisplayName("Varga")]
		public Basics.DivisionType Varga
		{
			get => div.MultipleDivisions[0].Varga;
			set => div = new Division(value);
		}

		[PGDisplayName("Seed Nakshatra")]
		public StartBodyType SeedBody
		{
			get => user_start_graha;
			set
			{
				user_start_graha = value;
				switch (value)
				{
					case StartBodyType.Lagna:
						start_graha      = Elements.Body.Name.Lagna;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Jupiter:
						start_graha      = Elements.Body.Name.Jupiter;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Moon:
						start_graha      = Elements.Body.Name.Moon;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Utpanna:
						start_graha      = Elements.Body.Name.Moon;
						nakshatra_offset = 5;
						break;
					case StartBodyType.Kshema:
						start_graha      = Elements.Body.Name.Moon;
						nakshatra_offset = 4;
						break;
					case StartBodyType.Aadhaana:
						start_graha      = Elements.Body.Name.Moon;
						nakshatra_offset = 8;
						break;
					case StartBodyType.Maandi:
						start_graha      = Elements.Body.Name.Maandi;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Gulika:
						start_graha      = Elements.Body.Name.Gulika;
						nakshatra_offset = 1;
						break;
				}
			}
		}

		public object Clone()
		{
			var options = new UserOptions();
			options.start_graha      = start_graha;
			options.nakshatra_offset = nakshatra_offset;
			options.SeedBody         = SeedBody;
			options.div              = (Division) div.Clone();
			return options;
		}
	}
}