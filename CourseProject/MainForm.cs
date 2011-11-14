using System;
using System.Windows.Forms;

namespace OS
{
    public partial class MainForm : Form
    {
        private Button EditBtn = new Button();

        public MainForm()
        {
            InitializeComponent();

            // инициализация редактора заявок
            EditBtn = new Button();
            EditBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            EditBtn.Text = "Изменить заявку";
            EditBtn.Click += new System.EventHandler(EditBtn_Click);
            tableLayoutPanel2.RowStyles[1].Height = 40;
            EditorPanel.SetColumnSpan(EditBtn, EditorPanel.ColumnCount);
            EditorPanel.SetRowSpan(EditBtn, EditorPanel.RowCount);
            EditorPanel.Controls.Add(EditBtn);
            VisualAdapter.EditRequestBtn = this.EditBtn;
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

        private void DGRequsts_SelectionChanged(object sender, EventArgs e)
        {
            VisualAdapter.RefreshRequests();
        }
    }
}
