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
        Zayavka request;

        public RequestEditor(int i_process, int i_request)
        {
            InitializeComponent();
            // получение запроса
            this.i_process = i_process;
            this.i_request = i_request;
            request = DispetcherProcessov.Processes[i_process].Zayavki[i_request];

            // диапазоны значений таблиц дескрипторов
            SUpDn1.Minimum = Array.IndexOf(Memory.Stranici, DispetcherProcessov.Processes[i_process].AdresnieProstranstva[0]);
            SUpDn1.Maximum = Array.IndexOf(Memory.Stranici, DispetcherProcessov.Processes[i_process].AdresnieProstranstva[DispetcherProcessov.Processes[i_process].AdresnieProstranstva.Length - 1]);
            DUpDn1.Minimum = Array.IndexOf(Memory.Stranici, DispetcherProcessov.Processes[i_process].AdresnieProstranstva[0]);
            DUpDn1.Maximum = Array.IndexOf(Memory.Stranici, DispetcherProcessov.Processes[i_process].AdresnieProstranstva[DispetcherProcessov.Processes[i_process].AdresnieProstranstva.Length - 1]);

            // заполнение начальных данных с редактируемой заявки
            RequestTypeBox.SelectedIndex = (int)request.Type_zayavky;
            SBox.Items.AddRange(HDD.Catalog.ToArray<ZapisVCataloge>()); SBox.SelectedIndex = 0;
            DBox.Items.AddRange(HDD.Catalog.ToArray<ZapisVCataloge>()); DBox.SelectedIndex = 0;
            switch (request.Type_zayavky)
            {
                case RequestTypes.Copy:
                    SUpDn1.Value = request.IzTablici;
                    SUpDn2.Value = request.IzDes;
                    DUpDn1.Value = request.VTablicu;
                    DUpDn2.Value = request.VDes;
                    break;
                case RequestTypes.IzMemory:
                    SUpDn1.Value = request.IzTablici;
                    SUpDn2.Value = request.IzDes;
                    DBox.Text = request.VFile;
                    DUpDn2.Value = request.NomerFB;
                    break;
                case RequestTypes.VMemory:
                    SBox.Text = request.IzFile;
                    SUpDn2.Value = request.NomerFB;
                    DUpDn1.Value = request.VTablicu;
                    DUpDn2.Value = request.VDes;
                    break;
                case RequestTypes.Deistvie:
                    SUpDn1.Value = request.IzTablici;
                    SUpDn2.Value = request.IzDes;
                    break;
            }

            // первое обновление
            RefreshForm();
        }

        private void RefreshForm()
        {
            switch ((RequestTypes)RequestTypeBox.SelectedIndex)
            {
                case RequestTypes.Copy:
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

                case RequestTypes.IzMemory:
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

                case RequestTypes.VMemory:
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
                case RequestTypes.Deistvie:
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
            request.Type_zayavky = (RequestTypes)RequestTypeBox.SelectedIndex;
            switch (request.Type_zayavky)
            {
                case RequestTypes.Copy:
                    request.IzTablici = (int)SUpDn1.Value;
                    request.IzDes = (int)SUpDn2.Value;
                    request.VTablicu = (int)DUpDn1.Value;
                    request.VDes = (int)DUpDn2.Value;
                    break;

                case RequestTypes.IzMemory:
                    request.IzTablici = (int)SUpDn1.Value;
                    request.IzDes = (int)SUpDn2.Value;
                    request.VFile = DBox.Text;
                    request.NomerFB = (int)DUpDn2.Value;
                    break;

                case RequestTypes.VMemory:
                    request.IzFile = SBox.Text;
                    request.NomerFB = (int)SUpDn2.Value;
                    request.VTablicu = (int)DUpDn1.Value;
                    request.VDes = (int)DUpDn2.Value;
                    break;
                case RequestTypes.Deistvie:
                    request.IzTablici = (int)SUpDn1.Value;
                    request.IzDes = (int)SUpDn2.Value;
                    break;
            }
        }
    }
}
