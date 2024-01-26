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
using Mhora.Elements;

namespace Mhora.Chart;

public class EastIndianChart : IDrawChart
{
	private const    int Xw = 200;
	private const    int Yw = 200;
	private readonly Pen _pnBlack;

	public EastIndianChart()
	{
		_pnBlack = new Pen(Color.Black, (float) 0.1);
	}

	public int GetLength()
	{
		return Xw;
	}

	public bool SeparateGrahaHandling => false;

	public void DrawOutline(Graphics g)
	{
		g.DrawLine(_pnBlack, Xw        / 3, 0, Xw      / 3, Yw);
		g.DrawLine(_pnBlack, Xw * 2    / 3, 0, Xw * 2  / 3, Yw);
		g.DrawLine(_pnBlack, 0, Yw     / 3, Xw, Yw     / 3);
		g.DrawLine(_pnBlack, 0, Yw * 2 / 3, Xw, Yw * 2 / 3);
		g.DrawLine(_pnBlack, Xw        / 3, Yw         / 3, 0, 0);
		g.DrawLine(_pnBlack, Xw * 2    / 3, Yw         / 3, Xw, 0);
		g.DrawLine(_pnBlack, Xw        / 3, Yw * 2     / 3, 0, Yw);
		g.DrawLine(_pnBlack, Xw                * 2     / 3, Yw * 2 / 3, Xw, Yw);
	}

	public Point GetInnerSquareOffset()
	{
		return new Point(Xw / 3, Yw / 3);
	}

	public Point GetBodyTextPosition(Longitude l, Size itemSize)
	{
		return Point.Empty;
	}

	public Point GetBodyPosition(Longitude l)
	{
		var zh      = l.ToZodiacHouse().Sign;
		var dOffset = l.ToZodiacHouseOffset();
		var iOff    = (int) (dOffset / 30.0 * (Xw / 3));
		var pBase   = GetZhouseOffset(l.ToZodiacHouse());
		switch (zh)
		{
			case ZodiacHouse.Rasi.Pis:
			case ZodiacHouse.Rasi.Ari:
			case ZodiacHouse.Rasi.Tau:
				pBase.X -= iOff;
				break;
			case ZodiacHouse.Rasi.Gem:
			case ZodiacHouse.Rasi.Can:
			case ZodiacHouse.Rasi.Leo:
				pBase.Y += iOff;
				break;
			case ZodiacHouse.Rasi.Vir:
			case ZodiacHouse.Rasi.Lib:
			case ZodiacHouse.Rasi.Sco:
				pBase.X += iOff;
				break;
			case ZodiacHouse.Rasi.Sag:
			case ZodiacHouse.Rasi.Cap:
			case ZodiacHouse.Rasi.Aqu:
				pBase.Y -= iOff;
				break;
		}

		return pBase;
	}

	public Point GetSingleItemOffset(ZodiacHouse zh, Size itemSize)
	{
		switch (zh.Sign)
		{
			case ZodiacHouse.Rasi.Ari: return new Point(90, 0);
			case ZodiacHouse.Rasi.Can: return new Point(5, 90);
			case ZodiacHouse.Rasi.Lib: return new Point(90, 185);
			case ZodiacHouse.Rasi.Cap: return new Point(180, 90);
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
		var wi = Xw / 3 / 5;
		//int yi = (xw/3)/6;
		Point pret;
		switch (zh.Sign)
		{
			case ZodiacHouse.Rasi.Gem: return GetSmallGemOffset(n);
			case ZodiacHouse.Rasi.Tau:
				pret   = GetSmallGemOffset(n);
				pret.Y = 0;
				pret.X = Xw / 3 - pret.X - wi;
				return pret;
			case ZodiacHouse.Rasi.Pis:
				pret   =  GetSmallGemOffset(n);
				pret.Y =  0;
				pret.X += Xw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Aqu:
				pret   = GetSmallGemOffset(n);
				pret.X = Xw / 3 - pret.X + Xw * 2 / 3 - wi;
				return pret;
			case ZodiacHouse.Rasi.Vir:
				pret   =  GetSmallGemOffset(n);
				pret.X =  Xw / 3 - pret.X - wi;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Sco:
				pret   =  GetSmallGemOffset(n);
				pret.X += Xw * 2 / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Sag:
				pret   = GetSmallGemOffset(n);
				pret.Y = Yw * 2 / 3;
				pret.X = Xw / 3 - pret.X + Xw * 2 / 3 - wi;
				return pret;
			case ZodiacHouse.Rasi.Leo:
				pret   = GetSmallGemOffset(n);
				pret.Y = Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Ari:
				pret   =  GetSmallGemOffset(n);
				pret.X += Xw / 3;
				return pret;
			case ZodiacHouse.Rasi.Can:
				pret   =  GetSmallGemOffset(n);
				pret.Y += Yw / 3;
				return pret;
			case ZodiacHouse.Rasi.Lib:
				pret   =  GetSmallGemOffset(n);
				pret.X += Xw     / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Cap:
				pret   =  GetSmallGemOffset(n);
				pret.X += Xw * 2 / 3;
				pret.Y += Yw     / 3;
				return pret;
		}

		return new Point(100, 100);
	}

	public Point GetZhouseOffset(ZodiacHouse zh)
	{
		var iOff = Xw / 3;
		switch (zh.Sign)
		{
			case ZodiacHouse.Rasi.Ari: return new Point(iOff * 2, 0);
			case ZodiacHouse.Rasi.Tau: return new Point(iOff, 0);
			case ZodiacHouse.Rasi.Gem: return new Point(0, 0);
			case ZodiacHouse.Rasi.Can: return new Point(0, iOff);
			case ZodiacHouse.Rasi.Leo: return new Point(0, iOff    * 2);
			case ZodiacHouse.Rasi.Vir: return new Point(0, iOff    * 3);
			case ZodiacHouse.Rasi.Lib: return new Point(iOff, iOff * 3);
			case ZodiacHouse.Rasi.Sco: return new Point(iOff       * 2, iOff * 3);
			case ZodiacHouse.Rasi.Sag: return new Point(iOff       * 3, iOff * 3);
			case ZodiacHouse.Rasi.Cap: return new Point(iOff       * 3, iOff * 2);
			case ZodiacHouse.Rasi.Aqu: return new Point(iOff       * 3, iOff);
			case ZodiacHouse.Rasi.Pis: return new Point(iOff       * 3, 0);
		}

		return new Point(0, 0);
	}

	public Point GetGemOffset(int n)
	{
		var wi = Xw / 3 / 4;
		var yi = Yw / 3 / 6;
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
		var wi = Xw / 3 / 5;
		var yi = Xw / 3 / 6;
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
		return new Point(Xw / 3 / 4, Xw / 3 * 2 / 3);
	}

	public Point FromGemOffset(ZodiacHouse zh, Point pret)
	{
		var wi = Xw / 3 / 4;
		var yi = Yw / 3 / 6;
		switch (zh.Sign)
		{
			case ZodiacHouse.Rasi.Gem: return pret;
			case ZodiacHouse.Rasi.Aqu:
				pret.X = Xw - pret.X - wi;
				return pret;
			case ZodiacHouse.Rasi.Leo:
				pret.Y = Yw - pret.Y - yi;
				return pret;
			case ZodiacHouse.Rasi.Sag:
				pret.X = Xw - pret.X - wi;
				pret.Y = Yw - pret.Y - yi;
				return pret;
			case ZodiacHouse.Rasi.Pis:
				pret.X += Xw * 2 / 3;
				pret.Y =  Yw / 3 - pret.Y - yi;
				return pret;
			case ZodiacHouse.Rasi.Tau:
				pret.X = Xw / 3 - pret.X - wi;
				pret.Y = Yw / 3 - pret.Y - yi;
				return pret;
			case ZodiacHouse.Rasi.Vir:
				pret.X =  Xw / 3 - pret.X - wi;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Sco:
				pret.X += Xw * 2 / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Ari:
				pret.X += Xw / 3;
				return pret;
			case ZodiacHouse.Rasi.Can:
				pret.Y += Yw / 3;
				return pret;
			case ZodiacHouse.Rasi.Lib:
				pret.X += Xw     / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Rasi.Cap:
				pret.X += Xw * 2 / 3;
				pret.Y += Yw     / 3;
				return pret;
		}

		return new Point(100, 100);
	}
}