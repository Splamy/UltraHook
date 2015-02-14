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
			this.label1 = new System.Windows.Forms.Label();
			this.cobCHT = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.openFromImg = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(320, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "====>";
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
			this.cobCHT.Location = new System.Drawing.Point(152, 34);
			this.cobCHT.Name = "cobCHT";
			this.cobCHT.Size = new System.Drawing.Size(93, 21);
			this.cobCHT.TabIndex = 5;
			this.cobCHT.SelectedIndexChanged += new System.EventHandler(this.cobCHT_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Location = new System.Drawing.Point(12, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(134, 21);
			this.label2.TabIndex = 6;
			this.label2.Text = "Crosshair style:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBox1
			// 
			this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBox1.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBox1.Location = new System.Drawing.Point(251, 34);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(81, 21);
			this.checkBox1.TabIndex = 9;
			this.checkBox1.Text = "use dot";
			this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// openFromImg
			// 
			this.openFromImg.Appearance = System.Windows.Forms.Appearance.Button;
			this.openFromImg.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.openFromImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.openFromImg.Location = new System.Drawing.Point(152, 61);
			this.openFromImg.Name = "openFromImg";
			this.openFromImg.Size = new System.Drawing.Size(180, 24);
			this.openFromImg.TabIndex = 10;
			this.openFromImg.Text = "Generate from Image";
			this.openFromImg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.openFromImg.UseVisualStyleBackColor = true;
			this.openFromImg.CheckedChanged += new System.EventHandler(this.openFromImg_CheckedChanged);
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(152, 91);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(180, 180);
			this.panel1.TabIndex = 11;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 283);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.openFromImg);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cobCHT);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Form1";
			this.Text = "UltraHook [Close this window to disable the hook]";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cobCHT;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox openFromImg;
		private System.Windows.Forms.Panel panel1;
    }
}

