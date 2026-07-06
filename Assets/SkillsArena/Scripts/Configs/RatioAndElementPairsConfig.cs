using System.Collections.Generic;
using UnityEngine;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class RatioAndElementPairsConfig : ScriptableObject
    {
        public List<ElementPair> elementPairsList;
        public List<RatioData> ratioList;

        public List<RatioData> GetRatioList()
        {
            List<RatioData> shallowRatioList = new();
            foreach(var ratioData in ratioList)
            {
                shallowRatioList.Add(new RatioData(ratioData));
            }
            return shallowRatioList;
        }
    }
}