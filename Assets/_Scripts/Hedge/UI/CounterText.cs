using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Hedge.Tools;

namespace Hedge
{
    namespace UI
    {
        public enum TextType
        {
            Points
        }


        [RequireComponent(typeof(Text))]
        public class CounterText : MonoBehaviour
        {
            Text counterTXT;
            public TextType txtType;
            static public Action<TextType, object> Update;

            private void Awake()
            {

            }

            public CounterText()
            {
                Update += TextCatcher;
            }


            void ChangeText(string str)
            {
                counterTXT.text = str;
            }
            void ChangeText(int str)
            {
                ChangeText((float)str);
            }

            void ChangeText(float number)
            {

                counterTXT.text = number.ToShortNumber();
            }


            void TextCatcher(TextType txtType, object obj)
            {
                if (!this || this.txtType != txtType) return;
                //Проверка необходима здесь, т.к. объект может быть отключен в начале сцены, но информацию уже должен получать
                if (!counterTXT) counterTXT = GetComponent<Text>();
                if (counterTXT)
                {
                    if ((obj is Int32) || (obj is Single))
                    {
                        ChangeText((int)obj);
                    }
                    else if (obj is String)
                    {
                        ChangeText((string)obj);
                    }
                    else
                    {
                        Debug.LogWarning("Для данного типа данных не написан сценарий обработки.");
                    }
                }
            }


            private void OnDestroy()
            {
                Update -= TextCatcher;
            }
        }
    }
}