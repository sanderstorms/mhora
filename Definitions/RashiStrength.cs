using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Definitions
{
	// Maintain numerical values for forward compatibility
	[TypeConverter(typeof(EnumDescConverter))]
	public enum RashiStrength
	{
		[Description("Giving up: Arbitrarily choosing one")]
		First,

		[Description("Rasi has more grahas in it")]
		Conjunction,

		[Description("Rasi contains more exalted grahas")]
		Exaltation,

		[Description("Rasi has a Graha with higher longitude offset")]
		Longitude,

		[Description("Rasi contains Atma Karakas")]
		AtmaKaraka,

		[Description("Rasi's lord is Atma Karakas")]
		LordIsAtmaKaraka,

		[Description("Rasi is stronger by nature")]
		RasisNature,

		[Description("Rasi has more rasi drishtis of lord, Mer, Jup")]
		AspectsRasi,

		[Description("Rasi has more Graha drishtis of lord, Mer, Jup")]
		AspectsGraha,

		[Description("Rasi's lord is in a rasi of different oddity")]
		LordInDifferentOddity,

		[Description("Rasi's lord has a higher longitude offset")]
		LordsLongitude,

		[Description("Rasi has longer narayana dasa length")]
		NarayanaDasaLength,

		[Description("Rasi has a Graha in moolatrikona")]
		MoolaTrikona,

		[Description("Rasi's lord is place there")]
		OwnHouse,

		[Description("Rasi has more grahas in kendras")]
		KendraConjunction,

		[Description("Rasi's dispositor is stronger by nature")]
		LordsNature,

		[Description("Rasi has a Graha with longer karaka kendradi Graha dasa length")]
		KarakaKendradiGrahaDasaLength,

		[Description("Rasi has a Graha with longer vimsottari dasa length")]
		VimsottariDasaLength
	}

}
