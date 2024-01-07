using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Genghis.Windows.Forms;
using Mhora.Components;
using Mhora.Components.Jhora;
using Mhora.Components.SplashScreen;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Elements.Hora;
using Mhora.SwissEph;
using Mhora.Tables;

namespace Mhora
{
	public partial class MainForm : Form
	{
		private bool _showForm;
		public MainForm()
		{
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			gOpts = MhoraGlobalOptions.ReadFromFile();
			MhoraGlobalOptions.mainControl = this;
			if (MhoraGlobalOptions.Instance.ShowSplashScreen)
			{
				var ss = new SplashScreen(typeof(MhoraSplash), SplashScreenStyles.TopMost);
				Thread.Sleep(0);
				ss.Close(null, 1000);
			}

			using (var birthDetails = new BirthDetailsDialog())
			{
				if (birthDetails.ShowDialog() == DialogResult.OK)
				{
					AddChild(birthDetails.Horoscope, "test");
				}
			}
		}

		private void menuItemNewView_Click(object sender, EventArgs e)
		{
			var curr = (MhoraChild)ActiveMdiChild;
			if (null == curr)
			{
				return;
			}

			var child2 = new MhoraChild(curr.getHoroscope());
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

		private void openNewJhdFile()
		{
			var tNow = DateTime.Now;
			var mNow = new Moment(tNow.Year, tNow.Month, tNow.Day, tNow.Hour, tNow.Minute, tNow.Second);
			var info = new HoraInfo(mNow, MhoraGlobalOptions.Instance.Latitude, MhoraGlobalOptions.Instance.Longitude, MhoraGlobalOptions.Instance.TimeZone);

			childCount++;
			var h = new Horoscope(info, (HoroscopeOptions)MhoraGlobalOptions.Instance.HOptions.Clone());
			//new HoroscopeOptions());
			var child = new MhoraChild(h);
			child.Text = childCount + " - Prasna Chart";
			child.MdiParent = this;
			child.Name = child.Text;
			//info.name = child.Text;
			try
			{
				child.Show();
			}
			catch (OutOfMemoryException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void openJhdFileNow()
		{
			var tNow = DateTime.Now;
			var mNow = new Moment(tNow.Year, tNow.Month, tNow.Day, tNow.Hour, tNow.Minute, tNow.Second);

			var ofd = new OpenFileDialog();
			ofd.Filter = "JHD Files (*.jhd)|*.jhd";

			if (ofd.ShowDialog() != DialogResult.OK)
			{
				return;
			}


			var info = new Jhd(ofd.FileName).toHoraInfo();
			info.tob = mNow;

			var _path_split = ofd.FileName.Split('/', '\\');
			var path_split = new ArrayList(_path_split);

			childCount++;
			var h = new Horoscope(info, (HoroscopeOptions)MhoraGlobalOptions.Instance.HOptions.Clone());

			//Horoscope h = new Horoscope (info, new HoroscopeOptions());
			var child = new MhoraChild(h);
			child.Text = childCount + " - Prasna Chart";
			child.MdiParent = this;
			child.Name = child.Text;
			child.Show();
		}


		private void openJhdFile()
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
				info = new Jhd(ofd.FileName).toHoraInfo();
			}
			else
			{
				info = new Mhd(ofd.FileName).toHoraInfo();
			}

			var _path_split = ofd.FileName.Split('/', '\\');
			var path_split = new ArrayList(_path_split);

			childCount++;
			var h = new Horoscope(info, new HoroscopeOptions());
			var child = new MhoraChild(h);
			child.Text = childCount + " - " + path_split[path_split.Count - 1];
			child.MdiParent = this;
			child.Name = child.Text;
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

		private void menuItemFileOpen_Click(object sender, EventArgs e)
		{
			openJhdFile();
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

		private void MdiToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == toolbarButtonNew)
			{
				openNewJhdFile();
			}
			else if (e.Button == toolbarButtonOpen)
			{
				openJhdFile();
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
				if (mc != null)
				{
					mc.menuPrint();
				}
			}
			else if (e.Button == toolbarButtonPreview)
			{
				var mc = ActiveMdiChild as MhoraChild;
				if (mc != null)
				{
					mc.menuPrintPreview();
				}
			}
			else if (e.Button == toolbarButtonDob)
			{
				var mc = ActiveMdiChild as MhoraChild;
				if (mc != null)
				{
					mc.menuShowDobOptions();
				}
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

		private void menuItemFileNewPrasna_Click(object sender, EventArgs e)
		{
			openJhdFileNow();
		}


		private void mSavePreferences_Click(object sender, EventArgs e)
		{
			MhoraGlobalOptions.Instance.SaveToFile();
		}

		private object updateDisplayPreferences(object o)
		{
			MhoraGlobalOptions.NotifyDisplayChange();
			sweph.SetPath(MhoraGlobalOptions.Instance.HOptions.EphemerisPath);
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


		private bool checkJhd(string fileName)
		{
			var info = new Jhd(fileName).toHoraInfo();
			var h = new Horoscope(info, new HoroscopeOptions());
			if (h.getPosition(Body.BodyType.Ketu).toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house.Sign == h.getPosition(Body.BodyType.Lagna).toDivisionPosition(new Division(Vargas.DivisionType.Rasi)).zodiac_house.Sign)
			{
				return true;
			}

			return false;
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
			sweph.SetPath(MhoraGlobalOptions.Instance.HOptions.EphemerisPath);
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
