using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Maze.Cell mazeCellPrefab;
    private Maze.Cell[,] cells;

    public Maze.Maze mazePrefab;
    private Maze.Maze mazeInstance;
    // Start is called before the first frame update
    void Start()
    {
        BeginGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze.Maze;
        StartCoroutine(mazeInstance.Generate());
      
    }

    void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
