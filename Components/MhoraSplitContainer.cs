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

namespace Mhora.Components;

/// <summary>
///     Summary description for MhoraSplitContainer.
/// </summary>
public class MhoraSplitContainer : UserControl
{
	public enum DrawStyle
	{
		LeftRight,
		UpDown
	}

	/// <summary>
	///     Required designer variable.
	/// </summary>
	private readonly Container components = null;

	private UserControl mControl2;
	private DrawStyle   mDrawDock;
	private int         nItems;
	public  Splitter    sp;

	public MhoraSplitContainer(UserControl _mControl)
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitForm call
		Control1      = _mControl;
		Control1.Dock = DockStyle.Fill;
		Controls.Add(Control1);
		sp           = new Splitter();
		sp.BackColor = Color.LightGray;
		sp.Dock      = DockStyle.Left;
		DrawDock     = DrawStyle.LeftRight;
		nItems       = 1;

		Dock      =  DockStyle.Fill;
		sp.Height += 2;
		sp.Width  += 2;
	}

	public DrawStyle DrawDock
	{
		get => mDrawDock;
		set
		{
			mDrawDock = value;

			if (nItems < 1)
			{
				Control1.Dock = DockStyle.Fill;
				return;
			}

			if (mDrawDock == DrawStyle.UpDown)
			{
				Control1.Dock = DockStyle.Top;
				sp.Dock       = DockStyle.Top;
			}
			else
			{
				Control1.Dock = DockStyle.Left;
				sp.Dock       = DockStyle.Left;
			}

			mControl2.Dock = DockStyle.Fill;
		}
	}

	public UserControl Control1
	{
		get;
		set;
	}

	public UserControl Control2
	{
		get => mControl2;
		set
		{
			mControl2 = value;
			DrawDock  = DrawDock;
			//mControl1.Dock = DockStyle.Left;
			//mControl2.Dock = DockStyle.Fill;
			if (nItems == 1)
			{
				nItems++;
				Controls.Remove(Control1);
				/*if (this.DrawDock == DrawStyle.UpDown)
				    sp.SplitPosition = this.Width / 2;
				else
				    sp.SplitPosition = this.Height / 2;
				*/
				Controls.AddRange(new Control[]
				{
					mControl2,
					sp,
					Control1
				});
			}
		}
	}

	public DockStyle SplitterDockStyle
	{
		get => sp.Dock;
		set => sp.Dock = value;
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

#region Component Designer generated code

	/// <summary>
	///     Required method for Designer support - do not modify
	///     the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		// 
		// MhoraSplitContainer
		// 
		this.Name   =  "MhoraSplitContainer";
		this.Resize += new System.EventHandler(this.MhoraSplitContainer_Resize);
		this.Load   += new System.EventHandler(this.MhoraSplitContainer_Load);
	}

#endregion

	private void MhoraSplitContainer_Load(object sender, EventArgs e)
	{
	}

	private void MhoraSplitContainer_Resize(object sender, EventArgs e)
	{
	}
}