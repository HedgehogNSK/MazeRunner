using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public static class MazeExtensions
    {
       public static CellCoordinates GetCenter(this CellCoordinates size)
        {
            int x = size.X / 2 + size.X % 2;
            int y = size.Y / 2 + size.Y % 2;
            return new CellCoordinates(x, y);
           
        }

        public static Vector3 ToWorld(this CellCoordinates coords)
        {

            return new Vector3(coords.X + 0.5f, coords.Y + 0.5f, 0f);

        }
    }
}