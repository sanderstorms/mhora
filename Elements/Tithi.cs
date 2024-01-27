using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Elements
{
	[TypeConverter(typeof(EnumDescConverter))]
	public enum Tithi
	{
		[Description("Shukla Pratipada")]
		ShuklaPratipada = 1,

		[Description("Shukla Dvitiya")]
		ShuklaDvitiya,

		[Description("Shukla Tritiya")]
		ShuklaTritiya,

		[Description("Shukla Chaturti")]
		ShuklaChaturti,

		[Description("Shukla Panchami")]
		ShuklaPanchami,

		[Description("Shukla Shashti")]
		ShuklaShashti,

		[Description("Shukla Saptami")]
		ShuklaSaptami,

		[Description("Shukla Ashtami")]
		ShuklaAshtami,

		[Description("Shukla Navami")]
		ShuklaNavami,

		[Description("Shukla Dashami")]
		ShuklaDasami,

		[Description("Shukla Ekadasi")]
		ShuklaEkadasi,

		[Description("Shukla Dwadasi")]
		ShuklaDvadasi,

		[Description("Shukla Trayodasi")]
		ShuklaTrayodasi,

		[Description("Shukla Chaturdasi")]
		ShuklaChaturdasi,

		[Description("Paurnami")]
		Paurnami,

		[Description("Krishna Pratipada")]
		KrishnaPratipada,

		[Description("Krishna Dvitiya")]
		KrishnaDvitiya,

		[Description("Krishna Tritiya")]
		KrishnaTritiya,

		[Description("Krishna Chaturti")]
		KrishnaChaturti,

		[Description("Krishna Panchami")]
		KrishnaPanchami,

		[Description("Krishna Shashti")]
		KrishnaShashti,

		[Description("Krishna Saptami")]
		KrishnaSaptami,

		[Description("Krishna Ashtami")]
		KrishnaAshtami,

		[Description("Krishna Navami")]
		KrishnaNavami,

		[Description("Krishna Dashami")]
		KrishnaDasami,

		[Description("Krishna Ekadasi")]
		KrishnaEkadasi,

		[Description("Krishna Dwadasi")]
		KrishnaDvadasi,

		[Description("Krishna Trayodasi")]
		KrishnaTrayodasi,

		[Description("Krishna Chaturdasi")]
		KrishnaChaturdasi,

		[Description("Amavasya")]
		Amavasya
	}
}
