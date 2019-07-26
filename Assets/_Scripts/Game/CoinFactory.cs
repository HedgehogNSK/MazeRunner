using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{
    public static class CoinFactory
    {
        public static Coin Create(Coin coinPrefab, Coordinates coords)
        {
            Coin coin = Object.Instantiate(coinPrefab) as Coin;
            coin.Coords = coords;
            return coin;
        }
        public static Coin CreateSet(Coin coinPrefab, Explorer.Graph graph)
        {
            return new Coin();
        }

    }
}