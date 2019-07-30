using System;
using System.Linq;
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
        float alertRange = 5;
        float chaseRange = 6;
        const int borderPatrolSize = 2;
        bool pursuit = false;

        List<Coordinates> movePath = new List<Coordinates>();
        IEnumerable<Coordinates> patrolPoints = new List<Coordinates>();
        Coordinates cachedCoords = new Coordinates(int.MinValue, int.MinValue);

        bool Alarm(Coordinates other) => Coords.SqrDistance(other) < alertRange * alertRange;
        bool IsObjToFarToChase(Coordinates other) => Coords.SqrDistance(other) > chaseRange * chaseRange;
        private List<Coordinates> PatrolPath => Map.AStar(Coords, patrolPoints.ElementAt(UnityEngine.Random.Range(0, patrolPoints.Count())));

        static public event Action<Dweller> Gotcha;

        Coordinates lastPlayerCoords;

        protected override void Awake()
        {
            base.Awake();
            OnChangingPosition += OnOtherCharMove;
        }

        public override void Init(Coordinates startPosition)
        {
           base.Init(startPosition);
           if(Map!=null)
            {
                patrolPoints = Map.NeighboursAround(startPosition, borderPatrolSize);
            }
        }

        public void SetParams(float alertRange, float chaseRange)
        {
            this.alertRange = alertRange;
            this.chaseRange = chaseRange;
        }
        protected override void FixedUpdate()
        {
           
            Move(); 
        }


        protected override void Move()
        {
            //Situation when enemy becomes between first and second waypoint 
            if(movePath.Count>1)
            {
                Vector2 pathDirection= (movePath[1] - movePath[0]).ToVector2;
                Vector2 direction =  GetDirectionTo(movePath[0]);

                if ( Mathf.Sign(direction.x) == -Mathf.Sign(pathDirection.x) && direction.y ==pathDirection.y ||
                     Mathf.Sign(direction.y) == -Mathf.Sign(pathDirection.y) && direction.x == pathDirection.x)
                {
                    movePath.RemoveAt(0);
                }
            }
            if (movePath.Count != 0)
            {
                rigid.velocity = CalcVelocity(GetDirectionTo(movePath[0]), movePath[0]);
                if (rigid.velocity == Vector2.zero)
                {
                    movePath.RemoveAt(0);
                }
            }
            //Place for random walking of enemies;   
            else
            {
                
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
            cachedCoords = movePath[0];
            return speedVertex;
        }

        private Vector2 GetDirectionTo(Coordinates coords)
        {
            Vector2 Vertex = coords.ToWorld - transform.position;
            if (Mathf.Abs(Vertex.x) <= 0.001f && Mathf.Abs(Vertex.y) <= 0.001f) return Vector2.zero;
            return Vertex.normalized;
        }


       
        private void OnOtherCharMove(MovingCharacter obj)
        {

            if (obj is PlayerController)
            {
                lastPlayerCoords = obj.Coords;
                if (IsObjToFarToChase(lastPlayerCoords) && pursuit)
                {
                    movePath.Clear();
                    currentSpeed = baseSpeed;
                    pursuit = false;
                    return;
                }


                if (Alarm(lastPlayerCoords)) pursuit = true;
                SearchPursuitPath(lastPlayerCoords);
                currentSpeed = obj.Speed * speedDamper;
                
            }


        }

        private void SearchPursuitPath(Coordinates target)
        {
            

            if (pursuit)
            {
                if (movePath.IsAny())
                {
                    IEnumerable<Coordinates> newPath = Map.AStar(movePath.Last(), target);
                    IEnumerable<Coordinates> tmp = movePath.Intersect(newPath);
                    tmp = tmp.Except(new Coordinates[] { tmp.FirstOrDefault() });
                    movePath = movePath.Union(newPath).Except(tmp).Distinct().ToList();

                }
                else
                    movePath = Map.AStar(Coords, target);
                
            }
            else
            {
                if (!movePath.IsAny())
                {
                    movePath = PatrolPath;
                }
            }
            

        }

        public void StopGame()
        {
            OnChangingPosition -= OnOtherCharMove;
            movePath.Clear();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != "Player") return;

            OnChangingPosition -= OnOtherCharMove;
            movePath.Clear();
            Gotcha?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnChangingPosition -= OnOtherCharMove;
        }
    }
}