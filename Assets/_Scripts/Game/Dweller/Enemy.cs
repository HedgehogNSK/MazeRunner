using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Graph map;
        List<Coordinates> itinerary = new List<Coordinates> ();
        

        public override void Init(Dweller prefab, Graph mazeGraph)
        {         
            map = mazeGraph;
            OnMove += HotPursuit;            
            sqrObservDistance = observableDistance * observableDistance;
            sqrUnobservDistance = unobservableDistance * unobservableDistance;


        }

       
        // Start is called before the first frame update
        void Start()
        {
           
        }

        void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            Coords = Coordinates.FromWorld(transform.position);
            Vector2 diff =Coords.ToWorld - transform.position;
            if (diff != Vector2.zero)
            {
                rigid.velocity = Speed * diff.x > 0 ? new Vector2(1, 0) : diff.x < 0 ? new Vector2(-1, 0) : diff.y > 0 ? new Vector2(0, 1) : diff.y < 0 ? new Vector2(0, 1) : Vector2.zero;
                return;
            }
            if (itinerary.Count!=0)
            {               
                if (Coords == itinerary[0])
                    itinerary.RemoveAt(0);
                if(itinerary.Count!=0 && Coords != itinerary[0])
                {
                    rigid.velocity = Speed * (itinerary[0]- Coords).ToVertex();
                }
            }

            else
            {
             //Place for random walking of enemies;   
                rigid.velocity = Vector3.zero;
            }

        }
        private void HotPursuit(MovingCharacter obj)
        {
            
            if (!(obj is PlayerController)) return;

            if (Coords.SqrDistance(obj.Coords) > sqrUnobservDistance)
            {
                itinerary.Clear();
                return;
            }
            if (Coords.SqrDistance(obj.Coords) <= sqrObservDistance)
            {
                Debug.Log(obj.GetType());
                itinerary = map.Pathfinder(coords, obj.Coords);
                speed = obj.Speed * speedDamper;
            }

           
        }

        private void OnDestroy()
        {
            OnMove -= HotPursuit;
        }
    }
}