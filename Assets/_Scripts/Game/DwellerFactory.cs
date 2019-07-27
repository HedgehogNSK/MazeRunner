using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Game
{
    public static class DwellerFactory
    {
 
       
        public static IEnumerable<Dweller> CreateSet(Dweller prefab, int amount, int distanceBeetween, Transform parent =null)
        {
            if(Dweller.map==null)
            {
                Debug.LogError("Before create maze dwellers you should add maze map");
                return null;
            }
            var waypoints = Dweller.map.GetWaypoints(distanceBeetween);

            List<Dweller> dwellers = new List<Dweller>();
            Dweller dweller;
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
   
        public static Dweller Create(Dweller prefab, Coordinates coords)
        {
            Dweller dweller = Object.Instantiate(prefab) as Dweller;
            dweller.Init(coords);
            return dweller;
        }

    }
}