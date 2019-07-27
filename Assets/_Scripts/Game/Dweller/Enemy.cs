using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hedge.Tools;

namespace Maze.Game
{
    using Explorer;

    public class Enemy : MovingCharacter
    {

        [SerializeField] float speedDamper = 0.9f;
        [SerializeField]
        float observableDistance = 5;
        [SerializeField] float unobservableDistance = 6;
        float sqrObservDistance, sqrUnobservDistance;
        
        List<Coordinates> waypoints = new List<Coordinates> ();
        Coordinates cachedCoords = new Coordinates(int.MinValue, int.MinValue);

        protected override void Awake()
        {
            base.Awake();
            OnChangingPosition += HotPursuit;
        }
        // Start is called before the first frame update
        void Start()
        {            
            sqrObservDistance = observableDistance * observableDistance;
            sqrUnobservDistance = unobservableDistance * unobservableDistance;
        }

        protected override void FixedUpdate()
        {           
            Move();
        }

        protected override void Move()
        {
            //When the player hooked up by enemy.
            if (waypoints.Count!=0)
            {
                
                Vector2 direction = GetDirectionTo(waypoints[0]);

                rigid.velocity = CalcVelocity(direction, waypoints[0]);

                cachedCoords = waypoints[0];
                if (direction == Vector2.zero)
                    waypoints.RemoveAt(0);

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
            if (cachedCoords == coords && (Mathf.Sign(rigid.velocity.x) != Mathf.Sign(speedVertex.x) || Mathf.Sign(rigid.velocity.y) != Mathf.Sign(speedVertex.y)))
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
                if (Coords.SqrDistance(obj.Coords) > sqrUnobservDistance)
                {
                    waypoints.Clear();
                    return;
                }
                if (Coords.SqrDistance(obj.Coords) <= sqrObservDistance)
                {
                    waypoints = map.Pathfinder(Coords, obj.Coords);
                    waypoints.RemoveAt(0);
                    speed = obj.Speed * speedDamper;
                }
            }

           
        }

        private void OnDestroy()
        {
            OnChangingPosition -= HotPursuit;
        }
    }
}