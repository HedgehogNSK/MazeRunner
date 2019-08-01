using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{
    public static class DwellerFactory
    {
        public static List<T> CreateSet<T>(T prefab, LevelSettings settings, Transform parent = null, Dweller target= null) where T:Dweller
        {
            if (Dweller.Map == null)
            {
                Debug.LogError("Before create maze dwellers you should add maze map");
                return null;
            }

            int distanceBetween;
            int amount;
            if (prefab is Enemy) {
                distanceBetween = (int)settings.AlertRange;
                amount = settings.EnemiesAmount;

            }
            else if (prefab is Coin) {
                distanceBetween = settings.DistanceBetweenCoins;
                amount = settings.СoinsAmount;
            }
            else
            {
                Debug.LogError("Behaviour for this type of Dweller aren't write yet");
                return null;
            }
                

            var spawnPoints = Dweller.Map.GenerateSpawnPoints(distanceBetween,(prefab is Enemy)?Explorer.Graph.Distance.From : Explorer.Graph.Distance.Between ,target.Coords); 

            List<T> dwellers = new List<T>();
            T dweller;
            for (int i = 0; i != amount; i++)
            {
                int id = Random.Range(0, spawnPoints.Count);
                dweller = Create(prefab, spawnPoints[id]);
                dweller.transform.parent = parent;
                dwellers.Add(dweller);
                if(dweller is Enemy)
                {
                    (dweller as Enemy).SetParams(settings.AlertRange, settings.ChaseRange, target);
                }
                spawnPoints.RemoveAt(id);
            }

            
            return dwellers;
        }
        public static T Create<T>(T prefab, Coordinates coords) where T: Dweller 
        {
            T dweller = Object.Instantiate(prefab) as T;
            dweller.Init(coords);
            return dweller;
        }

    }
}