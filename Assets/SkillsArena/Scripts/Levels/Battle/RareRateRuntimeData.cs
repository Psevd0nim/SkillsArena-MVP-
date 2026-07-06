using System;

namespace SkillsArena
{
    [Serializable]
    public class RareRateRuntimeData
    {
        public SkillRareType skillRareType;
        public int currentWeight;
        public int maxWeight;
        public int changeValuePerLevel;

        public RareRateRuntimeData(RareRateDefaultData rareRateDefaultData)
        {
            skillRareType = rareRateDefaultData.skillRareType;
            currentWeight = rareRateDefaultData.startWeight;
            maxWeight = rareRateDefaultData.maxWeight;
            changeValuePerLevel = rareRateDefaultData.changeValuePerLevel;
        }

        public RareRateRuntimeData()
        {

        }

        public void IncreaseRateLevel()
        {
            currentWeight += changeValuePerLevel;
            if (currentWeight > maxWeight)
                currentWeight = maxWeight;
        }
    }
}