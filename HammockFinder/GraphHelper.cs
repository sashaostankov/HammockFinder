//
// Ostankov Alexander
// Higher School of Economics
// sashaostankov@gmail.com
// (c) 2015
//

using System.Collections.Generic;

namespace HammockFinder
{
    using Graph = List<List<int>>;

    public static class GraphHelper
    {
        /// <summary>
        /// Транспонирует граф.
        /// </summary>
        /// <returns>Транспонированный граф в виде списков смежности.</returns>
        /// <param name="gp">Граф в виде списков смежности.</param>
        static Graph TransposeGraph(Graph gp)
        {
            var res = new Graph();

            for (int i = 0; i < gp.Count; i++)
                res.Add(new List<int>());

            for (int i = 0; i < gp.Count; i++)
                for (int j = 0; j < gp[i].Count; j++)
                    res[gp[i][j]].Add(i);

            return res;
        }

        /// <summary>
        /// В графе находит доминатоворы для каждой вершины
        /// Доминонаторы для вершины v - множество вершин графа,
        /// через которые проходит любой путь из начала графа в вершину v.
        /// </summary>
        /// <returns>Множество доминаторов для каждой вершинв графа</returns>
        /// <param name="gp">Граф в виде списков смежности</param>
        /// <param name="end">Номер конечной вершины графа</param>
        static List<SortedSet<int>> CalcDynamicOnGraph(Graph gp, int end)
        {
            var dp = new List<SortedSet<int>>();

            for (int i = 0; i < gp.Count; i++)
                dp.Add(new SortedSet<int>());

            for (int i = 0; i < gp.Count; i++)
                for (int j = 0; j < gp.Count; j++)
                    dp[i].Add(j);

            dp[end].Clear();
            dp[end].Add(end);

            bool change;

            do
            {
                change = false;

                for (int i = 0; i < gp.Count; i++)
                {
                    int oldSize = dp[i].Count;

                    foreach (int next in gp[i])
                    {
                        dp[i].IntersectWith(dp[next]);
                    }

                    dp[i].Add(i);

                    if (oldSize != dp[i].Count)
                        change = true;
                }
            }
            while (change);

            return dp;
        }

        /// <summary>
        ///  Определяет является ли гамак минимальным или нет
        ///  Гамак НЕ является минимальным, если его можно разбить на несколько гамаков
        /// </summary>
        /// <returns><c>true</c> если гамак НЕ минимальный, иначе <c>false</c>.</returns>
        /// <param name="hammocks">Множество гамаков для каждой вершины графа</param>
        /// <param name="start">Начальная вершина гамака</param>
        /// <param name="end">Конечная вершина гамака</param>
        static bool IsBadHammock(List<SortedSet<int>> hammocks, int start, int end)
        {
            var used = new SortedSet<int>();
            var q = new Queue<int>();

            q.Enqueue(start);
            used.Add(start);

            while (q.Count > 0)
            {
                int cur = q.Dequeue();

                if (cur == end)
                    return true;

                foreach (int next in hammocks[cur])
                    if (!used.Contains(next))
                    {
                        if (cur == start && next == end)
                            continue;

                        q.Enqueue(next);
                        used.Add(next);
                    }
            }

            return false;
        }

        /// <summary>
        ///  Находит все минимальные гамаки в графе
        /// </summary>
        /// <returns>Возвращает множество гамаков для каждой вершины</returns>
        /// <param name="gi">Граф</param>
        public static List<SortedSet<int>> GetAllHammocks(GraphInfo gi)
        {
            var gp = gi.Gr;
            var tgp = TransposeGraph(gp);
            var dp1 = CalcDynamicOnGraph(gp, gi.EndVertex);
            var dp2 = CalcDynamicOnGraph(tgp, gi.StartVertex);

            // Оставляем только нужные вершины (переходы)
            if (gi.Include != null)
            {
                for (int i = 0; i < gp.Count; i++)
                {
                    if (!gi.Include.Contains(i))
                    {
                        dp1[i].Clear();
                        dp2[i].Clear();
                    }
                }
            }

            // Удаляем гамаки между позициями (оставляем между переходами)
            for (int i = 0; i < gp.Count; i++)
            {
                dp1[i].Remove(i);

                var newSet = new SortedSet<int>();

                foreach (int v in dp1[i])
                    if (dp2[v].Contains(i))
                        newSet.Add(v);

                dp1[i] = newSet;
            }

            // Оставляем минимальные гамаки
            // Гамак является минимальным, если его нельзя разбить на два гамака
            for (int v = 0; v < dp1.Count; v++)
                dp1[v].RemoveWhere(x => IsBadHammock(dp1, v, x));

            return dp1;
        }
    }
}