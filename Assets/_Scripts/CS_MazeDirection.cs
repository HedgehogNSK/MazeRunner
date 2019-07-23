using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public enum MazeDirection
{
    North,    
    East,
    South,
    West
}

public static class MazeDirections
{
    public const int Count = 4;

    static IEnumerable<MazeDirection> allDirections
    {
        get { return (IEnumerable<MazeDirection>)System.Enum.GetValues(typeof(MazeDirection)); }
    }

    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }
    private static CellCoordinates[] iVectors =
    {
        new CellCoordinates(0,1), //NORTH
        new CellCoordinates(1,0), //EASt
        new CellCoordinates(0,-1), //South
        new CellCoordinates(-1,0) //West
    };

    public static CellCoordinates ToIntVector2(this MazeDirection direction)
    {
        return iVectors[(int)direction];
    }

    private static MazeDirection[] opposites =
    {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    public static MazeDirection GetOpposite (this MazeDirection direction)
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

    public static Quaternion ToRotate (this MazeDirection direction)
    {
        return rotations[(int)direction];
    }

    static IEnumerable<MazeDirection> UninitializedDirections (IEnumerable<MazeDirection> initializedDirections)
    {
        return allDirections.Except(initializedDirections);
    }

    public static MazeDirection RandomUninitialisedDirection(IEnumerable<MazeDirection> initializedDirections)
    {
       IEnumerable<MazeDirection> availableDirections =  UninitializedDirections(initializedDirections);
       int r = Random.Range(0, availableDirections.Count());
       return availableDirections.ElementAt(r);
    }

}