namespace OS
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DGRequsts = new System.Windows.Forms.DataGridView();
            this.Процесс = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Заявка = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.PagesInMemory = new System.Windows.Forms.DataGridView();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionPages = new System.Windows.Forms.DataGridView();
            this.DescriptionTable = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.Catalog = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.CellsArray = new System.Windows.Forms.DataGridView();
            this.RunStepBtn = new System.Windows.Forms.Button();
            this.EditorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGRequsts)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PagesInMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionPages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTable)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Catalog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CellsArray)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.59472F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.40528F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.52137F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.47863F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1320, 775);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DGRequsts);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(741, 362);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Заявки";
            // 
            // DGRequsts
            // 
            this.DGRequsts.AllowUserToAddRows = false;
            this.DGRequsts.AllowUserToDeleteRows = false;
            this.DGRequsts.AllowUserToResizeColumns = false;
            this.DGRequsts.AllowUserToResizeRows = false;
            this.DGRequsts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGRequsts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Процесс,
            this.Заявка,
            this.Column13});
            this.DGRequsts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGRequsts.Location = new System.Drawing.Point(3, 16);
            this.DGRequsts.MultiSelect = false;
            this.DGRequsts.Name = "DGRequsts";
            this.DGRequsts.ReadOnly = true;
            this.DGRequsts.RowHeadersVisible = false;
            this.DGRequsts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGRequsts.Size = new System.Drawing.Size(735, 343);
            this.DGRequsts.TabIndex = 5;
            this.DGRequsts.SelectionChanged += new System.EventHandler(this.DGRequsts_SelectionChanged);
            // 
            // Процесс
            // 
            this.Процесс.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Процесс.HeaderText = "Владелец";
            this.Процесс.Name = "Процесс";
            this.Процесс.ReadOnly = true;
            this.Процесс.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Процесс.Width = 62;
            // 
            // Заявка
            // 
            this.Заявка.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Заявка.HeaderText = "Заявка";
            this.Заявка.Name = "Заявка";
            this.Заявка.ReadOnly = true;
            this.Заявка.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column13
            // 
            this.Column13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column13.HeaderText = "Скопировано";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column13.Width = 80;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 371);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(741, 401);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Основная память";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.90476F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.20408F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.7551F));
            this.tableLayoutPanel3.Controls.Add(this.PagesInMemory, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.DescriptionPages, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.DescriptionTable, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(735, 382);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // PagesInMemory
            // 
            this.PagesInMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PagesInMemory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PagesInMemory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12});
            this.PagesInMemory.GridColor = System.Drawing.Color.DimGray;
            this.PagesInMemory.Location = new System.Drawing.Point(533, 23);
            this.PagesInMemory.Name = "PagesInMemory";
            this.PagesInMemory.RowHeadersVisible = false;
            this.PagesInMemory.Size = new System.Drawing.Size(199, 356);
            this.PagesInMemory.TabIndex = 2;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column8.HeaderText = "Адрес";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column9.HeaderText = "0";
            this.Column9.Name = "Column9";
            this.Column9.Width = 38;
            // 
            // Column10
            // 
            this.Column10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column10.HeaderText = "1";
            this.Column10.Name = "Column10";
            this.Column10.Width = 38;
            // 
            // Column11
            // 
            this.Column11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column11.HeaderText = "2";
            this.Column11.Name = "Column11";
            this.Column11.Width = 38;
            // 
            // Column12
            // 
            this.Column12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column12.HeaderText = "3";
            this.Column12.Name = "Column12";
            this.Column12.Width = 38;
            // 
            // DescriptionPages
            // 
            this.DescriptionPages.AllowUserToAddRows = false;
            this.DescriptionPages.AllowUserToDeleteRows = false;
            this.DescriptionPages.AllowUserToResizeColumns = false;
            this.DescriptionPages.AllowUserToResizeRows = false;
            this.DescriptionPages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionPages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DescriptionPages.GridColor = System.Drawing.Color.DimGray;
            this.DescriptionPages.Location = new System.Drawing.Point(164, 23);
            this.DescriptionPages.MultiSelect = false;
            this.DescriptionPages.Name = "DescriptionPages";
            this.DescriptionPages.RowHeadersVisible = false;
            this.DescriptionPages.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.DescriptionPages.Size = new System.Drawing.Size(363, 356);
            this.DescriptionPages.TabIndex = 1;
            // 
            // DescriptionTable
            // 
            this.DescriptionTable.AllowUserToAddRows = false;
            this.DescriptionTable.AllowUserToDeleteRows = false;
            this.DescriptionTable.AllowUserToResizeColumns = false;
            this.DescriptionTable.AllowUserToResizeRows = false;
            this.DescriptionTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DescriptionTable.GridColor = System.Drawing.Color.DimGray;
            this.DescriptionTable.Location = new System.Drawing.Point(3, 23);
            this.DescriptionTable.Name = "DescriptionTable";
            this.DescriptionTable.RowHeadersVisible = false;
            this.DescriptionTable.Size = new System.Drawing.Size(155, 356);
            this.DescriptionTable.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Дескрипторы таблиц:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(164, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Дескрипторы страниц:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(533, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Основная память:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.RunStepBtn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.EditorPanel, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(750, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(567, 769);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 168);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(561, 598);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Файловая система";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.95009F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.04991F));
            this.tableLayoutPanel4.Controls.Add(this.Catalog, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.CellsArray, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(555, 579);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // Catalog
            // 
            this.Catalog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Catalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Catalog.Location = new System.Drawing.Point(3, 23);
            this.Catalog.Name = "Catalog";
            this.Catalog.Size = new System.Drawing.Size(260, 553);
            this.Catalog.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Каталог:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(269, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "ВЗУ:";
            // 
            // CellsArray
            // 
            this.CellsArray.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CellsArray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CellsArray.Location = new System.Drawing.Point(269, 23);
            this.CellsArray.Name = "CellsArray";
            this.CellsArray.Size = new System.Drawing.Size(283, 553);
            this.CellsArray.TabIndex = 4;
            // 
            // RunStepBtn
            // 
            this.RunStepBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunStepBtn.Location = new System.Drawing.Point(3, 3);
            this.RunStepBtn.Name = "RunStepBtn";
            this.RunStepBtn.Size = new System.Drawing.Size(561, 34);
            this.RunStepBtn.TabIndex = 0;
            this.RunStepBtn.Text = "Выполнить такт";
            this.RunStepBtn.UseVisualStyleBackColor = true;
            this.RunStepBtn.Click += new System.EventHandler(this.RunStepBtn_Click);
            // 
            // EditorPanel
            // 
            this.EditorPanel.ColumnCount = 3;
            this.EditorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.EditorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.EditorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.EditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditorPanel.Location = new System.Drawing.Point(0, 40);
            this.EditorPanel.Margin = new System.Windows.Forms.Padding(0);
            this.EditorPanel.Name = "EditorPanel";
            this.EditorPanel.RowCount = 4;
            this.EditorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EditorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EditorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EditorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.EditorPanel.Size = new System.Drawing.Size(567, 125);
            this.EditorPanel.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 775);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Курсовой проект ОС";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGRequsts)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PagesInMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionPages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTable)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Catalog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CellsArray)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button RunStepBtn;
        private System.Windows.Forms.DataGridView PagesInMemory;
        private System.Windows.Forms.DataGridView DescriptionPages;
        private System.Windows.Forms.DataGridView DescriptionTable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView DGRequsts;
        private System.Windows.Forms.DataGridViewTextBoxColumn Процесс;
        private System.Windows.Forms.DataGridViewTextBoxColumn Заявка;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.DataGridView Catalog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView CellsArray;
        private System.Windows.Forms.TableLayoutPanel EditorPanel;


    }
}

