using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public abstract class CellEdge : MonoBehaviour
    {
        public Cell curCell, neighbourCell;
        public Direction direction;
        public void Initialise(Cell curCell, Cell neighCell, Direction direction)
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
}