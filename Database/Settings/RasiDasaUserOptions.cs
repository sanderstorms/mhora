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
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Database.Settings;

public class RasiDasaUserOptions : ICloneable
{
	protected Horoscope             h;
	protected Body.Name             mCoLordAqu;
	protected Body.Name             mCoLordSco;
	protected Division              mDtype;
	protected OrderedZodiacHouses   mKetuExceptions;
	protected ArrayList             mRules;
	protected OrderedZodiacHouses   mSaturnExceptions;
	protected ZodiacHouse.Name      mSeed;
	protected int                   mSeedHouse;
	protected OrderedZodiacHouses[] mSeventhStrengths;

	public RasiDasaUserOptions(Horoscope _h, ArrayList _rules)
	{
		h                 = _h;
		mRules            = _rules;
		mSeventhStrengths = new OrderedZodiacHouses[6];
		mSaturnExceptions = new OrderedZodiacHouses();
		mKetuExceptions   = new OrderedZodiacHouses();
		mDtype            = new Division(Basics.DivisionType.Rasi);

		calculateCoLords();
		calculateExceptions();
		calculateSeed();
		calculateSeventhStrengths();
	}

	[PGNotVisible]
	public Division Division
	{
		get => mDtype;
		set => mDtype = value;
	}

	[PGDisplayName("Division")]
	public Basics.DivisionType UIVarga
	{
		get => mDtype.MultipleDivisions[0].Varga;
		set => mDtype = new Division(value);
	}

	[PropertyOrder(99)]
	[PGDisplayName("Seed Rasi")]
	[Description("The rasi from which the dasa should be seeded.")]
	public ZodiacHouse.Name SeedRasi
	{
		get => mSeed;
		set => mSeed = value;
	}

	[PropertyOrder(100)]
	[PGDisplayName("Seed House")]
	[Description("House from which dasa should be seeded (reckoned from seed rasi)")]
	public int SeedHouse
	{
		get => mSeedHouse;
		set => mSeedHouse = value;
	}

	[PropertyOrder(101)]
	[PGDisplayName("Lord of Aquarius")]
	public Body.Name ColordAqu
	{
		get => mCoLordAqu;
		set => mCoLordAqu = value;
	}

	[PropertyOrder(102)]
	[PGDisplayName("Lord of Scorpio")]
	public Body.Name ColordSco
	{
		get => mCoLordSco;
		set => mCoLordSco = value;
	}

	[PropertyOrder(103)]
	[PGDisplayName("Rasi Strength Order")]
	public OrderedZodiacHouses[] SeventhStrengths
	{
		get => mSeventhStrengths;
		set => mSeventhStrengths = value;
	}

	[PropertyOrder(104)]
	[PGDisplayName("Rasis with Saturn Exception")]
	public OrderedZodiacHouses SaturnExceptions
	{
		get => mSaturnExceptions;
		set => mSaturnExceptions = value;
	}

	[PropertyOrder(105)]
	[PGDisplayName("Rasis with Ketu Exception")]
	public OrderedZodiacHouses KetuExceptions
	{
		get => mKetuExceptions;
		set => mKetuExceptions = value;
	}

	public virtual object Clone()
	{
		var uo = new RasiDasaUserOptions(h, mRules);
		uo.Division         = (Division) Division.Clone();
		uo.ColordAqu        = ColordAqu;
		uo.ColordSco        = ColordSco;
		uo.mSeed            = mSeed;
		uo.SeventhStrengths = SeventhStrengths;
		uo.KetuExceptions   = KetuExceptions;
		uo.SaturnExceptions = SaturnExceptions;
		uo.SeedHouse        = SeedHouse;
		return uo;
	}


	public void recalculate()
	{
		calculateCoLords();
		calculateExceptions();
		calculateSeed();
		calculateSeventhStrengths();
	}

	public virtual object CopyFrom(object _uo)
	{
		CopyFromNoClone(_uo);
		return Clone();
	}

	public virtual void CopyFromNoClone(object _uo)
	{
		var uo = (RasiDasaUserOptions) _uo;

		var bDivisionChanged  = false;
		var bRecomputeChanged = false;

		if (Division != uo.Division)
		{
			bDivisionChanged = true;
		}

		if (ColordAqu != uo.ColordAqu || ColordSco != uo.ColordSco)
		{
			bRecomputeChanged = true;
		}

		Division   = (Division) uo.Division.Clone();
		ColordAqu  = uo.ColordAqu;
		ColordSco  = uo.ColordSco;
		mSeed      = uo.mSeed;
		mSeedHouse = uo.mSeedHouse;
		for (var i = 0; i < 6; i++)
		{
			SeventhStrengths[i] = (OrderedZodiacHouses) uo.SeventhStrengths[i].Clone();
		}

		//this.SeventhStrengths = uo.SeventhStrengths.Clone();
		KetuExceptions   = (OrderedZodiacHouses) uo.KetuExceptions.Clone();
		SaturnExceptions = (OrderedZodiacHouses) uo.SaturnExceptions.Clone();

		if (bDivisionChanged)
		{
			calculateCoLords();
		}

		if (bDivisionChanged || bRecomputeChanged)
		{
			calculateSeed();
			calculateSeventhStrengths();
			calculateExceptions();
		}
	}

	public ZodiacHouse getSeed()
	{
		return new ZodiacHouse(mSeed).add(SeedHouse);
	}

	public void calculateSeed()
	{
		mSeed      = h.getPosition(Body.Name.Lagna).toDivisionPosition(Division).zodiac_house.value;
		mSeedHouse = 1;
	}

	public void calculateCoLords()
	{
		var fs = new FindStronger(h, mDtype, FindStronger.RulesStrongerCoLord(h));
		mCoLordAqu = fs.StrongerGraha(Body.Name.Saturn, Body.Name.Rahu, true);
		mCoLordSco = fs.StrongerGraha(Body.Name.Mars, Body.Name.Ketu, true);
	}

	public void calculateExceptions()
	{
		KetuExceptions.houses.Clear();
		SaturnExceptions.houses.Clear();

		var zhKetu = h.getPosition(Body.Name.Ketu).toDivisionPosition(Division).zodiac_house.value;
		var zhSat  = h.getPosition(Body.Name.Saturn).toDivisionPosition(Division).zodiac_house.value;

		if (zhKetu != zhSat)
		{
			mKetuExceptions.houses.Add(zhKetu);
			mSaturnExceptions.houses.Add(zhSat);
		}
		else
		{
			var rule = new ArrayList();
			rule.Add(FindStronger.EGrahaStrength.Longitude);
			var fs = new FindStronger(h, Division, rule);
			var b  = fs.StrongerGraha(Body.Name.Saturn, Body.Name.Ketu, false);
			if (b == Body.Name.Ketu)
			{
				mKetuExceptions.houses.Add(zhKetu);
			}
			else
			{
				mSaturnExceptions.houses.Add(zhSat);
			}
		}
	}

	public ZodiacHouse.Name findStrongerRasi(OrderedZodiacHouses[] mList, ZodiacHouse.Name za, ZodiacHouse.Name zb)
	{
		for (var i = 0; i < mList.Length; i++)
		{
			for (var j = 0; j < mList[i].houses.Count; j++)
			{
				if ((ZodiacHouse.Name) mList[i].houses[j] == za)
				{
					return za;
				}

				if ((ZodiacHouse.Name) mList[i].houses[j] == zb)
				{
					return zb;
				}
			}
		}

		return za;
	}

	public bool ketuExceptionApplies(ZodiacHouse.Name zh)
	{
		for (var i = 0; i < mKetuExceptions.houses.Count; i++)
		{
			if ((ZodiacHouse.Name) mKetuExceptions.houses[i] == zh)
			{
				return true;
			}
		}

		return false;
	}

	public bool saturnExceptionApplies(ZodiacHouse.Name zh)
	{
		for (var i = 0; i < mSaturnExceptions.houses.Count; i++)
		{
			if ((ZodiacHouse.Name) mSaturnExceptions.houses[i] == zh)
			{
				return true;
			}
		}

		return false;
	}

	public void calculateSeventhStrengths()
	{
		var fs   = new FindStronger(h, mDtype, mRules);
		var zAri = new ZodiacHouse(ZodiacHouse.Name.Ari);
		for (var i = 0; i < 6; i++)
		{
			mSeventhStrengths[i] = new OrderedZodiacHouses();
			var za = zAri.add(i + 1);
			var zb = za.add(7);
			if (fs.CmpRasi(za.value, zb.value, false))
			{
				mSeventhStrengths[i].houses.Add(za.value);
				mSeventhStrengths[i].houses.Add(zb.value);
			}
			else
			{
				mSeventhStrengths[i].houses.Add(zb.value);
				mSeventhStrengths[i].houses.Add(za.value);
			}
		}
	}
}