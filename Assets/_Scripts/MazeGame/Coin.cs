using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze;

namespace MazeGame
{
   
    public class Coin : MonoBehaviour
    {
        CellCoordinates coords;
        public CellCoordinates Coords
        {
            get { return coords; }
            set
            {
                coords = value;
                transform.localPosition = coords.ToWorld();
            }
        }
        static public event System.Action OnCollect;

        public Coin(CellCoordinates coords)
        {
            Coords = coords;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Зашли");
            if (collision.tag == "Player")
            {
                Destroy(gameObject, 0.2f);
                OnCollect?.Invoke();
            }

        }

        
        
    }
}