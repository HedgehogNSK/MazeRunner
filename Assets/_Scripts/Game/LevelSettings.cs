using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    namespace Game
    {
        public class LevelSettings
        {            
            int coinsAmount;
            //Possible minimum distance between coins on the scene
            int distanceBetweenCoins;            
            int enemiesAmount;
            //the distance at which the enemy will discover the player
            float enemyAlertRange;
            //the distance at which the enemy will lose the player
            float enemyChaseRange;
        

            public int СoinsAmount
            {
                get { return coinsAmount; }
                set
                {
                    if (value > 0)
                        coinsAmount = value;
                    else
                        Debug.LogError("Amount of coins on level must be greater than 0");

                }
            }

            public int DistanceBetweenCoins
            {
                get { return distanceBetweenCoins; }
                set
                {
                    if (value > 0)
                        distanceBetweenCoins = value;
                    else
                        Debug.LogError("Distance between coins on level must be greater than 0");

                }
            }
            public int EnemiesAmount
            {
                get { return enemiesAmount; }
                set
                {
                    if (value >= 0)
                        enemiesAmount = value;
                    else
                        Debug.LogError("Amount of enemies in level must be greater of equal 0");

                }
            }
            public float AlertRange
            {
                get { return enemyAlertRange; }
                set
                {
                    if (value > 0)
                        enemyAlertRange = value;
                    else
                        Debug.LogError("Enemy range of detection of the player must be greater 0");

                }
            }
            public float ChaseRange
            {
                get { return enemyChaseRange; }
                set
                {
                    if (value >enemyAlertRange)
                        enemyChaseRange = value;
                    else
                        Debug.LogError("Enemy's chase range of the player must be greater 0 and greater than enemy alert range");

                }
            }



            static public LevelSettingBuilder CreateBuilder()
            {
                return new LevelSettingBuilder();
            }

        }
    }
}