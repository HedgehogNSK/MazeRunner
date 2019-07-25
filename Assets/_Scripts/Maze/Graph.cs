using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hedge.Tools;

namespace Maze
{
    namespace Explorer
    {
        public class Graph
        {
            private List<List<Coordinates>> graph = new List<List<Coordinates>>();

            public Graph(Coordinates root)
            {
                graph.Add(new List<Coordinates>());
                graph.Last().Add(root);
            }
            public bool AddEdge(Coordinates node1, Coordinates node2)
            {
                bool firstIsInGraph = graph.Any(brench => brench.Any(node => node == node1));
                bool secondIsInGraph = graph.Any(brench => brench.Any(node => node == node2));

                if (firstIsInGraph && secondIsInGraph) return false;


                //Select Index of those brenches where node1 is exists.
                var index = graph.Where(brench => brench.LastOrDefault() == (firstIsInGraph ? node1 : node2)).Select(brench => graph.IndexOf(brench));

                if (index.IsAny())
                {
                    graph[index.First()].Add(firstIsInGraph ? node2 : node1);
                }
                else
                {
                    graph.Add(new List<Coordinates>());
                    graph.Last().Add(node1);
                    graph.Last().Add(node2);
                }

                return true;

            }


            public void Print()
            {
                string message = "Graph:\n";
                foreach (var branch in graph)
                {
                    foreach (var node in branch)
                    {
                        message = message + "->(" + node.X + "," + node.Y + ")";
                    }
                    message = message + "\n";
                }
                Debug.Log(message);
            }

            //public IEnumerable<Coordinates> Pathfind(Coordinates point1,Coordinates point2)
            //{
            //    graph.Select(brench => brench.Where(point => point == point1 && brench.Any(another => another == point2)));
            //}
        }
    }
}