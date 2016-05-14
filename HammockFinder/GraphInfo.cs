//
//  Ostankov Alexander
//  Higher Scool of Economics
//  sashaostankov@gmail.com
//  (c) 2015
//

using System.Collections.Generic;
using PNEditorLib;

namespace HammockFinder
{
    using Graph = List<List<int>>;

    public class GraphInfo
    {
        /// <summary>
        /// Граф, заданный списками смежности.
        /// </summary>
        public Graph Gr;

        /// <summary>
        /// Номера вершин, которые можно включать в гамаки.
        /// </summary>
        public SortedSet<int> Include;

        /// <summary>
        /// Список всех гамаков.
        /// </summary>
        public List<HammockTree> ListOfHammocks;

        /// <summary>
        /// Каким гамакам принадлежит вершина.
        /// </summary>
        public List<SortedSet<int>> HammocksAtVertex;

        /// <summary>
        /// Какие гамаки начинаются в текущей вершине.
        /// </summary>
        public List<SortedSet<int>> HammocksStartedAtVertex;

        /// <summary>
        /// Корень дерева гамаков.
        /// </summary>
        public HammockTree Root;

        /// <summary>
        /// По индексу возвращает объект.
        /// Index -> Figure.
        /// </summary>
        public List<Figure> Vertexes;

        /// <summary>
        /// По объекты возвращает индекс.
        /// Figure -> Index.
        /// </summary>
        public SortedDictionary<Figure, int> Indexes;

        /// <summary>
        /// Получает количество вершин в графе.
        /// </summary>
        /// <value>Количесто вершин</value>
        public int VertexNumber
        {
            get { return Gr.Count; }
        }

        /// <summary>
        /// The start vertex.
        /// </summary>
        public int StartVertex;

        /// <summary>
        /// The end vertex.
        /// </summary>
        public int EndVertex;

        /// <summary>
        /// Initializes a new instance of the <see cref="HammockFinder.GraphInfo"/> class.
        /// </summary>
        public GraphInfo()
        {
            Gr = new Graph();

            Include = new SortedSet<int>();
            ListOfHammocks = new List<HammockTree>();
            HammocksAtVertex = new List<SortedSet<int>>(); 
            HammocksStartedAtVertex = new List<SortedSet<int>>();

            Root = new HammockTree();

            Vertexes = new List<Figure>();
            Indexes = new SortedDictionary<Figure, int>(new Comparator());
        }

        /// <summary>
        /// Всем вершинам гамака устанваливает принадлежность данному гамаку.
        /// </summary>
        /// <returns>Количество вершин в гамаке</returns>
        /// <param name="hammock">Гамак</param>
        int AddHammockToVertexes(HammockTree hammock)
        {
            var q = new Queue<int>();
            int count = 0;

            q.Enqueue(hammock.Start);

            while (q.Count > 0)
            {
                int cur = q.Dequeue();

                if (HammocksAtVertex[cur].Contains(hammock.Identifier))
                    continue;

                count++;
                HammocksAtVertex[cur].Add(hammock.Identifier);

                if (cur == hammock.End)
                    continue;

                foreach (int next in Gr[cur])
                    if (!HammocksStartedAtVertex[next].Contains(hammock.Identifier))
                        q.Enqueue(next);
            }

            return count;
        }

        /// <summary>
        /// Устанавливает всем вершинам множества гамаков, к которым они принадлежат.
        /// </summary>
        /// <param name="hammocks">Множество гамаков</param>
        public void SetAllHammocksToVertexes(List<SortedSet<int>> hammocks)
        {
            HammocksStartedAtVertex = new List<SortedSet<int>>();
            HammocksAtVertex = new List<SortedSet<int>>(); 
            ListOfHammocks = new List<HammockTree>();

            for (int i = 0; i < hammocks.Count; i++)
            {
                HammocksAtVertex.Add(new SortedSet<int>());
                HammocksStartedAtVertex.Add(new SortedSet<int>());

                foreach (int v in hammocks[i])
                    ListOfHammocks.Add(new HammockTree(i, v));
            }

            int id = 0;

            foreach (var node in ListOfHammocks)
            {
                node.Identifier = id++;
                node.Size = AddHammockToVertexes(node);
                HammocksStartedAtVertex[node.Start].Add(node.Identifier);
            }
        }
    }
}

