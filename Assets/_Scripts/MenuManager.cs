using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Maze
{
    namespace Game
    {
        public class MenuManager : MonoBehaviour
        {
            [SerializeField]GameObject[] screens;
            [SerializeField]Button loadLvLbtn;
            [SerializeField]RectTransform content;
            private void Awake()
            {
                screens[0].SetActive(true);
                for (int i = 1; i < screens.Length; i++)
                    screens[i].SetActive(false);

                CreateLvLButtons(50);
            }

            public void LoadScreen(int id)
            {
                for(int i=0; i!=screens.Length;i++)
                    screens[i].SetActive(screens[id] == screens[i]);                
            }

            public void CreateLvLButtons(int lvlsAmount)
            {
                for(int i=1; i!=lvlsAmount+1; i++)
                {
                    Button btn = Instantiate(loadLvLbtn, content);
                    UnityEditor.Events.UnityEventTools.AddIntPersistentListener(btn.onClick, LoadLevel, i);                   
                    btn.GetComponent<Text>().text = "Level " + i;
                }
                RectTransform shopContentRectTransform = content.GetComponent<RectTransform>();
                float spaceBetween = content.GetComponent<VerticalLayoutGroup>().spacing;
                shopContentRectTransform.sizeDelta = new Vector2(shopContentRectTransform.sizeDelta.x,
                    lvlsAmount * (loadLvLbtn.transform.GetComponent<RectTransform>().rect.height + spaceBetween) - spaceBetween);
            }
           

            public void LoadLevel(int param)
            {
                Debug.Log(param);
                PlayerPrefs.SetInt("level", param);
                PlayerPrefs.Save();
                SceneManager.LoadSceneAsync(1);
                SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            }

            public void Exit()
            {
                Application.Quit();
            }

            
        }

    }
}
