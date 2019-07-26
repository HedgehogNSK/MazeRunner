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
            
            public IEnumerable<Coordinates> Neighbours(Coordinates vertice)
            {
                var branchesWithNode = graph.Where(branch => branch.Any(node => node == vertice));
                if (!branchesWithNode.IsAny()) return null;
                List<Coordinates> neigbours = new List<Coordinates>();
                foreach (var branch in branchesWithNode)
                {
                    int i = graph.IndexOf(branch);
                    int j = branch.IndexOf(vertice);

                    if (j != 0) neigbours.Add(graph[i][j - 1]);
                    if (j != graph[i].Count - 1) neigbours.Add(graph[i][j + 1]);


                }
                return neigbours;
            }

            public IEnumerable<Coordinates> Pathfinder(Coordinates start, Coordinates target)
            {
                Queue<Coordinates> que = new Queue<Coordinates>();
                Dictionary<Coordinates, Coordinates> path = new Dictionary<Coordinates, Coordinates>();
                que.Enqueue(start);
                path.Add(start, start);
                bool found = false;
                while (que.Any())
                {
                    Coordinates current = que.Dequeue();

                    if (current == target) {
                        found = true;
                        break;
                    }

                    foreach(var neighbour in Neighbours(current))
                    {
                        if(!path.ContainsKey(neighbour))
                        {
                            que.Enqueue(neighbour);
                            path.Add(neighbour, current);
                        }
                    }
                }
                
                if(found)
                {
                    List<Coordinates> pathList = new List<Coordinates>();
                    pathList.Add(target);

                    Coordinates value;
                    int i = 0;
                    while (path.TryGetValue(target, out value) && value != target)
                    {

                        pathList.Add(value);
                        target = value;
                        if (i > 1000) { Debug.LogError("Всё плохо"); break; }
                        i++;

                    }
                    pathList.Reverse();
                    return pathList;
                }

                return null;
            }

            
        }
    }
}