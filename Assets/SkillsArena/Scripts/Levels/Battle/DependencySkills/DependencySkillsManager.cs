using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillsArena
{
    public class DependencySkillsManager : MonoBehaviour
    {
        [SerializeField] private RatioAndElementPairsConfig ratioAndElementPairsConfig;
        [SerializeField] private ElementsPair_UI elementsPair_UI_prefab;
        [SerializeField] private List<DependencyRoundParent_UI> _dayParentsList;
        [SerializeField] private SkillElementConfig _skillElementConfig;
        [SerializeField] private DependencyPanel_UI _dependencyPanel_UI;

        private DependencySkillsData _dependencySkillsData;
        private List<ElementsPair_UI> _elementsPairsList = new List<ElementsPair_UI>();

        public void Init(DependencySkillsData dependencySkillsData)
        {
            _dependencyPanel_UI.gameObject.SetActive(true);
            if (_elementsPairsList.Count > 0)
            {
                foreach (ElementsPair_UI elementsPair in _elementsPairsList)
                {
                    Destroy(elementsPair.gameObject);
                }
                _elementsPairsList.Clear();
            }

            _dependencySkillsData = dependencySkillsData;

            for (int i = 0; i < _dayParentsList.Count; i++)
            {
                DependencyRoundParent_UI tempRoundParent = _dayParentsList[i];
                tempRoundParent.SetRoundText(i + 1);
                DependencySkillsOnRound dependencySkillsOnRound = _dependencySkillsData.GetDependencySkillsOnRound(i + 1);
                Dictionary<float, List<ElementPair>> ratioAndElementPairsList = dependencySkillsOnRound.ratioAndElementPairsDictionary;
                foreach (var keyValuePair in ratioAndElementPairsList)
                {
                    foreach (var elementPair in keyValuePair.Value)
                    {
                        ElementsPair_UI elementsPair = Instantiate(elementsPair_UI_prefab, tempRoundParent.transform);
                        elementsPair.Init(_skillElementConfig.GetStarSpriteByElementType(elementPair.firstElementType),
                        _skillElementConfig.GetStarSpriteByElementType(elementPair.secondElementType)
                        , keyValuePair.Key);
                        _elementsPairsList.Add(elementsPair);
                    }
                }
            }
            _dependencyPanel_UI.gameObject.SetActive(false);
        }

        public float GetRatioForElementPair(ElementPair elementPair, int round)
        {
            if (round < 1 || round > 3)
            {
                throw new System.Exception("Round must be from 1 to 3. Check and fix that");
            }
            DependencySkillsOnRound dependencySkillsOnRound = _dependencySkillsData.dependeciesDictionary[round];
            float ratio = dependencySkillsOnRound.GetRatioForElementPair(elementPair);
            return ratio;
        }

        public DependencySkillsData GetDependencySkillsDataRandom()
        {
            DependencySkillsData dependencySkillsData = new DependencySkillsData();
            for (int round = 1; round <= 3; round++)
            {
                DependencySkillsOnRound dependencySkillsOnRound = new DependencySkillsOnRound();

                int currentRatioIndex = 0;
                int currentPairIndex = 0;
                List<ElementPair> elementPairsAll = ratioAndElementPairsConfig.elementPairsList.ToList();
                List<RatioData> ratioList = ratioAndElementPairsConfig.GetRatioList();
                CheckForEqualRatioAndPairCounts(elementPairsAll.Count, ratioList);
                while (currentPairIndex < ratioAndElementPairsConfig.elementPairsList.Count)
                {
                    RatioData ratioData = ratioList[currentRatioIndex];
                    for (int i = 0; i < ratioData.minCount; i++)
                    {
                        int rndIndex = Random.Range(0, elementPairsAll.Count);
                        ElementPair elementPair = elementPairsAll.ElementAt(rndIndex);
                        elementPairsAll.RemoveAt(rndIndex);
                        dependencySkillsOnRound.AddElementPairAndRatio(ratioData.ratio, elementPair);
                        currentPairIndex++;
                    }
                    currentRatioIndex++;
                }
                dependencySkillsData.AddDependencySkillsOnRound(round, dependencySkillsOnRound);
            }
            return dependencySkillsData;
        }

        private void CheckForEqualRatioAndPairCounts(int elementPairsCount, List<RatioData> ratioList)
        {
            int currentCountForRatio = ratioList.Sum(x => x.minCount);
            if (elementPairsCount < currentCountForRatio)
            {
                throw new System.Exception("Ratio count more than pair count. Made less ratio count");
            }
            else if (elementPairsCount > currentCountForRatio)
            {
                int countDif = elementPairsCount - currentCountForRatio;
                for (int i = 0; i < countDif; i++)
                {
                    RatioData ratioData = ratioList.ElementAt(Random.Range(0, ratioList.Count));
                    ratioData.minCount++;
                }
            }
        }
    }
}