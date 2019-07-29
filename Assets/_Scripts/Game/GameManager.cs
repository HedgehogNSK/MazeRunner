using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hedge.Tools;
using Hedge.UI;
using UnityEngine.SceneManagement;
using Maze.Explorer;
using System;

namespace Maze.Game

{
    public class GameManager : MonoBehaviour
    {
        #region SINGLETON
        static private GameManager _instance;
        static public GameManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<GameManager>();
                }
                return _instance;
            }
        }
        #endregion
        Transform camTransform;
        Camera cam;
        Vector3 viewportHalfDelta;
        float visibleBehindBoundsRate = 1.03f;
        

        [SerializeField] Maze mazePrefab;
        private Maze mazeInstance;

        [SerializeField] PlayerController playerPrefab;
        [SerializeField] Coin coinPrefab;
        [SerializeField] Enemy enemyPrefab;
        PlayerController player;
        List<Enemy> enemies;
        List<Coin> coins;

        public int Points { get; private set; }
        public bool IsGameOver
        {
            get
            {
                if (coins == null)
                {
                    return true;
                }
                return false;
            }
        }

        private void Awake()
        {
            cam = Camera.main;
            camTransform = Camera.main.transform;
            viewportHalfDelta = (cam.ViewportToWorldPoint(Vector3.one) - cam.ViewportToWorldPoint(Vector3.zero))/(2* visibleBehindBoundsRate);
            SceneManager.sceneLoaded += OnSceneLoad;

        }

      
        // Update is called once per frame
        void Update()
        {

#if KEYBOARD
            if (Input.GetKeyDown(KeyCode.Space))
            {
               StartCoroutine( RestartGame());
            }
#endif
            if(player)
                MoveCamera();
        }

        private void MoveCamera()
        {
            Bounds mazeBounds = mazeInstance.GetBounds();
            
            camTransform.position = new Vector3(
               (player.transform.position.x - mazeBounds.min.x) < viewportHalfDelta.x? mazeBounds.min.x + viewportHalfDelta.x: 
               (mazeBounds.max.x - player.transform.position.x)< viewportHalfDelta.x ? mazeBounds.max.x - viewportHalfDelta.x:
                        player.transform.position.x,
               (player.transform.position.y - mazeBounds.min.y) < viewportHalfDelta.y ? mazeBounds.min.y + viewportHalfDelta.y :
               (mazeBounds.max.y - player.transform.position.y) < viewportHalfDelta.y ? mazeBounds.max.y - viewportHalfDelta.y :
                        player.transform.position.y,
                        camTransform.position.z
               );
        }

        IEnumerator LoadGame()
        {
            
            mazeInstance = Instantiate(mazePrefab) as Maze;
            yield return new WaitForEndOfFrame();
            mazeInstance.Generate();
            Dweller.Map = mazeInstance.Structure;
            LevelSettings level = LevelFactory.Create(PlayerPrefs.GetInt("level", 1));
            player = DwellerFactory.Create(playerPrefab, mazeInstance.Size.GetCenter);
            yield return new WaitForEndOfFrame();           
            coins = DwellerFactory.CreateSet(coinPrefab, level,transform);
            yield return new WaitForEndOfFrame();
            enemies = DwellerFactory.CreateSet(enemyPrefab, level,transform);           
            yield return new WaitForEndOfFrame();
            Coin.OnCollect += OnCoinCollect;
            ControllButtons.PressedButton += player.Move;
            Enemy.Gotcha += OnCaughtByEnemy;
           CounterText.Update?.Invoke(TextType.Points, Points = 0);
           

        }

        private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            //If Interface is loaded we can start
            if (arg0.buildIndex == 1)
                StartCoroutine(LoadGame());
        }

        private void OnCoinCollect(Coin obj)
        {

            Points++;
            CounterText.Update?.Invoke(TextType.Points, Points);
            if (IsGameOver)
                StartCoroutine(EndGame(2));

        }

        void OnCaughtByEnemy(Dweller killer)
        {
            Enemy.Gotcha -= OnCaughtByEnemy;
            StartCoroutine(EndGame(2));
        }

        IEnumerator EndGame(float delay)
        {
            yield return new WaitForSeconds(delay);
            if(mazeInstance)
            Destroy(mazeInstance.gameObject);
            if(player)
            Destroy(player.gameObject);
            foreach (var coin in coins)
               if(coin) Destroy(coin.gameObject);
            coins.Clear();
            foreach (var enemy in enemies)
                if (enemy) Destroy(enemy.gameObject);
            enemies.Clear();
            Coin.OnCollect -= OnCoinCollect;
            ControllButtons.PressedButton -= player.Move;
        }

        IEnumerator RestartGame()
        {
           yield return StartCoroutine(EndGame(0));
           
           StartCoroutine(LoadGame());
        }


        private void OnDestroy()
        {
           
        }
    }
}