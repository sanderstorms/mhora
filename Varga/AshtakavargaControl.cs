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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Chart;
using Mhora.Components;
using Mhora.Components.Property;
using Mhora.Delegates;
using Mhora.Settings;

namespace Mhora.Varga;

public class AshtakavargaControl : MhoraControl
{
    public enum EChartStyle
    {
        SouthIndian,
        EastIndian
    }

    public enum EDisplayStyle
    {
        Chancha,
        NavaSav
    }

    public enum ESavType
    {
        Normal,
        Rao
    }

    private readonly Brush b_black;


    private readonly IContainer          components = null;
    private readonly Body.Body.Name[]    innerBodies;
    private readonly AshtakavargaOptions userOptions;

    private Ashtakavarga  av;
    private Brush         b_red;
    private Bitmap        bmpBuffer;
    private ContextMenu   contextMenu;
    private Font          fBig;
    private Font          fBigBold;
    private EDisplayStyle mDisplayStyle = EDisplayStyle.Chancha;
    private MenuItem      menuItem1;
    private MenuItem      menuItem2;
    private MenuItem      menuJhoraSav;
    private MenuItem      menuOptions;
    private MenuItem      menuPavJupiter;
    private MenuItem      menuPavLagna;
    private MenuItem      menuPavMars;
    private MenuItem      menuPavMercury;

    private MenuItem         menuPavMoon;
    private MenuItem         menuPavSaturn;
    private MenuItem         menuPavSun;
    private MenuItem         menuPavVenus;
    private MenuItem         menuSav;
    private Body.Body.Name[] outerBodies;

    public bool PrintMode = false;

    public AshtakavargaControl(Horoscope _h)
    {
        // This call is required by the Windows Form Designer.
        InitializeComponent();
        userOptions                            =  new AshtakavargaOptions();
        h                                      =  _h;
        h.Changed                              += OnRecalculate;
        MhoraGlobalOptions.DisplayPrefsChanged += onRedisplay;
        av                                     =  new Ashtakavarga(h, userOptions.VargaType);
        outerBodies = new[]
        {
            Body.Body.Name.Sun,
            Body.Body.Name.Moon,
            Body.Body.Name.Mars,
            Body.Body.Name.Mercury,
            Body.Body.Name.Jupiter,
            Body.Body.Name.Venus,
            Body.Body.Name.Saturn,
            Body.Body.Name.Lagna
        };

        b_black = new SolidBrush(Color.Black);

        innerBodies = (Body.Body.Name[]) outerBodies.Clone();
        resetContextMenuChecks(menuSav);
        onRedisplay(MhoraGlobalOptions.Instance);
    }

    /// <summary>
    ///     Clean up any resources being used.
    /// </summary>
    private void onRedisplay(object o)
    {
        userOptions.ChartStyle = (EChartStyle) MhoraGlobalOptions.Instance.VargaStyle;
        fBig                   = new Font(MhoraGlobalOptions.Instance.GeneralFont.FontFamily, MhoraGlobalOptions.Instance.GeneralFont.SizeInPoints + 3);
        fBigBold               = new Font(MhoraGlobalOptions.Instance.GeneralFont.FontFamily, MhoraGlobalOptions.Instance.GeneralFont.SizeInPoints + 3, FontStyle.Bold | FontStyle.Underline | FontStyle.Italic);
        b_red                  = new SolidBrush(MhoraGlobalOptions.Instance.VargaGrahaColor);
        DrawToBuffer();
        Invalidate();
    }

    private void OnRecalculate(object _h)
    {
        av = new Ashtakavarga(h, userOptions.VargaType);
        DrawToBuffer();
        Invalidate();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }
        }

        base.Dispose(disposing);
    }

#region Designer generated code

    /// <summary>
    ///     Required method for Designer support - do not modify
    ///     the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.contextMenu    = new System.Windows.Forms.ContextMenu();
        this.menuOptions    = new System.Windows.Forms.MenuItem();
        this.menuJhoraSav   = new System.Windows.Forms.MenuItem();
        this.menuSav        = new System.Windows.Forms.MenuItem();
        this.menuPavSun     = new System.Windows.Forms.MenuItem();
        this.menuPavMoon    = new System.Windows.Forms.MenuItem();
        this.menuPavMars    = new System.Windows.Forms.MenuItem();
        this.menuPavMercury = new System.Windows.Forms.MenuItem();
        this.menuPavJupiter = new System.Windows.Forms.MenuItem();
        this.menuPavVenus   = new System.Windows.Forms.MenuItem();
        this.menuPavSaturn  = new System.Windows.Forms.MenuItem();
        this.menuPavLagna   = new System.Windows.Forms.MenuItem();
        this.menuItem1      = new System.Windows.Forms.MenuItem();
        this.menuItem2      = new System.Windows.Forms.MenuItem();
        // 
        // contextMenu
        // 
        this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
        {
            this.menuOptions,
            this.menuJhoraSav,
            this.menuSav,
            this.menuPavSun,
            this.menuPavMoon,
            this.menuPavMars,
            this.menuPavMercury,
            this.menuPavJupiter,
            this.menuPavVenus,
            this.menuPavSaturn,
            this.menuPavLagna,
            this.menuItem1,
            this.menuItem2
        });
        // 
        // menuOptions
        // 
        this.menuOptions.Index =  0;
        this.menuOptions.Text  =  "Options";
        this.menuOptions.Click += new System.EventHandler(this.menuOptions_Click);
        // 
        // menuJhoraSav
        // 
        this.menuJhoraSav.Index =  1;
        this.menuJhoraSav.Text  =  "SAV, PAV";
        this.menuJhoraSav.Click += new System.EventHandler(this.menuJhoraSav_Click);
        // 
        // menuSav
        // 
        this.menuSav.Index =  2;
        this.menuSav.Text  =  "SAV, PAV, BAV";
        this.menuSav.Click += new System.EventHandler(this.menuSav_Click);
        // 
        // menuPavSun
        // 
        this.menuPavSun.Index =  3;
        this.menuPavSun.Text  =  "PAV - Sun";
        this.menuPavSun.Click += new System.EventHandler(this.menuPavSun_Click);
        // 
        // menuPavMoon
        // 
        this.menuPavMoon.Index =  4;
        this.menuPavMoon.Text  =  "PAV - Moon";
        this.menuPavMoon.Click += new System.EventHandler(this.menuPavMoon_Click);
        // 
        // menuPavMars
        // 
        this.menuPavMars.Index =  5;
        this.menuPavMars.Text  =  "PAV - Mars";
        this.menuPavMars.Click += new System.EventHandler(this.menuPavMars_Click);
        // 
        // menuPavMercury
        // 
        this.menuPavMercury.Index =  6;
        this.menuPavMercury.Text  =  "PAV - Mercury";
        this.menuPavMercury.Click += new System.EventHandler(this.menuPavMercury_Click);
        // 
        // menuPavJupiter
        // 
        this.menuPavJupiter.Index =  7;
        this.menuPavJupiter.Text  =  "PAV - Jupiter";
        this.menuPavJupiter.Click += new System.EventHandler(this.menuPavJupiter_Click);
        // 
        // menuPavVenus
        // 
        this.menuPavVenus.Index =  8;
        this.menuPavVenus.Text  =  "PAV - Venus";
        this.menuPavVenus.Click += new System.EventHandler(this.menuPavVenus_Click);
        // 
        // menuPavSaturn
        // 
        this.menuPavSaturn.Index =  9;
        this.menuPavSaturn.Text  =  "PAV - Saturn";
        this.menuPavSaturn.Click += new System.EventHandler(this.menuPavSaturn_Click);
        // 
        // menuPavLagna
        // 
        this.menuPavLagna.Index =  10;
        this.menuPavLagna.Text  =  "PAV - Lagna";
        this.menuPavLagna.Click += new System.EventHandler(this.menuPavLagna_Click);
        // 
        // menuItem1
        // 
        this.menuItem1.Index = 11;
        this.menuItem1.Text  = "-";
        // 
        // menuItem2
        // 
        this.menuItem2.Index = 12;
        this.menuItem2.Text  = "-";
        // 
        // AshtakavargaControl
        // 
        this.AllowDrop   =  true;
        this.ContextMenu =  this.contextMenu;
        this.Name        =  "AshtakavargaControl";
        this.Size        =  new System.Drawing.Size(208, 128);
        this.DragEnter   += new System.Windows.Forms.DragEventHandler(this.AshtakavargaControl_DragEnter);
        this.Resize      += new System.EventHandler(this.AshtakavargaControl_Resize);
        this.Load        += new System.EventHandler(this.AshtakavargaControl_Load);
        this.Paint       += new System.Windows.Forms.PaintEventHandler(this.AshtakavargaControl_Paint);
        this.DragDrop    += new System.Windows.Forms.DragEventHandler(this.AshtakavargaControl_DragDrop);
    }

#endregion

    private void AshtakavargaControl_Load(object sender, EventArgs e)
    {
        AddViewsToContextMenu(contextMenu);
    }


    private void DrawJhoraChakra(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.HighQuality;
        if (false == PrintMode)
        {
            g.Clear(BackColor);
        }

        var        offset = 5;
        var        size   = Math.Min(bmpBuffer.Width, bmpBuffer.Height) / 3 - 10;
        IDrawChart dc     = null;
        switch (userOptions.ChartStyle)
        {
            default:
            case EChartStyle.EastIndian:
                dc = new EastIndianChart();
                break;
            case EChartStyle.SouthIndian:
                dc = new SouthIndianChart();
                break;
        }

        Body.Body.Name[] bin_body =
        {
            Body.Body.Name.Lagna,
            Body.Body.Name.Lagna,
            Body.Body.Name.Sun,
            Body.Body.Name.Moon,
            Body.Body.Name.Mars,
            Body.Body.Name.Mercury,
            Body.Body.Name.Jupiter,
            Body.Body.Name.Venus,
            Body.Body.Name.Saturn
        };
        var bins = new int[9][];

        if (userOptions.SavType == ESavType.Normal)
        {
            bins[0] = av.getSav();
        }
        else
        {
            bins[0] = av.getSavRao();
        }

        bins[1] = av.getPav(Body.Body.Name.Lagna);
        bins[2] = av.getPav(Body.Body.Name.Sun);
        bins[3] = av.getPav(Body.Body.Name.Moon);
        bins[4] = av.getPav(Body.Body.Name.Mars);
        bins[5] = av.getPav(Body.Body.Name.Mercury);
        bins[6] = av.getPav(Body.Body.Name.Jupiter);
        bins[7] = av.getPav(Body.Body.Name.Venus);
        bins[8] = av.getPav(Body.Body.Name.Saturn);

        var strs = new string[9];
        strs[0] = "SAV";
        strs[1] = Body.Body.toString(Body.Body.Name.Lagna);
        strs[2] = Body.Body.toString(Body.Body.Name.Sun);
        strs[3] = Body.Body.toString(Body.Body.Name.Moon);
        strs[4] = Body.Body.toString(Body.Body.Name.Mars);
        strs[5] = Body.Body.toString(Body.Body.Name.Mercury);
        strs[6] = Body.Body.toString(Body.Body.Name.Jupiter);
        strs[7] = Body.Body.toString(Body.Body.Name.Venus);
        strs[8] = Body.Body.toString(Body.Body.Name.Saturn);

        Brush b_background = new SolidBrush(MhoraGlobalOptions.Instance.ChakraBackgroundColor);
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                g.ResetTransform();
                g.TranslateTransform(i * size + (i + 1) * offset, j * size + (j + 1) * offset);
                var scale = size / (float) dc.GetLength();
                g.ScaleTransform(scale, scale);
                if (false == PrintMode)
                {
                    g.FillRectangle(b_background, 0, 0, dc.GetLength(), dc.GetLength());
                }

                dc.DrawOutline(g);
                var off = j * 3 + i;
                var bin = bins[off];
                Debug.Assert(bin.Length == 12, "PAV/SAV: unknown size");
                for (var z = 0; z < 12; z++)
                {
                    var f  = fBig;
                    var zh = (int) h.getPosition(bin_body[off]).toDivisionPosition(userOptions.VargaType).zodiac_house.value;
                    if (z == zh - 1)
                    {
                        f = fBigBold;
                    }

                    var p = dc.GetSingleItemOffset(new ZodiacHouse((ZodiacHouse.Name) z + 1));
                    g.DrawString(bin[z].ToString(), f, b_black, p);
                }

                var sz = g.MeasureString(strs[off], fBig);
                g.DrawString(strs[off], fBig, b_red, 100 - sz.Width / 2, 100 - sz.Height / 2);

                if (off == 0 && userOptions.SavType == ESavType.Rao)
                {
                    sz = g.MeasureString("Rao", fBig);
                    g.DrawString("Rao", fBig, b_red, 100 - sz.Width / 2, 120 - sz.Height / 2);
                }
            }
        }
    }

    private Image DrawToBuffer()
    {
        if (bmpBuffer != null)
        {
            bmpBuffer.Dispose();
        }

        if (Width == 0 || Height == 0)
        {
            return bmpBuffer;
        }

        var displayGraphics = CreateGraphics();
        bmpBuffer = new Bitmap(Width, Height, displayGraphics);
        var imageGraphics = Graphics.FromImage(bmpBuffer);

        switch (mDisplayStyle)
        {
            case EDisplayStyle.Chancha:
                DrawChanchaChakra(imageGraphics);
                break;
            case EDisplayStyle.NavaSav:
                DrawJhoraChakra(imageGraphics);
                break;
        }

        displayGraphics.Dispose();
        return bmpBuffer;
    }

    public Bitmap DrawChanchaToImage(int size)
    {
        bmpBuffer = new Bitmap(size, size);
        var imageGraphics = Graphics.FromImage(bmpBuffer);
        DrawChanchaChakra(imageGraphics);
        return bmpBuffer;
    }

    public Bitmap DrawNavaChakrasToImage(int size)
    {
        bmpBuffer = new Bitmap(size, size);
        var imageGraphics = Graphics.FromImage(bmpBuffer);
        DrawJhoraChakra(imageGraphics);
        return bmpBuffer;
    }

    private void AshtakavargaControl_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.DrawImage(bmpBuffer, 0, 0);
    }

    private bool ChanchaReset(Graphics g, int ray)
    {
        var outerSize  = Math.Min(bmpBuffer.Width, bmpBuffer.Height);
        var scaleOuter = (float) outerSize / 200;

        g.ResetTransform();
        g.TranslateTransform(bmpBuffer.Width / 2, bmpBuffer.Height / 2);
        g.ScaleTransform(scaleOuter, scaleOuter);

        var   numEntries = 8             * 12;
        var   rotDegree  = (float) 360.0 / numEntries;
        float rotTotal   = 0;

        switch (userOptions.ChartStyle)
        {
            case EChartStyle.SouthIndian:
                rotTotal = rotDegree * ray - 120;
                g.RotateTransform(rotTotal);
                break;
            case EChartStyle.EastIndian:
                rotTotal = -1 * rotDegree * ray - 75 - rotDegree;
                g.RotateTransform(rotTotal);
                break;
        }

        while (rotTotal < 0)
        {
            rotTotal += 360;
        }

        while (rotTotal > 360)
        {
            rotTotal -= 360;
        }

        switch (userOptions.ChartStyle)
        {
            case EChartStyle.SouthIndian:
                if ((0 <= rotTotal && rotTotal < 90) || (270 <= rotTotal && rotTotal < 360))
                {
                    return true;
                }

                return false;
            case EChartStyle.EastIndian:
                if ((0 <= rotTotal && rotTotal < 105) || (285 <= rotTotal && rotTotal < 360))
                {
                    return true;
                }

                return false;
        }

        return true;
    }

    private void DrawChanchaInner(Graphics g)
    {
        IDrawChart dc = null;
        switch (userOptions.ChartStyle)
        {
            default:
            case EChartStyle.EastIndian:
                dc = new EastIndianChart();
                break;
            case EChartStyle.SouthIndian:
                dc = new SouthIndianChart();
                break;
        }

        var innerSize  = (int) ((float) Math.Min(bmpBuffer.Width, bmpBuffer.Height) / 3.15);
        var scaleInner = innerSize / (float) dc.GetLength();

        g.ResetTransform();
        g.TranslateTransform(bmpBuffer.Width / 2, bmpBuffer.Height / 2);
        g.TranslateTransform(-1 * innerSize  / 2, -1 * innerSize   / 2);
        g.ScaleTransform(scaleInner, scaleInner);

        dc.DrawOutline(g);

        int[] inner_bindus;

        if (outerBodies.Length > 1)
        {
            if (userOptions.SavType == ESavType.Rao)
            {
                inner_bindus = av.getSavRao();
            }
            else
            {
                inner_bindus = av.getSav();
            }
        }
        else
        {
            inner_bindus = av.getPav(outerBodies[0]);
        }

        for (var i = 0; i < 12; i++)
        {
            var zh = new ZodiacHouse((ZodiacHouse.Name) i + 1);
            var p  = dc.GetSingleItemOffset(zh);
            g.DrawString(inner_bindus[i].ToString(), fBig, b_black, p);
        }

        var av_desc = "SAV";
        if (outerBodies.Length == 1)
        {
            av_desc = "PAV";
        }

        var sz = g.MeasureString(av_desc, fBig);
        g.DrawString(av_desc, fBig, b_black, 100 - sz.Width / 2, 80 - sz.Height / 2);

        if (outerBodies.Length == 1)
        {
            var desc = Body.Body.toString(outerBodies[0]);
            sz = g.MeasureString(desc, fBig);
            g.DrawString(desc, fBig, b_black, 100 - sz.Width / 2, 120 - sz.Height / 2);
        }

        if (userOptions.SavType == ESavType.Rao)
        {
            var desc = "Rao";
            sz = g.MeasureString(desc, fBig);
            g.DrawString(desc, fBig, b_black, 100 - sz.Width / 2, 120 - sz.Height / 2);
        }

        {
            var desc = Basics.numPartsInDivisionString(userOptions.VargaType);
            ;
            sz = g.MeasureString(desc, fBig);
            g.DrawString(desc, fBig, b_black, 100 - sz.Width / 2, 100 - sz.Height / 2);
        }
    }

    private void DrawChanchaChakra(Graphics g)
    {
        string[] sBindus =
        {
            "Su",
            "Mo",
            "Ma",
            "Me",
            "Ju",
            "Ve",
            "Sa",
            "As"
        };
        var   pn_black = new Pen(Color.Black, (float) 0.01);
        var   pn_grey  = new Pen(Color.LightGray, (float) 0.01);
        var   pn_dgrey = new Pen(Color.Gray, (float) 0.01);
        Brush b_black  = new SolidBrush(Color.Black);
        Brush b_red    = new SolidBrush(Color.Red);
        var   f        = new Font(MhoraGlobalOptions.Instance.FixedWidthFont.FontFamily, MhoraGlobalOptions.Instance.FixedWidthFont.SizeInPoints - 6);


        g.SmoothingMode = SmoothingMode.HighQuality;

        if (PrintMode == false)
        {
            g.Clear(MhoraGlobalOptions.Instance.ChakraBackgroundColor);
        }

        DrawChanchaInner(g);


        // inner and outer bounding circles
        ChanchaReset(g, 0);
        for (var i = 1; i <= 8; i++)
        {
            var w = 45 + i * 4;
            g.DrawEllipse(pn_grey, -w, -w, w * 2, w * 2);
        }

        g.DrawEllipse(pn_black, -45, -45, 90, 90);
        g.DrawEllipse(pn_black, -85, -85, 85 * 2, 85 * 2);
        g.DrawEllipse(pn_black, -98, -98, 98 * 2, 98 * 2);


        // draw per-spoke stuff: spoke, bindus
        var numEntries = 8             * 12;
        var rotDegree  = (float) 360.0 / numEntries;
        for (var i = 0; i < numEntries; i++)
        {
            var bDir = ChanchaReset(g, i);
            var p    = pn_grey;
            if (i % 8 == 7 && userOptions.ChartStyle == EChartStyle.EastIndian)
            {
                p = pn_black;
            }

            if (i % 8 == 0 && userOptions.ChartStyle == EChartStyle.SouthIndian)
            {
                p = pn_black;
            }

            g.DrawLine(p, 45, 0, 98, 0);

            var b = b_black;
            //if (this.outerBodies.Length == 1 &&	av.BodyToInt(this.outerBodies[0]) == i%8)
            //	b = b_red;
            if (outerBodies.Length > 1)
            {
                if (bDir)
                {
                    g.DrawString(sBindus[i % 8], f, b, 49 + 9 * 4, 0);
                }
                else
                {
                    g.ScaleTransform((float) -1.0, (float) -1.0);
                    g.DrawString(sBindus[i % 8], f, b, -1 * (49 + 11 * 4), -6);
                }
            }
            //g.DrawString(i.ToString(), f, b, 49+9*4, 0);
        }

        // write the pav values at the top of the circle
        foreach (var bOuter in outerBodies)
        {
            var pav = av.getPav(bOuter);
            for (var i = 0; i < 12; i++)
            {
                var iRing = i * 8 + av.BodyToInt(bOuter);
                var bDir  = ChanchaReset(g, iRing);
                var sz    = g.MeasureString(pav[i].ToString(), f);
                if (bDir)
                {
                    g.DrawString(pav[i].ToString(), f, b_black, 49 + 7 * 4, 0);
                }
                else
                {
                    g.ScaleTransform((float) -1.0, (float) -1.0);
                    g.DrawString(pav[i].ToString(), f, b_black, new RectangleF(-1 * (49 + 7 * 4 + sz.Width), -1 * (sz.Height - 1), sz.Width, sz.Height));
                }
            }
        }

        // draw the bindus
        foreach (var bOuter in outerBodies)
        {
            foreach (var bInner in innerBodies)
            {
                var   iOuter = av.BodyToInt(bOuter);
                var   iInner = av.BodyToInt(bInner);
                var   zhBins = av.getBindus(bOuter, bInner);
                Brush br     = new SolidBrush(MhoraGlobalOptions.Instance.getBinduColor(bInner));

                foreach (var zh in zhBins)
                {
                    var iRing = ((int) zh - 1) * 8 + iInner;
                    var bDir  = ChanchaReset(g, iRing);
                    g.FillEllipse(br, 50       + (iOuter - 1) * 4, 1, 2, 2);
                    g.DrawEllipse(pn_dgrey, 50 + (iOuter - 1) * 4, 1, 2, 2);

                    if (outerBodies.Length == 1)
                    {
                        if (bDir)
                        {
                            g.DrawString(sBindus[iRing % 8], f, b_black, 49 + 9 * 4, 0);
                        }
                        else
                        {
                            g.ScaleTransform((float) -1.0, (float) -1.0);
                            g.DrawString(sBindus[iRing % 8], f, b_black, -1 * (49 + 11 * 4), -6);
                        }
                    }
                }
            }
        }
    }

    private void menuSav_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Sun,
            Body.Body.Name.Moon,
            Body.Body.Name.Mars,
            Body.Body.Name.Mercury,
            Body.Body.Name.Jupiter,
            Body.Body.Name.Venus,
            Body.Body.Name.Saturn,
            Body.Body.Name.Lagna
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuSav);
    }

    private void menuPavSun_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Sun
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavSun);
    }

    private void menuPavMoon_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Moon
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavMoon);
    }

    private void menuPavJupiter_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Jupiter
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavJupiter);
    }

    private void menuPavMars_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Mars
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavMars);
    }

    private void menuPavMercury_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Mercury
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavMercury);
    }

    private void menuPavVenus_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Venus
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavVenus);
    }

    private void menuPavSaturn_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Saturn
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavSaturn);
    }

    private void menuPavLagna_Click(object sender, EventArgs e)
    {
        outerBodies = new[]
        {
            Body.Body.Name.Lagna
        };
        mDisplayStyle = EDisplayStyle.Chancha;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuPavLagna);
    }

    private void AshtakavargaControl_Resize(object sender, EventArgs e)
    {
        DrawToBuffer();
        Invalidate();
    }

    protected override void copyToClipboard()
    {
        Clipboard.SetDataObject(bmpBuffer);
    }

    private object SetOptions(object o)
    {
        var ao = (AshtakavargaOptions) o;
        if (ao.VargaType != userOptions.VargaType)
        {
            av = new Ashtakavarga(h, ao.VargaType);
        }

        userOptions.SetOptions(ao);
        DrawToBuffer();
        Invalidate();
        return userOptions.Clone();
    }

    private void menuOptions_Click(object sender, EventArgs e)
    {
        var f = new MhoraOptions(userOptions.Clone(), SetOptions);
        f.ShowDialog();
    }

    private void menuJhoraSav_Click(object sender, EventArgs e)
    {
        mDisplayStyle = EDisplayStyle.NavaSav;
        DrawToBuffer();
        Invalidate();
        resetContextMenuChecks(menuJhoraSav);
    }

    private void resetContextMenuChecks(MenuItem mi)
    {
        menuJhoraSav.Checked   = false;
        menuSav.Checked        = false;
        menuPavLagna.Checked   = false;
        menuPavSun.Checked     = false;
        menuPavMoon.Checked    = false;
        menuPavMars.Checked    = false;
        menuPavMercury.Checked = false;
        menuPavJupiter.Checked = false;
        menuPavVenus.Checked   = false;
        menuPavSaturn.Checked  = false;
        mi.Checked             = true;
    }

    private void AshtakavargaControl_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(DivisionalChart)))
        {
            e.Effect = DragDropEffects.Copy;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private void AshtakavargaControl_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(DivisionalChart)))
        {
            var div = Division.CopyFromClipboard();
            if (null == div)
            {
                return;
            }

            userOptions.VargaType = div;
            SetOptions(userOptions);
            OnRecalculate(h);
        }
    }

    public class AshtakavargaOptions : ICloneable
    {
        public AshtakavargaOptions()
        {
            VargaType  = new Division(Basics.DivisionType.Rasi);
            ChartStyle = (EChartStyle) MhoraGlobalOptions.Instance.VargaStyle;
        }

        [PGNotVisible]
        public Division VargaType
        {
            get;
            set;
        }

        [PGDisplayName("Varga Type")]
        public Basics.DivisionType UIVargaType
        {
            get =>
                VargaType.MultipleDivisions[0].Varga;
            set =>
                VargaType = new Division(value);
        }

        [PGDisplayName("SAV Type")]
        public ESavType SavType
        {
            get;
            set;
        }

        public EChartStyle ChartStyle
        {
            get;
            set;
        }

        public object Clone()
        {
            var ao = new AshtakavargaOptions();
            ao.VargaType  = VargaType;
            ao.ChartStyle = ChartStyle;
            ao.SavType    = SavType;
            return ao;
        }

        public void SetOptions(AshtakavargaOptions ao)
        {
            VargaType  = ao.VargaType;
            ChartStyle = ao.ChartStyle;
            SavType    = ao.SavType;
        }
    }
}