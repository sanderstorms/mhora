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

using System.Drawing;
using Mhora.Calculation;

namespace Mhora.Chart;

public class EastIndianChart : IDrawChart
{
	private const    int xw = 200;
	private const    int yw = 200;
	private readonly Pen pn_black;

	public EastIndianChart()
	{
		pn_black = new Pen(Color.Black, (float) 0.1);
	}

	public int GetLength()
	{
		return xw;
	}

	public bool SeparateGrahaHandling => false;

	public void DrawOutline(Graphics g)
	{
		g.DrawLine(pn_black, xw        / 3, 0, xw      / 3, yw);
		g.DrawLine(pn_black, xw * 2    / 3, 0, xw * 2  / 3, yw);
		g.DrawLine(pn_black, 0, yw     / 3, xw, yw     / 3);
		g.DrawLine(pn_black, 0, yw * 2 / 3, xw, yw * 2 / 3);
		g.DrawLine(pn_black, xw        / 3, yw         / 3, 0, 0);
		g.DrawLine(pn_black, xw * 2    / 3, yw         / 3, xw, 0);
		g.DrawLine(pn_black, xw        / 3, yw * 2     / 3, 0, yw);
		g.DrawLine(pn_black, xw                * 2     / 3, yw * 2 / 3, xw, yw);
	}

	public Point GetInnerSquareOffset()
	{
		return new Point(xw / 3, yw / 3);
	}

	public Point GetBodyTextPosition(Longitude l, Size itemSize)
	{
		return Point.Empty;
	}

	public Point GetBodyPosition(Longitude l)
	{
		var zh      = l.toZodiacHouse().value;
		var dOffset = l.toZodiacHouseOffset();
		var iOff    = (int) (dOffset / 30.0 * (xw / 3));
		var pBase   = GetZhouseOffset(l.toZodiacHouse());
		switch (zh)
		{
			case ZodiacHouse.Name.Pis:
			case ZodiacHouse.Name.Ari:
			case ZodiacHouse.Name.Tau:
				pBase.X -= iOff;
				break;
			case ZodiacHouse.Name.Gem:
			case ZodiacHouse.Name.Can:
			case ZodiacHouse.Name.Leo:
				pBase.Y += iOff;
				break;
			case ZodiacHouse.Name.Vir:
			case ZodiacHouse.Name.Lib:
			case ZodiacHouse.Name.Sco:
				pBase.X += iOff;
				break;
			case ZodiacHouse.Name.Sag:
			case ZodiacHouse.Name.Cap:
			case ZodiacHouse.Name.Aqu:
				pBase.Y -= iOff;
				break;
		}

		return pBase;
	}

	public Point GetSingleItemOffset(ZodiacHouse zh, Size itemSize)
	{
		switch (zh.value)
		{
			case ZodiacHouse.Name.Ari: return new Point(90, 0);
			case ZodiacHouse.Name.Can: return new Point(5, 90);
			case ZodiacHouse.Name.Lib: return new Point(90, 185);
			case ZodiacHouse.Name.Cap: return new Point(180, 90);
			default:
				var pret = GetSingleGemOffset();
				return FromGemOffset(zh, pret);
		}
	}

	public Point GetItemOffset(ZodiacHouse zh, Size itemSize, int n)
	{
		var pret = GetGemOffset(n);
		return FromGemOffset(zh, pret);
	}

	public Point GetSmallItemOffset(ZodiacHouse zh, Size itemSize, int n)
	{
		var wi = xw / 3 / 5;
		//int yi = (xw/3)/6;
		Point pret;
		switch (zh.value)
		{
			case ZodiacHouse.Name.Gem: return GetSmallGemOffset(n);
			case ZodiacHouse.Name.Tau:
				pret   = GetSmallGemOffset(n);
				pret.Y = 0;
				pret.X = xw / 3 - pret.X - wi;
				return pret;
			case ZodiacHouse.Name.Pis:
				pret   =  GetSmallGemOffset(n);
				pret.Y =  0;
				pret.X += xw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Aqu:
				pret   = GetSmallGemOffset(n);
				pret.X = xw / 3 - pret.X + xw * 2 / 3 - wi;
				return pret;
			case ZodiacHouse.Name.Vir:
				pret   =  GetSmallGemOffset(n);
				pret.X =  xw / 3 - pret.X - wi;
				pret.Y += yw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Sco:
				pret   =  GetSmallGemOffset(n);
				pret.X += xw * 2 / 3;
				pret.Y += yw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Sag:
				pret   = GetSmallGemOffset(n);
				pret.Y = yw * 2 / 3;
				pret.X = xw / 3 - pret.X + xw * 2 / 3 - wi;
				return pret;
			case ZodiacHouse.Name.Leo:
				pret   = GetSmallGemOffset(n);
				pret.Y = yw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Ari:
				pret   =  GetSmallGemOffset(n);
				pret.X += xw / 3;
				return pret;
			case ZodiacHouse.Name.Can:
				pret   =  GetSmallGemOffset(n);
				pret.Y += yw / 3;
				return pret;
			case ZodiacHouse.Name.Lib:
				pret   =  GetSmallGemOffset(n);
				pret.X += xw     / 3;
				pret.Y += yw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Cap:
				pret   =  GetSmallGemOffset(n);
				pret.X += xw * 2 / 3;
				pret.Y += yw     / 3;
				return pret;
		}

		return new Point(100, 100);
	}

	public Point GetZhouseOffset(ZodiacHouse zh)
	{
		var iOff = xw / 3;
		switch (zh.value)
		{
			case ZodiacHouse.Name.Ari: return new Point(iOff * 2, 0);
			case ZodiacHouse.Name.Tau: return new Point(iOff, 0);
			case ZodiacHouse.Name.Gem: return new Point(0, 0);
			case ZodiacHouse.Name.Can: return new Point(0, iOff);
			case ZodiacHouse.Name.Leo: return new Point(0, iOff    * 2);
			case ZodiacHouse.Name.Vir: return new Point(0, iOff    * 3);
			case ZodiacHouse.Name.Lib: return new Point(iOff, iOff * 3);
			case ZodiacHouse.Name.Sco: return new Point(iOff       * 2, iOff * 3);
			case ZodiacHouse.Name.Sag: return new Point(iOff       * 3, iOff * 3);
			case ZodiacHouse.Name.Cap: return new Point(iOff       * 3, iOff * 2);
			case ZodiacHouse.Name.Aqu: return new Point(iOff       * 3, iOff);
			case ZodiacHouse.Name.Pis: return new Point(iOff       * 3, 0);
		}

		return new Point(0, 0);
	}

	public Point GetGemOffset(int n)
	{
		var wi = xw / 3 / 4;
		var yi = yw / 3 / 6;
		switch (n)
		{
			case 4: return new Point(0, yi          * 4);
			case 3: return new Point(wi             * 1, yi * 4);
			case 8: return new Point(wi             * 2, yi * 4);
			case 1: return new Point(0, yi          * 3);
			case 5: return new Point(wi             * 1, yi * 3);
			case 2: return new Point(0, yi          * 2);
			case 6: return new Point(wi * 1 - 4, yi * 2);
			case 7: return new Point(0, yi          * 1);
		}

		return GetGemOffset(1);
	}

	public Point GetSmallGemOffset(int n)
	{
		var wi = xw / 3 / 5;
		var yi = xw / 3 / 6;
		switch (n)
		{
			case 4: return new Point(0, yi * 5);
			case 1: return new Point(wi    * 1, yi * 5);
			case 3: return new Point(wi    * 2, yi * 5);
			case 2: return new Point(wi    * 3, yi * 5);
			case 5: return new Point(wi    * 4, yi * 5);
		}

		return new Point(100, 100);
	}

	public Point GetSingleGemOffset()
	{
		return new Point(xw / 3 / 4, xw / 3 * 2 / 3);
	}

	public Point FromGemOffset(ZodiacHouse zh, Point pret)
	{
		var wi = xw / 3 / 4;
		var yi = yw / 3 / 6;
		switch (zh.value)
		{
			case ZodiacHouse.Name.Gem: return pret;
			case ZodiacHouse.Name.Aqu:
				pret.X = xw - pret.X - wi;
				return pret;
			case ZodiacHouse.Name.Leo:
				pret.Y = yw - pret.Y - yi;
				return pret;
			case ZodiacHouse.Name.Sag:
				pret.X = xw - pret.X - wi;
				pret.Y = yw - pret.Y - yi;
				return pret;
			case ZodiacHouse.Name.Pis:
				pret.X += xw * 2 / 3;
				pret.Y =  yw / 3 - pret.Y - yi;
				return pret;
			case ZodiacHouse.Name.Tau:
				pret.X = xw / 3 - pret.X - wi;
				pret.Y = yw / 3 - pret.Y - yi;
				return pret;
			case ZodiacHouse.Name.Vir:
				pret.X =  xw / 3 - pret.X - wi;
				pret.Y += yw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Sco:
				pret.X += xw * 2 / 3;
				pret.Y += yw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Ari:
				pret.X += xw / 3;
				return pret;
			case ZodiacHouse.Name.Can:
				pret.Y += yw / 3;
				return pret;
			case ZodiacHouse.Name.Lib:
				pret.X += xw     / 3;
				pret.Y += yw * 2 / 3;
				return pret;
			case ZodiacHouse.Name.Cap:
				pret.X += xw * 2 / 3;
				pret.Y += yw     / 3;
				return pret;
		}

		return new Point(100, 100);
	}
}