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
using Mhora.Components.Property;
using Mhora.Definitions;

namespace Mhora.Elements.Dasas.RasiDasa;

public class NaisargikaRasiDasa : Dasa, IDasa
{
	private readonly Horoscope   _h;
	private readonly UserOptions _options;

	public NaisargikaRasiDasa(Horoscope h)
	{
		_h       = h;
		_options = new UserOptions();
	}

	public void RecalculateOptions()
	{
	}

	public double ParamAyus()
	{
		switch (_options.ParamAyus)
		{
			case UserOptions.ParamAyusType.Long:   return 120.0;
			case UserOptions.ParamAyusType.Middle: return 108.0;
			default:                               return 96.0;
		}
	}

	public ArrayList Dasa(int cycle)
	{
		int[] order =
		{
			4,
			2,
			8,
			10,
			12,
			6,
			5,
			11,
			1,
			7,
			9,
			3
		};
		int[] shortLength =
		{
			9,
			7,
			8
		};
		var al = new ArrayList(9);

		var    cycleStart = ParamAyus() * cycle;
		var    curr        = 0.0;
		double dasaLength;
		var    zlagna = _h.GetPosition(Body.Lagna).Longitude.ToZodiacHouse();
		for (var i = 0; i < 12; i++)
		{
			var zh = zlagna.Add(order[i]);
			switch (_options.ParamAyus)
			{
				case UserOptions.ParamAyusType.Long:
					dasaLength = 10.0;
					break;
				case UserOptions.ParamAyusType.Middle:
					dasaLength = 9.0;
					break;
				default:
					var mod = (int) zh % 3;
					dasaLength = shortLength[mod];
					break;
			}

			al.Add(new DasaEntry(zh, cycleStart + curr, dasaLength, 1, zh.ToString()));
			curr += dasaLength;
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		return new ArrayList();
	}

	public string Description()
	{
		return "Naisargika Rasi Dasa";
	}

	public object GetOptions()
	{
		return _options.Clone();
	}

	public object SetOptions(object a)
	{
		var uo = (UserOptions) a;
		_options.ParamAyus = uo.ParamAyus;
		RecalculateEvent();
		return _options.Clone();
	}

	public class UserOptions : ICloneable
	{
		[PGDisplayName("Life Expectancy")]
		public enum ParamAyusType
		{
			Short,
			Middle,
			Long
		}

		public UserOptions()
		{
			ParamAyus = ParamAyusType.Middle;
		}

		[PGDisplayName("Total Param Ayus")]
		public ParamAyusType ParamAyus
		{
			get;
			set;
		}

		public object Clone()
		{
			var uo = new UserOptions
			{
				ParamAyus = ParamAyus
			};
			return uo;
		}
	}
}