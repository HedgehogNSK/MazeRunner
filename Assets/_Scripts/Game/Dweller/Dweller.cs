using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{

    using Explorer;

    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Dweller :MonoBehaviour
    {

        static public Graph map;
        protected Rigidbody2D rigid;
        public Coordinates Coords => Coordinates.FromWorld(transform.position);

        protected virtual void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

    }
}