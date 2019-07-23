using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CS_MazeCell : MonoBehaviour
{
    public CellCoordinates coords;
    private int iInitialisedEdgeCount;
    private CS_MazeCellEdge[] edges = new CS_MazeCellEdge[MazeDirections.Count];
    public CS_MazeCellEdge GetEdge(MazeDirection dir)
    {
        return edges[(int)dir];
    }
    
    public void SetEdge(MazeDirection dir, CS_MazeCellEdge edge)
    {
        edges[(int)dir] = edge;
        iInitialisedEdgeCount += 1;
    }

    public bool IsFullyInitialised
    {
        get
        {
            return iInitialisedEdgeCount == MazeDirections.Count;
        }
    }

  
    public MazeDirection RandomUninitialisedDirection
    {
        get
        {
            return MazeDirections.RandomUninitialisedDirection(edges.Where(edge => edge).Select(edge => edge.direction)); 
        }
    }
}
