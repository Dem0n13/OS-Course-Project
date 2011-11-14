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
        public static IMemoryPage[] Pages = new IMemoryPage[GlobalConsts.PagesCount + GlobalConsts.CountOfGroup + GlobalConsts.PagesAreaSize];

        /// <summary>
        /// Инициализация начальных переменных
        /// </summary>
        static Memory()
        {
            //первый дескриптор таблицы
            Pages[0] = new TableDescriptor()
            {
                TargetAddress = 3,
                GroupSize = GlobalConsts.SizesOfGroup[0],
                Address = 0
            };

            //второй дескриптор таблицы
            Pages[1] = new TableDescriptor()
            {
                TargetAddress = (Pages[0] as TableDescriptor).TargetAddress + (Pages[0] as TableDescriptor).GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[1],
                Address = 1
            };

            //третий дескриптор таблицы
            Pages[2] = new TableDescriptor()
            {
                TargetAddress = (Pages[1] as TableDescriptor).TargetAddress + (Pages[1] as TableDescriptor).GroupSize,
                GroupSize = GlobalConsts.SizesOfGroup[2],
                Address = 2
            };

            //инициализируем дескрипторы страниц
            for (int i = GlobalConsts.CountOfGroup; i < GlobalConsts.CountOfGroup + GlobalConsts.PagesCount; i++)
            {
                Pages[i] = new PageDescriptor()
                {
                    Present = false,
                    TargetAddress = -1,
                    Mutex = false,
                    AddressInSwap = -1,
                    Address = i,
                    Access=false,
                    Counter = 0
                };
            }

            //инициализируем память под страницы
            for (int i = GlobalConsts.StartAddressAreaOfPages; i < GlobalConsts.StartAddressAreaOfPages + GlobalConsts.PagesAreaSize; i++)
            {
                Pages[i] = new Page()
                {
                    Data = new byte[GlobalConsts.PageSize],
                    Address = i
                };
                for (int j = 0; j < GlobalConsts.PageSize; j++)
                {
                    (Pages[i] as Page).Data[j] = 0;
                }
            }
        }

        /// <summary>
        /// Поиск свободной ячейки ОЗУ
        /// </summary>
        /// <returns>Вызвращает незанятую ячейку, либо -1, если не найдена</returns>
        private static int FindFreePage()
        {
            for (int i = GlobalConsts.StartAddressAreaOfPages; i < Pages.Length; i++)
                if (!(Pages[i] as Page).Dirty)
                    return i;
            return -1;
        }

        /// <summary>
        /// Проверка страницы на доступность для чтения/записи
        /// </summary>
        /// <param name="table">Номер таблицы</param>
        /// <param name="descriptor">Номер дескриптора</param>
        /// <param name="offset">Смещение в странице</param>
        /// <returns></returns>
        public static bool CheckMutex(int table, int descriptor, int offset, bool write)
        {
            // проверка, инициализирована ли страница
            if (((Pages[(Pages[table] as TableDescriptor).TargetAddress + descriptor] as PageDescriptor).TargetAddress == -1) && ((Pages[(Pages[table] as TableDescriptor).TargetAddress + descriptor] as PageDescriptor).AddressInSwap == -1) && !write)
            {
                return false;
            }
            int desc_a = (Pages[table] as TableDescriptor).TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, занята ли страница другим процессом
            if (((Pages[desc_a] as PageDescriptor).Mutex) && (offset == 0))
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
            int desc_a = (Pages[table] as TableDescriptor).TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // проверка, находится ли страница в ФП
            if (!(Pages[desc_a] as PageDescriptor).Present)
            {
                // освобождаем место для страницы
                UnloadToSwap(ReplacementAlgorithm());
                // восстанавливаем страницу со swap
                RestoreFromSwap(desc_a);
            }
            // проверки пройдены, считываем байт
            (Pages[desc_a] as PageDescriptor).Mutex = true;
            (Pages[desc_a] as PageDescriptor).Access = true;
            data = (Pages[(Pages[desc_a] as PageDescriptor).TargetAddress] as Page).Data[offset];
            if (offset == 3)
                (Pages[desc_a] as PageDescriptor).Mutex = false;
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
            int desc_a = (Pages[table] as TableDescriptor).TargetAddress + descriptor; // Получение абсолютного адреса дескриптора
            // если нет в памяти
            if (!(Pages[desc_a] as PageDescriptor).Present)
            {
                // если нет в ФП, то дескриптор свободный
                if ((Pages[desc_a] as PageDescriptor).AddressInSwap == -1)
                {
                    // если нет свободной страницы в памяти
                    if (FindFreePage() == -1)
                    {
                        // выгружаем страницу на swap
                        UnloadToSwap(ReplacementAlgorithm());
                        (Pages[desc_a] as PageDescriptor).TargetAddress = FindFreePage();
                    }
                    // если есть свободная страница
                    else
                    {
                        (Pages[desc_a] as PageDescriptor).TargetAddress = FindFreePage(); // присваиваем ссылку дескриптору
                    }
                }
                // иначе страница в ФП
                else
                {
                    // освобождаем место для страницы
                    UnloadToSwap(ReplacementAlgorithm());
                    // восстанавливаем страницу со swap
                    RestoreFromSwap(desc_a);
                }
            }

            // проверки пройдены, пишем байт
            (Pages[desc_a] as PageDescriptor).Present = true; // признак того, что страница в памяти
            (Pages[desc_a] as PageDescriptor).Mutex = true;
            (Pages[desc_a] as PageDescriptor).Access = true;
            (Pages[(Pages[desc_a] as PageDescriptor).TargetAddress] as Page).Data[offset] = data;
            (Pages[(Memory.Pages[desc_a] as PageDescriptor).TargetAddress] as Page).Dirty = true;
            if (offset == 3)
                (Pages[desc_a] as PageDescriptor).Mutex = false;
        }

        /// <summary>
        /// Выгружает страницу из ОП.
        /// </summary>
        /// <param name="UPageDescriptor">Абсолютный адрес дескриптора</param>
        /// <returns>Адрес, куда будет перемещает страница в ФП(-1 - неудача)</returns>
        private static int UnloadToSwap(int AbsoluteAddress)
        {
            //проверяем, на валидность
            if (AbsoluteAddress != -1)
            {
                //смотрим в дескрипторе адрес на свапе(два случая -1 и x)
                int AddressInSwap = (Pages[AbsoluteAddress] as PageDescriptor).AddressInSwap;
                //попытка слить на свап несуществующую страницу или которой нет в ОП
                if ((Pages[AbsoluteAddress] as PageDescriptor).TargetAddress == -1 && (Pages[AbsoluteAddress] as PageDescriptor).Present == false)
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
                                HDD.CellsArray[i].Data[j] = (Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Data[j];
                            }
                            //обнуляем страницу в памяти
                            for (int j = 0; j < (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Data.Length; j++)
                            {
                                (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Data[j] = 0;
                            }
                            //скидываем адрес в массиве страниц в ОП
                            (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Dirty = false;
                            (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).TargetAddress = -1;
                            HDD.CellsArray[i].IsFree = false;
                            //ставим бит присутствия в 0
                            (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).Present = false;
                            (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).AddressInSwap = i;
                            //выходим с адресом, куда записали
                            return i;
                        }
                    }
                }
                //если страница есть на swap'e
                else
                {
                    (Pages[AbsoluteAddress] as PageDescriptor).Present = false;
                    //копируем данные
                    for (int j = 0; j < GlobalConsts.PageSize; j++)
                    {
                        HDD.CellsArray[AddressInSwap].Data[j] = (Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Data[j];
                    }
                    //обнуляем страницу в памяти
                    for (int j = 0; j < (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Data.Length; j++)
                    {
                        (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Data[j] = 0;
                    }
                    //скидываем адрес в массиве страниц в ОП
                    (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).TargetAddress] as Page).Dirty = false;
                    (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).TargetAddress = -1;
                    //ставим бит присутствия в 0
                    (Memory.Pages[(Pages[AbsoluteAddress] as PageDescriptor).Address] as PageDescriptor).Present = false;
                    return (Pages[AbsoluteAddress] as PageDescriptor).AddressInSwap;
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
                if (WhereIs != -1 && (Pages[AbsoluteAddress] as PageDescriptor).AddressInSwap != -1 && (Pages[AbsoluteAddress] as PageDescriptor).Present == false)
                {
                    //копируем страницу в ОП
                    for (int i = 0; i < GlobalConsts.PageSize; i++)
                    {
                        (Pages[WhereIs] as Page).Data[i] = HDD.CellsArray[(Pages[AbsoluteAddress] as PageDescriptor).AddressInSwap].Data[i];
                    }
                    //выставляем параметры присутствия в дескрипторе и прочее
                    (Pages[AbsoluteAddress] as PageDescriptor).Present = true;
                    (Pages[AbsoluteAddress] as PageDescriptor).TargetAddress = WhereIs;
                    (Pages[WhereIs] as Page).Dirty = true;
                    return WhereIs;
                }
            }
            return -1;
        }

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
    }
}
