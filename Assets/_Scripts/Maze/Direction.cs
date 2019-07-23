using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Maze
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class DirectionTools
    {
        public const int Count = 4;

        static IEnumerable<Direction> allDirections
        {
            get { return (IEnumerable<Direction>)System.Enum.GetValues(typeof(Direction)); }
        }

        public static Direction RandomValue
        {
            get
            {
                return (Direction)Random.Range(0, Count);
            }
        }
        private static CellCoordinates[] iVectors =
        {
        new CellCoordinates(0,1), //NORTH
        new CellCoordinates(1,0), //EASt
        new CellCoordinates(0,-1), //South
        new CellCoordinates(-1,0) //West
    };

        public static CellCoordinates ToIntVector2(this Direction direction)
        {
            return iVectors[(int)direction];
        }

        private static Direction[] opposites =
        {
        Direction.South,
        Direction.West,
        Direction.North,
        Direction.East
    };

        public static Direction GetOpposite(this Direction direction)
        {
            return opposites[(int)direction];
        }

        private static Quaternion[] rotations =
        {
        Quaternion.identity,
        Quaternion.Euler(0f,90f,0f),
        Quaternion.Euler(0f,180f,0f),
        Quaternion.Euler(0f,270f,0f)
    };

        public static Quaternion ToRotate(this Direction direction)
        {
            return rotations[(int)direction];
        }

        static IEnumerable<Direction> UninitializedDirections(IEnumerable<Direction> initializedDirections)
        {
            return allDirections.Except(initializedDirections);
        }

        public static Direction RandomUninitialisedDirection(IEnumerable<Direction> initializedDirections)
        {
            IEnumerable<Direction> availableDirections = UninitializedDirections(initializedDirections);
            int r = Random.Range(0, availableDirections.Count());
            return availableDirections.ElementAt(r);
        }

    }
}