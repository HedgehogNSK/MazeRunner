using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Maze : MonoBehaviour
{
    public CS_MazePassage mazePassagePrefab;
    public CS_MazeWall mazeWallPrefab;
    public CellCoordinates mazeSize;
    public CS_MazeCell mazeCellPrefab;
    private CS_MazeCell mazeCellInstance;
    private CS_MazeCell[,] cells;

    public float fGenerationStopDelay;

    public IEnumerator Generate()
    {
        var t = Time.time;
        WaitForSeconds delay = new WaitForSeconds(fGenerationStopDelay);
        cells = new CS_MazeCell[mazeSize.x, mazeSize.z];
        List<CS_MazeCell> activeCells = new List<CS_MazeCell>();
        DoFirstGenerationStep(activeCells);
        
        while (activeCells.Count>0)
        {
            yield return delay;           
            DoNextGenerationStep(activeCells);
        }
        Debug.Log("Загрузка лабиринта заняла: " + (Time.time -t) + " секунд");
    }

    private CS_MazeCell CreateCell(CellCoordinates coords)
    {
        CS_MazeCell newCell = Instantiate(mazeCellPrefab) as CS_MazeCell;
        cells[coords.x, coords.z] = newCell;
        newCell.coords = coords;
        newCell.name = "MazeCell" + coords.x + "," + coords.z;
        newCell.transform.parent = transform;
        //For Camera in 0,0,0 make Labirinth in the center
        newCell.transform.localPosition = new Vector3(coords.x - mazeSize.x * 0.5f + 0.5f, 0f, coords.z - mazeSize.z * 0.5f + 0.5f);
        return newCell;
    }

    public CellCoordinates RandomCoordinates
    {

        get
        {
            return new CellCoordinates(Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.z));
        }
    }

    public bool ContainsCoordinates(CellCoordinates coords)
    {
        return coords.x >= 0 && coords.x < mazeSize.x && coords.z >= 0 && coords.z < mazeSize.z;
    }

    public CS_MazeCell GetCell(CellCoordinates coords)
    {
        return cells[coords.x, coords.z];
    }

    private void DoFirstGenerationStep(List<CS_MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    private void DoNextGenerationStep(List<CS_MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        CS_MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialised)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction;

        direction = currentCell.RandomUninitialisedDirection;


        CellCoordinates coords = currentCell.coords + direction.ToIntVector2();
        if(ContainsCoordinates(coords))
        {
            CS_MazeCell neighbour = GetCell(coords);
            if(!neighbour)
            {
                neighbour = CreateCell(coords);
                CreatePassage(currentCell, neighbour, direction);
                activeCells.Add(neighbour);
            }
            else
            {
                CreateWall(currentCell, neighbour, direction);
            }

        }
        else
        {
            CreateWall(currentCell, null, direction);            
        }

       
    }

    private void CreatePassage(CS_MazeCell cell, CS_MazeCell neighbourCell, MazeDirection dir)
    {
        CS_MazePassage passage = Instantiate(mazePassagePrefab) as CS_MazePassage;
        passage.Initialise(cell, neighbourCell, dir);
        passage = Instantiate(mazePassagePrefab) as CS_MazePassage;
        passage.Initialise(neighbourCell, cell, dir.GetOpposite());
    }

    private void CreateWall(CS_MazeCell cell, CS_MazeCell neighbourCell, MazeDirection dir)
    {
        CS_MazeWall wall = Instantiate(mazeWallPrefab) as CS_MazeWall;
        wall.Initialise(cell, neighbourCell, dir);
        if (neighbourCell != null)
        {
            wall = Instantiate(mazeWallPrefab) as CS_MazeWall;
            wall.Initialise(neighbourCell, cell, dir.GetOpposite());
        }
    }

  
}

