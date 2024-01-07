/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.Diagnostics;
using Mhora.Tables;
using mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     A package related to a ZodiacHouse
/// </summary>
public class ZodiacHouse : ICloneable
{
	public enum Rasi
	{
		Ari = 1,
		Tau = 2,
		Gem = 3,
		Can = 4,
		Leo = 5,
		Vir = 6,
		Lib = 7,
		Sco = 8,
		Sag = 9,
		Cap = 10,
		Aqu = 11,
		Pis = 12
	}

	public enum RiseType
	{
		RisesWithHead,
		RisesWithFoot,
		RisesWithBoth
	}

	public static Rasi[] AllNames =
	{
		Rasi.Ari,
		Rasi.Tau,
		Rasi.Gem,
		Rasi.Can,
		Rasi.Leo,
		Rasi.Vir,
		Rasi.Lib,
		Rasi.Sco,
		Rasi.Sag,
		Rasi.Cap,
		Rasi.Aqu,
		Rasi.Pis
	};


	private Rasi _rasi;
	public ZodiacHouse(Rasi sign)
	{
		_rasi = sign;
	}

	public static implicit operator Rasi(ZodiacHouse zh)
	{
		return (zh._rasi);
	}

	public Rasi Sign
	{
		get => (_rasi);
		set => _rasi = value;
	}

	public Longitude Origin => new((_rasi.Index() - 1) * 30.0);

	public object Clone()
	{
		return new ZodiacHouse(_rasi);
	}

	public override string ToString()
	{
		return _rasi.ToString();
	}

	public int Normalize()
	{
		return Basics.normalize_inc(1, 12, (int) _rasi);
	}

	public ZodiacHouse Add(int i)
	{
		var znum = Basics.normalize_inc(1, 12, (int) _rasi + i - 1);
		return new ZodiacHouse((Rasi) znum);
	}

	public ZodiacHouse AddReverse(int i)
	{
		var znum = Basics.normalize_inc(1, 12, (int) _rasi - i + 1);
		return new ZodiacHouse((Rasi) znum);
	}

	public int NumHousesBetweenReverse(ZodiacHouse zrel)
	{
		return Basics.normalize_inc(1, 12, 14 - NumHousesBetween(zrel));
	}

	public int NumHousesBetween(ZodiacHouse zrel)
	{
		var ret = Basics.normalize_inc(1, 12, (int) zrel._rasi - (int) _rasi + 1);
		Trace.Assert(ret >= 1 && ret <= 12, "ZodiacHouse.numHousesBetween failed");
		return ret;
	}

	public Longitude DivisionalLongitude(Longitude longitude, int nrOfDivisions)
	{
		var houseBase = Origin;
		var div       = 30.0            / nrOfDivisions;
		var offset    = longitude.value % div;

		return new Longitude(houseBase.value + offset * nrOfDivisions);
	}

	public bool IsDaySign()
	{
		switch (_rasi)
		{
			case Rasi.Ari:
			case Rasi.Tau:
			case Rasi.Gem:
			case Rasi.Can: return false;

			case Rasi.Leo:
			case Rasi.Vir:
			case Rasi.Lib:
			case Rasi.Sco: return true;

			case Rasi.Sag:
			case Rasi.Cap: return false;

			case Rasi.Aqu:
			case Rasi.Pis: return true;

			default:
				Trace.Assert(false, "isDaySign internal error");
				return true;
		}
	}

	public bool IsOdd()
	{
		switch (_rasi)
		{
			case Rasi.Ari:
			case Rasi.Gem:
			case Rasi.Leo:
			case Rasi.Lib:
			case Rasi.Sag:
			case Rasi.Aqu: return true;

			case Rasi.Tau:
			case Rasi.Can:
			case Rasi.Vir:
			case Rasi.Sco:
			case Rasi.Cap:
			case Rasi.Pis: return false;

			default:
				Trace.Assert(false, "isOdd internal error");
				return true;
		}
	}

	public bool IsOddFooted()
	{
		switch (_rasi)
		{
			case Rasi.Ari: return true;
			case Rasi.Tau: return true;
			case Rasi.Gem: return true;
			case Rasi.Can: return false;
			case Rasi.Leo: return false;
			case Rasi.Vir: return false;
			case Rasi.Lib: return true;
			case Rasi.Sco: return true;
			case Rasi.Sag: return true;
			case Rasi.Cap: return false;
			case Rasi.Aqu: return false;
			case Rasi.Pis: return false;
		}

		Trace.Assert(false, "ZOdiacHouse::isOddFooted");
		return false;
	}

	public bool RasiDristi(ZodiacHouse b)
	{
		var ma = (int) _rasi   % 3;
		var mb = (int) b._rasi % 3;

		switch (ma)
		{
			case 1:
				if (mb == 2 && Add(2)._rasi != b._rasi)
				{
					return true;
				}

				return false;
			case 2:
				if (mb == 1 && AddReverse(2)._rasi != b._rasi)
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

	public RiseType RisesWith()
	{
		switch (_rasi)
		{
			case Rasi.Ari:
			case Rasi.Tau:
			case Rasi.Can:
			case Rasi.Sag:
			case Rasi.Cap: return RiseType.RisesWithFoot;
			case Rasi.Gem:
			case Rasi.Leo:
			case Rasi.Vir:
			case Rasi.Lib:
			case Rasi.Sco:
			case Rasi.Aqu: return RiseType.RisesWithHead;
			default: return RiseType.RisesWithBoth;
		}
	}

	public ZodiacHouse LordsOtherSign()
	{
		var ret = Rasi.Ari;
		switch (_rasi)
		{
			case Rasi.Ari:
				ret = Rasi.Sco;
				break;
			case Rasi.Tau:
				ret = Rasi.Lib;
				break;
			case Rasi.Gem:
				ret = Rasi.Vir;
				break;
			case Rasi.Can:
				ret = Rasi.Can;
				break;
			case Rasi.Leo:
				ret = Rasi.Leo;
				break;
			case Rasi.Vir:
				ret = Rasi.Gem;
				break;
			case Rasi.Lib:
				ret = Rasi.Tau;
				break;
			case Rasi.Sco:
				ret = Rasi.Ari;
				break;
			case Rasi.Sag:
				ret = Rasi.Pis;
				break;
			case Rasi.Cap:
				ret = Rasi.Aqu;
				break;
			case Rasi.Aqu:
				ret = Rasi.Cap;
				break;
			case Rasi.Pis:
				ret = Rasi.Sag;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::KalachakraMirrorSign");
				break;
		}

		return new ZodiacHouse(ret);
	}

	public ZodiacHouse AdarsaSign()
	{
		var ret = Rasi.Ari;
		switch (_rasi)
		{
			case Rasi.Ari:
				ret = Rasi.Sco;
				break;
			case Rasi.Tau:
				ret = Rasi.Lib;
				break;
			case Rasi.Gem:
				ret = Rasi.Vir;
				break;
			case Rasi.Can:
				ret = Rasi.Aqu;
				break;
			case Rasi.Leo:
				ret = Rasi.Cap;
				break;
			case Rasi.Vir:
				ret = Rasi.Gem;
				break;
			case Rasi.Lib:
				ret = Rasi.Tau;
				break;
			case Rasi.Sco:
				ret = Rasi.Ari;
				break;
			case Rasi.Sag:
				ret = Rasi.Pis;
				break;
			case Rasi.Cap:
				ret = Rasi.Leo;
				break;
			case Rasi.Aqu:
				ret = Rasi.Can;
				break;
			case Rasi.Pis:
				ret = Rasi.Sag;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::AdarsaSign");
				break;
		}

		return new ZodiacHouse(ret);
	}

	public ZodiacHouse AbhimukhaSign()
	{
		var ret = Rasi.Ari;
		switch (_rasi)
		{
			case Rasi.Ari:
				ret = Rasi.Sco;
				break;
			case Rasi.Tau:
				ret = Rasi.Lib;
				break;
			case Rasi.Gem:
				ret = Rasi.Sag;
				break;
			case Rasi.Can:
				ret = Rasi.Aqu;
				break;
			case Rasi.Leo:
				ret = Rasi.Cap;
				break;
			case Rasi.Vir:
				ret = Rasi.Pis;
				break;
			case Rasi.Lib:
				ret = Rasi.Tau;
				break;
			case Rasi.Sco:
				ret = Rasi.Ari;
				break;
			case Rasi.Sag:
				ret = Rasi.Gem;
				break;
			case Rasi.Cap:
				ret = Rasi.Leo;
				break;
			case Rasi.Aqu:
				ret = Rasi.Can;
				break;
			case Rasi.Pis:
				ret = Rasi.Vir;
				break;
			default:
				Debug.Assert(false, "ZodiacHouse::AbhimukhaSign");
				break;
		}

		return new ZodiacHouse(ret);
	}

	public static string ToShortString(Rasi z)
	{
		switch (z)
		{
			case Rasi.Ari: return "Ar";
			case Rasi.Tau: return "Ta";
			case Rasi.Gem: return "Ge";
			case Rasi.Can: return "Cn";
			case Rasi.Leo: return "Le";
			case Rasi.Vir: return "Vi";
			case Rasi.Lib: return "Li";
			case Rasi.Sco: return "Sc";
			case Rasi.Sag: return "Sg";
			case Rasi.Cap: return "Cp";
			case Rasi.Aqu: return "Aq";
			case Rasi.Pis: return "Pi";
			default:       return string.Empty;
		}
	}
}