using Mhora.Definitions;

namespace Mhora.Elements.Extensions
{
	public static class Tatvas
	{
		public static readonly double[] Duration =
		{
			6,	//Prithvi
			12,	//Jal
			18,	//Agni
			24,	//Vayu
			30	//Akash
		};

		public static Tatva[] DayTatva =
		{
			Tatva.Jal,     //Mon
			Tatva.Agni,    //Tue
			Tatva.Prithvi, //Wed
			Tatva.Akash,   //Thu
			Tatva.Jal,     //Fri
			Tatva.Vayu,    //Sat
			Tatva.Agni,    //Sun
		};
	}
}
