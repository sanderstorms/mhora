using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace mhora.Controls
{
    public partial class ProgressBar : UserControl
    {
        private int _progress = 0;

        public ProgressBar()
        {
            InitializeComponent();
            ProgressBarColor = Color.FromArgb(224, 224, 224);
            ProgressBackColor = Color.FromArgb(255, 128, 255);
            ProgressFont = new Font(Font.FontFamily, (int)(this.Height * 0.7), FontStyle.Bold);
            ProgressFontColor = Color.Black;
            Value = 0;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            var g = pe.Graphics;
            //graphics
            g = pe.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;

            //clear graphics
            g.Clear(ProgressBackColor);

            var _unit = ClientSize.Width / 100.0;
            using var path = new GraphicsPath();
            Rectangle innerBounds = new Rectangle(0, 0, (int)(_progress * _unit), ClientSize.Height);

            //progressbar region filling
            Region r = new Region(GetRoundRectangle(innerBounds));

            g.FillRegion(new SolidBrush(ProgressBarColor), r);

            //draw string
            g.DrawString(_progress + "%", new Font(ProgressFont.FontFamily, (int)(ClientSize.Height * 0.5), ProgressFont.Style), new SolidBrush(ProgressFontColor), new PointF(ClientSize.Width / 2f - ClientSize.Height, ClientSize.Height / 20f));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

        }

        public int Value { get; set; }
        [Category("Appearance")]
        public Color ProgressBarColor { get; set; }
        [Category("Appearance")]
        public Color ProgressBackColor { get; set; }
        [Category("Appearance")]
        public Font ProgressFont { get; set; }
        [Category("Appearance")]
        public Color ProgressFontColor { get; set; }


        private GraphicsPath GetRoundRectangle(Rectangle bounds)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = bounds.Height;
            if (bounds.Height <= 0) radius = 20;
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius,
                radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }
        private void RecreateRegion()
        {
            var bounds = new Rectangle(this.ClientRectangle.Location, this.ClientRectangle.Size);
            bounds.Inflate(-1, -1);
            this.Region = new Region(GetRoundRectangle(bounds));
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.RecreateRegion();
        }
        public void Step()
        {
            _progress++;
            Refresh();
        }
    }
}
