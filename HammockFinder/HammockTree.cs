//
//  Ostankov Alexander
//  Higher Scool of Economics
//  sashaostankov@gmail.com
//  (c) 2015
//

using System.Collections.Generic;

namespace HammockFinder
{
    using Graph = List<List<int>>;

    public class HammockTree
    {
        /// <summary>
        /// Индивидуальный номер гамака.
        /// </summary>
        public int Identifier;

        /// <summary>
        /// Номер вершины, в которой начинает гамак.
        /// </summary>
        public int Start;

        /// <summary>
        /// Номер вершины, в которой кончается гамак.
        /// </summary>
        public int End;

        /// <summary>
        /// Количество вершин в гамаке.
        /// </summary>
        public int Size;

        /// <summary>
        /// Родитель в дереве гамаков.
        /// </summary>
        public HammockTree Parent;

        /// <summary>
        /// Минимальные гамаки, которые содержит текущий гамак.
        /// </summary>
        public HashSet<HammockTree> Childs;

        /// <summary>
        /// Все гамаки, с которым пересекается данный гамак.
        /// </summary>
        public HashSet<HammockTree> Siblings;


        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HammockFinder.HammockTree"/>
        /// </summary>
        public HammockTree()
        {
            Identifier = -1;
            Start = -1;
            End = -1;

            Size = 0;

            Parent = null;

            Childs = new HashSet<HammockTree>();
            Siblings = new HashSet<HammockTree>();
        }

        /// <summary>
        ///  Инициализирует новый экземпляр класса <see cref="HammockFinder.HammockTree"/>
        /// </summary>
        /// <param name="start">Начальная вершина гамака</param>
        /// <param name="end">Конечная вершина гамака</param>
        public HammockTree(int start, int end)
            : this()
        {
            Start = start;
            End = end;
        }

        /// <summary>
        ///  Строит дерево гамаков по данному графу
        /// </summary>
        /// <returns>Возвращает указатель на корень дерева</returns>
        /// <param name="gi">Граф</param>
        public static HammockTree CreateHammockTree(GraphInfo gi)
        {
            var root = new HammockTree();

            foreach (var node in gi.ListOfHammocks)
                if (node.Size > root.Size)
                    root = node;

            root.FindChildren(gi);

            return root;
        }

        /// <summary>
        ///  Ищем всех потомков данной вершины в дереве гамаков
        ///  (то есть все гамаки, которые содержит этот гамак)
        /// </summary>
        /// <param name="gi">Граф</param>
        protected virtual void FindChildren(GraphInfo gi)
        {
            var used = new SortedSet<int>();
            var q = new Queue<int>();

            q.Enqueue(Start);
            used.Add(Start);

            while (q.Count > 0)
            {
                int cur = q.Dequeue();

                if (cur == End)
                    continue;

                if (gi.HammocksStartedAtVertex[cur].Count > 0)
                {
                    var next = GetChildren(gi, cur, Size);

                    if (next.Size > 0)
                    {
                        if (!used.Contains(next.End))
                        {
                            used.Add(next.End);
                            q.Enqueue(next.End);
                        }

                        Childs.Add(next);
                        next.Parent = this;
                        next.FindChildren(gi);

                        continue;
                    }
                }

                foreach (int next in gi.Gr[cur])
                {
                    if (!used.Contains(next))
                    {
                        used.Add(next);
                        q.Enqueue(next);
                    }
                }
            }

            foreach (var child in Childs)
                child.FindSiblings();
        }

        /// <summary>
        ///  Находит гамак, начинающийся в данной вершине максимального размера меньше заданного.
        ///  Это есть ребенок данного гамака
        /// </summary>
        /// <returns>Гамак</returns>
        /// <param name="gi">Граф</param>
        /// <param name="v">Номер вершины</param>
        /// <param name="size">Размер текущего гамака</param>
        protected virtual HammockTree GetChildren(GraphInfo gi, int v, int size)
        {
            var ans = new HammockTree();

            foreach (var node in gi.HammocksStartedAtVertex[v])
                if (gi.ListOfHammocks[node].Size > ans.Size &&
                    gi.ListOfHammocks[node].Size < size)
                    ans = gi.ListOfHammocks[node];
            
            return ans;
        }

        /// <summary>
        /// Ищет всех детей своего предка в дереве гамаков, 
        /// которые пересекаются с этим гамаком
        /// </summary>
        protected virtual void FindSiblings()
        {
            if (Parent == null)
                return;
            
            foreach (var child in Parent.Childs)
            {
                if (child.End == Start)
                    Siblings.Add(child);
                else if (child.Start == End)
                    Siblings.Add(child);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="HammockFinder.HammockTree"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="HammockFinder.HammockTree"/>.</returns>
        public override string ToString()
        {
            string res = "", strChilds = "", strSiblings = "";
            string strParent = ((Parent == null) ? -1 : Parent.Identifier).ToString();

            foreach (var child in Childs)
                strChilds += child.Identifier + ", ";
            
            foreach (var sibling in Siblings)
                strSiblings += sibling.Identifier + ", ";

            if (strChilds.Length > 2)
                strChilds = strChilds.Remove(strChilds.Length - 2);

            if (strSiblings.Length > 2)
                strSiblings = strSiblings.Remove(strSiblings.Length - 2);


            res += string.Format("[Id: {0}, Start: {1}, End: {2}, Size: {3}, ", Identifier, Start, End, Size);
            res += string.Format("ParentId: {0}, Childs: [{1}], Siblings: [{2}]]", strParent, strChilds, strSiblings);

            return res;
        }
    }
}