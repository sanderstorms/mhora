using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Tables;

public static class Tithi
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
	public enum Value
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

	public static readonly string[] specialNames =
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

	public static string ToUnqualifiedString(this Value value)
	{
		switch (value)
		{
			case Value.KrishnaPratipada:
			case Value.ShuklaPratipada: return "Prathama";
			case Value.KrishnaDvitiya:
			case Value.ShuklaDvitiya: return "Dvitiya";
			case Value.KrishnaTritiya:
			case Value.ShuklaTritiya: return "Tritiya";
			case Value.KrishnaChaturti:
			case Value.ShuklaChaturti: return "Chaturthi";
			case Value.KrishnaPanchami:
			case Value.ShuklaPanchami: return "Panchami";
			case Value.KrishnaShashti:
			case Value.ShuklaShashti: return "Shashti";
			case Value.KrishnaSaptami:
			case Value.ShuklaSaptami: return "Saptami";
			case Value.KrishnaAshtami:
			case Value.ShuklaAshtami: return "Ashtami";
			case Value.KrishnaNavami:
			case Value.ShuklaNavami: return "Navami";
			case Value.KrishnaDasami:
			case Value.ShuklaDasami: return "Dashami";
			case Value.KrishnaEkadasi:
			case Value.ShuklaEkadasi: return "Ekadashi";
			case Value.KrishnaDvadasi:
			case Value.ShuklaDvadasi: return "Dwadashi";
			case Value.KrishnaTrayodasi:
			case Value.ShuklaTrayodasi: return "Trayodashi";
			case Value.KrishnaChaturdasi:
			case Value.ShuklaChaturdasi: return "Chaturdashi";
			case Value.Paurnami: return "Poornima";
			case Value.Amavasya: return "Amavasya";
		}

		return string.Empty;
	}
}