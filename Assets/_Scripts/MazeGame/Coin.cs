using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze;

namespace MazeGame
{
   
    public class Coin : MonoBehaviour
    {
        Coordinates coords;
        public Coordinates Coords
        {
            get { return coords; }
            set
            {
                coords = value;
                transform.localPosition = coords.ToWorld;
            }
        }
        static public event System.Action OnCollect;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Destroy(gameObject, 0.2f);
                OnCollect?.Invoke();
            }

        }        
        
    }
}