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
        /// Объем файла подкачки в страницах
        /// </summary>
        private static int SwapFileSize = GlobalConsts.PagesCount;

        /// <summary>
        /// Каталог
        /// </summary>
        public static List<CatalogRecord> Catalog = new List<CatalogRecord>();

        /// <summary>
        /// Массив данных на жестком диске
        /// </summary>
        public static HDDCell[] CellsArray = new HDDCell[GlobalConsts.HDDCellsCount + SwapFileSize];

        static HDD()
        {
            #region Инициализируем для перечисления
#if FS_WITH_INDEX_ENUM
            // Инициализируем массив данных
            for (int i = 0; i < GlobalConsts.HDDCellsCount + SwapFileSize; i++)
            {
                //ставим адрес у ячейки
                CellsArray[i] = new HDDCell()
                {
                    Address = i
                };
                CellsArray[i].IsFree = true;
                CellsArray[i].Data = new byte[GlobalConsts.PageSize];
            }
            for (int i = 0; i < GlobalConsts.CatalogRecordsCount; i++)
            {
                Catalog.Add(new CatalogRecord() { Address = i + GlobalConsts.StartCatalogRecords, IsOpen = false, FileSize = 0 });
            }

            //инициалицируем start.txt
            Catalog[0].Filename = "File1.f";
            int temp = 7;
            for (int i = 0; i < 5; i++)
            {
                temp ++;
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    CellsArray[temp].Data[j] = (byte)Program.RND.Next(0, 256);
                }
                Catalog[0].FileSize++;
                CellsArray[temp].IsFree = false;
                Catalog[0].Indexes.Add(temp);
            }
            Catalog[0].IsOpen = false;

            //инициалицируем File2.f
            Catalog[1].Filename = "File2.f";

#endif
            #endregion;

            #region Инициализируем каталог для связанной последовательности индексов
#if FS_WITH_INDEX_SEQ
            // Инициализируем массив данных
            for (int i = 0; i < GlobalConsts.HDDCellsCount + SwapFileSize; i++)
            {
                //ставим адрес у ячейки
                CellsArray[i] = new HDDCell()
                    {
                        Address = i
                    };
                CellsArray[i].IsFree = true;
                CellsArray[i].Next = -1;
                CellsArray[i].Data = new byte[GlobalConsts.PageSize];
            }
            int temp = 7; //с какого начинать адреса писать первый файл

            Catalog.Add(new CatalogRecord() { Address = 0 + GlobalConsts.StartCatalogRecords, IsOpen = false, StartIndex = -1, FileSize = 0, Filename = "File1" });
            Catalog.Add(new CatalogRecord() { Address = 1 + GlobalConsts.StartCatalogRecords, IsOpen = false, StartIndex = -1, FileSize = 0, Filename = "File2" });

            Catalog[0].StartIndex = temp;
            for (int j = 0; j < GlobalConsts.PageSize; j++)
            {
                CellsArray[Catalog[0].StartIndex].Data[j] = (byte)Program.RND.Next(0, 256);
            }
            Catalog[0].FileSize++;
            CellsArray[Catalog[0].StartIndex].IsFree = false;
            //пишем остальные 4 блока
            int prev = Catalog[0].StartIndex;
            for (int i = 0; i < 4; i++)
            {

                Catalog[0].FileSize++;
                CellsArray[prev].IsFree = false;
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    CellsArray[prev].Data[j] = (byte)Program.RND.Next(0, 256);
                }
                CellsArray[prev].IsFree = false;
                temp++;
                CellsArray[prev].Next = temp;
                prev = CellsArray[prev].Next;
            }
            // конец файла start.txt+1блок
            CellsArray[prev].Next = -1;
            CellsArray[prev].IsFree = false;
            for (int j = 0; j < GlobalConsts.PageSize; j++)
            {
                CellsArray[prev].Data[j] = (byte)Program.RND.Next(0, 256);
            }

            Catalog[0].IsOpen = false;
#endif
            #endregion;
        }

        /// <summary>
        /// Поиск файла в каталоге по имени
        /// </summary>
        /// <param name="fname">Имя файла</param>
        /// <returns>Индекс файла в каталоге, либо -1, если не найден</returns>
        public static int FindFile(string fname)
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
            if (IsRead == true && fileblock > Catalog[FindFile(filename)].FileSize - 1)
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
#if FS_WITH_INDEX_ENUM
            int block_a = Catalog[file_i].Indexes[fileblock];
#endif
#if FS_WITH_INDEX_SEQ
            int cur_index = Catalog[file_i].StartIndex;
            int counter = 0;
            while (CellsArray[cur_index].Next != -1)
            {
                if (counter == fileblock)
                {
                    break;
                }
                counter++;
                cur_index = CellsArray[cur_index].Next;
            }
            int block_a = cur_index;
#endif
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
#if FS_WITH_INDEX_ENUM
            //если блок не существует то создаем его
            while (Catalog[file_i].Indexes.Count<fileblock+1)
            {
                AddFileBlock(FindFile(filename));
            }
            int block_a = Catalog[file_i].Indexes[fileblock];
#endif
#if FS_WITH_INDEX_SEQ
            if (Catalog[file_i].StartIndex == -1)
            {
                AddFileBlock(file_i);
            }
            int cur_index = Catalog[file_i].StartIndex;
            int counter = 0;
            while (CellsArray[cur_index].Next != -1)
            {
                if (counter == fileblock)
                {
                    break;
                }
                counter++;
                cur_index = CellsArray[cur_index].Next;
            }
            while (counter < fileblock)
            {
                cur_index = AddFileBlock(file_i);
                counter++;
            }
            int block_a = cur_index;
#endif
            Catalog[file_i].IsOpen = true;
            CellsArray[block_a].Data[offset] = data;
            if (offset == 3)
                Catalog[file_i].IsOpen = false;
        }


#if FS_WITH_INDEX_ENUM
        public static void AddFileBlock(int file)
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

        public static void CreateFile(String FileName)
        {
            Catalog.Add(new CatalogRecord() { Address = Catalog.Count + GlobalConsts.StartCatalogRecords, IsOpen = false, Filename = FileName }); ;
        }
#endif


#if FS_WITH_INDEX_SEQ
        /// <summary>
        /// Добавляет файловый блок к СУЩЕСТВУЮЩЕМУ файлу
        /// </summary>
        /// <param name="file">номер файла в каталоге(индексация с 0)</param>
        /// <returns>если файла нет, вызывай CreateFile()</returns>
        public static int AddFileBlock(int file)
        {
            if (file + 1 <= Catalog.Count)
            {
                int CurrentFileBlock = Catalog[file].StartIndex;
                if (Catalog[file].StartIndex != -1)
                {
                    //ищем самую последнюю ячейку(Next= -1)
                    for (; ; )
                    {
                        if (CellsArray[CurrentFileBlock].Next == -1)
                        {
                            break;
                        }
                        CurrentFileBlock = CellsArray[CurrentFileBlock].Next;
                    }

                    //нашли, ставим адрес Next найденой свободной
                    CellsArray[CurrentFileBlock].Next = FindFreeFileBlock();
                    CellsArray[CellsArray[CurrentFileBlock].Next].IsFree = false;
                    Catalog[file].FileSize++;
                    return CellsArray[CurrentFileBlock].Next;
                }
                if (Catalog[file].StartIndex == -1)
                {
                    Catalog[file].StartIndex = FindFreeFileBlock();
                    CellsArray[Catalog[file].StartIndex].IsFree = false;
                    Catalog[file].FileSize++;
                    return Catalog[file].StartIndex;
                }
            }
            return -1;//файла нет

        }

        /// <summary>
        /// Ищет свободную ячейку на HDD и возвращает ее адрес
        /// </summary>
        /// <returns>адрес свободной ячейки</returns>
        public static int FindFreeFileBlock()
        {
            for (int i = 0; i < GlobalConsts.StartSwapArea; i++)
            {
                if (CellsArray[i].IsFree == true)
                {
                    return i;
                }
            }
            return -2;//не найдено
        }

        public static void CreateFile(String FileName)
        {
            Catalog.Add(new CatalogRecord() { Address = Catalog.Count + GlobalConsts.StartCatalogRecords, IsOpen = false, StartIndex = -1, Filename = FileName }); ;
        }
#endif
    }
}