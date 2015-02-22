namespace UltraHookInject
{
	partial class Form1
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.lblInfo = new System.Windows.Forms.Label();
			this.cobCHT = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkUseDot = new System.Windows.Forms.CheckBox();
			this.checkFPSLimit = new System.Windows.Forms.CheckBox();
			this.preview = new System.Windows.Forms.Panel();
			this.openImage = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.lblFPS = new System.Windows.Forms.Label();
			this.numFPSLimit = new System.Windows.Forms.NumericUpDown();
			this.resizer = new System.Windows.Forms.Timer(this.components);
			this.cbxProcessList = new System.Windows.Forms.ComboBox();
			this.checkFilerList = new System.Windows.Forms.CheckBox();
			this.checkAutoInject = new System.Windows.Forms.CheckBox();
			this.checkAutoReInject = new System.Windows.Forms.CheckBox();
			this.btnStartSearch = new System.Windows.Forms.Button();
			this.checkSettings = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.numResX = new System.Windows.Forms.NumericUpDown();
			this.numResY = new System.Windows.Forms.NumericUpDown();
			this.mTitlebar1 = new MetroObjects.MTitlebar();
			((System.ComponentModel.ISupportInitialize)(this.numFPSLimit)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numResX)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numResY)).BeginInit();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// lblInfo
			// 
			this.lblInfo.BackColor = System.Drawing.SystemColors.Control;
			this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblInfo.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfo.Location = new System.Drawing.Point(5, 25);
			this.lblInfo.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(240, 25);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "<==== INFOBAR ====>";
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cobCHT
			// 
			this.cobCHT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cobCHT.FormattingEnabled = true;
			this.cobCHT.Items.AddRange(new object[] {
            "none",
            "lines",
            "thin lines",
            "square",
            "custom"});
			this.cobCHT.Location = new System.Drawing.Point(96, 58);
			this.cobCHT.Name = "cobCHT";
			this.cobCHT.Size = new System.Drawing.Size(85, 21);
			this.cobCHT.TabIndex = 5;
			this.cobCHT.DropDown += new System.EventHandler(this.cobCHT_DropDown);
			this.cobCHT.SelectedIndexChanged += new System.EventHandler(this.cobCHT_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.Control;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Location = new System.Drawing.Point(5, 55);
			this.label2.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 25);
			this.label2.TabIndex = 6;
			this.label2.Text = "Crosshair style:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUseDot
			// 
			this.checkUseDot.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkUseDot.BackColor = System.Drawing.SystemColors.Control;
			this.checkUseDot.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkUseDot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkUseDot.Location = new System.Drawing.Point(190, 55);
			this.checkUseDot.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.checkUseDot.Name = "checkUseDot";
			this.checkUseDot.Size = new System.Drawing.Size(55, 25);
			this.checkUseDot.TabIndex = 9;
			this.checkUseDot.Text = "use dot";
			this.checkUseDot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkUseDot.UseVisualStyleBackColor = false;
			this.checkUseDot.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// checkFPSLimit
			// 
			this.checkFPSLimit.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkFPSLimit.BackColor = System.Drawing.SystemColors.Control;
			this.checkFPSLimit.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkFPSLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkFPSLimit.Location = new System.Drawing.Point(250, 85);
			this.checkFPSLimit.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.checkFPSLimit.Name = "checkFPSLimit";
			this.checkFPSLimit.Size = new System.Drawing.Size(80, 25);
			this.checkFPSLimit.TabIndex = 10;
			this.checkFPSLimit.Text = "limit FPS";
			this.checkFPSLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkFPSLimit.UseVisualStyleBackColor = false;
			this.checkFPSLimit.CheckedChanged += new System.EventHandler(this.SetFPSLimit_Changed);
			// 
			// preview
			// 
			this.preview.Location = new System.Drawing.Point(338, 9);
			this.preview.Name = "preview";
			this.preview.Size = new System.Drawing.Size(10, 23);
			this.preview.TabIndex = 11;
			this.preview.Paint += new System.Windows.Forms.PaintEventHandler(this.preview_Paint);
			// 
			// openImage
			// 
			this.openImage.BackColor = System.Drawing.SystemColors.Control;
			this.openImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.openImage.Location = new System.Drawing.Point(250, 55);
			this.openImage.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.openImage.Name = "openImage";
			this.openImage.Size = new System.Drawing.Size(80, 25);
			this.openImage.TabIndex = 12;
			this.openImage.Text = "Open Image";
			this.openImage.UseVisualStyleBackColor = false;
			this.openImage.Click += new System.EventHandler(this.openImage_Click);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(5, 85);
			this.label3.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 25);
			this.label3.TabIndex = 13;
			this.label3.Text = "FPS Reader:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblFPS
			// 
			this.lblFPS.BackColor = System.Drawing.SystemColors.Control;
			this.lblFPS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblFPS.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFPS.Location = new System.Drawing.Point(96, 85);
			this.lblFPS.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.lblFPS.Name = "lblFPS";
			this.lblFPS.Size = new System.Drawing.Size(85, 25);
			this.lblFPS.TabIndex = 14;
			this.lblFPS.Text = "--===--";
			this.lblFPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// numFPSLimit
			// 
			this.numFPSLimit.Location = new System.Drawing.Point(190, 88);
			this.numFPSLimit.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.numFPSLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numFPSLimit.Name = "numFPSLimit";
			this.numFPSLimit.Size = new System.Drawing.Size(55, 20);
			this.numFPSLimit.TabIndex = 16;
			this.numFPSLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numFPSLimit.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.numFPSLimit.ValueChanged += new System.EventHandler(this.SetFPSLimit_Changed);
			// 
			// resizer
			// 
			this.resizer.Interval = 10;
			this.resizer.Tick += new System.EventHandler(this.resizer_Tick);
			// 
			// cbxProcessList
			// 
			this.cbxProcessList.FormattingEnabled = true;
			this.cbxProcessList.Location = new System.Drawing.Point(5, 126);
			this.cbxProcessList.Name = "cbxProcessList";
			this.cbxProcessList.Size = new System.Drawing.Size(240, 21);
			this.cbxProcessList.TabIndex = 18;
			this.cbxProcessList.DropDown += new System.EventHandler(this.comboBox1_DropDown);
			// 
			// checkFilerList
			// 
			this.checkFilerList.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkFilerList.Checked = true;
			this.checkFilerList.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkFilerList.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkFilerList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkFilerList.Location = new System.Drawing.Point(250, 123);
			this.checkFilerList.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.checkFilerList.Name = "checkFilerList";
			this.checkFilerList.Size = new System.Drawing.Size(80, 25);
			this.checkFilerList.TabIndex = 19;
			this.checkFilerList.Text = "Filter list";
			this.checkFilerList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkFilerList.UseVisualStyleBackColor = true;
			// 
			// checkAutoInject
			// 
			this.checkAutoInject.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkAutoInject.BackColor = System.Drawing.SystemColors.Control;
			this.checkAutoInject.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkAutoInject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkAutoInject.Location = new System.Drawing.Point(5, 155);
			this.checkAutoInject.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.checkAutoInject.Name = "checkAutoInject";
			this.checkAutoInject.Size = new System.Drawing.Size(85, 25);
			this.checkAutoInject.TabIndex = 20;
			this.checkAutoInject.Text = "Auto Inject";
			this.checkAutoInject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkAutoInject.UseVisualStyleBackColor = false;
			this.checkAutoInject.CheckedChanged += new System.EventHandler(this.checkAutoInject_CheckedChanged);
			// 
			// checkAutoReInject
			// 
			this.checkAutoReInject.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkAutoReInject.BackColor = System.Drawing.SystemColors.Control;
			this.checkAutoReInject.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkAutoReInject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkAutoReInject.Location = new System.Drawing.Point(95, 155);
			this.checkAutoReInject.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.checkAutoReInject.Name = "checkAutoReInject";
			this.checkAutoReInject.Size = new System.Drawing.Size(85, 25);
			this.checkAutoReInject.TabIndex = 21;
			this.checkAutoReInject.Text = "Auto Re-Inject";
			this.checkAutoReInject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkAutoReInject.UseVisualStyleBackColor = false;
			// 
			// btnStartSearch
			// 
			this.btnStartSearch.BackColor = System.Drawing.SystemColors.Control;
			this.btnStartSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStartSearch.Location = new System.Drawing.Point(190, 155);
			this.btnStartSearch.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.btnStartSearch.Name = "btnStartSearch";
			this.btnStartSearch.Size = new System.Drawing.Size(140, 25);
			this.btnStartSearch.TabIndex = 22;
			this.btnStartSearch.Text = "Start one-time Search";
			this.btnStartSearch.UseVisualStyleBackColor = false;
			this.btnStartSearch.Click += new System.EventHandler(this.btnStartSearch_Click);
			// 
			// checkSettings
			// 
			this.checkSettings.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkSettings.BackColor = System.Drawing.SystemColors.Control;
			this.checkSettings.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkSettings.Location = new System.Drawing.Point(250, 25);
			this.checkSettings.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.checkSettings.Name = "checkSettings";
			this.checkSettings.Size = new System.Drawing.Size(80, 25);
			this.checkSettings.TabIndex = 23;
			this.checkSettings.Text = "Settings";
			this.checkSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkSettings.UseVisualStyleBackColor = false;
			this.checkSettings.CheckedChanged += new System.EventHandler(this.checkSettings_CheckedChanged);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Location = new System.Drawing.Point(5, 185);
			this.label1.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(175, 25);
			this.label1.TabIndex = 13;
			this.label1.Text = "Ingame Resolution [ X | Y ]";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// numResX
			// 
			this.numResX.Location = new System.Drawing.Point(190, 189);
			this.numResX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.numResX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numResX.Name = "numResX";
			this.numResX.Size = new System.Drawing.Size(65, 20);
			this.numResX.TabIndex = 24;
			this.numResX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numResX.Value = new decimal(new int[] {
            1920,
            0,
            0,
            0});
			this.numResX.ValueChanged += new System.EventHandler(this.numResX_ValueChanged);
			// 
			// numResY
			// 
			this.numResY.Location = new System.Drawing.Point(265, 189);
			this.numResY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.numResY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numResY.Name = "numResY";
			this.numResY.Size = new System.Drawing.Size(65, 20);
			this.numResY.TabIndex = 25;
			this.numResY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numResY.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
			this.numResY.ValueChanged += new System.EventHandler(this.numResY_ValueChanged);
			// 
			// mTitlebar1
			// 
			this.mTitlebar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.mTitlebar1.ColorOne = System.Drawing.Color.White;
			this.mTitlebar1.ColorThree = System.Drawing.Color.Silver;
			this.mTitlebar1.ColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.mTitlebar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.mTitlebar1.DragElement = null;
			this.mTitlebar1.Icon = null;
			this.mTitlebar1.Location = new System.Drawing.Point(0, 0);
			this.mTitlebar1.Name = "mTitlebar1";
			this.mTitlebar1.Size = new System.Drawing.Size(335, 20);
			this.mTitlebar1.TabIndex = 26;
			this.mTitlebar1.Text = "UltraHook";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(335, 215);
			this.Controls.Add(this.mTitlebar1);
			this.Controls.Add(this.numResY);
			this.Controls.Add(this.numResX);
			this.Controls.Add(this.checkSettings);
			this.Controls.Add(this.btnStartSearch);
			this.Controls.Add(this.checkAutoReInject);
			this.Controls.Add(this.checkAutoInject);
			this.Controls.Add(this.checkFilerList);
			this.Controls.Add(this.cbxProcessList);
			this.Controls.Add(this.numFPSLimit);
			this.Controls.Add(this.lblFPS);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.openImage);
			this.Controls.Add(this.preview);
			this.Controls.Add(this.checkFPSLimit);
			this.Controls.Add(this.checkUseDot);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cobCHT);
			this.Controls.Add(this.lblInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "UltraHook";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			((System.ComponentModel.ISupportInitialize)(this.numFPSLimit)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numResX)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numResY)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.ComboBox cobCHT;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkUseDot;
		private System.Windows.Forms.CheckBox checkFPSLimit;
		private System.Windows.Forms.Panel preview;
		private System.Windows.Forms.Button openImage;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblFPS;
		private System.Windows.Forms.NumericUpDown numFPSLimit;
		private System.Windows.Forms.Timer resizer;
		private System.Windows.Forms.ComboBox cbxProcessList;
		private System.Windows.Forms.CheckBox checkFilerList;
		private System.Windows.Forms.CheckBox checkAutoInject;
		private System.Windows.Forms.CheckBox checkAutoReInject;
		private System.Windows.Forms.Button btnStartSearch;
		private System.Windows.Forms.CheckBox checkSettings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numResX;
		private System.Windows.Forms.NumericUpDown numResY;
		private MetroObjects.MTitlebar mTitlebar1;
	}
}

