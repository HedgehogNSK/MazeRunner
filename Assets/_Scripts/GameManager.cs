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
        [SerializeField] PlayerController playerPrefab;
        PlayerController player;

        [SerializeField] Maze.Maze mazePrefab;
        private Maze.Maze mazeInstance;
        // Start is called before the first frame update
        private void Awake()
        {
            cam = Camera.main;
            camTransform = Camera.main.transform;
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
           // MoveCamera();
        }

        private void MoveCamera()
        {
            Bounds mazeBounds = mazeInstance.GetBounds();

            Vector3 upperRightPoint = cam.ViewportToWorldPoint(Vector3.zero);
            Vector3 downLeftPoint = cam.ViewportToWorldPoint(Vector3.one);


            Vector3 newCamPosition = new Vector3((downLeftPoint.x < mazeBounds.min.x) ? mazeBounds.min.x : (upperRightPoint.x > mazeBounds.max.x) ? mazeBounds.max.x : player.transform.position.x,
               (downLeftPoint.y < mazeBounds.min.y) ? mazeBounds.min.y : (upperRightPoint.y > mazeBounds.max.y) ? mazeBounds.max.y : player.transform.position.y,
                camTransform.position.z
               );

            camTransform.position = newCamPosition;
        }

        void BeginGame()
        {
            mazeInstance = Instantiate(mazePrefab) as Maze.Maze;
            mazeInstance.GenerateLabirynth();
            //player = Instantiate(playerPrefab) as PlayerController;

        }

        void RestartGame()
        {
            StopAllCoroutines();
            Destroy(mazeInstance.gameObject);
            Destroy(player.gameObject);

            BeginGame();
        }
    }
}