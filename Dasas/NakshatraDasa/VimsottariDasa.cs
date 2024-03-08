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
using Mhora.Calculation;
using Mhora.Components.Property;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Dasas.NakshatraDasa;

public class VimsottariDasa : NakshatraDasa, INakshatraDasa
{
	public Horoscope   Horoscope;
	public UserOptions Options;

	public VimsottariDasa(Horoscope h)
	{
		var grahas = h.FindGrahas(DivisionType.BhavaPada);
		var rules  = h.RulesVimsottariGraha();
		Common    = this;
		Options   = new UserOptions();
		Horoscope = h;

		var stronger = grahas.Stronger(Body.Moon, Body.Lagna, false, rules, out _);

		if (stronger == Body.Lagna)
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

	public TimeOffset LengthOfDasa(Body plt)
	{
		return DasaLength(plt);
	}

	public Body LordOfNakshatra(Nakshatra n)
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

	private Body NextDasaLordHelper(Body b)
	{
		switch (b)
		{
			case Body.Sun:     return Body.Moon;
			case Body.Moon:    return Body.Mars;
			case Body.Mars:    return Body.Rahu;
			case Body.Rahu:    return Body.Jupiter;
			case Body.Jupiter: return Body.Saturn;
			case Body.Saturn:  return Body.Mercury;
			case Body.Mercury: return Body.Ketu;
			case Body.Ketu:    return Body.Venus;
			case Body.Venus:   return Body.Sun;
		}

		Trace.Assert(false, "VimsottariDasa::NextDasaLord");
		return Body.Lagna;
	}

	public static double DasaLength(Body plt)
	{
		switch (plt)
		{
			case Body.Sun:     return 6;
			case Body.Moon:    return 10;
			case Body.Mars:    return 7;
			case Body.Rahu:    return 18;
			case Body.Jupiter: return 16;
			case Body.Saturn:  return 19;
			case Body.Mercury: return 17;
			case Body.Ketu:    return 7;
			case Body.Venus:   return 20;
		}

		Trace.Assert(false, "Vimsottari::LengthOfDasa");
		return 0;
	}

	public static Body NakshatraLord(Nakshatra n)
	{
		var lords = new Body[9]
		{
			Body.Mercury,
			Body.Ketu,
			Body.Venus,
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Rahu,
			Body.Jupiter,
			Body.Saturn
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

		public Division      Div = new(DivisionType.Rasi);
		public int           NakshatraOffset;
		public Body     StartGraha;
		public StartBodyType UserStartGraha;


		[PGDisplayName("Vargas")]
		public DivisionType Varga
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
						StartGraha      = Body.Lagna;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Jupiter:
						StartGraha      = Body.Jupiter;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Moon:
						StartGraha      = Body.Moon;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Utpanna:
						StartGraha      = Body.Moon;
						NakshatraOffset = 5;
						break;
					case StartBodyType.Kshema:
						StartGraha      = Body.Moon;
						NakshatraOffset = 4;
						break;
					case StartBodyType.Aadhaana:
						StartGraha      = Body.Moon;
						NakshatraOffset = 8;
						break;
					case StartBodyType.Maandi:
						StartGraha      = Body.Maandi;
						NakshatraOffset = 1;
						break;
					case StartBodyType.Gulika:
						StartGraha      = Body.Gulika;
						NakshatraOffset = 1;
						break;
				}
			}
		}

		public object Clone()
		{
			var options = new UserOptions
			{
				StartGraha = StartGraha,
				NakshatraOffset = NakshatraOffset,
				SeedBody = SeedBody,
				Div = (Division) Div.Clone()
			};
			return options;
		}
	}
}