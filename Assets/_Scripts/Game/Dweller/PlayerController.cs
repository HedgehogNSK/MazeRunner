using System.Collections;
using System.Collections.Generic;
using Maze.Explorer;
using UnityEngine;
using Hedge.UI;

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
#if KEYBOARD
            MoveByKeyboard();
#endif
        }

#if KEYBOARD
        private void MoveByKeyboard()
        {
            rigid.velocity = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow)) rigid.velocity = speed * Vector2.up;
            if (Input.GetKey(KeyCode.DownArrow)) rigid.velocity = speed * Vector2.down;
            if (Input.GetKey(KeyCode.LeftArrow)) rigid.velocity = speed * Vector2.left;
            if (Input.GetKey(KeyCode.RightArrow)) rigid.velocity = speed * Vector2.right;


        }

#endif
        public void Move(PressedButton button)
        {
            switch(button)
            {
                case PressedButton.Up: rigid.velocity = speed * Vector2.up; break;
                case PressedButton.Down: rigid.velocity = speed * Vector2.down; break;
                case PressedButton.Left:  rigid.velocity = speed * Vector2.left; ; break;
                case PressedButton.Right: rigid.velocity = speed * Vector2.right; break;
                default: rigid.velocity = Vector2.zero;break;
                   
            }
        }

    }
}