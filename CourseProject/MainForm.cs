using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OS
{
    public partial class MainForm : Form
    {
        #region Ручная инициализация некоторых компонентов
        private Button EditBtn = new Button();
#if UI_REQUEST_EDITOR_ON_MAIN_FORM
        private System.Windows.Forms.ComboBox RequestTypeBox = new ComboBox();
        private System.Windows.Forms.NumericUpDown SUpDn2 = new NumericUpDown();
        private System.Windows.Forms.NumericUpDown DUpDn2 = new NumericUpDown();
        private System.Windows.Forms.NumericUpDown DUpDn1 = new NumericUpDown();
        private System.Windows.Forms.NumericUpDown SUpDn1 = new NumericUpDown();
        private System.Windows.Forms.ComboBox SBox = new ComboBox();
        private System.Windows.Forms.ComboBox DBox = new ComboBox();
        private System.Windows.Forms.Label label1 = new Label();
#endif
        #endregion

        public MainForm()
        {
            InitializeComponent();

            // инициализация редактора заявок
            EditBtn = new Button();
            EditBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            EditBtn.Text = "Изменить заявку";
            EditBtn.Click += new System.EventHandler(EditBtn_Click);
#if UI_REQUEST_EDITOR_ON_EDITOR_FORM
            tableLayoutPanel2.RowStyles[1].Height = 40;
            EditorPanel.SetColumnSpan(EditBtn, EditorPanel.ColumnCount);
            EditorPanel.SetRowSpan(EditBtn, EditorPanel.RowCount);
            EditorPanel.Controls.Add(EditBtn);
            VisualAdapter.EditRequestBtn = this.EditBtn;
#endif
#if UI_REQUEST_EDITOR_ON_MAIN_FORM
            this.EditorPanel.SetColumnSpan(this.EditBtn, 3);
            this.EditorPanel.Controls.Add(this.EditBtn, 0, 3);
            
            this.RequestTypeBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.RequestTypeBox.SelectedIndexChanged += new System.EventHandler(this.RequestTypeBox_SelectedIndexChanged);
            this.RequestTypeBox.Items.AddRange(new object[] {
            "Память > Память",
            "Память > ВЗУ",
            "ВЗУ > Память",
            "Обработка данных"});
            this.EditorPanel.Controls.Add(this.RequestTypeBox, 1, 0);

            this.SUpDn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.SUpDn1.ValueChanged += this.RequestTypeBox_SelectedIndexChanged;
            this.EditorPanel.Controls.Add(this.SUpDn1, 0, 1);

            this.DUpDn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.DUpDn1.ValueChanged += this.RequestTypeBox_SelectedIndexChanged;
            this.EditorPanel.Controls.Add(this.DUpDn1, 2, 1);

            this.SUpDn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.EditorPanel.Controls.Add(this.SUpDn2, 0, 2);

            this.DUpDn2.Dock = System.Windows.Forms.DockStyle.Top;
            this.EditorPanel.Controls.Add(this.DUpDn2, 2, 2);

            this.SBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.SBox.Visible = false;
            this.EditorPanel.Controls.Add(this.SBox, 0, 1);

            this.DBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.DBox.Visible = false;
            this.EditorPanel.Controls.Add(this.DBox, 2, 1);

            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditorPanel.SetRowSpan(this.label1, 2);
            this.label1.Text = ">>";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EditorPanel.Controls.Add(this.label1, 1, 1);

            VisualAdapter.EditorPanel = this.EditorPanel;
            VisualAdapter.RequestTypeBox = this.RequestTypeBox;
            VisualAdapter.SUpDn2 = this.SUpDn2;
            VisualAdapter.DUpDn2 = this.DUpDn2;
            VisualAdapter.DUpDn1 = this.DUpDn1;
            VisualAdapter.SUpDn1 = this.SUpDn1;
            VisualAdapter.SBox = this.SBox;
            VisualAdapter.DBox = this.DBox;
#endif
            VisualAdapter.DGDescriptionTable = this.DescriptionTable;
            VisualAdapter.DGDescriptionPages = this.DescriptionPages;
            VisualAdapter.DGPagesInMemory = this.PagesInMemory;
            VisualAdapter.DGCatalog = this.Catalog;
            VisualAdapter.DGCellsArray = this.CellsArray;
            VisualAdapter.DGRequests = this.DGRequsts;
            VisualAdapter.Init();
        }

        private void RunStepBtn_Click(object sender, EventArgs e)
        {
            TaskManager.ResumeProcess();
            VisualAdapter.RefreshRequests();
            VisualAdapter.RefreshMemoryState();
            VisualAdapter.RefreshHDDState();
        }
        //123
#if UI_REQUEST_EDITOR_ON_EDITOR_FORM
        private void EditBtn_Click(object sender, EventArgs e)
        {
            // получаем номер процесса и номер заявки
            int i_process = 0;
            int i_request = 0;
            int k = 0;
            for (i_process = 0; i_process < GlobalConsts.ProcessesCount; i_process++)
            {
                for (i_request = 0; i_request < TaskManager.Processes[i_process].Requests.Length; i_request++)
                {
                    if (k == DGRequsts.SelectedRows[0].Index)
                    {
                        if ((new RequestEditor(i_process, i_request)).ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            VisualAdapter.RefreshRequests();
                        return;
                    }
                    k++;
                }
            }
        }
#endif
#if UI_REQUEST_EDITOR_ON_MAIN_FORM
        private void EditBtn_Click(object sender, EventArgs e)
        {
            // запись измененной заявки
            VisualAdapter.Request.Type = (RequestTypes)RequestTypeBox.SelectedIndex;
            switch (VisualAdapter.Request.Type)
            {
                case RequestTypes.MemoryToMemory:
                    VisualAdapter.Request.FromTable = (int)SUpDn1.Value;
                    VisualAdapter.Request.FromDescriptor = (int)SUpDn2.Value;
                    VisualAdapter.Request.ToTable = (int)DUpDn1.Value;
                    VisualAdapter.Request.ToDescriptor = (int)DUpDn2.Value;
                    break;

                case RequestTypes.MemoryToHDD:
                    VisualAdapter.Request.FromTable = (int)SUpDn1.Value;
                    VisualAdapter.Request.FromDescriptor = (int)SUpDn2.Value;
                    VisualAdapter.Request.ToFile = DBox.Text;
                    VisualAdapter.Request.FileBlockNum = (int)DUpDn2.Value;
                    break;

                case RequestTypes.HDDToMemory:
                    VisualAdapter.Request.FromFile = SBox.Text;
                    VisualAdapter.Request.FileBlockNum = (int)SUpDn2.Value;
                    VisualAdapter.Request.ToTable = (int)DUpDn1.Value;
                    VisualAdapter.Request.ToDescriptor = (int)DUpDn2.Value;
                    break;
                case RequestTypes.Action:
                    VisualAdapter.Request.FromTable = (int)SUpDn1.Value;
                    VisualAdapter.Request.FromDescriptor = (int)SUpDn2.Value;
                    break;
            }
            VisualAdapter.RefreshRequests();
        }

        private void RequestTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisualAdapter.RefreshEditorEditMode();
        }   
#endif

        private void DGRequsts_SelectionChanged(object sender, EventArgs e)
        {
            VisualAdapter.RefreshRequests();
        }
    }
}
