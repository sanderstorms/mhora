
using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Tables
{
	public static class Tithi
	{
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

		public static readonly string [] Name =
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

		public static string ToUnqualifiedString(this Value value)
		{
			switch (value)
			{
				case Tables.Tithi.Value.KrishnaPratipada:
				case Tables.Tithi.Value.ShuklaPratipada:
					return "Prathama";
				case Tables.Tithi.Value.KrishnaDvitiya:
				case Tables.Tithi.Value.ShuklaDvitiya:
					return "Dvitiya";
				case Tables.Tithi.Value.KrishnaTritiya:
				case Tables.Tithi.Value.ShuklaTritiya:
					return "Tritiya";
				case Tables.Tithi.Value.KrishnaChaturti:
				case Tables.Tithi.Value.ShuklaChaturti:
					return "Chaturthi";
				case Tables.Tithi.Value.KrishnaPanchami:
				case Tables.Tithi.Value.ShuklaPanchami:
					return "Panchami";
				case Tables.Tithi.Value.KrishnaShashti:
				case Tables.Tithi.Value.ShuklaShashti:
					return "Shashti";
				case Tables.Tithi.Value.KrishnaSaptami:
				case Tables.Tithi.Value.ShuklaSaptami:
					return "Saptami";
				case Tables.Tithi.Value.KrishnaAshtami:
				case Tables.Tithi.Value.ShuklaAshtami:
					return "Ashtami";
				case Tables.Tithi.Value.KrishnaNavami:
				case Tables.Tithi.Value.ShuklaNavami:
					return "Navami";
				case Tables.Tithi.Value.KrishnaDasami:
				case Tables.Tithi.Value.ShuklaDasami:
					return "Dashami";
				case Tables.Tithi.Value.KrishnaEkadasi:
				case Tables.Tithi.Value.ShuklaEkadasi:
					return "Ekadashi";
				case Tables.Tithi.Value.KrishnaDvadasi:
				case Tables.Tithi.Value.ShuklaDvadasi:
					return "Dwadashi";
				case Tables.Tithi.Value.KrishnaTrayodasi:
				case Tables.Tithi.Value.ShuklaTrayodasi:
					return "Trayodashi";
				case Tables.Tithi.Value.KrishnaChaturdasi:
				case Tables.Tithi.Value.ShuklaChaturdasi:
					return "Chaturdashi";
				case Tables.Tithi.Value.Paurnami: return "Poornima";
				case Tables.Tithi.Value.Amavasya: return "Amavasya";
			}

			return string.Empty;
		}

		public enum NandaType
		{
			Nanda,
			Bhadra,
			Jaya,
			Rikta,
			Purna
		}
	}
}
