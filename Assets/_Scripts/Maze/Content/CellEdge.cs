using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    namespace Content
    {
        public abstract class CellEdge : MonoBehaviour
        {

            public Cell FirstCell { get; private set; }
            public Cell SecondCell { get; private set; }
            public Direction EdgeDirection { get; private set; }
            public void Initialise(Cell curCell, Cell neighCell, Direction direction)
            {
                this.FirstCell = curCell;
                this.SecondCell = neighCell;
                this.EdgeDirection = direction;
                curCell.SetEdge(direction, this);
                transform.parent = curCell.transform;
                transform.localPosition = Vector3.zero;
                transform.localRotation = direction.ToRotate();
            }
        }
    }
}