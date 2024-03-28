using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Genghis.Windows.Forms;
using Mhora.Components;
using Mhora.Components.File;
using Mhora.Components.SplashScreen;
using Mhora.Database.Settings;
using Mhora.Definitions;
using Mhora.Elements;
using Mhora.SwissEph;

namespace Mhora
{
	public partial class MainForm : Form
	{
		private bool _showForm;
		public MainForm()
		{
			InitializeComponent();
			Text = Application.ActiveAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title + " " + Application.VersionString;
			//Text       = Application.VersionString;
			childCount = 0;

			CreateHandle();
			Task.Run(() =>
			{
				Invoke(async () =>
				{
					await Application.InitDb();
					_showForm = true;
					Visible   = true;
				});
			});
		}

		protected sealed override void CreateHandle()
		{
			base.CreateHandle();
		}

		/// <inheritdoc />
		/// <summary>
		///     Make controls visible or not
		/// </summary>
		/// <param name="showControl">visible ?</param>
		protected override void SetVisibleCore(bool showControl)
		{
			if (Application.Running)
			{
				var visible = showControl & _showForm;
				base.SetVisibleCore(visible);
				if (visible)
				{
					BringToFront();
				}
			}
			else
			{
				base.SetVisibleCore(showControl);
			}
		}


		protected override async void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			gOpts = MhoraGlobalOptions.ReadFromFile();
			MhoraGlobalOptions.MainControl = this;
			if (MhoraGlobalOptions.Instance.ShowSplashScreen)
			{
				var ss = new SplashScreen(typeof(MhoraSplash), SplashScreenStyles.TopMost);
				Thread.Sleep(0);
				ss.Close(null, 1000);
			}
			await openNewJhdFile();
		}

		private void menuItemNewView_Click(object sender, EventArgs e)
		{
			var curr = (MhoraChild)ActiveMdiChild;
			if (null == curr)
			{
				return;
			}

			var child2 = new MhoraChild(curr.Horoscope);
			child2.Text = curr.Text;
			child2.MdiParent = this;
			child2.Name = curr.Name;
			child2.Show();
		}


		private void menuItemWindowTile_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void menuItemWindowCascade_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private async Task openNewJhdFile()
		{
			using (var birthDetails = new BirthDetailsDialog())
			{
				if (birthDetails.ShowDialog() == DialogResult.OK)
				{
					var horoscope = new Horoscope(birthDetails.Info, new HoroscopeOptions());
					AddChild(horoscope, birthDetails.ChartName);
				}
			}
		}

		private async Task openJhdFileNow()
		{
			var mNow = DateTime.Now;

			var ofd = new OpenFileDialog();
			ofd.Filter = "JHD Files (*.jhd)|*.jhd";

			if (ofd.ShowDialog() != DialogResult.OK)
			{
				return;
			}


			var info = new Jhd(ofd.FileName).ToHoraInfo();
			info.DateOfBirth = mNow;

			var _path_split = ofd.FileName.Split('/', '\\');

			childCount++;
			var h = new Horoscope(info, (HoroscopeOptions)MhoraGlobalOptions.Instance.HOptions.Clone());

			//Horoscope h = new Horoscope (info, new HoroscopeOptions());
			var child = new MhoraChild(h);
			child.Text = childCount + " - Prasna Chart";
			child.MdiParent = this;
			child.Name = child.Text;
			child.Show();
		}


		private async Task openJhdFile()
		{
			var ofd = new OpenFileDialog();
			ofd.Filter = "Hora Files (*.jhd; *.mhd)|*.jhd;*.mhd";

			if (ofd.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			var sparts = ofd.FileName.ToLower().Split('.');
			HoraInfo info = null;

			if (sparts[sparts.Length - 1] == "jhd")
			{
				info = new Jhd(ofd.FileName).ToHoraInfo();
			}
			else
			{
				info = new Mhd(ofd.FileName).ToHoraInfo();
			}

			var _path_split = ofd.FileName.Split('/', '\\');

			childCount++;
			var h = new Horoscope(info, new HoroscopeOptions());
			var child = new MhoraChild(h);
			child.Text         = childCount + " - " + _path_split.Last();
			child.MdiParent    = this;
			child.Name         = child.Text;
			child.mJhdFileName = ofd.FileName;

			child.Show();
		}

		public void AddChild(Horoscope h, string name)
		{
			childCount++;
			var child = new MhoraChild(h);
			h.OnChanged();
			child.Text = childCount + " - " + name;
			child.MdiParent = this;
			child.Name = child.Text;
			child.Show();
		}

		private async void menuItemFileOpen_Click(object sender, EventArgs e)
		{
			await openJhdFile();
		}

		private void menuItemHelpAboutMhora_Click(object sender, EventArgs e)
		{
			Form dlg = new AboutMhora();
			dlg.ShowDialog();
		}

		private void menuItemHelpAboutSjc_Click(object sender, EventArgs e)
		{
			Form dlg = new AboutSjc();
			dlg.ShowDialog();
		}

		private async void MdiToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolbarButtonNew)
			{
				await openNewJhdFile();
			}
			else if (e.Button == toolbarButtonOpen)
			{
				await openJhdFile();
			}
			else if (e.Button == toolbarButtonSave)
			{
				var curr = (MhoraChild)ActiveMdiChild;
				if (null == curr)
				{
					return;
				}

				curr.saveJhdFile();
			}
			else if (e.Button == toolbarButtonHelp)
			{
				Form dlg = new AboutMhora();
				dlg.ShowDialog();
			}
			else if (e.Button == toolbarButtonPrint)
			{
				var mc = ActiveMdiChild as MhoraChild;
				mc?.menuPrint();
			}
			else if (e.Button == toolbarButtonPreview)
			{
				var mc = ActiveMdiChild as MhoraChild;
				mc?.menuPrintPreview();
			}
			else if (e.Button == toolbarButtonDob)
			{
				var mc = ActiveMdiChild as MhoraChild;
				mc?.menuShowDobOptions();
			}
			else if (e.Button == toolbarButtonDisp)
			{
				showMenuGlobalDisplayPrefs();
			}
		}

		private void OnClosing()
		{
			if (MhoraGlobalOptions.Instance.SavePrefsOnExit)
			{
				MhoraGlobalOptions.Instance.SaveToFile();
			}
		}


		private void menuItemFileExit_Click(object sender, EventArgs e)
		{
			OnClosing();
			Close();
		}


		private void menuItemFileNew_Click(object sender, EventArgs e)
		{
			openNewJhdFile();
		}

		private void menuItem1_Click(object sender, EventArgs e)
		{
		}

		private async void menuItemFileNewPrasna_Click(object sender, EventArgs e)
		{
			await openJhdFileNow();
		}


		private void mSavePreferences_Click(object sender, EventArgs e)
		{
			MhoraGlobalOptions.Instance.SaveToFile();
		}

		private object updateDisplayPreferences(object o)
		{
			MhoraGlobalOptions.NotifyDisplayChange();
			sweph.SetEphePath(MhoraGlobalOptions.Instance.HOptions.EphemerisPath);
			return o;
		}

		private void showMenuGlobalDisplayPrefs()
		{
			//object wrapper = new GlobalizedPropertiesWrapper(MhoraGlobalOptions.Instance);
			var f = new MhoraOptions(MhoraGlobalOptions.Instance, updateDisplayPreferences, true);
			f.ShowDialog();
		}

		private void menuItem4_Click(object sender, EventArgs e)
		{
			showMenuGlobalDisplayPrefs();
		}


		private void mResetPreferences_Click(object sender, EventArgs e)
		{
			var mh = new MhoraGlobalOptions();
			MhoraGlobalOptions.Instance = mh;
			MhoraGlobalOptions.NotifyDisplayChange();
			MhoraGlobalOptions.NotifyCalculationChange();
		}

		private void mResetStrengthPreferences_Click(object sender, EventArgs e)
		{
			MhoraGlobalOptions.Instance.SOptions = new StrengthOptions();
			MhoraGlobalOptions.NotifyCalculationChange();
		}

		private void menuItemSplash_Click(object sender, EventArgs e)
		{
			//MhoraSplash f = new MhoraSplash();
			//f.Show();
			var ss = new SplashScreen(typeof(MhoraSplash), SplashScreenStyles.None);
			Thread.Sleep(0);
			ss.Close(null, 10000);
		}

		private void mIncreaseFontSize_Click(object sender, EventArgs e)
		{
			MhoraGlobalOptions.Instance.IncreaseFontSize();
			MhoraGlobalOptions.NotifyDisplayChange();
		}

		private void mDecreaseFontSize_Click(object sender, EventArgs e)
		{
			MhoraGlobalOptions.Instance.DecreaseFontSize();
			MhoraGlobalOptions.NotifyDisplayChange();
		}

		public object updateCalcPreferences(object o)
		{
			sweph.SetEphePath(MhoraGlobalOptions.Instance.HOptions.EphemerisPath);
			sweph.SetSidMode((int) MhoraGlobalOptions.Instance.HOptions.Ayanamsa, 0.0, 0.0);

			MhoraGlobalOptions.NotifyCalculationChange();
			return o;
		}

		private void mEditCalcPrefs_Click(object sender, EventArgs e)
		{
			//object wrapper = new GlobalizedPropertiesWrapper(MhoraGlobalOptions.Instance.HOptions);
			var f = new MhoraOptions(MhoraGlobalOptions.Instance.HOptions, updateCalcPreferences, true);
			f.ShowDialog();
		}

		private void mEditStrengthPrefs_Click(object sender, EventArgs e)
		{
			var f = new MhoraOptions(MhoraGlobalOptions.Instance.SOptions, updateCalcPreferences, true);
			f.ShowDialog();
		}

		private void MhoraContainer_Closing(object sender, CancelEventArgs e)
		{
			OnClosing();
		}
	}
}
