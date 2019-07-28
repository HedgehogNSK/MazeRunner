using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{
    public static class DwellerFactory
    {
 
       
        public static List<T> CreateSet<T>(T prefab, int amount, int minDistanceBetween, Transform parent =null) where T: Dweller
        {
            if(Dweller.Map==null)
            {
                Debug.LogError("Before create maze dwellers you should add maze map");
                return null;
            }

            if(prefab is PlayerController)
            {
                Debug.LogError("It's impossible to create set of players, use Create method instead.");
                return null;
            }
            
            var waypoints = Dweller.Map.GetWaypoints(minDistanceBetween);

            List<T> dwellers = new List<T>();
            T dweller;
            for (int i = 0; i != amount; i++)
            {
                int id = Random.Range(0, waypoints.Count);
                dweller = Create(prefab, waypoints[id]);
                dweller.transform.parent = parent;
                dwellers.Add(dweller);
                waypoints.RemoveAt(id);
            }
            return dwellers;
        }

        public static List<T> CreateSet<T>(T prefab, LevelSettings settings, Transform parent = null, Coordinates startPoint= null) where T:Dweller
        {
            if (Dweller.Map == null)
            {
                Debug.LogError("Before create maze dwellers you should add maze map");
                return null;
            }

            int distanceBetween;
            int amount;
            if (prefab is Enemy) {
                distanceBetween = (int)settings.ChaseRange;
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
                

            var waypoints = Dweller.Map.GetWaypoints(distanceBetween, startPoint); 

            List<T> dwellers = new List<T>();
            T dweller;
            for (int i = 0; i != amount; i++)
            {
                int id = Random.Range(0, waypoints.Count);
                dweller = Create(prefab, waypoints[id]);
                dweller.transform.parent = parent;
                dwellers.Add(dweller);
                if(dweller is Enemy)
                {
                    (dweller as Enemy).SetParams(settings.AlertRange, settings.ChaseRange);
                }
                waypoints.RemoveAt(id);
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