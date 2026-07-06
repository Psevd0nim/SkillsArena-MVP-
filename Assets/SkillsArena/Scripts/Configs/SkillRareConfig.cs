using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillsArena
{
    [CreateAssetMenu]
    public class SkillRareConfig : ScriptableObject
    {
        public List<SkillRareData> skillRareDataList;

        public SkillRareData GetSkillRareDataByType(SkillRareType skillRareType)
        {
            SkillRareData skillRareData = skillRareDataList.First(x => x.skillRareType == skillRareType);
            if (skillRareData != null)
                return skillRareData;
            throw new Exception($"Can't return SkillRareData by type {skillRareType}");
        }
    }
}
