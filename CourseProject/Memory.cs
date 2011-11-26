using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace OS
{
    /// <summary>
    /// Класс основной памяти (ОЗУ)
    /// </summary>
    public static class Memory
    {
        /// <summary>
        /// Массив страниц, вся основная память
        /// </summary>
        public static TipObiektaVPamyati[] Stranizi = new TipObiektaVPamyati[GlobalConsts.PagesCount + GlobalConsts.CountOfGroup + GlobalConsts.PagesAreaSize];

#if ClockWithTwoArrows
        /// <summary>
        /// Абсолютный адрес дескриптора, страницу которой мы будем замещать
        /// </summary>
        public static int CandidatForOverride=GlobalConsts.StartAddressDescriptionPage;
#endif

#if FIFO || FIFO_SC
        /// <summary>
        /// Очередь для FIFO
        /// </summary>
        public static Queue FIFOQueue = new Queue(GlobalConsts.PagesAreaSize);
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
        public static int Arrow = GlobalConsts.StartAddressDescriptionPage;
#endif

        /// <summary>
        /// Инициализация начальных переменных
        /// </summary>
        static Memory()
        {
            //первый дескриптор таблицы
            Stranizi[0] = new TableDescriptor()
            {
                TargetAddress = 3,
                GroupSize = GlobalConsts.SizesOfGroup[0],
                Address = 0
            };

            //второй дескриптор таблицы
            Stranizi[1] = new TableDescriptor()
            {
                TargetAddress = (Stranizi[0] as TableDescriptor).TargetAddress + (Stranizi[0] as TableDescriptor).GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[1],
                Address = 1
            };

            //третий дескриптор таблицы
            Stranizi[2] = new TableDescriptor()
            {
                TargetAddress = (Stranizi[1] as TableDescriptor).TargetAddress + (Stranizi[1] as TableDescriptor).GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[2],
                Address = 2
            };

            //инициализируем дескрипторы страниц
            for (int i = GlobalConsts.CountOfGroup; i < GlobalConsts.CountOfGroup + GlobalConsts.PagesCount; i++)
            {
                Stranizi[i] = new PageDescriptor()
                {
                    Present = false,
                    AddressVOP = -1,
                    Mutex = false,
                    AddresVPamyati = -1,
                    Address = i
#if (WS || WSClock)
                    ,VozrastStranizi=0
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
            for (int i = GlobalConsts.StartAddressAreaOfPages; i < GlobalConsts.StartAddressAreaOfPages + GlobalConsts.PagesAreaSize; i++)
            {
                Stranizi[i] = new Page()
                {
                    Data = new byte[GlobalConsts.PageSize],
                    Address = i
                };
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    (Stranizi[i] as Page).Data[j] = 0;
                }
            }

            // демо пример: первая таблица уже проинициализирована
            for (int i = GlobalConsts.CountOfGroup; i < GlobalConsts.CountOfGroup + GlobalConsts.SizesOfGroup[0]; i++)
            {
                (Stranizi[i] as PageDescriptor).Present = true;
                (Stranizi[i] as PageDescriptor).AddressVOP = NaitiSvobodnyuStarnicy();
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    (Stranizi[(Stranizi[i] as PageDescriptor).AddressVOP] as Page).Zanyato = true;
                    (Stranizi[(Stranizi[i] as PageDescriptor).AddressVOP] as Page).Data[j] = (byte)Program.RND.Next(0, 256);
                }
            }
        }

        /// <summary>
        /// Поиск свободной ячейки ОЗУ
        /// </summary>
        /// <returns>Вызвращает незанятую ячейку, либо -1, если не найдена</returns>
        private static int NaitiSvobodnyuStarnicy()
        {
            for (int i = GlobalConsts.StartAddressAreaOfPages; i < Stranizi.Length; i++)
                if (!(Stranizi[i] as Page).Zanyato)
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
        public static bool ProveritMutex(int table, int descriptor, int offset, bool write)
        {
            // проверка, инициализирована ли страница
            if (((Stranizi[(Stranizi[table] as TableDescriptor).TargetAddress + descriptor] as PageDescriptor).AddressVOP == -1) && ((Stranizi[(Stranizi[table] as TableDescriptor).TargetAddress + descriptor] as PageDescriptor).AddresVPamyati == -1) && !write)
            {
                return false;
            }
            int desc_a = (Stranizi[table] as TableDescriptor).TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, занята ли страница другим процессом
            if (((Stranizi[desc_a] as PageDescriptor).Mutex) && (offset == 0))
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
        public static byte ChitatByte(int table, int descriptor, int offset, byte data)
        {
            data = 0;
            int desc_a = (Stranizi[table] as TableDescriptor).TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, находится ли страница в ФП
            if (!(Stranizi[desc_a] as PageDescriptor).Present)
            {
                // освобождаем место для страницы
                VigruzitVPodcach(AlgoritmPeremesheniya());
                // восстанавливаем страницу со swap
                VostanovitIzPodkach(desc_a);
#if FIFO || FIFO_SC
                //заносим элемент в очередь
                if (offset == 0)
                {
                    FIFOQueue.Enqueue(Pages[desc_a] as PageDescriptor);
                }
#endif
            }
            // проверки пройдены, считываем байт
            (Stranizi[desc_a] as PageDescriptor).Mutex = true;
#if (WSClock || NFU || FIFO_SC || LRU || ClockWithOneArrow || ClockWithTwoArrows)
            (Pages[desc_a] as PageDescriptor).Access = true;
#endif
            data = (Stranizi[(Stranizi[desc_a] as PageDescriptor).AddressVOP] as Page).Data[offset];
            if (offset == 3)
                (Stranizi[desc_a] as PageDescriptor).Mutex = false;
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
        public static void PisatByte(int table, int descriptor, int offset, byte data)
        {
            int desc_a = (Stranizi[table] as TableDescriptor).TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // если нет в памяти
            if (!(Stranizi[desc_a] as PageDescriptor).Present)
            {
                // если нет в ФП, то дескриптор свободный
                if ((Stranizi[desc_a] as PageDescriptor).AddresVPamyati == -1)
                {
                    // если нет свободной страницы в памяти
                    if (NaitiSvobodnyuStarnicy() == -1)
                    {
                        // выгружаем страницу на swap
                        VigruzitVPodcach(AlgoritmPeremesheniya());
                        (Stranizi[desc_a] as PageDescriptor).AddressVOP = NaitiSvobodnyuStarnicy();
                    }
                    // если есть свободная страница
                    else
                    {
                        (Stranizi[desc_a] as PageDescriptor).AddressVOP = NaitiSvobodnyuStarnicy(); // присваиваем ссылку дескриптору
                    }
#if FIFO || FIFO_SC
                    //заносим элемент в очередь
                    if (offset == 0)
                    {
                        FIFOQueue.Enqueue(Pages[desc_a] as PageDescriptor);
                    }
#endif
                }
                // иначе страница в ФП
                else
                {
                    // освобождаем место для страницы
                    VigruzitVPodcach(AlgoritmPeremesheniya());
                    // восстанавливаем страницу со swap
                    VostanovitIzPodkach(desc_a);
#if FIFO || FIFO_SC
                    //заносим элемент в очередь
                    if (offset == 0)
                    {
                        FIFOQueue.Enqueue(Pages[desc_a] as PageDescriptor);
                    }
#endif
                }
            }

            // проверки пройдены, пишем байт
            (Stranizi[desc_a] as PageDescriptor).Present = true; // признак того, что страница в памяти
            (Stranizi[desc_a] as PageDescriptor).Mutex = true;

#if (WSClock ||NFU || FIFO_SC ||LRU || ClockWithOneArrow || ClockWithTwoArrows)
            (Pages[desc_a] as PageDescriptor).Access = true;
#endif
            (Stranizi[(Stranizi[desc_a] as PageDescriptor).AddressVOP] as Page).Data[offset] = data;
            (Stranizi[(Memory.Stranizi[desc_a] as PageDescriptor).AddressVOP] as Page).Zanyato = true;
            if (offset == 3)
                (Stranizi[desc_a] as PageDescriptor).Mutex = false;
        }

        /// <summary>
        /// Выгружает страницу из ОП.
        /// </summary>
        /// <param name="UPageDescriptor">Абсолютный адрес дескриптора</param>
        /// <returns>Адрес, куда будет перемещает страница в ФП(-1 - неудача)</returns>
        private static int VigruzitVPodcach(int AbsoluteAddress)
        {
            //проверяем, на валидность
            if (AbsoluteAddress != -1)
            {
                //смотрим в дескрипторе адрес на свапе(два случая -1 и x)
                int AddressInSwap = (Stranizi[AbsoluteAddress] as PageDescriptor).AddresVPamyati;
                //попытка слить на свап несуществующую страницу или которой нет в ОП
                if ((Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP == -1 && (Stranizi[AbsoluteAddress] as PageDescriptor).Present == false)
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
                        if (HDD.MassivYacheek[i].Svobodno == true)
                        {
                            //копируем данные
                            for (int j = 0; j < GlobalConsts.PageSize; j++)
                            {
                                HDD.MassivYacheek[i].Data[j] = (Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Data[j];
                            }
                            //обнуляем страницу в памяти
                            for (int j = 0; j < (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Data.Length; j++)
                            {
                                (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Data[j] = 0;
                            }
                            //скидываем адрес в массиве страниц в ОП
                            (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Zanyato = false;
                            (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).AddressVOP = -1;
                            HDD.MassivYacheek[i].Svobodno = false;
                            //ставим бит присутствия в 0
                            (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).Present = false;
                            (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).AddresVPamyati = i;
                            //выходим с адресом, куда записали
                            return i;
                        }
                    }
                }
                //если страница есть на swap'e
                else
                {
                    (Stranizi[AbsoluteAddress] as PageDescriptor).Present = false;
                    //копируем данные
                    for (int j = 0; j < GlobalConsts.PageSize; j++)
                    {
                        HDD.MassivYacheek[AddressInSwap].Data[j] = (Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Data[j];
                    }
                    //обнуляем страницу в памяти
                    for (int j = 0; j < (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Data.Length; j++)
                    {
                        (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Data[j] = 0;
                    }
                    //скидываем адрес в массиве страниц в ОП
                    (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP] as Page).Zanyato = false;
                    (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).AddressVOP = -1;
                    //ставим бит присутствия в 0
                    (Memory.Stranizi[(Stranizi[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).Present = false;
                    return (Stranizi[AbsoluteAddress] as PageDescriptor).AddresVPamyati;
                }
            }
            return -1;
        }

        /// <summary>
        /// Функция восстанавливает в ОП из Swap
        /// </summary>
        /// <param name="AbsoluteAddress">абсолютный адрес дескриптор страницы</param>
        /// <returns>возвращает -1 в случае неудачи и адрес страницы в ОП, куда записали</returns>
        private static int VostanovitIzPodkach(int AbsoluteAddress)
        {
            //проверяем, на валидность
            if (AbsoluteAddress != -1)
            {
                //ищем свободное место в ОП
                int WhereIs = NaitiSvobodnyuStarnicy();
                //если все заебись
                if (WhereIs != -1 && (Stranizi[AbsoluteAddress] as PageDescriptor).AddresVPamyati != -1 && (Stranizi[AbsoluteAddress] as PageDescriptor).Present == false)
                {
                    //копируем страницу в ОП
                    for (int i = 0; i < GlobalConsts.PageSize; i++)
                    {
                        (Stranizi[WhereIs] as Page).Data[i] = HDD.MassivYacheek[(Stranizi[AbsoluteAddress] as PageDescriptor).AddresVPamyati].Data[i];
                    }
                    //выставляем параметры присутствия в дескрипторе и прочее
                    (Stranizi[AbsoluteAddress] as PageDescriptor).Present = true;
                    (Stranizi[AbsoluteAddress] as PageDescriptor).AddressVOP = WhereIs;
                    (Stranizi[WhereIs] as Page).Zanyato = true;
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
            //for (int i = CandidatForOverride; i < GlobalConsts.StartAddressAreaOfPages; i++)
            //{
            //    if ((Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Present == true)
            //    {
            //        CandidatForOverride = i;
            //        return i;
            //    }
            //}
            ////если не нашли раньше, будет выполняться этот кусок, который найдет до текущего
            //for (int i = GlobalConsts.StartAddressDescriptionPage; i < CandidatForOverride; i++)
            //{
            //    if ((Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Present == true)
            //    {
            //        CandidatForOverride = i;
            //        return i;
            //    }
            //}
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
                if (Arrow == GlobalConsts.StartAddressAreaOfPages)
                {
                    //если мы прошли круг но не удалили, снижаем критерий тау
                    if (local_circle == true)
                    {
                        tau--;
                        delta++;
                    }
                    Arrow = GlobalConsts.StartAddressDescriptionPage;
                    local_circle = true;
                }
                //скидываем бит access если он =1 и страница не подходит для удаления
                if ((Pages[Arrow] as PageDescriptor).Present == true && (Pages[Arrow] as PageDescriptor).Access == true && (Pages[Arrow] as PageDescriptor).Mutex == false)
                {
                    (Pages[Arrow] as PageDescriptor).Access = false;
                }
                //если стрелка указывает на подходящую для удаления страницу - удаляем
                if ((Pages[Arrow] as PageDescriptor).AgeOfPage >= tau &&  
                    (Pages[Arrow] as PageDescriptor).Present == true && 
                    (Pages[Arrow] as PageDescriptor).Access == false &&
                    (Pages[Arrow] as PageDescriptor).Mutex == false)
                {
                    (Pages[Arrow] as PageDescriptor).AgeOfPage = 0;
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
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                if ((Pages[i] as PageDescriptor).Mutex == true && (Pages[i] as PageDescriptor).Access == true && (Pages[i] as PageDescriptor).Present == true)
                {
                    (Pages[i] as PageDescriptor).AgeOfPage = 0;
                    //(Pages[i] as PageDescriptor).Access = false;
                }
                else
                {
                    if ((Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Access == false && (Pages[i] as PageDescriptor).Mutex == false)
                    {
                        (Pages[i] as PageDescriptor).AgeOfPage++;
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
        public static int AlgoritmPeremesheniya()
        {
            //определяем возраст для каждой страницы
            int delta = 0;
            int MaxAge = 0;
            int AddressMaxPage=0;
            bool IsFinded = false;
            for (; ; )
            {
                //ищем самую старую страницу
                for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
                {
                    //удовлетворяет ли страница общим критериям?
                    if ((Stranizi[i] as PageDescriptor).VozrastStranizi >= tau  && (Stranizi[i] as PageDescriptor).Present == true && (Stranizi[i] as PageDescriptor).Mutex==false)
                    {
                        //больше ли самой старой?
                        if ((Stranizi[i] as PageDescriptor).VozrastStranizi > MaxAge)
                        {
                            MaxAge = (Stranizi[i] as PageDescriptor).VozrastStranizi;
                            AddressMaxPage = i;
                            IsFinded = true;
                        }
                    }
                }
                //если нашли, выкидываем, восстанавливаем критерий
                if (IsFinded == true)
                {
                    IsFinded = false;
                    (Stranizi[AddressMaxPage] as PageDescriptor).VozrastStranizi = 0;
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
        public static int UvelichitVozrast()
        {
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                //если занято, возраст = 0
                if ((Stranizi[i] as PageDescriptor).Mutex == true  && (Stranizi[i] as PageDescriptor).Present == true)
                {
                    (Stranizi[i] as PageDescriptor).VozrastStranizi = 0;
                }
                //иначе если не занято то ++
                else
                {
                    if ((Stranizi[i] as PageDescriptor).Present == true  && (Stranizi[i] as PageDescriptor).Mutex == false)
                    {
                        (Stranizi[i] as PageDescriptor).VozrastStranizi++;
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
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                if ((Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Access == false && (Pages[i] as PageDescriptor).Mutex == false)
                {
                    if ((Pages[i] as PageDescriptor).Counter < MinCounter)
                    {
                        MinCounter = (Pages[i] as PageDescriptor).Counter;
                        MinAddress = i;
                    }
                }
            }
            (Pages[MinAddress] as PageDescriptor).Counter = 0;
            return MinAddress;
        }

        /// <summary>
        /// Обновляем состояние дескрипторов
        /// </summary>
        /// <returns></returns>
        public static void RefreshDescriptorsState()
        {
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                //если к страничке обращаются, увеличиваем счетчики сбрасываем access
                if ((Pages[i] as PageDescriptor).Access == true)
                {
                    (Pages[i] as PageDescriptor).Access = false;
                    (Pages[i] as PageDescriptor).Counter++;
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
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                if ((Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Access == false && (Pages[i] as PageDescriptor).Mutex == false)
                {
                    if ((Pages[i] as PageDescriptor).Counter < MinCounter)
                    {
                        MinCounter = (Pages[i] as PageDescriptor).Counter;
                        MinAddress = i;
                    }
                }
            }
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                (Pages[i] as PageDescriptor).Counter = 0;
            }
            return MinAddress;
        }

        /// <summary>
        /// Обновляем состояние дескрипторов
        /// </summary>
        /// <returns></returns>
        public static void RefreshDescriptorsState()
        {
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                //если к страничке обращаются, увеличиваем счетчики сбрасываем access
                if ((Pages[i] as PageDescriptor).Access == true)
                {
                    (Pages[i] as PageDescriptor).Access = false;
                    (Pages[i] as PageDescriptor).Counter++;
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
            //for (; ; )
            //{
            //    for (int i = CandidatForOverride; i < GlobalConsts.StartAddressAreaOfPages; i++)
            //    {
            //        if ((Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Access == false)
            //        {
            //            CandidatForOverride = i;
            //            return i;
            //        }
            //        if ((Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Access == true)
            //        {
            //            (Pages[i] as PageDescriptor).Access = false;
            //        }
            //    }
            //    //если не нашли раньше, будет выполняться этот кусок, который найдет до текущего
            //    for (int i = GlobalConsts.StartAddressDescriptionPage; i < CandidatForOverride; i++)
            //    {
            //        if ((Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Access == false)
            //        {
            //            CandidatForOverride = i;
            //            return i;
            //        }
            //        if ((Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Access == true)
            //        {
            //            (Pages[i] as PageDescriptor).Access = false;
            //        }
            //    }
            //}
            //сюда не должно доходить
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
                for (int i = Arrow; i < GlobalConsts.StartAddressAreaOfPages; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if ((Pages[i] as PageDescriptor).Present == true &&
                        (Pages[i] as PageDescriptor).Mutex == false &&
                        (Pages[i] as PageDescriptor).Access == false)
                    {
                        Arrow = i;
                        return i;
                    }
                    //если у страницы бит Access=1, скидываем
                    if ((Pages[i] as PageDescriptor).Present == true &&
                        (Pages[i] as PageDescriptor).Mutex == false &&
                        (Pages[i] as PageDescriptor).Access == true)
                    {
                        (Pages[i] as PageDescriptor).Access = false;
                    }
                }
                //идем от начала до стрелки
                for (int i = GlobalConsts.StartAddressDescriptionPage; i < Arrow; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if ((Pages[i] as PageDescriptor).Present == true &&
                        (Pages[i] as PageDescriptor).Mutex == false &&
                        (Pages[i] as PageDescriptor).Access == false)
                    {
                        Arrow = i;
                        return i;
                    }
                    //если у страницы бит Access=1, скидываем
                    if ((Pages[i] as PageDescriptor).Present == true &&
                        (Pages[i] as PageDescriptor).Mutex == false &&
                        (Pages[i] as PageDescriptor).Access == true)
                    {
                        (Pages[i] as PageDescriptor).Access = false;
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
            if ((Pages[Arrow] as PageDescriptor).Present == true && (Pages[Arrow] as PageDescriptor).Mutex == false && (Pages[Arrow] as PageDescriptor).Access == true)
                (Pages[Arrow] as PageDescriptor).Access = false;
            Arrow = Arrow == GlobalConsts.StartAddressAreaOfPages - 1 ? GlobalConsts.StartAddressDescriptionPage : ++Arrow;
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
                for (int i = CandidatForOverride; i < GlobalConsts.StartAddressAreaOfPages; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if ((Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Access == false)
                    {
                        CandidatForOverride = i + 1;
                        return i;
                    }
                }
                //идем от начала до стрелки
                for (int i = GlobalConsts.StartAddressDescriptionPage; i < CandidatForOverride; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if ((Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Mutex == false && (Pages[i] as PageDescriptor).Access == false)
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
