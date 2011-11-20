using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Файл содержит все мелкие нестатические классы, "кирпичики"

namespace OS
{
    #region Процессы

    /// <summary>
    /// Процесс
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Имя процесса
        /// </summary>
        public string ID;

        /// <summary>
        /// Статус процесса
        /// </summary>
        public ProcessState State = ProcessState.NotProcessed;

        /// <summary>
        /// Массив заявок
        /// </summary>
        public Request[] Requests;

        /// <summary>
        /// Логические пространства процесса
        /// </summary>
        public TableDescriptor[] LogicAreas;

        /// <summary>
        /// Контекст процесса для реализации псевдопараллельной работы
        /// </summary>
        public ProcessContext Context = new ProcessContext()
        {
            CurrentRequest = 0,
            TotalCopied = 0
        };
    }

    /// <summary>
    /// Контекст процесса
    /// </summary>
    public struct ProcessContext
    {
        /// <summary>
        /// Номер текущей заявки процесса
        /// </summary>
        public int CurrentRequest;

        /// <summary>
        /// Прогресс выполнения текущей заявки
        /// </summary>
        public int TotalCopied;
    }

    /// <summary>
    /// Заявка
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Тип заявки
        /// </summary>
        public RequestTypes Type;

        // Всевозможные адресные параметры (???)
        public string FromFile;
        public string ToFile;
        public int FileBlockNum;

        public int FromTable;
        public int ToTable;
        public int FromDescriptor;
        public int ToDescriptor;

        public override string ToString()
        {
            string result = "";
            switch (Type)
            {
                case RequestTypes.MemoryToMemory:
                    result = String.Format("Заявка типа Память>Память (из ЛА={0}_{1} в ЛА={2}_{3})", FromTable, FromDescriptor, ToTable, ToDescriptor);
                    break;
                case RequestTypes.MemoryToHDD:
                    result = String.Format("Заявка типа Память>ВЗУ (из ЛА={0}_{1} в {2}, ФБ={3})", FromTable, FromDescriptor, ToFile, FileBlockNum);
                    break;
                case RequestTypes.HDDToMemory:
                    result = String.Format("Заявка типа ВЗУ>Память (из {0}, ФБ={1} в ЛА={2}_{3})", FromFile, FileBlockNum, ToTable, ToDescriptor);
                    break;
                case RequestTypes.Action:
                    result = String.Format("Заявка типа Обработка данных (в ЛА={0}_{1})", FromTable, FromDescriptor);
                    break;
            }
            return result;
        }
    }

    /// <summary>
    /// Возможное типы заявок
    /// </summary>
    public enum RequestTypes
    {
        MemoryToMemory,
        MemoryToHDD,
        HDDToMemory,
        Action
    }

    /// <summary>
    /// Возможные состояния заявок
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// Не обработана
        /// </summary>
        NotProcessed,
        /// <summary>
        /// Обрабатывается
        /// </summary>
        Active,
        /// <summary>
        /// Приостановлена
        /// </summary>
        Paused,
        /// <summary>
        /// Выполнена
        /// </summary>
        Completed
    }

    #endregion

    #region Основная память

    /// <summary>
    /// Дескриптор таблицы
    /// </summary>
    public class TableDescriptor
    {
        /// <summary>
        /// Адрес таблицы дескрипторов в памяти 
        /// </summary>
        public int TargetAddress;

        /// <summary>
        /// Размер группы
        /// </summary>
        public int GroupSize;

        /// <summary>
        /// Абсолютный адрес в адресном пространстве 
        /// </summary>
        public int Address;
    }

    /// <summary>
    /// Дескриптор страницы
    /// </summary>
    public class PageDescriptor
    {
        /// <summary>
        /// Адрес страницы
        /// </summary>
        public int TargetAddress;

        /// <summary>
        /// Бит присутствия в памяти. 0-нет 1-есть
        /// </summary>
        public bool Present;

        /// <summary>
        /// Адрес страницы в файле подкачки
        /// </summary>
        public int AddressInSwap;

        /// <summary>
        /// Бит доступа (Для процессов)
        /// </summary>
        public bool Mutex;

#if (WSClock || NFU ||FIFO_SC || LRU || ClockWithOneArrow || ClockWithTwoArrows)

        /// <summary>
        /// Бит доступа (Для алгоритмов замещения)
        /// </summary>
        public bool Access;
#endif

#if NFU||LRU
        /// <summary>
        /// Счетчик
        /// </summary>
        public int Counter;
#endif

        /// <summary>
        /// Абсолютный адрес в адресном пространстве 
        /// </summary>
        public int Address;

#if (WS || WSClock)
        /// <summary>
        /// Возраст страницы
        /// </summary>
        public int AgeOfPage;
#endif
    }

    /// <summary>
    /// Сама страница с данными
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Массив байтов страницы
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Признак заполненности страницы
        /// </summary>
        public bool Dirty = false;

        /// <summary>
        /// Абсолютный адрес в адресном пространстве 
        /// </summary>
        public int Address;
    }

    #endregion

    #region ВЗУ

    #region Перечисление
#if FS_WITH_INDEX_ENUM
    /// <summary>
    /// Описание одного пункта из этой таблицы. ФС - перечисление.
    /// </summary>
    public class CatalogRecord
    {
        /// <summary>
        /// Адрес записи каталога
        /// </summary>
        public int Address;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Filename;

        /// <summary>
        /// открыт ли?
        /// </summary>
        public bool IsOpen;

        /// <summary>
        /// Индекы, занятые файлом
        /// </summary>
        public List<int> Indexes = new List<int>();

        /// <summary>
        ///  Размер файла
        /// </summary>
        public int FileSize;

        public override string ToString()
        {
            return Filename;
        }
    }

    /// <summary>
    /// Ячейка памяти ВЗУ файловая система которй -
    /// перечисление
    /// </summary>
    public class HDDCell
    {
        /// <summary>
        /// Данные в ячейке
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Адрес ячейки
        /// </summary>
        public int Address;

        /// <summary>
        /// Свободно ли?
        /// </summary>
        public bool IsFree;
    }

#endif
    #endregion

    #region Для связанной последованости
#if FS_WITH_INDEX_SEQ
    /// <summary>
    /// Описание одного пункта из этой таблицы. ФС - связанная последовательность.
    /// </summary>
    public class CatalogRecord
    {
        /// <summary>
        /// Адрес записи каталога
        /// </summary>
        public int Address;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Filename;

        /// <summary>
        /// открыт ли?
        /// </summary>
        public bool IsOpen;

        /// <summary>
        /// Начальный индекс, занятые файлом
        /// </summary>
        public int StartIndex;

        /// <summary>
        ///  Размер файла
        /// </summary>
        public int FileSize;

        public override string ToString()
        {
            return Filename;
        }
    }

    /// <summary>
    /// Ячейка памяти ВЗУ файловая система которй -
    /// связанная последовательность
    /// </summary>
    public class HDDCell
    {
        /// <summary>
        /// Данные в ячейке
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Адрес ячейки
        /// </summary>
        public int Address;

        /// <summary>
        /// Свободно ли?
        /// </summary>
        public bool IsFree;

        /// <summary>
        /// Адрес следующей ячейки
        /// </summary>
        public int Next;
    }


#endif
    #endregion

    #endregion
}
