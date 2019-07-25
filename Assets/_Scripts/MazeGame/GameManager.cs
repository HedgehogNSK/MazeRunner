using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hedge.Tools;

namespace MazeGame

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
        float visibleBehindBoundsRate = 1.04f;
        [SerializeField] PlayerController playerPrefab;
        PlayerController player;

        [SerializeField] Maze.Maze mazePrefab;
        private Maze.Maze mazeInstance;

        [SerializeField] Coin coinPrefab;
        Coin coin;
        private void Awake()
        {
            cam = Camera.main;
            camTransform = Camera.main.transform;
            viewportHalfDelta = (cam.ViewportToWorldPoint(Vector3.one) - cam.ViewportToWorldPoint(Vector3.zero))/(2* visibleBehindBoundsRate);
            Coin.OnCollect += DebugText;
        }
        void Start()
        {
            BeginGame();
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
            MoveCamera();
        }

        private void MoveCamera()
        {
            Bounds mazeBounds = mazeInstance.GetBounds();

            camTransform.position = new Vector3(
               (player.transform.position.x - mazeBounds.min.x) > viewportHalfDelta.x && (mazeBounds.max.x - player.transform.position.x) > viewportHalfDelta.x ?
                        player.transform.position.x : camTransform.position.x,
               (player.transform.position.y - mazeBounds.min.y) > viewportHalfDelta.y && (mazeBounds.max.y - player.transform.position.y) > viewportHalfDelta.y ?
                        player.transform.position.y : camTransform.position.y,
                        camTransform.position.z
               );
        }

        void BeginGame()
        {
            mazeInstance = Instantiate(mazePrefab) as Maze.Maze;
            mazeInstance.Generate();
            player = Instantiate(playerPrefab) as PlayerController;
            player.transform.position = mazeInstance.GetCell(mazeInstance.Size / 2 + new Maze.CellCoordinates(1,1)).transform.position;
            camTransform.position = player.transform.position.XY()+ camTransform.position.Z();
            

        }

        void RestartGame()
        {
            StopAllCoroutines();
            Destroy(mazeInstance.gameObject);
            Destroy(player.gameObject);

            BeginGame();
        }

        private void DebugText()
        {
            Debug.Log("coin collected");
        }

        private void OnDestroy()
        {
            Coin.OnCollect -= DebugText;
        }
    }
}