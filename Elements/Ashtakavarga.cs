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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mhora.Definitions;
using Mhora.Elements.Extensions;
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

	private Body[] _avBodies;

	public Ashtakavarga(Horoscope h, Division dtype)
	{
		_h           = h;
		_dtype = dtype;

		_avBodies =
		[
			Body.Sun,
			Body.Moon,
			Body.Mars,
			Body.Mercury,
			Body.Jupiter,
			Body.Venus,
			Body.Saturn,
			Body.Lagna
		];
	}

	public void SetKakshyaType(EKakshya k)
	{
		switch (k)
		{
			case EKakshya.EkStandard:
				_avBodies =
				[
					Body.Sun,
					Body.Moon,
					Body.Mars,
					Body.Mercury,
					Body.Jupiter,
					Body.Venus,
					Body.Saturn,
					Body.Lagna
				];
			break;

			case EKakshya.EkRegular:
			default:
				_avBodies =
				[
					Body.Saturn,
					Body.Jupiter,
					Body.Mars,
					Body.Sun,
					Body.Venus,
					Body.Mercury,
					Body.Moon,
					Body.Lagna
				];
			break;
		}

	}

	public int BodyToInt(Body b)
	{
		return Array.IndexOf(_avBodies, b);
	}

	public Body[] Bodies => _avBodies;


	public static int[][] BindusSun()
	{
		int[][] bindus =
		[
			[
				1,
				2,
				4,
				7,
				8,
				9,
				10,
				11
			],
			[
				3,
			6,
			10,
			11
			],
			[
				1,
			2,
			4,
			7,
			8,
			9,
			10,
			11
			],
			[
				3,
			5,
			6,
			9,
			10,
			11,
			12
			],
			[
				5,
			6,
			9,
			11
			],
			[
				6,
			7,
			12
			],
			[
				1,
			2,
			4,
			7,
			8,
			9,
			10,
			11
			],
			[
				3,
			4,
			6,
			10,
			11,
			12
			]
		];
		return bindus;
	}

	public static int[][] BindusMoon()
	{
		int[][] bindus =
		[
			[
				3,
				6,
				7,
				8,
				10,
				11
			],
			[
				1,
			3,
			6,
			7,
			9,
			10,
			11
			],
			[
				2,
			3,
			5,
			6,
			10,
			11
			],
			[
				1,
			3,
			4,
			5,
			7,
			8,
			10,
			11
			],
			[
				1,
			2,
			4,
			7,
			8,
			10,
			11
			],
			[
				3,
			4,
			5,
			7,
			9,
			10,
			11
			],
			[
				3,
			5,
			6,
			11
			],
			[
				3,
			6,
			10,
			11
			]
		];
		return bindus;
	}

	public static int[][] BindusMars()
	{
		int[][] bindus =
		[
			[
				3,
				5,
				6,
				10,
				11
			],
			[
				3,
			6,
			11
			],
			[
				1,
			2,
			4,
			7,
			8,
			10,
			11
			],
			[
				3,
			5,
			6,
			11
			],
			[
				6,
			10,
			11,
			12
			],
			[
				6,
			8,
			11,
			12
			],
			[
				1,
			4,
			7,
			8,
			9,
			10,
			11
			],
			[
				1,
			3,
			6,
			10,
			11
			]
		];
		return bindus;
	}

	public static int[][] BindusMercury()
	{
		int[][] bindus =
		[
			[
				5,
				6,
				9,
				11,
				12
			],
			[
				2,
			4,
			6,
			8,
			10,
			11
			],
			[
				1,
			2,
			4,
			7,
			8,
			9,
			10,
			11
			],
			[
				1,
			3,
			5,
			6,
			9,
			10,
			11,
			12
			],
			[
				6,
			8,
			11,
			12
			],
			[
				1,
			2,
			3,
			4,
			5,
			8,
			9,
			11
			],
			[
				1,
			2,
			4,
			7,
			8,
			9,
			10,
			11
			],
			[
				1,
			2,
			4,
			6,
			8,
			10,
			11
			]
		];
		return bindus;
	}

	public static int[][] BindusJupiter()
	{
		int[][] bindus =
		[
			[
				1,
				2,
				3,
				4,
				7,
				8,
				9,
				10,
				11
			],
			[
				2,
			5,
			7,
			9,
			11
			],
			[
				1,
			2,
			4,
			7,
			8,
			10,
			11
			],
			[
				1,
			2,
			4,
			5,
			6,
			9,
			10,
			11
			],
			[
				1,
			2,
			3,
			4,
			7,
			8,
			10,
			11
			],
			[
				2,
			5,
			6,
			9,
			10,
			11
			],
			[
				3,
			5,
			6,
			12
			],
			[
				1,
			2,
			4,
			5,
			6,
			7,
			9,
			10,
			11
			]
		];
		return bindus;
	}

	public static int[][] BindusVenus()
	{
		int[][] bindus =
		[
			[
				8,
				11,
				12
			],
			[
				1,
			2,
			3,
			4,
			5,
			8,
			9,
			11,
			12
			],
			[
				3,
			4,
			6,
			9,
			11,
			12
			],
			[
				3,
			5,
			6,
			9,
			11
			],
			[
				5,
			8,
			9,
			10,
			11
			],
			[
				1,
			2,
			3,
			4,
			5,
			8,
			9,
			10,
			11
			],
			[
				3,
			4,
			5,
			8,
			9,
			10,
			11
			],
			[
				1,
			2,
			3,
			4,
			5,
			8,
			9,
			11
			]
		];
		return bindus;
	}

	public static int[][] BindusSaturn()
	{
		int[][] bindus =
		[
			[
				1,
				2,
				4,
				7,
				8,
				10,
				11
			],
			[
				3,
			6,
			11
			],
			[
				3,
			5,
			6,
			10,
			11,
			12
			],
			[
				6,
			8,
			9,
			10,
			11,
			12
			],
			[
				5,
			6,
			11,
			12
			],
			[
				6,
			11,
			12
			],
			[
				3,
			5,
			6,
			11
			],
			[
				1,
			3,
			4,
			6,
			10,
			11
			]
		];
		return bindus;
	}

	public static int[][] BindusLagna()
	{
		int[][] bindus =
		[
			[
				3,
				4,
				6,
				10,
				11,
				12
			],
			[
				3,
			6,
			10,
			11,
			12
			],
			[
				1,
			3,
			6,
			10,
			11
			],
			[
				1,
			2,
			4,
			6,
			8,
			10,
			11
			],
			[
				1,
			2,
			4,
			5,
			6,
			7,
			9,
			10,
			11
			],
			[
				1,
			2,
			3,
			4,
			5,
			8,
			9
			],
			[
				1,
			3,
			4,
			6,
			10,
			11
			],
			[
				3,
			6,
			10,
			11
			]
		];
		return bindus;
	}

	public int[] GetPav(Body m)
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
		foreach (var inner in Bodies)
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

		var zl = _h.GetPosition(Body.Lagna).ToDivisionPosition(_dtype).ZodiacHouse;

		foreach (var b in Bodies)
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
		foreach (var b in Bodies)
		{
			// Lagna's bindus are not included in SAV
			if (b == Body.Lagna)
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

	public ZodiacHouse[] GetBindus(Body m, Body n)
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

		var al = new List<ZodiacHouse>();

		var zh = _h.GetPosition(n).ToDivisionPosition(_dtype).ZodiacHouse;
		foreach (var i in allBindus[BodyToInt(m)][BodyToInt(n)])
		{
			al.Add(zh.Add(i));
		}

		return al.ToArray();
	}
}