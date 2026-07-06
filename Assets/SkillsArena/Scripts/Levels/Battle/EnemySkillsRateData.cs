using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace SkillsArena
{
    [Serializable]
    public class EnemySkillsRateData
    {
        public List<RareRateRuntimeData> ratesList = new();

        public EnemySkillsRateData(List<RareRateDefaultData> defaultRatesList)
        {
            foreach (var defaultRateData in defaultRatesList)
            {
                ratesList.Add(new RareRateRuntimeData(defaultRateData));
            }
        }

        public EnemySkillsRateData()
        {

        }

        public SkillRareType GetRandomSkillRare()
        {
            SkillRareType skillRareType = SkillRareType.Common;
            int totalWeight = 0;
            foreach (var rateData in ratesList)
                totalWeight += rateData.currentWeight;
            int rndValue = Random.Range(0, totalWeight);
            foreach (var rateData in ratesList)
            {
                if (rateData.currentWeight > rndValue)
                {
                    skillRareType = rateData.skillRareType;
                    break;
                }
                else
                    rndValue -= rateData.currentWeight;
            }
            return skillRareType;
        }

        public void IncreaseRateLevel()
        {
            foreach (var rateData in ratesList)
                rateData.IncreaseRateLevel();
        }
    }
}