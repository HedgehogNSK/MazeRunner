using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{

    using Explorer;

  
    public abstract class Dweller :MonoBehaviour
    {
        static public Graph Map { get; set; }       
        public Coordinates Coords => Coordinates.FromWorld(transform.position);
        public void Init(Coordinates startPosition)
        {
            transform.position = startPosition.ToWorld;
        }

    }
}