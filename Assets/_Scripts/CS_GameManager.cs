using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour
{
    public CS_MazeCell mazeCellPrefab;
    private CS_MazeCell mazeCellInstance;
    private CS_MazeCell[,] cells;

    public CS_Maze mazePrefab;
    private CS_Maze mazeInstance;
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
        mazeInstance = Instantiate(mazePrefab) as CS_Maze;
        StartCoroutine(mazeInstance.Generate());
      
    }

    void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
