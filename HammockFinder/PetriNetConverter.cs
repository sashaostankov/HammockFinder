//
//  Ostankov Alexander
//  Higher Scool of Economics
//  sashaostankov@gmail.com
//  (c) 2015
//

using System;
using System.Collections.Generic;
using PNEditorLib;

namespace HammockFinder
{
    using Graph = List<List<int>>;

    /// <summary>
    /// Comparator for the Figure class
    /// </summary>1
    public class Comparator : IComparer<Figure>
    {
        public int Compare(Figure a, Figure b)
        {
            return Math.Abs(a.CoordX - b.CoordX) > 1e-6 ? a.CoordX.CompareTo(b.CoordX) : a.CoordY.CompareTo(b.CoordY);
        }
    }

    /// <summary>
    /// Petri net converter.
    /// </summary>
    public static class PetriNetConverter
    {
        /// <summary>
        /// Конвертирует сеть Петри в граф
        /// </summary>
        /// <returns>Граф.</returns>
        /// <param name="pn">Сеть Петри.</param>
        public static GraphInfo ConvertToGraph(PetriNet pn)
        {
            var gi = new GraphInfo();

            var gp = gi.Gr; // Graph
            var indexes = gi.Indexes; // Figure -> index
            var vertexes = gi.Vertices; // index -> Figure
            var include = gi.Include;

            var incoming = new List<int>();
            var outgoing = new List<int>();

            foreach (var node in pn.places)
                indexes.Add(node, indexes.Count);

            foreach (var node in pn.transitions)
                indexes.Add(node, indexes.Count);

            vertexes.AddRange(HammockFinder.GetRange<Figure>(indexes.Count));
            incoming.AddRange(HammockFinder.GetRange<int>(indexes.Count));
            outgoing.AddRange(HammockFinder.GetRange<int>(indexes.Count));
            gp.AddRange(HammockFinder.GetRange<List<int>>(indexes.Count));

            foreach (var item in indexes)
                vertexes[item.Value] = item.Key;

            foreach (var arc in pn.arcs)
            {
                incoming[indexes[arc.To]]++;
                outgoing[indexes[arc.From]]++;
            }

            int start = -1, end = -1;

            for (int i = 0; i < incoming.Count; i++)
            {
                if (incoming[i] == 0 && outgoing[i] > 0)
                {
                    if (start != -1)
                        throw new Exception("More than one Figure have zero incoming arcs");

                    start = i;
                }
                if (outgoing[i] == 0 && incoming[i] > 0)
                {
                    if (end != -1)
                        throw new Exception("More than one Figure have zero outgoing arcs");

                    end = i;
                }
            }

            if (start == -1 || end == -1 || start == end)
                throw new Exception("The Net does not have start or end");

            gi.StartVertex = start;
            gi.EndVertex = end;

            foreach (var arc in pn.arcs)
                gp[indexes[arc.From]].Add(indexes[arc.To]);

            foreach (var node in pn.transitions)
                include.Add(indexes[node]);

            return gi;
        }
    }
}