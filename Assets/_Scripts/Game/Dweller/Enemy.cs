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
        
        List<Coordinates> waypoints = new List<Coordinates> ();
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

            if (waypoints.Count != 0 && rigid.position == (Vector2)waypoints[0].ToWorld)
            {
                    waypoints.RemoveAt(0);                
            }

            Vector2 direction;
            //When the player hooked up by enemy.
            if (waypoints.Count > 1)
            {
                bool condition = (((Vector2)waypoints[1].ToWorld - (Vector2)waypoints[0].ToWorld).sqrMagnitude < (rigid.position - (Vector2)waypoints[0].ToWorld).sqrMagnitude)
                && (Coordinates.FromWorld(rigid.position).Equals(waypoints[0]) || Coordinates.FromWorld(rigid.position).Equals(waypoints[1]));
                if (condition)
                    waypoints.RemoveAt(0);

            }
            if (waypoints.Count != 0)
            {
               
                direction = GetDirectionTo(waypoints[0]);

                rigid.velocity = CalcVelocity(direction, waypoints[0]);

                cachedCoords = waypoints[0];

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
            return speedVertex;
        }

        private Vector2 GetDirectionTo(Coordinates coords)
        {
            Vector2 Vertex = coords.ToWorld - transform.position;
            if (Mathf.Abs(Vertex.x) <= 0.001f && Mathf.Abs(Vertex.y) <= 0.001f) return Vector2.zero;

            if (Mathf.Abs(Vertex.x) > Mathf.Abs(Vertex.y)) Vertex /= Mathf.Abs(Vertex.x);
            else Vertex /= Mathf.Abs(Vertex.y);
            return Vertex;



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
                    waypoints = Map.Pathfinder(Coords, obj.Coords);
                    //waypoints.RemoveAt(0);
                    speed = obj.Speed * speedDamper;
                }
            }

           
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != "Player") return;

            OnChangingPosition -= HotPursuit;
            waypoints.Clear();
            Gotcha(this);
        }

        private void OnDestroy()
        {
            OnChangingPosition -= HotPursuit;
        }
    }
}