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
using Mhora.Elements.Calculation;
using Mhora.Tables;
using Mhora.Util;

namespace Mhora.Elements.Dasas.Nakshatra;

public class VimsottariDasa : NakshatraDasa, INakshatraDasa
{
	public Horoscope   horoscope;
	public UserOptions options;

	public VimsottariDasa(Horoscope h)
	{
		common    = this;
		options   = new UserOptions();
		horoscope = h;

		var fs_graha = new FindStronger(h, new Division(Vargas.DivisionType.BhavaPada), FindStronger.RulesVimsottariGraha(h));
		var stronger = fs_graha.StrongerGraha(Body.BodyType.Moon, Body.BodyType.Lagna, false);

		if (stronger == Body.BodyType.Lagna)
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
		return _Dasa(horoscope.GetPosition(options.start_graha).ExtrapolateLongitude(options.div), options.nakshatra_offset, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Vimsottari Dasa Seeded from " + options.SeedBody;
	}

	public double ParamAyus()
	{
		return 120.0;
	}

	public int NumberOfDasaItems()
	{
		return 9;
	}

	public DasaEntry NextDasaLord(DasaEntry di)
	{
		return new DasaEntry(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);
	}

	public double LengthOfDasa(Body.BodyType plt)
	{
		return DasaLength(plt);
	}

	public Body.BodyType LordOfNakshatra(Nakshatras.Nakshatra n)
	{
		return NakshatraLord(n);
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

	private Body.BodyType NextDasaLordHelper(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return Body.BodyType.Moon;
			case Body.BodyType.Moon:    return Body.BodyType.Mars;
			case Body.BodyType.Mars:    return Body.BodyType.Rahu;
			case Body.BodyType.Rahu:    return Body.BodyType.Jupiter;
			case Body.BodyType.Jupiter: return Body.BodyType.Saturn;
			case Body.BodyType.Saturn:  return Body.BodyType.Mercury;
			case Body.BodyType.Mercury: return Body.BodyType.Ketu;
			case Body.BodyType.Ketu:    return Body.BodyType.Venus;
			case Body.BodyType.Venus:   return Body.BodyType.Sun;
		}

		Trace.Assert(false, "VimsottariDasa::NextDasaLord");
		return Body.BodyType.Lagna;
	}

	public static double DasaLength(Body.BodyType plt)
	{
		switch (plt)
		{
			case Body.BodyType.Sun:     return 6;
			case Body.BodyType.Moon:    return 10;
			case Body.BodyType.Mars:    return 7;
			case Body.BodyType.Rahu:    return 18;
			case Body.BodyType.Jupiter: return 16;
			case Body.BodyType.Saturn:  return 19;
			case Body.BodyType.Mercury: return 17;
			case Body.BodyType.Ketu:    return 7;
			case Body.BodyType.Venus:   return 20;
		}

		Trace.Assert(false, "Vimsottari::LengthOfDasa");
		return 0;
	}

	public static Body.BodyType NakshatraLord(Nakshatras.Nakshatra n)
	{
		var lords = new Body.BodyType[9]
		{
			Body.BodyType.Mercury,
			Body.BodyType.Ketu,
			Body.BodyType.Venus,
			Body.BodyType.Sun,
			Body.BodyType.Moon,
			Body.BodyType.Mars,
			Body.BodyType.Rahu,
			Body.BodyType.Jupiter,
			Body.BodyType.Saturn
		};
		var nak_val = (int) n % 9;
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

		public Division      div = new(Vargas.DivisionType.Rasi);
		public int           nakshatra_offset;
		public Body.BodyType     start_graha;
		public StartBodyType user_start_graha;


		[PGDisplayName("Vargas")]
		public Vargas.DivisionType Varga
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
						start_graha      = Body.BodyType.Lagna;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Jupiter:
						start_graha      = Body.BodyType.Jupiter;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Moon:
						start_graha      = Body.BodyType.Moon;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Utpanna:
						start_graha      = Body.BodyType.Moon;
						nakshatra_offset = 5;
						break;
					case StartBodyType.Kshema:
						start_graha      = Body.BodyType.Moon;
						nakshatra_offset = 4;
						break;
					case StartBodyType.Aadhaana:
						start_graha      = Body.BodyType.Moon;
						nakshatra_offset = 8;
						break;
					case StartBodyType.Maandi:
						start_graha      = Body.BodyType.Maandi;
						nakshatra_offset = 1;
						break;
					case StartBodyType.Gulika:
						start_graha      = Body.BodyType.Gulika;
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