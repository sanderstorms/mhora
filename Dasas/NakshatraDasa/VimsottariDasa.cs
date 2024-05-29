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

	public override object GetOptions() => Options.Clone();

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

	public List<DasaEntry> Dasa(int cycle) => _Dasa(Horoscope.GetPosition(Options.StartGraha).ExtrapolateLongitude(Options.Div), Options.NakshatraOffset, cycle);

	public List<DasaEntry> AntarDasa(DasaEntry di) => _AntarDasa(di);

	public string Description() => "Vimsottari Dasa Seeded from " + Options.SeedBody;

	public double ParamAyus() => 120.0;

	public int NumberOfDasaItems() => 9;

	public DasaEntry NextDasaLord(DasaEntry di) => new(NextDasaLordHelper(di.Graha), 0, 0, di.Level, string.Empty);

	public TimeOffset LengthOfDasa(Body plt) => DasaLength(plt);

	public Body LordOfNakshatra(Nakshatra n) => NakshatraLord(n);

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
		return b switch
	       {
		       Body.Sun     => Body.Moon,
		       Body.Moon    => Body.Mars,
		       Body.Mars    => Body.Rahu,
		       Body.Rahu    => Body.Jupiter,
		       Body.Jupiter => Body.Saturn,
		       Body.Saturn  => Body.Mercury,
		       Body.Mercury => Body.Ketu,
		       Body.Ketu    => Body.Venus,
		       Body.Venus   => Body.Sun,
		       _            => throw new ArgumentOutOfRangeException(nameof(b), b, null)
	       };
	}

	public static double DasaLength(Body plt)
	{
		return plt switch
	       {
		       Body.Sun     => 6,
		       Body.Moon    => 10,
		       Body.Mars    => 7,
		       Body.Rahu    => 18,
		       Body.Jupiter => 16,
		       Body.Saturn  => 19,
		       Body.Mercury => 17,
		       Body.Ketu    => 7,
		       Body.Venus   => 20,
		       _            => throw new ArgumentOutOfRangeException(nameof(plt), plt, null)
	       };
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