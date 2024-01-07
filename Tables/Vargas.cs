using System.ComponentModel;
using System.Diagnostics;
using Mhora.Elements;
using Mhora.Util;

namespace Mhora.Tables;

public static class Vargas
{
	/// <summary>
	///     Enumeration of the various division types. Where a varga has multiple
	///     definitions, each of these should be specified separately below
	/// </summary>
	[TypeConverter(typeof(EnumDescConverter))]
	public enum DivisionType
	{
		[Description("D-1: Rasi")]
		Rasi = 0,

		[Description("D-9: Navamsa")]
		Navamsa,

		[Description("D-2: Hora (Parashara)")]
		HoraParasara,

		[Description("D-2: Hora (Jagannatha)")]
		HoraJagannath,

		[Description("D-2: Hora (Parivritti)")]
		HoraParivrittiDwaya,

		[Description("D-2: Hora (Kashinatha)")]
		HoraKashinath,

		[Description("D-3: Dreshkana (Parashara)")]
		DrekkanaParasara,

		[Description("D-3: Dreshkana (Jagannatha)")]
		DrekkanaJagannath,

		[Description("D-3: Dreshkana (Somnatha)")]
		DrekkanaSomnath,

		[Description("D-3: Dreshkana (Parivritti)")]
		DrekkanaParivrittitraya,

		[Description("D-4: Chaturthamsa")]
		Chaturthamsa,

		[Description("D-5: Panchamsa")]
		Panchamsa,

		[Description("D-6: Shashtamsa")]
		Shashthamsa,

		[Description("D-7: Saptamsa")]
		Saptamsa,

		[Description("D-8: Ashtamsa")]
		Ashtamsa,

		[Description("D-8: Ashtamsa (Raman)")]
		AshtamsaRaman,

		[Description("D-10: Dasamsa")]
		Dasamsa,

		[Description("D-11: Rudramsa (Rath)")]
		Rudramsa,

		[Description("D-11: Rudramsa (Raman)")]
		RudramsaRaman,

		[Description("D-12: Dwadasamsa")]
		Dwadasamsa,

		[Description("D-16: Shodasamsa")]
		Shodasamsa,

		[Description("D-20: Vimsamsa")]
		Vimsamsa,

		[Description("D-24: Chaturvimsamsa")]
		Chaturvimsamsa,

		[Description("D-27: Nakshatramsa")]
		Nakshatramsa,

		[Description("D-30: Trimsamsa (Parashara)")]
		Trimsamsa,

		[Description("D-30: Trimsamsa (Parivritti)")]
		TrimsamsaParivritti,

		[Description("D-30: Trimsamsa (Simple)")]
		TrimsamsaSimple,

		[Description("D-40: Khavedamsa")]
		Khavedamsa,

		[Description("D-45: Akshavedamsa")]
		Akshavedamsa,

		[Description("D-60: Shashtyamsa")]
		Shashtyamsa,

		[Description("D-108: Ashtottaramsa (Regular)")]
		Ashtottaramsa,

		[Description("D-150: Nadiamsa (Equal Division)")]
		Nadiamsa,

		[Description("D-150: Nadiamsa (Chandra Kala Nadi)")]
		NadiamsaCKN,

		[Description("D-9-12: Navamsa-Dwadasamsa (Composite)")]
		NavamsaDwadasamsa,

		[Description("D-12-12: Dwadasamsa-Dwadasamsa (Composite)")]
		DwadasamsaDwadasamsa,

		[Description("D-1: Bhava (9 Padas)")]
		BhavaPada,

		[Description("D-1: Bhava (Equal Length)")]
		BhavaEqual,

		[Description("D-1: Bhava (Alcabitus)")]
		BhavaAlcabitus,

		[Description("D-1: Bhava (Axial)")]
		BhavaAxial,

		[Description("D-1: Bhava (Campanus)")]
		BhavaCampanus,

		[Description("D-1: Bhava (Koch)")]
		BhavaKoch,

		[Description("D-1: Bhava (Placidus)")]
		BhavaPlacidus,

		[Description("D-1: Bhava (Regiomontanus)")]
		BhavaRegiomontanus,

		[Description("D-1: Bhava (Sripati)")]
		BhavaSripati,

		[Description("Regular: Parivritti")]
		GenericParivritti,

		[Description("Regular: Shashtamsa (d-6)")]
		GenericShashthamsa,

		[Description("Regular: Saptamsa (d-7)")]
		GenericSaptamsa,

		[Description("Regular: Dasamsa (d-10)")]
		GenericDasamsa,

		[Description("Regular: Dwadasamsa (d-12)")]
		GenericDwadasamsa,

		[Description("Regular: Chaturvimsamsa (d-24)")]
		GenericChaturvimsamsa,

		[Description("Trikona: Drekkana (d-3)")]
		GenericDrekkana,

		[Description("Trikona: Shodasamsa (d-16)")]
		GenericShodasamsa,

		[Description("Trikona: Vimsamsa (d-20)")]
		GenericVimsamsa,

		[Description("Kendra: Chaturthamsa (d-4)")]
		GenericChaturthamsa,

		[Description("Kendra: Nakshatramsa (d-27)")]
		GenericNakshatramsa
	}

	public static readonly string[] Avasthas =
	{
		"Saisava (child - quarter)",
		"Kumaara (adolescent - half)",
		"Yuva (youth - full)",
		"Vriddha (old - some)",
		"Mrita (dead - none)"
	};

	public static class Rulers
	{
		public static readonly string[] Hora =
		{
			"Devaas - Sun",
			"Pitris - Moon"
		};

		public static readonly string[] Drekkana =
		{
			"Naarada",
			"Agastya",
			"Durvaasa"
		};

		public static readonly string[] Chaturthamsa =
		{
			"Sanaka",
			"Sanandana",
			"Sanat Kumaara",
			"Sanaatana"
		};

		public static readonly string[] Saptamsa =
		{
			"Kshaara",
			"Ksheera",
			"Dadhi",
			"Ghrita",
			"Ikshurasa",
			"Madya",
			"Shuddha Jala"
		};

		public static readonly string[] Navamsa =
		{
			"Deva",
			"Nara",
			"Rakshasa"
		};

		public static readonly string[] Dasamsa =
		{
			"Indra",
			"Agni",
			"Yama",
			"Rakshasa",
			"Varuna",
			"Vayu",
			"Kubera",
			"Ishana",
			"Brahma",
			"Ananta"
		};

		public static readonly string[] Dwadasamsa =
		{
			"Ganesha",
			"Ashwini Kumars",
			"Yama",
			"Sarpa"
		};

		public static readonly string[] Shodasamsa =
		{
			"Brahma",
			"Vishnu",
			"Shiva",
			"Surya"
		};

		public static readonly string[] Vimsamsa =
		{
			"Kali",
			"Gauri",
			"Jaya",
			"Lakshmi",
			"Vijaya",
			"Vimala",
			"Sati",
			"Tara",
			"Jwalamukhi",
			"Shaveta",
			"Lalita",
			"Bagla",
			"Pratyangira",
			"Shachi",
			"Raudri",
			"Bhavani",
			"Varda",
			"Jaya",
			"Tripura",
			"Sumukhi",
			"Daya",
			"Medha",
			"China Shirsha",
			"Pishachini",
			"Dhoomavati",
			"Matangi",
			"Bala",
			"Bhadra",
			"Aruna",
			"Anala",
			"Pingala",
			"Chuccuka",
			"Ghora",
			"Varahi",
			"Vaishnavi",
			"Sita",
			"Bhuvaneshi",
			"Bhairavi",
			"Mangla",
			"Aparajita"
		};

		public static readonly string[] Chaturvimsamsa =
		{
			"Skanda",
			"Parashudhara",
			"Anala",
			"Vishvakarma",
			"Bhaga",
			"Mitra",
			"Maya",
			"Antaka",
			"Vrishdhwaja",
			"Govinda",
			"Madana",
			"Bhima"
		};

		public static readonly string[] Nakshatramsa =
		{
			"Ashwini Kumara",
			"Yama",
			"Agni",
			"Brahma",
			"Chandra Isa",
			"Aditi",
			"Jiva",
			"Abhi",
			"Pitara",
			"Bhaga",
			"Aryama",
			"Surya",
			"Tvashta",
			"Maruta",
			"Shakragni",
			"Mitra",
			"Indra",
			"Rakshasa",
			"Varuna",
			"Vishvadeva",
			"Brahma",
			"Govinda",
			"Vasu",
			"Varuna",
			"Ajapata",
			"Ahirbudhnya",
			"Pusha"
		};

		public static readonly string[] Trimsamsa =
		{
			"Agni",
			"Vayu",
			"Indra",
			"Kubera",
			"Varuna"
		};

		public static readonly string[] Khavedamsa =
		{
			"Vishnu",
			"Chandra",
			"Marichi",
			"Twashta",
			"Brahma",
			"Shiva",
			"Surya",
			"Yama",
			"Yakshesha",
			"Ghandharva",
			"Kala",
			"Varuna"
		};

		public static readonly string[] Akshavedamsa =
		{
			"Brahma",
			"Shiva",
			"Vishnu"
		};

		public static readonly string[] Shashtyamsa =
		{
			"Ghora",
			"Rakshasa",
			"Deva",
			"Kubera",
			"Yaksha",
			"Kinnara",
			"Bharashta",
			"Kulaghna",
			"Garala",
			"Vahni",
			"Maya",
			"Purishaka",
			"Apampathi",
			"Marut",
			"Kaala",
			"Sarpa",
			"Amrita",
			"Indu",
			"Mridu",
			"Komala",
			"Heramba",
			"Brahma",
			"Vishnu",
			"Maheshwara",
			"Deva",
			"Ardra",
			"Kalinasa",
			"Kshitishwara",
			"Kamalakara",
			"Gulika",
			"Mrityu",
			"Kala",
			"Davagni",
			"Ghora",
			"Yama",
			"Kantaka",
			"Sudha",
			"Amrita",
			"Poornachandra",
			"Vishadagdha",
			"Kulanasa",
			"Vamsa Khaya",
			"Utpata",
			"Kala",
			"Saumya",
			"Komala",
			"Sheetala",
			"Karala damshtra",
			"Chandramukhi",
			"Praveena",
			"Kala Pavaka",
			"Dandayudha",
			"Nirmala",
			"Saumya",
			"Kroora",
			"AtiSheetala",
			"Amrita",
			"Payodhi",
			"Bhramana",
			"Chandrarekha"
		};

		public static string[] NadiamsaRajan =
		{
			"Vasudha",
			"Vaishnavi",
			"Brahmi",
			"Kalakoota",
			"Sankari",
			"Sadaakari",
			"Samaa",
			"Saumya",
			"Suraa",
			"Maayaa",
			"Manoharaa",
			"Maadhavi",
			"Manjuswana",
			"Ghoraa",
			"Kumbhini",
			"Kutilaa",
			"Prabhaa",
			"Paraa",
			"Payaswini",
			"Maala",
			"Jagathi",
			"Jarjharaa",
			"Dhruva",
			"TO BE CONTINUED"
		};

		public static readonly string[] NadiamsaCKN =
		{
			"Vasudha",
			"Vaishnavi",
			"Brahmi",
			"Kalakoota",
			"Sankari",
			"Sudhakarasama",
			"Saumya",
			"Suraa",
			"Maaya",
			"Manoharaa",
			"Maadhavi",
			"Manjuswana",
			"Ghoraa",
			"Kumbhini",
			"Kutilaa",
			"Prabhaa",
			"Paraa",
			"Payaswini",
			"Malaa",
			"Jagathi",
			"Jarjhari",
			"Dhruvaa",
			"Musalaa",
			"Mudgala",
			"Pasaa",
			"Chambaka",
			"Daamini",
			"Mahi",
			"Kalushaa",
			"Kamalaa",
			"Kanthaa",
			"Kaalaa",
			"Karikaraa",
			"Kshamaa",
			"Durdharaa",
			"Durbhagaa",
			"Viswa",
			"Visirnaa",
			"Vihwala",
			"Anilaa",
			"Bhima",
			"Sukhaprada",
			"Snigdha",
			"Sodaraa",
			"Surasundari",
			"Amritaprasini",
			"Karalaa",
			"KamadrukkaraVeerini",
			"Gahwaraa",
			"Kundini",
			"Kanthaa",
			"Vishakhya",
			"Vishanaasini",
			"Nirmada",
			"Seethala",
			"Nimnaa",
			"Preeta",
			"Priyavivardhani",
			"Manaadha",
			"Durbhaga",
			"Chitraa",
			"Vichitra",
			"Chirajivini",
			"Boopa",
			"Gadaaharaa",
			"Naalaa",
			"Gaalavee",
			"Nirmalaa",
			"Nadi",
			"Sudha",
			"Mritamsuga",
			"Kaali",
			"Kaalika",
			"Kalushankura",
			"Krailokyamohanakari",
			"Mahaamaayaa",
			"Suseethala",
			"Sukhadaa",
			"Suprabhaa",
			"Sobhaa",
			"Sobhana",
			"Sivadaa",
			"Siva",
			"Balaa",
			"Jwalaa",
			"Gadaa",
			"Gaadaa",
			"Sootana",
			"Sumanoharaa",
			"Somavalli",
			"Somalatha",
			"Mangala",
			"Mudrika",
			"Sudha",
			"Melaa",
			"Apavargaa",
			"Pasyathaa",
			"Navaneetha",
			"Nisachari",
			"Nivrithi",
			"Nirgathaa",
			"Saaraa",
			"Samagaa",
			"Samadaa",
			"Samaa",
			"Visvambharaa",
			"Kumari",
			"Kokila",
			"Kunjarakrithi",
			"Indra",
			"Swaahaa",
			"Swadha",
			"Vahni",
			"Preethaa",
			"Yakshi",
			"Achalaprabha",
			"Saarini",
			"Madhuraa",
			"Maitri",
			"Harini",
			"Haarini",
			"Maruthaa",
			"DHananjaya",
			"Dhanakari",
			"Dhanada",
			"Kaccapa",
			"Ambuja",
			"Isaani",
			"Soolini",
			"Raudri",
			"Sivaasivakari",
			"Kalaa",
			"Kundaa",
			"Mukundaa",
			"Bharata",
			"Kadali",
			"Smaraa",
			"Basitha",
			"Kodala",
			"Kokilamsa",
			"Kaamini",
			"Kalasodbhava",
			"Viraprasoo",
			"Sangaraa",
			"Sathayagna",
			"Sataavari",
			"Sragvi",
			"Paatalini",
			"Naagapankaja",
			"Parameswari"
		};
	}

	public static string VariationNameOfDivision(this Division d)
	{
		if (d.MultipleDivisions.Length > 1)
		{
			return "Composite";
		}

		switch (d.MultipleDivisions[0].Varga)
		{
			case DivisionType.Rasi: return "Rasi";
			case DivisionType.Navamsa: return "Navamsa";
			case DivisionType.HoraParasara: return "Parasara";
			case DivisionType.HoraJagannath: return "Jagannath";
			case DivisionType.HoraParivrittiDwaya: return "Parivritti";
			case DivisionType.HoraKashinath: return "Kashinath";
			case DivisionType.DrekkanaParasara: return "Parasara";
			case DivisionType.DrekkanaJagannath: return "Jagannath";
			case DivisionType.DrekkanaParivrittitraya: return "Parivritti";
			case DivisionType.DrekkanaSomnath: return "Somnath";
			case DivisionType.Chaturthamsa: return string.Empty;
			case DivisionType.Panchamsa: return string.Empty;
			case DivisionType.Shashthamsa: return string.Empty;
			case DivisionType.Saptamsa: return string.Empty;
			case DivisionType.Ashtamsa: return "Rath";
			case DivisionType.AshtamsaRaman: return "Raman";
			case DivisionType.Dasamsa: return string.Empty;
			case DivisionType.Rudramsa: return "Rath";
			case DivisionType.RudramsaRaman: return "Raman";
			case DivisionType.Dwadasamsa: return string.Empty;
			case DivisionType.Shodasamsa: return string.Empty;
			case DivisionType.Vimsamsa: return string.Empty;
			case DivisionType.Chaturvimsamsa: return string.Empty;
			case DivisionType.Nakshatramsa: return string.Empty;
			case DivisionType.Trimsamsa: return string.Empty;
			case DivisionType.TrimsamsaParivritti: return "Parivritti";
			case DivisionType.TrimsamsaSimple: return "Simple";
			case DivisionType.Khavedamsa: return string.Empty;
			case DivisionType.Akshavedamsa: return string.Empty;
			case DivisionType.Shashtyamsa: return string.Empty;
			case DivisionType.Ashtottaramsa: return string.Empty;
			case DivisionType.Nadiamsa: return "Equal Size";
			case DivisionType.NadiamsaCKN: return "Chandra Kala";
			case DivisionType.NavamsaDwadasamsa: return "Composite";
			case DivisionType.DwadasamsaDwadasamsa: return "Composite";
			case DivisionType.BhavaPada: return "9 Padas";
			case DivisionType.BhavaEqual: return "Equal Houses";
			case DivisionType.BhavaAlcabitus: return "Alcabitus";
			case DivisionType.BhavaAxial: return "Axial";
			case DivisionType.BhavaCampanus: return "Campanus";
			case DivisionType.BhavaKoch: return "Koch";
			case DivisionType.BhavaPlacidus: return "Placidus";
			case DivisionType.BhavaRegiomontanus: return "Regiomontanus";
			case DivisionType.BhavaSripati: return "Sripati";
			case DivisionType.GenericParivritti: return "Parivritti";
			case DivisionType.GenericShashthamsa: return "Regular: Shashtamsa";
			case DivisionType.GenericSaptamsa: return "Regular: Saptamsa";
			case DivisionType.GenericDasamsa: return "Regular: Dasamsa";
			case DivisionType.GenericDwadasamsa: return "Regular: Dwadasamsa";
			case DivisionType.GenericChaturvimsamsa: return "Regular: Chaturvimsamsa";
			case DivisionType.GenericChaturthamsa: return "Kendra: Chaturtamsa";
			case DivisionType.GenericNakshatramsa: return "Kendra: Nakshatramsa";
			case DivisionType.GenericDrekkana: return "Trikona: Drekkana";
			case DivisionType.GenericShodasamsa: return "Trikona: Shodasamsa";
			case DivisionType.GenericVimsamsa: return "Trikona: Vimsamsa";
		}

		Debug.Assert(false, string.Format("Basics::numPartsInBasics.DivisionType. Unknown Division {0}", d.MultipleDivisions[0].Varga));
		return string.Empty;
	}

	public static string NumPartsInDivisionString(this Division div)
	{
		var sRet = "D";
		foreach (var dSingle in div.MultipleDivisions)
		{
			sRet = string.Format("{0}-{1}", sRet, NumPartsInDivision(dSingle));
		}

		return sRet;
	}

	public static int NumPartsInDivision(this Division div)
	{
		var parts = 1;
		foreach (var dSingle in div.MultipleDivisions)
		{
			parts *= NumPartsInDivision(dSingle);
		}

		return parts;
	}

	public static int NumPartsInDivision(this Division.SingleDivision dSingle)
	{
		switch (dSingle.Varga)
		{
			case DivisionType.Rasi: return 1;
			case DivisionType.Navamsa: return 9;
			case DivisionType.HoraParasara:
			case DivisionType.HoraJagannath:
			case DivisionType.HoraParivrittiDwaya:
			case DivisionType.HoraKashinath: return 2;
			case DivisionType.DrekkanaParasara:
			case DivisionType.DrekkanaJagannath:
			case DivisionType.DrekkanaParivrittitraya:
			case DivisionType.DrekkanaSomnath: return 3;
			case DivisionType.Chaturthamsa: return 4;
			case DivisionType.Panchamsa: return 5;
			case DivisionType.Shashthamsa: return 6;
			case DivisionType.Saptamsa: return 7;
			case DivisionType.Ashtamsa:
			case DivisionType.AshtamsaRaman: return 8;
			case DivisionType.Dasamsa: return 10;
			case DivisionType.Rudramsa:
			case DivisionType.RudramsaRaman: return 11;
			case DivisionType.Dwadasamsa: return 12;
			case DivisionType.Shodasamsa: return 16;
			case DivisionType.Vimsamsa: return 20;
			case DivisionType.Chaturvimsamsa: return 24;
			case DivisionType.Nakshatramsa: return 27;
			case DivisionType.Trimsamsa:
			case DivisionType.TrimsamsaParivritti:
			case DivisionType.TrimsamsaSimple: return 30;
			case DivisionType.Khavedamsa: return 40;
			case DivisionType.Akshavedamsa: return 45;
			case DivisionType.Shashtyamsa: return 60;
			case DivisionType.Ashtottaramsa: return 108;
			case DivisionType.Nadiamsa:
			case DivisionType.NadiamsaCKN: return 150;
			case DivisionType.NavamsaDwadasamsa: return 108;
			case DivisionType.DwadasamsaDwadasamsa: return 144;
			case DivisionType.BhavaPada:
			case DivisionType.BhavaEqual:
			case DivisionType.BhavaAlcabitus:
			case DivisionType.BhavaAxial:
			case DivisionType.BhavaCampanus:
			case DivisionType.BhavaKoch:
			case DivisionType.BhavaPlacidus:
			case DivisionType.BhavaRegiomontanus:
			case DivisionType.BhavaSripati: return 1;
			default: return dSingle.NumParts;
		}
	}

	public static Division[] Shadvargas()
	{
		return new[]
		{
			new Division(DivisionType.Rasi),
			new Division(DivisionType.HoraParasara),
			new Division(DivisionType.DrekkanaParasara),
			new Division(DivisionType.Navamsa),
			new Division(DivisionType.Dwadasamsa),
			new Division(DivisionType.Trimsamsa)
		};
	}

	public static Division[] Saptavargas()
	{
		return new[]
		{
			new Division(DivisionType.Rasi),
			new Division(DivisionType.HoraParasara),
			new Division(DivisionType.DrekkanaParasara),
			new Division(DivisionType.Saptamsa),
			new Division(DivisionType.Navamsa),
			new Division(DivisionType.Dwadasamsa),
			new Division(DivisionType.Trimsamsa)
		};
	}

	public static Division[] Dasavargas()
	{
		return new[]
		{
			new Division(DivisionType.Rasi),
			new Division(DivisionType.HoraParasara),
			new Division(DivisionType.DrekkanaParasara),
			new Division(DivisionType.Saptamsa),
			new Division(DivisionType.Navamsa),
			new Division(DivisionType.Dasamsa),
			new Division(DivisionType.Dwadasamsa),
			new Division(DivisionType.Shodasamsa),
			new Division(DivisionType.Trimsamsa),
			new Division(DivisionType.Shashtyamsa)
		};
	}

	public static Division[] Shodasavargas()
	{
		return new[]
		{
			new Division(DivisionType.Rasi),
			new Division(DivisionType.HoraParasara),
			new Division(DivisionType.DrekkanaParasara),
			new Division(DivisionType.Chaturthamsa),
			new Division(DivisionType.Saptamsa),
			new Division(DivisionType.Navamsa),
			new Division(DivisionType.Dasamsa),
			new Division(DivisionType.Dwadasamsa),
			new Division(DivisionType.Shodasamsa),
			new Division(DivisionType.Vimsamsa),
			new Division(DivisionType.Chaturvimsamsa),
			new Division(DivisionType.Nakshatramsa),
			new Division(DivisionType.Trimsamsa),
			new Division(DivisionType.Khavedamsa),
			new Division(DivisionType.Akshavedamsa),
			new Division(DivisionType.Shashtyamsa)
		};
	}
}