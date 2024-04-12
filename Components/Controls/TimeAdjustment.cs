using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Mhora.Elements;

namespace Mhora.Components.Controls
{
    public partial class TimeAdjustment: UserControl
    {
#region move window
	    public const int WM_NCLBUTTONDOWN = 0xA1;
	    public const int HT_CAPTION       = 0x2;

	    private bool _add = true;

	    [DllImport("user32.dll")]
	    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
	    [DllImport("user32.dll")]
	    public static extern bool ReleaseCapture();

	    protected void OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
	    {
		    if (mouseEventArgs.Button == MouseButtons.Left)
		    {
			    ReleaseCapture();
			    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
		    }
	    }
#endregion

	    public static class Event
	    {
		    public delegate void OnChange(DateTime dateTime);
	    }

	    public event Event.OnChange OnChange;

	    public enum Unit
	    {
			Seconds,
			Minutes,
			Hours,
			Days,
			Months,
			Years
	    }

        public TimeAdjustment()
        {
	        MouseDown += OnMouseDown;
            InitializeComponent();
			comboBoxUnit.SelectedIndex = 0;
			nudAmount.Value            = 1;
        }


        private Unit TimeUnit => (Unit)comboBoxUnit.SelectedIndex;
		private int  TimeValue
		{
			get
			{
				if (_add)
				{
					return (int) nudAmount.Value;
				}
				return (int) -nudAmount.Value;
			}
		}

		public Horoscope Horoscope
		{
			get;
			set;
		}

        public DateTime DateTime
        {
	        get
	        {
		        return TimeUnit switch
		               {
			               Unit.Seconds => Horoscope.Info.DateOfBirth.AddSeconds(TimeValue),
			               Unit.Minutes => Horoscope.Info.DateOfBirth.AddMinutes(TimeValue),
			               Unit.Hours   => Horoscope.Info.DateOfBirth.AddHours(TimeValue),
			               Unit.Days    => Horoscope.Info.DateOfBirth.AddDays(TimeValue),
			               Unit.Months  => Horoscope.Info.DateOfBirth.AddMonths(TimeValue),
			               Unit.Years   => Horoscope.Info.DateOfBirth.AddYears(TimeValue),
			               _            => (Horoscope.Info.DateOfBirth)
		               };
	        }
        }

		private void BtnDownClick(object sender, EventArgs e)
		{
			_add = false;
			OnChange?.Invoke(DateTime);
		}

		private void BtnUpClick(object sender, EventArgs e)
		{
			_add = true;
			OnChange?.Invoke(DateTime);
		}
	}
}
