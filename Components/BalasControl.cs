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
using System.Drawing;
using System.Windows.Forms;
using Mhora.Elements;
using Mhora.Elements.Calculation;

namespace Mhora.Components;

public class BalasControl : MhoraControl
{
	private readonly IContainer  components = null;
	private          ContextMenu contextMenu;
	private          ListView    mList;

	public BalasControl(Horoscope _h)
	{
		// This call is required by the Windows Form Designer.
		InitializeComponent();
		h               =  _h;
		h.Changed       += onRecalculate;
		mList.BackColor =  Color.AliceBlue;
		Repopulate();
		AddViewsToContextMenu(contextMenu);
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
		this.mList       = new System.Windows.Forms.ListView();
		this.contextMenu = new System.Windows.Forms.ContextMenu();
		this.SuspendLayout();
		// 
		// mList
		// 
		this.mList.Anchor               =  ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
		this.mList.ContextMenu          =  this.contextMenu;
		this.mList.FullRowSelect        =  true;
		this.mList.Location             =  new System.Drawing.Point(8, 8);
		this.mList.Name                 =  "mList";
		this.mList.Size                 =  new System.Drawing.Size(520, 272);
		this.mList.TabIndex             =  0;
		this.mList.View                 =  System.Windows.Forms.View.Details;
		this.mList.SelectedIndexChanged += new System.EventHandler(this.mList_SelectedIndexChanged);
		// 
		// BalasControl
		// 
		this.Controls.Add(this.mList);
		this.Name = "BalasControl";
		this.Size = new System.Drawing.Size(536, 288);
		this.ResumeLayout(false);
	}

#endregion

	private void mList_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	public void onRecalculate(object h)
	{
		Repopulate();
	}

	private void ResizeColumns()
	{
		for (var i = 0; i < mList.Columns.Count; i++)
		{
			mList.Columns[i].Width = -1;
		}

		mList.Columns[mList.Columns.Count - 1].Width = -2;
	}

	public string fmtBala(double rupas)
	{
		var fmt = string.Format("{0:0.00}", rupas);
		return fmt;
	}

	public void Repopulate()
	{
		Body.BodyType[] grahas =
		{
			Body.BodyType.Sun,
			Body.BodyType.Moon,
			Body.BodyType.Mars,
			Body.BodyType.Mercury,
			Body.BodyType.Jupiter,
			Body.BodyType.Venus,
			Body.BodyType.Saturn
		};
		mList.Clear();

		mList.Columns.Add("Bala", 120, HorizontalAlignment.Left);
		foreach (var b in grahas)
		{
			mList.Columns.Add(b.ToString(), 70, HorizontalAlignment.Left);
		}

		{
			var li = new ListViewItem("Sthana");
			foreach (var b in grahas)
			{
				li.SubItems.Add("-");
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Uccha");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.UcchaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Oja-Yugma");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.OjaYugmaRasyAmsaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Kendra");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.KendraBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Drekkana");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.DrekkanaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("Dik");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.DigBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("Kaala");
			foreach (var b in grahas)
			{
				li.SubItems.Add("-");
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Nathonnatha");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.NathonnathaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Paksha");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.PakshaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Tribhaaga");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.TribhaagaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Abda");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.AbdaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Masa");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.MasaBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Vara");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.VaraBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("-> Hora");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.HoraBala(b)));
			}

			mList.Items.Add(li);
		}
		{
			var li = new ListViewItem("Naisargika");
			foreach (var b in grahas)
			{
				li.SubItems.Add(fmtBala(h.NaisargikaBala(b)));
			}

			mList.Items.Add(li);
		}


		ColorAndFontRows(mList);
	}
}