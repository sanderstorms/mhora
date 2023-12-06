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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Mhora.Calculation;
using Mhora.Settings;

namespace Mhora.Components
{
    public class VaraChakra : MhoraControl
    {
        private readonly Brush       b_black;
        private          Bitmap      bmpBuffer;
        private readonly IContainer  components = null;
        private          ContextMenu contextMenu;
        private          Font        f;
        private          Pen         pn_black;
        private readonly Pen         pn_grey;

        public VaraChakra(Horoscope _h)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            h                                      =  _h;
            h.Changed                              += OnRecalculate;
            MhoraGlobalOptions.DisplayPrefsChanged += OnResize;
            pn_black                               =  new Pen(Color.Black, (float)0.1);
            pn_grey                                =  new Pen(Color.Gray, (float)0.1);
            b_black                                =  new SolidBrush(Color.Black);
            AddViewsToContextMenu(contextMenu);
            OnResize(MhoraGlobalOptions.Instance);
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
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
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            // 
            // VaraChakra
            // 
            this.ContextMenu =  this.contextMenu;
            this.Name        =  "VaraChakra";
            this.Size        =  new System.Drawing.Size(456, 240);
            this.Resize      += new System.EventHandler(this.VaraChakra_Resize);
            this.Load        += new System.EventHandler(this.VaraChakra_Load);
            this.Paint       += new System.Windows.Forms.PaintEventHandler(this.VaraChakra_Paint);
        }

#endregion

        private void VaraChakra_Load(object sender, EventArgs e)
        {
        }

        public void OnResize(object o)
        {
            f = new Font(MhoraGlobalOptions.Instance.GeneralFont.FontFamily,
                         MhoraGlobalOptions.Instance.GeneralFont.SizeInPoints - 4);
            DrawToBuffer(true);
            Invalidate();
        }

        public void OnRecalculate(object o)
        {
            Invalidate();
        }

        private void ResetChakra(Graphics g, double rot)
        {
            var size  = Math.Min(bmpBuffer.Width, bmpBuffer.Height);
            var scale = (float)size / 310;
            g.ResetTransform();
            g.TranslateTransform(bmpBuffer.Width / 2, bmpBuffer.Height / 2);
            g.ScaleTransform(scale, scale);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.RotateTransform((float)(270.0 + 360.0 / (9.0 * 2.0) - rot));
        }

        private void DrawChakra(Graphics g)
        {
            Body.Body.Name[] bodies =
            {
                Body.Body.Name.Sun,
                Body.Body.Name.Moon,
                Body.Body.Name.Mars,
                Body.Body.Name.Mercury,
                Body.Body.Name.Jupiter,
                Body.Body.Name.Venus,
                Body.Body.Name.Saturn,
                Body.Body.Name.Rahu,
                Body.Body.Name.Ketu
            };

            g.Clear(MhoraGlobalOptions.Instance.ChakraBackgroundColor);

            ResetChakra(g, 0.0);
            g.DrawEllipse(pn_grey, -150, -150, 300, 300);
            g.DrawEllipse(pn_grey, -140, -140, 280, 280);

            for (var i = 0; i < 9; i++)
            {
                ResetChakra(g, i * (360.0 / 9.0));
                g.DrawLine(pn_grey, 0, 0, 150, 0);
            }

            for (var i = 0; i < 9; i++)
            {
                ResetChakra(g, i * (360.0 / 9.0) + 360.0 / (9.0 * 2.0));
                g.TranslateTransform(135, 0);
                g.RotateTransform((float)90.0);
                var sz = g.MeasureString(Body.Body.toString(bodies[i]), f);
                g.DrawString(Body.Body.toString(bodies[i]), f, b_black, -sz.Width / 2, 0);
            }

            if (h.isDayBirth())
            {
            }
        }

        private Image DrawToBuffer(bool bRecalc)
        {
            if (bmpBuffer != null && bmpBuffer.Size != Size)
            {
                bmpBuffer.Dispose();
                bmpBuffer = null;
            }

            if (Width == 0 || Height == 0)
            {
                return bmpBuffer;
            }

            if (bRecalc == false && Width == bmpBuffer.Width && Height == bmpBuffer.Height)
            {
                return bmpBuffer;
            }

            var displayGraphics = CreateGraphics();
            bmpBuffer = new Bitmap(Width, Height, displayGraphics);
            var imageGraphics = Graphics.FromImage(bmpBuffer);
            DrawChakra(imageGraphics);
            displayGraphics.Dispose();
            return bmpBuffer;
        }

        private void VaraChakra_Resize(object sender, EventArgs e)
        {
            DrawToBuffer(true);
            Invalidate();
        }

        private void VaraChakra_Paint(object sender, PaintEventArgs e)
        {
            DrawChakra(e.Graphics);
        }

        protected override void copyToClipboard()
        {
            Clipboard.SetDataObject(bmpBuffer);
        }
    }
}