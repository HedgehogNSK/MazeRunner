using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    static public event System.Action OnCollect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Зашли");
        if(collision.tag == "Player")
        {
            Destroy(gameObject, 0.2f);
            OnCollect?.Invoke();
        }
          
    }
}
