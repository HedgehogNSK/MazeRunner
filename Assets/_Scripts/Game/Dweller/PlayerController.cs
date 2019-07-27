﻿using System.Collections;
using System.Collections.Generic;
using Maze.Explorer;
using UnityEngine;

namespace Maze.Game
{
    sealed public class PlayerController : MovingCharacter
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
         protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Move();
        }

        protected override void Move()
        {
            rigid.velocity = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow)) rigid.velocity = speed * Vector2.up;
            if (Input.GetKey(KeyCode.DownArrow)) rigid.velocity = speed * Vector2.down;
            if (Input.GetKey(KeyCode.LeftArrow)) rigid.velocity = speed * Vector2.left;
            if (Input.GetKey(KeyCode.RightArrow)) rigid.velocity = speed * Vector2.right;

        }

    }
}