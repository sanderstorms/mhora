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

		switch (d.MultipleDivisions[0].Varga)
		{
			case DivisionType.Rasi:                    return "Rasi";
			case DivisionType.Navamsa:                 return "Navamsa";
			case DivisionType.HoraParasara:            return "Parasara";
			case DivisionType.HoraJagannath:           return "Jagannath";
			case DivisionType.HoraParivrittiDwaya:     return "Parivritti";
			case DivisionType.HoraKashinath:           return "Kashinath";
			case DivisionType.DrekkanaParasara:        return "Parasara";
			case DivisionType.DrekkanaJagannath:       return "Jagannath";
			case DivisionType.DrekkanaParivrittitraya: return "Parivritti";
			case DivisionType.DrekkanaSomnath:         return "Somnath";
			case DivisionType.Chaturthamsa:            return string.Empty;
			case DivisionType.Panchamsa:               return string.Empty;
			case DivisionType.Shashthamsa:             return string.Empty;
			case DivisionType.Saptamsa:                return string.Empty;
			case DivisionType.Ashtamsa:                return "Rath";
			case DivisionType.AshtamsaRaman:           return "Raman";
			case DivisionType.Dasamsa:                 return string.Empty;
			case DivisionType.Rudramsa:                return "Rath";
			case DivisionType.RudramsaRaman:           return "Raman";
			case DivisionType.Dwadasamsa:              return string.Empty;
			case DivisionType.Shodasamsa:              return string.Empty;
			case DivisionType.Vimsamsa:                return string.Empty;
			case DivisionType.Chaturvimsamsa:          return string.Empty;
			case DivisionType.Nakshatramsa:            return string.Empty;
			case DivisionType.Trimsamsa:               return string.Empty;
			case DivisionType.TrimsamsaParivritti:     return "Parivritti";
			case DivisionType.TrimsamsaSimple:         return "Simple";
			case DivisionType.Khavedamsa:              return string.Empty;
			case DivisionType.Akshavedamsa:            return string.Empty;
			case DivisionType.NavaNavamsa:             return string.Empty;
			case DivisionType.Shashtyamsa:             return string.Empty;
			case DivisionType.Ashtottaramsa:           return string.Empty;
			case DivisionType.Nadiamsa:                return "Equal Size";
			case DivisionType.NadiamsaCKN:             return "Chandra Kala";
			case DivisionType.NavamsaDwadasamsa:       return "Composite";
			case DivisionType.DwadasamsaDwadasamsa:    return "Composite";
			case DivisionType.BhavaPada:               return "9 Padas";
			case DivisionType.BhavaEqual:              return "Equal Houses";
			case DivisionType.BhavaAlcabitus:          return "Alcabitus";
			case DivisionType.BhavaAxial:              return "Axial";
			case DivisionType.BhavaCampanus:           return "Campanus";
			case DivisionType.BhavaKoch:               return "Koch";
			case DivisionType.BhavaPlacidus:           return "Placidus";
			case DivisionType.BhavaRegiomontanus:      return "Regiomontanus";
			case DivisionType.BhavaSripati:            return "Sripati";
			case DivisionType.GenericParivritti:       return "Parivritti";
			case DivisionType.GenericShashthamsa:      return "Regular: Shashtamsa";
			case DivisionType.GenericSaptamsa:         return "Regular: Saptamsa";
			case DivisionType.GenericDasamsa:          return "Regular: Dasamsa";
			case DivisionType.GenericDwadasamsa:       return "Regular: Dwadasamsa";
			case DivisionType.GenericChaturvimsamsa:   return "Regular: Chaturvimsamsa";
			case DivisionType.GenericChaturthamsa:     return "Kendra: Chaturtamsa";
			case DivisionType.GenericNakshatramsa:     return "Kendra: Nakshatramsa";
			case DivisionType.GenericDrekkana:         return "Trikona: Drekkana";
			case DivisionType.GenericShodasamsa:       return "Trikona: Shodasamsa";
			case DivisionType.GenericVimsamsa:         return "Trikona: Vimsamsa";
		}

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
		switch (varga)
		{
			case DivisionType.Rasi:    return 1;
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
			case DivisionType.Panchamsa:    return 5;
			case DivisionType.Shashthamsa:  return 6;
			case DivisionType.Saptamsa:     return 7;
			case DivisionType.Ashtamsa:
			case DivisionType.AshtamsaRaman: return 8;
			case DivisionType.Dasamsa: return 10;
			case DivisionType.Rudramsa:
			case DivisionType.RudramsaRaman: return 11;
			case DivisionType.Dwadasamsa:     return 12;
			case DivisionType.Shodasamsa:     return 16;
			case DivisionType.Vimsamsa:       return 20;
			case DivisionType.Chaturvimsamsa: return 24;
			case DivisionType.Nakshatramsa:   return 27;
			case DivisionType.Trimsamsa:
			case DivisionType.TrimsamsaParivritti:
			case DivisionType.TrimsamsaSimple: return 30;
			case DivisionType.Khavedamsa:    return 40;
			case DivisionType.Akshavedamsa:  return 45;
			case DivisionType.Shashtyamsa:   return 60;
			case DivisionType.NavaNavamsa:   return 81;
			case DivisionType.Ashtottaramsa: return 108;
			case DivisionType.Nadiamsa:
			case DivisionType.NadiamsaCKN: return 150;
			case DivisionType.NavamsaDwadasamsa:    return 108;
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
		}

		throw new Exception("Unknown varga type");
	}

	public static Division[] Shadvargas()
	{
		return
		[
			new Division(DivisionType.Rasi),
			new Division(DivisionType.HoraParasara),
			new Division(DivisionType.DrekkanaParasara),
			new Division(DivisionType.Navamsa),
			new Division(DivisionType.Dwadasamsa),
			new Division(DivisionType.Trimsamsa)
		];
	}

	public static Division[] Saptavargas()
	{
		return
		[
			new Division(DivisionType.Rasi),
			new Division(DivisionType.HoraParasara),
			new Division(DivisionType.DrekkanaParasara),
			new Division(DivisionType.Saptamsa),
			new Division(DivisionType.Navamsa),
			new Division(DivisionType.Dwadasamsa),
			new Division(DivisionType.Trimsamsa)
		];
	}

	public static Division[] Dasavargas()
	{
		return
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
	}

	public static Division[] Shodasavargas()
	{
		return
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
	}

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
