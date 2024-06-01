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
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.Elements.Extensions;

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

	public int GetLength() => Xw;

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

	public Point GetInnerSquareOffset() => new(Xw / 3, Yw / 3);

	public Point GetBodyTextPosition(Longitude l, Size itemSize) => Point.Empty;

	public Point GetBodyPosition(Longitude l)
	{
		var zh      = l.ToZodiacHouse();
		var dOffset = l.ToZodiacHouseOffset();
		var iOff    = (int) (dOffset / 30.0 * (Xw / 3));
		var pBase   = GetZhouseOffset(l.ToZodiacHouse());
		switch (zh)
		{
			case ZodiacHouse.Pis:
			case ZodiacHouse.Ari:
			case ZodiacHouse.Tau:
				pBase.X -= iOff;
				break;
			case ZodiacHouse.Gem:
			case ZodiacHouse.Can:
			case ZodiacHouse.Leo:
				pBase.Y += iOff;
				break;
			case ZodiacHouse.Vir:
			case ZodiacHouse.Lib:
			case ZodiacHouse.Sco:
				pBase.X += iOff;
				break;
			case ZodiacHouse.Sag:
			case ZodiacHouse.Cap:
			case ZodiacHouse.Aqu:
				pBase.Y -= iOff;
				break;
		}

		return pBase;
	}

	public Point GetSingleItemOffset(ZodiacHouse zh, Size itemSize)
	{
		switch (zh)
		{
			case ZodiacHouse.Ari: return new Point(90, 0);
			case ZodiacHouse.Can: return new Point(5, 90);
			case ZodiacHouse.Lib: return new Point(90, 185);
			case ZodiacHouse.Cap: return new Point(180, 90);
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
		switch (zh)
		{
			case ZodiacHouse.Gem: return GetSmallGemOffset(n);
			case ZodiacHouse.Tau:
				pret   = GetSmallGemOffset(n);
				pret.Y = 0;
				pret.X = Xw / 3 - pret.X - wi;
				return pret;
			case ZodiacHouse.Pis:
				pret   =  GetSmallGemOffset(n);
				pret.Y =  0;
				pret.X += Xw * 2 / 3;
				return pret;
			case ZodiacHouse.Aqu:
				pret   = GetSmallGemOffset(n);
				pret.X = Xw / 3 - pret.X + Xw * 2 / 3 - wi;
				return pret;
			case ZodiacHouse.Vir:
				pret   =  GetSmallGemOffset(n);
				pret.X =  Xw / 3 - pret.X - wi;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Sco:
				pret   =  GetSmallGemOffset(n);
				pret.X += Xw * 2 / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Sag:
				pret   = GetSmallGemOffset(n);
				pret.Y = Yw * 2 / 3;
				pret.X = Xw / 3 - pret.X + Xw * 2 / 3 - wi;
				return pret;
			case ZodiacHouse.Leo:
				pret   = GetSmallGemOffset(n);
				pret.Y = Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Ari:
				pret   =  GetSmallGemOffset(n);
				pret.X += Xw / 3;
				return pret;
			case ZodiacHouse.Can:
				pret   =  GetSmallGemOffset(n);
				pret.Y += Yw / 3;
				return pret;
			case ZodiacHouse.Lib:
				pret   =  GetSmallGemOffset(n);
				pret.X += Xw     / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Cap:
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
		return zh switch
		       {
			       ZodiacHouse.Ari => new Point(iOff * 2, 0),
			       ZodiacHouse.Tau => new Point(iOff, 0),
			       ZodiacHouse.Gem => new Point(0, 0),
			       ZodiacHouse.Can => new Point(0, iOff),
			       ZodiacHouse.Leo => new Point(0, iOff    * 2),
			       ZodiacHouse.Vir => new Point(0, iOff    * 3),
			       ZodiacHouse.Lib => new Point(iOff, iOff * 3),
			       ZodiacHouse.Sco => new Point(iOff       * 2, iOff * 3),
			       ZodiacHouse.Sag => new Point(iOff       * 3, iOff * 3),
			       ZodiacHouse.Cap => new Point(iOff       * 3, iOff * 2),
			       ZodiacHouse.Aqu => new Point(iOff       * 3, iOff),
			       ZodiacHouse.Pis => new Point(iOff       * 3, 0),
			       _               => new Point(0, 0)
		       };
	}

	public Point GetGemOffset(int n)
	{
		var wi = Xw / 3 / 4;
		var yi = Yw / 3 / 6;
		return n switch
		       {
			       4 => new Point(0, yi          * 4),
			       3 => new Point(wi             * 1, yi * 4),
			       8 => new Point(wi             * 2, yi * 4),
			       1 => new Point(0, yi          * 3),
			       5 => new Point(wi             * 1, yi * 3),
			       2 => new Point(0, yi          * 2),
			       6 => new Point(wi * 1 - 4, yi * 2),
			       7 => new Point(0, yi          * 1),
			       _ => GetGemOffset(1)
		       };
	}

	public Point GetSmallGemOffset(int n)
	{
		var wi = Xw / 3 / 5;
		var yi = Xw / 3 / 6;
		return n switch
		       {
			       4 => new Point(0, yi * 5),
			       1 => new Point(wi    * 1, yi * 5),
			       3 => new Point(wi    * 2, yi * 5),
			       2 => new Point(wi    * 3, yi * 5),
			       5 => new Point(wi    * 4, yi * 5),
			       _ => new Point(100, 100)
		       };
	}

	public Point GetSingleGemOffset() => new(Xw / 3 / 4, Xw / 3 * 2 / 3);

	public Point FromGemOffset(ZodiacHouse zh, Point pret)
	{
		var wi = Xw / 3 / 4;
		var yi = Yw / 3 / 6;
		switch (zh)
		{
			case ZodiacHouse.Gem: return pret;
			case ZodiacHouse.Aqu:
				pret.X = Xw - pret.X - wi;
				return pret;
			case ZodiacHouse.Leo:
				pret.Y = Yw - pret.Y - yi;
				return pret;
			case ZodiacHouse.Sag:
				pret.X = Xw - pret.X - wi;
				pret.Y = Yw - pret.Y - yi;
				return pret;
			case ZodiacHouse.Pis:
				pret.X += Xw * 2 / 3;
				pret.Y =  Yw / 3 - pret.Y - yi;
				return pret;
			case ZodiacHouse.Tau:
				pret.X = Xw / 3 - pret.X - wi;
				pret.Y = Yw / 3 - pret.Y - yi;
				return pret;
			case ZodiacHouse.Vir:
				pret.X =  Xw / 3 - pret.X - wi;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Sco:
				pret.X += Xw * 2 / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Ari:
				pret.X += Xw / 3;
				return pret;
			case ZodiacHouse.Can:
				pret.Y += Yw / 3;
				return pret;
			case ZodiacHouse.Lib:
				pret.X += Xw     / 3;
				pret.Y += Yw * 2 / 3;
				return pret;
			case ZodiacHouse.Cap:
				pret.X += Xw * 2 / 3;
				pret.Y += Yw     / 3;
				return pret;
		}

		return new Point(100, 100);
	}
}