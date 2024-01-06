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
using System.Windows.Forms;
using Mhora.Elements;
using Mhora.Elements.Calculation;
using Mhora.SwissEph;
using Mhora.Util;

namespace Mhora.Components.Dasa;

/// <summary>
///     Summary description for Dasa3Parts.
/// </summary>
public class Dasa3Parts : Form
{
	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;

	private readonly DasaEntry de;
	private readonly Horoscope h;

	private readonly ToDate       td;
	private          ColumnHeader columnHeader1;
	private          ListView     mList;
	private          Label        txtDesc;


	public Dasa3Parts(Horoscope _h, DasaEntry _de, ToDate _td)
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		td = _td;
		de = _de;
		h  = _h;
		repopulate();
	}


	private void populateDescription()
	{
		sweph.obtainLock(h);
		var start = td.AddYears(de.startUT);
		var end   = td.AddYears(de.startUT + de.DasaLength);
		sweph.releaseLock(h);
		var zh = new ZodiacHouse(de.ZHouse);
		if ((int) de.ZHouse != 0)
		{
			txtDesc.Text = string.Format("{0} - {1} to {2}", zh, start, end);
		}
		else
		{
			txtDesc.Text = string.Format("{0} - {1} to {2}", de.graha, start, end);
		}
	}


	private void repopulate()
	{
		populateDescription();

		var partLength = de.DasaLength / 3.0;

		sweph.obtainLock(h);
		var alParts = new ArrayList();
		for (var i = 0; i < 4; i++)
		{
			var m = td.AddYears(de.startUT + partLength * i);
			alParts.Add(m);
		}

		var momentParts = (Moment[]) alParts.ToArray(typeof(Moment));
		sweph.releaseLock(h);

		for (var i = 1; i < momentParts.Length; i++)
		{
			var fmt = string.Format("Equal Part {0} - {1} to {2}", i, momentParts[i - 1], momentParts[i]);
			mList.Items.Add(fmt);
		}
	}


	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
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

#region Windows Form Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		this.txtDesc       = new System.Windows.Forms.Label();
		this.mList         = new System.Windows.Forms.ListView();
		this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
		this.SuspendLayout();
		// 
		// txtDesc
		// 
		this.txtDesc.Anchor   =  ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.txtDesc.Location =  new System.Drawing.Point(8, 8);
		this.txtDesc.Name     =  "txtDesc";
		this.txtDesc.Size     =  new System.Drawing.Size(472, 23);
		this.txtDesc.TabIndex =  0;
		this.txtDesc.Text     =  "txtDesc";
		this.txtDesc.Click    += new System.EventHandler(this.label1_Click);
		// 
		// mList
		// 
		this.mList.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.mList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this.columnHeader1
		});
		this.mList.FullRowSelect        =  true;
		this.mList.Location             =  new System.Drawing.Point(8, 40);
		this.mList.Name                 =  "mList";
		this.mList.Size                 =  new System.Drawing.Size(472, 272);
		this.mList.TabIndex             =  1;
		this.mList.View                 =  System.Windows.Forms.View.Details;
		this.mList.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
		// 
		// columnHeader1
		// 
		this.columnHeader1.Width = 1000;
		// 
		// Dasa3Parts
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize        = new System.Drawing.Size(488, 318);
		this.Controls.Add(this.mList);
		this.Controls.Add(this.txtDesc);
		this.Name =  "Dasa3Parts";
		this.Text =  "Dasa 3 Parts Reckoner";
		this.Load += new System.EventHandler(this.Dasa3Parts_Load);
		this.ResumeLayout(false);
	}

#endregion

	private void Dasa3Parts_Load(object sender, EventArgs e)
	{
	}

	private void label1_Click(object sender, EventArgs e)
	{
	}

	private void listView1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}
}