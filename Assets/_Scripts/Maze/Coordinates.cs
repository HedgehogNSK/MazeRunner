using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    [System.Serializable]
    public struct Coordinates
    {
         [SerializeField]int x;
         [SerializeField]int y;
        public int X => x;
        public int Y => y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public Coordinates GetCenter
        {
            get
            {
                int x = X / 2 + X % 2;
                int y = Y / 2 + Y % 2;
                return new Coordinates(x, y);
            }
        }

        public Vector3 ToWorld => new Vector3(X + 0.5f, Y + 0.5f, 0f);
        
        public Coordinates MoveTo(Direction direction)
        {
            return this + DirectionTools.ToIntVector2(direction);
        }

        public static Coordinates RandomCoordinates(Coordinates areaSize) => new Coordinates(Random.Range(0, areaSize.X), Random.Range(0, areaSize.Y));

        public static Coordinates operator +(Coordinates a, Coordinates b) => new Coordinates(a.X + b.X, a.Y + b.Y);
        public static Coordinates operator /(Coordinates a, int b) => new Coordinates(a.X / b, a.Y / b);

        public static bool operator !=(Coordinates a, Coordinates b) => (a.x != b.x || a.y != b.y);
        public static bool operator ==(Coordinates a, Coordinates b) => (a.x == b.x && a.y == b.y);

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinates))
            {
                return false;
            }

            var coordinates = (Coordinates)obj;
            return x == coordinates.x &&
                   y == coordinates.y;
        }

    }
}
