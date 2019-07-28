using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hedge
{
    namespace UI
    {
        public class CustomButton : UnityEngine.UI.Button
        {
            public event System.Action<PointerEventData> OnPress;
            public event System.Action<PointerEventData> OnRelease;

            public override void OnPointerDown(PointerEventData eventData)
            {
                OnPress?.Invoke(eventData);
            }

            public override void OnPointerUp(PointerEventData eventData)
            {
                OnRelease?.Invoke(eventData);
            }

        }
    }
}

