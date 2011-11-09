namespace OS
{
    partial class RequestEditor
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
            this.RequestTypeBox = new System.Windows.Forms.ComboBox();
            this.S1 = new System.Windows.Forms.Label();
            this.S2 = new System.Windows.Forms.Label();
            this.SUpDn2 = new System.Windows.Forms.NumericUpDown();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.DUpDn2 = new System.Windows.Forms.NumericUpDown();
            this.D2 = new System.Windows.Forms.Label();
            this.D1 = new System.Windows.Forms.Label();
            this.DUpDn1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.SUpDn1 = new System.Windows.Forms.NumericUpDown();
            this.SBox = new System.Windows.Forms.ComboBox();
            this.DBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.SUpDn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DUpDn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DUpDn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUpDn1)).BeginInit();
            this.SuspendLayout();
            // 
            // RequestTypeBox
            // 
            this.RequestTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestTypeBox.FormattingEnabled = true;
            this.RequestTypeBox.Items.AddRange(new object[] {
            "Память > Память",
            "Память > ВЗУ",
            "ВЗУ > Память",
            "Обработка данных"});
            this.RequestTypeBox.Location = new System.Drawing.Point(12, 12);
            this.RequestTypeBox.Name = "RequestTypeBox";
            this.RequestTypeBox.Size = new System.Drawing.Size(233, 21);
            this.RequestTypeBox.TabIndex = 1;
            this.RequestTypeBox.SelectedIndexChanged += new System.EventHandler(this.ValueChanged);
            // 
            // S1
            // 
            this.S1.AutoSize = true;
            this.S1.Location = new System.Drawing.Point(13, 40);
            this.S1.Name = "S1";
            this.S1.Size = new System.Drawing.Size(35, 13);
            this.S1.TabIndex = 2;
            this.S1.Text = "label1";
            // 
            // S2
            // 
            this.S2.AutoSize = true;
            this.S2.Location = new System.Drawing.Point(13, 83);
            this.S2.Name = "S2";
            this.S2.Size = new System.Drawing.Size(35, 13);
            this.S2.TabIndex = 3;
            this.S2.Text = "label2";
            // 
            // SUpDn2
            // 
            this.SUpDn2.Location = new System.Drawing.Point(12, 99);
            this.SUpDn2.Name = "SUpDn2";
            this.SUpDn2.Size = new System.Drawing.Size(100, 20);
            this.SUpDn2.TabIndex = 4;
            // 
            // SaveBtn
            // 
            this.SaveBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveBtn.Location = new System.Drawing.Point(170, 135);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 5;
            this.SaveBtn.Text = "ОК";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // DUpDn2
            // 
            this.DUpDn2.Location = new System.Drawing.Point(145, 99);
            this.DUpDn2.Name = "DUpDn2";
            this.DUpDn2.Size = new System.Drawing.Size(100, 20);
            this.DUpDn2.TabIndex = 9;
            // 
            // D2
            // 
            this.D2.AutoSize = true;
            this.D2.Location = new System.Drawing.Point(146, 83);
            this.D2.Name = "D2";
            this.D2.Size = new System.Drawing.Size(35, 13);
            this.D2.TabIndex = 8;
            this.D2.Text = "label3";
            // 
            // D1
            // 
            this.D1.AutoSize = true;
            this.D1.Location = new System.Drawing.Point(146, 40);
            this.D1.Name = "D1";
            this.D1.Size = new System.Drawing.Size(35, 13);
            this.D1.TabIndex = 7;
            this.D1.Text = "label4";
            // 
            // DUpDn1
            // 
            this.DUpDn1.Location = new System.Drawing.Point(145, 56);
            this.DUpDn1.Name = "DUpDn1";
            this.DUpDn1.Size = new System.Drawing.Size(100, 20);
            this.DUpDn1.TabIndex = 6;
            this.DUpDn1.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(118, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = ">";
            // 
            // SUpDn1
            // 
            this.SUpDn1.Location = new System.Drawing.Point(12, 56);
            this.SUpDn1.Name = "SUpDn1";
            this.SUpDn1.Size = new System.Drawing.Size(100, 20);
            this.SUpDn1.TabIndex = 0;
            this.SUpDn1.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // SBox
            // 
            this.SBox.FormattingEnabled = true;
            this.SBox.Location = new System.Drawing.Point(12, 56);
            this.SBox.Name = "SBox";
            this.SBox.Size = new System.Drawing.Size(100, 21);
            this.SBox.TabIndex = 11;
            // 
            // DBox
            // 
            this.DBox.FormattingEnabled = true;
            this.DBox.Location = new System.Drawing.Point(145, 56);
            this.DBox.Name = "DBox";
            this.DBox.Size = new System.Drawing.Size(100, 21);
            this.DBox.TabIndex = 12;
            // 
            // RequestEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 170);
            this.Controls.Add(this.DBox);
            this.Controls.Add(this.SBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DUpDn2);
            this.Controls.Add(this.D2);
            this.Controls.Add(this.D1);
            this.Controls.Add(this.DUpDn1);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.SUpDn2);
            this.Controls.Add(this.S2);
            this.Controls.Add(this.S1);
            this.Controls.Add(this.RequestTypeBox);
            this.Controls.Add(this.SUpDn1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RequestEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RequestEditor";
            ((System.ComponentModel.ISupportInitialize)(this.SUpDn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DUpDn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DUpDn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUpDn1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox RequestTypeBox;
        private System.Windows.Forms.Label S1;
        private System.Windows.Forms.Label S2;
        private System.Windows.Forms.NumericUpDown SUpDn2;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.NumericUpDown DUpDn2;
        private System.Windows.Forms.Label D2;
        private System.Windows.Forms.Label D1;
        private System.Windows.Forms.NumericUpDown DUpDn1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown SUpDn1;
        private System.Windows.Forms.ComboBox SBox;
        private System.Windows.Forms.ComboBox DBox;
    }
}