using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Definitions
{
	// Maintain numerical values for forward compatibility
	[TypeConverter(typeof(EnumDescConverter))]
	public enum GrahaStrength
	{
		[Description("Giving up: Arbitrarily choosing one")]
		First,

		[Description("Graha is conjunct more grahas")]
		Conjunction,

		[Description("Graha is exalted")]
		Exaltation,

		[Description("Graha has higher longitude offset")]
		Longitude,

		[Description("Graha is Atma Karakas")]
		AtmaKaraka,

		[Description("Graha is in a rasi with stronger nature")]
		RasisNature,

		[Description("Graha has more rasi drishti of dispositor, Jup, Mer")]
		AspectsRasi,

		[Description("Graha has more Graha drishti of dispositor, Jup, Mer")]
		AspectsGraha,

		[Description("Graha has a larger narayana dasa length")]
		NarayanaDasaLength,

		[Description("Graha is in its moola trikona rasi")]
		MoolaTrikona,

		[Description("Graha is in own house")]
		OwnHouse,

		[Description("Graha is not in own house")]
		NotInOwnHouse,

		[Description("Graha's dispositor is in own house")]
		LordInOwnHouse,

		[Description("Graha has more grahas in kendras")]
		KendraConjunction,

		[Description("Graha's dispositor is in a rasi with stronger nature")]
		LordsNature,

		[Description("Graha's dispositor is in a rasi with different oddify")]
		LordInDifferentOddity,

		[Description("Graha has a larger Karakas Kendradi Graha Dasa length")]
		KarakaKendradiGrahaDasaLength,

		[Description("Graha has a larger Vimsottari Dasa length")]
		VimsottariDasaLength
	}
}
