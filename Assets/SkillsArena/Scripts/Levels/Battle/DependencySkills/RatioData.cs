using System;
using UnityEngine;

namespace SkillsArena
{
    [Serializable]
    public class RatioData
    {
        public float ratio;
        [Range(1, 3)]
        public int minCount = 1;

        public RatioData(RatioData ratioData)
        {
            ratio = ratioData.ratio;
            minCount = ratioData.minCount;
        }
    }
}