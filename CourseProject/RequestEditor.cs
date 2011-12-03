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
    public partial class RequestEditor : Form
    {
        int i_process;
        int i_request;
        Request request;

        public RequestEditor(int i_process, int i_request)
        {
            InitializeComponent();
            // получение запроса
            this.i_process = i_process;
            this.i_request = i_request;
            request = TaskManager.Processes[i_process].Requests[i_request];

            // диапазоны значений таблиц дескрипторов
            SUpDn1.Minimum = Array.IndexOf(InternalMemory.Pages, TaskManager.Processes[i_process].LogicAreas[0]);
            SUpDn1.Maximum = Array.IndexOf(InternalMemory.Pages, TaskManager.Processes[i_process].LogicAreas[TaskManager.Processes[i_process].LogicAreas.Length - 1]);
            DUpDn1.Minimum = Array.IndexOf(InternalMemory.Pages, TaskManager.Processes[i_process].LogicAreas[0]);
            DUpDn1.Maximum = Array.IndexOf(InternalMemory.Pages, TaskManager.Processes[i_process].LogicAreas[TaskManager.Processes[i_process].LogicAreas.Length - 1]);

            // заполнение начальных данных с редактируемой заявки
            RequestTypeBox.SelectedIndex = (int)request.Type;
            SBox.Items.AddRange(HDD.Catalog.ToArray<CatalogRecord>()); SBox.SelectedIndex = 0;
            DBox.Items.AddRange(HDD.Catalog.ToArray<CatalogRecord>()); DBox.SelectedIndex = 0;
            switch (request.Type)
            {
                case RequestTypes.MemoryToMemory:
                    SUpDn1.Value = request.FromTable;
                    SUpDn2.Value = request.FromDescriptor;
                    DUpDn1.Value = request.ToTable;
                    DUpDn2.Value = request.ToDescriptor;
                    break;
                case RequestTypes.MemoryToHDD:
                    SUpDn1.Value = request.FromTable;
                    SUpDn2.Value = request.FromDescriptor;
                    DBox.Text = request.ToFile;
                    DUpDn2.Value = request.FileBlockNum;
                    break;
                case RequestTypes.HDDToMemory:
                    SBox.Text = request.FromFile;
                    SUpDn2.Value = request.FileBlockNum;
                    DUpDn1.Value = request.ToTable;
                    DUpDn2.Value = request.ToDescriptor;
                    break;
                case RequestTypes.Action:
                    SUpDn1.Value = request.FromTable;
                    SUpDn2.Value = request.FromDescriptor;
                    break;
            }

            // первое обновление
            RefreshForm();
        }

        private void RefreshForm()
        {
            switch ((RequestTypes)RequestTypeBox.SelectedIndex)
            {
                case RequestTypes.MemoryToMemory:
                    // видимость форм
                    SBox.Visible = DBox.Visible = false;
                    SUpDn1.Visible = DUpDn1.Visible = DUpDn2.Visible = true;
                    // надписи
                    S1.Text = D1.Text = "Выбор таблицы";
                    S2.Text = D2.Text = "Выбор дескриптора";
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)SUpDn1.Value] - 1;
                    DUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)DUpDn1.Value] - 1;
                    break;

                case RequestTypes.MemoryToHDD:
                    // видимость форм
                    SBox.Visible = DUpDn1.Visible = false;
                    SUpDn1.Visible = DBox.Visible = DUpDn2.Visible = true;
                    // надписи
                    S1.Text  = "Выбор таблицы";
                    S2.Text = "Выбор дескриптора";
                    D1.Text = "Выбор файла";
                    D2.Text = "Выбор ФБ";
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)SUpDn1.Value] - 1;
                    DUpDn2.Maximum = GlobalConsts.HDDCellsCount;
                    break;

                case RequestTypes.HDDToMemory:
                    // видимость форм
                    SUpDn1.Visible = DBox.Visible = false;
                    SBox.Visible = DUpDn1.Visible = DUpDn2.Visible = true;
                    // надписи
                    S1.Text = "Выбор файла";
                    S2.Text = "Выбор ФБ";
                    D1.Text  = "Выбор таблицы";
                    D2.Text = "Выбор дескриптора";
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.HDDCellsCount;
                    DUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)DUpDn1.Value] - 1;
                    break;
                case RequestTypes.Action:
                    // видимость форм
                    SBox.Visible = DUpDn1.Visible = DBox.Visible = DUpDn2.Visible = false;
                    SUpDn1.Visible = true;
                    // надписи
                    S1.Text = "Выбор таблицы";
                    S2.Text = "Выбор дескриптора";
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)SUpDn1.Value] - 1;
                    break;
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            // запись измененной заявки
            request.Type = (RequestTypes)RequestTypeBox.SelectedIndex;
            switch (request.Type)
            {
                case RequestTypes.MemoryToMemory:
                    request.FromTable = (int)SUpDn1.Value;
                    request.FromDescriptor = (int)SUpDn2.Value;
                    request.ToTable = (int)DUpDn1.Value;
                    request.ToDescriptor = (int)DUpDn2.Value;
                    break;

                case RequestTypes.MemoryToHDD:
                    request.FromTable = (int)SUpDn1.Value;
                    request.FromDescriptor = (int)SUpDn2.Value;
                    request.ToFile = DBox.Text;
                    request.FileBlockNum = (int)DUpDn2.Value;
                    break;

                case RequestTypes.HDDToMemory:
                    request.FromFile = SBox.Text;
                    request.FileBlockNum = (int)SUpDn2.Value;
                    request.ToTable = (int)DUpDn1.Value;
                    request.ToDescriptor = (int)DUpDn2.Value;
                    break;
                case RequestTypes.Action:
                    request.FromTable = (int)SUpDn1.Value;
                    request.FromDescriptor = (int)SUpDn2.Value;
                    break;
            }
        }
    }
}
