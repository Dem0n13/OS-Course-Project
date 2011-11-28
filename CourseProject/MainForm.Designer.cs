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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.DGRequsts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PagesInMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionPages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Catalog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CellsArray)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGRequsts
            // 
            this.DGRequsts.AllowUserToAddRows = false;
            this.DGRequsts.AllowUserToDeleteRows = false;
            this.DGRequsts.AllowUserToResizeColumns = false;
            this.DGRequsts.AllowUserToResizeRows = false;
            this.DGRequsts.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.DGRequsts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGRequsts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Процесс,
            this.Заявка,
            this.Column13});
            this.DGRequsts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGRequsts.Location = new System.Drawing.Point(203, 24);
            this.DGRequsts.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DGRequsts.MultiSelect = false;
            this.DGRequsts.Name = "DGRequsts";
            this.DGRequsts.ReadOnly = true;
            this.DGRequsts.RowHeadersVisible = false;
            this.DGRequsts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGRequsts.Size = new System.Drawing.Size(562, 339);
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
            this.Процесс.Width = 70;
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
            this.Column13.Width = 95;
            // 
            // PagesInMemory
            // 
            this.PagesInMemory.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.PagesInMemory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PagesInMemory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12});
            this.PagesInMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PagesInMemory.GridColor = System.Drawing.Color.DimGray;
            this.PagesInMemory.Location = new System.Drawing.Point(3, 24);
            this.PagesInMemory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PagesInMemory.Name = "PagesInMemory";
            this.PagesInMemory.RowHeadersVisible = false;
            this.PagesInMemory.Size = new System.Drawing.Size(194, 339);
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
            this.Column9.Width = 40;
            // 
            // Column10
            // 
            this.Column10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column10.HeaderText = "1";
            this.Column10.Name = "Column10";
            this.Column10.Width = 40;
            // 
            // Column11
            // 
            this.Column11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column11.HeaderText = "2";
            this.Column11.Name = "Column11";
            this.Column11.Width = 40;
            // 
            // Column12
            // 
            this.Column12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column12.HeaderText = "3";
            this.Column12.Name = "Column12";
            this.Column12.Width = 40;
            // 
            // DescriptionPages
            // 
            this.DescriptionPages.AllowUserToAddRows = false;
            this.DescriptionPages.AllowUserToDeleteRows = false;
            this.DescriptionPages.AllowUserToResizeColumns = false;
            this.DescriptionPages.AllowUserToResizeRows = false;
            this.DescriptionPages.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.DescriptionPages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DescriptionPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionPages.GridColor = System.Drawing.Color.DimGray;
            this.DescriptionPages.Location = new System.Drawing.Point(203, 431);
            this.DescriptionPages.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DescriptionPages.MultiSelect = false;
            this.DescriptionPages.Name = "DescriptionPages";
            this.DescriptionPages.RowHeadersVisible = false;
            this.DescriptionPages.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.DescriptionPages.Size = new System.Drawing.Size(562, 339);
            this.DescriptionPages.TabIndex = 1;
            // 
            // DescriptionTable
            // 
            this.DescriptionTable.AllowUserToAddRows = false;
            this.DescriptionTable.AllowUserToDeleteRows = false;
            this.DescriptionTable.AllowUserToResizeColumns = false;
            this.DescriptionTable.AllowUserToResizeRows = false;
            this.DescriptionTable.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.DescriptionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DescriptionTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionTable.GridColor = System.Drawing.Color.DimGray;
            this.DescriptionTable.Location = new System.Drawing.Point(3, 431);
            this.DescriptionTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DescriptionTable.Name = "DescriptionTable";
            this.DescriptionTable.RowHeadersVisible = false;
            this.DescriptionTable.Size = new System.Drawing.Size(194, 339);
            this.DescriptionTable.TabIndex = 0;
            // 
            // Catalog
            // 
            this.Catalog.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.Catalog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Catalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Catalog.Location = new System.Drawing.Point(771, 431);
            this.Catalog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Catalog.Name = "Catalog";
            this.Catalog.Size = new System.Drawing.Size(444, 339);
            this.Catalog.TabIndex = 0;
            // 
            // CellsArray
            // 
            this.CellsArray.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.CellsArray.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CellsArray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CellsArray.Location = new System.Drawing.Point(771, 24);
            this.CellsArray.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CellsArray.Name = "CellsArray";
            this.CellsArray.Size = new System.Drawing.Size(444, 339);
            this.CellsArray.TabIndex = 4;
            // 
            // RunStepBtn
            // 
            this.RunStepBtn.BackColor = System.Drawing.Color.Gainsboro;
            this.tableLayoutPanel1.SetColumnSpan(this.RunStepBtn, 2);
            this.RunStepBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunStepBtn.Location = new System.Drawing.Point(3, 371);
            this.RunStepBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RunStepBtn.Name = "RunStepBtn";
            this.RunStepBtn.Size = new System.Drawing.Size(762, 32);
            this.RunStepBtn.TabIndex = 0;
            this.RunStepBtn.Text = "Следующая заявка";
            this.RunStepBtn.UseVisualStyleBackColor = false;
            this.RunStepBtn.Click += new System.EventHandler(this.RunStepBtn_Click);
            // 
            // EditRequestBtn
            // 
            this.EditRequestBtn.Location = new System.Drawing.Point(1382, 268);
            this.EditRequestBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EditRequestBtn.Name = "EditRequestBtn";
            this.EditRequestBtn.Size = new System.Drawing.Size(132, 44);
            this.EditRequestBtn.TabIndex = 1;
            this.EditRequestBtn.Text = "Редактировать заявку";
            this.EditRequestBtn.UseVisualStyleBackColor = true;
            this.EditRequestBtn.Click += new System.EventHandler(this.EditRequestBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(203, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(562, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Заявки процессов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 407);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Дескрипторы таблиц";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 407);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Дескрипторы страниц";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Основная память";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(771, 407);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Каталог";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(771, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Файловые блоки";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(771, 367);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(444, 40);
            this.label7.TabIndex = 12;
            this.label7.Text = "label7";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tableLayoutPanel1.Controls.Add(this.RunStepBtn, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.DGRequsts, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Catalog, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.DescriptionTable, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.CellsArray, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.PagesInMemory, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.DescriptionPages, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1218, 774);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1218, 774);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.EditRequestBtn);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Курсовой проект Степанова Максима";
            ((System.ComponentModel.ISupportInitialize)(this.DGRequsts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PagesInMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionPages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Catalog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CellsArray)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;


    }
}

