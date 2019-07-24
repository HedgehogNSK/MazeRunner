﻿using System.Collections;
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
            cells = new Cell[mazeSize.X, mazeSize.Y];
            List<Cell> activeCells = new List<Cell>();
            activeCells.Add(CreateCell(RandomCoordinates));
            int i = 0;
            while (activeCells.Count > 0)
            {

                CellGenerator(activeCells);
                ++i;
            }
            watch.Stop();
            Debug.Log("Загрузка лабиринта заняла: " + watch.ElapsedMilliseconds / 1000f + " секунд и "+i+" циклов" );
            gameObject.AddComponent<CompositeCollider2D>();

        }

        private Cell CreateCell(CellCoordinates coords)
        {
            Cell newCell = Instantiate(mazeCellPrefab) as Cell;
            cells[coords.X, coords.Y] = newCell;
            newCell.coords = coords;
            newCell.name = "MazeCell" + coords.X + "," + coords.Y;
            newCell.transform.parent = transform;
            //For Camera in 0,0,0 make Labirinth in the center
            newCell.transform.localPosition = GetWorldPosition(coords);
            return newCell;
        }

        public CellCoordinates RandomCoordinates
        {

            get
            {
                return new CellCoordinates(Random.Range(0, mazeSize.X), Random.Range(0, mazeSize.Y));
            }
        }

        public bool ContainsCoordinates(CellCoordinates coords)
        {
            return coords.X >= 0 && coords.X < mazeSize.X && coords.Y >= 0 && coords.Y < mazeSize.Y;
        }

        public Cell GetCell(CellCoordinates coords)
        {
            return cells[coords.X, coords.Y];
        }
        public Cell GetCell(int x, int y)
        {
            return cells[x, y];
        }

        private void CellGenerator(List<Cell> activeCells)
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

        public Bounds GetBounds()
        {            
            Bounds b = new Bounds(transform.position,new Vector2(Size.X, Size.Y));
            return b;
        }

        public Vector3 GetWorldPosition(CellCoordinates coords)
        {

            return new Vector3(coords.X - mazeSize.X * 0.5f + 0.5f, coords.Y - mazeSize.Y * 0.5f + 0.5f, 0f);
        }

    }

}