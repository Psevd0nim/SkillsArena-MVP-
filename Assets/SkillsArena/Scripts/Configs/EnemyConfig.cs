using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class EnemyConfig : ScriptableObject
    {
        public int defaultHealth;
        public List<RareRateDefaultData> ratesList;
        public List<ColorAndType> colorAndTypeList;

        public Color GetColorByType(ColorType colorType)
        {
            return colorAndTypeList.Find(x => x.colorType == colorType).color;
        }

        private void OnValidate()
        {
            foreach (var value in ratesList)
            {
                if (value.startWeight > value.maxWeight)
                    value.maxWeight = value.startWeight;
            }
        }
    }

    [Serializable]
    public class ColorAndType
    {
        public ColorType colorType;
        public Color color;
    }

    public enum ColorType
    {
        Red, Green, Blue, Yellow, Purple
    }
}