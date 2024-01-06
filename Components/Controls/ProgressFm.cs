using System.Windows.Forms;

namespace Mhora.Components.Controls;

public partial class ProgressFm : Form
{
	public ProgressFm()
	{
		InitializeComponent();
	}

	public int Count
	{
		get => ProgressBar.Value;
		set => ProgressBar.Value = value;
	}

	public void Step()
	{
		ProgressBar.Step();
	}
}