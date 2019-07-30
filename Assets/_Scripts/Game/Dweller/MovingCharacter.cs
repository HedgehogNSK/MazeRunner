using System.Collections;
using System.Collections.Generic;
using Maze.Explorer;
using UnityEngine;

namespace Maze.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MovingCharacter : Dweller
    {
        [SerializeField] protected float baseSpeed = 3;
        public float BaseSpeed => BaseSpeed;
        protected float currentSpeed = 3;
        public float Speed => currentSpeed;        

        Coordinates currentCoords = new Coordinates(0,0);

        protected Rigidbody2D rigid;
        protected virtual void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            currentSpeed = baseSpeed;
        }


        protected virtual void FixedUpdate()
        {
            if (!currentCoords.Equals(Coords))
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