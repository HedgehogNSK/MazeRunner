using System.Collections;
using System.Collections.Generic;
using Maze.Explorer;
using UnityEngine;

namespace Maze.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MovingCharacter : MonoBehaviour,Dweller
    {
        [SerializeField]protected float speed = 3;
        public float Speed => speed;

        protected Coordinates coords = new Coordinates(0,0);
        public Coordinates Coords
        {
            get { return coords; }
            protected set
            {
                if (value != coords)
                {
                    coords = value;
                    OnMove?.Invoke(this);
                }                   
                
            }
        }

        protected Rigidbody2D rigid;
        protected virtual void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        static public event System.Action<MovingCharacter> OnMove;


        public abstract void Init(Dweller prefab, Graph mazeStructure);

    }
}