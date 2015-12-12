namespace QRCoderDemo
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
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.textBoxQRCode = new System.Windows.Forms.TextBox();
            this.pictureBoxQRCode = new System.Windows.Forms.PictureBox();
            this.comboBoxECC = new System.Windows.Forms.ComboBox();
            this.labelECC = new System.Windows.Forms.Label();
            this.labelIcon = new System.Windows.Forms.Label();
            this.iconPath = new System.Windows.Forms.TextBox();
            this.selectIconBtn = new System.Windows.Forms.Button();
            this.labelIconsize = new System.Windows.Forms.Label();
            this.iconSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconSize)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerate.Location = new System.Drawing.Point(228, 23);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(62, 23);
            this.buttonGenerate.TabIndex = 0;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // textBoxQRCode
            // 
            this.textBoxQRCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxQRCode.Location = new System.Drawing.Point(40, 23);
            this.textBoxQRCode.Name = "textBoxQRCode";
            this.textBoxQRCode.Size = new System.Drawing.Size(182, 20);
            this.textBoxQRCode.TabIndex = 1;
            this.textBoxQRCode.Text = "www.code-bude.net";
            // 
            // pictureBoxQRCode
            // 
            this.pictureBoxQRCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxQRCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxQRCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxQRCode.Location = new System.Drawing.Point(40, 141);
            this.pictureBoxQRCode.Name = "pictureBoxQRCode";
            this.pictureBoxQRCode.Size = new System.Drawing.Size(250, 260);
            this.pictureBoxQRCode.TabIndex = 2;
            this.pictureBoxQRCode.TabStop = false;
            // 
            // comboBoxECC
            // 
            this.comboBoxECC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxECC.FormattingEnabled = true;
            this.comboBoxECC.Items.AddRange(new object[] {
            "L",
            "M",
            "Q",
            "H"});
            this.comboBoxECC.Location = new System.Drawing.Point(97, 49);
            this.comboBoxECC.Name = "comboBoxECC";
            this.comboBoxECC.Size = new System.Drawing.Size(125, 21);
            this.comboBoxECC.TabIndex = 3;
            // 
            // labelECC
            // 
            this.labelECC.AutoSize = true;
            this.labelECC.Location = new System.Drawing.Point(37, 52);
            this.labelECC.Name = "labelECC";
            this.labelECC.Size = new System.Drawing.Size(57, 13);
            this.labelECC.TabIndex = 4;
            this.labelECC.Text = "ECC-Level";
            // 
            // labelIcon
            // 
            this.labelIcon.AutoSize = true;
            this.labelIcon.Location = new System.Drawing.Point(37, 85);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(31, 13);
            this.labelIcon.TabIndex = 5;
            this.labelIcon.Text = "Icon:";
            // 
            // iconPath
            // 
            this.iconPath.Location = new System.Drawing.Point(97, 79);
            this.iconPath.Name = "iconPath";
            this.iconPath.Size = new System.Drawing.Size(125, 20);
            this.iconPath.TabIndex = 6;
            // 
            // selectIconBtn
            // 
            this.selectIconBtn.Location = new System.Drawing.Point(229, 79);
            this.selectIconBtn.Name = "selectIconBtn";
            this.selectIconBtn.Size = new System.Drawing.Size(61, 25);
            this.selectIconBtn.TabIndex = 7;
            this.selectIconBtn.Text = "Select";
            this.selectIconBtn.UseVisualStyleBackColor = true;
            this.selectIconBtn.Click += new System.EventHandler(this.selectIconBtn_Click);
            // 
            // labelIconsize
            // 
            this.labelIconsize.AutoSize = true;
            this.labelIconsize.Location = new System.Drawing.Point(37, 118);
            this.labelIconsize.Name = "labelIconsize";
            this.labelIconsize.Size = new System.Drawing.Size(52, 13);
            this.labelIconsize.TabIndex = 8;
            this.labelIconsize.Text = "Icon size:";
            // 
            // iconSize
            // 
            this.iconSize.Location = new System.Drawing.Point(97, 114);
            this.iconSize.Name = "iconSize";
            this.iconSize.Size = new System.Drawing.Size(125, 20);
            this.iconSize.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 418);
            this.Controls.Add(this.iconSize);
            this.Controls.Add(this.labelIconsize);
            this.Controls.Add(this.selectIconBtn);
            this.Controls.Add(this.iconPath);
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.labelECC);
            this.Controls.Add(this.comboBoxECC);
            this.Controls.Add(this.pictureBoxQRCode);
            this.Controls.Add(this.textBoxQRCode);
            this.Controls.Add(this.buttonGenerate);
            this.MinimumSize = new System.Drawing.Size(347, 361);
            this.Name = "Form1";
            this.Text = "QRCoder Demo - www.code-bude.net";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.TextBox textBoxQRCode;
        private System.Windows.Forms.PictureBox pictureBoxQRCode;
        private System.Windows.Forms.ComboBox comboBoxECC;
        private System.Windows.Forms.Label labelECC;
        private System.Windows.Forms.Label labelIcon;
        private System.Windows.Forms.TextBox iconPath;
        private System.Windows.Forms.Button selectIconBtn;
        private System.Windows.Forms.Label labelIconsize;
        private System.Windows.Forms.NumericUpDown iconSize;
    }
}

