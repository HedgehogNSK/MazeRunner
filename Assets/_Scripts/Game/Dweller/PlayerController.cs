using System.Collections;
using System.Collections.Generic;
using Maze.Explorer;
using UnityEngine;
using Hedge.UI;

namespace Maze.Game
{
    sealed public class PlayerController : MovingCharacter
    {       
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
            if (Input.GetKey(KeyCode.UpArrow)) rigid.velocity = currentSpeed * Vector2.up;
            if (Input.GetKey(KeyCode.DownArrow)) rigid.velocity = currentSpeed * Vector2.down;
            if (Input.GetKey(KeyCode.LeftArrow)) rigid.velocity = currentSpeed * Vector2.left;
            if (Input.GetKey(KeyCode.RightArrow)) rigid.velocity = currentSpeed * Vector2.right;


        }

#endif
        public void Move(PressedButton button)
        {
            switch(button)
            {
                case PressedButton.Up: rigid.velocity = currentSpeed * Vector2.up; break;
                case PressedButton.Down: rigid.velocity = currentSpeed * Vector2.down; break;
                case PressedButton.Left:  rigid.velocity = currentSpeed * Vector2.left; ; break;
                case PressedButton.Right: rigid.velocity = currentSpeed * Vector2.right; break;
                default: rigid.velocity = Vector2.zero;break;
                   
            }
        }

    }
}