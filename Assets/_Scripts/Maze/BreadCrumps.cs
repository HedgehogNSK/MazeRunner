using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class BreadCrump : Coordinates
    {
        public BreadCrump Origin { get; private set; }
        public int PathCost { get; private set; }

        public BreadCrump(Coordinates current, BreadCrump previous, int pathCost) : base(current.X, current.Y)
        {
            Origin = previous;
            PathCost = pathCost;
        }

        public BreadCrump FindCrump(Coordinates coords)
        {
            BreadCrump breadCrump = this;
            while (breadCrump.Origin != null && !breadCrump.Equals(coords))
            {
                breadCrump = breadCrump.Origin;
            }

            return breadCrump.Equals(coords) ? breadCrump : null;
        }

    }
}