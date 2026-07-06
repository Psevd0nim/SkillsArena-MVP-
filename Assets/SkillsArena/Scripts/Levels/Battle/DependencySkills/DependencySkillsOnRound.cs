using System;
using System.Collections.Generic;

namespace SkillsArena
{
    [Serializable]
    public class DependencySkillsOnRound
    {
        public Dictionary<float, List<ElementPair>> ratioAndElementPairsDictionary { get; private set; } = new();

        public void AddElementPairAndRatio(float ratio, ElementPair elementPair)
        {
            if (!ratioAndElementPairsDictionary.ContainsKey(ratio))
            {
                ratioAndElementPairsDictionary[ratio] = new List<ElementPair>();
            }
            ratioAndElementPairsDictionary[ratio].Add(elementPair);
        }

        public float GetRatioForElementPair(ElementPair elementPair)
        {
            foreach (var keyValuePair in ratioAndElementPairsDictionary)
            {
                List<ElementPair> elementPairsList = keyValuePair.Value;
                foreach (var elementPairTemp in elementPairsList)
                {
                    if(elementPair.Equals(elementPairTemp))
                    {
                        return keyValuePair.Key;
                    }
                }
            }
            throw new Exception("Ratio not found for element pair. Check and fix that");
        }
    }
}