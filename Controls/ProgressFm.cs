using System.Windows.Forms;

namespace Mhora.Controls
{
    public partial class ProgressFm : Form
    {
        public ProgressFm()
        {
            InitializeComponent();
        }
        public int Count
        {
            get
            {
                return (ProgressBar.Value);
            }
            set
            {
                ProgressBar.Value = value;
            }
        }

        public void Step()
        {
            ProgressBar.Step();
        }
    }
}

 
