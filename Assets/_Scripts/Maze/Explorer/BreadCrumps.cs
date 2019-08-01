using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Explorer
{
    public class BreadCrump : Coordinates
    {
        public int PathCost { get; set; }
        public BreadCrump(Coordinates @this, int pathCost) : base(@this.X, @this.Y)
        {
            PathCost = pathCost;
        }
    }
}