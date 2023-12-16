using System.Windows.Forms;

namespace mhora.Controls
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
        }
    }
}

 
