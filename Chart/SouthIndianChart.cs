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
using System.Drawing;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;
using Mhora.Util;

namespace Mhora.Chart;

public class SouthIndianChart : IDrawChart
{
	private const int Xw = 200;
	private const int Yw = 200;
	private const int Xo = 0;
	private const int Yo = 0;

	private const    int Hxs  = 200;
	private const    int Hys  = 125;
	private const    int Hsys = 75;
	private readonly Pen _pnBlack;

	public SouthIndianChart()
	{
		_pnBlack = new Pen(Color.Black, (float) 0.1);
	}

	public bool SeparateGrahaHandling => false;

	public Point GetInnerSquareOffset() => new(Xw / 4, Yw / 4);

	public int GetLength() => Xw;

	public Point GetBodyTextPosition(Longitude l, Size itemSize) => Point.Empty;

	public Point GetBodyPosition(Longitude l)
	{
		var zh      = l.ToZodiacHouse();
		var dOffset = l.ToZodiacHouseOffset();
		var iOff    = (int) (dOffset / 30.0 * (Xw / 4));
		var pBase   = GetZhouseOffset(l.ToZodiacHouse());
		switch (zh)
		{
			case ZodiacHouse.Ari:
			case ZodiacHouse.Tau:
			case ZodiacHouse.Gem:
				pBase.X += iOff;
				break;
			case ZodiacHouse.Can:
			case ZodiacHouse.Leo:
			case ZodiacHouse.Vir:
				pBase.X += Xw / 4;
				pBase.Y += iOff;
				break;
			case ZodiacHouse.Lib:
			case ZodiacHouse.Sco:
			case ZodiacHouse.Sag:
				pBase.X += Xw / 4 - iOff;
				pBase.Y += Xw / 4;
				break;
			case ZodiacHouse.Cap:
			case ZodiacHouse.Aqu:
			case ZodiacHouse.Pis:
				pBase.Y += Xw / 4 - iOff;
				break;
		}

		return pBase;
	}


	public void DrawOutline(Graphics g)
	{
		g.DrawLine(_pnBlack, Xo, Yo + 0, Xo              + 0, Yo  + Yw);
		g.DrawLine(_pnBlack, Xo, Yo + 0, Xo              + Xw, Yo + 0);
		g.DrawLine(_pnBlack, Xo     + Xw, Yo             + Yw, Xo + 0, Yo  + Yw);
		g.DrawLine(_pnBlack, Xo     + Xw, Yo             + Yw, Xo + Xw, Yo + 0);
		g.DrawLine(_pnBlack, Xo, Yo + Yw     / 4, Xo     + Xw, Yo + Yw     / 4);
		g.DrawLine(_pnBlack, Xo, Yo + Yw * 3 / 4, Xo     + Xw, Yo + Yw * 3 / 4);
		g.DrawLine(_pnBlack, Xo     + Xw     / 4, Yo, Xo + Xw              / 4, Yo + Yw);
		g.DrawLine(_pnBlack, Xo     + Xw * 3 / 4, Yo, Xo + Xw * 3          / 4, Yo + Yw);
		g.DrawLine(_pnBlack, Xo     + Xw     / 2, Yo, Xo + Xw              / 2, Yo + Yw          / 4);
		g.DrawLine(_pnBlack, Xo     + Xw     / 2, Yo     + Yw * 3          / 4, Xo + Xw          / 2, Yo + Yw);
		g.DrawLine(_pnBlack, Xo, Yo + Yw     / 2, Xo     + Xw              / 4, Yo + Yw          / 2);
		g.DrawLine(_pnBlack, Xo     + Xw * 3 / 4, Yo     + Yw              / 2, Xo + Xw, Yo + Yw / 2);
	}

	public Point GetSingleItemOffset(ZodiacHouse zh, Size itemSize)
	{
		var p = GetZhouseOffset(zh);
		return new Point(p.X + 15, p.Y + 15);
	}

	public Point GetItemOffset(ZodiacHouse zh, Size itemSize, int n)
	{
		var p = GetZhouseOffset(zh);
		var q = GetZhouseItemOffset(n);
		return new Point(p.X + q.X, p.Y + q.Y);
	}

	public Point GetSmallItemOffset(ZodiacHouse zh, Size itemSize, int n)
	{
		var p = GetZhouseOffset(zh);
		var q = GetSmallZhouseItemOffset(n);
		return new Point(p.X + q.X, p.Y + q.Y);
	}

	private Point GetSmallZhouseItemOffset(int n)
	{
		if (n >= 7)
		{
			Debug.WriteLine("South Indian Chart (s) is too small for data");
			return new Point(0, 0);
		}

		var itemMap = new int[7]
		{
			0,
			6,
			2,
			3,
			4,
			2,
			1
		};
		n = itemMap[n - 1];

		var xiw = Hxs  / 4;
		var yiw = Hsys / 6;

		var row = (int) (n / 3.0).Floor();
		var col = n - row * 3;

		return new Point(xiw * row / 3, Hys / 4 + yiw * col / 3);
	}

	private Point GetZhouseItemOffset(int n)
	{
		if (n >= 10)
		{
			Debug.WriteLine("South Indian Chart is too small for data");
			return GetSmallZhouseItemOffset(n - 10 + 1);
		}

		var itemMap = new int[10]
		{
			0,
			5,
			7,
			9,
			3,
			1,
			2,
			4,
			6,
			8
		};
		n = itemMap[n] - 1;

		var xiw = Hxs / 4;
		var yiw = Hys / 4;

		var col = (int) (n / 3.0).Floor();
		var row = n - col * 3;

		return new Point(xiw * row / 3, yiw * col / 3);
	}

	private Point GetZhouseOffset(ZodiacHouse zh)
	{
		return zh.Index() switch
		       {
			       1  => new Point(Xo + Xw     / 4, Yo + 0),
			       2  => new Point(Xo + Xw * 2 / 4, Yo + 0),
			       3  => new Point(Xo + Xw * 3 / 4, Yo + 0),
			       4  => new Point(Xo + Xw * 3 / 4, Yo + Yw     / 4),
			       5  => new Point(Xo + Xw * 3 / 4, Yo + Yw * 2 / 4),
			       6  => new Point(Xo + Xw * 3 / 4, Yo + Yw * 3 / 4),
			       7  => new Point(Xo + Xw * 2 / 4, Yo + Yw * 3 / 4),
			       8  => new Point(Xo + Xw     / 4, Yo + Yw * 3 / 4),
			       9  => new Point(Xo + 0, Yo          + Yw * 3 / 4),
			       10 => new Point(Xo + 0, Yo          + Yw * 2 / 4),
			       11 => new Point(Xo + 0, Yo          + Yw * 1 / 4),
			       12 => new Point(Xo + 0, Yo),
			       _  => new Point(0, 0)
		       };
	}
}