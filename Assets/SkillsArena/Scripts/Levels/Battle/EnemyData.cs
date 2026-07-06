using System;
using UnityEngine;

namespace SkillsArena
{
    [Serializable]
    public class EnemyData
    {
        public int currentHealth;
        [HideInInspector] public SkillCombinationData skillCombinationData = new();
        [HideInInspector] public EnemySkillsRateData enemySkillsRateData;

        public EnemyData(int currentHealth, SkillCombinationData skillCombinationData, EnemySkillsRateData enemySkillsRateData)
        {
            this.currentHealth = currentHealth;
            this.skillCombinationData = skillCombinationData;
            this.enemySkillsRateData = enemySkillsRateData; 
        }

        public EnemyData()
        {
            
        }
    }
}