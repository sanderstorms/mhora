using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;

namespace Mhora.Controls;

public partial class ProgressBar : UserControl
{
    private int _progress = 1000;

    private Color _progressBackColor = Color.FromArgb(255, 128, 255);

    private Color _progressBarColor = Color.FromArgb(224, 224, 224);

    private Font _progressFont;

    private Color _progressFontColor = Color.Black;
    private int   _progressValue     = 300;

    private int _value;

    public ProgressBar()
    {
        InitializeComponent();
        ProgressFont = new Font(Font.FontFamily, (int) (Height * 0.7), FontStyle.Bold);
    }

    public int Value
    {
        get => _value;
        set
        {
            _value         = value;
            _progressValue = 0;
            _progress      = 0;
        }
    }

    [Category("Appearance")]
    public Color ProgressBarColor
    {
        get => _progressBarColor;
        set
        {
            _progressBarColor = value;
            Refresh();
        }
    }

    [Category("Appearance")]
    public Color ProgressBackColor
    {
        get => _progressBackColor;
        set
        {
            _progressBackColor = value;
            Refresh();
        }
    }

    [Category("Appearance")]
    public Font ProgressFont
    {
        get => _progressFont;
        set
        {
            _progressFont = value;
            Refresh();
        }
    }

    [Category("Appearance")]
    public Color ProgressFontColor
    {
        get => _progressFontColor;
        set
        {
            _progressFontColor = value;
            Refresh();
        }
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
        base.OnPaint(pe);
        var g = pe.Graphics;
        //graphics
        g                   = pe.Graphics;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;
        g.SmoothingMode     = SmoothingMode.HighQuality;

        //clear graphics
        g.Clear(ProgressBackColor);

        var       percentage  = _progressValue   / 10f;
        var       _unit       = ClientSize.Width / 100.0;
        using var path        = new GraphicsPath();
        var       innerBounds = new Rectangle(0, 0, (int) (percentage * _unit), ClientSize.Height);

        //progressbar region filling
        var r = new Region(GetRoundRectangle(innerBounds));

        g.FillRegion(new SolidBrush(ProgressBarColor), r);

        var progressText = percentage.ToString("0.0", CultureInfo.InvariantCulture);
        //draw string
        g.DrawString(progressText + "%", new Font(ProgressFont.FontFamily, (int) (ClientSize.Height * 0.5), ProgressFont.Style), new SolidBrush(ProgressFontColor), new PointF(ClientSize.Width / 2f - ClientSize.Height, ClientSize.Height / 20f));
    }


    private GraphicsPath GetRoundRectangle(Rectangle bounds)
    {
        var path   = new GraphicsPath();
        var radius = bounds.Height;
        if (bounds.Height <= 0)
        {
            radius = 20;
        }

        path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
        path.AddArc(bounds.X           + bounds.Width  - radius, bounds.Y, radius, radius, 270, 90);
        path.AddArc(bounds.X           + bounds.Width  - radius, bounds.Y + bounds.Height - radius, radius, radius, 0, 90);
        path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
        path.CloseAllFigures();
        return path;
    }

    private void RecreateRegion()
    {
        var bounds = new Rectangle(ClientRectangle.Location, ClientRectangle.Size);
        bounds.Inflate(-1, -1);
        Region = new Region(GetRoundRectangle(bounds));
        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        RecreateRegion();
    }

    public void Step()
    {
        _progress++;
        var value = (int) (_progress / (float) Value * 1000);
        if (_progressValue != value)
        {
            _progressValue = value;
            Invoke(Refresh);
        }
    }
}