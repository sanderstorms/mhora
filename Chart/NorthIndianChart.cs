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
using Mhora.Body;
using Mhora.Calculation;
using mhora.Util;

namespace Mhora.Chart;

public class NorthIndianChart : IDrawChart
{
    private const int xw = 200;
    private const int yw = 200;
    private const int xo = 0;
    private const int yo = 0;

	private readonly Pen         pn_black;
	private readonly ZodiacHouse _lagna;

	public NorthIndianChart(ZodiacHouse lagna)
    {
	    _lagna = lagna;
		pn_black = new Pen(Color.Black, (float) 0.1);
    }

    public int GetLength()
    {
        return xw;
    }

    public int Bhava(ZodiacHouse zh)
    {
	    var bhava = ((zh.value.Index() - _lagna.value.Index()) + 1);

	    if (bhava <= 0)
	    {
		    bhava = 12 + bhava;
	    }
		return (bhava);
	}

    public Point GetBodyTextPosition(Longitude l, Size itemSize)
    {
	    var p     = GetBodyPosition(l);
	    var bhava = Bhava(l.toZodiacHouse());

	    p.X -= (itemSize.Width / 2);
		p.Y -= (itemSize.Height / 2);

	    switch (bhava)
	    {
		    case 12:
		    case 1:
		    case 2:
		    {
			    p.Y += 10;
		    }
			break;

		    case 3:
		    case 4:
		    case 5:
		    {
				p.X += 10;
		    }
			break;

		    case 6:
		    case 7:
		    case 8:
		    {
			    p.Y -= 10;
		    }
			break;

		    case 9:
		    case 10:
		    case 11:
		    {
				p.X -= 10;
		    }
			break;
	    }

		return (p);
    }

	public Point GetBodyPosition(Longitude l)
    {
	    var dOffset = (l.toZodiacHouseOffset() - 15.0);
	    var bhava   = Bhava(l.toZodiacHouse());
	    var pBase   = GetBhavaCentre(bhava);

		switch (bhava)
        {
	        case 12:
	        case 2:
	        case 6:
	        case 8:
	        {
		        var margin = (xw / 6) / 15.0;
				pBase.X += (int) (dOffset * margin);
	        } 
		    break;

	        case 3:
	        case 5:
	        case 9:
	        case 11:
	        {
		        var margin = (yw / 6) / 15.0;
		        pBase.Y += (int)(dOffset * margin);
			}
			break;

	        case 1:
	        case 7:
	        {
		        var margin = (xw / 4) / 15.0;
		        pBase.X += (int)(dOffset * margin);
			}
			break;

	        case 4:
	        case 10:
	        {
		        var margin = (yw / 4)  / 15.0;
		        pBase.Y += (int)(dOffset * margin);
	        }
		    break;
		}

		return pBase;
    }

    private Point GetBhavaCentre(int bhava)
    {
		switch (bhava)
	    {
			case 12: return new Point(xo + xw * 3 / 4, yo + yw * 1 / 10);
		    case 1:  return new Point(xo + xw * 2 / 4, yo + yw * 2 /  8);
		    case 2:  return new Point(xo + xw * 1 / 4, yo + yw * 1 / 10);

		    case 3: return new Point(xo + xw * 1 / 10, yo + yw * 1 / 4);
		    case 4: return new Point(xo + xw * 2 /  8, yo + yw * 2 / 4);
		    case 5: return new Point(xo + xw * 1 / 10, yo + yw * 3 / 4);

		    case 6: return new Point(xo + xw * 1 / 4, yo + yw * 9 / 10);
		    case 7: return new Point(xo + xw * 2 / 4, yo + yw * 6 /  8);
		    case 8: return new Point(xo + xw * 3 / 4, yo + yw * 9 / 10);

		    case 9:  return new Point(xo + xw * 9 / 10, yo + yw * 3 / 4);
		    case 10: return new Point(xo + xw * 6 /  8, yo + yw * 2 / 4);
		    case 11: return new Point(xo + xw * 9 / 10, yo + yw * 1 / 4);
	    }

	    return new Point(0, 0);
    }

	public Point GetInnerSquareOffset()
    {
	    return new Point(0, 0);
    }

	public void DrawOutline(Graphics g)
    {
	    g.DrawLine(pn_black, xw, yw, 0, 0);
	    g.DrawLine(pn_black, 0, yw, xw, 0);

	    g.DrawLine(pn_black, xw / 2, yw, xw, yw / 2);
	    g.DrawLine(pn_black, xw / 2, 0, xw, yw  / 2);
	    g.DrawLine(pn_black, xw / 2, 0, 0, yw   / 2);
	    g.DrawLine(pn_black, 0, yw / 2, xw / 2, yw );

	}

	public Point GetSingleItemOffset(ZodiacHouse zh, Size itemSize)
	{
		var bhava = Bhava(zh);

		var offset = GetBhavaCentre(bhava);
		offset.X -= itemSize.Width  / 2;
		offset.Y -= itemSize.Height / 2;

		return (offset);
	}

	public Point GetItemOffset(ZodiacHouse zh, int n)
	{
		var bhava = Bhava(zh);

		var p     = GetBhavaCentre(bhava);
		var q     = GetZhouseItemOffset(bhava, n);
		return new Point(p.X + q.X, p.Y + q.Y);
	}

	public Point GetSmallItemOffset(ZodiacHouse zh, int n)
	{
		var bhava = Bhava(zh);

		var p     = GetBhavaCentre(bhava);
		var q     = GetZhouseItemOffset(bhava, n);
		return new Point(p.X + q.X, p.Y + q.Y);
	}

	private Point GetSmallZhouseItemOffset(int bhava, int n)
	{
		if (n >= 7)
		{
			Debug.WriteLine("North Indian Chart (s) is too small for data");
			return new Point(0, 0);
		}

		var item_map = new int[7]
		{
			0,
			6,
			2,
			3,
			4,
			2,
			1
		};
		n = item_map[n - 1];

		var xiw = xw / 8;
		var yiw = yw / 8;

		var row = (int)Math.Floor(n / (double)3);
		var col = n - row * 3;


		return new Point(xiw * row / 3, yiw * col / 3);
	}

	private Point GetZhouseItemOffset(int bhava, int n)
	{
		if (n >= 10)
		{
			Debug.WriteLine("North Indian Chart is too small for data");
			return GetSmallZhouseItemOffset(bhava, n - 10 + 1);
		}

		switch (n)
		{
			case 1: return new Point(15, 15);
			case 2: return new Point(-15, -15);
			case 3: return new Point(15, -15);
			case 4: return new Point(-15, 15);
		}

		var item_map = new int[10]
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
		n = item_map[n] - 1;

		var xiw = xw / 8;
		var yiw = yw / 8;

		var col = (int)Math.Floor(n / (double)3);
		var row = n - col * 3;

		return new Point(xiw * row / 3, yiw * col / 3);
	}


}