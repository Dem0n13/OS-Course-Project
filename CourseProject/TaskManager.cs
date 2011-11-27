using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS
{
    /// <summary>
    /// Диспетчер процессов
    /// </summary>
    public static class DispetcherProcessov
    {
        public static int Tekushiy = 0;
        
        /// <summary>
        /// Массив процессов
        /// </summary>
        public static Proc[] Processes;

        /// <summary>
        /// Инициализация начальных переменных
        /// </summary>
        static DispetcherProcessov()
        {
            // создаем список процессов и их ID
            Processes = new Proc[GlobalConsts.ProcessesCount];
            for (int i = 0; i < GlobalConsts.ProcessesCount; i++)
            {
                Processes[i] = new Proc();
            }
            
            // Демо: пусть для 3х процессов А Б В существуют логические пространства А, АБ, В
            Processes[0].ID = "A";
            Processes[0].AdresnieProstranstva = new TablicaDes[] { Memory.Stranici[0] as TablicaDes, Memory.Stranici[1] as TablicaDes };
            Processes[1].ID = "Б";
            Processes[1].AdresnieProstranstva = new TablicaDes[] { Memory.Stranici[1] as TablicaDes };
            Processes[2].ID = "В";
            Processes[2].AdresnieProstranstva = new TablicaDes[] { Memory.Stranici[2] as TablicaDes };
            
            // пусть для каждого процесса существуют следующие заявки
            Processes[0].Zayavki = new Zayavka[GlobalConsts.SizesOfGroup[0]];
            Processes[1].Zayavki = new Zayavka[GlobalConsts.SizesOfGroup[0] * 2];
            Processes[2].Zayavki = new Zayavka[GlobalConsts.SizesOfGroup[0]];
            for (int i = 0; i < GlobalConsts.SizesOfGroup[0]; i++)
            {
                Processes[0].Zayavki[i] = new Zayavka() { Type_zayavky = RequestTypes.Copy, IzTablici = 0, IzDes = i, VTablicu = 1, VDes = i };

                Processes[1].Zayavki[2 * i] = new Zayavka() { Type_zayavky = RequestTypes.Deistvie, IzTablici = 1, IzDes = i };
                Processes[1].Zayavki[2 * i + 1] = new Zayavka() { Type_zayavky = RequestTypes.IzMemory, IzTablici = 1, IzDes = i, VFile = "File2", NomerFB = i, };

                Processes[2].Zayavki[i] = new Zayavka() { Type_zayavky = RequestTypes.VMemory, IzFile = "File2", NomerFB = i, VTablicu = 2, VDes = i };
            }
#if TM_SRT       
            // сортируем процессы по длительности
            SortProcesses();
#endif
            // задаем первый процесс на выполнение
            Processes[Tekushiy].Sostoyania = Sostoyania.Vipolnyaetsa;
        }

        /// <summary>
        /// Выполнение следующей заявки текущего процесса
        /// </summary>
        public static void Go()
        {
            // если процесса для выполнения нет - выходим
            if (Tekushiy == -1)
                return;
#if (WS || WSClock)
            //Вставка нужна для того, счтобы с каждой заявкой увеличивать системное время
            Memory.AgesUp();
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
            int CurrentRequest = Processes[Tekushiy].Context.Tekushaa;
            int TotalCopied = Processes[Tekushiy].Context.TotalCopied;

            byte buffer = 0;
            // выполнение текущей заявки
            switch (Processes[Tekushiy].Zayavki[CurrentRequest].Type_zayavky)
            {
                case RequestTypes.Copy:
                    // если есть доступ к источнику и приемнику
                    if (Memory.UstanovlenMutex(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, false) && Memory.UstanovlenMutex(Processes[Tekushiy].Zayavki[CurrentRequest].VTablicu, Processes[Tekushiy].Zayavki[CurrentRequest].VDes, TotalCopied, true))
                    {
                        // считываем и записываем
                        buffer = Memory.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, buffer);
                        Memory.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].VTablicu, Processes[Tekushiy].Zayavki[CurrentRequest].VDes, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, buffer);
                        Memory.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].VTablicu, Processes[Tekushiy].Zayavki[CurrentRequest].VDes, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;

                case RequestTypes.IzMemory:
                    // если есть доступ к источнику и приемнику
                    if (Memory.UstanovlenMutex(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, false) && HDD.ProveritMutex(Processes[Tekushiy].Zayavki[CurrentRequest].VFile, Processes[Tekushiy].Zayavki[CurrentRequest].NomerFB, TotalCopied,false))
                    {
                        // считываем и записываем
                        buffer = Memory.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, buffer);
                        HDD.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].VFile, Processes[Tekushiy].Zayavki[CurrentRequest].NomerFB, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, buffer);
                        HDD.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].VFile, Processes[Tekushiy].Zayavki[CurrentRequest].NomerFB, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;

                case RequestTypes.VMemory:
                    // если есть доступ к источнику и приемнику
                    if (HDD.ProveritMutex(Processes[Tekushiy].Zayavki[CurrentRequest].IzFile, Processes[Tekushiy].Zayavki[CurrentRequest].NomerFB, TotalCopied,true) && Memory.UstanovlenMutex(Processes[Tekushiy].Zayavki[CurrentRequest].VTablicu, Processes[Tekushiy].Zayavki[CurrentRequest].VDes, TotalCopied, true))
                    {
                        // считываем и записываем
                        buffer = HDD.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzFile, Processes[Tekushiy].Zayavki[CurrentRequest].NomerFB, TotalCopied, buffer);
                        Memory.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].VTablicu, Processes[Tekushiy].Zayavki[CurrentRequest].VDes, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = HDD.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzFile, Processes[Tekushiy].Zayavki[CurrentRequest].NomerFB, TotalCopied, buffer);
                        Memory.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].VTablicu, Processes[Tekushiy].Zayavki[CurrentRequest].VDes, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;
                case RequestTypes.Deistvie:
                    // если есть доступ к источнику
                    if (Memory.UstanovlenMutex(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, false))
                    {
                        // считываем и записываем измененное
                        buffer = Memory.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, buffer);
                        Memory.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, (byte)(255 - buffer));
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.Chtenie(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, buffer);
                        Memory.Zapis(Processes[Tekushiy].Zayavki[CurrentRequest].IzTablici, Processes[Tekushiy].Zayavki[CurrentRequest].IzDes, TotalCopied, (byte)(255 - buffer));
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
            if (CurrentRequest == Processes[Tekushiy].Zayavki.Length)
                Processes[Tekushiy].Sostoyania = Sostoyania.Vipolnen;
            // иначе сохраняем данные в его контекст и приостанавливаем
            else
            {
                Processes[Tekushiy].Context.Tekushaa = CurrentRequest;
                Processes[Tekushiy].Context.TotalCopied = TotalCopied;
                Processes[Tekushiy].Sostoyania = Sostoyania.Zhdet;
            }

            // следующий процесс на выполнение
            Tekushiy = PerekluchitProccess();
            if (Tekushiy != -1)
                Processes[Tekushiy].Sostoyania = Sostoyania.Vipolnyaetsa;
        }

#if TM_RR
        /// <summary>
        /// Поиск нового процесса RoundRobin
        /// </summary>
        /// <returns>Возвращает номер процесса на выполнение. -1 - нет процесса для выполнения</returns>
        private static int PerekluchitProccess()
        {
            // начинаем с текущего+1 и до последнего
            for (int i = Tekushiy+1; i < GlobalConsts.ProcessesCount; i++)
                if (Processes[i].Sostoyania != Sostoyania.Vipolnen)
                    return i;
            // начинаем с 0 до текущего
            for (int i = 0; i < Tekushiy + 1; i++)
                if (Processes[i].Sostoyania != Sostoyania.Vipolnen)
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
