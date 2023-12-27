namespace Mhora.Components
{
    partial class BirthDetailsDialog
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
            this.tabControlBirthDetails = new System.Windows.Forms.TabControl();
            this.tabBirthData = new System.Windows.Forms.TabPage();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.groupBoxGender = new System.Windows.Forms.GroupBox();
            this.rbFemale = new System.Windows.Forms.RadioButton();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labDST = new System.Windows.Forms.Label();
            this.labTimezone = new System.Windows.Forms.Label();
            this.labLatitude = new System.Windows.Forms.Label();
            this.labLongitude = new System.Windows.Forms.Label();
            this.labCity = new System.Windows.Forms.Label();
            this.labState = new System.Windows.Forms.Label();
            this.labCountry = new System.Windows.Forms.Label();
            this.labTime = new System.Windows.Forms.Label();
            this.labDate = new System.Windows.Forms.Label();
            this.labGender = new System.Windows.Forms.Label();
            this.labName = new System.Windows.Forms.Label();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.timePicker1 = new TimePicker.Opulos.Core.UI.TimePicker();
            this.tabControlBirthDetails.SuspendLayout();
            this.tabBirthData.SuspendLayout();
            this.groupBoxGender.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlBirthDetails
            // 
            this.tabControlBirthDetails.Controls.Add(this.tabBirthData);
            this.tabControlBirthDetails.Controls.Add(this.tabEvents);
            this.tabControlBirthDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlBirthDetails.Location = new System.Drawing.Point(0, 0);
            this.tabControlBirthDetails.Name = "tabControlBirthDetails";
            this.tabControlBirthDetails.SelectedIndex = 0;
            this.tabControlBirthDetails.Size = new System.Drawing.Size(800, 540);
            this.tabControlBirthDetails.TabIndex = 0;
            // 
            // tabBirthData
            // 
            this.tabBirthData.Controls.Add(this.timePicker1);
            this.tabBirthData.Controls.Add(this.dateTimePicker);
            this.tabBirthData.Controls.Add(this.groupBoxGender);
            this.tabBirthData.Controls.Add(this.textBox1);
            this.tabBirthData.Controls.Add(this.labDST);
            this.tabBirthData.Controls.Add(this.labTimezone);
            this.tabBirthData.Controls.Add(this.labLatitude);
            this.tabBirthData.Controls.Add(this.labLongitude);
            this.tabBirthData.Controls.Add(this.labCity);
            this.tabBirthData.Controls.Add(this.labState);
            this.tabBirthData.Controls.Add(this.labCountry);
            this.tabBirthData.Controls.Add(this.labTime);
            this.tabBirthData.Controls.Add(this.labDate);
            this.tabBirthData.Controls.Add(this.labGender);
            this.tabBirthData.Controls.Add(this.labName);
            this.tabBirthData.Location = new System.Drawing.Point(4, 29);
            this.tabBirthData.Name = "tabBirthData";
            this.tabBirthData.Padding = new System.Windows.Forms.Padding(3);
            this.tabBirthData.Size = new System.Drawing.Size(792, 507);
            this.tabBirthData.TabIndex = 0;
            this.tabBirthData.Text = "Birth data";
            this.tabBirthData.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(153, 104);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowUpDown = true;
            this.dateTimePicker.Size = new System.Drawing.Size(376, 27);
            this.dateTimePicker.TabIndex = 13;
            // 
            // groupBoxGender
            // 
            this.groupBoxGender.Controls.Add(this.rbFemale);
            this.groupBoxGender.Controls.Add(this.rbMale);
            this.groupBoxGender.Location = new System.Drawing.Point(156, 49);
            this.groupBoxGender.Name = "groupBoxGender";
            this.groupBoxGender.Size = new System.Drawing.Size(373, 48);
            this.groupBoxGender.TabIndex = 12;
            this.groupBoxGender.TabStop = false;
            // 
            // rbFemale
            // 
            this.rbFemale.AutoSize = true;
            this.rbFemale.Location = new System.Drawing.Point(182, 16);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(85, 24);
            this.rbFemale.TabIndex = 1;
            this.rbFemale.TabStop = true;
            this.rbFemale.Text = "Female";
            this.rbFemale.UseVisualStyleBackColor = true;
            // 
            // rbMale
            // 
            this.rbMale.AutoSize = true;
            this.rbMale.Location = new System.Drawing.Point(6, 16);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(66, 24);
            this.rbMale.TabIndex = 0;
            this.rbMale.TabStop = true;
            this.rbMale.Text = "Male";
            this.rbMale.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(153, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(377, 27);
            this.textBox1.TabIndex = 11;
            // 
            // labDST
            // 
            this.labDST.AutoSize = true;
            this.labDST.Location = new System.Drawing.Point(16, 384);
            this.labDST.Name = "labDST";
            this.labDST.Size = new System.Drawing.Size(43, 20);
            this.labDST.TabIndex = 10;
            this.labDST.Text = "DST";
            // 
            // labTimezone
            // 
            this.labTimezone.AutoSize = true;
            this.labTimezone.Location = new System.Drawing.Point(16, 349);
            this.labTimezone.Name = "labTimezone";
            this.labTimezone.Size = new System.Drawing.Size(82, 20);
            this.labTimezone.TabIndex = 9;
            this.labTimezone.Text = "Timezone";
            // 
            // labLatitude
            // 
            this.labLatitude.AutoSize = true;
            this.labLatitude.Location = new System.Drawing.Point(16, 314);
            this.labLatitude.Name = "labLatitude";
            this.labLatitude.Size = new System.Drawing.Size(69, 20);
            this.labLatitude.TabIndex = 8;
            this.labLatitude.Text = "Latitude";
            // 
            // labLongitude
            // 
            this.labLongitude.AutoSize = true;
            this.labLongitude.Location = new System.Drawing.Point(16, 279);
            this.labLongitude.Name = "labLongitude";
            this.labLongitude.Size = new System.Drawing.Size(82, 20);
            this.labLongitude.TabIndex = 7;
            this.labLongitude.Text = "Longitude";
            // 
            // labCity
            // 
            this.labCity.AutoSize = true;
            this.labCity.Location = new System.Drawing.Point(16, 244);
            this.labCity.Name = "labCity";
            this.labCity.Size = new System.Drawing.Size(38, 20);
            this.labCity.TabIndex = 6;
            this.labCity.Text = "City";
            // 
            // labState
            // 
            this.labState.AutoSize = true;
            this.labState.Location = new System.Drawing.Point(16, 209);
            this.labState.Name = "labState";
            this.labState.Size = new System.Drawing.Size(48, 20);
            this.labState.TabIndex = 5;
            this.labState.Text = "State";
            // 
            // labCountry
            // 
            this.labCountry.AutoSize = true;
            this.labCountry.Location = new System.Drawing.Point(16, 174);
            this.labCountry.Name = "labCountry";
            this.labCountry.Size = new System.Drawing.Size(67, 20);
            this.labCountry.TabIndex = 4;
            this.labCountry.Text = "Country";
            // 
            // labTime
            // 
            this.labTime.AutoSize = true;
            this.labTime.Location = new System.Drawing.Point(16, 139);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(46, 20);
            this.labTime.TabIndex = 3;
            this.labTime.Text = "Time";
            // 
            // labDate
            // 
            this.labDate.AutoSize = true;
            this.labDate.Location = new System.Drawing.Point(16, 104);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(45, 20);
            this.labDate.TabIndex = 2;
            this.labDate.Text = "Date";
            // 
            // labGender
            // 
            this.labGender.AutoSize = true;
            this.labGender.Location = new System.Drawing.Point(16, 69);
            this.labGender.Name = "labGender";
            this.labGender.Size = new System.Drawing.Size(64, 20);
            this.labGender.TabIndex = 1;
            this.labGender.Text = "Gender";
            // 
            // labName
            // 
            this.labName.AutoSize = true;
            this.labName.Location = new System.Drawing.Point(16, 19);
            this.labName.Name = "labName";
            this.labName.Size = new System.Drawing.Size(53, 20);
            this.labName.TabIndex = 0;
            this.labName.Text = "Name";
            // 
            // tabEvents
            // 
            this.tabEvents.Location = new System.Drawing.Point(4, 25);
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Size = new System.Drawing.Size(792, 511);
            this.tabEvents.TabIndex = 1;
            this.tabEvents.Text = "Events";
            this.tabEvents.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(713, 546);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 546);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // timePicker1
            // 
            this.timePicker1.AutoCloseMenuFocusLost = true;
            this.timePicker1.AutoCloseMenuWindowChanged = true;
            this.timePicker1.ByDigit = false;
            this.timePicker1.CaretVisible = false;
            this.timePicker1.CaretWrapsAround = true;
            this.timePicker1.ChopRunningText = true;
            this.timePicker1.Cursor = System.Windows.Forms.Cursors.Default;
            this.timePicker1.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.timePicker1.DateTimeFormat = "HH:mm:ss.fff";
            this.timePicker1.DeleteKeyShiftsTextLeft = true;
            this.timePicker1.EscapeKeyRevertsValue = false;
            this.timePicker1.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.timePicker1.KeepSelectedIncludesWhitespace = false;
            this.timePicker1.KeepTokenSelected = true;
            this.timePicker1.Location = new System.Drawing.Point(153, 139);
            this.timePicker1.Mask = "99:99:99.999";
            this.timePicker1.Name = "timePicker1";
            this.timePicker1.PromptChar = '0';
            this.timePicker1.Size = new System.Drawing.Size(157, 27);
            this.timePicker1.SplitChars = null;
            this.timePicker1.TabIndex = 14;
            this.timePicker1.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.timePicker1.UseMaxValueIfTooLarge = false;
            this.timePicker1.Value = new System.DateTime(((long)(0)));
            this.timePicker1.ValuesCarryOver = false;
            this.timePicker1.ValuesWrapAround = true;
            this.timePicker1.ValuesWrapIfNoCarryRoom = true;
            this.timePicker1.ValueTooLargeFixMode = TimePicker.Opulos.Core.UI.ValueFixMode.KeepExistingValue;
            this.timePicker1.ValueTooSmallFixMode = TimePicker.Opulos.Core.UI.ValueFixMode.KeepExistingValue;
            // 
            // BirthDetailsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(800, 584);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControlBirthDetails);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BirthDetailsDialog";
            this.tabControlBirthDetails.ResumeLayout(false);
            this.tabBirthData.ResumeLayout(false);
            this.tabBirthData.PerformLayout();
            this.groupBoxGender.ResumeLayout(false);
            this.groupBoxGender.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlBirthDetails;
        private System.Windows.Forms.TabPage tabBirthData;
        private System.Windows.Forms.TabPage tabEvents;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labCity;
        private System.Windows.Forms.Label labState;
        private System.Windows.Forms.Label labCountry;
        private System.Windows.Forms.Label labTime;
        private System.Windows.Forms.Label labDate;
        private System.Windows.Forms.Label labGender;
        private System.Windows.Forms.Label labName;
        private System.Windows.Forms.Label labTimezone;
        private System.Windows.Forms.Label labLatitude;
        private System.Windows.Forms.Label labLongitude;
        private System.Windows.Forms.Label labDST;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.GroupBox groupBoxGender;
        private System.Windows.Forms.RadioButton rbFemale;
        private System.Windows.Forms.RadioButton rbMale;
        private System.Windows.Forms.TextBox textBox1;
        private TimePicker.Opulos.Core.UI.TimePicker timePicker1;
    }
}