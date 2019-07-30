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
        //Action throw true if win, false if lose
        static public event Action<bool> GameOver;
        Transform camTransform;
        Camera cam;
        Vector3 viewportHalfDelta;
        [Range(1.0f,1.20f)]
        [SerializeField]float visibleBehindBoundsRate = 1.01f;
        const float delayForCoroutines = 3;

#pragma warning disable CS0649
        [SerializeField] Maze mazePrefab;    
        [SerializeField] PlayerController playerPrefab;
        [SerializeField] Coin coinPrefab;
        [SerializeField] Enemy enemyPrefab;
#pragma warning restore CS0649

        new AudioSource audio;

        private Maze maze;
        PlayerController player;
        List<Enemy> enemies;
        List<Coin> coins;

        public int Points { get; private set; }
        public bool IsGameOver
        {
            get
            {
                if (coins ==null || coins.Count == 0 )
                {
                    return true;
                }
                if (coins == null) Debug.Log("Список не инициализирован");
                else Debug.Log("Осталось ещё монет:" + coins.Count);
                return false;
            }
        }

        void Awake()
        {
            audio = GetComponent<AudioSource>();
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

        void MoveCamera()
        {
            Bounds mazeBounds = maze.GetBounds();
            
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

            maze = Instantiate(mazePrefab) as Maze;
            yield return new WaitForEndOfFrame();
#if _DEBUG
                var watch = System.Diagnostics.Stopwatch.StartNew();
                watch.Start();
#endif
            maze.Generate();
#if _DEBUG
              watch.Stop();
              Debug.Log("Labyrinth generation time: " + watch.ElapsedMilliseconds / 1000f);
#endif
            Dweller.Map = maze.Map;

            LevelSettings level = LevelFactory.Create(PlayerPrefs.GetInt("level", 1));
            player = DwellerFactory.Create(playerPrefab, maze.Size.GetCenter);
            yield return new WaitForEndOfFrame();           
            coins = DwellerFactory.CreateSet(coinPrefab, level,transform, player.Coords);
            yield return new WaitForEndOfFrame();
#if _DEBUG
                watch = System.Diagnostics.Stopwatch.StartNew();
                watch.Start();
#endif
            enemies = DwellerFactory.CreateSet(enemyPrefab, level,transform, player.Coords);
#if _DEBUG
              watch.Stop();
              Debug.Log("Enemies spawn time: " + watch.ElapsedMilliseconds / 1000f);
#endif
            yield return new WaitForEndOfFrame();
            Coin.OnCollect += OnCoinCollect;
            ControllButtons.PressedButton += player.Move;
            Enemy.Gotcha += OnCaughtByEnemy;
            CounterText.Update?.Invoke(TextType.Points, Points = 0);           

        }

        void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            //If Interface is loaded we can start
            if (arg0.buildIndex == 1)
                StartCoroutine(LoadGame());
        }

        void OnCoinCollect(Coin obj)
        {

            Points++;
            coins.Remove(obj);
            audio.Play();
            CounterText.Update?.Invoke(TextType.Points, Points);
            if (IsGameOver)
            {
                GameOver?.Invoke(true);
                StartCoroutine(EndGame(delayForCoroutines));
            }

        }

        void OnCaughtByEnemy(Dweller killer)
        {            
            GameOver?.Invoke(false);
            StartCoroutine(EndGame(delayForCoroutines));
        }

        IEnumerator EndGame(float delay)
        {            
            foreach (var enemy in enemies) enemy.StopGame();

            Coin.OnCollect -= OnCoinCollect;
            Enemy.Gotcha -= OnCaughtByEnemy;
            ControllButtons.PressedButton -= player.Move;          

            yield return new WaitForSeconds(delay);
            if(maze)
            Destroy(maze.gameObject);
            if(player)
            Destroy(player.gameObject);
            foreach (var coin in coins)
               if(coin) Destroy(coin.gameObject);
            coins.Clear();
            foreach (var enemy in enemies)
                if (enemy) Destroy(enemy.gameObject);
            enemies.Clear();
            
        }

        IEnumerator RestartGame()
        {
           yield return StartCoroutine(EndGame(0));           
           StartCoroutine(LoadGame());
        }


        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
    }
}