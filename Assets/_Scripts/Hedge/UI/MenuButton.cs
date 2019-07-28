using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Hedge
{ namespace UI
    {
        public class MenuButton : MonoBehaviour
        {

            public void GoToMenu()
            {
                SceneManager.LoadSceneAsync(0);
            }



        }

    }
}
