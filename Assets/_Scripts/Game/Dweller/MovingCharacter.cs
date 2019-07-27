using System.Collections;
using System.Collections.Generic;
using Maze.Explorer;
using UnityEngine;

namespace Maze.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MovingCharacter : Dweller
    {
        [SerializeField]protected float speed = 3;
        public float Speed => speed;        

        Coordinates currentCoords = new Coordinates(0,0);

        protected Rigidbody2D rigid;
        protected virtual void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }


        protected virtual void FixedUpdate()
        {
            if (currentCoords != Coords)
            {
                currentCoords = Coords;
                OnChangingPosition?.Invoke(this);
            }
        }

        protected virtual void Move()
        {
            
        }

        static public event System.Action<MovingCharacter> OnChangingPosition;

    }
}