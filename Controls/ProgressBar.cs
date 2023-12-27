using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace mhora.Controls
{
    public partial class ProgressBar : UserControl
    {
        private int _progress = 1000;
        private int _progressValue = 300;
 
        public ProgressBar()
        {
            InitializeComponent();
            ProgressFont = new Font(Font.FontFamily, (int)(this.Height * 0.7), FontStyle.Bold);
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

            var percentage = (_progressValue / 10f);
            var _unit = ClientSize.Width / 100.0;
            using var path = new GraphicsPath();
            Rectangle innerBounds = new Rectangle(0, 0, (int)(percentage * _unit), ClientSize.Height);

            //progressbar region filling
            Region r = new Region(GetRoundRectangle(innerBounds));

            g.FillRegion(new SolidBrush(ProgressBarColor), r);

            var progressText = percentage.ToString("0.0", CultureInfo.InvariantCulture);
            //draw string
            g.DrawString(progressText + "%", new Font(ProgressFont.FontFamily, (int)(ClientSize.Height * 0.5), ProgressFont.Style), new SolidBrush(ProgressFontColor), new PointF(ClientSize.Width / 2f - ClientSize.Height, ClientSize.Height / 20f));
        }

        private int _value = 0;
        public int Value 
        {
            get
            {
                return (_value);
            }
            set
            {
                _value = value;
                _progressValue = 0;
                _progress = 0;
            }
        }

        private Color _progressBarColor = Color.FromArgb(224, 224, 224);
        [Category("Appearance")]
        public Color ProgressBarColor
        {
            get
            {
                return (_progressBarColor);
            }
            set
            {
                _progressBarColor = value;
                Refresh();
            }
        }

        private Color _progressBackColor = Color.FromArgb(255, 128, 255);
        [Category("Appearance")]
        public Color ProgressBackColor
        {
            get
            {
                return (_progressBackColor);
            }
            set
            {
                _progressBackColor = value;
                Refresh();
            }
        }

        private Font _progressFont;
        [Category("Appearance")]
        public Font ProgressFont
        {
            get
            {
                return (_progressFont);
            }
            set
            {
                _progressFont = value;
                Refresh();
            }
        }

        private Color _progressFontColor = Color.Black;
        [Category("Appearance")]
        public Color ProgressFontColor
        {
            get
            {
                return (_progressFontColor);
            }
            set
            {
                _progressFontColor = value;
                Refresh();
            }
        }


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
            var value = (int) ((_progress / (float) Value) * 1000);
            if (_progressValue != value)
            {
                _progressValue = value;
                Invoke(Refresh);
            }
        }
    }
}
