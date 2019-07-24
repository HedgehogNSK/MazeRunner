using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed = 3;
        Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            rigid.velocity = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow)) rigid.velocity = speed * Vector2.up;
            if (Input.GetKey(KeyCode.DownArrow)) rigid.velocity = speed * Vector2.down;
            if (Input.GetKey(KeyCode.LeftArrow)) rigid.velocity = speed * Vector2.left;
            if (Input.GetKey(KeyCode.RightArrow)) rigid.velocity = speed * Vector2.right;
        }
    }
}