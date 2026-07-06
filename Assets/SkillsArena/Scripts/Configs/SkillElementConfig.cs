using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class SkillElementConfig : ScriptableObject
    {
        public List<SkillElementData> skillElementsList;
        public int multiplyFactor;
        public int firstDamage;

        [Header("Config Additional Settings")]
        public bool wantSetDefaultValue;

        public SkillElementData GetRandomElement()
        {
            return skillElementsList.ElementAt(Random.Range(0, skillElementsList.Count));
        }

        public Color GetColorByElementType(SkillElementType skillElementType)
        {
            foreach (var skillElementData in skillElementsList)
            {
                if (skillElementData.elementType == skillElementType)
                    return skillElementData.color;
            }
            throw new Exception($"Can't return Color by Element Type: {skillElementType}");
        }

        public Sprite GetStarSpriteByElementType(SkillElementType skillElementType)
        {
            foreach (var skillElementData in skillElementsList)
            {
                if (skillElementData.elementType == skillElementType)
                    return skillElementData.starViewSprite;
            }
            throw new Exception($"Can't return Sprite by Element Type: {skillElementType}");
        }

        public SkillElementData GetSkillElementDataByType(SkillElementType skillElementType)
        {
            SkillElementData skillElementData = skillElementsList.First(x => x.elementType == skillElementType);
            if (skillElementData != null)
                return skillElementData;
            throw new Exception($"Can't return SkillElementData by type {skillElementType}");
        }
    }
}