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
            this.DGRequsts = new System.Windows.Forms.DataGridView();
            this.Процесс = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Заявка = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PagesInMemory = new System.Windows.Forms.DataGridView();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionPages = new System.Windows.Forms.DataGridView();
            this.DescriptionTable = new System.Windows.Forms.DataGridView();
            this.Catalog = new System.Windows.Forms.DataGridView();
            this.CellsArray = new System.Windows.Forms.DataGridView();
            this.RunStepBtn = new System.Windows.Forms.Button();
            this.EditRequestBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGRequsts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PagesInMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionPages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Catalog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CellsArray)).BeginInit();
            this.SuspendLayout();
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
            this.DGRequsts.Location = new System.Drawing.Point(142, 37);
            this.DGRequsts.MultiSelect = false;
            this.DGRequsts.Name = "DGRequsts";
            this.DGRequsts.ReadOnly = true;
            this.DGRequsts.RowHeadersVisible = false;
            this.DGRequsts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGRequsts.Size = new System.Drawing.Size(660, 246);
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
            // PagesInMemory
            // 
            this.PagesInMemory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PagesInMemory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12});
            this.PagesInMemory.GridColor = System.Drawing.Color.DimGray;
            this.PagesInMemory.Location = new System.Drawing.Point(583, 318);
            this.PagesInMemory.Name = "PagesInMemory";
            this.PagesInMemory.RowHeadersVisible = false;
            this.PagesInMemory.Size = new System.Drawing.Size(219, 262);
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
            this.DescriptionPages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DescriptionPages.GridColor = System.Drawing.Color.DimGray;
            this.DescriptionPages.Location = new System.Drawing.Point(212, 318);
            this.DescriptionPages.MultiSelect = false;
            this.DescriptionPages.Name = "DescriptionPages";
            this.DescriptionPages.RowHeadersVisible = false;
            this.DescriptionPages.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.DescriptionPages.Size = new System.Drawing.Size(365, 262);
            this.DescriptionPages.TabIndex = 1;
            // 
            // DescriptionTable
            // 
            this.DescriptionTable.AllowUserToAddRows = false;
            this.DescriptionTable.AllowUserToDeleteRows = false;
            this.DescriptionTable.AllowUserToResizeColumns = false;
            this.DescriptionTable.AllowUserToResizeRows = false;
            this.DescriptionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DescriptionTable.GridColor = System.Drawing.Color.DimGray;
            this.DescriptionTable.Location = new System.Drawing.Point(14, 319);
            this.DescriptionTable.Name = "DescriptionTable";
            this.DescriptionTable.RowHeadersVisible = false;
            this.DescriptionTable.Size = new System.Drawing.Size(188, 261);
            this.DescriptionTable.TabIndex = 0;
            // 
            // Catalog
            // 
            this.Catalog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Catalog.Location = new System.Drawing.Point(809, 37);
            this.Catalog.Name = "Catalog";
            this.Catalog.Size = new System.Drawing.Size(282, 86);
            this.Catalog.TabIndex = 0;
            // 
            // CellsArray
            // 
            this.CellsArray.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CellsArray.Location = new System.Drawing.Point(809, 167);
            this.CellsArray.Name = "CellsArray";
            this.CellsArray.Size = new System.Drawing.Size(282, 413);
            this.CellsArray.TabIndex = 4;
            // 
            // RunStepBtn
            // 
            this.RunStepBtn.Location = new System.Drawing.Point(14, 37);
            this.RunStepBtn.Name = "RunStepBtn";
            this.RunStepBtn.Size = new System.Drawing.Size(113, 34);
            this.RunStepBtn.TabIndex = 0;
            this.RunStepBtn.Text = "Выполнить такт";
            this.RunStepBtn.UseVisualStyleBackColor = true;
            this.RunStepBtn.Click += new System.EventHandler(this.RunStepBtn_Click);
            // 
            // EditRequestBtn
            // 
            this.EditRequestBtn.Location = new System.Drawing.Point(14, 77);
            this.EditRequestBtn.Name = "EditRequestBtn";
            this.EditRequestBtn.Size = new System.Drawing.Size(113, 34);
            this.EditRequestBtn.TabIndex = 1;
            this.EditRequestBtn.Text = "Редактировать заявку";
            this.EditRequestBtn.UseVisualStyleBackColor = true;
            this.EditRequestBtn.Click += new System.EventHandler(this.EditRequestBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(143, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Диспетчеризация процессов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(15, 292);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Дескрипторы таблиц";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(208, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Дескрипторы страниц";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(579, 292);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "Страницы";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(805, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "Каталог";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(808, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 24);
            this.label6.TabIndex = 11;
            this.label6.Text = "ВЗУ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(423, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "label7";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 595);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DescriptionTable);
            this.Controls.Add(this.DescriptionPages);
            this.Controls.Add(this.DGRequsts);
            this.Controls.Add(this.Catalog);
            this.Controls.Add(this.EditRequestBtn);
            this.Controls.Add(this.RunStepBtn);
            this.Controls.Add(this.PagesInMemory);
            this.Controls.Add(this.CellsArray);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Курсовой проект";
            ((System.ComponentModel.ISupportInitialize)(this.DGRequsts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PagesInMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionPages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Catalog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CellsArray)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RunStepBtn;
        private System.Windows.Forms.DataGridView PagesInMemory;
        private System.Windows.Forms.DataGridView DescriptionPages;
        private System.Windows.Forms.DataGridView DescriptionTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridView DGRequsts;
        private System.Windows.Forms.DataGridViewTextBoxColumn Процесс;
        private System.Windows.Forms.DataGridViewTextBoxColumn Заявка;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridView Catalog;
        private System.Windows.Forms.DataGridView CellsArray;
        private System.Windows.Forms.Button EditRequestBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;


    }
}

