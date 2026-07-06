using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        public int defaultHealth;
        public List<RareRateDefaultData> ratesList;

        private void OnValidate()
        {
            foreach (var value in ratesList)
            {
                if (value.startWeight > value.maxWeight)
                    value.maxWeight = value.startWeight;
            }
        }
    }
}