using System.ComponentModel;
using Mhora.Util;

namespace Mhora.Definitions
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

		[Description("D-81: Nava Navamsa")]
		NavaNavamsa,

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
}
