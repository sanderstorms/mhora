using System;

namespace Mhora.Elements.Yoga
{
	[Flags]
	public enum Conditions : ulong
	{
		None         = 0x00000000,
		OwnHouse     = 0x00000001,
		Exalted      = 0x00000002,
		Moolatrikona = 0x00000004,
		Debilitated  = 0x00000008,
		MalificRasi  = 0x00000010,
		BenificRasi  = 0x00000020,
		Papagraha    = 0x00000040,
		ShubhaGraha  = 0x00000080,
		KarakaPlanet = 0x00000100,
	}

}
