using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{
   
    public class Coin : Dweller
    {        
        
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