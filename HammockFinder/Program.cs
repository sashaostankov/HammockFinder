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

    class MainClass
    {
        public static void Main()
        {
            var gi = new GraphInfo();

            GetGraph(0, out gi.Gr, out gi.Include);

            var ham = GraphHelper.GetAllHammocks(gi);

            gi.SetAllHammocksToVertexes(ham);
            var root = HammockTree.CreateHammockTree(gi);
            PrintTree(root);
        }

        public static void PrintTree ( HammockTree v )
        {
            Console.WriteLine(v);

            foreach (var child in v.Childs)
                PrintTree(child);
        }

        public static void GetGraph(int index, out Graph gp, out SortedSet<int> include )
        {
            gp = new Graph();
            include = new SortedSet<int>();

            switch ( index )
            {
                case 0:
                    for (int i = 0; i < 16; i++)
                        gp.Add(new List<int>());

                    gp[0] = new List<int> { 1 };
                    gp[1] = new List<int> { 2, 3 };
                    gp[2] = new List<int> { 4, 5 };
                    gp[3] = new List<int> { 6, 7 };
                    gp[4] = new List<int> { 8, 9 };
                    gp[5] = new List<int> { 13 };
                    gp[6] = new List<int> { 10 };
                    gp[7] = new List<int> { 11 };
                    gp[8] = new List<int> { 12 };
                    gp[9] = new List<int> { 12 };
                    gp[10] = new List<int> { 14 };
                    gp[11] = new List<int> { 14 };
                    gp[12] = new List<int> { 13 };
                    gp[13] = new List<int> { 15 };
                    gp[14] = new List<int> { 15 };

                    include = new SortedSet<int>(new []{ 1, 2, 3, 4, 12, 13, 14, 15 });
                    break;
                case 1:
                    for (int i = 0; i < 13; i++)
                        gp.Add(new List<int>());

                    gp[0] = new List<int> { 10, 11 };
                    gp[1] = new List<int> { 3 };
                    gp[2] = new List<int> { 3 };
                    gp[3] = new List<int> { 4, 5 };
                    gp[4] = new List<int> { 6 };
                    gp[5] = new List<int> { 6 };
                    gp[6] = new List<int> { 7, 8 };
                    gp[7] = new List<int> { 9 };
                    gp[8] = new List<int> { 9 };
                    gp[9] = new List<int> { 12 };
                    gp[10] = new List<int> { 1, 2 };
                    gp[11] = new List<int> { 12 };

                    include = new SortedSet<int>(new []{ 0, 3, 6, 9, 10, 12 });

                    break;
                default:
                    Console.WriteLine("GetGraph ERROR");
                    return;
            }

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
