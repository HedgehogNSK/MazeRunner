using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CS_MazeCellEdge : MonoBehaviour
{
    public CS_MazeCell curCell, neighbourCell;
    public MazeDirection direction;
    public void Initialise(CS_MazeCell curCell, CS_MazeCell neighCell, MazeDirection direction)
    {
        this.curCell = curCell;
        this.neighbourCell = neighCell;
        this.direction = direction;
        curCell.SetEdge(direction, this);
        transform.parent = curCell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotate();
    }
   
}
