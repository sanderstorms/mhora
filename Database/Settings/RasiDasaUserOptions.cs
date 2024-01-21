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
	protected Body.BodyType             mCoLordAqu;
	protected Body.BodyType             mCoLordSco;
	protected Division              mDtype;
	protected OrderedZodiacHouses   mKetuExceptions;
	protected ArrayList             mRules;
	protected OrderedZodiacHouses   mSaturnExceptions;
	protected ZodiacHouse.Rasi      mSeed;
	protected int                   mSeedHouse;
	protected OrderedZodiacHouses[] mSeventhStrengths;

	public RasiDasaUserOptions(Horoscope _h, ArrayList _rules)
	{
		h                 = _h;
		mRules            = _rules;
		mSeventhStrengths = new OrderedZodiacHouses[6];
		mSaturnExceptions = new OrderedZodiacHouses();
		mKetuExceptions   = new OrderedZodiacHouses();
		mDtype            = new Division(Vargas.DivisionType.Rasi);

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
	public Vargas.DivisionType UIVarga
	{
		get => mDtype.MultipleDivisions[0].Varga;
		set => mDtype = new Division(value);
	}

	[PropertyOrder(99)]
	[PGDisplayName("Seed Rasi")]
	[Description("The rasi from which the dasa should be seeded.")]
	public ZodiacHouse.Rasi SeedRasi
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
	public Body.BodyType ColordAqu
	{
		get => mCoLordAqu;
		set => mCoLordAqu = value;
	}

	[PropertyOrder(102)]
	[PGDisplayName("Lord of Scorpio")]
	public Body.BodyType ColordSco
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
		return new ZodiacHouse(mSeed).Add(SeedHouse);
	}

	public void calculateSeed()
	{
		mSeed      = h.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(Division).ZodiacHouse.Sign;
		mSeedHouse = 1;
	}

	public void calculateCoLords()
	{
		var fs = new FindStronger(h, mDtype, FindStronger.RulesStrongerCoLord(h));
		mCoLordAqu = fs.StrongerGraha(Body.BodyType.Saturn, Body.BodyType.Rahu, true);
		mCoLordSco = fs.StrongerGraha(Body.BodyType.Mars, Body.BodyType.Ketu, true);
	}

	public void calculateExceptions()
	{
		KetuExceptions.houses.Clear();
		SaturnExceptions.houses.Clear();

		var zhKetu = h.GetPosition(Body.BodyType.Ketu).ToDivisionPosition(Division).ZodiacHouse.Sign;
		var zhSat  = h.GetPosition(Body.BodyType.Saturn).ToDivisionPosition(Division).ZodiacHouse.Sign;

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
			var b  = fs.StrongerGraha(Body.BodyType.Saturn, Body.BodyType.Ketu, false);
			if (b == Body.BodyType.Ketu)
			{
				mKetuExceptions.houses.Add(zhKetu);
			}
			else
			{
				mSaturnExceptions.houses.Add(zhSat);
			}
		}
	}

	public ZodiacHouse.Rasi findStrongerRasi(OrderedZodiacHouses[] mList, ZodiacHouse.Rasi za, ZodiacHouse.Rasi zb)
	{
		for (var i = 0; i < mList.Length; i++)
		{
			for (var j = 0; j < mList[i].houses.Count; j++)
			{
				if ((ZodiacHouse.Rasi) mList[i].houses[j] == za)
				{
					return za;
				}

				if ((ZodiacHouse.Rasi) mList[i].houses[j] == zb)
				{
					return zb;
				}
			}
		}

		return za;
	}

	public bool ketuExceptionApplies(ZodiacHouse.Rasi zh)
	{
		for (var i = 0; i < mKetuExceptions.houses.Count; i++)
		{
			if ((ZodiacHouse.Rasi) mKetuExceptions.houses[i] == zh)
			{
				return true;
			}
		}

		return false;
	}

	public bool saturnExceptionApplies(ZodiacHouse.Rasi zh)
	{
		for (var i = 0; i < mSaturnExceptions.houses.Count; i++)
		{
			if ((ZodiacHouse.Rasi) mSaturnExceptions.houses[i] == zh)
			{
				return true;
			}
		}

		return false;
	}

	public void calculateSeventhStrengths()
	{
		var fs   = new FindStronger(h, mDtype, mRules);
		var zAri = new ZodiacHouse(ZodiacHouse.Rasi.Ari);
		for (var i = 0; i < 6; i++)
		{
			mSeventhStrengths[i] = new OrderedZodiacHouses();
			var za = zAri.Add(i + 1);
			var zb = za.Add(7);
			if (fs.CmpRasi(za.Sign, zb.Sign, false))
			{
				mSeventhStrengths[i].houses.Add(za.Sign);
				mSeventhStrengths[i].houses.Add(zb.Sign);
			}
			else
			{
				mSeventhStrengths[i].houses.Add(zb.Sign);
				mSeventhStrengths[i].houses.Add(za.Sign);
			}
		}
	}
}