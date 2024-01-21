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
using Mhora.Elements.Calculation;

namespace Mhora.Elements.Dasas.Graha;

public class NaisargikaGrahaDasaSP : Dasa, IDasa
{
	private readonly UserOptions options;
	private          Horoscope   h;

	public NaisargikaGrahaDasaSP(Horoscope _h)
	{
		h       = _h;
		options = new UserOptions();
	}

	public double ParamAyus()
	{
		return 108.0;
	}

	public void RecalculateOptions()
	{
	}

	public ArrayList Dasa(int cycle)
	{
		var al = new ArrayList(36);
		Body.BodyType[] order =
		{
			Body.BodyType.Moon,
			Body.BodyType.Mercury,
			Body.BodyType.Mars,
			Body.BodyType.Venus,
			Body.BodyType.Jupiter,
			Body.BodyType.Sun,
			Body.BodyType.Ketu,
			Body.BodyType.Rahu,
			Body.BodyType.Saturn
		};

		var cycle_start = ParamAyus() * cycle;
		var curr        = 0.0;
		for (var i = 0; i < 3; i++)
		{
			foreach (var bn in order)
			{
				al.Add(new DasaEntry(bn, cycle_start + curr, 4.0, 1, bn.ToString()));
				curr += 4.0;
			}
		}

		return al;
	}

	public ArrayList AntarDasa(DasaEntry pdi)
	{
		return new ArrayList();
	}

	public string Description()
	{
		return "Naisargika Graha Dasa (SP)";
	}

	public object GetOptions()
	{
		return options.Clone();
	}

	public object SetOptions(object a)
	{
		var uo = (UserOptions) a;
		RecalculateEvent?.Invoke();

		return options.Clone();
	}

	public class UserOptions : ICloneable
	{
		public object Clone()
		{
			var uo = new UserOptions();
			return uo;
		}
	}
}