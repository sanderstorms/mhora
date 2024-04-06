using System;
using Mhora.Calculation;
using Mhora.Definitions;
using Mhora.Util;

namespace Mhora.Elements.Extensions;

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


	public static Tithi ToTithi(this int index)
	{
		return (Tithi) index.NormalizeInc(1, 30);
	}

	public static NandaType ToNandaType(this Tithi tithi)
	{
		// 1 based index starting with prathama
		var t = tithi.Index();

		// check for new moon and full moon 

		if (t == 30 || t == 15)
		{
			return NandaType.Purna;
		}

		// coalesce pakshas
		if (t >= 16)
		{
			t -= 15;
		}

		switch (t)
		{
			case 1:
			case 6:
			case 11: return NandaType.Nanda;
			case 2:
			case 7:
			case 12: return NandaType.Bhadra;
			case 3:
			case 8:
			case 13: return NandaType.Jaya;
			case 4:
			case 9:
			case 14: return NandaType.Rikta;
			case 5:
			case 10: return NandaType.Purna;
		}

		return NandaType.Nanda;
	}

	public static Body GetLord(this Tithi tithi)
	{
		// 1 based index starting with prathama
		var t = tithi.Index();

		//Mhora.Log.Debug ("Looking for lord of tithi {0}", t);
		// check for new moon and full moon 
		if (t == 30)
		{
			return Body.Rahu;
		}

		if (t == 15)
		{
			return Body.Saturn;
		}

		// coalesce pakshas
		if (t >= 16)
		{
			t -= 15;
		}

		switch (t)
		{
			case 1:
			case 9: return Body.Sun;
			case 2:
			case 10: return Body.Moon;
			case 3:
			case 11: return Body.Mars;
			case 4:
			case 12: return Body.Mercury;
			case 5:
			case 13: return Body.Jupiter;
			case 6:
			case 14: return Body.Venus;
			case 7: return Body.Saturn;
			case 8: return Body.Rahu;
		}

		return Body.Sun;
	}

	public static Tithi Add(this Tithi tithi, int i)
	{
		var tnum = (tithi.Index() + i - 1).NormalizeInc(1, 30);
		return (Tithi)tnum;
	}

	public static Tithi AddReverse(this Tithi tithi, int i)
	{
		var tnum = (tithi.Index() - i + 1).NormalizeInc(1, 30);
		return (Tithi)tnum;
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

	public static Tithi ToTithi(this Longitude l)
	{
		var tIndex = (int) (Math.Floor(l.Value / (360M / 30)) + 1);
		var t      = tIndex.ToTithi();
		return t;
	}

	public static double ToTithiBase(this Longitude l)
	{
		var num  = l.ToTithi().Index();
		var cusp = (num - 1) * (360.0 / 30.0);
		return cusp;
	}

	public static double ToTithiOffset(this Longitude l)
	{
		return (double) l.Value - l.ToTithiBase();
	}

}