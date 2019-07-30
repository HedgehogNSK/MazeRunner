using System.Collections;
using System.Collections.Generic;
using Maze.Explorer;
using UnityEngine;
using Hedge.Tools;

namespace Maze
{
    using Content;
    public class Maze : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] Passage mazePassagePrefab;
        [SerializeField] Wall mazeWallPrefab;
        [SerializeField] Cell mazeCellPrefab;
        [SerializeField] Coordinates mazeSize;
        [SerializeField] bool idealLabyrinth = true;
#pragma warning restore CS0649
        
        //For not ideal labyrinth
        const float leakyRate = 0.8f;

        public Coordinates Size => mazeSize;
        public Graph Map { get; private set; }

        private Cell[,] cells;

        

        public void Generate()
        {

            cells = new Cell[mazeSize.X, mazeSize.Y];
            
            List<Cell> activeCells = new List<Cell>();
            Coordinates randomStartCoords = Coordinates.RandomCoordinates(Size);
            activeCells.Add(CreateCell(randomStartCoords));
            Map = new Graph(randomStartCoords);

            int i = 0;
            while (activeCells.Count > 0)
            {

                CellGenerator(activeCells);
                ++i;
            }

            gameObject.AddComponent<CompositeCollider2D>();
            
        }

        private Cell CreateCell(Coordinates coords)
        {
            Cell newCell = Instantiate(mazeCellPrefab) as Cell;
            cells[coords.X, coords.Y] = newCell;
            newCell.coords = coords;
            newCell.name = "MazeCell" + coords.X + "," + coords.Y;
            newCell.transform.parent = transform;
            //For Camera in 0,0,0 make Labirinth in the center
            newCell.transform.localPosition = coords.ToWorld;
            return newCell;
        }

        public bool ContainsCoordinates(Coordinates coords)
        {
            return coords.X >= 0 && coords.X < mazeSize.X && coords.Y >= 0 && coords.Y < mazeSize.Y;
        }

        public Cell GetCell(Coordinates coords)
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
           
            Coordinates neighbourCoords = currentCell.coords + direction.ToIntVector2();
            
            if (ContainsCoordinates(neighbourCoords))
            {
                
                Cell neighbour = GetCell(neighbourCoords);
                if (!neighbour)
                {
                    
                    neighbour = CreateCell(neighbourCoords);
                    CreatePassage(currentCell, neighbour, direction);
                    activeCells.Add(neighbour);
                    Map.AddEdge(currentCell.coords, neighbourCoords);
                    
                }
                else
                {
                    if (idealLabyrinth)
                        CreateWall(currentCell, neighbour, direction);
                    else
                    {
                        if (Random.Range(0f, 1f) < leakyRate)
                            CreateWall(currentCell, neighbour, direction);
                        else
                        {
                            CreatePassage(currentCell, neighbour, direction);
                            Map.AddEdge(currentCell.coords, neighbourCoords);
                        }
                    }
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
            Bounds b = new Bounds(Size.GetCenter.ToWorld- Vector3.one.XY(),new Vector2(Size.X, Size.Y));
            return b;
        }


    }

}