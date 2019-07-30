using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Maze
{ namespace Game
    {
        public class UIManager : MonoBehaviour
        {
#pragma warning disable CS0649
            [SerializeField]UnityEngine.UI.Text txt;
#pragma warning restore CS0649
            const float delayBeforeClose = 3;
            public void GoToMenu()
            {
                SceneManager.LoadSceneAsync(0);
            }
            private void Awake()
            {
                GameManager.GameOver += LoadWinScreen;
            }

            private void Start()
            {
                txt.gameObject.SetActive(false);
            }
            private void OnDestroy()
            {
                GameManager.GameOver -= LoadWinScreen;
            }

            void LoadWinScreen(bool win)
            {
                txt.text = win ? "YOU WIN!" : "GAME OVER";
                txt.gameObject.SetActive(true);
                StartCoroutine(UnloadWinScreen(delayBeforeClose));
            }
            IEnumerator UnloadWinScreen(float delaySeconds)
            {
                yield return new WaitForSeconds(delaySeconds); txt.gameObject.SetActive(false);
            }

        }

    }
}
