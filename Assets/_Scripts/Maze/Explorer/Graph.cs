﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hedge.Tools;
using System;
using Priority_Queue;

namespace Maze
{
    namespace Explorer
    {
        public class Graph
        {            
            protected List<List<Coordinates>> graph = new List<List<Coordinates>>();
            public int VerticeAmount { get; protected set; }
            
            //Magical number made for reducing distance beetwen available spawn spots to increase amount of spawnspots
            const float distanceDivider = 2;
            public Graph(Coordinates root)
            {
                graph.Add(new List<Coordinates>());
                graph.First().Add(root);
                VerticeAmount=1;
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

          
            public bool ContainsVertice(Coordinates node) => graph.Any(branch => branch.Contains(node));
            public bool ContainsEdge(Coordinates node1, Coordinates node2)
            {
                Coordinates previous = null;
                return graph.Any(branch => branch.Contains(node1) && branch.Contains(node2) 
                && branch.Any(node =>
                {
                    bool edgeIsSet = node.Equals(node1) && node2.Equals(previous) || node.Equals(node2) && node1.Equals(previous);
                    previous = node;
                    return edgeIsSet;
                })
                );

            }
            public IEnumerable<Coordinates> Neighbours(Coordinates vertice)
            {
                var branchesWithNode = graph.Where(branch => branch.Any(node => node.Equals(vertice)));
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
            public IEnumerable<Coordinates> NeighboursAround(Coordinates center, int range)
            {
                List<Coordinates> spots = new List<Coordinates>();

                foreach (var branch in graph)
                {
                    foreach (var node in branch)
                    {
                        if (!spots.Contains(node) && center.SqrDistance(node) < range * range)
                            spots.Add(node);
                    }
                }

                return spots.Where(spot => Neighbours(spot).Any(node => spots.Contains(node)));
            }

            public bool AddEdge(Coordinates node1, Coordinates node2)
            {
                if (ContainsEdge(node1, node2)) return false;

                var branchWithNode = graph.FirstOrDefault(branch => {
                    Coordinates tmp = branch.LastOrDefault();
                    return tmp.Equals(node1) || tmp.Equals(node2);
                });

                if (!ContainsVertice(node1)) VerticeAmount++;
                if (!ContainsVertice(node2)) VerticeAmount++;
                //If node1 or node2 is already in graph and it is last in graph then add another node to branch
                //Otherwise add new branch with to graph
                if (branchWithNode.IsAny())
                {
                    branchWithNode.Add(branchWithNode.Last().Equals(node1) ? node2 : node1);

                }
                else
                {
                    graph.Add(new List<Coordinates>());
                    graph.Last().Add(node1);
                    graph.Last().Add(node2);

                }

                return true;

            }
            public List<Coordinates> AStar(Coordinates start , Coordinates target)
            {

#if _DEBUG
                var watch = System.Diagnostics.Stopwatch.StartNew();
                watch.Start();
#endif 
                Queue<BreadCrump> nodes = new Queue<BreadCrump>();
                
                FastPriorityQueue<BreadCrump> nodesQueue =new FastPriorityQueue<BreadCrump>(2*VerticeAmount);
               
                BreadCrump breadCrump = new BreadCrump(start, null, 0);
                nodesQueue.Enqueue(breadCrump,0);
               
                while(nodesQueue.IsValidQueue())
                {
                    BreadCrump current = nodesQueue.Dequeue();
                    if (current.Equals(target))
                    {
                        breadCrump = current;
                        break;
                    }

                    foreach(var neighbour in Neighbours(current))
                    {
                        int newCost = 1 + current.PathCost;
                        if(current.FindCrump(neighbour)==null || newCost < current.FindCrump(neighbour).PathCost)
                        {
                            BreadCrump next = new BreadCrump(neighbour, current, newCost);
                            int priority = newCost + Heuristic(neighbour, target);
                            try
                            {
                                nodesQueue.Enqueue(next, priority);                               
                            }
                            catch
                            {                                
                                string msg ="Search way from "+start +"->" + target +"\n";
                                foreach(var nod in nodesQueue)
                                {
                                    
                                    msg+="->"+nod;
                                }
                                Debug.LogError(msg);
                                Print();
                            }
                        }
                    }
                    
                }
                
                if (breadCrump.Equals(target))
                {
                    List<Coordinates> path = new List<Coordinates>();
                    path.Add(breadCrump);
                    while(breadCrump.Origin!=null)
                    {
                        breadCrump = breadCrump.Origin;
                        path.Add(breadCrump);                        
                    }
                    path.Reverse();
#if _DEBUG
                    watch.Stop();
                    Debug.Log("AStar path find time: " + watch.ElapsedMilliseconds / 1000f);
#endif
                    return path;

                }
#if _DEBUG
                watch.Stop();
#endif
                return null;
            }
            private int Heuristic(Coordinates node1, Coordinates node2) =>  Mathf.Abs(node1.X - node2.X) + Mathf.Abs(node1.Y - node2.Y);         
            public List<Coordinates> BreadthFirstSearch(Coordinates start, Coordinates target)
            {
#if _DEBUG
                var watch = System.Diagnostics.Stopwatch.StartNew();
                watch.Start();
#endif 
                Queue<Coordinates> que = new Queue<Coordinates>();
                Dictionary<Coordinates, Coordinates> path = new Dictionary<Coordinates, Coordinates>();
                que.Enqueue(start);
                path.Add(start, start);
                bool found = false;
                while (que.Any())
                {
                    Coordinates current = que.Dequeue();

                    if (current.Equals(target))
                    {
                        found = true;
                        break;
                    }

                    foreach (var neighbour in Neighbours(current))
                    {
                        if (!path.ContainsKey(neighbour))
                        {
                            que.Enqueue(neighbour);
                            path.Add(neighbour, current);
                        }
                    }
                }

                if (found)
                {
                    List<Coordinates> pathList = new List<Coordinates>();
                    pathList.Add(target);

                    Coordinates value;
                    while (path.TryGetValue(target, out value) && value != target)
                    {
                        pathList.Add(value);
                        target = value;

                    }
                    pathList.Reverse();
#if _DEBUG
                    watch.Stop();
                    Debug.Log("Path find time: " + watch.ElapsedMilliseconds / 1000f);
#endif
                    return pathList;
                }
#if _DEBUG
                watch.Stop();
#endif
                return null;
            }           
            
            public enum Distance
            {
                Between,
                From
            }
            public List<Coordinates> GenerateSpawnPoints(int distance, Distance type = Distance.Between, Coordinates startCoords =null)
            {   
                List<Coordinates> spawnSpots = new List<Coordinates>();               
                switch(type)
                    {
                    case Distance.Between: {
                            SearchInDepth(startCoords == null ? graph[0][0] : startCoords, 0, distance, new List<Coordinates>(),  spawnSpots);
                        } break;
                    case Distance.From: {
                            spawnSpots = SearchAround( distance, startCoords);
                        } break;
                    default: { Debug.LogError("There is no behaviour for this type of Distance"); }break;
                    
                }
                
                return spawnSpots;
            }         
            private void SearchInDepth(Coordinates current, int depth, int distance, List<Coordinates> previous, List<Coordinates> resultSpots)
            {
                depth++;

                if (depth >= distance)
                {
#if _DEBUG
                    if (resultSpots.Count > 500)
                    {
                        Debug.LogError("Recursive search in depth must be looped");
                        return;
                    }
#endif
                    depth = 0;
                    resultSpots.Add(current);
                   
                }
                foreach (var neigbour in Neighbours(current).Except( previous)) 
                {
                    previous.Add(neigbour);
                    SearchInDepth(neigbour, depth,distance,previous, resultSpots);
                    
                }
            }
            private List<Coordinates> SearchAround(int minDistance, Coordinates centerPoint=null)
            {
                List<Coordinates> spots = new List<Coordinates>();
                if (centerPoint != null)
                    foreach (var branch in graph)
                    {
                        foreach (var node in branch)
                        {
                            if (centerPoint.SqrDistance(node) > minDistance* minDistance && !spots.Contains(node))
                                spots.Add(node);
                        }
                    }
                else
                    foreach (var branch in graph)
                    {
                        foreach (var node in branch)
                        {
                            if (!spots.Contains(node)) spots.Add(node);
                        }
                    }
                               
                List<Coordinates> randomSpots = new List<Coordinates>();
                randomSpots.Add(spots[0]);
                foreach (var spot in spots)
                {
                    if (randomSpots.All(other => !spot.Equals(other) && (spot.SqrDistance(other) > minDistance/ distanceDivider)))
                        randomSpots.Add(spot);

                }

                return randomSpots;
                
            }

        

        }


    }
}