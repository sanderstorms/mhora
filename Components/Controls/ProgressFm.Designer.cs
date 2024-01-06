namespace Mhora.Components.Controls
{
    partial class ProgressFm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgressBar = new ProgressBar();
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.ProgressBar.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ProgressBar.Location = new System.Drawing.Point(10, 10);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.ProgressBackColor = System.Drawing.SystemColors.ControlDark;
            this.ProgressBar.ProgressBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ProgressBar.ProgressFont = new System.Drawing.Font("Microsoft Sans Serif", 62F, System.Drawing.FontStyle.Bold);
            this.ProgressBar.ProgressFontColor = System.Drawing.Color.Black;
            this.ProgressBar.Size = new System.Drawing.Size(428, 53);
            this.ProgressBar.TabIndex = 0;
            this.ProgressBar.TabStop = false;
            this.ProgressBar.Value = 0;
            // 
            // ProgressFm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(450, 74);
            this.ControlBox = false;
            this.Controls.Add(this.ProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressFm";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.ResumeLayout(false);

        }

        #endregion

        private ProgressBar ProgressBar;
    }
}