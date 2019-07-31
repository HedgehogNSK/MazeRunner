using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    namespace Game
    {
        static public class LevelFactory
        {
            static public LevelSettings Create(int level)
            {
                LevelSettingBuilder builder = new LevelSettingBuilder().SetDistanceBetweenCoins(5);

                int baseCoinAmount = 2;
                int baseEnemyAmount = 2;                
                
                if (level < 20 && level>0)
                {
                    builder = builder.SetEnemiesAmount(baseEnemyAmount + level)
                        .SetCoinsAmount(baseCoinAmount + level)
                        .SetEnemyVigilance(4, 6);
                    
                }
                else if(level>=20 && level<40)
                {
                    builder = builder.SetEnemiesAmount(baseEnemyAmount + level-19)
                        .SetCoinsAmount(baseCoinAmount + level-19)
                        .SetEnemyVigilance(5, 7);
                }
                else if(level>=40 && level <=50)
                {
                    builder = builder.SetEnemiesAmount(baseEnemyAmount + level - 39)
                       .SetCoinsAmount(baseCoinAmount + level - 39)
                       .SetEnemyVigilance(6, 9);
                }
                else
                {
                    Debug.LogError("There is no settings for this level: "+ level);
                }               
                return builder;
            }

        }
    }
}