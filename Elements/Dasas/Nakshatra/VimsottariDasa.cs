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
	public Horoscope   Horoscope;
	public UserOptions Options;

	public VimsottariDasa(Horoscope h)
	{
		Common    = this;
		Options   = new UserOptions();
		Horoscope = h;

		var fsGraha = new FindStronger(h, new Division(Vargas.DivisionType.BhavaPada), FindStronger.RulesVimsottariGraha(h));
		var stronger = fsGraha.StrongerGraha(Body.BodyType.Moon, Body.BodyType.Lagna, false);

		if (stronger == Body.BodyType.Lagna)
		{
			Options.SeedBody = UserOptions.StartBodyType.Lagna;
		}
		else
		{
			Options.SeedBody = UserOptions.StartBodyType.Moon;
		}

		h.Changed += ChangedHoroscope;
	}

	public override object GetOptions()
	{
		return Options.Clone();
	}

	public override object SetOptions(object a)
	{
		var uo           = (UserOptions) a;
		var bRecalculate = false;
		if (Options.SeedBody != uo.SeedBody)
		{
			Options.SeedBody = uo.SeedBody;
			bRecalculate     = true;
		}

		if (Options.Div != uo.Div)
		{
			Options.Div  = uo.Div;
			bRecalculate = true;
		}

		if (bRecalculate && RecalculateEvent != null)
		{
			RecalculateEvent();
		}

		return Options.Clone();
	}

	public ArrayList Dasa(int cycle)
	{
		return _Dasa(Horoscope.GetPosition(Options.StartGraha).ExtrapolateLongitude(Options.Div), Options.NakshatraOffset, cycle);
	}

	public ArrayList AntarDasa(DasaEntry di)
	{
		return _AntarDasa(di);
	}

	public string Description()
	{
		return "Vimsottari Dasa Seeded from " + Options.SeedBody;
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
		var uoNew = (UserOptions) Options.Clone();
		uoNew.Div = (Division) div.Clone();
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
		var nakVal = (int) n % 9;
		return lords[nakVal];
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

		public Division      Div = new(Vargas.DivisionType.Rasi);
		public int           NakshatraOffset;
		public Body.BodyType     StartGraha;
		public StartBodyType UserStartGraha;


		[PGDisplayName("Vargas")]
		public Vargas.DivisionType Varga
		{
			get => Div.MultipleDivisions[0].Varga;
			set => Div = new Division(value);
		}

		[PGDisplayName("Seed Nakshatra")]
		public StartBodyType SeedBody
		{
			get => UserStartGraha;
			set
			{
				UserStartGraha = value;
				switch (value)
				{
					case StartBodyType.Lagna:
						StartGraha      = Body.BodyType.Lagna;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Jupiter:
						StartGraha      = Body.BodyType.Jupiter;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Moon:
						StartGraha      = Body.BodyType.Moon;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Utpanna:
						StartGraha      = Body.BodyType.Moon;
						NakshatraOffset = 5;
						break;
					case StartBodyType.Kshema:
						StartGraha      = Body.BodyType.Moon;
						NakshatraOffset = 4;
						break;
					case StartBodyType.Aadhaana:
						StartGraha      = Body.BodyType.Moon;
						NakshatraOffset = 8;
						break;
					case StartBodyType.Maandi:
						StartGraha      = Body.BodyType.Maandi;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Gulika:
						StartGraha      = Body.BodyType.Gulika;
						NakshatraOffset = 1;
						break;
				}
			}
		}

		public object Clone()
		{
			var options = new UserOptions();
			options.StartGraha      = StartGraha;
			options.NakshatraOffset = NakshatraOffset;
			options.SeedBody         = SeedBody;
			options.Div              = (Division) Div.Clone();
			return options;
		}
	}
}