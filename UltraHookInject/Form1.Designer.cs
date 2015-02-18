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
            if(disposing && (components != null))
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
			((System.ComponentModel.ISupportInitialize)(this.numFPSLimit)).BeginInit();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// lblInfo
			// 
			this.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblInfo.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfo.Location = new System.Drawing.Point(12, 9);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(320, 22);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "====>";
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
			this.cobCHT.Location = new System.Drawing.Point(103, 34);
			this.cobCHT.Name = "cobCHT";
			this.cobCHT.Size = new System.Drawing.Size(86, 21);
			this.cobCHT.TabIndex = 5;
			this.cobCHT.SelectedIndexChanged += new System.EventHandler(this.cobCHT_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Location = new System.Drawing.Point(12, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 23);
			this.label2.TabIndex = 6;
			this.label2.Text = "Crosshair style:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkUseDot
			// 
			this.checkUseDot.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkUseDot.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkUseDot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkUseDot.Location = new System.Drawing.Point(195, 34);
			this.checkUseDot.Name = "checkUseDot";
			this.checkUseDot.Size = new System.Drawing.Size(53, 23);
			this.checkUseDot.TabIndex = 9;
			this.checkUseDot.Text = "use dot";
			this.checkUseDot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkUseDot.UseVisualStyleBackColor = true;
			this.checkUseDot.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// checkFPSLimit
			// 
			this.checkFPSLimit.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkFPSLimit.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkFPSLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkFPSLimit.Location = new System.Drawing.Point(254, 63);
			this.checkFPSLimit.Name = "checkFPSLimit";
			this.checkFPSLimit.Size = new System.Drawing.Size(78, 22);
			this.checkFPSLimit.TabIndex = 10;
			this.checkFPSLimit.Text = "limit FPS";
			this.checkFPSLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkFPSLimit.UseVisualStyleBackColor = true;
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
			this.openImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.openImage.Location = new System.Drawing.Point(254, 34);
			this.openImage.Name = "openImage";
			this.openImage.Size = new System.Drawing.Size(78, 23);
			this.openImage.TabIndex = 12;
			this.openImage.Text = "Open Image";
			this.openImage.UseVisualStyleBackColor = true;
			this.openImage.Click += new System.EventHandler(this.openImage_Click);
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(12, 63);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 22);
			this.label3.TabIndex = 13;
			this.label3.Text = "FPS Reader:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblFPS
			// 
			this.lblFPS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblFPS.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFPS.Location = new System.Drawing.Point(104, 63);
			this.lblFPS.Name = "lblFPS";
			this.lblFPS.Size = new System.Drawing.Size(85, 22);
			this.lblFPS.TabIndex = 14;
			this.lblFPS.Text = "--===--";
			// 
			// numFPSLimit
			// 
			this.numFPSLimit.Location = new System.Drawing.Point(195, 63);
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
			this.numFPSLimit.Size = new System.Drawing.Size(53, 20);
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(341, 94);
			this.Controls.Add(this.numFPSLimit);
			this.Controls.Add(this.lblFPS);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.openImage);
			this.Controls.Add(this.preview);
			this.Controls.Add(this.checkFPSLimit);
			this.Controls.Add(this.checkUseDot);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cobCHT);
			this.Controls.Add(this.lblInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Form1";
			this.Text = "UltraHook [Close this window to disable the hook]";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.numFPSLimit)).EndInit();
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
    }
}

