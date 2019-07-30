using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hedge.Tools;

namespace Maze.Game
{
    using Explorer;
    [Serializable]
    public class Enemy : MovingCharacter
    {

        [SerializeField] float speedDamper = 0.9f;
        [SerializeField]
        float alertRange = 5;
        [SerializeField] float chaseRange = 6;
        float sqrAlertRange, sqrChaseRange;

        List<Coordinates> waypoints = new List<Coordinates>();
        Coordinates cachedCoords = new Coordinates(int.MinValue, int.MinValue);

        static public event Action<Dweller> Gotcha;

        protected override void Awake()
        {
            base.Awake();
            OnChangingPosition += HotPursuit;
        }
        // Start is called before the first frame update
        void Start()
        {
            sqrAlertRange = alertRange * alertRange;
            sqrChaseRange = chaseRange * chaseRange;
        }

        public void SetParams(float alertRange, float chaseRange)
        {
            this.alertRange = alertRange;
            this.chaseRange = chaseRange;
            sqrAlertRange = alertRange * alertRange;
            sqrChaseRange = chaseRange * chaseRange;
        }
        protected override void FixedUpdate()
        {
            Move();
        }


        protected override void Move()
        {
            //Situation when enemy becomes between first and second waypoint 
            if(waypoints.Count>1)
            {
                Vector2 direction= (waypoints[1] - waypoints[0]).ToVector2;
                Vector2 playerDirection =  GetDirectionTo(waypoints[0]);

                if ( Mathf.Sign(playerDirection.x) == -Mathf.Sign(direction.x) && playerDirection.y ==direction.y ||
                     Mathf.Sign(playerDirection.y) == -Mathf.Sign(direction.y) && playerDirection.x == direction.x)
                {
                    waypoints.RemoveAt(0);
                }
            }
            if (waypoints.Count != 0)
            {
                rigid.velocity = CalcVelocity(GetDirectionTo(waypoints[0]), waypoints[0]);
                if (rigid.position == (Vector2)waypoints[0].ToWorld)
                {
                    waypoints.RemoveAt(0);
                }
            }


            //Place for random walking of enemies;   
            else
            {
                rigid.velocity = Vector3.zero;
            }
        }
        
        private Vector2 CalcVelocity(Vector2 direction, Coordinates coords)
        {
            Vector2 speedVertex = Speed * direction;            

            //Overjump check
            if (cachedCoords.Equals(coords) && (Mathf.Sign(rigid.velocity.x) != Mathf.Sign(speedVertex.x) || Mathf.Sign(rigid.velocity.y) != Mathf.Sign(speedVertex.y)))
            {
                rigid.MovePosition(coords.ToWorld);
                speedVertex = Vector2.zero;
            }
            cachedCoords = waypoints[0];
            return speedVertex;
        }

        private Vector2 GetDirectionTo(Coordinates coords)
        {
            Vector2 Vertex = coords.ToWorld - transform.position;
            if (Mathf.Abs(Vertex.x) <= 0.001f && Mathf.Abs(Vertex.y) <= 0.001f) return Vector2.zero;
            return Vertex.normalized;
        }

        private void RandomWalking()
        {
           
        }

        private void HotPursuit(MovingCharacter obj)
        {

            if ((obj is PlayerController))

            {
                if (Coords.SqrDistance(obj.Coords) > sqrChaseRange)
                {
                    waypoints.Clear();
                    return;
                }
                if (Coords.SqrDistance(obj.Coords) <= sqrAlertRange)
                {
                    waypoints = Map.AStar(Coords, obj.Coords);                    
                    speed = obj.Speed * speedDamper;
                }
            }

           
        }

        public void StopGame()
        {
            OnChangingPosition -= HotPursuit;
            waypoints.Clear();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != "Player") return;

            OnChangingPosition -= HotPursuit;
            waypoints.Clear();
            Gotcha?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnChangingPosition -= HotPursuit;
        }
    }
}