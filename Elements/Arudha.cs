using System.Collections.Generic;
using Mhora.Definitions;

namespace Mhora.Elements
{
	public static class Arudha
	{
		public struct Lordship
		{
			public readonly ZodiacHouse ZodiacHouse;
			public readonly Body        Body;

			public Lordship(ZodiacHouse zh, Body b)
			{
				ZodiacHouse = zh;
				Body        = b;
			}
		}
		
		public static readonly List<Lordship> Lord = new ()
		{
			new (ZodiacHouse.Ari, Body.Mars),
			new (ZodiacHouse.Tau, Body.Venus),
			new (ZodiacHouse.Gem, Body.Mercury),
			new	(ZodiacHouse.Can, Body.Moon),
			new (ZodiacHouse.Leo, Body.Sun),
			new (ZodiacHouse.Vir, Body.Mercury),
			new (ZodiacHouse.Lib, Body.Venus),
			new	(ZodiacHouse.Sco, Body.Mars),
			new (ZodiacHouse.Sag, Body.Jupiter),
			new	(ZodiacHouse.Cap, Body.Saturn),
			new	(ZodiacHouse.Aqu, Body.Saturn),
			new	(ZodiacHouse.Pis, Body.Jupiter),
			new	(ZodiacHouse.Sco, Body.Ketu),
			new	(ZodiacHouse.Aqu, Body.Rahu)
		};

		public static Body[] Position =
		{
			Body.Other,
			Body.AL,
			Body.A2,
			Body.A3,
			Body.A4,
			Body.A5,
			Body.A6,
			Body.A7,
			Body.A8,
			Body.A9,
			Body.A10,
			Body.A11,
			Body.UL
		};

	}
}
