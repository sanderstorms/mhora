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

namespace Mhora.Chart
{
    public class SouthIndianChart : IDrawChart
    {
        private const int xw = 200;
        private const int yw = 200;
        private const int xo = 0;
        private const int yo = 0;

        private const    int hxs  = 200;
        private const    int hys  = 125;
        private const    int hsys = 75;
        private readonly Pen pn_black;

        public SouthIndianChart()
        {
            pn_black = new Pen(Color.Black, (float)0.1);
        }

        public Point GetInnerSquareOffset()
        {
            return new Point(xw / 4, yw / 4);
        }

        public int GetLength()
        {
            return xw;
        }

        public Point GetDegreeOffset(Longitude l)
        {
            var zh      = l.toZodiacHouse().value;
            var dOffset = l.toZodiacHouseOffset();
            var iOff    = (int)(dOffset / 30.0 * (xw / 4));
            var pBase   = GetZhouseOffset(l.toZodiacHouse());
            switch (zh)
            {
                case ZodiacHouse.Name.Ari:
                case ZodiacHouse.Name.Tau:
                case ZodiacHouse.Name.Gem:
                    pBase.X += iOff;
                    break;
                case ZodiacHouse.Name.Can:
                case ZodiacHouse.Name.Leo:
                case ZodiacHouse.Name.Vir:
                    pBase.X += xw / 4;
                    pBase.Y += iOff;
                    break;
                case ZodiacHouse.Name.Lib:
                case ZodiacHouse.Name.Sco:
                case ZodiacHouse.Name.Sag:
                    pBase.X += xw / 4 - iOff;
                    pBase.Y += xw / 4;
                    break;
                case ZodiacHouse.Name.Cap:
                case ZodiacHouse.Name.Aqu:
                case ZodiacHouse.Name.Pis:
                    pBase.Y += xw / 4 - iOff;
                    break;
            }

            return pBase;
        }


        public void DrawOutline(Graphics g)
        {
            g.DrawLine(pn_black, xo, yo + 0, xo              + 0, yo  + yw);
            g.DrawLine(pn_black, xo, yo + 0, xo              + xw, yo + 0);
            g.DrawLine(pn_black, xo     + xw, yo             + yw, xo + 0, yo  + yw);
            g.DrawLine(pn_black, xo     + xw, yo             + yw, xo + xw, yo + 0);
            g.DrawLine(pn_black, xo, yo + yw     / 4, xo     + xw, yo + yw     / 4);
            g.DrawLine(pn_black, xo, yo + yw * 3 / 4, xo     + xw, yo + yw * 3 / 4);
            g.DrawLine(pn_black, xo     + xw     / 4, yo, xo + xw              / 4, yo + yw);
            g.DrawLine(pn_black, xo     + xw * 3 / 4, yo, xo + xw * 3          / 4, yo + yw);
            g.DrawLine(pn_black, xo     + xw     / 2, yo, xo + xw              / 2, yo + yw          / 4);
            g.DrawLine(pn_black, xo     + xw     / 2, yo     + yw * 3          / 4, xo + xw          / 2, yo + yw);
            g.DrawLine(pn_black, xo, yo + yw     / 2, xo     + xw              / 4, yo + yw          / 2);
            g.DrawLine(pn_black, xo     + xw * 3 / 4, yo     + yw              / 2, xo + xw, yo + yw / 2);
        }

        public Point GetSingleItemOffset(ZodiacHouse zh)
        {
            var p = GetZhouseOffset(zh);
            return new Point(p.X + 15, p.Y + 15);
        }

        public Point GetItemOffset(ZodiacHouse zh, int n)
        {
            var p = GetZhouseOffset(zh);
            var q = GetZhouseItemOffset(n);
            return new Point(p.X + q.X, p.Y + q.Y);
        }

        public Point GetSmallItemOffset(ZodiacHouse zh, int n)
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

            var xiw = hxs  / 4;
            var yiw = hsys / 6;

            var row = (int)Math.Floor(n / (double)3);
            var col = n - row * 3;

            return new Point(xiw * row / 3, hys / 4 + yiw * col / 3);
        }

        private Point GetZhouseItemOffset(int n)
        {
            if (n >= 10)
            {
                Debug.WriteLine("South Indian Chart is too small for data");
                return GetSmallZhouseItemOffset(n - 10 + 1);
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

            var xiw = hxs / 4;
            var yiw = hys / 4;

            var col = (int)Math.Floor(n / (double)3);
            var row = n - col * 3;

            return new Point(xiw * row / 3, yiw * col / 3);
        }

        private Point GetZhouseOffset(ZodiacHouse zh)
        {
            switch ((int)zh.value)
            {
                case 1:  return new Point(xo + xw     / 4, yo + 0);
                case 2:  return new Point(xo + xw * 2 / 4, yo + 0);
                case 3:  return new Point(xo + xw * 3 / 4, yo + 0);
                case 4:  return new Point(xo + xw * 3 / 4, yo + yw     / 4);
                case 5:  return new Point(xo + xw * 3 / 4, yo + yw * 2 / 4);
                case 6:  return new Point(xo + xw * 3 / 4, yo + yw * 3 / 4);
                case 7:  return new Point(xo + xw * 2 / 4, yo + yw * 3 / 4);
                case 8:  return new Point(xo + xw     / 4, yo + yw * 3 / 4);
                case 9:  return new Point(xo + 0, yo          + yw * 3 / 4);
                case 10: return new Point(xo + 0, yo          + yw * 2 / 4);
                case 11: return new Point(xo + 0, yo          + yw * 1 / 4);
                case 12: return new Point(xo + 0, yo);
            }

            return new Point(0, 0);
        }
    }
}