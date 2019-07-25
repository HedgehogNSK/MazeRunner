using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public static class CoinFactory
    {
        public static Coin Create(Coin coinPrefab, Maze.Coordinates coords)
        {
            Coin coin = Object.Instantiate(coinPrefab) as Coin;
            coin.Coords = coords;
            return coin;
        }
        public static Coin CreateSet(Coin coinPrefab, Maze.Explorer.Graph graph)
        {
            return new Coin();
        }

    }
}