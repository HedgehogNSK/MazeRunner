using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Maze : MonoBehaviour
    {
        public Passage mazePassagePrefab;
        public Wall mazeWallPrefab;
        public CellCoordinates mazeSize;
        public Cell mazeCellPrefab;
        private Cell mazeCellInstance;
        private Cell[,] cells;

        public float fGenerationStopDelay;

        public IEnumerator Generate()
        {
            var t = Time.time;
            WaitForSeconds delay = new WaitForSeconds(fGenerationStopDelay);
            cells = new Cell[mazeSize.x, mazeSize.z];
            List<Cell> activeCells = new List<Cell>();
            DoFirstGenerationStep(activeCells);

            while (activeCells.Count > 0)
            {
                yield return delay;
                DoNextGenerationStep(activeCells);
            }
            Debug.Log("Загрузка лабиринта заняла: " + (Time.time - t) + " секунд");
        }

        private Cell CreateCell(CellCoordinates coords)
        {
            Cell newCell = Instantiate(mazeCellPrefab) as Cell;
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

        public Cell GetCell(CellCoordinates coords)
        {
            return cells[coords.x, coords.z];
        }

        private void DoFirstGenerationStep(List<Cell> activeCells)
        {
            activeCells.Add(CreateCell(RandomCoordinates));
        }

        private void DoNextGenerationStep(List<Cell> activeCells)
        {
            int currentIndex = activeCells.Count - 1;
            Cell currentCell = activeCells[currentIndex];
            if (currentCell.IsFullyInitialised)
            {
                activeCells.RemoveAt(currentIndex);
                return;
            }
            Direction direction;

            direction = currentCell.RandomUninitialisedDirection;


            CellCoordinates coords = currentCell.coords + direction.ToIntVector2();
            if (ContainsCoordinates(coords))
            {
                Cell neighbour = GetCell(coords);
                if (!neighbour)
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

        private void CreatePassage(Cell cell, Cell neighbourCell, Direction dir)
        {
            Passage passage = Instantiate(mazePassagePrefab) as Passage;
            passage.Initialise(cell, neighbourCell, dir);
            passage = Instantiate(mazePassagePrefab) as Passage;
            passage.Initialise(neighbourCell, cell, dir.GetOpposite());
        }

        private void CreateWall(Cell cell, Cell neighbourCell, Direction dir)
        {
            Wall wall = Instantiate(mazeWallPrefab) as Wall;
            wall.Initialise(cell, neighbourCell, dir);
            if (neighbourCell != null)
            {
                wall = Instantiate(mazeWallPrefab) as Wall;
                wall.Initialise(neighbourCell, cell, dir.GetOpposite());
            }
        }


    }

}