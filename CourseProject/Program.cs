/*****Все символы прекомпиляции*****
 ******************************* 
 + #define FS_WITH_INDEX_ENUM
 + #define FS_WITH_INDEX_SEQ
 ******************************* 
 + #define TM_RR
 + #define TM_SRT
 ******************************* 
 + #define IO_TWO_BYTES_PER_STEP
 ******************************* 
 + #define FIFO
 + #define WSClock
 + #define WS
 + #define NFU
 + #define FIFO_SC
 + #define LRU
 + #define ClockWithOneArrow
 + #define ClockWithTwoArrows
 ******************************* 
 + UI_REQUEST_EDITOR_ON_MAIN_FORM
 + UI_REQUEST_EDITOR_ON_EDITOR_FORM
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OS
{
    /// <summary>
    /// Класс для хранения глобальных переменных
    /// </summary>
    public static class GlobalConsts
    {
        #region Диспетчеризация процессов
        /// <summary>
        /// Количество процессов
        /// </summary>
        public const int ProcessesCount = 3;

        /// <summary>
        /// Минимум кол-ва заявок в процессе
        /// </summary>
        public const int MinRequestsCount = 10;

        /// <summary>
        /// Максимум кол-ва заявок в процессе
        /// </summary>
        public const int MaxRequestsCount = 15;

        #endregion

        #region Управление памятью

        /// <summary>
        /// Размеры групп страниц. Первая группа должна помещаться в остальных
        /// </summary>
        public static int[] SizesOfGroup = new int[] { 5, 5, 5 };

        /// <summary>
        /// Объем в оперативе под страницы.
        /// </summary>
        public static int PagesAreaSize = 5;

        /// <summary>
        /// количество групп
        /// </summary>
        public static int CountOfGroup = SizesOfGroup.Length;

        /// <summary>
        /// Делегат
        /// </summary>
        /// <param name="value">Массив интов</param>
        /// <returns>Сумма</returns>
        private delegate int Dlg(int[] value);
        /// <summary>
        /// Считаем сумму массива
        /// </summary>
        private static Dlg SumOfMas = x => {
            int rez = 0;
            for (int i = 0; i < x.Length; i++)
            {
                rez += x[i];
            }
            return rez;
        };

        /// <summary>
        /// Количество страниц в системе
        /// </summary>
        public static int PagesCount = SumOfMas(SizesOfGroup);

        /// <summary>
        /// количество байт в странице (и в файловом блоке)
        /// </summary>
        public static int PageSize = 4;

        /// <summary>
        /// Адрес начала оперативы, где лежат дескрипторы таблиц. 
        /// </summary>
        public static int StartAddressDescriptionTable = 0;

        /// <summary>
        /// Адрес начала оперативы, где лежат дескрипторы страниц.
        /// </summary>
        public static int StartAddressDescriptionPage = CountOfGroup;

        /// <summary>
        /// Адрес начала оперативы, где лежат страницы.
        /// </summary>
        public static int StartAddressAreaOfPages = CountOfGroup + PagesCount;



        #endregion

        #region Файловая система

        /// <summary>
        /// Количество файлов в системе
        /// </summary>
        public static int CatalogRecordsCount = 1;

        /// <summary>
        /// Объем памяти ВЗУ в ячейках
        /// </summary>
        public const int HDDCellsCount = 16;

        /// <summary>
        /// Адрес начала записей каталога
        /// </summary>
        public static int StartCatalogRecords = StartAddressAreaOfPages + PagesAreaSize;

        /// <summary>
        /// Адрес начала файла подкачки
        /// </summary>
        public static int StartSwapArea = GlobalConsts.HDDCellsCount;

        #endregion
    }

    static class Program
    {
        public static Random RND = new Random();

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
