using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Definitions
{
	[TypeConverter(typeof(EnumDescConverter))]
	public enum Body
	{
		// Do NOT CHANGE ORDER WITHOUT CHANING NARAYANA DASA ETC
		// RELY ON EXPLICIT EQUAL CONVERSION FOR STRONGER CO_LORD ETC
		Sun     = 0,
		Moon    = 1,
		Mars    = 2,
		Mercury = 3,
		Jupiter = 4,
		Venus   = 5,
		Saturn  = 6,
		Rahu    = 7,
		Ketu    = 8,
		Lagna   = 9,

		// And now, we're no longer uptight about the ordering :-)
		[Description("Bhava Lagna")]
		BhavaLagna,

		[Description("Hora Lagna")]
		HoraLagna,

		[Description("Ghati Lagna")]
		GhatiLagna,

		[Description("Sree Lagna")]
		SreeLagna,
		[Description("Pranapada Lagna")]
		Pranapada,

		[Description("Vighati Lagna")]
		VighatiLagna,
		Dhuma,
		Vyatipata,
		Parivesha,
		Indrachapa,
		Upaketu,
		Kala,
		Mrityu,
		ArthaPraharaka,
		YamaGhantaka,
		Gulika,
		Maandi,

		[Description("Chandra Ayur Lagna")]
		ChandraAyurLagna,
		MrityuPoint,
		Other,
		AL,					
		A2,
		A3,
		A4,
		A5,
		A6,
		A7,
		A8,
		A9,
		A10,
		A11,
		UL
	}


	// A1 Arudha lagna, Pada lagna, Arudha, Pada
	// A2 Dhanarudha, Vittarudha, Dhana pada, Vitta pada
	// A3 Bhatrarudha, Bhratri pada, Vikramarudha, Vikrama pada
	// A4 Matri pada, Vahana pada, Sukha pada, Matrarudha, Vahanarudha, Sukharudha
	// A5 Mantra pada, Mantrarudha, Putrarudha, Putra pada, Buddhi pada
	// A6 Roga pada, Satru pada, Rogarudha, Satrarudha
	// A7 Dara pada, Dararudha
	// A8 Mrityu pada, Kashta pada, Kashtarudha, Randhrarudha
	// A9 Bhagya pada, Bhagyarudha, Pitri pada, Pitrarudha, Dharma pada, Guru pada
	// A10 Karma pada, Karmarudha, Swarga pada, Swargarudha, Rajya pada
	// A11 Labha pada, Labharudha
	// A12 Upapada lagna, Upapada, Gaunapada, Vyayarudha, Moksha pada
}
