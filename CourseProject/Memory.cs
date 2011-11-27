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
        public static YacheykaPamyati[] Stranici = new YacheykaPamyati[GlobalConsts.PagesCount + GlobalConsts.CountOfGroup + GlobalConsts.PagesAreaSize];

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
        public static int Strelka = GlobalConsts.StartAddressDescriptionPage;
#endif

        /// <summary>
        /// Инициализация начальных переменных
        /// </summary>
        static Memory()
        {
            //первый дескриптор таблицы
            Stranici[0] = new TablicaDes()
            {
                Ssilka = 3,
                GroupSize = GlobalConsts.SizesOfGroup[0],
                Address = 0
            };

            //второй дескриптор таблицы
            Stranici[1] = new TablicaDes()
            {
                Ssilka = (Stranici[0] as TablicaDes).Ssilka + (Stranici[0] as TablicaDes).GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[1],
                Address = 1
            };

            //третий дескриптор таблицы
            Stranici[2] = new TablicaDes()
            {
                Ssilka = (Stranici[1] as TablicaDes).Ssilka + (Stranici[1] as TablicaDes).GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[2],
                Address = 2
            };

            //инициализируем дескрипторы страниц
            for (int i = GlobalConsts.CountOfGroup; i < GlobalConsts.CountOfGroup + GlobalConsts.PagesCount; i++)
            {
                Stranici[i] = new Des()
                {
                    P = false,
                    Ssilka = -1,
                    M = false,
                    SsailkaHDD = -1,
                    Address = i
#if (WS || WSClock)
                    ,AgeOfPage=0
#endif
#if (WSClock || FIFO_SC || LRU || NFU || ClockWithOneArrow)
,
                    A=false
#endif
#if NFU||LRU
                    ,Counter = 0
#endif
                   
                };
            }

            //инициализируем память под страницы
            for (int i = GlobalConsts.StartAddressAreaOfPages; i < GlobalConsts.StartAddressAreaOfPages + GlobalConsts.PagesAreaSize; i++)
            {
                Stranici[i] = new Str()
                {
                    Data = new byte[GlobalConsts.PageSize],
                    Address = i
                };
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    (Stranici[i] as Str).Data[j] = 0;
                }
            }

            // демо пример: первая таблица уже проинициализирована
            for (int i = GlobalConsts.CountOfGroup; i < GlobalConsts.CountOfGroup + GlobalConsts.SizesOfGroup[0]; i++)
            {
                (Stranici[i] as Des).P = true;
                (Stranici[i] as Des).Ssilka = PoicsSvobodnogoMEsta();
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    (Stranici[(Stranici[i] as Des).Ssilka] as Str).Zanyata = true;
                    (Stranici[(Stranici[i] as Des).Ssilka] as Str).Data[j] = (byte)Program.RND.Next(0, 256);
                }
            }
        }

        /// <summary>
        /// Поиск свободной ячейки ОЗУ
        /// </summary>
        /// <returns>Вызвращает незанятую ячейку, либо -1, если не найдена</returns>
        private static int PoicsSvobodnogoMEsta()
        {
            for (int i = GlobalConsts.StartAddressAreaOfPages; i < Stranici.Length; i++)
                if (!(Stranici[i] as Str).Zanyata)
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
        public static bool UstanovlenMutex(int table, int descriptor, int offset, bool write)
        {
            // проверка, инициализирована ли страница
            if (((Stranici[(Stranici[table] as TablicaDes).Ssilka + descriptor] as Des).Ssilka == -1) && ((Stranici[(Stranici[table] as TablicaDes).Ssilka + descriptor] as Des).SsailkaHDD == -1) && !write)
            {
                return false;
            }
            int desc_a = (Stranici[table] as TablicaDes).Ssilka + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, занята ли страница другим процессом
            if (((Stranici[desc_a] as Des).M) && (offset == 0))
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
        public static byte Chtenie(int table, int descriptor, int offset, byte data)
        {
            data = 0;
            int desc_a = (Stranici[table] as TablicaDes).Ssilka + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, находится ли страница в ФП
            if (!(Stranici[desc_a] as Des).P)
            {
                // освобождаем место для страницы
                Vitesnit(KakuuStrVitesnyat());
                // восстанавливаем страницу со swap
                Vosstanovit(desc_a);
#if FIFO || FIFO_SC
                //заносим элемент в очередь
                if (offset == 0)
                {
                    FIFOQueue.Enqueue(Pages[desc_a] as PageDescriptor);
                }
#endif
            }
            // проверки пройдены, считываем байт
            (Stranici[desc_a] as Des).M = true;
#if (WSClock || NFU || FIFO_SC || LRU || ClockWithOneArrow || ClockWithTwoArrows)
            (Stranici[desc_a] as Des).A = true;
#endif
            data = (Stranici[(Stranici[desc_a] as Des).Ssilka] as Str).Data[offset];
            if (offset == 3)
                (Stranici[desc_a] as Des).M = false;
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
        public static void Zapis(int table, int descriptor, int offset, byte data)
        {
            int desc_a = (Stranici[table] as TablicaDes).Ssilka + descriptor; // Получение абсолютного адреса дескриптора
            // если нет в памяти
            if (!(Stranici[desc_a] as Des).P)
            {
                // если нет в ФП, то дескриптор свободный
                if ((Stranici[desc_a] as Des).SsailkaHDD == -1)
                {
                    // если нет свободной страницы в памяти
                    if (PoicsSvobodnogoMEsta() == -1)
                    {
                        // выгружаем страницу на swap
                        Vitesnit(KakuuStrVitesnyat());
                        (Stranici[desc_a] as Des).Ssilka = PoicsSvobodnogoMEsta();
                    }
                    // если есть свободная страница
                    else
                    {
                        (Stranici[desc_a] as Des).Ssilka = PoicsSvobodnogoMEsta(); // присваиваем ссылку дескриптору
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
                    Vitesnit(KakuuStrVitesnyat());
                    // восстанавливаем страницу со swap
                    Vosstanovit(desc_a);
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
            (Stranici[desc_a] as Des).P = true; // признак того, что страница в памяти
            (Stranici[desc_a] as Des).M = true;

#if (WSClock ||NFU || FIFO_SC ||LRU || ClockWithOneArrow || ClockWithTwoArrows)
            (Stranici[desc_a] as Des).A = true;
#endif
            (Stranici[(Stranici[desc_a] as Des).Ssilka] as Str).Data[offset] = data;
            (Stranici[(Memory.Stranici[desc_a] as Des).Ssilka] as Str).Zanyata = true;
            if (offset == 3)
                (Stranici[desc_a] as Des).M = false;
        }

        /// <summary>
        /// Выгружает страницу из ОП.
        /// </summary>
        /// <param name="UPageDescriptor">Абсолютный адрес дескриптора</param>
        /// <returns>Адрес, куда будет перемещает страница в ФП(-1 - неудача)</returns>
        private static int Vitesnit(int AbsoluteAddress)
        {
            //проверяем, на валидность
            if (AbsoluteAddress != -1)
            {
                //смотрим в дескрипторе адрес на свапе(два случая -1 и x)
                int AddressInSwap = (Stranici[AbsoluteAddress] as Des).SsailkaHDD;
                //попытка слить на свап несуществующую страницу или которой нет в ОП
                if ((Stranici[AbsoluteAddress] as Des).Ssilka == -1 && (Stranici[AbsoluteAddress] as Des).P == false)
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
                        if (HDD.Yacheyki[i].Svobodna == true)
                        {
                            //копируем данные
                            for (int j = 0; j < GlobalConsts.PageSize; j++)
                            {
                                HDD.Yacheyki[i].Data[j] = (Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Data[j];
                            }
                            //обнуляем страницу в памяти
                            for (int j = 0; j < (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Data.Length; j++)
                            {
                                (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Data[j] = 0;
                            }
                            //скидываем адрес в массиве страниц в ОП
                            (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Zanyata = false;
                            (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Address] as Des).Ssilka = -1;
                            HDD.Yacheyki[i].Svobodna = false;
                            //ставим бит присутствия в 0
                            (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Address] as Des).P = false;
                            (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Address] as Des).SsailkaHDD = i;
                            //выходим с адресом, куда записали
                            return i;
                        }
                    }
                }
                //если страница есть на swap'e
                else
                {
                    (Stranici[AbsoluteAddress] as Des).P = false;
                    //копируем данные
                    for (int j = 0; j < GlobalConsts.PageSize; j++)
                    {
                        HDD.Yacheyki[AddressInSwap].Data[j] = (Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Data[j];
                    }
                    //обнуляем страницу в памяти
                    for (int j = 0; j < (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Data.Length; j++)
                    {
                        (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Data[j] = 0;
                    }
                    //скидываем адрес в массиве страниц в ОП
                    (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Ssilka] as Str).Zanyata = false;
                    (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Address] as Des).Ssilka = -1;
                    //ставим бит присутствия в 0
                    (Memory.Stranici[(Stranici[AbsoluteAddress] as Des).Address] as Des).P = false;
                    return (Stranici[AbsoluteAddress] as Des).SsailkaHDD;
                }
            }
            return -1;
        }

        /// <summary>
        /// Функция восстанавливает в ОП из Swap
        /// </summary>
        /// <param name="AbsoluteAddress">абсолютный адрес дескриптор страницы</param>
        /// <returns>возвращает -1 в случае неудачи и адрес страницы в ОП, куда записали</returns>
        private static int Vosstanovit(int AbsoluteAddress)
        {
            //проверяем, на валидность
            if (AbsoluteAddress != -1)
            {
                //ищем свободное место в ОП
                int WhereIs = PoicsSvobodnogoMEsta();
                //если все заебись
                if (WhereIs != -1 && (Stranici[AbsoluteAddress] as Des).SsailkaHDD != -1 && (Stranici[AbsoluteAddress] as Des).P == false)
                {
                    //копируем страницу в ОП
                    for (int i = 0; i < GlobalConsts.PageSize; i++)
                    {
                        (Stranici[WhereIs] as Str).Data[i] = HDD.Yacheyki[(Stranici[AbsoluteAddress] as Des).SsailkaHDD].Data[i];
                    }
                    //выставляем параметры присутствия в дескрипторе и прочее
                    (Stranici[AbsoluteAddress] as Des).P = true;
                    (Stranici[AbsoluteAddress] as Des).Ssilka = WhereIs;
                    (Stranici[WhereIs] as Str).Zanyata = true;
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
                for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
                {
                    //удовлетворяет ли страница общим критериям?
                    if ((Pages[i] as PageDescriptor).AgeOfPage >= tau  && (Pages[i] as PageDescriptor).Present == true && (Pages[i] as PageDescriptor).Mutex==false)
                    {
                        //больше ли самой старой?
                        if ((Pages[i] as PageDescriptor).AgeOfPage > MaxAge)
                        {
                            MaxAge = (Pages[i] as PageDescriptor).AgeOfPage;
                            AddressMaxPage = i;
                            IsFinded = true;
                        }
                    }
                }
                //если нашли, выкидываем, восстанавливаем критерий
                if (IsFinded == true)
                {
                    IsFinded = false;
                    (Pages[AddressMaxPage] as PageDescriptor).AgeOfPage = 0;
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
            for (int i = GlobalConsts.StartAddressDescriptionPage; i < GlobalConsts.StartAddressAreaOfPages; i++)
            {
                //если занято, возраст = 0
                if ((Pages[i] as PageDescriptor).Mutex == true  && (Pages[i] as PageDescriptor).Present == true)
                {
                    (Pages[i] as PageDescriptor).AgeOfPage = 0;
                }
                //иначе если не занято то ++
                else
                {
                    if ((Pages[i] as PageDescriptor).Present == true  && (Pages[i] as PageDescriptor).Mutex == false)
                    {
                        (Pages[i] as PageDescriptor).AgeOfPage++;
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
        public static int KakuuStrVitesnyat()
        {
            for (; ; )
            {
                //идем от стрелки до конца
                for (int i = Strelka; i < GlobalConsts.StartAddressAreaOfPages; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if ((Stranici[i] as Des).P == true &&
                        (Stranici[i] as Des).M == false &&
                        (Stranici[i] as Des).A == false)
                    {
                        Strelka = i;
                        return i;
                    }
                    //если у страницы бит Access=1, скидываем
                    if ((Stranici[i] as Des).P == true &&
                        (Stranici[i] as Des).M == false &&
                        (Stranici[i] as Des).A == true)
                    {
                        (Stranici[i] as Des).A = false;
                    }
                }
                //идем от начала до стрелки
                for (int i = GlobalConsts.StartAddressDescriptionPage; i < Strelka; i++)
                {
                    //если страница подходит для замещения - возвращаем адрес
                    if ((Stranici[i] as Des).P == true &&
                        (Stranici[i] as Des).M == false &&
                        (Stranici[i] as Des).A == false)
                    {
                        Strelka = i;
                        return i;
                    }
                    //если у страницы бит Access=1, скидываем
                    if ((Stranici[i] as Des).P == true &&
                        (Stranici[i] as Des).M == false &&
                        (Stranici[i] as Des).A == true)
                    {
                        (Stranici[i] as Des).A = false;
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
