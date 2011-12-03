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
    public class Proc
    {
        /// <summary>
        /// Имя процесса
        /// </summary>
        public string ID;

        /// <summary>
        /// Статус процесса
        /// </summary>
        public Sostoyania Sostoyania = Sostoyania.NeZapuskalsa;

        /// <summary>
        /// Массив заявок
        /// </summary>
        public Zayavka[] Zayavki;

        /// <summary>
        /// Логические пространства процесса
        /// </summary>
        public TablicaDes[] AdresnieProstranstva;

        /// <summary>
        /// Контекст процесса для реализации псевдопараллельной работы
        /// </summary>
        public Kontext Context = new Kontext()
        {
            Tekushaa = 0,
            TotalCopied = 0
        };
    }

    /// <summary>
    /// Контекст процесса
    /// </summary>
    public struct Kontext
    {
        /// <summary>
        /// Номер текущей заявки процесса
        /// </summary>
        public int Tekushaa;

        /// <summary>
        /// Прогресс выполнения текущей заявки
        /// </summary>
        public int TotalCopied;
    }

    /// <summary>
    /// Заявка
    /// </summary>
    public class Zayavka
    {
        /// <summary>
        /// Тип заявки
        /// </summary>
        public RequestTypes Type_zayavky;

        // Всевозможные адресные параметры (???)
        public string IzFile;
        public string VFile;
        public int NomerFB;

        public int IzTablici;
        public int VTablicu;
        public int IzDes;
        public int VDes;

        public override string ToString()
        {
            string result = "";
            switch (Type_zayavky)
            {
                case RequestTypes.Copy:
                    result = String.Format("Память {0}_{1} -> Память {2}_{3}", IzTablici, IzDes, VTablicu, VDes);
                    break;
                case RequestTypes.IzMemory:
                    result = String.Format("Память {0}_{1} ->{2}, {3}", IzTablici, IzDes, VFile, NomerFB);
                    break;
                case RequestTypes.VMemory:
                    result = String.Format("{0}, {1} -> Память {2}_{3}", IzFile, NomerFB, VTablicu, VDes);
                    break;
                case RequestTypes.Deistvie:
                    result = String.Format("Обработка +6 в памяти {0}_{1}", IzTablici, IzDes);
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
        Copy,
        IzMemory,
        VMemory,
        Deistvie
    }

    /// <summary>
    /// Возможные состояния заявок
    /// </summary>
    public enum Sostoyania
    {
        /// <summary>
        /// Не обработана
        /// </summary>
        NeZapuskalsa,
        /// <summary>
        /// Обрабатывается
        /// </summary>
        Vipolnyaetsa,
        /// <summary>
        /// Приостановлена
        /// </summary>
        Zhdet,
        /// <summary>
        /// Выполнена
        /// </summary>
        Vipolnen
    }

    #endregion

    #region Основная память

    /// <summary>
    /// Интерфейс, описывает все, что может содержать страница основной памяти
    /// </summary>
    public interface YacheykaPamyati {}

    /// <summary>
    /// Дескриптор таблицы
    /// </summary>
    public class TablicaDes : YacheykaPamyati
    {
        /// <summary>
        /// Адрес таблицы дескрипторов в памяти 
        /// </summary>
        public int Ssilka;

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
    public class Des : YacheykaPamyati
    {
        /// <summary>
        /// Адрес страницы
        /// </summary>
        public int Ssilka;

        /// <summary>
        /// Бит присутствия в памяти. 0-нет 1-есть
        /// </summary>
        public bool P;

        /// <summary>
        /// Адрес страницы в файле подкачки
        /// </summary>
        public int SsilkaHDD;

        /// <summary>
        /// Бит доступа (Для процессов)
        /// </summary>
        public bool M;

#if (WSClock || NFU ||FIFO_SC || LRU || ClockWithOneArrow || ClockWithTwoArrows)

        /// <summary>
        /// Бит доступа (Для алгоритмов замещения)
        /// </summary>
        public bool A;
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
    public class Str : YacheykaPamyati
    {
        /// <summary>
        /// Массив байтов страницы
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Признак заполненности страницы
        /// </summary>
        public bool Zanyata = false;

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
    public class ZapisVCataloge
    {
        /// <summary>
        /// Адрес записи каталога
        /// </summary>
        public int Address;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Imya;

        /// <summary>
        /// открыт ли?
        /// </summary>
        public bool Otkrit;

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
            return Imya;
        }
    }

    /// <summary>
    /// Ячейка памяти ВЗУ файловая система которй -
    /// связанная последовательность
    /// </summary>
    public class Yacheyka
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
        public bool Svobodna;

        /// <summary>
        /// Адрес следующей ячейки
        /// </summary>
        public int Next;
    }


#endif
    #endregion

    #endregion
}
