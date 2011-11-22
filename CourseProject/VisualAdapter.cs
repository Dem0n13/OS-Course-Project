using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OS
{
    /// <summary>
    /// Класс-посредник между формами и данными. Будет все выводить.
    /// </summary>
    public static class VisualAdapter
    {
        #region Ссылки на формы для вывода
        public static DataGridView DGDescriptionTable;
        public static DataGridView DGDescriptionPages;
        public static DataGridView DGPagesInMemory;
        public static Label DebugLabel;
        public static DataGridView DGCatalog;
        public static DataGridView DGCellsArray;
        public static DataGridView DGRequests;
        public static Label LabelProc;
#if UI_REQUEST_EDITOR_ON_EDITOR_FORM
        public static Button EditRequestBtn;
#endif
#if UI_REQUEST_EDITOR_ON_MAIN_FORM
        public static TableLayoutPanel EditorPanel;
        public static ComboBox RequestTypeBox;
        public static NumericUpDown SUpDn2;
        public static Button SaveBtn;
        public static NumericUpDown DUpDn2;
        public static NumericUpDown DUpDn1;
        public static NumericUpDown SUpDn1;
        public static ComboBox SBox;
        public static ComboBox DBox;
#endif
        #endregion

        /// <summary>
        /// Инициализирует все формы неизменяемыми данными
        /// </summary>
        public static void Init()
        {
            #region Основная память
            // Дескрипторы таблиц
            DGDescriptionTable.ColumnCount=3;
            DGDescriptionTable.Columns[0].HeaderText = "Адрес";
            DGDescriptionTable.Columns[1].HeaderText = "Владельцы";
            DGDescriptionTable.Columns[2].HeaderText = "Ссылка";
            DGDescriptionTable.RowCount = GlobalConsts.SizesOfGroup.Length;
            for (int i = 0; i < GlobalConsts.CountOfGroup; i++)
            {
                //расставляем адреса в первой и третьей колонке колонке
                DGDescriptionTable.Rows[i].Cells[0].Value = Memory.TableDess[i].Address;
                DGDescriptionTable.Rows[i].Cells[2].Value = (Memory.TableDess[i].TargetAddress + GlobalConsts.CountOfGroup).ToString();

                // расскрашиваем группы
                //DGDescriptionTable.Rows[i].Cells[1].Style.BackColor = System.Drawing.Color.FromArgb(Program.RND.Next(100, 255), Program.RND.Next(100, 255), Program.RND.Next(100, 255));
                DGDescriptionTable.Rows[i].Cells[2].Style.BackColor = DGDescriptionTable.Rows[i].Cells[1].Style.BackColor;
            }
            
            // Расстановка владельцев у таблиц дескрипторов
            for (int i = 0; i < GlobalConsts.ProcessesCount; i++)
                foreach (TableDescriptor td in TaskManager.Processes[i].LogicAreas)
                    DGDescriptionTable.Rows[Array.IndexOf(Memory.TableDess, td)].Cells[1].Value += TaskManager.Processes[i].ID + ", ";
            
            //удаляем лишние символы ", " (запятая пробел)
            for (int i = 0; i < DGDescriptionTable.RowCount; i++)
                DGDescriptionTable[1, i].Value = DGDescriptionTable[1, i].Value.ToString().Substring(0, DGDescriptionTable[1, i].Value.ToString().Length - 2);

            DGDescriptionTable.Columns[0].Width = 40;
            DGDescriptionTable.Columns[1].Width = 66;
            DGDescriptionTable.Columns[2].Width = 50;

            // Дескрипторы страниц
            DGDescriptionPages.RowCount = GlobalConsts.PagesCount;
#if FIFO
            DGDescriptionPages.ColumnCount = 5;
#endif
#if WS || FIFO_SC || ClockWithOneArrow || ClockWithTwoArrows
            DGDescriptionPages.ColumnCount = 6;
#endif
#if WSClock || NFU || LRU
            DGDescriptionPages.ColumnCount = 7;
#endif
            DGDescriptionPages.Columns[0].HeaderText = "Адрес";
            DGDescriptionPages.Columns[0].Width = 50;
            DGDescriptionPages.Columns[1].HeaderText = "Present";
            DGDescriptionPages.Columns[1].Width = 50;
            DGDescriptionPages.Columns[2].HeaderText = "Мьютекс";
            DGDescriptionPages.Columns[2].Width = 50;
            DGDescriptionPages.Columns[3].HeaderText = "Адр. в ФП";
            DGDescriptionPages.Columns[3].Width = 50;
            DGDescriptionPages.Columns[4].HeaderText = "Ссылка";
            DGDescriptionPages.Columns[4].Width = 50;
#if (WS || WSClock)
            DGDescriptionPages.Columns[5].HeaderText = "Возраст";
            DGDescriptionPages.Columns[5].Width = 50;
#endif
#if NFU || LRU
            DGDescriptionPages.Columns[5].HeaderText = "Счетчик";
            DGDescriptionPages.Columns[5].Width = 50;
#endif
#if (FIFO_SC || ClockWithOneArrow || ClockWithTwoArrows)
            DGDescriptionPages.Columns[5].HeaderText = "Access";
            DGDescriptionPages.Columns[5].Width = 50;
#endif
#if (WSClock || NFU || LRU)

            DGDescriptionPages.Columns[6].HeaderText = "А";
            DGDescriptionPages.Columns[6].Width = 50;
#endif

            int local_buf = 0;
            for (int i = 0; i < GlobalConsts.CountOfGroup; i++)
            {
                for (int j = local_buf; j < local_buf + GlobalConsts.SizesOfGroup[i]; j++)
                {
                    DGDescriptionPages.Rows[j].Cells[0].Style.BackColor = DGDescriptionTable.Rows[i].Cells[1].Style.BackColor;
                    DGDescriptionPages.Rows[j].Cells[4].Style.BackColor = DGDescriptionTable.Rows[i].Cells[1].Style.BackColor;
                    DGDescriptionPages.Rows[j].Cells[0].Value = Memory.PageDess[j].Address + GlobalConsts.CountOfGroup;
                }
                local_buf += GlobalConsts.SizesOfGroup[i];
            }

            // Основная область памяти (заполнение адресов)
            DGPagesInMemory.RowCount = GlobalConsts.PagesAreaSize;
            for (int i = 0; i < GlobalConsts.PagesAreaSize; i++)
            {
                DGPagesInMemory.Rows[i].Cells[0].Value = i + GlobalConsts.StartAddressAreaOfPages;
            }

            #endregion

            #region Заявки

            for (int i = 0; i < GlobalConsts.ProcessesCount; i++)
                for (int j = 0; j < TaskManager.Processes[i].Requests.Length; j++)
                    DGRequests.Rows.Add("");

            #endregion

            #region HDD

            #region Для перечисления
#if FS_WITH_INDEX_ENUM

            // заполнение каталога
            DGCatalog.ColumnCount = 5;
            DGCatalog.RowCount = GlobalConsts.CatalogRecordsCount;
            DGCatalog.RowHeadersVisible = false;
            DGCatalog.Columns[0].HeaderText = "Адрес";
            DGCatalog.Columns[1].HeaderText = "Открыт";
            DGCatalog.Columns[2].HeaderText = "Имя файла";
            DGCatalog.Columns[3].HeaderText = "Перечисление";
            DGCatalog.Columns[4].HeaderText = "Размер";

            DGCatalog.Columns[0].Width = 42;
            DGCatalog.Columns[1].Width = 50;
            DGCatalog.Columns[2].Width = 50;
            DGCatalog.Columns[3].Width = 85;
            DGCatalog.Columns[4].Width = 50;

            

            // заполнение ВЗУ
            DGCellsArray.ColumnCount = 6;
            DGCellsArray.RowCount = HDD.CellsArray.Length;
            DGCellsArray.RowHeadersVisible = false;
            DGCellsArray.Columns[0].HeaderText = "Адрес";
            DGCellsArray.Columns[1].HeaderText = "0";
            DGCellsArray.Columns[2].HeaderText = "1";
            DGCellsArray.Columns[3].HeaderText = "2";
            DGCellsArray.Columns[4].HeaderText = "3";
            DGCellsArray.Columns[5].HeaderText = "Свободно";

            DGCellsArray.Columns[0].Width = 42;
            DGCellsArray.Columns[1].Width = 30;
            DGCellsArray.Columns[2].Width = 30;
            DGCellsArray.Columns[3].Width = 30;
            DGCellsArray.Columns[4].Width = 30;
            DGCellsArray.Columns[5].Width = 60;
            
            //расставляем адреса
            for (int i = 0; i < GlobalConsts.HDDCellsCount+GlobalConsts.PagesCount; i++)
            {
                DGCellsArray.Rows[i].Cells[0].Value = HDD.CellsArray[i].Address;
            }
            //красим swap
            for (int i = GlobalConsts.HDDCellsCount; i < GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount; i++)
            {
                DGCellsArray.Rows[i].Cells[0].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[1].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[2].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
            }

#endif
            #endregion

            #region Для последовательности
#if FS_WITH_INDEX_SEQ

            //Первая таблица
            DGCatalog.RowCount = HDD.Catalog.Count;
            DGCatalog.ColumnCount = 5;
            DGCatalog.RowHeadersVisible = false;
            DGCatalog.Columns[0].HeaderText = "Адрес";
            DGCatalog.Columns[1].HeaderText = "Открыт";
            DGCatalog.Columns[2].HeaderText = "Имя файла";
            DGCatalog.Columns[3].HeaderText = "Индекс";
            DGCatalog.Columns[4].HeaderText = "Размер";
            //for (int i = 0; i < HDD.Catalog.Count; i++)
            //{
            //    DGCatalog.Rows[i].Cells[0].Value = HDD.Catalog[i].Address;
            //    DGCatalog.Rows[i].Cells[2].Value = HDD.Catalog[i].Filename;
            //    DGCatalog.Rows[i].Cells[3].Value = HDD.Catalog[i].StartIndex;
            //}

            DGCatalog.Columns[0].Width = 42;
            DGCatalog.Columns[1].Width = 52;
            DGCatalog.Columns[2].Width = 50;
            DGCatalog.Columns[3].Width = 50;
            DGCatalog.Columns[4].Width = 50;

            //вторая таблица
            DGCellsArray.RowCount = GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount;
            DGCellsArray.ColumnCount = 7;
            DGCellsArray.RowHeadersVisible = false;
            DGCellsArray.RowCount = HDD.CellsArray.Length;
            DGCellsArray.RowHeadersVisible = false;
            DGCellsArray.Columns[0].HeaderText = "Адрес";
            DGCellsArray.Columns[1].HeaderText = "0";
            DGCellsArray.Columns[2].HeaderText = "1";
            DGCellsArray.Columns[3].HeaderText = "2";
            DGCellsArray.Columns[4].HeaderText = "3";
            DGCellsArray.Columns[5].HeaderText = "След.";
            DGCellsArray.Columns[6].HeaderText = "Свободно";

            DGCellsArray.Columns[0].Width = 42;
            DGCellsArray.Columns[1].Width = 30;
            DGCellsArray.Columns[2].Width = 30;
            DGCellsArray.Columns[3].Width = 30;
            DGCellsArray.Columns[4].Width = 30;
            DGCellsArray.Columns[5].Width = 40;
            DGCellsArray.Columns[6].Width = 60;

            //расставляем адреса
            for (int i = 0; i < GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount; i++)
            {
                DGCellsArray.Rows[i].Cells[0].Value = HDD.CellsArray[i].Address;
            }
            //красим swap
            for (int i = GlobalConsts.HDDCellsCount; i < GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount; i++)
            {
                DGCellsArray.Rows[i].Cells[0].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[1].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[2].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
                DGCellsArray.Rows[i].Cells[4].Style.BackColor = System.Drawing.Color.LightGray;
            }

#endif
            #endregion

            #endregion

            // первое обновление
            RefreshRequests();
            RefreshMemoryState();
            RefreshHDDState();
        }

        /// <summary>
        /// Обновить таблицу заявок
        /// </summary>
        public static void RefreshRequests()
        {
            int k = 0;
            for (int i = 0; i < GlobalConsts.ProcessesCount; i++)
            {
                for (int j = 0; j < TaskManager.Processes[i].Requests.Length; j++, k++)
                {
                    // текст
                    DGRequests[0, k].Value = "Процесс " + TaskManager.Processes[i].ID;
                    DGRequests[1, k].Value = TaskManager.Processes[i].Requests[j].ToString();
                    DGRequests[2, k].Value = "";
                    // для текущих заявок активного и блокированного процесса выводить колво скопированных байт
                    if (((TaskManager.Processes[i].State == ProcessState.Active) || (TaskManager.Processes[i].State == ProcessState.Paused)) && (TaskManager.Processes[i].Context.CurrentRequest == j))
                        DGRequests[2, k].Value = TaskManager.Processes[i].Context.TotalCopied.ToString();

                    // раскрашиваем v3
                    if ((j < TaskManager.Processes[i].Context.CurrentRequest) || (TaskManager.Processes[i].State == ProcessState.Completed))
                        DGRequests.Rows[k].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                    else if (j == TaskManager.Processes[i].Context.CurrentRequest)
                    {
                        //if (TaskManager.Processes[i].State == ProcessState.Active)
                        //    DGRequests.Rows[k].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                        //if (TaskManager.Processes[i].State == ProcessState.Paused)
                        //    DGRequests.Rows[k].DefaultCellStyle.BackColor = System.Drawing.Color.SandyBrown;
                    }
                }
            }

            // активность редактора заявок. Выполненные заявки не изменяются
#if UI_REQUEST_EDITOR_ON_EDITOR_FORM
            if ((DGRequests.SelectedRows.Count != 0) && ((DGRequests.SelectedRows[0].DefaultCellStyle.BackColor == System.Drawing.Color.Empty) || (DGRequests.SelectedRows[0].Cells[2].Value.ToString() == "0")))
                EditRequestBtn.Enabled = true;
            else
                EditRequestBtn.Enabled = false;
#endif
#if UI_REQUEST_EDITOR_ON_MAIN_FORM
            if ((DGRequests.SelectedRows.Count != 0) && ((DGRequests.SelectedRows[0].DefaultCellStyle.BackColor == System.Drawing.Color.Empty) || (DGRequests.SelectedRows[0].Cells[2].Value.ToString() == "0")))
            {
                // включаем панель и заполняем
                EditorPanel.Enabled = true;
                // получаем номер процесса и номер заявки
                int i_process = 0;
                int i_request = 0;
                k = 0;
                for (i_process = 0; i_process < GlobalConsts.ProcessesCount; i_process++)
                {
                    for (i_request = 0; i_request < TaskManager.Processes[i_process].Requests.Length; i_request++)
                    {
                        if (k == DGRequests.SelectedRows[0].Index)
                        {
                            RefreshRequestEditor(i_process, i_request);
                            return;
                        }
                        k++;
                    }
                }
            }
            else
            {
                EditorPanel.Enabled = false;
            }
#endif
            LabelProc.Text = "Процесс на выполнение: " + (TaskManager.CurrentProcessIndex == -1 ? "Нет" : TaskManager.Processes[TaskManager.CurrentProcessIndex].ID);
        }

#if UI_REQUEST_EDITOR_ON_MAIN_FORM
        public static int i_process;
        public static int i_request;
        public static Request Request;

        public static void RefreshRequestEditor(int i_pr, int i_req)
        {
            i_process = i_pr;
            i_request = i_req;
            Request = TaskManager.Processes[i_process].Requests[i_request];

            // диапазоны значений таблиц дескрипторов
            SUpDn1.Minimum = Array.IndexOf(Memory.Pages, TaskManager.Processes[i_process].LogicAreas[0]);
            SUpDn1.Maximum = Array.IndexOf(Memory.Pages, TaskManager.Processes[i_process].LogicAreas[TaskManager.Processes[i_process].LogicAreas.Length - 1]);
            DUpDn1.Minimum = Array.IndexOf(Memory.Pages, TaskManager.Processes[i_process].LogicAreas[0]);
            DUpDn1.Maximum = Array.IndexOf(Memory.Pages, TaskManager.Processes[i_process].LogicAreas[TaskManager.Processes[i_process].LogicAreas.Length - 1]);

            // список доступных файлов
            SBox.Items.Clear();
            SBox.Items.AddRange(HDD.Catalog.ToArray<CatalogRecord>());
            DBox.Items.Clear();
            DBox.Items.AddRange(HDD.Catalog.ToArray<CatalogRecord>());

            // скидываем диапазоны со старой заявки
            SUpDn2.Maximum = decimal.MaxValue;
            DUpDn2.Maximum = decimal.MaxValue;

            // заполняем редактор снизу вверх
            switch (Request.Type)
            {
                case RequestTypes.MemoryToMemory:
                    SUpDn2.Value = Request.FromDescriptor;
                    DUpDn2.Value = Request.ToDescriptor;
                    SUpDn1.Value = Request.FromTable;
                    DUpDn1.Value = Request.ToTable;

                    break;
                case RequestTypes.MemoryToHDD:
                    SUpDn2.Value = Request.FromDescriptor;
                    DUpDn2.Value = Request.FileBlockNum;
                    SUpDn1.Value = Request.FromTable;
                    DBox.Text = Request.ToFile;
                    break;
                case RequestTypes.HDDToMemory:
                    SUpDn2.Value = Request.FileBlockNum;
                    DUpDn2.Value = Request.ToDescriptor;
                    SBox.Text = Request.FromFile;

                    DUpDn1.Value = Request.ToTable;

                    break;
                case RequestTypes.Action:
                    SUpDn2.Value = Request.FromDescriptor;
                    SUpDn1.Value = Request.FromTable;

                    break;
            }
            SBox.SelectedIndex = 0;
            DBox.SelectedIndex = 0;
            RequestTypeBox.SelectedIndex = (int)Request.Type;

            RefreshEditorEditMode();
        }

        public static void RefreshEditorEditMode()
        {
            switch ((RequestTypes)RequestTypeBox.SelectedIndex)
            {
                case RequestTypes.MemoryToMemory:
                    // видимость форм
                    SBox.Visible = DBox.Visible = false;
                    SUpDn1.Visible = DUpDn1.Visible = DUpDn2.Visible = true;
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)SUpDn1.Value] - 1;
                    DUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)DUpDn1.Value] - 1;
                    break;

                case RequestTypes.MemoryToHDD:
                    // видимость форм
                    SBox.Visible = DUpDn1.Visible = false;
                    SUpDn1.Visible = DBox.Visible = DUpDn2.Visible = true;
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)SUpDn1.Value] - 1;
                    DUpDn2.Maximum = GlobalConsts.HDDCellsCount;
                    break;

                case RequestTypes.HDDToMemory:
                    // видимость форм
                    SUpDn1.Visible = DBox.Visible = false;
                    SBox.Visible = DUpDn1.Visible = DUpDn2.Visible = true;
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.HDDCellsCount;
                    DUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)DUpDn1.Value] - 1;
                    break;

                case RequestTypes.Action:
                    // видимость форм
                    SBox.Visible = DUpDn1.Visible = DBox.Visible = DUpDn2.Visible = false;
                    SUpDn1.Visible = true;
                    // корректировка диапазонов
                    SUpDn2.Maximum = GlobalConsts.SizesOfGroup[(int)SUpDn1.Value] - 1;
                    break;
            }
        }
#endif

        /// <summary>
        /// Обновить состояние оперативы на форме
        /// </summary>
        public static void RefreshMemoryState()
        {
            //заполняем таблицу дескрипторы страниц и раскрашиваем
            for (int i = 0; i < GlobalConsts.StartAddressAreaOfPages - GlobalConsts.StartAddressDescriptionPage; i++)
            {
                DGDescriptionPages.Rows[i].Cells[1].Value = Memory.PageDess[i].Present;
                //DGDescriptionPages[1, i].Style.BackColor = Memory.PageDess[i].Present == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
                DGDescriptionPages.Rows[i].Cells[2].Value = Memory.PageDess[i].Mutex;
                //DGDescriptionPages[2, i].Style.BackColor = Memory.PageDess[i].Mutex == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
                DGDescriptionPages.Rows[i].Cells[3].Value = Memory.PageDess[i].AddressInSwap;
                DGDescriptionPages.Rows[i].Cells[4].Value = Memory.PageDess[i].TargetAddress == -1 ? "-1" : (Memory.PageDess[i].TargetAddress + GlobalConsts.StartAddressAreaOfPages).ToString();
#if (WS || WSClock)
                DGDescriptionPages.Rows[i].Cells[5].Value = Memory.PageDess[i].AgeOfPage;
#endif
#if (FIFO_SC || ClockWithOneArrow || ClockWithTwoArrows)
                DGDescriptionPages.Rows[i].Cells[5].Value = Memory.PageDess[i].Access;
                //DGDescriptionPages[5, i].Style.BackColor = (Memory.Pages[i + GlobalConsts.StartAddressDescriptionPage] as PageDescriptor).Access == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
#endif
#if (NFU ||LRU)
                DGDescriptionPages.Rows[i].Cells[5].Value = Memory.PageDess[i].Counter;
#endif
#if (WSClock||NFU ||LRU)
                DGDescriptionPages.Rows[i].Cells[6].Value = Memory.PageDess[i].Access;
                //DGDescriptionPages[6, i].Style.BackColor = Memory.Pages[i].Access == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
#endif
            }

            //заполняем таблицу основной памяти
            for (int i = 0; i < GlobalConsts.PagesAreaSize; i++)
            {
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    DGPagesInMemory.Rows[i].Cells[j+1].Value = Memory.Pages[i].Data[j];
                }
            }
        }

        /// <summary>
        /// Обновить состояние HDD на форме
        /// </summary>
        public static void RefreshHDDState()
        {
            #region Для перечисления
#if FS_WITH_INDEX_ENUM
            //первая таблица
            DGCatalog.RowCount = HDD.Catalog.Count;

            for (int i = 0; i < HDD.Catalog.Count; i++)
            {
                DGCatalog.Rows[i].Cells[1].Value = HDD.Catalog[i].IsOpen;
                //DGCatalog[1, i].Style.BackColor = HDD.Catalog[i].IsOpen == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
                DGCatalog.Rows[i].Cells[0].Value = HDD.Catalog[i].Address;
                DGCatalog.Rows[i].Cells[2].Value = HDD.Catalog[i].Filename;
                DGCatalog.Rows[i].Cells[4].Value = HDD.Catalog[i].FileSize;

                DGCatalog.Rows[i].Cells[3].Value = "";
                for (int j = 0; j < HDD.Catalog[i].Indexes.Count; j++)
                {
                    DGCatalog.Rows[i].Cells[3].Value += HDD.Catalog[i].Indexes[j].ToString() + ", ";
                }
                //удаляем лишнее ", "
                
                string a = DGCatalog.Rows[i].Cells[3].Value.ToString();
                if (a.Length>2)
                DGCatalog.Rows[i].Cells[3].Value = a.Substring(0, a.Length - 2);
            }

            //вторая таблица
            for (int i = 0; i < GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount; i++)
            {
                for (int j = 0; j < HDD.CellsArray[0].Data.Length; j++)
                {
                    DGCellsArray.Rows[i].Cells[1 + j].Value = HDD.CellsArray[i].Data[j];
                }
                DGCellsArray.Rows[i].Cells[5].Value = HDD.CellsArray[i].IsFree;
                //DGCellsArray.Rows[i].Cells[5].Style.BackColor = HDD.CellsArray[i].IsFree == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
            }
#endif
            #endregion

            #region Для последовательности
#if FS_WITH_INDEX_SEQ
            //первая таблица
            DGCatalog.RowCount = HDD.Catalog.Count;
            for (int i = 0; i < HDD.Catalog.Count; i++)
            {
                DGCatalog.Rows[i].Cells[1].Value = HDD.Catalog[i].IsOpen;
                //DGCatalog[1, i].Style.BackColor = HDD.Catalog[i].IsOpen == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
                DGCatalog.Rows[i].Cells[0].Value = HDD.Catalog[i].Address;
                DGCatalog.Rows[i].Cells[2].Value = HDD.Catalog[i].Filename;
                DGCatalog.Rows[i].Cells[3].Value = HDD.Catalog[i].StartIndex;
                DGCatalog.Rows[i].Cells[4].Value = HDD.Catalog[i].FileSize;
            }

            //вторая таблица

            for (int i = 0; i < GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount; i++)
            {
                for (int j = 0; j < HDD.CellsArray[0].Data.Length; j++)
                {
                    DGCellsArray.Rows[i].Cells[1 + j].Value = HDD.CellsArray[i].Data[j];
                }
                DGCellsArray.Rows[i].Cells[5].Value = HDD.CellsArray[i].Next;
                DGCellsArray.Rows[i].Cells[6].Value = HDD.CellsArray[i].IsFree;
                //DGCellsArray.Rows[i].Cells[6].Style.BackColor = HDD.CellsArray[i].IsFree == true ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightCoral;
            }
#endif
            #endregion
        }
    }
}