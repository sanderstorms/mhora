using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Tables;

public static class Tithis
{
	public enum NandaType
	{
		Nanda,
		Bhadra,
		Jaya,
		Rikta,
		Purna
	}

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

	public static readonly string[] SpecialNames =
	{
		string.Empty,
		"Janma",
		"Dhana",
		"Bhratri",
		"Matri",
		"Putra",
		"Shatru",
		"Kalatra",
		"Mrityu",
		"Bhagya",
		"Karma",
		"Laabha",
		"Vyaya"
	};

	public static readonly string[] Name =
	{
		"Prathama",
		"Dvitiya",
		"Tritiya",
		"Chaturthi",
		"Panchami",
		"Shashti",
		"Saptami",
		"Ashtami",
		"Navami",
		"Dashami",
		"Ekadasi",
		"Dvadashi",
		"Trayodashi",
		"Chaturdashi"
	};

	public static string ToUnqualifiedString(this Tithi tithi)
	{
		switch (tithi)
		{
			case Tithi.KrishnaPratipada:
			case Tithi.ShuklaPratipada: return "Prathama";
			case Tithi.KrishnaDvitiya:
			case Tithi.ShuklaDvitiya: return "Dvitiya";
			case Tithi.KrishnaTritiya:
			case Tithi.ShuklaTritiya: return "Tritiya";
			case Tithi.KrishnaChaturti:
			case Tithi.ShuklaChaturti: return "Chaturthi";
			case Tithi.KrishnaPanchami:
			case Tithi.ShuklaPanchami: return "Panchami";
			case Tithi.KrishnaShashti:
			case Tithi.ShuklaShashti: return "Shashti";
			case Tithi.KrishnaSaptami:
			case Tithi.ShuklaSaptami: return "Saptami";
			case Tithi.KrishnaAshtami:
			case Tithi.ShuklaAshtami: return "Ashtami";
			case Tithi.KrishnaNavami:
			case Tithi.ShuklaNavami: return "Navami";
			case Tithi.KrishnaDasami:
			case Tithi.ShuklaDasami: return "Dashami";
			case Tithi.KrishnaEkadasi:
			case Tithi.ShuklaEkadasi: return "Ekadashi";
			case Tithi.KrishnaDvadasi:
			case Tithi.ShuklaDvadasi: return "Dwadashi";
			case Tithi.KrishnaTrayodasi:
			case Tithi.ShuklaTrayodasi: return "Trayodashi";
			case Tithi.KrishnaChaturdasi:
			case Tithi.ShuklaChaturdasi: return "Chaturdashi";
			case Tithi.Paurnami: return "Poornima";
			case Tithi.Amavasya: return "Amavasya";
		}

		return string.Empty;
	}
}