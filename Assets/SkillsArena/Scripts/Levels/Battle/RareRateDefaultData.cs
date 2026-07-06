using System;
using UnityEngine;

namespace SkillsArena
{
    [Serializable]
    public class RareRateDefaultData
    {
        public SkillRareType skillRareType;
        [Range(0, 100)] public int startWeight;
        [Range(0, 100)] public int maxWeight;
        [Range(0, 10)] public int changeValuePerLevel;
    }
}