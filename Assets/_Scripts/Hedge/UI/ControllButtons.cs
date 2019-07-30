using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
namespace Hedge
{
    namespace UI
    {
        public enum PressedButton
        {
            None,
            Up,
            Down,
            Left,
            Right            
        }
        public class ControllButtons : MonoBehaviour
        {
#pragma warning disable CS0649
            [SerializeField] CustomButton upButton;
            [SerializeField] CustomButton leftButton;
            [SerializeField] CustomButton rightButton;
            [SerializeField] CustomButton downButton;
#pragma warning restore CS0649
            PressedButton current;
            Dictionary<PressedButton,CustomButton> buttonsDictionary = new Dictionary<PressedButton, CustomButton>();
            private void Awake()
            {
                buttonsDictionary[UI.PressedButton.Up] = upButton;
                buttonsDictionary[UI.PressedButton.Left] = leftButton;
                buttonsDictionary[UI.PressedButton.Right] = rightButton;
                buttonsDictionary[UI.PressedButton.Down] = downButton;

                foreach(var button in buttonsDictionary.Values)
                {
                    button.OnPress+= OnButtonPressed;
                    button.OnRelease += OnButtonRelease;
                }                

            }

            private void OnButtonPressed(PointerEventData eventData)
            {
               current = buttonsDictionary.Single(x => x.Value.gameObject == eventData.pointerPressRaycast.gameObject).Key;             
                PressedButton?.Invoke(current);
            }

            private void OnButtonRelease(PointerEventData eventData)
            {
                if(eventData.pointerPressRaycast.gameObject == buttonsDictionary[current].gameObject)
                PressedButton?.Invoke(UI.PressedButton.None);               
            }

            static public event System.Action<PressedButton> PressedButton;
        }
    } }

