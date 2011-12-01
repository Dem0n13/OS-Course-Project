using System.Collections.Generic;

namespace OS
{
    /// <summary>
    /// Класс основной памяти (ОЗУ)
    /// </summary>
    public static class Memory
    {
        public static TableDescriptor[] TableDess = new TableDescriptor[GlobalConsts.CountOfGroup];
        public static PageDescriptor[] PageDess = new PageDescriptor[GlobalConsts.PagesCount];
        public static Page[] Pages = new Page[GlobalConsts.PagesAreaSize];

#if ClockWithTwoArrows
        /// <summary>
        /// Абсолютный адрес дескриптора, страницу которой мы будем замещать
        /// </summary>
        public static int CandidatForOverride=0;
#endif

#if FIFO || FIFO_SC
        /// <summary>
        /// Очередь для FIFO
        /// </summary>
        public static Queue<PageDescriptor> FIFOQueue = new Queue<PageDescriptor>(GlobalConsts.PagesAreaSize);
#endif

#if WS || WSClock
        /// <summary>
        /// Тау время. Может принимать различные значения
        /// </summary>
        public static int tau = 2;
#endif

#if ClockWithOneArrow || ClockWithTwoArrows|| WSClock
        /// <summary>
        /// Стрелка часов
        /// </summary>
        public static int Arrow = 0;
#endif

        /// <summary>
        /// Инициализация начальных переменных
        /// </summary>
        static Memory()
        {
            //первый дескриптор таблицы
            TableDess[0] = new TableDescriptor()
            {
                TargetAddress = 0,
                GroupSize = GlobalConsts.SizesOfGroup[0],
                Address = 0
            };

            //второй дескриптор таблицы
            TableDess[1] = new TableDescriptor()
            {
                TargetAddress = TableDess[0].TargetAddress + TableDess[0].GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[1],
                Address = 1
            };

            //третий дескриптор таблицы
            TableDess[2] = new TableDescriptor()
            {
                TargetAddress = TableDess[1].TargetAddress + TableDess[1].GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[2],
                Address = 2
            };

            //инициализируем дескрипторы страниц
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                PageDess[i] = new PageDescriptor()
                {
                    Present = false,
                    TargetAddress = -1,
                    Mutex = false,
                    AddressInSwap = -1,
                    Address = i
#if (WS || WSClock)
                    ,AgeOfPage=0
#endif
#if (WSClock || FIFO_SC || LRU || NFU || ClockWithOneArrow)
,
                    Access=false
#endif
#if NFU||LRU
                    ,Counter = 0
#endif
                   
                };
            }

            //инициализируем память под страницы
            for (int i = 0; i < GlobalConsts.PagesAreaSize; i++)
            {
                Pages[i] = new Page()
                {
                    Data = new byte[GlobalConsts.PageSize],
                    Address = i
                };
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    Pages[i].Data[j] = 0;
                }               
            }

            // демо пример: первая таблица уже проинициализирована
            for (int i = 0; i < GlobalConsts.SizesOfGroup[0]; i++)
            {
                PageDess[i].Present = true;
                PageDess[i].TargetAddress = FindFreePage();
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    Pages[PageDess[i].TargetAddress].Dirty = true;
                    Pages[PageDess[i].TargetAddress].Data[j] = (byte)Program.RND.Next(0, 256);
                }
#if FIFO || FIFO_SC
                FIFOQueue.Enqueue(PageDess[i]);
#endif
            }
        }

        /// <summary>
        /// Поиск свободной ячейки ОЗУ
        /// </summary>
        /// <returns>Вызвращает незанятую ячейку, либо -1, если не найдена</returns>
        private static int FindFreePage()
        {
            for (int i = 0; i < Pages.Length; i++)
                if (!Pages[i].Dirty)
                    return i;
            return -1;
        }

        /// <summary>
        /// Проверка страницы на доступность для чтения/записи
        /// </summary>
        /// <param name="table"></param>
        /// <param name="descriptor"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static bool CheckMutex(int table, int descriptor, int offset, bool write)
        {
            // проверка, инициализирована ли страница
            if ((PageDess[TableDess[table].TargetAddress + descriptor].TargetAddress == -1) && ((PageDess[TableDess[table].TargetAddress + descriptor].AddressInSwap == -1) && !write))
            {
                return false;
            }
            int desc_a = TableDess[table].TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, занята ли страница другим процессом
            if ((PageDess[desc_a].Mutex) && (offset == 0))
                return false;
            return true;
        }

        /// <summary>
        /// Считывает байт из памяти, если это возможно
        /// </summary>
        /// <param name="table">Номер таблицы</param>
        /// <param name="descriptor">Номер дескриптора</param>
        /// <param name="offset">Смещение в странице</param>
        /// <param name="data">Буфер для чтения</param>
        /// <returns>true - чтение завершено, false - чтение невозможно</returns>
        public static byte ReadByte(int table, int descriptor, int offset, byte data)
        {
            data = 0;
            int desc_a = TableDess[table].TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, находится ли страница в ФП
            if (!PageDess[desc_a].Present)
            {
                // освобождаем место для страницы
                UnloadToSwap(ReplacementAlgorithm());
                // восстанавливаем страницу со swap
                RestoreFromSwap(desc_a);
#if FIFO || FIFO_SC
                //заносим элемент в очередь
                FIFOQueue.Enqueue(PageDess[desc_a]);
#endif
            }
            // проверки пройдены, считываем байт
            PageDess[desc_a].Mutex = true;
#if (WSClock || NFU || FIFO_SC || LRU || ClockWithOneArrow || ClockWithTwoArrows)
            PageDess[desc_a].Access = true;
#endif
            data = Pages[PageDess[desc_a].TargetAddress].Data[offset];
            if (offset == 3)
                PageDess[desc_a].Mutex = false;
            return data;
        }

        /// <summary>
        /// Записывает байт в память, если это возможно
        /// </summary>
        /// <param name="table">Номер таблицы</param>
        /// <param name="descriptor">Номер дескриптора</param>
        /// <param name="offset">Смещение в странице</param>
        /// <param name="data">Буфер на запись</param>
        /// <returns>true - запись завершена, false - запись невозможна</returns>
        public static void WriteByte(int table, int descriptor, int offset, byte data)
        {
            int desc_a = TableDess[table].TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // если нет в памяти
            if (!PageDess[desc_a].Present)
            {
                // если нет в ФП, то дескриптор свободный
                if (PageDess[desc_a].AddressInSwap == -1)
                {
                    // если нет свободной страницы в памяти
                    if (FindFreePage() == -1)
                    {
                        // выгружаем страницу на swap
                        UnloadToSwap(ReplacementAlgorithm());
                        PageDess[desc_a].TargetAddress = FindFreePage();
                    }
                    // если есть свободная страница
                    else
                    {
                        PageDess[desc_a].TargetAddress = FindFreePage(); // присваиваем ссылку дескриптору
                    }
#if FIFO || FIFO_SC
                    //заносим элемент в очередь
                    if (offset == 0)
                    {
                        FIFOQueue.Enqueue(PageDess[desc_a]);
                    }
#endif
                }
                // иначе страница в ФП
                else
                {
                    // освобождаем место для страницы
                    UnloadToSwap(ReplacementAlgorithm());
                    // восстанавливаем страницу со swap
                    RestoreFromSwap(desc_a);
#if FIFO || FIFO_SC
                    //заносим элемент в очередь
                    if (offset == 0)
                    {
                        FIFOQueue.Enqueue(PageDess[desc_a]);
                    }
#endif
                }
            }

            // проверки пройдены, пишем байт
            PageDess[desc_a].Present = true; // признак того, что страница в памяти
            PageDess[desc_a].Mutex = true;

#if (WSClock ||NFU || FIFO_SC ||LRU || ClockWithOneArrow || ClockWithTwoArrows)
            PageDess[desc_a].Access = true;
#endif
            Pages[PageDess[desc_a] .TargetAddress].Data[offset] = data;
            Pages[PageDess[desc_a].TargetAddress].Dirty = true;
            if (offset == 3)
                PageDess[desc_a].Mutex = false;
        }

        /// <summary>
        /// Выгружает страницу из ОП.
        /// </summary>
        /// <param name="AbsoluteAddress">Абсолютный адрес дескриптора</param>
        /// <returns>Адрес, куда будет перемещает страница в ФП(-1 - неудача)</returns>
        private static int UnloadToSwap(int AbsoluteAddress)
        {
            //проверяем, на валидность
            if (AbsoluteAddress != -1)
            {
                //смотрим в дескрипторе адрес на свапе(два случая -1 и x)
                int AddressInSwap = PageDess[AbsoluteAddress].AddressInSwap;
                //попытка слить на свап несуществующую страницу или которой нет в ОП
                if (PageDess[AbsoluteAddress].TargetAddress == -1 && PageDess[AbsoluteAddress].Present == false)
                {
                    return -1;
                }

                //если страницы нет на swap'e
                if (AddressInSwap == -1)
                {
                    //ищем свободную ячейку в swap
                    for (int i = GlobalConsts.StartSwapArea; i < GlobalConsts.StartSwapArea + GlobalConsts.PagesCount; i++)
                    {
                        //находим первую попавшуюся свободную ячейку на свапе
                        if (HDD.CellsArray[i].IsFree == true)
                        {
                            //копируем данные
                            for (int j = 0; j < GlobalConsts.PageSize; j++)
                            {
                                HDD.CellsArray[i].Data[j] = Pages[PageDess[AbsoluteAddress].TargetAddress].Data[j];
                            }
                            //обнуляем страницу в памяти
                            for (int j = 0; j < Pages[PageDess[AbsoluteAddress].TargetAddress].Data.Length; j++)
                            {
                                Pages[PageDess[AbsoluteAddress].TargetAddress].Data[j] = 0;
                            }
                            //скидываем адрес в массиве страниц в ОП
                            Pages[PageDess[AbsoluteAddress].TargetAddress].Dirty = false;
                            PageDess[AbsoluteAddress].TargetAddress = -1;
                            HDD.CellsArray[i].IsFree = false;
                            //ставим бит присутствия в 0
                            PageDess[AbsoluteAddress].Present = false;
                            PageDess[AbsoluteAddress].AddressInSwap = i;
                            //выходим с адресом, куда записали
                            return i;
                        }
                    }
                }
                //если страница есть на swap'e
                else
                {
                    PageDess[AbsoluteAddress].Present = false;
                    //копируем данные
                    for (int j = 0; j < GlobalConsts.PageSize; j++)
                    {
                        HDD.CellsArray[AddressInSwap].Data[j] = Pages[PageDess[AbsoluteAddress].TargetAddress].Data[j];
                    }
                    //обнуляем страницу в памяти
                    for (int j = 0; j < Pages[PageDess[AbsoluteAddress].TargetAddress].Data.Length; j++)
                    {
                        Pages[PageDess[AbsoluteAddress].TargetAddress].Data[j] = 0;
                    }
                    //скидываем адрес в массиве страниц в ОП
                    Pages[PageDess[AbsoluteAddress].TargetAddress].Dirty = false;
                    PageDess[AbsoluteAddress].TargetAddress = -1;
                    //ставим бит присутствия в 0
                    PageDess[AbsoluteAddress].Present = false;
                    return PageDess[AbsoluteAddress].AddressInSwap;
                }
            }
            return -1;
        }

        /// <summary>
        /// Функция восстанавливает в ОП из Swap
        /// </summary>
        /// <param name="AbsoluteAddress">абсолютный адрес дескриптор страницы</param>
        /// <returns>возвращает -1 в случае неудачи и адрес страницы в ОП, куда записали</returns>
        private static int RestoreFromSwap(int AbsoluteAddress)
        {
            //проверяем, на валидность
            if (AbsoluteAddress != -1)
            {
                //ищем свободное место в ОП
                int WhereIs = FindFreePage();
                if (WhereIs != -1 && PageDess[AbsoluteAddress].AddressInSwap != -1 && PageDess[AbsoluteAddress].Present == false)
                {
                    //копируем страницу в ОП
                    for (int i = 0; i < GlobalConsts.PageSize; i++)
                    {
                        Pages[WhereIs].Data[i] = HDD.CellsArray[PageDess[AbsoluteAddress].AddressInSwap].Data[i];
                    }
                    //выставляем параметры присутствия в дескрипторе и прочее
                    PageDess[AbsoluteAddress].Present = true;
                    PageDess[AbsoluteAddress].TargetAddress = WhereIs;
                    Pages[WhereIs].Dirty = true;
                    return WhereIs;
                }
            }
            return -1;
        }

        #region FIFO
#if FIFO
        /// <summary>
        /// Поиск замещаемого элемента по алгоритму FIFO
        /// </summary>
        /// <returns>Абсолютный адрес дескриптора для замещения</returns>
        public static int ReplacementAlgorithm()
        {
            return (FIFOQueue.Dequeue() as PageDescriptor).Address;
        }
#endif
        #endregion

        #region WSClock
#if WSClock
        /// <summary>
        /// Поиск замещаемого элемента по алгоритму WSClock
        /// </summary>
        /// <returns>Абсолютный адрес дескриптора для замещения</returns>
        public static int ReplacementAlgorithm()
        {
            //идем от стрелки до конца
            bool local_circle=false;
            int delta = 0;
            for (; ; )
            {
                //скидываем стрелку в 0 если круг пройден
                if (Arrow == GlobalConsts.PagesCount)
                {
                    //если мы прошли круг но не удалили, снижаем критерий тау
                    if (local_circle == true)
                    {
                        tau--;
                        delta++;
                    }
                    Arrow = 0;
                    local_circle = true;
                }
                //скидываем бит access если он =1 и страница не подходит для удаления
                if (PageDess[Arrow].Present == true && PageDess[Arrow].Access == true && PageDess[Arrow].Mutex == false)
                {
                    PageDess[Arrow].Access = false;
                }
                //если стрелка указывает на подходящую для удаления страницу - удаляем
                if (PageDess[Arrow].AgeOfPage >= tau &&
                    PageDess[Arrow].Present == true &&
                    PageDess[Arrow].Access == false &&
                    PageDess[Arrow].Mutex == false)
                {
                    PageDess[Arrow].AgeOfPage = 0;
                    Arrow++;
                    tau += delta;
                    return Arrow-1;
                }
                Arrow++;
            }
        }

        /// <summary>
        /// Обновляет возраст страниц с каждым тактом
        /// </summary>
        /// <returns></returns>
        public static int AgesUp()
        {
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                if (PageDess[i].Mutex == true && PageDess[i].Access == true && PageDess[i].Present == true)
                {
                    PageDess[i].AgeOfPage = 0;
                    //PageDess[i].Access = false;
                }
                else
                {
                    if (PageDess[i].Present == true && PageDess[i].Access == false && PageDess[i].Mutex == false)
                    {
                        PageDess[i].AgeOfPage++;
                    }
                }
            }
            return 1;
        }
#endif
        #endregion

        #region WS
#if WS
        /// <summary>
        /// Поиск замещаемого элемента по алгоритму WS
        /// </summary>
        /// <returns>Абсолютный адрес дескриптора для замещения</returns>
        public static int ReplacementAlgorithm()
        {
            //определяем возраст для каждой страницы
            int delta = 0;
            int MaxAge = 0;
            int AddressMaxPage=0;
            bool IsFinded = false;
            for (; ; )
            {
                //ищем самую старую страницу
                for (int i = 0; i < GlobalConsts.PagesCount; i++)
                {
                    //удовлетворяет ли страница общим критериям?
                    if (PageDess[i].AgeOfPage >= tau && PageDess[i].Present == true && PageDess[i].Mutex == false)
                    {
                        //больше ли самой старой?
                        if (PageDess[i].AgeOfPage > MaxAge)
                        {
                            MaxAge = PageDess[i].AgeOfPage;
                            AddressMaxPage = i;
                            IsFinded = true;
                        }
                    }
                }
                //если нашли, выкидываем, восстанавливаем критерий
                if (IsFinded == true)
                {
                    IsFinded = false;
                    PageDess[AddressMaxPage].AgeOfPage = 0;
                    tau += delta;
                    return AddressMaxPage;
                }
                //если не нашли, снижаем tau и повторяем все с самого начала
                tau--;
                delta++;
            }
        }

        /// <summary>
        /// Обновляет возраст страниц с каждым тактом
        /// </summary>
        /// <returns></returns>
        public static int AgesUp()
        {
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                //если занято, возраст = 0
                if (PageDess[i].Mutex == true && PageDess[i].Present == true)
                {
                    PageDess[i].AgeOfPage = 0;
                }
                //иначе если не занято то ++
                else
                {
                    if (PageDess[i].Present == true && PageDess[i].Mutex == false)
                    {
                        PageDess[i].AgeOfPage++;
                    }
                }
            }
            return 1;
        }
#endif
        #endregion

        #region NFU
#if NFU
        /// <summary>
        /// Поиск замещаемого элемента по алгоритму NFU
        /// </summary>
        /// <returns>Абсолютный адрес дескриптора для замещения</returns>
        private static int ReplacementAlgorithm()
        {
            int MinCounter = int.MaxValue;
            int MinAddress = 0;
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                if (PageDess[i].Present == true && PageDess[i].Access == false && PageDess[i].Mutex == false)
                {
                    if (PageDess[i].Counter < MinCounter)
                    {
                        MinCounter = PageDess[i].Counter;
                        MinAddress = i;
                    }
                }
            }
            PageDess[MinAddress].Counter = 0;
            return MinAddress;
        }

        /// <summary>
        /// Обновляем состояние дескрипторов
        /// </summary>
        /// <returns></returns>
        public static void RefreshDescriptorsState()
        {
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                //если к страничке обращаются, увеличиваем счетчики сбрасываем access
                if (PageDess[i].Access == true)
                {
                    PageDess[i].Access = false;
                    PageDess[i].Counter++;
                }
            }
        }
#endif
        #endregion

        #region LRU
#if LRU
        /// <summary>
        /// Поиск замещаемого элемента по алгоритму LRU
        /// </summary>
        /// <returns>Абсолютный адрес дескриптора для замещения</returns>
        public static int ReplacementAlgorithm()
        {
            int MinCounter = int.MaxValue;
            int MinAddress = 0;
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                if (PageDess[i].Present == true && PageDess[i].Access == false && PageDess[i].Mutex == false)
                {
                    if (PageDess[i].Counter < MinCounter)
                    {
                        MinCounter = PageDess[i].Counter;
                        MinAddress = i;
                    }
                }
            }
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                PageDess[i].Counter = 0;
            }
            return MinAddress;
        }

        /// <summary>
        /// Обновляем состояние дескрипторов
        /// </summary>
        /// <returns></returns>
        public static void RefreshDescriptorsState()
        {
            for (int i = 0; i < GlobalConsts.PagesCount; i++)
            {
                //если к страничке обращаются, увеличиваем счетчики сбрасываем access
                if (PageDess[i].Access == true)
                {
                    PageDess[i].Access = false;
                    PageDess[i].Counter++;
                }
            }
        }
#endif
        #endregion

        #region FIFO_SC
#if FIFO_SC
        /// <summary>
        /// Поиск замещаемого элемента по алгоритму FIFO_SC
        /// </summary>
        /// <returns>Абсолютный адрес дескриптора для замещения</returns>
        public static int ReplacementAlgorithm()
        {
            for (; ; )
            {
                if ((FIFOQueue.Peek() as PageDescriptor).Access == true)
                {
                    PageDescriptor buf = (FIFOQueue.Dequeue() as PageDescriptor);
                    buf.Access = false;
                    FIFOQueue.Enqueue(buf);
                }
                else
                {
                    return (FIFOQueue.Dequeue() as PageDescriptor).Address;
                }

            }
            return -1;
        }

        
#endif
        #endregion

        #region ClockWithOneArrow
#if ClockWithOneArrow
        /// <summary>
        /// Поиск замещаемого элемента по алгоритму ClockWithOneArrow
        /// </summary>
        /// <returns></returns>
        public static int ReplacementAlgorithm()
        {
            for (; ; )
            {
                //идем от стрелки до конца
                for (int i = Arrow; i < GlobalConsts.PagesCount; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if (PageDess[i].Present == true &&
                        PageDess[i].Mutex == false &&
                        PageDess[i].Access == false)
                    {
                        Arrow = i;
                        return i;
                    }
                    //если у страницы бит Access=1, скидываем
                    if (PageDess[i].Present == true &&
                        PageDess[i].Mutex == false &&
                        PageDess[i].Access == true)
                    {
                        PageDess[i].Access = false;
                    }
                }
                //идем от начала до стрелки
                for (int i = 0; i < Arrow; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if (PageDess[i].Present == true &&
                        PageDess[i].Mutex == false &&
                        PageDess[i].Access == false)
                    {
                        Arrow = i;
                        return i;
                    }
                    //если у страницы бит Access=1, скидываем
                    if (PageDess[i].Present == true &&
                        PageDess[i].Mutex == false &&
                        PageDess[i].Access == true)
                    {
                        PageDess[i].Access = false;
                    }
                }
            }
        }
#endif
        #endregion

        #region ClockWithTwoArrows
#if ClockWithTwoArrows
        /// <summary>
        /// Движение стрелки, сбрасывающей бит доступа
        /// </summary>
        public static void GoArrow()
        {
            if (PageDess[Arrow].Present == true && PageDess[Arrow].Mutex == false && PageDess[Arrow].Access == true)
                PageDess[Arrow].Access = false;
            Arrow = Arrow == GlobalConsts.PagesCount - 1 ? 0 : ++Arrow;
        }

        /// <summary>
        /// Поиск замещаемого элемента по часовому алгоритму с двумя стрелками
        /// </summary>
        /// <returns></returns>
        public static int ReplacementAlgorithm()
        {
            while (true)
            {
                //идем от стрелки до конца
                for (int i = CandidatForOverride; i < GlobalConsts.PagesCount; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if (PageDess[i].Present == true && PageDess[i].Mutex == false && PageDess[i].Access == false)
                    {
                        CandidatForOverride = i + 1;
                        return i;
                    }
                }
                //идем от начала до стрелки
                for (int i = 0; i < CandidatForOverride; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if (PageDess[i].Present == true && PageDess[i].Mutex == false && PageDess[i].Access == false)
                    {
                        CandidatForOverride = i + 1;
                        return i;
                    }
                }
            }
        }
#endif
        #endregion
    }
}
