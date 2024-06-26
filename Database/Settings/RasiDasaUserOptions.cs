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
using Mhora.Calculation;
using Mhora.Components.Property;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

namespace Mhora.Database.Settings;

public class RasiDasaUserOptions : ICloneable
{
	protected Horoscope             H;
	protected Body                  MCoLordAqu;
	protected Body                  MCoLordSco;
	protected DivisionType          MDtype = DivisionType.Rasi;
	protected OrderedZodiacHouses   MKetuExceptions;
	protected List<RashiStrength>   MRules;
	protected OrderedZodiacHouses   MSaturnExceptions;
	protected ZodiacHouse           MSeed;
	protected int                   MSeedHouse;
	protected OrderedZodiacHouses[] MSeventhStrengths;

	public RasiDasaUserOptions(Horoscope h, List<RashiStrength> rules)
	{
		H                 = h;
		MRules            = rules;
		MSeventhStrengths = new OrderedZodiacHouses[6];
		MSaturnExceptions = new OrderedZodiacHouses();
		MKetuExceptions   = new OrderedZodiacHouses();
		MDtype            = DivisionType.Rasi;

		CalculateCoLords();
		CalculateExceptions();
		CalculateSeed();
		CalculateSeventhStrengths();
	}

	[PGDisplayName("Division")]
	public DivisionType Division
	{
		get => MDtype;
		set => MDtype = value;
	}

	[PropertyOrder(99)]
	[PGDisplayName("Seed Rasi")]
	[Description("The rasi from which the dasa should be seeded.")]
	public ZodiacHouse SeedZodiacHouse
	{
		get => MSeed;
		set => MSeed = value;
	}

	[PropertyOrder(100)]
	[PGDisplayName("Seed House")]
	[Description("House from which dasa should be seeded (reckoned from seed rasi)")]
	public int SeedHouse
	{
		get => MSeedHouse;
		set => MSeedHouse = value;
	}

	[PropertyOrder(101)]
	[PGDisplayName("Lord of Aquarius")]
	public Body ColordAqu
	{
		get => MCoLordAqu;
		set => MCoLordAqu = value;
	}

	[PropertyOrder(102)]
	[PGDisplayName("Lord of Scorpio")]
	public Body ColordSco
	{
		get => MCoLordSco;
		set => MCoLordSco = value;
	}

	[PropertyOrder(103)]
	[PGDisplayName("Rasi Strength Order")]
	public OrderedZodiacHouses[] SeventhStrengths
	{
		get => MSeventhStrengths;
		set => MSeventhStrengths = value;
	}

	[PropertyOrder(104)]
	[PGDisplayName("Rasis with Saturn Exception")]
	public OrderedZodiacHouses SaturnExceptions
	{
		get => MSaturnExceptions;
		set => MSaturnExceptions = value;
	}

	[PropertyOrder(105)]
	[PGDisplayName("Rasis with Ketu Exception")]
	public OrderedZodiacHouses KetuExceptions
	{
		get => MKetuExceptions;
		set => MKetuExceptions = value;
	}

	public virtual object Clone()
	{
		var uo = new RasiDasaUserOptions(H, MRules)
		{
			Division = Division,
			ColordAqu = ColordAqu,
			ColordSco = ColordSco,
			MSeed = MSeed,
			SeventhStrengths = SeventhStrengths,
			KetuExceptions = KetuExceptions,
			SaturnExceptions = SaturnExceptions,
			SeedHouse = SeedHouse
		};
		return uo;
	}


	public void Recalculate()
	{
		CalculateCoLords();
		CalculateExceptions();
		CalculateSeed();
		CalculateSeventhStrengths();
	}

	public virtual object CopyFrom(object uo)
	{
		CopyFromNoClone(uo);
		return Clone();
	}

	public virtual void CopyFromNoClone(object options)
	{
		var uo = (RasiDasaUserOptions) options;

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

		Division   = uo.Division;
		ColordAqu  = uo.ColordAqu;
		ColordSco  = uo.ColordSco;
		MSeed      = uo.MSeed;
		MSeedHouse = uo.MSeedHouse;
		for (var i = 0; i < 6; i++)
		{
			SeventhStrengths[i] = (OrderedZodiacHouses) uo.SeventhStrengths[i].Clone();
		}

		//this.SeventhStrengths = uo.SeventhStrengths.Clone();
		KetuExceptions   = (OrderedZodiacHouses) uo.KetuExceptions.Clone();
		SaturnExceptions = (OrderedZodiacHouses) uo.SaturnExceptions.Clone();

		if (bDivisionChanged)
		{
			CalculateCoLords();
		}

		if (bDivisionChanged || bRecomputeChanged)
		{
			CalculateSeed();
			CalculateSeventhStrengths();
			CalculateExceptions();
		}
	}

	public ZodiacHouse GetSeed() => MSeed.Add(SeedHouse);

	public void CalculateSeed()
	{
		MSeed      = H.GetPosition(Body.Lagna).ToDivisionPosition(Division).ZodiacHouse;
		MSeedHouse = 1;
	}

	public void CalculateCoLords()
	{
		var grahas = H.FindGrahas(MDtype);
		var rules  = H.RulesStrongerCoLord();
		MCoLordAqu = grahas.Stronger(Body.Saturn, Body.Rahu, true, rules, out _);
		MCoLordSco = grahas.Stronger(Body.Mars, Body.Ketu, true, rules, out _);
	}

	public void CalculateExceptions()
	{
		var grahas = H.FindGrahas(Division);
		KetuExceptions.houses.Clear();
		SaturnExceptions.houses.Clear();

		var zhKetu = H.GetPosition(Body.Ketu).ToDivisionPosition(Division).ZodiacHouse;
		var zhSat  = H.GetPosition(Body.Saturn).ToDivisionPosition(Division).ZodiacHouse;

		if (zhKetu != zhSat)
		{
			MKetuExceptions.houses.Add(zhKetu);
			MSaturnExceptions.houses.Add(zhSat);
		}
		else
		{
			var rule = new List<GrahaStrength>()
			{
				GrahaStrength.Longitude
			};
			var b  = grahas.Stronger(Body.Saturn, Body.Ketu, false, rule, out _);
			if (b == Body.Ketu)
			{
				MKetuExceptions.houses.Add(zhKetu);
			}
			else
			{
				MSaturnExceptions.houses.Add(zhSat);
			}
		}
	}

	public ZodiacHouse FindStrongerRasi(OrderedZodiacHouses[] mList, ZodiacHouse za, ZodiacHouse zb)
	{
		for (var i = 0; i < mList.Length; i++)
		{
			for (var j = 0; j < mList[i].houses.Count; j++)
			{
				if (mList[i].houses[j] == za)
				{
					return za;
				}

				if (mList[i].houses[j] == zb)
				{
					return zb;
				}
			}
		}

		return za;
	}

	public bool KetuExceptionApplies(ZodiacHouse zh)
	{
		for (var i = 0; i < MKetuExceptions.houses.Count; i++)
		{
			if (MKetuExceptions.houses[i] == zh)
			{
				return true;
			}
		}

		return false;
	}

	public bool SaturnExceptionApplies(ZodiacHouse zh)
	{
		for (var i = 0; i < MSaturnExceptions.houses.Count; i++)
		{
			if (MSaturnExceptions.houses[i] == zh)
			{
				return true;
			}
		}

		return false;
	}

	public void CalculateSeventhStrengths()
	{
		var rashis = H.FindRashis(MDtype);
		var zAri   = ZodiacHouse.Ari;
		for (var i = 0; i < 6; i++)
		{
			MSeventhStrengths[i] = new OrderedZodiacHouses();
			var za = zAri.Add(i + 1);
			var zb = za.Add(7);
			if (rashis.Compare(za, zb, false, MRules, out _) <= 0)
			{
				MSeventhStrengths[i].houses.Add(za);
				MSeventhStrengths[i].houses.Add(zb);
			}
			else
			{
				MSeventhStrengths[i].houses.Add(zb);
				MSeventhStrengths[i].houses.Add(za);
			}
		}
	}
}