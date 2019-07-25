using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Maze
{
    public class Cell : MonoBehaviour
    {
        public Coordinates coords;
        private int iInitialisedEdgeCount;
        private CellEdge[] edges = new CellEdge[DirectionTools.Count];
        public CellEdge GetEdge(Direction dir)
        {
            return edges[(int)dir];
        }

        public void SetEdge(Direction dir, CellEdge edge)
        {
            edges[(int)dir] = edge;
            iInitialisedEdgeCount += 1;
        }

        public bool IsFullyInitialised
        {
            get
            {
                return iInitialisedEdgeCount == DirectionTools.Count;
            }
        }


        public Direction RandomUninitialisedDirection
        {
            get
            {
                return DirectionTools.RandomUninitialisedDirection(edges.Where(edge => edge).Select(edge => edge.EdgeDirection));
            }
        }
    }
}