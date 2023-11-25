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
using System.Windows.Forms;
using mhora.Components.Property;

namespace mhora.Components
{
    /// <summary>
    ///     Display a PropertyGrid for any object, and deal with
    ///     event handlers to perform any requested updates
    /// </summary>
    public class MhoraOptions : Form
    {
        private readonly ApplyOptions applyEvent;
        private          Button       bApply;
        private          Button       bCancel;
        private          Button       bOK;

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private readonly Container components = null;

        public PropertyGrid pGrid;

        public MhoraOptions(object a, ApplyOptions o, bool NoCancel)
        {
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            pGrid.SelectedObject = new GlobalizedPropertiesWrapper(a);
            pGrid.HelpVisible    = true;
            applyEvent           = o;
            bCancel.Enabled      = false;
        }

        public MhoraOptions(object a, ApplyOptions o)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            pGrid.SelectedObject = new GlobalizedPropertiesWrapper(a);
            applyEvent           = o;
            //this.applyEvent(pGrid.SelectedObject);
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
            this.pGrid   = new System.Windows.Forms.PropertyGrid();
            this.bApply  = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK     = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pGrid
            // 
            this.pGrid.Anchor                     =  ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pGrid.CommandsVisibleIfAvailable =  true;
            this.pGrid.LargeButtons               =  false;
            this.pGrid.LineColor                  =  System.Drawing.SystemColors.ScrollBar;
            this.pGrid.Location                   =  new System.Drawing.Point(8, 8);
            this.pGrid.Name                       =  "pGrid";
            this.pGrid.PropertySort               =  System.Windows.Forms.PropertySort.Categorized;
            this.pGrid.Size                       =  new System.Drawing.Size(284, 216);
            this.pGrid.TabIndex                   =  1;
            this.pGrid.Text                       =  "propertyGrid1";
            this.pGrid.ToolbarVisible             =  false;
            this.pGrid.ViewBackColor              =  System.Drawing.SystemColors.Window;
            this.pGrid.ViewForeColor              =  System.Drawing.SystemColors.WindowText;
            this.pGrid.Click                      += new System.EventHandler(this.pGrid_Click);
            // 
            // bApply
            // 
            this.bApply.Anchor   =  ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bApply.Location =  new System.Drawing.Point(8, 232);
            this.bApply.Name     =  "bApply";
            this.bApply.TabIndex =  0;
            this.bApply.Text     =  "Apply";
            this.bApply.Click    += new System.EventHandler(this.bApply_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor       =  ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult =  System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location     =  new System.Drawing.Point(204, 232);
            this.bCancel.Name         =  "bCancel";
            this.bCancel.TabIndex     =  2;
            this.bCancel.Text         =  "Cancel";
            this.bCancel.Click        += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.Anchor   =  ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location =  new System.Drawing.Point(104, 232);
            this.bOK.Name     =  "bOK";
            this.bOK.Size     =  new System.Drawing.Size(79, 23);
            this.bOK.TabIndex =  3;
            this.bOK.Text     =  "OK";
            this.bOK.Click    += new System.EventHandler(this.bOK_Click);
            // 
            // MhoraOptions
            // 
            this.AcceptButton      = this.bApply;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton      = this.bCancel;
            this.ClientSize        = new System.Drawing.Size(296, 273);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bApply);
            this.Controls.Add(this.pGrid);
            this.Name =  "MhoraOptions";
            this.Text =  "Options";
            this.Load += new System.EventHandler(this.MhoraOptions_Load);
            this.ResumeLayout(false);
        }

#endregion

        private void MhoraOptions_Load(object sender, EventArgs e)
        {
        }

        private void pGrid_Click(object sender, EventArgs e)
        {
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Apply(bool bKeepOpen)
        {
            var wrapper    = (GlobalizedPropertiesWrapper)pGrid.SelectedObject;
            var objApplied = applyEvent(wrapper.GetWrappedObject());
            if (bKeepOpen)
            {
                pGrid.SelectedObject = new GlobalizedPropertiesWrapper(objApplied);
            }
        }

        private void bApply_Click(object sender, EventArgs e)
        {
            Apply(true);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Apply(false);
            Close();
        }
    }
}