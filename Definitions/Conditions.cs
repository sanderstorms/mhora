using System;

namespace Mhora.Definitions
{
	[Flags]
	public enum Conditions : ulong
	{
		None              = 0x00000000,
		OwnHouse          = 0x00000001,
		Exalted           = 0x00000002,
		Moolatrikona      = 0x00000004,
		Debilitated       = 0x00000008,
		EnemySign         = 0x00000010,
		FriendlySign      = 0x00000020,
		KarakaPlanet      = 0x00000040,
		DigBala           = 0x00000080,
		Combust           = 0x00000100,
		Eclipsed          = 0x00000200,
		PushkarNavamsa    = 0x00000400,
		PushkaraBhaga     = 0x00000800,
		FunctionalBenefic = 0x000001000,
		FunctionalMalefic = 0x000002000,
		YogaKaraka		  = 0x000004000,
	}

}
