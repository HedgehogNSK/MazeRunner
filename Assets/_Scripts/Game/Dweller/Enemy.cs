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
       

        bool Alarm(Coordinates other) => Coords.SqrDistance(other) < alertRange * alertRange;
        bool IsObjToFarToChase(Coordinates other) => Coords.SqrDistance(other) > chaseRange * chaseRange;
        private List<Coordinates> PatrolPath => Map.AStar(Coords, patrolPoints.ElementAt(UnityEngine.Random.Range(0, patrolPoints.Count())));

        static public event Action<Dweller> Gotcha;

        MovingCharacter targetCharacter;

        protected override void Awake()
        {
            base.Awake();
            OnChangingPosition += OnOtherCharMove;
        }

        public override void Init(Coordinates startPosition)
        {
            base.Init(startPosition);
            if (Map != null)
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
            CheckMoveState(targetCharacter);
            SearchPath(targetCharacter);

            if (movePath.Any())
            {              
                Vector2 difference = rigid.position - (Vector2)movePath[0].ToWorld; 
                if (difference.sqrMagnitude < 1e-4)
                    movePath.RemoveAt(0);

                Vector2 velocity =Vector2.zero;
                
                //If enemy is between current and next waypoint, than remove current waypoint
                if (movePath.Count > 1)
                {
                    Vector2 pathDirection = (movePath[1] - movePath[0]).ToVector2;
                    Vector2 directionToCellCenter = GetDirectionTo(movePath[0]);

                    if (Mathf.Sign(directionToCellCenter.x) == -Mathf.Sign(pathDirection.x) && directionToCellCenter.y == pathDirection.y ||
                         Mathf.Sign(directionToCellCenter.y) == -Mathf.Sign(pathDirection.y) && directionToCellCenter.x == pathDirection.x)
                    {                       
                        movePath.RemoveAt(0);
                    }
                }
                if (movePath.Any())
                {
                    velocity = CalcVelocity(movePath[0]);
                }

                rigid.velocity = velocity;

            }
        }

        Coordinates cachedCoords;
        Vector2 cachedDistance = Vector2.zero;
        private Vector2 CalcVelocity(Coordinates coords)
        {
            Vector2 speedVertex = Speed * GetDirectionTo(coords);

            bool getOverTargetInNextFram =((Vector2)coords.ToWorld - rigid.position).sqrMagnitude*2 < cachedDistance.sqrMagnitude;                    
            if(getOverTargetInNextFram && coords.Equals(cachedCoords))
            {
                rigid.MovePosition(Coords.ToWorld);
                speedVertex = Vector2.zero;
            }
            
            cachedCoords = coords;
            cachedDistance = (Vector2)Coords.ToWorld - rigid.position;
            return speedVertex;
        }
        private Vector2 GetDirectionTo(Coordinates coords)
        {
            Vector2 Vertex = coords.ToWorld - transform.position;
            
            if (Mathf.Abs(Vertex.x) <= 1e-4f && Mathf.Abs(Vertex.y) <= 1e-4f) return Vector2.zero;
            return Vertex.normalized;
        }

        public void CheckMoveState(MovingCharacter target)
        {
            if (!target) return;
            //Check if target has gone to far
            if (IsObjToFarToChase(target.Coords) && pursuit)
            {
                pursuit = false;
                movePath.Clear();
                currentSpeed = baseSpeed;
                return;
            }

            //Check if target to near
            if (Alarm(target.Coords) && !pursuit)
            {
                pursuit = true;
                movePath.Clear();
                currentSpeed = target.Speed * speedDamper;
            }
        }
        private void SearchPath(MovingCharacter target)
        {


            if (target && pursuit)
            {
                movePath = Map.AStar(Coords, target.Coords);
            }
            else
            {
                if (!movePath.IsAny())
                {
                    movePath = PatrolPath;

                }
            }


        }
        private void OnOtherCharMove(MovingCharacter target)
        {
            if (target is PlayerController)
            {
                this.targetCharacter = target;
            }

        }

        public void StopGame()
        {
            OnChangingPosition -= OnOtherCharMove;
            targetCharacter = null;
            currentSpeed = 0;
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