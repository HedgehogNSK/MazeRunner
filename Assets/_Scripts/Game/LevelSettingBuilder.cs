using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    namespace Game
    {
        public class LevelSettingBuilder
        {
            private LevelSettings level;
            public LevelSettingBuilder()
            {
                level = new LevelSettings();
            }
            public LevelSettingBuilder SetCoinsAmount(int amount)
            {
                level.СoinsAmount = amount;
                return this;
            }
            public LevelSettingBuilder SetDistanceBetweenCoins(int distance)
            {
                level.DistanceBetweenCoins = distance;
                return this;
            }
            public LevelSettingBuilder SetEnemiesAmount(int amount)
            {
                level.EnemiesAmount = amount;
                return this;
            }
            public LevelSettingBuilder SetEnemyVigilance(int alertRange, int chaseRange)
            {
                level.AlertRange = alertRange;
                level.ChaseRange = chaseRange;

                return this;
            }


            public static implicit operator LevelSettings(LevelSettingBuilder builder)
            {
                return builder.level;
            }
        }

    }
}
