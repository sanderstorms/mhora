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
using Mhora.Calculation;
using mhora.Util;

namespace Mhora.Chart;

public class NorthIndianChart : IDrawChart
{
    private const    int    xw = 200;
    private const    int    yw = 200;
    private const    int    xo = 0;
    private const    int    yo = 0;
	private readonly double diagonal;
	private readonly int    offset;
	private readonly int    travel;

	private readonly Pen         pn_black;
	private readonly ZodiacHouse _lagna;

	public Point [] BodyOffset = new Point[12];
	
	public NorthIndianChart(ZodiacHouse lagna)
    {
	    _lagna   = lagna;
		pn_black = new Pen(Color.Black, (float) 0.1);
		diagonal = Math.Sqrt(xw * xw + yw * yw);

		var r      = (diagonal / 2);
		var r2     = (r        / 6); //1/3 edge triangle
		offset = (int)(Math.Sqrt(r2 * r2 / 2));
		var r3 = 2 * r2;
		travel = (int)(Math.Sqrt(r3 * r3 * 2));

		var offset2 = 2 * offset;
		BodyOffset[0].X = ((xw / 2) + offset2);
		BodyOffset[0].Y = offset2;

		BodyOffset[3].X = offset2;
		BodyOffset[3].Y = ((yw / 2) - offset2);

		BodyOffset[6].X = (xw / 2) - offset2;
		BodyOffset[6].Y = (yw - offset2);

		BodyOffset[9].X = (xw - offset2);
		BodyOffset[9].Y = (yw / 2) + offset2;

		BodyOffset[1].X = (xw / 2) - offset;
		BodyOffset[1].Y = offset;

		BodyOffset[2].X = offset;
		BodyOffset[2].Y = offset;

		BodyOffset[4].X = offset;
		BodyOffset[4].Y = (yw / 2) + offset;

		BodyOffset[5].X = offset;
		BodyOffset[5].Y = yw - offset;

		BodyOffset[7].X = (xw / 2) + offset;
		BodyOffset[7].Y = (yw - offset);

		BodyOffset[8].X = xw - offset;
		BodyOffset[8].Y = yw - offset;

		BodyOffset[10].X = xw - offset;
		BodyOffset[10].Y = (yw / 2) - offset;

		BodyOffset[11].X = (xw - offset);
		BodyOffset[11].Y = offset;
    }

	public int GetLength()
    {
        return xw;
    }

	public bool SeparateGrahaHandling => (true);

	public Point GetInnerSquareOffset()
    {
	    return new Point(0, 0);
    }

    public void DrawOutline(Graphics g)
    {
	    g.DrawLine(pn_black, xw, yw, 0, 0);
	    g.DrawLine(pn_black, 0, yw, xw, 0);

	    g.DrawLine(pn_black, xw    / 2, yw, xw, yw / 2);
	    g.DrawLine(pn_black, xw    / 2, 0, xw, yw  / 2);
	    g.DrawLine(pn_black, xw    / 2, 0, 0, yw   / 2);
	    g.DrawLine(pn_black, 0, yw / 2, xw         / 2, yw);
		/*
	    var r      = (diagonal / 2);
	    var r2     = (r / 6);
	    var offset = (int)(Math.Sqrt(r2 * r2 / 2));

		var ri   = (int) r;
	    var o    = (int) ((xw - r) / 2);
	    var diff = (int) (xw - r);
	    g.DrawEllipse(pn_black, o, o, ri, ri);

	    g.DrawRectangle(pn_black, offset, offset, xw - 2 * offset, yw - 2 * offset);
	    g.DrawRectangle(pn_black, 2 * offset, 2 * offset, xw - 4 * offset, yw - 4 * offset);
		*/	
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
			    p.Y -= 10;
		    }
			break;

		    case 3:
		    case 4:
		    case 5:
		    {
				p.X -= 10;
		    }
			break;

		    case 6:
		    case 7:
		    case 8:
		    {
			    p.Y += 10;
		    }
			break;

		    case 9:
		    case 10:
		    case 11:
		    {
				p.X += 10;
		    }
			break;
	    }

		return (p);
    }

	//X = r * cosine(angle)  
	// Y = r * sine(angle)
	public Point GetBodyPosition(Longitude l)
	{
		var dOffset = l.toZodiacHouseOffset();
		var bhava   = Bhava(l.toZodiacHouse());
		var factor  = ((travel - 4) / 30.0);
		var degrees = (int) (dOffset * factor);

		var offset = BodyOffset [bhava - 1];
		switch (bhava)
        {
	        case 12:
	        case 1:
	        case 2:
	        {
		        offset.X -= (degrees + 2);
			}
			break;

	        case 3:
	        case 4:
	        case 5:
	        {
		        offset.Y += degrees + 2;
	        }
			break;

	        case 6:
	        case 7:
	        case 8:
	        {
		        offset.X += degrees + 2;
	        } 
		    break;

			default:
	        {
		        offset.Y -= (degrees + 2);
			}
			break;

		}

		return offset;
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
		var q     = GetSmallZhouseItemOffset(bhava, n);
		return new Point(p.X + q.X, p.Y + q.Y);
	}

	private Point GetSmallZhouseItemOffset(int bhava, int n)
	{
		var offset = BodyOffset[bhava - 1];

		switch (bhava)
		{
			case 12:
			case 1:
			case 2:
			{
				offset.Y +=  20;
				offset.X -= (n * 20);
			}
			break;

			case 6:
			case 7:
			case 8:
			{
				offset.Y -= 20;
				offset.X += (n * 20);
			}
			break;

			case 3:
			case 4:
			case 5:
			{
				offset.X += 20;
				offset.Y += (n * 20);
			}
			break;

			default:
			{
				offset.X -= 20;
				offset.Y -= (n * 20);
			}
			break;
		}

		return offset;
	}

	private Point GetZhouseItemOffset(int bhava, int n)
	{
		if (n >= 10)
		{
			Debug.WriteLine("North Indian Chart is too small for data");
			return GetSmallZhouseItemOffset(bhava, n - 10 + 1);
		}

		var shift = new Point[4];

		shift[0].Y = -offset;  //Up
		shift[1].X = -offset;  //Left
		shift[2].X = offset;   //Right
		shift[3].X = -offset;  //Down

		int shiftDirection = 0;

		n--;
		switch (bhava)
		{
			case 1:
			case 4:
			case 7:
			case 10:
			{
				shiftDirection = (n % 4);
			}
			break;

			case 2:
			case 12:
			{
				shiftDirection = (n % 3)  + 1;
			}
			break;

			case 6:
			case 8:
			{
				shiftDirection = (n % 3);
			}
			break;

			case 3:
			case 5:
			{
				if (n >= 1)
				{
					n++;
				}
				shiftDirection = (n % 4);
			}
			break;

			default:
			{
				if (n >= 2)
				{
					n++;
				}
				shiftDirection = (n % 4);
			}
			break;
		}

		return (shift[shiftDirection]);
	}

}