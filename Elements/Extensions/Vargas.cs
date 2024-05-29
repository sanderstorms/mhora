using System;
using System.Diagnostics;
using Mhora.Definitions;

namespace Mhora.Elements.Extensions;

public static class Vargas
{

	public static readonly string[] Avasthas =
	[
		"Saisava (child - quarter)",
		"Kumaara (adolescent - half)",
		"Yuva (youth - full)",
		"Vriddha (old - some)",
		"Mrita (dead - none)"
	];

	public static class Rulers
	{
		public static readonly string[] Hora =
		[
			"Devaas - Sun",
			"Pitris - Moon"
		];

		public static readonly string[] Drekkana =
		[
			"Naarada",
			"Agastya",
			"Durvaasa"
		];

		public static readonly string[] Chaturthamsa =
		[
			"Sanaka",
			"Sanandana",
			"Sanat Kumaara",
			"Sanaatana"
		];

		public static readonly string[] Saptamsa =
		[
			"Kshaara",
			"Ksheera",
			"Dadhi",
			"Ghrita",
			"Ikshurasa",
			"Madya",
			"Shuddha Jala"
		];

		public static readonly string[] Navamsa =
		[
			"Deva",
			"Nara",
			"Rakshasa"
		];

		public static readonly string[] Dasamsa =
		[
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
		];

		public static readonly string[] Dwadasamsa =
		[
			"Ganesha",
			"Ashwini Kumars",
			"Yama",
			"Sarpa"
		];

		public static readonly string[] Shodasamsa =
		[
			"Brahma",
			"Vishnu",
			"Shiva",
			"Surya"
		];

		public static readonly string[] Vimsamsa =
		[
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
		];

		public static readonly string[] Chaturvimsamsa =
		[
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
		];

		public static readonly string[] Nakshatramsa =
		[
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
		];

		public static readonly string[] Trimsamsa =
		[
			"Agni",
			"Vayu",
			"Indra",
			"Kubera",
			"Varuna"
		];

		public static readonly string[] Khavedamsa =
		[
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
		];

		public static readonly string[] Akshavedamsa =
		[
			"Brahma",
			"Shiva",
			"Vishnu"
		];

		public static readonly string[] Shashtyamsa =
		[
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
		];

		public static string[] NadiamsaRajan =
		[
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
		];

		public static readonly string[] NadiamsaCKN =
		[
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
		];
	}

	public static string VariationNameOfDivision(this Division d)
	{
		if (d.MultipleDivisions.Length > 1)
		{
			return "Composite";
		}

		return d.MultipleDivisions[0].Varga switch
		{
			DivisionType.Rasi                    => "Rasi",
			DivisionType.Navamsa                 => "Navamsa",
			DivisionType.HoraParasara            => "Parasara",
			DivisionType.HoraJagannath           => "Jagannath",
			DivisionType.HoraParivrittiDwaya     => "Parivritti",
			DivisionType.HoraKashinath           => "Kashinath",
			DivisionType.DrekkanaParasara        => "Parasara",
			DivisionType.DrekkanaJagannath       => "Jagannath",
			DivisionType.DrekkanaParivrittitraya => "Parivritti",
			DivisionType.DrekkanaSomnath         => "Somnath",
			DivisionType.Chaturthamsa            => string.Empty,
			DivisionType.Panchamsa               => string.Empty,
			DivisionType.Shashthamsa             => string.Empty,
			DivisionType.Saptamsa                => string.Empty,
			DivisionType.Ashtamsa                => "Rath",
			DivisionType.AshtamsaRaman           => "Raman",
			DivisionType.Dasamsa                 => string.Empty,
			DivisionType.Rudramsa                => "Rath",
			DivisionType.RudramsaRaman           => "Raman",
			DivisionType.Dwadasamsa              => string.Empty,
			DivisionType.Shodasamsa              => string.Empty,
			DivisionType.Vimsamsa                => string.Empty,
			DivisionType.Chaturvimsamsa          => string.Empty,
			DivisionType.Nakshatramsa            => string.Empty,
			DivisionType.Trimsamsa               => string.Empty,
			DivisionType.TrimsamsaParivritti     => "Parivritti",
			DivisionType.TrimsamsaSimple         => "Simple",
			DivisionType.Khavedamsa              => string.Empty,
			DivisionType.Akshavedamsa            => string.Empty,
			DivisionType.NavaNavamsa             => string.Empty,
			DivisionType.Shashtyamsa             => string.Empty,
			DivisionType.Ashtottaramsa           => string.Empty,
			DivisionType.Nadiamsa                => "Equal Size",
			DivisionType.NadiamsaCKN             => "Chandra Kala",
			DivisionType.NavamsaDwadasamsa       => "Composite",
			DivisionType.DwadasamsaDwadasamsa    => "Composite",
			DivisionType.BhavaPada               => "9 Padas",
			DivisionType.BhavaEqual              => "Equal Houses",
			DivisionType.BhavaAlcabitus          => "Alcabitus",
			DivisionType.BhavaAxial              => "Axial",
			DivisionType.BhavaCampanus           => "Campanus",
			DivisionType.BhavaKoch               => "Koch",
			DivisionType.BhavaPlacidus           => "Placidus",
			DivisionType.BhavaRegiomontanus      => "Regiomontanus",
			DivisionType.BhavaSripati            => "Sripati",
			DivisionType.GenericParivritti       => "Parivritti",
			DivisionType.GenericShashthamsa      => "Regular: Shashtamsa",
			DivisionType.GenericSaptamsa         => "Regular: Saptamsa",
			DivisionType.GenericDasamsa          => "Regular: Dasamsa",
			DivisionType.GenericDwadasamsa       => "Regular: Dwadasamsa",
			DivisionType.GenericChaturvimsamsa   => "Regular: Chaturvimsamsa",
			DivisionType.GenericChaturthamsa     => "Kendra: Chaturtamsa",
			DivisionType.GenericNakshatramsa     => "Kendra: Nakshatramsa",
			DivisionType.GenericDrekkana         => "Trikona: Drekkana",
			DivisionType.GenericShodasamsa       => "Trikona: Shodasamsa",
			DivisionType.GenericVimsamsa         => "Trikona: Vimsamsa",
		};
		Debug.Assert(false, string.Format("Basics::numPartsInBasics.DivisionType. Unknown Division {0}", d.MultipleDivisions[0].Varga));
		return string.Empty;
	}

	public static string AmsaRuler(this Position bp, DivisionType varga, int ri)
	{
		return varga switch
		       {
			       DivisionType.HoraParasara     => Rulers.Hora[ri],
			       DivisionType.DrekkanaParasara => Rulers.Drekkana[ri],
			       DivisionType.Chaturthamsa     => Rulers.Chaturthamsa[ri],
			       DivisionType.Saptamsa         => Rulers.Saptamsa[ri],
			       DivisionType.Navamsa          => Rulers.Navamsa[ri],
			       DivisionType.Dasamsa          => Rulers.Dasamsa[ri],
			       DivisionType.Dwadasamsa       => Rulers.Dwadasamsa[ri],
			       DivisionType.Shodasamsa       => Rulers.Shodasamsa[ri],
			       DivisionType.Vimsamsa         => Rulers.Vimsamsa[ri],
			       DivisionType.Chaturvimsamsa   => Rulers.Chaturvimsamsa[ri],
			       DivisionType.Nakshatramsa     => Rulers.Nakshatramsa[ri],
			       DivisionType.Trimsamsa        => Rulers.Trimsamsa[ri],
			       DivisionType.Khavedamsa       => Rulers.Khavedamsa[ri],
			       DivisionType.Akshavedamsa     => Rulers.Akshavedamsa[ri],
			       DivisionType.Shashtyamsa      => Rulers.Shashtyamsa[ri],
			       DivisionType.Nadiamsa         => Rulers.NadiamsaCKN[ri],
			       DivisionType.NadiamsaCKN      => Rulers.NadiamsaCKN[ri],
			       _                             => string.Empty
		       };
	}

	public static string NumPartsInDivisionString(this Division div)
	{
		var sRet = "D";
		foreach (var dSingle in div.MultipleDivisions)
		{
			sRet = string.Format("{0}-{1}", sRet, NumPartsInDivision(dSingle.Varga));
		}

		return sRet;
	}

	public static int NumPartsInDivision(this Division div)
	{
		var parts = 1;
		foreach (var dSingle in div.MultipleDivisions)
		{
			parts *= NumPartsInDivision(dSingle.Varga);
		}

		return parts;
	}

	public static int NumPartsInDivision(this DivisionType varga)
	{
		return varga switch
		       {
			       DivisionType.Rasi                    => 1,
			       DivisionType.Navamsa                 => 9,
			       DivisionType.HoraParasara            => 2,
			       DivisionType.HoraJagannath           => 2,
			       DivisionType.HoraParivrittiDwaya     => 2,
			       DivisionType.HoraKashinath           => 2,
			       DivisionType.DrekkanaParasara        => 3,
			       DivisionType.DrekkanaJagannath       => 3,
			       DivisionType.DrekkanaParivrittitraya => 3,
			       DivisionType.DrekkanaSomnath         => 3,
			       DivisionType.Chaturthamsa            => 4,
			       DivisionType.Panchamsa               => 5,
			       DivisionType.Shashthamsa             => 6,
			       DivisionType.Saptamsa                => 7,
			       DivisionType.Ashtamsa                => 8,
			       DivisionType.AshtamsaRaman           => 8,
			       DivisionType.Dasamsa                 => 10,
			       DivisionType.Rudramsa                => 11,
			       DivisionType.RudramsaRaman           => 11,
			       DivisionType.Dwadasamsa              => 12,
			       DivisionType.Shodasamsa              => 16,
			       DivisionType.Vimsamsa                => 20,
			       DivisionType.Chaturvimsamsa          => 24,
			       DivisionType.Nakshatramsa            => 27,
			       DivisionType.Trimsamsa               => 30,
			       DivisionType.TrimsamsaParivritti     => 30,
			       DivisionType.TrimsamsaSimple         => 30,
			       DivisionType.Khavedamsa              => 40,
			       DivisionType.Akshavedamsa            => 45,
			       DivisionType.Shashtyamsa             => 60,
			       DivisionType.NavaNavamsa             => 81,
			       DivisionType.Ashtottaramsa           => 108,
			       DivisionType.Nadiamsa                => 150,
			       DivisionType.NadiamsaCKN             => 150,
			       DivisionType.NavamsaDwadasamsa       => 108,
			       DivisionType.DwadasamsaDwadasamsa    => 144,
			       DivisionType.BhavaPada               => 1,
			       DivisionType.BhavaEqual              => 1,
			       DivisionType.BhavaAlcabitus          => 1,
			       DivisionType.BhavaAxial              => 1,
			       DivisionType.BhavaCampanus           => 1,
			       DivisionType.BhavaKoch               => 1,
			       DivisionType.BhavaPlacidus           => 1,
			       DivisionType.BhavaRegiomontanus      => 1,
			       DivisionType.BhavaSripati            => 1,
			       _                                    => throw new Exception("Unknown varga type")
		       };
	}

	public static Division[] Shadvargas() =>
	[
		new Division(DivisionType.Rasi),
		new Division(DivisionType.HoraParasara),
		new Division(DivisionType.DrekkanaParasara),
		new Division(DivisionType.Navamsa),
		new Division(DivisionType.Dwadasamsa),
		new Division(DivisionType.Trimsamsa)
	];

	public static Division[] Saptavargas() =>
	[
		new Division(DivisionType.Rasi),
		new Division(DivisionType.HoraParasara),
		new Division(DivisionType.DrekkanaParasara),
		new Division(DivisionType.Saptamsa),
		new Division(DivisionType.Navamsa),
		new Division(DivisionType.Dwadasamsa),
		new Division(DivisionType.Trimsamsa)
	];

	public static Division[] Dasavargas() =>
	[
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
	];

	public static Division[] Shodasavargas() =>
	[
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
	];

	//D-Chart Karya Rasi / Controlling House Karyesha/Significator
	// D2 2H Jupiter
	// D3 3H Mars
	// D4 4H Mars & Ketu
	// D5 5H Sun
	// D6 6H Saturn
	// D7 5H Jupiter
	// D8 8H Saturn
	// D9 7H Venus/Jupiter
	// D10 10H Saturn, Sun, Mercury & Jupiter
	// D11 11H Mars, Saturn & Nodes
	// D12 12H Sun & Moon
	// D16 4H Venus
	// D20 8H Ketu
	// D24 4H Jupiter & Mercury
	// D30 6H Mars & Saturn
	// D40 4H Moon
	// D45 9H Sun
	// D60 12H

	public static Bhava ControllingHouse(DivisionType divisionType)
	{
		return divisionType switch
		       {
			       DivisionType.Rasi             => Bhava.LagnaBhava,
			       DivisionType.HoraParasara     => Bhava.DhanaBhava,
			       DivisionType.DrekkanaParasara => Bhava.SahajaBhava,
			       DivisionType.Chaturthamsa     => Bhava.SukhaBhava,
			       DivisionType.Panchamsa        => Bhava.PutraBhava,
			       DivisionType.Shashthamsa      => Bhava.ShatruBhava,
			       DivisionType.Saptamsa         => Bhava.JayaBhava,
			       DivisionType.Ashtamsa         => Bhava.MrtyuBhava,
			       DivisionType.Navamsa          => Bhava.DharmaBhava,
			       DivisionType.Dasamsa          => Bhava.KarmaBhava,
			       DivisionType.Rudramsa         => Bhava.LabhaBhava,
			       DivisionType.Dwadasamsa       => Bhava.VyayaBhava,
			       DivisionType.Shodasamsa       => Bhava.SukhaBhava,
			       DivisionType.Vimsamsa         => Bhava.MrtyuBhava,
			       DivisionType.Trimsamsa        => Bhava.ShatruBhava,
			       DivisionType.Khavedamsa       => Bhava.SukhaBhava,
			       DivisionType.Akshavedamsa     => Bhava.DharmaBhava,
			       _                             => Bhava.LagnaBhava
		       };
	}
}

	// Rasi D-1 Existence at the physical level
	// Hora D-2 Wealth and money
	// Drekkana D-3 Everything related to brothers and sisters
	// Chaturthamsa D-4 Residence, houses owned, properties and fortune
	// Panchamsa D-5 Fame, authority and power
	// Shashthamsa D-6 Health troubles
	// Saptamsa D-7 Everything related to children (and grand-children)
	// Ashtamsa D-8 Sudden and unexpected troubles, litigation etc
	// Navamsa D-9 Marriage and everything related to spouse(s),  dharma (duty and righteousness),
	//			interaction with other people, basic skills, inner self
	// Dasamsa D-10 Career, activities and achievements in society
	// Rudramsa D-11 Death and destruction
	// Dwadasamsa D-12 Everything related to parents (also uncles, aunts and grand-parents, i.e. blood-relatives of parents)
	// Shodasamsa D-16 Vehicles, pleasures, comforts and discomforts
	// Vimsamsa D-20 Religious activities and spiritual matters
	// Chaturvimsamsa D-24 Learning, knowledge and education
	// Nakshatramsa D-27 Strengths and weaknesses, inherent nature
	// Trimsamsa D-30 Evils and punishment, sub-conscious self, some diseases
	// Khavedamsa D-40 Auspicious and inauspicious events
	// Akshavedamsa D-45 All matters
	// Shashtyamsa D-60 Karma of past life, all matters


	//a. The lord of first, 4th and 7th Navamsa is Deva which means the divine lord. It represent the qualities
	//   of generosity, power, wealth. Native born in this part of Navamsa are generous, religious and powerful.
	//b. The lord of 2nd, 5th and 8th Navamsa represent Nara which means a man or person of kind, soul and reputed. '
	//	 Engaged in the pursuits of human welfare.
	//c. The lords of 3rd, 6th and 9th Navamsa represent the charatertistics of Rakshaha or a demon. This part represent cruel,
	//	 quarry, violent, selfish and person of a negative qualities.
