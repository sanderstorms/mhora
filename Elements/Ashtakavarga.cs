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

using System.Collections;
using System.Diagnostics;
using Mhora.Elements.Calculation;
using Mhora.Util;

namespace Mhora.Elements;

/// <summary>
///     Summary description for Ashtakavarga.
/// </summary>
public class Ashtakavarga
{
	public enum EKakshya
	{
		EkRegular,
		EkStandard
	}

	private readonly Division  _dtype;
	private readonly Horoscope _h;

	private Body.BodyType[] _avBodies;

	public Ashtakavarga(Horoscope h, Division dtype)
	{
		_h           = h;
		_dtype = dtype;
		_avBodies = new[]
		{
			Body.BodyType.Sun,
			Body.BodyType.Moon,
			Body.BodyType.Mars,
			Body.BodyType.Mercury,
			Body.BodyType.Jupiter,
			Body.BodyType.Venus,
			Body.BodyType.Saturn,
			Body.BodyType.Lagna
		};
	}

	public void SetKakshyaType(EKakshya k)
	{
		switch (k)
		{
			case EKakshya.EkStandard:
				_avBodies = new[]
				{
					Body.BodyType.Sun,
					Body.BodyType.Moon,
					Body.BodyType.Mars,
					Body.BodyType.Mercury,
					Body.BodyType.Jupiter,
					Body.BodyType.Venus,
					Body.BodyType.Saturn,
					Body.BodyType.Lagna
				};
				break;
			case EKakshya.EkRegular:
				_avBodies = new[]
				{
					Body.BodyType.Saturn,
					Body.BodyType.Jupiter,
					Body.BodyType.Mars,
					Body.BodyType.Sun,
					Body.BodyType.Venus,
					Body.BodyType.Mercury,
					Body.BodyType.Moon,
					Body.BodyType.Lagna
				};
				break;
		}
	}

	public int[][] BindusSun()
	{
		int[][] bindus =
		{
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				9,
				10,
				11
			},
			new[]
			{
				3,
				6,
				10,
				11
			},
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				9,
				10,
				11
			},
			new[]
			{
				3,
				5,
				6,
				9,
				10,
				11,
				12
			},
			new[]
			{
				5,
				6,
				9,
				11
			},
			new[]
			{
				6,
				7,
				12
			},
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				9,
				10,
				11
			},
			new[]
			{
				3,
				4,
				6,
				10,
				11,
				12
			}
		};
		return bindus;
	}

	public int[][] BindusMoon()
	{
		int[][] bindus =
		{
			new[]
			{
				3,
				6,
				7,
				8,
				10,
				11
			},
			new[]
			{
				1,
				3,
				6,
				7,
				9,
				10,
				11
			},
			new[]
			{
				2,
				3,
				5,
				6,
				10,
				11
			},
			new[]
			{
				1,
				3,
				4,
				5,
				7,
				8,
				10,
				11
			},
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				10,
				11
			},
			new[]
			{
				3,
				4,
				5,
				7,
				9,
				10,
				11
			},
			new[]
			{
				3,
				5,
				6,
				11
			},
			new[]
			{
				3,
				6,
				10,
				11
			}
		};
		return bindus;
	}

	public int[][] BindusMars()
	{
		int[][] bindus =
		{
			new[]
			{
				3,
				5,
				6,
				10,
				11
			},
			new[]
			{
				3,
				6,
				11
			},
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				10,
				11
			},
			new[]
			{
				3,
				5,
				6,
				11
			},
			new[]
			{
				6,
				10,
				11,
				12
			},
			new[]
			{
				6,
				8,
				11,
				12
			},
			new[]
			{
				1,
				4,
				7,
				8,
				9,
				10,
				11
			},
			new[]
			{
				1,
				3,
				6,
				10,
				11
			}
		};
		return bindus;
	}

	public int[][] BindusMercury()
	{
		int[][] bindus =
		{
			new[]
			{
				5,
				6,
				9,
				11,
				12
			},
			new[]
			{
				2,
				4,
				6,
				8,
				10,
				11
			},
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				9,
				10,
				11
			},
			new[]
			{
				1,
				3,
				5,
				6,
				9,
				10,
				11,
				12
			},
			new[]
			{
				6,
				8,
				11,
				12
			},
			new[]
			{
				1,
				2,
				3,
				4,
				5,
				8,
				9,
				11
			},
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				9,
				10,
				11
			},
			new[]
			{
				1,
				2,
				4,
				6,
				8,
				10,
				11
			}
		};
		return bindus;
	}

	public int[][] BindusJupiter()
	{
		int[][] bindus =
		{
			new[]
			{
				1,
				2,
				3,
				4,
				7,
				8,
				9,
				10,
				11
			},
			new[]
			{
				2,
				5,
				7,
				9,
				11
			},
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				10,
				11
			},
			new[]
			{
				1,
				2,
				4,
				5,
				6,
				9,
				10,
				11
			},
			new[]
			{
				1,
				2,
				3,
				4,
				7,
				8,
				10,
				11
			},
			new[]
			{
				2,
				5,
				6,
				9,
				10,
				11
			},
			new[]
			{
				3,
				5,
				6,
				12
			},
			new[]
			{
				1,
				2,
				4,
				5,
				6,
				7,
				9,
				10,
				11
			}
		};
		return bindus;
	}

	public int[][] BindusVenus()
	{
		int[][] bindus =
		{
			new[]
			{
				8,
				11,
				12
			},
			new[]
			{
				1,
				2,
				3,
				4,
				5,
				8,
				9,
				11,
				12
			},
			new[]
			{
				3,
				4,
				6,
				9,
				11,
				12
			},
			new[]
			{
				3,
				5,
				6,
				9,
				11
			},
			new[]
			{
				5,
				8,
				9,
				10,
				11
			},
			new[]
			{
				1,
				2,
				3,
				4,
				5,
				8,
				9,
				10,
				11
			},
			new[]
			{
				3,
				4,
				5,
				8,
				9,
				10,
				11
			},
			new[]
			{
				1,
				2,
				3,
				4,
				5,
				8,
				9,
				11
			}
		};
		return bindus;
	}

	public int[][] BindusSaturn()
	{
		int[][] bindus =
		{
			new[]
			{
				1,
				2,
				4,
				7,
				8,
				10,
				11
			},
			new[]
			{
				3,
				6,
				11
			},
			new[]
			{
				3,
				5,
				6,
				10,
				11,
				12
			},
			new[]
			{
				6,
				8,
				9,
				10,
				11,
				12
			},
			new[]
			{
				5,
				6,
				11,
				12
			},
			new[]
			{
				6,
				11,
				12
			},
			new[]
			{
				3,
				5,
				6,
				11
			},
			new[]
			{
				1,
				3,
				4,
				6,
				10,
				11
			}
		};
		return bindus;
	}

	public int[][] BindusLagna()
	{
		int[][] bindus =
		{
			new[]
			{
				3,
				4,
				6,
				10,
				11,
				12
			},
			new[]
			{
				3,
				6,
				10,
				11,
				12
			},
			new[]
			{
				1,
				3,
				6,
				10,
				11
			},
			new[]
			{
				1,
				2,
				4,
				6,
				8,
				10,
				11
			},
			new[]
			{
				1,
				2,
				4,
				5,
				6,
				7,
				9,
				10,
				11
			},
			new[]
			{
				1,
				2,
				3,
				4,
				5,
				8,
				9
			},
			new[]
			{
				1,
				3,
				4,
				6,
				10,
				11
			},
			new[]
			{
				3,
				6,
				10,
				11
			}
		};
		return bindus;
	}

	public int BodyToInt(Body.BodyType b)
	{
		switch (b)
		{
			case Body.BodyType.Sun:     return 0;
			case Body.BodyType.Moon:    return 1;
			case Body.BodyType.Mars:    return 2;
			case Body.BodyType.Mercury: return 3;
			case Body.BodyType.Jupiter: return 4;
			case Body.BodyType.Venus:   return 5;
			case Body.BodyType.Saturn:  return 6;
			case Body.BodyType.Lagna:   return 7;
			default:
				Trace.Assert(false, "Ashtakavarga:BodyToInt");
				return 0;
		}
	}

	public Body.BodyType[] GetBodies()
	{
		return _avBodies;
	}

	public int[] GetPav(Body.BodyType m)
	{
		var ret = new int[12]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		foreach (var inner in GetBodies())
		{
			foreach (var zh in GetBindus(m, inner))
			{
				ret[(int) zh - 1]++;
			}
		}

		return ret;
	}

	public int[] GetSavRao()
	{
		var sav = new int[12]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		var zl = (ZodiacHouse) _h.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(_dtype).ZodiacHouse;

		foreach (var b in GetBodies())
		{
			var pav = GetPav(b);
			Debug.Assert(pav.Length == 12, "Internal error: Pav didn't have 12 entries");

			var zb = _h.GetPosition(b).ToDivisionPosition(_dtype).ZodiacHouse;

			for (var i = 0; i < 12; i++)
			{
				var zi   = (ZodiacHouse) i + 1;
				var rasi = zb.NumHousesBetween(zi);
				rasi          =  zl.Add(rasi).Index();
				sav[rasi - 1] += pav[i];
			}
		}

		return sav;
	}

	public int[] GetSav()
	{
		var sav = new int[12]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		foreach (var b in GetBodies())
		{
			// Lagna's bindus are not included in SAV
			if (b == Body.BodyType.Lagna)
			{
				continue;
			}

			var pav = GetPav(b);
			Debug.Assert(pav.Length == 12, "Internal error: Pav didn't have 12 entries");
			for (var i = 0; i < 12; i++)
			{
				sav[i] += pav[i];
			}
		}

		return sav;
	}

	public ZodiacHouse[] GetBindus(Body.BodyType m, Body.BodyType n)
	{
		var allBindus = new int[8][][];
		allBindus[0] = BindusSun();
		allBindus[1] = BindusMoon();
		allBindus[2] = BindusMars();
		allBindus[3] = BindusMercury();
		allBindus[4] = BindusJupiter();
		allBindus[5] = BindusVenus();
		allBindus[6] = BindusSaturn();
		allBindus[7] = BindusLagna();

		var al = new ArrayList();

		var zh = (ZodiacHouse) _h.GetPosition(n).ToDivisionPosition(_dtype).ZodiacHouse;
		foreach (var i in allBindus[BodyToInt(m)][BodyToInt(n)])
		{
			al.Add(zh.Add(i));
		}

		return (ZodiacHouse[]) al.ToArray(typeof(ZodiacHouse));
	}
}