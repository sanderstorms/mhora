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
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Mhora.Components.Property;
using Mhora.Database.Settings;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.Tables;

namespace Mhora.Components;

public class YogaControl : MhoraControl
{
	private readonly IContainer components = null;
	private readonly FindYogas  fy;


	private readonly ToolTip      tt = new();
	private          ContextMenu  mContext;
	private          MenuItem     menuItem1;
	private          MenuItem     menuItem2;
	private          ListView     mList;
	private          MenuItem     mReset;
	private          ColumnHeader Test1;

	public YogaControl(Horoscope _h)
	{
		// This call is required by the Windows Form Designer.
		InitializeComponent();
		h               = _h;
		fy              = new FindYogas(h, new Division(Basics.DivisionType.Rasi));
		mList.BackColor = MhoraGlobalOptions.Instance.ChakraBackgroundColor;
		AddViewsToContextMenu(mContext);
		h.Changed += OnRecalculate;

		evaluateYogas();
		// TODO: Add any initialization after the InitializeComponent call
	}

	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
	private void OnRecalculate(object o)
	{
		mList.Items.Clear();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (components != null)
			{
				components.Dispose();
			}
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
		this.mList     = new System.Windows.Forms.ListView();
		this.Test1     = new System.Windows.Forms.ColumnHeader();
		this.mContext  = new System.Windows.Forms.ContextMenu();
		this.mReset    = new System.Windows.Forms.MenuItem();
		this.menuItem1 = new System.Windows.Forms.MenuItem();
		this.menuItem2 = new System.Windows.Forms.MenuItem();
		this.SuspendLayout();
		// 
		// mList
		// 
		this.mList.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.mList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this.Test1
		});
		this.mList.ContextMenu          =  this.mContext;
		this.mList.Cursor               =  System.Windows.Forms.Cursors.Default;
		this.mList.FullRowSelect        =  true;
		this.mList.Location             =  new System.Drawing.Point(8, 8);
		this.mList.Name                 =  "mList";
		this.mList.Size                 =  new System.Drawing.Size(344, 200);
		this.mList.TabIndex             =  0;
		this.mList.View                 =  System.Windows.Forms.View.Details;
		this.mList.MouseMove            += new System.Windows.Forms.MouseEventHandler(this.mList_MouseMove);
		this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
		// 
		// Test1
		// 
		this.Test1.Width = 141;
		// 
		// mContext
		// 
		this.mContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]
		{
			this.mReset,
			this.menuItem1,
			this.menuItem2
		});
		// 
		// mReset
		// 
		this.mReset.Index =  0;
		this.mReset.Text  =  "&Reset";
		this.mReset.Click += new System.EventHandler(this.mReset_Click);
		// 
		// menuItem1
		// 
		this.menuItem1.Index = 1;
		this.menuItem1.Text  = "-";
		// 
		// menuItem2
		// 
		this.menuItem2.Index = 2;
		this.menuItem2.Text  = "-";
		// 
		// YogaControl
		// 
		this.Controls.Add(this.mList);
		this.Name =  "YogaControl";
		this.Load += new System.EventHandler(this.YogaControl_Load);
		this.ResumeLayout(false);
	}

#endregion

	private void YogaControl_Load(object sender, EventArgs e)
	{
	}

	private void mList_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void resetColumns()
	{
		mList.Columns.Clear();
		mList.Items.Clear();
		mList.Columns.Add("True", 30, HorizontalAlignment.Left);
		mList.Columns.Add("Cat", 70, HorizontalAlignment.Left);
		mList.Columns.Add("Yoga", 70, HorizontalAlignment.Left);
		mList.Columns.Add("Result", 500, HorizontalAlignment.Left);
		mList.Columns.Add("Rule", 500, HorizontalAlignment.Left);
	}

	private void evaluateYoga(XmlYogaNode n)
	{
		var bRet = fy.evaluateYoga(n);
		var li   = new ListViewItem();
		li.Text = bRet.ToString();
		li.SubItems.Add(n.yogaCat);
		li.SubItems.Add(n.yogaName);
		li.SubItems.Add(n.result);
		li.SubItems.Add(n.mhoraRule);
		mList.Items.Add(li);
	}

	private void evaluateYogas()
	{
		try
		{
			evaluateYogasHelper();
		}
		catch
		{
			MessageBox.Show("An error occured while reading file " + MhoraGlobalOptions.Instance.YogasFileName);
		}
	}

	private void evaluateYogasHelper()
	{
		resetColumns();
		XmlYogaNode yn    = null;
		var         sLine = string.Empty;
		var         sType = string.Empty;

		var objReader = new StreamReader(MhoraGlobalOptions.Instance.YogasFileName);

		while ((sLine = objReader.ReadLine()) != null)
		{
			if (sLine.Length > 0 && sLine[0] == '#')
			{
				continue;
			}

			sType = string.Empty;


			var m = Regex.Match(sLine, "^([^:]*)::(.*)$");
			if (m.Success)
			{
				sType = m.Groups[1].Value;
				sLine = m.Groups[2].Value;
			}

			switch (sType.ToLower())
			{
				case "entry":
					if (null != yn && yn.mhoraRule != null && yn.mhoraRule.Length > 0)
					{
						evaluateYoga(yn);
					}

					yn = new XmlYogaNode();
					break;
				case "rule":
					yn.mhoraRule += sLine;
					break;
				case "sourceref":
				case "ref":
					yn.sourceRef += sLine;
					break;
				case "sourcetext":
					yn.sourceText += sLine;
					break;
				case "sourceitxtext":
					yn.sourceItxText += sLine;
					break;
				case "yogacat":
					yn.yogaCat += sLine;
					break;
				case "yoganame":
					yn.yogaName += sLine;
					break;
				default:
				case "result":
					if (null != yn)
					{
						yn.result += sLine;
					}

					break;
			}
		}

		objReader.Close();
		ColorAndFontRows(mList);
	}

	private void mList_MouseMove(object sender, MouseEventArgs e)
	{
		var di = mList.GetItemAt(e.X, e.Y);

		if (di == null)
		{
			tt.Active = false;
			return;
		}

		tt.SetToolTip(this, di.SubItems[2].Text);
		tt.Active       = true;
		tt.InitialDelay = 0;
	}

	private void mReset_Click(object sender, EventArgs e)
	{
		evaluateYogas();
	}
}