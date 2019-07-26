using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{

    using Explorer;

    public interface Dweller
    {
        Coordinates Coords { get; }
        void Init(Dweller prefab, Graph mazeStructure);

    }
}