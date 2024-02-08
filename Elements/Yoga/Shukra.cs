using Mhora.Definitions;

namespace Mhora.Elements.Yoga
{
	public static class Shukra
	{
		//Conjunction between Venus and Mars.
		//A cheat, a liar or gambler, addicted to other's wives, opposed to all, skilled in math’s,
		//a shepherd, a wrestler, distinguished among men because of his virtues.
		public static bool ShukraMangal(this DivisionType varga)
		{
			var venus = Graha.Find(Body.Venus, varga);
			return venus.IsConjuctWith(Body.Mars);
		}
	}
}
