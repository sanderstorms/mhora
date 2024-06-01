
using System;

namespace Mhora.Definitions
{
	[Flags]
	public enum ShadBala
	{
		SthanBala		= 0x01, //Positional strength
		DigBala			= 0x02, //Directional strength
		KaalBala		= 0x04, //Temporalstrength, inclusive of Ayan Bal (Equinoctial strength)
		CheshtBala		= 0x08, //Motional strength
		NaisargikaBala	= 0x10, //Natural strength
		DrikBala		= 0x20, //Aspectual strength

	}

	[Flags]
	public enum ShtanaBala
	{
		UcchaBala			= 0x01,	//exaltation
		SaptaVargiyaBala	= 0x02, //Rashi, Hora, Drekkana, Saptamsha, Navamsa, Dwadasamsha and Trimsamsha 
		OjaYugmaBala		= 0x04,	//planet’s placement in the odd or even signs in the Rashi and Navamsa.
		KendradiBala		= 0x08,	//Kendra - full strength, Panapara - half Apoklimas - quarter strength
		DrekkanaBala		= 0x10,	//Strength of planet being in male or female drekkana
	}

	[Flags]
	public enum KalaaBala
	{
		NathonnathaBala = 0x0001, //Day-Night Strength
		PakshaBala		= 0x0002, //Monthly Strength
		TribangaBala	= 0x0004, //Four Hour Strength
		AbdadhipatiBala = 0x0008, //Lord of the Year Strength
		MasadhipatiBala = 0x0010, //Lord of the Month Strength
		VaradhipatiBala = 0x0020, //Lord the Day Strength
		HoraBala		= 0x0040, //Lord of the Hour Strength 
		AyanaBala		= 0x0080, //Declinational Strength
		YuddhaBala		= 0x0100, //Planetary War Strength
	}

	public enum ChestaBala
	{
		Vakra		= 1, //retrogression
		Anuvakra	= 2, //entering the previous sign in retrograde motion
		Vikala		= 3, //devoid of motion
		Manda		= 4, //somewhat slower motion than usual
		Mandatara	= 5, //slower than the previous
		Sama		= 6, //neither fast nor slow
		Chara		= 7, //faster than Sama
		Atichara	= 8, //entering next sign in accelerated motion
	}

}
