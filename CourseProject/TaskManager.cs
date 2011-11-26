using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS
{
    /// <summary>
    /// Диспетчер процессов
    /// </summary>
    public static class DispZadach
    {
        public static int NomerTekushegoProcessa = 0;
        
        /// <summary>
        /// Массив процессов
        /// </summary>
        public static Process[] Processes;

        /// <summary>
        /// Инициализация начальных переменных
        /// </summary>
        static DispZadach()
        {
            // создаем список процессов и их ID
            Processes = new Process[GlobalConsts.ProcessesCount];
            for (int i = 0; i < GlobalConsts.ProcessesCount; i++)
            {
                Processes[i] = new Process() { ImyaProcessa = (i + 1).ToString() };
            }
            
            // Демо: пусть для 3х процессов А Б В существуют логические пространства А, АБ, В
            Processes[0].LogichescoeProstanstvo = new TableDescriptor[] { Memory.Stranizi[0] as TableDescriptor, Memory.Stranizi[1] as TableDescriptor };
            Processes[1].LogichescoeProstanstvo = new TableDescriptor[] { Memory.Stranizi[1] as TableDescriptor };
            Processes[2].LogichescoeProstanstvo = new TableDescriptor[] { Memory.Stranizi[2] as TableDescriptor };
            
            // пусть для каждого процесса существуют следующие заявки
            Processes[0].Zayavki = new Deistviya[GlobalConsts.SizesOfGroup[0]];
            Processes[1].Zayavki = new Deistviya[GlobalConsts.SizesOfGroup[0] * 2];
            Processes[2].Zayavki = new Deistviya[GlobalConsts.SizesOfGroup[0]];
            for (int i = 0; i < GlobalConsts.SizesOfGroup[0]; i++)
            {
                Processes[0].Zayavki[i] = new Deistviya() { Type = TipDeistviya.MemoryToMemory, FromTable = 0, FromDescriptor = i, ToTable = 1, ToDescriptor = i };

                Processes[1].Zayavki[2 * i] = new Deistviya() { Type = TipDeistviya.Deistvie, FromTable = 1, FromDescriptor = i };
                Processes[1].Zayavki[2 * i + 1] = new Deistviya() { Type = TipDeistviya.MemoryToHDD, FromTable = 1, FromDescriptor = i, ToFile = "File2.f", FileBlockNum = i, };

                Processes[2].Zayavki[i] = new Deistviya() { Type = TipDeistviya.HDDToMemory, FromFile = "File2.f", FileBlockNum = i, ToTable = 2, ToDescriptor = i };
            }
#if TM_SRT       
            // сортируем процессы по длительности
            SortProcesses();
#endif
            // задаем первый процесс на выполнение
            Processes[NomerTekushegoProcessa].VipolnenoBytes = SostoyanieProcessa.Active;
        }

        /// <summary>
        /// Выполнение следующей заявки текущего процесса
        /// </summary>
        public static void VozobnovitProcess()
        {
            // если процесса для выполнения нет - выходим
            if (NomerTekushegoProcessa == -1)
                return;
#if (WS || WSClock)
            //Вставка нужна для того, счтобы с каждой заявкой увеличивать системное время
            Memory.UvelichitVozrast();
#endif
#if (NFU || LRU)
            //обновляем состояние дескрипторов
            Memory.RefreshDescriptorsState();
#endif
#if ClockWithTwoArrows
            //двигаем стрелку
            Memory.GoArrow();
#endif
            // восстановление процесса из контекста
            int CurrentRequest = Processes[NomerTekushegoProcessa].Context.CurrentRequest;
            int TotalCopied = Processes[NomerTekushegoProcessa].Context.TotalCopied;

            byte buffer = 0;
            // выполнение текущей заявки
            switch (Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].Type)
            {
                case TipDeistviya.MemoryToMemory:
                    // если есть доступ к источнику и приемнику
                    if (Memory.ProveritMutex(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromDescriptor, TotalCopied, false) && Memory.ProveritMutex(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToDescriptor, TotalCopied, true))
                    {
                        // считываем и записываем
                        buffer = Memory.ChitatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        Memory.PisatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;

                case TipDeistviya.MemoryToHDD:
                    // если есть доступ к источнику и приемнику
                    if (Memory.ProveritMutex(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromDescriptor, TotalCopied, false) && HDD.ProveritMutex(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToFile, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FileBlockNum, TotalCopied,false))
                    {
                        // считываем и записываем
                        buffer = Memory.ChitatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        HDD.PisatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToFile, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        HDD.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;

                case TipDeistviya.HDDToMemory:
                    // если есть доступ к источнику и приемнику
                    if (HDD.ProveritMutex(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromFile, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FileBlockNum, TotalCopied,true) && Memory.ProveritMutex(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToDescriptor, TotalCopied, true))
                    {
                        // считываем и записываем
                        buffer = HDD.ChitatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromFile, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        Memory.PisatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = HDD.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;
                case TipDeistviya.Deistvie:
                    // если есть доступ к источнику
                    if (Memory.ProveritMutex(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromDescriptor, TotalCopied, false))
                    {
                        // считываем и записываем измененное
                        buffer = Memory.ChitatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        Memory.PisatByte(Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromTable, Processes[NomerTekushegoProcessa].Zayavki[CurrentRequest].FromDescriptor, TotalCopied, (byte)(buffer + 5));
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, (byte)(buffer + 5));
                        TotalCopied++;
#endif
                    }
                    break;
            }

            // Если заявка выполнена полностью - переходим к следующей
            if (TotalCopied == GlobalConsts.PageSize)
            {
                CurrentRequest++;
                TotalCopied = 0;
            }
            // если это была последняя заявка процесса - процесс выполнен
            if (CurrentRequest == Processes[NomerTekushegoProcessa].Zayavki.Length)
                Processes[NomerTekushegoProcessa].VipolnenoBytes = SostoyanieProcessa.Completed;
            // иначе сохраняем данные в его контекст и приостанавливаем
            else
            {
                Processes[NomerTekushegoProcessa].Context.CurrentRequest = CurrentRequest;
                Processes[NomerTekushegoProcessa].Context.TotalCopied = TotalCopied;
                Processes[NomerTekushegoProcessa].VipolnenoBytes = SostoyanieProcessa.Paused;
            }

            // следующий процесс на выполнение
            NomerTekushegoProcessa = PoluchitSledProcess();
            if (NomerTekushegoProcessa != -1)
                Processes[NomerTekushegoProcessa].VipolnenoBytes = SostoyanieProcessa.Active;
        }

#if TM_RR
        /// <summary>
        /// Поиск нового процесса RoundRobin
        /// </summary>
        /// <returns>Возвращает номер процесса на выполнение. -1 - нет процесса для выполнения</returns>
        private static int PoluchitSledProcess()
        {
            // начинаем с текущего+1 и до последнего
            for (int i = NomerTekushegoProcessa+1; i < GlobalConsts.ProcessesCount; i++)
                if (Processes[i].VipolnenoBytes != SostoyanieProcessa.Completed)
                    return i;
            // начинаем с 0 до текущего
            for (int i = 0; i < NomerTekushegoProcessa + 1; i++)
                if (Processes[i].VipolnenoBytes != SostoyanieProcessa.Completed)
                    return i;
            // если все процессы выполнены
            return -1;
        }
#endif

#if TM_SRT
        /// <summary>
        /// Поиск нового процесса SRT
        /// </summary>
        /// <returns>Возвращает номер процесса на выполнение. -1 - нет процесса для выполнения</returns>
        private static int FindNextProcess()
        {
            // начинаем с текущего+1 и до последнего
            for (int i = CurrentProcessIndex + 1; i < GlobalConsts.ProcessesCount; i++)
                if (Processes[i].State != ProcessState.Completed)
                    return i;
            // начинаем с 0 до текущего
            for (int i = 0; i < CurrentProcessIndex + 1; i++)
                if (Processes[i].State != ProcessState.Completed)
                    return i;
            // если все процессы выполнены
            return -1;
        }

        /// <summary>
        /// Сортировка процессов по длительности
        /// </summary>
        private static void SortProcesses()
        {
            Process buf;
            for (int i = 0; i < Processes.Length; i++)
            {
                for (int j = Processes.Length - 1; j > i; j--)
                {
                    if (Processes[j].Requests.Length < Processes[j - 1].Requests.Length)
                    {
                        buf = Processes[j];
                        Processes[j] = Processes[i];
                        Processes[i] = buf;
                    }
                }
            }
        }
#endif
    }
}
