//
// Ostankov Alexander
// Higher School of Economics
// sashaostankov@gmail.com
// (c) 2015
//
using System;
using System.Collections.Generic;

namespace HammockFinder
{
    using Graph = List<List<int>>;

    public static class HammockFinder
    {
        public static void PrintTree(HammockTree v)
        {
            Console.WriteLine(v);

            foreach (var child in v.Childs)
                PrintTree(child);
        }

        /// <summary>
        ///  Генерирует n элементов со значение по умолчанию.
        /// </summary>
        /// <returns>Возвращает перечисление.</returns>
        /// <param name="n">Количество элементов.</param>
        /// <typeparam name="T">Любой тип.</typeparam>
        public static IEnumerable<T> GetRange<T>(int n)
        {
            for (int i = 0; i < n; i++)
                yield return default(T);
        }
    }
}
