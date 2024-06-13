using Mhora.Definitions;

namespace Mhora.Divisions.D60
{
	public static class Shastiamsa
	{
		//1		Ghora			Adhyapakastu Vedanam 
		//2		Rakshasa		Sevaka				 
		//3		Deva			Shastra Pathaka		 
		//4		Kubera			Ashwasadi			 
		//5		Yaksha			Bhasadi				 
		//6		Kinnara			Lipilekhan Tatpara	 
		//7		Bhrasta			Mandurabandhak		 
		//8		Kulaghna		Natya				 
		//9		Garala			Deshika				 
		//10	Agni			Yagyik				 
		//11	Maya			Guru				 
		//12	Purish			Daansheela			 
		//13	Apampati		Trinak				 
		//14	Marut			Gramani				 
		//15	Kaal			Vyasnadhip			 
		//16	Sarp/Ahi		Aaramkarani			 
		//17	Amrit			Pushpvikaraytatpara	 
		//18	Indu			Rajkaryarata		 
		//19	Mridu			Senalatapushpvikrayi 
		//20	Komala			Nirtyageetakushal	 
		//21	Heramb			Tamboolphalvikrayi	 
		//22	Brahma			Nissidhvikraykar	 
		//23	Vishnu			Gramanamadhikarkrit	 
		//24	Mahesh			Bandi				 
		//25	Deva			Deshik				 
		//26	Ardra			Pragya				 
		//27	Kalinash		Dhoopak 			 
		//28	Kshitish		Aushadhikriyah		 
		//29	Kamlakara		Kayastha			 
		//30	Gulika			Bharak				 
		//31	Mrityu			Bhandvikrayi		 
		//32	Kaal			Krishikriccha		 
		//33	Davagni			Vanik				 
		//34	Ghora			Dhatukaari			 
		//35	Yama			Charmkari			 
		//36	Kantaka			Karshak				 
		//37	Sudha			Shastradhikari		 
		//38	Amrita			Vigyani				 
		//39	Purnachandra	Tejasvi				 
		//40	Vishdagdha		Pustakoranjaka		 
		//41	Kulnash			Vanik				 
		//42	Vanshakshaya	Vedvedangvetta		 
		//43	Utpaat			Shastragya			 
		//44	Kaal			Vandipathak			 
		//45	Soumya			Gramani				 
		//46	Komala			Adhikari			 
		//47	Sheetala		Ganika				 
		//48	Drinshtakaral	Dandkarak			 
		//49	Indumukh		Bharak				 
		//50	Praveena		Indhanhaari			 
		//51	Kalagni			Phalmooladivikrayi	 
		//52	Dandayudh		Shantkrit			 
		//53	Nirmala			Swarnkari			 
		//54	Soumya			Krishikrit			 
		//55	Krura			Palvikaryi			 
		//56	Atisheetala		Yajak				 
		//57	Sudha			Adhyapak			 
		//58	Payodhi			Adhyaksha			 
		//59	Bhramana		Pratigrahpar		 
		//60	Chandrarekha	Phali				 

		public enum Deities
		{
			Ghora			= 1	, //34
			Rakshasa		= 2	,
			Deva			= 3	, //25
			Kubera			= 4	,
			Yaksha			= 5	,
			Kinnara			= 6	,
			Bhrasta			= 7	,
			Kulaghna		= 8	,
			Garala			= 9	,
			Agni			= 10,
			Maya			= 11,
			Purish			= 12,
			Apampati		= 13,
			Marut			= 14,
			Kaal			= 15, //32,44
			Sarp_Ahi		= 16,
			Amrit			= 17,
			Indu			= 18,
			Mridu			= 19,
			Komala			= 20, //46
			Heramb			= 21,
			Brahma			= 22,
			Vishnu			= 23,
			Mahesh			= 24,
			//Deva			= 25,
			Ardra			= 26,
			Kalinash		= 27,
			Kshitish		= 28,
			Kamlakara		= 29,
			Gulika			= 30,
			Mrityu			= 31,
			//Kaal			= 32,
			Davagni			= 33,
			//Ghora			= 34,
			Yama			= 35,
			Kantaka			= 36,
			Sudha			= 37, //57
			Amrita			= 38,
			Purnachandra	= 39,
			Vishdagdha		= 40,
			Kulnash			= 41,
			Vanshakshaya	= 42,
			Utpaat			= 43,
			//Kaal			= 44,
			Soumya			= 45, //54
			//Komala			= 46,
			Sheetala		= 47,
			Drinshtakaral	= 48,
			Indumukh		= 49,
			Praveena		= 50,
			Kalagni			= 51,
			Dandayudh		= 52,
			Nirmala			= 53,
			//Soumya			= 54,
			Krura			= 55,
			Atisheetala		= 56,
			//Sudha			= 57,
			Payodhi			= 58,
			Bhramana		= 59,
			Chandrarekha	= 60,
		};

		public enum Profession
		{
			AdhyapakastuVedanam		= 1	,
			Sevaka					= 2	,
			ShastraPathaka			= 3	,
			Ashwasadi				= 4	,
			Bhasadi					= 5	,
			LipilekhanTatpara		= 6	,
			Mandurabandhak			= 7	,
			Natya					= 8	,
			Deshika					= 9	,
			Yagyik					= 10,
			Guru					= 11,
			Daansheela				= 12,
			Trinak					= 13,
			Gramani					= 14, //45
			Vyasnadhip				= 15,
			Aaramkarani				= 16,
			Pushpvikaraytatpara		= 17,
			Rajkaryarata			= 18,
			Senalatapushpvikrayi	= 19,
			Nirtyageetakushal		= 20,
			Tamboolphalvikrayi		= 21,
			Nissidhvikraykar		= 22,
			Gramanamadhikarkrit		= 23,
			Bandi					= 24,
			Deshik					= 25,
			Pragya					= 26,
			Dhoopak 				= 27,
			Aushadhikriyah			= 28,
			Kayastha				= 29,
			Bharak					= 30, //49
			Bhandvikrayi			= 31,
			Krishikriccha			= 32,
			Vanik					= 33, //41
			Dhatukaari				= 34,
			Charmkari				= 35,
			Karshak					= 36,
			Shastradhikari			= 37,
			Vigyani					= 38,
			Tejasvi					= 39,
			Pustakoranjaka			= 40,
			//Vanik					= 41,
			Vedvedangvetta			= 42,
			Shastragya				= 43,
			Vandipathak				= 44,
			//Gramani					= 45,
			Adhikari				= 46,
			Ganika					= 47,
			Dandkarak				= 48,
			//Bharak					= 49,
			Indhanhaari				= 50,
			Phalmooladivikrayi		= 51,
			Shantkrit				= 52,
			Swarnkari				= 53,
			Krishikrit				= 54,
			Palvikaryi				= 55,
			Yajak					= 56,
			Adhyapak				= 57,
			Adhyaksha				= 58,
			Pratigrahpar			= 59,
			Phali					= 60,
		}

		//In the case of an odd sign, the Krura Shashtiāńśa or unpropitious 1/60th portions are 
		//1st, 2nd, 8th, 9th, 10th, 11th, 12th, 15th, 16th, 30th, 31st, 32nd, 33rd, 34th, 35th,
		//39th, 40th, 42nd, 43rd, 44th, 48th, 51st, 52nd, and 59th. 
		//The rest are Saumya or propitious ones. In the case of even signs, it is reverse, that is-the
		//Shashtiāńśa portions stated as Krura in the odd signs are the propitious or Saumya ones in
		//the even signs and vice versa.


		public static Nature [] NatureOfDiety =
		{
			Nature.Malefic, //1	
			Nature.Malefic,	//2	
			Nature.Benefic,	//3	
			Nature.Benefic,	//4	
			Nature.Neutral,	//5	
			Nature.Neutral,	//6	
			Nature.Neutral,	//7	
			Nature.Malefic,	//8	
			Nature.Neutral,	//9	
			Nature.Neutral,	//10
			Nature.Malefic,	//11
			Nature.Malefic,	//12
			Nature.Benefic,	//13
			Nature.Neutral,	//14
			Nature.Malefic,	//15
			Nature.Malefic,	//16
			Nature.Benefic,	//17
			Nature.Benefic,	//18
			Nature.Benefic,	//19
			Nature.Neutral,	//20
			Nature.Benefic,	//21
			Nature.Benefic,	//22
			Nature.Neutral,	//23
			Nature.Neutral,	//24
			Nature.Neutral,	//25
			Nature.Benefic,	//26
			Nature.Benefic,	//27
			Nature.Benefic,	//28
			Nature.Benefic,	//29
			Nature.Neutral,	//30
			Nature.Neutral,	//31
			Nature.Malefic,	//32
			Nature.Malefic,	//33
			Nature.Malefic,	//34
			Nature.Malefic,	//35
			Nature.Neutral,	//36
			Nature.Neutral,	//37
			Nature.Neutral,	//38
			Nature.Malefic,	//39
			Nature.Malefic,	//40
			Nature.Neutral,	//41
			Nature.Malefic,	//42
			Nature.Neutral,	//43
			Nature.Malefic,	//44
			Nature.Benefic,	//45
			Nature.Benefic,	//46
			Nature.Benefic,	//47
			Nature.Malefic,	//48
			Nature.Benefic,	//49
			Nature.Benefic,	//50
			Nature.Neutral,	//51
			Nature.Neutral,	//52
			Nature.Benefic,	//53
			Nature.Neutral,	//54
			Nature.Neutral,	//55
			Nature.Neutral,	//56
			Nature.Neutral,	//57
			Nature.Neutral,	//58
			Nature.Neutral,	//59
			Nature.Benefic,	//60
		};

		public static string [] Description =
		{
/*1	*/		"Teacher of Veda’s. Eternal wisdom, In modern parlance teacher of advanced subjects such as Chemistry, Engineering.",
/*2	*/		"Servant, In modern parlance one engaged in Job",
/*3	*/		"One who read Shastras, ancient Indian philosophical works dealing with knowledge. It can also mean a teacher of Shastra which includes teaching Astrology, Sanskrit, Dharma etc.",
/*4	*/		"One well versed in riding horses, in modern parlance driver of small vehicles",
/*5	*/		"One well versed in riding elephants, in the modern parlance driver of big vehicles such as aeroplanes, helicopters etc",
/*6	*/		"One engaged in writing [Books, Blogs, Articles]",
/*7	*/		"One in charge of a stable [Where horses are kept]. In modern parlance owner of parking lots, godowns, car showrooms etc",
/*8	*/		"Dancer",
/*9	*/		"It means one who is engaged in the profession of keeping records and forecasting such as Astrologers, Philosophers, Self Help Leaders, Orators etc",
/*10*/		"One who performs Yagya, austerities.",
/*11*/		"Teacher, Preceptor, Master",
/*12*/		"One indulged in Charity",
/*13*/		"Trina means Grass, Roots, Herbs and Trinaka means one dealing with them. In modern parlance, it can mean doctors, healers, beauty parlours etc",
/*14*/		"Head of a Village, In modern parlance team leaders, guides etc",
/*15*/		"One engaged in disaster management or such professions where one is endowed with the power to protect anyone or analyse anyone",
/*16*/		"One who takes care of gardens or people in horticulture, soil scientists, or those connected with plants in any manner.",
/*17*/		"One engaged in the selling of flowers can also mean designers etc or those selling vegetables.",
/*18*/		"One engaged in work of King, nowadays government servants etc",
/*19*/		"[Sena and Latapushpavikaryi are two different things, In my humble opinion it have to be Sena only because Pushpavikrayatatpara also appeared in 17th Shastiamsa] Pushpvikrayatatpara means one engaged in the trade of flowers and vegetables. Sena means people in Army",
/*20*/		"One well versed in Dance and Music",
/*21*/		"Tamboolphala means Tobacco, one engaged in its professions such as those producing or selling tobacco and other such things such as those dealing with alcohol, drug-dealers etc",
/*22*/		"Nisiddha [Prohibited] Vikray [Selling] Kar [Engaged in], One engaged in buying and selling of prohibited items",
/*23*/		"One having authority over village such as the head of a village, district magistrate, IPS/IAS etc",
/*24*/		"In prison/Prisoner",
/*25*/		"One engaged in fortune telling or prediction based on Data such as Astrologer, Big data analyst, those working in weather department etc",
/*26*/		"Highly learned",
/*27*/		"Dhoopa means scent and it may mean one dealing with fragrance items such as perfumes, incense sticks or other things such as mosquito coils etc",
/*28*/		"One dealing with medicines",
/*29*/		"One making different outlooks, this can include people such as actors etc",
/*30*/		"One carrying loads such as rickshaw-pullers, truck drivers, coolies etc",
/*31*/		"One selling utensils or can also mean dealing with other steel items",
/*32*/		"One engaged in farming",
/*33*/		"One engaged in trading, can also mean businessman",
/*34*/		"One dealing with Dhatu [Metals], or general physicians who deal with metabolism and functioning of the body can also mean to include gymnasium owners and other such people",
/*35*/		"One engaged in work related to leather",
/*36*/		"Farmer or other physical workers",
/*37*/		"One having authority over Shastra [Shastra means an expert in any knowledge]",
/*38*/		"Scientist, intellectual",
/*39*/		"It literally means lordly, it can mean authoritative people or influential people",
/*40*/		"One dealing with books or writers, authors or any other profession dealing with books such as printing press, bookshop owners etc",
/*41*/		"Trader",
/*42*/		"One who talks about Veda and Vedanga which means Teacher or people authoritative in any science/learning can also mean scholar, expert and likely people",
/*43*/		"One knowing Shastra, Knowledgeable intelligent people",
/*44*/		"One engaged in singing glories of others",
/*45*/		"Lord/King of village/small institution",
/*46*/		"Authoritative person",
/*47*/		"One engaged in the profession of calculations",
/*48*/		"One giving punishment to others",
/*49*/		"One carrying loads",
/*50*/		"One dealing with fuels",
/*51*/		"One dealing in fruits, vegetables, roots of plants and herbs",
/*52*/		"One engaged in propagating and establishing peace",
/*53*/		"One working with Gold",
/*54*/		"One engaged in farming",
/*55*/		"One dealing with meat/flesh",
/*56*/		"One engaged in doing Yagya and other Vedic rituals",
/*57*/		"Teacher",
/*58*/		"Head/Leader",
/*59*/		"One receiving donations/charity",
/*60*/		"Successful in his profession, one engaged in the stocking of fruits or cold storage, one engaged in giving results after analysis etc.",
		};

		public static string [] Meaning =
		{
/*1	*/		"Terrible",
/*2	*/		"Demon",
/*3	*/		"God",
/*4	*/		"God of Wealth",
/*5	*/		"Celestial Beings",
/*6	*/		"Genderqueer",
/*7	*/		"Corrupt",
/*8	*/		"One who commit lowly deeds with respect to his caste, like a son of teacher engaged in theft",
/*9	*/		"Poison",
/*10*/		"Fire",
/*11*/		"Illusion",
/*12*/		"Lord of a place [The place under consideration]",
/*13*/		"Aap [water] Pati [Lord] God of waters",
/*14*/		"God of Air",
/*15*/		"God of Time/God of Death/Destruction",
/*16*/		"Serpents",
/*17*/		"Nectar",
/*18*/		"Moon",
/*19*/		"Tender",
/*20*/		"Gentle",
/*21*/		"A Name for Ganesha [Obstruction Removing Deity]",
/*22*/		"Deity of Creation",
/*23*/		"Deity of Sustenance",
/*24*/		"Deity of Destruction [Shiva]",
/*25*/		"Gods",
/*26*/		"Dry",
/*27*/		"End of Bad things [Kali is the present Yuga, Kali means black/bad]",
/*28*/		"Lord of Sky [Indra]",
/*29*/		"Lord of Lotus/Name for Vishnu",
/*30*/		"Son of Saturn [ Also takes separately an entity in Jyotish]",
/*31*/		"Death",
/*32*/		"Time/Death/Destruction",
/*33*/		"Fire which catches automatically and is terrible",
/*34*/		"Terrible",
/*35*/		"God of Death",
/*36*/		"Thorn",
/*37*/		"Nectar/Honey/Water",
/*38*/		"Nectar",
/*39*/		"Full Moon/Giver of Nectar",
/*40*/		"Suffering from Poison",
/*41*/		"Destroys lineage",
/*42*/		"Lineage is destroyed",
/*43*/		"Violence/Incitement/Disaster",
/*44*/		"Time/Death/Destruction",
/*45*/		"Benefic/Benign",
/*46*/		"Gentle",
/*47*/		"Cold [Gentally Cold]",
/*48*/		"A demon with big teeth/eating something/destruction",
/*49*/		"Face like Moon/All these are used as named of Vishnu",
/*50*/		"Proficient/Well Versed",
/*51*/		"Kala [Time] Agni [Fire], Negative sense of Death as Fire",
/*52*/		"Dand [Punishment] Ayudh [Weapon]/Weapon of Punishment",
/*53*/		"Clear",
/*54*/		"Gentle",
/*55*/		"Ruthless/Cruel",
/*56*/		"Very Cold/Negative",
/*57*/		"Nectar/Honey/Water",
/*58*/		"Sea/Too much Water/Negative sense",
/*59*/		"Wandering/Good Sense",
/*60*/		"Chandra [Moon] Rekha [Line]/Line of Moon or Moonshining as a line like Moon of Shulka Tritiya/ Bad in Jyotish",
		};
	}
}
