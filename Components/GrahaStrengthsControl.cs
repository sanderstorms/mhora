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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Mhora.Components.Delegates;
using Mhora.Components.Property;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Tables;
using mhora.Util;

namespace Mhora.Components;

public class GrahaStrengthsControl : Form
{
	private const    int         RCoLord                        = 0;
	private const    int         RNaisargikaDasa                = 1;
	private const    int         RVimsottariDasa                = 2;
	private const    int         RKarakaKendradiGrahaDasa       = 3;
	private const    int         RCoLordKarakaKendradiGrahaDasa = 4;
	private readonly IContainer  components                     = null;
	private readonly UserOptions options;
	private          ComboBox    cbGraha1;
	private          ComboBox    cbGraha2;
	private          ComboBox    cbStrength;
	private          ContextMenu cMenu;
	private          Horoscope   h;
	private          Label       lColords;
	private          Label       lVarga;
	private          MenuItem    menuOptions;
	private          ListView    mList;

	public GrahaStrengthsControl(Horoscope _h)
	{
		// This call is required by the Windows Form Designer.
		InitializeComponent();
		h         =  _h;
		h.Changed += OnRecalculate;
		options   =  new UserOptions();
		InitializeComboBoxes();
	}

	public void OnRecalculate(object _h)
	{
		h = (Horoscope) _h;
		Compute();
	}


	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
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
		this.cbStrength  = new System.Windows.Forms.ComboBox();
		this.cbGraha1    = new System.Windows.Forms.ComboBox();
		this.cbGraha2    = new System.Windows.Forms.ComboBox();
		this.mList       = new System.Windows.Forms.ListView();
		this.cMenu       = new System.Windows.Forms.ContextMenu();
		this.menuOptions = new System.Windows.Forms.MenuItem();
		this.lVarga      = new System.Windows.Forms.Label();
		this.lColords    = new System.Windows.Forms.Label();
		this.SuspendLayout();
		// 
		// cbStrength
		// 
		this.cbStrength.Anchor               =  ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.cbStrength.Location             =  new System.Drawing.Point(8, 8);
		this.cbStrength.Name                 =  "cbStrength";
		this.cbStrength.Size                 =  new System.Drawing.Size(120, 21);
		this.cbStrength.TabIndex             =  0;
		this.cbStrength.Text                 =  "cbStrength";
		this.cbStrength.SelectedIndexChanged += new System.EventHandler(this.cbStrength_SelectedIndexChanged);
		// 
		// cbGraha1
		// 
		this.cbGraha1.Location             =  new System.Drawing.Point(8, 40);
		this.cbGraha1.Name                 =  "cbGraha1";
		this.cbGraha1.Size                 =  new System.Drawing.Size(104, 21);
		this.cbGraha1.TabIndex             =  1;
		this.cbGraha1.Text                 =  "cbGraha1";
		this.cbGraha1.SelectedIndexChanged += new System.EventHandler(this.cbGraha1_SelectedIndexChanged);
		// 
		// cbGraha2
		// 
		this.cbGraha2.Anchor               =  ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		this.cbGraha2.Location             =  new System.Drawing.Point(152, 40);
		this.cbGraha2.Name                 =  "cbGraha2";
		this.cbGraha2.Size                 =  new System.Drawing.Size(112, 21);
		this.cbGraha2.TabIndex             =  2;
		this.cbGraha2.Text                 =  "cbGraha2";
		this.cbGraha2.SelectedIndexChanged += new System.EventHandler(this.cbGraha2_SelectedIndexChanged);
		// 
		// mList
		// 
		this.mList.Anchor               =  ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.mList.FullRowSelect        =  true;
		this.mList.Location             =  new System.Drawing.Point(16, 72);
		this.mList.Name                 =  "mList";
		this.mList.Size                 =  new System.Drawing.Size(240, 208);
		this.mList.TabIndex             =  3;
		this.mList.View                 =  System.Windows.Forms.View.Details;
		this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
		// 
		// cMenu
		// 
		this.cMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.menuOptions
		});
		// 
		// menuOptions
		// 
		this.menuOptions.Index =  0;
		this.menuOptions.Text  =  "Options";
		this.menuOptions.Click += new System.EventHandler(this.menuOptions_Click);
		// 
		// lVarga
		// 
		this.lVarga.Anchor    = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
		this.lVarga.Location  = new System.Drawing.Point(144, 8);
		this.lVarga.Name      = "lVarga";
		this.lVarga.Size      = new System.Drawing.Size(104, 23);
		this.lVarga.TabIndex  = 4;
		this.lVarga.Text      = "lVarga";
		this.lVarga.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		// 
		// lColords
		// 
		this.lColords.Anchor   =  ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.lColords.Location =  new System.Drawing.Point(16, 288);
		this.lColords.Name     =  "lColords";
		this.lColords.Size     =  new System.Drawing.Size(240, 16);
		this.lColords.TabIndex =  5;
		this.lColords.Text     =  "lColords";
		this.lColords.Click    += new System.EventHandler(this.lColords_Click);
		// 
		// GrahaStrengthsControl
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize        = new System.Drawing.Size(264, 310);
		this.ContextMenu       = this.cMenu;
		this.Controls.Add(this.lColords);
		this.Controls.Add(this.lVarga);
		this.Controls.Add(this.mList);
		this.Controls.Add(this.cbGraha2);
		this.Controls.Add(this.cbGraha1);
		this.Controls.Add(this.cbStrength);
		this.Name   =  "GrahaStrengthsControl";
		this.Text   =  "Graha Strengths Reckoner";
		this.Resize += new System.EventHandler(this.GrahaStrengthsControl_Resize);
		this.Load   += new System.EventHandler(this.GrahaStrengthsControl_Load);
		this.ResumeLayout(false);
	}

#endregion

	private void mList_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private ArrayList GetRules(ref bool bSimpleLord)
	{
		bSimpleLord = false;
		switch (cbStrength.SelectedIndex)
		{
			case 0:
				bSimpleLord = true;
				return FindStronger.RulesStrongerCoLord(h);
			case 1:                              return FindStronger.RulesNaisargikaDasaGraha(h);
			case RVimsottariDasa:                return FindStronger.RulesVimsottariGraha(h);
			case RKarakaKendradiGrahaDasa:       return FindStronger.RulesKarakaKendradiGrahaDasaGraha(h);
			case RCoLordKarakaKendradiGrahaDasa: return FindStronger.RulesKarakaKendradiGrahaDasaColord(h);
			default:                             return FindStronger.RulesStrongerCoLord(h);
		}
	}

	private void InitializeComboBoxes()
	{
		for (var i = (int) Body.BodyType.Sun; i <= (int) Body.BodyType.Lagna; i++)
		{
			var s = ((Body.BodyType) i).Name();
			cbGraha1.Items.Add(s);
			cbGraha2.Items.Add(s);
		}

		cbGraha1.SelectedIndex = (int) Body.BodyType.Mars;
		cbGraha2.SelectedIndex = (int) Body.BodyType.Ketu;

		cbStrength.Items.Add("Co-Lord");
		cbStrength.Items.Add("Naisargika Graha Dasa");
		cbStrength.Items.Add("Vimsottari Dasa");
		cbStrength.Items.Add("Karakas Kendradi Graha Dasa");
		cbStrength.Items.Add("Karakas Kendradi Graha Dasa Co-Lord");
		cbStrength.SelectedIndex = 0;

		lVarga.Text = options.Division.ToString();
		populateColordLabel();
	}

	private void Compute()
	{
		mList.BeginUpdate();
		mList.Clear();

		mList.BackColor = Color.AliceBlue;


		mList.Columns.Add("Body", -1, HorizontalAlignment.Left);
		mList.Columns.Add("Winner", -1, HorizontalAlignment.Left);

		var winner = 0;
		var b1     = (Body.BodyType) cbGraha1.SelectedIndex;
		var b2     = (Body.BodyType) cbGraha2.SelectedIndex;

		var bSimpleLord = false;
		var al          = GetRules(ref bSimpleLord);
		for (var i = 0; i < al.Count; i++)
		{
			var rule = new ArrayList();
			rule.Add(al[i]);
			var fs = new FindStronger(h, options.Division, rule);
			var bw = fs.StrongerGraha(b1, b2, bSimpleLord, ref winner);

			var li        = new ListViewItem();
			var enumValue = (Enum) al[i];
			li.Text = string.Format("{0}", enumValue.GetEnumDescription());

			if (winner == 0)
			{
				li.SubItems.Add(bw.Name());
			}

			mList.Items.Add(li);
		}

		mList.Columns[0].Width = -1;
		mList.Columns[1].Width = -2;

		mList.EndUpdate();
	}


	private void GrahaStrengthsControl_Load(object sender, EventArgs e)
	{
		if (false == MhoraGlobalOptions.Instance.GrahaStrengthsFormSize.IsEmpty)
		{
			Size = MhoraGlobalOptions.Instance.GrahaStrengthsFormSize;
		}
	}

	private void cbStrength_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cbStrength.SelectedIndex == RVimsottariDasa)
		{
			options.Division       = new Division(Vargas.DivisionType.BhavaPada);
			cbGraha1.SelectedIndex = (int) Body.BodyType.Lagna;
			cbGraha1.SelectedIndex = (int) Body.BodyType.Moon;
		}

		lVarga.Text = options.Division.ToString();
		Compute();
	}

	private void cbGraha1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cbStrength.SelectedIndex == RCoLord)
		{
			switch (cbGraha1.SelectedIndex)
			{
				case (int) Body.BodyType.Mars:
					cbGraha2.SelectedIndex = (int) Body.BodyType.Ketu;
					break;
				case (int) Body.BodyType.Ketu:
					cbGraha2.SelectedIndex = (int) Body.BodyType.Mars;
					break;
				case (int) Body.BodyType.Saturn:
					cbGraha2.SelectedIndex = (int) Body.BodyType.Rahu;
					break;
				case (int) Body.BodyType.Rahu:
					cbGraha2.SelectedIndex = (int) Body.BodyType.Saturn;
					break;
			}
		}

		Compute();
	}

	private void cbGraha2_SelectedIndexChanged(object sender, EventArgs e)
	{
		Compute();
	}

	public object SetOptions(object o)
	{
		var o2 = options.SetOptions(o);
		lVarga.Text = options.Division.ToString();
		populateColordLabel();
		return o2;
	}

	private void menuOptions_Click(object sender, EventArgs e)
	{
		new MhoraOptions(options, SetOptions).ShowDialog();
	}

	private void populateColordLabel()
	{
		var lAqu = h.LordOfZodiacHouse((ZodiacHouse.Aqu), options.Division);
		var lSco = h.LordOfZodiacHouse((ZodiacHouse.Sco), options.Division);
		lColords.Text = string.Format("{0} and {1} are the stronger co-lords", lSco, lAqu);
	}

	private void lColords_Click(object sender, EventArgs e)
	{
	}

	private void GrahaStrengthsControl_Resize(object sender, EventArgs e)
	{
		MhoraGlobalOptions.Instance.GrahaStrengthsFormSize = Size;
	}

	private class UserOptions : ICloneable
	{
		public UserOptions()
		{
			Division = new Division(Vargas.DivisionType.Rasi);
		}

		[PGNotVisible]
		public Division Division
		{
			get;
			set;
		}

		[PGDisplayName("Vargas")]
		public Vargas.DivisionType UIDivision
		{
			get => Division.MultipleDivisions[0].Varga;
			set => Division = new Division(value);
		}

		public object Clone()
		{
			var uo = new UserOptions();
			uo.Division = Division;
			return uo;
		}

		public object SetOptions(object _uo)
		{
			var uo = (UserOptions) _uo;
			Division = uo.Division;
			return Clone();
		}
	}
}