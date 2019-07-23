using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CellCoordinates
{
    public int x, z;

    public CellCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static CellCoordinates operator+(CellCoordinates a, CellCoordinates b)
    {
        CellCoordinates tmp;
        tmp.x = a.x + b.x;
        tmp.z = a.z + b.z;
        return tmp;
    }
}