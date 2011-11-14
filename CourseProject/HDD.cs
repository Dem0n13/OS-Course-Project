using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace OS
{
    /// <summary>
    /// Статический класс ВЗУ
    /// </summary>
    public static class HDD
    {
        /// <summary>
        /// Каталог
        /// </summary>
        public static List<CatalogRecord> Catalog = new List<CatalogRecord>();

        /// <summary>
        /// Массив данных на жестком диске
        /// </summary>
        public static HDDCell[] CellsArray = new HDDCell[GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount];

        static HDD()
        {
            // Инициализируем массив данных
            for (int i = 0; i < GlobalConsts.HDDCellsCount + GlobalConsts.PagesCount; i++)
            {
                //ставим адрес у ячейки
                CellsArray[i] = new HDDCell()
                {
                    Address = i
                };
                CellsArray[i].IsFree = true;
                CellsArray[i].Data = new byte[GlobalConsts.PageSize];
            }
            //NonRepeatEnum NESForHDD = new NonRepeatEnum(0, GlobalConsts.HDDCellsCount - 1);
            for (int i = 0; i < GlobalConsts.CatalogRecordsCount; i++)
            {
                Catalog.Add(new CatalogRecord() { Address = i + GlobalConsts.StartCatalogRecords, IsOpen = false, FileSize = 0 });
            }

            //инициалицируем start.txt
            Catalog[0].Filename = "Start.txt";
            NonRepeatEnum CatalogIndexses = new NonRepeatEnum(0, GlobalConsts.HDDCellsCount - 1);
            for (int i = 0; i < 5; i++)
            {
                int temp = CatalogIndexses.Next();
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    CellsArray[temp].Data[j] = (byte)Program.RND.Next(0, 256);
                    Catalog[0].FileSize++;
                }
                CellsArray[temp].IsFree = false;
                Catalog[0].Indexes.Add(temp);
            }
            Catalog[0].IsOpen = false;
        }

        /// <summary>
        /// Поиск файла в каталоге по имени
        /// </summary>
        /// <param name="fname">Имя файла</param>
        /// <returns>Индекс файла в каталоге, либо -1, если не найден</returns>
        private static int FindFile(string fname)
        {
            for (int i = 0; i < Catalog.Count; i++)
            {
                if (Catalog[i].Filename == fname)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Проверка файла на доступность для чтения/записи. Также не дает прочесть несуществующий файл и не существующий блок
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <param name="fileblock">Номер файлового блока</param>
        /// <param name="offset">Смещение в байтах</param>
        /// <returns>true - доступен на запись/чтение, false - не доступен на запись/чтение</returns>
        public static bool CheckMutex(string filename, int fileblock, int offset, bool IsRead)
        {
            // нахождение файла в каталоге
            int file_i = FindFile(filename);

            //нет файла и при попытке чтения
            if (IsRead == true && file_i == -1)
            {
                return false;
            }

            //попытка прочесть из несуществующего блока
            if (IsRead == true && fileblock>Catalog[FindFile(filename)].FileSize-1)
            {
                return false;
            }

            //файла нет
            if (file_i == -1)
            {
                return true;
            }
            // проверка, открыт ли файл на запись
            if ((Catalog[file_i].IsOpen) && (offset == 0))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Считывает байт из файла, если это возможно
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <param name="fileblock">Номер файлового блока (0 - первый блок)</param>
        /// <param name="offset">Смещение относительно начала блока</param>
        /// <param name="data">Буфер для чтения</param>
        public static byte ReadByte(string filename, int fileblock, int offset, byte data)
        {
            data = 0;
            // нахождение файла в каталоге
            int file_i = FindFile(filename);
            // проверки пройдены, получаем адрес файлового блока и считываем байт
            int block_a = Catalog[file_i].Indexes[fileblock];
            Catalog[file_i].IsOpen = true;
            data = CellsArray[block_a].Data[offset];
            if (offset == 3)
            {
                Catalog[file_i].IsOpen = false;
            }
            return data;
        }

        /// <summary>
        /// Пишет байт в файл, если это возможно
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <param name="fileblock">Номер файлового блока</param>
        /// <param name="offset">Смещение относительно начала блока</param>
        /// <param name="data">Буфер на запись</param>
        /// <returns>true - запись завершена, false - запись невозможна</returns>
        public static void WriteByte(string filename, int fileblock, int offset, byte data)
        {
            // нахождение файла в каталоге
            if (FindFile(filename) == -1)
            {
                CreateFile(filename);
            }
            int file_i = FindFile(filename);
            // проверки пройдены, получаем адрес файлового блока и пишем байт
            //если блок не существует то создаем его
            while (Catalog[file_i].Indexes.Count<fileblock+1)
            {
                AddFileBlock(FindFile(filename));
            }
            int block_a = Catalog[file_i].Indexes[fileblock];
            Catalog[file_i].IsOpen = true;
            CellsArray[block_a].Data[offset] = data;
            if (offset == 3)
                Catalog[file_i].IsOpen = false;
        }

        private static void AddFileBlock(int file)
        {
            //ищем свободный блок
            for (int i=0;i<GlobalConsts.StartSwapArea;i++)
            {
                if (CellsArray[i].IsFree == true)
                {
                    Catalog[file].Indexes.Add(i);
                    Catalog[file].FileSize++;
                    CellsArray[i].IsFree = false;
                    break;
                }
            }
        }

        private static void CreateFile(String FileName)
        {
            Catalog.Add(new CatalogRecord() { Address = Catalog.Count + GlobalConsts.StartCatalogRecords, IsOpen = false, Filename = FileName }); ;
        }
    }
}