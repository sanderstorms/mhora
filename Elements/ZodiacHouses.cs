using System.Diagnostics;
using Mhora.Definitions;
using Mhora.Elements.Yoga;
using Mhora.Util;

namespace Mhora.Elements;

public static class ZodiacHouses
{
	public enum RiseType
	{
		RisesWithHead,
		RisesWithFoot,
		RisesWithBoth
	}

	public static bool IsDaySign(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
			case ZodiacHouse.Tau:
			case ZodiacHouse.Gem:
			case ZodiacHouse.Can: return false;

			case ZodiacHouse.Leo:
			case ZodiacHouse.Vir:
			case ZodiacHouse.Lib:
			case ZodiacHouse.Sco: return true;

			case ZodiacHouse.Sag:
			case ZodiacHouse.Cap: return false;

			case ZodiacHouse.Aqu:
			case ZodiacHouse.Pis: return true;

			default:
				Trace.Assert(false, "isDaySign internal error");
				return true;
		}
	}

	public static bool IsFixedSign(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Tau:
			case ZodiacHouse.Leo:
			case ZodiacHouse.Sco:
			case ZodiacHouse.Aqu:
				return (true);
		}
		return (false);
	}

	public static bool IsMoveableSign(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
			case ZodiacHouse.Can:
			case ZodiacHouse.Lib:
			case ZodiacHouse.Cap:
				return (true);
		}
		return (false);
	}

	public static bool IsDualSign(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Gem:
			case ZodiacHouse.Vir:
			case ZodiacHouse.Sag:
			case ZodiacHouse.Pis:
				return (true);
		}
		return (false);
	}

	public static bool IsOdd(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
			case ZodiacHouse.Gem:
			case ZodiacHouse.Leo:
			case ZodiacHouse.Lib:
			case ZodiacHouse.Sag:
			case ZodiacHouse.Aqu: return true;

			case ZodiacHouse.Tau:
			case ZodiacHouse.Can:
			case ZodiacHouse.Vir:
			case ZodiacHouse.Sco:
			case ZodiacHouse.Cap:
			case ZodiacHouse.Pis: return false;

			default:
				Trace.Assert(false, "isOdd internal error");
				return true;
		}
	}

	public static bool IsOddFooted(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari: return true;
			case ZodiacHouse.Tau: return true;
			case ZodiacHouse.Gem: return true;
			case ZodiacHouse.Can: return false;
			case ZodiacHouse.Leo: return false;
			case ZodiacHouse.Vir: return false;
			case ZodiacHouse.Lib: return true;
			case ZodiacHouse.Sco: return true;
			case ZodiacHouse.Sag: return true;
			case ZodiacHouse.Cap: return false;
			case ZodiacHouse.Aqu: return false;
			case ZodiacHouse.Pis: return false;
		}

		Trace.Assert(false, "ZOdiacHouse::isOddFooted");
		return false;
	}

	public static RiseType RisesWith1695567545(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
			case ZodiacHouse.Tau:
			case ZodiacHouse.Can:
			case ZodiacHouse.Sag:
			case ZodiacHouse.Cap: return RiseType.RisesWithFoot;
			case ZodiacHouse.Gem:
			case ZodiacHouse.Leo:
			case ZodiacHouse.Vir:
			case ZodiacHouse.Lib:
			case ZodiacHouse.Sco:
			case ZodiacHouse.Aqu: return RiseType.RisesWithHead;
			default: return RiseType.RisesWithBoth;
		}
	}

	public static ZodiacHouse LordsOtherSign1417701897(this ZodiacHouse zodiacHouse)
	{
		var ret = ZodiacHouse.Ari;
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
				ret = ZodiacHouse.Sco;
				break;
			case ZodiacHouse.Tau:
				ret = ZodiacHouse.Lib;
				break;
			case ZodiacHouse.Gem:
				ret = ZodiacHouse.Vir;
				break;
			case ZodiacHouse.Can:
				ret = ZodiacHouse.Can;
				break;
			case ZodiacHouse.Leo:
				ret = ZodiacHouse.Leo;
				break;
			case ZodiacHouse.Vir:
				ret = ZodiacHouse.Gem;
				break;
			case ZodiacHouse.Lib:
				ret = ZodiacHouse.Tau;
				break;
			case ZodiacHouse.Sco:
				ret = ZodiacHouse.Ari;
				break;
			case ZodiacHouse.Sag:
				ret = ZodiacHouse.Pis;
				break;
			case ZodiacHouse.Cap:
				ret = ZodiacHouse.Aqu;
				break;
			case ZodiacHouse.Aqu:
				ret = ZodiacHouse.Cap;
				break;
			case ZodiacHouse.Pis:
				ret = ZodiacHouse.Sag;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::KalachakraMirrorSign");
				break;
		}

		return (ret);
	}

	public static ZodiacHouse AdarsaSign50056938(this ZodiacHouse zodiacHouse)
	{
		var ret = ZodiacHouse.Ari;
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
				ret = ZodiacHouse.Sco;
				break;
			case ZodiacHouse.Tau:
				ret = ZodiacHouse.Lib;
				break;
			case ZodiacHouse.Gem:
				ret = ZodiacHouse.Vir;
				break;
			case ZodiacHouse.Can:
				ret = ZodiacHouse.Aqu;
				break;
			case ZodiacHouse.Leo:
				ret = ZodiacHouse.Cap;
				break;
			case ZodiacHouse.Vir:
				ret = ZodiacHouse.Gem;
				break;
			case ZodiacHouse.Lib:
				ret = ZodiacHouse.Tau;
				break;
			case ZodiacHouse.Sco:
				ret = ZodiacHouse.Ari;
				break;
			case ZodiacHouse.Sag:
				ret = ZodiacHouse.Pis;
				break;
			case ZodiacHouse.Cap:
				ret = ZodiacHouse.Leo;
				break;
			case ZodiacHouse.Aqu:
				ret = ZodiacHouse.Can;
				break;
			case ZodiacHouse.Pis:
				ret = ZodiacHouse.Sag;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::AdarsaSign");
				break;
		}

		return (ret);
	}

	public static ZodiacHouse AbhimukhaSign917507216(this ZodiacHouse zodiacHouse)
	{
		var ret = ZodiacHouse.Ari;
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
				ret = ZodiacHouse.Sco;
				break;
			case ZodiacHouse.Tau:
				ret = ZodiacHouse.Lib;
				break;
			case ZodiacHouse.Gem:
				ret = ZodiacHouse.Sag;
				break;
			case ZodiacHouse.Can:
				ret = ZodiacHouse.Aqu;
				break;
			case ZodiacHouse.Leo:
				ret = ZodiacHouse.Cap;
				break;
			case ZodiacHouse.Vir:
				ret = ZodiacHouse.Pis;
				break;
			case ZodiacHouse.Lib:
				ret = ZodiacHouse.Tau;
				break;
			case ZodiacHouse.Sco:
				ret = ZodiacHouse.Ari;
				break;
			case ZodiacHouse.Sag:
				ret = ZodiacHouse.Gem;
				break;
			case ZodiacHouse.Cap:
				ret = ZodiacHouse.Leo;
				break;
			case ZodiacHouse.Aqu:
				ret = ZodiacHouse.Can;
				break;
			case ZodiacHouse.Pis:
				ret = ZodiacHouse.Vir;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::AbhimukhaSign");
				break;
		}

		return (ret);
	}

	public static string ToShortString(this ZodiacHouse z)
	{
		switch (z)
		{
			case ZodiacHouse.Ari: return "Ar";
			case ZodiacHouse.Tau: return "Ta";
			case ZodiacHouse.Gem: return "Ge";
			case ZodiacHouse.Can: return "Cn";
			case ZodiacHouse.Leo: return "Le";
			case ZodiacHouse.Vir: return "Vi";
			case ZodiacHouse.Lib: return "Li";
			case ZodiacHouse.Sco: return "Sc";
			case ZodiacHouse.Sag: return "Sg";
			case ZodiacHouse.Cap: return "Cp";
			case ZodiacHouse.Aqu: return "Aq";
			case ZodiacHouse.Pis: return "Pi";
			default:       return string.Empty;
		}
	}

	public static  RiseType RisesWith(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
			case ZodiacHouse.Tau:
			case ZodiacHouse.Can:
			case ZodiacHouse.Sag:
			case ZodiacHouse.Cap: 
				return RiseType.RisesWithFoot;
			case ZodiacHouse.Gem:
			case ZodiacHouse.Leo:
			case ZodiacHouse.Vir:
			case ZodiacHouse.Lib:
			case ZodiacHouse.Sco:
			case ZodiacHouse.Aqu: 
				return RiseType.RisesWithHead;
			default: return RiseType.RisesWithBoth;
		}
	}


	/// <summary>
	///     Specify the Lord of a ZodiacHouse. The owernership of the nodes is not considered
	/// </summary>
	/// <param name="zh">The House whose lord should be returned</param>
	/// <returns>The lord of zh</returns>
	public static Body SimpleLordOfZodiacHouse(this ZodiacHouse zh)
	{
		switch (zh)
		{
			case ZodiacHouse.Ari: return Body.Mars;
			case ZodiacHouse.Tau: return Body.Venus;
			case ZodiacHouse.Gem: return Body.Mercury;
			case ZodiacHouse.Can: return Body.Moon;
			case ZodiacHouse.Leo: return Body.Sun;
			case ZodiacHouse.Vir: return Body.Mercury;
			case ZodiacHouse.Lib: return Body.Venus;
			case ZodiacHouse.Sco: return Body.Mars;
			case ZodiacHouse.Sag: return Body.Jupiter;
			case ZodiacHouse.Cap: return Body.Saturn;
			case ZodiacHouse.Aqu: return Body.Saturn;
			case ZodiacHouse.Pis: return Body.Jupiter;
		}

		Trace.Assert(false, string.Format("Basics.SimpleLordOfZodiacHouse for {0} failed", (int) zh));
		return Body.Other;
	}


	public static ZodiacHouse LordsOtherSign(this ZodiacHouse zodiacHouse)
	{
		var ret = ZodiacHouse.Ari;
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
				ret = ZodiacHouse.Sco;
				break;
			case ZodiacHouse.Tau:
				ret = ZodiacHouse.Lib;
				break;
			case ZodiacHouse.Gem:
				ret = ZodiacHouse.Vir;
				break;
			case ZodiacHouse.Can:
				ret = ZodiacHouse.Can;
				break;
			case ZodiacHouse.Leo:
				ret = ZodiacHouse.Leo;
				break;
			case ZodiacHouse.Vir:
				ret = ZodiacHouse.Gem;
				break;
			case ZodiacHouse.Lib:
				ret = ZodiacHouse.Tau;
				break;
			case ZodiacHouse.Sco:
				ret = ZodiacHouse.Ari;
				break;
			case ZodiacHouse.Sag:
				ret = ZodiacHouse.Pis;
				break;
			case ZodiacHouse.Cap:
				ret = ZodiacHouse.Aqu;
				break;
			case ZodiacHouse.Aqu:
				ret = ZodiacHouse.Cap;
				break;
			case ZodiacHouse.Pis:
				ret = ZodiacHouse.Sag;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::KalachakraMirrorSign");
				break;
		}

		return (ret);
	}

	public static ZodiacHouse AdarsaSign(this ZodiacHouse zodiacHouse)
	{
		var ret = ZodiacHouse.Ari;
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
				ret = ZodiacHouse.Sco;
				break;
			case ZodiacHouse.Tau:
				ret = ZodiacHouse.Lib;
				break;
			case ZodiacHouse.Gem:
				ret = ZodiacHouse.Vir;
				break;
			case ZodiacHouse.Can:
				ret = ZodiacHouse.Aqu;
				break;
			case ZodiacHouse.Leo:
				ret = ZodiacHouse.Cap;
				break;
			case ZodiacHouse.Vir:
				ret = ZodiacHouse.Gem;
				break;
			case ZodiacHouse.Lib:
				ret = ZodiacHouse.Tau;
				break;
			case ZodiacHouse.Sco:
				ret = ZodiacHouse.Ari;
				break;
			case ZodiacHouse.Sag:
				ret = ZodiacHouse.Pis;
				break;
			case ZodiacHouse.Cap:
				ret = ZodiacHouse.Leo;
				break;
			case ZodiacHouse.Aqu:
				ret = ZodiacHouse.Can;
				break;
			case ZodiacHouse.Pis:
				ret = ZodiacHouse.Sag;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::AdarsaSign");
				break;
		}

		return (ret);
	}

	public static ZodiacHouse AbhimukhaSign(this ZodiacHouse zodiacHouse)
	{
		var ret = ZodiacHouse.Ari;
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari:
				ret = ZodiacHouse.Sco;
				break;
			case ZodiacHouse.Tau:
				ret = ZodiacHouse.Lib;
				break;
			case ZodiacHouse.Gem:
				ret = ZodiacHouse.Sag;
				break;
			case ZodiacHouse.Can:
				ret = ZodiacHouse.Aqu;
				break;
			case ZodiacHouse.Leo:
				ret = ZodiacHouse.Cap;
				break;
			case ZodiacHouse.Vir:
				ret = ZodiacHouse.Pis;
				break;
			case ZodiacHouse.Lib:
				ret = ZodiacHouse.Tau;
				break;
			case ZodiacHouse.Sco:
				ret = ZodiacHouse.Ari;
				break;
			case ZodiacHouse.Sag:
				ret = ZodiacHouse.Gem;
				break;
			case ZodiacHouse.Cap:
				ret = ZodiacHouse.Leo;
				break;
			case ZodiacHouse.Aqu:
				ret = ZodiacHouse.Can;
				break;
			case ZodiacHouse.Pis:
				ret = ZodiacHouse.Vir;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::AbhimukhaSign");
				break;
		}

		return (ret);
	}

	public static Body LordOfSign(this ZodiacHouse zodiacHouse)
	{
		switch (zodiacHouse)
		{
			case ZodiacHouse.Ari: 
			case ZodiacHouse.Sco:
				return Body.Mars;
			case ZodiacHouse.Tau:
			case ZodiacHouse.Lib:
				return Body.Venus;
			case ZodiacHouse.Gem:
			case ZodiacHouse.Vir:
				return Body.Mercury;
			case ZodiacHouse.Can:
				return Body.Moon;
			case ZodiacHouse.Leo:
				return Body.Sun;
			case ZodiacHouse.Sag:
			case ZodiacHouse.Pis:
				return Body.Jupiter;
			case ZodiacHouse.Cap:
			case ZodiacHouse.Aqu:
				return Body.Saturn;
			default:
				Debug.Assert(false, "ZodiacHouse::KalachakraMirrorSign");
				break;
		}

		return Body.Lagna;
	}


	public static bool RasiDristi(this ZodiacHouse zodiacHouse, ZodiacHouse b)
	{
		var ma = (int) zodiacHouse   % 3;
		var mb = (int) b % 3;

		switch (ma)
		{
			case 1:
				if (mb == 2 && zodiacHouse.Add(2) != b)
				{
					return true;
				}

				return false;
			case 2:
				if (mb == 1 && zodiacHouse.AddReverse(2) != b)
				{
					return true;
				}

				return false;
			case 0:
				if (mb == 0)
				{
					return true;
				}

				return false;
		}

		Trace.Assert(false, "ZodiacHouse.RasiDristi");
		return false;
	}

	public static ZodiacHouse Add(this ZodiacHouse zodiacHouse, int i)
	{
		var znum = ((int) zodiacHouse + i - 1).NormalizeInc(1, 12);
		return ((ZodiacHouse) znum);
	}

	public static ZodiacHouse AddReverse(this ZodiacHouse zodiacHouse, int i)
	{
		var znum = ((int) zodiacHouse - i + 1).NormalizeInc(1, 12);
		return ((ZodiacHouse) znum);
	}

	public static int Normalize(this ZodiacHouse zodiacHouse)
	{
		return zodiacHouse.Index ().NormalizeInc(1, 12);
	}

	public static int NumHousesBetweenReverse(this ZodiacHouse zodiacHouse, ZodiacHouse zrel)
	{
		return (14 - zodiacHouse.NumHousesBetween(zrel)).NormalizeInc(1, 12);
	}

	public static int NumHousesBetween(this ZodiacHouse zodiacHouse, ZodiacHouse zrel)
	{
		var ret = ((int) zrel - (int) zodiacHouse + 1).NormalizeInc(1, 12);
		Trace.Assert(ret >= 1 && ret <= 12, "ZodiacHouse.numHousesBetween failed");
		return ret;
	}

	public static Longitude Origin (this ZodiacHouse zodiacHouse)
	{
		return new Longitude((zodiacHouse.Index() - 1) * 30.0);
	}

	public static Longitude DivisionalLongitude(this ZodiacHouse zodiacHouse, Longitude longitude, int nrOfDivisions)
	{
		var houseBase = zodiacHouse.Origin ();
		var div       = 30.0            / nrOfDivisions;
		var offset    = longitude.Value % div;

		return new Longitude(houseBase.Value + offset * nrOfDivisions);
	}
}