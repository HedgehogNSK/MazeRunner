using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    [System.Serializable]
    public struct CellCoordinates
    {
         [SerializeField]int x;
         [SerializeField]int y;
        public int X
        {
            get
            {
                return x;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
        }

        public CellCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static CellCoordinates operator +(CellCoordinates a, CellCoordinates b)
        {           
            return new CellCoordinates(a.X + b.X, a.Y + b.Y);
        }
        public static CellCoordinates operator /(CellCoordinates a, int b)
        {
            return new CellCoordinates(a.X/b, a.Y/b);
        }

    }
}
