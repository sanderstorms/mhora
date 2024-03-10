namespace Mhora.Definitions
{
	public enum SayanadiAvastha
	{
		Nidra       = 0,  // sleeping.
		Sayana      = 1,  // lying down, resting.
		Upavesana   = 2,  // (or Upavesa) – sitting.
		Netrapani   = 3,  // a leading hand.
		Prakasana   = 4,  // (or Prakasa) – shining.
		Gamanechcha = 5,  // desirous of sexual union.
		Gamana      = 6,  // going.
		Sabhaavaasa = 7,  // (or Sabha) – remaining in an assembly.
		Agamana     = 8,  // arriving.
		Bhojana     = 9,  // food (i.e. eating).
		Nrityalipsa = 10, // inflamed to (i.e. fond of) dance.
		Kautuka     = 11, // curious.
	}

	public enum BaaladiAvastha
	{
		None   = 0,
		Baal   = 1,  //infant
		Kumaar = 2,  //Adolescent 
		Yuva   = 3,  //Youth 
		Vriddh = 4,  //infant 
		Mrit   = 5 , //dead 
	}

	public enum DeeptadiAvasthas
	{
		None    = 0,
		Dipta   = 1, //: Bright
		Svastha = 2, //: Contentment
		Mudita  = 3, //: Delighted
		Santa   = 4, //: Peaceful
		Dina    = 5, //: Depression
		Dukhita = 6, //: Distressed
		Vikala  = 7, //: Crippled
		Khala   = 8, //: Mischievious
		Kopa    = 9, //: furious
	}

	public enum JagradadiAvasthas
	{
		None    = 0,
		Jagrata = 1, //in own sign(rashi) or exaltation
		Swapna  = 2, //In rasi of a friend or neutral
		Susupta = 3, //In enemy’s rasi or debilitation
	}

	public enum LajjitadiAvasthas
	{
		None      = 0,
		Lajjita   = 1, // When planets are in putra bhav or 5th house with rahu , ketu or  shani .
		Garvita   = 2, // When planets are in exaltation or multrikona.
		Ksudhita  = 3, // In an enemy’s rasi or yuti with an enemy, or receives a drishti from an enemy, or even if a planet is yuti with Saturn.
		Krsita    = 4, // When planets are in watery sign, not receiving dristi form any beneffic but aspecting malefic 
		Mudita    = 5, // When planets are associated or aspected by friendly planets or aspected by functional beneffic planets , yogakaraka planets etc
		Kshobhita = 6, // Yuti with sun or aspected by sun or associated with maleffic and no beneffic dristi 
	}
}
