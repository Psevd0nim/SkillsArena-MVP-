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
        [HideInInspector] public ColorType colorType;

        public EnemyData(int currentHealth, SkillCombinationData skillCombinationData, EnemySkillsRateData enemySkillsRateData, ColorType colorType)
        {
            this.currentHealth = currentHealth;
            this.skillCombinationData = skillCombinationData;
            this.enemySkillsRateData = enemySkillsRateData; 
            this.colorType = colorType;
        }

        public EnemyData()
        {
            
        }
    }
}