using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Maze : MonoBehaviour
    {
        [Range(1, 700)]
        [SerializeField] int visualizationSpeed =1;
        [SerializeField] Passage mazePassagePrefab;
        [SerializeField] Wall mazeWallPrefab;
        [SerializeField] Cell mazeCellPrefab;
        [SerializeField] CellCoordinates mazeSize;
        public CellCoordinates Size { get { return mazeSize; } }

        private Cell[,] cells;

        public void Generate()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            cells = new Cell[mazeSize.x, mazeSize.y];
            List<Cell> activeCells = new List<Cell>();
            DoFirstGenerationStep(activeCells);
            int i = 0;
            while (activeCells.Count > 0)
            {

                DoNextGenerationStep(activeCells);
                ++i;
            }
            watch.Stop();
            Debug.Log("Загрузка лабиринта заняла: " + watch.ElapsedMilliseconds / 1000f + " секунд и "+i+" циклов" );

        }

        private Cell CreateCell(CellCoordinates coords)
        {
            Cell newCell = Instantiate(mazeCellPrefab) as Cell;
            cells[coords.x, coords.y] = newCell;
            newCell.coords = coords;
            newCell.name = "MazeCell" + coords.x + "," + coords.y;
            newCell.transform.parent = transform;
            //For Camera in 0,0,0 make Labirinth in the center
            newCell.transform.localPosition = new Vector3(coords.x - mazeSize.x * 0.5f + 0.5f, coords.y - mazeSize.y * 0.5f + 0.5f, 0f);
            return newCell;
        }

        public CellCoordinates RandomCoordinates
        {

            get
            {
                return new CellCoordinates(Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y));
            }
        }

        public bool ContainsCoordinates(CellCoordinates coords)
        {
            return coords.x >= 0 && coords.x < mazeSize.x && coords.y >= 0 && coords.y < mazeSize.y;
        }

        public Cell GetCell(CellCoordinates coords)
        {
            return cells[coords.x, coords.y];
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

            var watch = System.Diagnostics.Stopwatch.StartNew();
            direction = currentCell.RandomUninitialisedDirection;
            watch.Stop();
            Debug.Log(watch.ElapsedMilliseconds);

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

        public Bounds GetBounds()
        {            
            Bounds b = new Bounds(transform.position,new Vector2(Size.x, Size.y));
            return b;
        }


    }

}