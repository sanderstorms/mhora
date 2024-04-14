using System;

namespace Mhora.Definitions
{
	[Flags]
	public enum Conditions : ulong
	{
		None         = 0x00000000,
		OwnHouse     = 0x00000001,
		Exalted      = 0x00000002,
		Moolatrikona = 0x00000004,
		Debilitated  = 0x00000008,
		EnemySign    = 0x00000010,
		FriendlySign = 0x00000020,
		KarakaPlanet = 0x00000040,
		DigBala      = 0x00000080,
		Combust      = 0x00000100,
		Eclipsed     = 0x00000200
	}

}
