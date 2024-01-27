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
using Mhora.Elements.Calculation;
using Mhora.Elements.Dasas;
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
	private readonly Container _components = null;

	private readonly DasaEntry _de;
	private readonly Horoscope _h;

	private readonly ToDate       _td;
	private          ColumnHeader _columnHeader1;
	private          ListView     _mList;
	private          Label        _txtDesc;


	public Dasa3Parts(Horoscope h, DasaEntry de, ToDate td)
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		_td       = td;
		_de = de;
		_h  = h;
		Repopulate();
	}


	private void PopulateDescription()
	{
		var start = _td.AddYears(_de.StartUt);
		var end   = _td.AddYears(_de.StartUt + _de.DasaLength);
		var zh = (_de.ZHouse);
		if ((int) _de.ZHouse != 0)
		{
			_txtDesc.Text = string.Format("{0} - {1} to {2}", zh, start, end);
		}
		else
		{
			_txtDesc.Text = string.Format("{0} - {1} to {2}", _de.Graha, start, end);
		}
	}


	private void Repopulate()
	{
		PopulateDescription();

		var partLength = _de.DasaLength / 3.0;

		var alParts = new ArrayList();
		for (var i = 0; i < 4; i++)
		{
			var m = _td.AddYears(_de.StartUt + partLength * i);
			alParts.Add(m);
		}

		var momentParts = (DateTime[]) alParts.ToArray(typeof(DateTime));

		for (var i = 1; i < momentParts.Length; i++)
		{
			var fmt = string.Format("Equal Part {0} - {1} to {2}", i, momentParts[i - 1], momentParts[i]);
			_mList.Items.Add(fmt);
		}
	}


	/// <summary>
	///     Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_components?.Dispose();
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
		this._txtDesc       = new System.Windows.Forms.Label();
		this._mList         = new System.Windows.Forms.ListView();
		this._columnHeader1 = new System.Windows.Forms.ColumnHeader();
		this.SuspendLayout();
		// 
		// txtDesc
		// 
		this._txtDesc.Anchor   =  ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this._txtDesc.Location =  new System.Drawing.Point(8, 8);
		this._txtDesc.Name     =  "_txtDesc";
		this._txtDesc.Size     =  new System.Drawing.Size(472, 23);
		this._txtDesc.TabIndex =  0;
		this._txtDesc.Text     =  "txtDesc";
		this._txtDesc.Click    += new System.EventHandler(this.label1_Click);
		// 
		// mList
		// 
		this._mList.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this._mList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
		{
			this._columnHeader1
		});
		this._mList.FullRowSelect        =  true;
		this._mList.Location             =  new System.Drawing.Point(8, 40);
		this._mList.Name                 =  "_mList";
		this._mList.Size                 =  new System.Drawing.Size(472, 272);
		this._mList.TabIndex             =  1;
		this._mList.View                 =  System.Windows.Forms.View.Details;
		this._mList.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
		// 
		// columnHeader1
		// 
		this._columnHeader1.Width = 1000;
		// 
		// Dasa3Parts
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize        = new System.Drawing.Size(488, 318);
		this.Controls.Add(this._mList);
		this.Controls.Add(this._txtDesc);
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