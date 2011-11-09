using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS
{
    /// <summary>
    /// Диспетчер процессов
    /// </summary>
    public static class TaskManager
    {
        public static int CurrentProcessIndex = 0;
        
        /// <summary>
        /// Массив процессов
        /// </summary>
        public static Process[] Processes;

        /// <summary>
        /// Инициализация начальных переменных
        /// </summary>
        static TaskManager()
        {
            // создаем список процессов и их ID
            Processes = new Process[GlobalConsts.ProcessesCount];
            
            Processes[0] = new Process() { ID = "A"};
            Processes[1] = new Process() { ID = "Б" };
            Processes[2] = new Process() { ID = "С" };
            
            // Демо: пусть для 3х процессов А Б В существуют логические пространства А, АБ, В
            Processes[0].LogicAreas = new TableDescriptor[] { Memory.TableDess[0], Memory.TableDess[1] };
            Processes[1].LogicAreas = new TableDescriptor[] { Memory.TableDess[1] };
            Processes[2].LogicAreas = new TableDescriptor[] { Memory.TableDess[2] };
            
            // пусть для каждого процесса существуют следующие заявки
            Processes[0].Requests = new Request[GlobalConsts.SizesOfGroup[0]];
            Processes[1].Requests = new Request[GlobalConsts.SizesOfGroup[0] * 2];
            Processes[2].Requests = new Request[GlobalConsts.SizesOfGroup[0]];
            for (int i = 0; i < GlobalConsts.SizesOfGroup[0]; i++)
            {
                Processes[0].Requests[i] = new Request() { Type = RequestTypes.MemoryToMemory, FromTable = 0, FromDescriptor = i, ToTable = 1, ToDescriptor = i };

                Processes[1].Requests[2 * i] = new Request() { Type = RequestTypes.Action, FromTable = 1, FromDescriptor = i };
                Processes[1].Requests[2 * i + 1] = new Request() { Type = RequestTypes.MemoryToHDD, FromTable = 1, FromDescriptor = i, ToFile = "File2", FileBlockNum = i, };

                Processes[2].Requests[i] = new Request() { Type = RequestTypes.HDDToMemory, FromFile = "File2", FileBlockNum = i, ToTable = 2, ToDescriptor = i };
            }
#if TM_SRT       
            // сортируем процессы по длительности
            SortProcesses();
#endif
            // задаем первый процесс на выполнение
            Processes[CurrentProcessIndex].State = ProcessState.Active;
        }

        /// <summary>
        /// Выполнение следующей заявки текущего процесса
        /// </summary>
        public static void ResumeProcess()
        {
            // если процесса для выполнения нет - выходим
            if (CurrentProcessIndex == -1)
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
            int CurrentRequest = Processes[CurrentProcessIndex].Context.CurrentRequest;
            int TotalCopied = Processes[CurrentProcessIndex].Context.TotalCopied;

            byte buffer = 0;
            // выполнение текущей заявки
            switch (Processes[CurrentProcessIndex].Requests[CurrentRequest].Type)
            {
                case RequestTypes.MemoryToMemory:
                    // если есть доступ к источнику и приемнику
                    if (Memory.CheckMutex(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, false) && Memory.CheckMutex(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, true))
                    {
                        // считываем и записываем
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;

                case RequestTypes.MemoryToHDD:
                    // если есть доступ к источнику и приемнику
                    if (Memory.CheckMutex(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, false) && HDD.CheckMutex(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied,false))
                    {
                        // считываем и записываем
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        HDD.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        HDD.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;

                case RequestTypes.HDDToMemory:
                    // если есть доступ к источнику и приемнику
                    if (HDD.CheckMutex(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied,true) && Memory.CheckMutex(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, true))
                    {
                        // считываем и записываем
                        buffer = HDD.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = HDD.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromFile, Processes[CurrentProcessIndex].Requests[CurrentRequest].FileBlockNum, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].ToTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].ToDescriptor, TotalCopied, buffer);
                        TotalCopied++;
#endif
                    }
                    break;
                case RequestTypes.Action:
                    // если есть доступ к источнику
                    if (Memory.CheckMutex(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, false))
                    {
                        // считываем и записываем измененное
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        if (buffer != 255)
                        {
                            Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, (byte)(buffer + 1));
                        }
                        else
                        {
                            Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, (byte)(0));
                        }
                        TotalCopied++;
#if IO_TWO_BYTES_PER_STEP
                        buffer = Memory.ReadByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, buffer);
                        Memory.WriteByte(Processes[CurrentProcessIndex].Requests[CurrentRequest].FromTable, Processes[CurrentProcessIndex].Requests[CurrentRequest].FromDescriptor, TotalCopied, (byte)(255 - buffer));
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
            if (CurrentRequest == Processes[CurrentProcessIndex].Requests.Length)
                Processes[CurrentProcessIndex].State = ProcessState.Completed;
            // иначе сохраняем данные в его контекст и приостанавливаем
            else
            {
                Processes[CurrentProcessIndex].Context.CurrentRequest = CurrentRequest;
                Processes[CurrentProcessIndex].Context.TotalCopied = TotalCopied;
                Processes[CurrentProcessIndex].State = ProcessState.Paused;
            }

            // следующий процесс на выполнение
            CurrentProcessIndex = FindNextProcess();
            if (CurrentProcessIndex != -1)
                Processes[CurrentProcessIndex].State = ProcessState.Active;
        }

#if TM_RR
        /// <summary>
        /// Поиск нового процесса RoundRobin
        /// </summary>
        /// <returns>Возвращает номер процесса на выполнение. -1 - нет процесса для выполнения</returns>
        private static int FindNextProcess()
        {
            // начинаем с текущего+1 и до последнего
            for (int i = CurrentProcessIndex+1; i < GlobalConsts.ProcessesCount; i++)
                if (Processes[i].State != ProcessState.Completed)
                    return i;
            // начинаем с 0 до текущего
            for (int i = 0; i < CurrentProcessIndex + 1; i++)
                if (Processes[i].State != ProcessState.Completed)
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
