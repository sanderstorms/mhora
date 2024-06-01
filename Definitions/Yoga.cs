using System;

namespace Mhora.Definitions
{
	[Flags]
	public enum AakritiYoga : uint
	{
		None                = 0x00000001,
		AakritiArdhachandra = 0x00000002,
		AakritiChakra       = 0x00000004,
		AakritiChhatra      = 0x00000008,
		AakritiDanda        = 0x00000010,
		AakritiDhanush      = 0x00000020,
		AakritiGada         = 0x00000040,
		AakritiHalaMoksha   = 0x00000080,
		AakritiHalaKama     = 0x00000100,
		AakritiHalaArtha    = 0x00000200,
		AakritiKoota        = 0x00000400,
		AakritiNauka        = 0x00000800,
		AakritiPakshi       = 0x00001000,
		AakritiSamudra      = 0x00002000,
		AakritiShakata      = 0x00004000,
		AakritiShakti       = 0x00008000,
		AakritiShara        = 0x00010000,
		AakritiShringataka  = 0x00020000,
		AakritiVaapi        = 0x00040000,
		AakritiVajra        = 0x00080000,
		AakritiYava         = 0x00100000,
		AakritiYoopa        = 0x00200000,
	}

	[Flags]
	public enum ChandraYoga : uint
	{
		None                = 0x00000001,
		ChandraAdhi         = 0x00000002,
		ChandraAnapha       = 0x00000004,
		ChandraDurudhara    = 0x00000008,
		ChandraKalpadruma1  = 0x00000010,
		ChandraKalpadruma2  = 0x00000020,
		ChandraKalpadruma3  = 0x00000040,
		ChandraKalpadruma4  = 0x00000080,
		ChandraKalpadruma5  = 0x00000100,
		ChandraKalpadruma6  = 0x00000200,
		ChandraKalpadruma7  = 0x00000400,
		ChandraKalpadruma8  = 0x00000800,
		ChandraKalpadruma9  = 0x00001000,
		ChandraKalpadruma10 = 0x00002000,
		ChandraKalpadruma11 = 0x00004000,
		ChandraKalpadruma12 = 0x00008000,
		ChandraKalpadruma13 = 0x00010000,
		ChandraKalpadruma14 = 0x00020000,
		ChandraKalpadruma15 = 0x00040000,
		ChandraKalpadruma16 = 0x00080000,
		ChandraKalpadruma17 = 0x00100000,
		ChandraKalpadruma18 = 0x00200000,
		ChandraKalpadruma19 = 0x00400000,
		ChandraKalpadruma20 = 0x00800000,
		ChandraSunapha      = 0x01000000,
	}

	[Flags]
	public enum DhanaYoga : uint
	{
		None              = 0x00000001,
		DhanaSurya        = 0x00000002,
		DhanaGuru5th      = 0x00000004,
		DhanaLagna        = 0x00000008,
		DhanaChandra      = 0x00000010,
		Dhana2ndHouseLord = 0x00000020,
		DanaShukraLagna   = 0x00000040,
		DhanaGuru9th      = 0x00000080,
		DhanaBuddh5th     = 0x00000100,
		DhanaBuddhLagna   = 0x00000200,
		DhanaShukra5th    = 0x00000400,
		DhanaGuruLagna    = 0x00000800,
		DhanaShaniLagna   = 0x00001000,
		DhanaShani5th     = 0x00002000,
		DhanaMangal5th    = 0x00004000,
		DhanaChandra5th   = 0x00008000,
		DhanaSurya5th     = 0x00010000,
		Dhana9thLord      = 0x00020000,
		Dhana5thLord      = 0x00040000,
		DhanaMangalLagna  = 0x00080000,
		DhanaBhava        = 0x00100000,
		DhanaNight        = 0x00200000,
		DhanaDay          = 0x00400000,
	}

	[Flags]
	public enum GenericYoga : ulong
	{
		None               = 0x00000000000001,
		GajaKesariChandra  = 0x00000000000002,
		GrahaYoga          = 0x00000000000004,
		HariHaraBramha7th  = 0x00000000000008,
		HariHaraBramha2nd  = 0x00000000000010,
		HariHaraBramha1st  = 0x00000000000020,
		KalsarpaYoga       = 0x00000000000040,
		RaviUbhaychariYoga = 0x00000000000080,
		RaviVeshiYoga      = 0x00000000000100,
		RaviVoshiYoga      = 0x00000000000200,
		SankhyaYoga        = 0x00000000000400,
		AashrayaMusala     = 0x00000000000800,
		AashrayaNala       = 0x00000000001000,
		AashrayaRajju      = 0x00000000002000,
		AmalaKirti         = 0x00000000004000,
		AmaraYogaBenefics  = 0x00000000008000,
		AmaraYogaMalefics  = 0x00000000010000,
		Bheri              = 0x00000000020000,
		Chaamara1          = 0x00000000040000,
		Chaamara2          = 0x00000000080000,
		ChatussagaraKendra = 0x00000000100000,
		ChatussagaraChara  = 0x00000000200000,
		DalaMala           = 0x00000000400000,
		DalaSarpa          = 0x00000000800000,
		DalaMaala          = 0x00000001000000,
		Dhwaja             = 0x00000002000000,
		Hamsa              = 0x00000004000000,
		HathaHantaa        = 0x00000008000000,
		Indra              = 0x00000010000000,
		Kaahala1           = 0x00000020000000,
		Kaahala2           = 0x00000040000000,
		Karika             = 0x00000080000000,
		PaapaKartari       = 0x00000100000000,
		ShubhaKartari      = 0x00000200000000,
		Khadga             = 0x00000400000000,
		Lagnadhi           = 0x00000800000000,
		Lakshmi            = 0x00001000000000,
		MahaBhagyaNight    = 0x00002000000000,
		MahaBhagyaDay      = 0x00004000000000,
		MahaPataka         = 0x00008000000000,
		ParvataKendra      = 0x00010000000000,
		ParvataLagna       = 0x00020000000000,
		Saraswati          = 0x00040000000000,
		Shakata            = 0x00080000000000,
		Shankha            = 0x00100000000000,
		Shrinatha          = 0x00200000000000,
		Simhasana          = 0x00400000000000,
		AlpaUttamadi       = 0x00800000000000,
		MadhyaUttamadi     = 0x01000000000000,
		UttamaUttamadi     = 0x02000000000000,
		Vasumna            = 0x04000000000000,
	}

	[Flags]
	public enum MahaParivartanaYoga : ushort
	{
		None              = 0x00000001,
		MahaParivartana1  = 0x00000002,
		MahaParivartana2  = 0x00000004,
		MahaParivartana4  = 0x00000008,
		MahaParivartana5  = 0x00000010,
		MahaParivartana7  = 0x00000020,
		MahaParivartana9  = 0x00000040,
		MahaParivartana10 = 0x00000080,
	}
	
	[Flags]					
	public enum MahapurushaYoga : ushort
	{
		None                   = 0x00000001,
		MahapurushaBhadraBuddh = 0x00000002,
		MahapurushaHamsa       = 0x00000004,
		MahapurushaMalavya     = 0x00000008,
		MahapurushaRuchaka     = 0x00000010,
		MahapurushaSasha       = 0x00000020,
	}

	[Flags]
	public enum MalikaYoga : ushort
	{
		None          = 0x00000001,
		LabhaMalika   = 0x00000002,
		DhanuMalika   = 0x00000004,
		BhagyaMalika  = 0x00000008,
		LagnaMalika   = 0x00000010,
		KalatraMalika = 0x00000020,
		KarmaMalika   = 0x00000040,
		PutraMalika   = 0x00000080,
		RandhraMalika = 0x00000100,
		SatruMalika   = 0x00000200,
		SukhaMalika   = 0x00000400,
		VrayaMalika   = 0x00000800,
	}

	[Flags]
	public enum RajaYoga : uint
	{
		None               = 0x00000001,
		Raja1              = 0x00000002,
		Raja2              = 0x00000004,
		Raja3              = 0x00000008,
		Raja4              = 0x00000010,
		Raja5              = 0x00000020,
		Raja6              = 0x00000040,
		Raja7              = 0x00000080,
		Raja8              = 0x00000100,
		Raja9              = 0x00000200,
		Raja10             = 0x00000400,
		Raja11             = 0x00000800,
		Raja12             = 0x00001000,
		Raja13             = 0x00002000,
		Raja14             = 0x00004000,
		Raja15             = 0x00008000,
		Raja16             = 0x00010000,
		Raja17             = 0x00020000,
		Raja18             = 0x00040000,
		Raja19             = 0x00080000,
		Raja20             = 0x00100000,
		Raja21             = 0x00200000,
		Raja22             = 0x00400000,
		Raja23             = 0x00800000,
		ViparitaHarshaRaja = 0x01000000,
		ViparitaSaralaRaja = 0x02000000,
		ViparitaVimalaRaja = 0x04000000
	}
}
