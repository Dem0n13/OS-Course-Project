using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS
{
    class NonRepeatEnum
    {
        /// <summary>
        /// Массив неповторяющихся значений
        /// </summary>
        public int[] Enumeration;

        /// <summary>
        /// Текущий номер
        /// </summary>
        public int CurrentNumber;

        /// <summary>
        /// Рандомайзер
        /// </summary>
        public Random RND = new Random();

        /// <summary>
        /// Конструктор для неповторяющейся последовательности
        /// </summary>
        /// <param name="a">Нижняя граница(включительно)</param>
        /// <param name="b">Верхняя граница(включительно)</param>
        public NonRepeatEnum(int below, int above)
        {
            CurrentNumber = 0;
            //инициализация массива
            Enumeration = new int[above - below + 1];

            int buff = 0;
            bool IsBe = false;
            int LocalCurrentNumber = 0;
            while (LocalCurrentNumber != Enumeration.Length)
            {
                buff = RND.Next(below, above + 1);
                for (int j = 0; j < LocalCurrentNumber; j++)
                {
                    if (Enumeration[j] == buff)
                    {
                        IsBe = true;
                    }
                }
                if (IsBe == false)
                {
                    Enumeration[LocalCurrentNumber] = buff;
                    LocalCurrentNumber++;
                }
                IsBe = false;
            }
        }
        /// <summary>
        /// Возвращает следующее случайное число
        /// </summary>
        /// <returns>Случайное число (либо -1 при окончании диапазона)</returns>
        public int Next()
        {
            if (CurrentNumber < Enumeration.Length)
            {
                CurrentNumber++;
                return Enumeration[CurrentNumber - 1];
            }
            else
                return -1;
        }
    }
}
